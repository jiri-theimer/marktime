
Public Class p41_framework_detail_rec
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            p31summary1.Factory = .Factory
            ff1.Factory = .Factory
        End With
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                If .DataPID = 0 Then .StopPage("masterpid is missing")

                If Request.Item("lasttabkey") <> "" Then
                    Master.Factory.j03UserBL.SetUserParam(Request.Item("lasttabkey"), Request.Item("lasttabval"))
                End If
            End With
            Dim lisPars As New List(Of String)
            With lisPars
                ''.Add("p41_framework_detail-pid")
                .Add("p41_framework_detail-chkFFShowFilledOnly")
            End With
            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)

                Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail-chkFFShowFilledOnly", "0"))

            End With
            RefreshRecord()


        End If
    End Sub



    Private Sub RefreshRecord()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")

        Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

        Handle_Permissions(cRec, cP42)

        Dim cClient As BO.p28Contact = Nothing

        With cRec
            ViewState("p28id_client") = .p28ID_Client.ToString

            Me.Project.Text = .p41Name & " <span style='color:gray;padding-left:10px;'>" & .p41Code & "</span>"
            If .p41ParentID > 0 Then Me.Project.ForeColor = basUIMT.ChildProjectColor
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
            If .p41ParentID <> 0 Then
                Me.trParent.Visible = True
                Me.ParentProject.NavigateUrl = "p41_framework.aspx?pid=" & .p41ParentID.ToString
                Me.ParentProject.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .p41ParentID)
            End If
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


        If cRecSum.o23_Count > 0 And cP42.p42SubgridO23Flag = 0 Then
            Dim mqO23 As New BO.myQueryO23
            mqO23.p41ID = Master.DataPID
            mqO23.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
            If lisO23.Count > 0 Then
                Me.boxO23.Visible = True
                With Me.boxO23Title
                    .Text = BO.BAS.OM2(.Text, lisO23.Count.ToString)
                    ''If menu1.FindItemByValue("cmdO23").Visible Then
                    ''    .Text = "<a href='javascript:notepads()'>" & .Text & "</a>"
                    ''    If lisO23.Count > 10 Then
                    ''        .Text += ", 10 nejnovějších:"
                    ''        lisO23 = lisO23.Take(10)
                    ''    End If
                    ''End If
                End With
                notepad1.RefreshData(lisO23, Master.DataPID)
            Else
                boxO23.Visible = False
            End If
        Else
            Me.boxO23.Visible = False
        End If

        If cRecSum.p30_Exist Then
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
                cRecSum.p30_Exist = False
            End If
        End If
        Me.boxP30.Visible = cRecSum.p30_Exist

        Dim mq As New BO.myQueryP31
        mq.p41ID = cRec.PID

        If cRec.p41LimitFee_Notification > 0 Or cRec.p41LimitHours_Notification > 0 Then
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            If cWorksheetSum.RowsCount = 0 Then
                boxP31Summary.Visible = False
            Else
                p31summary1.RefreshData(cWorksheetSum, "p41", Master.DataPID, Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates), cRec.p41LimitHours_Notification, cRec.p41LimitFee_Notification)
            End If
        Else
            boxP31Summary.Visible = False
        End If



        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p41Project, Master.DataPID, cRec.p42ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p41Project).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:window.parent.sw_decide('x18_binding.aspx?prefix=p41&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p41Project, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p41Project, cRec.PID))
        Else
            boxX18.Visible = False
        End If
        If cRecSum.childs_Count > 0 Then
            cmdChilds.Visible = True
            cmdChilds.Text += "<span class='badge1'>" & cRecSum.childs_Count.ToString & "</span>"
        End If
        If cRecSum.is_My_Favourite Then
            cmdFavourite.ImageUrl = "Images/favourite.png"
            cmdFavourite.ToolTip = "Vyřadit z mých oblíbených projektů"
        Else
            cmdFavourite.ImageUrl = "Images/not_favourite.png"
            cmdFavourite.ToolTip = "Zařadit do mých oblíbených projektů"
        End If

        RefreshP40(cRecSum)

    End Sub

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


    Private Sub Handle_Permissions(cRec As BO.p41Project, cP42 As BO.p42ProjectType)
        
        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)

        x18_binding.Visible = cDisp.OwnerAccess
        With Master.Factory
            
           
            If cP42.p42IsModule_p31 Then
                Dim bolCanApproveOrInvoice As Boolean = .TestPermission(BO.x53PermValEnum.GR_P31_Approver, BO.x53PermValEnum.GR_P91_Creator)
                If Not bolCanApproveOrInvoice Then bolCanApproveOrInvoice = .TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator)
                If bolCanApproveOrInvoice = False And cDisp.x67IDs.Count > 0 Then
                    Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Master.Factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                    If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                        bolCanApproveOrInvoice = True
                    End If
                End If

                If Not bolCanApproveOrInvoice Then Me.p31summary1.DisableApprovingButton()

            Else

                p31summary1.Visible = False
            End If

            aP48.Visible = cP42.p42IsModule_p48
        End With
        With cDisp
          
            boxP30.Visible = .OwnerAccess
         

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

    End Sub

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        RefreshRecord()
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


    Private Sub cmdFavourite_Click(sender As Object, e As ImageClickEventArgs) Handles cmdFavourite.Click
        Master.Factory.j03UserBL.AppendOrRemoveFavouriteProject(Master.Factory.SysUser.PID, BO.BAS.ConvertPIDs2List(Master.DataPID), Master.Factory.p41ProjectBL.IsMyFavouriteProject(Master.DataPID))
        ClientScript.RegisterStartupScript(Me.GetType, "hash", "window.open('p41_framework.aspx','_top');", True)
    End Sub
End Class