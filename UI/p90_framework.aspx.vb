Imports Telerik.Web.UI
Public Class p90_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub p90_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Zálohové faktury"
                .SiteMenuValue = "p90"
                .TestNeededPermission(BO.x53PermValEnum.GR_P90_Reader)

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p90_framework-pagesize")
                    .Add("p90_framework-query-closed")
                    .Add("p90_framework-search")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)



            End With
            With Master.Factory.j03UserBL
                cbxPaging.SelectedValue = .GetUserParam("p90_framework-pagesize", "20")
                basUI.SelectDropdownlistValue(Me.cbxValidity, .GetUserParam("p90_framework-query-closed", "1"))
                Me.txtSearch.Text = .GetUserParam("p90_framework-search")
            End With

            If Request.Item("search") <> "" Then
                txtSearch.Text = Request.Item("search")   'externě předaná podmínka
                txtSearch.Focus()
            End If

            SetupGrid()

        End If
    End Sub

    Private Sub SetupGrid()
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
        End With
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
        Dim mq As New BO.myQueryP90
        Select Case Me.cbxValidity.SelectedValue
            Case "1"
                mq.Closed = BO.BooleanQueryMode.NoQuery
            Case "2"
                mq.Closed = BO.BooleanQueryMode.FalseQuery
            Case "3"
                mq.Closed = BO.BooleanQueryMode.TrueQuery
        End Select
        mq.SearchExpression = Trim(Me.txtSearch.Text)

        Dim lis As IEnumerable(Of BO.p90Proforma) = Master.Factory.p90ProformaBL.GetList(mq)
       
        grid1.DataSource = lis
    End Sub


    Private Sub ReloadPage()
        Response.Redirect("p90_framework.aspx")
    End Sub
    
    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p90_framework-pagesize", cbxPaging.SelectedValue)

        ReloadPage()
    End Sub

    Private Sub cbxValidity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxValidity.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("p90_framework-query-closed", Me.cbxValidity.SelectedValue)
        grid1.Rebind(False)
    End Sub

    Private Sub Handle_RunSearch()
        Master.Factory.j03UserBL.SetUserParam("p90_framework-search", txtSearch.Text)

        grid1.Rebind(False)

        txtSearch.Focus()
    End Sub

    Private Sub p90_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Trim(txtSearch.Text) = "" Then
            txtSearch.Style.Item("background-color") = ""
        Else
            txtSearch.Style.Item("background-color") = "red"
        End If
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSearch.Click
        Handle_RunSearch()
    End Sub
End Class