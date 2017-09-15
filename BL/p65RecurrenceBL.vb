Public Interface Ip65RecurrenceBL
    Inherits IFMother
    Function Save(cRec As BO.p65Recurrence) As Boolean
    Function Load(intPID As Integer) As BO.p65Recurrence
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p65Recurrence)

End Interface
Class p65RecurrenceBL
    Inherits BLMother
    Implements Ip65RecurrenceBL
    Private WithEvents _cDL As DL.p65RecurrenceDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p65RecurrenceDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p65Recurrence) As Boolean Implements Ip65RecurrenceBL.Save
        With cRec
            If Trim(.p65Name) = "" Then _Error = "Chybí název pravidla." : Return False

        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.p65Recurrence Implements Ip65RecurrenceBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip65RecurrenceBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p65Recurrence) Implements Ip65RecurrenceBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
