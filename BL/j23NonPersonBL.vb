
Public Interface Ij23NonPersonBL
    Inherits IFMother
    Function Save(cRec As BO.j23NonPerson) As Boolean
    Function Load(intPID As Integer) As BO.j23NonPerson
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.j23NonPerson)

End Interface
Class j23NonPersonBL
    Inherits BLMother
    Implements Ij23NonPersonBL
    Private WithEvents _cDL As DL.j23NonPersonDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.j23NonPersonDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.j23NonPerson) As Boolean Implements Ij23NonPersonBL.Save
        With cRec
            If Trim(.j23Name) = "" Then _Error = "Chybí název zdroje." : Return False
            If .j24ID = 0 Then _Error = "Chybí typ zdroje." : Return False
        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.j23NonPerson Implements Ij23NonPersonBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ij23NonPersonBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.j23NonPerson) Implements Ij23NonPersonBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
