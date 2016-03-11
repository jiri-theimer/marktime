Public Class clue_o22_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub clue_o22_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
            If Request.Item("noclue") = "1" Or Request.Item("dr") = "1" Then
                ViewState("noclue") = "1"
            End If
            RefreshRecord()

            If Request.Item("mode") = "readonly" Then
                panContainer.Style.Clear()
                cmDetail.Visible = False
                Master.HeaderText = Me.o21Name.Text & " | " & ph1.Text
                ph1.Visible = False
            End If
            comments1.RefreshData(Master.Factory, BO.x29IdEnum.o22Milestone, Master.DataPID)
        End If
    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then Master.StopPage("pid is missing", , , False)

        Dim cO22 As BO.o22Milestone = Master.Factory.o22MilestoneBL.Load(Master.DataPID)
        With cO22
            Master.DataPID = .PID
            If .p41ID <> 0 Then
                ViewState("masterprefix") = "p41"
                ViewState("masterpid") = .p41ID.ToString
            End If
            If .p28ID <> 0 Then
                ViewState("masterprefix") = "p28"
                ViewState("masterpid") = .p28ID.ToString
            End If
            If .j02ID <> 0 Then
                ViewState("masterprefix") = "j02"
                ViewState("masterpid") = .j02ID.ToString
            End If
            ph1.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.o22Milestone, Master.DataPID, True)
            Select Case Request.Item("mode")
                Case "timeline"
                    Me.o21Name.Visible = False
                    If .o21Flag = BO.o21FlagEnum.DeadlineOrMilestone Then
                        img1.ImageUrl = "Images/calendar_32.png"
                    Else
                        img1.ImageUrl = "Images/event_32.png"
                    End If

                Case Else

                    Me.o21Name.Text = .o21Name
                    Select Case .x29ID
                        Case BO.x29IdEnum.p41Project
                            img1.ImageUrl = "Images/project_32.png"
                        Case BO.x29IdEnum.p28Contact
                            img1.ImageUrl = "Images/contact_32.png"
                        Case BO.x29IdEnum.j02Person
                            img1.ImageUrl = "Images/person_32.png"
                    End Select
            End Select
            Master.HeaderText = .o21Name

            Me.Period.Text = .Period
            Me.o22Name.Text = .o22Name
            Me.o22Location.Text = .o22Location
            If .o22Location = "" Then Me.lblLocation.Visible = False
            Me.o22Description.Text = .o22Description
            Me.Timestamp.Text = .Timestamp
            If .o22DateUntil.Value < Now Then
                Me.Period.Font.Strikeout = True
                Me.lblPeriodMessage.Visible = True
            End If
            If Not BO.BAS.IsNullDBDate(.o22ReminderDate) Is Nothing Then
                Me.o22ReminderDate.Text = BO.BAS.FD(.o22ReminderDate.Value, True, True)
                If .o22ReminderDate.Value < Now Then
                    Me.o22ReminderDate.Font.Strikeout = True
                End If
            Else
                Me.lblReminder.Visible = False
            End If
        End With

        rpO20.DataSource = Master.Factory.o22MilestoneBL.GetList_o20(Master.DataPID)
        rpO20.DataBind()
        If rpO20.Items.Count = 0 Then rpO20.Visible = False

        rpO19.DataSource = Master.Factory.o22MilestoneBL.GetList_o19(Master.DataPID)
        rpO19.DataBind()
        If rpO19.Items.Count = 0 Then panO19.Visible = False


    End Sub

    Private Sub clue_o22_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If ViewState("noclue") = "1" Then
            panContainer.Style.Clear()  'stránka nemá mít chování info bubliny
            panHeader.Visible = False
        Else
            panHeader.Visible = True
        End If
    End Sub
End Class