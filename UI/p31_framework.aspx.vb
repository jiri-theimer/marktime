Imports Telerik.Web.UI
Public Class p31_framework
    Inherits System.Web.UI.Page

    Protected WithEvents _MasterPage As Site
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private _lastP41ID As Integer = 0
    Private Property _needFilterIsChanged As Boolean = False

    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property
    
    Public ReadOnly Property CurrentJ02ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidCurrentJ02ID.Value)
        End Get
    End Property
    Public ReadOnly Property GridPrefix As String
        Get
            If tabs1.SelectedIndex = 2 Then Return "p56" Else Return "p41"
        End Get

    End Property
    Public Property IsUseReceiversInLine As Boolean
        Get
            If hidReceiversInLine.Value = "1" Then Return True Else Return False
        End Get
        Set(value As Boolean)
            hidReceiversInLine.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public Property IsUseTasksWorksheetColumns As Boolean
        Get
            If Me.hidTasksWorksheetColumns.Value = "1" Then Return True Else Return False
        End Get
        Set(value As Boolean)
            hidTasksWorksheetColumns.Value = BO.BAS.GB(value)
        End Set
    End Property
    

    Private Sub p31_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
            Response.Redirect("p31_framework_detail.aspx", True)
        End If
        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_framework"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Zapisování úkonů"
                .SiteMenuValue = "p31_framework"
                If Request.Item("showtimer") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p31_framework-timer", Request.Item("showtimer"))
                End If
                If Request.Item("tab") = "" Then
                    .Factory.j03UserBL.InhaleUserParams("p31_framework-tabindex")
                    tabs1.SelectedIndex = CInt(.Factory.j03UserBL.GetUserParam("p31_framework-tabindex", "0"))
                Else
                    tabs1.SelectedIndex = CInt(Request.Item("tab"))
                End If
                InitialGroupByCombo(tabs1.SelectedIndex)

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_framework-pagesize-" & Me.GridPrefix)
                    .Add("p31_framework-search")
                    .Add("p31_framework-navigationPane_width")
                    .Add("p31_framework_detail-j02id")  'výchozí osoba pro nové úkony
                    .Add("p31_framework-j74id-" & Me.GridPrefix)
                    .Add("p31_framework-groupby-" & Me.GridPrefix)
                    .Add("p31_framework-sort-" & Me.GridPrefix)
                    .Add("p31_framework-groups-autoexpanded")
                    .Add("p31_framework-timer")
                    If tabs1.SelectedIndex <> 1 Then    'v top10 se nefiltruje
                        .Add("p31_framework-filter_setting_p41")
                        .Add("p31_framework-filter_sql_p41")
                        .Add("p31_framework-filter_setting_p56")
                        .Add("p31_framework-filter_sql_p56")
                    End If
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                    hidCurrentJ02ID.Value = .GetUserParam("p31_framework_detail-j02id", Master.Factory.SysUser.j02ID.ToString)
                    Me.txtSearch.Text = .GetUserParam("p31_framework-search")
                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("p31_framework-pagesize-" & Me.GridPrefix, "20"))
                    Dim strW As String = .GetUserParam("p31_framework-navigationPane_width", "350")
                    If strW = "-1" Then
                        Me.navigationPane.Collapsed = True
                    Else
                        Me.navigationPane.Width = Unit.Parse(.GetUserParam("p31_framework-navigationPane_width", "350") & "px")
                    End If

                    
                    basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("p31_framework-groupby-" & Me.GridPrefix, IIf(Me.GridPrefix = "p56", "Client", "")))
                    If tabs1.SelectedIndex = 0 Then
                        Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam("p31_framework-groups-autoexpanded", "0"))
                    Else
                        chkGroupsAutoExpanded.Checked = True
                        chkGroupsAutoExpanded.Visible = False
                    End If
                    If .GetUserParam("p31_framework-sort-" & Me.GridPrefix) <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam("p31_framework-sort-" & Me.GridPrefix))
                    End If
                    If .GetUserParam("p31_framework-timer", "1") = "1" Then
                        rightPane.ContentUrl = "p31_framework_timer.aspx"
                    Else
                        rightPane.ContentUrl = ""
                        rightPane.Visible = False
                        RadSplitter1.Items.Remove(rightPane)
                    End If
                End With
            End With

            If Request.Item("search") <> "" Then
                txtSearch.Text = Request.Item("search")   'externě předaná podmínka
                txtSearch.Focus()
            End If

            If tabs1.SelectedIndex > 0 Then
                txtSearch.Visible = False : txtSearch.Text = "" : cmdSearch.Visible = False
            End If

            grid1.radGridOrig.MasterTableView.FilterExpression = Master.Factory.j03UserBL.GetUserParam("p31_framework-filter_sql_p41")
            RecalcVirtualRowCount()

            grid1.radGridOrig.MasterTableView.FilterExpression = Master.Factory.j03UserBL.GetUserParam("p31_framework-filter_sql_p56")
            RecalcTasksCount()

            SetupJ74Combo(BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam("p31_framework-j74id-" & Me.GridPrefix)))
            With Master.Factory.j03UserBL
                SetupGrid(.GetUserParam("p31_framework-filter_setting_" & Me.GridPrefix), .GetUserParam("p31_framework-filter_sql_" & Me.GridPrefix))
            End With



            Handle_DefaultSelectedRecord()


            If tabs1.SelectedIndex = 2 Then
                'úkoly
                cmdNewTask.Visible = True
            Else
                cmdNewTask.Visible = False
            End If
            
            
        End If
    End Sub

    Private Sub InitialGroupByCombo(intTabIndex As Integer)
        With Me.cbxGroupBy
            .Items.Clear()
            .Items.Add(New ListItem(BL.My.Resources.common.BezSouhrnu, ""))
            .Items.Add(New ListItem(BL.My.Resources.common.Klient, "Client"))
            If intTabIndex = 2 Then
                .Items.Add(New ListItem(BL.My.Resources.common.TypUkolu, "p57Name"))
                .Items.Add(New ListItem(BL.My.Resources.common.Projekt, "ProjectCodeAndName"))
                .Items.Add(New ListItem(BL.My.Resources.common.Prijemce, "ReceiversInLine"))
                .Items.Add(New ListItem(BL.My.Resources.common.Milnik, "o22Name"))
                .Items.Add(New ListItem(BL.My.Resources.common.VlastnikZaznamu, "Owner"))

            Else
                .Items.Add(New ListItem(BL.My.Resources.common.TypProjektu, "p42Name"))
                .Items.Add(New ListItem(BL.My.Resources.common.Stredisko, "j18Name"))
                
            End If
        End With
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.BAS.GetX29FromPrefix(Me.GridPrefix), Master.Factory.SysUser.PID, "p31_framework")
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo(cJ74.PID)
                End If
            End If
            Dim strAddSqlFrom As String = ""
            If tabs1.SelectedIndex = 0 Then
                Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, False, , strFilterSetting, strFilterExpression, , strAddSqlFrom, 16)
            Else
                Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 100, False, False, , strFilterSetting, strFilterExpression, , strAddSqlFrom, 16)
            End If
            hidFrom.Value = strAddSqlFrom

            If tabs1.SelectedIndex = 1 Or tabs1.SelectedIndex = 3 Then grid1.AllowFilteringByColumn = False 'v top10 a v oblíbených se nefiltruje
            Me.txtSearch.Visible = Not cJ74.j74IsFilteringByColumn
            cmdSearch.Visible = Me.txtSearch.Visible
        End With
        With grid1
            .radGridOrig.ShowFooter = False
            .radGridOrig.SelectedItemStyle.BackColor = Drawing.Color.Red
        End With
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
    End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
        
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As System.Data.DataRowView = CType(e.Item.DataItem, System.Data.DataRowView)
        

        If Me.GridPrefix = "p41" Then
            'If cRec.Item("IsClosed") Then dataItem.Font.Strikeout = True
            'dataItem("systemcolumn").Text = "<a title='Zapsat úkon' href='javascript:nw(" & cRec.Item("pid").ToString & ")'><img src='Images/new.png' border=0/></a>"
            basUIMT.p41_grid_Handle_ItemDataBound(sender, e, True, False)
            With dataItem("systemcolumn")
                .Text = "<a class='reczoom' title='Detail projektu' rel='clue_p41_myworksheet.aspx?parent_url_reload=p31_framework.aspx&pid=" & cRec.Item("pid").ToString & "&j02id=" & Me.CurrentJ02ID.ToString & "' style='margin-left:-10px;'>i</a>" & .Text
            End With
        Else
            With dataItem("systemcolumn")
                .Text = "<a class='reczoom' title='Detail úkolu' rel='clue_p56_record.aspx?&pid=" & cRec.Item("pid").ToString & "' style='margin-left:-10px;'>i</a>"
            End With
            If Not cRec.Item("p56PlanUntil_Grid") Is System.DBNull.Value Then
                If Now > cRec.Item("p56PlanUntil_Grid") Then dataItem.ForeColor = Drawing.Color.DarkRed
            End If
        End If



    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("p31_framework-filter_setting_" & Me.GridPrefix, grid1.GetFilterSetting())
                .SetUserParam("p31_framework-filter_sql_" & Me.GridPrefix, grid1.GetFilterExpression())
            End With
            Select Case tabs1.SelectedIndex
                Case 0
                    RecalcVirtualRowCount()
                Case 2
                    RecalcTasksCount()
            End Select
        End If
        If Me.GridPrefix = "p41" Then
            Dim mq As New BO.myQueryP41
            With mq
                If tabs1.SelectedIndex = 0 Then
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                Else
                    .TopRecordsOnly = 0
                    .MG_PageSize = 0
                    .MG_CurrentPageIndex = 0
                End If

            End With

            InhaleMyQuery(mq)
            If tabs1.SelectedIndex = 3 Then mq.IsFavourite = BO.BooleanQueryMode.TrueQuery

            Dim dt As DataTable = Master.Factory.p41ProjectBL.GetGridDataSource(mq)
            If dt Is Nothing Then
                Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
            Else
                If tabs1.SelectedIndex = 1 Then
                    dt = basUIMT.QueryProjectListByTop10(Master.Factory, Me.CurrentJ02ID, Me.hidCols.Value, Me.cbxGroupBy.SelectedValue)
                    'lis = basUIMT.QueryProjectListByTop10(Master.Factory, Me.CurrentJ02ID)    'omezit na TOP 10
                End If
                If tabs1.SelectedIndex = 3 And dt.Rows.Count = 0 Then
                    Master.Notify("Váš seznam oblíbených projektů je prázdný.")
                End If
                grid1.DataSourceDataTable = dt
            End If

        End If
        If Me.GridPrefix = "p56" Then
            Dim mq As New BO.myQueryP56
            InhaleMyTaskQuery(mq)
            grid1.DataSourceDataTable = Master.Factory.p56TaskBL.GetGridDataSource(mq)

        End If


    End Sub

    ''Private Function QueryProjectListByTop10(lis As IEnumerable(Of BO.p41Project)) As IEnumerable(Of BO.p41Project)
    ''    Dim mqP31 As New BO.myQueryP31
    ''    If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then
    ''        mqP31.j02ID = Me.CurrentJ02ID
    ''    Else
    ''        mqP31.j02ID = Master.Factory.SysUser.j02ID
    ''    End If
    ''    mqP31.TopRecordsOnly = 100
    ''    mqP31.MG_SortString = "p31dateinsert desc"
    ''    Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mqP31)
    ''    Dim p41ids As New List(Of Integer)
    ''    If lisP31.Count > 0 Then
    ''        If lisP31.Select(Function(p) p.p41ID).Distinct.Count > 10 Then
    ''            For Each c In lisP31
    ''                If p41ids.Where(Function(p) p = c.p41ID).Count = 0 Then
    ''                    p41ids.Add(c.p41ID)
    ''                End If
    ''                If p41ids.Count >= 10 Then Exit For
    ''            Next
    ''        Else
    ''            p41ids = lisP31.Select(Function(p) p.p41ID).Distinct.ToList
    ''        End If
    ''    Else
    ''        p41ids.Add(-1)
    ''    End If
    ''    Dim mqP41 As New BO.myQueryP41
    ''    mqP41.PIDs = p41ids
    ''    mqP41.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
    ''    Return Master.Factory.p41ProjectBL.GetList(mqP41)
    ''End Function

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP41)
        With mq
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_AdditionalSqlFROM = Me.hidFrom.Value

            .Closed = BO.BooleanQueryMode.NoQuery
            .SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
            If tabs1.SelectedIndex = 0 Then
                .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            End If

            If Me.txtSearch.Visible Then .SearchExpression = Trim(Me.txtSearch.Text)
            If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then .j02ID_ExplicitQueryFor = Me.CurrentJ02ID


        End With

    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_framework-pagesize-" & Me.GridPrefix, Me.cbxPaging.SelectedValue)

        ReloadPage()
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSearch.Click
        Handle_RunSearch()

    End Sub

    Private Sub Handle_RunSearch()
        Master.Factory.j03UserBL.SetUserParam("p31_framework-search", txtSearch.Text)

        RecalcVirtualRowCount()
        grid1.Rebind(False)

        txtSearch.Focus()
    End Sub



    Private Sub Handle_DefaultSelectedRecord()
        If Me.GridPrefix = "p56" Then Return

        Dim intSelPID As Integer = 0
        If Not Page.IsPostBack Then
            Dim strDefPID As String = Request.Item("p41id")

            If strDefPID > "" Then intSelPID = BO.BAS.IsNullInt(strDefPID)
        End If

        If intSelPID > 0 Then
            'označit výchozí projekt
            grid1.SelectRecords(intSelPID)
            If grid1.GetSelectedPIDs.Count = 0 Then
                'hledaný projekt nebyl nalezen na první stránce
                Dim mq As New BO.myQueryP41
                InhaleMyQuery(mq)
                mq.MG_SelectPidFieldOnly = True
                mq.TopRecordsOnly = 0
                Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq), x As Integer, intNewPageIndex As Integer = 0
                If lis Is Nothing Then
                    Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
                    Return
                End If
                For Each c In lis
                    x += 1
                    If x > grid1.PageSize Then
                        intNewPageIndex += 1 : x = 1
                    End If
                    If c.PID = intSelPID Then
                        grid1.radGridOrig.CurrentPageIndex = intNewPageIndex
                        grid1.Rebind(False)
                        grid1.SelectRecords(intSelPID)
                    End If
                Next

            End If

        End If
    End Sub

   


    Private Sub p31_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If txtSearch.Visible Then
            If Trim(txtSearch.Text) = "" Then
                txtSearch.Style.Item("background-color") = ""
            Else
                txtSearch.Style.Item("background-color") = "red"
            End If
        End If
        
        Select Case Me.tabs1.SelectedIndex
            Case 0, 1
                img1.ImageUrl = "Images/project_32.png"
                lblFormHeader.Text = Resources.p31_framework.tabs1_p41
            Case 2
                img1.ImageUrl = "Images/task_32.png"
                lblFormHeader.Text = Resources.p31_framework.tabs1_todo
        End Select

        If grid1.GetFilterExpression <> "" Then
            cmdCĺearFilter.Visible = True
        Else
            cmdCĺearFilter.Visible = False
        End If

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        

        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub

    Private Sub SetupJ74Combo(intDef As Integer)
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.BAS.GetX29FromPrefix(Me.GridPrefix)).Where(Function(p) p.j74MasterPrefix = "p31_framework")
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.BAS.GetX29FromPrefix(Me.GridPrefix), Master.Factory.SysUser.PID, "p31_framework")
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.BAS.GetX29FromPrefix(Me.GridPrefix)).Where(Function(p) p.j74MasterPrefix = "p31_framework")
        End If
        j74id.DataSource = lisJ74
        j74id.DataBind()

        If intDef > 0 Then
            basUI.SelectDropdownlistValue(Me.j74id, intDef.ToString)
        End If
        If Me.CurrentJ74ID > 0 Then
            _curJ74 = lisJ74.Where(Function(p) p.PID = Me.CurrentJ74ID)(0)
            If _curJ74.j74ColumnNames.IndexOf("ReceiversInLine") > 0 Then
                Me.IsUseReceiversInLine = True
            Else
                Me.IsUseReceiversInLine = False
            End If
            If _curJ74.j74ColumnNames.IndexOf("Hours_Orig") > 0 Or _curJ74.j74ColumnNames.IndexOf("Expenses_Orig") > 0 Then
                Me.IsUseTasksWorksheetColumns = True
            Else
                Me.IsUseTasksWorksheetColumns = False
            End If
        End If


    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
            .GroupsDefaultExpanded = chkGroupsAutoExpanded.Checked
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strFieldHeader

            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)

        End With
    End Sub
    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_framework-groupby-" & Me.GridPrefix, Me.cbxGroupBy.SelectedValue)
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)
    End Sub
    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        SaveLastJ74Reference()
        ReloadPage()
    End Sub

    Private Sub SaveLastJ74Reference()
        With Master.Factory.j03UserBL
            .SetUserParam("p31_framework-j74id-" & Me.GridPrefix, Me.CurrentJ74ID.ToString)
            .SetUserParam("p31_framework-sort-" & Me.GridPrefix, "")
            .SetUserParam("p31_framework-filter_setting_" & Me.GridPrefix, "")
            .SetUserParam("p31_framework-filter_sql_" & Me.GridPrefix, "")
        End With
        
    End Sub
    Private Sub ReloadPage()
        Response.Redirect("p31_framework.aspx")
    End Sub

    Private Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryP41
        InhaleMyQuery(mq)
        grid1.VirtualRowCount = Master.Factory.p41ProjectBL.GetVirtualCount(mq)

        grid1.radGridOrig.CurrentPageIndex = 0
        With tabs1.Tabs(0)
            .Text = BO.BAS.OM2(.Text, grid1.VirtualRowCount.ToString)
        End With
        'Me.lblFormHeader.Text = "Worksheet (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
    End Sub
    Private Sub RecalcTasksCount()
        With tabs1.Tabs(2)
            .Text = BO.BAS.OM2(.Text, GetTasksCount().ToString)
        End With
    End Sub


   

    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_framework-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)


    End Sub

    Private Sub tabs1_TabClick(sender As Object, e As RadTabStripEventArgs) Handles tabs1.TabClick
        Master.Factory.j03UserBL.SetUserParam("p31_framework-tabindex", tabs1.SelectedIndex.ToString)
        ReloadPage()
    End Sub

    
    Private Function GetTasksCount() As Integer
        Dim mq As New BO.myQueryP56
        InhaleMyTaskQuery(mq)
        Return Master.Factory.p56TaskBL.GetVirtualCount(mq)
    End Function
    Private Function GetTasksList() As IEnumerable(Of BO.p56Task)
        Dim mq As New BO.myQueryP56
        InhaleMyTaskQuery(mq)

        Return Master.Factory.p56TaskBL.GetList(mq, Me.IsUseReceiversInLine)
    End Function
    Private Function GetTasksListWithWorksheetSum() As IEnumerable(Of BO.p56TaskWithWorksheetSum)
        Dim mq As New BO.myQueryP56
        InhaleMyTaskQuery(mq)

        Return Master.Factory.p56TaskBL.GetList_WithWorksheetSum(mq, Me.IsUseReceiversInLine)
    End Function

    Private Sub InhaleMyTaskQuery(ByRef mq As BO.myQueryP56)
        With mq
            .j02ID = Me.CurrentJ02ID
            .SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForRead
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            .Closed = BO.BooleanQueryMode.FalseQuery
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            If Me.txtSearch.Visible Then .SearchExpression = Trim(Me.txtSearch.Text)
        End With
        

    End Sub

    ''Private Sub rpP56_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP56.ItemDataBound
    ''    Dim cRec As BO.p56Task = CType(e.Item.DataItem, BO.p56Task)
    ''    With CType(e.Item.FindControl("TodoHeader"), Label)
    ''        .Text = cRec.p57Name & ": <i><b>" & cRec.p56Name & "</b></i>"
    ''    End With
    ''    With CType(e.Item.FindControl("cmdP31"), HyperLink)
    ''        .NavigateUrl = "javascript:nw_p56(" & cRec.PID.ToString & ")"
    ''    End With
    ''    With CType(e.Item.FindControl("p56PlanUntil"), Label)
    ''        If Not cRec.p56PlanUntil Is Nothing Then
    ''            .Text = BO.BAS.FD(cRec.p56PlanUntil, True, True)
    ''            If cRec.p56PlanUntil < Now Then
    ''                .ForeColor = Drawing.Color.Red
    ''            End If
    ''        End If
    ''    End With
    ''    With CType(e.Item.FindControl("cmdEdit"), HyperLink)
    ''        .NavigateUrl = "javascript:p56_edit(" & cRec.PID.ToString & ")"
    ''    End With
    ''    With CType(e.Item.FindControl("Project"), Label)
    ''        If cRec.p41ID = _lastP41ID Then
    ''            e.Item.FindControl("panProject").Visible = False
    ''        Else
    ''            If cRec.Client = "" Then
    ''                .Text = cRec.p41Name
    ''            Else
    ''                .Text = cRec.Client & " - " & cRec.p41Name
    ''            End If
    ''        End If
    ''    End With
    ''    With CType(e.Item.FindControl("clue1"), HyperLink)
    ''        .Attributes("rel") = "clue_p56_record.aspx?mode=readonly&pid=" & cRec.PID.ToString
    ''    End With
    ''    _lastP41ID = cRec.p41ID
    ''End Sub

    Private Sub cmdCĺearFilter_Click(sender As Object, e As EventArgs) Handles cmdCĺearFilter.Click
        With Master.Factory.j03UserBL
            .SetUserParam("p31_framework-filter_setting_" & Me.GridPrefix, "")
            .SetUserParam("p31_framework-filter_sql_" & Me.GridPrefix, "")
        End With
        ReloadPage()
    End Sub

    Private Sub grid1_SortCommand(SortExpression As String, strOwnerTableName As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam("p31_framework-sort-" & Me.GridPrefix, SortExpression)
    End Sub
End Class