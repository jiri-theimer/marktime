Public Interface Ij75DrillDownTemplateBL
    Inherits IFMother
    Function Save(cRec As BO.j75DrillDownTemplate, lisJ76 As List(Of BO.j76DrillDownTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
    Function Load(intPID As Integer) As BO.j75DrillDownTemplate
    Function LoadSystemTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As BO.j75DrillDownTemplate
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As IEnumerable(Of BO.j75DrillDownTemplate)
    Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As Boolean
    Function ColumnsPallete() As List(Of BO.PivotSumField)
    Function GetList_j76(intPID As Integer) As IEnumerable(Of BO.j76DrillDownTemplate_Item)
   
End Interface
Class j75DrillDownTemplateBL
    Inherits BLMother
    Implements Ij75DrillDownTemplateBL
    Private WithEvents _cDL As DL.j75DrillDownTemplateDL
    Private _x29id As BO.x29IdEnum

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.j75DrillDownTemplateDL(ServiceUser)
        _cUser = ServiceUser
    End Sub

    Public Function Delete(intPID As Integer) As Boolean Implements Ij75DrillDownTemplateBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As System.Collections.Generic.IEnumerable(Of BO.j75DrillDownTemplate) Implements Ij75DrillDownTemplateBL.GetList
        Return _cDL.GetList(myQuery, _x29id)
    End Function


    Public Function Load(intPID As Integer) As BO.j75DrillDownTemplate Implements Ij75DrillDownTemplateBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadSystemTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As BO.j75DrillDownTemplate Implements Ij75DrillDownTemplateBL.LoadSystemTemplate
        Dim cRec As BO.j75DrillDownTemplate = _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix)
        If cRec Is Nothing Then
            If Me.CheckDefaultTemplate(x29id, intJ03ID, strMasterPrefix) Then
                cRec = _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix)
            End If
        End If
        Return cRec
    End Function
    Public Function Save(cRec As BO.j75DrillDownTemplate, lisJ76 As List(Of BO.j76DrillDownTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ij75DrillDownTemplateBL.Save
        With cRec
            If .j03ID = 0 Then .j03ID = _cUser.PID
            If Trim(.j75Name) = "" Then _Error = "Chybí název drill-down šablony."
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí x29ID."
        End With

        If _Error <> "" Then Return False

        Return _cDL.Save(cRec, lisJ76, lisX69)

    End Function

    Public Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As Boolean Implements Ij75DrillDownTemplateBL.CheckDefaultTemplate
        If Not _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix) Is Nothing Then Return True 'systém šablona již existuje

        Dim c As New BO.j75DrillDownTemplate
        c.x29ID = x29id
        c.j75IsSystem = True
        c.j75Name = BL.My.Resources.common.VychoziDatovyPrehled
        c.j75MasterPrefix = strMasterPrefix
        c.j03ID = intJ03ID

        Select Case strMasterPrefix
            Case "j02_framework_detail"
                c.j75Name = "Osoba->Klient->Projekt"
                c.j75Level1 = BO.PivotRowColumnFieldType.Person
                c.j75Level2 = BO.PivotRowColumnFieldType.p28Name
                c.j75Level3 = BO.PivotRowColumnFieldType.p41Name

            Case "p28_framework_detail"
                c.j75Name = "Klient->Projekt->Osoba"
                c.j75Level1 = BO.PivotRowColumnFieldType.p28Name
                c.j75Level2 = BO.PivotRowColumnFieldType.p41Name
                c.j75Level3 = BO.PivotRowColumnFieldType.Person
            Case "p41_framework_detail"
                c.j75Name = "Projekt->Osoba->Měsíc"
                c.j75Level1 = BO.PivotRowColumnFieldType.p41Name
                c.j75Level2 = BO.PivotRowColumnFieldType.Person
                c.j75Level3 = BO.PivotRowColumnFieldType.Month
            Case "p91_framework_detail"
                c.j75Name = "Faktura->Sešit->Osoba"
                c.j75Level1 = BO.PivotRowColumnFieldType.p91Code
                c.j75Level2 = BO.PivotRowColumnFieldType.p34Name
                c.j75Level3 = BO.PivotRowColumnFieldType.Person
            Case Else
                c.j75Name = "Klient->Projekt->Osoba"
                c.j75Level1 = BO.PivotRowColumnFieldType.p28Name
                c.j75Level2 = BO.PivotRowColumnFieldType.p41Name
                c.j75Level3 = BO.PivotRowColumnFieldType.Person
        End Select

        Dim lisCols As New List(Of BO.j76DrillDownTemplate_Item)

        Dim row As New BO.j76DrillDownTemplate_Item
        row.j76PivotSumFieldType = BO.PivotSumFieldType.p31Hours_Orig
        lisCols.Add(row)
        row = New BO.j76DrillDownTemplate_Item
        row.j76PivotSumFieldType = BO.PivotSumFieldType.p31Hours_WIP
        lisCols.Add(row)
        row = New BO.j76DrillDownTemplate_Item
        row.j76PivotSumFieldType = BO.PivotSumFieldType.p31Hours_Approved_Billing
        lisCols.Add(row)

        Return Save(c, lisCols, Nothing)
    End Function

    Public Function ColumnsPallete() As List(Of BO.PivotSumField) Implements Ij75DrillDownTemplateBL.ColumnsPallete
        Dim lis As New List(Of BO.PivotSumField)

        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Orig))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_WIP))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Approved_Billing))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Approved_FixedPrice))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Approved_WriteOff))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Approved_InvoiceLater))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Invoiced))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Invoiced_FixedPrice))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_Invoiced_WriteOff))
        lis.Add(New BO.PivotSumField(BO.PivotSumFieldType.p31Hours_BIN))

        Return lis

    End Function
    Public Function GetList_j76(intPID As Integer) As IEnumerable(Of BO.j76DrillDownTemplate_Item) Implements Ij75DrillDownTemplateBL.GetList_j76
        Return _cDL.GetList_j76(intPID)
    End Function
End Class
