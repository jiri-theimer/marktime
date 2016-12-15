Imports Telerik.Web.UI
Imports System.Web.Script.Serialization
Public Class p41_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    
    Public Property CurrentTab As String
        Get
            If tabs1.Tabs.Count = 0 Then Return ""
            If tabs1.SelectedTab Is Nothing Then
                tabs1.SelectedIndex = 0
            End If
            Return tabs1.SelectedTab.Value
        End Get
        Set(value As String)
            If tabs1.FindTabByValue(value) Is Nothing Then Return
            tabs1.FindTabByValue(value).Selected = True
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
                .SiteMenuValue = "p41"
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p41_framework_detail-tab", Request.Item("tab"))
                End If

                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneProjectPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneProjectPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p41_framework_detail-pid")
                    .Add("p41_framework_detail-tab")
                    .Add("p41_framework_detail-tabskin")
                    .Add("p41_framework_detail-chkFFShowFilledOnly")
                    .Add("p41_framework_detail-switch")
                    .Add("p41_framework_detail_pos")
                    ''.Add("p41_framework_detail-switchHeight")
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
                    ''Dim strHeight As String = .GetUserParam("p41_framework_detail-switchHeight", "auto")
                    ''If strHeight = "auto" Then
                    ''    panSwitch.Style.Item("height") = "" : panSwitch.Style.Item("overflow") = ""
                    ''Else
                    ''    panSwitch.Style.Item("height") = strHeight & "px"
                    ''End If
                    If Request.Item("force") = "comment" Then
                        Me.CurrentTab = "workflow"
                    End If
                    
                End With


            End With

            RefreshRecord()

            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                basUIMT.RenderSawMenuItemAsGrid(menu1.FindItemByValue("saw"), "p41")
            End If
            tabs1.Skin = Master.Factory.j03UserBL.GetUserParam("p41_framework_detail-tabskin", "Default")   'až zde jsou vygenerované tab záložky
            Me.CurrentTab = Master.Factory.j03UserBL.GetUserParam("p41_framework_detail-tab")
        End If

        If Me.CurrentTab <> "" Then
            fraSubform.Visible = True
            fraSubform.Attributes.Item("src") = Me.tabs1.SelectedTab.NavigateUrl.Replace("lasttabkey", "nic")
            Select Case Me.CurrentTab
                Case "p31", "time", "expense", "fee", "kusovnik"
                    If Me.hidHardRefreshFlag.Value = "p31-save" Then
                        fraSubform.Attributes.Item("src") += "&pid=" & Me.hidHardRefreshPID.Value
                    End If
            End Select
        Else
            fraSubform.Visible = False : imgLoading.Visible = False
            panSwitch.Style.Item("height") = ""
        End If
       

    End Sub


    Private Sub RefreshRecord()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")

        Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)
        SetupTabs(cRecSum, cP42)
        Handle_Permissions(cRec, cP42)

        Dim cClient As BO.p28Contact = Nothing

        With cRec
            ViewState("p28id_client") = .p28ID_Client.ToString

            Me.Project.Text = .p41Name & " <span style='color:gray;padding-left:10px;'>" & .p41Code & "</span>"
            If .p41ParentID > 0 Then Me.Project.ForeColor = basUIMT.ChildProjectColor
            If .p41NameShort <> "" Then
                Me.Project.Text += "<div style='color:green;'>" & .p41NameShort & "</div>"
            End If

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
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) And .p41BillingMemo <> "" Then
                boxBillingMemo.Visible = True
                Me.p41BillingMemo.Text = BO.BAS.CrLfText2Html(.p41BillingMemo)
                If Not cClient Is Nothing Then
                    If cClient.p28BillingMemo <> "" Then
                        Me.p41BillingMemo.Text += "<hr>" & String.Format("Fakturační poznámka klienta: {0}", BO.BAS.CrLfText2Html(cClient.p28BillingMemo))
                    End If
                End If
            Else
                boxBillingMemo.Visible = False
            End If

        End With



        If cP42.p42IsModule_p31 Then
            RefreshBillingLanguage(cRec, cClient)
            RefreshPricelist(cRec, cClient)
            ''RefreshOtherBillingSetting(cRec, cClient)
        Else
            trP51.Visible = False
        End If




        If cP42.p42IsModule_p31 Then
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
                    menu1.Skin = "Black"
                    mi.Text = Resources.p41_framework_detail.VArchivuNelzeWorksheet
                End If
                If cRec.p41IsDraft Then mi.Text = Resources.p41_framework_detail.VDraftNelzeWorksheet
                mi.ForeColor = Drawing.Color.Red
                menu1.FindItemByValue("p31").Items.Add(mi)
            End If
        Else
            hmi("p31", False)
        End If
        


        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)


        If cRecSum.o23_Count > 0 And cP42.p42SubgridO23Flag = 0 Then
            Dim mqO23 As New BO.myQueryO23
            mqO23.p41ID = Master.DataPID
            mqO23.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
            If lisO23.Count > 0 Then
                Me.boxO23.Visible = True
                With Me.boxO23Title
                    .Text = BO.BAS.OM2(.Text, lisO23.Count.ToString)
                    If menu1.FindItemByValue("cmdO23").Visible Then
                        .Text = "<a href='javascript:notepads()'>" & .Text & "</a>"
                        If lisO23.Count > 10 Then
                            .Text += ", 10 nejnovějších:"
                            lisO23 = lisO23.Take(10)
                        End If
                    End If
                End With
                notepad1.RefreshData(lisO23, Master.DataPID)
            Else
                boxO23.Visible = False
            End If
        Else
            Me.boxO23.Visible = False
        End If

        If cRecSum.p30_Exist Then
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
                cRecSum.p30_Exist = False
            End If
        End If
        Me.boxP30.Visible = cRecSum.p30_Exist

        Dim mq As New BO.myQueryP31
        mq.p41ID = cRec.PID

        If cRec.p41LimitFee_Notification > 0 Or cRec.p41LimitHours_Notification > 0 Then
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            If cWorksheetSum.RowsCount = 0 Then
                boxP31Summary.Visible = False
            Else
                p31summary1.RefreshData(cWorksheetSum, "p41", Master.DataPID, Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates), cRec.p41LimitHours_Notification, cRec.p41LimitFee_Notification)
            End If
        Else
            boxP31Summary.Visible = False
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
        If cRecSum.childs_Count > 0 Then
            cmdChilds.Visible = True
            cmdChilds.Text += "<span class='badge1'>" & cRecSum.childs_Count.ToString & "</span>"
        End If
        If cRecSum.is_My_Favourite Then
            cmdFavourite.ImageUrl = "Images/favourite.png"
            cmdFavourite.ToolTip = "Vyřadit z mých oblíbených projektů"
        Else
            cmdFavourite.ImageUrl = "Images/not_favourite.png"
            cmdFavourite.ToolTip = "Zařadit do mých oblíbených projektů"
        End If

        RefreshP40(cRecSum)

    End Sub

    Private Sub RefreshP40(cRecSum As BO.p41ProjectSum)
        If cRecSum.p40_Exist Then
            Dim lisP40 As IEnumerable(Of BO.p40WorkSheet_Recurrence) = Master.Factory.p40WorkSheet_RecurrenceBL.GetList(Master.DataPID)
            rpP40.DataSource = lisP40
            rpP40.DataBind()
        Else
            cRecSum.p40_Exist = False
        End If
        boxP40.Visible = cRecSum.p40_Exist
    End Sub


    Private Sub hmi(strMenuValue As String, bolVisible As Boolean)
        Dim mi As RadMenuItem = menu1.FindItemByValue(strMenuValue)
        If mi Is Nothing Then Return
        mi.Visible = bolVisible

        ''If Not bolVisible Then mi.Remove()
        'If Not bolVisible Then menu1.Items.Remove(mi)
    End Sub

    Private Sub Handle_Permissions(cRec As BO.p41Project, cP42 As BO.p42ProjectType)
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString

        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        menu1.FindItemByValue("cmdLog").Visible = cDisp.OwnerAccess
        x18_binding.Visible = cDisp.OwnerAccess
        With Master.Factory
            menu1.FindItemByValue("cmdNew").Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)
            menu1.FindItemByValue("cmdCopy").Visible = menu1.FindItemByValue("cmdNew").Visible
            menu1.FindItemByValue("cmdNewChild").Visible = menu1.FindItemByValue("cmdNew").Visible
            If cP42.p42IsModule_p31 Then
                menu1.FindItemByValue("cmdPivot").Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
                menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString
            Else
                hmi("cmdPivot", False)
            End If

            If cP42.p42IsModule_o23 Then menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator) Else hmi("cmdO23", False)
            If cP42.p42IsModule_o22 Then menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator) Else hmi("cmdO22", False)
            menu1.FindItemByValue("calendarO22").Visible = menu1.FindItemByValue("cmdO22").Visible

            If cRec.b01ID <> 0 Then menu1.FindItemByValue("cmdB07").Visible = False

            If cP42.p42IsModule_p31 Then
                Dim bolCanApproveOrInvoice As Boolean = .TestPermission(BO.x53PermValEnum.GR_P31_Approver, BO.x53PermValEnum.GR_P91_Creator)
                If Not bolCanApproveOrInvoice Then bolCanApproveOrInvoice = .TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator)
                If bolCanApproveOrInvoice = False And cDisp.x67IDs.Count > 0 Then
                    Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Master.Factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                    If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                        bolCanApproveOrInvoice = True
                    End If
                End If
                menu1.FindItemByValue("cmdApprove").Visible = bolCanApproveOrInvoice
                If Not bolCanApproveOrInvoice Then Me.p31summary1.DisableApprovingButton()
                Me.hidIsCanApprove.Value = BO.BAS.GB(bolCanApproveOrInvoice)
            Else
                hmi("cmdApprove", False)
                p31summary1.Visible = False
            End If
            hmi("cmdP48", cP42.p42IsModule_p48)
            aP48.Visible = cP42.p42IsModule_p48
        End With
        With cDisp
            If cP42.p42IsModule_p56 Then menu1.FindItemByValue("cmdP56").Visible = .P56_Create Else hmi("cmdP56", False)

            boxP30.Visible = .OwnerAccess
            If cP42.p42IsModule_p31 Then
                menu1.FindItemByValue("cmdP31Recalc").Visible = .P31_RecalcRates
                menu1.FindItemByValue("cmdP31Move2Bin").Visible = .P31_Move2Bin
                menu1.FindItemByValue("cmdP31MoveToOtherProject").Visible = .P31_MoveToOtherProject
                menu1.FindItemByValue("cmdP40Create").Visible = .OwnerAccess
            Else
                hmi("cmdP31Recalc", False) : hmi("cmdP31Move2Bin", False) : hmi("cmdP31MoveToOtherProject", False) : hmi("cmdP40Create", False)
            End If
            

            menu1.FindItemByValue("cmdEdit").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdP30").Visible = .OwnerAccess

            If Not .p91_Read Then RemoveTab("p91")

            If Not .p45_Read Then RemoveTab("p45")

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
        If cRec.IsClosed Then hmi("cmdO22", False) : hmi("cmdP40Create", False) : hmi("cmdP56", False) 'projekt je v archivu

    End Sub

    ''Private Sub RefreshOtherBillingSetting(cRec As BO.p41Project, cClient As BO.p28Contact)
    ''    Dim b As Boolean = False
    ''    With cRec
    ''        If .p87ID > 0 Or .p92ID > 0 Or .p28ID_Billing > 0 Then
    ''            b = True
    ''        End If
    ''    End With

    ''    Me.clue_p41_billing.Visible = b
    ''    If b Then
    ''        Me.clue_p41_billing.Attributes("rel") = "clue_p41_record_billingsetting.aspx?pid=" & cRec.PID.ToString
    ''    End If
    ''End Sub

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
                If Master.IsTopWindow Then
                    ReloadPage(Master.DataPID.ToString)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType, "hash", "parent.window.location.replace('p41_framework.aspx');", True)
                End If

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

    Private Sub cti(strName As String, strX61Code As String)
        Dim cX61 As New BO.x61PageTab
        cX61.x61Code = strX61Code
        Dim tab As New RadTab(strName, strX61Code)
        tabs1.Tabs.Add(tab)
        If strX61Code = "rec" Then            
            Return
        End If
        tab.NavigateUrl = cX61.GetPageUrl("p41", Master.DataPID, Me.hidIsCanApprove.Value)
        tab.NavigateUrl += "&lasttabkey=p41_framework_detail-tab&lasttabval=" & strX61Code
        tab.Target = "fraSubform"
        If tabs1.Tabs.Count = 0 Then tab.Selected = True
    End Sub
    Private Sub SetupTabs(crs As BO.p41ProjectSum, cP42 As BO.p42ProjectType)
        tabs1.Tabs.Clear()
        Dim s As String = ""
        If Me.hidPOS.Value = "1" Then
            panSwitch.Style.Item("position") = "absolute"
            panSwitch.Style.Item("top") = "100px"
            cti("Projekt", "rec")
        End If
        If cP42.p42IsModule_p31 Then
            s = "Summary" : cti(s, "summary")
            s = "Worksheet" : cti(s, "p31")
            s = "Hodiny"
            If crs.p31_Wip_Time_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
            If crs.p31_Approved_Time_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
            cti(s, "time")
            s = "Výdaje"
            If crs.p31_Wip_Expense_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
            If crs.p31_Approved_Expense_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
            cti(s, "expense")
            s = "Odměny"
            If crs.p31_Wip_Fee_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
            If crs.p31_Approved_Fee_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
            cti(s, "fee")
            s = "Faktury"
            If crs.p91_Count > 0 Then s += "<span class='badge1'>" & crs.p91_Count.ToString & "</span>"
            cti(s, "p91")
        End If
        If crs.childs_Count > 0 Then
            s = "Pod-projekty<span class='badge1'>" & crs.childs_Count.ToString & "</span>"
            cti(s, "p41")
        End If
        If cP42.p42IsModule_p56 Then
            s = "Úkoly"
            If crs.p56_Actual_Count > 0 Then s += "<span class='badge1'>" & crs.p56_Actual_Count.ToString & "</span>"
            cti(s, "p56")
        End If
        If cP42.p42IsModule_p45 Then
            s = "Rozpočet"
            If crs.p45_Count > 0 Then s += "<span class='badge1'>" & crs.p45_Count.ToString & "</span>"
            cti(s, "p45")
        End If
        If cP42.p42IsModule_o23 And cP42.p42SubgridO23Flag = 1 Then
            s = "Dokumenty"
            If crs.o23_Count > 0 Then s += "<span class='badge1'>" & crs.o23_Count.ToString & "</span>"
            cti(s, "o23")
        End If
        If crs.b07_Count > 0 Then
            s = "Komentáře a workflow<span class='badge1'>" & crs.b07_Count.ToString & "</span>"
            cti(s, "workflow")
        End If
        
    End Sub

   



    Private Sub RemoveTab(strTabValue As String)
        With tabs1.Tabs
            If Not .FindTabByValue(strTabValue) Is Nothing Then
                ''Master.Notify(String.Format("Pro záložku [{0}] nemáte oprávnění.", .FindTabByValue(strTabValue).Text), NotifyLevel.InfoMessage)
                .Remove(.FindTabByValue(strTabValue))
                If .Count > 0 Then tabs1.SelectedIndex = 0

            End If
        End With
    End Sub


    
End Class