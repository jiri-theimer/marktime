Public Class p91_proforma
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p91_proforma_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then .StopPage("pid missing")
                .HeaderIcon = "Images/proforma_32.png"
                .HeaderText = "Spárovat fakturu s uhrazenou zálohou"
                .AddToolbarButton("Uložit vazbu na zálohu", "save", , "Images/save.png")

            End With
            Dim mq As New BO.myQueryP90
            mq.IsP99Bounded = BO.BooleanQueryMode.FalseQuery
            Dim cRec As BO.p91Invoice = Master.Factory.p91InvoiceBL.Load(Master.DataPID)
            mq.j27ID = cRec.j27ID
            Me.p90ID.DataSource = Master.Factory.p90ProformaBL.GetList(mq).Where(Function(p) p.p90Amount_Debt < 20)
            Me.p90ID.DataBind()

            
            rpP99.DataSource = Master.Factory.p90ProformaBL.GetList_p99(Master.DataPID)
            rpP99.DataBind()

        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim intP90ID As Integer = BO.BAS.IsNullInt(Me.p90ID.SelectedValue)

            If intP90ID = 0 Then
                Master.Notify("Musíte vybrat zálohovou fakturu.", NotifyLevel.WarningMessage)
                Return
            End If
            If BO.BAS.IsNullNum(dblAmount.Value) <= 0 Then
                Master.Notify("Částka musí být větší než NULA.", NotifyLevel.ErrorMessage)
                Return
            End If
            dblAmount.Value = Math.Round(BO.BAS.IsNullNum(dblAmount.Value), 2)

            With Master.Factory.p91InvoiceBL
                If .SaveP99(Master.DataPID, intP90ID, BO.BAS.IsNullNum(Me.dblAmount.Value)) Then
                    Master.CloseAndRefreshParent("p91-save")
                Else
                    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                End If
            End With
        End If
    End Sub

    Private Sub p90ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles p90ID.SelectedIndexChanged
        Dim intPID As Integer = BO.BAS.IsNullInt(Me.p90ID.SelectedValue)
        If intPID > 0 Then
            Me.clue_p90.Visible = True
            Me.clue_p90.Attributes("rel") = "clue_p90_record.aspx?pid=" & intPID.ToString
            Handle_Amount(intPID, True)

        Else
            Me.clue_p90.Visible = False
        End If

    End Sub

    Private Sub Handle_Amount(intP90ID As Integer, bolFromPerc As Boolean)
        If intP90ID = 0 Then Return
        Dim cRec As BO.p90Proforma = Master.Factory.p90ProformaBL.Load(intP90ID)
        Dim cInvoice As BO.p91Invoice = Master.Factory.p91InvoiceBL.Load(Master.DataPID)
        Dim dblBasis As Double = cRec.p90Amount_Billed
        If cRec.p90Amount_Billed > cInvoice.p91Amount_TotalDue Then
            dblBasis = cInvoice.p91Amount_TotalDue
        End If
        If Not bolFromPerc Then
            Try
                Me.Percentage.Value = 100 * BO.BAS.IsNullNum(dblAmount.Value) / dblBasis
            Catch ex As Exception

            End Try
        Else
            dblAmount.Value = dblBasis * BO.BAS.IsNullNum(Me.Percentage.Value) / 100
        End If

        Dim dbl As Double = dblAmount.Value / (1 + cRec.p90VatRate / 100)
        dblAmountWithoutVat.Text = BO.BAS.FN(dbl)
        dblAmountVAT.Text = BO.BAS.FN(dblAmount.Value - dbl)
    End Sub

    Private Sub rpP99_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpP99.ItemCommand
        Dim intP90ID As Integer = CInt(e.CommandArgument)
        If Master.Factory.p91InvoiceBL.DeleteP99(Master.DataPID, intP90ID) Then
            Master.CloseAndRefreshParent("p91-save")
        End If
    End Sub

    Private Sub rpP99_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP99.ItemDataBound
        Dim cRec As BO.p99Invoice_Proforma = CType(e.Item.DataItem, BO.p99Invoice_Proforma)
        CType(e.Item.FindControl("cmdDelete"), Button).CommandArgument = cRec.p90ID.ToString
    End Sub

    Private Sub Percentage_TextChanged(sender As Object, e As EventArgs) Handles Percentage.TextChanged
        Handle_Amount(BO.BAS.IsNullInt(Me.p90ID.SelectedValue), True)
    End Sub

    Private Sub p91_proforma_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If BO.BAS.IsNullInt(Me.p90ID.SelectedValue) = 0 Then
            panAmount.Visible = False
        Else
            panAmount.Visible = True
        End If
    End Sub

    Private Sub dblAmount_TextChanged(sender As Object, e As EventArgs) Handles dblAmount.TextChanged
        Handle_Amount(BO.BAS.IsNullInt(Me.p90ID.SelectedValue), False)


    End Sub
End Class