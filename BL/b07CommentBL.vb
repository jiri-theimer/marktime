Public Interface Ib07CommentBL
    Inherits IFMother
    Function Save(cRec As BO.b07Comment, strUploadGUID As String, notifyReceivers As List(Of BO.PersonOrTeam)) As Boolean
    Function Load(intPID As Integer) As BO.b07Comment
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQueryB07) As IEnumerable(Of BO.b07Comment)

End Interface
Class b07CommentBL
    Inherits BLMother
    Implements Ib07CommentBL
    Private WithEvents _cDL As DL.b07CommentDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.b07CommentDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.b07Comment, strUploadGUID As String, notifyReceivers As List(Of BO.PersonOrTeam)) As Boolean Implements Ib07CommentBL.Save
        Dim lisTempUpload As IEnumerable(Of BO.p85TempBox) = Nothing

        If strUploadGUID <> "" Then
            lisTempUpload = Me.Factory.p85TempBoxBL.GetList(strUploadGUID, True)
        End If
        With cRec
            If Len(Trim(.b07Value)) <= 1 And .b07WorkflowInfo = "" Then _Error = "Chybí obsah komentáře." : Return False
            If .j02ID_Owner = 0 Then .j02ID_Owner = _cUser.j02ID
        End With

        If _cDL.Save(cRec) Then
            If strUploadGUID <> "" Then
                Me.Factory.o27AttachmentBL.UploadAndSaveUserControl(lisTempUpload, BO.x29IdEnum.b07Comment, Me.LastSavedPID)
            End If
            Me.RaiseAppEvent(BO.x45IDEnum.b07_new, _LastSavedPID)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Load(intPID As Integer) As BO.b07Comment Implements Ib07CommentBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ib07CommentBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQueryB07) As IEnumerable(Of BO.b07Comment) Implements Ib07CommentBL.GetList
        Return _cDL.GetList(mq)
    End Function
End Class
