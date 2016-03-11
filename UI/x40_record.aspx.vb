Public Class x40_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub x40_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.uploadlist1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then .StopPage("Na vstupu chybí ID zprávy.")
                .HeaderText = "Poštovní zpráva"
                .HeaderIcon = "Images/email_32.png"

            End With


            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.x40MailQueue = Master.Factory.x40MailQueueBL.Load(Master.DataPID)
        With cRec
            Me.DateInsert.Text = BO.BAS.FD(.DateInsert)
            Me.x40SenderName.Text = .x40SenderName & " (" & .x40SenderAddress & ")"
            Me.x40Recipient.Text = .x40Recipient
            Me.x40CC.Text = .x40CC
            Me.x40BCC.Text = .x40BCC

            Me.x40Subject.Text = .x40Subject
            Me.x40Body.Text = .x40Body
            Me.x40ErrorMessage.Text = .x40ErrorMessage
            Select Case .x40State
                Case BO.x40StateENUM.InQueque
                    Me.x40State.Text = "Čeká ve frontě na odeslání"
                    cmdChangeState.Text = "Změnit stav zprávy na [Zastaveno]" : cmdChangeState.CommandArgument = "4"

                Case BO.x40StateENUM.IsError
                    Me.x40State.Text = "Při pokusu o odeslání došlo k chybě"
                    Me.x40State.ForeColor = Drawing.Color.Red
                    cmdChangeState.Text = "Změnit stav zprávy na [Čeká na odeslání]" : cmdChangeState.CommandArgument = "1"
                Case BO.x40StateENUM.IsProceeded
                    Me.x40State.Text = "Odesláno (" & BO.BAS.FD(.x40WhenProceeded, True) & ")"
                    Me.x40State.ForeColor = Drawing.Color.Green
                    cmdChangeState.Visible = False
                Case BO.x40StateENUM.IsStopped
                    Me.x40State.Text = "Zastaveno"
                    Me.x40State.ForeColor = Drawing.Color.Magenta
                    cmdChangeState.Text = "Změnit stav zprávy na [Čeká na odeslání]" : cmdChangeState.CommandArgument = "1"
            End Select
            Me.Timestamp.Text = .Timestamp
        End With

        Dim mq As New BO.myQueryO27
        mq.x40ID = Master.DataPID

        Me.uploadlist1.RefreshData(mq)
    End Sub

    Private Sub cmdChangeState_Click(sender As Object, e As EventArgs) Handles cmdChangeState.Click
        Dim newState As BO.x40StateENUM = BO.x40StateENUM._NotSpecified
        Select Case Me.cmdChangeState.CommandArgument
            Case "4"
                newState = BO.x40StateENUM.IsStopped
            Case "1"
                newState = BO.x40StateENUM.InQueque
        End Select
        If Master.Factory.x40MailQueueBL.UpdateMessageState(Master.DataPID, newState) Then
            Master.CloseAndRefreshParent("save")
        End If
    End Sub
End Class