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
            p11TodayStart.SelectedDate = Now
            p11TodayEnd.SelectedDate = Now
            With Master.Factory.j03UserBL
                
            End With
            RefreshButtons()
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

    Private Sub RefreshButtons()
        Dim mq As New BO.myQueryP32
        mq.p33ID = BO.p33IdENUM.Cas
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        Dim lis As IEnumerable(Of BO.p32Activity) = Master.Factory.p32ActivityBL.GetList(mq).Where(Function(p) p.p32AttendanceFlag > BO.p32AttendanceFlagENUM._None)
        rp1.DataSource = lis
        rp1.DataBind()

    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.p32Activity = CType(e.Item.DataItem, BO.p32Activity)
        With CType(e.Item.FindControl("link1"), HyperLink)
            If Len(cRec.p32Name) > 30 Then
                .ToolTip = cRec.p32Name
                .Text = BO.BAS.OM3(cRec.p32Name, 28)
            Else
                .Text = cRec.p32Name
            End If

            .NavigateUrl = "javascript:p31_entry_attendance(" & cRec.PID.ToString & ")"
        End With
    End Sub
End Class