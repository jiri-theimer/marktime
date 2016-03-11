Public Class _default1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Server.Transfer("p31_framework_mobile.aspx", False)
    End Sub

End Class