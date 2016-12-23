Public Class p28_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub p28_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        menu1.Factory = Master.Factory
        menu1.DataPrefix = "p28"
        p31summary1.Factory = Master.Factory
        ff1.Factory = Master.Factory
    End Sub

   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .SiteMenuValue = "p28"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Request.Item("tab") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p28_framework_detail-tab", Request.Item("tab"))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p28_framework_detail-pid")
                    .Add("p28_framework_detail-tab")
                    .Add("p28_menu-tabskin")
                    .Add("p28_framework_detail-chkFFShowFilledOnly")
                    .Add("p28_framework_detail_pos")
                End With
                
                Dim intPID As Integer = Master.DataPID
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    'úvodní dispečerská stránka
                    If intPID = 0 Then
                        intPID = BO.BAS.IsNullInt(.GetUserParam("p28_framework_detail-pid", "O"))
                        If intPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p28")
                    Else
                        If intPID <> BO.BAS.IsNullInt(.GetUserParam("p28_framework_detail-pid", "O")) Then
                            .SetUserParam("p28_framework_detail-pid", intPID.ToString)
                        End If
                    End If
                    If Request.Item("board") = "" Then
                        Dim strTab As String = .GetUserParam("p28_framework_detail-tab", "board")
                        Select Case strTab
                            Case "p31", "time", "expense", "fee", "kusovnik"
                                Server.Transfer("entity_framework_rec_p31.aspx?masterprefix=p28&masterpid=" & intPID.ToString & "&p31tabautoquery=" & strTab, False)
                            Case "o23", "p91", "p56", "summary", "p41"
                                Server.Transfer("entity_framework_rec_" & strTab & ".aspx?masterprefix=p28&masterpid=" & intPID.ToString, False)
                            Case Else
                                'zůstat zde na BOARD stránce
                        End Select
                    End If
                    menu1.TabSkin = .GetUserParam("p28_menu-tabskin")
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p28_framework_detail-chkFFShowFilledOnly", "0"))
                End With
                Master.DataPID = intPID
            End With


            RefreshRecord()

        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p28")

        Dim cRecSum As BO.p28ContactSum = Master.Factory.p28ContactBL.LoadSumRow(cRec.PID)
        Dim cDisp As BO.p28RecordDisposition = Master.Factory.p28ContactBL.InhaleRecordDisposition(cRec)

        Handle_Permissions(cRec, cDisp)

        menu1.p28_RefreshRecord(cRec, cRecSum, "board", cDisp)

        With cRec
            Me.Contact.Text = .p28Name
            If .p28Code <> "" Then
                Me.Contact.Text += " <span style='color:gray;padding-left:10px;'>" & .p28Code & "</span>"
            End If
            If .p28TreeLevel = 1 Then Me.Contact.ForeColor = basUIMT.TreeColorLevel1
            If .p28TreeLevel > 1 Then Me.Contact.ForeColor = basUIMT.TreeColorLevel2


            If .p28CompanyShortName > "" Then
                Me.Contact.Text += "<div style='color:green;'>" & .p28CompanyName & "</div>"
            End If
            If .p28RegID <> "" Or .p28VatID <> "" Or .p28Person_BirthRegID <> "" Then
                Dim cM As New BO.SubjectMonitoring(cRec)
                If .p28RegID <> "" Then
                    Me.linkIC.Visible = True
                    Me.linkIC.Text = .p28RegID
                    Me.linkIC.NavigateUrl = cM.JusticeUrl : Me.linkIC.ToolTip = cM.JusticeName
                    Me.linkARES.NavigateUrl = cM.AresUrl
                    If Me.linkARES.NavigateUrl <> "" Then Me.linkARES.Visible = True
                End If
                If .p28VatID <> "" Then
                    Me.linkDIC.Visible = True
                    Me.linkDIC.Text = .p28VatID
                    Me.linkDIC.NavigateUrl = "javascript:vat_info('" & .p28VatID & "')"
                End If
                If cM.IsirUrl > "" Then
                    Me.linkISIR.Visible = True : Me.linkISIR.NavigateUrl = cM.IsirUrl
                End If


            Else
                trICDIC.Visible = False
            End If

            If .p29ID > 0 Then
                Me.p29Name.Text = "[" & .p29Name & "]"
            End If
            imgDraft.Visible = .p28IsDraft
            If .p28IsDraft Then imgRecord.ImageUrl = "Images/draft.png"
            ''If .p28ParentID <> 0 Then
            ''    Me.trParent.Visible = True
            ''    Me.ParentContact.NavigateUrl = "p28_framework.aspx?pid=" & .p28ParentID.ToString
            ''    Me.ParentContact.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, .p28ParentID)
            ''End If
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) And .p28BillingMemo <> "" Then
                boxBillingMemo.Visible = True
                Me.p28BillingMemo.Text = BO.BAS.CrLfText2Html(.p28BillingMemo)
            Else
                boxBillingMemo.Visible = False
            End If
        End With

        RefreshBillingLanguage(cRec)

        RefreshPricelist(cRec)

        ''RefreshProjectList(cRec)


        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p28Contact, cRec.PID)
        Me.roles1.RefreshData(lisX69, cRec.PID)
        If Me.roles1.RowsCount = 0 Then panRoles.Visible = False

        Dim lisO37 As IEnumerable(Of BO.o37Contact_Address) = Master.Factory.p28ContactBL.GetList_o37(cRec.PID)
        Dim lisO32 As IEnumerable(Of BO.o32Contact_Medium) = Master.Factory.p28ContactBL.GetList_o32(cRec.PID)
        If lisO37.Count > 0 Or lisO32.Count > 0 Then
            Me.boxO37.Visible = True
            If lisO37.Count > 0 Then Me.address1.FillData(lisO37)
            If lisO32.Count > 0 Then Me.medium1.FillData(lisO32)
        Else
            Me.boxO37.Visible = False
        End If


        If cRecSum.p30_Exist Then
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
                cRecSum.p30_Exist = False
            End If
        End If
        Me.boxP30.Visible = cRecSum.p30_Exist



        Me.Last_Invoice.Text = cRecSum.Last_Invoice
        Me.Last_WIP_Worksheet.Text = cRecSum.Last_Wip_Worksheet
        If cRecSum.p31_Approved_Time_Count > 0 Or cRecSum.p31_Wip_Time_Count > 0 Or cRecSum.p31_Wip_Expense_Count > 0 Or cRecSum.p31_Wip_Fee_Count > 0 Or cRecSum.p31_Approved_Expense_Count > 0 Then
            Dim mq As New BO.myQueryP31
            mq.p28ID_Client = cRec.PID
            Dim cWorksheetSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            If cWorksheetSum.RowsCount = 0 Then
                boxP31Summary.Visible = False
            Else
                p31summary1.RefreshData(cWorksheetSum, "p28", Master.DataPID, Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates), 0, 0)
            End If
        Else
            p31summary1.Visible = False
            If cRecSum.Last_Invoice = "" And cRecSum.Last_Wip_Worksheet = "" Then Me.boxP31Summary.Visible = False
        End If

        If cRec.b02ID > 0 Then
            Me.trWorkflow.Visible = True
            Me.b02Name.Text = cRec.b02Name
        Else
            Me.trWorkflow.Visible = False
        End If

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p28Contact).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_decide('x18_binding.aspx?prefix=p28&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p28Contact, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p28Contact, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p28Contact, Master.DataPID, cRec.p29ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        End If
        If cRec.p28ParentID <> 0 Then
            RenderTree(cRec, cRecSum)
        End If

        If cRecSum.o48_Exist Then linkISIR_Monitoring.Text = "ANO" : linkISIR.Font.Bold = True Else linkISIR_Monitoring.Text = "NE"


        If cRecSum.b07_Count > 0 Or cRec.b02ID > 0 Then
            comments1.Visible = True
            comments1.RefreshData(Master.Factory, BO.x29IdEnum.p28Contact, cRec.PID)
        Else
            comments1.Visible = False
        End If
    End Sub

    Private Sub RenderTree(cRec As BO.p28Contact, cRecSum As BO.p28ContactSum)
        tree1.Visible = True
        Dim c As BO.p28Contact = Master.Factory.p28ContactBL.LoadTreeTop(cRec.p28TreeIndex)
        If c Is Nothing Then Return
        Dim mq As New BO.myQueryP28
        mq.TreeIndexFrom = c.p28TreePrev
        mq.TreeIndexUntil = c.p28TreeNext
        Dim lis As IEnumerable(Of BO.p28Contact) = Master.Factory.p28ContactBL.GetList(mq).Where(Function(p) (p.p28TreeNext > p.p28TreePrev And p.p28TreeLevel < cRec.p28TreeLevel) Or p.PID = cRec.PID).OrderBy(Function(p) p.p28TreeIndex)
        For Each c In lis
            Dim n As Telerik.Web.UI.RadTreeNode = tree1.AddItem(c.p28Name, c.PID.ToString, "p28_framework.aspx?pid=" & c.PID.ToString, c.p28ParentID.ToString, "Images/tree.png", , "_top")
            If c.p28TreeLevel = 1 Then n.ForeColor = basUIMT.TreeColorLevel1
            If c.p28TreeLevel > 1 Then n.ForeColor = basUIMT.TreeColorLevel2
            If c.IsClosed Then n.Font.Strikeout = True
        Next
        tree1.ExpandAll()
    End Sub

    Private Sub Handle_Permissions(cRec As BO.p28Contact, cDisp As BO.p28RecordDisposition)

        If Not cDisp.ReadAccess Then
            Master.StopPage("Nedisponujete přístupovým oprávněním ke klientovi.")
        End If
        x18_binding.Visible = cDisp.OwnerAccess



        cmdEditP30.Visible = cDisp.OwnerAccess

        If cRec.b02ID = 0 And cRec.p28IsDraft And cDisp.OwnerAccess Then
            panDraftCommands.Visible = True 'pokud je vlastník a projekt nemá workflow šablonu
        Else
            panDraftCommands.Visible = False
        End If


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



   


    Private Sub ReloadPage()
        Response.Redirect("p28_framework_detail.aspx?pid=" & Master.DataPID.ToString)
    End Sub


    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage()
    End Sub

    Private Sub cmdConvertDraft2Normal_Click(sender As Object, e As EventArgs) Handles cmdConvertDraft2Normal.Click
        With Master.Factory.p28ContactBL
            If .ConvertFromDraft(Master.DataPID) Then
                ReloadPage()
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub
End Class