Imports Telerik.Web.UI
Public Class p31_sumgrid_pivot
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Private Property _lisAllSums As List(Of BO.PivotSumField) = Nothing
    Private ReadOnly Property lisAllSums As List(Of BO.PivotSumField)
        Get
            If _lisAllSums Is Nothing Then
                _lisAllSums = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()
            End If
            Return _lisAllSums
        End Get

    End Property

    Private Sub p31_sumgrid_pivot_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master

    End Sub

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then
            With Master
                If Not System.IO.File.Exists(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_schema.xml") Then
                    .StopPage(Master.Factory.SysUser.PID.ToString & "_pivot_schema.xml not found.")
                End If
                .AddToolbarButton("XLS", "xls", , "Images/xls.png")
                .AddToolbarButton("PDF", "pdf", , "Images/pdf.png", False, "javascript:exportRadPivotGrid()")
            End With


            SetupFields()
        End If

        With RadClientExportManager1.PdfSettings
            .Fonts.Clear()
            .Fonts.Add("Arial Unicode MS", "Fonts/ArialUnicodeMS.ttf")
        End With

        For i As Integer = 1 To 9
            If Not pivot1.Fields.GetFieldByUniqueName("c" & i.ToString) Is Nothing Then
                pivot1.Fields.Remove(pivot1.Fields.GetFieldByUniqueName("c" & i.ToString))
            End If
            If Not pivot1.Fields.GetFieldByUniqueName("s" & i.ToString) Is Nothing Then
                pivot1.Fields.Remove(pivot1.Fields.GetFieldByUniqueName("s" & i.ToString))
            End If
        Next
        If Not pivot1.Fields.GetFieldByUniqueName("row2") Is Nothing Then
            pivot1.Fields.Remove(pivot1.Fields.GetFieldByUniqueName("row2"))
        End If
    End Sub

    Private Sub SetupFields()

        Dim lisAllCols As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)

        Dim dt As New DataTable
        dt.ReadXmlSchema(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_schema.xml")
        dt.ReadXml(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_data.xml")

        With pivot1.Fields.GetFieldByUniqueName("row1")
            .DataField = dt.Columns(0).ColumnName
            .UniqueName = dt.Columns(0).ColumnName
            Dim col As BO.GridColumn = lisAllCols.Where(Function(p) p.ColumnName = dt.Columns(0).ColumnName).First
            .Caption = col.ColumnHeader
        End With
        If dt.Columns(1).ColumnName <> "pid" Then
            With pivot1.Fields.GetFieldByUniqueName("row2")
                .DataField = dt.Columns(1).ColumnName
                .UniqueName = dt.Columns(1).ColumnName
                Dim col As BO.GridColumn = lisAllCols.Where(Function(p) p.ColumnName = dt.Columns(1).ColumnName).First
                .Caption = col.ColumnHeader
            End With
        
        End If
        Dim x As Integer = 0, y As Integer = 0
        For Each dc As DataColumn In dt.Columns
            If Left(dc.ColumnName, 3) = "col" Then
                Dim col As BO.GridColumn = lisAllCols.Where(Function(p) "col" & p.ColumnName = dc.ColumnName).First
                x += 1
                With pivot1.Fields.GetFieldByUniqueName("c" & x.ToString)
                    .DataField = dc.ColumnName
                    .UniqueName = dc.ColumnName
                    .Caption = col.ColumnHeader
                End With
            End If

            If Left(dc.ColumnName, 3) = "sum" Then
                Dim col As BO.PivotSumField = Me.lisAllSums.Where(Function(p) "sum" & p.FieldTypeID.ToString = dc.ColumnName).First
                y += 1
                With pivot1.Fields.GetFieldByUniqueName("s" & y.ToString)
                    .DataField = dc.ColumnName
                    .UniqueName = dc.ColumnName
                    .Caption = col.Caption

                End With
            End If

        Next
        
    End Sub

    Private Sub pivot1_NeedDataSource(sender As Object, e As Telerik.Web.UI.PivotGridNeedDataSourceEventArgs) Handles pivot1.NeedDataSource


        Dim dt As New DataTable
        dt.ReadXmlSchema(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_schema.xml")
        dt.ReadXml(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_data.xml")


        pivot1.DataSource = dt

    End Sub
    Private Sub pivot1_CellDataBound(sender As Object, e As Telerik.Web.UI.PivotGridCellDataBoundEventArgs) Handles pivot1.CellDataBound
        If TypeOf e.Cell Is PivotGridRowHeaderCell Then
            e.Cell.Text = Replace(e.Cell.Text, "Grand Total", "Celkový součet")
        End If
        If TypeOf e.Cell Is PivotGridDataCell Then
            e.Cell.HorizontalAlign = HorizontalAlign.Right
        End If

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            e.Cell.Text = Replace(e.Cell.Text, "Sum of", "")
            e.Cell.Text = Replace(e.Cell.Text, "Grand Total", "Celkový součet")

          
            If Not e.Cell.Field Is Nothing Then
                If Left(e.Cell.Field.UniqueName, 3) = "sum" Then
                    e.Cell.Text = e.Cell.Field.Caption
                    e.Cell.ToolTip = e.Cell.Field.DataField
                End If
                
            End If
            If Left(Trim(e.Cell.Text), 3) = "sum" Then
                Dim col As BO.PivotSumField = Me.lisAllSums.Where(Function(p) "sum" & p.FieldTypeID.ToString = Trim(e.Cell.Text)).First
                e.Cell.Text = col.Caption

            End If

        End If


        
      
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        Select Case strButtonValue
            Case "xls"
                With pivot1.ExportSettings
                    .FileName = "MARKTIME_PIVOT"
                    .IgnorePaging = True
                    .OpenInNewWindow = True
                End With
                pivot1.ExportToExcel()
        End Select
    End Sub
End Class