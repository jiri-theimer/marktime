Imports Telerik.Web.UI

Public Class entity_framework_p41subform
    Inherits System.Web.UI.Page
    Private Property _curIsExport As Boolean
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
    Public Property DefaultSelectedPID As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            If Me.CurrentMasterPID = 0 Or Me.CurrentMasterPrefix = "" Then Master.StopPage("masterpid or masterprefix missing.")
            If Request.Item("lasttabkey") <> "" Then
                Master.Factory.j03UserBL.SetUserParam(Request.Item("lasttabkey"), Request.Item("lasttabval"))
            End If

            ViewState("j74id") = ""
            With Master.Factory.j03UserBL
                Dim lisPars As New List(Of String), strKey As String = "entiy_framework_p41subform-j74id_" & Me.CurrentMasterPrefix
                With lisPars
                    .Add("entiy_framework_p41subform-groupby-" & Me.CurrentMasterPrefix)
                    .Add("entiy_framework_p41subform-pagesize")
                    .Add("entiy_framework_p41subform-validity")
                    .Add(strKey)
                End With
                .InhaleUserParams(lisPars)
                ViewState("j74id") = .GetUserParam(strKey, "0")

                If ViewState("j74id") = "" Or ViewState("j74id") = "0" Then
                    Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p41Project, Master.Factory.SysUser.PID, Me.CurrentMasterPrefix)
                    Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p41Project, Master.Factory.SysUser.PID, Me.CurrentMasterPrefix)
                    ViewState("j74id") = cJ74.PID
                    .SetUserParam(strKey, ViewState("j74id"))
                End If
                basUI.SelectDropdownlistValue(Me.cbxValidity, .GetUserParam("entiy_framework_p41subform-validity", "1"))
                basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("entiy_framework_p41subform-pagesize", "10"))
                basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("entiy_framework_p41subform-groupby-" & Me.CurrentMasterPrefix))
            End With
            panExport.Visible = Master.Factory.TestPermission(BO.x53PermValEnum.GR_GridTools)

            SetupGrid()
        End If

    End Sub


    Private Sub SetupGrid()
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))
        If cJ74 Is Nothing Then
            cJ74 = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p41Project, Master.Factory.SysUser.PID, Me.CurrentMasterPrefix)
        End If

        Me.hidDefaultSorting.Value = cJ74.j74OrderBy
        Dim strAddSqlFrom As String = ""
        Me.hidCols.Value = basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, CInt(Me.cbxPaging.SelectedValue), False, Not _curIsExport, True, , , , strAddSqlFrom)
        Me.hidFrom.Value = strAddSqlFrom
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With



    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
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
        Master.Factory.j03UserBL.SetUserParam("entiy_framework_p41subform-groupby-" & Me.CurrentMasterPrefix, Me.cbxGroupBy.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("entiy_framework_p41subform-pagesize", Me.cbxPaging.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("entity_framework_p41subform.aspx?masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString, True)
    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))
        Dim cXLS As New clsExportToXls(Master.Factory)

        Dim mq As New BO.myQueryP41
        InhaleQuery(mq)
        mq.MG_GridGroupByField = ""

        Dim dt As DataTable = Master.Factory.p41ProjectBL.GetGridDataSource(mq)

        Dim strFileName As String = cXLS.ExportGridData(dt.AsEnumerable, cJ74)
        If strFileName = "" Then
            Response.Write(cXLS.ErrorMessage)
        Else
            Response.Redirect("binaryfile.aspx?tempfile=" & strFileName)
        End If
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p41_grid_Handle_ItemDataBound(sender, e, True)
        If _curIsExport Then
            If TypeOf e.Item Is GridHeaderItem Then
                e.Item.BackColor = Drawing.Color.Silver
            End If
            If TypeOf e.Item Is GridGroupHeaderItem Then
                e.Item.BackColor = Drawing.Color.WhiteSmoke
            End If
            If TypeOf e.Item Is GridDataItem Or TypeOf e.Item Is GridHeaderItem Then
                e.Item.Cells(0).Visible = False
            End If
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        
        Dim mq As New BO.myQueryP41
        InhaleQuery(mq)

        Dim dt As DataTable = Master.Factory.p41ProjectBL.GetGridDataSource(mq)

        If dt Is Nothing Then
            Return
        Else
            grid1.DataSourceDataTable = dt
        End If

        lblHeaderP41.Text = BO.BAS.OM2(lblHeaderP41.Text, dt.Rows.Count.ToString)

        If Me.DefaultSelectedPID <> 0 Then
            If dt.AsEnumerable.Where(Function(p) p.Item("pid") = Me.DefaultSelectedPID).Count > 0 Then
                'záznam je na první stránce
            Else
                Dim mqAll As New BO.myQueryP41
                mqAll.TopRecordsOnly = 0
                mqAll.MG_SelectPidFieldOnly = True
                InhaleQuery(mqAll)
                Dim dtAll As DataTable = Master.Factory.p41ProjectBL.GetGridDataSource(mqAll)
                Dim x As Integer, intNewPageIndex As Integer = 0
                For Each dbRow As DataRow In dtAll.Rows
                    x += 1
                    If x > grid1.PageSize Then
                        intNewPageIndex += 1 : x = 1
                    End If
                    If dbRow.Item("pid") = Me.DefaultSelectedPID Then
                        InhaleQuery(mq)
                        grid1.radGridOrig.CurrentPageIndex = intNewPageIndex
                        mq.MG_CurrentPageIndex = intNewPageIndex
                        dt = Master.Factory.p41ProjectBL.GetGridDataSource(mq) 'nový zdroj pro grid
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub InhaleQuery(ByRef mq As BO.myQueryP41)
        mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead

        Select Case Me.CurrentMasterPrefix
            Case "p41"
                mq.p41ParentID = Me.CurrentMasterPID
            Case "p28"
                mq.p28ID = Me.CurrentMasterPID
            Case Else

        End Select

        With mq
            Select Case Me.cbxValidity.SelectedValue
                Case "1" : .Closed = BO.BooleanQueryMode.NoQuery
                Case "2" : .Closed = BO.BooleanQueryMode.FalseQuery
                Case "3" : .Closed = BO.BooleanQueryMode.TrueQuery
            End Select
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If

        End With
    End Sub

    Private Sub GridExport(strFormat As String)
        _curIsExport = True

        SetupGrid()
        basUIMT.Handle_GridTelerikExport(Me.grid1, strFormat)
    End Sub
    Private Sub cbxValidity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxValidity.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("entiy_framework_p41subform-validity", Me.cbxValidity.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub cmdPDF_Click(sender As Object, e As EventArgs) Handles cmdPDF.Click
        GridExport("pdf")
    End Sub

    Private Sub cmdXLS_Click(sender As Object, e As EventArgs) Handles cmdXLS.Click
        GridExport("xls")
    End Sub

    Private Sub cmdDOC_Click(sender As Object, e As EventArgs) Handles cmdDOC.Click
        GridExport("doc")
    End Sub
End Class