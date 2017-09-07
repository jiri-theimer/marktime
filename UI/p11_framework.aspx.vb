Public Class p11_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub p11_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        gridP31.Factory = Master.Factory
        Master.HelpTopicID = "p11_framework"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SiteMenuValue = "p11_framework"
            datToday.SelectedDate = Today

            With Master.Factory.j03UserBL
                
            End With
            setupgrid()
        End If
    End Sub

    Private Sub SetupGrid()
        With gridP31
            .Visible = True
            .MasterDataPID = Master.Factory.SysUser.j02ID
            .ExplicitDateFrom = datToday.SelectedDate
            .ExplicitDateUntil = datToday.SelectedDate
            .RecalcVirtualRowCount()
            .Rebind(False)
        End With
    End Sub
End Class