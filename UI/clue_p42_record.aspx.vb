Public Class clue_p42_record
    Inherits System.Web.UI.Page
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            RefreshRecord(BO.BAS.IsNullInt(Request.Item("pid")))
        End If
    End Sub

    Private Sub RefreshRecord(intPID As Integer)
        If intPID = 0 Then Master.StopPage("pid is missing", , , False)

        Dim cRec As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(intPID)
        With cRec
            Me.ph1.Text = .p42Name


        End With

        Dim mq As New BO.myQuery
        mq.AddItemToPIDs(-1)
        For Each c In Master.Factory.p42ProjectTypeBL.GetList_p43(intPID)
            mq.AddItemToPIDs(c.p34ID)
        Next

        Me.rpP34.DataSource = Master.Factory.p34ActivityGroupBL.GetList(mq)
        Me.rpP34.DataBind()


    End Sub

End Class