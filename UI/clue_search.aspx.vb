Public Class clue_search
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                Dim lis As New List(Of String)
                lis.Add("handler_search_project-toprecs")
                lis.Add("handler_search_project-bin")
                lis.Add("handler_search_contact-toprecs")
                lis.Add("handler_search_contact-bin")
                lis.Add("handler_search_person-bin")
                lis.Add("handler_search_invoice-toprecs")

                With .Factory.j03UserBL
                    .InhaleUserParams(lis)
                    Me.chkP41Bin.Checked = BO.BAS.BG(.GetUserParam("handler_search_project-bin", "0"))
                    Me.chkP28Bin.Checked = BO.BAS.BG(.GetUserParam("handler_search_contact-bin", "0"))
                    Me.chkJ02Bin.Checked = BO.BAS.BG(.GetUserParam("handler_search_person-bin", "0"))
                    basUI.SelectDropdownlistValue(Me.cbxP41Top, .GetUserParam("handler_search_project-toprecs", "20"))
                    basUI.SelectDropdownlistValue(Me.cbxP28Top, .GetUserParam("handler_search_contact-toprecs", "20"))
                    basUI.SelectDropdownlistValue(Me.cbxP91Top, .GetUserParam("handler_search_invoice-toprecs", "20"))
                End With
                With .Factory.SysUser
                    trP41.Visible = .j04IsMenu_Project
                    trP28.Visible = .j04IsMenu_Contact
                    trP91.Visible = .j04IsMenu_Invoice
                    trJ02.Visible = .j04IsMenu_People
                End With
            End With
            If Request.Item("fulltext") = "1" Then RadTabStrip1.SelectedIndex = 1 : RadMultiPage1.SelectedIndex = 1
        End If
        Me.p41id_search.radComboBoxOrig.DropDownWidth = Unit.Parse("570px")
        Me.p28id_search.radComboBoxOrig.DropDownWidth = Unit.Parse("570px")
        Me.j02id_search.RadCombo.DropDownWidth = Unit.Parse("570px")
        Me.p91id_search.RadComboOrig.DropDownWidth = Unit.Parse("570px")
        If trP28.Visible Then Me.p28id_search.radComboBoxOrig.OnClientSelectedIndexChanged = "p28id_search"
        If trJ02.Visible Then j02id_search.RadCombo.OnClientSelectedIndexChanged = "j02id_search"
        If trP91.Visible Then p91id_search.RadComboOrig.OnClientSelectedIndexChanged = "p91id_search"
        fsP41.Visible = trP41.Visible
        fsP28.Visible = trP28.Visible
        fsP91.Visible = trP91.Visible
        fsJ02.Visible = trJ02.Visible
    End Sub

    Private Sub chkP41Bin_CheckedChanged(sender As Object, e As EventArgs) Handles chkP41Bin.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_project-bin", BO.BAS.GB(Me.chkP41Bin.Checked))
        Me.p41id_search.Text = ""
    End Sub

    Private Sub chkP28Bin_CheckedChanged(sender As Object, e As EventArgs) Handles chkP28Bin.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_contact-bin", BO.BAS.GB(Me.chkP28Bin.Checked))
        Me.p28id_search.Text = ""
    End Sub

    Private Sub chkJ02Bin_CheckedChanged(sender As Object, e As EventArgs) Handles chkJ02Bin.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_person-bin", BO.BAS.GB(Me.chkJ02Bin.Checked))
        Me.j02id_search.Text = ""
    End Sub

    Private Sub cbxP28Top_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxP28Top.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_contact-toprecs", Me.cbxP28Top.SelectedValue)
    End Sub

    Private Sub cbxP41Top_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxP41Top.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_project-toprecs", Me.cbxP41Top.SelectedValue)
    End Sub

    Private Sub cbxP91Top_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxP91Top.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("handler_search_invoice-toprecs", Me.cbxP91Top.SelectedValue)
    End Sub
End Class