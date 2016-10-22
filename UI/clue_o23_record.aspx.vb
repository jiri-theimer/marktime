Public Class clue_o23_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Clue

    Private Sub clue_o23_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        files1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            If Request.Item("dr") = "1" Then
                panContainer.Style.Clear()
            End If
            RefreshRecord(BO.BAS.IsNullInt(Request.Item("pid")))
        End If
    End Sub

    Private Sub RefreshRecord(intPID As Integer)
        If intPID = 0 Then Master.StopPage("pid is missing", , , False)

        Dim cRec As BO.o23Notepad = Master.Factory.o23NotepadBL.Load(intPID)
        If cRec Is Nothing Then Master.StopPage("Record not found.")
        Dim cDisp As BO.o23RecordDisposition = Master.Factory.o23NotepadBL.InhaleRecordDisposition(cRec)
        With cRec
            If .o23IsEncrypted Then
                Master.StopPage("Obsah dokumentu je ochráněn heslem.")
            End If
            If Not cDisp.ReadAccess Then
                Master.StopPage("Nedisponujete oprávněním číst tento dokument.")
            End If
            Master.DataPID = .PID
            ph1.Text = .o24Name & ": " & .o23Code
            If .o23BodyPlainText <> "" And Not .o23IsEncrypted Then
                o23BodyPlainText.Text = BO.BAS.CrLfText2Html(.o23BodyPlainText)
            Else
                panBody.Visible = False
            End If
            If .o23Name = "" Then
                trO23Name.Visible = False
            Else
                Me.o23Name.Text = .o23Name
            End If

            Me.o23Date.Text = BO.BAS.FD(.o23Date)
            If Not .o23ReminderDate Is Nothing Then
                Me.o23ReminderDate.Text = "Čas připomenutí: " & BO.BAS.FD(.o23ReminderDate, True, True)
            End If

            If .b02ID <> 0 Then
                Me.b02Name.Text = .b02Name
            Else
                trWorkflow.Visible = False
            End If
            Me.Owner.Text = .Owner
            If .p41ID <> 0 Then
                Me.BindName.Text = "Projekt:"
                Me.BindValue.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .p41ID)
            End If
            If .p28ID <> 0 Then
                Me.BindName.Text = "Klient:"
                Me.BindValue.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, .p28ID)
            End If
            If .j02ID <> 0 Then
                Me.BindName.Text = "Osoba:"
                Me.BindValue.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, .j02ID)
            End If
            If .p31ID <> 0 Then
                Me.BindName.Text = "Worksheet úkon:"
                Me.BindValue.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p31Worksheet, .p31ID)
            End If
            If .p56ID <> 0 Then
                Me.BindName.Text = "Úkol:"
                Me.BindValue.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .p56ID)
            End If
            If .p41ID = 0 And .x29ID = BO.x29IdEnum.p41Project Then
                lblMessage.Text = "Dokument čeká na přiřazení k projektu."
            End If
            If .p31ID = 0 And .x29ID = BO.x29IdEnum.p31Worksheet Then
                lblMessage.Text = "Dokument čeká na přiřazení k worksheet úkonu."
            End If
            If .p28ID = 0 And .x29ID = BO.x29IdEnum.p28Contact Then
                lblMessage.Text = "Dokument čeká na přiřazení ke klientovi."
            End If
            Me.Timestamp.Text = .Timestamp

        End With

        Me.files1.RefreshData_O23(intPID)
        If Me.files1.ItemsCount = 0 Then
            panFiles.Visible = False
        End If

        Me.comments1.RefreshData(Master.Factory, BO.x29IdEnum.o23Notepad, intPID)
    End Sub

End Class