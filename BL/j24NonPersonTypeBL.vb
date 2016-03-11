
Public Interface Ij24NonPersonTypeBL
    Inherits IFMother
    Function Save(cRec As BO.j24NonPersonType) As Boolean
    Function Load(intPID As Integer) As BO.j24NonPersonType
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.j24NonPersonType)

End Interface
Class j24NonPersonTypeBL
    Inherits BLMother
    Implements Ij24NonPersonTypeBL
    Private WithEvents _cDL As DL.j24NonPersonTypeDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.j24NonPersonTypeDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.j24NonPersonType) As Boolean Implements Ij24NonPersonTypeBL.Save
        With cRec
            If Trim(.j24Name) = "" Then _Error = "Chybí název typu zdroje." : Return False

        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.j24NonPersonType Implements Ij24NonPersonTypeBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ij24NonPersonTypeBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.j24NonPersonType) Implements Ij24NonPersonTypeBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
