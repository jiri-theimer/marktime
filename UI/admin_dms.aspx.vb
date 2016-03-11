Public Class admin_dms
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub admin_dms_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderText = "Nastavení DMS"
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With


            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        With Master.Factory.x35GlobalParam
            Me.Dropbox_IsUse.Checked = BO.BAS.BG(.GetValueString("Dropbox_IsUse", "0"))
         
        End With
    End Sub

    Private Sub admin_dms_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
           
            With Master.Factory.x35GlobalParam

                Dim cRec As BO.x35GlobalParam = .Load("Dropbox_IsUse", True)
                cRec.x35Value = BO.BAS.GB(Me.Dropbox_IsUse.Checked)

                .Save(cRec)


            End With

            Master.CloseAndRefreshParent("dms")
        End If
    End Sub
End Class