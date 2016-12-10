Public Class o10_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub o10_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "o10_framework"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim lisPars As New List(Of String)
            With Master
                .PageTitle = "Nástěnka"
                .SiteMenuValue = "o10_framework"

                Me.linkNew.Visible = .Factory.TestPermission(BO.x53PermValEnum.GR_O10_Creator)
                Me.cbxOwner.Visible = Me.linkNew.Visible
                With lisPars
                    .Add("o10_framework-query-closed")
                    .Add("o10_framework-query-owner")

                End With
            End With
            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                basUI.SelectDropdownlistValue(Me.cbxValidity, .GetUserParam("o10_framework-query-closed", "1"))
                basUI.SelectDropdownlistValue(Me.cbxOwner, .GetUserParam("o10_framework-query-owner", ""))
            End With
            If Not Me.cbxOwner.Visible Then Me.cbxOwner.SelectedIndex = 0

            RefreshData()

        End If
    End Sub

    Private Sub RefreshData()
        Dim mq As New BO.myQuery
        Select Case Me.cbxValidity.SelectedValue
            Case "1" : mq.Closed = BO.BooleanQueryMode.FalseQuery
            Case "2" : mq.Closed = BO.BooleanQueryMode.NoQuery
            Case "3" : mq.Closed = BO.BooleanQueryMode.TrueQuery
        End Select
        If Me.cbxOwner.SelectedValue = "1" Then mq.j02ID_Owner = Master.Factory.SysUser.j02ID
        Dim lis As IEnumerable(Of BO.o10NoticeBoard) = Master.Factory.o10NoticeBoardBL.GetList(mq)
        Dim bolAdmin As Boolean = Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin)

        For Each c In lis
            If bolAdmin Or c.j02ID_Owner = Master.Factory.SysUser.j02ID Then
                c.StyleDisplayEdit = "block"
            Else
                c.StyleDisplayEdit = "none"

            End If
        Next
        
        rp1.DataSource = lis
        rp1.DataBind()

    End Sub
    Private Sub cbxValidity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxValidity.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("o10_framework-query-closed", Me.cbxValidity.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("o10_framework.aspx" & basUI.GetCompleteQuerystring(Request, True))
    End Sub

    Private Sub cbxOwner_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxOwner.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("o10_framework-query-owner", Me.cbxOwner.SelectedValue)
        ReloadPage()
    End Sub
End Class