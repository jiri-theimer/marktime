Imports Telerik.Web.UI
Public Class p31_subgrid
    Inherits System.Web.UI.UserControl

    Public Property Factory As BL.Factory           'proměnná nedrží stav!
    Public Property ExplicitMyQuery As BO.myQueryP31    'proměnná nedrží stav!
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Public Property MasterDataPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterDataPID.Value)
        End Get
        Set(value As Integer)
            Me.hidMasterDataPID.Value = value.ToString
        End Set
    End Property
    ''Private Property CurrentQuickQuery As BO.myQueryP31_QuickQuery
    ''    Get
    ''        Return CType(BO.BAS.IsNullInt(Me.hidQuickQuery.Value), BO.myQueryP31_QuickQuery)
    ''    End Get
    ''    Set(value As BO.myQueryP31_QuickQuery)
    ''        Me.hidQuickQuery.Value = CInt(value).ToString
    ''    End Set
    ''End Property

    Public Property CurrentJ70ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j70ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j70ID, value.ToString)
        End Set
    End Property
    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidJ74ID.Value)
        End Get
        Set(value As Integer)
            Me.hidJ74ID.Value = value.ToString
            If Me.j74id.Items.Count > 0 Then basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property
    Public Property j74RecordState As BO.p31RecordState
        Get
            Return CType(BO.BAS.IsNullInt(Me.hidJ74RecordState.Value), BO.p31RecordState)
        End Get
        Set(value As BO.p31RecordState)
            Me.hidJ74RecordState.Value = CInt(value).ToString
        End Set
    End Property

    Public Property ExplicitDateFrom As Date
        Get
            Return BO.BAS.ConvertString2Date(Me.hidExplicitDateFrom.Value)
        End Get
        Set(value As Date)
            Me.hidExplicitDateFrom.Value = Format(value, "dd.MM.yyyy")

        End Set
    End Property
    Public Property ExplicitDateUntil As Date
        Get
            Return BO.BAS.ConvertString2Date(Me.hidExplicitDateUntil.Value)
        End Get
        Set(value As Date)
            Me.hidExplicitDateUntil.Value = Format(value, "dd.MM.yyyy")

        End Set
    End Property
    Public Property HeaderText As String
        Get
            Return Me.lblHeaderP31.Text
        End Get
        Set(value As String)
            Me.lblHeaderP31.Text = value
        End Set
    End Property
    Public Property OnRowSelected As String
        Get
            Return gridP31.OnRowSelected
        End Get
        Set(value As String)
            gridP31.OnRowSelected = value
        End Set
    End Property
    Public Property OnRowDblClick As String
        Get
            Return gridP31.OnRowDblClick
        End Get
        Set(value As String)
            gridP31.OnRowDblClick = value
        End Set
    End Property

    Public Property AllowMultiSelect As Boolean
        Get
            Return gridP31.AllowMultiSelect
        End Get
        Set(value As Boolean)
            gridP31.AllowMultiSelect = value
        End Set
    End Property
    Public Property AllowApproving As Boolean
        Get
            Return cmdApprove.Visible
        End Get
        Set(value As Boolean)
            cmdApprove.Visible = value
            imgApprove.Visible = value
        End Set
    End Property

    Public Property EntityX29ID As BO.x29IdEnum
        Get
            If Me.hidX29ID.Value <> "" Then
                Return CInt(Me.hidX29ID.Value)
            Else
                Return BO.x29IdEnum._NotSpecified
            End If
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidX29ID.Value = CInt(value).ToString
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.MasterDataPID = 0 Then Return

        If Not Page.IsPostBack Then
            With Factory.j03UserBL
                Dim lisPars As New List(Of String), strKey As String = "p31_subgrid-j74id_" & BO.BAS.GetDataPrefix(Me.EntityX29ID)
                If Me.hidJ74RecordState.Value <> "" Then strKey += "-" & Me.hidJ74RecordState.Value
                With lisPars
                    .Add("periodcombo-custom_query")
                    .Add("p31_grid-period")
                    .Add("p31_subgrid-j70id")
                    .Add("p31_subgrid-pagesize")
                    .Add("p31_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.EntityX29ID))
                    .Add("p31_subgrid-search")
                    .Add("p31_subgrid-groups-autoexpanded")
                    .Add(strKey)
                End With
                .InhaleUserParams(lisPars)
                Me.CurrentJ74ID = CInt(.GetUserParam(strKey, "0"))

                If Me.CurrentJ74ID = 0 Then
                    Me.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p31Worksheet, Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.EntityX29ID), Me.j74RecordState)
                    Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.EntityX29ID), Me.j74RecordState)
                    _curJ74 = cJ74
                    .SetUserParam(strKey, cJ74.PID.ToString)
                    Me.hidDrillDownField.Value = _curJ74.j74DrillDownField1
                End If
                SetupJ74Combo()
                period1.SetupData(Me.Factory, .GetUserParam("periodcombo-custom_query"))

                period1.SelectedValue = .GetUserParam("p31_grid-period")
               
                basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("p31_subgrid-pagesize", "10"))
                basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("p31_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.EntityX29ID)))
                Me.txtSearch.Text = .GetUserParam("p31_subgrid-search")
                Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam("p31_subgrid-groups-autoexpanded", "1"))
            End With

            SetupJ70Combo(BO.BAS.IsNullInt(Factory.j03UserBL.GetUserParam("p31_subgrid-j70id")))
            SetupP31Grid()
            RecalcVirtualRowCount()
        End If

    End Sub
    Private Sub SetupJ74Combo()
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet).Where(Function(p) p.j74MasterPrefix = BO.BAS.GetDataPrefix(Me.EntityX29ID) And p.j74RecordState = Me.j74RecordState)
        j74id.DataSource = lisJ74
        j74id.DataBind()

        If Me.CurrentJ74ID > 0 Then
            basUI.SelectDropdownlistValue(Me.j74id, Me.CurrentJ74ID.ToString)
        End If

    End Sub
    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Bez filtrování--")
        basUI.SelectDropdownlistValue(Me.j70ID, intDef.ToString)
        
    End Sub

    Private Sub SetupP31Grid()
        If _curJ74 Is Nothing Then _curJ74 = Me.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        If _curJ74 Is Nothing Then
            _curJ74 = Me.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Me.Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.EntityX29ID))
            Me.CurrentJ74ID = _curJ74.PID
        End If
        hidDrillDownField.Value = _curJ74.j74DrillDownField1
        gridP31.ClearColumns()

        Me.hidDefaultSorting.Value = _curJ74.j74OrderBy
        basUIMT.SetupGrid(Me.Factory, Me.gridP31, _curJ74, CInt(Me.cbxPaging.SelectedValue), True, Me.AllowMultiSelect, Me.AllowMultiSelect)

        If _curJ74.j74IsFilteringByColumn Then
            Me.txtSearch.Visible = False : cmdSearch.Visible = False : txtSearch.Text = ""
        End If
       
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With

    End Sub
    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        gridP31.radGridOrig.GroupingSettings.RetainGroupFootersVisibility = True
        With gridP31.radGridOrig.MasterTableView
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

    Private Sub gridP31_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles gridP31.ItemDataBound
        If TypeOf e.Item.DataItem Is DataRowView Then Return
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)
    End Sub

    Private Sub gridP31_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gridP31.NeedDataSource
        If Me.MasterDataPID = 0 Or Me.EntityX29ID = BO.x29IdEnum._NotSpecified Then Return
        If e.IsFromDetailTable Then
            Return
        End If

        Dim mq As New BO.myQueryP31
        p31_InhaleMyQuery(mq)
        With mq
            .MG_PageSize = CInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = gridP31.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = gridP31.radGridOrig.MasterTableView.SortExpressions.GetSortString()
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
        If Me.hidDrillDownField.Value <> "" Then
            'drill down úroveň
            Dim colDrill As BO.GridGroupByColumn = Factory.j74SavedGridColTemplateBL.GroupByPallet(BO.x29IdEnum.p31Worksheet).Where(Function(p) p.ColumnField = Me.hidDrillDownField.Value).First

            Dim dt As DataTable = Factory.p31WorksheetBL.GetDrillDownDataTable(colDrill, mq, gridP31.radGridOrig.MasterTableView.Attributes("sumfields"))
            gridP31.VirtualRowCount = dt.Rows.Count
            gridP31.DataSourceDataTable = dt
            Return
        End If

        gridP31.DataSource = Me.Factory.p31WorksheetBL.GetList(mq)

    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Me.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        If Me.hidDrillDownField.Value = "" Then
            RecalcVirtualRowCount()
            gridP31.Rebind(False)
        Else
            ReloadPage()
        End If
        
    End Sub

    Private Sub gridP31_NeedFooterSource(footerItem As Telerik.Web.UI.GridFooterItem, footerDatasource As Object) Handles gridP31.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"


        gridP31.ParseFooterItemString(footerItem, ViewState("footersum"))
    End Sub

    Public Sub Rebind(bolKeepSelectedItems As Boolean, Optional intExplicitSelectedPID As Integer = 0)
        If gridP31.radGridOrig.Columns.Count = 0 Then
            Return
        End If
        gridP31.Rebind(bolKeepSelectedItems, intExplicitSelectedPID)
    End Sub
    Public Sub RecalcVirtualRowCount()

        If Me.MasterDataPID = 0 Or Me.EntityX29ID = BO.x29IdEnum._NotSpecified Then Return
        Dim mq As New BO.myQueryP31
        p31_InhaleMyQuery(mq)

        Dim cSum As BO.p31WorksheetSum = Me.Factory.p31WorksheetBL.LoadSumRow(mq, False, False)
        If Not cSum Is Nothing Then
            gridP31.VirtualRowCount = cSum.RowsCount

            ViewState("footersum") = gridP31.GenerateFooterItemString(cSum)
        Else
            ViewState("footersum") = ""
            gridP31.VirtualRowCount = 0
        End If

        'grid1.VirtualRowCount = Master.Factory.p31WorksheetBL.GetVirtualCount(mq)
        gridP31.radGridOrig.CurrentPageIndex = 0
        Me.lblHeaderP31.Text = BO.BAS.OM2(Me.lblHeaderP31.Text, BO.BAS.FNI(gridP31.VirtualRowCount))
    End Sub

    Private Sub p31_InhaleMyQuery(ByRef mq As BO.myQueryP31)
        RefreshState()
        If Not Me.ExplicitMyQuery Is Nothing Then
            mq = Me.ExplicitMyQuery
            Return
        End If
        With mq
            Select Case Me.EntityX29ID
                Case BO.x29IdEnum.p41Project
                    .p41ID = Me.MasterDataPID
                Case BO.x29IdEnum.p28Contact
                    .p28ID_Client = Me.MasterDataPID
                Case BO.x29IdEnum.j02Person
                    .j02ID = Me.MasterDataPID
                Case BO.x29IdEnum.p56Task
                    .p56IDs = New List(Of Integer)
                    .p56IDs.Add(Me.MasterDataPID)
            End Select
            .j70ID = Me.CurrentJ70ID
            ''.SearchExpression = Trim(Me.txtSearch.Text)
            ''.QuickQuery = Me.CurrentQuickQuery
            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            If period1.Visible Then
                If period1.SelectedValue <> "" Then
                    .DateFrom = period1.DateFrom
                    .DateUntil = period1.DateUntil
                End If
            Else
                .DateFrom = Me.ExplicitDateFrom
                .DateUntil = Me.ExplicitDateUntil
            End If
            .SearchExpression = Trim(Me.txtSearch.Text)
        End With
    End Sub



    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Me.Factory.j03UserBL.SetUserParam("p31_subgrid-pagesize", Me.cbxPaging.SelectedValue)
        If Me.hidDrillDownField.Value = "" Then
            SetupP31Grid()
            gridP31.Rebind(True)
        Else
            ReloadPage()
        End If
    End Sub


   

    ''Private Sub cmdSearch_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSearch.Click
    ''    Me.Factory.j03UserBL.SetUserParam("p31_subgrid-search", txtSearch.Text)
    ''    RecalcVirtualRowCount()
    ''    gridP31.Rebind(False)
    ''End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        RefreshState()
    End Sub
    Private Sub RefreshState()
        If Not Me.ExplicitMyQuery Is Nothing Then
            Me.panCommand.Visible = False
        Else
            Me.panCommand.Visible = True
        End If
        If Not BO.BAS.IsNullDBDate(Me.ExplicitDateFrom) Is Nothing Then
            Me.period1.Visible = False
            Me.cmdExplicitPeriod.Text = BO.BAS.FD(Me.ExplicitDateFrom)
            cmdExplicitPeriod.Visible = True
            If Me.ExplicitDateFrom < Me.ExplicitDateUntil Then
                Me.cmdExplicitPeriod.Text += " - " & BO.BAS.FD(Me.ExplicitDateUntil)
            End If

        Else
            Me.cmdExplicitPeriod.Visible = False
            With Me.period1
                .Visible = True
                If .SelectedValue <> "" Then
                    .BackColor = Drawing.Color.Red
                Else
                    .BackColor = Nothing
                End If
            End With
        End If
        If Me.CurrentJ70ID > 0 Then
            Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & Me.CurrentJ70ID.ToString
            Me.clue_query.Visible = True
            Me.j70ID.BackColor = Drawing.Color.Red
        Else
            Me.clue_query.Visible = False
            Me.j70ID.BackColor = Nothing
        End If
        If Trim(txtSearch.Text) = "" Then
            txtSearch.Style.Item("background-color") = ""
        Else
            txtSearch.Style.Item("background-color") = "red"
        End If
       
    End Sub
    Private Sub cmdExplicitPeriod_Click(sender As Object, e As EventArgs) Handles cmdExplicitPeriod.Click
        Me.ExplicitDateFrom = DateSerial(1900, 1, 1)
        Me.ExplicitDateUntil = DateSerial(3000, 1, 1)
        RecalcVirtualRowCount()
        gridP31.Rebind(False)
    End Sub

    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Factory.j03UserBL.SetUserParam("p31_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.EntityX29ID), Me.cbxGroupBy.SelectedValue)
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        gridP31.Rebind(True)
    End Sub
    

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        Dim cXLS As New clsExportToXls(Me.Factory)

        Dim mq As New BO.myQueryP31
        p31_InhaleMyQuery(mq)

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Me.Factory.p31WorksheetBL.GetList(mq)

        Dim strFileName As String = cXLS.ExportGridData(lis, cJ74)
        If strFileName = "" Then
            Response.Write(cXLS.ErrorMessage)
        Else
            Response.Redirect("binaryfile.aspx?tempfile=" & strFileName)
        End If
    End Sub

    Private Sub Handle_RunSearch()
        Factory.j03UserBL.SetUserParam("p31_subgrid-search", Trim(txtSearch.Text))

        If Me.hidDrillDownField.Value = "" Then
            gridP31.Rebind(False)
        Else
            ReloadPage()
        End If

        txtSearch.Focus()
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSearch.Click
        Handle_RunSearch()
    End Sub

    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Factory.j03UserBL.SetUserParam("p31_subgrid-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        gridP31.Rebind(True)
    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Factory.j03UserBL.SetUserParam("p31_subgrid-j70id", Me.CurrentJ70ID.ToString)
        If Me.hidDrillDownField.Value = "" Then
            gridP31.Rebind(True)
        Else
            ReloadPage()
        End If

    End Sub

    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        Factory.j03UserBL.SetUserParam("p31_subgrid-j74id_" & BO.BAS.GetDataPrefix(Me.EntityX29ID), Me.j74id.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect(Request.Url.AbsoluteUri.ToString())
    End Sub
End Class