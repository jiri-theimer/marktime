Imports Telerik.Web.UI

Public Class p49_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _needFilterIsChanged As Boolean = False

    Private Sub p49_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Rozpočty výdajů a fixních odměn"
                .SiteMenuValue = "p49"

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p49_framework-pagesize")
                    .Add("p49_framework-filter_setting")
                    .Add("p49_framework-filter_sql")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)



            End With
            With Master.Factory.j03UserBL
                cbxPaging.SelectedValue = .GetUserParam("p49_framework-pagesize", "20")
            End With

            With Master.Factory.j03UserBL
                SetupGrid(.GetUserParam("p49_framework-filter_setting"), .GetUserParam("p49_framework-filter_sql"))
            End With

        End If
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With grid1
            .ClearColumns()
            .PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
            .radGridOrig.ShowFooter = False
            .AddSystemColumn(20)
            .AddColumn("p90Code", "Číslo")
            .AddColumn("p90Date", "Datum", BO.cfENUM.DateOnly)
            .AddColumn("p28Name", "Klient")

            .AddColumn("p90Amount", "Částka", BO.cfENUM.Numeric2)
            .AddColumn("p90Amount_Debt", "Dluh", BO.cfENUM.Numeric2)
            .AddColumn("p90Text1", "Text")
            .AddColumn("j27Code", "")

            .SetFilterSetting(strFilterSetting, strFilterExpression)
        End With

    End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p90Proforma = CType(e.Item.DataItem, BO.p90Proforma)
        If cRec.IsClosed Then dataItem.Font.Strikeout = True
        If cRec.p90Amount_Debt > 0 Then
            dataItem.Item("systemcolumn").BackColor = Drawing.Color.Red
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("p49_framework-filter_setting", grid1.GetFilterSetting())
                .SetUserParam("p49_framework-filter_sql", grid1.GetFilterExpression())
            End With
        End If
        Dim mq As New BO.myQueryP49
        mq.ColumnFilteringExpression = grid1.GetFilterExpression

        Dim lis As IEnumerable(Of BO.p49FinancialPlanExtended) = Master.Factory.p49FinancialPlanBL.GetList_Extended(mq)

        grid1.DataSource = lis
    End Sub


    Private Sub ReloadPage()
        Response.Redirect("p49_framework.aspx")
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p49_framework-pagesize", cbxPaging.SelectedValue)

        ReloadPage()
    End Sub


End Class