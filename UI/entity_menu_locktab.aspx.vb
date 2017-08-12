Public Class entity_menu_locktab
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub entity_menu_locktab_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim strPrefix As String = Request.Item("prefix")
            Dim strTab As String = Request.Item("tab")
            Dim strPage As String = Server.UrlDecode(Request.Item("page"))
            With Master.Factory.j03UserBL
                .SetUserParam(strPrefix & "_menu-remember-tab", "1")
                .SetUserParam(strPrefix & "_framework_detail-tab", strTab)
            End With
            Response.Redirect(strPage)
        End If

    End Sub

End Class