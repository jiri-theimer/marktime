
Public Interface Io10NoticeBoardBL
    Inherits IFMother
    Function Save(cRec As BO.o10NoticeBoard, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
    Function Load(intPID As Integer) As BO.o10NoticeBoard
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery) As IEnumerable(Of BO.o10NoticeBoard)

End Interface
Public Class o10NoticeBoardBL
    Inherits BLMother
    Implements Io10NoticeBoardBL
    Private WithEvents _cDL As DL.o10NoticeBoardDL

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.o10NoticeBoardDL(ServiceUser)
        _cUser = ServiceUser
    End Sub

    Public Function Delete(intPID As Integer) As Boolean Implements Io10NoticeBoardBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(myQuery As BO.myQuery) As System.Collections.Generic.IEnumerable(Of BO.o10NoticeBoard) Implements Io10NoticeBoardBL.GetList
        Return _cDL.GetList(myQuery)
    End Function


    Public Function Load(intPID As Integer) As BO.o10NoticeBoard Implements Io10NoticeBoardBL.Load
        Return _cDL.Load(intPID)
    End Function

    Public Function Save(cRec As BO.o10NoticeBoard, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Io10NoticeBoardBL.Save
        With cRec
            If .j02ID_Owner = 0 Then .j02ID_Owner = _cUser.j02ID
            If Trim(.o10Name) = "" Then _Error = "Chybí název článku."
           
        End With

        If _Error <> "" Then Return False

        Return _cDL.Save(cRec, lisX69)

    End Function
End Class
