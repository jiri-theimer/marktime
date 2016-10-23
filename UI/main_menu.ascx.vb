﻿Imports Telerik.Web.UI

Public Class main_menu
    Inherits System.Web.UI.UserControl
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
    Public Property SelectedValue As String
        Get
            Return menu1.SelectedValue
        End Get
        Set(value As String)
            If Not menu1.FindItemByValue(value) Is Nothing Then
                menu1.FindItemByValue(value).HighlightPath()
            End If
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub ClearAll()
        panContainer.Controls.Remove(menu1)
        panContainer.Visible = False
    End Sub
    Public Sub RefreshData(factory As BL.Factory, strCurrentHelpID As String)
        Me.panContainer.Visible = True
        Dim bolAdmin As Boolean = factory.TestPermission(BO.x53PermValEnum.GR_Admin), n As RadMenuItem
        With factory.SysUser
            menu1.ClickToOpen = .j03IsSiteMenuOnClick
            If .j03SiteMenuSkin > "" Then menu1.Skin = .j03SiteMenuSkin

            ai("", "begin", "", "")
            If .j04IsMenu_Project Then
                ai("", "searchbox", "", "")
            End If

            If .HomeMenu = "" Then
                ai(Resources.Site.Menu_UVOD, "dashboard", "default.aspx", "")
            Else
                n = ai(.HomeMenu, "dashboard", "default.aspx", "")
                RenderCustomHomeMenu(factory, n)

            End If

            If .j04IsMenu_Worksheet Then
                n = ai("WORKSHEET", "p31", "", "~/Images/menuarrow.png")
                ai(Resources.Site.Zapisovat, "p31_framework", "p31_framework.aspx", "Images/worksheet.png", n)
                ai("Zapisovat přes KALENDÁŘ", "p31_scheduler", "p31_scheduler.aspx", "Images/worksheet.png", n)
                ai("Zapisovat přes DAYLINE", "p31_timeline", "p31_timeline.aspx", "Images/worksheet.png", n)
                ai(Resources.Site.cmdP31_Timer, "cmdP31_Timer", "p31_timer.aspx", "Images/worksheet.png", n)
                ai(Resources.Site.Grid, "cmdP31_Grid", "p31_grid.aspx", "Images/grid.png", n)
                If factory.SysUser.IsApprovingPerson Then
                    ai(Resources.Site.Schvalovat_Pripravit_Fakturaci, "cmdP31_Approving", "approving_framework.aspx", "Images/approve.png", n)
                End If


                If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then ai("PIVOT", "cmdP31_Pivot", "p31_pivot.aspx", "Images/pivot.png", n)
            End If

            If .j04IsMenu_Project Then ai(Resources.Site.Menu_PROJEKTY, "p41", "p41_framework.aspx", "")
            If .j04IsMenu_Contact Then ai(Resources.Site.Menu_KLIENTI, "p28", "p28_framework.aspx", "")
            If .j04IsMenu_People Then ai(Resources.Site.Menu_LIDE, "j02", "j02_framework.aspx", "")
            If .j04IsMenu_Invoice Then ai(Resources.Site.Menu_FAKTURY, "p91", "p91_framework.aspx", "")

            n = ai(.Person, "me", "", "~/Images/menuarrow.png")
            If .Person = "" Then n.Text = .j03Login
            If .j04IsMenu_MyProfile Then ai(Resources.Site.MujProfil, "cmdMyProfile", "j03_myprofile.aspx", "Images/user.png", n)
            ai(Resources.Site.ZmenitHeslo, "cmdChangePassword", "changepassword.aspx", "Images/password.png", n)
            ai(Resources.Site.OdhlasitSe, "cmdLogout", "Account/Login.aspx?logout=1", "Images/logout.png", n)

            If .j04IsMenu_More Then
                n = ai(Resources.Site.Menu_DALSI, "more", "", "~/Images/menuarrow.png")
                n.GroupSettings.RepeatColumns = 2
                n.GroupSettings.RepeatDirection = MenuRepeatDirection.Vertical
                ''menu1.Items.Add(n)
                If bolAdmin Then ai(Resources.Site.AdministraceSystemu, "cmdAdmin", "admin_framework.aspx", "Images/setting.png", n)
                If .j04IsMenu_Report Then ai(Resources.Site.cmdReports, "cmdReports", "report_framework.aspx", "Images/reporting.png", n)
                ai(Resources.Site.DispecinkUkolu, "p56", "p56_framework.aspx", "Images/task.png", n)
                ai("Nástěnka", "o10", "o10_framework.aspx", "Images/article.png", n)
                If factory.TestPermission(BO.x53PermValEnum.GR_O23_Creator) Or factory.TestPermission(BO.x53PermValEnum.GR_O23_Draft_Creator) Then
                    ai(Resources.Site.Dokumenty, "o23", "o23_framework.aspx", "Images/notepad.png", n)
                End If

                ai(Resources.Site.Kalendar, "entity_scheduler", "entity_scheduler.aspx", "Images/calendar.png", n)
                If factory.TestPermission(BO.x53PermValEnum.GR_P48_Creator) Then
                    ai(Resources.Site.OperativniPlanovani, "p48", "p48_framework.aspx", "Images/oplan.png", n)
                End If

                ''ai("Rozpočty výdajů a fixních odměn", "p49", "p49_framework.aspx", "Images/finplan.png", n)
                If factory.TestPermission(BO.x53PermValEnum.GR_P90_Reader) Then
                    ai(Resources.Site.ZalohoveFaktury, "p90_framework", "p90_framework.aspx", "Images/proforma.png", n)
                End If

                ai(Resources.Site.Ceniky, "p51", "p51_framework.aspx", "Images/billing.png", n)
                ai(Resources.Site.OdeslanaPosta, "x40", "x40_framework.aspx", "Images/email.png", n)
                If bolAdmin Then ai(Resources.Site.NavrharWorkflow, "cmdWorkflow", "admin_workflow.aspx", "Images/workflow.png", n)
                ai(Resources.Site.Stitky, "x18", "x18_framework.aspx", "Images/label.png", n)

                If n.Items.Count <= 8 Then n.GroupSettings.RepeatColumns = 1
            End If

            If .MessagesCount > 0 Then
                n = ai(.MessagesCount.ToString, "messages", "javascript:messages()", "Images/messages.png")
                n.ToolTip = "Zprávy a upozornění ze systému"
            End If
            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                n = ai("<img src='Images/saw_turn_off.png'/>", "saw", "javascript:setsaw('0')", "")
                n.ToolTip = "Zobrazit levý panel"
            Else
                ai("<img src='Images/saw_turn_on.png'/>", "saw", "javascript:setsaw('1')", "")
                n.ToolTip = "Skrýt levý panel"
            End If
            n = ai("", "lang", "", "~/Images/menuarrow.png")
            ai("Česky", "", "javascript:setlang('-')", "Images/Flags/menu_czech.gif", n)
            ai("English", "", "javascript:setlang('en-US')", "Images/Flags/menu_uk.gif", n)

            n = ai("?", "help", "http://www.marktime.net/doc/html", "")
            n.Target = "_blank"
            n.ToolTip = Resources.Site.Napoveda
            If strCurrentHelpID <> "" Then
                n.NavigateUrl = "http://www.marktime.net/doc/html/index.html?" & strCurrentHelpID & ".htm"
            End If
        End With

        SetupSearchbox()
    End Sub

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
        Dim template As New TextBoxTemplate()
        template.InstantiateIn(menu1.FindItemByValue("searchbox"))
        menu1.DataBind()

        Dim mi As RadMenuItem = menu1.FindItemByValue("searchbox")
        hidSearchBox1.Value = DirectCast(mi.FindControl("searchbox1"), TextBox).ClientID
    End Sub


    Private Sub RenderCustomHomeMenu(factory As BL.Factory, nDashBoard As RadMenuItem)
        Dim lisJ62 As IEnumerable(Of BO.j62MenuHome) = factory.j62MenuHomeBL.GetList(New BO.myQuery)
        For Each c In lisJ62
            Dim n As New RadMenuItem(c.j62Name)
            n.NavigateUrl = c.j62Url
            If n.NavigateUrl.IndexOf("javascript") < 0 Then
                If n.NavigateUrl.IndexOf("?") > 0 Then
                    n.NavigateUrl += "&j62id=" & c.PID.ToString
                Else
                    n.NavigateUrl += "?j62id=" & c.PID.ToString
                End If
            End If

            n.ImageUrl = c.j62ImageUrl
            n.Target = c.j62Target
            n.Value = "hm" + c.PID.ToString
            Dim nParent As RadMenuItem = Nothing
            If c.j62ParentID > 0 Then
                nParent = menu1.FindItemByValue("dashboard").Items.FindItemByValue("hm" + c.j62ParentID.ToString)
            End If
            If nParent Is Nothing Then nParent = nDashBoard
            nParent.Items.Add(n)
        Next
        If lisJ62.Count > 0 Then
            With menu1.FindItemByValue("dashboard")
                .ImageUrl = "~/Images/menuarrow.png"
                If menu1.ClickToOpen Then .NavigateUrl = ""
            End With
        End If
    End Sub
End Class