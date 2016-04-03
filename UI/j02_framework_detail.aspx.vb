Imports Telerik.Web.UI
''Imports System.Web.Script.Serialization

Public Class j02_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Enum SubgridType
        summary = -1
        p31 = 1
        p91 = 2
        p56 = 4
        _NotSpecified = 0
    End Enum
    Public Property CurrentSubgrid As SubgridType
        Get
            Return DirectCast(CInt(Me.opgSubgrid.SelectedTab.Value), SubgridType)
        End Get
        Set(value As SubgridType)
            Me.opgSubgrid.FindTabByValue(CInt(value).ToString).Selected = True
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
            ViewState("j03id") = 0
            With Master
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("j02_framework_detail-subgrid", Request.Item("tab"))
                End If
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OnePersonPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OnePersonPage, "pid=" & .DataPID.ToString))
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("j02_framework_detail-pid")
                    .Add("j02_framework_detail-subgrid")
                    .Add("j02_framework_detail-chkFFShowFilledOnly")
                    .Add("j02_framework_detail-switch")
                    .Add("j02_framework_detail-switchHeight")
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
                    Me.CurrentSubgrid = DirectCast(CInt(.GetUserParam("j02_framework_detail-subgrid", "1")), SubgridType)
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("j02_framework_detail-chkFFShowFilledOnly", "0"))
                End With
                
            End With


            RefreshRecord()

        End If

        With Me.opgSubgrid.Tabs
            .FindTabByValue("-1").NavigateUrl = "entity_framework_p31summary.aspx?masterprefix=j02&masterpid=" & Master.DataPID.ToString
            .FindTabByValue("1").NavigateUrl = "entity_framework_p31subform.aspx?masterprefix=j02&masterpid=" & Master.DataPID.ToString
            .FindTabByValue("2").NavigateUrl = "entity_framework_p91subform.aspx?masterprefix=j02&masterpid=" & Master.DataPID.ToString
            .FindTabByValue("3").NavigateUrl = "entity_framework_b07subform.aspx?masterprefix=j02&masterpid=" & Master.DataPID.ToString
            .FindTabByValue("4").NavigateUrl = "entity_framework_p56subform.aspx?masterprefix=j02&masterpid=" & Master.DataPID.ToString
        End With
        If Me.CurrentSubgrid = SubgridType._NotSpecified Then
            fraSubform.Visible = False : imgLoading.Visible = False
            panSwitch.Style.Item("height") = ""
            For i As Integer = 0 To Me.opgSubgrid.Tabs.Count - 1
                Me.opgSubgrid.Tabs(i).NavigateUrl = ""
            Next
        Else
            fraSubform.Visible = True
            fraSubform.Attributes.Item("src") = Me.opgSubgrid.SelectedTab.NavigateUrl
            If Me.CurrentSubgrid = SubgridType.p31 And Me.hidHardRefreshFlag.Value = "p31-save" Then
                fraSubform.Attributes.Item("src") += "&pid=" & Me.hidHardRefreshPID.Value
            End If
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=j02")
        With cRec
            Me.panIntraPerson.Visible = .j02IsIntraPerson
            Me.boxJ05.Visible = .j02IsIntraPerson
            Me.topLink1.Visible = .j02IsIntraPerson
            Me.topLink2.Visible = .j02IsIntraPerson
            Me.topLink3.Visible = .j02IsIntraPerson
            Me.topLink0.Visible = .j02IsIntraPerson
            Me.topLink6.Visible = .j02IsIntraPerson
        End With
        

        With Me.panIntraPerson
            Me.c21Name.Visible = .Visible
            Me.lblFond.Visible = .Visible
        End With

        If cRec.j02IsIntraPerson Then
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
            Me.CurrentSubgrid = SubgridType._NotSpecified
            Me.opgSubgrid.Visible = False
        End If
        

        
        With cRec
            cmdNewWindow.NavigateUrl = "j02_framework.aspx?blankwindow=1&title=" & .FullNameDesc & "&pid=" & .PID.ToString
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
        End With

        Dim mqO23 As New BO.myQueryO23
        mqO23.j02ID = Master.DataPID
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)

        If lisO23.Count > 0 Then
            Me.boxO23.Visible = True
            notepad1.RefreshData(lisO23, Master.DataPID)
            boxO23Title.Text = BO.BAS.OM2(boxO23Title.Text, lisO23.Count.ToString)

        Else
            Me.boxO23.Visible = False
        End If

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
                topLink6.Visible = False
                If Not Me.opgSubgrid.Tabs.FindTabByValue("2") Is Nothing Then
                    Me.opgSubgrid.Tabs.Remove(Me.opgSubgrid.Tabs.FindTabByValue("2"))
                    If Me.CurrentSubgrid = SubgridType.p91 Then Me.CurrentSubgrid = SubgridType.p31
                End If

            End If
        End With
        

        Dim b As Boolean = Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin)
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
        With menu1.FindItemByValue("cmdPivot")
            .Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            If .Visible Then .NavigateUrl = "p31_pivot.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString
        End With
        If cRec.IsClosed Then
            menu1.FindItemByValue("cmdO22").Visible = False
            Me.hidIsBin.Value = "1"
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

   
End Class