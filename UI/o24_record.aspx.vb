Public Class o24_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub o24_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Typ dokumentu"

                Dim lisX38 As IEnumerable(Of BO.x38CodeLogic) = .Factory.x38CodeLogicBL.GetList(BO.x29IdEnum.o23Notepad)
                Me.x38ID.DataSource = lisX38.Where(Function(p) p.x38IsDraft = False)
                Me.x38ID.DataBind()
                Me.x38ID_Draft.DataSource = lisX38.Where(Function(p) p.x38IsDraft = True)
                Me.x38ID_Draft.DataBind()
                Me.b01ID.DataSource = .Factory.b01WorkflowTemplateBL.GetList().Where(Function(p) p.x29ID = BO.x29IdEnum.o23Notepad)
                Me.b01ID.DataBind()
            End With

            RefreshRecord()

            If Master.IsRecordClone Then
                Master.DataPID = 0
            End If
        End If
    End Sub



    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then Return


        Dim cRec As BO.o24NotepadType = Master.Factory.o24NotepadTypeBL.Load(Master.DataPID)
        With cRec
            basUI.SelectDropdownlistValue(Me.x29ID, CInt(.x29ID).ToString)
            Me.o24Name.Text = .o24name
            Me.o24Ordinary.Value = .o24Ordinary
            Me.b01ID.SelectedValue = .b01ID.ToString
            Me.x38ID.SelectedValue = .x38ID.ToString
            Me.x38ID_Draft.SelectedValue = .x38ID_Draft.ToString
            Master.Timestamp = .Timestamp
            Me.o24IsBillingMemo.Checked = .o24IsBillingMemo
            Me.o24IsEntityRelationRequired.SelectedValue = BO.BAS.GB(.o24IsEntityRelationRequired)
            Me.o24HelpText.Text = .o24HelpText
            Me.o24IsAllowDropbox.Checked = .o24IsAllowDropbox
            Me.o24AllowedFileExtensions.Text = .o24AllowedFileExtensions
            basUI.SelectDropdownlistValue(Me.o24MaxOneFileSize, .o24MaxOneFileSize.ToString)
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With
        Me.x29ID.Enabled = False
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.o24NotepadTypeBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("o24-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        If Me.o24IsEntityRelationRequired.SelectedItem Is Nothing Then
            Master.Notify("Specifikujte povinnost vazby na záznam entity.", NotifyLevel.WarningMessage)
            Return
        End If
        With Master.Factory.o24NotepadTypeBL
            Dim cRec As BO.o24NotepadType = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.o24NotepadType)
            cRec.x29ID = BO.BAS.IsNullInt(Me.x29ID.SelectedValue)
            cRec.o24Name = Me.o24Name.Text
            cRec.o24Ordinary = BO.BAS.IsNullInt(Me.o24Ordinary.Value)
            cRec.x38ID = BO.BAS.IsNullInt(Me.x38ID.SelectedValue)
            cRec.x38ID_Draft = BO.BAS.IsNullInt(Me.x38ID_Draft.SelectedValue)
            cRec.b01ID = BO.BAS.IsNullInt(Me.b01ID.SelectedValue)
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
            cRec.o24HelpText = Trim(Me.o24HelpText.Text)
            cRec.o24IsAllowDropbox = Me.o24IsAllowDropbox.Checked
            cRec.o24IsBillingMemo = Me.o24IsBillingMemo.Checked
            cRec.o24IsEntityRelationRequired = BO.BAS.BG(Me.o24IsEntityRelationRequired.SelectedValue)
            cRec.o24MaxOneFileSize = BO.BAS.IsNullInt(Me.o24MaxOneFileSize.SelectedValue)
            cRec.o24AllowedFileExtensions = Me.o24AllowedFileExtensions.Text
            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID


                Master.CloseAndRefreshParent("o24-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub o24_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.o24IsBillingMemo.Visible = False
        Select Case CType(CInt(Me.x29ID.SelectedValue), BO.x29IdEnum)
            Case BO.x29IdEnum.p41Project

                o24IsEntityRelationRequired.Items(0).Text = "Tento typ vyžaduje přiřazení projektu již při vytváření dokumentu"
                o24IsEntityRelationRequired.Items(1).Text = "Dokument může být vytvořen bez přiřazení k projektu (k přiřazení může dojít později)"
                Me.o24IsBillingMemo.Visible = True
            Case BO.x29IdEnum.p28Contact
                o24IsEntityRelationRequired.Items(0).Text = "Tento typ vyžaduje přiřazení klienta již při vytváření dokumentu"
                o24IsEntityRelationRequired.Items(1).Text = "Dokument může být vytvořen bez přiřazení ke klientovi (k přiřazení může dojít později)"
                Me.o24IsBillingMemo.Visible = True
            Case BO.x29IdEnum.j02Person
                o24IsEntityRelationRequired.Items(0).Text = "Tento typ vyžaduje přiřazení osoby již při vytváření dokumentu"
                o24IsEntityRelationRequired.Items(1).Text = "Dokument může být vytvořen bez přiřazení k osobě (k přiřazení může dojít později)"
                Me.o24IsBillingMemo.Visible = True
            Case BO.x29IdEnum.p56Task
                o24IsEntityRelationRequired.Items(0).Text = "Tento typ vyžaduje přiřazení úkolu již při vytváření dokumentu"
                o24IsEntityRelationRequired.Items(1).Text = "Dokument může být vytvořen bez přiřazení k úkolu (k přiřazení může dojít později)"
            Case BO.x29IdEnum.p31Worksheet
                o24IsEntityRelationRequired.Items(0).Text = "Tento typ vyžaduje přiřazení worksheet úkonu již při vytváření dokumentu"
                o24IsEntityRelationRequired.Items(1).Text = "Vazba k worksheet úkonu může vzniknout později"
            Case BO.x29IdEnum.p91Invoice
                o24IsEntityRelationRequired.Items(0).Text = "Tento typ vyžaduje přiřazení vystavené faktury již při vytváření dokumentu"
                o24IsEntityRelationRequired.Items(1).Text = "Dokument může být vytvořen bez přiřazení k faktuře (k přiřazení může dojít později)"
        End Select
    End Sub
End Class