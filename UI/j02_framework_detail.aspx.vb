Imports Telerik.Web.UI
''Imports System.Web.Script.Serialization

Public Class j02_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Property CurrentTab As String
        Get
            If Not tabs1.Visible Then Return ""
            If tabs1.SelectedTab Is Nothing Then
                tabs1.SelectedIndex = 0
            End If
            Return tabs1.SelectedTab.Value
        End Get
        Set(value As String)
            If value = "" Then Return
            If tabs1.FindTabByValue(value) Is Nothing Then Return
            tabs1.FindTabByValue(value).Selected = True
        End Set
    End Property
    Private Sub j02_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Public ReadOnly Property CurrentJ03ID As Integer
        Get
            Return ViewState("j03id")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            ff1.Factory = .Factory
        End With

        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            ViewState("j03id") = 0
            With Master
                .SiteMenuValue = "j02"
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("j02_framework_detail-tab", Request.Item("tab"))
                End If
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OnePersonPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OnePersonPage, "pid=" & .DataPID.ToString))
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("j02_framework_detail-pid")
                    .Add("j02_framework_detail-tab")
                    .Add("j02_framework_detail-tabskin")
                    .Add("j02_framework_detail-chkFFShowFilledOnly")
                    .Add("j02_framework_detail-switch")
                    .Add("j02_framework_detail-switchHeight")
                    .Add("j02_framework_detail-searchbox")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                End With
                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("j02_framework_detail-pid", "O"))
                    If .DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=j02")
                Else
                    If .DataPID <> BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("j02_framework_detail-pid", "O")) Then
                        .Factory.j03UserBL.SetUserParam("j02_framework_detail-pid", .DataPID.ToString)
                    End If
                End If

                With .Factory.j03UserBL
                    panSwitch.Style.Item("display") = .GetUserParam("j02_framework_detail-switch", "block")
                    Dim strHeight As String = .GetUserParam("j02_framework_detail-switchHeight", "auto")
                    If strHeight = "auto" Then
                        panSwitch.Style.Item("height") = "" : panSwitch.Style.Item("overflow") = ""
                    Else
                        panSwitch.Style.Item("height") = strHeight & "px"
                    End If
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("j02_framework_detail-chkFFShowFilledOnly", "0"))
                    menu1.FindItemByValue("searchbox").Visible = BO.BAS.BG(.GetUserParam("j02_framework_detail-searchbox", "0"))
                End With

            End With


            RefreshRecord()

            If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
                basUIMT.RenderSawMenuItemAsGrid(menu1.FindItemByValue("saw"), "j02")
            End If
            tabs1.Skin = Master.Factory.j03UserBL.GetUserParam("j02_framework_detail-tabskin", "Default")   'až zde jsou vygenerované tab záložky
            Me.CurrentTab = Master.Factory.j03UserBL.GetUserParam("j02_framework_detail-tab", "summary")

        End If

        If Me.CurrentTab <> "" Then
            fraSubform.Visible = True
            fraSubform.Attributes.Item("src") = Me.tabs1.SelectedTab.NavigateUrl.Replace("lasttabkey", "nic")
            Select Case Me.CurrentTab
                Case "p31", "time", "expense", "fee", "kusovnik"
                    If Me.hidHardRefreshFlag.Value = "p31-save" Then
                        fraSubform.Attributes.Item("src") += "&pid=" & Me.hidHardRefreshPID.Value
                    End If
            End Select
        Else
            fraSubform.Visible = False : imgLoading.Visible = False
            panSwitch.Style.Item("height") = ""
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=j02")
        With cRec
            Me.panIntraPerson.Visible = .j02IsIntraPerson
            Me.boxJ05.Visible = .j02IsIntraPerson
            menu1.FindItemByValue("cmdApprove").Visible = .j02IsIntraPerson

            menu1.FindItemByValue("cmdCalendar").Visible = .j02IsIntraPerson
            menu1.FindItemByValue("cmdP48").Visible = .j02IsIntraPerson
            menu1.FindItemByValue("cmdPivot").Visible = .j02IsIntraPerson
        End With
        

        With Me.panIntraPerson
            Me.c21Name.Visible = .Visible
            Me.lblFond.Visible = .Visible
        End With
        Dim cRecSum As BO.j02PersonSum = Master.Factory.j02PersonBL.LoadSumRow(cRec.PID)

        If cRec.j02IsIntraPerson Then
            SetupTabs(cRecSum)

            Dim cUser As BO.j03User = Nothing
            Dim mq As New BO.myQueryJ03
            mq.j02ID = cRec.PID
            Dim lisJ03 As IEnumerable(Of BO.j03User) = Master.Factory.j03UserBL.GetList(mq)
            ViewState("j03id") = 0
            If lisJ03.Count > 0 Then
                panAccount.Visible = True
                cUser = lisJ03(0)
                With cUser
                    Me.j03Login.Text = .j03Login
                    Me.j04Name.Text = .j04Name
                End With
                AccountMessage.Text = ""
                ViewState("j03id") = cUser.PID
                cmdLog.Visible = True
            Else
                Me.panAccount.Visible = False
                AccountMessage.Text = "Tento osobní profil není svázán s uživatelským účtem."
                cmdLog.Visible = False
            End If
        Else
            tabs1.Visible = False
        End If
        

        
        With cRec
            basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), .FullNameAsc, "j02_framework_detail.aspx?pid=" & Master.DataPID.ToString, .IsClosed)
            basUIMT.RenderHeaderMenu(.IsClosed, Me.panMenuContainer, menu1)
            Me.FullNameAsc.Text = .FullNameAsc
            Me.j02Email.Text = .j02Email
            Me.j02Code.Text = .j02Code
            Me.j02Email.NavigateUrl = "mailto:" & .j02Email
            If .j02Phone <> "" Then
                Me.Mediums.Text += " | " & .j02Phone
            End If
            If .j02Mobile <> "" Then
                Me.Mediums.Text += " | " & .j02Mobile
            End If
            If .j02Office <> "" Then
                Me.Mediums.Text += " | " & .j02Office
            End If
            If Me.Mediums.Text <> "" Then
                Me.Mediums.Text = BO.BAS.OM1(Trim(Me.Mediums.Text))
            End If
            If .j02Salutation <> "" Then
                Me.Correspondence.Text = String.Format("Oslovení pro korespondenci: {0}", "<b>" & .j02Salutation & "</b>")
            End If


            Me.j07Name.Text = .j07Name
            If Not cRec.j02IsIntraPerson Then
                Me.j07Name.Text = .j02JobTitle
            End If
            Me.c21Name.Text = .c21Name
            If .j17ID > 0 Then
                Me.j17Name.Text = Master.Factory.j17CountryBL.Load(.j17ID).j17Name : lblJ17Name.Visible = True
            Else
                Me.lblJ17Name.Visible = False
            End If
            If .j18ID = 0 Then
                lblJ18Name.Visible = False
            Else
                lblJ18Name.Visible = True : Me.j18Name.Text = .j18Name
            End If
            Me.TeamsInLine.Text = Master.Factory.j02PersonBL.GetTeamsInLine(.PID)
            If Me.TeamsInLine.Text = "" Then lblTeams.Visible = False
        End With

        If cRecSum.o23_Exist Then
            Dim mqO23 As New BO.myQueryO23
            mqO23.j02ID = Master.DataPID
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)

            If lisO23.Count > 0 Then
                Me.boxO23.Visible = True
                notepad1.RefreshData(lisO23, Master.DataPID)
                boxO23Title.Text = BO.BAS.OM2(boxO23Title.Text, lisO23.Count.ToString)

            Else
                cRecSum.o23_Exist = False
            End If
        End If
        Me.boxO23.Visible = cRecSum.o23_Exist

        panMasters.Visible = False : panSlaves.Visible = False
        cmdAddJ05.Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin)
        Dim lisJ05 As IEnumerable(Of BO.j05MasterSlave) = Master.Factory.j05MasterSlaveBL.GetList(cRec.PID, 0, 0)

        If lisJ05.Count > 0 Then
            panSlaves.Visible = True
            rpSlaves.DataSource = lisJ05
            rpSlaves.DataBind()
        End If
        lisJ05 = Master.Factory.j05MasterSlaveBL.GetList(0, cRec.PID, 0)
        If lisJ05.Count > 0 Then
            panMasters.Visible = True
            rpMasters.DataSource = lisJ05
            rpMasters.DataBind()
        End If


        Handle_Permissions(cRec)

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.j02Person, Master.DataPID, cRec.j07ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.j02Person).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_decide('x18_binding.aspx?prefix=j02&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.j02Person, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.j02Person, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        Me.rpP30.DataSource = Master.Factory.p30Contact_PersonBL.GetList(0, 0, Master.DataPID)
        Me.rpP30.DataBind()
        If rpP30.Items.Count > 0 Then
            panP30.Visible = True
        Else
            panP30.Visible = False
        End If

    End Sub



    Private Sub Handle_Permissions(cRec As BO.j02Person)
        With Master.Factory
            menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator)
            menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)
            If Not .SysUser.j04IsMenu_Invoice Then
                RemoveTab("p91")

            End If
        End With
        

        Dim b As Boolean = Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin)
        x18_binding.Visible = b
        menu1.FindItemByValue("cmdNew").Visible = b
        menu1.FindItemByValue("cmdEdit").Visible = b
        menu1.FindItemByValue("cmdCopy").Visible = b
        With cmdAccount
            .Visible = b
            If Me.CurrentJ03ID = 0 And b Then
                .NavigateUrl = "javascript:j03_create()"
                .Text = "Založit uživatelský účet"
            Else
                .NavigateUrl = "javascript:j03_edit()"
                .Text = "Nastavení uživatelského účtu"
            End If
        End With
        
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString
        If cRec.j02IsIntraPerson Then
            With menu1.FindItemByValue("cmdPivot")
                .Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
                If .Visible Then .NavigateUrl = "p31_pivot.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString
            End With
        End If
        
        If cRec.IsClosed Then
            menu1.FindItemByValue("cmdO22").Visible = False
            menu1.Skin = "Black"
        End If
    End Sub

    

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "p31-save", "p31-delete"
               
            Case Else
                ReloadPage(Master.DataPID.ToString)
        End Select
        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub

   

    Private Sub ReloadPage(strPID As String)
        Response.Redirect("j02_framework_detail.aspx?pid=" & strPID)
    End Sub


    
    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j02_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub rpP30_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP30.ItemDataBound
        Dim cRec As BO.p30Contact_Person = CType(e.Item.DataItem, BO.p30Contact_Person)
        With CType(e.Item.FindControl("Company"), HyperLink)
            If cRec.p28CompanyName <> "" Then
                .Text = cRec.p28CompanyName
            Else
                .Text = cRec.p28Name
            End If
            .NavigateUrl = "p28_framework.aspx?pid=" & cRec.p28ID.ToString
        End With
    End Sub


    Private Sub SetupTabs(crs As BO.j02PersonSum)
        tabs1.Tabs.Clear()

        Dim lisX61 As IEnumerable(Of BO.x61PageTab) = Master.Factory.j03UserBL.GetList_PageTabs(Master.Factory.SysUser.PID, BO.x29IdEnum.j02Person)
        For Each c In lisX61
            Dim tab As New RadTab(c.x61Name, c.x61Code)
            tabs1.Tabs.Add(tab)
            tab.NavigateUrl = c.GetPageUrl("j02", Master.DataPID, BO.BAS.GB(Master.Factory.SysUser.IsApprovingPerson))
            tab.NavigateUrl += "&lasttabkey=j02_framework_detail-tab&lasttabval=" & c.x61Code
            tab.Target = "fraSubform"
            If tabs1.Tabs.Count = 0 Then tab.Selected = True
            Select Case c.x61Code
                Case "time"
                    If crs.p31_Wip_Time_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
                    If crs.p31_Approved_Time_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
                Case "expense"
                    If crs.p31_Wip_Expense_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
                    If crs.p31_Approved_Expense_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
                Case "fee"
                    If crs.p31_Wip_Fee_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
                    If crs.p31_Approved_Fee_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
                Case "kusovnik"
                    If crs.p31_Wip_Kusovnik_Count > 0 Then tab.Text += "<span class='badge1wip'>" & crs.p31_Wip_Kusovnik_Count.ToString & "</span>"
                    If crs.p31_Approved_Kusovnik_Count > 0 Then tab.Text += "<span class='badge1approved'>" & crs.p31_Approved_Kusovnik_Count.ToString & "</span>"
                Case "p91"
                    If crs.p91_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.p91_Count.ToString & "</span>"
                Case "p56"
                    If crs.p56_Actual_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.p56_Actual_Count.ToString & "</span>"
                Case "workflow"
                    If crs.b07_Count > 0 Then tab.Text += "<span class='badge1'>" & crs.b07_Count.ToString & "</span>"
            End Select
        Next
    End Sub



    Private Sub RemoveTab(strTabValue As String)
        With tabs1.Tabs
            If Not .FindTabByValue(strTabValue) Is Nothing Then
                Master.Notify(String.Format("Pro záložku [{0}] nemáte oprávnění.", .FindTabByValue(strTabValue).Text), NotifyLevel.InfoMessage)
                .Remove(.FindTabByValue(strTabValue))
                If .Count > 0 Then tabs1.SelectedIndex = 0

            End If
        End With
    End Sub
End Class