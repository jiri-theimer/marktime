Imports Telerik.Web.UI
Public Class p31_sumgrid_pivot
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

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
    End Sub

    Private Sub SetupFields()
        Dim lisAllSums As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()
        Dim lisAllCols As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)

        Dim dt As New DataTable
        dt.ReadXmlSchema(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_schema.xml")
        dt.ReadXml(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_data.xml")

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
                Dim col As BO.PivotSumField = lisAllSums.Where(Function(p) "sum" & p.FieldTypeID.ToString = dc.ColumnName).First
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

        If TypeOf e.Cell Is PivotGridColumnHeaderCell Then
            e.Cell.Text = Replace(e.Cell.Text, "Sum of", "")
            e.Cell.Text = Replace(e.Cell.Text, "Grand Total", "Celkový součet")

            

        End If
        If TypeOf e.Cell Is PivotGridRowHeaderCell Then
            e.Cell.Text = Replace(e.Cell.Text, "Grand Total", "Celkový součet")
        End If
        If TypeOf e.Cell Is PivotGridDataCell Then
            e.Cell.HorizontalAlign = HorizontalAlign.Right
        End If
        If TypeOf e.Cell Is PivotGridHeaderCell Then

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