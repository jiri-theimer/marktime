Imports System.Configuration
Imports System.Web.Configuration
Imports System.Net.Configuration

Public Class admin_smtp
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub admin_smtp_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderText = "Nastavení SMTP serveru"
                .HeaderIcon = "Images/settings_32.png"
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With


            RefreshRecord()
        End If
    End Sub
    Private Sub RefreshRecord()
        Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
        Dim settings As MailSettingsSectionGroup = DirectCast(config.GetSectionGroup("system.net/mailSettings"), MailSettingsSectionGroup)
        If Not settings Is Nothing Then
            With settings.Smtp.Network
                default_server.Text = .Host
            End With

        End If

        With Master.Factory.x35GlobalParam
            Me.AppHost.Text = .GetValueString("AppHost")
            If Me.AppHost.Text = "" Then
                Me.AppHost.Text = Context.Request.Url.GetLeftPart(UriPartial.Authority)
            End If
            Dim bolUseWC As Boolean = BO.BAS.BG(.GetValueString("IsUseWebConfigSetting", "1"))
            Me.SMTP_SenderAddress.Text = .GetValueString("SMTP_SenderAddress")
            Me.chkIsSMTP_UseWebConfigSetting.Checked = bolUseWC
            Me.SMTP_Server.Text = .GetValueString("SMTP_Server")
            Me.SMTP_Login.Text = .GetValueString("SMTP_Login")
            Me.SMTP_IsVerify.Checked = BO.BAS.BG(.GetValueString("SMTP_IsVerify", "0"))
            Me.SMTP_Password.Text = .GetValueString("SMTP_Password")

        End With

    End Sub

    Private Sub admin_smtp_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.panRec.Visible = Not Me.chkIsSMTP_UseWebConfigSetting.Checked
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            With Master.Factory.x35GlobalParam
                If Not chkIsSMTP_UseWebConfigSetting.Checked Then
                    'zkontrolovat vlastní nastavení
                    If Me.SMTP_IsVerify.Checked And Me.SMTP_Password.Text <> Me.txtVerify.Text Then
                        Master.Notify("Heslo nesouhlasí s ověřením.", NotifyLevel.ErrorMessage)
                        Return
                    End If
                    If Len(Trim(SMTP_Server.Text)) <= 2 Then
                        Master.Notify("Chybí adresa SMTP serveru.", NotifyLevel.ErrorMessage)
                        Return
                    End If
                End If
                If Trim(Me.SMTP_SenderAddress.Text) = "" Then
                    Master.Notify("Chybí adresa odesílatele.", NotifyLevel.WarningMessage)
                    Return
                End If

                Dim cRec As BO.x35GlobalParam = .Load("IsUseWebConfigSetting")
                cRec.x35Value = BO.BAS.GB(Me.chkIsSMTP_UseWebConfigSetting.Checked)
                .Save(cRec)

                cRec = .Load("SMTP_SenderAddress")
                cRec.x35Value = Me.SMTP_SenderAddress.Text
                .Save(cRec)

                cRec = .Load("SMTP_Server")
                cRec.x35Value = Me.SMTP_Server.Text
                .Save(cRec)

                cRec = .Load("SMTP_Login")
                cRec.x35Value = Me.SMTP_Login.Text
                .Save(cRec)

                cRec = .Load("SMTP_IsVerify")
                cRec.x35Value = BO.BAS.GB(SMTP_IsVerify.Checked)
                .Save(cRec)

                cRec = .Load("SMTP_Password")
                cRec.x35Value = Me.SMTP_Password.Text
                .Save(cRec)

                cRec = .Load("AppHost")
                cRec.x35Value = Me.AppHost.Text
                .Save(cRec)

            End With
            Master.CloseAndRefreshParent("smtp")
        End If
    End Sub
End Class