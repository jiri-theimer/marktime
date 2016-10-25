Public Class p56_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Public ReadOnly Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidCurP41ID.Value)
        End Get
    End Property
    Private Sub p56_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            gridP31.Factory = .Factory
            ff1.Factory = .Factory
        End With

        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            With Master
                .SiteMenuValue = "p56"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneContactPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneContactPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p56_framework_detail-opgSubgrid")
                    .Add("p56_framework_detail-pid")
                    .Add("p56_framework_detail-chkFFShowFilledOnly")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                End With
                With .Factory.j03UserBL

                    If Master.DataPID = 0 Then
                        Master.DataPID = BO.BAS.IsNullInt(.GetUserParam("p56_framework_detail-pid", "O"))
                        If Master.DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p56")
                    Else
                        If Master.DataPID <> BO.BAS.IsNullInt(.GetUserParam("p56_framework_detail-pid", "O")) Then
                            .SetUserParam("p56_framework_detail-pid", Master.DataPID.ToString)
                        End If
                    End If
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p56_framework_detail-chkFFShowFilledOnly", "0"))
                    If .GetUserParam("p56_framework_detail-opgSubgrid", "p31") = "b05" Then
                        Me.opgSubgrid.SelectedIndex = 1
                    End If
                End With

            End With


            RefreshRecord()

            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                basUIMT.RenderSawMenuItemAsGrid(menu1.FindItemByValue("saw"), "p56")
            End If
        End If

        gridP31.MasterDataPID = Master.DataPID
    End Sub

    Private Sub Handle_Permissions(cRec As BO.p56Task, cP41 As BO.p41Project)
        Dim cDisp As BO.p56RecordDisposition = Master.Factory.p56TaskBL.InhaleRecordDisposition(cRec)
        With cDisp
            If Not .ReadAccess Then
                Master.StopPage("Nedisponujete oprávněním číst tento úkol.")
            End If
            x18_binding.Visible = .OwnerAccess
            menu1.FindItemByValue("cmdEdit").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdCopy").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdNew").Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P56_Creator)
            
        End With
        menu1.FindItemByValue("cmdPivot").Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
        menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p56&masterpid=" & cRec.PID.ToString

        If cDisp.P31_Create Then
            If Not cRec.IsClosed Then
                Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = Master.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.p41ID, cP41.p42ID, cP41.j18ID, Master.Factory.SysUser.j02ID)
                With menu1.FindItemByValue("p31")
                    For Each c In lisP34
                        Dim mi As New Telerik.Web.UI.RadMenuItem(String.Format("Zapsat úkon do [{0}]", c.p34Name), "javascript:p31_entry_menu(" & c.PID.ToString & ")")
                        mi.ImageUrl = "Images/worksheet.png"
                        .Items.Add(mi)
                    Next
                    If lisP34.Count = 0 Then
                        Dim mi As New Telerik.Web.UI.RadMenuItem("V projektu úkolu nedisponujete oprávněním k zapisování úkonů.")
                        mi.ForeColor = Drawing.Color.Red
                        menu1.FindItemByValue("p31").Items.Add(mi)
                    End If
                End With
            Else
                Dim mi As New Telerik.Web.UI.RadMenuItem("Do uzavřeného úkolu nelze zapisovat nové úkony.")
                mi.ForeColor = Drawing.Color.Red
                menu1.FindItemByValue("p31").Items.Add(mi)
                menu1.Skin = "Black"
            End If
        Else
            Dim mi As New Telerik.Web.UI.RadMenuItem("V úkolu nedisponujete oprávněním k zapisování úkonů.")
            mi.ForeColor = Drawing.Color.Red
            menu1.FindItemByValue("p31").Items.Add(mi)
        End If
        Dim bolCanApprove As Boolean = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Approver)
        Dim cDispP41 As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cP41)

        If bolCanApprove = False And cDispP41.x67IDs.Count > 0 Then
            Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Master.Factory.x67EntityRoleBL.GetList_o28(cDispP41.x67IDs)
            If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                bolCanApprove = True
            End If
        End If
        menu1.FindItemByValue("cmdApprove").Visible = bolCanApprove
        If cRec.b01ID <> 0 Then
            menu1.FindItemByValue("cmdB07").Visible = False
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim mq As New BO.myQueryP56
        mq.AddItemToPIDs(Master.DataPID)
        mq.Closed = BO.BooleanQueryMode.NoQuery
        Dim lis As IEnumerable(Of BO.p56TaskWithWorksheetSum) = Master.Factory.p56TaskBL.GetList_WithWorksheetSum(mq)
        If lis.Count = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p56")
        Dim cRec As BO.p56TaskWithWorksheetSum = lis(0)

        Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(cRec.p41ID)
        Me.hidCurP41ID.Value = cP41.PID.ToString

        Handle_Permissions(cRec, cP41)

        With cRec
            Me.Owner.Text = .Owner : Me.Timestamp.Text = .Timestamp
            Me.p56Code.Text = .p56Code
            Me.p56Name.Text = .p56Name
            Me.Project.Text = .ProjectCodeAndName
            Me.Project.NavigateUrl = "p41_framework.aspx?pid=" & .p41ID.ToString
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
            Me.Hours_Orig.Text = BO.BAS.FN(.Hours_Orig)
            If .Expenses_Orig <> 0 Then
                trExpenses.Visible = True
                Me.Expenses_Orig.Text = BO.BAS.FN(.Expenses_Orig)
            End If
            If cRec.p56Plan_Hours > 0 Then
                trPlanHours.Visible = True
                p56Plan_Hours.Text = BO.BAS.FN(.p56Plan_Hours)
                Select Case .p56Plan_Hours - .Hours_Orig
                    Case Is > 0
                        Me.PlanHoursSummary.Text += "zbývá vykázat <span style='color:blue;'>" & BO.BAS.FN(.p56Plan_Hours - .Hours_Orig) & "h.</span>"
                    Case Is < 0
                        Me.PlanHoursSummary.Text += " <img src='Images/warning.png'/> vykázáno přes plán <span style='color:red;'>" & BO.BAS.FN(.Hours_Orig - .p56Plan_Hours) & "h.</span>"
                    Case 0
                        Me.PlanHoursSummary.Text += "vykázáno přesně podle plánu."
                End Select
            End If
            If .p56Plan_Expenses > 0 Then
                trPlanExpenses.Visible = True
                p56Plan_Expenses.Text = BO.BAS.FN(.p56Plan_Expenses)
                Select Case .p56Plan_Expenses - .Expenses_Orig
                    Case Is > 0
                        Me.PlanExpensesSummary.Text += "zbývá vykázat <span style='color:blue;'>" & BO.BAS.FN(.p56Plan_Expenses - .Expenses_Orig) & ",-</span>"
                    Case Is < 0
                        PlanExpensesSummary.Text += " <img src='Images/warning.png'/> vykázáno přes plán <span style='color:red;'>" & BO.BAS.FN(.Expenses_Orig - .p56Plan_Expenses) & ",-.</span>"
                    Case 0
                        PlanExpensesSummary.Text = "vykázáno přesně podle plánu."
                End Select
            End If
        End With


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


        basUIMT.RenderHeaderMenu(cRec.IsClosed, Me.panMenuContainer, menu1)
        basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), cRec.p57Name & ": " & cRec.p56Code, "p56_framework_detail.aspx?pid=" & Master.DataPID.ToString, cRec.IsClosed)

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

        If opgSubgrid.SelectedIndex = 1 Then
            history1.RefreshData(Master.Factory, BO.x29IdEnum.p56Task, Master.DataPID)
        End If
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
    


    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "p56-save"
                Master.DataPID = BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value)
            Case "p56-delete"
                Response.Redirect("entity_framework_detail_missing.aspx?prefix=p56")
            Case "p31-save"
                gridP31.DefaultSelectedPID = BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value)
                gridP31.Rebind(False)
                Return
            Case Else

        End Select
        ReloadPage(Master.DataPID.ToString)

        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""

    End Sub

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p56_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub ReloadPage(strPID As String)
        Response.Redirect("p56_framework_detail.aspx?pid=" & strPID)
    End Sub

    Private Sub opgSubgrid_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles opgSubgrid.TabClick
        Master.Factory.j03UserBL.SetUserParam("p56_framework_detail-opgSubgrid", Me.opgSubgrid.SelectedTab.Value)
        If Me.opgSubgrid.SelectedTab.Value = "b05" Then
            history1.RefreshData(Master.Factory, BO.x29IdEnum.p56Task, Master.DataPID)
        End If

    End Sub

    Private Sub p56_framework_detail_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        gridP31.Visible = False : history1.Visible = False
        Select Case Me.opgSubgrid.SelectedIndex
            Case 0
                gridP31.Visible = True
            Case 1
                history1.Visible = True
        End Select
    End Sub
End Class