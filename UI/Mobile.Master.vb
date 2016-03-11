Public Class Mobile
    Inherits System.Web.UI.MasterPage

    Public Property _Factory As BL.Factory = Nothing

    Public ReadOnly Property Factory As BL.Factory
        Get
            Return _Factory
        End Get
    End Property
    Public Property DataPID() As Integer
        Get
            Return BO.BAS.IsNullInt(hidDataPID.Value)
        End Get
        Set(ByVal value As Integer)
            hidDataPID.Value = value.ToString
        End Set
    End Property
    Public Property MenuPrefix As String
        Get
            Return Me.hidMenuPrefix.Value
        End Get
        Set(value As String)
            Me.hidMenuPrefix.Value = value
        End Set
    End Property



    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If HttpContext.Current.User.Identity.IsAuthenticated And _Factory Is Nothing Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            _Factory = New BL.Factory(, strLogin)
            If _Factory.SysUser Is Nothing Then DoLogOut()

            PersonalizeMenu()


        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub PersonalizeMenu()
        miUser.Text = "<span class='glyphicon glyphicon-user'></span>"
        With _Factory.SysUser
            If .Person = "" Then
                miUser.Text += .j03Login
            Else
                miUser.Text += .Person
            End If
            miUser.Text += "<span class='caret'></span>"
        End With
       
    End Sub

    Public Property PageTitle() As String
       
    Public Property SiteMenuValue() As String
    Public Property HelpTopicID As String

    Private Sub DoLogOut()
        Response.Redirect("~/Account/Login.aspx?autologout=1") 'automatické odhlášení
    End Sub

    Public Sub Notify(ByVal strText As String, Optional ByVal msgLevel As NotifyLevel = NotifyLevel.InfoMessage, Optional ByVal strTitle As String = "")
        basUI.NotifyMessage(Me.notify1, strText, msgLevel)
    End Sub
    Public Sub TestNeededPermission(neededPerm As BO.x53PermValEnum)
        If _Factory Is Nothing Then Return

        If Not _Factory.TestPermission(neededPerm) Then
            StopPage("Nedisponujete dostatečným oprávněním pro zobrazení této stránky.", True)
        End If
    End Sub


    Public Sub StopPage(ByVal strMessage As String, Optional ByVal bolErrorInfo As Boolean = True)
        Server.Transfer("~/stoppage_site.aspx?&err=" & BO.BAS.GB(bolErrorInfo) & "&message=" & Server.UrlEncode(strMessage), False)
    End Sub
End Class