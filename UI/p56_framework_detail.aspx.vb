﻿Public Class p56_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub p56_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        menu1.Factory = Master.Factory
        menu1.DataPrefix = "p56"
        ff1.Factory = Master.Factory
    End Sub

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .SiteMenuValue = "p56"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Me.menu1.PageSource = "2" Then
                    .IsHideAllRecZooms = True
                    ''If Request.Item("tab") <> "" Then
                    ''    .Factory.j03UserBL.SetUserParam("p41_framework_detail-tab", Request.Item("tab"))
                    ''End If
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p56_framework_detail-pid")
                    .Add("p56_framework_detail-tab")
                    .Add("p56_menu-tabskin")
                    .Add("p56_framework_detail-chkFFShowFilledOnly")
                    .Add("p56_framework_detail_pos")
                End With
                Dim intPID As Integer = Master.DataPID
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    'úvodní dispečerská stránka
                    If intPID = 0 Then
                        intPID = BO.BAS.IsNullInt(.GetUserParam("p56_framework_detail-pid", "O"))
                        If intPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p56")
                    Else
                        If intPID <> BO.BAS.IsNullInt(.GetUserParam("p56_framework_detail-pid", "O")) Then
                            .SetUserParam("p56_framework_detail-pid", intPID.ToString)
                        End If
                    End If
                    Dim strTab As String = Request.Item("tab")
                    If strTab = "" Then strTab = .GetUserParam("p56_framework_detail-tab", "board")
                    Select Case strTab
                        Case "p31", "time", "expense", "fee", "kusovnik"
                            Server.Transfer("entity_framework_rec_p31.aspx?masterprefix=p56&masterpid=" & intPID.ToString & "&p31tabautoquery=" & strTab & "&source=" & menu1.PageSource, False)
                        Case "o23", "p91", "p56", "summary", "p41"
                            Server.Transfer("entity_framework_rec_" & strTab & ".aspx?masterprefix=p56&masterpid=" & intPID.ToString & "&source=" & menu1.PageSource, False)
                        Case Else
                            'zůstat zde na BOARD stránce
                    End Select
                    menu1.TabSkin = .GetUserParam("p56_menu-tabskin")
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p56_framework_detail-chkFFShowFilledOnly", "0"))

                End With
                Master.DataPID = intPID
            End With


            RefreshRecord()

        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p56Task = Master.Factory.p56TaskBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p56")
        Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(cRec.p41ID)
        Dim cRecSum As BO.p56TaskSum = Master.Factory.p56TaskBL.LoadSumRow(Master.DataPID)
        Dim cDisp As BO.p56RecordDisposition = Master.Factory.p56TaskBL.InhaleRecordDisposition(cRec)

        menu1.p56_RefreshRecord(cRec, cRecSum, cP41, "board", cDisp)

        If Not cDisp.ReadAccess Then Master.StopPage("Nedisponujete oprávněním číst tento úkol.")
        x18_binding.Visible = cDisp.OwnerAccess

        With cRec
            Me.Owner.Text = .Owner : Me.Timestamp.Text = .UserInsert & "/" & .DateInsert
            Me.p56Code.Text = .p56Code
            Me.p56Name.Text = .p56Name
            Me.Project.Text = .ProjectCodeAndName
            If Master.Factory.SysUser.j04IsMenu_Project Then
                Me.Project.NavigateUrl = "p41_framework.aspx?pid=" & .p41ID.ToString
            End If
            Me.clue_project.Attributes("rel") = "clue_p41_record.aspx?pid=" & .p41ID.ToString

            Me.p57Name.Text = .p57Name
            If .p59ID_Submitter > 0 Then
                Me.p59NameSubmitter.Text = .p59NameSubmitter
            Else
                Me.lblp59NameSubmitter.Visible = False
            End If

            If .p58ID > 0 Then
                Me.p58Name.Text = .p58Name
            Else
                trProduct.Visible = False
            End If


            If .p56Description <> "" Then
                panDescription.Visible = True : Me.p56Description.Text = BO.BAS.CrLfText2Html(.p56Description)
            Else
                panDescription.Visible = False
            End If

            If .b01ID > 0 Then
                Me.trWorkflow.Visible = True
                Me.b02Name.Text = .b02Name
            Else
                Me.trWorkflow.Visible = False
            End If
            If Not BO.BAS.IsNullDBDate(.p56PlanUntil) Is Nothing Then
                Me.p56PlanUntil.Text = BO.BAS.FD(.p56PlanUntil, True, True)
                If cRec.p56PlanUntil < Now Then
                    Me.p56PlanUntil.Text += "...je po termínu!" : p56PlanUntil.ForeColor = Drawing.Color.Red
                Else
                    p56PlanUntil.ForeColor = Drawing.Color.Green
                End If
            Else
                lblDeadline.Visible = False
            End If
            Me.Hours_Orig.Text = BO.BAS.FN(cRecSum.Hours_Orig)
            If cRecSum.Expenses_Orig <> 0 Then
                trExpenses.Visible = True
                Me.Expenses_Orig.Text = BO.BAS.FN(cRecSum.Expenses_Orig)
            End If
            If cRec.p56Plan_Hours > 0 Then
                trPlanHours.Visible = True
                p56Plan_Hours.Text = BO.BAS.FN(.p56Plan_Hours)
                Select Case .p56Plan_Hours - cRecSum.Hours_Orig
                    Case Is > 0
                        Me.PlanHoursSummary.Text += "zbývá vykázat <span style='color:blue;'>" & BO.BAS.FN(.p56Plan_Hours - cRecSum.Hours_Orig) & "h.</span>"
                    Case Is < 0
                        Me.PlanHoursSummary.Text += " <img src='Images/warning.png'/> vykázáno přes plán <span style='color:red;'>" & BO.BAS.FN(cRecSum.Hours_Orig - .p56Plan_Hours) & "h.</span>"
                    Case 0
                        Me.PlanHoursSummary.Text += "vykázáno přesně podle plánu."
                End Select
            End If
            If .p56Plan_Expenses > 0 Then
                trPlanExpenses.Visible = True
                p56Plan_Expenses.Text = BO.BAS.FN(.p56Plan_Expenses)
                Select Case .p56Plan_Expenses - cRecSum.Expenses_Orig
                    Case Is > 0
                        Me.PlanExpensesSummary.Text += "zbývá vykázat <span style='color:blue;'>" & BO.BAS.FN(.p56Plan_Expenses - cRecSum.Expenses_Orig) & ",-</span>"
                    Case Is < 0
                        PlanExpensesSummary.Text += " <img src='Images/warning.png'/> vykázáno přes plán <span style='color:red;'>" & BO.BAS.FN(cRecSum.Expenses_Orig - .p56Plan_Expenses) & ",-.</span>"
                    Case 0
                        PlanExpensesSummary.Text = "vykázáno přesně podle plánu."
                End Select
            End If
        End With
        Me.Last_Invoice.Text = cRecSum.Last_Invoice
        Me.Last_WIP_Worksheet.Text = cRecSum.Last_Wip_Worksheet

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p56Task, cRec.PID)
        Me.roles_task.RefreshData(lisX69, cRec.PID)

        Dim mqO23 As New BO.myQueryO23
        mqO23.p56ID = Master.DataPID
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
        If lisO23.Count > 0 Then
            Me.boxO23.Visible = True
            notepad1.RefreshData(lisO23, Master.DataPID)
            With Me.boxO23Title
                .Text = BO.BAS.OM2(.Text, lisO23.Count.ToString)
            End With
        Else
            Me.boxO23.Visible = False
        End If



        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p56Task).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_decide('x18_binding.aspx?prefix=p56&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p56Task, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p56Task, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p56Task, Master.DataPID, cRec.p57ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

        RefreshImapBox(cRec)

        comments1.RefreshData(Master.Factory, BO.x29IdEnum.p56Task, cRec.PID)
    End Sub
    Private Sub RefreshImapBox(cRec As BO.p56Task)
        If cRec.o43ID <> 0 Then
            'úkol byl založen IMAP robotem
            imap1.RefreshData(Master.Factory.o42ImapRuleBL.LoadHistoryByID(cRec.o43ID))
            boxIMAP.Visible = True
        Else
            boxIMAP.Visible = False
        End If
    End Sub
End Class