Public Class robot
    Inherits System.Web.UI.Page
    Private _Factory As BL.Factory

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            log4net.LogManager.GetLogger("robotlog").Info("Start")
            _Factory = New BL.Factory(, BO.ASS.GetConfigVal("robot_account", "admin"))
            If _Factory.SysUser Is Nothing Then
                log4net.LogManager.GetLogger("robotlog").Info("Service user is not inhaled!")
                Response.Write("Service user is not inhaled!")
                Return
            End If


            Handle_MailQueque()

            Handle_p40Queue()

            Handle_ImapRobot()

            Handle_o22Reminder()
            Handle_p56Reminder()

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

        Dim lisP40 As IEnumerable(Of BO.p40WorkSheet_Recurrence) = _Factory.p40WorkSheet_RecurrenceBL.GetList_WaitingForGenerate(datNow)
        If lisP40.Count = 0 Then Return

        log4net.LogManager.GetLogger("robotlog").Info("p40-GetList_WaitingForGenerate: " & lisP40.ToString)

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
                    _Factory.p40WorkSheet_RecurrenceBL.Update_p31Instance(cP39.p39ID, _Factory.p31WorksheetBL.LastSavedPID, "")
                Else
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
End Class