Imports Telerik.Web.UI

Public Class p31_grid
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private Property _needFilterIsChanged As Boolean = False

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Item("masterpid") <> "" Then
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid")) : Me.CurrentMasterPrefix = Request.Item("masterprefix")
            End If
            With Master
                .PageTitle = "Worksheet datový přehled"
                .SiteMenuValue = "cmdP31_Grid"

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
                If .Factory.j03UserBL.GetUserParam("p31_grid-sort") <> "" Then
                    grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.Factory.j03UserBL.GetUserParam("p31_grid-sort"))
                End If
            End With

            With Master.Factory.j03UserBL
                Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam("p31_grid-groups-autoexpanded", "0"))
                SetupJ70Combo(BO.BAS.IsNullInt(.GetUserParam("p31-j70id")))
                SetupJ74Combo(BO.BAS.IsNullInt(.GetUserParam("p31_grid-j74id")))
                SetupGrid(.GetUserParam("p31_grid-filter_setting"), .GetUserParam("p31_grid-filter_sql"))

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
        j70ID.Items.Insert(0, "--Bez filtrování--")
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

        grid1.PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
        If grid1.radGridOrig.CurrentPageIndex > 0 Then grid1.radGridOrig.CurrentPageIndex = 0
        grid1.Rebind(True)
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p31_grid")
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo(cJ74.PID)
                End If
            End If
            Me.hidDefaultSorting.Value = cJ74.j74OrderBy
            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, True, , strFilterSetting, strFilterExpression)
            Me.txtSearch.Visible = Not cJ74.j74IsFilteringByColumn
            cmdSearch.Visible = Me.txtSearch.Visible
            If Not Me.txtSearch.Visible Then Me.txtSearch.Text = ""
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

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("p31_grid-filter_setting", grid1.GetFilterSetting())
                .SetUserParam("p31_grid-filter_sql", grid1.GetFilterExpression())
            End With
            RecalcVirtualRowCount()
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
            If Me.cbxGroupBy.SelectedValue <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.cbxGroupBy.SelectedValue
                Else
                    .MG_SortString = Me.cbxGroupBy.SelectedValue & "," & .MG_SortString
                End If
            End If
        End With
        InhaleMyQuery(mq)

        grid1.DataSource = Master.Factory.p31WorksheetBL.GetList(mq)
        'Master.Notify(grid1.GetFilterExpression())

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            Select Case Me.CurrentMasterPrefix
                Case "p41"
                    .p41ID = Me.CurrentMasterPID
                Case "p28"
                    .p28ID = Me.CurrentMasterPID
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
            .ColumnFilteringExpression = grid1.GetFilterExpression()
            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            If period1.SelectedValue <> "" Then
                .DateFrom = period1.DateFrom
                .DateUntil = period1.DateUntil
            End If
        End With
    End Sub

    Private Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)


        Dim cSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, False, False)
        If Not cSum Is Nothing Then
            grid1.VirtualRowCount = cSum.RowsCount
            
            ViewState("footersum") = grid1.GenerateFooterItemString(cSum)
        Else
            ViewState("footersum") = ""
            grid1.VirtualRowCount = 0
        End If
        
        'grid1.VirtualRowCount = Master.Factory.p31WorksheetBL.GetVirtualCount(mq)
        grid1.radGridOrig.CurrentPageIndex = 0
        If grid1.VirtualRowCount > 100000 Then
            Me.lblFormHeader.Text = " (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
        Else
            Me.lblFormHeader.Text = "Worksheet (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
        End If

    End Sub



    Private Sub ReloadPage()
        If Me.CurrentMasterPID = 0 Then
            Response.Redirect("p31_grid.aspx")
        Else
            Response.Redirect("p31_grid.aspx?masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString)
        End If

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


    End Sub



    Private Sub grid1_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"


        grid1.ParseFooterItemString(footerItem, ViewState("footersum"))

    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        Dim cXLS As New clsExportToXls(Master.Factory)

        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)

        Dim strFileName As String = cXLS.ExportGridData(lis, cJ74)
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
        With Master.Factory.SysUser
            cmdApprove.Visible = .IsApprovingPerson
        End With


    End Sub

    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-groupby", Me.cbxGroupBy.SelectedValue)
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)
    End Sub
    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
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

    Private Sub grid1_SortCommand(SortExpression As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam("p31_grid-sort", SortExpression)
    End Sub

    Private Sub cmdCĺearFilter_Click(sender As Object, e As EventArgs) Handles cmdCĺearFilter.Click
        With Master.Factory.j03UserBL
            .SetUserParam("p31_grid-filter_setting", "")
            .SetUserParam("p31_grid-filter_sql", "")
        End With
        ReloadPage()
    End Sub
    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        ReloadPage()
    End Sub
End Class