Imports Telerik.Web.UI
Public Class myscheduler
    Inherits System.Web.UI.UserControl
    Public Property factory As BL.Factory

    Public Property NumberOfDays As Integer
        Get
            Return Me.scheduler1.AgendaView.NumberOfDays
        End Get
        Set(value As Integer)
            Me.scheduler1.AgendaView.NumberOfDays = value
        End Set
    End Property
    Public Property Prefix As String
        Get
            Return hidPrefix.Value
        End Get
        Set(value As String)
            hidPrefix.Value = value
        End Set
    End Property
    Public Property RecordPID As Integer
        Get
            Return BO.BAS.IsNullInt(hidRecordPID.Value)
        End Get
        Set(value As Integer)
            hidRecordPID.Value = value.ToString
        End Set
    End Property
    Public ReadOnly Property RowsCount As Integer
        Get
            Return scheduler1.Appointments.Count
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RefreshData(d0 As Date)
        If Me.RecordPID = 0 Then Return
        scheduler1.SelectedDate = d0
        scheduler1.Appointments.Clear()

        Dim d1 As Date = d0.AddDays(-5)
        Dim d2 As Date = d1.AddDays(Me.NumberOfDays)

        fill_o22(d1, d2)
        fill_p56(d1.AddDays(-100), d2)

    End Sub
    Private Sub fill_o22(d1 As Date, d2 As Date)
        Dim intRecordPID As Integer = Me.RecordPID
        Dim mq As New BO.myQueryO22
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        'mq.SpecificQuery = BO.myQueryO22_SpecificQuery.AllowedForRead
        Select Case hidPrefix.Value
            Case "p28"
                mq.p28ID = intRecordPID
            Case "p41"
                mq.p41ID = intRecordPID

            Case "j02"
                mq.j02IDs = BO.BAS.ConvertInt2List(intRecordPID)
            Case Else
                Return
        End Select
        mq.DateFrom = d1 : mq.DateUntil = d2
        Dim lis As IEnumerable(Of BO.o22Milestone) = factory.o22MilestoneBL.GetList(mq)
        For Each cRec In lis
            Dim c As New Appointment()
            With cRec
                c.ID = .PID.ToString & ",'o22'"
                c.Description = "clue_o22_record.aspx?pid=" & .PID.ToString
                c.Subject = .o22Name
                Select Case .o21Flag
                    Case BO.o21FlagEnum.DeadlineOrMilestone
                        'c.BackColor = Drawing.Color.Aquamarine
                        c.BackColor = Drawing.Color.Salmon
                        c.Start = .o22DateUntil.Value
                        c.End = .o22DateUntil.Value


                    Case BO.o21FlagEnum.EventFromUntil
                        c.BackColor = Drawing.Color.AntiqueWhite
                        If Not .o22DateFrom Is Nothing Then
                            c.Start = .o22DateFrom
                        Else
                            c.Start = .o22DateUntil
                        End If
                        c.End = .o22DateUntil

                        If .o22IsAllDay Then
                            c.Start = DateSerial(Year(c.Start), Month(c.Start), Day(c.Start))
                            c.End = c.End.AddDays(1)
                        End If
                        

                End Select
                If c.End.Hour = 0 And c.End.Minute = 0 And c.End.Second = 0 Then    'nastavit jako celo-denní událost bez času od-do
                    c.Start = DateSerial(Year(c.Start), Month(c.Start), Day(c.Start))
                    c.End = DateSerial(Year(c.End), Month(c.End), Day(c.End)).AddDays(1)

                End If
                If c.End < Today Then c.Font.Strikeout = True

                c.BorderColor = Drawing.Color.Gray
                c.BorderStyle = BorderStyle.Dashed
            End With
            scheduler1.InsertAppointment(c)
        Next
    End Sub
    Private Sub fill_p56(d1 As Date, d2 As Date)
        Dim intRecordPID As Integer = Me.RecordPID
        Dim mq As New BO.myQueryP56
        mq.TopRecordsOnly = 100
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        Select Case hidPrefix.Value
            Case "p28"
                mq.p28ID = intRecordPID
            Case "p41"
                mq.p41ID = intRecordPID
            Case "j02"
                mq.j02ID = intRecordPID
            Case Else
                Return
        End Select
        mq.p56PlanUntil_D1 = d1 : mq.p56PlanUntil_D2 = d2
        Dim lis As IEnumerable(Of BO.p56Task) = factory.p56TaskBL.GetList(mq)
        For Each cRec In lis
            Dim c As New Appointment()
            With cRec
                c.ID = .PID.ToString & ",'p56'"
                c.Description = "clue_p56_record.aspx?pid=" & .PID.ToString
                c.Subject = "<img src='Images/task.png'/>" & .p56Name


                If .p57PlanDatesEntryFlag = 4 And Not .p56PlanFrom Is Nothing Then
                    c.Start = .p56PlanFrom
                Else
                    c.Start = .p56PlanUntil
                End If
                c.End = .p56PlanUntil

                c.BackColor = Drawing.Color.FromName("#3CB371")

                If (c.End.Hour = 23 And c.End.Minute = 59) Or (c.End.Hour = 0 And c.End.Minute = 0 And c.End.Second = 0) Then   'scheduler1.SelectedView = SchedulerViewType.DayView And 
                    c.Start = DateSerial(Year(c.End), Month(c.End), Day(c.End))
                    c.End = DateSerial(Year(c.End), Month(c.End), Day(c.End)).AddDays(1)

                End If

            End With

            scheduler1.InsertAppointment(c)
        Next

    End Sub

    Private Sub scheduler1_NavigationComplete(sender As Object, e As SchedulerNavigationCompleteEventArgs) Handles scheduler1.NavigationComplete
        Dim bolChangeView As Boolean = False
        Select Case e.Command
            Case SchedulerNavigationCommand.SwitchToAgendaView, SchedulerNavigationCommand.SwitchToMonthView, SchedulerNavigationCommand.SwitchToTimelineView, SchedulerNavigationCommand.SwitchToWeekView, SchedulerNavigationCommand.SwitchToMultiDayView
                bolChangeView = True
        End Select
        
        RefreshData(scheduler1.SelectedDate)
    End Sub
End Class