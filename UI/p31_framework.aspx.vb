﻿Imports Telerik.Web.UI
Public Class p31_framework
    Inherits System.Web.UI.Page

    Protected WithEvents _MasterPage As Site
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private _lastP41ID As Integer = 0

    Public Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j74id, value.ToString)
        End Set
    End Property
    
    Public ReadOnly Property CurrentJ02ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidCurrentJ02ID.Value)
        End Get
    End Property
    Public ReadOnly Property GridPrefix As String
        Get
            If tabs1.SelectedIndex = 2 Then Return "p56" Else Return "p41"
        End Get

    End Property
    Public Property IsUseReceiversInLine As Boolean
        Get
            If hidReceiversInLine.Value = "1" Then Return True Else Return False
        End Get
        Set(value As Boolean)
            hidReceiversInLine.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public Property IsUseTasksWorksheetColumns As Boolean
        Get
            If Me.hidTasksWorksheetColumns.Value = "1" Then Return True Else Return False
        End Get
        Set(value As Boolean)
            hidTasksWorksheetColumns.Value = BO.BAS.GB(value)
        End Set
    End Property
    

    Private Sub p31_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_framework"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Zapisování úkonů"
                .SiteMenuValue = "cmdP31_Calendar"
                If Request.Item("tab") = "" Then
                    .Factory.j03UserBL.InhaleUserParams("p31_framework-tabindex")
                    tabs1.SelectedIndex = CInt(.Factory.j03UserBL.GetUserParam("p31_framework-tabindex", "0"))
                Else
                    tabs1.SelectedIndex = CInt(Request.Item("tab"))
                End If
                InitialGroupByCombo(tabs1.SelectedIndex)

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_framework-pagesize-" & Me.GridPrefix)
                    .Add("p31_framework-search")

                    .Add("p31_framework-navigationPane_width")
                    .Add("p31_framework_detail-j02id")  'výchozí osoba pro nové úkony
                    .Add("p31_framework-j74id-" & Me.GridPrefix)
                    .Add("p31_framework-groupby-" & Me.GridPrefix)
                    .Add("p31_framework-sort-" & Me.GridPrefix)
                    .Add("p31_framework-groups-autoexpanded")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                    hidCurrentJ02ID.Value = .GetUserParam("p31_framework_detail-j02id", Master.Factory.SysUser.j02ID.ToString)
                    Me.txtSearch.Text = .GetUserParam("p31_framework-search")
                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("p31_framework-pagesize-" & Me.GridPrefix, "20"))
                    Me.navigationPane.Width = Unit.Parse(.GetUserParam("p31_framework-navigationPane_width", "350") & "px")
                    
                    basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("p31_framework-groupby-" & Me.GridPrefix, IIf(Me.GridPrefix = "p56", "Client", "")))
                    If tabs1.SelectedIndex = 0 Then
                        Me.chkGroupsAutoExpanded.Checked = BO.BAS.BG(.GetUserParam("p31_framework-groups-autoexpanded", "0"))
                    Else
                        chkGroupsAutoExpanded.Checked = True
                        chkGroupsAutoExpanded.Visible = False
                    End If

                    If .GetUserParam("p31_framework-sort-" & Me.GridPrefix) <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam("p31_framework-sort-" & Me.GridPrefix))
                    End If
                End With

            End With

            If Request.Item("search") <> "" Then
                txtSearch.Text = Request.Item("search")   'externě předaná podmínka
                txtSearch.Focus()
            End If

            If tabs1.SelectedIndex > 0 Then
                txtSearch.Visible = False : txtSearch.Text = "" : cmdSearch.Visible = False
            End If

            RecalcVirtualRowCount()
            RecalcTasksCount()
            SetupJ74Combo(BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam("p31_framework-j74id-" & Me.GridPrefix)))
            SetupGrid()
            Handle_DefaultSelectedRecord()


            If tabs1.SelectedIndex = 2 Then
                'úkoly
                cmdNewTask.Visible = True
            Else
                cmdNewTask.Visible = False
            End If
            
            
        End If
    End Sub

    Private Sub InitialGroupByCombo(intTabIndex As Integer)
        With Me.cbxGroupBy
            .Items.Clear()
            .Items.Add(New ListItem("Bez souhrnů", ""))
            .Items.Add(New ListItem("Klient", "Client"))
            If intTabIndex < 2 Then
                .Items.Add(New ListItem("Typ projektu", "p42Name"))
                .Items.Add(New ListItem("Středisko", "j18Name"))
            Else
                .Items.Add(New ListItem("Typ úkolu", "p57Name"))
                .Items.Add(New ListItem("Projekt", "ProjectCodeAndName"))
                .Items.Add(New ListItem("Příjemce", "ReceiversInLine"))
                .Items.Add(New ListItem("Milník", "o22Name"))
                .Items.Add(New ListItem("Vlastník", "Owner"))
            End If
        End With
    End Sub

    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(BO.BAS.GetX29FromPrefix(Me.GridPrefix), Master.Factory.SysUser.PID, "p31_framework")
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo(cJ74.PID)
                End If
            End If
            If tabs1.SelectedIndex = 0 Then
                basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, False)
            Else
                basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 100, False, False)
            End If

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

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        If Me.GridPrefix = "p41" Then
            Dim cRec As BO.p41Project = CType(e.Item.DataItem, BO.p41Project)
            If cRec.IsClosed Then dataItem.Font.Strikeout = True
            dataItem("systemcolumn").Text = "<a title='Zapsat úkon' href='javascript:nw(" & cRec.PID.ToString & ")'><img src='Images/new.png' border=0/></a>"
        Else
            Dim cRec As BO.p56Task = CType(e.Item.DataItem, BO.p56Task)
            With dataItem("systemcolumn")
                .Text = "<a class='reczoom' title='Detail úkolu' rel='clue_p56_record.aspx?&pid=" & cRec.PID.ToString & "'>i</a>"
            End With
            If Not cRec.p56PlanUntil Is Nothing Then
                If Now > cRec.p56PlanUntil Then dataItem.ForeColor = Drawing.Color.DarkRed
            End If
        End If



    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.GridPrefix = "p41" Then
            Dim mq As New BO.myQueryP41
            With mq
                If tabs1.SelectedIndex = 0 Then
                    .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                Else
                    .TopRecordsOnly = 0
                    .MG_PageSize = 0
                    .MG_CurrentPageIndex = 0
                End If
            End With

            InhaleMyQuery(mq)

            Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq)
            If lis Is Nothing Then
                Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
            Else
                If tabs1.SelectedIndex = 1 Then
                    lis = basUIMT.QueryProjectListByTop10(Master.Factory, Me.CurrentJ02ID, lis)
                End If
                grid1.DataSource = lis
            End If

        End If
        If Me.GridPrefix = "p56" Then
            If Me.IsUseTasksWorksheetColumns Then
                grid1.DataSource = GetTasksListWithWorksheetSum()
            Else
                grid1.DataSource = GetTasksList()
            End If

        End If


    End Sub

    ''Private Function QueryProjectListByTop10(lis As IEnumerable(Of BO.p41Project)) As IEnumerable(Of BO.p41Project)
    ''    Dim mqP31 As New BO.myQueryP31
    ''    If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then
    ''        mqP31.j02ID = Me.CurrentJ02ID
    ''    Else
    ''        mqP31.j02ID = Master.Factory.SysUser.j02ID
    ''    End If
    ''    mqP31.TopRecordsOnly = 100
    ''    mqP31.MG_SortString = "p31dateinsert desc"
    ''    Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mqP31)
    ''    Dim p41ids As New List(Of Integer)
    ''    If lisP31.Count > 0 Then
    ''        If lisP31.Select(Function(p) p.p41ID).Distinct.Count > 10 Then
    ''            For Each c In lisP31
    ''                If p41ids.Where(Function(p) p = c.p41ID).Count = 0 Then
    ''                    p41ids.Add(c.p41ID)
    ''                End If
    ''                If p41ids.Count >= 10 Then Exit For
    ''            Next
    ''        Else
    ''            p41ids = lisP31.Select(Function(p) p.p41ID).Distinct.ToList
    ''        End If
    ''    Else
    ''        p41ids.Add(-1)
    ''    End If
    ''    Dim mqP41 As New BO.myQueryP41
    ''    mqP41.PIDs = p41ids
    ''    mqP41.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
    ''    Return Master.Factory.p41ProjectBL.GetList(mqP41)
    ''End Function

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP41)
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
            .SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
            .SearchExpression = Trim(Me.txtSearch.Text)
            If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then .j02ID_ExplicitQueryFor = Me.CurrentJ02ID


        End With

    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_framework-pagesize-" & Me.GridPrefix, Me.cbxPaging.SelectedValue)

        RecalcVirtualRowCount()
        grid1.Rebind(True)
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSearch.Click
        Handle_RunSearch()

    End Sub

    Private Sub Handle_RunSearch()
        Master.Factory.j03UserBL.SetUserParam("p31_framework-search", txtSearch.Text)

        RecalcVirtualRowCount()
        grid1.Rebind(False)

        txtSearch.Focus()
    End Sub



    Private Sub Handle_DefaultSelectedRecord()
        If Me.GridPrefix = "p56" Then Return

        Dim intSelPID As Integer = 0
        If Not Page.IsPostBack Then
            Dim strDefPID As String = Request.Item("p41id")

            If strDefPID > "" Then intSelPID = BO.BAS.IsNullInt(strDefPID)
        End If

        If intSelPID > 0 Then
            'označit výchozí projekt
            grid1.SelectRecords(intSelPID)
            If grid1.GetSelectedPIDs.Count = 0 Then
                'hledaný projekt nebyl nalezen na první stránce
                Dim mq As New BO.myQueryP41
                InhaleMyQuery(mq)
                mq.MG_SelectPidFieldOnly = True
                mq.TopRecordsOnly = 0
                Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq), x As Integer, intNewPageIndex As Integer = 0
                If lis Is Nothing Then
                    Master.Notify(Master.Factory.p41ProjectBL.ErrorMessage, NotifyLevel.ErrorMessage)
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
                    End If
                Next

            End If

        End If
    End Sub

   


    Private Sub p31_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Trim(txtSearch.Text) = "" Then
            txtSearch.Style.Item("background-color") = ""
        Else
            txtSearch.Style.Item("background-color") = "red"
        End If
        
        
      
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        

        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""
    End Sub

    Private Sub SetupJ74Combo(intDef As Integer)
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.BAS.GetX29FromPrefix(Me.GridPrefix)).Where(Function(p) p.j74MasterPrefix = "p31_framework")
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.BAS.GetX29FromPrefix(Me.GridPrefix), Master.Factory.SysUser.PID, "p31_framework")
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, BO.BAS.GetX29FromPrefix(Me.GridPrefix)).Where(Function(p) p.j74MasterPrefix = "p31_framework")
        End If
        j74id.DataSource = lisJ74
        j74id.DataBind()

        If intDef > 0 Then
            basUI.SelectDropdownlistValue(Me.j74id, intDef.ToString)
        End If
        If Me.CurrentJ74ID > 0 Then
            _curJ74 = lisJ74.Where(Function(p) p.PID = Me.CurrentJ74ID)(0)
            If _curJ74.j74ColumnNames.IndexOf("ReceiversInLine") > 0 Then
                Me.IsUseReceiversInLine = True
            Else
                Me.IsUseReceiversInLine = False
            End If
            If _curJ74.j74ColumnNames.IndexOf("Hours_Orig") > 0 Or _curJ74.j74ColumnNames.IndexOf("Expenses_Orig") > 0 Then
                Me.IsUseTasksWorksheetColumns = True
            Else
                Me.IsUseTasksWorksheetColumns = False
            End If
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
        Master.Factory.j03UserBL.SetUserParam("p31_framework-groupby-" & Me.GridPrefix, Me.cbxGroupBy.SelectedValue)
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
        Master.Factory.j03UserBL.SetUserParam("p31_framework-j74id-" & Me.GridPrefix, Me.CurrentJ74ID.ToString)
        Master.Factory.j03UserBL.SetUserParam("p31_framework-sort-" & Me.GridPrefix, "")
    End Sub
    Private Sub ReloadPage()
        Response.Redirect("p31_framework.aspx")
    End Sub

    Private Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryP41
        InhaleMyQuery(mq)
        grid1.VirtualRowCount = Master.Factory.p41ProjectBL.GetVirtualCount(mq)

        grid1.radGridOrig.CurrentPageIndex = 0
        With tabs1.Tabs(0)
            .Text = BO.BAS.OM2(.Text, grid1.VirtualRowCount.ToString)
        End With
        'Me.lblFormHeader.Text = "Worksheet (" & BO.BAS.FNI(grid1.VirtualRowCount) & ")"
    End Sub
    Private Sub RecalcTasksCount()
        With tabs1.Tabs(2)
            .Text = BO.BAS.OM2(.Text, GetTasksCount().ToString)
        End With
    End Sub


    Private Sub grid1_SortCommand(SortExpression As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam("p31_framework-sort-" & Me.GridPrefix, SortExpression)
    End Sub

    Private Sub chkGroupsAutoExpanded_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupsAutoExpanded.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_framework-groups-autoexpanded", BO.BAS.GB(Me.chkGroupsAutoExpanded.Checked))
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        grid1.Rebind(True)


    End Sub

    Private Sub tabs1_TabClick(sender As Object, e As RadTabStripEventArgs) Handles tabs1.TabClick
        Master.Factory.j03UserBL.SetUserParam("p31_framework-tabindex", tabs1.SelectedIndex.ToString)
        Server.Transfer("p31_framework.aspx")
    End Sub

    
    Private Function GetTasksCount() As Integer
        Dim mq As New BO.myQueryP56
        InhaleMyTaskQuery(mq)
        Return Master.Factory.p56TaskBL.GetVirtualCount(mq)
    End Function
    Private Function GetTasksList() As IEnumerable(Of BO.p56Task)
        Dim mq As New BO.myQueryP56
        InhaleMyTaskQuery(mq)

        Return Master.Factory.p56TaskBL.GetList(mq, Me.IsUseReceiversInLine)
    End Function
    Private Function GetTasksListWithWorksheetSum() As IEnumerable(Of BO.p56TaskWithWorksheetSum)
        Dim mq As New BO.myQueryP56
        InhaleMyTaskQuery(mq)

        Return Master.Factory.p56TaskBL.GetList_WithWorksheetSum(mq, Me.IsUseReceiversInLine)
    End Function

    Private Sub InhaleMyTaskQuery(ByRef mq As BO.myQueryP56)
        mq.j02ID = Me.CurrentJ02ID
        mq.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForRead
        mq.Closed = BO.BooleanQueryMode.FalseQuery
    End Sub

    ''Private Sub rpP56_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP56.ItemDataBound
    ''    Dim cRec As BO.p56Task = CType(e.Item.DataItem, BO.p56Task)
    ''    With CType(e.Item.FindControl("TodoHeader"), Label)
    ''        .Text = cRec.p57Name & ": <i><b>" & cRec.p56Name & "</b></i>"
    ''    End With
    ''    With CType(e.Item.FindControl("cmdP31"), HyperLink)
    ''        .NavigateUrl = "javascript:nw_p56(" & cRec.PID.ToString & ")"
    ''    End With
    ''    With CType(e.Item.FindControl("p56PlanUntil"), Label)
    ''        If Not cRec.p56PlanUntil Is Nothing Then
    ''            .Text = BO.BAS.FD(cRec.p56PlanUntil, True, True)
    ''            If cRec.p56PlanUntil < Now Then
    ''                .ForeColor = Drawing.Color.Red
    ''            End If
    ''        End If
    ''    End With
    ''    With CType(e.Item.FindControl("cmdEdit"), HyperLink)
    ''        .NavigateUrl = "javascript:p56_edit(" & cRec.PID.ToString & ")"
    ''    End With
    ''    With CType(e.Item.FindControl("Project"), Label)
    ''        If cRec.p41ID = _lastP41ID Then
    ''            e.Item.FindControl("panProject").Visible = False
    ''        Else
    ''            If cRec.Client = "" Then
    ''                .Text = cRec.p41Name
    ''            Else
    ''                .Text = cRec.Client & " - " & cRec.p41Name
    ''            End If
    ''        End If
    ''    End With
    ''    With CType(e.Item.FindControl("clue1"), HyperLink)
    ''        .Attributes("rel") = "clue_p56_record.aspx?mode=readonly&pid=" & cRec.PID.ToString
    ''    End With
    ''    _lastP41ID = cRec.p41ID
    ''End Sub
End Class