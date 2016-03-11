Public Interface Ix18EntityCategoryBL
    Inherits ifMother
    Function Save(cRec As BO.x18EntityCategory) As Boolean
    Function Load(intPID As Integer) As BO.x18EntityCategory
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.x18EntityCategory)

End Interface
Class x18EntityCategoryBL
    Inherits BLMother
    Implements Ix18EntityCategoryBL
    Private WithEvents _cDL As DL.x18EntityCategoryDL

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.x18EntityCategoryDL(ServiceUser)
        _cUser = ServiceUser
    End Sub

    Public Function Load(intPID As Integer) As BO.x18EntityCategory Implements Ix18EntityCategoryBL.Load
        Return _cDL.Load(intPID)
    End Function

    Public Function Save(cRec As BO.x18EntityCategory) As Boolean Implements Ix18EntityCategoryBL.Save
        Return _cDL.Save(cRec)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ix18EntityCategoryBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.x18EntityCategory) Implements Ix18EntityCategoryBL.GetList
        Return _cDL.GetList(myQuery)
    End Function
End Class
