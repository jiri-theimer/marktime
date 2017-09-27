Imports Telerik.Web.UI
Public Class entity_menu
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory
    Public Property DataPrefix As String
        Get
            Return hidDataPrefix.Value
        End Get
        Set(value As String)
            hidDataPrefix.Value = value
        End Set
    End Property
    Public Property DataPID As Integer

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
    Public Property LockedTab As String
        Get
            Return hidLockedTab.Value
        End Get
        Set(value As String)
            hidLockedTab.Value = value
        End Set
    End Property
   
    Public ReadOnly Property IsExactApprovingPerson As Boolean
        Get
            Return BO.BAS.BG(Me.hidIsCanApprove.Value)
        End Get
    End Property
   
    Public Property TabSkin As String
        Get
            Return tabs1.Skin
        End Get
        Set(value As String)
            If value = "" Then value = "Default"
            tabs1.Skin = value
        End Set
    End Property
    Public Property MenuSkin As String
        Get
            Return menu1.Skin
        End Get
        Set(value As String)
            If value = "" Then value = "Metro"
            menu1.Skin = value
        End Set
    End Property
    ''Public Property ShowLevel1 As Boolean
    ''    Get
    ''        Return FNO("level1").Visible
    ''    End Get
    ''    Set(value As Boolean)
    ''        FNO("level1").Visible = value
    ''    End Set
    ''End Property
    Public Property x31ID_Plugin As String
        Get
            Return Me.hidPlugin.Value
        End Get
        Set(value As String)
            Me.hidPlugin.Value = value
        End Set
    End Property
    Public ReadOnly Property PageSource As String
        Get
            Return Me.hidSource.Value
        End Get
    End Property

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            Me.hidSource.Value = Request.Item("source")
            ''Me.hidParentWidth.Value = Request.Item("parentWidth")
            
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Me.Factory.SysUser.OneProjectPage <> "" Then
                Server.Transfer(basUI.AddQuerystring2Page(Me.Factory.SysUser.OneProjectPage, "pid=" & Me.DataPID.ToString))
            End If
            If Request.Item("savetab") = "1" Then
                Me.Factory.j03UserBL.SetUserParam(Me.DataPrefix & "_framework_detail-tab", Request.Item("tab"))
            End If

            ''FNO("saw").NavigateUrl = Me.DataPrefix & "_framework_detail.aspx?saw=1"
        End If
        ''Dim cbx As RadComboBox = CType(FNO("search").FindControl("cbxSearch"), RadComboBox)
        Dim cbx As New RadComboBox()
        With cbx
            .DropDownWidth = Unit.Parse("400px")
            .RenderMode = RenderMode.Auto
            .EnableTextSelection = True
            .MarkFirstMatch = True
            .EnableLoadOnDemand = True
            .Width = Unit.Parse("200px")
            .Style.Item("margin-top") = "5px"
            .OnClientItemsRequesting = "cbxSearch_OnClientItemsRequesting"
            .OnClientSelectedIndexChanged = "cbxSearch_OnClientSelectedIndexChanged"
            .WebServiceSettings.Method = "LoadComboData"
            .Text = "Hledat..."
            ''.OnClientFocus = "cbxSearch_OnClientFocus"
        End With

        Dim s As String = "", strTop As String = "5px"
        If hidSource.Value = "3" Then strTop = "44px"
        Select Case Me.DataPrefix
            Case "p28"
                s = "<img src='Images/contact_32.png' style='position:absolute;left:5px;top:" & strTop & ";'/>"
                
                cbx.WebServiceSettings.Path = "~/Services/contact_service.asmx"
                cbx.ToolTip = "Hledat klienta"
            Case "j02"
                s = "<img src='Images/person_32.png' style='position:absolute;left:5px;top:" & strTop & ";'/>"
                
                cbx.WebServiceSettings.Path = "~/Services/person_service.asmx"
                cbx.ToolTip = "Hledat osobu"
            Case "p56"
                s = "<img src='Images/task_32.png' style='position:absolute;left:5px;top:" & strTop & ";'/>"
                cbx.WebServiceSettings.Path = "~/Services/task_service.asmx"
                cbx.ToolTip = "Hledat úkol"
               

            Case "o23"
                s = "<img src='Images/notepad_32.png' style='position:absolute;left:5px;top:" & strTop & ";'/>"
                cbx.WebServiceSettings.Path = "~/Services/doc_service.asmx"
                cbx.ToolTip = "Hledat dokument"
               
            Case "p41"
                s = "<img src='Images/project_32.png' style='position:absolute;left:5px;top:" & strTop & ";'/>"
                cbx.WebServiceSettings.Path = "~/Services/project_service.asmx"
                cbx.ToolTip = "Hledat projekt"

        End Select
        If hidSource.Value = "2" Or hidSource.Value = "1" Then
            'sb1.Visible = False
            'sb1.ashx = ""
            FNO("searchbox").Visible = False
        Else
            FNO("searchbox").Controls.Add(cbx)
        End If
        If hidSource.Value = "2" Then
            'panel nahoře a dole
        Else
            place0.Controls.Add(New LiteralControl(s))
        End If


        ''FNO("begin").Controls.Add(New LiteralControl(s))


        ''If sb1.ashx = "" Then
        ''    If Not FNO("searchbox") Is Nothing Then menu1.Items.Remove(FNO("searchbox")) 'searchbox není
        ''Else
        ''    Dim strToolTip As String = sb1.TextboxLabel
        ''    If Request.Browser.Browser = "IE" Or Request.Browser.Browser = "InternetExplorer" Then sb1.TextboxLabel = "" 'pro IE 7 - 11 nefunguje výchozí search text
        ''    With FNO("searchbox")
        ''        s = "<input id='search2' style='width: 100px; margin-top: 7px;' value='" & sb1.TextboxLabel & "' onfocus='search2Focus()' onblur='search2Blur()' title='" & strToolTip & "' />"
        ''        s += "<div id='search2_result' style='position: relative;left:-150px;'></div>"
        ''        .Controls.Add(New LiteralControl(s))
        ''    End With
        ''End If
        Handle_PluginBellowMenu()
    End Sub
    Private Sub Handle_PluginBellowMenu()
        If Me.x31ID_Plugin = "" Then Return
        If Me.hidPlugin_FileName.Value = "" Then
            Dim cX31 As BO.x31Report = Factory.x31ReportBL.Load(CInt(Me.x31ID_Plugin))
            If Not cX31 Is Nothing Then
                Me.hidPlugin_FileName.Value = cX31.ReportFileName
                Me.hidPlugin_Height.Value = cX31.x31PluginHeight.ToString
                If Me.hidPlugin_Height.Value = "0" Then Me.hidPlugin_Height.Value = "30"
            End If
        End If
        If Me.hidPlugin_FileName.Value = "" Then Return

        place1.Controls.Add(New LiteralControl("<iframe id='fraPlugin' width='100%' height='" & Me.hidPlugin_Height.Value & "px' frameborder='0' src='Plugins/" & Me.hidPlugin_FileName.Value & "?pid=" & Me.DataPID.ToString & "'></iframe>"))
    End Sub
    Public Sub p41_RefreshRecord(cRec As BO.p41Project, cRecSum As BO.p41ProjectSum, strTabValue As String, Optional cDisp As BO.p41RecordDisposition = Nothing)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID
        If cDisp Is Nothing Then cDisp = Me.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        Dim cP42 As BO.p42ProjectType = Me.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        p41_SetupTabs(cRecSum, cP42, cDisp)
        p41_SetupMenu(cRec, cP42, cDisp)
        SetupMenu_thePage("p41_framework_detail.aspx?pid=" & cRec.PID.ToString & "&source=" & Me.hidSource.Value, strTabValue)
        Me.CurrentTab = strTabValue


        Handle_SelectedTab()

    End Sub

    Private Sub HighLight_LockedTab(strTab As String)
        If strTab = "" Then Return
        If Not tabs1.FindTabByValue(strTab) Is Nothing Then
            tabs1.FindTabByValue(strTab).ImageUrl = "Images/lock.png"
        End If
    End Sub



    Private Sub p41_SetupMenu(cRec As BO.p41Project, cP42 As BO.p42ProjectType, cDisp As BO.p41RecordDisposition)
        If cRec.IsClosed Then menu1.Skin = "Black"
        If Not cDisp.ReadAccess Then
            Handle_NoAccess("Nedisponujete přístupovým oprávněním k projektu.")
        End If

        Dim mi As NavigationNode = FNO("record")
        If mi.Nodes.Count > 0 Then Return 'menu už bylo dříve zpracované

        mi.Text = "ZÁZNAM PROJEKTU"
        If cDisp.OwnerAccess Then
            ami("Upravit kartu projektu", "cmdEdit", "javascript:record_edit();", "Images/edit.png", mi, "Zahrnuje i možnost přesunutí do archviu nebo nenávratného odstranění.")
        End If
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator) Then
            ami("Založit projekt", "cmdNew", "javascript:record_new();", "Images/new.png", mi, "Z aktuálního projektu se předvyplní klient, typ, středisko,projektové role, fakturační ceník, jazyk a typ faktury.", True)
            ami("Založit projekt kopírováním", "cmdCopy", "javascript:record_clone();", "Images/copy.png", mi, "Nový projekt se kompletně předvyplní podle vzoru tohoto záznamu.")
            ami("Založit pod-projekt", "cmdNewChild", "javascript:record_new_child();", "Images/tree.png", mi, "Nový projekt bude pod-projektem aktuálního projektu.")
        End If
        If cP42.p42IsModule_p31 Then
            Dim bolCanApproveOrInvoice As Boolean = Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Approver, BO.x53PermValEnum.GR_P91_Creator)
            If Not bolCanApproveOrInvoice Then bolCanApproveOrInvoice = Me.Factory.TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator)
            If bolCanApproveOrInvoice = False And cDisp.x67IDs.Count > 0 Then
                Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Me.Factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                    bolCanApproveOrInvoice = True
                End If
            End If
            If bolCanApproveOrInvoice Then
                ami("Schvalovat nebo vystavit fakturu", "cmdApprove", "javascript:approve();", "Images/approve.png", mi, , True)
                If Factory.TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator, BO.x53PermValEnum.GR_P31_Approver) Then
                    ami("Vystavit fakturu zrychleně bez schvalování", "cmdDraft", "javascript:menu_p41_invoice_draft();", "Images/invoice.png", mi)
                End If
            End If
            Me.hidIsCanApprove.Value = BO.BAS.GB(bolCanApproveOrInvoice)
        End If
        If cRec.b01ID > 0 Then
            ami("Posunout/doplnit", "cmdWorkflow", "javascript:workflow();", "Images/workflow.png", mi, , True)
        End If
        ami("Tisková sestava/pdf/e-mail", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)
        ami("Odeslat e-mail", "cmdMail", "javascript:menu_sendmail();", "Images/email.png", mi, , True)

        If cP42.p42IsModule_p31 Then
            mi = ami("ZAPSAT WORKSHEET", "p31", "", "", Nothing)
            If Not (cRec.IsClosed Or cRec.p41IsDraft) Then
                Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = Me.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.PID, cRec.p42ID, cRec.j18ID, Me.Factory.SysUser.j02ID)
                For Each c In lisP34
                    ami(String.Format(Resources.p41_framework_detail.ZapsatUkonDo, c.p34Name), "", "javascript:p31_entry_menu(" & c.PID.ToString & ")", "Images/worksheet.png", mi, "")
                Next
                If lisP34.Count = 0 Then
                    mi = ami(Resources.p41_framework_detail.NedisponujeteOpravnenimZapisovat, "", "", "", mi)
                    mi.ForeColor = Drawing.Color.Red
                End If
            Else
                mi = ami("V projektu nemůžete zapisovat worksheet úkony.", "", "", "", mi)
                mi.ForeColor = Drawing.Color.Red
                If cRec.IsClosed Then
                    mi.Text = Resources.p41_framework_detail.VArchivuNelzeWorksheet
                End If
                If cRec.p41IsDraft Then mi.Text = Resources.p41_framework_detail.VDraftNelzeWorksheet
                mi.ForeColor = Drawing.Color.Red
            End If
        End If

        mi = ami("DALŠÍ", "more", "", "", Nothing)


        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            ami("WORKSHEET statistika projektu", "cmdPivot", "p31_sumgrid.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
        End If
        ami("Plugin", "cmdPlugin", "javascript:plugin();", "Images/plugin.png", mi, , True)
        ami("Štítky", "cmdTags", "javascript:tags_menu();", "Images/tag.png", mi, , True)
        If cDisp.OwnerAccess Then
            ami("Kontaktní osoby projektu", "cmdP30", "javascript:p30_record(0);", "Images/person.png", mi, , True)

            'ami("Vytvořit šanon", "cmdP64", "javascript:p64_record(0);", "Images/binder.png", mi)
        End If
        If Not cRec.IsClosed Then
            If cP42.p42IsModule_p56 Then ami("Vytvořit úkol", "cmdP56", "javascript:menu_p56_record(0);", "Images/task.png", mi, , True)

        End If
        If cP42.p42IsModule_o22 Then
            ami("Vytvořit událost v kalendáři", "cmdO22", "javascript:menu_o22_record(0);", "Images/calendar.png", mi, , True)
            ami("Kalendář projektu", "cmdScheduler", "javascript:scheduler()", "Images/calendar.png", mi)
        End If

        If cP42.p42IsModule_o23 Then
            ami("Vytvořit dokument", "cmdO23", "javascript:menu_o23_record(0);", "Images/notepad.png", mi, , True)
        End If
        
        If cDisp.OwnerAccess Then
            ami("Nastavit jako opakovaný projekt", "cmdRecurrence", "javascript:menu_p41_recur();", "Images/recurrence.png", mi, , True)
        End If
        If cP42.p42IsModule_p31 Then
            If cDisp.OwnerAccess Then
                ami("Definovat opakovanou odměnu/paušál/úkon", "cmdP40Create", "javascript:menu_p40_record(0);", "Images/worksheet_recurrence.png", mi, , True)
            End If
            If cDisp.P31_RecalcRates Then ami("Přepočítat sazby rozpracovaných čas.úkonů", "cmdP31Recalc", "javascript:p31_recalc();", "Images/recalc.png", mi)
            If cDisp.P31_Move2Bin Then ami("Přesunout nevyfakturované úkony do/z archivu", "cmdP31Move2Bin", "javascript:p31_move2bin();", "Images/bin.png", mi)
            If cDisp.P31_MoveToOtherProject Then ami("Přesunout rozpracovanost na jiný projekt", "cmdP31MoveToOtherProject", "javascript:p31_move2project();", "Images/cut.png", mi)
        End If

        If cRec.b01ID = 0 Then ami("Doplnit poznámku, komentář, přílohu", "cmdB07", "javascript:menu_b07_record();", "Images/comment.png", mi, , True)
        If cP42.p42IsModule_p48 Then
            ami("Operativní plán projektu", "cmdP48", "javascript:p48_plan();", "Images/oplan.png", mi, , True)
        End If
        ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")
        If cDisp.OwnerAccess Then
            ami("Historie záznamu", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        End If
        ami("Čárový kód", "barcode", "javascript:menu_barcode()", "Images/barcode.png", mi)


    End Sub

    Private Sub SetupMenu_thePage(strReloadUrl As String, strTabValue As String)
        Dim mi As NavigationNode = ami("STRÁNKA", "thePage", "", "", Nothing)
        ami("Ukotvit vybranou záložku", "", "entity_menu_locktab.aspx?page=" & Server.UrlEncode(strReloadUrl) & "&prefix=" & hidDataPrefix.Value & "&tab=" & strTabValue, "Images/lock.png", mi)
        ami("Nastavení vzhledu stránky", "", "javascript:page_setting()", "Images/setting.png", mi)
        Select Case Me.hidSource.Value
            Case "1"
                ami("Otevřít v nové záložce", "fs", "javascript:menu_fullscreen()", "Images/open_in_new_window.png", mi)
                ami("Obnovit stránku", "reload", strReloadUrl, "Images/refresh.png", mi)
            Case "2"
                ami("Otevřít v nové záložce", "fs", "javascript:menu_fullscreen()", "Images/open_in_new_window.png", mi)
            Case "3"
                ami("Přepnout do datového přehledu", "fs", "entity_framework.aspx?prefix=" & Me.DataPrefix, "Images/fullscreen.png", mi)
                ami("Obnovit stránku", "reload", strReloadUrl, "Images/refresh.png", mi)
        End Select

    End Sub


    Private Sub hmi(strMenuValue As String, bolVisible As Boolean)
        Dim mi As NavigationNode = FNO(strMenuValue)
        If mi Is Nothing Then Return
        mi.Visible = bolVisible
    End Sub
    Private Function ami(strText As String, strValue As String, strURL As String, strImg As String, miParent As NavigationNode, Optional strToolTip As String = "", Optional bolSeparatorBefore As Boolean = False, Optional strTarget As String = "") As NavigationNode
        If bolSeparatorBefore And Not miParent Is Nothing Then
            ''Dim sep As New RadMenuItem()
            ''sep.IsSeparator = True
            ''miParent.Nodes.Add(sep)
        End If
        Dim mi As New NavigationNode(strText)
        mi.NavigateUrl = strURL
        mi.ID = strValue
        mi.ImageUrl = strImg
        mi.ToolTip = strToolTip
        mi.Target = strTarget
        If Not miParent Is Nothing Then
            miParent.Nodes.Add(mi)
        Else
            menu1.Nodes.Insert(menu1.Nodes.Count - 1, mi)
        End If

        Return mi
    End Function


    Private Sub cti(strName As String, strX61Code As String)
        Dim cX61 As New BO.x61PageTab
        cX61.x61Code = strX61Code
        Dim tab As New RadTab(strName, strX61Code)
        tabs1.Tabs.Add(tab)

        'tab.NavigateUrl = cX61.GetPageUrl(Me.DataPrefix, Me.DataPID, Me.hidIsCanApprove.Value) & "&tab=" & strX61Code & "&savetab=1&source=" & Me.hidSource.Value
        tab.NavigateUrl = cX61.GetPageUrl(Me.DataPrefix, Me.DataPID, Me.hidIsCanApprove.Value) & "&tab=" & strX61Code & "&source=" & Me.hidSource.Value

        If tabs1.Tabs.Count = 0 Then tab.Selected = True
    End Sub
    Private Sub p41_SetupTabs(crs As BO.p41ProjectSum, cP42 As BO.p42ProjectType, cDisp As BO.p41RecordDisposition)
        tabs1.Tabs.Clear()
        Dim s As String = ""
        cti("Projekt", "board")
        If cP42.p42IsModule_p31 Then
            ''s = "Summary" : cti(s, "summary")
            s = "Worksheet"
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                With crs
                    If .p31_Wip_Time_Count > 0 Or .p31_Wip_Expense_Count > 0 Or .p31_Wip_Fee_Count > 0 Then
                        s += "<span class='badge1wip'>" & .p31_Wip_Time_Count.ToString & "+" & .p31_Wip_Expense_Count.ToString & "+" & .p31_Wip_Fee_Count.ToString & "</span>"
                    End If
                End With
            End If
            cti(s, "p31")

            s = "Hodiny"
            If crs.p31_Wip_Time_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
            If crs.p31_Approved_Time_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
            cti(s, "time")

            s = "Výdaje"
            If crs.p31_Wip_Expense_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
            If crs.p31_Approved_Expense_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
            cti(s, "expense")
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                s = "Odměny"
                If crs.p31_Wip_Fee_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
                If crs.p31_Approved_Fee_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
                cti(s, "fee")
                If cDisp.p91_Read Then
                    s = "Faktury"
                    If crs.p91_Count > 0 Then s += "<span class='badge1tab'>" & crs.p91_Count.ToString & "</span>"
                    cti(s, "p91")
                End If
            End If
        End If
        If crs.childs_Count > 0 Then
            s = "Pod-projekty<span class='badge1tab'>" & crs.childs_Count.ToString & "</span>"
            cti(s, "p41")
        End If
        If cP42.p42IsModule_p56 Then
            s = "Úkoly"
            If crs.p56_Actual_Count > 0 Then s += "<span class='badge1tab'>" & crs.p56_Actual_Count.ToString & "</span>"
            cti(s, "p56")
        End If
        If cP42.p42IsModule_p45 Then
            If cDisp.p45_Read Then
                s = "Rozpočet"
                If crs.p45_Count > 0 Then s += "<span class='badge1tab'>" & crs.p45_Count.ToString & "</span>"
                cti(s, "budget")
            End If
        End If
        If cP42.p42IsModule_o23 Then
            s = "Dokumenty"
            If crs.o23_Count > 0 Then s += "<span class='badge1tab'>" & crs.o23_Count.ToString & "</span>"
            cti(s, "o23")
        End If

    End Sub

    Private Sub RemoveTab(strTabValue As String)
        With tabs1.Tabs
            If Not .FindTabByValue(strTabValue) Is Nothing Then
                .Remove(.FindTabByValue(strTabValue))
                If .Count > 0 Then tabs1.SelectedIndex = 0

            End If
        End With
    End Sub
    Private Sub Handle_SelectedTab()
        If tabs1.SelectedTab Is Nothing And tabs1.Tabs.Count > 0 Then
            'pokud není označená záložka, pak skočit na první
            Dim c As New BO.x61PageTab
            c.x61Code = "board"
            Server.Transfer(c.GetPageUrl(Me.DataPrefix, Me.DataPID, "") & "&tab=board&source=" & Me.hidSource.Value, False)
        End If
        If Not tabs1.SelectedTab Is Nothing Then
            tabs1.SelectedTab.NavigateUrl = ""
            tabs1.SelectedTab.Style.Item("cursor") = "default"
        End If
    End Sub

    Public Sub p28_RefreshRecord(cRec As BO.p28Contact, cRecSum As BO.p28ContactSum, strTabValue As String, Optional cDisp As BO.p28RecordDisposition = Nothing)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID
        If cDisp Is Nothing Then cDisp = Me.Factory.p28ContactBL.InhaleRecordDisposition(cRec)
        p28_SetupTabs(cRecSum)
        p28_SetupMenu(cRec, cDisp)
        SetupMenu_thePage("p28_framework_detail.aspx?pid=" & cRec.PID.ToString & "&source=" & Me.hidSource.Value, strTabValue)

        Me.CurrentTab = strTabValue
        Handle_SelectedTab()
    End Sub

    Private Sub p28_SetupTabs(crs As BO.p28ContactSum)
        tabs1.Tabs.Clear()
        cti("Klient", "board")
        Dim bolAllowRates As Boolean = Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates)
        Dim lisX61 As IEnumerable(Of BO.x61PageTab) = Me.Factory.j03UserBL.GetList_PageTabs(Me.Factory.SysUser.PID, BO.x29IdEnum.p28Contact)
        For Each c In lisX61
            Dim s As String = c.x61Name, bolGo As Boolean = True
            Select Case c.x61Code
                Case "p31"
                    If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                        With crs
                            If .p31_Wip_Time_Count > 0 Or .p31_Wip_Expense_Count > 0 Or .p31_Wip_Fee_Count > 0 Then
                                s += "<span class='badge1wip'>" & .p31_Wip_Time_Count.ToString & "+" & .p31_Wip_Expense_Count.ToString & "+" & .p31_Wip_Fee_Count.ToString & "</span>"
                            End If
                        End With
                    End If
                Case "time"
                    If crs.p31_Wip_Time_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
                    If crs.p31_Approved_Time_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
                Case "kusovnik"
                    If crs.p31_Wip_Kusovnik_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Kusovnik_Count.ToString & "</span>"
                    If crs.p31_Approved_Kusovnik_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Kusovnik_Count.ToString & "</span>"
                Case "expense"
                    If crs.p31_Wip_Expense_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
                    If crs.p31_Approved_Expense_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
                Case "fee"
                    If crs.p31_Wip_Fee_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
                    If crs.p31_Approved_Fee_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
                    bolGo = bolAllowRates
                Case "p91"
                    If crs.p91_Count > 0 Then s += "<span class='badge1tab'>" & crs.p91_Count.ToString & "</span>"
                    bolGo = Me.Factory.TestPermission(BO.x53PermValEnum.GR_P91_Reader)
                Case "p56"
                    If crs.p56_Actual_Count > 0 Then s += "<span class='badge1tab'>" & crs.p56_Actual_Count.ToString & "</span>"
                Case "o23"
                    If crs.o23_Count > 0 Then s += "<span class='badge1tab'>" & crs.o23_Count.ToString & "</span>"
                Case "p41"
                    s += "<span class='badge1tab'>" & crs.p41_Actual_Count.ToString & "+" & crs.p41_Closed_Count.ToString & "</span>"
                Case "p90"
                    If crs.p90_Count > 0 Then s += "<span class='badge1tab'>" & crs.p90_Count.ToString & "</span>"
                    bolGo = Me.Factory.TestPermission(BO.x53PermValEnum.GR_P90_Reader)
            End Select

            If bolGo Then cti(s, c.x61Code)
        Next
       
    End Sub

    Private Sub Handle_NoAccess(strMessage As String)
        Response.Redirect("stoppage.aspx?err=1&message=" & Server.UrlEncode(strMessage), True)
    End Sub

    Private Sub p28_SetupMenu(cRec As BO.p28Contact, cDisp As BO.p28RecordDisposition)
        If cRec.IsClosed Then menu1.Skin = "Black"

        If Not cDisp.ReadAccess Then
            Handle_NoAccess("Nedisponujete přístupovým oprávněním ke klientovi.")
        End If
      

        Dim mi As NavigationNode = FNO("record")
        If mi.Nodes.Count > 0 Then Return 'menu už bylo dříve zpracované
        mi.Text = "ZÁZNAM KLIENTA"
        If cDisp.OwnerAccess Then
            ami("Upravit kartu klienta", "cmdEdit", "javascript:record_edit();", "Images/edit.png", mi, "Zahrnuje i možnost přesunutí do archviu nebo nenávratného odstranění.")
        End If
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator) Then
            ami("Založit klienta", "cmdNew", "javascript:record_new();", "Images/new.png", mi, , True)
            ami("Založit klienta kopírováním", "cmdCopy", "javascript:record_clone();", "Images/copy.png", mi, "Nový klient se kompletně předvyplní podle vzoru tohoto záznamu.")
        End If
        If cRec.p28SupplierFlag = BO.p28SupplierFlagENUM.ClientAndSupplier Or cRec.p28SupplierFlag = BO.p28SupplierFlagENUM.ClientOnly Then
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator) Then
                ami("Založit pro klienta nový projekt", "cmdNewP41", "javascript:p28_p41_new();", "Images/project.png", mi, , True)
            End If
        End If
        If Me.Factory.SysUser.IsApprovingPerson Then
            If cRec.p28SupplierFlag <> BO.p28SupplierFlagENUM.NotClientNotSupplier Then
                ami("Schvalovat nebo vystavit fakturu", "cmdApprove", "javascript:approve();", "Images/approve.png", mi, , True)
                If Factory.TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator, BO.x53PermValEnum.GR_P31_Approver) Then
                    ami("Vystavit fakturu zrychleně bez schvalování", "cmdDraft", "javascript:menu_p28_invoice_draft();", "Images/invoice.png", mi)
                End If
            End If

        End If
        If cRec.b02ID > 0 Then
            ami("Posunout/doplnit", "cmdWorkflow", "javascript:workflow();", "Images/workflow.png", mi, , True)
        End If
        ami("Tisková sestava/pdf/e-mail", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)
        ami("Odeslat e-mail", "cmdMail", "javascript:menu_sendmail();", "Images/email.png", mi, , True)

        mi = ami("DALŠÍ", "more", "", "", Nothing)
        'mi.GroupSettings.OffsetX = -200

        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            ami("WORKSHEET statistika klienta", "cmdPivot", "p31_sumgrid.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
        End If
        ami("Plugin", "cmdPlugin", "javascript:plugin();", "Images/plugin.png", mi, , True)
        ami("Štítky", "cmdTags", "javascript:tags_menu();", "Images/tag.png", mi, , True)

        If Me.Factory.SysUser.j04IsMenu_Notepad Then
            ami("Vytvořit dokument", "cmdO23", "javascript:menu_o23_record(0);", "Images/notepad.png", mi, , True)
        End If


        If Not cRec.IsClosed Then ami("Vytvořit událost v kalendáři", "cmdO22", "javascript:menu_o22_record(0);", "Images/calendar.png", mi, , True)
        ami("Kalendář klienta", "cmdScheduler", "javascript:scheduler()", "Images/calendar.png", mi)

        If cRec.b02ID = 0 Then ami("Doplnit poznámku, komentář, přílohu", "cmdB07", "javascript:menu_b07_record();", "Images/comment.png", mi, , True)



        ami("Operativní plán projektů klienta", "cmdP48", "javascript:p48_plan();", "Images/oplan.png", mi, , True)
        ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")
        If cDisp.OwnerAccess Then
            ami("Historie záznamu", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        End If
        ami("Čárový kód", "barcode", "javascript:menu_barcode()", "Images/barcode.png", mi)


    End Sub


    Public Sub j02_RefreshRecord(cRec As BO.j02Person, cRecSum As BO.j02PersonSum, strTabValue As String)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID

        j02_SetupTabs(cRec, cRecSum)
        j02_SetupMenu(cRec)
        SetupMenu_thePage("j02_framework_detail.aspx?pid=" & cRec.PID.ToString & "&source=" & Me.hidSource.Value, strTabValue)

        Me.CurrentTab = strTabValue
        Handle_SelectedTab()
    End Sub

    Private Sub j02_SetupMenu(cRec As BO.j02Person)
        If cRec.IsClosed Then menu1.Skin = "Black"

        If Not Me.Factory.SysUser.j04IsMenu_People Then
            Handle_NoAccess("Nedisponujete přístupovým oprávněním k prohlížení osobních profilů.")
        End If



        Dim mi As NavigationNode = FNO("record")
        mi.Text = "ZÁZNAM OSOBY"
        If mi.Nodes.Count > 0 Then Return 'menu už bylo dříve zpracované
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
            ami("Upravit kartu osoby", "cmdEdit", "javascript:record_edit();", "Images/edit.png", mi, "Zahrnuje i možnost přesunutí do archivu nebo nenávratného odstranění.")
            ami("Založit osobu", "cmdNew", "javascript:record_new();", "Images/new.png", mi, , True)
            ami("Založit osobu kopírováním", "cmdCopy", "javascript:record_clone();", "Images/copy.png", mi, "Nově zakládaná osoba se kompletně předvyplní z aktuálního osobního profilu.")
        End If

        If Me.Factory.SysUser.IsApprovingPerson Then
            If cRec.j02IsIntraPerson Then ami("Schvalovat nebo fakturovat práci osoby", "cmdApprove", "javascript:approve();", "Images/approve.png", mi, , True)
        End If
        ami("Tisková sestava/pdf/e-mail", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)
        ami("Odeslat e-mail", "cmdMail", "javascript:menu_sendmail();", "Images/email.png", mi, , True)

        mi = ami("DALŠÍ", "more", "", "", Nothing)
        ''mi.GroupSettings.OffsetX = -200

        If cRec.j02IsIntraPerson Then
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                ami("WORKSHEET statistika osoby", "cmdPivot", "p31_sumgrid.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
            End If
            ami("Plugin", "cmdPlugin", "javascript:plugin();", "Images/plugin.png", mi, , True)
            ami("Štítky", "cmdTags", "javascript:tags_menu();", "Images/tag.png", mi, , True)
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
                ami("Přepočítat sazby rozpracovaných čas.úkonů", "cmdP31Recalc", "javascript:p31_recalc();", "Images/recalc.png", mi)
            End If

            If Me.Factory.SysUser.j04IsMenu_Notepad Then
                ami("Vytvořit dokument", "cmdO23", "javascript:menu_o23_record(0);", "Images/notepad.png", mi, , True)
            End If
            If Not cRec.IsClosed Then ami("Vytvořit událost v kalendáři", "cmdO22", "javascript:menu_o22_record(0);", "Images/calendar.png", mi, , True)
            ami("Kalendář osoby", "cmdScheduler", "javascript:scheduler()", "Images/calendar.png", mi)

            ami("Operativní plán osoby", "cmdP48", "javascript:p48_plan();", "Images/oplan.png", mi, , True)
            ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")

            ami("Historie záznamu", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        Else
            ami("Štítky", "cmdTags", "javascript:tags_menu();", "Images/tag.png", mi, , True)
        End If

        ami("Doplnit poznámku, komentář, přílohu", "cmdB07", "javascript:menu_b07_record();", "Images/comment.png", mi, , True)
    End Sub

    Private Sub j02_SetupTabs(cRec As BO.j02Person, crs As BO.j02PersonSum)
        tabs1.Tabs.Clear()
        If cRec.j02IsIntraPerson Then
            cti(cRec.FullNameAsc, "board")
        Else
            cti(System.String.Format("{0} (kontaktní osoba)", cRec.FullNameAsc), "board")
            Return
        End If
        Dim lisX61 As IEnumerable(Of BO.x61PageTab) = Me.Factory.j03UserBL.GetList_PageTabs(Me.Factory.SysUser.PID, BO.x29IdEnum.j02Person)
        For Each c In lisX61
            Dim s As String = c.x61Name
            Select Case c.x61Code
                Case "p31"
                    If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                        With crs
                            If .p31_Wip_Time_Count > 0 Or .p31_Wip_Expense_Count > 0 Or .p31_Wip_Fee_Count > 0 Then
                                s += "<span class='badge1wip'>" & .p31_Wip_Time_Count.ToString & "+" & .p31_Wip_Expense_Count.ToString & "+" & .p31_Wip_Fee_Count.ToString & "</span>"
                            End If
                        End With
                    End If
                Case "time"
                    If crs.p31_Wip_Time_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
                    If crs.p31_Approved_Time_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
                Case "expense"
                    If crs.p31_Wip_Expense_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
                    If crs.p31_Approved_Expense_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
                Case "fee"
                    If crs.p31_Wip_Fee_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
                    If crs.p31_Approved_Fee_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
                Case "kusovnik"
                    If crs.p31_Wip_Kusovnik_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Kusovnik_Count.ToString & "</span>"
                    If crs.p31_Approved_Kusovnik_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Kusovnik_Count.ToString & "</span>"
                Case "p91"
                    If crs.p91_Count > 0 Then s += "<span class='badge1tab'>" & crs.p91_Count.ToString & "</span>"
                Case "p56"
                    If crs.p56_Actual_Count > 0 Then s += "<span class='badge1tab'>" & crs.p56_Actual_Count.ToString & "</span>"
                Case "o23"
                    If crs.o23_Count > 0 Then s += "<span class='badge1tab'>" & crs.o23_Count.ToString & "</span>"
            End Select

            cti(s, c.x61Code)
        Next


    End Sub

    Public Sub p56_RefreshRecord(cRec As BO.p56Task, crs As BO.p56TaskSum, cP41 As BO.p41Project, strTabValue As String, Optional cDisp As BO.p56RecordDisposition = Nothing)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID
        Dim cP42 As BO.p42ProjectType = Me.Factory.p42ProjectTypeBL.Load(cP41.p42ID)

        cti("Úkol", "board")
        Dim s As String = ""
        If cP42.p42IsModule_p31 Then
            s = "Worksheet"
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                With crs
                    If .p31_Wip_Time_Count > 0 Or .p31_Wip_Expense_Count > 0 Or .p31_Wip_Fee_Count > 0 Then
                        s += "<span class='badge1wip'>" & .p31_Wip_Time_Count.ToString & "+" & .p31_Wip_Expense_Count.ToString & "+" & .p31_Wip_Fee_Count.ToString & "</span>"
                    End If
                End With
            End If
            cti(s, "p31")
            s = "Hodiny"
            If crs.p31_Wip_Time_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
            If crs.p31_Approved_Time_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
            cti(s, "time")
            s = "Výdaje"
            If crs.p31_Wip_Expense_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
            If crs.p31_Approved_Expense_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
            cti(s, "expense")
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                s = "Odměny"
                If crs.p31_Wip_Fee_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
                If crs.p31_Approved_Fee_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
                cti(s, "fee")
            End If
            If Me.Factory.SysUser.j04IsMenu_Invoice Then
                s = "Faktury"
                If crs.p91_Count > 0 Then s += "<span class='badge1tab'>" & crs.p91_Count.ToString & "</span>"
                cti(s, "p91")
            End If
        End If
        s = "Dokumenty"
        If crs.o23_Count > 0 Then s += "<span class='badge1tab'>" & crs.o23_Count.ToString & "</span>"
        cti(s, "o23")

        p56_SetupMenu(cRec, cP41, cP42, cDisp)
        SetupMenu_thePage("p56_framework_detail.aspx?pid=" & cRec.PID.ToString & "&source=" & Me.hidSource.Value, strTabValue)

        Me.CurrentTab = strTabValue
        Handle_SelectedTab()


    End Sub

    Private Sub p56_SetupMenu(cRec As BO.p56Task, cP41 As BO.p41Project, cP42 As BO.p42ProjectType, cDisp As BO.p56RecordDisposition)
        If cRec.IsClosed Then menu1.Skin = "Black"
        If cDisp Is Nothing Then cDisp = Me.Factory.p56TaskBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then
            Handle_NoAccess("Nedisponujete oprávněním číst tento úkol.")
        End If
        
        Dim mi As NavigationNode = FNO("record")
        If mi.Nodes.Count > 0 Then Return 'menu už bylo dříve zpracované

        mi.Text = "ZÁZNAM ÚKOLU"
        If cDisp.OwnerAccess Then
            ami("Upravit kartu úkolu", "cmdEdit", "javascript:record_edit();", "Images/edit.png", mi)
        End If
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P56_Creator) Then
            ami("Založit úkol", "cmdNew", "javascript:p56_record_new(" & cRec.p41ID.ToString & ");", "Images/new.png", mi, , True)
            ami("Založit úkol kopírováním", "cmdCopy", "javascript:record_clone();", "Images/copy.png", mi, "Nový úkol se kompletně předvyplní podle vzoru tohoto záznamu.")
        End If
        Dim bolCanApprove As Boolean = Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Approver)
        Dim cDispP41 As BO.p41RecordDisposition = Me.Factory.p41ProjectBL.InhaleRecordDisposition(cP41)

        If bolCanApprove = False And cDispP41.x67IDs.Count > 0 Then
            Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Me.Factory.x67EntityRoleBL.GetList_o28(cDispP41.x67IDs)
            If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                bolCanApprove = True
            End If
        End If
        If bolCanApprove Then
            ami("Schvalovat nebo vystavit fakturu", "cmdApprove", "javascript:approve();", "Images/approve.png", mi, , True)
        End If
        Me.hidIsCanApprove.Value = BO.BAS.GB(bolCanApprove)
        If cRec.b01ID > 0 Then
            ami("Posunout/doplnit", "cmdWorkflow", "javascript:workflow();", "Images/workflow.png", mi, , True)
        End If
        ami("Tisková sestava/pdf/e-mail", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)
        ami("Odeslat e-mail", "cmdMail", "javascript:menu_sendmail();", "Images/email.png", mi, , True)

        If cDisp.P31_Create Then
            If cP42.p42IsModule_p31 Then
                mi = ami("ZAPSAT WORKSHEET", "p31", "", "Images/arrow_down_menu.png", Nothing)
                If Not (cRec.IsClosed Or cP41.p41IsDraft Or cP41.IsClosed) Then
                    Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = Me.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.p41ID, cP41.p42ID, cP41.j18ID, Me.Factory.SysUser.j02ID)
                    For Each c In lisP34
                        ami(String.Format(Resources.p41_framework_detail.ZapsatUkonDo, c.p34Name), "", "javascript:p31_entry_menu(" & c.PID.ToString & ")", "Images/worksheet.png", mi, "")
                    Next
                    If lisP34.Count = 0 Then
                        mi = ami(Resources.p41_framework_detail.NedisponujeteOpravnenimZapisovat, "", "", "", mi)
                        mi.ForeColor = Drawing.Color.Red
                    End If
                Else
                    mi = ami("V projektu nemůžete zapisovat worksheet úkony.", "", "", "", mi)
                    mi.ForeColor = Drawing.Color.Red
                    If cP41.IsClosed Then
                        mi.Text = Resources.p41_framework_detail.VArchivuNelzeWorksheet
                    End If
                    If cP41.p41IsDraft Then mi.Text = Resources.p41_framework_detail.VDraftNelzeWorksheet
                    If cRec.IsClosed Then
                        mi.Text = "Do uzavřeného úkolu nelze zapisovat worksheet úkony."
                    End If
                    mi.ForeColor = Drawing.Color.Red
                End If
            End If
        End If


        mi = ami("DALŠÍ", "more", "", "", Nothing)
        ''mi.GroupSettings.OffsetX = -270
        'If hidSource.Value <> "2" Then ami("Nastavení vzhledu stránky úkolu", "", "javascript:page_setting()", "Images/setting.png", mi)
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            ami("WORKSHEET statistika úkolu", "cmdPivot", "p31_sumgrid.aspx?masterprefix=p56&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
        End If
        ami("Štítky", "cmdTags", "javascript:tags_menu();", "Images/tag.png", mi, , True)

        ami("Vytvořit dokument", "cmdO23", "javascript:menu_o23_record(0);", "Images/notepad.png", mi, , True)


        If cRec.b01ID = 0 Then ami("Doplnit poznámku, komentář, přílohu", "cmdB07", "javascript:menu_b07_record();", "Images/comment.png", mi, , True)
        ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=p56&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")
        If cDisp.OwnerAccess Then
            ami("Historie záznamu", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        End If



    End Sub



    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Select Case hidSource.Value
            Case "1"
               
            Case "2"
                If menu1.Skin <> "Black" Then
                    menu1.Skin = "Metro"
                End If

            Case "3"

        End Select
        HighLight_LockedTab(hidLockedTab.Value)
    End Sub

    Private Function FNO(strValue As String) As NavigationNode
        Return menu1.GetAllNodes.First(Function(p) p.ID = strValue)
    End Function
End Class