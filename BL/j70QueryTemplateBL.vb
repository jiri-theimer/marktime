Public Interface Ij70QueryTemplateBL
    Inherits IFMother
    Function Save(cRec As BO.j70QueryTemplate, lisJ71 As List(Of BO.j71QueryTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
    Function Load(intPID As Integer) As BO.j70QueryTemplate
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As IEnumerable(Of BO.j70QueryTemplate)
    Function GetList_j71(intPID As Integer) As IEnumerable(Of BO.j71QueryTemplate_Item)
    Function GetList_OtherQueryItem(x29id As BO.x29IdEnum) As List(Of BO.OtherQueryItem)

    Function GetSqlWhere(intJ70ID As Integer) As String
    
    Sub Setupj71TempList(intPID As Integer, strGUID As String)
End Interface

Class j70QueryTemplateBL
    Inherits BLMother
    Implements Ij70QueryTemplateBL
    Private WithEvents _cDL As DL.j70QueryTemplateDL
    Private _x29id As BO.x29IdEnum

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.j70QueryTemplateDL(ServiceUser)
        _cUser = ServiceUser
    End Sub

    Public Function Delete(intPID As Integer) As Boolean Implements Ij70QueryTemplateBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As System.Collections.Generic.IEnumerable(Of BO.j70QueryTemplate) Implements Ij70QueryTemplateBL.GetList
        Return _cDL.GetList(myQuery, _x29id)
    End Function


    Public Function Load(intPID As Integer) As BO.j70QueryTemplate Implements Ij70QueryTemplateBL.Load
        Return _cDL.Load(intPID)
    End Function
   
    Public Function Save(cRec As BO.j70QueryTemplate, lisJ71 As List(Of BO.j71QueryTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ij70QueryTemplateBL.Save
        With cRec
            If .j03ID = 0 Then .j03ID = _cUser.PID
            If Trim(.j70Name) = "" Then _Error = "Chybí název šablony filtru."
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí x29ID."

        End With

        If _Error <> "" Then Return False

        Return _cDL.Save(cRec, lisJ71, lisX69)

    End Function

    Public Sub Setupj71TempList(intPID As Integer, strGUID As String) Implements Ij70QueryTemplateBL.Setupj71TempList
        _cDL.Setupj71TempList(intPID, strGUID)
    End Sub

    
    Public Function GetList_j71(intPID As Integer) As IEnumerable(Of BO.j71QueryTemplate_Item) Implements Ij70QueryTemplateBL.GetList_j71
        Return _cDL.GetList_j71(intPID)
    End Function


    Public Function GetList_OtherQueryItem(x29id As BO.x29IdEnum) As List(Of BO.OtherQueryItem) Implements Ij70QueryTemplateBL.GetList_OtherQueryItem
        Dim lis As New List(Of BO.OtherQueryItem)
        Select Case x29id
            Case BO.x29IdEnum.p41Project
                lis.Add(New BO.OtherQueryItem(3, "Obsahují rozpracované úkony, které čekají na schvalování"))
                lis.Add(New BO.OtherQueryItem(5, "Obsahují schválené úkony, které čekají na fakturaci"))
                lis.Add(New BO.OtherQueryItem(15, "Obsahují vyfakturované úkony"))
                lis.Add(New BO.OtherQueryItem(4, "Došlo k překročení limitu rozpracovanosti"))
                lis.Add(New BO.OtherQueryItem(7, "Obsahují úkoly (otevřené nebo uzavřené)"))
                lis.Add(New BO.OtherQueryItem(6, "Obsahují otevřené úkoly"))
                lis.Add(New BO.OtherQueryItem(9, "Obsahují minimálně jeden notepad dokument"))
                lis.Add(New BO.OtherQueryItem(10, "Obsahují opakované odměny/paušály"))
                lis.Add(New BO.OtherQueryItem(11, "Projekty v režimu DRAFT"))
                lis.Add(New BO.OtherQueryItem(12, "Projekty mimo režim DRAFT"))
                lis.Add(New BO.OtherQueryItem(13, "Není přiřazen ceník k projektu ani ke klientovi projektu"))
                lis.Add(New BO.OtherQueryItem(14, "Je přiřazen ceník k projektu nebo ke klientovi projektu"))
                lis.Add(New BO.OtherQueryItem(16, "Přiřazena minimálně jedna kontaktní osoba"))
                lis.Add(New BO.OtherQueryItem(17, "Bez přiřazení kontaktních osob"))
                lis.Add(New BO.OtherQueryItem(18, "Má nadřízený projekt"))
                lis.Add(New BO.OtherQueryItem(19, "Má pod sebou podřízené projekty"))
            Case BO.x29IdEnum.p28Contact
                lis.Add(New BO.OtherQueryItem(3, "Projekty klienta obsahují rozpracované úkony, které čekají na schvalování"))
                lis.Add(New BO.OtherQueryItem(5, "Projektovy klienta obsahují schválené úkony, které čekají na fakturaci"))
                lis.Add(New BO.OtherQueryItem(4, "Došlo k překročení limitu rozpracovanosti (buď limit na projektech nebo limit samotného klienta)"))
                lis.Add(New BO.OtherQueryItem(18, "Je klientem minimálně jednoho projektu"))
                lis.Add(New BO.OtherQueryItem(19, "Není klientem ani u jednoho projektu"))
                lis.Add(New BO.OtherQueryItem(16, "Přiřazena minimálně jedna kontaktní osoba"))
                lis.Add(New BO.OtherQueryItem(17, "Bez přiřazení kontaktních osob"))
                lis.Add(New BO.OtherQueryItem(20, "Obsahují minimálně jeden notepad dokument"))
                lis.Add(New BO.OtherQueryItem(21, "Vystupuje jako dodavatel"))
                lis.Add(New BO.OtherQueryItem(22, "Duplicitní klienti podle prvních 25 písmen v názvu"))
                lis.Add(New BO.OtherQueryItem(23, "Duplicitní klienti podle IČ"))
                lis.Add(New BO.OtherQueryItem(24, "Duplicitní klienti podle DIČ"))
                lis.Add(New BO.OtherQueryItem(25, "Má nadřízeného klienta"))
                lis.Add(New BO.OtherQueryItem(26, "Má pod sebou podřízené klienty"))
            Case BO.x29IdEnum.j02Person
                lis.Add(New BO.OtherQueryItem(3, "Existují rozpracované úkony, které čekají na schvalování"))
                lis.Add(New BO.OtherQueryItem(5, "Existují schválené úkony, které čekají na fakturaci"))
            Case BO.x29IdEnum.p91Invoice
                lis.Add(New BO.OtherQueryItem(3, "Ve splatnosti"))
                lis.Add(New BO.OtherQueryItem(4, "Neuhrazené po splatnosti"))
                lis.Add(New BO.OtherQueryItem(7, "Svázané se zálohou"))
                lis.Add(New BO.OtherQueryItem(8, "Svázané s opravným dokladem"))
                lis.Add(New BO.OtherQueryItem(5, "Pouze DRAFT faktury"))
                lis.Add(New BO.OtherQueryItem(6, "Pouze faktury s oficiálním číslem"))
                lis.Add(New BO.OtherQueryItem(9, "Obsahuje částku zaokrouhlení"))
                lis.Add(New BO.OtherQueryItem(10, "Obsahuje základ se základní sazbou DPH"))
                lis.Add(New BO.OtherQueryItem(11, "Obsahuje základ se sníženou sazbou DPH"))
                lis.Add(New BO.OtherQueryItem(12, "Obsahuje základ s nulovou sazbou DPH"))
                lis.Add(New BO.OtherQueryItem(13, "Obsahuje přepočet podle měnového kurzu"))
            Case BO.x29IdEnum.p31Worksheet
                lis.Add(New BO.OtherQueryItem(1, "Rozpracovanost, čeká na schvalování"))
                lis.Add(New BO.OtherQueryItem(2, "Schváleno, čeká na fakturaci"))
                lis.Add(New BO.OtherQueryItem(3, "Vyfakturováno"))
                lis.Add(New BO.OtherQueryItem(4, "Rozpracovanost přesunutá do archivu"))
                lis.Add(New BO.OtherQueryItem(6, "Přiřazená kontaktní osoba"))
                lis.Add(New BO.OtherQueryItem(7, "Přiřazen dokument"))
                lis.Add(New BO.OtherQueryItem(8, "Vyplněná výchozí korekce úkonu"))
                lis.Add(New BO.OtherQueryItem(9, "Přiřazen úkol"))
                lis.Add(New BO.OtherQueryItem(10, "Přiřazen dodavatel"))
                lis.Add(New BO.OtherQueryItem(11, "Vazba na rozpočet"))
                lis.Add(New BO.OtherQueryItem(12, "Vyplněn kód dokladu"))
                lis.Add(New BO.OtherQueryItem(13, "vygenerováno automaticky robotem"))
            Case BO.x29IdEnum.p56Task
                lis.Add(New BO.OtherQueryItem(3, "Obsahují rozpracované úkony, které čekají na schvalování"))
                lis.Add(New BO.OtherQueryItem(5, "Obsahují schválené úkony, které čekají na fakturaci"))
                lis.Add(New BO.OtherQueryItem(6, "Vyplněn termín dokončení úkolu"))
                lis.Add(New BO.OtherQueryItem(7, "Je po termínu dokončení úkolu"))
                lis.Add(New BO.OtherQueryItem(8, "Vyplněno plánované zahájení úkolu"))
                lis.Add(New BO.OtherQueryItem(9, "Vyplněn plán/limit hodin"))
                lis.Add(New BO.OtherQueryItem(10, "Vyplněn plán/limit výdajů"))
                lis.Add(New BO.OtherQueryItem(11, "Došlo k překročení plánu/limitu hodin"))
                lis.Add(New BO.OtherQueryItem(12, "Došlo k překročení plánu/limitu výdajů"))
            Case BO.x29IdEnum.o23Notepad
                lis.Add(New BO.OtherQueryItem(3, "Dokument byl svázán s projektem"))
                lis.Add(New BO.OtherQueryItem(4, "Dokument čeká na vazbu s projektem"))
                lis.Add(New BO.OtherQueryItem(5, "Dokument byl svázán s klientem"))
                lis.Add(New BO.OtherQueryItem(6, "Dokument čeká na vazbu s klientem"))
                lis.Add(New BO.OtherQueryItem(7, "Dokument byl svázán s fakturou"))
                lis.Add(New BO.OtherQueryItem(8, "Dokument čeká na vazbu s fakturou"))
                lis.Add(New BO.OtherQueryItem(9, "Dokument byl svázán s worksheet úkonem"))
                lis.Add(New BO.OtherQueryItem(10, "Dokument čeká na vazbu s úkonem"))
                lis.Add(New BO.OtherQueryItem(11, "Dokument byl svázán s osobou"))
                lis.Add(New BO.OtherQueryItem(12, "Dokument čeká na vazbu s osobou"))
        End Select
        Return lis
    End Function

    Public Function GetSqlWhere(intJ70ID As Integer) As String Implements Ij70QueryTemplateBL.GetSqlWhere
        Return _cDL.GetSqlWhere(intJ70ID)
    End Function
End Class
