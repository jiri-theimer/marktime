Imports Telerik.Web.UI

Public Class datagrid
    Inherits System.Web.UI.UserControl
    Public Event NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs)
    Public Event ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
    Public Event NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object)
    Public Event ItemCommand(sender As Object, e As GridCommandEventArgs, strPID As String)
    Public Event SortCommand(SortExpression As String)
    Public Event FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String)
    Public Property Skin As String
        Get
            Return grid1.Skin
        End Get
        Set(value As String)
            grid1.Skin = value
        End Set
    End Property
    
    Public Property OnRowSelected As String
        Get
            Return grid1.ClientSettings.ClientEvents.OnRowSelected
        End Get
        Set(value As String)
            grid1.ClientSettings.ClientEvents.OnRowSelected = value

        End Set
    End Property
    Public Property OnRowDblClick As String
        Get
            Return grid1.ClientSettings.ClientEvents.OnRowDblClick
        End Get
        Set(value As String)
            grid1.ClientSettings.ClientEvents.OnRowDblClick = value
        End Set
    End Property

    Public Property PageSize As Integer
        Get
            Return grid1.MasterTableView.PageSize
        End Get
        Set(ByVal value As Integer)
            grid1.MasterTableView.PageSize = value
        End Set
    End Property
    Public Property AllowCustomPaging As Boolean
        Get
            Return grid1.AllowCustomPaging
        End Get
        Set(value As Boolean)
            grid1.AllowCustomPaging = value
        End Set
    End Property
    Public Property VirtualRowCount As Integer
        Get
            Return grid1.MasterTableView.VirtualItemCount
        End Get
        Set(value As Integer)
            grid1.MasterTableView.VirtualItemCount = value
            If value > 100000 Then
                grid1.PagerStyle.Mode = GridPagerMode.NumericPages


            Else
                grid1.PagerStyle.Mode = GridPagerMode.NextPrevAndNumeric
            End If
        End Set
    End Property
    Public Property AllowFilteringByColumn As Boolean
        Get
            Return grid1.MasterTableView.AllowFilteringByColumn
        End Get
        Set(value As Boolean)
            grid1.MasterTableView.AllowFilteringByColumn = value
        End Set
    End Property
    Public Property DataKeyNames As String
        Get
            Return String.Join(",", grid1.MasterTableView.DataKeyNames)
        End Get
        Set(ByVal value As String)
            grid1.MasterTableView.DataKeyNames = Split(value, ",")
        End Set
    End Property
    Public Property ClientDataKeyNames As String
        Get
            Return String.Join(",", grid1.MasterTableView.ClientDataKeyNames)
        End Get
        Set(ByVal value As String)
            grid1.MasterTableView.ClientDataKeyNames = Split(value, ",")
        End Set
    End Property

    Public Property radGridOrig As RadGrid
        Get
            Return grid1
        End Get
        Set(ByVal value As RadGrid)
            grid1 = value
        End Set
    End Property
    Public Overridable Property DataSource As IEnumerable
        Get
            Return grid1.DataSource
        End Get
        Set(ByVal value As IEnumerable)
            grid1.DataSource = value

        End Set
    End Property
    Public Property AllowMultiSelect As Boolean
        Get
            Return grid1.AllowMultiRowSelection
        End Get
        Set(value As Boolean)
            grid1.AllowMultiRowSelection = value
        End Set
    End Property
    Public Property PagerAlwaysVisible As Boolean
        Get
            Return grid1.PagerStyle.AlwaysVisible
        End Get
        Set(value As Boolean)
            grid1.PagerStyle.AlwaysVisible = value
        End Set
    End Property

    Public Function GetSelectedPIDs() As List(Of Integer)
        Dim lis As New List(Of Integer)
        Dim ie As IEnumerable(Of GridDataItem) = grid1.MasterTableView.GetSelectedItems.AsEnumerable()
        If ie Is Nothing Then Return lis
        For Each it As GridDataItem In ie
            lis.Add(it.GetDataKeyValue("pid"))
        Next
        Return lis
    End Function
    Public Function GetAllPIDs() As List(Of Integer)
        Dim lis As New List(Of Integer)
        For Each it As GridDataItem In grid1.MasterTableView.Items
            lis.Add(it.GetDataKeyValue("pid"))
        Next
        Return lis
    End Function
    Public ReadOnly Property RowsCount As Integer
        Get
            Return grid1.MasterTableView.Items.Count
        End Get
    End Property

    Public Overridable Sub Rebind(bolKeepSelectedItems As Boolean, Optional intExplicitSelectedPID As Integer = 0)
        Dim ie As IEnumerable(Of GridDataItem) = Nothing
        If bolKeepSelectedItems And intExplicitSelectedPID = 0 Then ie = grid1.MasterTableView.GetSelectedItems.AsEnumerable()

        grid1.Rebind()

        If bolKeepSelectedItems Then
            If intExplicitSelectedPID <> 0 Then
                For Each it As GridDataItem In grid1.MasterTableView.Items
                    If it.GetDataKeyValue("pid") = intExplicitSelectedPID Then
                        it.Selected = True
                        Return
                    End If
                Next
            Else
                For Each it As GridDataItem In ie
                    it.Selected = True
                Next
            End If

        End If
    End Sub

    Public Overloads Sub SelectRecords(lisDataPIDs As List(Of Integer))
        For Each x As Integer In lisDataPIDs
            For Each it As GridDataItem In grid1.MasterTableView.Items
                If it.GetDataKeyValue("pid") = x Then
                    it.Selected = True : Exit For
                End If
            Next
        Next

    End Sub
    Public Overloads Sub SelectRecords(intOnePID As Integer)
        For Each it As GridDataItem In grid1.MasterTableView.Items
            If it.GetDataKeyValue("pid") = intOnePID Then
                it.Selected = True : Exit For
            End If
        Next
    End Sub

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        SetupGrid()
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

    Public Sub ClearColumns()
        grid1.Columns.Clear()
        grid1.MasterTableView.GroupByExpressions.Clear()
    End Sub

    Private Sub SetupGrid()
        With grid1.PagerStyle
            .PageSizeLabelText = ""
            .LastPageToolTip = "Poslední strana"
            .FirstPageToolTip = "První strana"
            .PrevPageToolTip = "Předchozí strana"
            .NextPageToolTip = "Další strana"
            .PagerTextFormat = "{4} Strana {0} z {1}, záznam {2} až {3} z {5}"
        End With

        With grid1.MasterTableView

            .NoMasterRecordsText = "Žádné záznamy"
        End With
    End Sub
    Public Sub AddCheckboxSelector()
        Dim col As New GridClientSelectColumn()
        col.ItemStyle.Width = Unit.Parse("20px")
        grid1.MasterTableView.Columns.Add(col)
    End Sub
    Public Sub AddButton(strText As String, strCommandName As String, strHeaderText As String)
        Dim cmd As New GridButtonColumn
        grid1.MasterTableView.Columns.Add(cmd)
        With cmd
            .CommandName = strCommandName
            .UniqueName = strCommandName
            .Text = strText
            .HeaderText = strHeaderText
        End With



    End Sub

    Public Sub AddTextboxColumn(strField As String, strHeader As String, strColumnEditorID As String, bolAllowSorting As Boolean, Optional ByVal strUniqueName As String = "")
        Dim col As New GridBoundColumn

        grid1.MasterTableView.Columns.Add(col)
        col.HeaderText = strHeader
        col.DataField = strField
        col.ColumnEditorID = strColumnEditorID
        If strUniqueName <> "" Then
            col.UniqueName = strUniqueName
            col.SortExpression = strUniqueName
        End If
        col.AllowSorting = bolAllowSorting
    End Sub
    Public Sub AddNumbericTextboxColumn(strField As String, strHeader As String, strColumnEditorID As String, bolAllowSorting As Boolean, Optional ByVal strUniqueName As String = "")
        Dim col As New GridNumericColumn

        grid1.MasterTableView.Columns.Add(col)
        col.HeaderText = strHeader
        col.DataField = strField
        col.ColumnEditorID = strColumnEditorID
        If strUniqueName <> "" Then
            col.UniqueName = strUniqueName
            col.SortExpression = strUniqueName
        End If
        col.AllowSorting = bolAllowSorting
    End Sub

    Public Sub AddColumn(ByVal strField As String, ByVal strHeader As String, Optional ByVal colformat As BO.cfENUM = BO.cfENUM.AnyString, Optional ByVal bolAllowSorting As Boolean = True, Optional ByVal bolVisible As Boolean = True, Optional ByVal strUniqueName As String = "", Optional strHeaderTooltip As String = "", Optional bolShowTotals As Boolean = False, Optional bolAllowFiltering As Boolean = True)
        Select Case colformat
            Case BO.cfENUM.Checkbox
                AddCheckboxColumn(strField, strHeader, bolAllowSorting, bolVisible)
                Return
            Case Else
        End Select
        Dim col As New GridBoundColumn

        grid1.MasterTableView.Columns.Add(col)
        col.HeaderText = strHeader
        col.DataField = strField
        col.HeaderTooltip = strHeaderTooltip
        col.ReadOnly = True
        If strUniqueName <> "" Then
            col.UniqueName = strUniqueName
            col.SortExpression = strUniqueName
        End If
        col.AllowSorting = bolAllowSorting
        col.Visible = bolVisible
        col.AllowFiltering = bolAllowFiltering
        If colformat = BO.cfENUM.AnyString And bolAllowFiltering Then
            col.AutoPostBackOnFilter = True
            col.CurrentFilterFunction = GridKnownFunction.StartsWith
        End If

        Select Case colformat
            Case BO.cfENUM.DateOnly
                col.DataFormatString = "{0:dd.MM.yyyy}"
                col.DataType = Type.GetType("System.DateTime")

            Case BO.cfENUM.DateTime
                col.DataFormatString = "{0:dd.MM.yyyy HH:mm}"
            Case BO.cfENUM.Numeric, BO.cfENUM.Numeric2
                'col.DataFormatString = "{0:F2}"
                col.DataFormatString = "{0:###,##0.00}"
                col.DataType = System.Type.GetType("System.Double")
                col.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                If bolShowTotals Then
                    'col.DefaultInsertValue = "sum"
                    col.Aggregate = GridAggregateFunction.Sum

                End If

            Case BO.cfENUM.Numeric3
                col.DataFormatString = "{0:###,##0.000}"
                col.DataType = System.Type.GetType("System.Double")
                col.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                If bolShowTotals Then
                    'col.DefaultInsertValue = "sum"
                    col.Aggregate = GridAggregateFunction.Sum
                End If
            Case BO.cfENUM.Numeric0
                col.DataType = System.Type.GetType("System.Int32")
                col.DataFormatString = "{0:F0}"
                col.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                If bolShowTotals Then
                    col.Aggregate = GridAggregateFunction.Sum
                End If
            Case BO.cfENUM.TimeOnly
                col.DataFormatString = "{0:HH:mm}"

        End Select
    End Sub
    Private Sub AddCheckboxColumn(ByVal strField As String, ByVal strHeader As String, Optional ByVal bolAllowSorting As Boolean = True, Optional ByVal bolVisible As Boolean = True)
        Dim col As GridCheckBoxColumn
        col = New GridCheckBoxColumn

        grid1.MasterTableView.Columns.Add(col)
        col.HeaderText = strHeader
        col.DataField = strField
        col.AllowSorting = bolAllowSorting
        col.Visible = bolVisible
    End Sub

    Public Sub AddSystemColumn(ByVal intWidth As Integer, Optional strFieldName As String = "systemcolumn")
        Dim col As GridBoundColumn
        col = New GridBoundColumn
        grid1.MasterTableView.Columns.Add(col)
        col.DataField = strFieldName
        col.AllowFiltering = False
        col.AllowSorting = False
        col.ItemStyle.Width = Unit.Parse(intWidth.ToString & "px")
        col.HeaderStyle.Width = Unit.Parse(intWidth.ToString & "px")
        col.Exportable = False
        col.ReadOnly = True
    End Sub

    Private Sub grid1_BiffExporting(sender As Object, e As Telerik.Web.UI.GridBiffExportingEventArgs) Handles grid1.BiffExporting
        For i = 1 To e.ExportStructure.Tables(0).Columns.Count
            e.ExportStructure.Tables(0).Columns(i).Style.HorizontalAlign = HorizontalAlign.Left

        Next

    End Sub




    Private Sub grid1_DataBound(sender As Object, e As System.EventArgs) Handles grid1.DataBound
        With grid1.MasterTableView
            If .Columns.Count > 0 Then
                If TypeOf .Columns(0) Is GridClientSelectColumn Then
                    .Columns(0).ItemStyle.Width = Unit.Parse("20px")
                End If
            End If


        End With

        If Not grid1.ShowFooter Then Return
        If grid1.MasterTableView.GetItems(GridItemType.Footer).Count = 0 Then Return
        Dim footerItem As GridFooterItem = grid1.MasterTableView.GetItems(GridItemType.Footer)(0)
        RaiseEvent NeedFooterSource(footerItem, Nothing)

    End Sub

    Private Sub grid1_Init(sender As Object, e As EventArgs) Handles grid1.Init
        Dim menu As GridFilterMenu = grid1.FilterMenu
        Dim i As Integer = 0
        With menu.Items
            While i < .Count
                With .Item(i)
                    Select Case .Text
                        Case "NoFilter"
                            .Text = "Nefiltrovat" : i += 1
                        Case "Contains"
                            .Text = "Obsahuje" : i += 1
                        Case "EqualTo"
                            .Text = "Je rovno" : i += 1
                        Case "GreaterThan"
                            .Text = "Je větší než" : i += 1
                        Case "LessThan"
                            .Text = "Je menší než" : i += 1
                        Case "IsNull"
                            .Text = "Je prázdné" : i += 1
                        Case "NotIsNull"
                            .Text = "Není prázdné" : i += 1
                        Case "StartsWith"
                            .Text = "Začíná na" : i += 1
                        Case Else
                            Dim ss As String = .Text
                            menu.Items.RemoveAt(i)
                    End Select
                End With
            End While
        End With

    End Sub



    Private Sub grid1_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles grid1.ItemCommand
        If Not (TypeOf e.Item Is GridPagerItem Or TypeOf e.Item Is GridHeaderItem) Then
            RaiseEvent ItemCommand(sender, e, grid1.Items.Item(e.Item.ItemIndex).GetDataKeyValue("pid"))
        End If
        If e.CommandName = RadGrid.FilterCommandName Then
            Dim filterPair As Pair = DirectCast(e.CommandArgument, Pair)

            Dim filterBox As TextBox = CType((CType(e.Item, GridFilteringItem))(filterPair.Second.ToString()).Controls(0), TextBox)

            RaiseEvent FilterCommand(filterPair.First, filterPair.Second, filterBox.Text)
        End If
    End Sub



    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If TypeOf e.Item Is GridPagerItem Then
            If Not e.Item.FindControl("PageSizeComboBox") Is Nothing Then
                e.Item.FindControl("PageSizeComboBox").Visible = False
            End If

        End If


        RaiseEvent ItemDataBound(sender, e)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        RaiseEvent NeedDataSource(sender, e)

    End Sub

    Public Function GenerateFooterItemString(cSum As Object) As String
        Dim s As String = ""
        For Each col In grid1.MasterTableView.Columns
            If TypeOf col Is GridBoundColumn Then
                If col.Aggregate = GridAggregateFunction.Sum Then
                    Dim o As Object = BO.BAS.GetPropertyValue(cSum, col.DataField)
                    If Not o Is Nothing Then
                        s += "|" & col.DataField & ";" & BO.BAS.FN(o)
                    End If
                End If
                'If col.DefaultInsertValue = "sum" Then


                'End If
            End If
        Next


        Return BO.BAS.OM1(s)
    End Function

    Public Sub ParseFooterItemString(footerItem As GridFooterItem, strFooterString As String)
        If strFooterString = "" Then Return
        Dim a() As String = Split(strFooterString, "|")
        For Each strPair As String In a
            Dim b() As String = Split(strPair, ";")
            footerItem.Item(b(0)).Text = b(1)
        Next

    End Sub

    Private Sub grid1_SortCommand(sender As Object, e As GridSortCommandEventArgs) Handles grid1.SortCommand
        Select Case e.NewSortOrder
            Case GridSortOrder.Ascending
                RaiseEvent SortCommand(e.SortExpression)
            Case GridSortOrder.Descending
                RaiseEvent SortCommand(e.SortExpression & " DESC")
            Case GridSortOrder.None
                RaiseEvent SortCommand("")
        End Select

    End Sub

    Public Function GetFilterExpression() As String

        ''Dim s As New List(Of String)
        ''For Each col As GridColumn In grid1.MasterTableView.Columns
        ''    s.Add(col.CurrentFilterValue)
        ''Next
        ''Return System.String.Join(" AND ", s)

        Return grid1.MasterTableView.FilterExpression
    End Function
    Public Sub ClearFilter()
        For Each col As GridColumn In grid1.MasterTableView.Columns
            col.CurrentFilterValue = ""
            col.CurrentFilterFunction = GridKnownFunction.NoFilter
        Next
        grid1.MasterTableView.FilterExpression = ""
    End Sub
End Class