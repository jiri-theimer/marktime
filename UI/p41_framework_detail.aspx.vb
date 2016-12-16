Public Class p41_framework_detail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim intPID As Integer = BO.BAS.IsNullInt(Request.Item("pid"))
            With Master.Factory.j03UserBL
                If Request.Item("tab") <> "" Then
                    .SetUserParam("p41_framework_detail-tab", Request.Item("tab"))
                End If
                .InhaleUserParams("p41_framework_detail-pid", "p41_framework_detail-tab")

                If intPID = 0 Then
                    intPID = BO.BAS.IsNullInt(.GetUserParam("p41_framework_detail-pid", "O"))
                    If intPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=p41")
                Else
                    If intPID <> BO.BAS.IsNullInt(.GetUserParam("p41_framework_detail-pid", "O")) Then
                        .SetUserParam("p41_framework_detail-pid", intPID.ToString)
                    End If
                End If
                Dim strTab As String = .GetUserParam("p41_framework_detail-tab", "board")
                Select Case strTab
                    Case "p31", "time", "expense", "fee", "kusovnik"
                        Server.Transfer("entity_framework_rec_p31.aspx?masterprefix=p41&masterpid=" & intPID.ToString & "&p31tabautoquery=" & strTab, False)
                    Case "o23", "p91", "p56", "summary", "p41"
                        Server.Transfer("entity_framework_rec_" & strTab & ".aspx?masterprefix=p41&masterpid=" & intPID.ToString, False)
                    Case "budget"
                        Server.Transfer("p41_framework_rec_budget.aspx?pid=" & intPID.ToString, False)
                    Case Else
                        Server.Transfer("p41_framework_rec_board.aspx?pid=" & intPID.ToString, False)
                End Select
            End With
           
        End If
    End Sub

End Class