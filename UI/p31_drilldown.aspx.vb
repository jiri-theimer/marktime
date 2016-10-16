Imports Telerik.Web.UI
Public Class p31_drilldown
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Public Property CurrentJ70ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j70ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j70ID, value.ToString)
        End Set
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return Me.hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Private ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            If Me.CurrentMasterPrefix = "" Then
                Return BO.x29IdEnum._NotSpecified
            Else
                Return BO.BAS.GetX29FromPrefix(Left(Me.CurrentMasterPrefix, 3))
            End If
        End Get
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            Me.hidMasterPID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentJ75ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j75ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j75ID, value.ToString)
        End Set
    End Property

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)

        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_drilldown"

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                If Request.Item("masterprefix") <> "" Then Me.CurrentMasterPrefix = Request.Item("masterprefix")
                Me.CurrentMasterPID = Request.Item("masterpid")
                If Me.CurrentMasterPrefix <> "" And Me.CurrentMasterPID <> 0 Then
                    panQueryByEntity.Visible = True
                    Me.MasterEntity.Text = .Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix), Me.CurrentMasterPID)
                    Me.MasterEntity.NavigateUrl = "entity_framework.aspx?pid=" & Me.CurrentMasterPID.ToString
                    Select Case Me.CurrentMasterPrefix
                        Case "p41" : imgEntity.ImageUrl = "Images/project_32.png"
                        Case "j02" : imgEntity.ImageUrl = "Images/person_32.png"
                        Case "p28" : imgEntity.ImageUrl = "Images/contact_32.png"
                        Case "p91" : imgEntity.ImageUrl = "Images/invoice_32.png"
                    End Select
                End If

                .PageTitle = "Worksheet DRILL-DOWN"
                .SiteMenuValue = "cmdP31_DrillDown"

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_drilldown-pagesize")
                    .Add(Me.CurrentMasterPrefix & "-j75id")
                    .Add("p31-j70id")
                    .Add("p31_grid-period")
                    .Add("periodcombo-custom_query")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("p31_drilldown-pagesize", "20"))


                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("p31_grid-period")
                    SetupJ75Combo(.GetUserParam(Me.CurrentMasterPrefix & "-j75id", "0"))
                End With



            End With

            SetupJ70Combo(BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam("p31-j70id")))


            SetupGrid()
        End If
    End Sub



    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
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
    Private Sub SetupJ75Combo(intDef As Integer)
        Dim lisJ75 As IEnumerable(Of BO.j75DrillDownTemplate) = Master.Factory.j75DrillDownTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j03ID = Master.Factory.SysUser.PID And p.j75MasterPrefix = Me.CurrentMasterPrefix)

        If lisJ75.Count = 0 Then
            Master.Factory.j75DrillDownTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID, Me.CurrentMasterPrefix)
            lisJ75 = Master.Factory.j75DrillDownTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j03ID = Master.Factory.SysUser.PID And p.j75MasterPrefix = Me.CurrentMasterPrefix)

        End If
        Me.j75ID.DataSource = lisJ75
        Me.j75ID.DataBind()
        basUI.SelectDropdownlistValue(Me.j75ID, intDef.ToString)


    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_drilldown-pagesize", Me.cbxPaging.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("p31_drilldown.aspx" & basUI.GetCompleteQuerystring(Request, True))
    End Sub

    Private Sub p31_drilldown_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        basUIMT.RenderQueryCombo(Me.j70ID)
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31-j70id", Me.j70ID.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub j75ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j75ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam(Me.CurrentMasterPrefix & "-j75id", Me.j75ID.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub SetupGrid()
        Dim cRec As BO.j75DrillDownTemplate = Master.Factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)
        Dim lisJ76 As IEnumerable(Of BO.j76DrillDownTemplate_Item) = Master.Factory.j75DrillDownTemplateBL.GetList_j76(cRec.PID)
        Dim lisLevel As List(Of BO.PivotRowColumnField) = Master.Factory.j75DrillDownTemplateBL.LevelPallete()
        Dim lisAllSumCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()
        Dim level1 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level1).First

        With grid1
            .ClearColumns()
            .PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
            .radGridOrig.ShowFooter = True
            .radGridOrig.MasterTableView.ExpandCollapseColumn.CollapseImageUrl = "Images/tree_collapse.png"
            .radGridOrig.MasterTableView.ExpandCollapseColumn.ExpandImageUrl = "Images/tree_expand.png"
            .AddColumn("group" & level1.FieldTypeID.ToString, level1.Caption)
            Me.hidGroup1.Value = level1.FieldTypeID.ToString
            For Each c In lisJ76.Where(Function(p) p.j76Level = 1)
                Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
                If Not col Is Nothing Then
                    .AddColumn("col" & col.FieldTypeID.ToString, col.Caption, BO.cfENUM.Numeric2)
                    Me.hidCols1.Value = BO.BAS.OM4(hidCols1.Value, col.FieldTypeID.ToString)
                End If
            Next

            Dim gtv As New GridTableView(.radGridOrig)
            If Not cRec.j75Level2 Is Nothing Then
                Dim level2 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level2).First
                With gtv
                    .HierarchyLoadMode = GridChildLoadMode.ServerOnDemand
                    .Name = "level2"
                    '.AllowCustomPaging = True

                    .AllowFilteringByColumn = False
                    .AllowSorting = True
                    .PageSize = 20
                    .DataKeyNames = Split("pid", ",")
                    .ClientDataKeyNames = Split("pid", ",")
                    .ShowHeadersWhenNoRecords = False
                    .ShowFooter = True

                End With
                .radGridOrig.MasterTableView.DetailTables.Add(gtv)
                .AddColumn("group" & level2.FieldTypeID.ToString, level2.Caption, , , , , , , , , gtv)
                Me.hidGroup2.Value = level2.FieldTypeID.ToString
                For Each c In lisJ76.Where(Function(p) p.j76Level = 2)
                    Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
                    If Not col Is Nothing Then
                        .AddColumn("col" & col.FieldTypeID.ToString, col.Caption, BO.cfENUM.Numeric2, , , , , , , , gtv)
                        Me.hidCols2.Value = BO.BAS.OM4(hidCols2.Value, col.FieldTypeID.ToString)
                    End If
                Next
            End If
            If Not cRec.j75Level3 Is Nothing Then
                Dim level3 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level3).First
                Dim gtv3 As New GridTableView(.radGridOrig)
                With gtv3
                    .HierarchyLoadMode = GridChildLoadMode.ServerOnDemand
                    .Name = "level3"
                    '.AllowCustomPaging = True

                    .AllowFilteringByColumn = False
                    .AllowSorting = True
                    .PageSize = 20
                    .DataKeyNames = Split("pid", ",")
                    .ClientDataKeyNames = Split("pid", ",")
                    .ShowHeadersWhenNoRecords = False
                    .ShowFooter = True

                End With
                gtv.DetailTables.Add(gtv3)

                '.radGridOrig.MasterTableView.DetailTables.Add(gtv3)

                .AddColumn("group" & level3.FieldTypeID.ToString, level3.Caption, , , , , , , , , gtv3)
                Me.hidGroup3.Value = level3.FieldTypeID.ToString
                For Each c In lisJ76.Where(Function(p) p.j76Level = 3)
                    Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
                    If Not col Is Nothing Then
                        .AddColumn("col" & col.FieldTypeID.ToString, col.Caption, BO.cfENUM.Numeric2, , , , , , , , gtv)
                        Me.hidCols3.Value = BO.BAS.OM4(hidCols3.Value, col.FieldTypeID.ToString)
                    End If
                Next
            End If


        End With

    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            .j70ID = Me.CurrentJ70ID
            If period1.SelectedValue <> "" Then
                .DateFrom = period1.DateFrom
                .DateUntil = period1.DateUntil
            End If
            Select Case Me.CurrentMasterPrefix
                Case "p41"
                    .p41ID = Me.CurrentMasterPID
                Case "j02"
                    .j02ID = Me.CurrentMasterPID
                Case "p28"
                    .p28ID_Client = Me.CurrentMasterPID
                Case "p91"
                    .p91ID = Me.CurrentMasterPID
            End Select
        End With
    End Sub

    Private Sub grid1_ItemCommand(sender As Object, e As GridCommandEventArgs, strPID As String) Handles grid1.ItemCommand
        If e.CommandName = RadGrid.ExpandCollapseCommandName Then
            Dim item As GridItem
            For Each item In e.Item.OwnerTableView.Items
                If item.Expanded AndAlso Not item Is e.Item Then
                    item.Expanded = False
                End If
            Next item
        End If
    End Sub

    Private Sub grid1_DetailTableDataBind(sender As Object, e As GridDetailTableDataBindEventArgs) Handles grid1.DetailTableDataBind
        Dim dataItem As GridDataItem = DirectCast(e.DetailTableView.ParentItem, GridDataItem)
        Dim intParentPID As Integer = BO.BAS.IsNullInt(dataItem.GetDataKeyValue("pid"))

        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim groupCol As New BO.PivotRowColumnField(CType(Me.hidGroup2.Value, BO.PivotRowColumnFieldType))

        Dim lastGroupCol As New BO.PivotRowColumnField(CType(Me.hidGroup1.Value, BO.PivotRowColumnFieldType))
        Dim strParentSqlWhere As String = lastGroupCol.GroupByField & "=" & intParentPID.ToString

        Dim lis As New List(Of BO.PivotSumField)
        For Each s In Split(Me.hidCols2.Value, ",")
            lis.Add(New BO.PivotSumField(CType(s, BO.PivotSumFieldType)))
        Next
        With e.DetailTableView
            ''.AllowCustomPaging = True
            .AllowSorting = True
            ''If .VirtualItemCount = 0 Then .VirtualItemCount = GetVirtualCount(mq)
            .DataSource = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(groupCol, lis, strParentSqlWhere, mq)


        End With
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If e.IsFromDetailTable Then
            Return
        End If
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim groupCol As New BO.PivotRowColumnField(CType(Me.hidGroup1.Value, BO.PivotRowColumnFieldType))
        Dim lis As New List(Of BO.PivotSumField)
        For Each s In Split(Me.hidCols1.Value, ",")
            lis.Add(New BO.PivotSumField(CType(s, BO.PivotSumFieldType)))
        Next
        grid1.DataSourceDataTable = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(groupCol, lis, "", mq)

    End Sub
End Class