Public Interface Ij70QueryTemplateBL
    Inherits IFMother
    Function Save(cRec As BO.j70QueryTemplate, lisJ71 As List(Of BO.j71QueryTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
    Function Load(intPID As Integer) As BO.j70QueryTemplate
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum, Optional strMasterPrefix As String = "") As IEnumerable(Of BO.j70QueryTemplate)
    Function GetList_j71(intPID As Integer) As IEnumerable(Of BO.j71QueryTemplate_Item)
    Function GetList_OtherQueryItem(x29id As BO.x29IdEnum) As List(Of BO.OtherQueryItem)

    Function GetSqlWhere(intJ70ID As Integer) As String
    
    Sub Setupj71TempList(intPID As Integer, strGUID As String)

    Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As Boolean
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

    Public Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum, Optional strMasterPrefix As String = "") As System.Collections.Generic.IEnumerable(Of BO.j70QueryTemplate) Implements Ij70QueryTemplateBL.GetList
        Return _cDL.GetList(myQuery, _x29id, strMasterPrefix)
    End Function


    Public Function Load(intPID As Integer) As BO.j70QueryTemplate Implements Ij70QueryTemplateBL.Load
        Return _cDL.Load(intPID)
    End Function
   
    Public Function Save(cRec As BO.j70QueryTemplate, lisJ71 As List(Of BO.j71QueryTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ij70QueryTemplateBL.Save
        With cRec
            If .j03ID = 0 Then .j03ID = _cUser.PID
            If Trim(.j70Name) = "" Then _Error = "Chybí název šablony filtru."
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí x29ID."
            If Trim(.j70ColumnNames) = "" Then
                _Error = "Přehled musí obsahovat minimálně jeden sloupec."
            End If
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
                lis.Add(New BO.OtherQueryItem(19, "Má pod-projekty"))
                lis.Add(New BO.OtherQueryItem(20, "Moje oblíbené projekty"))
                lis.Add(New BO.OtherQueryItem(21, "Vyplněna fakturační poznámka projektu"))
                lis.Add(New BO.OtherQueryItem(22, "Je založen šanon"))
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
                lis.Add(New BO.OtherQueryItem(28, "Ani klient, ani dodavatel"))
                lis.Add(New BO.OtherQueryItem(22, "Duplicitní klienti podle prvních 25 písmen v názvu"))
                lis.Add(New BO.OtherQueryItem(23, "Duplicitní klienti podle IČ"))
                lis.Add(New BO.OtherQueryItem(24, "Duplicitní klienti podle DIČ"))
                lis.Add(New BO.OtherQueryItem(25, "Má nadřízeného klienta"))
                lis.Add(New BO.OtherQueryItem(26, "Má pod sebou podřízené klienty"))
                lis.Add(New BO.OtherQueryItem(27, "Nastavena režijní fakturační přirážka"))
                lis.Add(New BO.OtherQueryItem(29, "Vyplněna fakturační poznámka klienta"))
                lis.Add(New BO.OtherQueryItem(30, "Zařazen do monitoringu insolvence"))
            Case BO.x29IdEnum.j02Person
                lis.Add(New BO.OtherQueryItem(6, "Pouze interní osoby"))
                lis.Add(New BO.OtherQueryItem(7, "Pouze kontaktní osoby"))
                lis.Add(New BO.OtherQueryItem(3, "Existují rozpracované úkony, které čekají na schvalování"))
                lis.Add(New BO.OtherQueryItem(5, "Existují schválené úkony, které čekají na fakturaci"))
            Case BO.x29IdEnum.p91Invoice
                lis.Add(New BO.OtherQueryItem(3, "Ve splatnosti"))
                lis.Add(New BO.OtherQueryItem(4, "Neuhrazené po splatnosti"))
                lis.Add(New BO.OtherQueryItem(15, "Neuhrazené"))
                lis.Add(New BO.OtherQueryItem(7, "Svázané se zálohou"))
                lis.Add(New BO.OtherQueryItem(8, "Svázané s opravným dokladem"))
                lis.Add(New BO.OtherQueryItem(5, "Pouze DRAFT faktury"))
                lis.Add(New BO.OtherQueryItem(6, "Pouze faktury s oficiálním číslem"))
                lis.Add(New BO.OtherQueryItem(9, "Obsahuje haléřové zaokrouhlení"))
                lis.Add(New BO.OtherQueryItem(10, "Obsahuje základ se základní sazbou DPH"))
                lis.Add(New BO.OtherQueryItem(11, "Obsahuje základ se sníženou sazbou DPH"))
                lis.Add(New BO.OtherQueryItem(12, "Obsahuje základ s nulovou sazbou DPH"))
                lis.Add(New BO.OtherQueryItem(13, "Obsahuje přepočet podle měnového kurzu"))
                lis.Add(New BO.OtherQueryItem(14, "Nastavena režijní fakturační přirážka"))
            Case BO.x29IdEnum.p31Worksheet
                lis.Add(New BO.OtherQueryItem(1, "Rozpracovanost, čeká na schvalování"))
                lis.Add(New BO.OtherQueryItem(2, "Schváleno, čeká na fakturaci"))
                lis.Add(New BO.OtherQueryItem(3, "Vyfakturováno"))
                lis.Add(New BO.OtherQueryItem(4, "Přesunuto do archivu"))
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

    Public Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As Boolean Implements Ij70QueryTemplateBL.CheckDefaultTemplate
        If Not _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix) Is Nothing Then Return True 'systém šablona již existuje

        Dim c As New BO.j70QueryTemplate
        c.x29ID = x29id
        c.j70IsSystem = True
        c.j70Name = BL.My.Resources.common.VychoziDatovyPrehled
        c.j70MasterPrefix = strMasterPrefix

        If c.j70MasterPrefix = "" Or (x29id = BO.x29IdEnum.p31Worksheet And c.j70MasterPrefix = "p31_grid") Or c.j70MasterPrefix = "p31_framework" Then
            c.j70IsFilteringByColumn = True 'pro hlavní přehledy nahodit sloupcový auto-filter
            c.j70ScrollingFlag = BO.j74ScrollingFlagENUM.StaticHeaders    'pro hlavní přehledy nastavit ukotvení záhlaví
        End If

        Select Case x29id
            Case BO.x29IdEnum.p31Worksheet
                Select Case strMasterPrefix
                    Case "j02", "j02-p31"
                        c.j70Name = String.Format(BL.My.Resources.common.VychoziPrehledOsoby, "osoby")
                        c.j70ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "j02-approved"
                        c.j70Name = "Schválené úkony osoby"
                        c.j70ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Amount_WithoutVat_Approved,p31Text"
                    Case "j02-time"
                        c.j70Name = "Hodiny osoby"
                        c.j70ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "j02-expense", "j02-fee"
                        c.j70Name = "Peněžní úkony osoby"
                        c.j70ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Amount_WithoutVat_Orig,p31VatRate_Orig,p31Amount_WithVat_Orig,p31Text"
                    Case "j02-kusovnik"
                        c.j70Name = "Kusovníkové úkony osoby"
                        c.j70ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Value_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p28", "p28-p31"
                        c.j70Name = My.Resources.common.VychoziPrehledKlienta
                        c.j70ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p28-approved"
                        c.j70Name = "Schválené úkony klienta"
                        c.j70ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"
                    Case "p28-time"
                        c.j70Name = "Hodiny klienta"
                        c.j70ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p28-expense", "p28-fee"
                        c.j70Name = "Peněžní úkony klienta"
                        c.j70ColumnNames = "p31Date,p41Name,p32Name,p31Amount_WithoutVat_Orig,p31VatRate_Orig,p31Amount_WithVat_Orig,p31Text"
                    Case "p28-kusovnik"
                        c.j70Name = "Kusovníkové úkony klienta"
                        c.j70ColumnNames = "p31Date,p41Name,p32Name,p31Value_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p41", "p41-p31"
                        c.j70Name = My.Resources.common.VychoziPrehledProjektu
                        c.j70ColumnNames = "p31Date,Person,p34Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p41-approved"
                        c.j70Name = "Schválené úkony projektu"
                        c.j70ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"
                    Case "p41-time"
                        c.j70Name = "Hodiny projektu"
                        c.j70ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p41-expense", "p41-fee"
                        c.j70Name = "Peněžní úkonu projektu"
                        c.j70ColumnNames = "p31Date,Person,p34Name,p32Name,p31Amount_WithoutVat_Orig,p31VatRate_Orig,p31Amount_WithVat_Orig,p31Text"
                    Case "p41-kusovnik"
                        c.j70Name = "Kusovníkové úkony projektu"
                        c.j70ColumnNames = "p31Date,Person,p32Name,p31Value_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p56", "p56-p31"
                        c.j70Name = "Úkony vybraného úkolu"
                        c.j70ColumnNames = "p31Date,Person,p34Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p56-approved"

                        c.j70Name = "Schválené úkony úkolu"
                        c.j70ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"

                    Case "p56-time"
                        c.j70Name = "Hodiny v úkolu"
                        c.j70ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p56-expense", "p56-fee"
                        c.j70Name = "Peněžní úkony v úkolu"
                        c.j70ColumnNames = "p31Date,Person,p34Name,p32Name,p31Amount_WithoutVat_Orig,p31VatRate_Orig,p31Amount_WithVat_Orig,p31Text"
                    Case "p56-kusovnik"
                        c.j70Name = "Kusovníkové úkony v úkolu"
                        c.j70ColumnNames = "p31Date,Person,p32Name,p31Value_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"

                    Case "p91"
                        c.j70Name = My.Resources.common.VychoziPrehledFaktury
                        c.j70ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Invoiced,p31Rate_Billing_Invoiced,p31Amount_WithoutVat_Invoiced,p31VatRate_Invoiced,p31Amount_WithVat_Invoiced,p31Text"
                    Case "approving_step3"  'schvalovací rozhraní
                        c.j70Name = "Rozhraní pro schvalování úkonů | Příprava k fakturaci"
                        c.j70ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Orig,p31Amount_WithoutVat_Approved,p31Text"
                    Case "p31_grid"
                        c.j70Name = My.Resources.common.VychoziPrehled
                        c.j70ColumnNames = "p31Date,Person,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "mobile_grid"
                        c.j70ColumnNames = "p31Date,Person,p41Name,p31Value_Orig,p31Text"
                    Case Else
                        c.j70ColumnNames = "p31Date,Person,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                End Select

            Case BO.x29IdEnum.p41Project
                Select Case strMasterPrefix
                    Case "p31_framework"
                        c.j70Name = My.Resources.common.VychoziPrehledZapisovaniUkonu
                    Case "mobile_grid"
                        c.j70ColumnNames = "FullName"
                    Case "p28"
                        c.j70Name = "Projekty klienta"
                        c.j70ColumnNames = "p41Name,p41Code,p42Name"    'projekty v záložce pod klientem
                    Case "p41"
                        c.j70Name = "Pod-projekty"
                        c.j70ColumnNames = "p41Code,p41Name,p42Name"    'podřízené projekty
                End Select
                If c.j70ColumnNames = "" Then c.j70ColumnNames = "Client,p41Name"
            Case BO.x29IdEnum.p28Contact
                c.j70ColumnNames = "p28Name"
            Case BO.x29IdEnum.p91Invoice

                Select Case strMasterPrefix
                    Case "p41"
                        c.j70Name = "Výchozí přehled v detailu projektu"
                        c.j70ColumnNames = "p91Code,p91DateSupply,p91Amount_WithoutVat,p91Amount_Debt"
                    Case "p28"
                        c.j70Name = "Výchozí přehled v detailu klienta"
                        c.j70ColumnNames = "p91Code,p91DateSupply,p91Amount_WithoutVat,p91Amount_Debt"
                    Case "j02"
                        c.j70Name = "Výchozí přehled v detailu osoby"
                        c.j70ColumnNames = "p91Code,p28Name,p91DateSupply,p91Amount_WithoutVat,p91Amount_Debt"
                    Case "mobile_grid"
                        c.j70ColumnNames = "p91Code,p91Client,p91Amount_WithoutVat"
                    Case Else
                        c.j70ColumnNames = "p91Code,p28Name,p91Amount_WithoutVat,p91Amount_Debt"
                End Select
            Case BO.x29IdEnum.j02Person
                c.j70ColumnNames = "FullNameDesc"
            Case BO.x29IdEnum.p56Task
                Select Case strMasterPrefix
                    Case "j02"
                        c.j70Name = My.Resources.common.VychoziPrehledOsoby
                        c.j70ColumnNames = "p57Name,Client,p41Name,p56Name,p56PlanUntil,ReceiversInLine,Hours_Orig,Owner"
                    Case "p28"
                        c.j70Name = My.Resources.common.VychoziPrehledKlienta
                        c.j70ColumnNames = "p57Name,p41Name,p56Name,p56PlanUntil,ReceiversInLine,Hours_Orig,Owner"
                    Case "p31_framework"
                        c.j70Name = My.Resources.common.VychoziPrehledZapisovaniUkonu
                        c.j70ColumnNames = "p57Name,p56Name"
                    Case "mobile_grid"
                        c.j70Name = My.Resources.common.VychoziPrehled
                        c.j70ColumnNames = "p56Code,p56Name"
                    Case Else
                        c.j70ColumnNames = "p56Code,p56Name,b02Name"
                End Select
            Case BO.x29IdEnum.o23Notepad
                Select Case strMasterPrefix
                    Case "p41"
                        c.j70ColumnNames = "o24Name,o23Name"
                    Case "mobile_grid"
                        c.j70ColumnNames = "o24Name,o23Name"
                    Case Else
                        c.j70ColumnNames = "o24Name,o23Name,Project"
                End Select

        End Select
        c.j03ID = intJ03ID
        Return Save(c, Nothing, Nothing)
    End Function
End Class
