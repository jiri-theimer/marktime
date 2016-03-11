
Public Interface Ip97Invoice_Round_SettingBL
    Inherits IFMother
    Function Save(cRec As BO.p97Invoice_Round_Setting) As Boolean
    Function Load(intPID As Integer) As BO.p97Invoice_Round_Setting
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p97Invoice_Round_Setting)

End Interface
Class p97Invoice_Round_SettingBL
    Inherits BLMother
    Implements Ip97Invoice_Round_SettingBL
    Private WithEvents _cDL As DL.p97Invoice_Round_SettingDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p97Invoice_Round_SettingDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p97Invoice_Round_Setting) As Boolean Implements Ip97Invoice_Round_SettingBL.Save
        With cRec
            If .j27ID = 0 Then _Error = "Chybí měna." : Return False
        End With
        If GetList().Where(Function(p) p.PID <> cRec.PID And p.j27ID = cRec.j27ID).Count > 0 Then
            _Error = "Pro tuto měnu již existuje zaokrouhlovací pravidlo." : Return False
        End If

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.p97Invoice_Round_Setting Implements Ip97Invoice_Round_SettingBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip97Invoice_Round_SettingBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p97Invoice_Round_Setting) Implements Ip97Invoice_Round_SettingBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
