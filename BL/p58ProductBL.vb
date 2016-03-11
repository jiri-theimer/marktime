Public Interface Ip58ProductBL
    Inherits IFMother
    Function Save(cRec As BO.p58Product) As Boolean
    Function Load(intPID As Integer) As BO.p58Product
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQuery, Optional intP28ID As Integer = 0) As IEnumerable(Of BO.p58Product)
End Interface

Class p58ProductBL
    Inherits BLMother
    Implements Ip58ProductBL
    Private WithEvents _cDL As DL.p58ProductDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p58ProductDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p58Product) As Boolean Implements Ip58ProductBL.Save
        With cRec
            If Trim(.p58Name) = "" Then _Error = "Chybí název produktu." : Return False
            If .p58ParentID <> 0 Then
                Dim cParent As BO.p58Product = Load(.p58ParentID)
                If .p58TreePrev <= cParent.p58TreeIndex And .p58TreeNext >= cParent.p58TreeIndex Then
                    If .p58TreePrev > 0 Or .p58TreeNext > 0 Or cParent.p58TreeIndex > 0 Then
                        _Error = "Stromové zatřídění záznamu není logické." : Return False
                    End If
                End If
            End If
        End With
        

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.p58Product Implements Ip58ProductBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip58ProductBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(mq As BO.myQuery, Optional intP28ID As Integer = 0) As IEnumerable(Of BO.p58Product) Implements Ip58ProductBL.GetList
        Return _cDL.GetList(mq, intP28ID)
    End Function
End Class
