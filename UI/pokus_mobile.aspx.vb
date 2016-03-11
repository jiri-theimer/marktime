Imports Telerik.Web.UI

Public Class pokus_mobile
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile

    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private _curLisX25 As IEnumerable(Of BO.x25EntityField_ComboValue)
    Private _freeComboCols As New List(Of GridBoundColumn)

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
    Private Sub p28_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p28_framework"
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                If Not .Factory.SysUser.j04IsMenu_Contact Then .StopPage("Nedisponujete oprávněním k zobrazení stránky [Klienti].")

                If Request.Item("title") = "" Then
                    .PageTitle = "Klienti"
                Else
                    .PageTitle = Request.Item("title")
                End If
                .SiteMenuValue = "p28"
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p28_framework-pagesize")
                    '.Add("p28_framework-search")
                    .Add("p28_framework-navigationPane_width")
                    .Add("p28_framework_detail-pid")

                    '.Add("p28_framework-quickquery")
                    '.Add("p28_framework-quickquery-bin")
                    .Add("p28_framework-j74id")
                    .Add("p28_framework-groupby")
                    .Add("p28_framework-sort")
                    .Add("p28-j70id")
                    .Add("p28_framework-groups-autoexpanded")
                    .Add("p28_framework-checkbox_selector")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)


                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("p28_framework-pagesize", "20"))
                    Me.navigationPane.Width = Unit.Parse(.GetUserParam("p28_framework-navigationPane_width", "350") & "px")
                    'Me.hidQuickQuery.Value = .GetUserParam("p28_framework-quickquery", "0")
                    'Me.hidQuickQueryBin.Value = .GetUserParam("p28_framework-quickquery-bin", "0")
                    'Me.clue_quickquery.Attributes("rel") = "clue_quickquery.aspx?prefix=p28&def=" & Me.hidQuickQuery.Value & "&bin=" & hidQuickQueryBin.Value
                    basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("p28_framework-groupby"))
                    Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam("p28_framework-groups-autoexpanded", "0"))
                    If .GetUserParam("p28_framework-sort") <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam("p28_framework-sort"))
                    End If
                    Me.chkCheckboxSelector.Checked = BO.BAS.BG(.GetUserParam("p28_framework-checkbox_selector", "0"))
                End With

            End With

            'If Request.Item("search") <> "" Then
            '    txtSearch.Text = Request.Item("search")   'externě předaná podmínka
            '    txtSearch.Focus()
            'End If
            If Request.Item("oauth_token") <> "" Then
                ''basUIMT.Handle_SaveDropboxAccessToken(Master)
            End If

            SetupJ70Combo(BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam("p28-j70id")))
            RecalcVirtualRowCount()
            SetupJ74Combo(BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam("p28_framework-j74id")))
            SetupGrid()

            Handle_DefaultSelectedRecord()
        End If
    End Sub
    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p28Contact)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Bez filtrování--")
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
    Private Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryP28
        InhaleMyQuery(mq)
        grid1.VirtualRowCount = Master.Factory.p28ContactBL.GetVirtualCount(mq)

        grid1.radGridOrig.CurrentPageIndex = 0
        'Me.lblFormHeader.Text = "Worksheet (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
    End Sub


    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.x29IdEnum.p28Contact, Master.Factory.SysUser.PID)
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo(cJ74.PID)
                End If
            End If
            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, True, Me.chkCheckboxSelector.Checked)
        End With
        With grid1
            .radGridOrig.ShowFooter = False
            .radGridOrig.SelectedItemStyle.BackColor = Drawing.Color.Red
        End With
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim cRec As BO.p28Contact = CType(e.Item.DataItem, BO.p28Contact)
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

        For Each col In _freeComboCols
            Dim o As Object = BO.BAS.GetPropertyValue(cRec, col.DataField)
            Dim intX25ID As Integer = BO.BAS.IsNullInt(o)
            If intX25ID > 0 Then
                If _curLisX25.Where(Function(p) p.PID = intX25ID).Count > 0 Then
                    dataItem(col).Text = _curLisX25.Where(Function(p) p.PID = intX25ID)(0).x25Name
                End If
            End If
        Next

        If cRec.IsClosed Then dataItem.Font.Strikeout = True

        If cRec.p28CompanyShortName > "" Then dataItem.ToolTip = cRec.p28CompanyName
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryP28
        With mq
            .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex

        End With
        InhaleMyQuery(mq)

        Dim lis As IEnumerable(Of BO.p28Contact) = Master.Factory.p28ContactBL.GetList(mq)
        If lis Is Nothing Then
            Master.Notify(Master.Factory.p28ContactBL.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            For i As Integer = 0 To grid1.radGridOrig.Columns.Count - 1
                If TypeOf grid1.radGridOrig.Columns(i) Is GridBoundColumn Then
                    Dim col As GridBoundColumn = grid1.radGridOrig.Columns(i)
                    If Mid(col.DataField, 4, 9) = "FreeCombo" Then
                        _freeComboCols.Add(col)
                    End If
                End If
            Next
            If _freeComboCols.Count > 0 Then
                _curLisX25 = Master.Factory.x25EntityField_ComboValueBL.GetList(0)
            End If

            grid1.DataSource = lis
        End If




    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP28)
        With mq
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.cbxGroupBy.SelectedValue <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.cbxGroupBy.SelectedValue
                Else
                    .MG_SortString = Me.cbxGroupBy.SelectedValue & "," & .MG_SortString
                End If
            End If

            .Closed = BO.BooleanQueryMode.NoQuery
            .SpecificQuery = BO.myQueryP28_SpecificQuery.AllowedForRead
            .j70ID = Me.CurrentJ70ID
            '.SearchExpression = Trim(Me.txtSearch.Text)

            '.QuickQuery = Me.CurrentQuickQuery
            'Select Case Me.hidQuickQueryBin.Value
            '    Case "1"
            '        .Closed = BO.BooleanQueryMode.FalseQuery
            '    Case "2"
            '        .Closed = BO.BooleanQueryMode.TrueQuery
            'End Select

        End With

    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework-pagesize", Me.cbxPaging.SelectedValue)

        ReloadPage()
    End Sub



    Private Sub Handle_DefaultSelectedRecord()
        Me.contentPane.ContentUrl = "p28_framework_detail.aspx"
        Dim intSelPID As Integer = 0
        If Not Page.IsPostBack Then
            Dim strDefPID As String = Request.Item("pid")
            If strDefPID = "" Then strDefPID = Master.Factory.j03UserBL.GetUserParam("p28_framework_detail-pid")
            If strDefPID > "" Then intSelPID = BO.BAS.IsNullInt(strDefPID)
        End If

        If intSelPID > 0 Then
            'označit výchozí záznam
            grid1.SelectRecords(intSelPID)
            If grid1.GetSelectedPIDs.Count = 0 Then
                'hledaný záznam nebyl nalezen na první stránce
                Dim mq As New BO.myQueryP28
                InhaleMyQuery(mq)
                mq.MG_SelectPidFieldOnly = True
                mq.TopRecordsOnly = 0
                Dim lis As IEnumerable(Of BO.p28Contact) = Master.Factory.p28ContactBL.GetList(mq), x As Integer, intNewPageIndex As Integer = 0
                If lis Is Nothing Then
                    Master.Notify(Master.Factory.p28ContactBL.ErrorMessage, NotifyLevel.ErrorMessage)
                    Return
                End If
                For Each c In lis
                    x += 1
                    If x > grid1.PageSize Then
                        intNewPageIndex += 1 : x = 1
                    End If
                    If c.PID = intSelPID Then
                        grid1.radGridOrig.CurrentPageIndex = intNewPageIndex
                        grid1.Rebind(False)
                        grid1.SelectRecords(intSelPID)
                        Exit For
                    End If
                Next
            End If

            Me.contentPane.ContentUrl = "p28_framework_detail.aspx?pid=" & intSelPID.ToString   'v detailu ho vybereme nezávisle na tom, zda byl nalezen v gridu
            If Request.Item("force") <> "" Then
                Me.contentPane.ContentUrl += "&force=" & Request.Item("force")
            End If
        End If
    End Sub

    Private Sub SetupJ74Combo(intDef As Integer)
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p28Contact).Where(Function(p) p.j74MasterPrefix = "")
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p28Contact, Master.Factory.SysUser.PID)
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.x29IdEnum.p28Contact).Where(Function(p) p.j74MasterPrefix = "")
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
        Master.Factory.j03UserBL.SetUserParam("p28_framework-groupby", Me.cbxGroupBy.SelectedValue)
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
        Master.Factory.j03UserBL.SetUserParam("p28_framework-j74id", Me.CurrentJ74ID.ToString)
        Master.Factory.j03UserBL.SetUserParam("p28_framework-sort", "")
    End Sub
    Private Sub ReloadPage()
        Response.Redirect("p28_framework.aspx")
    End Sub


    Private Sub p28_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        basUIMT.RenderQueryCombo(Me.j70ID)
        If Me.cbxGroupBy.SelectedIndex > 0 Then
            chkGroupsAutoExpanded.Visible = True
        Else
            chkGroupsAutoExpanded.Visible = False
        End If
    End Sub

    Private Sub grid1_SortCommand(SortExpression As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam("p28_framework-sort", SortExpression)
    End Sub
    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p28-j70id", Me.CurrentJ70ID.ToString)
        ReloadPage()
    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        Dim cXLS As New clsExportToXls(Master.Factory)

        Dim mq As New BO.myQueryP28
        InhaleMyQuery(mq)

        Dim lis As IEnumerable(Of BO.p28Contact) = Master.Factory.p28ContactBL.GetList(mq)

        Dim strFileName As String = cXLS.ExportGridData(lis, cJ74)
        If strFileName = "" Then
            Master.Notify(cXLS.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            Response.Redirect("binaryfile.aspx?tempfile=" & strFileName)
        End If
    End Sub

    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)
    End Sub

    Private Sub chkCheckboxSelector_CheckedChanged(sender As Object, e As EventArgs) Handles chkCheckboxSelector.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p28_framework-checkbox_selector", BO.BAS.GB(Me.chkCheckboxSelector.Checked))
        SetupGrid()
        grid1.Rebind(True)
    End Sub

End Class