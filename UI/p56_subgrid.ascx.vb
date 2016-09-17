﻿Imports Telerik.Web.UI
Public Class p56_subgrid
    Inherits System.Web.UI.UserControl
    Public Property MasterDataPID As Integer
    Public Property Factory As BL.Factory
    Public Property x29ID As BO.x29IdEnum

    Public Property AllowApproving As Boolean
        Get
            Return cmdApprove.Visible
        End Get
        Set(value As Boolean)
            cmdApprove.Visible = value
            imgApprove.Visible = value
        End Set
    End Property
    
    Public Property IsAllowedCreateTasks As Boolean
        Get
            Return cmdP56_new.Visible
        End Get
        Set(value As Boolean)
            Me.cmdP56_new.Visible = value
            cmdP56_clone.Visible = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("j74id") = ""
            With Factory.j03UserBL
                Dim lisPars As New List(Of String), strKey As String = "p56_subgrid-j74id_" & BO.BAS.GetDataPrefix(Me.x29ID)
                With lisPars
                    .Add("p56_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID))
                    .Add("p56_subgrid-pagesize")
                    .Add("p56_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID))
                    .Add("p56_subgrid-cbxP56Validity")
                    .Add(strKey)
                End With
                .InhaleUserParams(lisPars)
                ViewState("j74id") = .GetUserParam(strKey, "0")

                If ViewState("j74id") = "" Or ViewState("j74id") = "0" Then
                    Me.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(BO.x29IdEnum.p56Task, Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.x29ID))
                    Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p56Task, Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.x29ID))
                    ViewState("j74id") = cJ74.PID
                    .SetUserParam(strKey, ViewState("j74id"))
                End If
                basUI.SelectDropdownlistValue(Me.cbxP56Validity, .GetUserParam("p56_subgrid-cbxP56Validity", "1"))
                basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("p56_subgrid-pagesize", "10"))
                basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("p56_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID)))
            End With


            SetupGridP56()

        End If
    End Sub

    Private Sub SetupGridP56()
        Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))
        If cJ74 Is Nothing Then
            cJ74 = Me.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(BO.x29IdEnum.p56Task, Me.Factory.SysUser.PID, BO.BAS.GetDataPrefix(Me.x29ID))
        End If
        If cJ74.j74ColumnNames.IndexOf("ReceiversInLine") > 0 Then Me.hidReceiversInLine.Value = "1" Else Me.hidReceiversInLine.Value = ""
        If cJ74.j74ColumnNames.IndexOf("Hours_Orig") > 0 Or cJ74.j74ColumnNames.IndexOf("Expenses_Orig") > 0 Then Me.hidTasksWorksheetColumns.Value = "1" Else Me.hidTasksWorksheetColumns.Value = ""
        Me.hidDefaultSorting.Value = cJ74.j74OrderBy
        Me.hidCols.Value = basUIMT.SetupGrid(Me.Factory, Me.gridP56, cJ74, CInt(Me.cbxPaging.SelectedValue), False, True, True)

        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With



    End Sub


    Private Sub gridP56_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gridP56.ItemDataBound
        basUIMT.p56_grid_Handle_ItemDataBound(sender, e, True, True)
        
    End Sub
    Private Sub InhaleTasksQuery(ByRef mq As BO.myQueryP56)
        Select Case Me.x29ID
            Case BO.x29IdEnum.p41Project
                mq.p41ID = MasterDataPID
            Case BO.x29IdEnum.p28Contact
                mq.p28ID = MasterDataPID
            Case BO.x29IdEnum.j02Person
                mq.j02ID = MasterDataPID
        End Select

        mq.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForRead

        Select Case Me.cbxP56Validity.SelectedValue
            Case "1"
                mq.Closed = BO.BooleanQueryMode.NoQuery
            Case "2"
                mq.Closed = BO.BooleanQueryMode.FalseQuery
            Case "3"
                mq.Closed = BO.BooleanQueryMode.TrueQuery
        End Select
        With mq
            .MG_SortString = gridP56.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
            
        End With
    End Sub
    Private Sub gridP56_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridP56.NeedDataSource
        If MasterDataPID = 0 Then Return

        Dim mq As New BO.myQueryP56, intClosed As Integer, intOpened As Integer, bolReceiversInLine As Boolean = False
        InhaleTasksQuery(mq)

        ''If Me.hidReceiversInLine.Value = "1" Then bolReceiversInLine = True

        ''If Me.hidTasksWorksheetColumns.Value = "1" Then
        ''    Dim lis As IEnumerable(Of BO.p56TaskWithWorksheetSum) = Factory.p56TaskBL.GetList_WithWorksheetSum(mq, bolReceiversInLine)
        ''    intClosed = lis.Where(Function(p) p.IsClosed = True).Count
        ''    intOpened = lis.Where(Function(p) p.IsClosed = False).Count
        ''    Select Case Me.cbxP56Validity.SelectedValue
        ''        Case "1"
        ''        Case "2" : lis = lis.Where(Function(p) p.IsClosed = False)
        ''        Case "3" : lis = lis.Where(Function(p) p.IsClosed = True)
        ''    End Select
        ''    gridP56.DataSource = lis
        ''Else
        ''    Dim lis As IEnumerable(Of BO.p56Task) = Factory.p56TaskBL.GetList(mq, bolReceiversInLine)
        ''    intClosed = lis.Where(Function(p) p.IsClosed = True).Count
        ''    intOpened = lis.Where(Function(p) p.IsClosed = False).Count
        ''    Select Case Me.cbxP56Validity.SelectedValue
        ''        Case "1"
        ''        Case "2" : lis = lis.Where(Function(p) p.IsClosed = False)
        ''        Case "3" : lis = lis.Where(Function(p) p.IsClosed = True)
        ''    End Select
        ''    gridP56.DataSource = lis
        ''End If
        Dim dt As DataTable = Me.Factory.p56TaskBL.GetGridDataSource(hidCols.Value, mq, Me.cbxGroupBy.SelectedValue)

        If dt Is Nothing Then
            Return
        Else
            gridP56.DataSourceDataTable = dt
        End If

        Dim strCount As String = intOpened.ToString & "+" & intClosed.ToString
        If intClosed = 0 And intOpened = 0 Then strCount = "0"
        lblHeaderP56.Text = BO.BAS.OM2(lblHeaderP56.Text, strCount)
    End Sub

    Private Sub cbxP56Validity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxP56Validity.SelectedIndexChanged
        Factory.j03UserBL.SetUserParam("p56_subgrid-cbxP56Validity", Me.cbxP56Validity.SelectedValue)
        gridP56.Rebind(False)
    End Sub

    ''Private Sub gridP56_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles gridP56.NeedFooterSource
    ''    footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"

    ''    gridP56.ParseFooterItemString(footerItem, ViewState("footersum"))
    ''End Sub

    Public Sub Rebind(bolKeepSelectedRecord As Boolean)
        gridP56.Rebind(bolKeepSelectedRecord)
    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        With gridP56.radGridOrig.MasterTableView
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
        Factory.j03UserBL.SetUserParam("p56_subgrid-groupby-" & BO.BAS.GetDataPrefix(Me.x29ID), Me.cbxGroupBy.SelectedValue)
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        gridP56.Rebind(True)
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Me.Factory.j03UserBL.SetUserParam("p56_subgrid-pagesize", Me.cbxPaging.SelectedValue)
        SetupGridP56()
        gridP56.Rebind(True)
    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim cJ74 As BO.j74SavedGridColTemplate = Me.Factory.j74SavedGridColTemplateBL.Load(ViewState("j74id"))
        Dim cXLS As New clsExportToXls(Me.Factory)

        Dim mq As New BO.myQueryP56
        InhaleTasksQuery(mq)

        Dim lis As IEnumerable(Of BO.p56TaskWithWorksheetSum) = Me.Factory.p56TaskBL.GetList_WithWorksheetSum(mq)

        Dim strFileName As String = cXLS.ExportGridData(lis, cJ74)
        If strFileName = "" Then
            Response.Write(cXLS.ErrorMessage)
        Else
            Response.Redirect("binaryfile.aspx?tempfile=" & strFileName)
        End If
    End Sub
End Class