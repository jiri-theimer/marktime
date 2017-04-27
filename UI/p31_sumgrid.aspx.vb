Imports Telerik.Web.UI
Public Class p31_sumgrid
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private _lisDD As List(Of BO.GridColumn) = Nothing
    Private Property _curIsExport As Boolean


    Private Sub p31_sumgrid_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SiteMenuValue = "p31_sumgrid"
            hidJ70ID.Value = Request.Item("j70id")

            hidMasterPID.Value = Request.Item("masterpid")
            hidMasterPrefix.Value = Request.Item("masterprefix")
            hidTabQueryFlag.Value = Request.Item("tabqueryflag")

            If hidMasterPrefix.Value <> "" Then
                panQueryByEntity.Visible = True
                Me.MasterRecord.Text = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(hidMasterPrefix.Value), BO.BAS.IsNullInt(hidMasterPID.Value))
                Me.MasterRecord.NavigateUrl = hidMasterPrefix.Value & "_framework.aspx?pid=" & hidMasterPID.Value
                Select Case hidMasterPrefix.Value
                    Case "p41" : imgEntity.ImageUrl = "Images/project.png"
                    Case "j02" : imgEntity.ImageUrl = "Images/person.png"
                    Case "p28" : imgEntity.ImageUrl = "Images/contact.png"
                    Case "p91" : imgEntity.ImageUrl = "Images/invoice.png"
                End Select
            Else
                panQueryByEntity.Visible = False
            End If

            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p31-j70id")
                .Add("p31_grid-period")
                .Add("periodcombo-custom_query")
                .Add("p31_grid-filter_completesql")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-dd1")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-dd2")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-sumcols")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-addcols")
                .Add("p31_sumgrid-pagesize")
                .Add("p31_sumgrid-chkFirstLastCount")
            End With


            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("p31_sumgrid-pagesize", "100"))
                Me.chkFirstLastCount.Checked = BO.BAS.BG(.GetUserParam("p31_sumgrid-chkFirstLastCount", "1"))
                SetupJ70Combo(BO.BAS.IsNullInt(.GetUserParam("p31-j70id")))
                period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("p31_grid-period")

                Dim strDefDD1 As String = ""
                Select Case Me.hidMasterPrefix.Value
                    Case "j02" : strDefDD1 = "ClientName"
                    Case Else
                        strDefDD1 = "Person"
                End Select
                SetupGroupByCombo(.GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-dd1", strDefDD1), .GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-dd2"))

                SetupSumColsSetting(.GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-sumcols", "1"), .GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-addcols"))
                hidGridColumnSql.Value = .GetUserParam("p31_grid-filter_completesql")

            End With


            RefreshData()
            RenderQueryInfo()
            hidToggle.Value = ""
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
            .TabAutoQuery = Me.hidTabQueryFlag.Value

        End With
    End Sub
    Private Sub SetupSumColsSetting(strDefSumCols As String, strDefAddCols As String)
        Dim lisAllSums As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()

        sumsSource.Items.Clear()
        sumsDest.Items.Clear()
        For Each c In lisAllSums
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

            sumsSource.Items.Add(it)
        Next

        If strDefSumCols <> "" Then
            Dim a() As String = Split(strDefSumCols, ",")
            For Each s In a
                Dim it As RadListBoxItem = sumsSource.FindItem(Function(p) p.Value = s)
                If Not it Is Nothing Then
                    sumsSource.Transfer(it, sumsSource, sumsDest)
                    sumsSource.ClearSelection()
                    sumsDest.ClearSelection()
                End If
            Next
            sumsSource.ClearSelection()
        End If

        colsSource.Items.Clear()
        colsDest.Items.Clear()
        For Each c As RadComboBoxItem In Me.dd1.Items
            Dim it As New RadListBoxItem(c.Text, c.Value)
            it.ImageUrl = c.ImageUrl
            it.Enabled = c.Enabled
            colsSource.Items.Add(it)
        Next

        If strDefAddCols <> "" Then
            Dim a() As String = Split(strDefAddCols, ",")
            For Each s In a
                Dim b() As String = Split(s, "-")
                Dim it As RadListBoxItem = colsSource.FindItem(Function(p) p.Value = b(0))
                If Not it Is Nothing Then
                    If UBound(b) > 0 Then
                        it.Text += " (" & UCase(b(1)) & ")"
                    Else
                        it.Text += " (ALL)"
                    End If
                    colsSource.Transfer(it, colsSource, colsDest)
                    colsSource.ClearSelection()
                    colsDest.ClearSelection()
                End If
            Next
            colsSource.ClearSelection()
        End If
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
                .Text = "<a href='javascript:go2grid(" + Chr(34) + dataItem.GetDataKeyValue("pid").ToString + Chr(34) + ")'><img src='Images/worksheet.png' title='Přejít na přehled worksheet záznamů'></a>"
            End With
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource

        If Me.dd1.SelectedValue = "" Then Return

        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = cbxPaging.SelectedValue
            If _curIsExport Then .MG_PageSize = 2000
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With
        InhaleMyQuery(mq)
        Dim colDD2 As BO.GridColumn = Me.CurrentDD2
        If Not colDD2 Is Nothing Then
            If Me.CurrentDD1.ColumnName = colDD2.ColumnName Then colDD2 = Nothing

        End If
        hidSGF.Value = Me.CurrentDD1.ColumnName

        If Not colDD2 Is Nothing Then
            hidSGF.Value += "|" & colDD2.ColumnName

        End If

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownGridSource(Me.CurrentDD1, colDD2, Me.CurrentSumFields_PIVOT, Me.CurrentColFields_PIVOT, "", mq)
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
            Dim lis As New List(Of BO.PivotSumField), a() As String = Split(GetSumPIDsInLine(), ",")
            If GetSumPIDsInLine() = "" Then Return lis
            For i As Integer = 0 To UBound(a)
                lis.Add(New BO.PivotSumField(CType(a(i), BO.PivotSumFieldType)))
            Next
            Return lis
        End Get
    End Property
    Private ReadOnly Property CurrentColFields_PIVOT As List(Of BO.GridColumn)
        Get
            Dim lis As New List(Of BO.GridColumn)
            If GetColPIDsInLine() = "" Then Return lis
            Dim a() As String = Split(GetColPIDsInLine(), ",")
            For i As Integer = 0 To UBound(a)
                Dim b() As String = Split(a(i), "-")
                Dim c As BO.GridColumn = Me.lisDD.Where(Function(p) p.ColumnName = b(0)).First
                If UBound(b) > 0 Then
                    c.MyTag = b(1)
                Else
                    c.MyTag = "all"
                End If

                lis.Add(c)
            Next
            Return lis
        End Get
    End Property


    Private Function GetSumPIDsInLine() As String
        Dim s As String = ""
        For Each it As RadListBoxItem In sumsDest.Items
            If s = "" Then
                s = it.Value
            Else
                s += "," & it.Value
            End If
        Next
        Return s
    End Function
    Private Function GetColPIDsInLine() As String
        Dim s As String = ""
        For Each it As RadListBoxItem In colsDest.Items
            If s = "" Then
                s = it.Value
            Else
                s += "," & it.Value
            End If
            If it.Text.IndexOf("(ALL)") > 0 Then
                s += "-all"
            End If
            If it.Text.IndexOf("(MAX)") > 0 Then
                s += "-max"
            End If
            If it.Text.IndexOf("(MIN)") > 0 Then
                s += "-min"
            End If
        Next
        Return s
    End Function

    Private Sub SaveCurrentSettings()
        With Master.Factory.j03UserBL

            .SetUserParam(hidMasterPrefix.Value & "p31_sumgrid-dd1", Me.dd1.SelectedValue)
            .SetUserParam(hidMasterPrefix.Value & "p31_sumgrid-dd2", Me.dd2.SelectedValue)
            .SetUserParam(hidMasterPrefix.Value & "p31_sumgrid-sumcols", GetSumPIDsInLine())
            .SetUserParam(hidMasterPrefix.Value & "p31_sumgrid-addcols", GetColPIDsInLine())
        End With

    End Sub
    Private Sub RefreshData()
        SetupGrid()
        If Request.Item("pid") = "" Then
            grid1.Rebind(False)
        Else
            grid1.Rebind(False, BO.BAS.IsNullInt(Request.Item("pid")))
        End If

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        SaveCurrentSettings()
        RefreshData()
    End Sub

    Private Sub SetupGrid()
        If Me.dd1.SelectedValue = "" Then Return

        With grid1
            .ClearColumns()
            .PageSize = cbxPaging.SelectedValue
            .radGridOrig.ShowFooter = True
            .DataKeyNames = "pid"
            .AllowMultiSelect = False
            .AddSystemColumn(16)

            .AddColumn(Me.CurrentDD1.ColumnName, Me.CurrentDD1.ColumnHeader)
            If Not Me.CurrentDD2 Is Nothing Then
                .AddColumn(Me.CurrentDD2.ColumnName, Me.CurrentDD2.ColumnHeader)
            End If
            If Not Me.CurrentColFields_PIVOT Is Nothing Then
                For Each c In Me.CurrentColFields_PIVOT
                    .AddColumn("col" & c.ColumnName, c.ColumnHeader, c.ColumnType, , , , , , False)
                Next
            End If
            If Not Me.CurrentSumFields_PIVOT Is Nothing Then
                For Each c In Me.CurrentSumFields_PIVOT
                    .AddColumn("sum" & c.FieldTypeID.ToString, c.Caption, BO.cfENUM.Numeric2, , , , , True, False)
                Next
            End If

            .AddColumn("RecsCount", "Počet", BO.cfENUM.Numeric0, True, , , , True)
            If chkFirstLastCount.Checked Then
                .AddColumn("RecFirst", "První", BO.cfENUM.DateOnly, True)
                .AddColumn("RecLast", "Poslední", BO.cfENUM.DateOnly, True)
            End If
        End With
        If Not Me.CurrentDD2 Is Nothing Then
            SetupGrouping(Me.CurrentDD1.ColumnName, Me.CurrentDD1.ColumnHeader)
        End If
    End Sub





  

    Private Sub RenderQueryInfo()
       
        Select Case Me.hidTabQueryFlag.Value
            Case "expense"
                lblQuery.Text = BO.BAS.OM4(lblQuery.Text, "[Pouze výdaje]", "; ")
            Case "fee"
                lblQuery.Text = BO.BAS.OM4(lblQuery.Text, "[Pouze paušální odměny]", "; ")
            Case "time"
                lblQuery.Text = BO.BAS.OM4(lblQuery.Text, "[Pouze hodiny]", "; ")
        End Select
        
       
        If hidGridColumnSql.Value <> "" Then
            lblQuery.Text = BO.BAS.OM4(lblQuery.Text, "[Sloupcový filtr zdrojového přehledu]", "; ")
        End If
        If lblQuery.Text <> "" Then panQueryByEntity.Visible = True
    End Sub

    Private Sub dd1_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles dd1.SelectedIndexChanged
        SaveCurrentSettings()
        RefreshData()
    End Sub

    Private Sub dd2_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles dd2.SelectedIndexChanged
        SaveCurrentSettings()
        RefreshData()

        hidToggle.Value = "1"
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

  

   

    

    

   

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        grid1.Rebind(False)
    End Sub

    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Pojmenovaný filtr--")
        
        basUI.SelectDropdownlistValue(Me.j70ID, intDef.ToString)
        
    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        hidJ70ID.Value = Me.j70ID.SelectedValue
        Master.Factory.j03UserBL.SetUserParam("p31-j70id", Me.j70ID.SelectedValue)
        grid1.Rebind(False)
    End Sub

    Private Sub p31_sumgrid_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        With Me.j70ID
            If .SelectedIndex > 0 Then
                hidJ70ID.Value = .SelectedValue
                .ToolTip = .SelectedItem.Text
                Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & .SelectedValue
                Me.clue_query.Visible = True
            Else
                Me.clue_query.Visible = False
                hidJ70ID.Value = ""
            End If
        End With

        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
        basUIMT.RenderQueryCombo(Me.j70ID)


    End Sub

    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        Select Case hidHardRefreshFlag.Value
            Case "pdf"
                GridExport("pdf")
            Case "xls"
                GridExport("xls")
            Case "doc"
                GridExport("doc")
            Case Else
                grid1.Rebind(False)
        End Select

        hidHardRefreshFlag.Value = ""
        hidHardRefreshPID.Value = ""
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_sumgrid-pagesize", Me.cbxPaging.SelectedValue)
        RefreshData()
    End Sub

    Private Sub chkFirstLastCount_CheckedChanged(sender As Object, e As EventArgs) Handles chkFirstLastCount.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_sumgrid-chkFirstLastCount", BO.BAS.GB(Me.chkFirstLastCount.Checked))
        RefreshData()
    End Sub

    
    Private Sub cbxMaxMinAll_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxMaxMinAll.SelectedIndexChanged
        hidToggle.Value = "1"
        If colsDest.SelectedItem Is Nothing Then
            Master.Notify("Není vybrán sloupec.", NotifyLevel.WarningMessage)
        Else
            Dim s As String = lisDD.Where(Function(p) p.ColumnName = colsDest.SelectedValue).First.ColumnHeader
            colsDest.SelectedItem.Text = s & " (" & UCase(cbxMaxMinAll.SelectedValue) & ")"
            cbxMaxMinAll.SelectedIndex = 0
            SaveCurrentSettings()
        End If
        hidToggle.Value = "1"
    End Sub
End Class