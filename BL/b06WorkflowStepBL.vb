﻿Public Interface Ib06WorkflowStepBL
    Inherits ifMother
    Function Save(cRec As BO.b06WorkflowStep, lisB08 As List(Of BO.b08WorkflowReceiverToStep), lisB11 As List(Of BO.b11WorkflowMessageToStep), lisB10 As List(Of BO.b10WorkflowCommandCatalog_Binding)) As Boolean
    Function Load(intPID As Integer) As BO.b06WorkflowStep
    Function LoadKickOffStep(intB01ID As Integer) As BO.b06WorkflowStep
    Function Delete(intPID As Integer) As Boolean
    Function GetList(intB01ID As Integer) As IEnumerable(Of BO.b06WorkflowStep)
    Function GetList_Allb09IDs() As IEnumerable(Of BO.b09WorkflowCommandCatalog)
    Function GetList_B10(intPID As Integer) As IEnumerable(Of BO.b10WorkflowCommandCatalog_Binding)
    Function GetList_B08(intPID As Integer) As IEnumerable(Of BO.b08WorkflowReceiverToStep)
    Function GetList_B11(intPID As Integer) As IEnumerable(Of BO.b11WorkflowMessageToStep)

    Function GetPossibleWorkflowSteps4Person(strRecordPrefix As String, intRecordPID As Integer, intJ02ID As Integer) As List(Of BO.WorkflowStepPossible4User)

    Function RunWorkflowStep(cB06 As BO.b06WorkflowStep, intRecordPID As Integer, x29id As BO.x29IdEnum, strComment As String, strUploadGUID As String, bolManualStep As Boolean, lisNominee As List(Of BO.x69EntityRole_Assign)) As Boolean
End Interface
Class b06WorkflowStepBL
    Inherits BLMother
    Implements Ib06WorkflowStepBL
    Private WithEvents _cDL As DL.b06WorkflowStepDL

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.b06WorkflowStepDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Delete(intPID As Integer) As Boolean Implements Ib06WorkflowStepBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(intB01ID As Integer) As System.Collections.Generic.IEnumerable(Of BO.b06WorkflowStep) Implements Ib06WorkflowStepBL.GetList
        Return _cDL.GetList(intB01ID)
    End Function

    Public Function Load(intPID As Integer) As BO.b06WorkflowStep Implements Ib06WorkflowStepBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadKickOffStep(intB01ID As Integer) As BO.b06WorkflowStep Implements Ib06WorkflowStepBL.LoadKickOffStep
        Return _cDL.LoadKickOffStep(intB01ID)
    End Function

    Public Function Save(cRec As BO.b06WorkflowStep, lisB08 As List(Of BO.b08WorkflowReceiverToStep), lisB11 As List(Of BO.b11WorkflowMessageToStep), lisB10 As List(Of BO.b10WorkflowCommandCatalog_Binding)) As Boolean Implements Ib06WorkflowStepBL.Save
        If cRec.b02ID = 0 Then
            _Error = "[Workflow stav] je povinná pole k vyplnění."
        End If
        If cRec.b02ID_Target = 0 And Trim(cRec.b06Name) = "" Then
            _Error = "Pokud se nemění cílový stav, název kroku je povinný."
        End If
        If Not cRec.b06IsKickOffStep Then
            If lisB08.Count = 0 And cRec.b06IsManualStep Then _Error = "V nastavení kroku chybí určení, kdo spouští krok." : Return False
        End If
        If cRec.b06IsKickOffStep And cRec.b02ID_Target = 0 Then
            _Error = "Startovací workflow krok musí mít definován cílový stav." : Return False
        End If
        If cRec.b06IsNominee Then
            If cRec.x67ID_Nominee = 0 Then
                _Error = "Pro nominaci musíte specifikovat roli, kterou nominovaný obdrží."
            End If
        Else
            cRec.b06IsNomineeRequired = False : cRec.x67ID_Nominee = 0
        End If
        If cRec.j11ID_Direct <> 0 And cRec.x67ID_Direct = 0 Then
            _Error = "Musíte specifikovat roli automatického řešitele."
        End If
        If cRec.j11ID_Direct = 0 And cRec.x67ID_Direct <> 0 Then
            _Error = "Musíte specifikovat tým osob v automatické změně řešitele."
        End If
       
        If _Error <> "" Then
            Return False
        End If
        If _cDL.Save(cRec, lisB08, lisB11, lisB10) Then
            _LastSavedPID = _cDL.LastSavedRecordPID
            Return True
        Else
            _Error = _cDL.ErrorMessage
            Return False
        End If
    End Function

    Public Function GetList_Allb09IDs() As IEnumerable(Of BO.b09WorkflowCommandCatalog) Implements Ib06WorkflowStepBL.GetList_Allb09IDs
        Return _cDL.GetList_Allb09IDs()
    End Function
    Public Function GetList_B10(intPID As Integer) As IEnumerable(Of BO.b10WorkflowCommandCatalog_Binding) Implements Ib06WorkflowStepBL.GetList_B10
        Return _cDL.GetList_B10(intPID)
    End Function
    Public Function GetList_B08(intPID As Integer) As IEnumerable(Of BO.b08WorkflowReceiverToStep) Implements Ib06WorkflowStepBL.GetList_B08
        Return _cDL.GetList_B08(intPID)
    End Function
    Public Function GetList_B11(intPID As Integer) As IEnumerable(Of BO.b11WorkflowMessageToStep) Implements Ib06WorkflowStepBL.GetList_B11
        Return _cDL.GetList_B11(intPID)
    End Function

    

    Private Function ValidateWorkflowStepBeforeRun(intRecordPID As Integer, x29id As BO.x29IdEnum, cB06 As BO.b06WorkflowStep, strComment As String, strUploadGUID As String, bolManualStep As Boolean, lisNominee As List(Of BO.x69EntityRole_Assign)) As Boolean
        If cB06.PID = 0 Then
            _Error = "Musíte zvolit z nabídky konkrétní krok!"
        End If
        If cB06.b06IsCommentRequired And Trim(strComment) = "" Then
            _Error = "Krok [" & cB06.b06Name & "] vyžaduje zapsat komentář!" : Return False
        End If
        If cB06.b06IsNomineeRequired Then
            If lisNominee Is Nothing Then
                _Error = "Chybí nominace nového řešitele." : Return False                
            End If
            If lisNominee.Count = 0 Then
                _Error = "Chybí nominace nového řešitele." : Return False
            End If
        End If
        If Not lisNominee Is Nothing Then
            For Each c In lisNominee
                If c.j02ID <> 0 And c.j11ID <> 0 Then
                    _Error = "V jednom řádku nominace může být buď pouze osoba nebo pouze tým." : Return False
                End If
            Next
        End If

        If _Error <> "" Then Return False
        If cB06.b06IsRunOneInstanceOnly Then
            'krok lze spustit u akce pouze jednou
            Dim lisHistory As IEnumerable(Of BO.b05Workflow_History) = Me.Factory.b05Workflow_HistoryBL.GetList(intRecordPID, x29id)
            If lisHistory.Where(Function(p) p.b06ID = cB06.PID).Count > 0 Then
                _Error = "Krok [" & cB06.b06Name & "] je povoleno spouštět pouze jednou!"
            End If
        End If
        If _Error <> "" Then Return False

        If cB06.b06ValidateBeforeRunSQL <> "" Then
            'spuštění kroku je podmíněno splněním SQL dotazu
            If _cDL.GetBeforeRunWorkflowSQLResult(intRecordPID, cB06) <> 1 Then
                _Error = cB06.b06ValidateBeforeErrorMessage
                Return False
            End If

        End If



        If _Error <> "" Then
            Return False
        Else
            Return True
        End If
    End Function


    Function RunWorkflowStep(cB06 As BO.b06WorkflowStep, intRecordPID As Integer, x29id As BO.x29IdEnum, strComment As String, strUploadGUID As String, bolManualStep As Boolean, lisNominee As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ib06WorkflowStepBL.RunWorkflowStep
        If Not ValidateWorkflowStepBeforeRun(intRecordPID, x29id, cB06, strComment, strUploadGUID, bolManualStep, lisNominee) Then
            Return False
        End If
        If Not lisNominee Is Nothing Then
            For Each cX69 In lisNominee
                cX69.x67ID = cB06.x67ID_Nominee
                If cX69.j02ID = 0 And cX69.j11ID = 0 Then
                    _Error = "V nominaci chybí specifikace osoby nebo týmu osob." : Return False
                End If
            Next
        End If
        If cB06.x67ID_Direct <> 0 And cB06.j11ID_Direct <> 0 Then
            'automatická změna řešitele
            lisNominee = New List(Of BO.x69EntityRole_Assign)
            Dim cX69 As New BO.x69EntityRole_Assign
            cX69.j11ID = cB06.j11ID_Direct
            cX69.x67ID = cB06.x67ID_Direct
            lisNominee.Add(cX69)
        End If
        If Not lisNominee Is Nothing Then
            Dim lisAll As List(Of BO.x69EntityRole_Assign) = Me.Factory.x67EntityRoleBL.GetList_x69(x29id, intRecordPID).Where(Function(p) p.x67ID <> cB06.x67ID_Nominee).ToList
            For Each cX69 In lisNominee
                lisAll.Add(cX69)
            Next
            lisNominee = lisAll
        End If
        Dim intCurB02ID As Integer = 0
        Select Case x29id
            Case BO.x29IdEnum.p41Project
                Dim cRec As BO.p41Project = Me.Factory.p41ProjectBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID
                If Not lisNominee Is Nothing Then
                    Me.Factory.p41ProjectBL.Save(cRec, Nothing, Nothing, lisNominee, Nothing)
                End If
            Case BO.x29IdEnum.p56Task
                Dim cRec As BO.p56Task = Me.Factory.p56TaskBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID
                If Not lisNominee Is Nothing Then
                    Me.Factory.p56TaskBL.Save(cRec, lisNominee, Nothing, "")
                End If
            Case BO.x29IdEnum.p91Invoice
                Dim cRec As BO.p91Invoice = Me.Factory.p91InvoiceBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID
            Case BO.x29IdEnum.p28Contact
                Dim cRec As BO.p28Contact = Me.Factory.p28ContactBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID
                If Not lisNominee Is Nothing Then
                    Me.Factory.p28ContactBL.Save(cRec, Nothing, Nothing, Nothing, lisNominee, Nothing, Nothing)
                End If
        End Select

        If cB06.b02ID_Target <> 0 Then
            _cDL.SaveStatusMove(intRecordPID, x29id, cB06, cB06.b02ID_Target, bolManualStep, strComment)
        End If
        

        Dim intB07ID As Integer = 0
        Dim c As New BO.b07Comment
        c.b07RecordPID = intRecordPID
        c.x29ID = x29id
        c.b07Value = strComment
        c.b07WorkflowInfo = cB06.NameWithTargetStatus
        If Me.Factory.b07CommentBL.Save(c, strUploadGUID) Then
            intB07ID = Me.Factory.b07CommentBL.LastSavedPID
        End If

        Dim cB05 As New BO.b05Workflow_History
        With cB05
            .b05RecordPID = intRecordPID
            .x29ID = x29id
            .b06ID = cB06.PID
            .b07ID = intB07ID
            If cB06.b02ID_Target <> 0 Then
                .b02ID_From = intCurB02ID
                .b02ID_To = cB06.b02ID_Target
            End If

            .b05IsManualStep = bolManualStep
        End With
        Me.Factory.b05Workflow_HistoryBL.Save(cB05)
        If cB06.b06RunSQL <> "" Then
            _cDL.RunSQL(cB06.b06RunSQL, intRecordPID)
        End If
        _cDL.RunB09Commands(intRecordPID, x29id, cB06.PID)

        Return True
    End Function


    Public Function GetPossibleWorkflowSteps4Person(strRecordPrefix As String, intRecordPID As Integer, intJ02ID As Integer) As List(Of BO.WorkflowStepPossible4User) Implements Ib06WorkflowStepBL.GetPossibleWorkflowSteps4Person
        'vrací okruh možných workflow kroků pro záznam intRecordPID/strRecordPrefix
        Dim ret As New List(Of BO.WorkflowStepPossible4User), strLogin As String = Factory.SysUser.j03Login
        If intJ02ID = 0 Then intJ02ID = Factory.SysUser.j02ID
        If intJ02ID <> Factory.SysUser.j02ID Then
            Dim cUser As BO.j03User = Factory.j03UserBL.LoadByJ02ID(intJ02ID)
            strLogin = cUser.j03Login
        End If
        Dim x29id As BO.x29IdEnum = BO.BAS.GetX29FromPrefix(strRecordPrefix)
        Dim curPerson As BO.j02Person = Factory.j02PersonBL.Load(intJ02ID)
        Dim lisPersonJ11 As IEnumerable(Of BO.j11Team) = Factory.j02PersonBL.GetList_j11(Factory.SysUser.j02ID)

        Dim intCurB02ID As Integer = 0, intP41ID As Integer = 0, intRecOwnerID As Integer = 0, strRecUserInsert As String = ""
        Select Case strRecordPrefix
            Case "p41"
                Dim cRec As BO.p41Project = Factory.p41ProjectBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID : intRecOwnerID = cRec.j02ID_Owner : strRecUserInsert = cRec.UserInsert
            Case "p56"
                Dim cRec As BO.p56Task = Factory.p56TaskBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID : intP41ID = cRec.p41ID : intRecOwnerID = cRec.j02ID_Owner : strRecUserInsert = cRec.UserInsert
            Case "o23"
                Dim cRec As BO.o23Notepad = Factory.o23NotepadBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID : intP41ID = cRec.p41ID : intRecOwnerID = cRec.j02ID_Owner : strRecUserInsert = cRec.UserInsert
            Case "p28"
                Dim cRec As BO.p28Contact = Factory.p28ContactBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID : intRecOwnerID = cRec.j02ID_Owner : strRecUserInsert = cRec.UserInsert
            Case "p91"
                Dim cRec As BO.p91Invoice = Factory.p91InvoiceBL.Load(intRecordPID)
                intCurB02ID = cRec.b02ID : intRecOwnerID = cRec.j02ID_Owner : strRecUserInsert = cRec.UserInsert
        End Select

        Dim lisB06 As IEnumerable(Of BO.b06WorkflowStep) = GetList(0).Where(Function(p As BO.b06WorkflowStep) p.b06IsManualStep = True And p.b02ID = intCurB02ID)

        Dim lisX69 As List(Of BO.x69EntityRole_Assign) = Factory.x67EntityRoleBL.GetList_x69(x29id, intRecordPID).ToList
        If strRecordPrefix = "p56" And intP41ID <> 0 Then
            'ještě doplnit projektové role
            Dim lisX69_p41 As IEnumerable(Of BO.x69EntityRole_Assign) = Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, intP41ID)
            For Each c In lisX69_p41
                lisX69.Add(c)
            Next
        End If
        For Each cB06 As BO.b06WorkflowStep In lisB06
            Dim lisB08 As IEnumerable(Of BO.b08WorkflowReceiverToStep) = Factory.b06WorkflowStepBL.GetList_B08(cB06.PID)
            Dim bolOK As Boolean = False
            If lisB08.Where(Function(p) p.j04ID = Factory.SysUser.j04ID).Count > 0 Then bolOK = True
            If strRecUserInsert = Factory.SysUser.j03Login Then
                If lisB08.Where(Function(p) p.b08IsRecordCreator = True).Count > 0 Then bolOK = True 'zakladatel záznamu
            End If
            If intRecOwnerID = Factory.SysUser.j02ID Then
                If lisB08.Where(Function(p) p.b08IsRecordOwner = True).Count > 0 Then bolOK = True 'vlastník záznamu
            End If
            For Each c In lisX69
                If lisB08.Where(Function(p) p.x67ID = c.x67ID And c.j02ID <> 0 And c.j02ID = curPerson.PID).Count > 0 Then bolOK = True
                If lisB08.Where(Function(p) p.x67ID = c.x67ID And c.j07ID <> 0 And c.j07ID = curPerson.j07ID).Count > 0 Then bolOK = True
                For Each cc In lisPersonJ11
                    If lisB08.Where(Function(p) p.x67ID = c.x67ID And c.j11ID <> 0 And c.j11ID = cc.PID).Count > 0 Then bolOK = True
                Next
            Next
            For Each cc In lisPersonJ11
                If lisB08.Where(Function(p) p.j11ID <> 0 And p.j11ID = cc.PID).Count > 0 Then bolOK = True
            Next

            If bolOK Then
                Dim c As New BO.WorkflowStepPossible4User
                c.b06ID = cB06.PID
                c.b06Name = cB06.b06Name

                Dim strName As String = cB06.b06Name
                If cB06.b02ID_Target <> 0 Then
                    strName += "->" & cB06.TargetStatus
                End If
                c.RadioListText = strName

                ret.Add(c)
            End If
        Next
        Return ret
    End Function
End Class
