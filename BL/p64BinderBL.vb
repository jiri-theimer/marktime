
Public Interface Ip64BinderBL
    Inherits IFMother
    Function Save(cRec As BO.p64Binder) As Boolean
    Function Load(intPID As Integer) As BO.p64Binder
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.p64Binder)

End Interface
Public Class p64BinderBL
    Inherits BLMother
    Implements Ip64BinderBL
    Private WithEvents _cDL As DL.p64BinderDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p64BinderDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p64Binder) As Boolean Implements Ip64BinderBL.Save
        With cRec
            If Trim(.p64Name) = "" Then _Error = "Chybí název šanonu." : Return False
            If .p41ID = 0 Then _Error = "Chybí vazba na projekt." : Return False
            If .j02ID_Owner = 0 Then .j02ID_Owner = Factory.SysUser.j02ID
            
        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.p64Binder Implements Ip64BinderBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip64BinderBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.p64Binder) Implements Ip64BinderBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
