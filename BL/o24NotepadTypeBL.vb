Public Interface Io24NotepadTypeBL
    Inherits IFMother
    Function Save(cRec As BO.o24NotepadType) As Boolean
    Function Load(intPID As Integer) As BO.o24NotepadType
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.o24NotepadType)

End Interface

Class o24NotepadTypeBL
    Inherits BLMother
    Implements Io24NotepadTypeBL
    Private WithEvents _cDL As DL.o24NotepadTypeDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.o24NotepadTypeDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.o24NotepadType) As Boolean Implements Io24NotepadTypeBL.Save
        With cRec
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí druh entity." : Return False
            If Trim(.o24Name) = "" Then _Error = "Chybí název." : Return False
            If .x38ID = 0 Then _Error = "Chybí vazba na číselnou řadu dokumentů." : Return False
            Select Case .x29ID
                Case BO.x29IdEnum.p28Contact, BO.x29IdEnum.p41Project, BO.x29IdEnum.j02Person
                Case Else
                    .o24IsBillingMemo = False
            End Select
        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.o24NotepadType Implements Io24NotepadTypeBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Io24NotepadTypeBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.o24NotepadType) Implements Io24NotepadTypeBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
