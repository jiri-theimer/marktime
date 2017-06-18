Public Interface Ix18EntityCategoryBL
    Inherits ifMother
    Function Save(cRec As BO.x18EntityCategory, lisX20 As List(Of BO.x20EntiyToCategory), lisX69 As List(Of BO.x69EntityRole_Assign), lisX16 As List(Of BO.x16EntityCategory_FieldSetting)) As Boolean
    Function Load(intPID As Integer) As BO.x18EntityCategory
    Function Delete(intPID As Integer) As Boolean
    Function GetList(Optional myQuery As BO.myQuery = Nothing, Optional x29ID As BO.x29IdEnum = BO.x29IdEnum._NotSpecified, Optional intEntityType As Integer = -1, Optional bolInhaleAllCols As Boolean = False) As IEnumerable(Of BO.x18EntityCategory)
    Function GetList_x20(intX18ID As Integer) As IEnumerable(Of BO.x20EntiyToCategory)
    Function GetList_x20(x18IDs As List(Of Integer)) As IEnumerable(Of BO.x20EntiyToCategory)
    Function GetList_x20_join_x18(x29ID As BO.x29IdEnum, Optional intEntityType As Integer = 0) As IEnumerable(Of BO.x20_join_x18)
    Function SaveX19Binding(x29id As BO.x29IdEnum, intRecordPID As Integer, lisX19 As List(Of BO.x19EntityCategory_Binding), x20IDs As List(Of Integer)) As Boolean
    Function SaveX19TempBinding(intRecordPID As Integer, strTempGUID As String, lisX19 As List(Of BO.x19EntityCategory_Binding)) As Boolean
    Function GetList_X19(x29id As BO.x29IdEnum, intRecordPID As Integer, strTempGUID As String, x20IDs_Query As List(Of Integer)) As IEnumerable(Of BO.x19EntityCategory_Binding)
    Function GetList_X25(x29id As BO.x29IdEnum) As IEnumerable(Of BO.x25EntityField_ComboValue)   
    Function GetList_x16(intX18ID As Integer) As IEnumerable(Of BO.x16EntityCategory_FieldSetting)
End Interface
Class x18EntityCategoryBL
    Inherits BLMother
    Implements Ix18EntityCategoryBL
    Private WithEvents _cDL As DL.x18EntityCategoryDL

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.x18EntityCategoryDL(ServiceUser)
        _cUser = ServiceUser
    End Sub

    Public Function Load(intPID As Integer) As BO.x18EntityCategory Implements Ix18EntityCategoryBL.Load
        Return _cDL.Load(intPID)
    End Function

    Public Function Save(cRec As BO.x18EntityCategory, lisX20 As List(Of BO.x20EntiyToCategory), lisX69 As List(Of BO.x69EntityRole_Assign), lisX16 As List(Of BO.x16EntityCategory_FieldSetting)) As Boolean Implements Ix18EntityCategoryBL.Save
        If cRec.x23ID = 0 Then _Error = "Chybí datový zdroj (combo seznam)." : Return False
        If cRec.j02ID_Owner = 0 Then cRec.j02ID_Owner = _cUser.j02ID

        ''If cRec.x18IsAllEntityTypes Then
        ''    lisX22 = New List(Of BO.x22EntiyCategory_Binding)
        ''    cRec.x18IsRequired = False
        ''Else
        ''    If Not lisX22 Is Nothing Then
        ''        If lisX22.Count = 0 Then
        ''            _Error = "Musíte zaškrtnout minimálně jeden typ entity." : Return False
        ''        End If
        ''    End If
        ''End If
        If Not lisX16 Is Nothing Then
            For Each c In lisX16
                If Trim(c.x16Name) = "" Then
                    _Error = "V nastavení uživatelských polí štítku chybí název." : Return False
                End If
                If Trim(c.x16Field) = "" Then
                    _Error = "V nastavení uživatelských polí štítku chybí obsazené pole." : Return False
                End If
            Next
        End If
        If Not lisX20 Is Nothing Then
            
        End If

        If _cDL.Save(cRec, lisX20, lisX69, lisX16) Then
            Dim cX23 As BO.x23EntityField_Combo = Factory.x23EntityField_ComboBL.Load(cRec.x23ID)
            If cX23.x23Ordinary = -666 Then
                cX23.x23Name = cRec.x18Name
                Factory.x23EntityField_ComboBL.Save(cX23)
            End If

            Return True
        Else
            Return False
        End If
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ix18EntityCategoryBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing, Optional x29ID As BO.x29IdEnum = BO.x29IdEnum._NotSpecified, Optional intEntityType As Integer = -1, Optional bolInhaleAllCols As Boolean = False) As IEnumerable(Of BO.x18EntityCategory) Implements Ix18EntityCategoryBL.GetList
        Return _cDL.GetList(myQuery, x29ID, intEntityType, bolInhaleAllCols)
    End Function

    
    Public Function SaveX19Binding(x29id As BO.x29IdEnum, intRecordPID As Integer, lisX19 As List(Of BO.x19EntityCategory_Binding), x20IDs As List(Of Integer)) As Boolean Implements Ix18EntityCategoryBL.SaveX19Binding
        If x20IDs Is Nothing Then Return False
        If x20IDs.Count = 0 Then Return True
        Return _cDL.SaveX19Binding(x29id, intRecordPID, lisX19, x20IDs)
    End Function
    Public Function SaveX19TempBinding(intRecordPID As Integer, strTempGUID As String, lisX19 As List(Of BO.x19EntityCategory_Binding)) As Boolean Implements Ix18EntityCategoryBL.SaveX19TempBinding
        Return _cDL.SaveX19TempBinding(intRecordPID, strTempGUID, lisX19)
    End Function
    Public Function GetList_X19(x29id As BO.x29IdEnum, intRecordPID As Integer, strTempGUID As String, x20IDs_Query As List(Of Integer)) As IEnumerable(Of BO.x19EntityCategory_Binding) Implements Ix18EntityCategoryBL.GetList_X19
        Return _cDL.GetList_X19(x29id, intRecordPID, strTempGUID, x20IDs_Query)
    End Function
    Public Function GetList_X25(x29id As BO.x29IdEnum) As IEnumerable(Of BO.x25EntityField_ComboValue) Implements Ix18EntityCategoryBL.GetList_X25
        Return _cDL.GetList_X25(x29id)
    End Function
    Public Function GetList_x20(intX18ID As Integer) As IEnumerable(Of BO.x20EntiyToCategory) Implements Ix18EntityCategoryBL.GetList_x20
        Return _cDL.GetList_x20(BO.BAS.ConvertInt2List(intX18ID))
    End Function
    Public Function GetList_x20(x18IDs As List(Of Integer)) As IEnumerable(Of BO.x20EntiyToCategory) Implements Ix18EntityCategoryBL.GetList_x20
        Return _cDL.GetList_x20(x18IDs)
    End Function
    Public Function GetList_x20_join_x18(x29ID As BO.x29IdEnum, Optional intEntityType As Integer = 0) As IEnumerable(Of BO.x20_join_x18) Implements Ix18EntityCategoryBL.GetList_x20_join_x18
        Return _cDL.GetList_x20_join_x18(x29ID, intEntityType)
    End Function
    Public Function GetList_x16(intX18ID As Integer) As IEnumerable(Of BO.x16EntityCategory_FieldSetting) Implements Ix18EntityCategoryBL.GetList_x16
        Return _cDL.GetList_x16(intX18ID)
    End Function
End Class
