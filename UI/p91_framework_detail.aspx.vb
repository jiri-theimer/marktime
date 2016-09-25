Imports Telerik.Web.UI

Public Class p91_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub p91_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneInvoicePage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneInvoicePage, "pid=" & .DataPID.ToString))
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p91_framework_detail-pid")
                    .Add("p91_framework_detail-group")
                    .Add("p91_framework_detail-j74id")
                    .Add("p91_framework_detail-pagesize")
                    .Add("p91_framework_detail-chkFFShowFilledOnly")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                    basUI.SelectRadiolistValue(Me.opgGroupBy, .GetUserParam("p91_framework_detail-group", "flat"))
                    basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("p91_framework_detail-pagesize", "20"))
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("p91_framework_detail-chkFFShowFilledOnly", "0"))
                End With

                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p91_framework_detail-pid", "O"))
                    If .DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p91")
                Else
                    If .DataPID <> BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p91_framework_detail-pid", "O")) Then
                        .Factory.j03UserBL.SetUserParam("p91_framework_detail-pid", .DataPID.ToString)
                    End If
                End If


            End With

            RefreshRecord()
            SetupJ74Combo(CInt(Master.Factory.j03UserBL.GetUserParam("p91_framework_detail-j74id", "0")))
            SetupGrid()
            RecalcVirtualRowCount()

            With Master
                Select Case .Factory.j03UserBL.GetMyTag(True)
                    Case "draftisout"
                        .Notify("Číslo faktury je nyní [" & Me.p91Code.Text & "].", NotifyLevel.InfoMessage)
                End Select
            End With
        End If


    End Sub

    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property
    Private Sub SetupJ74Combo(intDef As Integer)
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet).Where(Function(p) p.j74MasterPrefix = "" Or p.j74MasterPrefix = "p91")
        j74id.DataSource = lisJ74
        j74id.DataBind()
        If intDef > 0 Then
            Me.CurrentJ74ID = intDef
        End If

    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p91Invoice = Master.Factory.p91InvoiceBL.Load(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p91")
        Handle_Permissions(cRec)
        With cRec
            cmdNewWindow.NavigateUrl = "p91_framework.aspx?blankwindow=1&pid=" & .PID.ToString & "&title=" & .p91Code
            basUIMT.RenderHeaderMenu(.IsClosed, Me.panMenuContainer, menu1)
            Me.p91Code.Text = .p91Code
            basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), .p92Name & ": " & .p91Code, "p91_framework_detail.aspx?pid=" & .PID.ToString, .IsClosed)
            If .IsClosed Then Me.hidIsBin.Value = "1"



            Me.p92Name.Text = .p92Name
            Me.clue_p92name.Attributes("rel") = "clue_p92_record.aspx?pid=" & cRec.p92ID.ToString
            With Me.Client
                .Text = cRec.p28Name
                .NavigateUrl = "p28_framework.aspx?pid=" & cRec.p28ID.ToString
            End With
            Me.clue_client.Attributes("rel") = "clue_p28_record.aspx?pid=" & .p28ID.ToString

            If .b01ID <> 0 Then
                Me.trWorkflow.Visible = True
                Me.b02Name.Text = .b02Name
            Else
                Me.trWorkflow.Visible = False
            End If


            Me.p91Amount_Debt.Text = BO.BAS.FN(.p91Amount_Debt)
            Me.p91Amount_WithoutVat.Text = BO.BAS.FN(.p91Amount_WithoutVat)
            Me.p91Amount_WithVat.Text = BO.BAS.FN(.p91Amount_WithVat)
            Me.p91Amount_Vat.Text = BO.BAS.FN(.p91Amount_Vat)

            Me.j27Code_debt.Text = .j27Code
            Me.j27Code_withoutvat.Text = .j27Code
            Me.j27Code_vat.Text = .j27Code
            Me.j27Code_withvat.Text = .j27Code

            If .p91ProformaAmount <> 0 Then
                Me.p91ProformaAmount.Text = BO.BAS.FN(.p91ProformaAmount)
                Me.j27Code_proforma.Text = .j27Code
            Else
                lblProforma.Visible = False
            End If


            Me.p91Date.Text = BO.BAS.FD(.p91Date, False, True)
            Me.p91DateMaturity.Text = BO.BAS.FD(.p91DateMaturity, False, True)
            If .p91Amount_Debt > 0 Then
                Me.p91DateMaturity.ForeColor = Drawing.Color.Red
            End If
            Me.p91DateSupply.Text = BO.BAS.FD(.p91DateSupply, False, True)
            Me.p91DateBilled.Text = BO.BAS.FD(.p91DateBilled, False, True)

            If .p91Text1 <> "" Then
                Me.p91Text1.Text = BO.BAS.CrLfText2Html(.p91Text1)
            Else
                panText1.Visible = False
            End If


            Me.p91Text2.Text = BO.BAS.CrLfText2Html(.p91Text2)
            Me.Owner.Text = .Owner
            Me.WorksheetRange.Text = BO.BAS.FD(.p91Datep31_From) & " - " & BO.BAS.FD(.p91Datep31_Until)
            Me.BillingAddress.Text = ParseAddress(.o38ID_Primary)
            Me.PostAddress.Text = ParseAddress(.o38ID_Delivery)
            HandleBankAccount(.p93ID, .j27ID)

            Me.p91RoundFitAmount.Text = BO.BAS.FN(.p91RoundFitAmount)
            Me.p91Amount_WithoutVat_None.Text = BO.BAS.FN(.p91Amount_WithoutVat_None)
            Me.p91VatRate_Low.Text = .p91VatRate_Low.ToString & "%"
            Me.p91Amount_Vat_Low.Text = BO.BAS.FN(.p91Amount_Vat_Low)
            Me.p91Amount_WithVat_Low.Text = BO.BAS.FN(.p91Amount_WithVat_Low)
            Me.p91VatRate_Standard.Text = .p91VatRate_Standard.ToString & "%"
            Me.p91Amount_WithoutVat_Standard.Text = BO.BAS.FN(.p91Amount_WithoutVat_Standard)
            Me.p91Amount_Vat_Standard.Text = BO.BAS.FN(.p91Amount_Vat_Standard)
            If .p91Amount_WithVat_Standard <> 0 Then Me.p91Amount_WithVat_Standard.Text = BO.BAS.FN(.p91Amount_WithVat_Standard)

            HandleDirectReports(.p92ID)

            Me.lblExchangeRate.Visible = False
            If .p91ExchangeRate <> 1 Then
                lblExchangeRate.Visible = True
                Me.p91ExchangeRate.Text = .p91ExchangeRate.ToString & " (" & BO.BAS.FD(.p91DateExchange) & ")"
            End If
        End With



        Me.comments1.RefreshData(Master.Factory, BO.x29IdEnum.p91Invoice, cRec.PID)
        Dim mqO23 As New BO.myQueryO23
        mqO23.p91ID = Master.DataPID
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
        If lisO23.Count > 0 Then
            notepad1.RefreshData(lisO23, cRec.PID)
        End If

        imgDocType.Visible = False : cmdConvertDraft.Visible = False
        If cRec.p91IsDraft Then
            imgDocType.ImageUrl = "Images/draft_icon.gif" : imgDocType.Visible = True
            If cRec.j02ID_Owner = Master.Factory.SysUser.j02ID Or Master.Factory.TestPermission(BO.x53PermValEnum.GR_P91_Owner) Then
                cmdConvertDraft.Visible = True
            End If
        End If
        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p91Invoice).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_local('x18_binding.aspx?prefix=p91&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.p91Invoice, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.p91Invoice, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p91Invoice, Master.DataPID, cRec.p92ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            Me.chkFFShowFilledOnly.Visible = False : Me.ff1.Visible = False
        End If

    End Sub

    Private Sub HandleDirectReports(intP92ID As Integer)
        Dim cRec As BO.p92InvoiceType = Master.Factory.p92InvoiceTypeBL.Load(intP92ID)
        With cRec
            If .x31ID_Invoice > 0 Then
                Me.cmdReportInvoice.NavigateUrl = "javascript: report(" & .x31ID_Invoice.ToString & ")"
            Else
                Me.cmdReportInvoice.Visible = False
            End If
            If .x31ID_Attachment > 0 Then
                Me.cmdReportAttachment.NavigateUrl = "javascript: report(" & .x31ID_Attachment.ToString & ")"
            Else
                Me.cmdReportAttachment.Visible = False
            End If
        End With
    End Sub
    Private Function ParseAddress(intO38ID As Integer) As String
        If intO38ID = 0 Then Return ""
        Dim cRec As BO.o38Address = Master.Factory.o38AddressBL.Load(intO38ID)
        Return cRec.FullAddress
    End Function
    Private Sub HandleBankAccount(intP93ID As Integer, intJ27ID As Integer)
        Dim lis As IEnumerable(Of BO.p88InvoiceHeader_BankAccount) = Master.Factory.p93InvoiceHeaderBL.GetList_p88(intP93ID)
        If lis.Where(Function(p) p.j27ID = intJ27ID).Count > 0 Then
            Dim cRec As BO.p86BankAccount = Master.Factory.p86BankAccountBL.Load(lis.Where(Function(p) p.j27ID = intJ27ID)(0).p86ID)
            Me.BankAccount.Text = cRec.p86BankAccount & "/" & cRec.p86BankCode
            Me.BankName.Text = cRec.p86BankName
        End If

    End Sub


    Private Sub Handle_Permissions(cRec As BO.p91Invoice)
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=p91&masterpid=" & cRec.PID.ToString
        Dim cDisp As BO.p91RecordDisposition = Master.Factory.p91InvoiceBL.InhaleRecordDisposition(cRec)
        x18_binding.Visible = cDisp.OwnerAccess

        With Master.Factory
            menu1.FindItemByValue("cmdCreateInvoice").Visible = .TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator)
            menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator)
            menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator)
            menu1.FindItemByValue("cmdPivot").Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
            menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p91&masterpid=" & cRec.PID.ToString
        End With
        With cDisp
            menu1.FindItemByValue("cmdPay").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdEdit").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdCreateInvoice").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdPay").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdAppendWorksheet").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdChangeCurrency").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdChangeVat").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdProforma").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdCreditNote").Visible = .OwnerAccess
        End With


        If cRec.p92InvoiceType = BO.p92InvoiceTypeENUM.CreditNote Then
            menu1.FindItemByValue("cmdPay").Visible = False
            menu1.FindItemByValue("cmdProforma").Visible = False
            menu1.FindItemByValue("cmdCreditNote").Visible = False
            lblp91DateBilled.Visible = False
            p91DateMaturity.Visible = False : lblp91DateMaturity.Visible = False
            imgRecord.Visible = True : imgRecord.ImageUrl = "Images\correction_down.gif"
            lblExchangeRate.Visible = False : p91ExchangeRate.Visible = False
        End If
    End Sub
    


    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = Nothing
            If Me.CurrentJ74ID > 0 Then cJ74 = .Load(Me.CurrentJ74ID)
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p91")
                If Not cJ74 Is Nothing Then

                    SetupJ74Combo(cJ74.PID)
                    Me.CurrentJ74ID = cJ74.PID
                End If
            End If
            Dim strF As String = ""
            Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, CInt(Me.cbxPaging.SelectedValue), True, True, , , , , strF)
            Me.hidFrom.Value = strF
        End With


        Dim strGroupField As String = "", strHeaderText As String = ""
        Select Case Me.opgGroupBy.SelectedValue
            Case "p41"
                strGroupField = "p41Name" : strHeaderText = "Projekt"
            Case "p34"
                strGroupField = "p34Name" : strHeaderText = "Sešit"
            Case "p95"
                strGroupField = "p95Name" : strHeaderText = "Fakturační oddíl"
            Case "p32"
                strGroupField = "p32Name" : strHeaderText = "Aktivita"
            Case "j02"
                strGroupField = "Person" : strHeaderText = "Osoba"
            Case "p56"
                strGroupField = "p56Code" : strHeaderText = "Úkol"
            Case "p70"
                strGroupField = "p70Name" : strHeaderText = "Fakturační status úkonu"
            Case "p31ApprovingSet"
                strGroupField = "p31ApprovingSet" : strHeaderText = "Billing dávka"
            Case Else
                Return
        End Select

        With grid1.radGridOrig.MasterTableView
            .ShowGroupFooter = True
            Dim GGE As New Telerik.Web.UI.GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strHeaderText

            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)

        End With

    End Sub

    Public Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim cSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, False, False)
        If Not cSum Is Nothing Then
            grid1.VirtualRowCount = cSum.RowsCount
            With tabs1.FindTabByValue("p31")
                .Text = BO.BAS.OM2(.Text, cSum.RowsCount.ToString)
                ''.Text = .Text & " (" & cSum.RowsCount.ToString & ")"
            End With
            ViewState("footersum") = grid1.GenerateFooterItemString(cSum)
        Else
            ViewState("footersum") = ""
            grid1.VirtualRowCount = 0
        End If


        grid1.radGridOrig.CurrentPageIndex = 0

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)

        mq.p91ID = Master.DataPID

    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e, True)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim strGroupField As String = ""
        Select Case Me.opgGroupBy.SelectedValue
            Case "p41"
                strGroupField = "p41name"
            Case "p34"
                strGroupField = "p34Name"
            Case "p95"
                strGroupField = "p95Name"
            Case "p32"
                strGroupField = "p32Name"
            Case "j02"
                strGroupField = "Person"
            Case "j27"
                strGroupField = "j27Code_Billing_Orig"
            Case "p56"
                strGroupField = "p56Code"
            Case "p70"
                strGroupField = "p70Name"
            Case "p31ApprovingSet"
                strGroupField = "p31ApprovingSet"
            Case Else
        End Select
        ''If Me.opgGroupBy.SelectedValue <> "" Then
        ''    Me.hidCols.Value += "," & strSort
        ''End If


        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)
        With mq
            .MG_PageSize = CInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            .MG_GridGroupByField = strGroupField
            .MG_GridSqlColumns = hidCols.Value & ",a.p41ID as p41IDX,p41.p41Name as p41NameX"
            .MG_AdditionalSqlFROM = hidFrom.Value
        End With

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetGridDataSource(mq)
        If dt Is Nothing Then
            Master.Notify(Master.Factory.p31WorksheetBL.ErrorMessage, NotifyLevel.ErrorMessage)
            Return
        Else
            grid1.DataSourceDataTable = dt
        End If

        ''Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        ''grid1.DataSource = lis
        Dim p41ids = dt.AsEnumerable.Select(Function(p) p.Item("p41IDX").ToString & "|" & p.Item("p41NameX")).Distinct

        'Dim p41ids As List(Of Object) = dt.AsEnumerable.Select(Function(p) p.Item("p41ID").ToString & "|" & p.Item("p41Name")).Distinct

        rpProject.DataSource = p41ids
        rpProject.DataBind()
        If rpProject.Items.Count > 1 Then
            With lblProject
                .Text = BO.BAS.OM2(.Text, rpProject.Items.Count.ToString)
            End With
            If rpProject.Items.Count >= 3 Then panProjects.Style.Item("height") = "60px" : panProjects.Style.Item("overflow") = "auto"
        End If

    End Sub
    Private Sub grid1_NeedFooterSource(footerItem As Telerik.Web.UI.GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"
        grid1.ParseFooterItemString(footerItem, ViewState("footersum"))
    End Sub

    Private Sub opgGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p91_framework_detail-group", Me.opgGroupBy.SelectedValue)

        SetupGrid()
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p91_framework_detail-pagesize", Me.cbxPaging.SelectedValue)
        SetupGrid()
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" Then Return
        Select Case Me.hidHardRefreshFlag.Value
            Case "p31-save", "p31-remove", "p31-add"
                RefreshRecord()
                grid1.Rebind(True)
                Me.tabs1.FindTabByValue("p31").Selected = True
            Case Else
                ReloadPage()
        End Select


        Me.hidHardRefreshFlag.Value = ""
    End Sub

    Private Sub cmdConvertDraft_Click(sender As Object, e As EventArgs) Handles cmdConvertDraft.Click
        With Master.Factory.p91InvoiceBL
            If .ConvertFromDraft(Master.DataPID) Then
                Master.Factory.j03UserBL.SetMyTag("draftisout")
                ReloadPage()
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
        
    End Sub
    Private Sub ReloadPage()
        Response.Redirect(menu1.FindItemByValue("level1").NavigateUrl)
    End Sub

    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p91_framework_detail-j74id", Me.j74id.SelectedValue)
        SetupGrid()
        RecalcVirtualRowCount()

        grid1.Rebind(True)
    End Sub

    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p91_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage()
    End Sub

    Private Sub rpProject_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpProject.ItemDataBound
        Dim s As String = CType(e.Item.DataItem, String)
        Dim a() As String = Split(s, "|")
        With CType(e.Item.FindControl("clue_project"), HyperLink)
            .Attributes("rel") = "clue_p41_record.aspx?pid=" & a(0)
        End With
        With CType(e.Item.FindControl("p41Name"), HyperLink)
            .Text = a(1)
            .NavigateUrl = "p41_framework.aspx?pid=" & a(0)
        End With
        
    End Sub
End Class