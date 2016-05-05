Public Class select_project
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub select_project_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("masterprefix") = Request.Item("masterprefix")
            ViewState("oper") = Request.Item("oper")
            Select Case ViewState("oper")
                Case "createtask"
                    Me.p41ID.Flag = "createtask"
                Case "createp49"
                    Me.p41ID.Flag = "createp49"
                Case Else

            End Select
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                .AddToolbarButton("Pokračovat", "continue", 0, "Images/continue.png")
            End With
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "continue" Then
            Dim intP41ID As Integer = BO.BAS.IsNullInt(Me.p41ID.Value)
            If intP41ID = 0 Then
                Master.Notify("Musíte vybrat projekt!", NotifyLevel.WarningMessage)
                Return
            End If
            Select Case ViewState("oper")
                Case "createtask"
                    Server.Transfer("p56_record.aspx?p41id=" & intP41ID.ToString, False)
                Case "createp49"
                    Server.Transfer("p49_record.aspx?p41id=" & intP41ID.ToString, False)
                Case Else
                    Master.Notify("Neznámá operace: " & ViewState("oper"), NotifyLevel.ErrorMessage)
            End Select
        End If
    End Sub
End Class