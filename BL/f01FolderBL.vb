
Public Interface If01FolderBL
    Inherits IFMother
    Function CreateUpdateFolder(cRec As BO.f01Folder) As Boolean
    Function Load(intPID As Integer) As BO.f01Folder
    Function Load_f02(intF02ID As Integer) As BO.f02FolderType
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQuery, Optional intRecordPID As Integer = 0, Optional x29ID As BO.x29IdEnum = BO.x29IdEnum._NotSpecified) As IEnumerable(Of BO.f01Folder)
    Function GetList_f02(mq As BO.myQuery) As IEnumerable(Of BO.f02FolderType)
End Interface
Class f01FolderBL
    Inherits BLMother
    Implements If01FolderBL
    Private WithEvents _cDL As DL.f01FolderDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.f01FolderDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function CreateUpdateFolder(cRec As BO.f01Folder) As Boolean Implements If01FolderBL.CreateUpdateFolder
        With cRec
            If Trim(.f01Name) = "" Then
                _Error = "Chybí název složky." : Return False
            End If
        End With



        Return _cDL.Save(cRec)


    End Function
    Public Function Load(intPID As Integer) As BO.f01Folder Implements If01FolderBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Load_f02(intF02ID As Integer) As BO.f02FolderType Implements If01FolderBL.Load_f02
        Return _cDL.Load_f02(intF02ID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements If01FolderBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQuery, Optional intRecordPID As Integer = 0, Optional x29ID As BO.x29IdEnum = BO.x29IdEnum._NotSpecified) As IEnumerable(Of BO.f01Folder) Implements If01FolderBL.GetList
        Return _cDL.GetList(mq, intRecordPID, x29ID)
    End Function
    Public Function GetList_f02(mq As BO.myQuery) As IEnumerable(Of BO.f02FolderType) Implements If01FolderBL.GetList_f02
        Return _cDL.GetList_f02(mq)
    End Function
End Class
