Public Class _Default
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master.Factory.SysUser
                If .j03IsSystemAccount Then
                    Response.Redirect("~/sys/membership_framework.aspx")
                End If
                If .PersonalPage <> "" Then
                    If .PersonalPage.IndexOf(".aspx") > 0 Then
                        If LCase(.PersonalPage) = "default.aspx" Then Response.Redirect("j03_mypage_greeting.aspx")
                        Response.Redirect(.PersonalPage)
                    Else
                        Server.Transfer("report_framework.aspx?defpage=1", False)
                    End If
                Else
                    Response.Redirect("j03_mypage_greeting.aspx")
                End If

            End With


            Master.SiteMenuValue = "dashboard"
       
        End If
    End Sub


    
End Class