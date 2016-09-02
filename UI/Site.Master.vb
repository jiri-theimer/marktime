Imports Telerik.Web.UI

Public Class Site
    Inherits System.Web.UI.MasterPage
    Public Property HelpTopicID As String = ""
    Public Property neededPermission As BO.x53PermValEnum

    Public Property _Factory As BL.Factory = Nothing

   

    Public ReadOnly Property Factory As BL.Factory
        Get
            Return _Factory
        End Get
    End Property

    Public Property PageTitle() As String
        Get            
            Return hidPageTitle.Value
        End Get
        Set(ByVal value As String)
            hidPageTitle.Value = "MARKTIME - " & value
        End Set
    End Property
    Public Property SiteMenuValue() As String
        Get
            Return menu1.SelectedValue
        End Get
        Set(ByVal value As String)
            If Not menu1.FindItemByValue(value) Is Nothing Then
                menu1.FindItemByValue(value).HighlightPath()
            
            End If

        End Set
    End Property
    Private Sub DoLogOut()
        Response.Redirect("~/Account/Login.aspx?autologout=1") 'automatické odhlášení
    End Sub

    Public Sub Notify(ByVal strText As String, Optional ByVal msgLevel As NotifyLevel = NotifyLevel.InfoMessage, Optional ByVal strTitle As String = "")
        basUI.NotifyMessage(Me.notify1, strText, msgLevel)
    End Sub

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If HttpContext.Current.User.Identity.IsAuthenticated And _Factory Is Nothing Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            _Factory = New BL.Factory(, strLogin)
            If _Factory.SysUser Is Nothing Then DoLogOut()

            PersonalizeMenu()
        End If
    End Sub

    Private Sub PersonalizeMenu()

        With _Factory.SysUser

            If .Person = "" Then
                menu1.FindItemByValue("me").Text = .j03Login
            Else
                menu1.FindItemByValue("me").Text = .Person.ToUpper
            End If
            menu1.ClickToOpen = .j03IsSiteMenuOnClick
            If .j03SiteMenuSkin > "" Then menu1.Skin = .j03SiteMenuSkin

            If .j04IsMenu_Worksheet Then
                menu1.FindItemByValue("p31").Visible = True
                ShowHideMI(Me.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot), "p31", "cmdP31_Pivot")

            Else
                menu1.Items.Remove(menu1.FindItemByValue("p31"))
            End If
            menu1.FindItemByValue("searchbox").Visible = .j04IsMenu_Project

            If Not .j04IsMenu_Project Then
                menu1.Items.Remove(menu1.FindItemByValue("p41"))
            End If
            If Not .j04IsMenu_Contact Then
                menu1.Items.Remove(menu1.FindItemByValue("p28"))
            End If
            If Not .j04IsMenu_People Then
                menu1.Items.Remove(menu1.FindItemByValue("j02"))
            End If
            If Not .j04IsMenu_Invoice Then
                menu1.Items.Remove(menu1.FindItemByValue("p91"))
            End If
            If Not _Factory.SysUser.j04IsMenu_MyProfile Then
                ShowHideMI(False, "me", "cmdMyProfile")
            End If
            If Not _Factory.SysUser.IsApprovingPerson Then
                If Not _Factory.SysUser.IsAdmin Then ShowHideMI(False, "p31", "cmdP31_Approving")
            End If
            If .MessagesCount = 0 Then
                menu1.Items.Remove(menu1.FindItemByValue("messages"))
            Else
                menu1.FindItemByValue("messages").Text = .MessagesCount.ToString
            End If

            If .HomeMenu <> "" Then
                menu1.FindItemByValue("dashboard").Text = .HomeMenu
                RenderHomeMenu()
            Else
                menu1.FindItemByValue("dashboard").Text = Resources.Site.Menu_UVOD
            End If
        End With

        If Not _Factory.SysUser.j04IsMenu_More Then
            menu1.Items.Remove(menu1.FindItemByValue("more"))
        Else
            With Factory
                ShowHideMI(.SysUser.j04IsMenu_Report, "more", "cmdReports")
                ShowHideMI(.SysUser.IsAdmin, "more", "cmdAdmin")
                ShowHideMI(.SysUser.IsAdmin, "more", "cmdWorkflow")
                ''ShowHideMI(.TestPermission(BO.x53PermValEnum.GR_P41_Owner), "more", "p47")
                ShowHideMI(.TestPermission(BO.x53PermValEnum.GR_P51_Admin), "more", "p51")
                ShowHideMI(.TestPermission(BO.x53PermValEnum.GR_P90_Reader), "more", "p90_framework")
                ShowHideMI(.TestPermission(BO.x53PermValEnum.GR_O23_Creator), "more", "o23")
                ShowHideMI((.TestPermission(BO.x53PermValEnum.GR_P48_Creator) Or .TestPermission(BO.x53PermValEnum.GR_P48_Reader)), "more", "p48")
                With menu1.FindItemByValue("more")
                    If .Items.Count <= 8 Then .GroupSettings.RepeatColumns = 1
                End With
            End With
        End If

        Dim cook As HttpCookie = Request.Cookies("MT50-CultureInfo")
        If Not cook Is Nothing Then
            Select Case cook.Value
                Case "en-US"
                    menu1.FindItemByValue("lang").ImageUrl = "Images/Flags/menu_uk.gif"
                    TranslateMenu()
                Case Else
                    menu1.FindItemByValue("lang").ImageUrl = "Images/Flags/menu_czech.gif"
            End Select
        End If
    End Sub

    Private Sub TranslateMenu()
        If Page.Culture.IndexOf("Czech") < 0 Then
            With menu1

                If Not .FindItemByValue("p41") Is Nothing Then .FindItemByValue("p41").Text = Resources.Site.Menu_PROJEKTY
                If Not .FindItemByValue("p28") Is Nothing Then .FindItemByValue("p28").Text = Resources.Site.Menu_KLIENTI
                If Not .FindItemByValue("j02") Is Nothing Then .FindItemByValue("j02").Text = Resources.Site.Menu_LIDE
                If Not .FindItemByValue("p91") Is Nothing Then .FindItemByValue("p91").Text = Resources.Site.Menu_FAKTURY
                If Not .FindItemByValue("more") Is Nothing Then .FindItemByValue("more").Text = Resources.Site.Menu_DALSI

                If Not .FindItemByValue("o23") Is Nothing Then .FindItemByValue("o23").Text = Resources.Site.Dokumenty
                If Not .FindItemByValue("cmdMyProfile") Is Nothing Then .FindItemByValue("cmdMyProfile").Text = Resources.Site.MujProfil
                .FindItemByValue("cmdChangePassword").Text = Resources.Site.ZmenitHeslo
                .FindItemByValue("cmdLogout").Text = Resources.Site.OdhlasitSe

                If Not .FindItemByValue("p31_framework") Is Nothing Then
                    .FindItemByValue("p31_framework").Text = Resources.Site.Zapisovat
                    .FindItemByValue("cmdP31_Grid").Text = Resources.Site.Grid
                    .FindItemByValue("cmdP31_Timer").Text = Resources.Site.cmdP31_Timer
                End If

                If Not .FindItemByValue("cmdP31_Approving") Is Nothing Then .FindItemByValue("cmdP31_Approving").Text = Resources.Site.Schvalovat_Pripravit_Fakturaci

                If Not .FindItemByValue("cmdAdmin") Is Nothing Then .FindItemByValue("cmdAdmin").Text = Resources.Site.AdministraceSystemu
                If Not .FindItemByValue("cmdReports") Is Nothing Then .FindItemByValue("cmdReports").Text = Resources.Site.cmdReports
                If Not .FindItemByValue("p56") Is Nothing Then .FindItemByValue("p56").Text = Resources.Site.DispecinkUkolu
                If Not .FindItemByValue("o23") Is Nothing Then .FindItemByValue("o23").Text = Resources.Site.Dokumenty
                If Not .FindItemByValue("entity_scheduler") Is Nothing Then .FindItemByValue("entity_scheduler").Text = Resources.Site.Kalendar
                If Not .FindItemByValue("p48") Is Nothing Then .FindItemByValue("p48").Text = Resources.Site.OperativniPlanovani
                If Not .FindItemByValue("p49") Is Nothing Then .FindItemByValue("p49").Text = Resources.Site.Rozpocty
                If Not .FindItemByValue("p90_framework") Is Nothing Then .FindItemByValue("p90_framework").Text = Resources.Site.ZalohoveFaktury
                If Not .FindItemByValue("p51") Is Nothing Then .FindItemByValue("p51").Text = Resources.Site.Ceniky
                If Not .FindItemByValue("x40") Is Nothing Then .FindItemByValue("x40").Text = Resources.Site.OdeslanaPosta

                If Not .FindItemByValue("cmdWorkflow") Is Nothing Then .FindItemByValue("cmdWorkflow").Text = Resources.Site.NavrharWorkflow
                If Not .FindItemByValue("x18") Is Nothing Then .FindItemByValue("x18").Text = Resources.Site.Stitky
                .FindItemByValue("cmdHelp").Text = Resources.Site.Napoveda
                .FindItemByValue("help").ToolTip = Resources.Site.Napoveda
            End With
            

        End If
    End Sub

    Private Sub RenderHomeMenu()
        Dim lisJ62 As IEnumerable(Of BO.j62MenuHome) = _Factory.j62MenuHomeBL.GetList(New BO.myQuery)
        For Each c In lisJ62
            Dim n As New RadMenuItem(c.j62Name)
            n.NavigateUrl = c.j62Url
            If n.NavigateUrl.IndexOf("?") > 0 Then
                n.NavigateUrl += "&j62id=" & c.PID.ToString
            Else
                n.NavigateUrl += "?j62id=" & c.PID.ToString
            End If
            n.ImageUrl = c.j62ImageUrl
            n.Target = c.j62Target
            n.Value = "hm" + c.PID.ToString
            Dim nParent As RadMenuItem = Nothing
            If c.j62ParentID > 0 Then
                nParent = menu1.FindItemByValue("dashboard").Items.FindItemByValue("hm" + c.j62ParentID.ToString)
            End If
            If nParent Is Nothing Then nParent = menu1.FindItemByValue("dashboard")
            nParent.Items.Add(n)
            ''If nParent.ImageUrl = "" Then nParent.ImageUrl = "~/Images/menuarrow.png"
        Next
        If lisJ62.Count > 0 Then
            With menu1.FindItemByValue("dashboard")
                .ImageUrl = "~/Images/menuarrow.png"
                If menu1.ClickToOpen Then .NavigateUrl = ""
            End With


        End If
    End Sub

    Private Sub ShowHideMI(bolVisible As Boolean, strGroupValue As String, strMenuValue As String)
        Dim mg As RadMenuItem = menu1.FindItemByValue(strGroupValue)
        If mg Is Nothing Then Return
        With mg
            Dim mi As RadMenuItem = .Items.FindItemByValue(strMenuValue)
            If Not mi Is Nothing Then
                If Not bolVisible Then
                    .Items.Remove(mi)
                Else
                    mi.Visible = True
                End If
            End If


        End With

        'menu1.FindItemByValue(strGroupValue).Items.FindItemByValue(strMenuValue).Visible = bolVisible


    End Sub


    Private Function AM(strText As String, strURL As String, strValue As String) As RadMenuItem
        Dim item As New RadMenuItem(strText, strURL)
        item.Value = strValue
        Return item
    End Function

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        title1.Text = hidPageTitle.Value
        If _Factory.SysUser.j03IsLiveChatSupport Then
            Dim s As New StringBuilder
            s.AppendLine("<!-- Start of SmartSupp Live Chat script -->")
            s.AppendLine("<script type='text/javascript'>")
            s.AppendLine("var _smartsupp = _smartsupp || {};")
            s.AppendLine("_smartsupp.key = 'f47180c069be93b0e812459452fea73be7887dde';")
            s.AppendLine("window.smartsupp||(function(d) {")
            s.AppendLine("var s,c,o=smartsupp=function(){ o._.push(arguments)};o._=[];")
            s.AppendLine("s=d.getElementsByTagName('script')[0];c=d.createElement('script');")
            s.AppendLine("c.type='text/javascript';c.charset='utf-8';c.async=true;")
            s.AppendLine("c.src='//www.smartsuppchat.com/loader.js';s.parentNode.insertBefore(c,s);")
            s.AppendLine("})(document);")
            s.AppendLine("</script>")
            s.AppendLine("<!-- End of SmartSupp Live Chat script -->")

            ''ScriptManager.RegisterStartupScript(Me.myHead, Me.GetType(), "LiveChat", s.ToString, False)
            myHead.Controls.Add(New LiteralControl(s.ToString))
        End If

        'menu1.FindItemByValue("more").Items.FindItemByValue("cmdHelp").NavigateUrl = "javascript:help('" & Request.FilePath & "')"
        If Me.HelpTopicID = "" Then
            menu1.FindItemByValue("help").NavigateUrl = "http://www.marktime.net/doc/html"
        Else
            menu1.FindItemByValue("help").NavigateUrl = "http://www.marktime.net/doc/html/index.html?" & Me.HelpTopicID & ".htm"
        End If
        If Not menu1.FindItemByValue("more") Is Nothing Then
            With menu1.FindItemByValue("more").Items.FindItemByValue("cmdHelp")
                .NavigateUrl = menu1.FindItemByValue("help").NavigateUrl
            End With
        End If

        SetupSearchbox()
        
    End Sub

    Private Sub SetupSearchbox()
        Dim template As New TextBoxTemplate()
        template.InstantiateIn(menu1.FindItemByValue("searchbox"))
        menu1.DataBind()

        Dim mi As RadMenuItem = menu1.FindItemByValue("searchbox")
        hidSearchBox1.Value = DirectCast(mi.FindControl("searchbox1"), TextBox).ClientID
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


    Class TextBoxTemplate
        Public Sub InstantiateIn(ByVal container As Control)
            Dim txt1 As New TextBox()
            txt1.ID = "searchbox1"
            txt1.Text = Resources.Site.NajitProjekt
            txt1.Style.Item("width") = "110px"
            txt1.Attributes.Item("onfocus") = "search1Focus()"
            txt1.Attributes.Item("onblur") = "search1Blur()"

            AddHandler txt1.DataBinding, AddressOf txt1_DataBinding
            container.Controls.Add(txt1)

        End Sub

        Private Sub txt1_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            ''Dim target As TextBox = DirectCast(sender, TextBox)
            ''Dim item As RadMenuItem = DirectCast(target.BindingContainer, RadMenuItem)
            ''Dim itemText As String = DirectCast(DataBinder.Eval(item, "Text"), String)
            ''target.Text = itemText
        End Sub
    End Class

    Private Sub menu1_ItemCreated(sender As Object, e As RadMenuEventArgs) Handles menu1.ItemCreated
        If Not TypeOf (e.Item) Is RadMenuItem Then Return
        If e.Item.Value = "begin" Then
            e.Item.Controls.Add(New LiteralControl("<a href='default.aspx' title='ÚVOD'><img src='Images/logo_transparent.png' style='border:0px;' /></a>"))
        End If
    End Sub
End Class

