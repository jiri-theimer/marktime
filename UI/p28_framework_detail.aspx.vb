Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

Public Class p28_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Private Property _curProjectIndex As Integer
    Private Property _curDocIndex As Integer

    Public Property CurrentTab As String
        Get
            If tabs1.SelectedTab Is Nothing Then
                tabs1.SelectedIndex = 0
            End If
            Return tabs1.SelectedTab.Value
        End Get
        Set(value As String)
            If value = "" Then Return
            If tabs1.FindTabByValue(value) Is Nothing Then Return
            tabs1.FindTabByValue(value).Selected = True
        End Set
    End Property

    Private Sub p28_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            p31summary1.Factory = .Factory
            ff1.Factory = .Factory
        End With
        

        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            With Master
                .SiteMenuValue = "p28"
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p28_framework_detail-tab", Request.Item("tab"))
                End If
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneContactPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneContactPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p28_framework_detail-pid")
                    .Add("p28_framework_detail-tab")
                    .Add("p28_framework_detail-tabskin")
                    .Add("p28_framework_detail-chkFFShowFilledOnly")
                    .Add("p28_framework_detail-switch")
                    .Add("p28_framework_detail-switchHeight")
                    .Add("p28_framework_detail-chkShowBoxP41")
                    .Add("p28_framework_detail-searchbox")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                End With
                With .Factory.j03UserBL
                    Me.chkShowBoxP41.Checked = BO.BAS.BG(.GetUserParam("p28_framework_detail-chkShowBoxP41", "1"))
                    panSwitch.Style.Item("display") = .GetUserParam("p28_framework_detail-switch", "block")
                    Dim strHeight As String = .GetUserParam("p28_framework_detail-switchHeight", "auto")
                    If strHeight = "auto" Then
                        panSwitch.Style.Item("height") = "" : panSwitch.Style.Item("overflow") = ""
                    Else
                        panSwitch.Style.Item("height") = strHeight & "px"
                    End If

                    If Master.DataPID = 0 Then
                        Master.DataPID = BO.BAS.IsNullInt(.GetUserParam("p28_framework_detail-pid", "O"))
                        If Master.DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p28")
                    Else
                        If Master.DataPID <> BO.BAS.IsNullInt(.GetUserParam("p28_framework_detail-pid", "O")) Then
                            .SetUserParam("p28_framework_detail-pid", Master.DataPID.ToString)
                        End If
                    End If
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p28_framework_detail-chkFFShowFilledOnly", "0"))
                    menu1.FindItemByValue("searchbox").Visible = BO.BAS.BG(.GetUserParam("p28_framework_detail-searchbox", "0"))
                End With

            End With
            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                basUIMT.RenderSawMenuItemAsGrid(menu1.FindItemByValue("saw"), "p28")
            End If

            RefreshRecord()

            tabs1.Skin = Master.Factory.j03UserBL.GetUserParam("p28_framework_detail-tabskin", "Default")   'až zde jsou vygenerované tab záložky
            Me.CurrentTab = Master.Factory.j03UserBL.GetUserParam("p28_framework_detail-tab", "summary")
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
        Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p28")
        Dim cRecSum As BO.p28ContactSum = Master.Factory.p28ContactBL.LoadSumRow(cRec.PID)
        SetupTabs(cRecSum)
        Handle_Permissions(cRec)

        Dim cClient As BO.p28Contact = Nothing

        
        With cRec
            basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), .p28Name, "p28_framework_detail.aspx?pid=" & Master.DataPID.ToString, .IsClosed)
            basUIMT.RenderHeaderMenu(.IsClosed, Me.panMenuContainer, menu1)
            Me.Contact.Text = .p28Name
            If .p28Code <> "" Then
                Me.Contact.Text += " <span style='color:gray;padding-left:10px;'>" & .p28Code & "</span>"
            End If

            If .p28CompanyShortName > "" Then
                Me.Contact.Text += "<div style='color:green;'>" & .p28CompanyName & "</div>"
            End If
            Me.Owner.Text = .Owner
            Me.p28RegID.Text = .p28RegID
            Me.p28VatID.Text = .p28VatID
            If .p29ID > 0 Then
                Me.p29Name.Text = "[" & .p29Name & "]"
            End If
            imgDraft.Visible = .p28IsDraft
            If .p28ParentID <> 0 Then
                Me.trParent.Visible = True
                Me.ParentContact.NavigateUrl = "p28_framework.aspx?pid=" & .p28ParentID.ToString
                Me.ParentContact.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, .p28ParentID)
            End If
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) And .p28BillingMemo <> "" Then
                boxBillingMemo.Visible = True
                Me.p28BillingMemo.Text = BO.BAS.CrLfText2Html(.p28BillingMemo)
            Else
                boxBillingMemo.Visible = False
            End If
        End With

        RefreshBillingLanguage(cRec)

        RefreshPricelist(cRec)

        RefreshProjectList(cRec)

        
        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p28Contact, cRec.PID)
        Me.roles1.RefreshData(lisX69, cRec.PID)
        If Me.roles1.RowsCount = 0 Then panRoles.Visible = False

        Dim lisO37 As IEnumerable(Of BO.o37Contact_Address) = Master.Factory.p28ContactBL.GetList_o37(cRec.PID)
        If lisO37.Count > 0 Then
            Me.boxO37.Visible = True
            Me.address1.FillData(lisO37)
            boxO37Title.Text = BO.BAS.OM2(boxO37Title.Text, lisO37.Count.ToString)
        Else
            Me.boxO37.Visible = False
        End If

        Dim lisO32 As IEnumerable(Of BO.o32Contact_Medium) = Master.Factory.p28ContactBL.GetList_o32(cRec.PID)
        If lisO32.Count > 0 Then
            Me.boxO32.Visible = True
            Me.medium1.FillData(lisO32)
            With Me.boxO32Title
                .Text = BO.BAS.OM2(.Text, lisO32.Count.ToString)
            End With
        Else
            Me.boxO32.Visible = False
        End If
        If cRecSum.p30_Exist Then
            Dim lisP30 As IEnumerable(Of BO.j02Person) = Master.Factory.p30Contact_PersonBL.GetList_J02(Master.DataPID, 0, True)
            If lisP30.Count > 0 Then
                Me.boxP30.Visible = True
                Me.persons1.FillData(lisP30)
                With Me.boxP30Title
                    If lisP30.Count > 0 Then
                        .Text = .Text & " (" & lisP30.Count.ToString & ")"
                        If Master.Factory.SysUser.j04IsMenu_People Then
                            .Text = "<a href='j02_framework.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString & "' target='_top'>" & .Text & "</a>"
                        End If

                    End If

                End With
            Else
                cRecSum.p30_Exist = False
            End If
        End If
        Me.boxP30.Visible = cRecSum.p30_Exist
        

  

        If cRecSum.p31_Approved_Time_Count > 0 Or cRecSum.p31_Wip_Time_Count > 0 Then
            Dim mq As New BO.myQueryP31
            mq.p28ID_Client = cRec.PID
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            If cWorksheetSum.RowsCount = 0 Then
                boxP31Summary.Visible = False
            Else
                p31summary1.RefreshData(cWorksheetSum, "p28", Master.DataPID, Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates), 0, 0)
            End If
        Else
            boxP31Summary.Visible = False
        End If

        If cRec.b02ID > 0 Then
            Me.trWorkflow.Visible = True
            Me.b02Name.Text = cRec.b02Name
        Else
            Me.trWorkflow.Visible = False
        End If
       
        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p28Contact).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_decide('x18_binding.aspx?prefix=p28&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p28Contact, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p28Contact, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p28Contact, Master.DataPID, cRec.p29ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If
        If cRecSum.childs_Count > 0 Then
            linkChilds.Visible = True
            linkChilds.Text += "<span class='badge1'>" & cRecSum.childs_Count.ToString & "</span>"
        Else
            linkChilds.Visible = False
        End If
        
    End Sub

    
    Private Sub Handle_Permissions(cRec As BO.p28Contact)
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString
        Dim cDisp As BO.p28RecordDisposition = Master.Factory.p28ContactBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then
            Master.StopPage("Nedisponujete přístupovým oprávněním ke klientovi.")
        End If
        x18_binding.Visible = cDisp.OwnerAccess

        With Master.Factory
            menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator)
            menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)

            menu1.FindItemByValue("cmdNew").Visible = .TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator)
            menu1.FindItemByValue("cmdCopy").Visible = menu1.FindItemByValue("cmdNew").Visible
            menu1.FindItemByValue("cmdPivot").Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString
            If cRec.p28SupplierFlag = BO.p28SupplierFlagENUM.ClientAndSupplier Or cRec.p28SupplierFlag = BO.p28SupplierFlagENUM.ClientOnly Then
                menu1.FindItemByValue("cmdNewP41").Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)
            Else
                menu1.FindItemByValue("cmdNewP41").Visible = False
            End If


            If .SysUser.IsApprovingPerson = False Or cRec.p28SupplierFlag = BO.p28SupplierFlagENUM.NotClientNotSupplier Then
                'schovat záložku pro schvalování
                menu1.FindItemByValue("cmdApprove").Visible = False
            Else
                menu1.FindItemByValue("cmdApprove").Visible = .TestPermission(BO.x53PermValEnum.PR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator)
            End If
            If Not .SysUser.j04IsMenu_Invoice Then
                RemoveTab("p91")
            End If
        End With

        menu1.FindItemByValue("cmdEdit").Visible = cDisp.OwnerAccess
        menu1.FindItemByValue("cmdCopy").Visible = cDisp.OwnerAccess

        cmdEditP30.Visible = cDisp.OwnerAccess
        menu1.FindItemByValue("cmdP30").Visible = cDisp.OwnerAccess
        menu1.FindItemByValue("cmdLog").Visible = cDisp.OwnerAccess

        If cRec.b02ID = 0 And cRec.p28IsDraft And cDisp.OwnerAccess Then
            panDraftCommands.Visible = True 'pokud je vlastník a projekt nemá workflow šablonu
        Else
            panDraftCommands.Visible = False
        End If

        If cRec.IsClosed Then
            menu1.FindItemByValue("cmdO22").Visible = False : menu1.FindItemByValue("cmdNewP41").Visible = False : menu1.FindItemByValue("cmdP30").Visible = False 'klient je v archivu
            menu1.Skin = "Black"
        End If
        If cRec.p28SupplierFlag = BO.p28SupplierFlagENUM.NotClientNotSupplier Then
            Me.CurrentTab = ""
        End If

    End Sub

    Private Sub RefreshProjectList(cRec As BO.p28Contact)

        Dim mq As New BO.myQueryP41
        mq.p28ID = cRec.PID
        mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
        mq.Closed = BO.BooleanQueryMode.NoQuery


        Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq).OrderBy(Function(p) p.IsClosed).ThenByDescending(Function(p) p.PID)
        If lis.Count = 0 Then
            boxP41.Visible = False : Return
        Else
            boxP41.Visible = True
            Dim intClosed As Integer = lis.Where(Function(p) p.IsClosed = True).Count
            Dim intOpened As Integer = lis.Count - intClosed
            Dim s As String = ""
            With boxP41Title
                If intClosed > 0 Then
                    ''.Text = .Text & " (" & intOpened.ToString & IIf(intClosed > 0, "+" & intClosed.ToString, "") & ")"
                    s = "<span class='badge1'>" & intOpened.ToString & IIf(intClosed > 0, "+" & intClosed.ToString, "") & "</span>"
                Else
                    ''.Text = .Text & " (" & intOpened.ToString & ")"
                    s = "<span class='badge1'>" & intOpened.ToString & "</span>"
                End If

                .Text = "<a href='javascript:projects()'>" & .Text & s & "</a>"
            End With

        End If
        If Not Me.chkShowBoxP41.Checked Then Return

        If lis.Count > 10 Then lis = lis.Take(11) 'omezit na maximálně 10+1
        rpP41.DataSource = lis.OrderBy(Function(p) p.IsClosed).ThenBy(Function(p) p.p41Name)
        rpP41.DataBind()

    End Sub



    

    Private Sub RefreshPricelist(cRec As BO.p28Contact)
        Me.clue_p51id_billing.Visible = False : Me.p51Name_Billing.Visible = False : lblX51.Visible = False
        With cRec
            If .p51ID_Billing > 0 Then
                Me.p51Name_Billing.Visible = True : clue_p51id_billing.Visible = True : lblX51.Visible = True
                Me.p51Name_Billing.Text = .p51Name_Billing
                Try
                    If .p51Name_Billing.IndexOf(cRec.p28Name) >= 0 Then
                        Me.p51Name_Billing.Text = "Sazby na míru"
                    End If
                    Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
                Catch ex As Exception

                End Try
                

            End If
        End With
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
            Me.clue_p51id_billing.Visible = False  'uživatel nemá oprávnění vidět sazby
        End If

    End Sub
    Private Sub RefreshBillingLanguage(cRec As BO.p28Contact)
        imgFlag_Contact.Visible = False
        With cRec
            If .p87ID > 0 Then
                Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID)
                If cP87.p87Icon <> "" Then
                    imgFlag_Contact.Visible = True
                    imgFlag_Contact.ImageUrl = "Images/flags/" & cP87.p87Icon
                End If

            End If
        End With
    End Sub



    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "draft2normal"
                With Master.Factory.p28ContactBL
                    If .ConvertFromDraft(Master.DataPID) Then
                        ReloadPage(Master.DataPID.ToString)
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End With
            Case "p31-save"
                'je třeba kvůli obnově pod-přehledu
            Case Else
                ReloadPage(Master.DataPID.ToString)
        End Select
        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub

    

    Private Sub ReloadPage(strPID As String)
        Response.Redirect("p28_framework_detail.aspx?pid=" & strPID)
    End Sub


    Private Sub p28_framework_detail_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not tabs1.Visible Then
            panProjects.Style.Item("max-height") = ""
            panProjects.Style.Item("overflow") = ""
        Else
            panProjects.Style.Item("max-height") = "250px"
            panProjects.Style.Item("overflow") = "auto"
        End If
        
    End Sub

    Private Sub rpP41_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP41.ItemDataBound
        _curProjectIndex += 1
        Dim cRec As BO.p41Project = CType(e.Item.DataItem, BO.p41Project)
        With CType(e.Item.FindControl("aProject"), HyperLink)
            If cRec.p41NameShort > "" Then
                .Text = cRec.p41NameShort
            Else
                .Text = cRec.p41Name
            End If
            .Text += " (" & cRec.p41Code & ")"
            If cRec.IsClosed Then .Font.Strikeout = True : .ForeColor = Drawing.Color.Gray
            If Master.Factory.SysUser.j04IsMenu_Project Then
                .NavigateUrl = "p41_framework.aspx?pid=" & cRec.PID.ToString


                If _curProjectIndex > 10 Then
                    'poslední nad 10
                    .Text = "Všechny projekty klienta>>"
                    .NavigateUrl = "javascript:projects()"
                    .Target = ""
                    .ForeColor = Drawing.Color.Green
                    .Font.Bold = True
                    e.Item.FindControl("clue_project").Visible = False
                End If
            End If
        End With
        If e.Item.FindControl("clue_project").Visible Then CType(e.Item.FindControl("clue_project"), HyperLink).Attributes.Item("rel") = "clue_p41_record.aspx?pid=" & cRec.PID.ToString


    End Sub

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

   
    Private Sub chkShowBoxP41_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowBoxP41.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework_detail-chkShowBoxP41", BO.BAS.GB(Me.chkShowBoxP41.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub SetupTabs(crs As BO.p28ContactSum)
        tabs1.Tabs.Clear()

        Dim lisX61 As IEnumerable(Of BO.x61PageTab) = Master.Factory.j03UserBL.GetList_PageTabs(Master.Factory.SysUser.PID, BO.x29IdEnum.p28Contact)
        For Each c In lisX61
            Dim tab As New RadTab(c.x61Name, c.x61Code)
            tabs1.Tabs.Add(tab)
            tab.NavigateUrl = c.GetPageUrl("p28", Master.DataPID, BO.BAS.GB(Master.Factory.SysUser.IsApprovingPerson))
            tab.NavigateUrl += "&lasttabkey=p28_framework_detail-tab&lasttabval=" & c.x61Code
            tab.Target = "fraSubform"
            If tabs1.Tabs.Count = 0 Then tab.Selected = True
            Select Case c.x61Code
                Case "time"
                    If crs.p31_Wip_Time_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
                    If crs.p31_Approved_Time_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
                Case "expense"
                    If crs.p31_Wip_Expense_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
                    If crs.p31_Approved_Expense_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
                Case "fee"
                    If crs.p31_Wip_Fee_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
                    If crs.p31_Approved_Fee_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
                Case "kusovnik"
                    If crs.p31_Wip_Kusovnik_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Kusovnik_Count.ToString & "</span>"
                    If crs.p31_Approved_Kusovnik_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Kusovnik_Count.ToString & "</span>"
                Case "p91"
                    If crs.p91_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.p91_Count.ToString & "</span>"
                Case "p56"
                    If crs.p56_Actual_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.p56_Actual_Count.ToString & "</span>"
                Case "o23"
                    If crs.o23_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.o23_Count.ToString & "</span>"
                Case "workflow"
                    If crs.b07_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.b07_Count.ToString & "</span>"
            End Select
        Next
    End Sub



    Private Sub RemoveTab(strTabValue As String)
        With tabs1.Tabs
            If Not .FindTabByValue(strTabValue) Is Nothing Then
                Master.Notify(String.Format("Pro záložku [{0}] nemáte oprávnění.", .FindTabByValue(strTabValue).Text), NotifyLevel.InfoMessage)
                .Remove(.FindTabByValue(strTabValue))
                If .Count > 0 Then tabs1.SelectedIndex = 0

            End If
        End With
    End Sub
End Class