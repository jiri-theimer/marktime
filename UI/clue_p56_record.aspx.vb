Public Class clue_p56_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub clue_p56_record_Init(sender As Object, e As EventArgs) Handles Me.Init
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
                Master.HeaderText = ph1.Text
                ph1.Visible = False
                cmdMove2Bin.Visible = False
                cmdWorkflow.Visible = False
            End If

            If Not cmdWorkflow.Visible Then
                comments1.RefreshData(Master.Factory, BO.x29IdEnum.p56Task, Master.DataPID)
            Else
                comments1.Visible = False
            End If

        End If
    End Sub

    Private Sub RefreshRecord()
        Dim mq As New BO.myQueryP56
        mq.AddItemToPIDs(Master.DataPID)
        mq.Closed = BO.BooleanQueryMode.NoQuery
        Dim lis As IEnumerable(Of BO.p56TaskWithWorksheetSum) = Master.Factory.p56TaskBL.GetList_WithWorksheetSum(mq)
        Dim cRec As BO.p56TaskWithWorksheetSum = lis(0)
        Me.Project.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, cRec.p41ID)
        With cRec
            Master.HeaderText = .p57Name & ": " & .p56Code
            ph1.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .PID, True)
            Me.p56Name.Text = .p56Name
            Me.p56Name.Font.Strikeout = .IsClosed
            Me.p56Description.Text = .p56Description
            Me.Hours_Orig.Text = BO.BAS.FN(.Hours_Orig)
            Me.b02Name.Text = .b02Name
            Me.Timestamp.Text = .Timestamp

        End With
        Me.RolesInLine.Text = Master.Factory.p56TaskBL.GetRolesInline(Master.DataPID)
        If Not BO.BAS.IsNullDBDate(cRec.p56PlanUntil) Is Nothing Then

            Me.p56PlanUntil.Text = BO.BAS.FD(cRec.p56PlanUntil, True, True)
            If cRec.p56PlanUntil < Now Then
                Me.p56PlanUntil.Text += "...je po termínu!" : p56PlanUntil.ForeColor = Drawing.Color.Red
            Else
                p56PlanUntil.ForeColor = Drawing.Color.Green
            End If

        End If
        If Not BO.BAS.IsNullDBDate(cRec.p56PlanFrom) Is Nothing Then
            Me.p56PlanFrom.Text = BO.BAS.FD(cRec.p56PlanFrom, True, True)
        Else
            lblp56PlanFrom.Visible = False
        End If
        If cRec.Expenses_Orig > 0 Then
            trExpenses.Visible = True
            Me.Expenses_Orig.Text = BO.BAS.FN(cRec.Expenses_Orig)
            
        End If
        If cRec.p56Plan_Hours > 0 Then
            trPlanHours.Visible = True
            p56Plan_Hours.Text = BO.BAS.FN(cRec.p56Plan_Hours)
            Select Case cRec.p56Plan_Hours - cRec.Hours_Orig
                Case Is > 0
                    Me.PlanHoursSummary.Text += "zbývá vykázat <span style='color:blue;'>" & BO.BAS.FN(cRec.p56Plan_Hours - cRec.Hours_Orig) & "h.</span>"
                Case Is < 0
                    Me.PlanHoursSummary.Text += " <img src='Images/warning.png'/> vykázáno přes plán <span style='color:red;'>" & BO.BAS.FN(cRec.Hours_Orig - cRec.p56Plan_Hours) & "h.</span>"
                Case 0
                    Me.PlanHoursSummary.Text += "vykázáno přesně podle plánu."
            End Select
            
            
        End If
        If cRec.p56Plan_Expenses > 0 Then
            trPlanExpenses.Visible = True
            p56Plan_Expenses.Text = BO.BAS.FN(cRec.p56Plan_Expenses)
            Select Case cRec.p56Plan_Expenses - cRec.Expenses_Orig
                Case Is > 0
                    Me.PlanExpensesSummary.Text += "zbývá vykázat <span style='color:blue;'>" & BO.BAS.FN(cRec.p56Plan_Expenses - cRec.Expenses_Orig) & ",-</span>"
                Case Is < 0
                    PlanExpensesSummary.Text += " <img src='Images/warning.png'/> vykázáno přes plán <span style='color:red;'>" & BO.BAS.FN(cRec.Expenses_Orig - cRec.p56Plan_Expenses) & ",-.</span>"
                Case 0
                    PlanExpensesSummary.Text = "vykázáno přesně podle plánu."
            End Select
            
        End If


        If Not cRec.IsClosed Then
            cmdMove2Bin.Visible = Master.Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.p41Project, cRec.p41ID, BO.x53PermValEnum.PR_P56_Bin, True)
            If Not cmdMove2Bin.Visible Then
                cmdMove2Bin.Visible = Master.Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.p41Project, cRec.p41ID, BO.x53PermValEnum.PR_P56_Owner, True)
            End If
        Else
            img1.ImageUrl = "Images/bin_32.png"
            p56Name.Font.Strikeout = True
            cmdMove2Bin.Visible = False
        End If
        If cRec.b01ID <> 0 Then
            cmdWorkflow.Visible = True
        Else
            cmdWorkflow.Visible = False
        End If
        trWorkflow.Visible = cmdWorkflow.Visible
        trName.Visible = Not cmdWorkflow.Visible
        If cmdWorkflow.Visible Then cmdMove2Bin.Visible = False

    End Sub

    Private Sub cmdMove2Bin_Click(sender As Object, e As EventArgs) Handles cmdMove2Bin.Click
        Dim cRec As BO.p56Task = Master.Factory.p56TaskBL.Load(Master.DataPID)
        cRec.ValidUntil = Now.AddMinutes(-1)
        If Not Master.Factory.p56TaskBL.Save(cRec, Nothing, Nothing, "") Then
            Master.Notify(Master.Factory.p56TaskBL.ErrorMessage, NotifyLevel.ErrorMessage)
            Return
        End If
        RefreshRecord()
    End Sub

    Private Sub clue_p56_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If ViewState("noclue") = "1" Then
            panContainer.Style.Clear()  'stránka nemá mít chování info bubliny
            panHeader.Visible = False
        Else
            panHeader.Visible = True
        End If
    End Sub
End Class