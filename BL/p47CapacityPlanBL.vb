
Public Interface Ip47CapacityPlanBL
    Inherits IFMother
    Function SaveProjectPlan(intP41ID As Integer, lisP47 As List(Of BO.p47CapacityPlan)) As Boolean
    
    Function GetList(mq As BO.myQueryP47) As IEnumerable(Of BO.p47CapacityPlan)

End Interface
Class p47CapacityPlanBL
    Inherits BLMother
    Implements Ip47CapacityPlanBL
    Private WithEvents _cDL As DL.p47CapacityPlanDL

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p47CapacityPlanDL(ServiceUser)
        _cUser = ServiceUser
    End Sub

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Function SaveProjectPlan(intP41ID As Integer, lisP47 As List(Of BO.p47CapacityPlan)) As Boolean Implements Ip47CapacityPlanBL.SaveProjectPlan
        Return _cDL.SaveProjectPlan(intP41ID, lisP47)
    End Function
    Public Function GetList(mq As BO.myQueryP47) As IEnumerable(Of BO.p47CapacityPlan) Implements Ip47CapacityPlanBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
