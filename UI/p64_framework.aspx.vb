Imports Telerik.Web.UI

Public Class p64_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _needFilterIsChanged As Boolean = False
    Private Property _curIsExport As Boolean

    Private Sub p64_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Šanony"
                .SiteMenuValue = "p64_framework"
                .neededPermission = BO.x53PermValEnum.GR_P41_Reader
                If Request.Item("masterprefix") = "p41" Then
                    p41id_search.Value = Request.Item("masterpid")
                    p41id_search.Text = .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, BO.BAS.IsNullInt(Request.Item("masterpid")))
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p64_framework-pagesize")
                    .Add("p64_framework-query-closed")
                    .Add("p64_framework-filter_setting")
                    .Add("p64_framework-filter_sql")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)
            End With
            With Master.Factory.j03UserBL
                cbxPaging.SelectedValue = .GetUserParam("p64_framework-pagesize", "20")
                basUI.SelectDropdownlistValue(Me.cbxValidity, .GetUserParam("p64_framework-query-closed", "1"))
            End With

            With Master.Factory.j03UserBL
                SetupGrid(.GetUserParam("p64_framework-filter_setting"), .GetUserParam("p64_framework-filter_sql"))
            End With

        End If
    End Sub
    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With grid1
            .ClearColumns()
            .PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
            .radGridOrig.ShowFooter = False
            .AddSystemColumn(20)
            .AddColumn("Client", "Klient")
            .AddColumn("Project", "Projekt")
            .AddColumn("p64Name", "Název")
            .AddColumn("p64Ordinary", "#", BO.cfENUM.Numeric0, , , , , , False)
            .AddColumn("p64ArabicCode", "#A")
            .AddColumn("p64Code", "Jiný kód")
            .AddColumn("p64Location", "Lokalita")



            .AddColumn("Owner", "Vlastník")
            .AddColumn("DateInsert", "Založeno", BO.cfENUM.DateTime, , , , , , False)

            .SetFilterSetting(strFilterSetting, strFilterExpression)
        End With

    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p64Binder = CType(e.Item.DataItem, BO.p64Binder)
        If cRec.IsClosed Then dataItem.Font.Strikeout = True
      
    End Sub
    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("p64_framework-filter_setting", grid1.GetFilterSetting())
                .SetUserParam("p64_framework-filter_sql", grid1.GetFilterExpression())
            End With
        End If
        Dim mq As New BO.myQuery
        Select Case Me.cbxValidity.SelectedValue
            Case "1"
                mq.Closed = BO.BooleanQueryMode.NoQuery
            Case "2"
                mq.Closed = BO.BooleanQueryMode.FalseQuery
            Case "3"
                mq.Closed = BO.BooleanQueryMode.TrueQuery
        End Select
        mq.ColumnFilteringExpression = grid1.GetFilterExpression

        Dim intP41ID As Integer = BO.BAS.IsNullInt(Me.p41id_search.Value)
        If intP41ID = 0 Then intP41ID = -1
        Dim lis As IEnumerable(Of BO.p64Binder) = Master.Factory.p64BinderBL.GetList(intP41ID, mq)

        grid1.DataSource = lis

        If lis.Count >= 1000 Then
            lblMessage.Text = "Limit tohoto přehledu je 1000 záznamů -> využívejte filtrování."
        End If
    End Sub


    Private Sub ReloadPage()
        Response.Redirect("p64_framework.aspx")
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p64_framework-pagesize", cbxPaging.SelectedValue)

        ReloadPage()
    End Sub

    Private Sub cbxValidity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxValidity.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p64_framework-query-closed", Me.cbxValidity.SelectedValue)
        grid1.Rebind(False)
    End Sub



    Private Sub p41id_search_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles p41id_search.AutoPostBack_SelectedIndexChanged
        grid1.Rebind(False)
    End Sub

    Private Sub GridExport(strFormat As String)
        _curIsExport = True
        basUIMT.Handle_GridTelerikExport(grid1, strFormat)


    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        Select Case hidHardRefreshFlag.Value
            Case "pdf"
                GridExport("pdf")
            Case "xls"
                GridExport("xls")
            Case "doc"
                GridExport("doc")
            Case Else
                grid1.Rebind(False)
        End Select

        hidHardRefreshFlag.Value = ""
    End Sub
End Class