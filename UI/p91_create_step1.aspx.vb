Imports Telerik.Web.UI
Public Class p91_create_step1
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p91_create_step1_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p91_create_start"
    End Sub
    Public Property CurrentPrefix As String
        Get
            Return Me.opgPrefix.SelectedValue
        End Get
        Set(value As String)
            Me.opgPrefix.SelectedValue = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("rows") = 0
            With Master
                Me.CurrentPrefix = IIf(Request.Item("prefix") = "", "p41", Request.Item("prefix"))
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Request.Item("nogateway") = "1" Then
                    panGateWay.Visible = False
                End If
                If Request.Item("quick") = "1" Then
                    'přímá žádost o vytvoření zrychlené faktury
                    Select Case Me.CurrentPrefix
                        Case "p41"
                            Me.p41ID_Quick.Value = .DataPID.ToString
                            Dim cP41 As BO.p41Project = .Factory.p41ProjectBL.Load(Master.DataPID)
                            Me.p41ID_Quick.Text = cP41.FullName
                    End Select
                    Me.CurrentPrefix = "quick" : Master.DataPID = 0
                Else
                    If Not .Factory.TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator) Then
                        'nemá oprávnění na zrychlené vygenerování faktury
                        With opgPrefix.Items
                            .Remove(.FindByValue("quick"))
                        End With
                    End If
                End If
                
                .HeaderIcon = "Images/invoice_32.png"
                Dim lisPars As New List(Of String), strGridKey As String = "p91_create-j74id_" & Me.CurrentPrefix & "-" & CInt(BO.p31RecordState.Approved).ToString
                With lisPars
                    .Add("p91_create-period")
                    .Add("periodcombo-custom_query")
                    .Add("p91_create-group")
                    .Add("p91_create-pagesize")
                    .Add(strGridKey)
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)
                ViewState("j74id") = .Factory.j03UserBL.GetUserParam(strGridKey, "0")
                If ViewState("j74id") = "" Or ViewState("j74id") = "0" Then
                    .Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p31Worksheet, .Factory.SysUser.PID, Me.CurrentPrefix, BO.p31RecordState.Approved)
                    Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, .Factory.SysUser.PID, Me.CurrentPrefix, BO.p31RecordState.Approved)
                    ViewState("j74id") = cJ74.PID
                    .Factory.j03UserBL.SetUserParam(strGridKey, ViewState("j74id"))
                End If
                .AddToolbarButton("Pokračovat", "continue", , "Images/continue.png")
                Dim strDefPeriod As String = .Factory.j03UserBL.GetUserParam("p91_create-period")
                If Request.Item("period") <> "" Then strDefPeriod = Request.Item("period")
                period1.SetupData(.Factory, .Factory.j03UserBL.GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = strDefPeriod
                basUI.SelectRadiolistValue(Me.opgGroupBy, .Factory.j03UserBL.GetUserParam("p91_create-group"))
                basUI.SelectDropdownlistValue(Me.cbxPaging, .Factory.j03UserBL.GetUserParam("p91_create-pagesize", "20"))
            End With

            SetupGrid()
            RecalcVirtualRowCount()
            RefreshRecord()

        End If


    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then Return
        lblEntityHeader.Text = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentPrefix), Master.DataPID)

        Dim mq As New BO.myQueryP31
        Dim mqO23 As New BO.myQueryO23

        notepad1.EntityX29ID = BO.BAS.GetX29FromPrefix(Me.CurrentPrefix)
        Select Case Me.CurrentPrefix
            Case "p28"
                mqO23.p28ID = Master.DataPID
            Case "p41"
                mqO23.p41ID = Master.DataPID
            Case "j02"
                mqO23.j02ID = Master.DataPID
        End Select
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23).Where(Function(p) p.o24IsBillingMemo = True)
        notepad1.RefreshData(lisO23, Master.DataPID)


    End Sub


    Private Sub ReloadPage(Optional strExplicitPID As String = "")

        Dim strURL As String = "p91_create_step1.aspx?prefix=" & Me.CurrentPrefix
        If strExplicitPID <> "" Then
            strURL += "&pid=" & strExplicitPID
        End If

        Server.Transfer(strURL, False)

    End Sub
    Private Sub p28id_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles p28id.AutoPostBack_SelectedIndexChanged
        ReloadPage(NewValue)
    End Sub
    Private Sub p41id_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles p41id.AutoPostBack_SelectedIndexChanged
        ReloadPage(NewValue)
    End Sub

    Private Sub j02id_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles j02id.AutoPostBack_SelectedIndexChanged
        ReloadPage(NewValue)
    End Sub
    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p91_create-period", period1.SelectedValue)
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub p91_create_step1_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.p28id.Visible = False : Me.j02id.Visible = False : Me.p41id.Visible = False : Me.panQuick.Visible = False

        Select Case Me.CurrentPrefix
            Case "p28"
                Me.p28id.Visible = True
                lblFindEntity.Text = "Vyberte klienta:"

                lblO23.Text = "Fakturační poznámky ke klientovi"
                imgEntity.ImageUrl = "Images/contact_32.png"
            Case "p41"
                Me.p41id.Visible = True
                Me.lblFindEntity.Text = "Vyberte projekt:"

                lblO23.Text = "Fakturační poznámky k projektu"
                imgEntity.ImageUrl = "Images/project_32.png"
            Case "j02"
                Me.j02id.Visible = True
                lblFindEntity.Text = "Vyberte osobu:"
                lblO23.Text = "Fakturační poznámky k osobě"
                imgEntity.ImageUrl = "Images/person_32.png"
            Case "quick"
                Me.panQuick.Visible = True
                RefreshQuickEnvironment()
        End Select
        Me.lblO23.Text += " (" & notepad1.RowsCount.ToString & ")"

        If Master.DataPID > 0 Then
            panSelectedEntity.Visible = True
            Master.HeaderText = "Vytvořit klientskou fakturu | " & lblEntityHeader.Text
        Else
            Master.HeaderText = "Vytvořit klientskou fakturu"
            panSelectedEntity.Visible = False
        End If
        panComment.Visible = panSelectedEntity.Visible

        With Me.period1
            .Visible = True
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    Private Sub opgPrefix_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgPrefix.SelectedIndexChanged
        ReloadPage()
    End Sub


    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        period1.SetupData(Master.Factory, Master.Factory.j03UserBL.GetUserParam("periodcombo-custom_query"))
        RefreshRecord()
        RecalcVirtualRowCount()
        grid1.Rebind(True)
    End Sub

    Private Function PrepareQuickInvoice() As Boolean
        Dim c As New BO.p31WorksheetEntryInput
        c.p31Date = Me.p31Date_Quick.SelectedDate
        c.p41ID = BO.BAS.IsNullInt(Me.p41ID_Quick.Value)
        c.p34ID = BO.BAS.IsNullInt(Me.p34ID_Quick.SelectedValue)
        c.p32ID = BO.BAS.IsNullInt(Me.p32ID_Quick.SelectedValue)
        c.p31Text = Me.p31Text.Text
        c.j02ID = Master.Factory.SysUser.j02ID
        c.Amount_WithoutVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithoutVat_Orig.Value)
        c.Value_Orig = c.Amount_WithoutVat_Orig
        c.VatRate_Orig = BO.BAS.IsNullNum(Me.p31VatRate_Orig.Text)
        c.j27ID_Billing_Orig = BO.BAS.IsNullInt(Me.j27ID.SelectedValue)
        c.RecalcEntryAmount(c.Value_Orig, c.VatRate_Orig)
        With Master.Factory.p31WorksheetBL
            If .SaveOrigRecord(c, Nothing) Then
                'zdrojový úkon uložen
                Dim intP31ID As Integer = .LastSavedPID
                Dim cRec As BO.p31Worksheet = .Load(intP31ID)
                Dim cA As New BO.p31WorksheetApproveInput(intP31ID, cRec.p33ID)
                cA.p72id = BO.p72IdENUM.Fakturovat
                cA.p71id = BO.p71IdENUM.Schvaleno
                cA.Value_Approved_Billing = cRec.p31Amount_WithoutVat_Orig
                cA.VatRate_Approved = cRec.p31VatRate_Orig
                cA.Value_Approved_Internal = cA.Value_Approved_Billing

                If .Save_Approving(cA, False) Then
                    'úkon schválen
                    Master.DataPID = cRec.PID
                    Return True
                Else
                    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                End If
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
        
    End Function

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "continue" Then
            If Me.CurrentPrefix = "quick" Then
                'zrychlený postup vytvoření faktury
                If PrepareQuickInvoice() Then
                    'jeden nový peněžní úkon je uložen a schválen, jeho ID je uloženo v: Master.DataPID
                    RecalcVirtualRowCount()
                Else
                    'při zakládání nebo schvalování worksheet úkonu došlo k chybě. zde skončit.
                    Return
                End If

            End If
            If Master.DataPID = 0 Then
                Master.Notify("Musíte vybrat klienta, projekt nebo osobu!", NotifyLevel.WarningMessage) : Return
            End If
            If ViewState("rows") = 0 Then
                Master.Notify("Na vstupu nejsou žádné worksheet úkony!", NotifyLevel.WarningMessage) : Return
            End If

            Dim mq As New BO.myQueryP31
            InhaleMyQuery(mq)
            Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
            If lisP31.Count = 0 Then
                Master.Notify("Na vstupu nejsou žádné worksheet úkony!", NotifyLevel.WarningMessage) : Return
            End If
            Dim strGUID As String = BO.BAS.GetGUID
            For Each c In lisP31
                Dim cTEMP As New BO.p85TempBox()
                cTEMP.p85GUID = strGUID
                cTEMP.p85DataPID = c.PID
                cTEMP.p85Prefix = "p31"
                Master.Factory.p85TempBoxBL.Save(cTEMP)
            Next
            If Me.CurrentPrefix = "quick" Then
                Me.CurrentPrefix = "p41"
                Master.DataPID = Master.Factory.p31WorksheetBL.Load(Master.DataPID).p41ID
            End If
            Server.Transfer("p91_create_step2.aspx?guid=" & strGUID & "&prefix=" & Me.CurrentPrefix & "&pid=" & Master.DataPID.ToString, False)
        End If
    End Sub


    Private Sub SetupGrid()
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))

        basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, CInt(Me.cbxPaging.SelectedValue), True, False)

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
            Case "j27"
                strGroupField = "j27Code_Billing_Orig" : strHeaderText = "Měna úkonu"
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
        If Master.DataPID = 0 Then Return
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim cSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
        If Not cSum Is Nothing Then
            grid1.VirtualRowCount = cSum.RowsCount
            Me.TotalAmount.Text = BO.BAS.FN(cSum.p31Amount_WithoutVat_Approved)
            ViewState("rows") = cSum.RowsCount
            ViewState("footersum") = grid1.GenerateFooterItemString(cSum)
        Else
            ViewState("footersum") = ""
            grid1.VirtualRowCount = 0
        End If


        grid1.radGridOrig.CurrentPageIndex = 0

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)

        Select Case Me.CurrentPrefix
            Case "p28"
                mq.p28ID = Master.DataPID
            Case "p41"
                mq.p41ID = Master.DataPID
            Case "j02"
                mq.j02ID = Master.DataPID
            Case "quick"
                mq.AddItemToPIDs(Master.DataPID)
        End Select
        If Me.CurrentPrefix <> "quick" Then
            mq.DateFrom = period1.DateFrom
            mq.DateUntil = period1.DateUntil
        End If
        mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForCreateInvoice

    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Master.DataPID = 0 Then Return
        Dim strSort As String = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
        Select Case Me.opgGroupBy.SelectedValue
            Case "p41"
                strSort = "p41Name"
            Case "p34"
                strSort = "p34Name"
            Case "p95"
                strSort = "p95Name"
            Case "p32"
                strSort = "p34name,p32Name"
            Case "j02"
                strSort = "Person"
            Case "j27"
                strSort = "j27Code_Billing_Orig"
            Case Else
        End Select
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)
        With mq
            .MG_PageSize = CInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = strSort

        End With

        grid1.DataSource = Master.Factory.p31WorksheetBL.GetList(mq)
    End Sub

    Private Sub grid1_NeedFooterSource(footerItem As Telerik.Web.UI.GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"
        grid1.ParseFooterItemString(footerItem, ViewState("footersum"))
    End Sub

    Private Sub opgGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p91_create-group", Me.opgGroupBy.SelectedValue)
        SetupGrid()
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p91_create-pagesize", Me.cbxPaging.SelectedValue)
        SetupGrid()
        RecalcVirtualRowCount()
        grid1.Rebind(False)

    End Sub

    Private Sub RefreshQuickEnvironment()
        If Me.p31Date_Quick.IsEmpty Then Me.p31Date_Quick.SelectedDate = Today
        
        If Me.j27ID.Rows <= 1 Then
            Me.j27ID.DataSource = Master.Factory.ftBL.GetList_J27(New BO.myQuery)
            Me.j27ID.DataBind()
            Me.j27ID.SelectedValue = Master.Factory.x35GlobalParam.j27ID_Invoice.ToString
        End If
        If Me.p41ID_Quick.Value <> "" And Me.p34ID_Quick.Rows = 0 Then
            Handle_ChangeP41_Quick()
        End If

    End Sub


    Private Sub Handle_ChangeP34()
        If Me.p34ID_Quick.SelectedValue = "" Then
            Me.p32ID_Quick.Clear()
            Return
        End If

        Dim mq As New BO.myQueryP32
        mq.p34ID = BO.BAS.IsNullInt(Me.p34ID_Quick.SelectedValue)
        Me.p32ID_Quick.DataSource = Master.Factory.p32ActivityBL.GetList(mq)
        Me.p32ID_Quick.DataBind()

        SetupVatRateCombo()
        
    End Sub

    Private Sub SetupVatRateCombo()

        Dim intJ27ID As Integer = BO.BAS.IsNullInt(Me.j27ID.SelectedValue), strVat As String = Me.p31VatRate_Orig.SelectedValue
        Dim lis As IEnumerable(Of BO.p53VatRate) = Master.Factory.p53VatRateBL.GetList(New BO.myQuery).Where(Function(p) p.j27ID = intJ27ID)
        Me.p31VatRate_Orig.DataSource = lis
        Me.p31VatRate_Orig.DataBind()

        If strVat <> "" Then Me.p31VatRate_Orig.SelectedValue = strVat

        With Me.p31VatRate_Orig
            If .SelectedValue = "" And .Rows > 1 Then
                .SelectedIndex = .Rows - 1
            End If
        End With
    End Sub

    Private Sub p34ID_Quick_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles p34ID_Quick.SelectedIndexChanged
        Handle_ChangeP34()
    End Sub


    Private Sub Handle_ChangeP41_Quick()
        If Me.p41ID_Quick.Value = "" Then
            Me.p34ID_Quick.Clear() : Me.p32ID_Quick.Clear()
            Return
        End If
        Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(BO.BAS.IsNullInt(Me.p41ID_Quick.Value))

        Me.p34ID_Quick.DataSource = Master.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cP41.PID, cP41.p42ID, cP41.j18ID, Master.Factory.SysUser.j02ID).Where(Function(p) p.p33ID = BO.p33IdENUM.PenizeBezDPH Or p.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu)
        Me.p34ID_Quick.DataBind()

        Handle_ChangeP34()

           

    End Sub

    Private Sub p41ID_Quick_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles p41ID_Quick.AutoPostBack_SelectedIndexChanged
        Handle_ChangeP41_Quick()
    End Sub
End Class