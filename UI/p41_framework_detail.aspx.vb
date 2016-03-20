Imports Telerik.Web.UI
Imports System.Web.Script.Serialization
Public Class p41_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Enum SubgridType
        summary = -1
        p31 = 1
        p91 = 2
        b07 = 3
        p56 = 4
        p45 = 5
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
    Public ReadOnly Property CurrentP28ID_Client As Integer
        Get
            Return BO.BAS.IsNullInt(ViewState("p28id_client"))
        End Get
    End Property
    Private Sub p41_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            gridP31.Factory = .Factory
            p31summary1.Factory = .Factory

            gridP91.Factory = .Factory
            gridP56.Factory = .Factory
            ff1.Factory = .Factory
            bigsummary1.Factory = .Factory
        End With
        

        If Not Page.IsPostBack Then
            ViewState("p28id_client") = ""
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneProjectPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneProjectPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p41_framework_detail-pid")
                    .Add("p41_framework_detail-subgrid")
                    .Add("p41_framework_detail-chkFFShowFilledOnly")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail-chkFFShowFilledOnly", "0"))
                End With


                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p41_framework_detail-pid", "O"))
                    If .DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")
                Else
                    If .DataPID <> BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p41_framework_detail-pid", "O")) Then
                        .Factory.j03UserBL.SetUserParam("p41_framework_detail-pid", .DataPID.ToString)
                    End If
                End If
                With .Factory.j03UserBL
                    'Me.chkLockedDocks.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail-chkLockedDocks", "0"))
                    Me.CurrentSubgrid = DirectCast(CInt(.GetUserParam("p41_framework_detail-subgrid", "1")), SubgridType)
                    If Request.Item("force") = "comment" Then
                        Me.CurrentSubgrid = SubgridType.b07
                    End If

                End With


            End With
           
            gridP91.Visible = False : gridP56.Visible = False
            Select Case Me.CurrentSubgrid
                Case SubgridType.p56
                    gridP56.Visible = True
                Case SubgridType.p91
                    gridP91.Visible = True
                Case SubgridType.p45
                    SetupGridBudget()
            End Select

            RefreshRecord()

        End If

        gridP31.MasterDataPID = 0     'uvnitř prvku nebude docházet k plnění gridu
        Select Case Me.CurrentSubgrid
            Case SubgridType.p56
                gridP56.MasterDataPID = Master.DataPID
            Case SubgridType.p31
                gridP31.MasterDataPID = Master.DataPID
            Case SubgridType.p91
                gridP91.MasterDataPID = Master.DataPID
            Case SubgridType.b07

        End Select
        
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")
        Handle_Permissions(cRec)

        Dim cClient As BO.p28Contact = Nothing

        With cRec
            cmdNewWindow.NavigateUrl = "p41_framework.aspx?blankwindow=1&pid=" & .PID.ToString & "&title=" & .FullName
            ViewState("p28id_client") = .p28ID_Client.ToString

            Me.Project.Text = .p41Name & " <span style='color:gray;padding-left:10px;'>" & .p41Code & "</span>"

            If .p28ID_Client > 0 Then
                Me.Client.Text = .Client : Me.Client.Visible = True
                If Master.Factory.SysUser.j04IsMenu_Contact Then
                    Me.Client.NavigateUrl = "p28_framework.aspx?pid=" & .p28ID_Client.ToString
                End If
                cClient = Master.Factory.p28ContactBL.Load(.p28ID_Client)
                Me.clue_client.Attributes("rel") = "clue_p28_record.aspx?pid=" & .p28ID_Client.ToString
            Else
                Me.clue_client.Visible = False : Me.Client.Visible = False
            End If
            If .j18ID > 0 Then
                Me.clue_j18name.Attributes("rel") = "clue_j18_record.aspx?pid=" & .j18ID.ToString
            Else
                Me.clue_j18name.Visible = False
            End If
            Me.p42Name.Text = .p42Name
            Me.clue_p42name.Attributes("rel") = "clue_p42_record.aspx?pid=" & .p42ID.ToString
            lblJ18Name.Visible = False : Me.j18Name.Visible = False
            If .j18ID > 0 Then
                lblJ18Name.Visible = True : Me.j18Name.Visible = True
                Me.j18Name.Text = .j18Name
            End If
            If .b01ID > 0 Then
                Me.trWorkflow.Visible = True
                Me.b02Name.Text = .b02Name
            Else
                Me.trWorkflow.Visible = False
            End If

            If Not (.p41PlanFrom Is Nothing Or .p41PlanUntil Is Nothing) Then
                Me.PlanPeriod.Text = "<b style='color:green;'>" & BO.BAS.FD(.p41PlanFrom.Value) & "</b> - <b style='color:red;'>" & BO.BAS.FD(.p41PlanUntil.Value) & "</b>"
                If DateDiff(DateInterval.Day, .p41PlanFrom.Value, .p41PlanUntil.Value) < 750 Then
                    Me.PlanPeriod.Text += " [" & DateDiff(DateInterval.Day, .p41PlanFrom.Value, .p41PlanUntil.Value).ToString & "d.]"
                End If
                trPlan.Visible = True
            Else
                trPlan.Visible = False

            End If
            Me.imgDraft.Visible = .p41IsDraft

        End With

        RefreshBillingLanguage(cRec, cClient)

        RefreshPricelist(cRec, cClient)

        RefreshOtherBillingSetting(cRec, cClient)

        If Not (cRec.IsClosed Or cRec.p41IsDraft) Then
            Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = Master.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.PID, cRec.p42ID, cRec.j18ID, Master.Factory.SysUser.j02ID)
            With menu1.FindItemByValue("p31")
                Dim rp As Repeater = CType(.ContentTemplateContainer.FindControl("rp1"), Repeater)
                AddHandler rp.ItemDataBound, AddressOf Me.p34_ItemDataBound
                With rp
                    .DataSource = lisP34
                    .DataBind()
                End With
                If lisP34.Count = 0 Then
                    lblP31Message.Text = "V tomto projektu nedisponujete oprávněním k zapisování úkonů."
                End If

            End With
        Else
            If cRec.IsClosed Then
                lblP31Message.Text = "Do projektu v archivu nelze zapisovat nové úkony."
                ScriptManager.RegisterStartupScript(Me.placeBinMenuCss, Me.GetType(), "BinMenu", "<style type='text/css'>.RadMenu_Silk .rmItem {background-color:black !important;}</style>", False)
            End If
            If cRec.p41IsDraft Then Me.lblP31Message.Text = "Do projektu v režimu DRAFT nelze zapisovat úkony."
        End If
        

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)

        Dim mqO23 As New BO.myQueryO23
        mqO23.p41ID = Master.DataPID
        mqO23.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
        If lisO23.Count > 0 Then
            Me.boxO23.Visible = True
            notepad1.RefreshData(lisO23, Master.DataPID)
            With Me.boxO23Title
                .Text = BO.BAS.OM2(.Text, lisO23.Count.ToString)
                If panO23.Visible Then
                    .Text = "<a href='javascript:notepads()'>" & .Text & "</a>"
                End If
            End With
        Else
            Me.boxO23.Visible = False
        End If
        Dim lisP30 As IEnumerable(Of BO.j02Person) = Master.Factory.p30Contact_PersonBL.GetList_J02(0, Master.DataPID, False)
        If lisP30.Count > 0 Then
            Me.boxP30.Visible = True
            Me.persons1.FillData(lisP30)
            With Me.boxP30Title
                .Text = BO.BAS.OM2(.Text, lisP30.Count.ToString)
                If Master.Factory.SysUser.j04IsMenu_People Then
                    .Text = "<a href='j02_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString & "' target='_top'>" & .Text & "</a>"
                End If
            End With
        Else
            Me.boxP30.Visible = False
        End If

        Dim mq As New BO.myQueryP31
        mq.p41ID = cRec.PID
        
        Dim cProjectSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

        If Me.CurrentSubgrid = SubgridType.summary Then
            bigsummary1.Visible = True
            bigsummary1.MasterDataPID = cRec.PID
            bigsummary1.RefreshData()
            boxP31Summary.Visible = False
        Else
            bigsummary1.Visible = False
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            p31summary1.RefreshData(cWorksheetSum, "p41", Master.DataPID, cRec.p41LimitHours_Notification)
        End If

        With Me.opgSubgrid.Tabs
            If Not .FindTabByValue("2") Is Nothing Then
                If cProjectSum.p91_Count > 0 Then
                    .FindTabByValue("2").Text += "<span class='badge1'>" & cProjectSum.p91_Count.ToString & "</span>"
                    topLink6.Text += "<span class='badge1'>" & cProjectSum.p91_Count.ToString & "</span>"
                End If
            End If
            With .FindTabByValue("4")
                If cProjectSum.p56_Actual_Count > 0 Then .Text += "<span class='badge1'>" & cProjectSum.p56_Actual_Count.ToString & "</span>"
            End With
        End With
        

        If cProjectSum.p56_Actual_Count > 0 Or cProjectSum.o22_Actual_Count > 0 Then
            If cProjectSum.p56_Actual_Count > 0 Then topLink2.Text = topLink2.Text & "<span title='Otevřené úkoly' class='badge1'>" & cProjectSum.p56_Actual_Count.ToString & "</span>"
            If cProjectSum.o22_Actual_Count > 0 Then topLink3.Text = topLink3.Text & "<span title='Události v kalendáři' class='badge1'>" & cProjectSum.o22_Actual_Count.ToString & "</span>"

        End If
        
        
        
        basUIMT.RenderHeaderMenu(cRec.IsClosed, Me.panMenuContainer, menu1)
        Dim strLevel1 As String = cRec.FullName
        If Len(cRec.Client) > 30 Then
            strLevel1 = Left(cRec.Client, 30) & "..."
            If cRec.p41NameShort <> "" Then
                strLevel1 += " - " & cRec.p41NameShort
            Else
                strLevel1 += " - " & cRec.p41Name
            End If
        End If
        basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), strLevel1, "p41_framework_detail.aspx?pid=" & cRec.PID.ToString, cRec.IsClosed)

        RefreshComments()


        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p41Project, Master.DataPID, cRec.p42ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

        If Me.CurrentSubgrid = SubgridType.p45 Then
            panP45.Visible = True
            Dim lis As IEnumerable(Of BO.p45Budget) = Master.Factory.p45BudgetBL.GetList(Master.DataPID)
            If lis.Count > 0 Then
                Me.p45ID.DataSource = lis
                Me.p45ID.DataBind()
                cmdP45.InnerText = "Nastavení rozpočtu"
            Else
                Me.p45ID.Visible = False
                cmdP45.InnerText = "Založit rozpočet"
            End If

        Else
            panP45.Visible = False
        End If


        RefreshP40(cRec)
    End Sub

    Private Sub RefreshP40(cRec As BO.p41Project)
        Dim lisP40 As IEnumerable(Of BO.p40WorkSheet_Recurrence) = Master.Factory.p40WorkSheet_RecurrenceBL.GetList(Master.DataPID)
        rpP40.DataSource = lisP40
        rpP40.DataBind()
        If lisP40.Count = 0 Then
            boxP40.Visible = False
        Else
            boxP40.Visible = True
        End If
    End Sub

   
    Private Sub RefreshComments()
        If Me.CurrentSubgrid = SubgridType.b07 Then
            comments1.RefreshData(Master.Factory, BO.x29IdEnum.p41Project, Master.DataPID)
            If comments1.RowsCount > 0 Then
                With Me.opgSubgrid.Tabs.FindTabByValue("3")
                    .Text += "<span class='badge1'>" & comments1.RowsCount.ToString & "</span>"
                End With
            End If
        Else
            Dim mqB07 As New BO.myQueryB07
            mqB07.RecordDataPID = Master.DataPID
            mqB07.x29id = BO.x29IdEnum.p41Project
            Dim lisB07 As IEnumerable(Of BO.b07Comment) = Master.Factory.b07CommentBL.GetList(mqB07)
            If lisB07.Count > 0 Then
                With Me.opgSubgrid.Tabs.FindTabByValue("3")
                    .Text += "<span class='badge1'>" & lisB07.Count.ToString & "</span>"
                End With
            End If
        End If
    End Sub

    Private Sub Handle_Permissions(cRec As BO.p41Project)
        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        cmdLog.Visible = cDisp.OwnerAccess

        With Master.Factory
            panCreateCommands.Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)
            panCommandPivot.Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            panO23.Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator)
            panO22.Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)
            If cRec.b01ID <> 0 Then Me.panB07.Visible = False

            Dim bolCanApprove As Boolean = .TestPermission(BO.x53PermValEnum.GR_P31_Approver)
            If bolCanApprove = False And cDisp.x67IDs.Count > 0 Then
                Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Master.Factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                    bolCanApprove = True
                End If
            End If
            topLink1.Visible = bolCanApprove
            If Not bolCanApprove Then Me.p31summary1.DisableApprovingButton()

            Me.bigsummary1.IsApprovingPerson = bolCanApprove
            gridP31.AllowApproving = bolCanApprove
            gridP56.AllowApproving = bolCanApprove
        End With
        With cDisp
            panP56.Visible = .P56_Create
            gridP56.IsAllowedCreateTasks = .P56_Create

            boxP30.Visible = .OwnerAccess
            panP31Recalc.Visible = .P31_RecalcRates
            panP31MoveToBin.Visible = .P31_Move2Bin
            panP31Move2OtherProject.Visible = .P31_MoveToOtherProject
            panEdit.Visible = .OwnerAccess
            panP40.Visible = .OwnerAccess
            panP30.Visible = .OwnerAccess
            topLink6.Visible = Master.Factory.SysUser.j04IsMenu_Invoice
            If Not .p91_Read Then
                topLink6.Visible = False
                With Me.opgSubgrid.Tabs
                    If Not .FindTabByValue("2") Is Nothing Then
                        .Remove(.FindTabByValue("2"))  'nemá právo vidět vystavené faktury v projektu
                    End If
                End With
                If Me.CurrentSubgrid = SubgridType.p91 Then Me.CurrentSubgrid = SubgridType.p31
            End If
        End With

        If Not (panEdit.Visible Or panCreateCommands.Visible) Then
            Try
                menu1.Items.Remove(menu1.FindItemByValue("record"))
            Catch ex As Exception

            End Try

        End If
        panDraftCommands.Visible = False
        If cRec.b02ID = 0 And cRec.p41IsDraft And cDisp.OwnerAccess Then
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator) Then panDraftCommands.Visible = True 'pokud je vlastník a má právo zakládat ostré projekty a projekt nemá workflow šablonu
        End If


        If cRec.IsClosed Then panO22.Visible = False : panP40.Visible = False : panP56.Visible = False 'projekt je v archivu
    End Sub

    Private Sub RefreshOtherBillingSetting(cRec As BO.p41Project, cClient As BO.p28Contact)
        Dim b As Boolean = False
        With cRec
            If .p87ID > 0 Or .p92ID > 0 Or .p28ID_Billing > 0 Then
                b = True
            End If
        End With
        'If Not cClient Is Nothing Then
        '    With cClient
        '        If .p87ID > 0 Or .p92ID > 0 Then
        '            b = True
        '        End If
        '    End With
        'End If
        Me.clue_p41_billing.Visible = b
        If b Then
            Me.clue_p41_billing.Attributes("rel") = "clue_p41_record_billingsetting.aspx?pid=" & cRec.PID.ToString
        End If
    End Sub

    Private Sub RefreshPricelist(cRec As BO.p41Project, cClient As BO.p28Contact)
        Me.clue_p51id_billing.Visible = False : Me.p51Name_Billing.Visible = False : Me.lblX51_Message.Text = ""
        With cRec
            If .p51ID_Billing > 0 Then
                Me.p51Name_Billing.Visible = True : clue_p51id_billing.Visible = True
                Me.p51Name_Billing.Text = .p51Name_Billing
                If .p51Name_Billing.IndexOf(cRec.p41Name) >= 0 Then
                    'sazby na míru
                    p51Name_Billing.Text = "Tento projekt má sazby na míru"
                End If
                Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
            Else
                If Not cClient Is Nothing Then
                    With cClient
                        If .p51ID_Billing > 0 Then
                            Me.lblX51_Message.Text = "(dědí se z klienta)"
                            Me.p51Name_Billing.Visible = True : clue_p51id_billing.Visible = True
                            Me.p51Name_Billing.Text = .p51Name_Billing
                            Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
                        End If
                    End With
                End If
            End If
        End With
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
            Me.clue_p51id_billing.Visible = False  'uživatel nemá oprávnění vidět sazby
        End If

    End Sub
    Private Sub RefreshBillingLanguage(cRec As BO.p41Project, cClient As BO.p28Contact)
        imgFlag_Project.Visible = False : imgFlag_Client.Visible = False
        With cRec
            If .p87ID > 0 Then
                Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID)
                If cP87.p87Icon <> "" Then
                    imgFlag_Project.Visible = True
                    imgFlag_Project.ImageUrl = "Images/flags/" & cP87.p87Icon
                End If

            End If
            If .p87ID_Client > 0 Then
                If Not cClient Is Nothing Then
                    Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID_Client)
                    If cP87.p87Icon <> "" Then
                        imgFlag_Client.Visible = True
                        imgFlag_Client.ImageUrl = "Images/flags/" & cP87.p87Icon
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub p34_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Dim cRec As BO.p34ActivityGroup = CType(e.Item.DataItem, BO.p34ActivityGroup)
        With CType(e.Item.FindControl("aP34"), HyperLink)
            .Text = "Zapsat úkon do [" & cRec.p34Name & "]"
            .NavigateUrl = "javascript:p31_entry_menu(" & cRec.PID.ToString & ")"
        End With

    End Sub


    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "draft2normal"
                With Master.Factory.p41ProjectBL
                    If .ConvertFromDraft(Master.DataPID) Then
                        ReloadPage(Master.DataPID.ToString)
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End With
            Case "p41-create"

                ReloadPage(Me.hidHardRefreshPID.Value)
            Case "p31-save", "p31-delete"
                Select Case Me.CurrentSubgrid
                    Case SubgridType.summary
                        RefreshRecord()
                    Case SubgridType.p31
                        gridP31.RecalcVirtualRowCount()
                        gridP31.Rebind(True)
                    Case SubgridType.p56
                        gridP56.Rebind(True)
                End Select
                If Me.CurrentSubgrid = SubgridType.p31 Then

                End If
                If Me.CurrentSubgrid = SubgridType.p56 Then

                End If

            Case "p51-save"
                Master.Notify("Pokud jste změnili sazby v ceníku a potřebujete přepočítat sazby u již uložené rozpracovanosti, použijte k tomu nástroj [Přepočítat sazby rozpracovaných úkonů].", NotifyLevel.InfoMessage)
            

            Case Else
                ReloadPage(Master.DataPID.ToString)
        End Select
        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub

    Private Sub p41_framework_detail_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.gridP31.Visible = False : comments1.Visible = False
        Select Case Me.CurrentSubgrid
            Case SubgridType.p31
                Me.gridP31.Visible = True
            Case SubgridType.b07
                comments1.Visible = True
        End Select
       
    End Sub

    

    Private Sub ReloadPage(strPID As String)
        Response.Redirect("p41_framework_detail.aspx?pid=" & strPID)
    End Sub

  


    
    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub rpP40_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP40.ItemDataBound
        Dim cRec As BO.p40WorkSheet_Recurrence = CType(e.Item.DataItem, BO.p40WorkSheet_Recurrence)
        With CType(e.Item.FindControl("p40Name"), HyperLink)
            .Text = cRec.p40Name & " (" & cRec.p34Name & "): " & BO.BAS.FN(cRec.p40Value) & ",-"
            .NavigateUrl = "javascript:p40_record(" & cRec.PID.ToString & ")"
        End With
        With CType(e.Item.FindControl("clue_p40"), HyperLink)
            .Attributes("rel") = "clue_p40_record.aspx?pid=" & cRec.PID.ToString
        End With

    End Sub

    Private Sub opgSubgrid_TabClick(sender As Object, e As RadTabStripEventArgs) Handles opgSubgrid.TabClick
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail-subgrid", Me.opgSubgrid.SelectedTab.Value)
        ReloadPage(Master.DataPID.ToString)
    End Sub

    Private Sub SetupGridBudget()
        With gridP46
            .ClearColumns()
            .radGridOrig.ShowFooter = True
            .radGridOrig.AllowSorting = False
            .radGridOrig.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            .PageSize = 20
            Dim group As New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            group.Name = "rozpocet_hodiny" : group.HeaderText = "Hodiny rozpočtu"
            group = New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            group.Name = "rozpocet_cena" : group.HeaderText = "Cena rozpočtu"
            group = New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            group.Name = "timesheet_hodiny" : group.HeaderText = "Vykázané hodiny"
            group = New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            group.Name = "timesheet_cena" : group.HeaderText = "Cena"

            .AddColumn("Person", "Osoba", , True)
            .AddColumn("p46HoursBillable", "Fa", BO.cfENUM.Numeric0, , , , , True, , "rozpocet_hodiny")
            .AddColumn("p46HoursNonBillable", "NeFa", BO.cfENUM.Numeric0, , , , , True, , "rozpocet_hodiny")
            .AddColumn("p46HoursTotal", "Celkem", BO.cfENUM.Numeric0, , , , , True, , "rozpocet_hodiny")

            .AddColumn("BillingAmount", "Fakturační", BO.cfENUM.Numeric, , , , , True, , "rozpocet_cena")
            .AddColumn("CostAmount", "Nákladová", BO.cfENUM.Numeric, , , , , True, , "rozpocet_cena")
           
            .AddColumn("TimesheetFa", "Fa", BO.cfENUM.Numeric2, , , , , True, , "timesheet_hodiny")
            .AddColumn("TimesheetNeFa", "NeFa", BO.cfENUM.Numeric2, , , , , True, , "timesheet_hodiny")
            .AddColumn("TimesheetAll", "Celkem", BO.cfENUM.Numeric2, , , , , True, , "timesheet_hodiny")
            .AddColumn("TimesheetAllVersusBudget", "+-", BO.cfENUM.Numeric, False, , , , True, , "timesheet_hodiny")
            
            .AddColumn("TimeshetAmountBilling", "Fakturační", BO.cfENUM.Numeric2, , , , , True, , "timesheet_cena")
            .AddColumn("TimesheetAmountCost", "Nákladová", BO.cfENUM.Numeric2, , , , , True, , "timesheet_cena")

        End With
        ''With Me.gridBudgetExpense
        ''    .ClearColumns()
        ''    .radGridOrig.ShowFooter = True
        ''    .AddColumn("p34Name", "Sešit")
        ''    .AddColumn("p32Name", "Aktivita")
        ''    .AddColumn("AmountWithoutVat", "Vykázáno bez DPH", BO.cfENUM.Numeric, , , , , True)
        ''    .AddColumn("Pocet", "Počet", BO.cfENUM.Numeric0)

        ''    Dim GGE As New Telerik.Web.UI.GridGroupByExpression
        ''    Dim fld As New GridGroupByField
        ''    fld.FieldName = "j27Code"
        ''    fld.HeaderText = "Měna"
        ''    GGE.SelectFields.Add(fld)
        ''    GGE.GroupByFields.Add(fld)
        ''    .radGridOrig.MasterTableView.GroupByExpressions.Add(GGE)
        ''End With
    End Sub

    Private Sub gridP46_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gridP46.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Or gridP46.Visible = False Then Return
        
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p46BudgetPersonExtented = CType(e.Item.DataItem, BO.p46BudgetPersonExtented)
        If cRec.TimesheetAllVersusBudget > 0 Then
            'vykázáno přes rozpočet
            dataItem.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Private Sub gridP46_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridP46.NeedDataSource
        If Me.p45ID.Items.Count = 0 Then
            gridP46.Visible = False
            Return
        End If
        Dim intP45ID As Integer = CInt(Me.p45ID.SelectedValue)
        Dim lis As IEnumerable(Of BO.p46BudgetPersonExtented) = Master.Factory.p45BudgetBL.GetList_p46_extended(intP45ID, Master.DataPID)
        gridP46.DataSource = lis


    End Sub

    ''Private Sub gridBudgetExpense_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridBudgetExpense.NeedDataSource
    ''    Dim mq As New BO.myQueryP31
    ''    mq.p41ID = Master.DataPID
    ''    mq.IsExpenses = True
    ''    Dim lis As IEnumerable(Of BO.WorksheetExpenseSummary) = Master.Factory.p31WorksheetBL.GetList_ExpenseSummary(mq)
    ''    Me.gridBudgetExpense.DataSource = lis

    ''End Sub
End Class