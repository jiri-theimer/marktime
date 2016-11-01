Public Class SubForm
    Inherits System.Web.UI.MasterPage
    Private Property _Factory As BL.Factory = Nothing

    Public ReadOnly Property Factory As BL.Factory
        Get
            Return _Factory
        End Get
    End Property
    Public Property HeaderText() As String
        Get
            Return hidPageTitle.Value
        End Get
        Set(ByVal value As String)
            hidPageTitle.Value = value
            pageTitle.Text = value

        End Set
    End Property
    Public Property DataPID() As Integer
        Get
            Return BO.BAS.IsNullInt(hidDataPID.Value)
        End Get
        Set(ByVal value As Integer)
            hidDataPID.Value = value.ToString
        End Set
    End Property
    Public Property HelpTopicID As String
    Public Property SiteMenuValue() As String
        Get
            Return Me.hidSiteMenuValue.Value
        End Get
        Set(ByVal value As String)
            Me.hidSiteMenuValue.Value = value
        End Set
    End Property
    Public Property IsMenuNever As Boolean
        Get
            Return BO.BAS.BG(Me.hidMenuNever.Value)
        End Get
        Set(value As Boolean)
            Me.hidMenuNever.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public ReadOnly Property IsTopWindow As Boolean
        Get
            If mm1.ItemsCount > 0 Then Return True Else Return False

        End Get
    End Property

    Public Sub StopPage(ByVal strMessage As String, Optional ByVal bolErrorInfo As Boolean = True, Optional ByVal strNeededPerms As String = "", Optional bolModalPage As Boolean = False)
        Server.Transfer("~/stoppage.aspx?err=" & BO.BAS.GB(bolErrorInfo) & "&message=" & Server.UrlEncode(strMessage) & "&neededperms=" & strNeededPerms & "&modal=" & BO.BAS.GB(bolModalPage), False)

    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If HttpContext.Current.User.Identity.IsAuthenticated And _Factory Is Nothing Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            _Factory = New BL.Factory(, strLogin)
            If _Factory.SysUser Is Nothing Then DoLogOut()
            basUI.PingAccessLog(_Factory, Request)
        End If

    End Sub
    Private Sub DoLogOut()
        Response.Redirect("~/Account/Login.aspx?autologout=1") 'automatické odhlášení
    End Sub
    Public Sub Notify(ByVal strText As String, Optional ByVal msgLevel As NotifyLevel = NotifyLevel.InfoMessage, Optional ByVal strTitle As String = "")
        basUI.NotifyMessage(Me.notify1, strText, msgLevel)
    End Sub
    

    
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Me.IsMenuNever Then mm1.ClearAll() : Return

            If Request.Item("saw") = "1" Or basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                mm1.RefreshData(_Factory, Me.HelpTopicID, Me.SiteMenuValue)
            Else
                mm1.ClearAll()
            End If
        End If

        If mm1.ItemsCount > 0 Then placeFavicon.Controls.Add(New LiteralControl("<link href='Images/favicon.ico' rel='shortcut icon' type='image/x-icon' />"))
    End Sub
End Class