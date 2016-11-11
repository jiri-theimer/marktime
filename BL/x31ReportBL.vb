Public Interface Ix31ReportBL
    Inherits IFMother
    Function Save(cRec As BO.x31Report, strUploadGUID As String, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
    Function Load(intPID As Integer) As BO.x31Report
    Function LoadByFilename(strFileName As String) As BO.x31Report
    Function LoadByCode(strCode As String) As BO.x31Report
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x31Report)
    Function GetList_PersonalPageSource() As List(Of BO.x31Report)
    Function IsWaiting4AutoGenerate(cRec As BO.x31Report) As Boolean
    Function UpdateLastScheduledRun(intX31ID As Integer, dat As Date) As Boolean
End Interface
Class x31ReportBL
    Inherits BLMother
    Implements Ix31ReportBL
    Private WithEvents _cDL As DL.x31ReportDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.x31ReportDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.x31Report, strUploadGUID As String, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ix31ReportBL.Save
        Dim lisTempUpload As IEnumerable(Of BO.p85TempBox) = Nothing

        If strUploadGUID <> "" Then
            lisTempUpload = Me.Factory.p85TempBoxBL.GetList(strUploadGUID, True)
            If lisTempUpload.Where(Function(p) p.p85IsDeleted = False).Count > 0 Then
                If lisTempUpload.Where(Function(p) p.p85IsDeleted = False).Count > 1 Then
                    _Error = "V nastavení šablony sestavy může být nahrán pouze jeden soubor." : Return False
                End If
                Dim cTemp As BO.p85TempBox = lisTempUpload.Where(Function(p) p.p85IsDeleted = False)(0)
                Select Case cRec.x31FormatFlag
                    Case BO.x31FormatFlagENUM.ASPX
                        If Right(LCase(cTemp.p85FreeText01), 4) <> "aspx" Then
                            _Error = "Souborová přípona MARKTIME pluginu musí být [ASPX]." : Return False
                        End If
                    Case BO.x31FormatFlagENUM.DOCX
                        If Right(LCase(cTemp.p85FreeText01), 4) <> "docx" Then
                            _Error = "Souborová přípona MARKTIME slučovacího dokumentu musí být [DOCX]." : Return False
                        End If
                    Case BO.x31FormatFlagENUM.Telerik
                        If Right(LCase(cTemp.p85FreeText01), 4) <> "trdx" Then
                            _Error = "Souborová přípona šablony tiskové sestavy musí být [TRDX]." : Return False
                        End If
                End Select
            Else
                If cRec.PID = 0 Then
                    _Error = "Chybí soubor šablony." : Return False
                End If

            End If
        End If
        With cRec
            If .x31IsUsableAsPersonalPage Then
                If .x31FormatFlag <> BO.x31FormatFlagENUM.ASPX Then
                    _Error = "Jako osobní (výchozí) stránka uživatele může fungovat pouze ASPX plugin." : Return False
                Else
                    If .x29ID > BO.x29IdEnum._NotSpecified Then
                        _Error = "Osobní (výchozí) stránka nemůže mít vztah se záznamem entity." : Return False
                    End If
                End If

            End If
            If Trim(.x31Name) = "" Then
                _Error = "Chybí název šablony sestavy/pluginu." : Return False
            End If
            If Trim(.x31Code) = "" Then
                _Error = "Chybí kód šablony sestavy/pluginu." : Return False
            End If
            If .x31IsScheduling Then
                Dim cT As New BO.clsTime
                If cT.ConvertTimeToSeconds(.x31RunInTime) = 0 Then _Error = "Čas automatického generování není zadán správně." : Return False

            End If
        End With

        If _cDL.Save(cRec, lisX69) Then
            Dim intX31ID As Integer = Me.LastSavedPID
            If strUploadGUID <> "" Then
                Dim mq As New BO.myQueryO27
                mq.x31ID = intX31ID
                Dim lisO27 As IEnumerable(Of BO.o27Attachment) = Me.Factory.o27AttachmentBL.GetList(mq)
                For Each cDoc In lisO27
                    'smazat soubor stávající šablony sestavy
                    Me.Factory.o27AttachmentBL.Delete(cDoc.PID)
                    For Each c In lisTempUpload
                        c.p85DataPID = 0
                    Next
                Next
                Me.Factory.o27AttachmentBL.UploadAndSaveUserControl(lisTempUpload, BO.x29IdEnum.x31Report, Me.LastSavedPID)
                lisO27 = Me.Factory.o27AttachmentBL.GetList(mq)
                If lisO27.Count > 0 Then
                    _cDL.UpdateReportFileName(intX31ID, lisO27(0).o27ArchiveFileName)   'aktualizovat info o souborovém jménu sestavy

                    If cRec.x31FormatFlag = BO.x31FormatFlagENUM.ASPX Then
                        'zkopírovat aspx soubor do aplikační složky [Plugins]
                        Dim strFullPath As String = Me.Factory.x35GlobalParam.UploadFolder & "\" & lisO27(0).o27ArchiveFolder & "\" & lisO27(0).o27ArchiveFileName
                        Dim cF As New BO.clsFile
                        If Not cF.CopyFile(strFullPath, BO.ASS.GetApplicationRootFolder & "\Plugins\" & lisO27(0).o27ArchiveFileName) Then
                            _Error = cF.ErrorMessage
                        End If
                    End If

                End If
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Load(intPID As Integer) As BO.x31Report Implements Ix31ReportBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadByFilename(strFileName As String) As BO.x31Report Implements Ix31ReportBL.LoadByFilename
        Return _cDL.LoadByFilename(strFileName)
    End Function
    Public Function LoadByCode(strCode As String) As BO.x31Report Implements Ix31ReportBL.LoadByCode
        Return _cDL.LoadByCode(strCode)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ix31ReportBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x31Report) Implements Ix31ReportBL.GetList
        Return _cDL.GetList(mq)
    End Function

    Public Function GetList_PersonalPageSource() As List(Of BO.x31Report) Implements Ix31ReportBL.GetList_PersonalPageSource
        Dim lisX31 As List(Of BO.x31Report) = Factory.x31ReportBL.GetList(New BO.myQuery).Where(Function(p) p.x31FormatFlag = BO.x31FormatFlagENUM.ASPX And p.x29ID = BO.x29IdEnum._NotSpecified And p.x31IsUsableAsPersonalPage = True).ToList
        Dim c As New BO.x31Report
        c.SetPID(-1)
        c.SetPluginUrl("j03_mypage_greeting.aspx", -10)
        c.x31Name = "Úvodní pozdrav"
        lisX31.Add(c)
        lisX31.Add(GPPS(-999, "Osobní stránka", "j03_mypage.aspx", -10))
        If _cUser.j04IsMenu_Worksheet Then
            lisX31.Add(GPPS(-2, "[Worksheet -> Zapisovat]", "p31_framework.aspx", -11))
            lisX31.Add(GPPS(-3, "[Worksheet -> Zapisovat přes KALENDÁŘ]", "p31_scheduler.aspx", -11))
            lisX31.Add(GPPS(-3, "[Worksheet -> Zapisovat přes DAYLINE]", "p31_timeline.aspx", -11))

            lisX31.Add(GPPS(-3, "[Worksheet -> Zapisovat přes STOPKY]", "p31_timer.aspx", -11))
            lisX31.Add(GPPS(-3, "[Worksheet -> Datový přehled]", "p31_grid.aspx", -11))
            If Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                lisX31.Add(GPPS(-3, "[Worksheet -> PIVOT]", "p31_pivot.aspx", -11))
            End If
        End If

        If _cUser.j04IsMenu_Project Then lisX31.Add(GPPS(-4, "[Projekty]", "p41_framework.aspx", -12))
        If _cUser.j04IsMenu_Contact Then lisX31.Add(GPPS(-5, "[Klienti]", "p28_framework.aspx", -13))
        If _cUser.j04IsMenu_Report Then lisX31.Add(GPPS(-6, "[Tiskové sestavy]", "report_framework.aspx", -14))

        lisX31.Add(GPPS(-998, "[Dokumenty]", "o23_framework.aspx", -30))
        lisX31.Add(GPPS(-998, "[Dispečink úkolů]", "p56_framework.aspx", -30))
        lisX31.Add(GPPS(-998, "[Kalendář]", "entity_scheduler.aspx", -30))
        lisX31.Add(GPPS(-998, "[Helpdesk (zapisování požadavků)]", "helpdesk_default.aspx", -100))

        Return lisX31
    End Function
    Private Function GPPS(intPID As Integer, strX31Name As String, strURL As String, intOrdinary As Integer) As BO.x31Report
        Dim c As New BO.x31Report
        c.SetPID(intPID)
        c.SetPluginUrl(strURL, intOrdinary)
        c.x31Name = strX31Name
        Return c
    End Function

    Public Sub GeneratePerScheduling(cRec As BO.x31Report)
        

        If cRec.x31LastScheduledRun Is Nothing Then

        End If
    End Sub
    

    Public Function IsWaiting4AutoGenerate(cRec As BO.x31Report) As Boolean Implements Ix31ReportBL.IsWaiting4AutoGenerate
        If Not cRec.x31IsScheduling Then Return False
        Dim b As Boolean = False
        With cRec
            If .x31IsRunInDay1 And Weekday(Now, FirstDayOfWeek.Monday) = 1 Then b = True
            If .x31IsRunInDay2 And Weekday(Now, FirstDayOfWeek.Monday) = 2 Then b = True
            If .x31IsRunInDay3 And Weekday(Now, FirstDayOfWeek.Monday) = 3 Then b = True
            If .x31IsRunInDay4 And Weekday(Now, FirstDayOfWeek.Monday) = 4 Then b = True
            If .x31IsRunInDay5 And Weekday(Now, FirstDayOfWeek.Monday) = 5 Then b = True
            If .x31IsRunInDay6 And Weekday(Now, FirstDayOfWeek.Monday) = 6 Then b = True
            If .x31IsRunInDay7 And Weekday(Now, FirstDayOfWeek.Monday) = 7 Then b = True
            If Not b Then Return False
            Dim cT As New BO.clsTime, secs As Integer = TimeOfDay.Hour * 60 * 60 + TimeOfDay.Minute * 60 + TimeOfDay.Second

            If cT.ConvertTimeToSeconds(.x31RunInTime) > secs Then
                If .x31LastScheduledRun Is Nothing Then Return True 'sestava ještě nikdy nebyla generována
                If Day(.x31LastScheduledRun) = Day(Now) And Month(.x31LastScheduledRun) = Month(Now) And Year(.x31LastScheduledRun) = Year(Now) Then
                    Return False    'dnes již byla generována
                End If
                Return True
            End If
        End With
        Return False
    End Function
    Public Function UpdateLastScheduledRun(intX31ID As Integer, dat As Date) As Boolean Implements Ix31ReportBL.UpdateLastScheduledRun
        Return _cDL.UpdateLastScheduledRun(intX31ID, dat)
    End Function
End Class
