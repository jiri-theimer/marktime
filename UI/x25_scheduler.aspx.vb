Imports Telerik.Web.UI

Public Class x25_scheduler
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _curD1 As Date
    Private Property _curD2 As Date

    Public ReadOnly Property CurrentX18ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.x18ID.SelectedValue)
        End Get
    End Property
    Public Property CurrentView As SchedulerViewType
        Get
            Return Me.scheduler1.SelectedView
        End Get
        Set(value As SchedulerViewType)
            Me.scheduler1.SelectedView = value
        End Set
    End Property

    Private Sub x25_scheduler_Init(sender As Object, e As EventArgs) Handles Me.Init
        persons1.Factory = Master.Factory
        projects1.Factory = Master.Factory
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .SiteMenuValue = "x25_framework"
                ViewState("loading_setting") = "0"

                Dim strX18ID As String = Request.Item("x18id")
                With Master.Factory.j03UserBL
                    If strX18ID = "" Then
                        .InhaleUserParams("x25_framework-x18id")
                        strX18ID = .GetUserParam("x25_framework-x18id")
                    End If
                End With
                SetupX18Combo(strX18ID)
                If Me.x18ID.Items.Count > 0 Then
                    strX18ID = Me.x18ID.SelectedValue
                    Handle_ChangeX18ID()
                Else
                    strX18ID = ""
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("x25_framework-x18id")
                    .Add("entity_scheduler-view")
                    .Add("entity_scheduler-daystarttime")
                    .Add("entity_scheduler-dayendtime")
                    .Add("entity_scheduler-multidays")
                    .Add("entity_scheduler-persons1-scope")
                    .Add("entity_scheduler-persons1-value")
                    .Add("entity_scheduler-persons1-personsrole")
                    .Add("entity_scheduler-projects1-scope")
                    .Add("entity_scheduler-projects1-value")
                    .Add("entity_scheduler-agendadays")
                    .Add("entity_scheduler-include_childs")

                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    Me.CurrentView = .GetUserParam("entity_scheduler-view", "1")

                    Me.persons1.CurrentScope = .GetUserParam("entity_scheduler-persons1-scope", "4")
                    Me.persons1.CurrentValue = .GetUserParam("entity_scheduler-persons1-value")

                    SetupPersonRolesCombo(.GetUserParam("entity_scheduler-persons1-personsrole"))
                    Me.persons1.CurrentPersonsRole = .GetUserParam("entity_scheduler-persons1-personsrole", "1")

                    Me.projects1.CurrentScope = .GetUserParam("entity_scheduler-projects1-scope", "1")
                    Me.projects1.CurrentValue = .GetUserParam("entity_scheduler-projects1-value")



                    basUI.SelectDropdownlistValue(Me.entity_scheduler_daystarttime, .GetUserParam("entity_scheduler-daystarttime", "8"))
                    basUI.SelectDropdownlistValue(Me.entity_scheduler_dayendtime, .GetUserParam("entity_scheduler-dayendtime", "20"))
                    basUI.SelectDropdownlistValue(Me.entity_scheduler_multidays, .GetUserParam("entity_scheduler-multidays", "2"))
                    basUI.SelectDropdownlistValue(Me.entity_scheduler_agendadays, .GetUserParam("entity_scheduler-agendadays", "20"))

                    
                End With
            End With


            RefreshData(False)
        End If
    End Sub

    Private Sub SetupX18Combo(strDef As String)
        Dim mq As New BO.myQuery
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_X18_Admin, BO.x53PermValEnum.GR_Admin) Then
            mq.MyRecordsDisponible = True
        End If

        Dim lis As IEnumerable(Of BO.x18EntityCategory) = Master.Factory.x18EntityCategoryBL.GetList(mq).Where(Function(p) p.x18IsCalendar = True)
        Me.x18ID.DataSource = lis
        Me.x18ID.DataBind()
        If lis.Count = 0 Then
            Master.Notify("V databázi zatím neexistuje štítek pro kalendářové rozhraní.", NotifyLevel.InfoMessage)
        Else
            If strDef <> "" Then basUI.SelectDropdownlistValue(Me.x18ID, strDef)
        End If

    End Sub

    Private Sub SetupPersonRolesCombo(strDef As String)
        Dim lis As New List(Of BO.ComboSource)
        Dim c As New BO.ComboSource
        c.pid = -1 : c.ItemText = "Zakladatel záznamu"
        lis.Add(c)
        c = New BO.ComboSource
        c.pid = 1
        c.ItemText = "Nominovaný (schvalovatel/řešitel)"
        lis.Add(c)
        persons1.SetupQueryPersonsRoles(lis)
        persons1.CurrentPersonsRole = strDef
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
        Response.Redirect("x25_scheduler.aspx?x18id=" & Me.CurrentX18ID.ToString)

    End Sub

    Private Sub Handle_ChangeX18ID()
        Dim c As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Me.CurrentX18ID)
        hidX23ID.Value = c.x23ID.ToString

        hidx18IsColors.Value = BO.BAS.GB(c.x18IsColors)
        hidCalendarFieldStart.Value = c.x18CalendarFieldStart
        hidCalendarFieldEnd.Value = c.x18CalendarFieldEnd

        Dim cDisp As BO.x18RecordDisposition = Master.Factory.x18EntityCategoryBL.InhaleDisposition(c)
        ''menu1.FindItemByValue("cmdNew").Visible = cDisp.CreateItem

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
            .AgendaView.NumberOfDays = BO.BAS.IsNullInt(Me.entity_scheduler_agendadays.SelectedValue)

        End With
        Dim d1 As Date = scheduler1.VisibleRangeStart.AddDays(-1), d2 As Date = scheduler1.VisibleRangeEnd.AddDays(1)

        Dim mq As New BO.myQueryX25(BO.BAS.IsNullInt(hidX23ID.Value))
        If panPersons.Visible Then
            If persons1.CurrentPersonsRole = "-1" Then
                mq.Owners = persons1.CurrentJ02IDs
            Else
                mq.j02IDs = persons1.CurrentJ02IDs
            End If
        End If
        If panProjects.Visible Then
            mq.p41IDs = projects1.CurrentP41IDs
        End If


        mq.CalendarDateFieldStart = hidCalendarFieldStart.Value
        mq.CalendarDateFieldEnd = hidCalendarFieldEnd.Value
        mq.DateFrom = d1
        mq.DateUntil = d2

        Master.Factory.x25EntityField_ComboValueBL.SetCalendarDateFields(hidCalendarFieldStart.Value, hidCalendarFieldEnd.Value)
        Dim lis As IEnumerable(Of BO.x25EntityField_ComboValue) = Master.Factory.x25EntityField_ComboValueBL.GetList(mq)
        For Each cRec In lis
            Dim c As New Appointment()
            With cRec
                c.ID = .PID.ToString & ",'p56'"
                c.Description = "clue_x25_record.aspx?pid=" & .PID.ToString
                c.Subject = .x25Name
                If c.Subject = "" Then c.Subject = .x25Code
                c.Start = .CalendarDateStart
                c.End = .CalendarDateEnd

                If .b02ID <> 0 Then
                    If .b02Color <> "" Then
                        c.BackColor = Drawing.Color.FromName(.b02Color)
                    End If
                Else
                    If .x25BackColor <> "" Then
                        c.BackColor = Drawing.Color.FromName(.x25BackColor)
                        c.ForeColor = Drawing.Color.FromName(.x25ForeColor)
                    End If
                End If


                If (c.End.Hour = 23 And c.End.Minute = 59) Or (c.End.Hour = 0 And c.End.Minute = 0 And c.End.Second = 0) Then
                    c.Start = DateSerial(Year(c.Start), Month(c.Start), Day(c.Start))
                    c.End = DateSerial(Year(c.End), Month(c.End), Day(c.End)).AddDays(1)
                End If

                Select Case Me.CurrentView
                    Case SchedulerViewType.MonthView, SchedulerViewType.TimelineView, SchedulerViewType.WeekView
                        If Len(c.Subject) > 0 Then c.Subject = BO.BAS.OM3(c.Subject, 15)


                End Select
            End With

            scheduler1.InsertAppointment(c)
        Next
    End Sub

    Private Sub persons1_OnChange() Handles persons1.OnChange
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-persons1-scope", persons1.CurrentScope.ToString)
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-persons1-value", persons1.CurrentValue)
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-persons1-personsrole", persons1.CurrentPersonsRole)

        RefreshData(False)
        hidIsPersonsChange.Value = "1"
    End Sub
    Private Sub projects1_OnChange() Handles projects1.OnChange
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-projects1-scope", projects1.CurrentScope.ToString)
        Master.Factory.j03UserBL.SetUserParam("entity_scheduler-projects1-value", projects1.CurrentValue)
        RefreshData(False)
        hidIsProjectsChange.Value = "1"
    End Sub

    Private Sub x25_scheduler_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        PersonsHeader.Text = persons1.CurrentHeader
        ProjectsHeader.Text = projects1.CurrentHeader
    End Sub

End Class