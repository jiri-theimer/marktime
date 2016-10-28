Imports Telerik.Web.UI
Public Class p31_drilldown
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _lisCols As List(Of ddColumn)

    Private Class ddColumn
        Public Property Header As String
        Public Property ColumnFormat As BO.cfENUM
        Public Property SqlSELECT As String
        Public Property SqlGROUPBY As String
        Public Property Total As Double = 0
        Public Property FieldTypeID As Integer

        Public Sub New(strHeader As String, colFormat As BO.cfENUM, strSqlSelect As String, strSqlGroupBy As String)
            Me.Header = strHeader
            Me.ColumnFormat = colFormat
            Me.SqlSELECT = strSqlSelect
            Me.SqlGROUPBY = strSqlGroupBy
        End Sub
    End Class
    Private Class ddPath
        Public Property Name As String
        Public Property PID As String
        Public Property LevelIndex As Integer
    End Class
   
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
    Private ReadOnly Property CurrentLevel As BO.PivotRowColumnField
        Get
            If hidGroup.Value = "" Then Return Nothing
            Dim ft As BO.PivotRowColumnFieldType = CType(CInt(Me.hidGroup.Value), BO.PivotRowColumnFieldType)
            Return New BO.PivotRowColumnField(ft)
        End Get
    End Property
    Private ReadOnly Property CurrentLevelIndex As Integer
        Get
            If Me.hidCurLevelIndex.Value <> "" Then
                Return CInt(hidCurLevelIndex.Value)
            End If
            Dim x As Integer = 0
            If Me.hidPath_Pids.Value = "" Then x = 1
            Dim a() As String = Split(Me.hidPath_Pids.Value, "->")
            x = UBound(a) + 2
            Me.hidCurLevelIndex.Value = x.ToString
            Return x
        End Get
    End Property
    Private Property MaxLevelIndex As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMaxLevel.Value)
        End Get
        Set(value As Integer)
            Me.hidMaxLevel.Value = value.ToString
        End Set
    End Property
    Private ReadOnly Property CurrentSumFields As List(Of BO.PivotSumField)
        Get
            Dim lis As New List(Of BO.PivotSumField), a() As String = Split(Me.hidCols.Value, ",")
            For i As Integer = 1 To UBound(a)
                lis.Add(New BO.PivotSumField(CType(a(i), BO.PivotSumFieldType)))
            Next
            Return lis
        End Get
    End Property
    Private ReadOnly Property CurrentDDFields As List(Of ddColumn)
        Get
            Dim lis As New List(Of ddColumn), a() As String = Split(Me.hidCols.Value, ",")
            For i = 0 To 0
                Dim c As New BO.PivotRowColumnField(CType(a(i), BO.PivotRowColumnFieldType))
                Dim cc As New ddColumn(c.Caption, c.FieldType, c.SelectField, c.GroupByField)
                cc.FieldTypeID = c.FieldTypeID
                lis.Add(cc)
            Next
            For i As Integer = 1 To UBound(a)
                Dim c As New BO.PivotSumField(CType(a(i), BO.PivotSumFieldType))
                Dim cc As New ddColumn(c.Caption, c.ColumnType, c.SelectField, "")
                cc.FieldTypeID = c.FieldTypeID
                lis.Add(cc)
            Next
            Return lis
        End Get
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

    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click
        SetupData(1)
        grid1.Rebind(False)
    End Sub

    Private Function GetParentSqlWhere(cRec As BO.j75DrillDownTemplate) As String
        If hidPath_Pids.Value = "" Then Return ""
        Dim parpids() As String = Split(hidPath_Pids.Value, "->"), lis As New List(Of String)
        For i As Integer = 0 To UBound(parpids)
            Dim strGF As String = ""
            Select Case i
                Case 0 : strGF = InhaleLevel(cRec.j75Level1).GroupByField
                Case 1 : strGF = InhaleLevel(cRec.j75Level2).GroupByField
                Case 2 : strGF = InhaleLevel(cRec.j75Level3).GroupByField
            End Select
            If parpids(i) = "0" Then
                lis.Add(strGF & " IS NULL")
            Else
                lis.Add(strGF & " = " & parpids(i))
            End If

        Next
        Return String.Join(" AND ", lis)
    End Function

    Private Sub SetupData(intLevel As Integer)
        Me.hidCols.Value = "" : Me.hidGroup.Value = ""
        Dim cRec As BO.j75DrillDownTemplate = Master.Factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)
        Dim lisAllSumCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()
        Dim lisLevel As List(Of BO.PivotRowColumnField) = Master.Factory.j75DrillDownTemplateBL.LevelPallete()

        Dim level As BO.PivotRowColumnField = Nothing
        Select Case intLevel
            Case 1
                If cRec.j75Level1 Is Nothing Then Return
                level = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level1).First
            Case 2
                If cRec.j75Level2 Is Nothing Then Return
                level = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level2).First
            Case 3
                If cRec.j75Level3 Is Nothing Then Return
                level = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level3).First
            Case 4
                If cRec.j75Level4 Is Nothing Then Return
                level = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level4).First
        End Select
        Me.hidGroup.Value = level.FieldTypeID.ToString
        If cRec.j75Level2 Is Nothing Then Me.MaxLevelIndex = 1
        If cRec.j75Level3 Is Nothing Then Me.MaxLevelIndex = 2
        If cRec.j75Level4 Is Nothing Then Me.MaxLevelIndex = 3 Else Me.MaxLevelIndex = 4

        _lisCols = New List(Of ddColumn)
        Dim cc As New ddColumn(level.Caption, BO.cfENUM.AnyString, level.SelectField, level.GroupByField)
        cc.FieldTypeID = level.FieldTypeID
        _lisCols.Add(cc)

        Dim lisJ76 As IEnumerable(Of BO.j76DrillDownTemplate_Item) = Master.Factory.j75DrillDownTemplateBL.GetList_j76(Me.CurrentJ75ID)
        For Each c In lisJ76.Where(Function(p) p.j76Level = intLevel)
            Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
            If Not col Is Nothing Then
                cc = New ddColumn(col.Caption, col.ColumnType, col.SelectField, "")
                cc.FieldTypeID = col.FieldTypeID
                _lisCols.Add(cc)
            End If
        Next

        With grid1
            .ClearColumns()
            .PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
            .radGridOrig.ShowFooter = True
            .DataKeyNames = "pid"
            .AllowMultiSelect = True
            .AddSystemColumn(16)
            .AddSystemColumn(16, "p31")
            .AddSystemColumn(16, "approve")


            For Each c In _lisCols
                Dim strDbField As String = "col" & c.FieldTypeID.ToString
                If c.SqlGROUPBY <> "" Then strDbField = "group" & c.FieldTypeID.ToString

                .AddColumn(strDbField, c.Header, c.ColumnFormat, , , , , IIf(c.ColumnFormat = BO.cfENUM.Numeric2, True, False), False)
                If c.ColumnFormat = BO.cfENUM.Numeric2 Then
                    .radGridOrig.MasterTableView.Columns.FindByDataField("col" & c.FieldTypeID.ToString).HeaderStyle.HorizontalAlign = HorizontalAlign.Right
                End If

                Me.hidCols.Value = BO.BAS.OM4(hidCols.Value, c.FieldTypeID.ToString)
            Next

        End With

        RenderPath()

    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As DataRowView = CType(e.Item.DataItem, DataRowView)
        If Me.CurrentLevelIndex < Me.MaxLevelIndex Then
            With dataItem("systemcolumn")
                .Text = "<a href='javascript:drill(" & cRec.Item("pid").ToString & ")'><img src='Images/arrow_right_dd.png' title='Rozklad na nižší úroveň'></a>"
            End With
        End If
        
        With dataItem("p31")
            .Text = "<a href='javascript:p31(" & cRec.Item("pid").ToString & ")'><img src='Images/worksheet.png' title='Přehled zdrojových worksheet úkonů'></a>"
        End With
        With dataItem("approve")
            .Text = "<a href='javascript:approve(" & cRec.Item("pid").ToString & ")' title='Schvalovat nebo fakturovat'><img src='Images/approve.png'></a>"
        End With
    End Sub

    


    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.CurrentLevel Is Nothing Then Return
        Dim cRec As BO.j75DrillDownTemplate = Master.Factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)

        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        If Me.CurrentLevelIndex < Me.MaxLevelIndex Then grid1.OnRowDblClick = "RowDoubleClick" Else grid1.OnRowDblClick = ""

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(Me.CurrentLevel, Me.CurrentSumFields, GetParentSqlWhere(cRec), mq)
        grid1.DataSourceDataTable = dt

        If dt.Rows.Count > 1 Then
            'zobrazovat footer
            _lisCols = Me.CurrentDDFields()

            For Each dbRow In dt.Rows
                For i As Integer = 0 To _lisCols.Count - 1
                    If dbRow(i + 1).GetType.ToString = "System.Double" Then
                        If Not dbRow(i + 1) Is System.DBNull.Value Then
                            _lisCols(i).Total += dbRow(i + 1)
                        End If
                    End If
                Next
            Next
        Else
            grid1.radGridOrig.ShowFooter = False
        End If
        

    End Sub

    Private Sub grid1_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles grid1.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"
        If _lisCols Is Nothing Then Return

        For Each c In _lisCols
            If c.ColumnFormat = BO.cfENUM.Numeric2 Then
                footerItem.Item("col" & c.FieldTypeID.ToString).Text = BO.BAS.FN(c.Total)
            End If
        Next

        
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click

        Select Case hidHardRefreshFlag.Value
            Case "drill"
                Dim strPID As String = Me.hidHardRefreshPID.Value
                Me.hidPath_Pids.Value = BO.BAS.OM4(Me.hidPath_Pids.Value, strPID, "->")
                Me.hidCurLevelIndex.Value = ""
                Dim strName As String = ""
                Dim dt As DataTable = grid1.radGridOrig.DataSource
                For Each row As GridDataItem In grid1.radGridOrig.MasterTableView.Items

                    If row.GetDataKeyValue("pid").ToString = strPID Then
                        Me.hidPath_Names.Value = BO.BAS.OM4(Me.hidPath_Names.Value, row.Item(grid1.radGridOrig.MasterTableView.Columns(3).UniqueName).Text, "->")
                    End If
                Next

                RenderPath()

                SetupData(Me.CurrentLevelIndex)
                grid1.Rebind(False)
            Case ""
                Return
        End Select

        hidHardRefreshFlag.Value = "" : hidHardRefreshPID.Value = ""
    End Sub
    Private Sub RenderPath()
        Dim lis As New List(Of ddPath)
        If Me.hidPath_Pids.Value <> "" Then
            Dim a() As String = Split(Me.hidPath_Pids.Value, "->")
            Dim b() As String = Split(Me.hidPath_Names.Value, "->")
            For i As Integer = 0 To UBound(a)
                Dim c As New ddPath
                c.PID = a(i)
                c.Name = b(i)
                c.LevelIndex = i + 1
                If c.LevelIndex = 1 Then c.Name = "[ROOT] " & c.Name
                lis.Add(c)
            Next
        Else
            Dim c As New ddPath
            c.PID = -1
            c.Name = "ROOT"
            c.LevelIndex = 1
            lis.Add(c)
        End If
        path1.DataSource = lis
        path1.DataBind()

    End Sub

    Private Function InhaleLevel(fld As BO.PivotRowColumnFieldType) As BO.PivotRowColumnField
        Return New BO.PivotRowColumnField(fld)
    End Function

    Private Sub path1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles path1.ItemCommand
        If e.CommandName = "drill" Then
            Dim intLevel As Integer = CInt(CType(e.Item.FindControl("levelindex"), HiddenField).Value)
            Dim intGridSelPID As Integer = BO.BAS.IsNullInt(CType(e.Item.FindControl("pid"), HiddenField).Value)
            If intLevel > 1 Then
                Dim strPID As String = CType(e.Item.FindControl("pid"), HiddenField).Value
                Dim a() As String = Split(Me.hidPath_Pids.Value, "->")
                Dim b() As String = Split(Me.hidPath_Names.Value, "->")
                hidPath_Pids.Value = "" : hidCurLevelIndex.Value = "" : hidCurLevelIndex.Value = ""
                For i As Integer = 0 To intLevel - 2
                    Me.hidPath_Pids.Value = BO.BAS.OM4(Me.hidPath_Pids.Value, a(i), "->")
                    Me.hidPath_Names.Value = BO.BAS.OM4(Me.hidPath_Names.Value, b(i), "->")
                Next
            Else
                hidPath_Pids.Value = "" : hidPath_Names.Value = "" : hidCurLevelIndex.Value = ""
            End If

            SetupData(intLevel)

            grid1.Rebind(False)
            'Master.Notify(intGridSelPID)
            If intGridSelPID > 0 Then grid1.SelectRecords(intGridSelPID)
        End If
    End Sub

    

    Private Sub path1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles path1.ItemDataBound
        Dim c As ddPath = CType(e.Item.DataItem, ddPath)
        CType(e.Item.FindControl("link1"), LinkButton).Text = c.Name
        CType(e.Item.FindControl("pid"), HiddenField).Value = c.PID
        CType(e.Item.FindControl("levelindex"), HiddenField).Value = c.LevelIndex.ToString
        If c.LevelIndex > 1 Then
            e.Item.FindControl("sipka").Visible = True
        Else
            e.Item.FindControl("sipka").Visible = False
        End If

    End Sub
End Class