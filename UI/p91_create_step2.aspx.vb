Imports Telerik.Web.UI

Public Class p91_create_step2
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
        Set(value As String)
            Me.hidPrefix.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentP28ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p28id.Value)
        End Get
    End Property

    Private Sub p91_create_step2_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p91_create_finish"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("rows") = 0
            With Master
                If Request.Item("guid") = "" Then .StopPage("guid missing")
                ViewState("guid") = Request.Item("guid")
                Me.CurrentPrefix = Request.Item("prefix")

                .HeaderIcon = "Images/invoice_32.png"
                Dim lisPars As New List(Of String), strGridKey As String = "p91_create-j74id_" & Me.CurrentPrefix & "-" & CInt(BO.p31RecordState.Approved).ToString
                With lisPars
                    .Add("p91_create-group")
                    .Add(strGridKey)
                    .Add("p91_create-pagesize")
                    .Add("p91_create-rememberdates")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)
                ViewState("j74id") = .Factory.j03UserBL.GetUserParam(strGridKey, "0")
                If ViewState("j74id") = "" Or ViewState("j74id") = "0" Then
                    .Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p31Worksheet, .Factory.SysUser.PID, Me.CurrentPrefix, BO.p31RecordState.Approved)
                    Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, .Factory.SysUser.PID, Me.CurrentPrefix, BO.p31RecordState.Approved)
                    ViewState("j74id") = cJ74.PID
                    .Factory.j03UserBL.SetUserParam(strGridKey, ViewState("j74id"))
                End If
                .AddToolbarButton("Uložit fakturu", "save", , "Images/save.png")

                basUI.SelectRadiolistValue(Me.opgGroupBy, .Factory.j03UserBL.GetUserParam("p91_create-group"))
                basUI.SelectDropdownlistValue(Me.cbxPaging, .Factory.j03UserBL.GetUserParam("p91_create-pagesize", "20"))
                Me.chkRememberDates.Checked = BO.BAS.BG(.Factory.j03UserBL.GetUserParam("p91_create-rememberdates", "0"))
                Dim lisP92 As IEnumerable(Of BO.p92InvoiceType) = .Factory.p92InvoiceTypeBL.GetList(New BO.myQuery)
                Me.p92ID.DataSource = lisP92.Where(Function(p) p.p92InvoiceType = BO.p92InvoiceTypeENUM.ClientInvoice).OrderBy(Function(p) p.j27ID)
                Me.p92ID.DataBind()
            End With

            InhaleDefaults()
            

            SetupGrid()
            RecalcVirtualRowCount()
            RefreshRecord()

            Handle_Permissions()
        End If
    End Sub

    Private Sub Handle_Permissions()
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P91_Creator) Then
            Me.opgCode.SelectedValue = "1"
            Me.opgCode.Enabled = False
        End If
    End Sub

    Private Sub InhaleDefaults()
        Dim intDataPID As Integer = BO.BAS.IsNullInt(Request.Item("pid")), intP92ID As Integer = 0
        Handle_p91Date()
        Handle_p91DateSupply()

        Me.p91DateSupply.SelectedDate = Today

        Dim cP28 As BO.p28Contact = Nothing, intMaturityDays As Integer = Master.Factory.x35GlobalParam.GetValueInteger("DefMaturityDays", "10")
        If intDataPID > 0 Then
            Select Case Me.CurrentPrefix
                Case "p28"
                    cP28 = Master.Factory.p28ContactBL.Load(intDataPID)
                    Me.p91text1.Text = cP28.p28InvoiceDefaultText1
                    If cP28.p28InvoiceMaturityDays > 0 Then intMaturityDays = cP28.p28InvoiceMaturityDays
                    intP92ID = cP28.p92ID
                Case "p41"
                    Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(intDataPID)
                    cP28 = Master.Factory.p28ContactBL.Load(cP41.p28ID_Client)
                    If Not cP28 Is Nothing Then
                        intMaturityDays = cP28.p28InvoiceMaturityDays
                    End If
                    If cP41.p41InvoiceDefaultText1 <> "" Then
                        Me.p91text1.Text = cP41.p41InvoiceDefaultText1
                    Else
                        If Not cP28 Is Nothing Then Me.p91text1.Text = cP28.p28InvoiceDefaultText1
                    End If
                    If cP41.p92ID > 0 Then
                        intP92ID = cP41.p92ID
                    Else
                        If Not cP28 Is Nothing Then intP92ID = cP28.p92ID
                    End If

                    If cP41.p28ID_Billing > 0 Then
                        cP28 = Master.Factory.p28ContactBL.Load(cP41.p28ID_Billing)
                        If intP92ID = 0 Then intP92ID = cP28.p92ID
                    End If
                Case Else

            End Select
        End If
        If Not cP28 Is Nothing Then
            Me.p28id.Value = cP28.PID.ToString
            Me.p28id.Text = cP28.p28Name
        End If
        If Not Me.p91Date.IsEmpty Then
            Me.p91DateMaturity.SelectedDate = Me.p91Date.SelectedDate.Value.AddDays(intMaturityDays)
        End If
        If intP92ID > 0 Then
            basUI.SelectDropdownlistValue(Me.p92ID, intP92ID.ToString)
        End If
        If Me.chkRememberDates.Checked Then
            Dim cLastRec As BO.p91Invoice = Master.Factory.p91InvoiceBL.LoadMyLastCreated()
            If Not cLastRec Is Nothing Then
                Me.opgDateSupply.ClearSelection()
                Me.opgDate.ClearSelection()
                With cLastRec
                    Me.p91Date.SelectedDate = .p91Date
                    Me.p91DateMaturity.SelectedDate = .p91DateMaturity
                    Me.p91DateSupply.SelectedDate = .p91DateSupply
                End With
            End If
        End If
    End Sub

    Private Sub InhaleClient(intP28ID As Integer)

    End Sub

    Private Sub RefreshRecord()
        If Me.CurrentP28ID = 0 Then Return

        Dim mq As New BO.myQueryP31
        Dim mqO23 As New BO.myQueryO23
        mqO23.p28ID = Me.CurrentP28ID
        notepad1.EntityX29ID = BO.x29IdEnum.p28Contact
        
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23).Where(Function(p) p.o24IsBillingMemo = True)
        notepad1.RefreshData(lisO23, mqO23.p28ID)

        Me.lblO23.Text = BO.BAS.OM2(Me.lblO23.Text, notepad1.RowsCount.ToString)

    End Sub

    Private Sub SetupGrid()
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))

        basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, CInt(Me.cbxPaging.SelectedValue), True, True)

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
            Case "p31ApprovingSet"
                strGroupField = "p31ApprovingSet" : strHeaderText = "Billing dávka"
            Case Else
                Return
        End Select
        With grid1.radGridOrig.MasterTableView
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

        Dim cSum As BO.p31WorksheetSum = Master.Factory.p31WorksheetBL.LoadSumRow(mq, True, True)
        If Not cSum Is Nothing Then
            grid1.VirtualRowCount = cSum.RowsCount
            Me.TotalAmount.Text = BO.BAS.FN(cSum.p31Amount_WithoutVat_Approved)
            Me.TotalCount.Text = BO.BAS.FNI(cSum.RowsCount) & "x"
            Me.TotalHours.Text = BO.BAS.FN(cSum.p31Hours_Approved_Billing) & " hod."
            ViewState("rows") = cSum.RowsCount
            ViewState("footersum") = grid1.GenerateFooterItemString(cSum)
        Else
            ViewState("footersum") = ""
            grid1.VirtualRowCount = 0
        End If


        grid1.radGridOrig.CurrentPageIndex = 0

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)

        Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        Dim pids As New List(Of Integer)
        mq.PIDs = lisTemp.Select(Function(p) p.p85DataPID).ToList
        If mq.PIDs.Count = 0 Then mq.AddItemToPIDs(-666)

    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim strSort As String = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
        Select Case Me.opgGroupBy.SelectedValue
            Case "p41"
                strSort = "p41name"
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

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        grid1.DataSource = lis

        If Me.p91Datep31_From.IsEmpty Then
            Me.p91Datep31_From.SelectedDate = lis.Min(Function(p) p.p31Date)
        End If
        If Me.p91Datep31_Until.IsEmpty Then
            Me.p91Datep31_Until.SelectedDate = lis.Max(Function(p) p.p31Date)
        End If
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

    Private Sub p28id_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles p28id.AutoPostBack_SelectedIndexChanged
        RefreshRecord()
    End Sub

    Private Sub p91_create_step2_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Master.HeaderText = "Vytvořit klientskou fakturu | " & Me.p28id.Text
    End Sub

    Private Sub cmdRemovePIDs_Click(sender As Object, e As EventArgs) Handles cmdRemovePIDs.Click
        Dim pids As List(Of Integer) = grid1.GetSelectedPIDs()
        If pids.Count = 0 Then
            Master.Notify("Není vybraný žádný záznam.", NotifyLevel.WarningMessage) : Return
        End If
        Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        For Each intP31ID As Integer In pids
            If lisTemp.Where(Function(p) p.p85DataPID = intP31ID).Count > 0 Then
                Master.Factory.p85TempBoxBL.Delete(lisTemp.Where(Function(p) p.p85DataPID = intP31ID)(0))
            End If
        Next
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        RefreshRecord()
        RecalcVirtualRowCount()
        grid1.Rebind(True)
    End Sub

    Private Sub opgDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgDate.SelectedIndexChanged
        Handle_p91Date()
    End Sub

    Private Sub Handle_p91Date()
        Select Case Me.opgDate.SelectedValue
            Case "1"    'dnes
                Me.p91Date.SelectedDate = Today
            Case "2"    'konec minulého měsíce
                Me.p91Date.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddDays(-1)
            Case "3"    'konec aktuálního měsíce
                Me.p91Date.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1).AddDays(-1)
            Case "4" '1.den příštího měsíce
                Me.p91Date.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1)
        End Select
    End Sub
    Private Sub Handle_p91DateSupply()
        Select Case Me.opgDateSupply.SelectedValue
            Case "1"    'dnes
                Me.p91DateSupply.SelectedDate = Today
            Case "2"    'konec minulého měsíce
                Me.p91DateSupply.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddDays(-1)
            Case "3"    'konec aktuálního měsíce
                Me.p91DateSupply.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1).AddDays(-1)
            Case "4" '1.den příštího měsíce
                Me.p91DateSupply.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1)
        End Select
    End Sub

    Private Sub opgDateSupply_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgDateSupply.SelectedIndexChanged
        Handle_p91DateSupply()
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p91_create-pagesize", Me.cbxPaging.SelectedValue)
        SetupGrid()
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim cRec As New BO.p91Create
            With cRec
                If Me.opgCode.SelectedValue = "1" Then
                    .IsDraft = True
                Else
                    .IsDraft = False
                End If
                .TempGUID = ViewState("guid")
                .p28ID = Me.CurrentP28ID
                .p92ID = BO.BAS.IsNullInt(Me.p92ID.SelectedValue)
                .InvoiceText1 = Me.p91text1.Text
                .DateIssue = Me.p91Date.SelectedDate
                .DateMaturity = Me.p91DateMaturity.SelectedDate
                .DateSupply = Me.p91DateSupply.SelectedDate
                .DateP31_From = Me.p91Datep31_From.SelectedDate
                .DateP31_Until = Me.p91Datep31_Until.SelectedDate
            End With
            Dim intP91ID As Integer = Master.Factory.p91InvoiceBL.Create(cRec)

            If intP91ID <> 0 Then                
                Master.Factory.j03UserBL.SetUserParam("p91_create-rememberdates", BO.BAS.GB(Me.chkRememberDates.Checked))
                Master.DataPID = intP91ID
                Master.CloseAndRefreshParent("p91-create")
            Else
                Master.Notify(Master.Factory.p91InvoiceBL.ErrorMessage, NotifyLevel.ErrorMessage)
            End If


        End If
    End Sub
End Class