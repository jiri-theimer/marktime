Public Class x18_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Public Property CurrentX18ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.x18ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.x18ID, value.ToString)
        End Set
    End Property
    Public ReadOnly Property CurrentX23ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidX23ID.Value)
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            
            With Master
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("x18_framework-pagesize")
                    .Add("x18_framework-navigationPane_width")
                    .Add("x18_framework_detail-pid")
                    .Add("x18_framework-x18id")
                    .Add("x18_framework-sort")
                    .Add("x18_framework-checkbox_selector")
                End With
                Me.x18ID.DataSource = .Factory.x18EntityCategoryBL.GetList()
                Me.x18ID.DataBind()
                If Me.x18ID.Items.Count = 0 Then
                    .Notify("V databázi zatím není zaveden ani jeden druh štítku.")
                End If

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                    basUI.SelectDropdownlistValue(Me.x18ID, .GetUserParam("x18_framework-x18id"))
                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("x18_framework-pagesize", "20"))
                    Me.navigationPane.Width = Unit.Parse(.GetUserParam("x18_framework-navigationPane_width", "420") & "px")
                    Me.chkCheckboxSelector.Checked = BO.BAS.BG(.GetUserParam("x18_framework-checkbox_selector", "0"))
                    If .GetUserParam("x18_framework-sort") <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam("x18_framework-sort"))
                    End If
                    Me.contentPane.ContentUrl = "x18_framework_detail.aspx?pid=" & .GetUserParam("x18_framework_detail-pid")
                End With

                .SiteMenuValue = "x18"
            End With


            If Me.CurrentX18ID <> 0 Then
                Dim cRec As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Me.CurrentX18ID)
                Me.hidX23ID.Value = cRec.x23ID.ToString
            End If
            SetupGrid()
            If Request.Item("blankwindow") = "1" Then Me.navigationPane.Collapsed = True
        End If
    End Sub
    Private Sub SetupGrid()
        With grid1
            .ClearColumns()
            .PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
            .radGridOrig.ShowFooter = False
            .AddSystemColumn(20)
            .AddColumn("x25Name", "Název")
        End With
    End Sub
    Private Sub ReloadPage()
        Response.Redirect("x18_framework.aspx", True)
    End Sub
    Private Sub x18ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles x18ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("x18_framework-x18id", Me.x18ID.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If Me.CurrentX23ID = 0 Then Return
        Dim lis As IEnumerable(Of BO.x25EntityField_ComboValue) = Master.Factory.x25EntityField_ComboValueBL.GetList(Me.CurrentX23ID)

        grid1.DataSource = lis
    End Sub

    Private Sub grid1_SortCommand(SortExpression As String, strOwnerTableName As String) Handles grid1.SortCommand
        Master.Factory.j03UserBL.SetUserParam("x18_framework-sort", SortExpression)
    End Sub
End Class