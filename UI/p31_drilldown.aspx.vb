Public Class p31_drilldown
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    
    Private Sub p31_summary_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hidJ70ID.Value = Request.Item("j70id")
            hidJ74ID.Value = Request.Item("j74id")

            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p31_drilldown-dd")
                .Add("p31_drilldown-sumcols")
            End With
            With Master
                .AddToolbarButton("Nastavení", "setting", 0, "Images/arrow_down.gif", False)
                .RadToolbar.FindItemByValue("setting").CssClass = "show_hide1"
            End With


            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                SetupGroupByCombo(.GetUserParam("p31_drilldown-dd", "Person"))

                SetupSumColsSetting(.GetUserParam("p31_drilldown-sumcols", "1"))
            End With


            RefreshData()
        End If
    End Sub

    Private Sub SetupGroupByCombo(strDef As String)
        Dim lisAllCols As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet).Where(Function(p) p.Pivot_SelectSql <> "").ToList
        Me.dd.Items.Clear()

        For Each c In lisAllCols.Where(Function(p) p.TreeGroup <> "").Select(Function(p) p.TreeGroup).Distinct
            Dim n As New Telerik.Web.UI.RadComboBoxItem(c, "group" & c)
            n.Enabled = False
            Me.dd.Items.Add(n)
        Next

        For i = lisAllCols.Count - 1 To 0 Step -1
            Dim c As BO.GridColumn = lisAllCols(i)
            Dim n As New Telerik.Web.UI.RadComboBoxItem(c.ColumnHeader, c.ColumnName)
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
                Me.dd.Items.Add(n)
            Else
                Dim y As Integer = Me.dd.FindItemIndexByValue("group" & c.TreeGroup)
                If y > -1 Then
                    Me.dd.Items.Insert(y + 1, n)
                End If



            End If

        Next
        If strDef <> "" Then Me.dd.SelectedValue = strDef

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
            If hidD1.Value <> "" Then
                .DateFrom = BO.BAS.ConvertString2Date(hidD1.Value)
                .DateUntil = BO.BAS.ConvertString2Date(hidD2.Value)
            End If
            .MG_AdditionalSqlWHERE = Me.hidMasterAW.Value

        End With
    End Sub
    Private Sub SetupSumColsSetting(strDefSumCols As String)
        Dim lisAllCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()

        colsSource.Items.Clear()
        colsDest.Items.Clear()
        For Each c In lisAllCols
            Dim it As New Telerik.Web.UI.RadListBoxItem(c.Caption, c.FieldTypeID.ToString)
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
            Dim it As Telerik.Web.UI.RadListBoxItem = colsSource.FindItem(Function(p) p.Value = s)
            If Not it Is Nothing Then
                colsSource.Transfer(it, colsSource, colsDest)
                colsSource.ClearSelection()
                colsDest.ClearSelection()
            End If
        Next
        colsSource.ClearSelection()

    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.dd.SelectedValue = "" Then Return

        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = 100
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With
        InhaleMyQuery(mq)

        Dim colDrill As BO.GridColumn = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet).Where(Function(p) p.ColumnName = Me.dd.SelectedValue).First
        Dim strSumFields As String = String.Join("|", Me.CurrentSumFields.Select(Function(p) p.SelectField))

        'Dim dtDD As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(colDrill, mq, strSumFields)
        'grid1.VirtualRowCount = dtDD.Rows.Count
        'grid1.DataSourceDataTable = dtDD
    End Sub

    Private ReadOnly Property CurrentSumFields As List(Of BO.PivotSumField)
        Get
            Dim lis As New List(Of BO.PivotSumField), a() As String = Split(GetSumPIDsInLine(), ",")
            If GetSumPIDsInLine() = "" Then Return lis
            For i As Integer = 0 To UBound(a)
                lis.Add(New BO.PivotSumField(CType(a(i), BO.PivotSumFieldType)))
            Next
            Return lis
        End Get
    End Property
    Private Function GetSumPIDsInLine() As String
        Dim s As String = ""
        For Each it As Telerik.Web.UI.RadListBoxItem In colsDest.Items
            If s = "" Then
                s = it.Value
            Else
                s += "," & it.Value
            End If
        Next
        Return s
    End Function

    Private Sub SaveCurrentSettings()
        Master.Factory.j03UserBL.SetUserParam("p31_drilldown-dd", Me.dd.SelectedValue)
        Master.Factory.j03UserBL.SetUserParam("p31_drilldown-sumcols", GetSumPIDsInLine())

        
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
        If Me.dd.SelectedValue = "" Then Return
        Dim colDrill As BO.GridColumn = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet).Where(Function(p) p.ColumnName = Me.dd.SelectedValue).First

        With grid1
            .ClearColumns()
            .PageSize = 100
            .radGridOrig.ShowFooter = True
            .DataKeyNames = "pid"
            .AllowMultiSelect = True

            .AddColumn(colDrill.ColumnName, colDrill.ColumnHeader)

            For Each c In Me.CurrentSumFields
                Dim strDbField As String = "sum" & c.FieldTypeID.ToString

                .AddColumn(strDbField, c.Caption, BO.cfENUM.Numeric2, , , , , True, False)
                '    If c.ColumnFormat = BO.cfENUM.Numeric2 Then
                '        .radGridOrig.MasterTableView.Columns.FindByDataField("col" & c.FieldTypeID.ToString).HeaderStyle.HorizontalAlign = HorizontalAlign.Right
                '    End If

                '    Me.hidCols.Value = BO.BAS.OM4(hidCols.Value, c.FieldTypeID.ToString)
            Next

        End With
    End Sub


   
    Private Sub dd_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles dd.SelectedIndexChanged
        SaveCurrentSettings()
        RefreshData()
    End Sub
End Class