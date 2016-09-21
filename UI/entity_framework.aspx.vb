Imports Telerik.Web.UI
Public Class entity_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private Property _curJ62 As BO.j62MenuHome
    Private Property _x29id As BO.x29IdEnum
    Private Property _needFilterIsChanged As Boolean = False
    Private Property _CurFilterDbField As String = ""

    Public Property CurrentX29ID As BO.x29IdEnum
        Get
            If _x29id = BO.x29IdEnum._NotSpecified Then _x29id = CType(CInt(Me.hidX29ID.Value), BO.x29IdEnum)
            Return _x29id
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidX29ID.Value = CInt(value).ToString
            Me.hidPrefix.Value = BO.BAS.GetDataPrefix(value)
        End Set
    End Property
    Public ReadOnly Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            hidMasterPID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property
    Public Property CurrentJ70ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j70ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j70ID, value.ToString)
        End Set
    End Property
    Public Property CurrentJ62ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidJ62ID.Value)
        End Get
        Set(value As Integer)
            hidJ62ID.Value = value.ToString
            Master.SiteMenuValue = "hm" & value.ToString
        End Set
    End Property

    Private Sub entity_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'hidAutoScrollHashID.Value = ""
            If Request.Item("prefix") <> "" Then
                Me.CurrentX29ID = BO.BAS.GetX29FromPrefix(Request.Item("prefix"))
            End If
            If Request.Item("masterpid") <> "" Then
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid")) : Me.CurrentMasterPrefix = Request.Item("masterprefix")
            End If
            Handle_Permissions_And_More()
            SetupPeriodQuery()
            With Master
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add(Me.CurrentPrefix + "_framework-pagesize")
                    .Add(Me.CurrentPrefix + "-j70id")
                    .Add(Me.CurrentPrefix + "_framework-navigationPane_width")
                    .Add(Me.CurrentPrefix + "_framework_detail-pid")
                    .Add(Me.CurrentPrefix + "_framework-j74id")
                    .Add(Me.CurrentPrefix + "_framework-groupby")
                    .Add(Me.CurrentPrefix + "_framework-sort")
                    .Add(Me.CurrentPrefix + "_framework-groups-autoexpanded")
                    .Add(Me.CurrentPrefix + "_framework-checkbox_selector")
                    .Add("periodcombo-custom_query")
                    .Add(Me.CurrentPrefix + "_framework-periodtype")
                    .Add(Me.CurrentPrefix + "_framework-period")
                    .Add(Me.CurrentPrefix + "_framework-queryflag")
                    .Add(Me.CurrentPrefix + "_framework-filter_setting")
                    .Add(Me.CurrentPrefix + "_framework-filter_sql")
                End With
                cbxGroupBy.DataSource = .Factory.j74SavedGridColTemplateBL.GroupByPallet(Me.CurrentX29ID)
                cbxGroupBy.DataBind()
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)


                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam(Me.CurrentPrefix + "_framework-pagesize", "20"))
                    Dim strDefWidth As String = "420"
                    Select Case Me.CurrentPrefix
                        Case "o23", "p56", "p91" : strDefWidth = "500"
                        Case Else
                    End Select
                    Dim strW As String = .GetUserParam(Me.CurrentPrefix + "_framework-navigationPane_width", strDefWidth)
                    If strW = "-1" Then
                        Me.navigationPane.Collapsed = True
                    Else
                        Me.navigationPane.Width = Unit.Parse(strW & "px")
                    End If

                    If Request.Item("blankwindow") = "1" Then Me.navigationPane.Collapsed = True

                    basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam(Me.CurrentPrefix + "_framework-groupby"))
                    Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam(Me.CurrentPrefix + "_framework-groups-autoexpanded", "1"))
                    Me.chkCheckboxSelector.Checked = BO.BAS.BG(.GetUserParam(Me.CurrentPrefix + "_framework-checkbox_selector", "0"))
                    If .GetUserParam(Me.CurrentPrefix + "_framework-sort") <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam(Me.CurrentPrefix + "_framework-sort"))
                    End If
                    basUI.SelectDropdownlistValue(Me.cbxPeriodType, .GetUserParam(Me.CurrentPrefix + "_framework-periodtype", ""))
                    If Me.cbxQueryFlag.Visible Then basUI.SelectDropdownlistValue(Me.cbxQueryFlag, .GetUserParam(Me.CurrentPrefix + "_framework-queryflag"))
                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam(Me.CurrentPrefix + "_framework-period")
                End With

            End With

            Me.CurrentJ62ID = BO.BAS.IsNullInt(Request.Item("j62id"))
            If Me.CurrentJ62ID <> 0 Then
                _curJ62 = Master.Factory.j62MenuHomeBL.Load(Me.CurrentJ62ID)
                If _curJ62 Is Nothing Then Master.StopPage("j62 record not found")
                Me.j62Name.Text = _curJ62.j62Name
            Else
                Master.SiteMenuValue = Me.CurrentPrefix
            End If

            With Master.Factory.j03UserBL
                SetupJ70Combo(BO.BAS.IsNullInt(.GetUserParam(Me.CurrentPrefix + "-j70id")))
                Dim intJ74ID As Integer = BO.BAS.IsNullInt(.GetUserParam(Me.CurrentPrefix + "_framework-j74id"))
                If intJ74ID = 0 Then
                    If Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID) Then
                        _curJ74 = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
                        .SetUserParam(Me.CurrentPrefix + "_framework-j74id", _curJ74.PID)
                    End If
                End If
                SetupJ74Combo(intJ74ID)
                SetupGrid(.GetUserParam(Me.CurrentPrefix + "_framework-filter_setting"), .GetUserParam(Me.CurrentPrefix + "_framework-filter_sql"))
            End With
            RecalcVirtualRowCount()

            If Me.CurrentMasterPID = 0 Then
                Handle_DefaultSelectedRecord()
            Else
                Me.contentPane.ContentUrl = "entity_framework_detail_missing.aspx?prefix=" & Me.CurrentPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString & "&masterprefix=" & Me.CurrentMasterPrefix
            End If


        End If
    End Sub

    Private Sub SetupPeriodQuery()
        Me.cbxQueryFlag.Visible = False
        With Me.cbxPeriodType.Items
            If .Count > 0 Then .Clear()

            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p41Project
                    .Add(New ListItem("Založení projektu:", "DateInsert"))
                    .Add(New ListItem("Plánované zahájení:", "p41PlanFrom"))
                    .Add(New ListItem("Plánované dokončení:", "p41PlanUntil"))
                Case BO.x29IdEnum.p28Contact
                    .Add(New ListItem("Založení klienta:", "DateInsert"))
                Case BO.x29IdEnum.p56Task
                    .Add(New ListItem("Založení úkolu:", "DateInsert"))
                    .Add(New ListItem("Plánované zahájení:", "p56PlanFrom"))
                    .Add(New ListItem("Termín dokončení:", "p56PlanUntil"))
                Case BO.x29IdEnum.o23Notepad
                    .Add(New ListItem("Založení dokumentu:", "DateInsert"))
                    .Add(New ListItem("Datum dokumentu:", "o23Date"))
                Case BO.x29IdEnum.p91Invoice
                    .Add(New ListItem("Založení faktury:", "DateInsert"))
                    .Add(New ListItem("Datum plnění:", "p91DateSupply"))
                    .Add(New ListItem("Datum splatnosti:", "p91DateMaturity"))
                    .Add(New ListItem("Datum vystavení:", "p91Date"))

                Case BO.x29IdEnum.j02Person
                    .Add(New ListItem("Založení záznamu:", "DateInsert"))
                    cbxQueryFlag.Items.Add(New ListItem("Pouze interní osoby", "1"))
                    cbxQueryFlag.Items.Add(New ListItem("Pouze kontaktní osoby", "2"))
                    cbxQueryFlag.Items.Add(New ListItem("Všechny osobní profily", "3"))
            End Select
            .Add(New ListItem("Datum worksheet úkonu:", "p31Date"))
        End With
        
        If Me.cbxQueryFlag.Items.Count > 1 Then cbxQueryFlag.Visible = True
    End Sub

    Private Sub Handle_Permissions_And_More()
        With Master
            .PageTitle = BO.BAS.GetX29EntityAlias(Me.CurrentX29ID, True)
            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p41Project
                    img1.ImageUrl = "Images/project_32.png"
                    If Not .Factory.SysUser.j04IsMenu_Project Then .StopPage("Nedisponujete oprávněním k zobrazení stránky [Projekty].")

                Case BO.x29IdEnum.p28Contact
                    img1.ImageUrl = "Images/contact_32.png"
                    If Not .Factory.SysUser.j04IsMenu_Contact Then .StopPage("Nedisponujete oprávněním k zobrazení stránky [Klienti].")
                Case BO.x29IdEnum.o23Notepad
                    img1.ImageUrl = "Images/notepad_32.png"
                Case BO.x29IdEnum.p56Task
                    img1.ImageUrl = "Images/task_32.png"
                Case BO.x29IdEnum.j02Person
                    img1.ImageUrl = "Images/person_32.png"
                    If Not .Factory.SysUser.j04IsMenu_People Then .StopPage("Nedisponujete oprávněním k zobrazení stránky [Lidé].")
                Case BO.x29IdEnum.p91Invoice
                    img1.ImageUrl = "Images/invoice_32.png"
                    If Not .Factory.SysUser.j04IsMenu_Invoice Then .StopPage("Nedisponujete oprávněním k zobrazení stránky [Faktury].")
            End Select
        End With


    End Sub

    Private Sub SetupJ70Combo(intDef As Integer)
        If Not _curJ62 Is Nothing Then
            If _curJ62.j70ID <> 0 Then
                Me.j70ID.Items.Add(New ListItem("", _curJ62.j70ID.ToString))
                Me.j70ID.Visible = False : cmdQuery.Visible = False
                Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & _curJ62.j70ID.ToString
                Return
            End If
        End If
        If Me.CurrentMasterPID > 0 Then
            Me.j70ID.Visible = False : cmdQuery.Visible = False : Me.clue_query.Visible = False
            With Me.j62Name
                .Text = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix), Me.CurrentMasterPID)
                .CssClass = ""
                .Text = "<a href='" & Me.CurrentMasterPrefix & "_framework.aspx?pid=" & Me.CurrentMasterPID.ToString & "'>" & .Text & "</a>"
            End With


            Return 'pokud se zobrazuje přehled v rámci nadřazeného záznam, pak se nefiltruje
        End If

        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, Me.CurrentX29ID)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Pojmenovaný filtr--")
        basUI.SelectDropdownlistValue(Me.j70ID, intDef.ToString)
        With Me.j70ID
            If .SelectedIndex > 0 Then
                .ToolTip = .SelectedItem.Text
                Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & .SelectedValue
            Else
                Me.clue_query.Visible = False
            End If
        End With

    End Sub


    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo(cJ74.PID)
                End If
            End If
            Me.hidDefaultSorting.Value = cJ74.j74OrderBy
            If cJ74.x29ID = BO.x29IdEnum.p56Task Then
                If cJ74.j74ColumnNames.IndexOf("Hours_Orig") > 0 Or cJ74.j74ColumnNames.IndexOf("Expenses_Orig") > 0 Then Me.hidTasksWorksheetColumns.Value = "1" Else Me.hidTasksWorksheetColumns.Value = ""
            End If
            Dim strAddtionalSqlFrom As String = ""
            Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, True, Me.chkCheckboxSelector.Checked, strFilterSetting, strFilterExpression, , strAddtionalSqlFrom)
            Me.hidAdditionalFrom.Value = strAddtionalSqlFrom
        End With
        With grid1
            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p91Invoice
                    .radGridOrig.ShowFooter = True
                Case Else
                    .radGridOrig.ShowFooter = False
            End Select
            '.radGridOrig.SelectedItemStyle.BackColor = Drawing.Color.Red

        End With
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
    End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
        _CurFilterDbField = strFilterColumn
    End Sub



    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                basUIMT.p41_grid_Handle_ItemDataBound(sender, e, True)
            Case BO.x29IdEnum.p28Contact
                basUIMT.p28_grid_Handle_ItemDataBound(sender, e, True)
            Case BO.x29IdEnum.o23Notepad
                basUIMT.o23_grid_Handle_ItemDataBound(sender, e, False, False)
            Case BO.x29IdEnum.p56Task
                basUIMT.p56_grid_Handle_ItemDataBound(sender, e, False, True)
            Case BO.x29IdEnum.j02Person
                basUIMT.j02_grid_Handle_ItemDataBound(sender, e)
            Case BO.x29IdEnum.p91Invoice
                basUIMT.p91_grid_Handle_ItemDataBound(sender, e, True)
        End Select

    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam(Me.CurrentPrefix + "_framework-filter_setting", grid1.GetFilterSetting())
                .SetUserParam(Me.CurrentPrefix + "_framework-filter_sql", grid1.GetFilterExpression())
            End With
            RecalcVirtualRowCount()
        End If
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim mq As New BO.myQueryP41
                With mq
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                InhaleMyQuery_p41(mq)

                ''Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq)
                ''If lis Is Nothing Then
                ''    Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
                ''Else
                ''    grid1.DataSource = lis
                ''End If
                Dim dt As DataTable = Master.Factory.p41ProjectBL.GetGridDataSource(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    grid1.DataSourceDataTable = dt
                End If
            Case BO.x29IdEnum.p28Contact
                Dim mq As New BO.myQueryP28
                With mq
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex

                End With
                InhaleMyQuery_p28(mq)

                Dim dt As DataTable = Master.Factory.p28ContactBL.GetGridDataSource(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    grid1.DataSourceDataTable = dt
                End If

                ''Dim lis As IEnumerable(Of BO.p28Contact) = Master.Factory.p28ContactBL.GetList(mq)
                ''If lis Is Nothing Then
                ''    Master.Notify(Master.Factory.p28ContactBL.ErrorMessage, NotifyLevel.ErrorMessage)
                ''Else
                ''    grid1.DataSource = lis
                ''End If
            Case BO.x29IdEnum.p56Task
                Dim mq As New BO.myQueryP56
                With mq
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex

                End With
                InhaleMyQuery_p56(mq)

                'Dim bolInhaleReceiversInLine As Boolean = True
                'Dim lis As IEnumerable(Of BO.p56Task) = Nothing
                'If Me.hidTasksWorksheetColumns.Value = "1" Then
                '    lis = Master.Factory.p56TaskBL.GetList_WithWorksheetSum(mq, bolInhaleReceiversInLine)
                'Else
                '    lis = Master.Factory.p56TaskBL.GetList(mq, bolInhaleReceiversInLine)
                'End If
                'If lis Is Nothing Then
                '    Master.Notify(Master.Factory.p56TaskBL.ErrorMessage, NotifyLevel.ErrorMessage)
                'Else
                '    grid1.DataSource = lis
                'End If
                Dim dt As DataTable = Master.Factory.p56TaskBL.GetGridDataSource(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.p56TaskBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    grid1.DataSourceDataTable = dt
                End If
            Case BO.x29IdEnum.o23Notepad
                Dim mq As New BO.myQueryO23
                With mq
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                InhaleMyQuery_o23(mq)

                ''Dim lis As IEnumerable(Of BO.o23NotepadGrid) = Master.Factory.o23NotepadBL.GetList4Grid(mq)
                ''If lis Is Nothing Then
                ''    Master.Notify(Master.Factory.o23NotepadBL.ErrorMessage, NotifyLevel.ErrorMessage)
                ''Else
                ''    grid1.DataSource = lis
                ''End If
                Dim dt As DataTable = Master.Factory.o23NotepadBL.GetGridDataSource(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.o23NotepadBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    grid1.DataSourceDataTable = dt
                End If
            Case BO.x29IdEnum.j02Person
                Dim mq As New BO.myQueryJ02
                With mq
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                InhaleMyQuery_j02(mq)

                Dim dt As DataTable = Master.Factory.j02PersonBL.GetGridDataSource(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.j02PersonBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    grid1.DataSourceDataTable = dt
                End If
            Case BO.x29IdEnum.p91Invoice
                Dim mq As New BO.myQueryP91
                With mq
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                InhaleMyQuery_p91(mq)

                ''Dim lis As IEnumerable(Of BO.p91Invoice) = Master.Factory.p91InvoiceBL.GetList(mq)

                'Dim mq2 As New BO.myQueryP41
                'Dim lis2 As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq2)

                'Dim qry = From p In lis Join q In lis2 On p.p41ID_First Equals q.PID Select p, q.p41Code

                Dim dt As DataTable = Master.Factory.p91InvoiceBL.GetGridDataSource(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.p91InvoiceBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    grid1.DataSourceDataTable = dt
                End If


                ''If Not lis Is Nothing Then
                ''    grid1.DataSource = lis
                ''Else
                ''    Master.Notify(Master.Factory.p91InvoiceBL.ErrorMessage, NotifyLevel.ErrorMessage)
                ''End If
            Case Else

        End Select





    End Sub

    Private Sub InhaleMyQuery_p91(ByRef mq As BO.myQueryP91)
        With mq
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_AdditionalSqlFROM = Me.hidAdditionalFrom.Value
            .Closed = BO.BooleanQueryMode.NoQuery
            Select Case Me.CurrentMasterPrefix
                Case "p41" : .p41ID = Me.CurrentMasterPID
                Case "j02" : .j02ID = Me.CurrentMasterPID
                Case "p28" : .p28ID = Me.CurrentMasterPID
            End Select
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            Select Case Me.cbxPeriodType.SelectedValue
                Case "p91DateSupply" : .PeriodType = BO.myQueryP91_PeriodType.p91DateSupply
                Case "p91DateMaturity" : .PeriodType = BO.myQueryP91_PeriodType.p91DateMaturity
                Case "p91Date" : .PeriodType = BO.myQueryP91_PeriodType.p91Date
                Case "DateInsert"
                    .DateInsertFrom = period1.DateFrom : .DateInsertUntil = period1.DateUntil
                Case "p31Date"
                    .p31Date_D1 = period1.DateFrom : .p31Date_D2 = period1.DateUntil
            End Select
            If Me.cbxPeriodType.SelectedValue <> "DateInsert" Then
                .DateFrom = period1.DateFrom
                .DateUntil = period1.DateUntil
            End If

            .j70ID = Me.CurrentJ70ID

            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
            If Me.cbxGroupBy.SelectedValue <> "" Then
                Dim strPrimarySortField As String = Me.cbxGroupBy.SelectedValue
                If .MG_SortString = "" Then
                    .MG_SortString = strPrimarySortField
                Else
                    .MG_SortString = strPrimarySortField & "," & .MG_SortString
                End If
            End If
        End With

    End Sub

    Private Sub InhaleMyQuery_o23(ByRef mq As BO.myQueryO23)
        With mq
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_AdditionalSqlFROM = Me.hidAdditionalFrom.Value
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            Select Case Me.CurrentMasterPrefix
                Case "p41" : .p41ID = Me.CurrentMasterPID
                Case "j02" : .j02ID = Me.CurrentMasterPID
                Case "p28" : .p28ID = Me.CurrentMasterPID
                Case "p56" : .p56ID = Me.CurrentMasterPID
            End Select
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
            If Me.cbxGroupBy.SelectedValue <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.cbxGroupBy.SelectedValue
                Else
                    .MG_SortString = Me.cbxGroupBy.SelectedValue & "," & .MG_SortString
                End If
            End If
            Select Case Me.cbxPeriodType.SelectedValue
                Case "DateInsert"
                    .DateInsertFrom = period1.DateFrom : .DateInsertUntil = period1.DateUntil
                Case "o23Date"
                    .DateFrom = period1.DateFrom : .DateUntil = period1.DateUntil
            End Select
            .Closed = BO.BooleanQueryMode.NoQuery
            .j70ID = Me.CurrentJ70ID
            .SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead


        End With

    End Sub

    Private Sub InhaleMyQuery_p56(ByRef mq As BO.myQueryP56)
        With mq
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_AdditionalSqlFROM = Me.hidAdditionalFrom.Value
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            Select Case Me.CurrentMasterPrefix
                Case "p41" : .p41ID = Me.CurrentMasterPID
                Case "j02" : .j02ID = Me.CurrentMasterPID
                Case "p28" : .p28ID = Me.CurrentMasterPID
            End Select
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If

            Select Case Me.cbxPeriodType.SelectedValue
                Case "DateInsert"
                    .DateInsertFrom = period1.DateFrom : .DateInsertUntil = period1.DateUntil
                Case "p56PlanFrom"
                    .p56PlanFrom_D1 = period1.DateFrom : .p56PlanFrom_D2 = period1.DateUntil
                Case "p56PlanUntil"
                    .p56PlanUntil_D1 = period1.DateFrom : .p56PlanUntil_D2 = period1.DateUntil
                Case "p31Date"
                    .p31Date_D1 = period1.DateFrom : .p31Date_D2 = period1.DateUntil
            End Select

            .Closed = BO.BooleanQueryMode.NoQuery
            .j70ID = Me.CurrentJ70ID
            .SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForRead
        End With
    End Sub
    Private Sub InhaleMyQuery_p41(ByRef mq As BO.myQueryP41)
        With mq
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_AdditionalSqlFROM = Me.hidAdditionalFrom.Value
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            Select Case Me.CurrentMasterPrefix
                Case "p28" : .p28ID = Me.CurrentMasterPID
            End Select
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If

            Select Case Me.cbxPeriodType.SelectedValue
                Case "DateInsert"
                    .DateInsertFrom = period1.DateFrom : .DateInsertUntil = period1.DateUntil
                Case "p41PlanFrom"
                    .p41PlanFrom_D1 = period1.DateFrom : .p41PlanFrom_D2 = period1.DateUntil
                Case "p41PlanUntil"
                    .p41PlanUntil_D1 = period1.DateFrom : .p41PlanUntil_D2 = period1.DateUntil
                Case "p31Date"
                    .p31Date_D1 = period1.DateFrom : .p31Date_D2 = period1.DateUntil
            End Select

            .Closed = BO.BooleanQueryMode.NoQuery
            .SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
            .j70ID = Me.CurrentJ70ID
        End With
    End Sub
    Private Sub InhaleMyQuery_p28(ByRef mq As BO.myQueryP28)
        With mq
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_AdditionalSqlFROM = Me.hidAdditionalFrom.Value
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If

            Select Case Me.cbxPeriodType.SelectedValue
                Case "DateInsert"
                    .DateInsertFrom = period1.DateFrom : .DateInsertUntil = period1.DateUntil
                Case "p31Date"
                    .p31Date_D1 = period1.DateFrom : .p31Date_D2 = period1.DateUntil
            End Select
            .Closed = BO.BooleanQueryMode.NoQuery
            .SpecificQuery = BO.myQueryP28_SpecificQuery.AllowedForRead
            .j70ID = Me.CurrentJ70ID


        End With

    End Sub
    Private Sub InhaleMyQuery_j02(ByRef mq As BO.myQueryJ02)
        With mq
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_AdditionalSqlFROM = Me.hidAdditionalFrom.Value
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            Select Case Me.CurrentMasterPrefix
                Case "p41" : .p41ID = Me.CurrentMasterPID
                Case "p28" : .p28ID = Me.CurrentMasterPID
            End Select
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
            If Me.cbxGroupBy.SelectedValue <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.cbxGroupBy.SelectedValue
                Else
                    .MG_SortString = Me.cbxGroupBy.SelectedValue & "," & .MG_SortString
                End If
            End If
            Select Case Me.cbxPeriodType.SelectedValue
                Case "DateInsert"
                    .DateInsertFrom = period1.DateFrom : .DateInsertUntil = period1.DateUntil
                Case "p31Date"
                    .p31Date_D1 = period1.DateFrom : .p31Date_D2 = period1.DateUntil
            End Select
            .Closed = BO.BooleanQueryMode.NoQuery
            .SpecificQuery = BO.myQueryJ02_SpecificQuery.AllowedForRead

            Select Case Me.cbxQueryFlag.SelectedValue
                Case "1" : .IntraPersons = BO.myQueryJ02_IntraPersons.IntraOnly
                Case "2" : .IntraPersons = BO.myQueryJ02_IntraPersons.NonIntraOnly
                Case Else : .IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified
            End Select
            .j70ID = Me.CurrentJ70ID
        End With
    End Sub
    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-pagesize", Me.cbxPaging.SelectedValue)
        ReloadPage()

    End Sub



    

    Private Sub Handle_DefaultSelectedRecord()
        Me.contentPane.ContentUrl = Me.CurrentPrefix + "_framework_detail.aspx"

        Dim intSelPID As Integer = 0
        If Not Page.IsPostBack Then
            Dim strDefPID As String = Request.Item("pid")
            If strDefPID = "" Then
                strDefPID = Master.Factory.j03UserBL.GetUserParam(Me.CurrentPrefix + "_framework_detail-pid")
            End If
            If strDefPID > "" Then intSelPID = BO.BAS.IsNullInt(strDefPID)
        End If

        If intSelPID > 0 Then
            'označit výchozí záznam
            grid1.SelectRecords(intSelPID)
            If grid1.GetSelectedPIDs.Count = 0 Then
                Dim dt As DataTable = Nothing
                Select Case Me.CurrentX29ID
                    Case BO.x29IdEnum.p41Project
                        Dim mq As New BO.myQueryP41
                        InhaleMyQuery_p41(mq)
                        mq.MG_SelectPidFieldOnly = True
                        mq.TopRecordsOnly = 0
                        dt = Master.Factory.p41ProjectBL.GetGridDataSource(mq)
                        If dt Is Nothing Then
                            Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
                            Return
                        End If
                    Case BO.x29IdEnum.p28Contact
                        Dim mq As New BO.myQueryP28
                        InhaleMyQuery_p28(mq)
                        mq.MG_SelectPidFieldOnly = True
                        mq.TopRecordsOnly = 0
                        dt = Master.Factory.p28ContactBL.GetGridDataSource(mq)
                        If dt Is Nothing Then
                            Master.Notify(Master.Factory.p28ContactBL.ErrorMessage, NotifyLevel.ErrorMessage)
                            Return
                        End If
                    Case BO.x29IdEnum.p56Task
                        Dim mq As New BO.myQueryP56
                        InhaleMyQuery_p56(mq)
                        mq.MG_SelectPidFieldOnly = True
                        mq.TopRecordsOnly = 0
                        dt = Master.Factory.p56TaskBL.GetGridDataSource(mq)

                        If dt Is Nothing Then
                            Master.Notify(Master.Factory.p56TaskBL.ErrorMessage, NotifyLevel.ErrorMessage)
                            Return
                        End If
                    Case BO.x29IdEnum.o23Notepad
                        Dim mq As New BO.myQueryO23
                        InhaleMyQuery_o23(mq)
                        mq.MG_SelectPidFieldOnly = True
                        mq.TopRecordsOnly = 0

                        dt = Master.Factory.o23NotepadBL.GetGridDataSource(mq)
                        If dt Is Nothing Then
                            Master.Notify(Master.Factory.o23NotepadBL.ErrorMessage, NotifyLevel.ErrorMessage)
                            Return
                        End If
                    Case BO.x29IdEnum.j02Person
                        Dim mq As New BO.myQueryJ02
                        InhaleMyQuery_j02(mq)
                        mq.MG_SelectPidFieldOnly = True
                        mq.TopRecordsOnly = 0
                        dt = Master.Factory.j02PersonBL.GetGridDataSource(mq)
                        If dt Is Nothing Then
                            Master.Notify(Master.Factory.j02PersonBL.ErrorMessage, NotifyLevel.ErrorMessage)
                            Return
                        End If
                    Case BO.x29IdEnum.p91Invoice
                        Dim mq As New BO.myQueryP91
                        InhaleMyQuery_p91(mq)
                        ''mq.MG_SelectPidFieldOnly = True
                        mq.TopRecordsOnly = 0
                        mq.MG_SelectPidFieldOnly = True
                        dt = Master.Factory.p91InvoiceBL.GetGridDataSource(mq)
                        If dt Is Nothing Then
                            Master.Notify(Master.Factory.p91InvoiceBL.ErrorMessage, NotifyLevel.ErrorMessage)
                            Return
                        End If
                End Select

                Dim x As Integer, intNewPageIndex As Integer = 0
                For Each dbRow As DataRow In dt.Rows
                    x += 1
                    If x > grid1.PageSize Then
                        intNewPageIndex += 1 : x = 1
                    End If
                    If dbRow.Item("pid") = intSelPID Then
                        grid1.radGridOrig.CurrentPageIndex = intNewPageIndex
                        grid1.Rebind(False)
                        grid1.SelectRecords(intSelPID)
                        Exit For
                    End If
                Next
            End If
            
            Me.contentPane.ContentUrl = Me.CurrentPrefix + "_framework_detail.aspx?pid=" & intSelPID.ToString   'v detailu ho vybereme nezávisle na tom, zda byl nalezen v gridu
        End If
        If Request.Item("force") <> "" Then
            Me.contentPane.ContentUrl += "&force=" & Request.Item("force")
        End If
    End Sub



    Private Sub SetupJ74Combo(intDef As Integer)
        If Not _curJ62 Is Nothing Then
            If _curJ62.j74ID <> 0 Then
                Me.j74id.Items.Add(New ListItem("", _curJ62.j74ID.ToString)) : Me.j74id.Visible = False : cmdGridDesiger.Visible = False
                _curJ74 = Master.Factory.j74SavedGridColTemplateBL.Load(_curJ62.j74ID)
                If _curJ62.j62GridGroupBy <> "" Then basUI.SelectDropdownlistValue(Me.cbxGroupBy, _curJ62.j62GridGroupBy)
                Return
            End If
        End If
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j74MasterPrefix = "")
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j74MasterPrefix = "")
        End If
        j74id.DataSource = lisJ74
        j74id.DataBind()

        If intDef > 0 Then
            basUI.SelectDropdownlistValue(Me.j74id, intDef.ToString)
        End If
        If Me.CurrentJ74ID > 0 Then
            _curJ74 = lisJ74.Where(Function(p) p.PID = Me.CurrentJ74ID)(0)
        End If


    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
            .GroupsDefaultExpanded = chkGroupsAutoExpanded.Checked
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strFieldHeader

            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)
        End With
    End Sub
    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-groupby", Me.cbxGroupBy.SelectedValue)
        ReloadPage()
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)
    End Sub
    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        SaveLastJ74Reference()
        ReloadPage()
    End Sub

    Private Sub SaveLastJ74Reference()
        With Master.Factory.j03UserBL
            .SetUserParam(Me.CurrentPrefix + "_framework-j74id", Me.CurrentJ74ID.ToString)
            .SetUserParam(Me.CurrentPrefix + "_framework-sort", "")
            .SetUserParam(Me.CurrentPrefix + "_framework-filter_setting", "")
            .SetUserParam(Me.CurrentPrefix + "_framework-filter_sql", "")
        End With
        
    End Sub
    Private Sub ReloadPage()
        Dim s As String = "entity_framework.aspx?prefix=" & Me.CurrentPrefix
        If Me.CurrentJ62ID > 0 Then s += "&j62id=" & Me.CurrentJ62ID.ToString
        If Me.CurrentMasterPID > 0 Then s += "&masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString
        Response.Redirect(s, True)
    End Sub

    Private Sub RecalcVirtualRowCount()
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim mq As New BO.myQueryP41
                InhaleMyQuery_p41(mq)
                grid1.VirtualRowCount = Master.Factory.p41ProjectBL.GetVirtualCount(mq)
            Case BO.x29IdEnum.p28Contact
                Dim mq As New BO.myQueryP28
                InhaleMyQuery_p28(mq)
                grid1.VirtualRowCount = Master.Factory.p28ContactBL.GetVirtualCount(mq)
            Case BO.x29IdEnum.p56Task
                Dim mq As New BO.myQueryP56
                InhaleMyQuery_p56(mq)
                grid1.VirtualRowCount = Master.Factory.p56TaskBL.GetVirtualCount(mq)
            Case BO.x29IdEnum.o23Notepad
                Dim mq As New BO.myQueryO23
                InhaleMyQuery_o23(mq)
                grid1.VirtualRowCount = Master.Factory.o23NotepadBL.GetVirtualCount(mq)
            Case BO.x29IdEnum.p91Invoice
                Dim mq As New BO.myQueryP91
                InhaleMyQuery_p91(mq)
                Dim cSum As BO.p91InvoiceSum = Master.Factory.p91InvoiceBL.GetSumRow(mq)
                grid1.VirtualRowCount = cSum.Count
                Me.hidFooterSum.Value = grid1.GenerateFooterItemString(cSum)
            Case BO.x29IdEnum.j02Person
                Dim mq As New BO.myQueryJ02
                InhaleMyQuery_j02(mq)
                grid1.VirtualRowCount = Master.Factory.j02PersonBL.GetList(mq).Count
        End Select
        

        grid1.radGridOrig.CurrentPageIndex = 0

    End Sub

    Private Sub grid1_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        If hidFooterSum.Value = "" Then Return
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"
        grid1.ParseFooterItemString(footerItem, hidFooterSum.Value)
    End Sub

    

   

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "-j70id", Me.CurrentJ70ID.ToString)
        ReloadPage()
    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        Dim cXLS As New clsExportToXls(Master.Factory)
        ''Dim lis As IEnumerable(Of Object) = Nothing
        Dim dt As DataTable = Nothing

        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim mq As New BO.myQueryP41
                InhaleMyQuery_p41(mq)
                mq.MG_GridGroupByField = ""
                ''lis = Master.Factory.p41ProjectBL.GetList(mq)
                dt = Master.Factory.p41ProjectBL.GetGridDataSource(mq)
            Case BO.x29IdEnum.p28Contact
                Dim mq As New BO.myQueryP28
                InhaleMyQuery_p28(mq)
                mq.MG_GridGroupByField = ""
                ''lis = Master.Factory.p28ContactBL.GetList(mq)
                dt = Master.Factory.p28ContactBL.GetGridDataSource(mq)
            Case BO.x29IdEnum.p56Task
                Dim mq As New BO.myQueryP56
                InhaleMyQuery_p56(mq)
                mq.MG_GridGroupByField = ""
                ''lis = Master.Factory.p56TaskBL.GetList(mq)
                dt = Master.Factory.p56TaskBL.GetGridDataSource(mq)
            Case BO.x29IdEnum.o23Notepad
                Dim mq As New BO.myQueryO23
                InhaleMyQuery_o23(mq)
                mq.MG_GridGroupByField = ""
                ''lis = Master.Factory.o23NotepadBL.GetList4Grid(mq)
                dt = Master.Factory.o23NotepadBL.GetGridDataSource(mq)
            Case BO.x29IdEnum.j02Person
                Dim mq As New BO.myQueryJ02
                InhaleMyQuery_j02(mq)
                mq.MG_GridGroupByField = ""
                ''lis = Master.Factory.j02PersonBL.GetList(mq)
                ''dt = Master.Factory.j02PersonBL.GetGridDataSource(Me.hidCols.Value, mq, "")
            Case BO.x29IdEnum.p91Invoice
                Dim mq As New BO.myQueryP91
                InhaleMyQuery_p91(mq)
                mq.MG_GridGroupByField = ""
                ''lis = Master.Factory.p91InvoiceBL.GetList(mq)
                dt = Master.Factory.p91InvoiceBL.GetGridDataSource(mq)

        End Select

        Dim strFileName As String = cXLS.ExportGridData(dt.AsEnumerable, cJ74)
        If strFileName = "" Then
            Master.Notify(cXLS.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            Response.Redirect("binaryfile.aspx?tempfile=" & strFileName)
        End If
    End Sub


    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)
    End Sub

    Private Sub chkCheckboxSelector_CheckedChanged(sender As Object, e As EventArgs) Handles chkCheckboxSelector.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-checkbox_selector", BO.BAS.GB(Me.chkCheckboxSelector.Checked))
        ReloadPage()
    End Sub

    Private Sub cbxPeriodType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPeriodType.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-periodtype", Me.cbxPeriodType.SelectedValue)
        RecalcVirtualRowCount()
        grid1.Rebind(False)
        hidUIFlag.Value = "period"
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-period", Me.period1.SelectedValue)
        RecalcVirtualRowCount()
        grid1.Rebind(False)
        hidUIFlag.Value = "period"
    End Sub

    Private Sub cbxQueryFlag_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxQueryFlag.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-queryflag", Me.cbxQueryFlag.SelectedValue)
        RecalcVirtualRowCount()
        grid1.Rebind(False)
    End Sub

    Private Sub entity_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        basUIMT.RenderQueryCombo(Me.j70ID)
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
                If Year(.DateFrom) = Year(.DateUntil) Then
                    Me.CurrentPeriodQuery.Text = Format(.DateFrom, "d.M") & "-" & Format(.DateUntil, "d.M.yyyy")
                Else
                    Me.CurrentPeriodQuery.Text = Format(.DateFrom, "d.M.yy") & "-" & Format(.DateUntil, "d.M.yyyy")
                End If
                Me.CurrentPeriodQuery.ToolTip = Me.cbxPeriodType.SelectedItem.Text & Me.CurrentPeriodQuery.Text
                Me.CurrentPeriodQuery.ForeColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
                Me.CurrentPeriodQuery.Text = "Filtr období"
                Me.CurrentPeriodQuery.ToolTip = ""
                Me.CurrentPeriodQuery.ForeColor = Nothing
            End If
        End With
        If Me.cbxGroupBy.SelectedIndex > 0 Then
            chkGroupsAutoExpanded.Visible = True
        Else
            chkGroupsAutoExpanded.Visible = False
        End If
        If grid1.GetFilterExpression <> "" Then
            cmdCĺearFilter.Visible = True
        Else
            cmdCĺearFilter.Visible = False
        End If
    End Sub

    Private Sub cmdCĺearFilter_Click(sender As Object, e As EventArgs) Handles cmdCĺearFilter.Click
        With Master.Factory.j03UserBL
            .SetUserParam(Me.CurrentPrefix + "_framework-filter_setting", "")
            .SetUserParam(Me.CurrentPrefix + "_framework-filter_sql", "")
        End With
        ReloadPage()
    End Sub

    Private Sub grid1_SortCommand(SortExpression As String, strOwnerTableName As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentPrefix + "_framework-sort", SortExpression)
    End Sub
End Class