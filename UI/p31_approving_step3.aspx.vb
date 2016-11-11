Imports Telerik.Web.UI

Public Class p31_approving_step3
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p31_approving_step3_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_approving_dialog"
    End Sub
    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property
    Public ReadOnly Property CurrentMasterPrefix As String
        Get
            Return Me.hidMasterPrefix.Value
        End Get
    End Property
    Public ReadOnly Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                Me.hidMasterPrefix.Value = Request.Item("masterprefix")
                Me.hidMasterPID.Value = Request.Item("masterpid")
                ViewState("guid") = Request.Item("guid")
                ViewState("guid_err") = BO.BAS.GetGUID
                If ViewState("guid") = "" Then .StopPage("guid is missing")
                ViewState("approvingset") = Server.UrlDecode(Request.Item("approvingset"))

                Dim s As String = "Schvalování worksheet úkonů"
                If Request.Item("clearapprove") = "1" Then
                    s = "Vyčištění schvalovacího příznaku"
                End If
                If Me.CurrentMasterPrefix <> "" Then
                    .HeaderText = s & " | " & .Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix), Me.CurrentMasterPID)
                Else
                    .HeaderText = s
                End If

                .HeaderIcon = "Images/approve_32.png"
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_approving_step3-j74id")
                    .Add("p31_approving-use_internal_approving")
                    .Add("p31_approving-group")
                    .Add("p31_approving-autofilter")
                    .Add("p31_approving-static_headers")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)
                If .Factory.j03UserBL.GetUserParam("p31_approving-group") <> "" Then
                    basUI.SelectRadiolistValue(Me.opgGroupBy, .Factory.j03UserBL.GetUserParam("p31_approving-group"))
                End If
                Me.chkUseInternalApproving.Checked = BO.BAS.BG(.Factory.j03UserBL.GetUserParam("p31_approving-use_internal_approving", "0"))
                Me.chkAutoFilter.Checked = BO.BAS.BG(.Factory.j03UserBL.GetUserParam("p31_approving-autofilter", "0"))
                grid1.radGridOrig.ClientSettings.Scrolling.UseStaticHeaders = BO.BAS.BG(.Factory.j03UserBL.GetUserParam("p31_approving-static_headers", "0"))

                .AddToolbarButton("Uložit změny", "save", , "Images/save.png")

                .AddToolbarButton("Tisková sestava", "report", 1, "Images/report.png", False, "javascript:report()")

                .AddToolbarButton("Zapsat nový úkon", "p31_create", 1, "Images/worksheet.png", False, "javascript:p31_create('" & Me.CurrentMasterPrefix & "id'," & Me.CurrentMasterPID.ToString & ")")

                .AddToolbarButton("Nastavení", "setting", 1, "Images/arrow_down.gif", False)
                .AddToolbarButton("Fakturační poznámky", "o23", 1, "Images/arrow_down.gif", False)

                .AddToolbarButton("Hromadné operace", "batch", 1, "Images/arrow_down.gif", False)


                .RadToolbar.FindItemByValue("batch").CssClass = "show_hide1"
                .RadToolbar.FindItemByValue("setting").CssClass = "show_hide2"
                .RadToolbar.FindItemByValue("o23").CssClass = "show_hide3"



            End With
            SetupJ74Combo()

            If Request.Item("reloadonly") = "" Then
                SetupTempData()
            End If


            SetupGrid()

            RefreshRecord()

        End If
    End Sub

    Private Sub RefreshRecord()
        Dim mqO23 As New BO.myQueryO23
        notepad1.EntityX29ID = BO.BAS.GetX29FromPrefix(ViewState("masterprefix"))
        Dim s As String = ""
        Select Case Me.CurrentMasterPrefix
            Case "p28"
                s = "Fakturační poznámky klienta"
                mqO23.p28ID = BO.BAS.IsNullInt(Me.CurrentMasterPID)
            Case "p41"
                s = "Fakturační poznámky projektu"
                mqO23.p41ID = BO.BAS.IsNullInt(Me.CurrentMasterPID)
            Case "j02"
                s = "Fakturační poznámky osoby"
                mqO23.j02ID = BO.BAS.IsNullInt(Me.CurrentMasterPID)
            Case Else
                Master.HideShowToolbarButton("o23", False)
        End Select
        If s <> "" Then
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23).Where(Function(p) p.o24IsBillingMemo = True)
            notepad1.RefreshData(lisO23, Master.DataPID)

            s += " (" & notepad1.RowsCount.ToString & ")"
            Master.RenameToolbarButton("o23", s)
        End If

    End Sub

    Private Sub BatchOper_P72(p71id As BO.p71IdENUM, explicit_p72id As BO.p72IdENUM)
        Dim lisPIDs As List(Of Integer) = grid1.GetSelectedPIDs()
        If lisPIDs.Count = 0 Then
            Master.Notify("Musíte vybrat (označit) alespoň jeden záznam (úkon).", NotifyLevel.WarningMessage)
            Return
        End If
        If p71id = BO.p71IdENUM.Nic Then explicit_p72id = BO.p72IdENUM._NotSpecified
        Dim sx As New System.Text.StringBuilder, xx As Integer = 0

        For Each intP31ID As Integer In lisPIDs
            xx += 1
            Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(intP31ID)
            Dim cApprove As New BO.p31WorksheetApproveInput(cRec.PID, cRec.p33ID)
            With cApprove
                .GUID_TempData = ViewState("guid")
                ''.p71id = BO.p71IdENUM.Schvaleno
                .p71id = p71id
                .p72id = explicit_p72id
                .p31ApprovingSet = cRec.p31ApprovingSet
                If explicit_p72id = BO.p72IdENUM.Fakturovat Then
                    Select Case cRec.p33ID
                        Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                            .Rate_Billing_Approved = cRec.p31Rate_Billing_Orig

                    End Select
                    .Value_Approved_Billing = cRec.p31Value_Orig

                End If
            End With

            Dim cErr As New BO.p85TempBox
            cErr.p85GUID = ViewState("guid_err")
            cErr.p85DataPID = cRec.PID

            If Not Master.Factory.p31WorksheetBL.Save_Approving(cApprove, True) Then
                cErr.p85FreeText01 = Master.Factory.p31WorksheetBL.ErrorMessage
                sx.Append("<br>#" & xx.ToString & ": " & Master.Factory.p31WorksheetBL.ErrorMessage)
            Else
                cErr.p85FreeText01 = ""
            End If

            Master.Factory.p85TempBoxBL.Save(cErr)
        Next

        grid1.Rebind(True, lisPIDs(0))

        If sx.ToString <> "" Then
            Master.Notify(sx.ToString, NotifyLevel.ErrorMessage)
        End If
    End Sub
    Private Sub SetupTempData()
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        For Each cTemp In lis
            Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(cTemp.p85DataPID)
            Dim bolOK As Boolean = True
            If cRec.p91ID > 0 Or cRec.p31IsPlanRecord Then bolOK = False
            If bolOK Then
                Dim cApprove As New BO.p31WorksheetApproveInput(cRec.PID, cRec.p33ID)
                With cApprove
                    .GUID_TempData = ViewState("guid")
                    .p31ApprovingSet = cRec.p31ApprovingSet
                    If ViewState("approvingset") <> "" And .p31ApprovingSet = "" Then
                        .p31ApprovingSet = ViewState("approvingset")    'dosud nezařazený záznam zařadit do dávky
                    End If

                    If cRec.p71ID = BO.p71IdENUM.Nic Then
                        'dosud neprošlo schvalováním
                        .p71id = BO.p71IdENUM.Schvaleno
                        If cRec.p32IsBillable Then
                            .p72id = BO.p72IdENUM.Fakturovat
                            .Value_Approved_Billing = cRec.p31Value_Orig
                            .Value_Approved_Internal = cRec.p31Value_Orig

                            Select Case .p33ID
                                Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                                    .Rate_Billing_Approved = cRec.p31Rate_Billing_Orig
                                    .VatRate_Approved = cRec.p31VatRate_Orig
                                    If cRec.p31Rate_Billing_Orig = 0 Then
                                        .p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                                    End If
                                    If cRec.p72ID_AfterTrimming > BO.p72IdENUM._NotSpecified Then
                                        'uživatel zadal v úkonu výchozí korekci pro schvalování
                                        .p72id = cRec.p72ID_AfterTrimming
                                        If .p72id = BO.p72IdENUM.Fakturovat Then
                                            .Value_Approved_Billing = cRec.p31Value_Trimmed
                                        Else
                                            .Rate_Billing_Approved = 0
                                            .Value_Approved_Billing = 0
                                        End If
                                    End If
                                Case BO.p33IdENUM.PenizeBezDPH
                                    If cRec.p31Value_Orig = 0 Then
                                        .p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                                    End If
                                Case BO.p33IdENUM.PenizeVcDPHRozpisu
                                    .VatRate_Approved = cRec.p31VatRate_Orig
                                    If cRec.p31Value_Orig = 0 Then
                                        .p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                                    End If
                            End Select
                        Else
                            If cRec.p72ID_AfterTrimming = BO.p72IdENUM._NotSpecified Or cRec.p72ID_AfterTrimming = BO.p72IdENUM.Fakturovat Then
                                .p72id = BO.p72IdENUM.SkrytyOdpis
                            Else
                                .p72id = cRec.p72ID_AfterTrimming
                            End If

                        End If
                    Else
                        'již dříve schválený záznam
                        .p71id = cRec.p71ID
                        .p72id = cRec.p72ID_AfterApprove
                        .Value_Approved_Billing = cRec.p31Value_Approved_Billing
                        .Value_Approved_Internal = cRec.p31Value_Approved_Internal
                        .VatRate_Approved = cRec.p31VatRate_Approved
                        .Rate_Billing_Approved = cRec.p31Rate_Billing_Approved
                        .Rate_Internal_Approved = cRec.p31Rate_Internal_Approved
                    End If

                End With
                If Not Master.Factory.p31WorksheetBL.Save_Approving(cApprove, True) Then
                    Dim cErr As New BO.p85TempBox
                    cErr.p85GUID = ViewState("guid_err")
                    cErr.p85DataPID = cRec.PID
                    cErr.p85FreeText01 = Master.Factory.p31WorksheetBL.ErrorMessage
                    Master.Factory.p85TempBoxBL.Save(cErr)

                End If
            End If


        Next


    End Sub

    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = Nothing
            If Me.CurrentJ74ID > 0 Then cJ74 = .Load(Me.CurrentJ74ID)
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "approving_step3")
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo()
                    Me.CurrentJ74ID = cJ74.PID
                End If
            End If
            Dim strF As String = ""
            Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 5000, False, True, , , , , strF)
            Me.hidFrom.Value = strF
        End With

        If Not Page.IsPostBack Then
            Dim intGridHeight As Integer = BO.BAS.IsNullInt(Request.Item("gridheight").Replace(".", ","))
            Dim intFraHeight As Integer = 500
            If intGridHeight > 0 Then intFraHeight = intGridHeight + 50
            Dim strGridHeight As String = "75%", strFraheight As String = "80%"
            If intGridHeight > 0 Then
                strGridHeight = intGridHeight.ToString & "px"
                strFraheight = intFraHeight.ToString & "px"
            End If
            With grid1.radGridOrig.ClientSettings.Scrolling
                .AllowScroll = True

                .ScrollHeight = Unit.Parse(strGridHeight)
            End With

            fraSubform.Attributes.Item("height") = strFraheight
        End If


        grid1.radGridOrig.MasterTableView.AllowFilteringByColumn = Me.chkAutoFilter.Checked

        Dim strGroupField As String = Me.opgGroupBy.SelectedValue
        If Me.opgGroupBy.SelectedValue = "" Then
            With grid1.radGridOrig.MasterTableView
                .ShowGroupFooter = False
                .GroupByExpressions.Clear()
            End With
            Return
        End If
        Dim strHeaderText As String = Me.opgGroupBy.SelectedItem.Text

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

  

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e, True)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = 0
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With
        InhaleMyQuery(mq)

        ''Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq, ViewState("guid"))

        ''grid1.DataSource = lis

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetGridDataSource(mq, ViewState("guid"))
        If dt Is Nothing Then
            Master.Notify(Master.Factory.p31WorksheetBL.ErrorMessage, NotifyLevel.ErrorMessage)
            Return
        Else
            grid1.DataSourceDataTable = dt
        End If

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq, ViewState("guid"))
        RecalcVirtualRowCount(lis)

        Dim sets As List(Of String) = Master.Factory.p31WorksheetBL.GetList_ApprovingSet(ViewState("guid"), Nothing, Nothing)
        With Me.p31ApprovingSet
            .Items.Clear()
            For Each s In sets
                .Items.Add(New Telerik.Web.UI.RadComboBoxItem(s))
            Next
        End With
    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            .MG_GridGroupByField = opgGroupBy.SelectedValue
        End With
    End Sub

    Private Sub SetupJ74Combo()
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet).Where(Function(p) p.j74MasterPrefix = "approving_step3")
        If lisJ74.Count = 0 Then
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "approving_step3")
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p31Worksheet).Where(Function(p) p.j74MasterPrefix = "approving_step3")
        End If
        j74id.DataSource = lisJ74
        j74id.DataBind()


        If Not Page.IsPostBack Then
            If Me.j74id.Items.Count > 0 Then Me.CurrentJ74ID = CInt(Master.Factory.j03UserBL.GetUserParam("p31_approving_step3-j74id", "0"))
        End If


    End Sub

    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_approving_step3-j74id", Me.CurrentJ74ID.ToString)
        SetupGrid()
        grid1.Rebind(True)
        RefreshSubform(Me.hiddatapid.Value)
    End Sub

    Private Sub RefreshSubform(strPID As String)
        If strPID = "" Then Return

        Me.fraSubform.Attributes.Item("src") = "p31_approving_step3_subform.aspx?pid=" & strPID & "&guid=" & ViewState("guid")

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click

        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "j74"
                SetupJ74Combo()
                Me.CurrentJ74ID = BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value)
                SetupGrid()
                grid1.Rebind(False)
            Case "p31text"

                grid1.Rebind(True, BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value))
            Case Else
                RefreshSubform(Me.hidHardRefreshPID.Value)
                grid1.Rebind(True, BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value))

        End Select

        RefreshRecord()


        Me.hidHardRefreshPID.Value = ""
        Me.hidHardRefreshFlag.Value = ""
    End Sub
    Private Sub RecalcVirtualRowCount(lis As IEnumerable(Of BO.p31Worksheet))
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)


        Dim cSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, False, False, ViewState("guid"))
        If Not cSum Is Nothing Then
            'grid1.VirtualRowCount = cSum.RowsCount

            ViewState("footersum") = grid1.GenerateFooterItemString(cSum)

            hours_billable_orig.Text = BO.BAS.FNI(cSum.p31Hours_Orig)
            hours_4.Text = BO.BAS.FN(cSum.p31Hours_Approved_Billing)
        Else
            ViewState("footersum") = ""
            grid1.VirtualRowCount = 0
        End If

        Me.RowCount.Text = BO.BAS.FNI(lis.Count)
        Dim dblHours_Orig_Billable As Double = lis.Where(Function(p) p.p33ID = BO.p33IdENUM.Cas And p.p32IsBillable = True).Select(Function(p) p.p31Hours_Orig).Sum()
        Me.hours_billable_orig.Text = BO.BAS.FN(dblHours_Orig_Billable)

        Dim dblHours_3 As Double = lis.Where(Function(p) p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.SkrytyOdpis).Select(Function(p) p.p31Hours_Orig).Sum
        hours_3.Text = BO.BAS.FN(dblHours_3)
        Dim dblHours_6 As Double = lis.Where(Function(p) p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.ZahrnoutDoPausalu).Select(Function(p) p.p31Hours_Orig).Sum
        hours_6.Text = BO.BAS.FN(dblHours_6)
        Dim dblHours_2 As Double = lis.Where(Function(p) p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.ViditelnyOdpis).Select(Function(p) p.p31Hours_Orig).Sum
        hours_2.Text = BO.BAS.FN(dblHours_2)

        Me.RowsCount_Approved.Text = BO.BAS.FNI(lis.Where(Function(p) p.p71ID = BO.p71IdENUM.Schvaleno).Count).ToString

        Dim lisJ27_Time As IEnumerable(Of String) = lis.Where(Function(p) p.j27ID_Billing_Orig > 0 And p.p32IsBillable = True And p.p33ID = BO.p33IdENUM.Cas).Select(Function(p) p.j27Code_Billing_Orig).Distinct
        Dim x As Integer = 0
        For Each strJ27Code In lisJ27_Time
            Dim s0 As String = BO.BAS.FN(lis.Where(Function(p) p.p33ID = BO.p33IdENUM.Cas And p.j27Code_Billing_Orig = strJ27Code).Select(Function(p) p.p31Amount_WithoutVat_Orig).Sum) & " " & strJ27Code
            Dim s1 As String = BO.BAS.FN(lis.Where(Function(p) p.p33ID = BO.p33IdENUM.Cas And p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.Fakturovat And p.j27Code_Billing_Orig = strJ27Code).Select(Function(p) p.p31Amount_WithoutVat_Approved).Sum) & " " & strJ27Code
            If x = 0 Then
                fee_billable_orig.Text = " -> " & s0
                fee_4.Text = " -> " & s1
            Else
                fee_billable_orig.Text += " + " & s0
                fee_4.Text += " + " & s1
            End If
            x += 1
        Next

        imgProfitLost_Time.Visible = False
        profit_lost_time.Visible = False
        If lisJ27_Time.Count = 1 Then
            Dim dblFeeOrig As Double = lis.Where(Function(p) p.p33ID = BO.p33IdENUM.Cas).Select(Function(p) p.p31Amount_WithoutVat_Orig).Sum
            Dim dblFee4 As Double = lis.Where(Function(p) p.p33ID = BO.p33IdENUM.Cas And p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.Fakturovat).Select(Function(p) p.p31Amount_WithoutVat_Approved).Sum
            If dblFee4 - dblFeeOrig <> 0 Then
                imgProfitLost_Time.Visible = True : profit_lost_time.Visible = True
            End If
            Select Case dblFee4 - dblFeeOrig
                Case Is > 0
                    imgProfitLost_Time.ImageUrl = "Images/correction_up.gif"
                    profit_lost_time.Text = BO.BAS.FN(dblFee4 - dblFeeOrig) : profit_lost_time.ForeColor = System.Drawing.Color.Blue
                Case Is < 0
                    imgProfitLost_Time.ImageUrl = "Images/correction_down.gif"
                    profit_lost_time.Text = BO.BAS.FN(dblFee4 - dblFeeOrig) : profit_lost_time.ForeColor = System.Drawing.Color.Red
            End Select
        End If

        Dim lisJ27_Other As IEnumerable(Of String) = lis.Where(Function(p) p.j27ID_Billing_Orig > 0 And p.p32IsBillable = True And p.p33ID <> BO.p33IdENUM.Cas).Select(Function(p) p.j27Code_Billing_Orig).Distinct
        x = 0
        For Each strJ27Code In lisJ27_Other
            Dim s0 As String = BO.BAS.FN(lis.Where(Function(p) p.p33ID <> BO.p33IdENUM.Cas And p.j27Code_Billing_Orig = strJ27Code).Select(Function(p) p.p31Amount_WithoutVat_Orig).Sum) & " " & strJ27Code
            Dim s1 As String = BO.BAS.FN(lis.Where(Function(p) p.p33ID <> BO.p33IdENUM.Cas And p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.Fakturovat And p.j27Code_Billing_Orig = strJ27Code).Select(Function(p) p.p31Amount_WithoutVat_Approved).Sum) & " " & strJ27Code
            If x = 0 Then
                other_income_orig.Text = s0
                other_income_approved.Text = s1
            Else
                other_income_orig.Text += " + " & s0
                other_income_approved.Text += " + " & s1
            End If
            x += 1
        Next
        If lisJ27_Other.Count = 1 Then
            Dim dblOrig As Double = lis.Where(Function(p) p.p33ID <> BO.p33IdENUM.Cas).Select(Function(p) p.p31Amount_WithoutVat_Orig).Sum
            Dim dblApproved As Double = lis.Where(Function(p) p.p33ID <> BO.p33IdENUM.Cas And p.p71ID = BO.p71IdENUM.Schvaleno And p.p72ID_AfterApprove = BO.p72IdENUM.Fakturovat).Select(Function(p) p.p31Amount_WithoutVat_Approved).Sum
            If dblApproved - dblOrig <> 0 Then
                imgProfitLost_Other.Visible = True : profit_lost_other.Visible = True
            End If
            Select Case dblApproved - dblOrig
                Case Is > 0
                    imgProfitLost_Other.ImageUrl = "Images/correction_up.gif"
                    profit_lost_other.Text = BO.BAS.FN(dblApproved - dblOrig) : profit_lost_time.ForeColor = System.Drawing.Color.Blue
                Case Is < 0
                    imgProfitLost_Time.ImageUrl = "Images/correction_down.gif"
                    profit_lost_time.Text = BO.BAS.FN(dblApproved - dblOrig) : profit_lost_time.ForeColor = System.Drawing.Color.Red
            End Select
        End If


        'grid1.VirtualRowCount = Master.Factory.p31WorksheetBL.GetVirtualCount(mq)
        'grid1.radGridOrig.CurrentPageIndex = 0

    End Sub

    Private Sub grid1_NeedFooterSource(footerItem As Telerik.Web.UI.GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"


        grid1.ParseFooterItemString(footerItem, ViewState("footersum"))

        If Not Page.IsPostBack Then
            If grid1.GetSelectedPIDs.Count = 0 And grid1.RowsCount > 0 Then
                Dim lis As New List(Of Integer)
                lis.Add(grid1.GetAllPIDs(0))
                grid1.SelectRecords(lis)
                RefreshSubform(lis(0).ToString)
            End If
        End If

    End Sub

    Private Sub cmdBatch_2_Click(sender As Object, e As EventArgs) Handles cmdBatch_2.Click
        BatchOper_P72(BO.p71IdENUM.Schvaleno, BO.p72IdENUM.ViditelnyOdpis)
    End Sub

    Private Sub cmdBatch_3_Click(sender As Object, e As EventArgs) Handles cmdBatch_3.Click
        BatchOper_P72(BO.p71IdENUM.Schvaleno, BO.p72IdENUM.SkrytyOdpis)
    End Sub

    Private Sub cmdBatch_4_Click(sender As Object, e As EventArgs) Handles cmdBatch_4.Click
        BatchOper_P72(BO.p71IdENUM.Schvaleno, BO.p72IdENUM.Fakturovat)
    End Sub

    Private Sub cmdBatch_6_Click(sender As Object, e As EventArgs) Handles cmdBatch_6.Click
        BatchOper_P72(BO.p71IdENUM.Schvaleno, BO.p72IdENUM.ZahrnoutDoPausalu)
    End Sub

    Private Sub cmdBatch_Clear_Click(sender As Object, e As EventArgs) Handles cmdBatch_Clear.Click
        BatchOper_P72(BO.p71IdENUM.Nic, BO.p72IdENUM._NotSpecified)
    End Sub


    Private Sub chkUseInternalApproving_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseInternalApproving.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_approving-use_internal_approving", BO.BAS.GB(Me.chkUseInternalApproving.Checked))
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            If Not SaveChanges() Then Return
            Master.CloseAndRefreshParent("approving")
        End If
    End Sub

    Private Function SaveChanges() As Boolean
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        Dim strErrs As String = ""
        For Each cTemp In lis
            Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.LoadTempRecord(cTemp.p85DataPID, ViewState("guid"))
            If cRec Is Nothing Then
                Master.Notify(String.Format("Metoda [LoadTempRecord], chyba: cTemp.p85DataPID={0}, guid={1}", cTemp.p85DataPID, ViewState("guid")), NotifyLevel.ErrorMessage)
                Return False
            End If
            Dim cApprove As New BO.p31WorksheetApproveInput(cRec.PID, cRec.p33ID)

            With cApprove
                .p71id = cRec.p71ID
                .p72id = cRec.p72ID_AfterApprove
                .p31ApprovingSet = cRec.p31ApprovingSet
                .Value_Approved_Billing = cRec.p31Value_Approved_Billing
                .Value_Approved_Internal = cRec.p31Value_Approved_Internal
                Select Case cRec.p33ID
                    Case BO.p33IdENUM.Kusovnik, BO.p33IdENUM.Cas
                        .Rate_Billing_Approved = cRec.p31Rate_Billing_Approved
                        .Rate_Internal_Approved = cRec.p31Rate_Internal_Approved
                        .VatRate_Approved = cRec.p31VatRate_Approved
                    Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                        .VatRate_Approved = cRec.p31VatRate_Approved
                End Select
                .p31Text = cRec.p31Text
            End With

            With Master.Factory.p31WorksheetBL
                If .Save_Approving(cApprove, False) Then

                Else
                    'strErrs += "<hr>" & .ErrorMessage
                End If
            End With
        Next

        Return True
    End Function

    Private Sub p31_approving_step3_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then
            If Request.Item("clearapprove") = "1" Then
                'Žádost z aplikace o hromadné vyčištění
                'Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
                Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(New BO.myQueryP31, ViewState("guid"))
                Dim p31ids As List(Of Integer) = lisP31.Select(Function(p) p.PID).ToList
                If p31ids.Count = 0 Then
                    Master.Notify("Na vstupu není ani jeden schválený úkon.", NotifyLevel.WarningMessage)
                    Return
                End If
                grid1.SelectRecords(p31ids)
                BatchOper_P72(BO.p71IdENUM.Nic, BO.p72IdENUM._NotSpecified)
                RecalcVirtualRowCount(lisP31)
                Master.Notify("Vyčištění schvalovacího příznaku u vybraných úkonů potvrdíte tlačítkem [Uložit změny].", NotifyLevel.InfoMessage)
            End If
        End If
    End Sub


    Private Sub opgGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_approving-group", Me.opgGroupBy.SelectedValue)
        SetupGrid()
        grid1.Rebind(True, BO.BAS.IsNullInt(Me.hiddatapid.Value))
        RefreshSubform(Me.hiddatapid.Value)
    End Sub

    Private Sub chkAutoFilter_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoFilter.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_approving-autofilter", BO.BAS.GB(Me.chkAutoFilter.Checked))
        SetupGrid()
        grid1.Rebind(True, BO.BAS.IsNullInt(Me.hiddatapid.Value))
    End Sub

    Private Sub chkStaticHeaders_CheckedChanged(sender As Object, e As EventArgs) Handles chkStaticHeaders.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_approving-static_headers", BO.BAS.GB(Me.chkStaticHeaders.Checked))
        grid1.radGridOrig.ClientSettings.Scrolling.UseStaticHeaders = chkStaticHeaders.Checked
        grid1.Rebind(True)
    End Sub

    Private Sub cmdBatch_ApprovingSet_Click(sender As Object, e As EventArgs) Handles cmdBatch_ApprovingSet.Click
        Dim strName As String = Trim(Me.p31ApprovingSet.Text)
        If strName = "" Then
            Master.Notify("Musíte specifikovat název billing dávky.", NotifyLevel.WarningMessage)
            Return
        End If
        Dim p31ids As List(Of Integer) = grid1.GetSelectedPIDs()
       
        Master.Factory.p31WorksheetBL.UpdateDeleteApprovingSet(strName, p31ids, False, ViewState("guid"))
        grid1.Rebind(True)
    End Sub

    Private Sub cmdBatch_ApprovingSet_Clear_Click(sender As Object, e As EventArgs) Handles cmdBatch_ApprovingSet_Clear.Click
        Dim p31ids As List(Of Integer) = grid1.GetSelectedPIDs()
        Master.Factory.p31WorksheetBL.UpdateDeleteApprovingSet("", p31ids, True, ViewState("guid"))
        grid1.Rebind(True)
    End Sub
End Class