Imports Telerik.Web.UI

Public Class o23_subgrid
    Inherits System.Web.UI.UserControl
    Public Property MasterDataPID As Integer
    Public Property DefaultSelectedPID As Integer = 0
    Public Property Factory As BL.Factory
    Public Property x29ID As BO.x29IdEnum
    Private Property _curIsExport As Boolean


    Public Property IsAllowedCreateDocs As Boolean
        Get
            Return recmenu1.FindItemByValue("new").Visible
        End Get
        Set(value As Boolean)
            recmenu1.FindItemByValue("new").Visible = value
            recmenu1.FindItemByValue("clone").Visible = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("j74id") = ""
            With Factory.j03UserBL
                Dim lisPars As New List(Of String), strKey As String = "o23_subgrid-j74id_" & BO.BAS.GetDataPrefix(Me.x29ID)
                With lisPars
                    .Add("o23_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID))
                    .Add("o23_subgrid-pagesize")
                    .Add("o23_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID))

                    .Add(strKey)
                End With
                .InhaleUserParams(lisPars)
                ViewState("j74id") = .GetUserParam(strKey, "0")

                If ViewState("j74id") = "" Or ViewState("j74id") = "0" Then
                    Me.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.o23Notepad, Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.x29ID))
                    Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.o23Notepad, Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.x29ID))
                    ViewState("j74id") = cJ74.PID
                    .SetUserParam(strKey, ViewState("j74id"))
                End If

                basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("o23_subgrid-pagesize", "10"))
                basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("o23_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID)))
            End With
            
            SetupgridO23()
            

        End If
    End Sub

    Private Sub SetupgridO23()
        Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))
        If cJ74 Is Nothing Then
            cJ74 = Me.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.o23Notepad, Me.Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.x29ID))
        End If

        Me.hidDefaultSorting.Value = cJ74.j74OrderBy
        Dim strAddSqlFrom As String = ""
        Me.hidCols.Value = basUIMT.SetupGrid(Me.Factory, Me.gridO23, cJ74, CInt(Me.cbxPaging.SelectedValue), False, Not _curIsExport, True, , , , strAddSqlFrom)
        Me.hidFrom.Value = strAddSqlFrom
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With



    End Sub


    Private Sub gridO23_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gridO23.ItemDataBound
        basUIMT.o23_grid_Handle_ItemDataBound(sender, e, True, True)
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
    Public Sub InhaleQuery(ByRef mq As BO.myQueryO23)
        Select Case Me.x29ID
            Case BO.x29IdEnum.p41Project
                mq.p41ID = MasterDataPID
            Case BO.x29IdEnum.p28Contact
                mq.p28ID = MasterDataPID
            Case BO.x29IdEnum.j02Person
                mq.j02ID = MasterDataPID
        End Select

        mq.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead

        With mq
            .Closed = BO.BooleanQueryMode.NoQuery
            .MG_GridGroupByField = Me.cbxGroupBy.SelectedValue
            .MG_GridSqlColumns = Me.hidCols.Value
            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            .MG_SortString = gridO23.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If

        End With
    End Sub
    Private Sub gridO23_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridO23.NeedDataSource
        If MasterDataPID = 0 Then Return

        Dim mq As New BO.myQueryO23
        InhaleQuery(mq)

      
        Dim dt As DataTable = Me.Factory.o23NotepadBL.GetGridDataSource(mq)
        If dt Is Nothing Then
            Return
        Else
            gridO23.DataSourceDataTable = dt
        End If
        lblHeaderO23.Text = BO.BAS.OM2(lblHeaderO23.Text, dt.Rows.Count.ToString)

        If Me.DefaultSelectedPID <> 0 Then
            If dt.AsEnumerable.Where(Function(p) p.Item("pid") = Me.DefaultSelectedPID).Count > 0 Then
                'záznam je na první stránce
            Else
                Dim mqAll As New BO.myQueryO23
                mqAll.TopRecordsOnly = 0
                mqAll.MG_SelectPidFieldOnly = True

                inhalequery(mqAll)
                Dim dtAll As DataTable = Me.Factory.o23NotepadBL.GetGridDataSource(mqAll)
                Dim x As Integer, intNewPageIndex As Integer = 0
                For Each dbRow As DataRow In dtAll.Rows
                    x += 1
                    If x > gridO23.PageSize Then
                        intNewPageIndex += 1 : x = 1
                    End If
                    If dbRow.Item("pid") = Me.DefaultSelectedPID Then
                        InhaleQuery(mq)
                        gridO23.radGridOrig.CurrentPageIndex = intNewPageIndex
                        mq.MG_CurrentPageIndex = intNewPageIndex
                        dt = Me.Factory.o23NotepadBL.GetGridDataSource(mq) 'nový zdroj pro grid
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

   

   
    Public Sub Rebind(bolKeepSelectedRecord As Boolean)
        gridO23.Rebind(bolKeepSelectedRecord)
    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        With gridO23.radGridOrig.MasterTableView
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
        Factory.j03UserBL.SetUserParam("o23_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID), Me.cbxGroupBy.SelectedValue)
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        gridO23.Rebind(True)
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Me.Factory.j03UserBL.SetUserParam("o23_subgrid-pagesize", Me.cbxPaging.SelectedValue)
        SetupgridO23()
        gridO23.Rebind(True)
    End Sub

   

    Private Sub gridO23_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles gridO23.NeedFooterSource
        If Me.DefaultSelectedPID <> 0 Then
            gridO23.SelectRecords(Me.DefaultSelectedPID)
        End If
    End Sub


    
End Class