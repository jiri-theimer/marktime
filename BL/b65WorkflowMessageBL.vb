Public Interface Ib65WorkflowMessageBL
    Inherits IFMother
    Function Save(cRec As BO.b65WorkflowMessage) As Boolean
    Function Load(intPID As Integer) As BO.b65WorkflowMessage
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.b65WorkflowMessage)

End Interface
Class b65WorkflowMessageBL
    Inherits BLMother
    Implements Ib65WorkflowMessageBL
    Private WithEvents _cDL As DL.b65WorkflowMessageDL

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.b65WorkflowMessageDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Delete(intPID As Integer) As Boolean Implements Ib65WorkflowMessageBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As System.Collections.Generic.IEnumerable(Of BO.b65WorkflowMessage) Implements Ib65WorkflowMessageBL.GetList
        Return _cDL.GetList(myQuery)
    End Function

    Public Function Load(intPID As Integer) As BO.b65WorkflowMessage Implements Ib65WorkflowMessageBL.Load
        Return _cDL.Load(intPID)
    End Function


    Public Function Save(cRec As BO.b65WorkflowMessage) As Boolean Implements Ib65WorkflowMessageBL.Save
        If Trim(cRec.b65Name) = "" Or Trim(cRec.b65MessageSubject) = "" Then
            _Error = "[Název šablony] a [Předmět zprávy] jsou povinná pole k vyplnění."
        End If
        'If cRec.x29ID = 0 Then
        '    _Error = "Entita je povinné pole."
        'End If
        If _Error <> "" Then Return False
        Return _cDL.Save(cRec)
    End Function
End Class
