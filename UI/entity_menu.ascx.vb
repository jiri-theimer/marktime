Imports Telerik.Web.UI
Public Class entity_menu
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory
    Public Property DataPrefix As String
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
        End If

    End Sub

    Public Sub p41_RefreshRecord(cRec As BO.p41Project, cRecSum As BO.p41ProjectSum, strTabValue As String)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID

        Dim cP42 As BO.p42ProjectType = Me.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        p41_SetupTabs(cRecSum, cP42)
        p41_SetupMenu(cRec, cP42)

        Me.CurrentTab = strTabValue
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

    

    Private Sub p41_SetupMenu(cRec As BO.p41Project, cP42 As BO.p42ProjectType)
        If cRec.IsClosed Then menu1.Skin = "Black"
        Dim cDisp As BO.p41RecordDisposition = Me.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
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
        If Not miParent Is Nothing Then miParent.Items.Add(mi) Else menu1.Items.Add(mi)

        Return mi
    End Function


    Private Sub cti(strName As String, strX61Code As String)
        Dim cX61 As New BO.x61PageTab
        cX61.x61Code = strX61Code
        Dim tab As New RadTab(strName, strX61Code)
        tabs1.Tabs.Add(tab)
      
        tab.NavigateUrl = cX61.GetPageUrl("p41", Me.DataPID, Me.hidIsCanApprove.Value) & "&tab=" & strX61Code

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
            cti(s, "p45")
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
End Class