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
                .AddToolbarButton("XLS export", "xls", , "Images/xls.png")
            End With

        End If
    End Sub

    Private Sub pivot1_NeedDataSource(sender As Object, e As Telerik.Web.UI.PivotGridNeedDataSourceEventArgs) Handles pivot1.NeedDataSource


        Dim dt As New DataTable
        dt.ReadXmlSchema(Master.Factory.x35GlobalParam.TempFolder & "\xx_schema.xml")
        dt.ReadXml(Master.Factory.x35GlobalParam.TempFolder & "\xx_data.xml")

        pivot1.DataSource = dt

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