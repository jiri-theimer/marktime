Public Class o10_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub o10_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "o10_framework"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Nástěnka"
                .SiteMenuValue = "o10"
                
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("o10_framework-query-closed")
                    
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)



            End With
            With Master.Factory.j03UserBL
                
                basUI.SelectDropdownlistValue(Me.cbxValidity, .GetUserParam("o10_framework-query-closed", "1"))

            End With


            With Master.Factory.j03UserBL

            End With
        End If
    End Sub

    Private Sub cbxValidity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxValidity.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("o10_framework-query-closed", Me.cbxValidity.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("o10_framework.aspx" & basUI.GetCompleteQuerystring(Request, True))
    End Sub
End Class