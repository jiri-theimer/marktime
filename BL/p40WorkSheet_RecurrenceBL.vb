
Public Interface Ip40WorkSheet_RecurrenceBL
    Inherits IFMother
    Function Save(cRec As BO.p40WorkSheet_Recurrence) As Boolean
    Function Load(intPID As Integer) As BO.p40WorkSheet_Recurrence
    Function Delete(intPID As Integer) As Boolean
    Function GetList(intP41ID As Integer) As IEnumerable(Of BO.p40WorkSheet_Recurrence)
    Function GetList_WaitingForGenerate(datNow As Date) As IEnumerable(Of BO.p40WorkSheet_Recurrence)
    Function LoadP39_FirstWaiting(intP40ID As Integer, datNow As DateTime) As BO.p39WorkSheet_Recurrence_Plan
    Function GetList_p39(intPID As Integer) As IEnumerable(Of BO.p39WorkSheet_Recurrence_Plan)
    Function Update_p31Instance(intP39ID As Integer, intP31ID As Integer, strErrorMessage As String) As Boolean
    Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.p39WorkSheet_Recurrence_Plan)

End Interface
Class p40WorkSheet_RecurrenceBL
    Inherits BLMother
    Implements Ip40WorkSheet_RecurrenceBL
    Private WithEvents _cDL As DL.p40WorkSheet_RecurrenceDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p40WorkSheet_RecurrenceDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p40WorkSheet_Recurrence) As Boolean Implements Ip40WorkSheet_RecurrenceBL.Save
        With cRec
            If .p41ID = 0 Then _Error = "Chybí vazba na projektu." : Return False
            If .p34ID = 0 Then _Error = "Chybí vazba na sešit." : Return False
            If .p32ID = 0 Then _Error = "Chybí vazba na aktivitu." : Return False
            If .j27ID = 0 Then _Error = "Chybí vazba na měnu." : Return False
            If .j02ID = 0 Then _Error = "Chybí vazba na osobu." : Return False
            If .p40Value = 0 Then _Error = "Chybí hodnota úkonu." : Return False
            If Trim(.p40Text) = "" Then _Error = "Chybí popis úkonu" : Return False

            If Trim(.p40Name) = "" Then
                _Error = "Chybí název (předmět)." : Return False
            End If
            If BO.BAS.IsNullDBDate(.p40FirstSupplyDate) Is Nothing Then
                _Error = "Chybí specifikace prvního rozhodného datumu." : Return False
            End If
            If BO.BAS.IsNullDBDate(.p40LastSupplyDate) Is Nothing Then
                _Error = "Chybí specifikace posledního rozhodného datumu." : Return False
            End If
        End With

        If _cDL.Save(cRec) Then
            If cRec.PID = 0 Then
                ''Me.RaiseAppEvent(BO.x45IDEnum.o22_new, _LastSavedPID)
            Else
                ''Me.RaiseAppEvent(BO.x45IDEnum.o22_update, _LastSavedPID)
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Load(intPID As Integer) As BO.p40WorkSheet_Recurrence Implements Ip40WorkSheet_RecurrenceBL.Load
        Return _cDL.Load(intPID)
    End Function
    
    Public Function Delete(intPID As Integer) As Boolean Implements Ip40WorkSheet_RecurrenceBL.Delete

        If _cDL.Delete(intPID) Then
            ''Me.RaiseAppEvent(BO.x45IDEnum.o22_delete, intPID, s)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(intP41ID As Integer) As IEnumerable(Of BO.p40WorkSheet_Recurrence) Implements Ip40WorkSheet_RecurrenceBL.GetList
        Return _cDL.GetList(intP41ID)
    End Function
    Public Function GetList_WaitingForGenerate(datNow As Date) As IEnumerable(Of BO.p40WorkSheet_Recurrence) Implements Ip40WorkSheet_RecurrenceBL.GetList_WaitingForGenerate
        Return _cDL.GetList_WaitingForGenerate(datNow)
    End Function
    Public Function LoadP39_FirstWaiting(intP40ID As Integer, datNow As DateTime) As BO.p39WorkSheet_Recurrence_Plan Implements Ip40WorkSheet_RecurrenceBL.LoadP39_FirstWaiting
        Return _cDL.LoadP39_FirstWaiting(intP40ID, datNow)
    End Function

    Public Function GetList_p39(intPID As Integer) As IEnumerable(Of BO.p39WorkSheet_Recurrence_Plan) Implements Ip40WorkSheet_RecurrenceBL.GetList_p39
        Return _cDL.GetList_p39(intPID)
    End Function

    Public Function Update_p31Instance(intP39ID As Integer, intP31ID As Integer, strErrorMessage As String) As Boolean Implements Ip40WorkSheet_RecurrenceBL.Update_p31Instance
        Return _cDL.Update_p31Instance(intP39ID, intP31ID, strErrorMessage)
    End Function

    Public Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.p39WorkSheet_Recurrence_Plan) Implements Ip40WorkSheet_RecurrenceBL.GetList_forMessagesDashboard
        Return _cDL.GetList_forMessagesDashboard(intJ02ID)
    End Function
End Class
