Imports Telerik.Web.UI
Public Class p31summary_drilldown
    Inherits System.Web.UI.UserControl

    Public Property Factory As BL.Factory

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
    Public Property EnableEntityChilds As Boolean
        Get
            Return Me.chkIncludeChilds.Visible
        End Get
        Set(value As Boolean)
            Me.chkIncludeChilds.Visible = value
        End Set
    End Property
    Public Property IncludeEntityChilds As Boolean
        Get
            Return Me.chkIncludeChilds.Checked
        End Get
        Set(value As Boolean)
            Me.chkIncludeChilds.Checked = value
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
    Public ReadOnly Property CurrentLevel As BO.PivotRowColumnField
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
    Public Property IsApprovingPerson As Boolean
        Get
            Return BO.BAS.BG(Me.hidIsApprovingPerson.Value)
        End Get
        Set(value As Boolean)
            Me.hidIsApprovingPerson.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public Sub Handle_Drill_After_DoubleClick(strPID As String)
        Me.hidPath_Pids.Value = BO.BAS.OM4(Me.hidPath_Pids.Value, strPID, "->")
        Me.hidCurLevelIndex.Value = ""
        Dim strName As String = ""
        Dim dt As DataTable = grid1.radGridOrig.DataSource
        For Each row As GridDataItem In grid1.radGridOrig.MasterTableView.Items

            If row.GetDataKeyValue("pid").ToString = strPID Then
                Me.hidPath_Names.Value = BO.BAS.OM4(Me.hidPath_Names.Value, row.Item(grid1.radGridOrig.MasterTableView.Columns(4).UniqueName).Text, "->")
            End If
        Next

        RenderPath()

        SetupData(Me.CurrentLevelIndex)
        grid1.Rebind(False)
    End Sub
    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        Handle_Drill_After_DoubleClick(hidHardRefreshPID.Value)
        hidHardRefreshPID.Value = ""
    End Sub

    ''Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ''    If Not Page.IsPostBack Then

    ''        Dim lisPars As New List(Of String)
    ''        With lisPars
    ''            .Add("p31_drilldown-pagesize")
    ''            .Add(Me.CurrentMasterPrefix & "-j75id")
    ''            .Add("p31_drilldown-j70id")
    ''            .Add("p31_grid-period")
    ''            .Add("periodcombo-custom_query")
    ''        End With
    ''        With Me.Factory.j03UserBL
    ''            .InhaleUserParams(lisPars)
    ''            basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("p31_drilldown-pagesize", "20"))


    ''            period1.SetupData(Me.Factory, .GetUserParam("periodcombo-custom_query"))
    ''            period1.SelectedValue = .GetUserParam("p31_grid-period")
    ''            SetupJ75Combo(.GetUserParam(Me.CurrentMasterPrefix & "-j75id", "0"))
    ''        End With




    ''        SetupJ70Combo(BO.BAS.IsNullInt(Me.Factory.j03UserBL.GetUserParam("p31_drilldown-j70id")))


    ''        SetupData(1)
    ''    End If
    ''End Sub

    Public Sub FirstSetup(strGridPageSize As String, strPeriodValue As String, strPeriodCustomQuery As String, strJ75ID As String, strJ70ID As String)
        basUI.SelectDropdownlistValue(cbxPaging, strGridPageSize)


        period1.SetupData(Me.Factory, strPeriodCustomQuery)
        period1.SelectedValue = strPeriodValue
        SetupJ75Combo(BO.BAS.IsNullInt(strJ75ID))

        SetupJ70Combo(BO.BAS.IsNullInt(strJ70ID))


        SetupData(1)
    End Sub


    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Me.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
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
    Private Sub SetupJ75Combo(intDef As Integer)
        Dim lisJ75 As IEnumerable(Of BO.j75DrillDownTemplate) = Me.Factory.j75DrillDownTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j03ID = Me.Factory.SysUser.PID And p.j75MasterPrefix = Me.CurrentMasterPrefix)

        If lisJ75.Count = 0 Then
            Me.factory.j75DrillDownTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Me.factory.SysUser.PID, Me.CurrentMasterPrefix)
            lisJ75 = Me.factory.j75DrillDownTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j03ID = Me.factory.SysUser.PID And p.j75MasterPrefix = Me.CurrentMasterPrefix)

        End If
        Me.j75ID.DataSource = lisJ75
        Me.j75ID.DataBind()
        basUI.SelectDropdownlistValue(Me.j75ID, intDef.ToString)


    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Me.factory.j03UserBL.SetUserParam("p31_drilldown-pagesize", Me.cbxPaging.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Me.factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("entity_framework_rec_summary.aspx?masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString, True)
    End Sub

    

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Me.factory.j03UserBL.SetUserParam("p31_drilldown-j70id", Me.j70ID.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub j75ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j75ID.SelectedIndexChanged
        Me.factory.j03UserBL.SetUserParam(Me.CurrentMasterPrefix & "-j75id", Me.j75ID.SelectedValue)
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
                    If Me.chkIncludeChilds.Visible Then .IncludeChildProjects = Me.chkIncludeChilds.Checked
                Case "j02"
                    .j02ID = Me.CurrentMasterPID
                Case "p28"
                    .p28ID_Client = Me.CurrentMasterPID
                Case "p91"
                    .p91ID = Me.CurrentMasterPID
            End Select
        End With
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
        Dim cRec As BO.j75DrillDownTemplate = Me.factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)
        Dim lisAllSumCols As List(Of BO.PivotSumField) = Me.factory.j75DrillDownTemplateBL.ColumnsPallete()
        Dim lisLevel As List(Of BO.PivotRowColumnField) = Me.factory.j75DrillDownTemplateBL.LevelPallete()

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

        Dim lisJ76 As IEnumerable(Of BO.j76DrillDownTemplate_Item) = Me.factory.j75DrillDownTemplateBL.GetList_j76(Me.CurrentJ75ID)
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
            .AddSystemColumn(16, "fullscreen")

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



        ''If Not cRec.Item("prefix") Is System.DBNull.Value Then
        With dataItem("p31")
            .Text = "<a href=" & Chr(34) & "javascript:p31(" & cRec.Item("pid").ToString & ")" & Chr(34) & "><img src='Images/worksheet.png' title='Přehled zdrojových worksheet úkonů'></a>"
        End With
        If Me.IsApprovingPerson Then
            With dataItem("approve")
                .Text = "<a href=" & Chr(34) & "javascript:approve_local(" & cRec.Item("pid").ToString & ")" & Chr(34) & " title='Schvalovat nebo fakturovat'><img src='Images/approve.png'></a>"
            End With
        End If
        If Not cRec.Item("prefix") Is System.DBNull.Value Then
            Dim b As Boolean = False
            Select Case cRec.Item("prefix")
                Case "p41"
                    b = Me.factory.SysUser.j04IsMenu_Project
                Case "p28"
                    b = Me.factory.SysUser.j04IsMenu_Contact
                Case "p91"
                    b = Me.factory.SysUser.j04IsMenu_Invoice
                Case "j02"
                    b = Me.factory.SysUser.j04IsMenu_People
            End Select
            If b Then
                With dataItem("fullscreen")
                    .Text = "<a href=" & Chr(34) & "javascript:fullscreen(" & cRec.Item("pid").ToString & ",'" & cRec.Item("prefix") & "')" & Chr(34) & " title='Přejít na detail entity'><img src='Images/fullscreen.png'></a>"
                End With
            End If
        End If


        ''End If

    End Sub




    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.CurrentLevel Is Nothing Then Return
        Dim cRec As BO.j75DrillDownTemplate = Me.factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)

        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        If Me.CurrentLevelIndex < Me.MaxLevelIndex Then grid1.OnRowDblClick = "RowDoubleClick" Else grid1.OnRowDblClick = ""
        Me.hidAdditionalWhere.Value = GetParentSqlWhere(cRec)

        Dim dt As DataTable = Me.factory.p31WorksheetBL.GetDrillDownDatasource(Me.CurrentLevel, Me.CurrentSumFields, Me.hidAdditionalWhere.Value, mq)
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

        '' Me.hidAdditionalWhere.Value = Server.UrlEncode(Replace(Me.hidAdditionalWhere.Value, "=", "xxx"))
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
        With CType(e.Item.FindControl("link1"), LinkButton)
            .Text = c.Name
            If c.Name = "ROOT" Then
                .ToolTip = "Nejvyšší datová úroveň DRILL-DOWN šablony"
            End If
        End With

        CType(e.Item.FindControl("pid"), HiddenField).Value = c.PID
        CType(e.Item.FindControl("levelindex"), HiddenField).Value = c.LevelIndex.ToString
        If c.LevelIndex > 1 Then
            e.Item.FindControl("sipka").Visible = True
        Else
            e.Item.FindControl("sipka").Visible = False
        End If

    End Sub
    Private Sub GridExport(strFormat As String)
        With grid1
            .Page.Response.ClearHeaders()
            .Page.Response.Cache.SetCacheability(HttpCacheability.[Private])
            .PageSize = 2000
            .Rebind(False)
            Select Case strFormat
                Case "xls"
                    .radGridOrig.ExportToExcel()
                Case "pdf"
                    With .radGridOrig.ExportSettings.Pdf
                        If grid1.radGridOrig.Columns.Count > 4 Then
                            .PageWidth = Unit.Parse("297mm")
                            .PageHeight = Unit.Parse("210mm")
                        Else
                            .PageHeight = Unit.Parse("297mm")
                            .PageWidth = Unit.Parse("210mm")
                        End If
                    End With
                    .radGridOrig.ExportToPdf()
                Case "doc"
                    .radGridOrig.ExportToWord()
            End Select

        End With
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



    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        basUIMT.RenderQueryCombo(Me.j70ID)
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    
    Private Sub chkIncludeChilds_CheckedChanged(sender As Object, e As EventArgs) Handles chkIncludeChilds.CheckedChanged
        Me.Factory.j03UserBL.SetUserParam("p31_drilldown-includechilds", BO.BAS.GB(Me.chkIncludeChilds.Checked))
        ReloadPage()
    End Sub
End Class