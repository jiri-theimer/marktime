Public Class mobile_search
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile

    Private Sub mobile_search_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.MenuPrefix = "search"
        

        With Me.p41id_search.radComboBoxOrig
            .RenderMode = Telerik.Web.UI.RenderMode.Mobile
            .DropDownWidth = Unit.Parse("350px")

        End With


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p41_framework_detail-pid")
                .Add("p28_framework_detail-pid")
                .Add("p91_framework_detail-pid")
                .Add("p56_framework_detail-pid")
                .Add("j02_framework_detail-pid")

            End With

            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)

            End With
        End If
    End Sub

End Class