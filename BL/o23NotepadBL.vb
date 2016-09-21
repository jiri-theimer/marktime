
Public Interface Io23NotepadBL
    Inherits IFMother
    Function Save(cRec As BO.o23Notepad, strUploadGUID As String, lisX69 As List(Of BO.x69EntityRole_Assign), lisFF As List(Of BO.FreeField)) As Boolean
    Function Load(intPID As Integer) As BO.o23Notepad
    Function Load4Grid(intPID As Integer) As BO.o23NotepadGrid
    Function LoadMyLastCreated() As BO.o23Notepad
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQueryO23) As IEnumerable(Of BO.o23Notepad)
    Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.o23NotepadGrid)
    Function GetVirtualCount(myQuery As BO.myQueryO23) As Integer
    Function GetList4Grid(myQuery As BO.myQueryO23) As IEnumerable(Of BO.o23NotepadGrid)
    Function GetGridDataSource(myQuery As BO.myQueryO23) As DataTable
    Sub Handle_Reminder()
    Function InhaleRecordDisposition(cRec As BO.o23Notepad) As BO.o23RecordDisposition
    Sub UpdateSelectedNotepadRole(intX67ID As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), intO23ID As Integer)
    Sub ClearSelectedNotepadRole(intX67ID As Integer, intO23ID As Integer)
    Function ConvertFromDraft(intPID As Integer) As Boolean
    Function AppendUploadedFiles(cRec As BO.o23Notepad, strUploadGUID As String) As Boolean
    Function LockFilesInDocument(cRec As BO.o23Notepad, lockFlag As BO.o23LockedTypeENUM) As Boolean
    Function UnLockFilesInDocument(cRec As BO.o23Notepad) As Boolean
    Function UpdateImapSource(intPID As Integer, intO43ID As Integer) As Boolean
End Interface
Class o23NotepadBL
    Inherits BLMother
    Implements Io23NotepadBL
    Private WithEvents _cDL As DL.o23NotepadDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.o23NotepadDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function AppendUploadedFiles(cRec As BO.o23Notepad, strUploadGUID As String) As Boolean Implements Io23NotepadBL.AppendUploadedFiles
        If strUploadGUID = "" Then _Error = "Na vstupu chybí GUID." : Return False
        Dim lisTempUpload As IEnumerable(Of BO.p85TempBox) = Me.Factory.p85TempBoxBL.GetList(strUploadGUID, False)
        If lisTempUpload.Count = 0 Then
            _Error = "Na vstupu je 0 nahraných souborů." : Return False
        End If
        If Me.Factory.o27AttachmentBL.UploadAndSaveUserControl(lisTempUpload, BO.x29IdEnum.o23Notepad, cRec.PID) Then
            Return True
        Else
            _Error = Me.Factory.o27AttachmentBL.ErrorMessage : Return False
        End If

    End Function
    Public Function Save(cRec As BO.o23Notepad, strUploadGUID As String, lisX69 As List(Of BO.x69EntityRole_Assign), lisFF As List(Of BO.FreeField)) As Boolean Implements Io23NotepadBL.Save
        Dim lisTempUpload As IEnumerable(Of BO.p85TempBox) = Nothing

        If strUploadGUID <> "" Then
            lisTempUpload = Me.Factory.p85TempBoxBL.GetList(strUploadGUID, True)
        End If
        With cRec
            If .o24ID = 0 Then
                _Error = "Chybí typ dokumentu" : Return False
            End If
            If BO.BAS.IsNullDBDate(.o23Date) Is Nothing Then
                _Error = "Chybí datum dokumentu." : Return False
            End If
            If Trim(.o23BodyPlainText) = "" And Trim(.o23Name) = "" Then
                _Error = "Minimálně musíte vyplnit buď název nebo podrobný popis dokumentu." : Return False
            End If
            Dim cO24 As BO.o24NotepadType = Factory.o24NotepadTypeBL.Load(.o24ID)
            Select Case cO24.x29ID
                Case BO.x29IdEnum.p28Contact
                    .p41ID = 0 : .p31ID = 0 : .p91ID = 0 : .p56ID = 0 : .j02ID = 0
                    If .p28ID = 0 And cO24.o24IsEntityRelationRequired Then
                        _Error = String.Format("Typ [{0}] vyžaduje přiřazení klienta již při vytváření dokumentu.", cO24.o24Name) : Return False
                    End If
                Case BO.x29IdEnum.p91Invoice
                    .p41ID = 0 : .p31ID = 0 : .p28ID = 0 : .p56ID = 0 : .j02ID = 0
                    If .p91ID = 0 And cO24.o24IsEntityRelationRequired Then
                        _Error = String.Format("Typ [{0}] vyžaduje přiřazení faktury již při vytváření dokumentu.", cO24.o24Name) : Return False
                    End If
                Case BO.x29IdEnum.p41Project
                    .p31ID = 0 : .p56ID = 0 : .p91ID = 0 : .j02ID = 0
                    'typ dokumentu s vazbou na projekt
                    If .p41ID = 0 And cO24.o24IsEntityRelationRequired Then
                        _Error = String.Format("Typ [{0}] vyžaduje přiřazení projektu již při vytváření dokumentu.", cO24.o24Name) : Return False
                    End If
                    If .p41ID <> 0 Then
                        .p28ID = 0  'pokud je již známa vazba na projekt, pak se automaticky ruší případná vazba na klienta
                    End If
                Case BO.x29IdEnum.p31Worksheet
                    .p56ID = 0 : .p91ID = 0 : .j02ID = 0
                    If .p31ID = 0 And cO24.o24IsEntityRelationRequired Then
                        _Error = String.Format("Typ [{0}] vyžaduje přiřazení worksheet úkonu již při vytváření dokumentu.", cO24.o24Name) : Return False
                    End If
                    If .p31ID = 0 Then
                        If .p41ID <> 0 And .p28ID <> 0 Then
                            _Error = "V dokumentu, který čeká na přiřazení k worksheet úkonu, lze zatím přiřadit buď projekt nebo klienta. Nikoliv obojí najednou!" : Return False
                        End If
                    Else
                        .p41ID = 0 : .p28ID = 0       'pokud je známa vazba na p31ID, pak se automaticky ruší vazba projekt a klienta
                    End If
                Case BO.x29IdEnum.p56Task
                    If .p56ID = 0 And cO24.o24IsEntityRelationRequired Then
                        _Error = String.Format("Typ [{0}] vyžaduje přiřazení úkolu již při vytváření dokumentu.", cO24.o24Name) : Return False
                    End If
                Case BO.x29IdEnum.j02Person
                    If .j02ID = 0 And cO24.o24IsEntityRelationRequired Then
                        _Error = String.Format("Typ [{0}] vyžaduje přiřazení osoby již při vytváření dokumentu.", cO24.o24Name) : Return False
                    End If
            End Select
            

            If Not .o23IsEncrypted Then .o23Password = ""
            If .PID = 0 Then
                .ValidFrom = DateSerial(Year(.ValidFrom), Month(.ValidFrom), Day(.ValidFrom))
            End If
            If .j02ID_Owner = 0 Then .j02ID_Owner = _cUser.j02ID
        End With
        If Not lisFF Is Nothing Then
            If Not BL.BAS.ValidateFF(lisFF) Then
                _Error = BL.BAS.ErrorMessage : Return False
            End If
        End If
        If Not Me.RaiseAppEvent_TailoringTestBeforeSave(cRec, lisFF, "o23") Then Return False

        If _cDL.Save(cRec, lisX69, lisFF) Then
            If strUploadGUID <> "" Then
                Me.Factory.o27AttachmentBL.UploadAndSaveUserControl(lisTempUpload, BO.x29IdEnum.o23Notepad, Me.LastSavedPID)
            End If
            Me.RaiseAppEvent_TailoringAfterSave(_LastSavedPID, "o23")
            Dim cO24 As BO.o24NotepadType = Me.Factory.o24NotepadTypeBL.Load(cRec.o24ID)
            If cRec.PID = 0 Then
                If cO24.b01ID > 0 Then
                    InhaleDefaultWorkflowMove(_LastSavedPID, cO24.b01ID)    'je třeba nahodit výchozí workflow stav
                End If
                Me.RaiseAppEvent(BO.x45IDEnum.o23_new, _LastSavedPID)
            Else
                If cO24.b01ID > 0 And cRec.b02ID = 0 Then
                    InhaleDefaultWorkflowMove(cRec.PID, cO24.b01ID) 'chybí hodnota workflow stavu
                End If
                Me.RaiseAppEvent(BO.x45IDEnum.o23_update, _LastSavedPID)
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub InhaleDefaultWorkflowMove(intO23ID As Integer, intB01ID As Integer)
        Dim cB06 As BO.b06WorkflowStep = Me.Factory.b06WorkflowStepBL.LoadKickOffStep(intB01ID)
        If cB06 Is Nothing Then Return
        Me.Factory.b06WorkflowStepBL.RunWorkflowStep(cB06, intO23ID, BO.x29IdEnum.o23Notepad, "", "", False, Nothing)
    End Sub
    Public Function Load(intPID As Integer) As BO.o23Notepad Implements Io23NotepadBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Load4Grid(intPID As Integer) As BO.o23NotepadGrid Implements Io23NotepadBL.Load4Grid
        Return _cDL.Load4Grid(intPID)
    End Function
    Public Function LoadMyLastCreated() As BO.o23Notepad Implements Io23NotepadBL.LoadMyLastCreated
        Return _cDL.LoadMyLastCreated()
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Io23NotepadBL.Delete
        Dim s As String = Me.Factory.GetRecordCaption(BO.x29IdEnum.o23Notepad, intPID)
        If _cDL.Delete(intPID) Then
            Me.RaiseAppEvent(BO.x45IDEnum.o23_delete, intPID, s)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(mq As BO.myQueryO23) As IEnumerable(Of BO.o23Notepad) Implements Io23NotepadBL.GetList
        Return _cDL.GetList(mq)
    End Function

    Public Sub Handle_Reminder() Implements Io23NotepadBL.Handle_Reminder
        Dim d1 As Date = DateAdd(DateInterval.Day, -2, Now)
        Dim d2 As Date = Now
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = _cDL.GetList_WaitingOnReminder(d1, d2)
        For Each cRec In lisO23
            Me.RaiseAppEvent(BO.x45IDEnum.o23_remind, cRec.PID, cRec.o23Name)

        Next

    End Sub
    Public Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.o23NotepadGrid) Implements Io23NotepadBL.GetList_forMessagesDashboard
        Return _cDL.GetList_forMessagesDashboard(intJ02ID)
    End Function

    Public Function GetVirtualCount(myQuery As BO.myQueryO23) As Integer Implements Io23NotepadBL.GetVirtualCount
        Return _cDL.GetVirtualCount(myQuery)
    End Function
    Public Function GetList4Grid(myQuery As BO.myQueryO23) As IEnumerable(Of BO.o23NotepadGrid) Implements Io23NotepadBL.GetList4Grid
        Return _cDL.GetList4Grid(myQuery)
    End Function

    Public Function InhaleRecordDisposition(cRec As BO.o23Notepad) As BO.o23RecordDisposition Implements Io23NotepadBL.InhaleRecordDisposition
        Dim c As New BO.o23RecordDisposition
        If Factory.SysUser.IsAdmin Or cRec.j02ID_Owner = Factory.SysUser.j02ID Then
            'vlasník záznamu nebo admin má plná práva
            c.ReadAccess = True : c.OwnerAccess = True : c.FileAppender = True : c.Comments = True : c.LockUnlockFiles_Flag1 = True
            Return c
        End If

        If Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.o23Notepad, cRec.PID, BO.x53PermValEnum.DR_O23_Owner, False) Then
            'v dokument roli má vlastnická oprávnění
            c.OwnerAccess = True : c.ReadAccess = True : c.FileAppender = True : c.Comments = True : c.LockUnlockFiles_Flag1 = True
            Return c
        End If
        If Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.o23Notepad, cRec.PID, BO.x53PermValEnum.DR_O23_Reader, True) Then
            'v dokument roli má oprávnění ke čtení dokumentu
            c.ReadAccess = True
        End If
        If Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.o23Notepad, cRec.PID, BO.x53PermValEnum.DR_O23_File_Appender, True) Then
            'v dokument roli má oprávnění nahrávat další přílohy
            c.FileAppender = True
        End If
        If Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.o23Notepad, cRec.PID, BO.x53PermValEnum.DR_O23_Comments, True) Then
            'v dokument roli má oprávnění zapisovat komentáře
            c.Comments = True
        End If
        If Factory.x67EntityRoleBL.TestEntityRolePermission(BO.x29IdEnum.o23Notepad, cRec.PID, BO.x53PermValEnum.DR_O23_Files_Lock1, True) Then
            'oprávnění uzamykat/odemykat přístup k souborům dokumentu
            c.LockUnlockFiles_Flag1 = True
        End If
        Return c
    End Function

    Public Sub UpdateSelectedNotepadRole(intX67ID As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), intO23ID As Integer) Implements Io23NotepadBL.UpdateSelectedNotepadRole
        _cDL.UpdateSelectedNotepadRole(intX67ID, lisX69, intO23ID)
    End Sub
    Public Sub ClearSelectedNotepadRole(intX67ID As Integer, intO23ID As Integer) Implements Io23NotepadBL.ClearSelectedNotepadRole
        _cDL.ClearSelectedNotepadRole(intX67ID, intO23ID)
    End Sub
    Public Function ConvertFromDraft(intPID As Integer) As Boolean Implements Io23NotepadBL.ConvertFromDraft
        Return _cDL.ConvertFromDraft(intPID)
    End Function
    Public Function LockFilesInDocument(cRec As BO.o23Notepad, lockFlag As BO.o23LockedTypeENUM) As Boolean Implements Io23NotepadBL.LockFilesInDocument
        Dim cDisp As BO.o23RecordDisposition = InhaleRecordDisposition(cRec)
        If Not cDisp.LockUnlockFiles_Flag1 Then
            _Error = "Nemáte oprávnění pro uzamykání/odemykání přístupu k souborům dokumentu." : Return False
        End If
        Return _cDL.LockFiles(cRec.PID, lockFlag)
    End Function
    Public Function UnLockFilesInDocument(cRec As BO.o23Notepad) As Boolean Implements Io23NotepadBL.UnLockFilesInDocument
        Dim cDisp As BO.o23RecordDisposition = InhaleRecordDisposition(cRec)
        If Not cDisp.LockUnlockFiles_Flag1 Then
            _Error = "Nemáte oprávnění pro uzamykání/odemykání přístupu k souborům dokumentu." : Return False
        End If
        Return _cDL.UnLockFiles(cRec.PID)
    End Function
    Public Function UpdateImapSource(intPID As Integer, intO43ID As Integer) As Boolean Implements Io23NotepadBL.UpdateImapSource
        Return _cDL.UpdateImapSource(intPID, intO43ID)
    End Function
    Public Function GetGridDataSource(myQuery As BO.myQueryO23) As DataTable Implements Io23NotepadBL.GetGridDataSource
        Return _cDL.GetGridDataSource(myQuery)
    End Function
End Class
