Imports Telerik.Web.UI
Imports System.Web.Script.Serialization
Public Class p41_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Enum SubgridType
        summary = -1
        p31 = 1
        p91 = 2
        b07 = 3
        p56 = 4
        p45 = 5
        _NotSpecified = 0
    End Enum
    Public Property CurrentSubgrid As SubgridType
        Get
            If opgSubgrid.SelectedTab Is Nothing Then
                Return SubgridType._NotSpecified
            End If
            Return DirectCast(CInt(Me.opgSubgrid.SelectedTab.Value), SubgridType)
        End Get
        Set(value As SubgridType)
            Me.opgSubgrid.FindTabByValue(CInt(value).ToString).Selected = True
        End Set
    End Property
    Public ReadOnly Property CurrentP28ID_Client As Integer
        Get
            Return BO.BAS.IsNullInt(ViewState("p28id_client"))
        End Get
    End Property
    
    Private Sub p41_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            p31summary1.Factory = .Factory
            ff1.Factory = .Factory
        End With
        If Not Page.IsPostBack Then
            ViewState("p28id_client") = ""
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            With Master
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p41_framework_detail-subgrid", Request.Item("tab"))
                End If
                
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneProjectPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneProjectPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p41_framework_detail-pid")
                    .Add("p41_framework_detail-subgrid")
                    .Add("p41_framework_detail-chkFFShowFilledOnly")
                    .Add("p41_framework_detail-switch")
                    .Add("p41_framework_detail-switchHeight")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail-chkFFShowFilledOnly", "0"))
                    panSwitch.Style.Item("display") = .GetUserParam("p41_framework_detail-switch", "block")
                End With


                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p41_framework_detail-pid", "O"))
                    If .DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")
                Else
                    If .DataPID <> BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p41_framework_detail-pid", "O")) Then
                        .Factory.j03UserBL.SetUserParam("p41_framework_detail-pid", .DataPID.ToString)
                    End If
                End If
                With .Factory.j03UserBL
                    Dim strHeight As String = .GetUserParam("p41_framework_detail-switchHeight", "auto")
                    If strHeight = "auto" Then
                        panSwitch.Style.Item("height") = "" : panSwitch.Style.Item("overflow") = ""
                    Else
                        panSwitch.Style.Item("height") = strHeight & "px"
                    End If
                    Me.CurrentSubgrid = DirectCast(CInt(.GetUserParam("p41_framework_detail-subgrid", "1")), SubgridType)
                    If Request.Item("force") = "comment" Then
                        Me.CurrentSubgrid = SubgridType.b07
                    End If

                End With


            End With

            RefreshRecord()

        End If

        For Each t As RadTab In Me.opgSubgrid.Tabs
            Select Case t.Value
                Case "-1" : t.NavigateUrl = "entity_framework_p31summary.aspx?masterprefix=p41&masterpid=" & Master.DataPID.ToString & "&IsApprovingPerson=" & Me.hidIsCanApprove.Value
                Case "1" : t.NavigateUrl = "entity_framework_p31subform.aspx?masterprefix=p41&masterpid=" & Master.DataPID.ToString & "&IsApprovingPerson=" & Me.hidIsCanApprove.Value
                Case "2" : t.NavigateUrl = "entity_framework_p91subform.aspx?masterprefix=p41&masterpid=" & Master.DataPID.ToString & "&IsApprovingPerson=" & Me.hidIsCanApprove.Value
                Case "3" : t.NavigateUrl = "entity_framework_b07subform.aspx?masterprefix=p41&masterpid=" & Master.DataPID.ToString
                Case "4" : t.NavigateUrl = "entity_framework_p56subform.aspx?masterprefix=p41&masterpid=" & Master.DataPID.ToString & "&IsApprovingPerson=" & Me.hidIsCanApprove.Value
                Case "5" : t.NavigateUrl = "p41_framework_detail_budget.aspx?masterpid=" & Master.DataPID.ToString & "&IsApprovingPerson=" & Me.hidIsCanApprove.Value
            End Select

        Next
        If Me.CurrentSubgrid = SubgridType._NotSpecified Then
            fraSubform.Visible = False : imgLoading.Visible = False
            panSwitch.Style.Item("height") = ""
            For Each t As RadTab In Me.opgSubgrid.Tabs
                t.NavigateUrl = ""
            Next
        Else
            fraSubform.Visible = True

            fraSubform.Attributes.Item("src") = Me.opgSubgrid.SelectedTab.NavigateUrl
            If Me.CurrentSubgrid = SubgridType.p31 And Me.hidHardRefreshFlag.Value = "p31-save" Then
                fraSubform.Attributes.Item("src") += "&pid=" & Me.hidHardRefreshPID.Value
            End If
        End If
    End Sub
    


    Private Sub RefreshRecord()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")
        Handle_Permissions(cRec)

        Dim cClient As BO.p28Contact = Nothing

        With cRec
            cmdNewWindow.NavigateUrl = "p41_framework.aspx?blankwindow=1&pid=" & .PID.ToString & "&title=" & .FullName
            ViewState("p28id_client") = .p28ID_Client.ToString

            Me.Project.Text = .p41Name & " <span style='color:gray;padding-left:10px;'>" & .p41Code & "</span>"

            If .p28ID_Client > 0 Then
                Me.Client.Text = .Client : Me.Client.Visible = True
                If Master.Factory.SysUser.j04IsMenu_Contact Then
                    Me.Client.NavigateUrl = "p28_framework.aspx?pid=" & .p28ID_Client.ToString
                End If
                cClient = Master.Factory.p28ContactBL.Load(.p28ID_Client)
                Me.clue_client.Attributes("rel") = "clue_p28_record.aspx?pid=" & .p28ID_Client.ToString
            Else
                Me.clue_client.Visible = False : Me.Client.Visible = False
            End If
            If .j18ID > 0 Then
                Me.clue_j18name.Attributes("rel") = "clue_j18_record.aspx?pid=" & .j18ID.ToString
            Else
                Me.clue_j18name.Visible = False
            End If
            Me.p42Name.Text = .p42Name
            Me.clue_p42name.Attributes("rel") = "clue_p42_record.aspx?pid=" & .p42ID.ToString
            lblJ18Name.Visible = False : Me.j18Name.Visible = False
            If .j18ID > 0 Then
                lblJ18Name.Visible = True : Me.j18Name.Visible = True
                Me.j18Name.Text = .j18Name
            End If
            If .b01ID > 0 Then
                Me.trWorkflow.Visible = True
                Me.b02Name.Text = .b02Name
            Else
                Me.trWorkflow.Visible = False
            End If

            If Not (.p41PlanFrom Is Nothing Or .p41PlanUntil Is Nothing) Then
                Me.PlanPeriod.Text = "<b style='color:green;'>" & BO.BAS.FD(.p41PlanFrom.Value) & "</b> - <b style='color:red;'>" & BO.BAS.FD(.p41PlanUntil.Value) & "</b>"
                If DateDiff(DateInterval.Day, .p41PlanFrom.Value, .p41PlanUntil.Value) < 750 Then
                    Me.PlanPeriod.Text += " [" & DateDiff(DateInterval.Day, .p41PlanFrom.Value, .p41PlanUntil.Value).ToString & "d.]"
                End If
                trPlan.Visible = True
            Else
                trPlan.Visible = False

            End If

            Me.imgDraft.Visible = .p41IsDraft
            If .p41ParentID <> 0 Then
                Me.trParent.Visible = True
                Me.ParentProject.NavigateUrl = "p41_framework.aspx?pid=" & .p41ParentID.ToString
                Me.ParentProject.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .p41ParentID)
            End If
        End With

        RefreshBillingLanguage(cRec, cClient)

        RefreshPricelist(cRec, cClient)

        RefreshOtherBillingSetting(cRec, cClient)

        If Not (cRec.IsClosed Or cRec.p41IsDraft) Then
            Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = Master.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.PID, cRec.p42ID, cRec.j18ID, Master.Factory.SysUser.j02ID)
            With menu1.FindItemByValue("p31")
                For Each c In lisP34
                    Dim mi As New Telerik.Web.UI.RadMenuItem(String.Format(Resources.p41_framework_detail.ZapsatUkonDo, c.p34Name), "javascript:p31_entry_menu(" & c.PID.ToString & ")")
                    mi.ImageUrl = "Images/worksheet.png"
                    .Items.Add(mi)
                Next
                If lisP34.Count = 0 Then
                    Dim mi As New Telerik.Web.UI.RadMenuItem(Resources.p41_framework_detail.NedisponujeteOpravnenimZapisovat)
                    mi.ForeColor = Drawing.Color.Red
                    menu1.FindItemByValue("p31").Items.Add(mi)
                End If
            End With
        Else
            Dim mi As New Telerik.Web.UI.RadMenuItem("V projektu nemůžete zapisovat worksheet úkony.")
            If cRec.IsClosed Then
                hidIsBin.Value = "1"
                mi.Text = Resources.p41_framework_detail.VArchivuNelzeWorksheet
            End If
            If cRec.p41IsDraft Then mi.Text = Resources.p41_framework_detail.VDraftNelzeWorksheet
            mi.ForeColor = Drawing.Color.Red
            menu1.FindItemByValue("p31").Items.Add(mi)
        End If


        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)

        Dim mqO23 As New BO.myQueryO23
        mqO23.p41ID = Master.DataPID
        mqO23.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
        If lisO23.Count > 0 Then
            Me.boxO23.Visible = True
            notepad1.RefreshData(lisO23, Master.DataPID)
            With Me.boxO23Title
                .Text = BO.BAS.OM2(.Text, lisO23.Count.ToString)
                If menu1.FindItemByValue("cmdO23").Visible Then
                    .Text = "<a href='javascript:notepads()'>" & .Text & "</a>"
                End If
            End With
        Else
            Me.boxO23.Visible = False
        End If
        Dim lisP30 As IEnumerable(Of BO.j02Person) = Master.Factory.p30Contact_PersonBL.GetList_J02(0, Master.DataPID, False)
        If lisP30.Count > 0 Then
            Me.boxP30.Visible = True
            Me.persons1.FillData(lisP30)
            With Me.boxP30Title
                .Text = BO.BAS.OM2(.Text, lisP30.Count.ToString)
                If Master.Factory.SysUser.j04IsMenu_People Then
                    .Text = "<a href='j02_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString & "' target='_top'>" & .Text & "</a>"
                End If
            End With
        Else
            Me.boxP30.Visible = False
        End If

        Dim mq As New BO.myQueryP31
        mq.p41ID = cRec.PID

        Dim cProjectSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

        If Me.CurrentSubgrid = SubgridType.summary Then
            boxP31Summary.Visible = False
        Else
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            p31summary1.RefreshData(cWorksheetSum, "p41", Master.DataPID, Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates), cRec.p41LimitHours_Notification, cRec.p41LimitFee_Notification)

        End If

        With Me.opgSubgrid.Tabs
            If Not .FindTabByValue("2") Is Nothing Then
                If cProjectSum.p91_Count > 0 Then
                    .FindTabByValue("2").Text += "<span class='badge1'>" & cProjectSum.p91_Count.ToString & "</span>"
                    topLink6.Text += "<span class='badge1'>" & cProjectSum.p91_Count.ToString & "</span>"
                End If
            End If
            With .FindTabByValue("4")
                If cProjectSum.p56_Actual_Count > 0 Then .Text += "<span class='badge1'>" & cProjectSum.p56_Actual_Count.ToString & "</span>"
            End With
        End With
        If Not Me.opgSubgrid.Tabs.FindTabByValue("5") Is Nothing Then
            Dim x As Integer = Master.Factory.p45BudgetBL.GetList(cRec.PID).Count
            If x > 0 Then
                With Me.opgSubgrid.Tabs.FindTabByValue("5")
                    .Text += "<span class='badge1'>" & x.ToString & "</span>"
                End With
            End If
        End If
        

        If cProjectSum.p56_Actual_Count > 0 Or cProjectSum.o22_Actual_Count > 0 Then
            If cProjectSum.p56_Actual_Count > 0 Then topLink2.Text = topLink2.Text & "<span title='Otevřené úkoly' class='badge1'>" & cProjectSum.p56_Actual_Count.ToString & "</span>"
            If cProjectSum.o22_Actual_Count > 0 Then topLink3.Text = topLink3.Text & "<span title='Události v kalendáři' class='badge1'>" & cProjectSum.o22_Actual_Count.ToString & "</span>"

        End If

        basUIMT.RenderHeaderMenu(cRec.IsClosed, Me.panMenuContainer, menu1)
        Dim strLevel1 As String = cRec.FullName
        If Len(cRec.Client) > 30 Then
            strLevel1 = Left(cRec.Client, 30) & "..."
            If cRec.p41NameShort <> "" Then
                strLevel1 += " - " & cRec.p41NameShort
            Else
                strLevel1 += " - " & cRec.p41Name
            End If
        End If
        basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), strLevel1, "p41_framework_detail.aspx?pid=" & cRec.PID.ToString, cRec.IsClosed)

        RefreshComments()


        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p41Project, Master.DataPID, cRec.p42ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p41Project).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_decide('x18_binding.aspx?prefix=p41&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p41Project, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p41Project, cRec.PID))
        Else
            boxX18.Visible = False
        End If
        If Master.Factory.p41ProjectBL.HasChildRecords(cRec.PID) Then
            topLink7.Visible = True
            Dim mq2 As New BO.myQueryP41
            mq2.MG_SelectPidFieldOnly = True
            mq2.p41ParentID = cRec.PID
            topLink7.Text += "<span class='badge1'>" & Master.Factory.p41ProjectBL.GetList(mq2).Count.ToString & "</span>"
        End If
        If Master.Factory.p41ProjectBL.IsMyFavouriteProject(cRec.PID) Then
            cmdFavourite.ImageUrl = "Images/favourite.png"
            cmdFavourite.ToolTip = "Vyřadit z mých oblíbených projektů"
        Else
            cmdFavourite.ImageUrl = "Images/not_favourite.png"
            cmdFavourite.ToolTip = "Zařadit do mých oblíbených projektů"
        End If

        RefreshP40(cRec)
    End Sub

    Private Sub RefreshP40(cRec As BO.p41Project)
        Dim lisP40 As IEnumerable(Of BO.p40WorkSheet_Recurrence) = Master.Factory.p40WorkSheet_RecurrenceBL.GetList(Master.DataPID)
        rpP40.DataSource = lisP40
        rpP40.DataBind()
        If lisP40.Count = 0 Then
            boxP40.Visible = False
        Else
            boxP40.Visible = True
        End If
    End Sub


    Private Sub RefreshComments()
        Dim mqB07 As New BO.myQueryB07
        mqB07.RecordDataPID = Master.DataPID
        mqB07.x29id = BO.x29IdEnum.p41Project
        Dim lisB07 As IEnumerable(Of BO.b07Comment) = Master.Factory.b07CommentBL.GetList(mqB07)
        If lisB07.Count > 0 Then
            With Me.opgSubgrid.Tabs.FindTabByValue("3")
                .Text += "<span class='badge1'>" & lisB07.Count.ToString & "</span>"
            End With
        End If
    End Sub

    Private Sub Handle_Permissions(cRec As BO.p41Project)
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString

        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        menu1.FindItemByValue("cmdLog").Visible = cDisp.OwnerAccess
        x18_binding.Visible = cDisp.OwnerAccess
        With Master.Factory
            menu1.FindItemByValue("cmdNew").Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)
            menu1.FindItemByValue("cmdCopy").Visible = menu1.FindItemByValue("cmdNew").Visible
            menu1.FindItemByValue("cmdPivot").Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString
            menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator)
            menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)
            If cRec.b01ID <> 0 Then menu1.FindItemByValue("cmdB07").Visible = False

            Dim bolCanApproveOrInvoice As Boolean = .TestPermission(BO.x53PermValEnum.GR_P31_Approver, BO.x53PermValEnum.GR_P91_Creator)
            If Not bolCanApproveOrInvoice Then bolCanApproveOrInvoice = .TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator)
            If bolCanApproveOrInvoice = False And cDisp.x67IDs.Count > 0 Then
                Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Master.Factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                    bolCanApproveOrInvoice = True
                End If
            End If
            topLink1.Visible = bolCanApproveOrInvoice
            If Not bolCanApproveOrInvoice Then Me.p31summary1.DisableApprovingButton()
            Me.hidIsCanApprove.Value = BO.BAS.GB(bolCanApproveOrInvoice)
            ''Me.bigsummary1.IsApprovingPerson = bolCanApprove
            ''gridP31.AllowApproving = bolCanApprove
            ''gridP56.AllowApproving = bolCanApprove
        End With
        With cDisp
            menu1.FindItemByValue("cmdP56").Visible = .P56_Create
            ''gridP56.IsAllowedCreateTasks = .P56_Create

            boxP30.Visible = .OwnerAccess
            menu1.FindItemByValue("cmdP31Recalc").Visible = .P31_RecalcRates
            menu1.FindItemByValue("cmdP31Move2Bin").Visible = .P31_Move2Bin
            menu1.FindItemByValue("cmdP31MoveToOtherProject").Visible = .P31_MoveToOtherProject
            menu1.FindItemByValue("cmdEdit").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdP40Create").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdP30").Visible = .OwnerAccess
            topLink6.Visible = Master.Factory.SysUser.j04IsMenu_Invoice
            If Not .p91_Read Then
                topLink6.Visible = False
                With Me.opgSubgrid.Tabs
                    If Not .FindTabByValue("2") Is Nothing Then .Remove(.FindTabByValue("2")) 'nemá právo vidět vystavené faktury v projektu
                End With
                If Me.CurrentSubgrid = SubgridType.p91 Then Me.CurrentSubgrid = SubgridType.p31
            End If
            If Not .p45_Read Then
                With Me.opgSubgrid.Tabs
                    If Not .FindTabByValue("5") Is Nothing Then .Remove(.FindTabByValue("5")) 'nemá právo vidět rozpočet projektu
                End With
                If Me.CurrentSubgrid = SubgridType.p45 Then Me.CurrentSubgrid = SubgridType.p31
            End If
        End With

        If Not (menu1.FindItemByValue("cmdEdit").Visible Or menu1.FindItemByValue("cmdNew").Visible) Then
            Try
                menu1.Items.Remove(menu1.FindItemByValue("record"))
            Catch ex As Exception

            End Try

        End If
        panDraftCommands.Visible = False
        If cRec.b02ID = 0 And cRec.p41IsDraft And cDisp.OwnerAccess Then
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator) Then panDraftCommands.Visible = True 'pokud je vlastník a má právo zakládat ostré projekty a projekt nemá workflow šablonu
        End If


        If cRec.IsClosed Then menu1.FindItemByValue("cmdO22").Visible = False : menu1.FindItemByValue("cmdP40Create").Visible = False : menu1.FindItemByValue("cmdP56").Visible = False 'projekt je v archivu
    End Sub

    Private Sub RefreshOtherBillingSetting(cRec As BO.p41Project, cClient As BO.p28Contact)
        Dim b As Boolean = False
        With cRec
            If .p87ID > 0 Or .p92ID > 0 Or .p28ID_Billing > 0 Then
                b = True
            End If
        End With
      
        Me.clue_p41_billing.Visible = b
        If b Then
            Me.clue_p41_billing.Attributes("rel") = "clue_p41_record_billingsetting.aspx?pid=" & cRec.PID.ToString
        End If
    End Sub

    Private Sub RefreshPricelist(cRec As BO.p41Project, cClient As BO.p28Contact)
        Me.clue_p51id_billing.Visible = False : Me.p51Name_Billing.Visible = False : Me.lblX51_Message.Text = ""
        With cRec
            If .p51ID_Billing > 0 Then
                Me.p51Name_Billing.Visible = True : clue_p51id_billing.Visible = True
                Me.p51Name_Billing.Text = .p51Name_Billing
                If .p51Name_Billing.IndexOf(cRec.p41Name) >= 0 Then
                    'sazby na míru
                    p51Name_Billing.Text = "Tento projekt má sazby na míru"
                End If
                Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
            Else
                If Not cClient Is Nothing Then
                    With cClient
                        If .p51ID_Billing > 0 Then
                            Me.lblX51_Message.Text = "(dědí se z klienta)"
                            Me.p51Name_Billing.Visible = True : clue_p51id_billing.Visible = True
                            Me.p51Name_Billing.Text = .p51Name_Billing
                            Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
                        End If
                    End With
                End If
            End If
        End With
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
            Me.clue_p51id_billing.Visible = False  'uživatel nemá oprávnění vidět sazby
        End If

    End Sub
    Private Sub RefreshBillingLanguage(cRec As BO.p41Project, cClient As BO.p28Contact)
        imgFlag_Project.Visible = False : imgFlag_Client.Visible = False
        With cRec
            If .p87ID > 0 Then
                Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID)
                If cP87.p87Icon <> "" Then
                    imgFlag_Project.Visible = True
                    imgFlag_Project.ImageUrl = "Images/flags/" & cP87.p87Icon
                End If

            End If
            If .p87ID_Client > 0 Then
                If Not cClient Is Nothing Then
                    Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID_Client)
                    If cP87.p87Icon <> "" Then
                        imgFlag_Client.Visible = True
                        imgFlag_Client.ImageUrl = "Images/flags/" & cP87.p87Icon
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "draft2normal"
                With Master.Factory.p41ProjectBL
                    If .ConvertFromDraft(Master.DataPID) Then
                        ReloadPage(Master.DataPID.ToString)
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End With
            Case "p41-create"

                ReloadPage(Me.hidHardRefreshPID.Value)
            Case "p31-save", "p31-delete"
                

            Case "p51-save"
                Master.Notify("Pokud jste změnili sazby v ceníku a potřebujete přepočítat sazby u již uložené rozpracovanosti, použijte k tomu nástroj [Přepočítat sazby rozpracovaných úkonů].", NotifyLevel.InfoMessage)

            Case "favourite"
                Master.Factory.j03UserBL.AppendOrRemoveFavouriteProject(Master.Factory.SysUser.PID, BO.BAS.ConvertPIDs2List(Master.DataPID), Master.Factory.p41ProjectBL.IsMyFavouriteProject(Master.DataPID))
                ''ReloadPage(Master.DataPID.ToString)
                ClientScript.RegisterStartupScript(Me.GetType, "hash", "parent.window.location.replace('p41_framework.aspx');", True)

            Case Else
                ReloadPage(Master.DataPID.ToString)
        End Select
        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub





    Private Sub ReloadPage(strPID As String)
        Response.Redirect("p41_framework_detail.aspx?pid=" & strPID)
    End Sub





    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub rpP40_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP40.ItemDataBound
        Dim cRec As BO.p40WorkSheet_Recurrence = CType(e.Item.DataItem, BO.p40WorkSheet_Recurrence)
        With CType(e.Item.FindControl("p40Name"), HyperLink)
            .Text = cRec.p40Name & " (" & cRec.p34Name & "): " & BO.BAS.FN(cRec.p40Value) & ",-"
            .NavigateUrl = "javascript:p40_record(" & cRec.PID.ToString & ")"
        End With
        With CType(e.Item.FindControl("clue_p40"), HyperLink)
            .Attributes("rel") = "clue_p40_record.aspx?pid=" & cRec.PID.ToString
        End With

    End Sub








End Class