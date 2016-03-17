
Public Interface Ip45BudgetBL
    Inherits IFMother
    Function Save(cRec As BO.p45Budget, lisP46 As List(Of BO.p46BudgetPerson)) As Boolean
    Function Load(intPID As Integer) As BO.p45Budget
    Function Delete(intPID As Integer) As Boolean
    Function GetList(intP41ID As Integer, Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p45Budget)
    Function GetList_p46(intPID As Integer) As IEnumerable(Of BO.p46BudgetPerson)
End Interface
Class p45BudgetBL
    Inherits BLMother
    Implements Ip45BudgetBL
    Private WithEvents _cDL As DL.p45BudgetDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p45BudgetDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p45Budget, lisP46 As List(Of BO.p46BudgetPerson)) As Boolean Implements Ip45BudgetBL.Save
        With cRec
            If BO.BAS.IsNullDBDate(.p45PlanFrom) Is Nothing Then _Error = "Chybí začátek rozpočtu." : Return False
            If BO.BAS.IsNullDBDate(.p45PlanUntil) Is Nothing Then _Error = "Chybí konec rozpočtu." : Return False
            If .p45PlanFrom > .p45PlanUntil Then _Error = "Plánované dokončení musí být větší než než plánované zahájení." : Return False
            If .p41ID = 0 Then _Error = "Chybí vazba na projekt." : Return False
            If .PID = 0 Then
                .p45VersionIndex = GetList(.p41ID).Count + 1
                If lisP46 Is Nothing Then lisP46 = New List(Of BO.p46BudgetPerson)
                If lisP46.Count = 0 Then _Error = "V rozpočtu musí být minimálně jedna osoba." : Return False
            End If
        End With

        Return _cDL.Save(cRec, lisP46)
    End Function
    Public Function Load(intPID As Integer) As BO.p45Budget Implements Ip45BudgetBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip45BudgetBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(intP41ID As Integer, Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p45Budget) Implements Ip45BudgetBL.GetList
        Return _cDL.GetList(intP41ID, mq)
    End Function
    Public Function GetList_p46(intPID As Integer) As IEnumerable(Of BO.p46BudgetPerson) Implements Ip45BudgetBL.GetList_p46
        Return _cDL.GetList_p46(intPID)
    End Function
End Class
