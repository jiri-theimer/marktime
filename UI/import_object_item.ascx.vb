Public Class import_object_item
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InhaleObjectRecord(strGUID As String, strPrefix As String)
        hidGUID.Value = strGUID : hidPrefix.Value = strPrefix

        Dim lis As IEnumerable(Of BO.p85TempBox) = Me.Factory.p85TempBoxBL.GetList(strGUID)


    End Sub
End Class