Public Class x18_readonly
    Inherits System.Web.UI.UserControl
    Private _lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RefreshData(x29id As BO.x29IdEnum, intRecordPID As Integer, lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding))
        Me.hidRecordPID.Value = intRecordPID.ToString
        Me.hidX29ID.Value = CInt(x29id).ToString

        _lisX19 = lisX19

        Dim x18IDs As List(Of Integer) = lisX19.Select(Function(p) p.x18ID).Distinct.ToList
        rp1.DataSource = x18IDs
        rp1.DataBind()

    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim intX18ID As Integer = CType(e.Item.DataItem, Integer)
        CType(e.Item.FindControl("x18Name"), Label).Text = _lisX19.Where(Function(p) p.x18ID = intX18ID).First.x18Name & ":"
        CType(e.Item.FindControl("items"), Label).Text = String.Join(", ", _lisX19.Where(Function(p) p.x18ID = intX18ID).Select(Function(p) p.x25Name))
    End Sub
End Class