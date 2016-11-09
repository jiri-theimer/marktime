Public Class clue_search
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                Dim lis As New List(Of String)
                lis.Add("handler_search_project-toprecs")
                lis.Add("handler_search_project-bin")
                lis.Add("handler_search_contact-bin")
                lis.Add("handler_search_person-bin")
                With .Factory.j03UserBL
                    .InhaleUserParams(lis)
                    Me.chkP41Bin.Checked = BO.BAS.BG(.GetUserParam("handler_search_project-bin", "0"))
                    Me.chkP28Bin.Checked = BO.BAS.BG(.GetUserParam("handler_search_contact-bin", "0"))
                    Me.chkJ02Bin.Checked = BO.BAS.BG(.GetUserParam("handler_search_person-bin", "0"))
                End With

            End With
        End If
        Me.p41id_search.radComboBoxOrig.DropDownWidth = Unit.Parse("480px")
        Me.p28id_search.radComboBoxOrig.DropDownWidth = Unit.Parse("480px")
        Me.j02id_search.RadCombo.DropDownWidth = Unit.Parse("480px")
        Me.p28id_search.radComboBoxOrig.OnClientSelectedIndexChanged = "p28id_search"
        j02id_search.RadCombo.OnClientSelectedIndexChanged = "j02id_search"
    End Sub

    Private Sub chkP41Bin_CheckedChanged(sender As Object, e As EventArgs) Handles chkP41Bin.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_project-bin", BO.BAS.GB(Me.chkP41Bin.Checked))
        Me.p41id_search.Text = ""
    End Sub

    Private Sub chkP28Bin_CheckedChanged(sender As Object, e As EventArgs) Handles chkP28Bin.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_contact-bin", BO.BAS.GB(Me.chkP41Bin.Checked))
        Me.p28id_search.Text = ""
    End Sub

    Private Sub chkJ02Bin_CheckedChanged(sender As Object, e As EventArgs) Handles chkJ02Bin.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_person-bin", BO.BAS.GB(Me.chkJ02Bin.Checked))
        Me.j02id_search.Text = ""
    End Sub
End Class