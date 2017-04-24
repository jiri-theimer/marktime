Imports Telerik.Web.UI

Public Class p31_grid
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private Property _curJ62 As BO.j62MenuHome
    Private Property _needFilterIsChanged As Boolean = False
    Private Property _curIsExport As Boolean

    Private Sub p31_grid_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_grid"
    End Sub
    Public Property CurrentJ70ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j70ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j70ID, value.ToString)
        End Set
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            hidMasterPID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentJ62ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidJ62ID.Value)
        End Get
        Set(value As Integer)
            hidJ62ID.Value = value.ToString
            Master.SiteMenuValue = "hm" & value.ToString
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Item("masterpid") <> "" Then
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid")) : Me.CurrentMasterPrefix = Request.Item("masterprefix")
            End If
            If Request.Item("aw") <> "" Then
                Me.hidMasterAW.Value = Replace(Server.UrlDecode(Request.Item("aw")), "xxx", "=")
            End If
            Me.hidMasterTabAutoQueryFlag.Value = Request.Item("p31tabautoquery")
            With Master
                .PageTitle = "WORKSHEET datový přehled"

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_grid-pagesize")
                    .Add("p31_grid-query-p34id")
                    .Add("p31_grid-j74id")
                    .Add("p31-j70id")
                    .Add("p31_grid-period")
                    .Add("periodcombo-custom_query")
                    .Add("p31_grid-groupby")
                    .Add("p31_grid-groups-autoexpanded")
                    .Add("p31_grid-search")
                    .Add("p31_grid-sort")
                    .Add("p31_grid-filter_setting")
                    .Add("p31_grid-filter_sql")
                End With
                cbxGroupBy.DataSource = .Factory.j74SavedGridColTemplateBL.GroupByPallet(BO.x29IdEnum.p31Worksheet)
                cbxGroupBy.DataBind()

                .Factory.j03UserBL.InhaleUserParams(lisPars)

                basUI.SelectDropdownlistValue(cbxPaging, .Factory.j03UserBL.GetUserParam("p31_grid-pagesize", "20"))
                period1.SetupData(Master.Factory, .Factory.j03UserBL.GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .Factory.j03UserBL.GetUserParam("p31_grid-period")
                Me.txtSearch.Text = .Factory.j03UserBL.GetUserParam("p31_grid-search")
                basUI.SelectDropdownlistValue(Me.cbxGroupBy, .Factory.j03UserBL.GetUserParam("p31_grid-groupby"))
                ''If .Factory.j03UserBL.GetUserParam("p31_grid-sort") <> "" Then
                ''    grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.Factory.j03UserBL.GetUserParam("p31_grid-sort"))
                ''End If
            End With

            Me.CurrentJ62ID = BO.BAS.IsNullInt(Request.Item("j62id"))
            If Me.CurrentJ62ID <> 0 Then
                _curJ62 = Master.Factory.j62MenuHomeBL.Load(Me.CurrentJ62ID)
                If _curJ62 Is Nothing Then Master.StopPage("j62 record not found")
            Else
                Master.SiteMenuValue = "p31_grid"
            End If

            With Master.Factory.j03UserBL
                Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam("p31_grid-groups-autoexpanded", "1"))
                SetupJ70Combo(BO.BAS.IsNullInt(.GetUserParam("p31-j70id")))
                Dim intJ74ID As Integer = BO.BAS.IsNullInt(.GetUserParam("p31_grid-j74id"))
                If intJ74ID = 0 Then
                    If Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p31_grid") Then
                        _curJ74 = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p31_grid")
                        .SetUserParam("p31_grid-j74id", _curJ74.PID)
                    End If
                End If
                SetupJ74Combo(intJ74ID)
                SetupGrid(.GetUserParam("p31_grid-filter_setting"), .GetUserParam("p31_grid-filter_sql"), .GetUserParam("p31_grid-sort"))

            End With
            

            RecalcVirtualRowCount()
            Handle_Permissions()
            If Me.CurrentMasterPrefix <> "" Then
                With Me.lblFormHeader
                    .CssClass = ""
                    .Text = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix), Me.CurrentMasterPID)
                    If .Text.Length > 30 Then .Text = Left(.Text, 28) & "..."
                    .Text = "<a href='" & Me.CurrentMasterPrefix & "_framework.aspx?pid=" & Me.CurrentMasterPID.ToString & "'>" & .Text & "</a>"
                End With
            End If
        End If
    End Sub
    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Pojmenovaný filtr--")
        If Not _curJ62 Is Nothing Then
            If _curJ62.j70ID <> 0 Then
                intDef = _curJ62.j70ID
                If Me.j70ID.Items.FindByValue(intDef.ToString) Is Nothing Then
                    Dim c As BO.j70QueryTemplate = Master.Factory.j70QueryTemplateBL.Load(intDef)
                    Me.j70ID.Items.Add(New ListItem(c.j70Name, intDef.ToString))
                End If
            End If
        End If
        basUI.SelectDropdownlistValue(Me.j70ID, intDef.ToString)
        With Me.j70ID
            If .SelectedIndex > 0 Then
                .ToolTip = .SelectedItem.Text
                Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & .SelectedValue
            Else
                Me.clue_query.Visible = False
            End If
        End With
    End Sub
    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property

    Private Sub SetupJ74Combo(intDef As Integer)
        If Not _curJ62 Is Nothing Then
            If _curJ62.j74ID <> 0 Then
                intDef = _curJ62.j74ID
                _curJ74 = Master.Factory.j74SavedGridColTemplateBL.Load(intDef)
                j74id.Items.Clear()
                Me.j74id.Items.Add(New ListItem(_curJ74.j74Name, intDef.ToString))
                Me.j74id.Enabled = False
                If _curJ62.j62GridGroupBy <> "" Then basUI.SelectDropdownlistValue(Me.cbxGroupBy, _curJ62.j62GridGroupBy)
                Return
            End If
        End If

        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet).Where(Function(p) p.j74MasterPrefix = "" Or p.j74MasterPrefix = "p31_grid")
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p31_grid")
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet).Where(Function(p) p.j74MasterPrefix = "" Or p.j74MasterPrefix = "p31_grid")
        End If
        j74id.DataSource = lisJ74
        j74id.DataBind()

        If intDef > 0 Then
            basUI.SelectDropdownlistValue(Me.j74id, intDef.ToString)
        End If
        If Me.CurrentJ74ID > 0 Then
            _curJ74 = lisJ74.Where(Function(p) p.PID = Me.CurrentJ74ID)(0)
        End If


    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click

        Select Case Me.hidHardRefreshFlag.Value
            Case "j74"
                If Me.hidHardRefreshPID.Value <> "" Then
                    Me.CurrentJ74ID = BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value)
                    SaveLastJ74Reference()
                End If
                ReloadPage()
            Case "quickquery"
                grid1.Rebind(False)
            Case "p31-save"
                grid1.Rebind(True)

            Case "p31-delete"

                ReloadPage()
            Case Else
                ReloadPage()
        End Select

        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""


    End Sub

    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        SaveLastJ74Reference()
        ReloadPage()
    End Sub

    Private Sub SaveLastJ74Reference()
        With Master.Factory.j03UserBL
            .SetUserParam("p31_grid-j74id", Me.CurrentJ74ID.ToString)
            .SetUserParam("p31_grid-sort", "")
            .SetUserParam("p31_grid-filter_setting", "")
            .SetUserParam("p31_grid-filter_sql", "")
        End With
        
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-pagesize", Me.cbxPaging.SelectedValue)

        ReloadPage()
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String, strSortExpression As String)
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p31_grid")
                If Not cJ74 Is Nothing Then SetupJ74Combo(cJ74.PID)
            End If
            Me.hidDefaultSorting.Value = cJ74.j74OrderBy : Me.hidDrillDownField.Value = cJ74.j74DrillDownField1
            Dim strAddSqlFrom As String = "", strSqlSumCols As String = ""
            Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, Not _curIsExport, True, strFilterSetting, strFilterExpression, strSortExpression, strAddSqlFrom, , strSqlSumCols)
            Me.hidFrom.Value = strAddSqlFrom
            Me.hidSumCols.Value = strSqlSumCols


            Me.txtSearch.Visible = Not cJ74.j74IsFilteringByColumn
            cmdSearch.Visible = Me.txtSearch.Visible
            If Not Me.txtSearch.Visible Then Me.txtSearch.Text = ""
            If cJ74.j74DrillDownField1 <> "" Then Me.panGroupBy.Visible = False : Me.cbxGroupBy.SelectedIndex = 0 'v drill-down se souhrny nepoužívají
        End With
        

        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
    End Sub
    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        grid1.radGridOrig.GroupingSettings.RetainGroupFootersVisibility = True
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

    Private Sub grid1_DataBinding(sender As Object, e As EventArgs) Handles grid1.DataBinding

    End Sub

    ''Private Sub grid1_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs) Handles grid1.DetailTableDataBind
    ''    Dim dataItem As GridDataItem = DirectCast(e.DetailTableView.ParentItem, GridDataItem)
    ''    Dim mq As New BO.myQueryP31
    ''    Dim colDrill As BO.GridGroupByColumn = Master.Factory.j74SavedGridColTemplateBL.GroupByPallet(BO.x29IdEnum.p31Worksheet).Where(Function(p) p.ColumnField = Me.hidDrillDownField.Value).First
    ''    Select Case LCase(colDrill.LinqQueryField)
    ''        Case "p71id"
    ''            mq.p71ID = DirectCast(BO.BAS.IsNullInt(dataItem.GetDataKeyValue("pid")), BO.p71IdENUM)
    ''        Case "p70id"
    ''            mq.p70ID = DirectCast(BO.BAS.IsNullInt(dataItem.GetDataKeyValue("pid")), BO.p70IdENUM)
    ''        Case Else
    ''            BO.BAS.SetPropertyValue(mq, colDrill.LinqQueryField, BO.BAS.IsNullInt(dataItem.GetDataKeyValue("pid")))
    ''    End Select


    ''    With mq
    ''        .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
    ''        .MG_CurrentPageIndex = e.DetailTableView.CurrentPageIndex
    ''        .MG_SortString = e.DetailTableView.SortExpressions.GetSortString()
    ''        If Me.hidDefaultSorting.Value <> "" Then
    ''            If .MG_SortString = "" Then
    ''                .MG_SortString = Me.hidDefaultSorting.Value
    ''            Else
    ''                .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
    ''            End If
    ''        End If
    ''        If Me.cbxGroupBy.SelectedValue <> "" Then
    ''            Dim strPrimarySortField As String = Me.cbxGroupBy.SelectedValue
    ''            If strPrimarySortField = "SupplierName" Then strPrimarySortField = "supplier.p28Name"
    ''            If strPrimarySortField = "ClientName" Then strPrimarySortField = "p28client.p28Name"
    ''            If strPrimarySortField = "Person" Then strPrimarySortField = "j02.j02LastName+char(32)+j02.j02Firstname"

    ''            If .MG_SortString = "" Then
    ''                .MG_SortString = strPrimarySortField
    ''            Else
    ''                .MG_SortString = strPrimarySortField & "," & .MG_SortString
    ''            End If
    ''        End If
    ''    End With
    ''    InhaleMyQuery(mq)
    ''    With e.DetailTableView
    ''        .AllowCustomPaging = True
    ''        .AllowSorting = True
    ''        If .VirtualItemCount = 0 Then .VirtualItemCount = GetRowsCount(mq)
    ''        .DataSource = Master.Factory.p31WorksheetBL.GetList(mq)

    ''    End With
    ''End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e, True)
        If _curIsExport Then
            If TypeOf e.Item Is GridHeaderItem Then
                e.Item.BackColor = Drawing.Color.Silver
            End If
            If TypeOf e.Item Is GridGroupHeaderItem Then
                e.Item.BackColor = Drawing.Color.WhiteSmoke
            End If
            If TypeOf e.Item Is GridDataItem Or TypeOf e.Item Is GridHeaderItem Then
                e.Item.Cells(0).Visible = False
            End If
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If e.IsFromDetailTable Then
            Return
        End If
        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
        End With
        InhaleMyQuery(mq)

        If Me.hidDrillDownField.Value <> "" Then
            'drill down úroveň
            Dim colDrill As BO.GridGroupByColumn = Master.Factory.j74SavedGridColTemplateBL.GroupByPallet(BO.x29IdEnum.p31Worksheet).Where(Function(p) p.ColumnField = Me.hidDrillDownField.Value).First

            Dim dtDD As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownDataTable(colDrill, mq, grid1.radGridOrig.MasterTableView.Attributes("sumfields"))
            grid1.VirtualRowCount = dtDD.Rows.Count
            grid1.DataSourceDataTable = dtDD
            Return
        End If

        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("p31_grid-filter_setting", grid1.GetFilterSetting())
                .SetUserParam("p31_grid-filter_sql", grid1.GetFilterExpression())
                .SetUserParam("p31_grid-filter_completesql", grid1.GetFilterExpressionCompleteSql())
            End With
            RecalcVirtualRowCount()
        End If
        If _curIsExport Then mq.MG_PageSize = 2000
        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetGridDataSource(mq)
        If dt Is Nothing Then
            Master.Notify(Master.Factory.p31WorksheetBL.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            grid1.DataSourceDataTable = dt
        End If

        

        ''grid1.DataSource = Master.Factory.p31WorksheetBL.GetList(mq)

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            Select Case Me.CurrentMasterPrefix
                Case "p41"
                    .p41ID = Me.CurrentMasterPID
                Case "p28"
                    .p28ID_Client = Me.CurrentMasterPID
                Case "j02"
                    .j02ID = Me.CurrentMasterPID
                Case "p56"
                    .p56IDs = New List(Of Integer)
                    .p56IDs.Add(Me.CurrentMasterPID)
                Case "p91"
                    .p91ID = Me.CurrentMasterPID
                Case Else

            End Select
            .j70ID = Me.CurrentJ70ID
            .SearchExpression = Trim(Me.txtSearch.Text)
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()

            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            If period1.SelectedValue <> "" Then
                .DateFrom = period1.DateFrom
                .DateUntil = period1.DateUntil
            End If
            .MG_AdditionalSqlWHERE = Me.hidMasterAW.Value
            .TabAutoQuery = Me.hidMasterTabAutoQueryFlag.Value
        End With
    End Sub

    
    Private Sub RecalcVirtualRowCount()
        If Me.hidDrillDownField.Value <> "" Then Return 'pro drill-down nepočítat
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetGridFooterSums(mq, Me.hidSumCols.Value)
        grid1.VirtualRowCount = dt.Rows(0).Item(0)
        Me.hidFooterString.Value = grid1.CompleteFooterString(dt, Me.hidSumCols.Value)

      
        grid1.radGridOrig.CurrentPageIndex = 0
        If grid1.VirtualRowCount > 100000 Then
            Me.lblFormHeader.Text = " (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
        Else
            Me.lblFormHeader.Text = "Worksheet (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
        End If

    End Sub



    Private Sub ReloadPage()
        Dim s As String = ""
        If Me.CurrentMasterPID <> 0 Then
            s += "masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString
        End If
        If Me.CurrentJ62ID > 0 Then
            If s = "" Then
                s = "j62id=" & Me.CurrentJ62ID.ToString
            Else
                s += "&j62id=" & Me.CurrentJ62ID.ToString
            End If
        End If
        Response.Redirect(basUI.AddQuerystring2Page("p31_grid.aspx", s))
    End Sub

    Private Sub p31_grid_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
        If Trim(txtSearch.Text) = "" Then
            txtSearch.Style.Item("background-color") = ""
        Else
            txtSearch.Style.Item("background-color") = "red"
        End If
        If grid1.GetFilterExpression <> "" Then
            cmdCĺearFilter.Visible = True
        Else
            cmdCĺearFilter.Visible = False
        End If

        basUIMT.RenderQueryCombo(Me.j70ID)

        If cbxGroupBy.SelectedValue <> "" Then chkGroupsAutoExpanded.Visible = True Else chkGroupsAutoExpanded.Visible = False
    End Sub



    Private Sub grid1_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"


        grid1.ParseFooterItemString(footerItem, Me.hidFooterString.Value)

    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        Dim cXLS As New clsExportToXls(Master.Factory)

        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)
        mq.MG_GridGroupByField = ""
        ''Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetGridDataSource(mq)

        Dim strFileName As String = cXLS.ExportGridData(dt.AsEnumerable, cJ74)
        If strFileName = "" Then
            Master.Notify(cXLS.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            Response.Redirect("binaryfile.aspx?tempfile=" & strFileName)
        End If
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub Handle_Permissions()
        With Master.Factory
            menu1.FindItemByValue("action").Visible = .SysUser.IsApprovingPerson
            panExport.Visible = .TestPermission(BO.x53PermValEnum.GR_GridTools)
            cmdGridDesigner.Visible = panExport.Visible
            cmdQuery.Visible = panExport.Visible
        End With
        If Not cmdGridDesigner.Visible And j74id.Items.Count <= 1 Then Me.j74id.Visible = False

    End Sub

    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Me.CurrentJ62ID = 0
        Master.Factory.j03UserBL.SetUserParam("p31_grid-groupby", Me.cbxGroupBy.SelectedValue)
        ReloadPage()

        ''With Me.cbxGroupBy.SelectedItem
        ''    SetupGrouping(.Value, .Text)
        ''End With
        ''grid1.Rebind(True)
    End Sub
    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Me.CurrentJ62ID = 0
        Master.Factory.j03UserBL.SetUserParam("p31-j70id", Me.CurrentJ70ID.ToString)
        ReloadPage()
    End Sub
  
    Private Sub Handle_RunSearch()
        Master.Factory.j03UserBL.SetUserParam("p31_grid-search", Trim(txtSearch.Text))

        grid1.Rebind(False)

        txtSearch.Focus()
    End Sub

    
    Private Sub cmdSearch_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSearch.Click
        Handle_RunSearch()
    End Sub

    

    Private Sub cmdCĺearFilter_Click(sender As Object, e As EventArgs) Handles cmdCĺearFilter.Click
        With Master.Factory.j03UserBL
            .SetUserParam("p31_grid-filter_setting", "")
            .SetUserParam("p31_grid-filter_sql", "")
            .SetUserParam("p31_grid-filter_completesql", "")
        End With
        ReloadPage()
    End Sub
    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        ReloadPage()
    End Sub

    Private Sub grid1_SortCommand(SortExpression As String, strOwnerTableName As String) Handles grid1.SortCommand
        If strOwnerTableName = "drilldown" Then Return 'neukládat třídění z drill-down
        Master.Factory.j03UserBL.SetUserParam("p31_grid-sort", SortExpression)
    End Sub

    Private Sub GridExport(strFormat As String)
        _curIsExport = True
        basUIMT.Handle_GridTelerikExport(grid1, strFormat)


    End Sub

    Private Sub cmdPDF_Click(sender As Object, e As EventArgs) Handles cmdPDF.Click
        GridExport("pdf")
    End Sub

    Private Sub cmdXLS_Click(sender As Object, e As EventArgs) Handles cmdXLS.Click
        GridExport("xls")
    End Sub

    Private Sub cmdDOC_Click(sender As Object, e As EventArgs) Handles cmdDOC.Click
        GridExport("doc")
    End Sub
End Class