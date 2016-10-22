Public Class clue_o24_record
    Inherits System.Web.UI.Page
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            RefreshRecord(BO.BAS.IsNullInt(Request.Item("pid")))
        End If
    End Sub

    Private Sub RefreshRecord(intPID As Integer)
        If intPID = 0 Then Master.StopPage("pid is missing", , , False)

        Dim cRec As BO.o24NotepadType = Master.Factory.o24NotepadTypeBL.Load(intPID)
        With cRec
            Me.ph1.Text = .o24Name
            Me.x29Name.Text = Master.Factory.ftBL.GetList_X29().Where(Function(p) p.PID = CInt(cRec.x29ID))(0).x29NameSingle
            If .o24HelpText <> "" Then
                panHelp.Visible = True
                Me.o24HelpText.Text = BO.BAS.CrLfText2Html(.o24HelpText)
            Else
                panHelp.Visible = False
            End If
            With Me.o24AllowedFileExtensions
                .Text = cRec.o24AllowedFileExtensions
                If .Text = "" Then .Text = "[bez omezení]"
            End With

            If .o24MaxOneFileSize > 0 Then
                Me.o24MaxOneFileSize.Text = BO.BAS.FormatFileSize(.o24MaxOneFileSize)
            Else
                Me.o24MaxOneFileSize.Text = "[maximum, co povolí server]"
            End If


            If .o24IsEntityRelationRequired Then
                lblBindInfo.Text = String.Format("Tento typ vyžaduje přiřazení záznamu entity [{0}] již při vytváření dokumentu.", Me.x29Name.Text)
            Else
                lblBindInfo.Text = "[" & Me.x29Name.Text & " záznam] je povoleno přiřadit do dokumentu později."
            End If
        End With

      

    End Sub
End Class