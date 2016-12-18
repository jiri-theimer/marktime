Public Class robot
    Inherits System.Web.UI.Page
    Private _Factory As BL.Factory

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            log4net.LogManager.GetLogger("robotlog").Info("Start")
            _Factory = New BL.Factory(, BO.ASS.GetConfigVal("robot_account", "admin"))
            If _Factory.SysUser Is Nothing Then
                log4net.LogManager.GetLogger("robotlog").Info("Service user is not inhaled!")
                Response.Write("Service user not exists!")
                Return
            End If
            Handle_p40Queue()

            Handle_MailQueque()

            Handle_ImapRobot()

            Handle_o22Reminder()
            Handle_p56Reminder()

            Handle_ScheduledReports()

            If Now > Today.AddHours(15) And Now < Today.AddHours(17) And Now.DayOfWeek <> DayOfWeek.Sunday And Now.DayOfWeek <> DayOfWeek.Saturday Then
                Handle_CnbKurzy()
            End If

            If Now > Today.AddHours(3) And Now < Today.AddHours(4) Then
                'mezi třetí a čtvrtou hodinou ráno vyčistit temp tabulky
                _Factory.p85TempBoxBL.Recovery_ClearCompleteTemp()
            End If

            log4net.LogManager.GetLogger("robotlog").Info("End")

            Me.lblMessage.Text = Format(Now, "dd.MM.yyyy HH:mm:ss") & " - robot spuštěn."
        End If

    End Sub

    Private Sub Handle_MailQueque()
        Dim mq As New BO.myQueryX40
        mq.x40State = BO.x40StateENUM.InQueque
        mq.TopRecordsOnly = 10

        Dim lisX40 As IEnumerable(Of BO.x40MailQueue) = _Factory.x40MailQueueBL.GetList(mq)
        If lisX40.Count > 0 Then
            've frontě čekají smtp zprávy k odeslání - maximálně 10 zpráv najednou
            For Each cMessage In lisX40
                _Factory.x40MailQueueBL.SendMessageFromQueque(cMessage)

            Next
        End If
    End Sub

    Private Sub Handle_p40Queue()
        Dim datNow As Date = Now
        If Request.Item("now") <> "" Then
            datNow = BO.BAS.ConvertString2Date(Request.Item("now"))
        End If
        Dim lisP40 As IEnumerable(Of BO.p40WorkSheet_Recurrence) = _Factory.p40WorkSheet_RecurrenceBL.GetList_WaitingForGenerate(datNow)
        If lisP40.Count = 0 Then Return

        log4net.LogManager.GetLogger("robotlog").Info("p40-GetList_WaitingForGenerate, records: " & lisP40.Count.ToString)

        Dim lisP53 As IEnumerable(Of BO.p53VatRate) = _Factory.p53VatRateBL.GetList(New BO.myQuery)

        For Each cRec In lisP40
            Dim cP39 As BO.p39WorkSheet_Recurrence_Plan = _Factory.p40WorkSheet_RecurrenceBL.LoadP39_FirstWaiting(cRec.PID, datNow)
            If Not cP39 Is Nothing Then
                'vygenerovat úkon
                Dim cP34 As BO.p34ActivityGroup = _Factory.p34ActivityGroupBL.Load(cRec.p34ID)
                Dim cP31 As New BO.p31WorksheetEntryInput
                With cP31
                    .j02ID = cRec.j02ID
                    .p41ID = cRec.p41ID
                    .p34ID = cRec.p34ID
                    .p32ID = cRec.p32ID
                    .p31Text = cP39.p39Text
                    .p31Date = cP39.p39Date
                    .Value_Orig = CStr(cRec.p40Value)
                    .Value_Orig_Entried = CStr(cRec.p40Value)
                    If cP34.p33ID = BO.p33IdENUM.PenizeBezDPH Or cP34.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                        .j27ID_Billing_Orig = cRec.j27ID
                        If cRec.x15ID > BO.x15IdEnum.BezDPH Then
                            Dim lisVR As IEnumerable(Of BO.p53VatRate) = lisP53.Where(Function(p) p.j27ID = cRec.j27ID And p.x15ID = cRec.x15ID)
                            If lisVR.Count > 0 Then
                                .VatRate_Orig = lisVR(0).p53Value
                            End If
                        End If

                        .Amount_WithoutVat_Orig = cRec.p40Value
                        If cP34.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                            .Amount_Vat_Orig = .VatRate_Orig / 100 + cRec.p40Value
                            .Amount_WithVat_Orig = .Amount_Vat_Orig + .Amount_WithoutVat_Orig
                        End If

                    End If
                    
                End With
                Dim bol As Boolean = _Factory.p31WorksheetBL.SaveOrigRecord(cP31, Nothing)
                If bol Then
                    log4net.LogManager.GetLogger("robotlog").Info("p40-new robot worksheet record,p39ID=" & cP39.p39ID.ToString & ", p31ID=" & _Factory.p31WorksheetBL.LastSavedPID.ToString)
                    _Factory.p40WorkSheet_RecurrenceBL.Update_p31Instance(cP39.p39ID, _Factory.p31WorksheetBL.LastSavedPID, "")
                Else
                    log4net.LogManager.GetLogger("robotlog").Info("p40-new robot worksheet record,p39ID=" & cP39.p39ID.ToString & ", ERROR=" & _Factory.p31WorksheetBL.ErrorMessage)
                    _Factory.p40WorkSheet_RecurrenceBL.Update_p31Instance(cP39.p39ID, 0, _Factory.p31WorksheetBL.ErrorMessage)
                End If
            End If

        Next
    End Sub

    Public Sub Handle_o22Reminder()
        _Factory.o22MilestoneBL.Handle_Reminder()
    End Sub
    Public Sub Handle_o23Reminder()
        _Factory.o23NotepadBL.Handle_Reminder()
    End Sub
    Public Sub Handle_p56Reminder()
        _Factory.p56TaskBL.Handle_Reminder()
    End Sub

    Public Sub Handle_ImapRobot()
        Dim lis As IEnumerable(Of BO.o41InboxAccount) = _Factory.o41InboxAccountBL.GetList(New BO.myQuery)
        For Each c In lis
            _Factory.o42ImapRuleBL.HandleWaitingImapMessages(c)
        Next

    End Sub

    Public Sub Handle_CnbKurzy()
        Dim datImport As Date = DateSerial(Year(Now), Month(Now), Day(Now))

        Dim lisM62 As IEnumerable(Of BO.m62ExchangeRate) = _Factory.m62ExchangeRateBL.GetList().Where(Function(p) p.m62RateType = BO.m62RateTypeENUM.InvoiceRate And p.m62Date = datImport And p.UserInsert = "robot")
        If lisM62.Count = 0 Then
            _Factory.m62ExchangeRateBL.ImportRateList_CNB(datImport)
        End If
    End Sub

    Public Sub Handle_ScheduledReports()
        Dim lis As IEnumerable(Of BO.x31Report) = _Factory.x31ReportBL.GetList(New BO.myQuery).Where(Function(p) p.x31IsScheduling = True And p.x31SchedulingReceivers <> "")
        For Each c In lis
            If _Factory.x31ReportBL.IsWaiting4AutoGenerate(c) Then

                Dim strRepFullPath As String = _Factory.x35GlobalParam.UploadFolder
                If c.ReportFolder <> "" Then
                    strRepFullPath += "\" & c.ReportFolder
                End If
                strRepFullPath += "\" & c.ReportFileName
                Dim cRep As New clsReportOnBehind()
                Dim strOutputFileName As String = cRep.GenerateReport2Temp(_Factory, strRepFullPath)

                Dim message As New BO.smtpMessage()
                With message
                    .Body = "Automaticky generovaná zpráva ze systému MARKTIME." & vbCrLf & vbCrLf & "Report: " & c.x31Name & vbCrLf & vbCrLf & vbCrLf & "Pozdrav posílá MARKTIME robot!"
                    .SenderAddress = _Factory.x35GlobalParam.GetValueString("SMTP_SenderAddress")
                    .SenderName = "MARKTIME robot"
                    .Subject = BO.BAS.OM3(c.x31Name, 30) & " | MARKTIME REPORT"
                    .AddOneFile2FullPath(_Factory.x35GlobalParam.TempFolder & "\" & strOutputFileName)
                End With
                c.x31SchedulingReceivers = Replace(c.x31SchedulingReceivers, ",", ";")
                Dim a() As String = Split(c.x31SchedulingReceivers, ";")
                Dim recipients As New List(Of BO.x43MailQueue_Recipient)
                For i = 0 To UBound(a)
                    Dim cc As New BO.x43MailQueue_Recipient()
                    cc.x43Email = a(i)
                    cc.x43RecipientFlag = BO.x43RecipientIdEnum.recBCC
                    recipients.Add(cc)
                Next
                With _Factory.x40MailQueueBL
                    Dim intMessageID As Integer = .SaveMessageToQueque(message, recipients, BO.x29IdEnum.x31Report, c.PID, BO.x40StateENUM.InQueque)
                    If intMessageID > 0 Then
                        _Factory.x31ReportBL.UpdateLastScheduledRun(c.PID, Now)
                        If Not .SendMessageFromQueque(intMessageID) Then
                            Response.Write(.ErrorMessage)
                        End If
                    Else
                        Response.Write(.ErrorMessage)
                    End If
                End With
            End If
        Next

    End Sub
End Class