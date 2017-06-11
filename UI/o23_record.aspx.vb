Public Class o23_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord
    Private Const _key As String = "Aesthe22derm"
    Public Enum RecordModeEnum
        Editing = 1
        ReadonlyView = 2
        WaitingOnPassword = 3
        WaitingOnDocType = 4
    End Enum
    Public Property CurrentMode As RecordModeEnum
        Get
            Return CType(CInt(Me.hidMode.Value), RecordModeEnum)
        End Get
        Set(value As RecordModeEnum)
            Me.hidMode.Value = CInt(value).ToString
        End Set
    End Property
    Public Property CurrentP31ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p31ID.Value)
        End Get
        Set(value As Integer)
            Me.p31ID.Value = value.ToString
        End Set
    End Property
    Public ReadOnly Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p41ID.Value)
        End Get
        
    End Property
    Private Sub o23_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "o23_record"
    End Sub
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            Me.hidMasterPID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return Me.hidMasterPrefix.Value
        End Get
        Set(value As String)
            Me.hidMasterPrefix.Value = value
        End Set
    End Property
    Public Property CurrentMasterGUID As String
        Get
            Return Me.hidMasterGUID.Value
        End Get
        Set(value As String)
            Me.hidMasterGUID.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentMasterX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
        End Get
    End Property
    Public ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return CType(CInt(Me.hidX29ID.Value), BO.x29IdEnum)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.upload1.Factory = Master.Factory
        Me.uploadlist1.Factory = Master.Factory
        Me.Fileupload_list__readonly.Factory = Master.Factory
        ff1.Factory = Master.Factory
        ff2.Factory = Master.Factory
        roles1.Factory = Master.Factory


        If Not Page.IsPostBack Then
            ViewState("verified") = ""
            Me.upload1.GUID = BO.BAS.GetGUID
            Me.uploadlist1.GUID = Me.upload1.GUID
            Me.CurrentMasterGUID = Request.Item("masterguid")
            
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Dokument"

                Me.CurrentMasterPrefix = Request.Item("masterprefix")
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                If .DataPID <> 0 Then
                    .AddToolbarButton("Přejít do rozhraní [Dokumenty]", "go2module", 10, "Images/go2module.png", False, "javascript:go2module()")
                End If
                If .DataPID = 0 Or .IsRecordClone Then
                    .neededPermission = BO.x53PermValEnum.GR_O23_Creator
                    .neededPermissionIfSecond = BO.x53PermValEnum.GR_O23_Draft_Creator
                End If
            End With
            If Me.CurrentMasterPrefix = "" Then
                opgQueue.SelectedIndex = 0
                opgQueue.Visible = False    'možnost spárovat dokument ve frontě skrýt
            End If

            If Master.DataPID = 0 Then  ''And (Me.CurrentMasterPID = 0 Or Me.CurrentMasterPrefix = "") 
                'na vstupu pro nový záznam není volající záznam
                Me.CurrentMode = RecordModeEnum.WaitingOnDocType
            End If

            RefreshRecord()

            If Master.IsRecordClone Then
                Me.upload1.GUID = BO.BAS.GetGUID
                Me.uploadlist1.GUID = Me.upload1.GUID
                Master.DataPID = 0
                Me.o23Date.SelectedDate = Now
                Me.uploadlist1.RefreshData_TEMP()
            End If

            If Master.DataPID <> 0 Then
                Master.HeaderText = Master.Factory.GetRecordCaption(BO.x29IdEnum.o23Notepad, Master.DataPID)
                If Me.CurrentMode = RecordModeEnum.ReadonlyView Or Me.CurrentMode = RecordModeEnum.WaitingOnPassword Then
                    comments1.RefreshData(Master.Factory, BO.x29IdEnum.o23Notepad, Master.DataPID)
                Else
                    comments1.Visible = False
                End If

            Else
                comments1.Visible = False
            End If

        End If
    End Sub

    Private Sub Handle_ChangeO24ID()
        panP41.Visible = False : panP28.Visible = False : panP56.Visible = False : panP91.Visible = False : panP31.Visible = False

        Dim intO24ID As Integer = BO.BAS.IsNullInt(Me.o24ID.SelectedValue)
        Me.clue_o24.Attributes("rel") = "clue_o24_record.aspx?pid=" & intO24ID.ToString

        Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.o23Notepad, Master.DataPID, intO24ID)
        Dim lisX18 As IEnumerable(Of BO.x18EntityCategory) = Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.o23Notepad)
        ff1.FillData(fields, lisX18, "o23Notepad_FreeField", Master.DataPID)
        ''.Text = String.Format(.Text, ff1.FieldsCount, lisX18.Count)

        ff2.FillData(fields, True)
        If ff1.FieldsCount = 0 And ff1.TagsCount = 0 Then
            panFF.Visible = False
        Else
            panFF.Visible = True
        End If


        If intO24ID = 0 Then Return
        Dim cRec As BO.o24NotepadType = Master.Factory.o24NotepadTypeBL.Load(intO24ID)
        Master.HeaderText = cRec.o24Name
        Me.hidX29ID.Value = CInt(cRec.x29ID).ToString
        upload1.MaxFileSize = cRec.o24MaxOneFileSize
        upload1.AllowedFileExtensions = cRec.o24AllowedFileExtensions
        Select Case cRec.x29ID
            Case BO.x29IdEnum.p41Project
                panP41.Visible = True
                If Me.CurrentP41ID = 0 And Not cRec.o24IsEntityRelationRequired Then
                    panP28.Visible = True
                    Me.lblMessage.Text = "Pokud zatím nelze přiřadit projekt, můžete vybrat alespoň klienta (pokud ho znáte). Přiřazení dokumentu k projektu může být provedeno později."
                End If
            Case BO.x29IdEnum.p28Contact
                panP28.Visible = True
            Case BO.x29IdEnum.j02Person
                panJ02.Visible = True
            Case BO.x29IdEnum.p56Task
                panP56.Visible = True
            Case BO.x29IdEnum.p91Invoice
                panP91.Visible = True
            Case BO.x29IdEnum.p31Worksheet
                If Me.CurrentMasterGUID = "" Then panP31.Visible = True
                If Me.CurrentP31ID = 0 And Me.CurrentMasterGUID = "" Then
                    'pokud chybí vazba na úkon a je prázdný externí guid, povolit vazbu na projekt nebo klienta
                    panP41.Visible = True
                    panP28.Visible = True
                    Me.lblMessage.Text = "Přiřazení dokumentu k worksheet úkonu bude provedeno později. Už nyní můžete přiřadit projekt nebo klienta (pokud je znáte)."
                End If
              
        End Select
    End Sub

    Private Sub SetupO24List()
        Dim lisO24 As IEnumerable(Of BO.o24NotepadType) = Master.Factory.o24NotepadTypeBL.GetList(New BO.myQuery)
        If Me.CurrentMasterX29ID > BO.x29IdEnum._NotSpecified Then
            lisO24 = lisO24.Where(Function(p) p.x29ID = Me.CurrentMasterX29ID)
        End If
        Me.o24ID.DataSource = lisO24
        Me.o24ID.DataBind()
    End Sub

    Private Sub InhaleMyDefault()
        If Master.DataPID <> 0 Then Return
        Me.o23Date.SelectedDate = Now
        Me.j02ID_Owner.Value = Master.Factory.SysUser.j02ID.ToString
        Me.j02ID_Owner.Text = Master.Factory.SysUser.PersonDesc

        Dim cRecLast As BO.o23Notepad = Master.Factory.o23NotepadBL.LoadMyLastCreated()
        If cRecLast Is Nothing Then Return
        With cRecLast
            If Me.CurrentMode = RecordModeEnum.WaitingOnDocType Then
                basUI.SelectRadiolistValue(Me.opgSelectDocType, .o24ID.ToString)
            Else
                Me.o24ID.SelectedValue = .o24ID.ToString
            End If

            roles1.InhaleInitialData(.PID)
        End With
    End Sub

    Private Sub RefreshRecord()
        SetupO24List()

        If Master.DataPID = 0 Then
            If Me.CurrentMode = RecordModeEnum.WaitingOnDocType And Me.CurrentMasterPrefix = "" Then Return
            Me.o23IsDraft.Visible = True
            If Me.CurrentMasterPID <> 0 And Me.CurrentMasterPrefix <> "" Then
                Select Case Me.CurrentMasterX29ID
                    Case BO.x29IdEnum.p41Project
                        Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(Me.CurrentMasterPID)
                        Me.p41ID.Value = Me.CurrentMasterPID.ToString
                        Me.p41ID.Text = cP41.FullName
                    Case BO.x29IdEnum.p28Contact
                        Me.p28ID.Value = Me.CurrentMasterPID.ToString
                        Me.p28ID.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, Me.CurrentMasterPID, True)
                    Case BO.x29IdEnum.j02Person
                        Me.j02ID.Value = Me.CurrentMasterPID.ToString
                        Me.j02ID.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, Me.CurrentMasterPID, True)
                    Case BO.x29IdEnum.p56Task
                        Me.p56_record.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, Me.CurrentMasterPID)
                        Me.p56ID.Value = Me.CurrentMasterPID.ToString
                    Case BO.x29IdEnum.p31Worksheet
                        Me.p31_record.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p31Worksheet, Me.CurrentMasterPID)
                        Me.CurrentP31ID = Me.CurrentMasterPID

                End Select
            End If
            InhaleMyDefault()

            If Me.o24ID.Rows > 1 And Me.o24ID.SelectedValue = "" Then Me.o24ID.SelectedIndex = 1
            Handle_ChangeO24ID()
            Return
        End If

        Dim cRec As BO.o23NotepadGrid = Master.Factory.o23NotepadBL.Load4Grid(Master.DataPID)
        If cRec Is Nothing Then Master.StopPage("record not found.")
        Handle_Permissions_and_CurrentMode(cRec)

        With cRec
            Select Case Me.CurrentMode
                Case RecordModeEnum.ReadonlyView
                    If .o23IsEncrypted Then
                        Me.o23BodyPlainText_readonly.Text = BO.BAS.CrLfText2Html(BO.Crypto.Decrypt(.o23BodyPlainText, _key))
                    Else
                        Me.o23BodyPlainText_readonly.Text = BO.BAS.CrLfText2Html(.o23BodyPlainText)
                    End If
                    If o23BodyPlainText_readonly.Text <> "" Then Me.panBodyReadonly.Visible = True Else Me.panBodyReadonly.Visible = False
                    Me.Fileupload_list__readonly.RefreshData_O23(Master.DataPID)
                    Fileupload_list__readonly.LockFlag = CInt(cRec.o23LockedFlag)
                    If Me.Fileupload_list__readonly.ItemsCount > 0 Then
                        filesPreview_readonly.NavigateUrl = "fileupload_preview.aspx?prefix=o23&pid=" & .PID.ToString
                        filesPreview_readonly.Text = BO.BAS.OM2(filesPreview_readonly.Text, Fileupload_list__readonly.ItemsCount.ToString)
                    End If

                    Me.o24Name_readonly.Text = .o24Name
                    If .o24IsBillingMemo Then
                        Me.o23Name_readonly.Text += "<img src='Images/billing_32.png'/>" & .o23Name
                    Else
                        Me.o23Name_readonly.Text += .o23Name
                    End If
                    Me.o23Code_readonly.Text = .o23Code
                    
                    Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.o23Notepad, Master.DataPID, cRec.o24ID)
                    ff2.FillData(fields, True)

                    Me.Timestamp_readonly.Text = .Timestamp
                Case RecordModeEnum.Editing
                    Me.o23Name.Text = .o23Name
                    Me.o23Date.SelectedDate = .o23Date
                    If Not BO.BAS.IsNullDBDate(.o23ReminderDate) Is Nothing Then
                        Me.o23ReminderDate.SelectedDate = .o23ReminderDate
                    End If
                    Me.j02ID_Owner.Value = .j02ID_Owner.ToString
                    Me.j02ID_Owner.Text = .Owner

                    Me.o23IsEncrypted.Checked = .o23IsEncrypted
                    If .o23IsEncrypted Then
                        Me.o23BodyPlainText.Text = BO.Crypto.Decrypt(.o23BodyPlainText, _key)
                    Else
                        Me.o23BodyPlainText.Text = .o23BodyPlainText
                    End If
                    Me.o24ID.SelectedValue = .o24ID.ToString
                    

                    Me.p41ID.Value = .p41ID.ToString
                    Me.p41ID.Text = .Project
                    Me.p28ID.Value = .p28ID.ToString
                    Me.p28ID.Text = .p28Name
                    Me.j02ID.Value = .j02ID.ToString
                    Me.j02ID.Text = .Person
                    Me.CurrentP31ID = .p31ID
                    If .p31ID <> 0 Then
                        cmdClearP31ID.Visible = True
                    Else
                        cmdClearP31ID.Visible = False
                    End If
                    Handle_ChangeO24ID()
                    Master.Timestamp = .Timestamp

                    If .p56ID <> 0 Then
                        Me.p56_record.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p56Task, .p56ID)
                        Me.p56ID.Value = .p56ID.ToString
                    End If
                    If Not Master.IsRecordClone Then
                        If .p31ID <> 0 Then
                            Me.p31_record.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p31Worksheet, .p31ID)
                        End If

                        Master.Factory.o27AttachmentBL.CopyRecordsToTemp(upload1.GUID, Master.DataPID, BO.x29IdEnum.o23Notepad)
                        uploadlist1.RefreshData_TEMP()
                    End If

                    Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
                    roles1.InhaleInitialData(cRec.PID)
                Case Else
            End Select
            
        End With
        

    End Sub

    Private Sub Handle_Permissions_and_CurrentMode(cRec As BO.o23Notepad)
        Dim cDisp As BO.o23RecordDisposition = Master.Factory.o23NotepadBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then
            Master.StopPage("Nedisponujete oprávněním číst tento dokument.")
        Else
            If cDisp.OwnerAccess Then
                Me.CurrentMode = RecordModeEnum.Editing
            Else
                Me.CurrentMode = RecordModeEnum.ReadonlyView
            End If
        End If
        With cRec
            If .o23IsEncrypted And ViewState("verified") <> "1" Then
                Me.CurrentMode = RecordModeEnum.WaitingOnPassword
            End If
        End With
        comments1.ShowInsertButton = cDisp.Comments

    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.o23NotepadBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("o23-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        If Me.o23IsEncrypted.Checked Then
            If Not TestSecureState() Then Return
        End If
        upload1.TryUploadhWaitingFilesOnClientSide()
        roles1.SaveCurrentTempData()
        Dim lisFF As List(Of BO.FreeField) = Me.ff1.GetValues()
        Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()
        If roles1.ErrorMessage <> "" Then
            Master.Notify(roles1.ErrorMessage, 2)
            Return
        End If

        With Master.Factory.o23NotepadBL
            Dim cRec As BO.o23Notepad = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.o23Notepad)

            With cRec
                If .PID = 0 Then .o23IsDraft = Me.o23IsDraft.Checked
                .o24ID = BO.BAS.IsNullInt(Me.o24ID.SelectedValue)
                .o23Name = Me.o23Name.Text
                .j02ID_Owner = BO.BAS.IsNullInt(Me.j02ID_Owner.Value)
                If Me.o23Date.IsEmpty Then
                    .o23Date = Now
                Else
                    .o23Date = Me.o23Date.SelectedDate
                End If

                .o23ReminderDate = BO.BAS.IsNullDBDate(Me.o23ReminderDate.SelectedDate)

                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
                .o23IsEncrypted = Me.o23IsEncrypted.Checked
                .o23GUID = Me.CurrentMasterGUID

                If .o23IsEncrypted Then
                    .o23BodyPlainText = BO.Crypto.Encrypt(Me.o23BodyPlainText.Text, _key)
                    If Me.o23password.Text <> "" Then
                        .o23Password = BO.Crypto.Encrypt(Me.o23password.Text, _key)
                    End If
                Else
                    .o23BodyPlainText = Trim(Me.o23BodyPlainText.Text)
                End If

                .p41ID = 0 : .p28ID = 0 : .p56ID = 0 : .p31ID = 0 : .j02ID = 0 : .p91ID = 0
                .p41ID = Me.CurrentP41ID
                .p28ID = BO.BAS.IsNullInt(Me.p28ID.Value)
                .j02ID = BO.BAS.IsNullInt(Me.j02ID.Value)
                .p56ID = BO.BAS.IsNullInt(Me.p56ID.Value)
                .p31ID = Me.CurrentP31ID

            End With




            

            If .Save(cRec, upload1.GUID, lisX69, lisFF) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("o23-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub o23_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        panRecord.Visible = False : panReadonly.Visible = False : panEntryPassword.Visible = False : panSelectDocType.Visible = False
        Select Case Me.CurrentMode
            Case RecordModeEnum.WaitingOnPassword
                panEntryPassword.Visible = True
                Master.HideShowToolbar(False)
            Case RecordModeEnum.Editing
                panRecord.Visible = True
                Master.HideShowToolbar(True)
                With Me.RadTabStrip1.Tabs.FindTabByValue("files")
                    .Text = BO.BAS.OM2(.Text, uploadlist1.ItemsCount.ToString)
                End With
            Case RecordModeEnum.ReadonlyView
                panReadonly.Visible = True
                Master.HideShowToolbar(True)
                Master.HideShowToolbarButton("save", False)
                Master.HideShowToolbarButton("bin", False)
                Master.HideShowToolbarButton("delete", False)
            Case RecordModeEnum.WaitingOnDocType
                Master.HideShowToolbar(False)
                Me.panSelectDocType.Visible = True
                If opgSelectDocType.Items.Count = 0 Then
                    Dim lisO24 As IEnumerable(Of BO.o24NotepadType) = Master.Factory.o24NotepadTypeBL.GetList(New BO.myQuery)
                    If Me.CurrentMasterPrefix <> "" And (Me.CurrentMasterPID <> 0 Or Me.CurrentMasterGUID <> "") Then
                        lisO24 = lisO24.Where(Function(p) p.x29ID = Me.CurrentMasterX29ID)
                    End If
                    For Each c In lisO24
                        opgSelectDocType.Items.Add(New ListItem(c.o24Name, c.PID.ToString))
                    Next
                    'Me.opgSelectDocType.DataSource = lisO24
                    'Me.opgSelectDocType.DataBind()
                    For Each c In lisO24.Where(Function(p) p.o24HelpText <> "")
                        Dim li As ListItem = opgSelectDocType.Items.FindByValue(c.PID.ToString)
                        li.Text += "<a href='#' class='reczoom' title='Nápověda' rel='clue_o24_record.aspx?pid=" & li.Value & "'>i</a>"
                    Next
                End If

            Case Else

        End Select

        Me.panPassword.Visible = Me.o23IsEncrypted.Checked
        
        If Me.CurrentP41ID <> 0 Then
            Me.clue_p41.Attributes.Item("rel") = "clue_p41_record.aspx?mode=readonly&pid=" & Me.CurrentP41ID.ToString : Me.clue_p41.Visible = True
        Else
            Me.clue_p41.Visible = False
        End If
        If Me.p28ID.Value <> "" Then
            Me.clue_p28.Attributes.Item("rel") = "clue_p28_record.aspx?mode=readonly&pid=" & Me.p28ID.Value : Me.clue_p28.Visible = True
        Else
            Me.clue_p28.Visible = False
        End If
        If Me.j02ID.Value <> "" Then
            Me.clue_j02.Attributes.Item("rel") = "clue_j02_record.aspx?mode=readonly&pid=" & Me.j02ID.Value : Me.clue_j02.Visible = True
        Else
            Me.clue_j02.Visible = False
        End If
        If Me.CurrentP31ID <> 0 Then
            Me.clue_p31.Attributes.Item("rel") = "clue_p31_record.aspx?mode=readonly&pid=" & Me.CurrentP31ID.ToString : clue_p31.Visible = True
        Else
            Me.clue_p31.Visible = False

        End If
        
        If Me.p56ID.Value <> "" Then
            Me.clue_p56.Attributes.Item("rel") = "clue_p56_record.aspx?mode=readonly&pid=" & Me.p56ID.Value : clue_p56.Visible = True
        Else
            Me.clue_p56.Visible = False
        End If
    End Sub

    Private Function TestSecureState() As Boolean
        If Len(o23password.Text) < 4 And Len(o23password.Text) > 0 Then
            Master.Notify("Minimální délka hesla jsou 4 znaky.", 2)
            Return False
        End If
        If o23password.Text <> txtVerify.Text And (txtVerify.Text <> "" Or o23password.Text <> "") Then
            Master.Notify("Heslo nesouhlasí s ověřením.", 2)
            Return False
        End If
        If Len(o23password.Text) < 4 Then
            If Master.IsRecordNew Then
                Master.Notify("Zadejte heslo (minimálně 4 znaky).", 2)
                o23password.Focus()
                Return False
            Else
                Dim cRecCurrent As BO.o23Notepad = Master.Factory.o23NotepadBL.Load(Master.DataPID)
                If Not cRecCurrent.o23IsEncrypted Then    'dokument dříve nebyl zašifrovaný a nyní má být a heslo je nedostatečné
                    Master.Notify("Zadejte heslo (minimálně 4 znaky).", 2)
                    o23password.Focus()
                    Return False
                End If
            End If

        End If
        Return True
    End Function

    Private Sub cmdUnLock_Click(sender As Object, e As EventArgs) Handles cmdUnLock.Click
        Dim cRec As BO.o23Notepad = Master.Factory.o23NotepadBL.Load(Master.DataPID)
        Dim strPWD As String = BO.Crypto.Decrypt(cRec.o23Password, _key)
        If Me.txtEntryPassword.Text <> strPWD Then
            Master.Notify("Heslo není správné", NotifyLevel.WarningMessage)
            Me.txtEntryPassword.Focus()
            Return
        Else
            ViewState("verified") = "1"
            Me.CurrentMode = RecordModeEnum.Editing
            RefreshRecord()
        End If



    End Sub

    Private Sub upload1_AfterUploadAll() Handles upload1.AfterUploadAll
        Me.uploadlist1.RefreshData_TEMP()
        Me.lblFilesMessage.Text = "Souborové přílohy se uloží do dokumentu až tlačítkem [Uložit změny]."
    End Sub

    Private Sub o24ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles o24ID.SelectedIndexChanged
        Handle_ChangeO24ID()
    End Sub

    Private Sub cmdSelectDocType_Click(sender As Object, e As EventArgs) Handles cmdSelectDocType.Click
        If Me.opgQueue.SelectedValue = "1" Then
            Dim strO24ID As String = ""
            If Not Me.opgSelectDocType.SelectedItem Is Nothing Then
                strO24ID = Me.opgSelectDocType.SelectedValue
            End If
            Server.Transfer("o23_queue.aspx?masterprefix=" & Me.CurrentMasterPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString & "&o24id=" & strO24ID & "&masterguid=" & Me.CurrentMasterGUID, False)
        End If
        If Me.opgSelectDocType.SelectedItem Is Nothing Then
            Master.Notify("Musíte vybrat typ dokumentu.", NotifyLevel.WarningMessage)
        Else
            Me.o24ID.SelectedValue = Me.opgSelectDocType.SelectedValue
            Handle_ChangeO24ID()
            Me.CurrentMode = RecordModeEnum.Editing
        End If
    End Sub

    Private Sub cmdAddX69_Click(sender As Object, e As EventArgs) Handles cmdAddX69.Click
        roles1.AddNewRow()
    End Sub

    
    Private Sub uploadlist1_AfterSetAsDeleted(cFile As BO.p85TempBox) Handles uploadlist1.AfterSetAsDeleted
        Me.lblFilesMessage.Text = "Provedené změny se uloží do dokumentu až tlačítkem [Uložit změny]."
    End Sub

    Private Sub cmdClearP31ID_Click(sender As Object, e As EventArgs) Handles cmdClearP31ID.Click
        Me.p31ID.Value = "" : Me.p31_record.Text = ""
        Master.Notify("Vyčištění potvrdíte tlačítkem [Uložit změny].")
    End Sub
End Class