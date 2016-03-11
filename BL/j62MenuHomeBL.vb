
Public Interface Ij62MenuHomeBL
    Inherits IFMother
    Function Save(cRec As BO.j62MenuHome, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
    Function Load(intPID As Integer) As BO.j62MenuHome
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.j62MenuHome)
End Interface

Class j62MenuHomeBL
    Inherits BLMother
    Implements Ij62MenuHomeBL
    Private WithEvents _cDL As DL.j62MenuHomeDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.j62MenuHomeDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.j62MenuHome, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ij62MenuHomeBL.Save
        With cRec
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí druh entity." : Return False
            If Trim(.j62Name) = "" Then _Error = "Chybí název položky." : Return False
            If Trim(.j62Url) = "" Then _Error = "Chybí odkaz (URL) menu položky." : Return False

        End With

        Return _cDL.Save(cRec, lisX69)
    End Function
    Public Function Load(intPID As Integer) As BO.j62MenuHome Implements Ij62MenuHomeBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ij62MenuHomeBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.j62MenuHome) Implements Ij62MenuHomeBL.GetList
        Return _cDL.GetList(mq)
    End Function

  
End Class
