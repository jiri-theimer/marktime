Imports Telerik.Web.UI

Public Class admin_menu
    Inherits System.Web.UI.Page

    Public Property CurrentJ60ID As Integer
        Get
            Return BO.BAS.IsNullInt(cbxJ60ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.cbxJ60ID, value.ToString)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "MENU návrhář"
                .SiteMenuValue = "admin_framework"
                .TestNeededPermission(BO.x53PermValEnum.GR_Admin)

               
            End With

            Me.cbxJ60ID.DataSource = Master.Factory.j62MenuHomeBL.GetList_J60()
            Me.cbxJ60ID.DataBind()

            Me.CurrentJ60ID = BO.BAS.IsNullInt(Request.Item("j60id"))
           

            RefreshRecord()


        End If
    End Sub

    Private Sub RefreshRecord()

        refreshtree()
    End Sub

    Private Sub RefreshTree()
        
        Dim lis As IEnumerable(Of BO.j62MenuHome) = Master.Factory.j62MenuHomeBL.GetList(Me.CurrentJ60ID, New BO.myQuery)

        With tree1
            .Clear()
            For Each c In lis
                Dim strParentID As String = ""
                If c.j62ParentID <> 0 Then strParentID = c.j62ParentID.ToString

                .AddItem(c.j62Name, c.PID.ToString, "javascript:rec(" & c.PID.ToString & ")", strParentID, c.j62ImageUrl, c.j62Name_ENG)

              

            Next
            .ExpandAll()
        End With

       
    End Sub

    Private Sub cmdRefreshOnBehind_Click(sender As Object, e As EventArgs) Handles cmdRefreshOnBehind.Click
        Me.cbxJ60ID.DataSource = Master.Factory.j62MenuHomeBL.GetList_J60()
        Me.cbxJ60ID.DataBind()

        RefreshRecord()

    End Sub
End Class