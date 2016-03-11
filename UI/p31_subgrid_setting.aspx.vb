Public Class p31_subgrid_setting
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p31_subgrid_setting_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("masterprefix") = Request.Item("masterprefix")

            Dim strKey As String = "p31_subgrid-j74id_" & ViewState("masterprefix")

            With Master
                If ViewState("masterprefix") = "" Then .StopPage("masterprefix is missing")
                .HeaderIcon = "Images/griddesigner_32.png"
                .HeaderText = "Nastavení worksheet přehledu"
                .AddToolbarButton("Uložit nastavení", "save", , "Images/save.png")

                .Factory.j03UserBL.InhaleUserParams(strKey)
            End With


            Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet)
            j74ID.DataSource = lisJ74.Where(Function(p) p.j74IsSystem = False Or (p.j74MasterPrefix = ViewState("masterprefix") And p.j74IsSystem = True))
            j74ID.DataBind()

            Me.j74ID.SelectedValue = Master.Factory.j03UserBL.GetUserParam(strKey)
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            If BO.BAS.IsNullInt(Me.j74ID.SelectedValue) = 0 Then
                Master.Notify("Musíte vybrat šablonu sloupců.", NotifyLevel.WarningMessage)
                Return
            End If
            Dim strKey As String = "p31_subgrid-j74id_" & ViewState("masterprefix")
            Master.Factory.j03UserBL.SetUserParam(strKey, Me.j74ID.SelectedValue)
            Master.CloseAndRefreshParent("p31_subgrid-setting")
        End If
    End Sub

   

    Private Sub cmdGridDesigner_Click(sender As Object, e As EventArgs) Handles cmdGridDesigner.Click
        Server.Transfer("grid_designer.aspx?prefix=p31&pid=" & Me.j74ID.SelectedValue & "&masterprefix=" & ViewState("masterprefix"), False)
    End Sub
End Class