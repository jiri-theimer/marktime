Public Class entity_framework_detail_setting
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentPrefix As String
        Get
            Return hidPrefix.Value
        End Get
        Set(value As String)
            hidPrefix.Value = value
        End Set
    End Property

    Private Sub entity_framework_detail_setting_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentPrefix = Request.Item("prefix")
            With Master
                .HeaderText = "Nastavení vzhledu stránky"
                .AddToolbarButton("Uložit změny", "ok")
            End With

            Dim lisPars As New List(Of String)
            With lisPars
                .Add(Me.CurrentPrefix + "_framework_detail-switchHeight")
            End With

            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                basUI.SelectRadiolistValue(Me.switchHeight, .GetUserParam(Me.CurrentPrefix + "_framework_detail-switchHeight", "auto"))
            End With
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework_detail-switchHeight", Me.switchHeight.SelectedValue)
            Master.CloseAndRefreshParent("setting")
        End If
    End Sub
End Class