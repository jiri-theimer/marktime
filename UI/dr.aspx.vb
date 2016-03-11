Public Class dr
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub dr_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.SiteMenuValue = "dashboard"
        If Not Page.IsPostBack Then
            If Request.Item("prefix") = "" Or Request.Item("pid") = "" Then
                Return
            End If
            ViewState("prefix") = Request.Item("prefix")
            ViewState("pid") = Request.Item("pid")
            Select Case ViewState("prefix")
                Case "p56"
                    Response.Redirect("p56_framework.aspx?pid=" & Request.Item("pid"))
                Case "o23"
                    If Master.Factory.TestPermission(BO.x53PermValEnum.GR_O23_Creator) Then
                        Response.Redirect("o23_framework.aspx?pid=" & Request.Item("pid"))
                    End If
                Case "p41"
                    If Master.Factory.SysUser.j04IsMenu_Project Then
                        Response.Redirect("p41_framework.aspx?pid=" & Request.Item("pid"))
                    End If
                Case "p28"
                    If Master.Factory.SysUser.j04IsMenu_Contact Then
                        Response.Redirect("p28_framework.aspx?pid=" & Request.Item("pid"))
                    End If
            End Select
            'pokud se nenajde jiná stránka, pak zobrazit přes cluetip zobrazení
            Dim strURL As String = "clue_" & ViewState("prefix") & "_record.aspx?pid=" & ViewState("pid") & "&dr=1"
            paneContent.ContentUrl = strURL

        End If

    End Sub

End Class