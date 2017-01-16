Imports Telerik.Web.UI

Public Class p31_pivot
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Public Property CurrentJ70ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j70ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j70ID, value.ToString)
        End Set
    End Property

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)

        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_pivot"

    End Sub
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                ViewState("masterprefix") = Request.Item("masterprefix")
                ViewState("masterpid") = Request.Item("masterpid")
                If ViewState("masterprefix") <> "" Then
                    panQueryByEntity.Visible = True
                    Me.lblEntity.Text = .Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(ViewState("masterprefix")), BO.BAS.IsNullInt(ViewState("masterpid")))
                    Select Case ViewState("masterprefix")
                        Case "p41" : imgEntity.ImageUrl = "Images/project_32.png"
                        Case "j02" : imgEntity.ImageUrl = "Images/person_32.png"
                        Case "p28" : imgEntity.ImageUrl = "Images/contact_32.png"
                        Case "p91" : imgEntity.ImageUrl = "Images/invoice_32.png"
                    End Select

                Else
                    panQueryByEntity.Visible = False
                End If

                .PageTitle = "Worksheet PIVOT"
                .SiteMenuValue = "p31_pivot"

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_pivot-pagesize")
                    .Add("p31_pivot-query-p34id")
                    .Add("p31-j70id")
                    .Add("p31_grid-period")
                    .Add("periodcombo-custom_query")
                    .Add("p31_pivot-row1")
                    .Add("p31_pivot-row2")
                    .Add("p31_pivot-row3")
                    .Add("p31_pivot-col1")
                    .Add("p31_pivot-sum1")
                    .Add("p31_pivot-sum2")
                    .Add("p31_pivot-sum3")
                    .Add("p31_pivot-sum4")
                End With
                Dim lisSumFields As List(Of BO.PivotSumField) = .Factory.j75DrillDownTemplateBL.ColumnsPallete()
                If Not .Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                    RemoveItems(Me.col1, "3101,3102,3103")
                    ''RemoveItems(Me.row1, "3101,3102,3103")
                    ''RemoveItems(Me.row2, "3101,3102,3103")
                    ''RemoveItems(Me.row3, "3101,3102,3103")
                End If

                Dim lisFields As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("p31_pivot-pagesize", "20"))
                    InhaleField(1, .GetUserParam("p31_pivot-row1", "Person"), lisFields)
                    InhaleField(2, .GetUserParam("p31_pivot-row2"), lisFields)
                    InhaleField(3, .GetUserParam("p31_pivot-row3"), lisFields)

                    basUI.SelectDropdownlistValue(Me.col1, .GetUserParam("p31_pivot-col1"))

                    SetupSumCombo(Me.sum1, lisSumFields, .GetUserParam("p31_pivot-sum1", "1"))
                    SetupSumCombo(Me.sum2, lisSumFields, .GetUserParam("p31_pivot-sum2"))
                    SetupSumCombo(Me.sum3, lisSumFields, .GetUserParam("p31_pivot-sum3"))
                    SetupSumCombo(Me.sum4, lisSumFields, .GetUserParam("p31_pivot-sum4"))

                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("p31_grid-period")
                  
                End With

                cmdQuery.Visible = .Factory.TestPermission(BO.x53PermValEnum.GR_GridTools)

            End With
            SetupJ70Combo(BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam("p31-j70id")))
            SetupGridFields()
            SetupPivotGrid()
        Else
            SetupGridFields()
        End If
    End Sub
    Private Sub SetupSumCombo(cbx As DropDownList, lisSumFields As List(Of BO.PivotSumField), strDefVal As String)
        cbx.DataSource = lisSumFields
        cbx.DataBind()
        cbx.Items.Insert(0, "")
        basUI.SelectDropdownlistValue(cbx, strDefVal)
    End Sub
    Private Sub RemoveItems(cbx As DropDownList, strVals As String)
        For Each s In Split(strVals, ",")
            With cbx.Items
                If Not .FindByValue(s) Is Nothing Then .Remove(.FindByValue(s))
            End With
        Next
    End Sub
    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Pojmenovaný filtr--")
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

    Private Sub SetupPivotGrid()
        With Me.pivot1
            .PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)

        End With
    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            .j70ID = Me.CurrentJ70ID
            If period1.SelectedValue <> "" Then
                .DateFrom = period1.DateFrom
                .DateUntil = period1.DateUntil
            End If
            Select Case ViewState("masterprefix")
                Case "p41"
                    .p41ID = BO.BAS.IsNullInt(ViewState("masterpid"))
                Case "j02"
                    .j02ID = BO.BAS.IsNullInt(ViewState("masterpid"))
                Case "p28"
                    .p28ID_Client = BO.BAS.IsNullInt(ViewState("masterpid"))
                Case "p91"
                    .p91ID = BO.BAS.IsNullInt(ViewState("masterpid"))
            End Select
        End With
    End Sub

    Private Sub pivot1_CellDataBound(sender As Object, e As Telerik.Web.UI.PivotGridCellDataBoundEventArgs) Handles pivot1.CellDataBound

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            e.Cell.Text = Replace(e.Cell.Text, "Sum of", "")
            e.Cell.Text = Replace(e.Cell.Text, "Grand Total", "Celkový součet")

            Dim sums As List(Of BO.PivotSumField) = GetSums()
            If sums.Count > 1 Then
                For x As Integer = 0 To sums.Count - 1
                    e.Cell.Text = Replace(e.Cell.Text, "Sum" & (x + 1).ToString, sums(x).Caption)
                Next
            End If

        End If
        If TypeOf e.Cell Is PivotGridRowHeaderCell Then
            e.Cell.Text = Replace(e.Cell.Text, "Grand Total", "Celkový součet")
        End If

    End Sub



    Private Sub pivot1_ItemCreated(sender As Object, e As PivotGridItemCreatedEventArgs) Handles pivot1.ItemCreated
        If TypeOf e.Item Is PivotGridPagerItem Then
            If Not e.Item.FindControl("PageSizeComboBox") Is Nothing Then
                e.Item.FindControl("PageSizeComboBox").Visible = False
            End If
        End If
    End Sub

    Private Function GetRows() As List(Of BO.GridColumn)
        Dim lis As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)

        Dim rows As New List(Of BO.GridColumn)
        If Me.hidRow1.Value <> "" Then
            rows.Add(InhaleField(1, Me.hidRow1.Value, lis))
        End If
        If Me.hidRow2.Value <> "" Then
            rows.Add(InhaleField(2, Me.hidRow2.Value, lis))
        End If
        If Me.hidRow3.Value <> "" Then
            rows.Add(InhaleField(3, Me.hidRow3.Value, lis))
        End If
        Return rows
    End Function
    Private Function GetCols() As List(Of BO.PivotRowColumnField)
        Dim cols As New List(Of BO.PivotRowColumnField)
        If Me.col1.SelectedValue <> "" Then
            cols.Add(New BO.PivotRowColumnField(CInt(col1.SelectedValue)))
        End If
       
        Return cols
    End Function
    Private Function GetSums() As List(Of BO.PivotSumField)
        Dim sums As New List(Of BO.PivotSumField)
        If Me.sum1.SelectedValue <> "" Then
            sums.Add(New BO.PivotSumField(CInt(sum1.SelectedValue)))
        End If
        If Me.sum2.SelectedValue <> "" Then
            sums.Add(New BO.PivotSumField(CInt(sum2.SelectedValue)))
        End If
        If Me.sum3.SelectedValue <> "" Then
            sums.Add(New BO.PivotSumField(CInt(sum3.SelectedValue)))
        End If
        If Me.sum4.SelectedValue <> "" Then
            sums.Add(New BO.PivotSumField(CInt(sum4.SelectedValue)))
        End If
        Return sums
    End Function


    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_pivot-pagesize", Me.cbxPaging.SelectedValue)

        pivot1.PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
        If pivot1.CurrentPageIndex > 0 Then pivot1.CurrentPageIndex = 0
        pivot1.Rebind()
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        pivot1.Rebind()
    End Sub

    
    Private Sub pivot1_NeedDataSource(sender As Object, e As Telerik.Web.UI.PivotGridNeedDataSourceEventArgs) Handles pivot1.NeedDataSource
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim rows As List(Of BO.GridColumn) = GetRows(), sums As List(Of BO.PivotSumField) = GetSums()
        Dim cols As List(Of BO.PivotRowColumnField) = GetCols()
        If (rows.Count = 0 And cols.Count = 0) Or sums.Count = 0 Then Return

        Dim lis As IEnumerable(Of BO.PivotRecord) = Master.Factory.p31WorksheetBL.GetList_Pivot(rows, cols, sums, mq)
       
        If Not lis Is Nothing Then
            pivot1.DataSource = lis
        Else
            Master.Notify(Master.Factory.p31WorksheetBL.ErrorMessage, NotifyLevel.ErrorMessage)
        End If

    End Sub

    Private Sub SetupGridFields()
        Dim x As Integer = 1
        With Me.pivot1
            .Fields.Clear()
            For Each row In GetRows()
                Dim item As New PivotGridRowField()
                .Fields.Add(item)
                item.DataField = "Row" & x.ToString
                item.UniqueName = "Row" & x.ToString
                item.Caption = row.ColumnHeader

                x += 1
            Next
            x = 1
            For Each col In GetCols()
                Dim item As New PivotGridColumnField()
                .Fields.Add(item)
                item.DataField = "Col" & x.ToString
                item.UniqueName = "Col" & x.ToString
                item.Caption = col.Caption

                x += 1
            Next
            x = 1
            For Each sum In GetSums()
                Dim item As New PivotGridAggregateField
                .Fields.Add(item)
                item.DataField = "Sum" & x.ToString
                item.UniqueName = "Sum" & x.ToString
                item.DataFormatString = "{0:F2}"
                item.TotalFormatString = "{0:F2}"
                item.Caption = sum.Caption


                x += 1
            Next
        End With

    End Sub

    Private Sub RefreshData()
        pivot1.Rebind()
    End Sub

    

    

    Private Sub sum1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sum1.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_pivot-sum1", Me.sum1.SelectedValue)
        RefreshData()
    End Sub

    Private Sub sum2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sum2.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_pivot-sum2", Me.sum2.SelectedValue)
        RefreshData()
    End Sub
    Private Sub sum3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sum3.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_pivot-sum3", Me.sum3.SelectedValue)
        RefreshData()
    End Sub
    Private Sub sum4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sum4.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_pivot-sum4", Me.sum4.SelectedValue)
        RefreshData()
    End Sub

    Private Sub col1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles col1.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_pivot-col1", Me.col1.SelectedValue)
        RefreshData()
    End Sub

    Private Sub p31_pivot_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        basUIMT.RenderQueryCombo(Me.j70ID)
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With

        If Me.hidRow2.Value = "" Then cmdClear2.Visible = False : linkRow2.Text = "Vybrat pole 2" Else cmdClear2.Visible = True
        If Me.hidRow3.Value = "" Then cmdClear3.Visible = False : linkRow3.Text = "Vybrat pole 3" Else cmdClear3.Visible = True
    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31-j70id", Me.CurrentJ70ID.ToString)
        ReloadPage()
    End Sub
    Private Sub ReloadPage()
        Response.Redirect("p31_pivot.aspx" & basUI.GetCompleteQuerystring(Request,true))
    End Sub
   

    
    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        GridExport("xls")
       

    End Sub

   
    Private Sub cmdRebind_Click(sender As Object, e As EventArgs) Handles cmdRebind.Click
        pivot1.Rebind()
    End Sub

    Private Sub GridExport(strFormat As String)
        With pivot1.ExportSettings
            .FileName = "MARKTIME_PIVOT"
            .IgnorePaging = True
            .OpenInNewWindow = True
        End With
        Select Case strFormat
            Case "xls"
                pivot1.ExportToExcel()
            Case "doc"
                pivot1.ExportToWord()
        End Select
       
    End Sub

    Private Sub cmdDOC_Click(sender As Object, e As EventArgs) Handles cmdDOC.Click
        GridExport("doc")
    End Sub

    Private Sub pivot1_PivotGridCellExporting(sender As Object, e As PivotGridCellExportingArgs) Handles pivot1.PivotGridCellExporting
     
        If TypeOf e.PivotGridCell Is PivotGridColumnHeaderCell Then

            e.ExportedCell.Style.Font.Bold = True

            For i As Integer = 1 To pivot1.Fields.Count
                If e.ExportedCell.ColIndex = i + 1 Then
                    e.ExportedCell.Value = pivot1.Fields(i).Caption

                End If

            Next
        Else
            If TypeOf e.PivotGridCell Is PivotGridRowHeaderCell Then
                e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Justify
            Else
                e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Right
            End If


        End If

    End Sub

   
    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        Dim lis As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)
        Select Case hidHardRefreshFlag.Value
            Case "field1"
                InhaleField(1, hidHardRefreshPID.Value, lis)
                Master.Factory.j03UserBL.SetUserParam("p31_pivot-row1", Me.hidRow1.Value)
            Case "field2"
                InhaleField(2, hidHardRefreshPID.Value, lis)
                Master.Factory.j03UserBL.SetUserParam("p31_pivot-row2", Me.hidRow2.Value)
            Case "field3"
                InhaleField(3, hidHardRefreshPID.Value, lis)
                Master.Factory.j03UserBL.SetUserParam("p31_pivot-row3", Me.hidRow3.Value)
        End Select
        hidHardRefreshFlag.Value = ""
        hidHardRefreshPID.Value = ""
        RefreshData()
    End Sub
    Private Function InhaleField(intRowIndex As Integer, strColumnName As String, lisAllFields As List(Of BO.GridColumn)) As BO.GridColumn
        Dim c As BO.GridColumn = lisAllFields.Where(Function(p) p.ColumnName = strColumnName)
        Select Case intRowIndex
            Case 1 : Me.hidRow1.Value = "" : Me.linkRow1.Text = "Vybrat pole 1"
            Case 2 : Me.hidRow2.Value = "" : Me.linkRow2.Text = "Vybrat pole 2"
            Case 3 : Me.hidRow3.Value = "" : Me.linkRow3.Text = "Vybrat pole 3"
        End Select
        If c Is Nothing Then
            Return Nothing
        End If
        Select Case intRowIndex
            Case 1 : Me.hidRow1.Value = strColumnName : Me.linkRow1.Text = c.ColumnHeader
            Case 2 : Me.hidRow2.Value = strColumnName : Me.linkRow2.Text = c.ColumnHeader
            Case 3 : Me.hidRow3.Value = strColumnName : Me.linkRow3.Text = c.ColumnHeader
        End Select
        Return c
    End Function

    
    Private Sub cmdClear2_Click(sender As Object, e As ImageClickEventArgs) Handles cmdClear2.Click
        hidRow2.Value = ""
        RefreshData()
    End Sub

    Private Sub cmdClear3_Click(sender As Object, e As ImageClickEventArgs) Handles cmdClear3.Click
        hidRow3.Value = ""
        RefreshData()
    End Sub
End Class