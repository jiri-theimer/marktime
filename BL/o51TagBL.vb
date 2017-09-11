
Public Interface Io51TagBL
    Inherits IFMother
    Function Save(cRec As BO.o51Tag) As Boolean
    Function Load(intPID As Integer) As BO.o51Tag
    Function LoadByName(strName As String) As BO.o51Tag
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQuery, strPrefix As String) As IEnumerable(Of BO.o51Tag)
    Function GetList_o52(strPrefix As String, intRecordPID As Integer) As IEnumerable(Of BO.o52TagBinding)
End Interface
Class o51TagBL
    Inherits BLMother
    Implements Io51TagBL
    Private WithEvents _cDL As DL.o51TagDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.o51TagDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.o51Tag) As Boolean Implements Io51TagBL.Save
        With cRec
            If Trim(.o51Name) = "" Then
                _Error = "Chybí název štítku." : Return False
            End If
        End With
        If cRec.PID = 0 Then
            If cRec.j02ID_Owner = 0 Then cRec.j02ID_Owner = _cUser.j02ID
            If Not _cDL.LoadByName(cRec.o51Name) Is Nothing Then
                _Error = String.Format("Štítek s názvem [%1%] již existuje!", cRec.o51Name) : Return False
            End If
        End If
        
        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.o51Tag Implements Io51TagBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadByName(strName As String) As BO.o51Tag Implements Io51TagBL.LoadByName
        Return _cDL.LoadByName(strName)
    End Function
    
    Public Function Delete(intPID As Integer) As Boolean Implements Io51TagBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQuery, strPrefix As String) As IEnumerable(Of BO.o51Tag) Implements Io51TagBL.GetList
        Return _cDL.GetList(mq, strPrefix)
    End Function
    Public Function GetList_o52(strPrefix As String, intRecordPID As Integer) As IEnumerable(Of BO.o52TagBinding) Implements Io51TagBL.GetList_o52
        Return _cDL.GetList_o52(strPrefix, intRecordPID)
    End Function
End Class
