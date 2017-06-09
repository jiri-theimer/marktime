Public Class o23_framework_detail
    Inherits System.Web.UI.Page

    Protected WithEvents _MasterPage As SubForm
    Private Const _key As String = "Aesthe22derm"

    Private Sub o23_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        With Master
            menu1.Factory = .Factory
            menu1.DataPrefix = "o23"
            ff1.Factory = .Factory
            Me.Fileupload_list__readonly.Factory = .Factory
            dropbox1.Factory = .Factory
            upload1.Factory = .Factory
        End With
    End Sub

  
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("verified") = ""
            upload1.GUID = BO.BAS.GetGUID
            With Master
                .SiteMenuValue = "o23"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .Factory.SysUser.OneContactPage <> "" Then
                    Server.Transfer(basUI.AddQuerystring2Page(.Factory.SysUser.OneContactPage, "pid=" & .DataPID.ToString))
                End If

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("o23_menu-tabskin")
                    .Add("o23_menu-menuskin")
                    .Add("o23_framework_detail-pid")
                    .Add("o23_framework_detail-chkFFShowFilledOnly")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                End With
                With .Factory.j03UserBL

                    If Master.DataPID = 0 Then
                        Master.DataPID = BO.BAS.IsNullInt(.GetUserParam("o23_framework_detail-pid", "O"))
                        If Master.DataPID = 0 Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=o23")
                    Else
                        If Master.DataPID <> BO.BAS.IsNullInt(.GetUserParam("o23_framework_detail-pid", "O")) Then
                            .SetUserParam("o23_framework_detail-pid", Master.DataPID.ToString)
                        End If
                    End If
                    menu1.TabSkin = .GetUserParam("o23_menu-tabskin")
                    menu1.MenuSkin = .GetUserParam("o23_menu-menuskin")
                    Me.chkFFShowFilledOnly.Checked = BO.BAS.BG(.GetUserParam("o23_framework_detail-chkFFShowFilledOnly", "0"))
                End With

            End With


        End If

        RefreshRecord()
    End Sub

    Private Sub Handle_Permissions(cRec As BO.o23Notepad, cDisp As BO.o23RecordDisposition)
        With cDisp
            If Not .ReadAccess Then
                Master.StopPage("Nedisponujete oprávněním přistupovat k tomuto dokumentu.")
            End If
            x18_binding.Visible = .OwnerAccess
            If Not .OwnerAccess Then
                lblPermissionMessage.Text = "Disponujete základní úrovní přístupu k dokumentu."
            End If
        End With
        If cRec.b02ID = 0 And cRec.o23IsDraft And cDisp.OwnerAccess Then
            panDraftCommands.Visible = True 'pokud je vlastník a dokument nemá workflow šablonu
        Else
            panDraftCommands.Visible = False
        End If

        upload1.Visible = cDisp.FileAppender

        Select Case cRec.o23LockedFlag
            Case BO.o23LockedTypeENUM.LockAllFiles
                upload1.Visible = False   'přístup k souborům dokumentu uzamčen
                Fileupload_list__readonly.LockFlag = CInt(cRec.o23LockedFlag)
                filesPreview.Visible = False
            Case Else
                Fileupload_list__readonly.LockFlag = 0
        End Select





    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.o23NotepadGrid = Master.Factory.o23NotepadBL.Load4Grid(Master.DataPID)
        If cRec Is Nothing Then Response.Redirect("entity_framework_detail_missing.aspx?prefix=o23")
        Dim cDisp As BO.o23RecordDisposition = Master.Factory.o23NotepadBL.InhaleRecordDisposition(cRec)

        menu1.o23_RefreshRecord(cRec, "board", cDisp)

        If cRec.o23IsEncrypted And ViewState("verified") <> "1" Then
            tableRecord.Visible = False
            menu1.Visible = False
            panEntryPassword.Visible = True
            panBody.Visible = False
            comments1.Visible = False
            Return
        Else
            tableRecord.Visible = True
            menu1.Visible = True
            panEntryPassword.Visible = False
        End If
        Handle_Permissions(cRec, cDisp)



        Me.BindEntity.Text = "Zatím čeká na přiřazení k dokumentu..." : clue_bind_entity.Visible = False
        With cRec
            boxCoreTitle.Text = cRec.o24Name & " (" & .o23Code & ")"
            If .b02ID <> 0 Then
                Me.boxCoreTitle.Text += ": " & .b02Name
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
            Me.clue_o24.Attributes("rel") = "clue_o24_record.aspx?pid=" & .o24ID.ToString

            Me.Owner.Text = .Owner : Me.Timestamp.Text = .Timestamp
            Me.o24Name.Text = .o24Name
            Me.o23Name.Text = .o23Name
            If .o23Code <> "" Then
                Me.o23Name.Text += " <span style='color:gray;padding-left:10px;'>" & .o23Code & "</span>"
            End If
            If .p41ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p41Project Then
                    Me.BindEntity.Text = .Project
                    Me.BindEntity.NavigateUrl = "p41_framework.aspx?pid=" & .p41ID.ToString
                    Me.BindEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Project
                    Me.clue_bind_entity.Attributes("rel") = "clue_p41_record.aspx?pid=" & .p41ID.ToString
                Else
                    lblBindTemp.Text = "Projekt:"
                    Me.BindTempEntity.Text = .Project
                    Me.BindTempEntity.NavigateUrl = "p41_framework.aspx?pid=" & .p41ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Project
                    Me.clue_bindtemp_entity.Attributes("rel") = "clue_p41_record.aspx?pid=" & .p41ID.ToString
                End If

            End If
            If .p28ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p28Contact Then
                    Me.BindEntity.Text = .p28Name
                    Me.BindEntity.NavigateUrl = "p28_framework.aspx?pid=" & .p28ID.ToString
                    Me.BindEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Contact
                    Me.clue_bind_entity.Attributes("rel") = "clue_p28_record.aspx?pid=" & .p28ID.ToString
                Else
                    lblBindTemp.Text = "Klient:"
                    Me.BindTempEntity.Text = .p28Name
                    Me.BindTempEntity.NavigateUrl = "p28_framework.aspx?pid=" & .p28ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Contact
                    Me.clue_bindtemp_entity.Attributes("rel") = "clue_p28_record.aspx?pid=" & .p28ID.ToString
                End If
            End If
            If .p91ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p91Invoice Then
                    Me.BindEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p91Invoice, .p91ID)
                    Me.BindEntity.NavigateUrl = "p28_framework.aspx?pid=" & .p41ID.ToString
                    Me.BindEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Invoice
                    Me.clue_bind_entity.Attributes("rel") = "clue_p91_record.aspx?pid=" & .p91ID.ToString
                Else
                    lblBindTemp.Text = "Faktura:"
                    Me.BindTempEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p91Invoice, .p91ID)
                    Me.BindTempEntity.NavigateUrl = "p28_framework.aspx?pid=" & .p91ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.SysUser.j04IsMenu_Invoice
                    Me.clue_bindtemp_entity.Attributes("rel") = "clue_p91_record.aspx?pid=" & .p91ID.ToString
                End If

            End If
            If .p56ID <> 0 Then
                If .x29ID = BO.x29IdEnum.p56Task Then
                    Me.BindEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .p56ID)
                    Me.BindEntity.NavigateUrl = "p56_framework.aspx?pid=" & .p56ID.ToString
                    Me.BindEntity.Enabled = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P56_Creator)
                    Me.clue_bind_entity.Attributes("rel") = "clue_p56_record.aspx?pid=" & .p41ID.ToString
                Else
                    lblBindTemp.Text = "Úkol:"
                    Me.BindTempEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .p56ID)
                    Me.BindTempEntity.NavigateUrl = "p56_framework.aspx?pid=" & .p56ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.TestPermission(BO.x53PermValEnum.GR_P56_Creator)
                    Me.clue_bindtemp_entity.Attributes("rel") = "clue_p56_record.aspx?pid=" & .p56ID.ToString
                End If
            End If
            If .j02ID <> 0 Then
                If .x29ID = BO.x29IdEnum.j02Person Then
                    Me.BindEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, .j02ID)
                    Me.BindEntity.NavigateUrl = "j02_framework.aspx?pid=" & .j02ID.ToString
                    Me.BindEntity.Enabled = Master.Factory.SysUser.j04IsMenu_People
                    Me.clue_bind_entity.Attributes("rel") = "clue_j02_record.aspx?pid=" & .j02ID.ToString
                Else
                    lblBindTemp.Text = "Osoba:"
                    Me.BindTempEntity.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, .p56ID)
                    Me.BindTempEntity.NavigateUrl = "j02_framework.aspx?pid=" & .j02ID.ToString
                    Me.BindTempEntity.Enabled = Master.Factory.SysUser.j04IsMenu_People
                    Me.clue_bindtemp_entity.Attributes("rel") = "clue_j02_record.aspx?pid=" & .j02ID.ToString
                End If
            End If
            If .p31ID <> 0 Then
                Dim cP31 As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(.p31ID)
                Me.BindEntity.Text = cP31.ClientName & " - " & cP31.p41Name
                If cP31.p33ID = BO.p33IdENUM.PenizeBezDPH Or cP31.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                    Me.BindEntity.Text += " <span class='badge1'>" & BO.BAS.FN(cP31.p31Amount_WithoutVat_Orig) & " " & cP31.j27Code_Billing_Orig & "</span>"
                End If
                Me.BindEntity.NavigateUrl = "javascript:p31_record(" & .p31ID.ToString & ")"
                Me.clue_bind_entity.Attributes("rel") = "clue_p31_record.aspx?pid=" & .p31ID.ToString
                lblBind.Text = cP31.p34Name & " (úkon):"
            End If
            If Me.clue_bind_entity.Attributes("rel") <> "" Then
                Me.clue_bind_entity.Visible = True
            Else
                Me.BindEntity.ForeColor = Drawing.Color.Red
            End If
            If Me.clue_bindtemp_entity.Attributes("rel") <> "" Then
                Me.trBindTemp.Visible = True
            Else
                Me.trBindTemp.Visible = False
            End If

            panBody.Visible = False
            With cRec
                If .o23BodyPlainText <> "" Then
                    If .o23IsEncrypted Then
                        If ViewState("verified") = "1" Then
                            Me.o23BodyPlainText.Text = BO.BAS.CrLfText2Html(BO.Crypto.Decrypt(.o23BodyPlainText, _key))
                            If Me.o23BodyPlainText.Text <> "" Then panBody.Visible = True
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
        With Me.filesPreview

            If Fileupload_list__readonly.ItemsCount > 0 Then
                .Text = BO.BAS.OM2(.Text, Fileupload_list__readonly.ItemsCount.ToString)
                .NavigateUrl = "javascript:file_preview('o23'," & Master.DataPID.ToString & ")"
            End If

        End With



        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.o23Notepad, cRec.PID)
        Me.roles_notepad.RefreshData(lisX69, cRec.PID)

        Dim lisFF As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.o23Notepad, Master.DataPID, cRec.o24ID)
        If lisFF.Count > 0 Then
            ff1.FillData(lisFF, Not Me.chkFFShowFilledOnly.Checked)
        Else
            boxFF.Visible = False
        End If
        comments1.RefreshData(Master.Factory, BO.x29IdEnum.o23Notepad, Master.DataPID)
        If comments1.RowsCount = 0 Then comments1.Visible = False

        Dim cO24 As BO.o24NotepadType = Master.Factory.o24NotepadTypeBL.Load(cRec.o24ID)
        If cO24.o24MaxOneFileSize > 0 Then upload1.MaxFileSize = cO24.o24MaxOneFileSize
        upload1.AllowedFileExtensions = cO24.o24AllowedFileExtensions

        If cO24.b01ID <> 0 Then
            Me.trWorkflow.Visible = True
            Me.b02Name.Text = cRec.b02Name
        Else
            Me.trWorkflow.Visible = False
        End If

        If Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.o23Notepad).Count > 0 Then
            x18_binding.NavigateUrl = String.Format("javascript:sw_local('x18_binding.aspx?prefix=o23&pid={0}','Images/label_32.png',false);", cRec.PID)
            labels1.RefreshData(BO.x29IdEnum.o23Notepad, cRec.PID, Master.Factory.x18EntityCategoryBL.GetList_X19(BO.x29IdEnum.o23Notepad, cRec.PID))
        Else
            boxX18.Visible = False
        End If

        RefreshDropbox(cO24)

        RefreshImapBox(cRec)
    End Sub

   
   
    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.hidHardRefreshFlag.Value = "" And Me.hidHardRefreshPID.Value = "" Then Return

        Select Case Me.hidHardRefreshFlag.Value
            Case "draft2normal"
                With Master.Factory.o23NotepadBL
                    If .ConvertFromDraft(Master.DataPID) Then
                        ReloadPage(Master.DataPID.ToString)
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End With
            Case "o23-save"
                Master.DataPID = BO.BAS.IsNullInt(Me.hidHardRefreshPID.Value)
            Case "o23-delete"
                Response.Redirect("entity_framework_detail_missing.aspx?prefix=o23")
            Case "lockunlock"
                LockUnlockDocument()
            Case Else

        End Select
        ReloadPage(Master.DataPID.ToString)

        Me.hidHardRefreshFlag.Value = ""
        Me.hidHardRefreshPID.Value = ""

    End Sub

    

    Private Sub ReloadPage(strPID As String)
        Response.Redirect("o23_framework_detail.aspx?pid=" & strPID & "&source=" & menu1.PageSource)
    End Sub

   
    Private Sub chkFFShowFilledOnly_CheckedChanged(sender As Object, e As EventArgs) Handles chkFFShowFilledOnly.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("o23_framework_detail-chkFFShowFilledOnly", BO.BAS.GB(Me.chkFFShowFilledOnly.Checked))
        ReloadPage(Master.DataPID.ToString)
    End Sub

   
    Private Sub RefreshDropbox(cO24 As BO.o24NotepadType)
        boxDropbox.Visible = cO24.o24IsAllowDropbox
        If boxDropbox.Visible Then
            dropbox1.RefreshData(BO.x29IdEnum.o23Notepad, Master.DataPID)
        End If

    End Sub


    Private Sub upload1_AfterUploadAll() Handles upload1.AfterUploadAll
        With Master.Factory.o23NotepadBL
            Dim cRec As BO.o23Notepad = .Load(Master.DataPID)
            If Not .AppendUploadedFiles(cRec, upload1.GUID) Then
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            Else
                ReloadPage(Master.DataPID.ToString)
            End If
        End With
        
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

    

    Private Sub RefreshImapBox(cRec As BO.o23Notepad)
        If cRec.o43ID <> 0 Then
            'dokument byl založen IMAP robotem
            imap1.RefreshData(Master.Factory.o42ImapRuleBL.LoadHistoryByID(cRec.o43ID))
            boxIMAP.Visible = True
        Else
            boxIMAP.Visible = False
        End If
    End Sub

    Private Sub LockUnlockDocument()
        With Master.Factory.o23NotepadBL
            Dim cRec As BO.o23Notepad = .Load(Master.DataPID), b As Boolean = False
            If cRec.o23LockedFlag = BO.o23LockedTypeENUM._NotSpecified Then
                b = .LockFilesInDocument(cRec, BO.o23LockedTypeENUM.LockAllFiles)
            Else
                b = .UnLockFilesInDocument(cRec)
            End If
            If Not b Then
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            Else
                ReloadPage(Master.DataPID.ToString)
            End If
        End With
    End Sub
End Class