
Public Interface Ip45BudgetBL
    Inherits IFMother
    Function Save(cRec As BO.p45Budget, lisP46 As List(Of BO.p46BudgetPerson), lisP49 As List(Of BO.p49FinancialPlan)) As Boolean
    Function Load(intPID As Integer) As BO.p45Budget
    Function LoadByProject(intP41ID As Integer) As BO.p45Budget
    Function LoadP46(intP46ID As Integer) As BO.p46BudgetPerson
    Function Delete(intPID As Integer) As Boolean
    Function GetList(intP41ID As Integer, Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p45Budget)
    Function GetList_p46(intPID As Integer) As IEnumerable(Of BO.p46BudgetPerson)
    Function GetList_p46_extended(intPID As Integer, intP41ID As Integer) As IEnumerable(Of BO.p46BudgetPersonExtented)
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
    Public Function Save(cRec As BO.p45Budget, lisP46 As List(Of BO.p46BudgetPerson), lisP49 As List(Of BO.p49FinancialPlan)) As Boolean Implements Ip45BudgetBL.Save
        With cRec
            If BO.BAS.IsNullDBDate(.p45PlanFrom) Is Nothing Then _Error = "Chybí datum plánovaného zahájení." : Return False
            If BO.BAS.IsNullDBDate(.p45PlanUntil) Is Nothing Then _Error = "Chybí datum plánovaného dokončení." : Return False
            If .p45PlanFrom > .p45PlanUntil Then _Error = "Plánované dokončení musí být větší než než plánované zahájení." : Return False
            If .p41ID = 0 Then _Error = "Chybí vazba na projekt." : Return False
            If .PID = 0 Then
                .p45VersionIndex = GetList(.p41ID).Count + 1
            End If
            If Not lisP46 Is Nothing And Not lisP49 Is Nothing Then
                If lisP46.Count = 0 And lisP49.Count = 0 Then _Error = "V rozpočtu musíte definovat buď rozpočet hodin nebo peněžní rozpočet nebo obojí." : Return False
            End If
            If Not lisP46 Is Nothing Then
                If cRec.PID <> 0 Then
                    Dim mqP47 As New BO.myQueryP47
                    mqP47.p45ID = cRec.PID
                    Dim lisP47 As IEnumerable(Of BO.p47CapacityPlan) = Factory.p47CapacityPlanBL.GetList(mqP47)
                    For Each c In lisP46.Where(Function(p) p.PID <> 0)
                        Dim lis As IEnumerable(Of BO.p47CapacityPlan) = lisP47.Where(Function(p) p.j02ID = c.j02ID)
                        If lis.Count > 0 Then
                            If lis.Sum(Function(p) p.p47HoursTotal) > c.p46HoursTotal Then
                                _Error = String.Format("[{0}]: Již existuje kapacitní plán({1}h.), který by překročil hodiny v rozpočtu ({2}h.).", Factory.j02PersonBL.Load(c.j02ID).FullNameAsc, lis.Sum(Function(p) p.p47HoursTotal), c.p46HoursTotal) : Return False
                            End If
                        End If
                    Next
                End If
                For Each c In lisP46.Where(Function(p) p.IsSetAsDeleted = False)
                    If c.p46HoursBillable = 0 And c.p46HoursNonBillable = 0 Then
                        _Error = String.Format("Rozpočet hodin [{0}]: Chybí hodiny nebo vyhoďte osobu z rozpočtu hodin.", Factory.j02PersonBL.Load(c.j02ID).FullNameAsc) : Return False
                    End If
                Next
            End If
            If Not lisP49 Is Nothing Then
                Dim x As Integer = 1
                For Each c In lisP49.Where(Function(p) p.IsSetAsDeleted = False)
                    If c.p49Amount = 0 Then
                        _Error = String.Format("Finanční rozpočet/řádek #{0}: částka je nulová.", x) : Return False
                    End If
                    If c.p49DateFrom < cRec.p45PlanFrom Or c.p49DateFrom > cRec.p45PlanUntil Then
                        _Error = String.Format("Finanční rozpočet: Měsíc [{0}] je mimo časové období rozpočtu.", c.Period) : Return False
                    End If
                    If c.p49DateUntil < cRec.p45PlanFrom Or c.p49DateUntil > cRec.p45PlanUntil Then
                        _Error = String.Format("Finanční rozpočet: Měsíc [{0}] je mimo časové období rozpočtu.", c.Period) : Return False
                    End If
                    x += 1
                Next
                For Each c In lisP49.Where(Function(p) p.IsSetAsDeleted = True And p.PID <> 0)
                    If Factory.p49FinancialPlanBL.LoadExtended(c.PID).p31ID > 0 Then
                        _Error = String.Format("[{0}: Nelze odstranit položku rozpočtu, která již má vazbu na reálný worksheet úkon.", Factory.GetRecordCaption(BO.x29IdEnum.p49FinancialPlan, c.PID)) : Return False
                    End If
                Next
            End If
        End With

        Return _cDL.Save(cRec, lisP46, lisP49)
    End Function
    Public Function Load(intPID As Integer) As BO.p45Budget Implements Ip45BudgetBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadByProject(intP41ID As Integer) As BO.p45Budget Implements Ip45BudgetBL.LoadByProject
        Return _cDL.LoadByProject(intP41ID)
    End Function
    Public Function LoadP46(intP46ID As Integer) As BO.p46BudgetPerson Implements Ip45BudgetBL.LoadP46
        Return _cDL.LoadP46(intP46ID)
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
    Public Function GetList_p46_extended(intPID As Integer, intP41ID As Integer) As IEnumerable(Of BO.p46BudgetPersonExtented) Implements Ip45BudgetBL.GetList_p46_extended
        Return _cDL.GetList_p46_extended(intPID, intP41ID)
    End Function
End Class
