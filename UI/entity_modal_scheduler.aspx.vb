Imports Telerik.Web.UI

Public Class entity_modal_scheduler
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentX29ID As BO.x29IdEnum
        Get
            Return DirectCast(CInt(Me.hidX29ID.Value), BO.x29IdEnum)
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidX29ID.Value = CInt(value).ToString
            Me.hidPrefix.Value = BO.BAS.GetDataPrefix(value)
        End Set
    End Property
    Public Property CurrentView As SchedulerViewType
        Get
            Return Me.scheduler1.SelectedView
        End Get
        Set(value As SchedulerViewType)
            Me.scheduler1.SelectedView = value
        End Set
    End Property
    Public ReadOnly Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentX29ID = BO.BAS.GetX29FromPrefix(Request.Item("prefix"))
            If Me.CurrentX29ID = BO.x29IdEnum._NotSpecified Then
                Master.StopPage("prefix is missing")
            End If
            With Master
                .AddToolbarButton("Nastavení", "setting", 0, "Images/arrow_down.gif", False)
                .RadToolbar.FindItemByValue("setting").CssClass = "show_hide1"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then
                    .StopPage("pid is missing.")
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add(Me.CurrentPrefix & "_timeline-view")
                    .Add(Me.CurrentPrefix & "_timeline-o23")
                    .Add(Me.CurrentPrefix & "_timeline-show-record")
                    .Add(Me.CurrentPrefix & "_timeline-show-o22")
                    .Add(Me.CurrentPrefix & "_timeline-show-o23")
                    .Add(Me.CurrentPrefix & "_timeline-show-p56")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    Me.CurrentView = .GetUserParam(Me.CurrentPrefix & "_timeline-view", "2")
                    'Me.chkSetting_Record.Checked = .GetUserParam(Me.CurrentPrefix & "_timeline-show-record", "1")
                    Me.chkSetting_O22.Checked = .GetUserParam(Me.CurrentPrefix & "_timeline-show-o22", "1")
                    'Me.chkSetting_O23.Checked = .GetUserParam(Me.CurrentPrefix & "_timeline-show-o23", "1")
                    Me.chkSetting_P56.Checked = .GetUserParam(Me.CurrentPrefix & "_timeline-show-p56", "1")
                End With
               


            End With

            RefreshRecord()
            Handle_Permissions()
            RefreshAppointments()
        End If
    End Sub

    Private Sub RefreshRecord()
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
                If cRec Is Nothing Then NoRec()
                Master.HeaderText = cRec.FullName

            Case BO.x29IdEnum.p28Contact
                Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
                If cRec Is Nothing Then NoRec()
                Master.HeaderText = cRec.p28Name

            Case BO.x29IdEnum.j02Person
                Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
                If cRec Is Nothing Then NoRec()
                If Not cRec.j02IsIntraPerson Then Master.StopPage("U kontaktních osob nelze aplikovat kalendář.", False)

                Master.HeaderText = cRec.FullNameAsc

        End Select

    End Sub

    Private Sub Handle_Permissions()
        
    End Sub



    Private Sub NoRec()
        Response.Redirect("entity_framework_detail_missing.aspx?prefix=" & Me.CurrentPrefix)

    End Sub



    Private Sub RefreshAppointments()
        With Me.scheduler1
            .Appointments.Clear()
        End With
        Dim d1 As Date = scheduler1.VisibleRangeStart.AddDays(-1), d2 As Date = scheduler1.VisibleRangeEnd.AddDays(1)
        Select Case Me.CurrentView
            Case SchedulerViewType.AgendaView, SchedulerViewType.TimelineView
                d1 = DateSerial(1900, 1, 1)
                d2 = DateSerial(3000, 1, 1)
        End Select
        Dim datFoundMin As Date = d2

        'If Me.chkSetting_Record.Checked Then
        '    Dim lisX90 As IEnumerable(Of BO.x90EntityLog) = Nothing
        '    Select Case Me.CurrentX29ID
        '        Case BO.x29IdEnum.p28Contact
        '            lisX90 = Master.Factory.p28ContactBL.GetList_x90(Master.DataPID, d1, d2).Where(Function(p) p.x90EventFlag <> BO.x90EventFlagEnum.Updated)
        '        Case BO.x29IdEnum.p41Project
        '            lisX90 = Master.Factory.p41ProjectBL.GetList_x90(Master.DataPID, d1, d2).Where(Function(p) p.x90EventFlag <> BO.x90EventFlagEnum.Updated)
        '        Case BO.x29IdEnum.j02Person
        '            lisX90 = Master.Factory.j02PersonBL.GetList_x90(Master.DataPID, d1, d2).Where(Function(p) p.x90EventFlag <> BO.x90EventFlagEnum.Updated)
        '    End Select
        '    For Each cRec In lisX90
        '        Dim c As New Appointment
        '        c.Description = "record.png"
        '        With cRec
        '            c.ID = "clue_x90_record.aspx?pid=" & .x90ID.ToString
        '            c.Start = .x90Date
        '            c.End = .x90Date
        '            If .x90Date < datFoundMin Then datFoundMin = .x90Date

        '            Select Case .x90EventFlag
        '                Case BO.x90EventFlagEnum.Created
        '                    c.BackColor = Drawing.Color.Lime
        '                    c.Subject = "Založení záznamu"

        '                Case BO.x90EventFlagEnum.MovedToBin
        '                    c.Subject = "Do koše"
        '                    If Me.CurrentView <> SchedulerViewType.AgendaView Then
        '                        c.BackColor = Drawing.Color.Black
        '                        c.ForeColor = Drawing.Color.White
        '                    End If


        '                Case BO.x90EventFlagEnum.RestoreFromBin
        '                    c.BackColor = Drawing.Color.Yellow
        '                    c.Subject = "Z koše"
        '            End Select
        '            c.BorderColor = Drawing.Color.Gray
        '            c.BorderStyle = BorderStyle.Dashed
        '            'c.ToolTip = BO.BAS.FD(.x90Date, True) & " | " & c.Subject

        '        End With
        '        scheduler1.InsertAppointment(c)
        '    Next
        'End If
        If Me.chkSetting_O22.Checked Then
            Dim mq As New BO.myQueryO22
            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p28Contact
                    mq.p28ID = Master.DataPID
                Case BO.x29IdEnum.p41Project
                    mq.p41ID = Master.DataPID
                Case BO.x29IdEnum.j02Person
                    mq.j02ID = Master.DataPID
            End Select
            mq.DateFrom = d1 : mq.DateUntil = d2
            Dim lis As IEnumerable(Of BO.o22Milestone) = Master.Factory.o22MilestoneBL.GetList(mq)
            For Each cRec In lis
                Dim c As New Appointment()
                With cRec
                    c.ID = "clue_o22_record.aspx?mode=timeline&pid=" & .PID.ToString
                    c.Subject = .o22Name
                    If .o22DateUntil < datFoundMin Then datFoundMin = .o22DateUntil
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
        If Me.chkSetting_P56.Checked Then
            Dim mq As New BO.myQueryP56
            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p28Contact
                    mq.p28ID = Master.DataPID
                Case BO.x29IdEnum.p41Project
                    mq.p41ID = Master.DataPID
                Case BO.x29IdEnum.j02Person
                    mq.j02ID = Master.DataPID
            End Select
            mq.DateFrom = d1 : mq.DateUntil = d2
            Dim lis As IEnumerable(Of BO.p56Task) = Master.Factory.p56TaskBL.GetList(mq)
            For Each cRec In lis
                Dim c As New Appointment()
                With cRec
                    c.ID = "clue_p56_record.aspx?mode=timeline&pid=" & .PID.ToString
                    c.Subject = .p56Name
                    c.Description = "task.png"

                    If BO.BAS.IsNullDBDate(.p56PlanUntil) Is Nothing Then
                        c.Start = .DateInsert
                        c.End = .DateInsert
                    Else
                        If Not BO.BAS.IsNullDBDate(.p56PlanFrom) Is Nothing Then
                            c.Start = .p56PlanFrom
                        Else
                            c.Start = .p56PlanUntil
                        End If
                        c.End = .p56PlanUntil
                    End If

                    c.BackColor = Drawing.Color.FromName("#3CB371")



                    Select Case Me.CurrentView
                        Case SchedulerViewType.MonthView, SchedulerViewType.TimelineView, SchedulerViewType.WeekView
                            If Len(.p56Name) > 0 Then c.Subject = BO.BAS.OM3(.p56Name, 15)


                    End Select
                End With

                scheduler1.InsertAppointment(c)
            Next
        End If

        'If Me.chkSetting_O23.Checked Then
        '    Dim mq As New BO.myQueryO23
        '    Select Case Me.CurrentX29ID
        '        Case BO.x29IdEnum.p28Contact
        '            mq.p28ID = Master.DataPID
        '        Case BO.x29IdEnum.p41Project
        '            mq.p41ID = Master.DataPID
        '        Case BO.x29IdEnum.j02Person
        '            mq.j02ID = Master.DataPID
        '    End Select
        '    mq.DateFrom = d1 : mq.DateUntil = d2
        '    Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mq)

        '    For Each cRec In lisO23
        '        Dim c As New Appointment
        '        c.Description = "notepad.png"

        '        With cRec
        '            c.ID = "clue_o23_record.aspx?mode=timeline&pid=" & .PID.ToString
        '            c.Start = .o23Date
        '            c.End = .o23Date
        '            c.Subject = .o23Name
        '            c.BackColor = Drawing.Color.Gold

        '            If .o23Date < datFoundMin Then datFoundMin = .o23Date
        '        End With
        '        scheduler1.InsertAppointment(c)
        '    Next
        'End If

        If Year(datFoundMin) < 3000 Then
            Select Case Me.CurrentView
                Case SchedulerViewType.AgendaView, SchedulerViewType.TimelineView
                    scheduler1.SelectedDate = datFoundMin
            End Select
        End If

    End Sub








    Private Sub scheduler1_NavigationComplete(sender As Object, e As SchedulerNavigationCompleteEventArgs) Handles scheduler1.NavigationComplete
        Dim bolChangeView As Boolean = False
        Select Case e.Command
            Case SchedulerNavigationCommand.SwitchToAgendaView, SchedulerNavigationCommand.SwitchToMonthView, SchedulerNavigationCommand.SwitchToTimelineView, SchedulerNavigationCommand.SwitchToWeekView
                bolChangeView = True
        End Select
        If bolChangeView Then
            Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix & "_timeline-view", CInt(Me.CurrentView).ToString)
            RefreshAppointments()
        End If

    End Sub

    'Private Sub chkSetting_Record_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_Record.CheckedChanged
    '    Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix & "_timeline-show-record", BO.BAS.GB(Me.chkSetting_Record.Checked))
    '    ReloadPage()
    'End Sub

    'Private Sub chkSetting_O23_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_O23.CheckedChanged
    '    Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix & "_timeline-show-o23", BO.BAS.GB(Me.chkSetting_O23.Checked))
    '    ReloadPage()
    'End Sub

    Private Sub chkSetting_O22_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_O22.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix & "_timeline-show-o22", BO.BAS.GB(Me.chkSetting_O22.Checked))
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("entity_modal_scheduler.aspx?prefix=" & Me.CurrentPrefix & "&pid=" & Master.DataPID.ToString)

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        RefreshAppointments()
    End Sub


    Private Sub chkSetting_P56_CheckedChanged(sender As Object, e As EventArgs) Handles chkSetting_P56.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix & "_timeline-show-p56", BO.BAS.GB(Me.chkSetting_P56.Checked))
        ReloadPage()
    End Sub
End Class