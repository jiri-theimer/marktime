Public Class j03_myprofile_defaultpage
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub j03_myprofile_defaultpage_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .HeaderText = "Nastavit si osobní (výchozí) stránku"
                .HeaderIcon = "Images/plugin_32.png"
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With

            With Me.opgPersonalPage
                .DataSource = Master.Factory.x31ReportBL.GetList_PersonalPageSource()
                .DataBind()
                Dim item As New ListItem("Ponechat rozhodnutí na nastavení systému", "")
                'item.Attributes("style") = "color:blue;"
                .Items.Insert(0, item)
            End With
            Dim cRec As BO.j03User = Master.Factory.j03UserBL.Load(Master.Factory.SysUser.PID)
            If cRec.j03Aspx_PersonalPage = "" Then
                Me.opgPersonalPage.SelectedIndex = 0
            Else
                basUI.SelectRadiolistValue(Me.opgPersonalPage, cRec.j03Aspx_PersonalPage)
            End If
       

        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Dim cRec As BO.j03User = Master.Factory.j03UserBL.Load(Master.Factory.SysUser.PID)
            cRec.j03Aspx_PersonalPage = Me.opgPersonalPage.SelectedValue
            If Master.Factory.j03UserBL.Save(cRec) Then
                Master.CloseAndRefreshParent("j03_myprofile_defaultpage")
            Else
                Master.Notify(Master.Factory.j03UserBL.ErrorMessage, NotifyLevel.ErrorMessage)
            End If

        End If
    End Sub
End Class