﻿Imports Telerik.Web.UI
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            If Me.Factory.SysUser.OneProjectPage <> "" Then
                Server.Transfer(basUI.AddQuerystring2Page(Me.Factory.SysUser.OneProjectPage, "pid=" & Me.DataPID.ToString))
            End If
            If Request.Item("tab") <> "" Then
                Me.Factory.j03UserBL.SetUserParam(Me.DataPrefix & "_framework_detail-tab", Request.Item("tab"))
            End If
            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                basUIMT.RenderSawMenuItemAsGrid(menu1.FindItemByValue("saw"), Me.DataPrefix)
            Else
                menu1.FindItemByValue("saw").NavigateUrl = Me.DataPrefix & "_framework_detail.aspx?saw=1"
            End If
        End If
        Select Case Me.DataPrefix
            Case "p28"
                sb1.ashx = "handler_search_contact.ashx"
                sb1.aspx = "p28_framework.aspx"
                sb1.TextboxLabel = "Najít klienta..."
            Case "j02"
                sb1.ashx = "handler_search_persont.ashx"
                sb1.aspx = "j02_framework.aspx"
                sb1.TextboxLabel = "Najít osobu..."
            Case "p41"
                sb1.Visible = False
        End Select
        
        
    End Sub

    Public Sub p41_RefreshRecord(cRec As BO.p41Project, cRecSum As BO.p41ProjectSum, strTabValue As String, Optional cDisp As BO.p41RecordDisposition = Nothing)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID

        Dim cP42 As BO.p42ProjectType = Me.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        p41_SetupTabs(cRecSum, cP42)
        p41_SetupMenu(cRec, cP42, cDisp)

        Me.CurrentTab = strTabValue
        Handle_SelectedTab()
    End Sub

    

    Private Sub p41_SetupMenu(cRec As BO.p41Project, cP42 As BO.p42ProjectType, cDisp As BO.p41RecordDisposition)
        If cRec.IsClosed Then menu1.Skin = "Black"
        If cDisp Is Nothing Then cDisp = Me.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then
            Handle_NoAccess("Nedisponujete přístupovým oprávněním k projektu.")
        End If
        With menu1.FindItemByValue("begin")
            CType(.FindControl("imgLogo"), Image).ImageUrl = "Images/project_32.png"
        End With
        

        With cRec
            basUIMT.RenderHeaderMenu(.IsClosed, Me.panMenuContainer, menu1)
            Dim strLevel1 As String = .FullName
            If Len(cRec.Client) > 30 Then
                strLevel1 = Left(.Client, 30) & "..."
                strLevel1 += "->" & .PrefferedName
            End If

            basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), strLevel1, "p41_framework_detail.aspx?pid=" & .PID.ToString, .IsClosed)
        End With

        Dim mi As RadMenuItem = menu1.FindItemByValue("record")
        If mi.Items.Count > 0 Then Return 'menu už bylo dříve zpracované

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
            End If
            Me.hidIsCanApprove.Value = BO.BAS.GB(bolCanApproveOrInvoice)
        End If
        ami("Tisková sestava", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)

        If cP42.p42IsModule_p31 Then
            mi = ami("ZAPSAT WORKSHEET", "p31", "", "Images/arrow_down_menu.png", Nothing)
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

        mi = ami("DALŠÍ", "more", "", "Images/arrow_down_menu.png", Nothing)
        ami("Nastavení vzhledu stránky", "", "javascript:page_setting()", "Images/setting.png", mi)
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            ami("PIVOT za projekt", "cmdPivot", "p31_pivot.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
        End If
        If cDisp.OwnerAccess Then
            ami("Přiřadit k projektu kontaktní osoby", "cmdP30", "javascript:p30_record(0);", "Images/person.png", mi, , True)
        End If
        If Not cRec.IsClosed Then
            If cP42.p42IsModule_p56 Then ami("Vytvořit úkol", "cmdP56", "javascript:p56_record(0);", "Images/person.png", mi, , True)
            If cP42.p42IsModule_o22 Then ami("Vytvořit kalendářovou událost/lhůtu", "cmdO22", "javascript:o22_record(0);", "Images/calendar.png", mi, , True)
        End If
        If cP42.p42IsModule_o23 Then
            ami("Vytvořit dokument", "cmdO23", "javascript:o23_record(0);", "Images/notepad.png", mi)
        End If
        If cP42.p42IsModule_p48 Then
            ami("Operativní plán projektu", "cmdP48", "javascript:p48_plan();", "Images/oplan.png", mi, , True)
        End If

        If cP42.p42IsModule_p31 Then
            If cDisp.OwnerAccess Then
                ami("Definovat opakovanou odměnu/paušál/úkon", "cmdP40Create", "javascript:p40_record(0);", "Images/worksheet_recurrence.png", mi, , True)
            End If
            If cDisp.P31_RecalcRates Then ami("Přepočítat sazby rozpracovaných čas.úkonů", "cmdP31Recalc", "javascript:p31_recalc();", "Images/recalc.png", mi)
            If cDisp.P31_Move2Bin Then ami("Přesunout nevyfakturované úkony do/z archivu", "cmdP31Move2Bin", "javascript:p31_move2bin();", "Images/bin.png", mi)
            If cDisp.P31_MoveToOtherProject Then ami("Přesunout rozpracovanost na jiný projekt", "cmdP31MoveToOtherProject", "javascript:p31_move2project();", "Images/cut.png", mi)
        End If

        If cRec.b01ID = 0 Then ami("Zapsat komentář/poznámku", "cmdB07", "javascript:b07_record();", "Images/comment.png", mi, , True)
        ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")
        If cDisp.OwnerAccess Then
            ami("Historie záznamu", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        End If

        menu1.Items.Remove(menu1.FindItemByValue("searchbox"))  'searchbox u projektu není

    End Sub


    Private Sub hmi(strMenuValue As String, bolVisible As Boolean)
        Dim mi As RadMenuItem = menu1.FindItemByValue(strMenuValue)
        If mi Is Nothing Then Return
        mi.Visible = bolVisible
    End Sub
    Private Function ami(strText As String, strValue As String, strURL As String, strImg As String, miParent As RadMenuItem, Optional strToolTip As String = "", Optional bolSeparatorBefore As Boolean = False, Optional strTarget As String = "") As RadMenuItem
        If bolSeparatorBefore And Not miParent Is Nothing Then
            Dim sep As New RadMenuItem()
            sep.IsSeparator = True
            miParent.Items.Add(sep)
        End If
        Dim mi As New RadMenuItem(strText, strURL)
        mi.Value = strValue
        mi.ImageUrl = strImg
        mi.ToolTip = strToolTip
        mi.Target = strTarget
        If Not miParent Is Nothing Then
            miParent.Items.Add(mi)
        Else
            menu1.Items.Insert(menu1.Items.Count - 1, mi)
        End If

        Return mi
    End Function


    Private Sub cti(strName As String, strX61Code As String)
        Dim cX61 As New BO.x61PageTab
        cX61.x61Code = strX61Code
        Dim tab As New RadTab(strName, strX61Code)
        tabs1.Tabs.Add(tab)
      
        tab.NavigateUrl = cX61.GetPageUrl(Me.DataPrefix, Me.DataPID, Me.hidIsCanApprove.Value) & "&tab=" & strX61Code

        If tabs1.Tabs.Count = 0 Then tab.Selected = True
    End Sub
    Private Sub p41_SetupTabs(crs As BO.p41ProjectSum, cP42 As BO.p42ProjectType)
        tabs1.Tabs.Clear()
        Dim s As String = ""
        If Me.hidPOS.Value = "1" Then
            cti("Projekt", "board")
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
            cti(s, "budget")
        End If
        If cP42.p42IsModule_o23 And cP42.p42SubgridO23Flag = 1 Then
            s = "Dokumenty"
            If crs.o23_Count > 0 Then s += "<span class='badge1'>" & crs.o23_Count.ToString & "</span>"
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
            Server.Transfer(c.GetPageUrl(Me.DataPrefix, Me.DataPID, ""), False)
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

        Me.CurrentTab = strTabValue
        Handle_SelectedTab()
    End Sub

    Private Sub p28_SetupTabs(crs As BO.p28ContactSum)
        tabs1.Tabs.Clear()
        If Me.hidPOS.Value = "1" Then
            cti("Klient", "board")
        End If
        Dim lisX61 As IEnumerable(Of BO.x61PageTab) = Me.Factory.j03UserBL.GetList_PageTabs(Me.Factory.SysUser.PID, BO.x29IdEnum.p28Contact)
        For Each c In lisX61
            Dim s As String = c.x61Name
            Select Case c.x61Code
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
                    If crs.p91_Count > 0 Then s += "<span class='badge1'>" & crs.p91_Count.ToString & "</span>"
                Case "p56"
                    If crs.p56_Actual_Count > 0 Then s += "<span class='badge1'>" & crs.p56_Actual_Count.ToString & "</span>"
                Case "o23"
                    If crs.o23_Count > 0 Then s += "<span class='badge1'>" & crs.o23_Count.ToString & "</span>"
                Case "p41"
                    s += "<span class='badge1'>" & crs.p41_Actual_Count.ToString & "+" & crs.p41_Closed_Count.ToString & "</span>"
            End Select

            cti(s, c.x61Code)
        Next
        ''If crs.b07_Count > 0 Then
        ''    If lisX61.Where(Function(p) p.x61Code = "workflow").Count = 0 Then Me.alert1.Append("Ke klientovi byl zapsán minimálně jeden komentář. V nastavení vzhledu stránky klienta si přidejte záložku [Komentáře a workflow].")
        ''End If

    End Sub

    Private Sub Handle_NoAccess(strMessage As String)
        Response.Redirect("stoppage.aspx?err=1&message=" & Server.UrlEncode(strMessage), True)
    End Sub

    Private Sub p28_SetupMenu(cRec As BO.p28Contact, cDisp As BO.p28RecordDisposition)
        If cRec.IsClosed Then menu1.Skin = "Black"
        With menu1.FindItemByValue("begin")
            CType(.FindControl("imgLogo"), Image).ImageUrl = "Images/contact_32.png"
        End With
        If Not cDisp.ReadAccess Then
            Handle_NoAccess("Nedisponujete přístupovým oprávněním ke klientovi.")
        End If
        basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), cRec.p28Name, "p28_framework_detail.aspx?pid=" & cRec.PID.ToString, cRec.IsClosed)


        Dim mi As RadMenuItem = menu1.FindItemByValue("record")
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
            End If

        End If

        ami("Tisková sestava", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)

        mi = ami("DALŠÍ", "more", "", "Images/arrow_down_menu.png", Nothing)
        ami("Nastavení vzhledu stránky", "", "javascript:page_setting()", "Images/setting.png", mi)
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            ami("PIVOT za klienta", "cmdPivot", "p31_pivot.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
        End If
        If cDisp.OwnerAccess And Not cRec.IsClosed Then
            ami("Přiřadit ke klientovi kontaktní osoby", "cmdP30", "javascript:p30_record(0);", "Images/person.png", mi, , True)
        End If
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator) Then
            ami("Vytvořit dokument", "cmdO23", "javascript:o23_record(0);", "Images/notepad.png", mi, , True)
        End If

        If Not cRec.IsClosed Then ami("Vytvořit kalendářovou událost/lhůtu", "cmdO22", "javascript:o22_record(0);", "Images/calendar.png", mi, , True)
        ami("Kalendář klienta", "cmdScheduler", "javascript:scheduler()", "Images/calendar.png", mi)

        If cRec.b02ID = 0 Then ami("Zapsat komentář/poznámku", "cmdB07", "javascript:b07_record();", "Images/comment.png", mi, , True)



        ami("Operativní plán projektů klienta", "cmdP48", "javascript:p48_plan();", "Images/oplan.png", mi, , True)
        ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")
        If cDisp.OwnerAccess Then
            ami("Historie záznamu", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        End If



    End Sub


    Public Sub j02_RefreshRecord(cRec As BO.j02Person, cRecSum As BO.j02PersonSum, strTabValue As String)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID

        j02_SetupTabs(cRecSum)
        j02_SetupMenu(cRec)

        Me.CurrentTab = strTabValue
        Handle_SelectedTab()
    End Sub

    Private Sub j02_SetupMenu(cRec As BO.j02Person)
        If cRec.IsClosed Then menu1.Skin = "Black"
        With menu1.FindItemByValue("begin")
            CType(.FindControl("imgLogo"), Image).ImageUrl = "Images/person_32.png"
        End With
        If Not Me.Factory.SysUser.j04IsMenu_People Then
            Handle_NoAccess("Nedisponujete přístupovým oprávněním k prohlížení osobních profilů.")
        End If
        basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), cRec.FullNameAsc, "j02_framework_detail.aspx?pid=" & cRec.PID.ToString, cRec.IsClosed)


        Dim mi As RadMenuItem = menu1.FindItemByValue("record")
        mi.Text = "ZÁZNAM OSOBY"
        If Me.Factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
            ami("Upravit kartu osoby", "cmdEdit", "javascript:record_new();", "Images/new.png", mi, "Zahrnuje i možnost přesunutí do archivu nebo nenávratného odstranění.")
            ami("Založit osobu", "cmdNew", "javascript:record_new();", "Images/new.png", mi, , True)
            ami("Založit osobu kopírováním", "cmdCopy", "javascript:record_clone();", "Images/copy.png", mi, "Nově zakládaná osoba se kompletně předvyplní z aktuálního osobního profilu.")
        End If
        
        If Me.Factory.SysUser.IsApprovingPerson Then
            If cRec.j02IsIntraPerson Then ami("Schvalovat nebo fakturovat práci osoby", "cmdApprove", "javascript:approve();", "Images/approve.png", mi, , True)
        End If
        ami("Tisková sestava", "cmdReport", "javascript:report();", "Images/report.png", mi, , True)

        mi = ami("DALŠÍ", "more", "", "Images/arrow_down_menu.png", Nothing)
        ami("Nastavení vzhledu stránky", "", "javascript:page_setting()", "Images/setting.png", mi)
        If cRec.j02IsIntraPerson Then
            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                ami("PIVOT za osobu", "cmdPivot", "p31_pivot.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString, "Images/pivot.png", mi, , True, "_top")
            End If

            If Me.Factory.TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator) Then
                ami("Vytvořit dokument", "cmdO23", "javascript:o23_record(0);", "Images/notepad.png", mi, , True)
            End If
            If Not cRec.IsClosed Then ami("Vytvořit kalendářovou událost/lhůtu", "cmdO22", "javascript:o22_record(0);", "Images/calendar.png", mi, , True)
            ami("Kalendář osoby", "cmdScheduler", "javascript:scheduler()", "Images/calendar.png", mi)

            ami("Operativní plán osoby", "cmdP48", "javascript:p48_plan();", "Images/oplan.png", mi, , True)
            ami("Historie odeslané pošty", "cmdX40", "x40_framework.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString, "Images/email.png", mi, , , "_top")

            ami("Historie aktivit osoby", "cmdLog", "javascript: timeline()", "Images/event.png", mi)
        End If

        ami("Zapsat komentář/poznámku", "cmdB07", "javascript:b07_record();", "Images/comment.png", mi, , True)
    End Sub

    Private Sub j02_SetupTabs(crs As BO.j02PersonSum)
        tabs1.Tabs.Clear()
        If Me.hidPOS.Value = "1" Then
            cti("Osoba", "board")
        End If
        Dim lisX61 As IEnumerable(Of BO.x61PageTab) = Me.Factory.j03UserBL.GetList_PageTabs(Me.Factory.SysUser.PID, BO.x29IdEnum.j02Person)
        For Each c In lisX61
            Dim s As String = c.x61Name
            Select Case c.x61Code
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
                    If crs.p91_Count > 0 Then s += "<span class='badge1'>" & crs.p91_Count.ToString & "</span>"
                Case "p56"
                    If crs.p56_Actual_Count > 0 Then s += "<span class='badge1'>" & crs.p56_Actual_Count.ToString & "</span>"
                Case "o23"
                    If crs.o23_Count > 0 Then s += "<span class='badge1'>" & crs.o23_Count.ToString & "</span>"
            End Select

            cti(s, c.x61Code)
        Next
      

    End Sub
End Class