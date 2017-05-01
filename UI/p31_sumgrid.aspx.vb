﻿Imports Telerik.Web.UI
Public Class p31_sumgrid
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private _lisDD As List(Of BO.GridColumn) = Nothing
    Private Property _curIsExport As Boolean

    Public ReadOnly Property CurrentJ77ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j77ID.SelectedValue)
        End Get
    End Property
    Private Sub p31_sumgrid_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SiteMenuValue = "p31_pivot"
            Master.neededPermission = BO.x53PermValEnum.GR_P31_Pivot
            hidJ70ID.Value = Request.Item("j70id")

            hidMasterPID.Value = Request.Item("masterpid")
            hidMasterPrefix.Value = Request.Item("masterprefix")

            If hidMasterPrefix.Value <> "" Then
                panQueryByEntity.Visible = True
                Me.MasterRecord.Text = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(hidMasterPrefix.Value), BO.BAS.IsNullInt(hidMasterPID.Value))
                Me.MasterRecord.NavigateUrl = hidMasterPrefix.Value & "_framework.aspx?pid=" & hidMasterPID.Value
                Select Case hidMasterPrefix.Value
                    Case "p41" : imgEntity.ImageUrl = "Images/project.png"
                    Case "j02" : imgEntity.ImageUrl = "Images/person.png"
                    Case "p28" : imgEntity.ImageUrl = "Images/contact.png"
                    Case "p91" : imgEntity.ImageUrl = "Images/invoice.png"
                End Select
            Else
                panQueryByEntity.Visible = False
            End If

            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p31_sumgrid-j77id")
                .Add("p31-j70id")
                .Add("p31_grid-period")
                .Add("periodcombo-custom_query")
                .Add("p31_grid-filter_completesql")
                .Add("p31_sumgrid-pagesize")
                .Add("p31_sumgrid-chkFirstLastCount")
                .Add("p31_grid-tabqueryflag")
            End With


            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                Dim strDefTabQueryFlag As String = Request.Item("p31tabautoquery")
                If strDefTabQueryFlag = "" Then strDefTabQueryFlag = Request.Item("tab")
                If strDefTabQueryFlag = "" Then strDefTabQueryFlag = .GetUserParam("p31_grid-tabqueryflag", "p31")
                basUI.SelectDropdownlistValue(Me.cbxTabQueryFlag, strDefTabQueryFlag)

                basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("p31_sumgrid-pagesize", "100"))
                Me.chkFirstLastCount.Checked = BO.BAS.BG(.GetUserParam("p31_sumgrid-chkFirstLastCount", "1"))
                SetupJ70Combo(BO.BAS.IsNullInt(.GetUserParam("p31-j70id")))
                period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("p31_grid-period")

                SetupJ77Combo(.GetUserParam("p31_sumgrid-j77id"))

                
                hidGridColumnSql.Value = .GetUserParam("p31_grid-filter_completesql")

            End With


            RefreshRecord()
            RenderQueryInfo()

        End If
    End Sub



    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            Select Case hidMasterPrefix.Value
                Case "p41"
                    .p41ID = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case "p28"
                    .p28ID_Client = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case "j02"
                    .j02ID = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case "p56"
                    .p56IDs = New List(Of Integer)
                    .p56IDs.Add(BO.BAS.IsNullInt(hidMasterPID.Value))
                Case "p91"
                    .p91ID = BO.BAS.IsNullInt(hidMasterPID.Value)
                Case Else

            End Select
            .j70ID = BO.BAS.IsNullInt(Me.j70ID.SelectedValue)

            '.ColumnFilteringExpression = grid1.GetFilterExpressionCompleteSql()
            .MG_AdditionalSqlFROM = Me.hidFrom.Value
            '.MG_GridSqlColumns = Me.hidCols.Value

            .SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead

            .DateFrom = period1.DateFrom
            .DateUntil = period1.DateUntil
            .ColumnFilteringExpression = hidGridColumnSql.Value
            .MG_AdditionalSqlWHERE = Me.hidMasterAW.Value
            .TabAutoQuery = Me.cbxTabQueryFlag.SelectedValue

        End With
    End Sub
    

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound

        If _curIsExport Then
            If TypeOf e.Item Is GridHeaderItem Then
                e.Item.BackColor = Drawing.Color.Silver
            End If
            If TypeOf e.Item Is GridGroupHeaderItem Then
                e.Item.BackColor = Drawing.Color.WhiteSmoke
            End If
            If TypeOf e.Item Is GridDataItem Or TypeOf e.Item Is GridHeaderItem Then
                ''e.Item.Cells(0).Visible = False
            End If
        Else
            If Not TypeOf e.Item Is GridDataItem Then Return
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
            ''Dim cRec As DataRowView = CType(e.Item.DataItem, DataRowView)
            With dataItem("systemcolumn")
                .Text = "<a href='javascript:go2grid(" + Chr(34) + dataItem.GetDataKeyValue("pid").ToString + Chr(34) + ")'><img src='Images/worksheet.png' title='Přejít na přehled worksheet záznamů'></a>"
            End With
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.CurrentDD1 Is Nothing Then Return

        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = cbxPaging.SelectedValue
            If _curIsExport Then .MG_PageSize = 2000
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With
        InhaleMyQuery(mq)
        Dim colDD2 As BO.GridColumn = Me.CurrentDD2
        If Not colDD2 Is Nothing Then
            If Me.CurrentDD1.ColumnName = colDD2.ColumnName Then colDD2 = Nothing

        End If
        hidSGF.Value = Me.CurrentDD1.ColumnName

        If Not colDD2 Is Nothing Then
            hidSGF.Value += "|" & colDD2.ColumnName

        End If

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownGridSource(Me.CurrentDD1, colDD2, Me.CurrentSumFields_PIVOT, Me.CurrentColFields_PIVOT, "", mq)
        grid1.VirtualRowCount = dt.Rows.Count
        grid1.DataSourceDataTable = dt

        If Request.Item("pivot") = "1" Then
            dt.WriteXmlSchema(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_schema.xml", True)
            dt.WriteXml(Master.Factory.x35GlobalParam.TempFolder & "\" & Master.Factory.SysUser.PID.ToString & "_pivot_data.xml", True)
            Server.Transfer("p31_sumgrid_pivot.aspx")
        End If
        

    End Sub
    Private ReadOnly Property lisDD As List(Of BO.GridColumn)
        Get
            If _lisDD Is Nothing Then _lisDD = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(BO.x29IdEnum.p31Worksheet)
            Return _lisDD
        End Get
    End Property
    Private ReadOnly Property CurrentDD1 As BO.GridColumn
        Get
            If hidDD1.Value = "" Then Return Nothing
            Return Me.lisDD.Where(Function(p) p.ColumnName = hidDD1.Value).First()
        End Get
    End Property
    Private ReadOnly Property CurrentDD2 As BO.GridColumn
        Get
            If hidDD2.Value = "" Then Return Nothing
            Return lisDD.Where(Function(p) p.ColumnName = Me.hidDD2.Value).First()
        End Get
    End Property
    Private ReadOnly Property CurrentSumFields_PIVOT As List(Of BO.PivotSumField)
        Get
            Dim lis As New List(Of BO.PivotSumField), a() As String = Split(hidSumCols.Value, ",")
            If hidSumCols.Value = "" Then Return lis
            For i As Integer = 0 To UBound(a)
                lis.Add(New BO.PivotSumField(CType(a(i), BO.PivotSumFieldType)))
            Next
            Return lis
        End Get
    End Property
    Private ReadOnly Property CurrentColFields_PIVOT As List(Of BO.GridColumn)
        Get
            Dim lis As New List(Of BO.GridColumn)
            If hidAddCols.Value = "" Then Return lis
            Dim a() As String = Split(hidAddCols.Value, ",")
            For i As Integer = 0 To UBound(a)
                Dim b() As String = Split(a(i), "-")
                Dim c As BO.GridColumn = Me.lisDD.Where(Function(p) p.ColumnName = b(0)).First
                If UBound(b) > 0 Then
                    c.MyTag = b(1)
                Else
                    c.MyTag = "all"
                End If

                lis.Add(c)
            Next
            Return lis
        End Get
    End Property


    

    
    Private Sub RefreshRecord()
        If Me.CurrentJ77ID = 0 Then
            Dim lisPars As New List(Of String)
            With lisPars
                .Add(hidMasterPrefix.Value & "p31_sumgrid-dd1")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-dd2")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-sumcols")
                .Add(hidMasterPrefix.Value & "p31_sumgrid-addcols")
            End With
            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)

                Dim strDefDD1 As String = ""
                Select Case Me.hidMasterPrefix.Value
                    Case "j02" : strDefDD1 = "ClientName"
                    Case Else
                        strDefDD1 = "Person"
                End Select
                hidDD1.Value = .GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-dd1", strDefDD1)
                hidDD2.Value = .GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-dd2")
                hidSumCols.Value = .GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-sumcols", "1")
                hidAddCols.Value = .GetUserParam(hidMasterPrefix.Value & "p31_sumgrid-addcols")
                
            End With

        Else
            Dim cRec As BO.j77WorksheetStatTemplate = Master.Factory.j77WorksheetStatTemplateBL.Load(Me.CurrentJ77ID)
            With cRec
                hidDD1.Value = .j77DD1
                hidDD2.Value = .j77DD2
                hidSumCols.Value = .j77SumFields
                hidAddCols.Value = .j77ColFields
                basUI.SelectDropdownlistValue(Me.j70ID, .j70ID.ToString)
                If Request.Item("p31tabautoquery") = "" Then
                    basUI.SelectDropdownlistValue(Me.cbxTabQueryFlag, .j77TabQueryFlag)
                End If

            End With
        End If
        SetupGrid()
        grid1.Rebind(False)
        If Request.Item("pid") <> "" Then
            SelectGridRow(Request.Item("pid"))
        End If
    End Sub
    Private Sub SelectGridRow(strStatPID As String)
        Dim x As Integer = 0
        With grid1.radGridOrig
            For Each it As GridDataItem In .MasterTableView.Items
                If it.GetDataKeyValue("pid") = strStatPID Then
                    it.Selected = True
                    Return
                End If
                x += 1
                If x > 1000 Then Return
            Next
        End With
    End Sub

    

    Private Sub SetupGrid()
        If Me.CurrentDD1 Is Nothing Then Return

        With grid1
            .ClearColumns()
            .PageSize = cbxPaging.SelectedValue
            .radGridOrig.ShowFooter = True
            .DataKeyNames = "pid"
            .AllowMultiSelect = False
            .AddSystemColumn(16)

            .AddColumn(Me.CurrentDD1.ColumnName, Me.CurrentDD1.ColumnHeader)
            If Not Me.CurrentDD2 Is Nothing Then
                .AddColumn(Me.CurrentDD2.ColumnName, Me.CurrentDD2.ColumnHeader)
            End If
            If Not Me.CurrentColFields_PIVOT Is Nothing Then
                For Each c In Me.CurrentColFields_PIVOT
                    .AddColumn("col" & c.ColumnName, c.ColumnHeader, c.ColumnType, , , , , , False)
                Next
            End If
            If Not Me.CurrentSumFields_PIVOT Is Nothing Then
                For Each c In Me.CurrentSumFields_PIVOT
                    .AddColumn("sum" & c.FieldTypeID.ToString, c.Caption, BO.cfENUM.Numeric2, , , , , True, False)
                Next
            End If

            .AddColumn("RecsCount", "Počet", BO.cfENUM.Numeric0, True, , , , True)
            If chkFirstLastCount.Checked Then
                .AddColumn("RecFirst", "První", BO.cfENUM.DateOnly, True)
                .AddColumn("RecLast", "Poslední", BO.cfENUM.DateOnly, True)
            End If
        End With
        If Not Me.CurrentDD2 Is Nothing Then
            SetupGrouping(Me.CurrentDD1.ColumnName, Me.CurrentDD1.ColumnHeader)
        End If
    End Sub





  

    Private Sub RenderQueryInfo()
       
        If hidGridColumnSql.Value <> "" Then
            lblQuery.Text = BO.BAS.OM4(lblQuery.Text, "[Sloupcový filtr zdrojového přehledu]", "; ")
        End If
        If lblQuery.Text <> "" Then panQueryByEntity.Visible = True
    End Sub

    
    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        grid1.radGridOrig.GroupingSettings.RetainGroupFootersVisibility = True
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
            .GroupsDefaultExpanded = True
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strFieldHeader
            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)
        End With

    End Sub

    Private Sub GridExport(strFormat As String)
        _curIsExport = True
        basUIMT.Handle_GridTelerikExport(grid1, strFormat)


    End Sub

  

   

    

    

   

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        grid1.Rebind(False)
    End Sub

    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, BO.x29IdEnum.p31Worksheet)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Pojmenovaný filtr--")
        
        basUI.SelectDropdownlistValue(Me.j70ID, intDef.ToString)
        
    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        hidJ70ID.Value = Me.j70ID.SelectedValue
        Master.Factory.j03UserBL.SetUserParam("p31-j70id", Me.j70ID.SelectedValue)
        grid1.Rebind(False)
    End Sub

    Private Sub p31_sumgrid_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        With Me.j70ID
            If .SelectedIndex > 0 Then
                hidJ70ID.Value = .SelectedValue
                .ToolTip = .SelectedItem.Text
                Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & .SelectedValue
                Me.clue_query.Visible = True
            Else
                Me.clue_query.Visible = False
                hidJ70ID.Value = ""
            End If
        End With

        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
        basUIMT.RenderQueryCombo(Me.j70ID)
        With Me.cbxTabQueryFlag
            If .SelectedIndex > 0 Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With

    End Sub

    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        Select Case hidHardRefreshFlag.Value
            Case "pdf"
                GridExport("pdf")
            Case "xls"
                GridExport("xls")
            Case "doc"
                GridExport("doc")
            Case "sumgrid_designer"
                ReloadPage()
            Case Else
                grid1.Rebind(False)
        End Select

        hidHardRefreshFlag.Value = ""
        hidHardRefreshPID.Value = ""
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_sumgrid-pagesize", Me.cbxPaging.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub chkFirstLastCount_CheckedChanged(sender As Object, e As EventArgs) Handles chkFirstLastCount.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p31_sumgrid-chkFirstLastCount", BO.BAS.GB(Me.chkFirstLastCount.Checked))
        ReloadPage()
    End Sub

    
    
    Private Sub SetupJ77Combo(strDef As String)
        Dim mq As BO.myQuery = Nothing
        Dim lisJ77 As IEnumerable(Of BO.j77WorksheetStatTemplate) = Master.Factory.j77WorksheetStatTemplateBL.GetList(mq)
        j77ID.DataSource = lisJ77
        j77ID.DataBind()
        Me.j77ID.Items.Insert(0, "--Výchozí WORKSHEET statistika--")
        basUI.SelectDropdownlistValue(Me.j77ID, strDef)
    End Sub

    Private Sub j77ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j77ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_sumgrid-j77id", Me.j77ID.SelectedValue)
        ReloadPage()
    End Sub
    Private Sub ReloadPage()
        Dim s As String = "p31_sumgrid.aspx"
        If Me.hidMasterPrefix.Value <> "" Then
            s += "?masterprefix=" & hidMasterPrefix.Value & "&masterpid=" & Me.hidMasterPID.Value
        End If
        Response.Redirect(s)
    End Sub

    Private Sub cbxTabQueryFlag_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxTabQueryFlag.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-tabqueryflag", Me.cbxTabQueryFlag.SelectedValue)
        grid1.Rebind(False)
    End Sub
End Class