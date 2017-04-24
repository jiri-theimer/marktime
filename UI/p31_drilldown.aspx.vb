Imports Telerik.Web.UI

Public Class p31_drilldown
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Private _lisDD As List(Of BO.GridColumn) = Nothing
    Private Property _curIsExport As Boolean

    Private Sub p31_summary_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hidJ70ID.Value = Request.Item("j70id")
            hidJ74ID.Value = Request.Item("j74id")
            hidMasterPID.Value = Request.Item("masterpid")
            hidMasterPrefix.Value = Request.Item("masterprefix")

            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p31_grid-period")
                .Add("periodcombo-custom_query")
                .Add("p31_grid-filter_completesql")
                .Add(hidMasterPrefix.Value & "p31_drilldown-dd1")
                .Add(hidMasterPrefix.Value & "p31_drilldown-dd2")
                .Add(hidMasterPrefix.Value & "p31_drilldown-is-sumcols")
                .Add(hidMasterPrefix.Value & "p31_drilldown-sumcols")
            End With
            With Master
                .AddToolbarButton("Nastavení", "setting", 0, "Images/arrow_down.gif", False)
                .RadToolbar.FindItemByValue("setting").CssClass = "show_hide1"
            End With


            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("p31_grid-period")
                Me.chkSpecificSumCols.Checked = BO.BAS.BG(.GetUserParam(hidMasterPrefix.Value & "p31_drilldown-is-sumcols", "0"))
                Dim strDefDD1 As String = "Person"
                Select Case Me.hidMasterPrefix.Value
                    Case "j02" : strDefDD1 = "ClientName"
                End Select
                SetupGroupByCombo(.GetUserParam(hidMasterPrefix.Value & "p31_drilldown-dd1", strDefDD1), .GetUserParam(hidMasterPrefix.Value & "p31_drilldown-dd2"))

                SetupSumColsSetting(.GetUserParam(hidMasterPrefix.Value & "p31_drilldown-sumcols", "1"))
                hidGridColumnSql.Value = .GetUserParam("p31_grid-filter_completesql")
            End With


            RefreshData()
            RenderQueryInfo()
        End If
    End Sub

    Private Sub SetupGroupByCombo(strDD1 As String, strDD2 As String)
        Dim lisAllCols As List(Of BO.GridColumn) = Me.lisDD.Where(Function(p) p.Pivot_SelectSql <> "").ToList
        Me.dd1.Items.Clear() : Me.dd2.Items.Clear()

        For Each c In lisAllCols.Where(Function(p) p.TreeGroup <> "").Select(Function(p) p.TreeGroup).Distinct
            Dim n As New RadComboBoxItem(c, "group" & c)
            n.Enabled = False
            Me.dd1.Items.Add(n)
        Next

        For i = lisAllCols.Count - 1 To 0 Step -1
            Dim c As BO.GridColumn = lisAllCols(i)
            Dim n As New RadComboBoxItem(c.ColumnHeader, c.ColumnName)
            Select Case c.ColumnType
                Case BO.cfENUM.DateTime, BO.cfENUM.DateTime
                    n.ImageUrl = "Images/type_datetime.png"
                Case BO.cfENUM.DateOnly
                    n.ImageUrl = "Images/type_date.png"
                Case BO.cfENUM.Numeric, BO.cfENUM.Numeric2, BO.cfENUM.Numeric0
                    n.ImageUrl = "Images/type_number.png"
                Case BO.cfENUM.AnyString
                    n.ImageUrl = "Images/type_text.png"
                Case BO.cfENUM.Checkbox
                    n.ImageUrl = "Images/type_checkbox.png"
            End Select

            If c.TreeGroup = "" Then
                Me.dd1.Items.Add(n)
            Else
                Dim y As Integer = Me.dd1.FindItemIndexByValue("group" & c.TreeGroup)
                If y > -1 Then
                    Me.dd1.Items.Insert(y + 1, n)
                End If



            End If
        Next
        For i As Integer = 0 To Me.dd1.Items.Count - 1 - 1
            Dim n As New RadComboBoxItem()
            With Me.dd1.Items(i)
                n.ImageUrl = .ImageUrl
                n.Text = .Text
                n.Value = .Value
                n.Enabled = .Enabled
            End With
            Me.dd2.Items.Add(n)
        Next
        Me.dd2.Items.Insert(0, "")
        If strDD1 <> "" Then Me.dd1.SelectedValue = strDD1
        If strDD2 <> "" Then Me.dd2.SelectedValue = strDD2
    End Sub


    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            Select Case hidMasterPrefix.Value
                Case "p41"
                    .p41ID = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case "p28"
                    .p28ID_Client = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case "j02"
                    .j02ID = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case "p56"
                    .p56IDs = New List(Of Integer)
                    .p56IDs.Add(BO.BAS.IsNullInt(hidMasterPID.Value))
                Case "p91"
                    .p91ID = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case Else

            End Select
            .j70ID = BO.BAS.IsNullInt(hidJ70ID.Value)

            '.ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            '.MG_GridSqlColumns = Me.hidCols.Value

            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead

            .DateFrom = period1.DateFrom
            .DateUntil = period1.DateUntil
            .ColumnFilteringExpression = hidGridColumnSql.Value
            .MG_AdditionalSqlWHERE = Me.hidMasterAW.Value


        End With
    End Sub
    Private Sub SetupSumColsSetting(strDefSumCols As String)
        Dim lisAllCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()

        colsSource.Items.Clear()
        colsDest.Items.Clear()
        For Each c In lisAllCols
            Dim it As New RadListBoxItem(c.Caption, c.FieldTypeID.ToString)
            Select Case c.ColumnType
                Case BO.cfENUM.DateTime, BO.cfENUM.DateTime
                    it.ImageUrl = "Images/type_datetime.png"
                Case BO.cfENUM.DateOnly
                    it.ImageUrl = "Images/type_date.png"
                Case BO.cfENUM.Numeric, BO.cfENUM.Numeric2, BO.cfENUM.Numeric0
                    it.ImageUrl = "Images/type_number.png"
                Case BO.cfENUM.AnyString
                    it.ImageUrl = "Images/type_text.png"
                Case BO.cfENUM.Checkbox
                    it.ImageUrl = "Images/type_checkbox.png"
            End Select

            colsSource.Items.Add(it)
        Next

        If strDefSumCols = "" Then Return

        Dim a() As String = Split(strDefSumCols, ",")
        For Each s In a
            Dim it As RadListBoxItem = colsSource.FindItem(Function(p) p.Value = s)
            If Not it Is Nothing Then
                colsSource.Transfer(it, colsSource, colsDest)
                colsSource.ClearSelection()
                colsDest.ClearSelection()
            End If
        Next
        colsSource.ClearSelection()

    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound

        If _curIsExport Then
            If TypeOf e.Item Is GridHeaderItem Then
                e.Item.BackColor = Drawing.Color.Silver
            End If
            If TypeOf e.Item Is GridGroupHeaderItem Then
                e.Item.BackColor = Drawing.Color.WhiteSmoke
            End If
            If TypeOf e.Item Is GridDataItem Or TypeOf e.Item Is GridHeaderItem Then
                ''e.Item.Cells(0).Visible = False
            End If
        Else
            If Not TypeOf e.Item Is GridDataItem Then Return
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            ''Dim cRec As DataRowView = CType(e.Item.DataItem, DataRowView)
            With dataItem("systemcolumn")
                .Text = "<a href='javascript:go2grid(" + Chr(34) + dataItem.GetDataKeyValue("pid").ToString + Chr(34) + ")'><img src='Images/worksheet.png' title='Zdrojový datový přehled'></a>"
            End With
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.dd1.SelectedValue = "" Then Return

        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = 200
            If _curIsExport Then .MG_PageSize = 2000
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With
        InhaleMyQuery(mq)
        Dim colDD2 As BO.GridColumn = Me.CurrentDD2
        If Not colDD2 Is Nothing Then
            If Me.CurrentDD1.ColumnName = colDD2.ColumnName Then colDD2 = Nothing
        End If

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownGridSource(Me.CurrentDD1, colDD2, Me.CurrentSumFields_PIVOT, Me.CurrentSumFields_GRID, "", mq)
        grid1.VirtualRowCount = dt.Rows.Count
        grid1.DataSourceDataTable = dt
    End Sub
    Private ReadOnly Property lisDD As List(Of BO.GridColumn)
        Get
            If _lisDD Is Nothing Then _lisDD = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)
            Return _lisDD
        End Get
    End Property
    Private ReadOnly Property CurrentDD1 As BO.GridColumn
        Get
            If Me.dd1.SelectedValue = "" Then Return Nothing
            Return Me.lisDD.Where(Function(p) p.ColumnName = Me.dd1.SelectedValue).First()
        End Get
    End Property
    Private ReadOnly Property CurrentDD2 As BO.GridColumn
        Get
            If Me.dd2.SelectedValue = "" Then Return Nothing
            Return lisDD.Where(Function(p) p.ColumnName = Me.dd2.SelectedValue).First()
        End Get
    End Property
    Private ReadOnly Property CurrentSumFields_PIVOT As List(Of BO.PivotSumField)
        Get
            If Not chkSpecificSumCols.Checked Then Return Nothing
            Dim lis As New List(Of BO.PivotSumField), a() As String = Split(GetSumPIDsInLine(), ",")
            If GetSumPIDsInLine() = "" Then Return lis
            For i As Integer = 0 To UBound(a)
                lis.Add(New BO.PivotSumField(CType(a(i), BO.PivotSumFieldType)))
            Next
            Return lis
        End Get
    End Property
    Private ReadOnly Property CurrentSumFields_GRID As List(Of BO.GridColumn)
        Get
            If Me.chkSpecificSumCols.Checked Then Return Nothing
            Dim lis As New List(Of BO.GridColumn)
            Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(BO.BAS.IsNullInt(hidJ74ID.Value))
            For Each s As String In Split(cJ74.j74ColumnNames, ",")
                Dim strField As String = Trim(s)

                Dim c As BO.GridColumn = Me.lisDD.Find(Function(p) p.ColumnName = strField)
                If Not c Is Nothing Then
                    If c.IsShowTotals Then
                        lis.Add(c)
                    End If

                End If
            Next
            Return lis
        End Get
    End Property
    Private Function GetSumPIDsInLine() As String
        Dim s As String = ""
        For Each it As RadListBoxItem In colsDest.Items
            If s = "" Then
                s = it.Value
            Else
                s += "," & it.Value
            End If
        Next
        Return s
    End Function

    Private Sub SaveCurrentSettings()
        With Master.Factory.j03UserBL
            .SetUserParam(hidMasterPrefix.Value & "p31_drilldown-is-sumcols", BO.BAS.GB(Me.chkSpecificSumCols.Checked))
            .SetUserParam(hidMasterPrefix.Value & "p31_drilldown-dd1", Me.dd1.SelectedValue)
            .SetUserParam(hidMasterPrefix.Value & "p31_drilldown-dd2", Me.dd2.SelectedValue)
            .SetUserParam(hidMasterPrefix.Value & "p31_drilldown-sumcols", GetSumPIDsInLine())
        End With

    End Sub
    Private Sub RefreshData()
        SetupGrid()
        grid1.Rebind(False)
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        SaveCurrentSettings()
        RefreshData()
    End Sub

    Private Sub SetupGrid()
        If Me.dd1.SelectedValue = "" Then Return

        With grid1
            .ClearColumns()
            .PageSize = 200
            .radGridOrig.ShowFooter = True
            .DataKeyNames = "pid"
            .AllowMultiSelect = False
            .AddSystemColumn(16)

            .AddColumn(Me.CurrentDD1.ColumnName, Me.CurrentDD1.ColumnHeader)
            If Not Me.CurrentDD2 Is Nothing Then
                .AddColumn(Me.CurrentDD2.ColumnName, Me.CurrentDD2.ColumnHeader)
            End If

            If Me.chkSpecificSumCols.Checked Then
                For Each c In Me.CurrentSumFields_PIVOT

                    .AddColumn("sum" & c.FieldTypeID.ToString, c.Caption, BO.cfENUM.Numeric2, , , , , True, False)
                Next
            Else
                For Each c In Me.CurrentSumFields_GRID
                    .AddColumn(IIf(c.ColumnDBName = "", "sum" & c.ColumnName, "sum" & c.ColumnDBName), c.ColumnHeader, BO.cfENUM.Numeric2, , , , , c.IsShowTotals, False)

                Next

            End If
            
            .AddColumn("RecsCount", "Počet", BO.cfENUM.Numeric0, True, , , , True)
        End With
        If Not Me.CurrentDD2 Is Nothing Then
            SetupGrouping(Me.CurrentDD1.ColumnName, Me.CurrentDD1.ColumnHeader)
        End If
    End Sub





    Private Sub p31_drilldown_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        panSumCols.Visible = chkSpecificSumCols.Checked
        
    End Sub

    Private Sub RenderQueryInfo()
        If period1.SelectedValue <> "" Then
            lblQuery.Text = BO.BAS.FD(period1.DateFrom) & " - " & BO.BAS.FD(period1.DateUntil)
        End If
        If BO.BAS.IsNullInt(hidJ70ID.Value) <> 0 Then
            Dim c As BO.j70QueryTemplate = Master.Factory.j70QueryTemplateBL.Load(CInt(hidJ70ID.Value))
            lblQuery.Text = BO.BAS.OM4(lblQuery.Text, c.j70Name, "; ")
        End If
        If hidGridColumnSql.Value <> "" Then
            lblQuery.Text = BO.BAS.OM4(lblQuery.Text, "[Sloupcový filtr]", "; ")
        End If
        If hidMasterPrefix.Value <> "" Then
            Dim s As String = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(hidMasterPrefix.Value), BO.BAS.IsNullInt(hidMasterPID.Value), True)
            lblQuery.Text = BO.BAS.OM4(Me.lblQuery.Text, s, "; ")
        End If
    End Sub

    Private Sub dd1_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles dd1.SelectedIndexChanged
        SaveCurrentSettings()
        RefreshData()
    End Sub

    Private Sub dd2_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles dd2.SelectedIndexChanged
        SaveCurrentSettings()
        RefreshData()
    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        grid1.radGridOrig.GroupingSettings.RetainGroupFootersVisibility = True
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
            .GroupsDefaultExpanded = True
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strFieldHeader
            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)
        End With

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

    Private Sub chkSpecificSumCols_CheckedChanged(sender As Object, e As EventArgs) Handles chkSpecificSumCols.CheckedChanged
        SaveCurrentSettings()
        RefreshData()
    End Sub
End Class