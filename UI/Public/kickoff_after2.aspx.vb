Public Class kickoff_after2
    Inherits System.Web.UI.Page
    Private _Factory As BL.Factory

    Private Sub kickoff_after2_Init(sender As Object, e As EventArgs) Handles Me.Init
        _Factory = New BL.Factory(, "mtservice")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.AppHost.Text = Context.Request.Url.GetLeftPart(UriPartial.Authority)
        End If
    End Sub

    Private Sub kickoff_after2_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.panSMTP.Visible = Me.chkSmtp.Checked
    End Sub

    Private Sub cmdGo_Click(sender As Object, e As EventArgs) Handles cmdGo.Click

        With _Factory.x35GlobalParam
            .UpdateValue("Upload_Folder", Me.txtUploadFolder.Text)
            If Me.chkSmtp.Checked Then
                .UpdateValue("IsUseWebConfigSetting", "0")
                .UpdateValue("SMTP_SenderAddress", Me.SMTP_SenderAddress.Text)
                .UpdateValue("SMTP_Server", Me.SMTP_Server.Text)
                .UpdateValue("SMTP_Login", Me.SMTP_Login.Text)
                .UpdateValue("SMTP_IsVerify", BO.BAS.GB(Me.SMTP_IsVerify.Checked))
                .UpdateValue("SMTP_Password", Me.SMTP_Password.Text)

                .UpdateValue("AppHost", Me.AppHost.Text)
            End If

            Response.Redirect("../default.aspx")
        End With
    End Sub
End Class