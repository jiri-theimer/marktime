Public Interface Ix25EntityField_ComboValueBL
    Inherits IFMother
    Function Save(cRec As BO.x25EntityField_ComboValue, intX18ID As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), lisX19 As List(Of BO.x19EntityCategory_Binding), x20IDs As List(Of Integer)) As Boolean
    Function Load(intPID As Integer) As BO.x25EntityField_ComboValue
    Function LoadByCode(strCode As String, intX23ID As Integer) As BO.x25EntityField_ComboValue
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQueryX25) As IEnumerable(Of BO.x25EntityField_ComboValue)
    Function GetDataTable4Grid(myQuery As BO.myQueryX25) As DataTable
    Function GetVirtualCount(myQuery As BO.myQueryX25) As Integer
    Sub SetCalendarDateFields(strCalendarFieldStart As String, strCalendarFieldEnd As String)
End Interface
Class x25EntityField_ComboValueBL
    Inherits BLMother
    Implements Ix25EntityField_ComboValueBL
    Private WithEvents _cDL As DL.x25EntityField_ComboValueDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.x25EntityField_ComboValueDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.x25EntityField_ComboValue, intX18ID As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), lisX19 As List(Of BO.x19EntityCategory_Binding), x20IDs As List(Of Integer)) As Boolean Implements Ix25EntityField_ComboValueBL.Save
        With cRec
            If .j02ID_Owner = 0 Then .j02ID_Owner = _cUser.j02ID
            If Trim(.x25Name) = "" And intX18ID <> 0 Then
                Dim cX18 As BO.x18EntityCategory = Factory.x18EntityCategoryBL.Load(intX18ID)
                If cX18.x18EntryNameFlag = BO.x18EntryNameENUM.Manual Then
                    _Error = "Chybí název." : Return False
                End If
            End If

            ''If .x23ID = 0 Then _Error = "Chybí vazba na číselník." : Return False

            ''Dim cX23 As BO.x23EntityField_Combo = Factory.x23EntityField_ComboBL.Load(.x23ID)
            ''If cX23.x23DataSource <> "" Then
            ''    _Error = "Combo seznam [" & cX23.x23Name & "] má externí datový zdroj combo položek." : Return False
            ''End If
        End With

        ''Dim cRecOld As BO.x25EntityField_ComboValue = Nothing
        ''If cRec.PID <> 0 Then cRecOld = Load(cRec.PID)

        If _cDL.Save(cRec, lisX69) Then
            Dim intX25ID As Integer = _LastSavedPID

            If Not lisX19 Is Nothing Then
                Factory.x18EntityCategoryBL.SaveX19Binding(intX25ID, lisX19, x20IDs)
            End If

            Me.RaiseAppEvent_TailoringAfterSave(intX25ID, "x25_aftersave")
            If intX18ID <> 0 Then
                Dim cX18 As BO.x18EntityCategory = Me.Factory.x18EntityCategoryBL.Load(intX18ID)
                Dim intB01ID As Integer = cX18.b01ID
                If cRec.PID = 0 Then
                    If intB01ID > 0 Then InhaleDefaultWorkflowMove(intX25ID, intB01ID) 'je třeba nahodit výchozí workflow stav
                Else
                    If intB01ID > 0 And cRec.b02ID = 0 Then InhaleDefaultWorkflowMove(cRec.PID, intB01ID) 'chybí hodnota workflow stavu
                End If
            End If


            Return True
        Else
            Return False
        End If
    End Function
    Private Sub InhaleDefaultWorkflowMove(intX25ID As Integer, intB01ID As Integer)
        Dim cB06 As BO.b06WorkflowStep = Me.Factory.b06WorkflowStepBL.LoadKickOffStep(intB01ID)
        If cB06 Is Nothing Then Return
        Me.Factory.b06WorkflowStepBL.RunWorkflowStep(cB06, intX25ID, BO.x29IdEnum.x25EntityField_ComboValue, "", "", False, Nothing)
    End Sub
    Public Function GetVirtualCount(myQuery As BO.myQueryX25) As Integer Implements Ix25EntityField_ComboValueBL.GetVirtualCount
        Return _cDL.GetVirtualCount(myQuery)
    End Function
    Public Function Load(intPID As Integer) As BO.x25EntityField_ComboValue Implements Ix25EntityField_ComboValueBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadByCode(strCode As String, intX23ID As Integer) As BO.x25EntityField_ComboValue Implements Ix25EntityField_ComboValueBL.LoadByCode
        Return _cDL.LoadByCode(strCode, intX23ID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ix25EntityField_ComboValueBL.Delete
        Dim cRec As BO.x25EntityField_ComboValue = Load(intPID)
        If _cDL.Delete(intPID) Then
            Dim lisX28 As IEnumerable(Of BO.x28EntityField) = Factory.x28EntityFieldBL.GetList(cRec.x23ID)
            For Each c In lisX28
                _cDL.ClearComboItemTextInData(c, intPID)
            Next
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(myQuery As BO.myQueryX25) As IEnumerable(Of BO.x25EntityField_ComboValue) Implements Ix25EntityField_ComboValueBL.GetList
        Return _cDL.GetList(myQuery)
    End Function
    Public Function GetDataTable4Grid(myQuery As BO.myQueryX25) As DataTable Implements Ix25EntityField_ComboValueBL.GetDataTable4Grid
        Return _cDL.GetDataTable4Grid(myQuery)
    End Function
    Public Sub SetCalendarDateFields(strCalendarFieldStart As String, strCalendarFieldEnd As String) Implements Ix25EntityField_ComboValueBL.SetCalendarDateFields
        _cDL.CalendarFieldStart = strCalendarFieldStart
        _cDL.CalendarFieldEnd = strCalendarFieldEnd
    End Sub
End Class
