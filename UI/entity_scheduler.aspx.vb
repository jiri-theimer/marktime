Imports Telerik.Web.UI

Public Class entity_scheduler
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _curD1 As Date
    Private Property _curD2 As Date

    Public Property CurrentView As SchedulerViewType
        Get
            Return Me.scheduler1.SelectedView
        End Get
        Set(value As SchedulerViewType)
            Me.scheduler1.SelectedView = value
        End Set
    End Property
    Public Property CurrentJ02IDs As List(Of Integer)
        Get
            Return BO.BAS.ConvertPIDs2List(Me.hidJ02IDs_All.Value, ",")
        End Get
        Set(value As List(Of Integer))
            Me.hidJ02IDs_All.Value = String.Join(",", value)
            If value.Count = 1 Then
                If value(0) = Master.Factory.SysUser.j02ID Then
                    Me.Persons.Text = Master.Factory.SysUser.Person
                    Return
                End If
            End If
            Me.Persons.Text = ""
            If Me.hidJ07IDs.Value <> "" Then
                Dim mq As New BO.myQuery
                mq.PIDs = BO.BAS.ConvertPIDs2List(Me.hidJ07IDs.Value)
                Me.Persons.Text += " ,<span style='color:red;'>" & String.Join(", ", Master.Factory.j07PersonPositionBL.GetList(mq).Select(Function(p) p.j07Name)) & "</span>"
            End If
            If Me.hidJ11IDs.Value <> "" Then
                Dim mq As New BO.myQuery
                mq.PIDs = BO.BAS.ConvertPIDs2List(Me.hidJ11IDs.Value)
                Me.Persons.Text += " ,<span style='color:green;'>" & String.Join(", ", Master.Factory.j11TeamBL.GetList(mq).Select(Function(p) p.j11Name)) & "</span>"
            End If
            If Me.hidJ02IDs.Value <> "" Then
                Dim mq As New BO.myQueryJ02
                mq.PIDs = BO.BAS.ConvertPIDs2List(Me.hidJ02IDs.Value)
                Me.Persons.Text += " ," & String.Join(", ", Master.Factory.j02PersonBL.GetList(mq).Select(Function(p) p.FullNameDesc))
            End If
            Me.Persons.Text = BO.BAS.OM1(Trim(Me.Persons.Text), True)

        End Set
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentMasterX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.hidMasterPrefix.Value)
        End Get
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            hidMasterPID.Value = value.ToString
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .SiteMenuValue = "entity_scheduler"
                If Request.Item("masterpid") <> "" Then
                    Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid")) : Me.CurrentMasterPrefix = Request.Item("masterprefix")
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("entity_scheduler-view")
                    .Add("entity_scheduler-daystarttime")
                    .Add("entity_scheduler-dayendtime")
                    .Add("entity_scheduler-multidays")
                    .Add("entity_scheduler-j02ids")
                    .Add("entity_scheduler-j11ids")
                    .Add("entity_scheduler-j07ids")
                    .Add("entity_scheduler-j02ids_all")
                    .Add("entity_scheduler-o22")
                    .Add("entity_scheduler-p48")
                    .Add("entity_scheduler-p56")
                    .Add("entity_scheduler-newrec_prefix")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    Me.CurrentView = .GetUserParam("entity_scheduler-view", "1")
                    Me.hidJ02IDs.Value = .GetUserParam("entity_scheduler-j02ids", Master.Factory.SysUser.j02ID.ToString)
                    Me.hidJ11IDs.Value = .GetUserParam("entity_scheduler-j11ids")
                    Me.hidJ07IDs.Value = .GetUserParam("entity_scheduler-j07ids")
                    Dim strJ02IDs As String = .GetUserParam("entity_scheduler-j02ids_all", Master.Factory.SysUser.j02ID.ToString)
                    Me.CurrentJ02IDs = BO.BAS.ConvertPIDs2List(strJ02IDs, ",")

                    basUI.SelectDropdownlistValue(Me.entity_scheduler_daystarttime, .GetUserParam("entity_scheduler-daystarttime", "8"))
                    basUI.SelectDropdownlistValue(Me.entity_scheduler_dayendtime, .GetUserParam("entity_scheduler-dayendtime", "20"))
                    basUI.SelectDropdownlistValue(Me.entity_scheduler_multidays, .GetUserParam("entity_scheduler-multidays", "2"))
                    basUI.SelectDropdownlistValue(Me.cbxNewRecType, .GetUserParam("entity_scheduler-newrec_prefix", "p48"))

                    Me.chkSetting_P48.Checked = .GetUserParam("entity_scheduler-p48", "1")
                    Me.chkSetting_O22.Checked = .GetUserParam("entity_scheduler-o22", "1")
                    Me.chkSetting_P56.Checked = .GetUserParam("entity_scheduler-p56", "0")

                End With



            End With
            Me.j11ID_Add.DataSource = Master.Factory.j11TeamBL.GetList(New BO.myQuery).Where(Function(p) p.j11IsAllPersons = False)
            Me.j11ID_Add.DataBind()
            Me.j07ID_Add.DataSource = Master.Factory.j07PersonPositionBL.GetList(New BO.myQuery)
            Me.j07ID_Add.DataBind()
            Me.j02ID_Add.Flag = "all"


            RefreshRecord()
            RefreshData(False)
        End If
    End Sub


    Private Sub Handle_ChangeJ02IDs(bolAppend As Boolean)
        Dim intJ11ID As Integer = BO.BAS.IsNullInt(Me.j11ID_Add.SelectedValue)
        Dim intJ07ID As Integer = BO.BAS.IsNullInt(Me.j07ID_Add.SelectedValue)
        Dim intJ02ID As Integer = BO.BAS.IsNullInt(Me.j02ID_Add.Value)
        If intJ02ID = 0 And intJ07ID = 0 And intJ11ID = 0 Then
            Master.Notify("Musíte vybrat osobu, tým nebo pozici.", NotifyLevel.WarningMessage)
            Return
        End If
        If Not bolAppend Then
            Me.hidJ02IDs.Value = "" : Me.hidJ07IDs.Value = "" : Me.hidJ11IDs.Value = ""
        End If
        Dim j02ids_all As New List(Of Integer)
        If intJ02ID > 0 Then
            j02ids_all.Add(intJ02ID)
            Me.hidJ02IDs.Value += "," & intJ02ID.ToString
            
        End If
        If intJ07ID > 0 Then
            Dim mq As New BO.myQueryJ02
            mq.j07ID = intJ07ID
            mq.SpecificQuery = BO.myQueryJ02_SpecificQuery.AllowedForRead
            For Each x In Master.Factory.j02PersonBL.GetList(mq).Select(Function(p) p.PID).ToList
                j02ids_all.Add(x)
            Next
            Me.hidJ07IDs.Value += "," & intJ07ID.ToString
        End If
        If intJ11ID <> 0 Then
            Dim mq As New BO.myQueryJ02
            mq.j11ID = intJ11ID
            mq.SpecificQuery = BO.myQueryJ02_SpecificQuery.AllowedForRead
            For Each x In Master.Factory.j02PersonBL.GetList(mq).Select(Function(p) p.PID).ToList
                j02ids_all.Add(x)
            Next
            Me.hidJ11IDs.Value += "," & intJ11ID.ToString
        End If
        If j02ids_all.Count = 0 Then
            Master.Notify("Vstupní podmínce neodpovídá ani jeden osobní profil.", NotifyLevel.WarningMessage)
            Return
        End If
        Me.hidJ02IDs.Value = BO.BAS.OM1(Me.hidJ02IDs.Value, True)
        Me.hidJ07IDs.Value = BO.BAS.OM1(Me.hidJ07IDs.Value, True)
        Me.hidJ11IDs.Value = BO.BAS.OM1(Me.hidJ11IDs.Value, True)
        If bolAppend Then
            AppendCurrentJ02IDs(j02ids_all)
        Else
            Me.CurrentJ02IDs = j02ids_all
        End If
        Me.SaveCurrentPersonsScope()
        RefreshData(False)

    End Sub

    Private Sub AppendCurrentJ02IDs(j02ids As List(Of Integer))
        Dim cj As List(Of Integer) = Me.CurrentJ02IDs
        For Each x In j02ids
            If cj.Where(Function(p) p = x).Count = 0 Then
                cj.Add(x)
            End If
        Next
        Me.CurrentJ02IDs = cj

    End Sub
    Private Sub SaveCurrentPersonsScope()
        With Master.Factory.j03UserBL
            .SetUserParam("entity_scheduler-j02ids_all", Me.hidJ02IDs_All.Value)
            .SetUserParam("entity_scheduler-j02ids", Me.hidJ02IDs.Value)
            .SetUserParam("entity_scheduler-j07ids", Me.hidJ07IDs.Value)
            .SetUserParam("entity_scheduler-j11ids", Me.hidJ11IDs.Value)
        End With

    End Sub

    Private Sub RefreshData(bolData4Export As Boolean)
        With Me.scheduler1
            .Appointments.Clear()
            .DayView.DayStartTime = System.TimeSpan.FromHours(CDbl(Me.entity_scheduler_daystarttime.SelectedValue))
            .DayView.DayEndTime = System.TimeSpan.FromHours(CDbl(Me.entity_scheduler_dayendtime.SelectedValue))
            .WeekView.DayStartTime = .DayView.DayStartTime
            .WeekView.DayEndTime = .DayView.DayEndTime
            .MultiDayView.DayStartTime = .DayView.DayStartTime
            .MultiDayView.DayEndTime = .DayView.DayEndTime
            .MultiDayView.NumberOfDays = BO.BAS.IsNullInt(Me.entity_scheduler_multidays.SelectedValue)
            .Localization.HeaderMultiDay = "Multi-den (" & .MultiDayView.NumberOfDays.ToString & ")"
            ''If .SelectedView = SchedulerViewType.MonthView Then
            ''    .RowHeight = Unit.Parse("40px")
            ''Else
            ''    .RowHeight = Nothing
            ''End If
        End With
        Dim d1 As Date = scheduler1.VisibleRangeStart.AddDays(-1), d2 As Date = scheduler1.VisibleRangeEnd.AddDays(1)
        

        If Me.chkSetting_O22.Checked Then
            Dim mq As New BO.myQueryO22
            'mq.SpecificQuery = BO.myQueryO22_SpecificQuery.AllowedForRead
            Select Case Me.CurrentMasterX29ID
                Case BO.x29IdEnum.p28Contact
                    mq.p28ID = Me.CurrentMasterPID
                Case BO.x29IdEnum.p41Project
                    mq.p41ID = Me.CurrentMasterPID
                Case BO.x29IdEnum.j02Person
                    mq.j02ID = Me.CurrentMasterPID
            End Select
            mq.DateFrom = d1 : mq.DateUntil = d2
            Dim lis As IEnumerable(Of BO.o22Milestone) = Master.Factory.o22MilestoneBL.GetList(mq)
            For Each cRec In lis
                Dim c As New Appointment()
                With cRec
                    c.ID = .PID.ToString & ",'o22'"
                    c.Description = "clue_o22_record.aspx?mode=timeline&pid=" & .PID.ToString
                    c.Subject = .o22Name
                    ''If .o22DateUntil < datFoundMin Then datFoundMin = .o22DateUntil
                    Select Case .o21Flag
                        Case BO.o21FlagEnum.DeadlineOrMilestone
                            c.Description = "calendar.png"
                            'c.BackColor = Drawing.Color.Aquamarine
                            c.BackColor = Drawing.Color.Salmon
                            c.Start = .o22DateUntil.Value
                            c.End = .o22DateUntil.Value


                        Case BO.o21FlagEnum.EventFromUntil
                            c.Description = "event.png"
                            c.BackColor = Drawing.Color.AntiqueWhite
                            c.Start = .o22DateFrom
                            c.End = .o22DateUntil
                            If .o22IsAllDay Then
                                c.Start = DateSerial(Year(c.Start), Month(c.Start), Day(c.Start))
                                c.End = c.Start.AddDays(1)
                            End If

                    End Select
                    c.BorderColor = Drawing.Color.Gray
                    c.BorderStyle = BorderStyle.Dashed

                    Select Case Me.CurrentView
                        Case SchedulerViewType.MonthView, SchedulerViewType.TimelineView, SchedulerViewType.WeekView
                            If Len(.o22Name) > 0 Then c.Subject = BO.BAS.OM3(.o22Name, 15)

                            c.ToolTip = BO.BAS.FD(.o22DateUntil, True)
                    End Select
                End With

                scheduler1.InsertAppointment(c)
            Next
        End If
        If Me.chkSetting_P48.Checked Then
            Dim mq As New BO.myQueryP48
            Select Case Me.CurrentMasterX29ID
                Case BO.x29IdEnum.p28Contact
                    mq.p28ID = Me.CurrentMasterPID
                Case BO.x29IdEnum.p41Project
                    mq.p41ID = Me.CurrentMasterPID
                Case BO.x29IdEnum.j02Person
                    mq.j02IDs = BO.BAS.ConvertInt2List(Me.CurrentMasterPID)
            End Select
            mq.DateFrom = d1 : mq.DateUntil = d2
            Dim lis As IEnumerable(Of BO.p48OperativePlan) = Master.Factory.p48OperativePlanBL.GetList(mq)
            For Each cRec In lis
                Dim c As New Appointment()
                With cRec
                    c.ID = .PID.ToString & ",'p48'"
                    c.Description = "clue_p48_record.aspx?pid=" & .PID.ToString & "&js_edit=p48_record(" & .PID.ToString & ")&js_convert=p48_convert(" & .PID.ToString & ")"

                    If Not .p48DateTimeFrom Is Nothing Then
                        c.Start = .p48DateTimeFrom
                    Else
                        c.Start = .p48Date
                    End If
                    If Not .p48DateTimeUntil Is Nothing Then
                        c.End = .p48DateTimeUntil
                    Else
                        c.End = .p48Date.AddDays(1)
                    End If
                    If .p31ID > 0 Then
                        c.Font.Strikeout = True 'plán byl zkonvertován do worksheetu
                        c.Description += "&js_p31record=p31_record(" & .p31ID.ToString & ")"
                    End If
                    c.BorderColor = Drawing.Color.Silver
                    'c.BorderStyle = BorderStyle.Dotted
                    c.BackColor = Drawing.Color.WhiteSmoke

                    c.Subject = .p48Hours.ToString & "h."
                    Select Case Me.CurrentMasterX29ID
                        Case BO.x29IdEnum.p41Project
                            c.Subject += " " & .Person
                        Case BO.x29IdEnum.p28Contact
                            c.Subject += " " & .Person & ": " & .Project
                        Case BO.x29IdEnum.j02Person
                            c.Subject += " " & .ClientAndProject
                        Case Else
                            If Me.CurrentJ02IDs.Count = 1 Then
                                c.Subject += " " & .ClientAndProject
                            Else
                                c.Subject += " " & .Person & ": " & .ClientAndProject
                            End If

                    End Select

                    Select Case Me.CurrentView
                        Case SchedulerViewType.MonthView, SchedulerViewType.TimelineView
                            c.ToolTip = c.Subject
                            If Len(c.Subject) > 22 Then
                                c.Subject = Left(c.Subject, 20) & "..."
                            End If
                        Case Else

                    End Select
                End With

                scheduler1.InsertAppointment(c)
            Next
        End If
        
    End Sub

    Private Sub RefreshRecord()
        If Me.CurrentMasterPrefix <> "" Then
            Me.MasterRecord.NavigateUrl = Me.CurrentMasterPrefix & "_framework.aspx?pid" & Me.CurrentMasterPID.ToString
            With Me.MasterRecord
                .Text = Master.Factory.GetRecordCaption(Me.CurrentMasterX29ID, Me.CurrentMasterPID)
                If .Text.Length > 37 Then .Text = Left(.Text, 35) & "..."
            End With
            Select Case Me.CurrentMasterPrefix
                Case "p41"
                    imgMaster.ImageUrl = "Images/project.png"
                Case "p28"
                    imgMaster.ImageUrl = "Images/contact.png"
                Case "j02"
                    imgMaster.ImageUrl = "Images/person.png"
                    panPersonScope.Visible = False
                    Me.Persons.Text = ""
            End Select
        Else
            panMasterRecord.Visible = False
        End If

    End Sub
    Private Sub cmdAppendJ02IDs_Click(sender As Object, e As EventArgs) Handles cmdAppendJ02IDs.Click
        Handle_ChangeJ02IDs(True)
    End Sub

    Private Sub cmdReplaceJ02IDs_Click(sender As Object, e As EventArgs) Handles cmdReplaceJ02IDs.Click
        Handle_ChangeJ02IDs(False)
    End Sub

    Private Sub scheduler1_NavigationComplete(sender As Object, e As SchedulerNavigationCompleteEventArgs) Handles scheduler1.NavigationComplete
        Dim bolChangeView As Boolean = False
        Select Case e.Command
            Case SchedulerNavigationCommand.SwitchToAgendaView, SchedulerNavigationCommand.SwitchToMonthView, SchedulerNavigationCommand.SwitchToTimelineView, SchedulerNavigationCommand.SwitchToWeekView, SchedulerNavigationCommand.SwitchToMultiDayView
                bolChangeView = True
        End Select
        If bolChangeView Then
            Master.Factory.j03UserBL.SetUserParam("entity_scheduler-view", CInt(Me.CurrentView).ToString)
        End If
        RefreshData(False)
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("entity_scheduler.aspx")

    End Sub

    Private Sub cmdHardRefreshOnBehind_Click(sender As Object, e As EventArgs) Handles cmdHardRefreshOnBehind.Click

        RefreshData(False)

    End Sub

    Private Sub entity_scheduler_dayendtime_SelectedIndexChanged(sender As Object, e As EventArgs) Handles entity_scheduler_dayendtime.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-dayendtime", Me.entity_scheduler_dayendtime.SelectedValue)
        RefreshData(False)
    End Sub

    Private Sub entity_scheduler_daystarttime_SelectedIndexChanged(sender As Object, e As EventArgs) Handles entity_scheduler_daystarttime.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-daystarttime", Me.entity_scheduler_daystarttime.SelectedValue)
        RefreshData(False)
    End Sub

    Private Sub p31_scheduler_multidays_SelectedIndexChanged(sender As Object, e As EventArgs) Handles entity_scheduler_multidays.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-multidays", Me.entity_scheduler_multidays.SelectedValue)
        Me.CurrentView = SchedulerViewType.MultiDayView
        RefreshData(False)

    End Sub

    Private Sub cmdExportICalendar_Click(sender As Object, e As EventArgs) Handles cmdExportICalendar.Click
        RefreshData(True)
        Dim s As String = RadScheduler.ExportToICalendar(scheduler1.Appointments())
        Dim response As HttpResponse = Page.Response
        response.Clear()
        response.Buffer = True
        response.ContentType = "text/calendar"
        response.ContentEncoding = Encoding.UTF8
        response.Charset = "utf-8"
        response.AddHeader("Content-Disposition", _
                  "attachment;filename=""MARKTIME_CALENDAR.ics""")
        response.Write(s)
        response.[End]()
    End Sub

    Private Sub chkSetting_P48_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_P48.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-p48", BO.BAS.GB(Me.chkSetting_P48.Checked))
        RefreshData(False)
    End Sub

    Private Sub chkSetting_O22_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_O22.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-o22", BO.BAS.GB(Me.chkSetting_O22.Checked))
        RefreshData(False)
    End Sub

    Private Sub chkSetting_P56_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_P56.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-p56", BO.BAS.GB(Me.chkSetting_P56.Checked))
        RefreshData(False)
    End Sub

    Private Sub cbxNewRecType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxNewRecType.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-newrec_prefix", Me.cbxNewRecType.SelectedValue)
    End Sub
End Class