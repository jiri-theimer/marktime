' NOTE: You can use the "Rename" command on the context menu to change the class name "mtdefault" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select mtdefault.svc or mtdefault.svc.vb at the Solution Explorer and start debugging.
Imports System.ServiceModel

Class mtService
    Implements ImtService
    
    Private _factory As BL.Factory
    Private _mqDef As BO.myQuery
    Public Sub New()
        _mqDef = New BO.myQuery
        _mqDef.Closed = BO.BooleanQueryMode.NoQuery
    End Sub



    Private Function res(bolSucceess As Boolean, Optional intPID As Integer = 0, Optional strErrorMessage As String = "", Optional strSuccessMessage As String = "") As BO.ServiceResult
        Dim c As New BO.ServiceResult
        c.PID = intPID
        c.ErrorMessage = strErrorMessage
        c.SuccessMessage = strSuccessMessage
        c.IsSuccess = bolSucceess
        Return c
    End Function

    Private Function VerifyUser(strLogin As String, strPassword As String) As BO.j03UserSYS
        Dim b As Boolean = Membership.ValidateUser(strLogin, strPassword)
        If Not b Then
            Throw New FaultException("Heslo nebo uživatelské jméno (login) je chybné.")
            'Return res(False, , "Heslo nebo uživatelské jméno (login) je chybné.")
        End If
        _factory = New BL.Factory(, strLogin)
        If _factory.SysUser Is Nothing Then
            Throw New FaultException("Účet uživatele nebyl nalezen v MARKTIME databázi.")
        End If
        With _factory.SysUser
            If .IsClosed Then
                Throw New FaultException("Uzavřený uživatelský účet pro přihlašování.")
            End If
        End With
        Return _factory.SysUser
    End Function
    Public Function Ping(strLogin As String, strPassword As String) As Boolean Implements ImtService.Ping
        VerifyUser(strLogin, strPassword)
        Return True
    End Function

    Public Function SaveTask(intPID As Integer, fields As Dictionary(Of String, Object), receivers As List(Of BO.x69EntityRole_Assign), strLogin As String, strPassword As String) As BO.ServiceResult Implements ImtService.SaveTask
        VerifyUser(strLogin, strPassword)
        Dim sr As New BO.ServiceResult()

        If fields Is Nothing Then
            sr.ErrorMessage = "fields is nothing" : Return sr
        End If
        Dim cRec As New BO.p56Task

        If intPID <> 0 Then cRec = _factory.p56TaskBL.Load(intPID)
        If cRec Is Nothing Then
            sr.ErrorMessage = "record not found" : Return sr
        End If
        For Each c In fields
            Try
                BO.BAS.SetPropertyValue(cRec, c.Key, c.Value)
            Catch ex As Exception
                sr.ErrorMessage = "Property [" & c.Key & "], error: " & ex.Message : Return sr
            End Try

        Next
        If _factory.p56TaskBL.Save(cRec, receivers, Nothing, "") Then
            sr.PID = _factory.p56TaskBL.LastSavedPID
            sr.IsSuccess = True
        Else
            sr.ErrorMessage = _factory.p56TaskBL.ErrorMessage
            sr.IsSuccess = False
        End If

        Return sr
    End Function

    Public Function LoadTaskExtended(intPID As Integer, strLogin As String, strPassword As String) As BO.p56TaskWsExtended Implements ImtService.LoadTaskExtended
        VerifyUser(strLogin, strPassword)
        Dim ret As New BO.p56TaskWsExtended

        Dim c As BO.p56Task = _factory.p56TaskBL.Load(intPID)
        If c Is Nothing Then
            ret.ErrorMessage = "Nelze načíst záznam." : Return ret
        End If

        ret.ConvertFromOrig(c)
        ret.IsSuccess = True
        Return ret

    End Function
    Public Function LoadTask(intPID As Integer, strLogin As String, strPassword As String) As BO.p56Task Implements ImtService.LoadTask
        VerifyUser(strLogin, strPassword)
        Return _factory.p56TaskBL.Load(intPID)

    End Function
    Public Function LoadTaskByExternalPID(strExternalPID As String, strLogin As String, strPassword As String) As BO.p56Task Implements ImtService.LoadTaskByExternalPID
        VerifyUser(strLogin, strPassword)
        Return _factory.p56TaskBL.LoadByExternalPID(strExternalPID)
    End Function
    Public Function ListProjects(intP28ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.p41Project) Implements ImtService.ListProjects
        VerifyUser(strLogin, strPassword)

        Dim mq As New BO.myQueryP41
        mq.Closed = BO.BooleanQueryMode.NoQuery
        mq.p28ID = intP28ID
        Return _factory.p41ProjectBL.GetList(mq)
    End Function
    Public Function ListClients(strLogin As String, strPassword As String) As IEnumerable(Of BO.p28Contact) Implements ImtService.ListClients
        VerifyUser(strLogin, strPassword)

        Dim mq As New BO.myQueryP28
        mq.Closed = BO.BooleanQueryMode.NoQuery
        Return _factory.p28ContactBL.GetList(mq)
    End Function
    Public Function ListProducts(intP28ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.p58Product) Implements ImtService.ListProducts
        VerifyUser(strLogin, strPassword)

        Return _factory.p58ProductBL.GetList(New BO.myQuery, intP28ID)
    End Function


    Public Function ListTaskTypes(strLogin As String, strPassword As String) As IEnumerable(Of BO.p57TaskType) Implements ImtService.ListTaskTypes
       VerifyUser(strLogin, strPassword)
        Return _factory.p57TaskTypeBL.GetList(_mqDef)
    End Function
    Public Function ListPriorities(strLogin As String, strPassword As String) As IEnumerable(Of BO.p59Priority) Implements ImtService.ListPriorities
        VerifyUser(strLogin, strPassword)

        Return _factory.p59PriorityBL.GetList(_mqDef)
    End Function
    Public Function ListSheets(strLogin As String, strPassword As String) As IEnumerable(Of BO.p34ActivityGroup) Implements ImtService.ListSheets
        VerifyUser(strLogin, strPassword)

        Return _factory.p34ActivityGroupBL.GetList(_mqDef)
    End Function
    Public Function ListActivities(strLogin As String, strPassword As String) As IEnumerable(Of BO.p32Activity) Implements ImtService.ListActivities
        VerifyUser(strLogin, strPassword)

        Dim mq As New BO.myQueryP32
        Return _factory.p32ActivityBL.GetList(mq)
    End Function

    Public Function ListPersons(strLogin As String, strPassword As String) As IEnumerable(Of BO.j02Person) Implements ImtService.ListPersons
        VerifyUser(strLogin, strPassword)

        Dim mq As New BO.myQueryJ02
        mq.Closed = BO.BooleanQueryMode.NoQuery
        Return _factory.j02PersonBL.GetList(mq)
    End Function
    Public Function ListContactPersons(strLogin As String, strPassword As String, intP28ID As Integer) As IEnumerable(Of BO.j02Person) Implements ImtService.ListContactPersons
        VerifyUser(strLogin, strPassword)
        Dim mq As New BO.myQueryJ02
        mq.p28ID = intP28ID
        Return _factory.j02PersonBL.GetList(mq)
    End Function
    Public Function LoadPerson(intPID As Integer, strLogin As String, strPassword As String) As BO.j02Person Implements ImtService.LoadPerson
        VerifyUser(strLogin, strPassword)
        Return _factory.j02PersonBL.Load(intPID)
    End Function
    Public Function LoadPersonByExternalPID(strExternalPID As String, strLogin As String, strPassword As String) As BO.j02Person Implements ImtService.LoadPersonByExternalPID
        VerifyUser(strLogin, strPassword)
        Return _factory.j02PersonBL.LoadByExternalPID(strExternalPID)
    End Function
    Public Function ListPersonTeams(strLogin As String, strPassword As String) As IEnumerable(Of BO.j11Team) Implements ImtService.ListPersonTeams
        VerifyUser(strLogin, strPassword)
        Return _factory.j11TeamBL.GetList(_mqDef)
    End Function
    Public Function ListRoles(strLogin As String, strPassword As String) As IEnumerable(Of BO.x67EntityRole) Implements ImtService.ListRoles
        VerifyUser(strLogin, strPassword)
        Return _factory.x67EntityRoleBL.GetList(_mqDef)
    End Function
    Public Function LoadProject(intPID As Integer, strLogin As String, strPassword As String) As BO.p41Project Implements ImtService.LoadProject
        VerifyUser(strLogin, strPassword)
        Return _factory.p41ProjectBL.Load(intPID)
    End Function
    Public Function LoadProjectByExternalPID(strExternalPID As String, strLogin As String, strPassword As String) As BO.p41Project Implements ImtService.LoadProjectByExternalPID
        VerifyUser(strLogin, strPassword)
        Return _factory.p41ProjectBL.LoadByExternalPID(strExternalPID)
    End Function
    Public Function LoadClient(intPID As Integer, strLogin As String, strPassword As String) As BO.p28Contact Implements ImtService.LoadClient
        VerifyUser(strLogin, strPassword)
        Return _factory.p28ContactBL.Load(intPID)
    End Function
    Public Function LoadClientByExternalPID(strExternalPID As String, strLogin As String, strPassword As String) As BO.p28Contact Implements ImtService.LoadClientByExternalPID
        VerifyUser(strLogin, strPassword)
        Return _factory.p28ContactBL.LoadByExternalPID(strExternalPID)
    End Function
    Public Function SaveWorksheet(intPID As Integer, fields As Dictionary(Of String, Object), strLogin As String, strPassword As String) As BO.ServiceResult Implements ImtService.SaveWorksheet
        VerifyUser(strLogin, strPassword)
        Dim sr As New BO.ServiceResult()
        If fields Is Nothing Then
            sr.ErrorMessage = "fields is nothing" : Return sr
        End If
        Dim cRec As New BO.p31WorksheetEntryInput
        cRec.SetPID(intPID)

        For Each c In fields
            Try
                BO.BAS.SetPropertyValue(cRec, c.Key, c.Value)
            Catch ex As Exception
                sr.ErrorMessage = "Property [" & c.Key & "], error: " & ex.Message : Return sr
            End Try
        Next
        With cRec
            If .p34ID = 0 And .p32ID <> 0 Then  'dohledat sešit, pokud na vstupu chybí
                Dim cP32 As BO.p32Activity = _factory.p32ActivityBL.Load(.p32ID)
                .p34ID = cP32.p34ID
            End If
            If .p31HoursEntryflag = BO.p31HoursEntryFlagENUM.NeniCas Then
                'ověřit, zda se jedná o hodiny
                If .p34ID <> 0 Then
                    Dim cP34 As BO.p34ActivityGroup = _factory.p34ActivityGroupBL.Load(.p34ID)
                    If cP34.p33ID = BO.p33IdENUM.Cas Then .p31HoursEntryflag = BO.p31HoursEntryFlagENUM.Hodiny
                End If
                If .TimeFrom <> "" Or .TimeUntil <> "" Then .p31HoursEntryflag = BO.p31HoursEntryFlagENUM.PresnyCasOdDo
            End If
        End With
        
        If _factory.p31WorksheetBL.SaveOrigRecord(cRec, Nothing) Then
            sr.PID = _factory.p31WorksheetBL.LastSavedPID
            sr.IsSuccess = True
        Else
            sr.ErrorMessage = _factory.p31WorksheetBL.ErrorMessage
            sr.IsSuccess = False
        End If

        Return sr
    End Function

    Public Function SaveProject(intPID As Integer, fields As Dictionary(Of String, Object), receivers As List(Of BO.x69EntityRole_Assign), strLogin As String, strPassword As String) As BO.ServiceResult Implements ImtService.SaveProject
        VerifyUser(strLogin, strPassword)
        Dim sr As New BO.ServiceResult()
        If fields Is Nothing Then
            sr.ErrorMessage = "fields is nothing" : Return sr
        End If
        Dim cRec As New BO.p41Project

        If intPID <> 0 Then cRec = _factory.p41ProjectBL.Load(intPID)
        If cRec Is Nothing Then
            sr.ErrorMessage = "record not found" : Return sr
        End If
        For Each c In fields
            Try
                BO.BAS.SetPropertyValue(cRec, c.Key, c.Value)
            Catch ex As Exception
                sr.ErrorMessage = "Property [" & c.Key & "], error: " & ex.Message : Return sr
            End Try
        Next
        If _factory.p41ProjectBL.Save(cRec, Nothing, Nothing, receivers, Nothing) Then
            sr.PID = _factory.p41ProjectBL.LastSavedPID
            sr.IsSuccess = True
        Else
            sr.ErrorMessage = _factory.p41ProjectBL.ErrorMessage
            sr.IsSuccess = False
        End If

        Return sr
    End Function
    Public Function SaveClient(intPID As Integer, fields As Dictionary(Of String, Object), addresses As List(Of BO.o37Contact_Address), p58ids As List(Of Integer), strLogin As String, strPassword As String) As BO.ServiceResult Implements ImtService.SaveClient
        VerifyUser(strLogin, strPassword)
        Dim sr As New BO.ServiceResult()
        If fields Is Nothing Then
            sr.ErrorMessage = "fields is nothing" : Return sr
        End If
        Dim cRec As New BO.p28Contact

        If intPID <> 0 Then cRec = _factory.p28ContactBL.Load(intPID)
        If cRec Is Nothing Then
            sr.ErrorMessage = "record not found" : Return sr
        End If
        For Each c In fields
            Try
                BO.BAS.SetPropertyValue(cRec, c.Key, c.Value)
            Catch ex As Exception
                sr.ErrorMessage = "Property [" & c.Key & "], error: " & ex.Message : Return sr
            End Try
        Next
        Try
            If _factory.p28ContactBL.Save(cRec, addresses, Nothing, Nothing, Nothing, Nothing, p58ids) Then
                sr.PID = _factory.p28ContactBL.LastSavedPID
                sr.IsSuccess = True
            Else
                sr.ErrorMessage = _factory.p28ContactBL.ErrorMessage
                sr.IsSuccess = False
            End If
        Catch ex As Exception
            Throw New FaultException(ex.Message)
        
        End Try
       

        Return sr
    End Function
    Public Function ListWorkflowStatuses(intB01ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.b02WorkflowStatus) Implements ImtService.ListWorkflowStatuses
        VerifyUser(strLogin, strPassword)
        Return _factory.b02WorkflowStatusBL.GetList(intB01ID)
    End Function
    Public Function ListWorkflowSteps(intB01ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.b06WorkflowStep) Implements ImtService.ListWorkflowSteps
        VerifyUser(strLogin, strPassword)
        Return _factory.b06WorkflowStepBL.GetList(intB01ID)
    End Function
    Public Function ListPossibleWorkflowSteps(intRecordPID As Integer, strRecordPrefix As String, intJ02ID As Integer, strLogin As String, strPassword As String) As List(Of BO.WorkflowStepPossible4User) Implements ImtService.ListPossibleWorkflowSteps
        VerifyUser(strLogin, strPassword)
        Return _factory.b06WorkflowStepBL.GetPossibleWorkflowSteps4Person(strRecordPrefix, intRecordPID, intJ02ID)
    End Function

    Public Function SavePerson(intPID As Integer, fields As Dictionary(Of String, Object), strLogin As String, strPassword As String) As BO.ServiceResult Implements ImtService.SavePerson
        VerifyUser(strLogin, strPassword)
        Dim sr As New BO.ServiceResult()
        If fields Is Nothing Then
            sr.ErrorMessage = "fields is nothing" : Return sr
        End If
        Dim cRec As New BO.j02Person

        If intPID <> 0 Then cRec = _factory.j02PersonBL.Load(intPID)
        If cRec Is Nothing Then
            sr.ErrorMessage = "record not found" : Return sr
        End If
        For Each c In fields
            Try
                BO.BAS.SetPropertyValue(cRec, c.Key, c.Value)
            Catch ex As Exception
                sr.ErrorMessage = "Property [" & c.Key & "], error: " & ex.Message : Return sr
            End Try
        Next
        Try
            If _factory.j02PersonBL.Save(cRec, Nothing) Then
                sr.PID = _factory.j02PersonBL.LastSavedPID
                sr.IsSuccess = True
            Else
                sr.ErrorMessage = _factory.j02PersonBL.ErrorMessage
                sr.IsSuccess = False
            End If
        Catch ex As Exception
            Throw New FaultException(ex.Message)

        End Try


        Return sr
    End Function
End Class
