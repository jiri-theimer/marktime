Public Class p41_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub p41_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        menu1.Factory = Master.Factory
        menu1.DataPrefix = "p41"
        ff1.Factory = Master.Factory
        p31summary1.Factory = Master.Factory
    End Sub

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cal1.factory = Master.Factory
        'výchozí stránka z entity_framework přehledu (levý panel)
        If Not Page.IsPostBack Then
            With Master
                .SiteMenuValue = "p41"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Me.menu1.PageSource = "2" Then
                    .IsHideAllRecZooms = True
                    ''If Request.Item("tab") <> "" Then
                    ''    .Factory.j03UserBL.SetUserParam("p41_framework_detail-tab", Request.Item("tab"))
                    ''End If
                End If


                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p41_framework_detail-pid")
                    .Add("p41_framework_detail-tab")
                    .Add("p41_menu-remember-tab")
                    .Add("p41_menu-tabskin")
                    .Add("p41_menu-menuskin")
                    .Add("p41_framework_detail-chkFFShowFilledOnly")
                    .Add("p41_framework_detail_pos")
                    .Add("p41_menu-x31id-plugin")
                    .Add("p41_menu-show-level1")
                    .Add("p41_menu-show-cal1")
                    .Add("myscheduler-maxtoprecs-p41")
                    .Add("myscheduler-firstday")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                    'jedná se o výchozí stránku projektu, která se zároveň stará o automatické přesměrování na další projektové stránky
                    Dim intPID As Integer = Master.DataPID
                    If intPID = 0 Then
                        intPID = BO.BAS.IsNullInt(.GetUserParam("p41_framework_detail-pid", "O"))
                        If intPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")
                    Else
                        If intPID <> BO.BAS.IsNullInt(.GetUserParam("p41_framework_detail-pid", "O")) Then
                            .SetUserParam("p41_framework_detail-pid", intPID.ToString)
                        End If
                    End If

                    Dim strTab As String = Request.Item("tab")

                    If strTab = "" And .GetUserParam("p41_menu-remember-tab", "0") = "1" Then
                        strTab = .GetUserParam("p41_framework_detail-tab")  'záložka je ukotvená
                    End If
                    
                    Select Case strTab
                        Case "p31", "time", "expense", "fee", "kusovnik"
                            Server.Transfer("entity_framework_rec_p31.aspx?masterprefix=p41&masterpid=" & intPID.ToString & "&p31tabautoquery=" & strTab & "&source=" & menu1.PageSource, False)
                        Case "o23", "p91", "p56", "p41"
                            Server.Transfer("entity_framework_rec_" & strTab & ".aspx?masterprefix=p41&masterpid=" & intPID.ToString & "&source=" & menu1.PageSource, False)
                        Case "budget"
                            Server.Transfer("p41_framework_rec_budget.aspx?pid=" & intPID.ToString & "&source=" & menu1.PageSource, False)
                        Case Else
                            'zůstat na BOARD stránce
                    End Select

                    Master.DataPID = intPID
                    cal1.FirstDayMinus = BO.BAS.IsNullInt(.GetUserParam("myscheduler-firstday", "-1"))
                    cal1.MaxTopRecs = BO.BAS.IsNullInt(.GetUserParam("myscheduler-maxtoprecs-p41", "10"))
                    hidCal1ShallBeActive.Value = .GetUserParam("p41_menu-show-cal1", "1")
                    menu1.MenuSkin = .GetUserParam("p41_menu-menuskin")
                    menu1.TabSkin = .GetUserParam("p41_menu-tabskin")
                    menu1.x31ID_Plugin = .GetUserParam("p41_menu-x31id-plugin")
                    If .GetUserParam("p41_menu-remember-tab", "0") = "1" Then
                        menu1.LockedTab = .GetUserParam("p41_framework_detail-tab")
                    End If

                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail-chkFFShowFilledOnly", "0"))

                End With
            End With


        End If

        RefreshRecord()
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")

        Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)
        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)

        menu1.p41_RefreshRecord(cRec, cRecSum, "board", cDisp)
        Handle_Permissions(cRec, cP42, cDisp)

        Dim cClient As BO.p28Contact = Nothing

        With cRec
            Me.boxCoreTitle.Text = .p42Name & " (" & .p41Code & ")"
            If .b02ID <> 0 Then
                Me.boxCoreTitle.Text += ": " & .b02Name
            End If
            Me.Owner.Text = .Owner : Me.Timestamp.Text = .UserInsert & "/" & .DateInsert

            Me.Project.Text = .p41Name & " <span style='color:gray;padding-left:10px;'>" & .p41Code & "</span>"
            Select Case .p41TreeLevel
                Case 1 : Me.Project.ForeColor = basUIMT.TreeColorLevel1
                Case Is > 1 : Me.Project.ForeColor = basUIMT.TreeColorLevel2

            End Select

            If .p41NameShort <> "" Then
                Me.Project.Text += "<div style='color:green;'>" & .p41NameShort & "</div>"
            End If

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
            If .p28ID_Billing > 0 Then
                lblClientBilling.Visible = True : Me.ClientBilling.Visible = True
                Dim cClientBilling As BO.p28Contact = Master.Factory.p28ContactBL.Load(.p28ID_Billing)
                Me.ClientBilling.Text = cClientBilling.p28Name
                Me.ClientBilling.NavigateUrl = "p28_framework.aspx?pid=" & .p28ID_Billing.ToString
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
            If .p41IsDraft Then imgRecord.ImageUrl = "Images/draft.png"
            If cRec.p41ParentID <> 0 Then
                RenderTree(cRec, cRecSum)
            End If
            ''If .p41ParentID <> 0 Then
            ''    Me.trParent.Visible = True
            ''    Me.ParentProject.NavigateUrl = "p41_framework.aspx?pid=" & .p41ParentID.ToString
            ''    Me.ParentProject.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .p41ParentID)
            ''End If
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) And .p41BillingMemo <> "" Then
                boxBillingMemo.Visible = True
                Me.p41BillingMemo.Text = BO.BAS.CrLfText2Html(.p41BillingMemo)
                If Not cClient Is Nothing Then
                    If cClient.p28BillingMemo <> "" Then
                        Me.p41BillingMemo.Text += "<hr>" & String.Format("Fakturační poznámka klienta: {0}", BO.BAS.CrLfText2Html(cClient.p28BillingMemo))
                    End If
                End If
            Else
                boxBillingMemo.Visible = False
            End If

        End With



        If cP42.p42IsModule_p31 Then
            RefreshBillingLanguage(cRec, cClient)
            RefreshPricelist(cRec, cClient)
            ''RefreshOtherBillingSetting(cRec, cClient)
        Else
            trP51.Visible = False
        End If




        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)


        
        If cRecSum.p30_Exist Then
            Dim lisP30 As IEnumerable(Of BO.j02Person) = Master.Factory.p30Contact_PersonBL.GetList_J02(cRec.p28ID_Client, Master.DataPID, False)
            If lisP30.Count > 0 Then
                Me.boxP30.Visible = True
                Me.persons1.FillData(lisP30)
                With Me.boxP30Title
                    .Text = BO.BAS.OM2(.Text, lisP30.Count.ToString)
                   
                End With
            Else
                cRecSum.p30_Exist = False
            End If
        End If
        Me.boxP30.Visible = cRecSum.p30_Exist

        Dim mq As New BO.myQueryP31
        mq.p41ID = cRec.PID

        If cP42.p42IsModule_p31 Then
            Me.Last_Invoice.Text = cRecSum.Last_Invoice
            Me.Last_WIP_Worksheet.Text = cRecSum.Last_Wip_Worksheet
            If cRec.p41LimitFee_Notification > 0 Or cRec.p41LimitHours_Notification > 0 Or cRecSum.p31_Wip_Time_Count > 0 Or cRecSum.p31_Wip_Expense_Count > 0 Or cRecSum.p31_Wip_Fee_Count > 0 Or cRecSum.p31_Approved_Time_Count > 0 Or cRecSum.p31_Approved_Expense_Count > 0 Then
                Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
                If cWorksheetSum.RowsCount = 0 Then
                    boxP31Summary.Visible = False
                Else
                    p31summary1.RefreshData(cWorksheetSum, "p41", Master.DataPID, Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates), cRec.p41LimitHours_Notification, cRec.p41LimitFee_Notification)
                End If
            Else
                p31summary1.Visible = False
                If cRecSum.Last_Invoice = "" And cRecSum.Last_Wip_Worksheet = "" Then Me.boxP31Summary.Visible = False
            End If
        End If




        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p41Project, Master.DataPID, cRec.p42ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

        labels1.RefreshData(Master.Factory, BO.x29IdEnum.p41Project, cRec.PID)
        boxX18.Visible = labels1.ContainsAnyData

        If cRecSum.is_My_Favourite Then
            cmdFavourite.ImageUrl = "Images/favourite.png"
            cmdFavourite.ToolTip = "Vyřadit z mých oblíbených projektů"
        Else
            cmdFavourite.ImageUrl = "Images/not_favourite.png"
            cmdFavourite.ToolTip = "Zařadit do mých oblíbených projektů"
        End If

        RefreshP40(cRecSum)

        If cRecSum.b07_Count > 0 Or cRec.b01ID <> 0 Then
            comments1.Visible = True
            comments1.RefreshData(Master.Factory, BO.x29IdEnum.p41Project, cRec.PID)
        Else
            comments1.Visible = False
        End If

        If hidCal1ShallBeActive.Value = "1" Then
            cal1.RecordPID = Master.DataPID
            If cRecSum.p56_Actual_Count > 0 Or cRecSum.o22_Actual_Count > 0 Then
                cal1.RefreshData(Today)
                cal1.RefreshTasksWithoutDate(False)
            Else
                cal1.Visible = False
            End If
        Else
            cal1.Visible = False
        End If

        tags1.RefreshData(Master.Factory, "p41", cRec.PID)

        'RefreshP64(cRecSum)

    End Sub
    'Private Sub RefreshP64(cRecSum As BO.p41ProjectSum)
    '    If cRecSum.p64_Exist Then
    '        boxP64.Visible = True
    '        Dim mq As New BO.myQuery
    '        mq.Closed = BO.BooleanQueryMode.NoQuery
    '        Dim lis As IEnumerable(Of BO.p64Binder) = Master.Factory.p64BinderBL.GetList(Master.DataPID, mq)
    '        rpP64.DataSource = lis
    '        rpP64.DataBind()
    '        With Me.boxP64Title
    '            .Text = BO.BAS.OM2(.Text, lis.Count.ToString)
    '        End With
    '    Else
    '        boxP64.Visible = False
    '    End If
    'End Sub

    Private Sub RefreshP40(cRecSum As BO.p41ProjectSum)
        If cRecSum.p40_Exist Then
            Dim lisP40 As IEnumerable(Of BO.p40WorkSheet_Recurrence) = Master.Factory.p40WorkSheet_RecurrenceBL.GetList(Master.DataPID)
            rpP40.DataSource = lisP40
            rpP40.DataBind()
        Else
            cRecSum.p40_Exist = False
        End If
        boxP40.Visible = cRecSum.p40_Exist
    End Sub


    Private Sub Handle_Permissions(cRec As BO.p41Project, cP42 As BO.p42ProjectType, cDisp As BO.p41RecordDisposition)

        With Master.Factory


            If cP42.p42IsModule_p31 Then
                If Not menu1.IsExactApprovingPerson Then Me.p31summary1.DisableApprovingButton()

            Else

                Me.boxP31Summary.Visible = False
            End If

            aP48.Visible = cP42.p42IsModule_p48
        End With
        With cDisp
            boxP30.Visible = .OwnerAccess
            If cRec.p41TreeNext > cRec.p41TreePrev And .OwnerAccess Then
                linkBatchUpdateChilds.Visible = True
            End If

        End With


        panDraftCommands.Visible = False
        If cRec.b02ID = 0 And cRec.p41IsDraft And cDisp.OwnerAccess Then
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator) Then panDraftCommands.Visible = True 'pokud je vlastník a má právo zakládat ostré projekty a projekt nemá workflow šablonu
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
        If Me.p51Name_Billing.Text = "" And lblX51_Message.Text = "" Then trP51.Visible = False
    End Sub

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage()
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
        With CType(e.Item.FindControl("linkChrono"), HyperLink)
            .NavigateUrl = "javascript:p40_chrono(" & cRec.PID.ToString & ")"
        End With
    End Sub


    Private Sub cmdFavourite_Click(sender As Object, e As ImageClickEventArgs) Handles cmdFavourite.Click
        Master.Factory.j03UserBL.AppendOrRemoveFavouriteProject(Master.Factory.SysUser.PID, BO.BAS.ConvertPIDs2List(Master.DataPID), Master.Factory.p41ProjectBL.IsMyFavouriteProject(Master.DataPID))
        ReloadPage()
        ''ClientScript.RegisterStartupScript(Me.GetType, "hash", "window.open('p41_framework.aspx','_top');", True)
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("p41_framework_detail.aspx?pid=" & Master.DataPID.ToString & "&source=" & menu1.PageSource)
    End Sub

    Private Sub RenderTree(cRec As BO.p41Project, cRecSum As BO.p41ProjectSum)
        If Not tree1.IsEmpty Then Return
        tree1.Visible = True
        Dim c As BO.p41Project = Master.Factory.p41ProjectBL.LoadTreeTop(cRec.p41TreeIndex)
        If c Is Nothing Then Return
        Dim mq As New BO.myQueryP41
        mq.TreeIndexFrom = c.p41TreePrev
        mq.TreeIndexUntil = c.p41TreeNext
        Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq).Where(Function(p) (p.p41TreeNext > p.p41TreePrev And p.p41TreeLevel < cRec.p41TreeLevel) Or p.PID = cRec.PID).OrderBy(Function(p) p.p41TreeIndex)
        For Each c In lis
            Dim n As Telerik.Web.UI.RadTreeNode = tree1.AddItem(c.PrefferedName, c.PID.ToString, "p41_framework.aspx?pid=" & c.PID.ToString, c.p41ParentID.ToString, "Images/tree.png", , "_top")
            If menu1.PageSource = "navigator" Then
                n.Target = "" : n.NavigateUrl = "p41_framework_detail.aspx?pid=" & c.PID.ToString
            End If
            If c.p41TreeLevel = 1 Then n.ForeColor = basUIMT.TreeColorLevel1
            If c.p41TreeLevel > 1 Then n.ForeColor = basUIMT.TreeColorLevel2
            If c.IsClosed Then n.Font.Strikeout = True
        Next
        tree1.ExpandAll()
    End Sub

    Private Sub cmdConvertDraft2Normal_Click(sender As Object, e As EventArgs) Handles cmdConvertDraft2Normal.Click
        With Master.Factory.p41ProjectBL
            If .ConvertFromDraft(Master.DataPID) Then
                ReloadPage()
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub

    
    'Private Sub rpP64_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP64.ItemDataBound
    '    Dim cRec As BO.p64Binder = CType(e.Item.DataItem, BO.p64Binder)
    '    With CType(e.Item.FindControl("p64Name"), HyperLink)
    '        .Text = cRec.p64Ordinary.ToString & " - " & cRec.p64Name & " (" & cRec.p64ArabicCode & ")"
    '        .NavigateUrl = "javascript:p64_record(" & cRec.PID.ToString & ")"
    '        If cRec.IsClosed Then .Font.Strikeout = True
    '    End With
    'End Sub

   
End Class