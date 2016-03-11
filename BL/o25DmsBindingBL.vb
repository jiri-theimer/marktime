
Public Interface Io25DmsBindingBL
    Inherits IFMother
    Function Save(cRec As BO.o25DmsBinding) As Boolean
    Function Load(intPID As Integer) As BO.o25DmsBinding
    Function Delete(intPID As Integer) As Boolean
    Function GetList(appDMS As BO.o25DmsAppENUM, Optional x29id As BO.x29IdEnum = BO.x29IdEnum._NotSpecified, Optional intRecordPID As Integer = 0) As IEnumerable(Of BO.o25DmsBinding)

End Interface
Class o25DmsBindingBL
    Inherits BLMother
    Implements Io25DmsBindingBL
    Private WithEvents _cDL As DL.o25DmsBindingDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.o25DmsBindingDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.o25DmsBinding) As Boolean Implements Io25DmsBindingBL.Save
        
        With cRec
            If Trim(.o25Path) = "" Then _Error = "Chybí [Cesta]." : Return False

        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.o25DmsBinding Implements Io25DmsBindingBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Io25DmsBindingBL.Delete
        Dim s As String = Me.Factory.GetRecordCaption(BO.x29IdEnum.o23Notepad, intPID)
        If _cDL.Delete(intPID) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(appDMS As BO.o25DmsAppENUM, Optional x29id As BO.x29IdEnum = BO.x29IdEnum._NotSpecified, Optional intRecordPID As Integer = 0) As IEnumerable(Of BO.o25DmsBinding) Implements Io25DmsBindingBL.GetList
        Return _cDL.GetList(appDMS, x29id, intRecordPID)
    End Function
End Class
