Imports Telerik.Web.UI
Public Class p31_drilldown
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private _lisCols As List(Of ddColumn)


    Private Class ddColumn
        Public Property Header As String
        Public Property ColumnFormat As BO.cfENUM
        Public Property SqlSELECT As String
        Public Property SqlGROUPBY As String
        Public Property Total As Double = 0

        Public Sub New(strHeader As String, colFormat As BO.cfENUM, strSqlSelect As String, strSqlGroupBy As String)
            Me.Header = strHeader
            Me.ColumnFormat = colFormat
            Me.SqlSELECT = strSqlSelect
            Me.SqlGROUPBY = strSqlGroupBy
        End Sub
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

    
    Private Sub SetupGrid()
        Dim cRec As BO.j75DrillDownTemplate = Master.Factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)
        Dim lisJ76 As IEnumerable(Of BO.j76DrillDownTemplate_Item) = Master.Factory.j75DrillDownTemplateBL.GetList_j76(cRec.PID)
        Dim lisLevel As List(Of BO.PivotRowColumnField) = Master.Factory.j75DrillDownTemplateBL.LevelPallete()
        Dim lisAllSumCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()
        Dim level1 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level1).First



        Me.hidGroup1.Value = level1.FieldTypeID.ToString
        For Each c In lisJ76.Where(Function(p) p.j76Level = 1)
            Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
            If Not col Is Nothing Then
                Me.hidCols1.Value = BO.BAS.OM4(hidCols1.Value, col.FieldTypeID.ToString)
            End If
        Next

        If Not cRec.j75Level2 Is Nothing Then
            Dim level2 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level2).First
           
            Me.hidGroup2.Value = level2.FieldTypeID.ToString
            For Each c In lisJ76.Where(Function(p) p.j76Level = 2)
                Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
                If Not col Is Nothing Then
                    Me.hidCols2.Value = BO.BAS.OM4(hidCols2.Value, col.FieldTypeID.ToString)
                End If
            Next
        End If
        If Not cRec.j75Level3 Is Nothing Then
            Dim level3 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level3).First

            Me.hidGroup3.Value = level3.FieldTypeID.ToString
            For Each c In lisJ76.Where(Function(p) p.j76Level = 3)
                Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
                If Not col Is Nothing Then
                    Me.hidCols3.Value = BO.BAS.OM4(hidCols3.Value, col.FieldTypeID.ToString)
                End If
            Next
        End If
        If Not cRec.j75Level4 Is Nothing Then
            Dim level4 As BO.PivotRowColumnField = lisLevel.Where(Function(p) p.FieldType = cRec.j75Level3).First

            Me.hidGroup4.Value = level4.FieldTypeID.ToString
            For Each c In lisJ76.Where(Function(p) p.j76Level = 3)
                Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
                If Not col Is Nothing Then
                    Me.hidCols3.Value = BO.BAS.OM4(hidCols3.Value, col.FieldTypeID.ToString)
                End If
            Next
        End If


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

    Private Sub Handle_DrillDown(strPID As String, strLevel As String)

        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim groupCurCol As BO.PivotRowColumnField = Nothing
        Dim strCurCols As String = hidCols2.Value

        Dim groupCol1 As New BO.PivotRowColumnField(CType(Me.hidGroup1.Value, BO.PivotRowColumnFieldType))

        Dim groupCol2 As BO.PivotRowColumnField = Nothing
        If hidGroup2.Value <> "" Then groupCol2 = New BO.PivotRowColumnField(CType(Me.hidGroup2.Value, BO.PivotRowColumnFieldType))
        Dim groupCol3 As BO.PivotRowColumnField = Nothing
        If hidGroup3.Value <> "" Then groupCol3 = New BO.PivotRowColumnField(CType(Me.hidGroup3.Value, BO.PivotRowColumnFieldType))
        Dim groupCol4 As BO.PivotRowColumnField = Nothing
        If Me.hidGroup4.Value <> "" Then groupCol4 = New BO.PivotRowColumnField(CType(Me.hidGroup4.Value, BO.PivotRowColumnFieldType))

        Dim strParentSqlWhere As String = ""

        Select Case strLevel
            Case "level2"
                groupCurCol = groupCol2
                strCurCols = hidCols2.Value
                hidSelPID1.Value = strPID
                strParentSqlWhere = groupCol1.GroupByField & "=" & hidSelPID1.Value
            Case "level3"
                groupCurCol = groupCol3
                strCurCols = hidCols3.Value
                hidSelPID2.Value = strPID
                strParentSqlWhere = groupCol1.GroupByField & "=" & hidSelPID1.Value & " AND " & groupCol2.GroupByField & "=" & hidSelPID2.Value
            Case "level4"
                groupCurCol = groupCol4
                strCurCols = hidCols4.Value
                hidSelPID3.Value = strPID
                strParentSqlWhere = groupCol1.GroupByField & "=" & hidSelPID1.Value & " AND " & groupCol2.GroupByField & "=" & hidSelPID2.Value & " AND " & groupCol3.GroupByField & "=" & hidSelPID3.Value
            Case Else
                Return
        End Select


        Dim lis As New List(Of BO.PivotSumField)
        For Each s In Split(strCurCols, ",")
            lis.Add(New BO.PivotSumField(CType(s, BO.PivotSumFieldType)))
        Next
        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(groupCurCol, lis, strParentSqlWhere, mq)

    End Sub

    Private Sub Handle_MasterTable()
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

        Dim groupCol As New BO.PivotRowColumnField(CType(Me.hidGroup1.Value, BO.PivotRowColumnFieldType))
        Dim lis As New List(Of BO.PivotSumField)
        For Each s In Split(Me.hidCols1.Value, ",")
            lis.Add(New BO.PivotSumField(CType(s, BO.PivotSumFieldType)))
        Next
        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(groupCol, lis, "", mq)
        hidSelPID1.Value = "" : hidSelPID2.Value = "" : hidSelPID3.Value = ""

        rpData.DataSource = dt
        rpData.DataBind()

    End Sub

    
    
    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click
        FillData(1)
    End Sub

    

    Private Sub FillData(intLevel As Integer)
        Dim mq As New BO.myQueryP31
        InhaleMyQuery(mq)

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


        _lisCols = New List(Of ddColumn)
        Dim lisSums As New List(Of BO.PivotSumField)
        _lisCols.Add(New ddColumn(level.Caption, BO.cfENUM.AnyString, level.SelectField, level.GroupByField))

        Dim lisJ76 As IEnumerable(Of BO.j76DrillDownTemplate_Item) = Master.Factory.j75DrillDownTemplateBL.GetList_j76(Me.CurrentJ75ID)
        For Each c In lisJ76.Where(Function(p) p.j76Level = intLevel)
            Dim col As BO.PivotSumField = lisAllSumCols.First(Function(p) p.FieldTypeID = c.j76PivotSumFieldType)
            If Not col Is Nothing Then
                _lisCols.Add(New ddColumn(col.Caption, col.ColumnType, col.SelectField, ""))
                lisSums.Add(col)
            End If
        Next

        rpH.DataSource = _lisCols
        rpH.DataBind()
        

        Dim dt As DataTable = Master.Factory.p31WorksheetBL.GetDrillDownDatasource(level, lisSums, "", mq)

        rpData.DataSource = dt
        rpData.DataBind()

        For Each dbRow In dt.Rows
            For i As Integer = 1 To _lisCols.Count - 1
                If dbRow(i + 1).GetType.ToString = "System.Double" Then
                    If Not dbRow(i + 1) Is System.DBNull.Value Then
                        _lisCols(i).Total += dbRow(i + 1)
                    End If
                End If
            Next
        Next

        rpF.DataSource = _lisCols
        rpF.DataBind()
    End Sub

    Private Sub rpH_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpH.ItemDataBound
        Dim c As ddColumn = CType(e.Item.DataItem, ddColumn)
        CType(e.Item.FindControl("lbl1"), Label).Text = c.Header

    End Sub

    Private Sub rpData_ItemCreated(sender As Object, e As RepeaterItemEventArgs) Handles rpData.ItemCreated
        AddHandler CType(e.Item.FindControl("rp1"), Repeater).ItemDataBound, AddressOf Me.rp1_ItemDataBound
    End Sub

    Private Sub rpData_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpData.ItemDataBound
        Dim dbRow As DataRowView = CType(e.Item.DataItem, DataRowView)
        CType(e.Item.FindControl("pid"), HiddenField).Value = dbRow("pid").ToString

        With CType(e.Item.FindControl("lbl0"), Label)
            If Not dbRow(1) Is System.DBNull.Value Then .Text = dbRow(1)
        End With
        Dim lis As New List(Of Object)
        For i As Integer = 1 To _lisCols.Count - 1
            lis.Add(dbRow(i + 1))
        Next

        With CType(e.Item.FindControl("rp1"), Repeater)
            .DataSource = lis
            .DataBind()
        End With
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Dim val1 As Object = e.Item.DataItem
        With CType(e.Item.FindControl("lbl1"), Label)
            If val1 Is System.DBNull.Value Then
            Else
                Select Case val1.GetType.ToString
                    Case "System.Double"
                        .Text = BO.BAS.FN2(val1)
                        '.Text = val1.ToString
                    Case "System.DateTime"
                        .Text = BO.BAS.FD(val1, True)
                    Case Else
                        .Text = val1.ToString
                End Select

            End If

        End With

    End Sub

    Private Sub rpF_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpF.ItemDataBound
        Dim c As ddColumn = CType(e.Item.DataItem, ddColumn)
        If c.ColumnFormat = BO.cfENUM.Numeric2 Then
            CType(e.Item.FindControl("lbl1"), Label).Text = BO.BAS.FN(c.Total)
        End If


    End Sub
End Class