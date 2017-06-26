Imports Telerik.Web.UI

Public Class o23_queue
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            Me.hidMasterPID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return Me.hidMasterPrefix.Value
        End Get
        Set(value As String)
            Me.hidMasterPrefix.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentMasterX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
        End Get
    End Property
    Public Property CurrentMasterGUID As String
        Get
            Return Me.hidMasterGUID.Value
        End Get
        Set(value As String)
            Me.hidMasterGUID.Value = value
        End Set
    End Property
    
    
    Private Sub o23_queue_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        designer1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            Dim lisPars As New List(Of String)
            With Master
                .HeaderIcon = "Images/notepad_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Fronta dokumentů, které čekají na rozřazení"

                Me.CurrentMasterPrefix = Request.Item("masterprefix")
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                Me.CurrentMasterGUID = Request.Item("masterguid")
                If Me.CurrentMasterPrefix = "" Or (Me.CurrentMasterPID = 0 And Me.CurrentMasterGUID = "") Then
                    .StopPage("masterprefix or masterpid missing...")
                End If
                Dim s As String = "Přiřadit vybrané dokumenty"
                Select Case Me.CurrentMasterX29ID
                    Case BO.x29IdEnum.p41Project : s = "Přiřadit vybrané dokumenty k projektu"
                    Case BO.x29IdEnum.p41Project : s = "Přiřadit vybrané dokumenty ke klientovi"
                    Case BO.x29IdEnum.p31Worksheet : s = "Přiřadit vybrané dokumenty k worksheet úkonu"
                End Select
                .AddToolbarButton(s, "save", , "Images/save.png")
                With lisPars
                    .Add("o23_queue-pagesize")
                    .Add(designer1.x36Key)
                    .Add("o23_queue-groupby")
                    .Add("o23_queue-sort")
                    .Add("o23_queue-o24id")
                    .Add("periodcombo-custom_query")
                    .Add("o23_queue-period")
                End With

                Me.o24ID.DataSource = .Factory.o24NotepadTypeBL.GetList(New BO.myQuery)
                Me.o24ID.DataBind()
                Me.o24ID.Items.Insert(0, "--Filtrovat podle typu dokumentu--")


            End With
            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("o23_queue-period")
                basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("o23_queue-pagesize", "20"))
                basUI.SelectDropdownlistValue(Me.o24ID, IIf(Request.Item("o24id") = "", .GetUserParam("o23_queue-o24id"), Request.Item("o24id")))

                basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("o23_queue-groupby"))
                designer1.RefreshData(BO.BAS.IsNullInt(.GetUserParam(designer1.x36Key)))

            End With

            RecalcVirtualRowCount()
            SetupGrid()
            Me.EntityRecord.Text = Master.Factory.GetRecordCaption(Me.CurrentMasterX29ID, Me.CurrentMasterPID)
            Me.EntityName.Text = BO.BAS.GetX29EntityAlias(Me.CurrentMasterX29ID, False)
        End If

    End Sub


    Private Sub RecalcVirtualRowCount()
        Dim mq As New BO.myQueryO23
        InhaleMyQuery(mq)
        grid1.VirtualRowCount = Master.Factory.o23NotepadBL.GetVirtualCount(mq)

        grid1.radGridOrig.CurrentPageIndex = 0

    End Sub


    Private Sub SetupGrid()
        Dim cJ70 As BO.j70QueryTemplate = Master.Factory.j70QueryTemplateBL.Load(designer1.CurrentJ70ID)
        Me.hidCols.Value = basUIMT.SetupDataGrid(Master.Factory, Me.grid1, cJ70, BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue), True, True, True)
        Me.hidDefaultSorting.Value = cJ70.j70OrderBy
       
        With grid1
            .radGridOrig.ShowFooter = False
            .radGridOrig.SelectedItemStyle.BackColor = Drawing.Color.Red
        End With
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.o23_grid_Handle_ItemDataBound(sender, e, True)

    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryO23
        With mq
            .MG_PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)
            .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
            .MG_GridSqlColumns = Me.hidCols.Value
        End With
        InhaleMyQuery(mq)

        Dim bolInhaleReceiversInLine As Boolean = True

        ''Dim lis As IEnumerable(Of BO.o23NotepadGrid) = Master.Factory.o23NotepadBL.GetList4Grid(mq)
        Dim dt As DataTable = Master.Factory.o23NotepadBL.GetGridDataSource(mq)
        If dt Is Nothing Then
            Master.Notify(Master.Factory.o23NotepadBL.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            grid1.DataSourceDataTable = dt
        End If




    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryO23)
        With mq
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()
            If Me.hidDefaultSorting.Value <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.hidDefaultSorting.Value
                Else
                    .MG_SortString = Me.hidDefaultSorting.Value & "," & .MG_SortString
                End If
            End If
            If Me.cbxGroupBy.SelectedValue <> "" Then
                If .MG_SortString = "" Then
                    .MG_SortString = Me.cbxGroupBy.SelectedValue
                Else
                    .MG_SortString = Me.cbxGroupBy.SelectedValue & "," & .MG_SortString
                End If
            End If
            .o24ID = BO.BAS.IsNullInt(o24ID.SelectedValue)
            .DateFrom = period1.DateFrom
            .DateUntil = period1.DateUntil
            .Closed = BO.BooleanQueryMode.FalseQuery

            .SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead

            Select Case Me.CurrentMasterX29ID
                Case BO.x29IdEnum.p41Project
                    .QuickQuery = BO.myQueryO23_QuickQuery.Bind2ProjectWait
                Case BO.x29IdEnum.p28Contact
                    .QuickQuery = BO.myQueryO23_QuickQuery.Bind2ClientWait
                Case BO.x29IdEnum.p31Worksheet
                    .QuickQuery = BO.myQueryO23_QuickQuery.Bind2WorksheetWait
                Case BO.x29IdEnum.j02Person
                    .QuickQuery = BO.myQueryO23_QuickQuery.Bind2PersonWait
                Case BO.x29IdEnum.p91Invoice
                    .QuickQuery = BO.myQueryO23_QuickQuery.Bind2InvoiceWait
                Case BO.x29IdEnum.p56Task

            End Select
        End With

    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("o23_queue-pagesize", Me.cbxPaging.SelectedValue)
        SetupGrid()
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        grid1.Rebind(False)
    End Sub

    
    Private Sub o24ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles o24ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("o23_queue-o24id", Me.o24ID.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
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

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim o23ids As List(Of Integer) = grid1.GetSelectedPIDs()
            If o23ids.Count = 0 Then
                Master.Notify("Musíte vybrat dokument.", NotifyLevel.WarningMessage)
                Return
            End If
            Dim errs As New List(Of String), x As Integer

            For Each intO23ID As Integer In o23ids
                With Master.Factory.o23NotepadBL
                    Dim cRec As BO.o23Notepad = .Load(intO23ID)
                    Select Case Me.CurrentMasterX29ID
                        Case BO.x29IdEnum.p41Project
                            cRec.p41ID = Me.CurrentMasterPID
                        Case BO.x29IdEnum.p28Contact
                            cRec.p28ID = Me.CurrentMasterPID
                        Case BO.x29IdEnum.p91Invoice
                            cRec.p91ID = Me.CurrentMasterPID
                        Case BO.x29IdEnum.p56Task
                            cRec.p56ID = Me.CurrentMasterPID
                        Case BO.x29IdEnum.p31Worksheet
                            cRec.p31ID = Me.CurrentMasterPID
                            cRec.o23GUID = Me.CurrentMasterGUID
                    End Select
                    If Not .Save(cRec, "", Nothing, Nothing) Then
                        errs.Add("#" & x.ToString & " - " & cRec.o23Code & ": " & .ErrorMessage)
                    End If
                End With
                x += 1
            Next
            If errs.Count = 0 Then

                Master.CloseAndRefreshParent("o23-queue")
            Else
                Master.Notify(String.Join("<hr>", errs), NotifyLevel.ErrorMessage)
            End If
        End If
    End Sub

    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("o23_queue-groupby", Me.cbxGroupBy.SelectedValue)
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With
        ReloadPage()
    End Sub

   

    Private Sub o23_queue_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.o24ID.SelectedIndex > 0 Then
            Me.o24ID.BackColor = Drawing.Color.Red
        Else
            Me.o24ID.BackColor = Nothing
        End If
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("o23_queue-period", Me.period1.SelectedValue)
        ReloadPage()
    End Sub

    
End Class