Imports Telerik.Web.UI
Public Class x25_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Public Property _curIsExport As Boolean
    Private Property _needFilterIsChanged As Boolean = False


    Public ReadOnly Property CurrentX18ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.x18ID.SelectedValue)
        End Get
    End Property
    Public ReadOnly Property CurrentX23ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidX23ID.Value)
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

    Private Sub x25_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Item("masterpid") <> "" Then
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid")) : Me.CurrentMasterPrefix = Request.Item("masterprefix")
            End If
            Dim strX18ID As String = Request.Item("x18id")
            With Master.Factory.j03UserBL
                If strX18ID = "" Then
                    .InhaleUserParams("x25_framework-x18id")
                    strX18ID = .GetUserParam("x25_framework-x18id")
                End If
            End With
            SetupX18Combo(strX18ID)
            If Me.x18ID.Items.Count > 0 Then
                strX18ID = Me.x18ID.SelectedValue
                Handle_ChangeX18ID()
            Else
                strX18ID = ""
            End If

            Handle_Permissions()
            SetupPeriodQuery()
            With Master
                .SiteMenuValue = "x25_framework"
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("x25_framework-pagesize")
                    .Add("x25_framework-navigationPane_width-" & strX18ID)
                    .Add("x25_framework-x18id")
                    .Add("x25_framework-x23id")
                    .Add("x25_framework_detail-pid-" & strX18ID)

                    .Add("x25_framework-sort-" & strX18ID)
                    .Add("periodcombo-custom_query")
                    .Add("x25_framework-periodtype-" & strX18ID)
                    .Add("x25_framework-period-" & strX18ID)
                    .Add("x25_framework-filter_setting-" & strX18ID)
                    .Add("x25_framework-filter_sql-" & strX18ID)
                    .Add("x25_framework-filter_validity-" & strX18ID)
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                    basUI.SelectDropdownlistValue(Me.cbxX25Validity, .GetUserParam("x25_framework-filter_validity-" & strX18ID, "1"))
                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("x25_framework-pagesize", "20"))
                    Dim strDefWidth As String = "800"
                    Dim strW As String = .GetUserParam("x25_framework-navigationPane_width-" & strX18ID, strDefWidth)
                    If strW = "-1" Then
                        Me.navigationPane.Collapsed = True
                    Else
                        Me.navigationPane.Width = Unit.Parse(strW & "px")
                    End If

                    If .GetUserParam("x25_framework-sort-" & strX18ID) <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam("x25_framework-sort-" & strX18ID))
                    End If
                    basUI.SelectDropdownlistValue(Me.cbxPeriodType, .GetUserParam("x25_framework-periodtype-" & strX18ID, ""))
                    If Me.cbxPeriodType.SelectedIndex > 0 Then
                        period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                        period1.SelectedValue = .GetUserParam("x25_framework-period-" & strX18ID)
                    End If
                End With
            End With



            With Master.Factory.j03UserBL


                SetupGrid(.GetUserParam("x25_framework-filter_setting-" & strX18ID), .GetUserParam("x25_framework-filter_sql-" & strX18ID))
            End With
            RecalcVirtualRowCount()

            Handle_DefaultSelectedRecord()

        End If
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        Dim lisSqlSEL As New List(Of String)
        With grid1
            .ClearColumns()
            .radGridOrig.ShowFooter = False
            .AllowMultiSelect = False
            .DataKeyNames = "pid"
            .AllowCustomSorting = True

            .AllowCustomPaging = True
            '.AddCheckboxSelector()
            If hidB01ID.Value <> "" Or hidx18IsColors.Value = "1" Then
                .AddSystemColumn(20)
            End If

            .PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)

            .radGridOrig.PagerStyle.Mode = Telerik.Web.UI.GridPagerMode.NextPrevAndNumeric
            .AllowFilteringByColumn = True

            .radGridOrig.ClientSettings.Scrolling.AllowScroll = True
            .radGridOrig.ClientSettings.Scrolling.UseStaticHeaders = True

            .radGridOrig.MasterTableView.Name = "grid"

            Dim lisX20X18 As IEnumerable(Of BO.x20_join_x18) = Master.Factory.x18EntityCategoryBL.GetList_x20_join_x18(Me.CurrentX18ID)
            lisX20X18 = lisX20X18.Where(Function(p) p.x20IsClosed = False And (p.x20GridColumnFlag = BO.x20GridColumnENUM.CategoryColumn Or p.x20GridColumnFlag = BO.x20GridColumnENUM.Both)).OrderBy(Function(p) p.x20IsMultiSelect).ThenBy(Function(p) p.x29ID)   'omezit pouze na otevřené vazby + vazby vyplňované přes záznam položky štítku
            For Each c In lisX20X18
                .AddColumn("Entita" & c.x20ID.ToString, c.BindName, BO.cfENUM.AnyString, True, , "dbo.stitek_entity(a.x25ID," & c.x20ID.ToString & ")", , False, True)
                lisSqlSEL.Add("dbo.stitek_entity(a.x25ID," & c.x20ID.ToString & ") as Entita" & c.x20ID.ToString)
            Next
            If hidB01ID.Value <> "" Then
                .AddColumn("b02Name", "Stav", BO.cfENUM.AnyString, True, , "b02Name", , False, True)
            End If
            Dim lisX16 As IEnumerable(Of BO.x16EntityCategory_FieldSetting) = Master.Factory.x18EntityCategoryBL.GetList_x16(Me.CurrentX18ID).Where(Function(p) p.x16IsGridField = True)
            If lisX16.Count = 0 Then
                .AddColumn("x25Name", "Název", BO.cfENUM.AnyString, True, , "x25Name", , False, True)
                .AddColumn("x25Code", "Kód", BO.cfENUM.AnyString, True, , "x25Code", , False, True)
                .AddColumn("x25Ordinary", "#", BO.cfENUM.Numeric0, True, , "x25Ordinary", , False, False)
            Else
                If hidx18GridColsFlag.Value = "1" Or hidx18GridColsFlag.Value = "3" Then
                    .AddColumn("x25Name", "Název", BO.cfENUM.AnyString, True, , "x25Name", , False, True)
                End If
                If hidx18GridColsFlag.Value = "1" Or hidx18GridColsFlag.Value = "2" Then
                    .AddColumn("x25Code", "Kód", BO.cfENUM.AnyString, True, , "x25Code", , False, True)
                End If
                For Each c In lisX16
                    Dim strH As String = c.x16NameGrid
                    If strH = "" Then strH = c.x16Name
                    .AddColumn(c.x16Field, strH, c.GridColumnType, True, , c.x16Field, , False, True)
                    lisSqlSEL.Add(c.x16Field)

                    If c.FieldType = BO.x24IdENUM.tDate Or c.FieldType = BO.x24IdENUM.tDateTime Then
                        Me.cbxPeriodType.Items.Add(New ListItem(c.x16Name, c.x16Field))
                    End If
                Next
            End If


            .SetFilterSetting(strFilterSetting, strFilterExpression)
        End With
        hidCols.Value = String.Join(",", lisSqlSEL)
    End Sub

    Private Sub SetupX18Combo(strDef As String)
        Dim mq As New BO.myQuery
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_X18_Admin, BO.x53PermValEnum.GR_Admin) Then
            mq.MyRecordsDisponible = True
        End If

        Dim lis As IEnumerable(Of BO.x18EntityCategory) = Master.Factory.x18EntityCategoryBL.GetList(mq)
        Me.x18ID.DataSource = lis
        Me.x18ID.DataBind()
        If lis.Count = 0 Then
            Master.Notify("V databázi zatím neexistuje štítek.", NotifyLevel.InfoMessage)
        Else
            If strDef <> "" Then basUI.SelectDropdownlistValue(Me.x18ID, strDef)
        End If

    End Sub

    Private Sub SetupPeriodQuery()
        With Me.cbxPeriodType.Items
            If .Count > 0 Then .Clear()
            .Add(New ListItem("--Filtrovat období--", ""))
            .Add(New ListItem("Založení záznamu", "DateInsert"))

        End With

    End Sub
    Private Sub Handle_Permissions()
        cmdSetting.Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_X18_Admin)
        cmdAdmin.Visible = cmdSetting.Visible
    End Sub

    Private Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryX25(0)
        InhaleMyQuery(mq)
        grid1.VirtualRowCount = Master.Factory.x25EntityField_ComboValueBL.GetVirtualCount(mq)



        grid1.radGridOrig.CurrentPageIndex = 0

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryX25)
        With mq
            .x23ID = Me.CurrentX23ID

            .MyRecordsDisponible = True
            .MG_GridSqlColumns = Me.hidCols.Value
            .ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            If Me.CurrentMasterPrefix <> "" Then
                .Record_x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
                .RecordPID = Me.CurrentMasterPID
            End If

            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
            With Me.cbxPeriodType
                If .SelectedValue <> "" Then
                    Select Case .SelectedValue
                        Case "DateInsert"
                            mq.DateInsertFrom = period1.DateFrom : mq.DateInsertUntil = period1.DateUntil
                        Case Else
                            mq.DateFrom = period1.DateFrom
                            mq.DateUntil = period1.DateUntil
                            mq.DateQueryFieldBy = .SelectedValue
                    End Select
                End If

            End With
            Select Case Me.cbxX25Validity.SelectedValue
                Case "1" : .Closed = BO.BooleanQueryMode.NoQuery
                Case "2" : .Closed = BO.BooleanQueryMode.FalseQuery
                Case "3" : .Closed = BO.BooleanQueryMode.TrueQuery
            End Select



        End With

    End Sub

    Private Sub Handle_DefaultSelectedRecord()
        Me.hidContentPaneDefUrl.Value = "x25_framework_detail.aspx"
        Dim intSelPID As Integer = 0
        If Not Page.IsPostBack Then
            Dim strDefPID As String = Request.Item("pid")
            If strDefPID = "" Then strDefPID = Master.Factory.j03UserBL.GetUserParam("x25_framework_detail-pid")
            If strDefPID <> "" Then intSelPID = BO.BAS.IsNullInt(strDefPID)
        End If

        If intSelPID > 0 Then
            'označit výchozí záznam
            grid1.SelectRecords(intSelPID)
            If grid1.GetSelectedPIDs.Count = 0 Then

                Dim mq As New BO.myQueryX25(Me.CurrentX23ID)
                InhaleMyQuery(mq)
                mq.MG_SelectPidFieldOnly = True
                mq.TopRecordsOnly = 0
                Dim dt As DataTable = Master.Factory.x25EntityField_ComboValueBL.GetDataTable4Grid(mq)
                If dt Is Nothing Then
                    Master.Notify(Master.Factory.x25EntityField_ComboValueBL.ErrorMessage, NotifyLevel.ErrorMessage)
                    Return
                End If
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

            Me.hidContentPaneDefUrl.Value = "x25_framework_detail.aspx?pid=" & intSelPID.ToString
        End If
    End Sub

    Private Sub ReloadPage()
        Dim s As String = "x25_framework.aspx?x18id=" & Me.CurrentX18ID
        If Me.CurrentMasterPID > 0 Then s += "&masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString
        Response.Redirect(s, True)
    End Sub

   

    Private Sub Handle_ChangeX18ID()
        Dim c As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Me.CurrentX18ID)
        hidX23ID.Value = c.x23ID.ToString
        hidx18GridColsFlag.Value = CInt(c.x18GridColsFlag).ToString
        hidx18IsColors.Value = BO.BAS.GB(c.x18IsColors)
        If c.b01ID <> 0 Then
            hidB01ID.Value = c.b01ID.ToString
        Else
            hidB01ID.Value = ""
        End If


        Dim cDisp As BO.x18RecordDisposition = Master.Factory.x18EntityCategoryBL.InhaleDisposition(c)
        menu1.FindItemByValue("cmdNew").Visible = cDisp.CreateItem

    End Sub

    Private Sub x25_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
       
        If cbxPeriodType.SelectedIndex > 0 Then
            With Me.period1
                .Visible = True
                If .SelectedValue <> "" Then
                    .BackColor = basUI.ColorQueryRGB
                    Me.CurrentPeriodQuery.Text = "<img src='Images/datepicker.png'/> " & Me.cbxPeriodType.SelectedItem.Text
                    If Year(.DateFrom) = Year(.DateUntil) Then
                        Me.CurrentPeriodQuery.Text += " " & Format(.DateFrom, "d.M") & "-" & Format(.DateUntil, "d.M.yyyy")
                    Else
                        Me.CurrentPeriodQuery.Text += " " & Format(.DateFrom, "d.M.yy") & "-" & Format(.DateUntil, "d.M.yyyy")
                    End If

                Else
                    .BackColor = Nothing
                    Me.CurrentPeriodQuery.Text = ""

                End If
            End With
        Else
            period1.Visible = False
        End If
        If grid1.GetFilterExpression <> "" Then
            cmdCĺearFilter.Visible = True
        Else
            cmdCĺearFilter.Visible = False
        End If
        Me.CurrentQuery.Text = ""
        If Me.cbxX25Validity.SelectedIndex > 0 Then
            Me.CurrentQuery.Text += "<img src='Images/query.png' style='margin-left:20px;'/>" & Me.cbxX25Validity.SelectedItem.Text
        End If
    End Sub

    Private Sub GridExport(strFormat As String)
        _curIsExport = True
        basUIMT.Handle_GridTelerikExport(Me.grid1, strFormat)


    End Sub

    Private Sub cmdDOC_Click(sender As Object, e As EventArgs) Handles cmdDOC.Click
        GridExport("doc")
    End Sub

    Private Sub cmdPDF_Click(sender As Object, e As EventArgs) Handles cmdPDF.Click
        GridExport("pdf")
    End Sub

    Private Sub cmdXLS_Click(sender As Object, e As EventArgs) Handles cmdXLS.Click
        GridExport("xls")
    End Sub
    Private Sub cmdCĺearFilter_Click(sender As Object, e As EventArgs) Handles cmdCĺearFilter.Click
        With Master.Factory.j03UserBL
            .SetUserParam("x25_framework-filter_setting-" & Me.CurrentX18ID.ToString, "")
            .SetUserParam("x25_framework-filter_sql-" & Me.CurrentX18ID.ToString, "")
        End With
        ReloadPage()
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As System.Data.DataRowView = CType(e.Item.DataItem, System.Data.DataRowView)
        If cRec.Item("IsClosed") Then dataItem.Font.Strikeout = True
        If hidx18IsColors.Value = "1" And hidB01ID.Value = "" Then
            If Not cRec.Item("x25BackColor") Is System.DBNull.Value Then
                dataItem.Item("systemcolumn").Style.Item("background-color") = cRec.Item("x25BackColor")
            End If
            If Not cRec.Item("x25ForeColor") Is System.DBNull.Value Then
                dataItem.Item("systemcolumn").Style.Item("color") = cRec.Item("x25ForeColor")
            End If
        End If
        If hidB01ID.Value <> "" Then
            If Not cRec.Item("b02Color") Is System.DBNull.Value Then
                dataItem.Item("systemcolumn").Style.Item("background-color") = cRec.Item("b02Color")
            End If
        End If
        
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("x25_framework-filter_setting-" & Me.CurrentX18ID.ToString, grid1.GetFilterSetting())
                .SetUserParam("x25_framework-filter_sql-" & Me.CurrentX18ID.ToString, grid1.GetFilterExpression())
            End With
            RecalcVirtualRowCount()
        End If
        Dim mq As New BO.myQueryX25(Me.CurrentX23ID)
        With mq
            .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
        End With
        InhaleMyQuery(mq)

        If _curIsExport Then mq.MG_PageSize = 2000
        Dim dt As DataTable = Master.Factory.x25EntityField_ComboValueBL.GetDataTable4Grid(mq)
        If dt Is Nothing Then
            Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            grid1.DataSourceDataTable = dt
        End If
    End Sub

    Private Sub grid1_SortCommand(SortExpression As String, strOwnerTableName As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam("x25_framework-sort-" & Me.CurrentX18ID.ToString, SortExpression)
    End Sub
    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
        ''_CurFilterDbField = strFilterColumn
    End Sub
    Private Sub cbxPeriodType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPeriodType.SelectedIndexChanged
        With Master.Factory.j03UserBL
            If Me.cbxPeriodType.SelectedIndex > 0 And Not period1.Visible Then
                .InhaleUserParams("periodcombo-custom_query", "x25_framework-period-" & Me.CurrentX18ID.ToString)
                period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("x25_framework-period-" & Me.CurrentX18ID.ToString)
            End If

            .SetUserParam("x25_framework-periodtype-" & Me.CurrentX18ID.ToString, Me.cbxPeriodType.SelectedValue)
        End With


        RecalcVirtualRowCount()
        grid1.Rebind(False)
        hidUIFlag.Value = "period"
    End Sub

    Private Sub cbxX25Validity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxX25Validity.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("x25_framework-filter_validity-" & Me.CurrentX18ID.ToString, Me.cbxX25Validity.SelectedValue)
        ReloadPage()
    End Sub
End Class