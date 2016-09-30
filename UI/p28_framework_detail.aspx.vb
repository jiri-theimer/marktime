Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

Public Class p28_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Private Property _curProjectIndex As Integer
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
            p31summary1.Factory = .Factory
            ff1.Factory = .Factory
        End With
        

        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            With Master
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p28_framework_detail-subgrid", Request.Item("tab"))
                End If
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
                    .Add("p28_framework_detail-switchHeight")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                End With
                With .Factory.j03UserBL
                    panSwitch.Style.Item("display") = .GetUserParam("p28_framework_detail-switch", "block")
                    Dim strHeight As String = .GetUserParam("p28_framework_detail-switchHeight", "auto")
                    If strHeight = "auto" Then
                        panSwitch.Style.Item("height") = "" : panSwitch.Style.Item("overflow") = ""
                    Else
                        panSwitch.Style.Item("height") = strHeight & "px"
                    End If
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


            RefreshRecord()

        End If

        For Each t As RadTab In Me.opgSubgrid.Tabs
            Select Case t.Value
                Case "-1" : t.NavigateUrl = "entity_framework_p31summary.aspx?masterprefix=p28&masterpid=" & Master.DataPID.ToString
                Case "1" : t.NavigateUrl = "entity_framework_p31subform.aspx?masterprefix=p28&masterpid=" & Master.DataPID.ToString
                Case "2" : t.NavigateUrl = "entity_framework_p91subform.aspx?masterprefix=p28&masterpid=" & Master.DataPID.ToString
                Case "3" : t.NavigateUrl = "entity_framework_b07subform.aspx?masterprefix=p28&masterpid=" & Master.DataPID.ToString
                Case "4" : t.NavigateUrl = "entity_framework_p56subform.aspx?masterprefix=p28&masterpid=" & Master.DataPID.ToString
            End Select
        Next
        If Me.CurrentSubgrid = SubgridType._NotSpecified Then
            fraSubform.Visible = False : imgLoading.Visible = False
            panSwitch.Style.Item("height") = ""
            For Each t As RadTab In Me.opgSubgrid.Tabs
                t.NavigateUrl = ""
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
            If .p28ParentID <> 0 Then
                Me.trParent.Visible = True
                Me.ParentContact.NavigateUrl = "p28_framework.aspx?pid=" & .p28ParentID.ToString
                Me.ParentContact.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, .p28ParentID)
            End If
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
                If menu1.FindItemByValue("cmdO23").Visible Then
                    .Text = "<a href='javascript:notepads()'>" & .Text & "</a>"
                End If
            End With
        Else
            Me.boxO23.Visible = False
        End If
        Dim mq As New BO.myQueryP31
        mq.p28ID_Client = cRec.PID

        If Me.CurrentSubgrid = SubgridType.summary Then
            boxP31Summary.Visible = False
        Else
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

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p28Contact).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_decide('x18_binding.aspx?prefix=p28&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p28Contact, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p28Contact, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p28Contact, Master.DataPID, cRec.p29ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If
        If Master.Factory.p28ContactBL.HasChildRecords(cRec.PID) Then
            topLink7.Visible = True
            Dim mq2 As New BO.myQueryP28
            mq2.MG_SelectPidFieldOnly = True
            mq2.p28ParentID = cRec.PID
            topLink7.Text += "<span class='badge1'>" & Master.Factory.p28ContactBL.GetList(mq2).Count.ToString & "</span>"
        End If
    End Sub

    Private Sub RefreshComments()
        Dim mqB07 As New BO.myQueryB07
        mqB07.RecordDataPID = Master.DataPID
        mqB07.x29id = BO.x29IdEnum.p28Contact
        Dim lisB07 As IEnumerable(Of BO.b07Comment) = Master.Factory.b07CommentBL.GetList(mqB07)
        If lisB07.Count > 0 Then
            With Me.opgSubgrid.Tabs.FindTabByValue("3")
                .Text = BO.BAS.OM2(.Text, lisB07.Count.ToString)
            End With
        End If
    End Sub
    Private Sub Handle_Permissions(cRec As BO.p28Contact)
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString
        Dim cDisp As BO.p28RecordDisposition = Master.Factory.p28ContactBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then
            Master.StopPage("Nedisponujete přístupovým oprávněním ke klientovi.")
        End If
        x18_binding.Visible = cDisp.OwnerAccess

        With Master.Factory
            menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator)
            menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)

            menu1.FindItemByValue("cmdNew").Visible = .TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator)
            menu1.FindItemByValue("cmdCopy").Visible = menu1.FindItemByValue("cmdNew").Visible
            menu1.FindItemByValue("cmdPivot").Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString
            menu1.FindItemByValue("cmdNewP41").Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)

            If Not .SysUser.IsApprovingPerson Then
                'schovat záložku pro schvalování
                topLink1.Visible = False
            Else
                topLink1.Visible = .TestPermission(BO.x53PermValEnum.PR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator)
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
        End With


        menu1.FindItemByValue("cmdEdit").Visible = cDisp.OwnerAccess
        menu1.FindItemByValue("cmdCopy").Visible = cDisp.OwnerAccess

        cmdEditP30.Visible = cDisp.OwnerAccess
        menu1.FindItemByValue("cmdP30").Visible = cDisp.OwnerAccess
        menu1.FindItemByValue("cmdLog").Visible = cDisp.OwnerAccess

        If cRec.b02ID = 0 And cRec.p28IsDraft And cDisp.OwnerAccess Then
            panDraftCommands.Visible = True 'pokud je vlastník a projekt nemá workflow šablonu
        Else
            panDraftCommands.Visible = False
        End If

        If cRec.IsClosed Then
            menu1.FindItemByValue("cmdO22").Visible = False : menu1.FindItemByValue("cmdNewP41").Visible = False : menu1.FindItemByValue("cmdP30").Visible = False 'klient je v archivu
            Me.hidIsBin.Value = "1"
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
        If lis.Count > 100 Then lis = lis.Take(101) 'omezit na maximálně 100+1
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
            Case "p31-save"
                'je třeba kvůli obnově pod-přehledu
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
        If Me.CurrentSubgrid = SubgridType._NotSpecified Then
            panProjects.Style.Item("max-height") = ""
            panProjects.Style.Item("overflow") = ""
        Else
            panProjects.Style.Item("max-height") = "250px"
            panProjects.Style.Item("overflow") = "auto"
        End If
        
    End Sub

    Private Sub rpP41_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP41.ItemDataBound
        _curProjectIndex += 1
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
            If _curProjectIndex > 100 Then
                'poslední nad 100
                .Text = "Další projekty klienta..."
                .NavigateUrl = "javascript:projects()"
                .ForeColor = Drawing.Color.Green
                .Font.Bold = True
                e.Item.FindControl("clue_project").Visible = False
            End If
        End With
        CType(e.Item.FindControl("clue_project"), HyperLink).Attributes.Item("rel") = "clue_p41_record.aspx?pid=" & cRec.PID.ToString


    End Sub

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

   
End Class