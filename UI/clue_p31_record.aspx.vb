Public Class clue_p31_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub clue_p31_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        files1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            Master.DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

            If Request.Item("dr") = "1" Then
                panContainer.Style.Clear()
            End If
            RefreshRecord()

            If Request.Item("js_clone") <> "" Then
                imgClone.Visible = True : cmdClone.Visible = True : cmdClone.NavigateUrl = "javascript: parent.window." & Request.Item("js_clone")
            End If
            If Request.Item("js_edit") <> "" Then
                imgEdit.Visible = True : cmdEdit.Visible = True : cmdEdit.NavigateUrl = "javascript: parent.window." & Request.Item("js_clone")
            End If
        End If
    End Sub
    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then Master.StopPage("pid is missing", , , False)

        Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
        If cRec Is Nothing Then Master.StopPage("record not found")
        With cRec
            Me.Project.Text = .p41Name
            Me.Client.Text = .p28Name
            Me.p32Name.Text = .p32Name
            Me.p34name.Text = .p34Name
            Me.Person.Text = .Person
            Me.p31Date.Text = BO.BAS.FD(.p31Date)
            Me.p31Value_Orig.Text = BO.BAS.FN(.p31Value_Orig)
            Select Case .p33ID
                Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                    Me.p31Value_Orig.Text += " " & .j27Code_Billing_Orig
                Case BO.p33IdENUM.Cas
                    If Not .p31DateTimeFrom_Orig Is Nothing Then
                        Me.TimePeriod.Text = .TimeFrom & "-" & .TimeUntil
                    End If
            End Select
            Me.p31Text.Text = BO.BAS.CrLfText2Html(.p31Text)
            If .p56ID <> 0 Then
                Dim cTask As BO.p56Task = Master.Factory.p56TaskBL.Load(.p56ID)
                Me.Task.Text = "<img src='Images/task.png'/>" & cTask.p57Name & ": " & cTask.p56Code & " - " & cTask.p56Name
            End If
            Me.Timestamp.Text = .Timestamp
        End With
        If cRec.o23ID_First <> 0 Then
            Dim cDoc As BO.o23Notepad = Master.Factory.o23NotepadBL.Load(cRec.o23ID_First)
            Me.o23Name.Text = cDoc.o24Name & ": " & cDoc.o23Code & " - " & cDoc.o23Name
            Me.files1.RefreshData_O23(cDoc.PID)
        Else
            panFiles.Visible = False
        End If


    End Sub
End Class