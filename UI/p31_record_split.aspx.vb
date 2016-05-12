Public Class p31_record_split
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p31_record_split_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then
                    .StopPage("PID is missing.")
                End If
                ViewState("guid") = Request.Item("guid")
                If ViewState("guid") = "" Then Master.StopPage("guid is missing")

                .HeaderText = "Rozdělení časového úkonu na 2 kusy"
                .HeaderIcon = "Images/worksheet_32.png"
                .AddToolbarButton("Uložit změny", "save", , "Images/save.png")

            End With

            RefreshRecord()
            
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
        If cRec Is Nothing Then
            Master.StopPage("Záznam nebyl nalezen.", True)
        End If
        With cRec
            If .p33ID <> BO.p33IdENUM.Cas Then
                Master.StopPage("Rozdělit lze pouze časový úkon.", True)
            End If
            If .p71ID > BO.p71IdENUM.Nic Then Master.StopPage("Záznam byl již dříve schválen.")
            If .p91ID <> 0 Then Master.StopPage("Záznam byl již dříve vyfakturován.")
        End With
        
        Dim cD As BO.p31WorksheetDisposition = Master.Factory.p31WorksheetBL.InhaleRecordDisposition(Master.DataPID)
        Select Case cD.RecordDisposition
            Case BO.p31RecordDisposition.CanApprove, BO.p31RecordDisposition.CanApproveAndEdit
            Case Else
                Master.StopPage("Nedisponujete oprávněním rozdělit tento úkon.", True)
        End Select
        With cRec
            Me.Person.Text = .Person
            Me.hours1.Value = .p31Hours_Orig
            Me.txt1.Text = .p31Text
            Me.txt2.Text = .p31Text
            Me.p31Date.Text = BO.BAS.FD(.p31Date)
            Me.Project.Text = .p41Name
            Me.p31Hours.Text = BO.BAS.FN(.p31Hours_Orig)
        End With



    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)

        End If
    End Sub
End Class