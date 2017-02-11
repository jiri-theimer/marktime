Public Interface Ip90ProformaBL
    Inherits IFMother
    Function Save(cRec As BO.p90Proforma, lisFF As List(Of BO.FreeField)) As Boolean
    Function Load(intPID As Integer) As BO.p90Proforma
    Function LoadMyLastCreated() As BO.p90Proforma
    Function LoadByP82ID(intP82ID As Integer) As BO.p90Proforma
    Function UpdateP82Code(intP90ID As Integer, strP82Code As String) As Boolean
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQueryP90) As IEnumerable(Of BO.p90Proforma)
    Function GetList_p99(intP91ID As Integer) As IEnumerable(Of BO.p99Invoice_Proforma)
End Interface
Class p90ProformaBL
    Inherits BLMother
    Implements Ip90ProformaBL
    Private WithEvents _cDL As DL.p90ProformaDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p90ProformaDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p90Proforma, lisFF As List(Of BO.FreeField)) As Boolean Implements Ip90ProformaBL.Save
        With cRec
            If .p28ID = 0 Then _Error = "Chybí klient." : Return False
            If .j27ID = 0 Then _Error = "Chybí měna." : Return False
            If .p89ID = 0 Then _Error = "Chybí typ zálohy." : Return False
            If .p90Amount > 0 And .p90Amount_WithoutVat > 0 Then
                If .p90Amount_WithoutVat + .p90Amount_Vat <> .p90Amount Then
                    _Error = "Částka bez DPH + částka DPH musí souhlasit s celkovou částkou." : Return False
                End If
            End If
            If .p90Amount_Billed < 0 Then
                _Error = "Částka úhrady musí být kladné číslo." : Return False
            End If
            If .p90Amount_Billed > 0 Then
                If .p90DateBilled Is Nothing Then
                    _Error = "Pokud je zapsaná úhrada, musí být vyplněno i datum úhrady." : Return False
                End If
            End If
            If Not .p90DateBilled Is Nothing Then
                If .p90Amount_Billed = 0 Then
                    _Error = "Pokud je uvedeno datum úhrady, musí být vyplněna i částka úhrady." : Return False
                End If
            End If
            If .j02ID_Owner = 0 Then .j02ID_Owner = _cUser.j02ID

            .p90Amount_Debt = .p90Amount - .p90Amount_Billed

        End With

        If _cDL.Save(cRec, lisFF) Then
            If cRec.PID = 0 Then
                Me.RaiseAppEvent(BO.x45IDEnum.p90_new, _LastSavedPID)
            Else
                Me.RaiseAppEvent(BO.x45IDEnum.p90_update, _LastSavedPID)
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Load(intPID As Integer) As BO.p90Proforma Implements Ip90ProformaBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadMyLastCreated() As BO.p90Proforma Implements Ip90ProformaBL.LoadMyLastCreated
        Return _cDL.LoadMyLastCreated()
    End Function
    Public Function LoadByP82ID(intP82ID As Integer) As BO.p90Proforma Implements Ip90ProformaBL.LoadByP82ID
        Return _cDL.LoadByP82ID(intP82ID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip90ProformaBL.Delete
        Dim s As String = Me.Factory.GetRecordCaption(BO.x29IdEnum.p90Proforma, intPID)
        If _cDL.Delete(intPID) Then
            Me.RaiseAppEvent(BO.x45IDEnum.p90_delete, intPID, s)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(mq As BO.myQueryP90) As IEnumerable(Of BO.p90Proforma) Implements Ip90ProformaBL.GetList
        Return _cDL.GetList(mq)
    End Function
    Public Function UpdateP82Code(intP90ID As Integer, strP82Code As String) As Boolean Implements Ip90ProformaBL.UpdateP82Code
        Return _cDL.UpdateP82Code(intP90ID, strP82Code)
    End Function
    Public Function GetList_p99(intP91ID As Integer) As IEnumerable(Of BO.p99Invoice_Proforma) Implements Ip90ProformaBL.GetList_p99
        Return _cDL.GetList_p99(intP91ID)
    End Function
End Class
