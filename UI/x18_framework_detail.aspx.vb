Imports Telerik.Web.UI
Public Class x18_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Private Property _curJ74 As BO.j74SavedGridColTemplate
    Private Property _needFilterIsChanged As Boolean = False

    Private Sub x18_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Public ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.tabs1.SelectedTab.Value)
        End Get
    End Property
    Public ReadOnly Property CurrentPrefix As String
        Get
            Return Me.tabs1.SelectedTab.Value
        End Get
    End Property
    Public ReadOnly Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74id.SelectedValue)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                With .Factory.j03UserBL
                    .InhaleUserParams("x18_framework_detail-tab")
                    If Not tabs1.FindTabByValue(.GetUserParam("x18_framework_detail-tab", "p41")) Is Nothing Then
                        tabs1.FindTabByValue(.GetUserParam("x18_framework_detail-tab", "p41")).Selected = True
                    End If
                End With
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("x18_framework_detail-pid")
                    .Add("x18_framework_detail-" & Me.CurrentPrefix & "-j74id")
                    .Add("x18_framework_detail-" & Me.CurrentPrefix & "-filter_setting")
                    .Add("x18_framework_detail-" & Me.CurrentPrefix & "-filter_sql")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                End With
                With .Factory.j03UserBL
                    If Master.DataPID = 0 Then
                        Master.DataPID = BO.BAS.IsNullInt(.GetUserParam("x18_framework_detail-pid", "O"))
                        If Master.DataPID = 0 Then Response.Redirect("blank.aspx")
                    Else
                        If Master.DataPID <> BO.BAS.IsNullInt(.GetUserParam("x18_framework_detail-pid", "O")) Then
                            .SetUserParam("x18_framework_detail-pid", Master.DataPID.ToString)
                        End If
                    End If
                    Dim intJ74ID As Integer = BO.BAS.IsNullInt(.GetUserParam("x18_framework_detail-" & Me.CurrentPrefix & "-j74id"))
                    If intJ74ID = 0 Then
                        If Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID) Then
                            _curJ74 = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
                            .SetUserParam("x18_framework_detail-" & Me.CurrentPrefix & "-j74id", _curJ74.PID)
                        End If
                    End If
                    SetupJ74Combo(intJ74ID)
                    SetupGrid(.GetUserParam("x18_framework_detail-" & Me.CurrentPrefix & "-filter_setting"), .GetUserParam("x18_framework_detail-" & Me.CurrentPrefix & "-filter_sql"))
                End With

            End With

            RefreshRecord()

        End If
    End Sub


    Private Sub RefreshRecord()
        Dim cRec As BO.x25EntityField_ComboValue = Master.Factory.x25EntityField_ComboValueBL.Load(Master.DataPID)
        With cRec
            Me.x25Name.Text = .x25Name
            Me.x23Name.Text = .x23Name
            Me.Timestamp.Text = .Timestamp
            cmdNewWindow.NavigateUrl = "x18_framework.aspx?blankwindow=1&pid=" & .PID.ToString & "&title=" & .NameWithComboName
        End With
        Dim pars As New List(Of BO.PluginDbParameter)
        pars.Add(New BO.PluginDbParameter("x25id", Master.DataPID))
        Dim dt As DataTable = Master.Factory.pluginBL.GetDataTable("select a.x29ID,left(min(b.x29TableName),3) as Prefix,COUNT(*) as Pocet FROM x19EntityCategory_Binding a INNER JOIN x29Entity b ON a.x29ID=b.x29ID WHERE a.x25ID=@x25id GROUP BY a.x29ID", pars)
        For Each dbRow As DataRow In dt.Rows
            If Not tabs1.FindTabByValue(dbRow.Item("Prefix")) Is Nothing Then
                tabs1.FindTabByValue(dbRow.Item("Prefix")).Text += "<span class='badge1'>" & dbRow.Item("Pocet").ToString & "</span>"
            End If
        Next

    End Sub

    Private Sub tabs1_TabClick(sender As Object, e As Telerik.Web.UI.RadTabStripEventArgs) Handles tabs1.TabClick
        Master.Factory.j03UserBL.SetUserParam("x18_framework_detail-tab", Me.tabs1.SelectedTab.Value)
        ReloadPage()
    End Sub
    Private Sub ReloadPage()
        Response.Redirect("x18_framework_detail.aspx", True)
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = _curJ74
            If cJ74 Is Nothing Then
                cJ74 = .LoadSystemTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
                If Not cJ74 Is Nothing Then
                    SetupJ74Combo(cJ74.PID)
                End If
            End If
            Me.hidDefaultSorting.Value = cJ74.j74OrderBy
            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 20, True, True, False, strFilterSetting, strFilterExpression)
        End With
        With grid1
            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p91Invoice
                    .radGridOrig.ShowFooter = True
                Case Else
                    .radGridOrig.ShowFooter = False
            End Select

        End With

    End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True

    End Sub



    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                basUIMT.p41_grid_Handle_ItemDataBound(sender, e, False)
            Case BO.x29IdEnum.p28Contact
                basUIMT.p28_grid_Handle_ItemDataBound(sender, e, False)
            Case BO.x29IdEnum.o23Notepad
                basUIMT.o23_grid_Handle_ItemDataBound(sender, e, False, False)
            Case BO.x29IdEnum.p56Task
                basUIMT.p56_grid_Handle_ItemDataBound(sender, e, False)
            Case BO.x29IdEnum.j02Person
                basUIMT.j02_grid_Handle_ItemDataBound(sender, e)
            Case BO.x29IdEnum.p91Invoice
                basUIMT.p91_grid_Handle_ItemDataBound(sender, e, False)
        End Select

    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam(Me.CurrentPrefix + "_framework-filter_setting", grid1.GetFilterSetting())
                .SetUserParam(Me.CurrentPrefix + "_framework-filter_sql", grid1.GetFilterExpression())
            End With
            'RecalcVirtualRowCount()
        End If
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim mq As New BO.myQueryP41
                With mq
                    .Closed = BO.BooleanQueryMode.NoQuery
                    .x25ID = Master.DataPID
                    .MG_PageSize = 20
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                grid1.DataSource = Master.Factory.p41ProjectBL.GetList(mq)
            Case BO.x29IdEnum.p28Contact
                Dim mq As New BO.myQueryP28
                With mq
                    .Closed = BO.BooleanQueryMode.NoQuery
                    .x25ID = Master.DataPID
                    .MG_PageSize = 20
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                grid1.DataSource = Master.Factory.p28ContactBL.GetList(mq)
            Case BO.x29IdEnum.p56Task
                Dim mq As New BO.myQueryP56
                With mq
                    .Closed = BO.BooleanQueryMode.NoQuery
                    .x25ID = Master.DataPID
                    .MG_PageSize = 20
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                grid1.DataSource = Master.Factory.p56TaskBL.GetList(mq, True)

            Case BO.x29IdEnum.o23Notepad
                Dim mq As New BO.myQueryO23
                With mq
                    .Closed = BO.BooleanQueryMode.NoQuery
                    .x25ID = Master.DataPID
                    .MG_PageSize = 20
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                grid1.DataSource = Master.Factory.o23NotepadBL.GetList4Grid(mq)
            Case BO.x29IdEnum.j02Person
                Dim mq As New BO.myQueryJ02
                With mq
                    .Closed = BO.BooleanQueryMode.NoQuery : .IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified
                    .x25ID = Master.DataPID
                    .MG_PageSize = 20
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                grid1.DataSource = Master.Factory.j02PersonBL.GetList(mq)
            Case BO.x29IdEnum.p91Invoice
                Dim mq As New BO.myQueryP91
                With mq
                    .Closed = BO.BooleanQueryMode.NoQuery
                    .x25ID = Master.DataPID
                    .MG_PageSize = 20
                    .MG_CurrentPageIndex = grid1.radGridOrig.CurrentPageIndex
                End With
                grid1.DataSource = Master.Factory.p91InvoiceBL.GetList(mq)
            Case Else

        End Select

    End Sub

    Private Sub SetupJ74Combo(intDef As Integer)
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j74MasterPrefix = "")
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j74MasterPrefix = "")
        End If
        j74id.DataSource = lisJ74
        j74id.DataBind()

        If intDef > 0 Then
            basUI.SelectDropdownlistValue(Me.j74id, intDef.ToString)
        End If
        If Me.CurrentJ74ID > 0 Then
            _curJ74 = lisJ74.Where(Function(p) p.PID = Me.CurrentJ74ID)(0)
        End If


    End Sub

    Private Sub j74id_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j74id.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("x18_framework_detail-" & Me.CurrentPrefix & "-j74id", Me.j74id.SelectedValue)
        ReloadPage()
    End Sub
End Class