Public Class clue_x90_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub clue_x90_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            RefreshRecord(BO.BAS.IsNullInt(Request.Item("pid")))
        End If
    End Sub

    Private Sub RefreshRecord(intPID As Integer)
        If intPID = 0 Then Master.StopPage("pid is missing", , , False)

        Dim cX90 As BO.x90EntityLog = Master.Factory.ftBL.LoadX90(intPID)
        With cX90
            Select Case .x29ID
                Case BO.x29IdEnum.p41Project
                    Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(.x90RecordPID)
                    ph1.Text = cRec.FullName : img1.ImageUrl = "Images/project_32.png"
                Case BO.x29IdEnum.p28Contact
                    Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(.x90RecordPID)
                    ph1.Text = cRec.p28Name : img1.ImageUrl = "Images/contact_32.png"
                Case BO.x29IdEnum.j02Person
                    Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(.x90RecordPID)
                    ph1.Text = cRec.FullNameAsc : img1.ImageUrl = "Images/person_32.png"
            End Select
            Select Case .x90EventFlag
                Case BO.x90EventFlagEnum.Created
                    Me.Event.Text = "Založení záznamu"
                Case BO.x90EventFlagEnum.MovedToBin
                    Me.Event.Text = "Přesunutí záznamu do koše"
                Case BO.x90EventFlagEnum.RestoreFromBin
                    Me.Event.Text = "Obnova záznamu z koše"
                Case BO.x90EventFlagEnum.Updated
                    Me.Event.Text = "Aktualizace záznamu"
            End Select
            Me.x90Date.Text = BO.BAS.FD(.x90Date, True, True)
            Me.Author.Text = .Person
        End With

       

    End Sub
End Class