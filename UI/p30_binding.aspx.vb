Public Class p30_binding
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentPrefix As String
        Get
            Return hidPrefix.Value
        End Get
        Set(value As String)
            hidPrefix.Value = value
        End Set
    End Property

    Private Sub p30_binding_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            With Master

                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                Me.CurrentPrefix = Request.Item("masterprefix")

                Select Case Me.CurrentPrefix
                    Case "p28"
                        .HeaderText = "Kontaktní osoby | " & .Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, .DataPID)
                        cmdSave.Text = "Přiřadit osobu ke klientovi"
                    Case "p41"
                        .HeaderText = "Kontaktní osoby | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
                        cmdSave.Text = "Přiřadit osobu k projektu"
                End Select
            End With

            

            RefreshList()
        End If
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Dim cRec As New BO.p30Contact_Person
        cRec.p27ID = BO.BAS.IsNullInt(Me.p27ID.SelectedValue)
        cRec.j02ID = BO.BAS.IsNullInt(Me.j02ID.Value)
        If cRec.j02ID = 0 Then
            Master.Notify("Musíte vybrat osobu.", NotifyLevel.ErrorMessage)
            Return
        End If
        Select Case Me.CurrentPrefix
            Case "p28"
                cRec.p28ID = Master.DataPID
            Case "p41"
                cRec.p41ID = Master.DataPID
        End Select
        With Master.Factory.p30Contact_PersonBL
            If .Save(cRec) Then
                Master.CloseAndRefreshParent("p30-save")
            Else
                Master.Notify(Master.Factory.p30Contact_PersonBL.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub

    Private Sub RefreshList()
        Dim lisP30 As IEnumerable(Of BO.p30Contact_Person) = Nothing
        Select Case Me.CurrentPrefix
            Case "p28"
                lisP30 = Master.Factory.p30Contact_PersonBL.GetList(Master.DataPID, 0, 0)
            Case "p41"
                Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
                If cP41.p28ID_Client = 0 Then
                    lisP30 = Master.Factory.p30Contact_PersonBL.GetList(0, Master.DataPID, 0)
                Else
                    lisP30 = Master.Factory.p30Contact_PersonBL.GetList(cP41.p28ID_Client, 0, 0)
                End If

        End Select
        rpP30.DataSource = lisP30
        rpP30.DataBind()
        Dim intJ02ID_Default As Integer = BO.BAS.IsNullInt(Request.Item("default_j02id"))
        If intJ02ID_Default <> 0 Then
            If lisP30.Where(Function(p) p.j02ID = intJ02ID_Default).Count = 0 Then
                Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(intJ02ID_Default)
                Me.j02ID.Value = cRec.PID.ToString
                Me.j02ID.Text = cRec.FullNameDesc
                Master.Notify(String.Format("Osobu [{0}] nyní můžete přiřadit.", cRec.FullNameAsc))
            End If
        End If
        
    End Sub

    Private Sub rpP30_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpP30.ItemCommand
        Dim intP30ID As Integer = BO.BAS.IsNullInt(CType(e.Item.FindControl("p30id"), HiddenField).Value)
        Select Case e.CommandName
            Case "delete"
                If Master.Factory.p30Contact_PersonBL.Delete(intP30ID) Then
                    Master.CloseAndRefreshParent("p30-delete")
                End If
            Case "default_add", "default_delete"
                Dim b As Boolean = True
                If e.CommandName = "default_delete" Then b = False
                Dim cRec As BO.p30Contact_Person = Master.Factory.p30Contact_PersonBL.Load(intP30ID)
                If Master.Factory.p30Contact_PersonBL.SetAsDefaultPerson(cRec, b) Then
                    RefreshList()
                End If
            Case "default_invoice_add", "default_invoice_delete"
                Dim b As Boolean = True
                If e.CommandName = "default_invoice_delete" Then b = False
                Dim cRec As BO.p30Contact_Person = Master.Factory.p30Contact_PersonBL.Load(intP30ID)
                If Master.Factory.p30Contact_PersonBL.SetAsDefaultInInvoice(cRec, b) Then
                    RefreshList()
                End If
        End Select
    End Sub

    Private Sub rpP30_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP30.ItemDataBound
        Dim cRec As BO.p30Contact_Person = CType(e.Item.DataItem, BO.p30Contact_Person)

        CType(e.Item.FindControl("p30id"), HiddenField).Value = cRec.PID.ToString

        With CType(e.Item.FindControl("cmdDelete"), LinkButton)
            Select Case Me.CurrentPrefix
                Case "p28"
                    .Text = "Odstranit vazbu ke klientovi"
                    If cRec.p41ID <> 0 Then
                        .Visible = False
                        CType(e.Item.FindControl("imgDel"), Image).Visible = False
                        CType(e.Item.FindControl("Message"), Label).Text = String.Format("Osoba je přímo přiřazená k projektu: {0}", "<b>" & cRec.Project & "</b>")
                    End If
                    If cRec.p28ID = Master.DataPID Then
                        e.Item.FindControl("cmdDefaultInWorksheet").Visible = Not cRec.p30IsDefaultInWorksheet
                        If cRec.p30IsDefaultInWorksheet Then e.Item.FindControl("cmdDeleteDefault").Visible = True
                        e.Item.FindControl("lblDefaultInWorksheet").Visible = cRec.p30IsDefaultInWorksheet
                    End If
                Case "p41"
                    .Text = "Odstranit vazbu k projektu"
                    If cRec.p28ID <> 0 And cRec.p41ID = 0 Then
                        .Visible = False
                        CType(e.Item.FindControl("imgDel"), Image).Visible = False
                        CType(e.Item.FindControl("Message"), Label).Text = "Osoba je přiřazená přímo ke klientovi projektu"
                    End If
                    If cRec.p41ID <> 0 And cRec.p41ID <> Master.DataPID Then
                        .Visible = False
                        CType(e.Item.FindControl("imgDel"), Image).Visible = False
                        CType(e.Item.FindControl("Message"), Label).Text = String.Format("Osoba je přímo přiřazená k projektu: {0}", "<b>" & cRec.Project & "</b>")
                    End If
                    If cRec.p41ID = Master.DataPID Then
                        e.Item.FindControl("cmdDefaultInWorksheet").Visible = Not cRec.p30IsDefaultInWorksheet
                        If cRec.p30IsDefaultInWorksheet Then e.Item.FindControl("cmdDeleteDefault").Visible = True
                        e.Item.FindControl("lblDefaultInWorksheet").Visible = cRec.p30IsDefaultInWorksheet
                    End If
            End Select
        End With
        With CType(e.Item.FindControl("Person"), Label)
            .Text = cRec.FullNameDesc
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        CType(e.Item.FindControl("p27Name"), Label).Text = cRec.p27Name
        CType(e.Item.FindControl("cmdJ02"), HyperLink).NavigateUrl = "javascript:j02_edit(" & cRec.j02ID.ToString & ")"
        CType(e.Item.FindControl("clue_j02"), HyperLink).Attributes("rel") = "clue_j02_record.aspx?pid=" & cRec.j02ID.ToString


    End Sub
End Class