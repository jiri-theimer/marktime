Public Class mobile_o23_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile
    Private Const _key As String = "Aesthe22derm"

    Private Sub mobile_o23_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With Master
            Me.Fileupload_list__readonly.Factory = .Factory
        End With
        If Not Page.IsPostBack Then
            ViewState("verified") = "0"
            With Master
                .MenuPrefix = "o23"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("o23_framework_detail-pid")

                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                End With
                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("o23_framework_detail-pid", "O"))
                    If .DataPID = 0 Then Response.Redirect("mobile_entity_framework_missing.aspx?prefix=o23")
                Else
                    If .DataPID <> BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("o23_framework_detail-pid", "O")) Then
                        .Factory.j03UserBL.SetUserParam("o23_framework_detail-pid", .DataPID.ToString)
                    End If
                End If

            End With

            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.o23NotepadGrid = Master.Factory.o23NotepadBL.Load4Grid(Master.DataPID)

        If cRec.o23IsEncrypted And ViewState("verified") <> "1" Then
            panRecord.Visible = False
            panEntryPassword.Visible = True
            Return
        Else
            panRecord.Visible = True
            panEntryPassword.Visible = False
        End If
        ''Handle_Permissions(cRec)

        Me.BindEntity.Text = "Zatím čeká na přiřazení k dokumentu..."
        With cRec
            Me.RecordHeader.Text = BO.BAS.OM3(.o24Name & ": " & .o23Name, 30)
            Me.RecordHeader.NavigateUrl = "mobile_o23_framework.aspx?pid=" & .PID.ToString
            If .o23Code <> "" Then
                Me.RecordName.Text = "[" & .o23Code & "] " & .o23Name
            Else
                Me.RecordName.Text = .o23Name
            End If
            Select Case .x29ID
                Case BO.x29IdEnum.p41Project
                    lblBind.Text = "Projekt:"
                Case BO.x29IdEnum.p28Contact
                    Me.lblBind.Text = "Klient:"
                Case BO.x29IdEnum.p91Invoice
                    Me.lblBind.Text = "Faktura:"
                Case BO.x29IdEnum.p56Task
                    Me.lblBind.Text = "Úkol:"
                Case BO.x29IdEnum.j02Person
                    Me.lblBind.Text = "Osoba:"
                Case BO.x29IdEnum.p31Worksheet
                    Me.lblBind.Text = "Worksheet úkon:"
            End Select
            Me.Owner.Text = .Owner : Me.Timestamp.Text = .Timestamp
            Me.o24Name.Text = .o24Name


            If .p41ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p41Project Then
                    Me.BindEntity.Text = .Project
                    Me.BindEntity.NavigateUrl = "mobile_p41_framework.aspx?pid=" & .p41ID.ToString
                Else
                    lblBindTemp.Text = "Projekt:"
                    Me.BindTempEntity.Text = .Project
                    Me.BindTempEntity.NavigateUrl = "mobile_p41_framework.aspx?pid=" & .p41ID.ToString
                End If
            End If
            If .p28ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p28Contact Then
                    Me.BindEntity.NavigateUrl = "mobile_p28_framework.aspx?pid=" & .p28ID.ToString
                    Me.BindEntity.Text = .p28Name
                Else
                    lblBindTemp.Text = "Klient:"
                    Me.BindTempEntity.Text = .Project
                    Me.BindTempEntity.NavigateUrl = "mobile_p28_framework.aspx?pid=" & .p41ID.ToString
                End If
            End If
            If .p91ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p91Invoice Then
                    Me.BindEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p91Invoice, .p91ID)
                    Me.BindEntity.NavigateUrl = "mobile_p28_framework.aspx?pid=" & .p41ID.ToString
                    Me.BindEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Invoice
                Else
                    lblBindTemp.Text = "Faktura:"
                    Me.BindTempEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p91Invoice, .p91ID)
                    Me.BindTempEntity.NavigateUrl = "mobile_p28_framework.aspx?pid=" & .p91ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Invoice
                End If

            End If
            If .p56ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p56Task Then
                    Me.BindEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .p56ID)
                    Me.BindEntity.NavigateUrl = "p56_framework.aspx?pid=" & .p56ID.ToString
                Else
                    lblBindTemp.Text = "Úkol:"
                    Me.BindTempEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .p56ID)
                    Me.BindTempEntity.NavigateUrl = "p56_framework.aspx?pid=" & .p56ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P56_Creator)
                End If
            End If
            If .j02ID <> 0 Then
                If .x29ID = BO.x29IdEnum.j02Person Then
                    Me.BindEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, .j02ID)
                Else
                    lblBindTemp.Text = "Osoba:"
                    Me.BindTempEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, .p56ID)
                End If
            End If
            If .p31ID <> 0 Then
                Dim cP31 As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(.p31ID)
                Me.BindEntity.Text = cP31.ClientName & " - " & cP31.p41Name
                If cP31.p33ID = BO.p33IdENUM.PenizeBezDPH Or cP31.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                    Me.BindEntity.Text += " <span class='badge1'>" & BO.BAS.FN(cP31.p31Amount_WithoutVat_Orig) & " " & cP31.j27Code_Billing_Orig & "</span>"
                End If
                Me.BindEntity.NavigateUrl = "javascript:p31_record(" & .p31ID.ToString & ")"
                lblBind.Text = cP31.p34Name & " (úkon):"
            End If
            
            If Me.BindTempEntity.Text <> "" Then
                Me.trBindTemp.Visible = True
            Else
                Me.trBindTemp.Visible = False
            End If

            With cRec
                If .o23BodyPlainText <> "" Then
                    If .o23IsEncrypted Then
                        If ViewState("verified") = "1" Then
                            .o23BodyPlainText = BO.Crypto.Decrypt(.o23BodyPlainText, _key)
                            Me.o23BodyPlainText.Text = BO.BAS.CrLfText2Html(.o23BodyPlainText)
                            panBody.Visible = True
                        End If
                    Else
                        Me.o23BodyPlainText.Text = BO.BAS.CrLfText2Html(.o23BodyPlainText) : panBody.Visible = True
                    End If
                End If
            End With

            Me.o23Date.Text = BO.BAS.FD(.o23Date, True)
            imgDraft.Visible = .o23IsDraft
            If .o23LockedFlag > BO.o23LockedTypeENUM._NotSpecified Then
                imgRecord.ImageUrl = "Images/lock.png"

            End If

        End With

        Me.Fileupload_list__readonly.RefreshData_O23(Master.DataPID)
        If Me.Fileupload_list__readonly.ItemsCount = 0 Then
            panFiles.Visible = False
        End If
        ''With Me.filesPreview

        ''    If Fileupload_list__readonly.ItemsCount > 0 Then
        ''        .Text = BO.BAS.OM2(.Text, Fileupload_list__readonly.ItemsCount.ToString)
        ''        .NavigateUrl = "javascript:file_preview('o23'," & Master.DataPID.ToString & ")"
        ''    End If

        ''End With



        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.o23Notepad, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)

        Dim cO24 As BO.o24NotepadType = Master.Factory.o24NotepadTypeBL.Load(cRec.o24ID)
        If cO24.b01ID <> 0 Then
            Me.trB02.Visible = True
            Me.b02Name.Text = cRec.b02Name
        Else
            Me.trB02.Visible = False
        End If

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.o23Notepad).Count > 0 Then
            labels1.RefreshData(BO.x29IdEnum.o23Notepad, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.o23Notepad, cRec.PID))
        Else
            boxX18.Visible = False
        End If
    End Sub

    Private Sub cmdDecrypt_Click(sender As Object, e As EventArgs) Handles cmdDecrypt.Click
        Dim cRec As BO.o23Notepad = Master.Factory.o23NotepadBL.Load(Master.DataPID)
        Dim strPWD As String = BO.Crypto.Decrypt(cRec.o23Password, _key)
        If Me.txtEntryPassword.Text <> strPWD Then
            Master.Notify("Heslo není správné", NotifyLevel.WarningMessage)
            Me.txtEntryPassword.Focus()
            Return
        Else
            ViewState("verified") = "1"
            RefreshRecord()
        End If
    End Sub
End Class