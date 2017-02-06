Imports Telerik.Web.UI

Public Class main_menu
    Inherits System.Web.UI.UserControl

    Class TextBoxTemplate
        Public Sub InstantiateIn(ByVal container As Control)
            Dim txt1 As New TextBox()
            txt1.ID = "search1"
            txt1.Text = Resources.Site.NajitProjekt
            txt1.ToolTip = Resources.Site.NajitProjekt
            txt1.Style.Item("width") = "110px"
            txt1.Attributes.Item("onfocus") = "search1Focus()"
            txt1.Attributes.Item("onblur") = "search1Blur()"

            AddHandler txt1.DataBinding, AddressOf txt1_DataBinding
            container.Controls.Add(txt1)

            Dim link1 As New HyperLink()
            With link1
                .CssClass = "button-reczoom"
                .Attributes.Item("rel") = "clue_search.aspx"
                .Attributes.Item("dialogheight") = "600"
                .Attributes.Item("title") = "Více hledání"
                ''.Style.Item("background-color") = "#25a0da"
                .Style.Item("padding-left") = "2px"
                .Style.Item("padding-bottom") = "0px"
                .Style.Item("padding-top") = "2px"
                .ImageUrl = "Images/menuarrow.png"

                .ToolTip = "Více hledání"

            End With
            
            container.Controls.Add(link1)
        End Sub

        Private Sub txt1_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            ''Dim target As TextBox = DirectCast(sender, TextBox)
            ''Dim item As RadMenuItem = DirectCast(target.BindingContainer, RadMenuItem)
            ''Dim itemText As String = DirectCast(DataBinder.Eval(item, "Text"), String)
            ''target.Text = itemText
        End Sub
    End Class
    Public Property SelectedValue As String
        Get
            Return menu1.SelectedValue
        End Get
        Set(value As String)
            If value = "" Then Return
            If Not menu1.FindItemByValue(value) Is Nothing Then
                menu1.FindItemByValue(value).HighlightPath()
            End If
        End Set
    End Property
    Public Property MasterPageName As String
        Get
            Return Me.hidMasterPageName.Value
        End Get
        Set(value As String)
            hidMasterPageName.Value = value
        End Set
    End Property
    Public ReadOnly Property ItemsCount As Integer
        Get
            If Not panContainer.Visible Then Return 0
            Return menu1.Items.Count
        End Get
    End Property

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
    End Sub

    Public Sub ClearAll()
        panContainer.Controls.Remove(menu1)
        panContainer.Visible = False
    End Sub
    Public Sub RefreshData(factory As BL.Factory, strCurrentHelpID As String, strCurrentSiteMenuValue As String)
        Dim bolCurSAW As Boolean = False, strLang As String = basUI.GetCookieValue(Request, "MT50-CultureInfo")
        If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then bolCurSAW = True

        Me.panContainer.Visible = True
        Dim bolAdmin As Boolean = factory.TestPermission(BO.x53PermValEnum.GR_Admin), n As RadMenuItem
        With factory.SysUser
            menu1.ClickToOpen = .j03IsSiteMenuOnClick
            If .j03SiteMenuSkin > "" Then menu1.Skin = .j03SiteMenuSkin
            If menu1.Items.Count > 0 Then menu1.Items.Clear()

            ai("", "begin", "", "")
            If .j04IsMenu_Project Then
                Me.hidAllowSearch1.Value = "1"
                ai("", "searchbox1", "", "")
            End If

            n = ai("", "new", "", "Images/new_menu.png", )
            n.SelectedCssClass = ""
            n.ToolTip = Resources.common.Novy
            Dim b As Boolean = False
            If .j04IsMenu_Contact Then
                If factory.TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator) Then ai(Resources.common.Klient, "", "javascript:p28_create()", "", n) : b = True
            End If
            If .j04IsMenu_Project Then
                If factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator) Then ai(Resources.common.Projekt, "", "javascript:p41_create()", "", n) : b = True
            End If
            If .j04IsMenu_Worksheet Then
                ai("Worksheet úkon", "", "javascript:p31_create()", "", n) : b = True
            End If
            If factory.TestPermission(BO.x53PermValEnum.GR_P56_Creator) Then
                ai(Resources.common.Ukol, "", "javascript:p56_create()", "", n)
            End If
            If .j04IsMenu_Invoice Then
                If factory.TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator) Then ai(Resources.common.Faktura, "", "javascript:p91_create()", "", n) : b = True
            End If
            If factory.TestPermission(BO.x53PermValEnum.GR_P90_Create) Then ai(Resources.common.ZalohovaFaktura, "", "javascript:p90_create()", "", n)
            If factory.TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator) Then
                ai(Resources.common.Dokument, "", "javascript:o23_create()", "", n) : b = True
            End If
            If Not b Then menu1.Items.Remove(n)
        End With

        RenderDbMenu(factory, strLang)

        With factory.SysUser
            n = ai(.Person, "me", "", "~/Images/menuarrow.png")
            If .Person = "" Then n.Text = .j03Login
            If .j04IsMenu_MyProfile Then ai(Resources.Site.MujProfil, "cmdMyProfile", "j03_myprofile.aspx", "", n)
            ai(Resources.Site.ZmenitHeslo, "cmdChangePassword", "changepassword.aspx", "", n)
            ai(Resources.Site.OdhlasitSe, "cmdLogout", "Account/Login.aspx?logout=1", "", n)

            If .MessagesCount > 0 Then
                n = ai("<img src='Images/globe.png'/>" + .MessagesCount.ToString, "messages", "javascript:messages()", "")
                n.ToolTip = "Zprávy a upozornění ze systému"
            End If
            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                n = ai("<img src='Images/saw_turn_off.png'/>", "saw", "javascript:setsaw('0')", "")
                n.ToolTip = "Přepnout do módu 1: zobrazovat levý navigační panel"
            Else
                n = ai("<img src='Images/saw_turn_on.png'/>", "saw", "javascript:setsaw('1')", "")
                n.ToolTip = "Přepnout do módu 2: skrývat levý navigační panel"
            End If
            ''End If

            Select Case strLang
                Case "en-US"
                    n = ai("<img src='Images/Flags/menu_uk.gif'/>", "lang", "", "")
                Case "-"
                    n = ai("<img src='Images/Flags/menu_czech.gif'/>", "lang", "", "")
                Case Else
                    n = ai("", "lang", "", "~/Images/menuarrow.png")
            End Select
            ai("Česky", "", "javascript:setlang('-')", "Images/Flags/menu_czech.gif", n)
            ai("English", "", "javascript:setlang('en-US')", "Images/Flags/menu_uk.gif", n)

            n = ai("?", "help", "http://www.marktime.net/doc/html", "")
            n.Target = "_blank"
            n.ToolTip = Resources.Site.Napoveda
            If strCurrentHelpID <> "" Then
                n.NavigateUrl = "http://www.marktime.net/doc/html/index.html?" & strCurrentHelpID & ".htm"
            End If
        End With
        Me.SelectedValue = strCurrentSiteMenuValue

        
    End Sub
    Private Function Is_SAW_Switcher() As Boolean
        If Request.Url.ToString.IndexOf("entity_framework") > 0 Or Request.Url.ToString.IndexOf("p31_framework") > 0 Then Return True
        If Request.Url.ToString.IndexOf("p28_framework") > 0 Or Request.Url.ToString.IndexOf("p41_framework") > 0 Then Return True
        If Request.Url.ToString.IndexOf("p91_framework") > 0 Or Request.Url.ToString.IndexOf("j02_framework") > 0 Then Return True
        If Request.Url.ToString.IndexOf("o23_framework") > 0 Or Request.Url.ToString.IndexOf("p56_framework") > 0 Then Return True

        Return False

    End Function

    Private Function ai(strText As String, strValue As String, strURL As String, strImg As String, Optional nParent As RadMenuItem = Nothing) As RadMenuItem

        Dim n As New RadMenuItem(strText, strURL)
        n.ImageUrl = strImg
        n.Value = strValue
        If Not nParent Is Nothing Then
            nParent.Items.Add(n)
        Else
            menu1.Items.Add(n)
        End If
        Return n

    End Function

    Private Sub menu1_ItemCreated(sender As Object, e As RadMenuEventArgs) Handles menu1.ItemCreated
        If Not TypeOf (e.Item) Is RadMenuItem Then Return
        If e.Item.Value = "begin" Then
            e.Item.Controls.Add(New LiteralControl("<a href='default.aspx' title='ÚVOD'><img src='Images/logo_transparent.png' style='border:0px;' /></a>"))
        End If
    End Sub

    Private Sub SetupSearchbox()
        Dim c As Control = menu1.FindItemByValue("searchbox1")
        If c Is Nothing Then Return
        Dim template As New TextBoxTemplate()
        template.InstantiateIn(c)
        menu1.DataBind()

        Dim mi As RadMenuItem = CType(c, RadMenuItem)
        hidSearch1.Value = DirectCast(mi.FindControl("search1"), TextBox).ClientID
    End Sub


    Private Sub RenderDbMenu(factory As BL.Factory, strLang As String)
        Dim mq As New BO.myQuery
        mq.MG_GridSqlColumns = "a.x29ID,j62Name,a.j62Name_ENG,a.j62ParentID,a.j74ID,a.j70ID,a.j62Url,a.j62Target,a.j62ImageUrl,a.j62Tag,a.j62TreeLevel as _j62TreeLevel"    'kvůli co nejvyšší rychlosti
        Dim lisJ62 As IEnumerable(Of BO.j62MenuHome) = factory.j62MenuHomeBL.GetList(factory.SysUser.j60ID, mq)
        Dim ns As New Dictionary(Of Integer, RadMenuItem), bolGO As Boolean = False

        For Each c In lisJ62
            bolGO = True
            With factory.SysUser
                Select Case c.x29ID
                    Case BO.x29IdEnum.p31Worksheet
                        bolGO = .j04IsMenu_Worksheet
                        If c.j62Tag = "p31_pivot" Then bolGO = factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
                        If c.j62Tag = "p31_approving" Then bolGO = .IsApprovingPerson
                    Case BO.x29IdEnum.p41Project
                        bolGO = .j04IsMenu_Project
                    Case BO.x29IdEnum.p28Contact
                        bolGO = factory.SysUser.j04IsMenu_Contact
                    Case BO.x29IdEnum.p91Invoice : bolGO = .j04IsMenu_Invoice
                    Case BO.x29IdEnum.p90Proforma : bolGO = .j04IsMenu_Proforma
                    Case BO.x29IdEnum.j02Person : bolGO = .j04IsMenu_People
                    Case BO.x29IdEnum.o23Notepad
                        bolGO = .j04IsMenu_Notepad
                    Case BO.x29IdEnum.System
                        bolGO = .IsAdmin
                        If c.j62Tag = "navigator" Then bolGO = factory.TestPermission(BO.x53PermValEnum.GR_Navigator)
                    Case BO.x29IdEnum.p51PriceList : bolGO = factory.TestPermission(BO.x53PermValEnum.GR_P51_Admin)
                    Case BO.x29IdEnum.x31Report : bolGO = .j04IsMenu_Report

                End Select
            End With

            If Not bolGO Then Continue For 'skočit na další c v cyklu

            Dim n As New RadMenuItem(c.j62Name)
            If strLang = "en-US" And c.j62Name_ENG <> "" Then n.Text = c.j62Name_ENG 'menu v angličtině
            n.ImageUrl = c.j62ImageUrl
            n.Value = c.j62Tag

            If c.j62IsSeparator Then
                If c.j62ParentID = 0 Then
                    n.NavigateUrl = ""
                Else
                    n.IsSeparator = True

                End If
            Else
                n.NavigateUrl = c.j62Url
                If c.j70ID > 0 Or c.j74ID > 0 Then
                    If n.NavigateUrl.IndexOf("?") > 0 Then
                        n.NavigateUrl += "&j62id=" & c.PID.ToString
                    Else
                        n.NavigateUrl += "?j62id=" & c.PID.ToString
                    End If
                    n.Value = "hm" & c.PID.ToString
                End If
                n.Target = c.j62Target
            End If



            Dim nParent As RadMenuItem = Nothing
            If c.j62ParentID > 0 Then
                Try
                    nParent = ns.First(Function(p) p.Key = c.j62ParentID).Value()
                Catch ex As Exception
                End Try
            End If
            If nParent Is Nothing Then
                menu1.Items.Add(n)
            Else
                With nParent
                    .Items.Add(n)
                    If .Level = 0 Then
                        .ImageUrl = "~/Images/menuarrow.png"
                        If menu1.ClickToOpen Then .NavigateUrl = ""
                        If .Items.Count > 7 Then
                            .GroupSettings.RepeatColumns = 2
                            .GroupSettings.RepeatDirection = MenuRepeatDirection.Vertical
                        End If
                    End If
                End With

            End If
            ns.Add(c.PID, n)
        Next

    End Sub

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If panContainer.Visible Then SetupSearchbox()
    End Sub
End Class