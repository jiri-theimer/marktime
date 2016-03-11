Public Interface Im62ExchangeRateBL
    Inherits IFMother
    Function Save(cRec As BO.m62ExchangeRate) As Boolean
    Function Load(intPID As Integer) As BO.m62ExchangeRate
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.m62ExchangeRate)

End Interface

Class m62ExchangeRateBL
    Inherits BLMother
    Implements Im62ExchangeRateBL
    Private WithEvents _cDL As DL.m62ExchangeRateDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.m62ExchangeRateDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.m62ExchangeRate) As Boolean Implements Im62ExchangeRateBL.Save
        With cRec
            If .j27ID_Master = 0 Then _Error = "Chybí zdrojová měna." : Return False
            If .j27ID_Slave = 0 Then _Error = "Chybí cílová měna." : Return False
            If .m62Rate <= 0 Then _Error = "Hodnota kurzu musí být větší než nula." : Return False

        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.m62ExchangeRate Implements Im62ExchangeRateBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Im62ExchangeRateBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.m62ExchangeRate) Implements Im62ExchangeRateBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
