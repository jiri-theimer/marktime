﻿Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

Public Class p28_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Enum SubgridType
        summary = -1
        p31 = 1
        p91 = 2
        p56 = 4
        b07 = 3
        _NotSpecified = 0
    End Enum
    Public Property CurrentSubgrid As SubgridType
        Get
            Return DirectCast(CInt(Me.opgSubgrid.SelectedTab.Value), SubgridType)
        End Get
        Set(value As SubgridType)
            Me.opgSubgrid.Tabs.FindTabByValue(CInt(value).ToString).Selected = True
        End Set
    End Property

    Private Sub p28_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            gridP31.Factory = .Factory
            p31summary1.Factory = .Factory
            gridP56.Factory = .Factory
            gridP91.Factory = .Factory
            ff1.Factory = .Factory
            bigsummary1.Factory = .Factory
        End With
        

        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneContactPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneContactPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars

                    .Add("p28_framework_detail-pid")
                    .Add("p28_framework_detail-subgrid")
                    .Add("p28_framework_detail-chkFFShowFilledOnly")
                    .Add("p28_framework_detail-switch")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                End With
                With .Factory.j03UserBL
                    panSwitch.Style.Item("display") = .GetUserParam("p28_framework_detail-switch", "block")

                    Me.CurrentSubgrid = DirectCast(CInt(.GetUserParam("p28_framework_detail-subgrid", "1")), SubgridType)
                    If Master.DataPID = 0 Then
                        Master.DataPID = BO.BAS.IsNullInt(.GetUserParam("p28_framework_detail-pid", "O"))
                        If Master.DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p28")
                    Else
                        If Master.DataPID <> BO.BAS.IsNullInt(.GetUserParam("p28_framework_detail-pid", "O")) Then
                            .SetUserParam("p28_framework_detail-pid", Master.DataPID.ToString)
                        End If
                    End If
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p28_framework_detail-chkFFShowFilledOnly", "0"))
                End With

            End With

            gridP91.Visible = False : gridP56.Visible = False
            Select Case Me.CurrentSubgrid
                Case SubgridType.p56
                    gridP56.Visible = True
                Case SubgridType.p91
                    gridP91.Visible = True
            End Select
            RefreshRecord()

        End If

        gridP31.MasterDataPID = 0     'uvnitř prvku nebude docházet k plnění gridu
        Select Case Me.CurrentSubgrid
            Case SubgridType.p31
                gridP31.MasterDataPID = Master.DataPID
            Case SubgridType.p91
                gridP91.MasterDataPID = Master.DataPID
            Case SubgridType.p56
                gridP56.MasterDataPID = Master.DataPID
        End Select
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p28")
        Handle_Permissions(cRec)

        Dim cClient As BO.p28Contact = Nothing

        
        With cRec
            cmdNewWindow.NavigateUrl = "p28_framework.aspx?blankwindow=1&pid=" & .PID.ToString & "&title=" & .p28Name
            basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), .p28Name, "p28_framework_detail.aspx?pid=" & Master.DataPID.ToString, .IsClosed)
            basUIMT.RenderHeaderMenu(.IsClosed, Me.panMenuContainer, menu1)
            Me.Contact.Text = .p28Name
            If .p28Code <> "" Then
                Me.Contact.Text += " <span style='color:gray;padding-left:10px;'>" & .p28Code & "</span>"
            End If

            If .p28CompanyShortName > "" Then
                Me.Contact.Text += "<div>" & .p28CompanyName & "</div>"
            End If
            Me.Owner.Text = .Owner
            Me.p28RegID.Text = .p28RegID
            Me.p28VatID.Text = .p28VatID
            If .p29ID > 0 Then
                Me.p29Name.Text = "[" & .p29Name & "]"
            End If
            imgDraft.Visible = .p28IsDraft

        End With



        RefreshBillingLanguage(cRec)

        RefreshPricelist(cRec)



        RefreshProjectList(cRec)

        
        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p28Contact, cRec.PID)
        Me.roles1.RefreshData(lisX69, cRec.PID)
        If Me.roles1.RowsCount = 0 Then panRoles.Visible = False

        Dim lisO37 As IEnumerable(Of BO.o37Contact_Address) = Master.Factory.p28ContactBL.GetList_o37(cRec.PID)
        If lisO37.Count > 0 Then
            Me.boxO37.Visible = True
            Me.address1.FillData(lisO37)
            boxO37Title.Text = BO.BAS.OM2(boxO37Title.Text, lisO37.Count.ToString)
        Else
            Me.boxO37.Visible = False
        End If

        Dim lisO32 As IEnumerable(Of BO.o32Contact_Medium) = Master.Factory.p28ContactBL.GetList_o32(cRec.PID)
        If lisO32.Count > 0 Then
            Me.boxO32.Visible = True
            Me.medium1.FillData(lisO32)
            With Me.boxO32Title
                .Text = BO.BAS.OM2(.Text, lisO32.Count.ToString)
            End With
        Else
            Me.boxO32.Visible = False
        End If
        Dim lisP30 As IEnumerable(Of BO.j02Person) = Master.Factory.p30Contact_PersonBL.GetList_J02(Master.DataPID, 0, True)
        If lisP30.Count > 0 Then
            Me.boxP30.Visible = True
            Me.persons1.FillData(lisP30)
            With Me.boxP30Title
                If lisP30.Count > 0 Then
                    .Text = .Text & " (" & lisP30.Count.ToString & ")"
                    If Master.Factory.SysUser.j04IsMenu_People Then
                        .Text = "<a href='j02_framework.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString & "' target='_top'>" & .Text & "</a>"
                    End If

                End If

            End With
        Else
            Me.boxP30.Visible = False
        End If


        Dim mqO23 As New BO.myQueryO23
        mqO23.p28ID = Master.DataPID
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)

        If lisO23.Count > 0 Then
            Me.boxO23.Visible = True
            notepad1.RefreshData(lisO23, Master.DataPID)
            With Me.boxO23Title
                .Text = .Text & " (" & lisO23.Count.ToString & ")"
                If panO23.Visible Then
                    .Text = "<a href='javascript:notepads()'>" & .Text & "</a>"
                End If
            End With
        Else
            Me.boxO23.Visible = False
        End If
        Dim mq As New BO.myQueryP31
        mq.p28ID_Client = cRec.PID

        If Me.CurrentSubgrid = SubgridType.summary Then
            bigsummary1.Visible = True
            bigsummary1.MasterDataPID = cRec.PID
            bigsummary1.RefreshData()
            boxP31Summary.Visible = False
        Else
            bigsummary1.Visible = False
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            p31summary1.RefreshData(cWorksheetSum, "p28", Master.DataPID)
        End If

        If cRec.b02ID > 0 Then
            Me.trWorkflow.Visible = True
            Me.b02Name.Text = cRec.b02Name
        Else
            Me.trWorkflow.Visible = False
        End If
       
        RefreshComments()


        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p28Contact, Master.DataPID, cRec.p29ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

    End Sub

    Private Sub RefreshComments()
        If Me.CurrentSubgrid = SubgridType.b07 Then
            comments1.RefreshData(Master.Factory, BO.x29IdEnum.p28Contact, Master.DataPID)
            If comments1.RowsCount > 0 Then
                'With Me.opgSubgrid.Items.FindByValue("3")
                '    .Text = BO.BAS.OM2(.Text, comments1.RowsCount.ToString)
                'End With
            End If
        Else
            Dim mqB07 As New BO.myQueryB07
            mqB07.RecordDataPID = Master.DataPID
            mqB07.x29id = BO.x29IdEnum.p28Contact
            Dim lisB07 As IEnumerable(Of BO.b07Comment) = Master.Factory.b07CommentBL.GetList(mqB07)
            If lisB07.Count > 0 Then
                'With Me.opgSubgrid.Items.FindByValue("3")
                '    .Text = BO.BAS.OM2(.Text, lisB07.Count.ToString)
                'End With
            End If
        End If
    End Sub
    Private Sub Handle_Permissions(cRec As BO.p28Contact)
        Dim cDisp As BO.p28RecordDisposition = Master.Factory.p28ContactBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then
            Master.StopPage("Nedisponujete přístupovým oprávněním ke klientovi.")
        End If
        

        With Master.Factory
            panO23.Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator)
            panO22.Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)

            panCreateCommands.Visible = .TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator)
            panCommandPivot.Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            panNewP41.Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)

            bigsummary1.IsApprovingPerson = .SysUser.IsApprovingPerson
            If Not .SysUser.IsApprovingPerson Then
                'schovat záložku pro schvalování
                topLink1.Visible = False

            End If
            If Not .SysUser.j04IsMenu_Project Then topLink5.Visible = False
            If Not .SysUser.j04IsMenu_Invoice Then
                topLink6.Visible = False
                With Me.opgSubgrid.Tabs
                    If Not .FindTabByValue("2") Is Nothing Then
                        .Remove(.FindTabByValue("2"))  'nemá právo vidět vystavené faktury
                    End If
                End With
                If Me.CurrentSubgrid = SubgridType.p91 Then Me.CurrentSubgrid = SubgridType.p31
            End If
            gridP31.AllowApproving = .SysUser.IsApprovingPerson
            gridP56.AllowApproving = .SysUser.IsApprovingPerson
        End With


        panEdit.Visible = cDisp.OwnerAccess
        If Not (panEdit.Visible Or panCreateCommands.Visible Or panNewP41.Visible) Then
            menu1.Items.Remove(menu1.FindItemByValue("record"))
        End If

        cmdEditP30.Visible = cDisp.OwnerAccess
        panP30.Visible = cDisp.OwnerAccess
        cmdLog.Visible = cDisp.OwnerAccess

        If cRec.b02ID = 0 And cRec.p28IsDraft And cDisp.OwnerAccess Then
            panDraftCommands.Visible = True 'pokud je vlastník a projekt nemá workflow šablonu
        Else
            panDraftCommands.Visible = False
        End If

        If cRec.IsClosed Then
            panO22.Visible = False : panNewP41.Visible = False : panP30.Visible = False 'klient je v archivu
            ScriptManager.RegisterStartupScript(Me.placeBinMenuCss, Me.GetType(), "BinMenu", "<style type='text/css'>.RadMenu_Silk .rmItem {background-color:black !important;}</style>", False)
        End If

    End Sub

    Private Sub RefreshProjectList(cRec As BO.p28Contact)

        Dim mq As New BO.myQueryP41
        mq.p28ID = cRec.PID
        mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
        mq.Closed = BO.BooleanQueryMode.NoQuery


        Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq)
        If lis.Count = 0 Then
            boxP41.Visible = False
        Else
            boxP41.Visible = True
            Dim intClosed As Integer = lis.Where(Function(p) p.IsClosed = True).Count
            Dim intOpened As Integer = lis.Count - intClosed
            With boxP41Title
                If intClosed > 0 Then
                    .Text = .Text & " (" & intOpened.ToString & IIf(intClosed > 0, "+" & intClosed.ToString, "") & ")"
                    topLink5.Text += "<span class='badge1'>" & intOpened.ToString & IIf(intClosed > 0, "+" & intClosed.ToString, "") & "</span>"
                Else
                    .Text = .Text & " (" & intOpened.ToString & ")"
                    topLink5.Text += "<span class='badge1'>" & intOpened.ToString & "</span>"
                End If                

                .Text = "<a href='javascript:projects()'>" & .Text & "</a>"
            End With

        End If

        rpP41.DataSource = lis.OrderBy(Function(p) p.IsClosed).ThenBy(Function(p) p.p41Name)
        rpP41.DataBind()

    End Sub



    

    Private Sub RefreshPricelist(cRec As BO.p28Contact)
        Me.clue_p51id_billing.Visible = False : Me.p51Name_Billing.Visible = False : lblX51.Visible = False
        With cRec
            If .p51ID_Billing > 0 Then
                Me.p51Name_Billing.Visible = True : clue_p51id_billing.Visible = True : lblX51.Visible = True
                Me.p51Name_Billing.Text = .p51Name_Billing
                Try
                    If .p51Name_Billing.IndexOf(cRec.p28Name) >= 0 Then
                        Me.p51Name_Billing.Text = "Sazby na míru"
                    End If
                    Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
                Catch ex As Exception

                End Try
                

            End If
        End With
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
            Me.clue_p51id_billing.Visible = False  'uživatel nemá oprávnění vidět sazby
        End If

    End Sub
    Private Sub RefreshBillingLanguage(cRec As BO.p28Contact)
        imgFlag_Contact.Visible = False
        With cRec
            If .p87ID > 0 Then
                Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID)
                If cP87.p87Icon <> "" Then
                    imgFlag_Contact.Visible = True
                    imgFlag_Contact.ImageUrl = "Images/flags/" & cP87.p87Icon
                End If

            End If
        End With
    End Sub



    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "draft2normal"
                With Master.Factory.p28ContactBL
                    If .ConvertFromDraft(Master.DataPID) Then
                        ReloadPage(Master.DataPID.ToString)
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End With
            Case "p31-save", "p31-delete"
                If Me.CurrentSubgrid = SubgridType.p31 Then
                    gridP31.RecalcVirtualRowCount()
                    gridP31.Rebind(True)
                End If

            Case Else
                ReloadPage(Master.DataPID.ToString)
        End Select
        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub

    

    Private Sub ReloadPage(strPID As String)
        Response.Redirect("p28_framework_detail.aspx?pid=" & strPID)
    End Sub


    Private Sub p28_framework_detail_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.gridP31.Visible = False : Me.comments1.Visible = False
        Select Case Me.CurrentSubgrid
            Case SubgridType.p31
                Me.gridP31.Visible = True
            Case SubgridType.b07
                Me.comments1.Visible = True
        End Select
        If Me.CurrentSubgrid = SubgridType._NotSpecified Then
            panProjects.Style.Item("max-height") = ""
            panProjects.Style.Item("overflow") = ""
        Else
            panProjects.Style.Item("max-height") = "250px"
            panProjects.Style.Item("overflow") = "auto"
        End If
        
    End Sub

    Private Sub rpP41_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP41.ItemDataBound
        Dim cRec As BO.p41Project = CType(e.Item.DataItem, BO.p41Project)
        With CType(e.Item.FindControl("aProject"), HyperLink)
            If cRec.p41NameShort > "" Then
                .Text = cRec.p41NameShort
            Else
                .Text = cRec.p41Name
            End If
            .Text += " (" & cRec.p41Code & ")"
            If Master.Factory.SysUser.j04IsMenu_Project Then
                .NavigateUrl = "p41_framework.aspx?pid=" & cRec.PID.ToString
            End If

            If cRec.IsClosed Then .Font.Strikeout = True : .ForeColor = Drawing.Color.Gray
        End With
        CType(e.Item.FindControl("clue_project"), HyperLink).Attributes.Item("rel") = "clue_p41_record.aspx?pid=" & cRec.PID.ToString
    End Sub

    'Private Function SaveState() As String
    '    Dim dockStates As List(Of DockState) = RadDockLayout1.GetRegisteredDocksState()
    '    Dim serializer As New JavaScriptSerializer()
    '    Dim converters As New List(Of JavaScriptConverter)()
    '    converters.Add(New UnitConverter())
    '    serializer.RegisterConverters(converters)

    '    Dim stateString As String = [String].Empty
    '    For Each state As DockState In dockStates
    '        Dim ser As String = serializer.Serialize(state)
    '        stateString = stateString + "|" + ser
    '    Next
    '    Return stateString
    'End Function

    

    

    'Private Sub LoadState(myDockState As String, dockParents As Dictionary(Of String, String), dockIndices As Dictionary(Of String, Integer))
    '    Dim dock As New RadDock()
    '    Dim serializer As New JavaScriptSerializer()
    '    Dim converters As New List(Of JavaScriptConverter)()
    '    converters.Add(New UnitConverter())
    '    serializer.RegisterConverters(converters)
    '    For Each str As String In myDockState.Split("|"c)
    '        If str <> [String].Empty Then
    '            Dim state As DockState = serializer.Deserialize(Of DockState)(str)
    '            dock = TryCast(RadDockLayout1.FindControl(state.UniqueName), RadDock)
    '            dock.ApplyState(state)
    '            dockParents(state.UniqueName) = state.DockZoneID
    '            dockIndices(state.UniqueName) = state.Index
    '        End If
    '    Next
    'End Sub

    'Private Sub RadDockLayout1_LoadDockLayout(sender As Object, e As Telerik.Web.UI.DockLayoutEventArgs) Handles RadDockLayout1.LoadDockLayout
    '    If Not Page.IsPostBack Then
    '        Dim s As String = Master.Factory.j03UserBL.LoadDockState("p28_framework_detail.aspx")
    '        If s <> "" Then
    '            LoadState(s, e.Positions, e.Indices)
    '        End If

    '    End If
    'End Sub


    'Private Sub cmdResetDockState_Click(sender As Object, e As EventArgs) Handles cmdResetDockState.Click
    '    Master.Factory.j03UserBL.SaveDockState("p28_framework_detail.aspx", "")
    '    ReloadPage(Master.DataPID.ToString)
    'End Sub

    'Private Sub cmdSaveDockState_Click(sender As Object, e As EventArgs) Handles cmdSaveDockState.Click
    '    Master.Factory.j03UserBL.SaveDockState("p28_framework_detail.aspx", SaveState())
    'End Sub

    

    

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub opgSubgrid_TabClick(sender As Object, e As RadTabStripEventArgs) Handles opgSubgrid.TabClick
        Master.Factory.j03UserBL.SetUserParam("p28_framework_detail-subgrid", Me.opgSubgrid.SelectedTab.Value)
        ReloadPage(Master.DataPID.ToString)
    End Sub
End Class