﻿Public Interface Ij74SavedGridColTemplateBL
    Inherits IFMother
    Function Save(cRec As BO.j74SavedGridColTemplate) As Boolean
    Function Load(intPID As Integer) As BO.j74SavedGridColTemplate
    Function LoadSystemTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "", Optional recState As BO.p31RecordState = BO.p31RecordState._NotExists) As BO.j74SavedGridColTemplate
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As IEnumerable(Of BO.j74SavedGridColTemplate)
    Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "", Optional recState As BO.p31RecordState = BO.p31RecordState._NotExists) As Boolean

    Function ColumnsPallete(x29id As BO.x29IdEnum) As List(Of BO.GridColumn)
    Function GroupByPallet(x29id As BO.x29IdEnum) As List(Of BO.GridGroupByColumn)
End Interface

Class j74SavedGridColTemplateBL
    Inherits BLMother
    Implements Ij74SavedGridColTemplateBL
    Private WithEvents _cDL As DL.j74SavedGridColTemplateDL
    Private _x29id As BO.x29IdEnum

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.j74SavedGridColTemplateDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    
    Public Function Delete(intPID As Integer) As Boolean Implements Ij74SavedGridColTemplateBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As System.Collections.Generic.IEnumerable(Of BO.j74SavedGridColTemplate) Implements Ij74SavedGridColTemplateBL.GetList
        Return _cDL.GetList(myQuery, _x29id)
    End Function


    Public Function Load(intPID As Integer) As BO.j74SavedGridColTemplate Implements Ij74SavedGridColTemplateBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadSystemTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "", Optional recState As BO.p31RecordState = BO.p31RecordState._NotExists) As BO.j74SavedGridColTemplate Implements Ij74SavedGridColTemplateBL.LoadSystemTemplate
        Dim cRec As BO.j74SavedGridColTemplate = _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix, recState)
        If cRec Is Nothing Then
            If Me.CheckDefaultTemplate(x29id, intJ03ID, strMasterPrefix, recState) Then
                cRec = _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix, recState)
            End If
        End If
        Return cRec
    End Function
    Public Function Save(cRec As BO.j74SavedGridColTemplate) As Boolean Implements Ij74SavedGridColTemplateBL.Save
        With cRec
            If .j03ID = 0 Then .j03ID = _cUser.PID
            If Trim(.j74Name) = "" Then _Error = "Chybí název šablony sloupců."
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí x29ID."
            If .j74ColumnNames = "" Then
                _Error = "Ve výběru musí být minimálně jeden sloupec."
            End If
        End With

        If _Error <> "" Then Return False

        Return _cDL.Save(cRec)

    End Function

    Public Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "", Optional recState As BO.p31RecordState = BO.p31RecordState._NotExists) As Boolean Implements Ij74SavedGridColTemplateBL.CheckDefaultTemplate
        If Not _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix, recState) Is Nothing Then Return True 'systém šablona již existuje

        Dim c As New BO.j74SavedGridColTemplate
        c.x29ID = x29id
        c.j74IsSystem = True
        c.j74Name = "Výchozí datový přehled"
        c.j74MasterPrefix = strMasterPrefix
        c.j74RecordState = recState
        If c.j74MasterPrefix = "" Or (x29id = BO.x29IdEnum.p31Worksheet And c.j74MasterPrefix = "p31_grid") Or c.j74MasterPrefix = "p31_framework" Then
            c.j74IsFilteringByColumn = True 'pro hlavní přehledy nahodit sloupcový auto-filter
        End If

        Select Case x29id
            Case BO.x29IdEnum.p31Worksheet
                Select Case strMasterPrefix
                    Case "j02"
                        c.j74Name = "Výchozí přehled v detailu osoby"
                        Select Case c.j74RecordState
                            Case BO.p31RecordState.Approved
                                c.j74Name = "Přehled schválených úkonů v detailu osoby"
                                c.j74ColumnNames = "p31Date,p28Name,p41Name,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Amount_WithoutVat_Approved,p31Text"
                            Case Else
                                c.j74ColumnNames = "p31Date,p28Name,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                        End Select

                    Case "p28"
                        c.j74Name = "Výchozí přehled v detailu klienta"
                        Select Case c.j74RecordState
                            Case BO.p31RecordState.Approved
                                c.j74Name = "Přehled schválených úkonů v detailu klienta"
                                c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"
                            Case Else
                                c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                        End Select

                    Case "p41"
                        c.j74Name = "Výchozí přehled v detailu projektu"
                        Select Case c.j74RecordState
                            Case BO.p31RecordState.Approved
                                c.j74Name = "Přehled schválených úkonů v detailu projektu"
                                c.j74ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"
                            Case Else
                                c.j74ColumnNames = "p31Date,Person,p34Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                        End Select
                    Case "p91"
                        c.j74Name = "Výchozí přehled v detailu faktury"
                        c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Invoiced,p31Rate_Billing_Invoiced,p31Amount_WithoutVat_Invoiced,p31VatRate_Invoiced,p31Amount_WithVat_Invoiced,p31Text"
                    Case "approving_step3"  'schvalovací rozhraní
                        c.j74Name = "Rozhraní pro schvalování úkonů | Příprava k fakturaci"
                        c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Orig,p31Amount_WithoutVat_Approved,p31Text,p31ApprovingSet"
                    Case "p31_grid"
                        c.j74Name = "Výchozí přehled"
                        c.j74ColumnNames = "p31Date,Person,p28Name,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case Else
                        c.j74ColumnNames = "p31Date,Person,p28Name,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                End Select

            Case BO.x29IdEnum.p41Project
                Select Case strMasterPrefix
                    Case "p31_framework"
                        c.j74Name = "Výchozí přehled v zapisování úkonů"
                End Select
                c.j74ColumnNames = "Client,p41Name"
            Case BO.x29IdEnum.p28Contact
                c.j74ColumnNames = "p28Name"
            Case BO.x29IdEnum.p91Invoice
                c.j74ColumnNames = "p91Code,p28Name,p91Amount_WithoutVat,p91Amount_Debt"
            Case BO.x29IdEnum.j02Person
                c.j74ColumnNames = "FullNameDesc"
            Case BO.x29IdEnum.p56Task
                Select Case strMasterPrefix
                    Case "j02"
                        c.j74Name = "Výchozí přehled v detailu osoby"
                        c.j74ColumnNames = "p57Name,Client,p41Name,p56Name,p56PlanUntil,ReceiversInLine,Hours_Orig,Owner"
                    Case "p28"
                        c.j74Name = "Výchozí přehled v detailu klienta"
                        c.j74ColumnNames = "p57Name,p41Name,p56Name,p56PlanUntil,ReceiversInLine,Hours_Orig,Owner"
                    Case "p31_framework"
                        c.j74Name = "Výchozí přehled v zapisování úkonů"
                        c.j74ColumnNames = "p57Name,p56Name"
                    Case Else
                        c.j74ColumnNames = "p56Code,p56Name,b02Name"
                End Select
            Case BO.x29IdEnum.o23Notepad
                c.j74ColumnNames = "o24Name,o23Name,Project"
        End Select
        c.j03ID = intJ03ID
        Return Save(c)
    End Function

    Public Function GetColumns(x29id As BO.x29IdEnum) As List(Of BO.GridColumn) Implements Ij74SavedGridColTemplateBL.ColumnsPallete
        _x29id = x29id
        Dim lis As New List(Of BO.GridColumn)
        
        Select Case _x29id
            Case BO.x29IdEnum.p31Worksheet
                InhaleP31ColList(lis)
            Case BO.x29IdEnum.p41Project
                InhaleP41ColList(lis)
            Case BO.x29IdEnum.p28Contact
                InhaleP28ColList(lis)
            Case BO.x29IdEnum.p91Invoice
                InhaleP91ColList(lis)
            Case BO.x29IdEnum.j02Person
                InhaleJ02ColList(lis)
            Case BO.x29IdEnum.p56Task
                InhaleP56ColList(lis)
            Case BO.x29IdEnum.o23Notepad
                InhaleO23ColList(lis)
        End Select

        Return lis


    End Function

    Private Sub AppendFreeFields(x29id As BO.x29IdEnum, ByRef lis As List(Of BO.GridColumn))
        Dim lisX28 As IEnumerable(Of BO.x28EntityField) = Factory.x28EntityFieldBL.GetList(x29id, -1)
        For Each c In lisX28
            If c.x23ID = 0 Then
                Select Case c.x24ID
                    Case BO.x24IdENUM.tString
                        lis.Add(AGC(c.x28Name, c.x28Field, BO.cfENUM.AnyString))
                    Case BO.x24IdENUM.tDate
                        lis.Add(AGC(c.x28Name, c.x28Field, BO.cfENUM.DateOnly))
                    Case BO.x24IdENUM.tDateTime
                        lis.Add(AGC(c.x28Name, c.x28Field, BO.cfENUM.DateTime))
                    Case BO.x24IdENUM.tDecimal
                       lis.Add(AGC(c.x28Name, c.x28Field, BO.cfENUM.Numeric))
                    Case BO.x24IdENUM.tInteger
                        lis.Add(AGC(c.x28Name, c.x28Field, BO.cfENUM.Numeric0))
                    Case BO.x24IdENUM.tBoolean
                        lis.Add(AGC(c.x28Name, c.x28Field, BO.cfENUM.Checkbox))
                End Select
            Else
                'text combo položky
                lis.Add(AGC(c.x28Name, c.x28Field & "Text", BO.cfENUM.AnyString))
            End If
        Next
    End Sub

    Private Sub InhaleP41ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Název projektu", "p41Name"))
            .Add(AGC("Kód", "p41Code"))
            .Add(AGC("DRAFT", "p41IsDraft", BO.cfENUM.Checkbox))
            .Add(AGC("Klient projektu", "Client"))
            .Add(AGC("Klient+projekt", "FullName", , False))
            .Add(AGC("Zkratka", "p41NameShort"))
            .Add(AGC("Typ projektu", "p42Name"))
            .Add(AGC("Středisko", "j18Name"))
            .Add(AGC("Fakturační ceník", "p51Name_Billing"))
            .Add(AGC("Ceník nákladových sazeb", "p51Name_Internal"))
            .Add(AGC("Typ faktury", "p92Name"))
            .Add(AGC("Plánované zahájení", "p41PlanFrom", BO.cfENUM.DateOnly))
            .Add(AGC("Plánované dokončení", "p41PlanUntil", BO.cfENUM.DateOnly))
            .Add(AGC("Limit hodin", "p41LimitHours_Notification", BO.cfENUM.Numeric))
            .Add(AGC("Limitní honorář", "p41LimitFee_Notification", BO.cfENUM.Numeric))

            .Add(AGC("Vlastník záznamu", "Owner"))
            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p41Project, lis)

    End Sub
    Private Sub InhaleP28ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Název", "p28Name"))
            .Add(AGC("Společnost", "p28CompanyName"))
            .Add(AGC("Kód", "p28Code"))
            .Add(AGC("Kód dodavatele", "p28SupplierID"))
            .Add(AGC("IČ", "p28RegID"))
            .Add(AGC("DIČ", "p28VatID"))
            .Add(AGC("Typ", "p29Name"))
            .Add(AGC("Zkratka", "p28CompanyShortName"))
            .Add(AGC("Fakturační ceník", "p51Name_Billing"))
            .Add(AGC("Ceník nákladových sazeb", "p51Name_Internal"))
            .Add(AGC("Typ faktury", "p92Name"))
            .Add(AGC("Fakt.jazyk", "p87Name"))

            .Add(AGC("Vlastník záznamu", "Owner"))
            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p28Contact, lis)
    End Sub
    Private Sub InhaleJ02ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Příjmení+jméno", "FullNameDesc"))
            .Add(AGC("Jméno+příjmení", "FullNameAsc"))
            .Add(AGC("Jméno", "j02FirstName"))
            .Add(AGC("Příjmení", "j02LastName"))
            .Add(AGC("Titul", "j02TitleBeforeName"))
            .Add(AGC("E-mail", "j02Email"))
            .Add(AGC("Pozice", "j07Name"))
            .Add(AGC("Fond", "c21Name"))
            .Add(AGC("Středisko", "j18Name"))

            .Add(AGC("Interní osoba", "j02IsIntraPerson", BO.cfENUM.Checkbox))
            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.j02Person, lis)
    End Sub
    Private Sub InhaleP31ColList(ByRef lis As List(Of BO.GridColumn))
        Dim bolHideRatesColumns As Boolean = Not Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates)   'zda uživatel nemá právo vidět sazby a fakturační údaje

        With lis
            .Add(AGC("Datum", "p31Date", BO.cfENUM.DateOnly))
            .Add(AGC("Čas od", "TimeFrom", , False))
            .Add(AGC("Čas do", "TimeUntil", , False))

            .Add(AGC("Osoba", "Person"))
            .Add(AGC("Aktivita", "p32Name"))
            .Add(AGC("FA", "p32IsBillable", BO.cfENUM.Checkbox))
            .Add(AGC("Sešit", "p34Name"))

            .Add(AGC("Název projektu", "p41Name"))
            .Add(AGC("Klient projektu", "p28Name"))
            .Add(AGC("Název úkolu", "p56Name"))
            .Add(AGC("Kód úkolu", "p56Code"))

            .Add(AGC("Text", "p31Text"))


            .Add(AGC("Schváleno", "p71Name"))

            .Add(AGC("Vykázaná hodnota", "p31Value_Orig", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vykázané hodiny", "p31Hours_Orig", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vykázáno HH:mm", "p31HHMM_Orig", , , "p31Hours_Orig"))
            If Not bolHideRatesColumns Then
                .Add(AGC("Výchozí sazba", "p31Rate_Billing_Orig", BO.cfENUM.Numeric2))
                .Add(AGC("Částka bez DPH", "p31Amount_WithoutVat_Orig", BO.cfENUM.Numeric2, , , True))
                .Add(AGC("Nákladová sazba", "p31Rate_Internal_Orig", BO.cfENUM.Numeric2))
                .Add(AGC("Nákladová částka", "p31Amount_Internal", BO.cfENUM.Numeric2, , , True))
            End If

            .Add(AGC("Sazba DPH", "p31VatRate_Orig", BO.cfENUM.Numeric0))
            .Add(AGC("Měna", "j27Code_Billing_Orig"))

            .Add(AGC("Návrh fakt.statusu", "approve_p72Name", , , "p72approve.j72Name"))
            .Add(AGC("Schválená hodnota", "p31Value_Approved_Billing", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Schválené hodiny", "p31Hours_Approved_Billing", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Schváleno HH:mm", "p31HHMM_Approved_Billing"))
            .Add(AGC("Schválená sazba", "p31Rate_Billing_Approved", BO.cfENUM.Numeric2))
            .Add(AGC("Schváleno interně", "p31Value_Approved_Internal", BO.cfENUM.Numeric2, , , True))
            If Not bolHideRatesColumns Then .Add(AGC("Schváleno bez DPH", "p31Amount_WithoutVat_Approved", BO.cfENUM.Numeric2, , , True))
            If Not bolHideRatesColumns Then .Add(AGC("Schváleno vč.DPH", "p31Amount_WithVat_Approved", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Kdy schváleno", "p31Approved_When", BO.cfENUM.DateTime))


            .Add(AGC("Vyfakt.hodnota", "p31Value_Invoiced", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vyfakt.hodiny", "p31Hours_Invoiced", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vyfakt.HH:mm", "p31HHMM_Invoiced"))
            If Not bolHideRatesColumns Then
                .Add(AGC("Vyfakt.sazba", "p31Rate_Billing_Invoiced", BO.cfENUM.Numeric2))
                .Add(AGC("Vyfakt.bez DPH", "p31Amount_WithoutVat_Invoiced", BO.cfENUM.Numeric2, , , True))
                .Add(AGC("Vyfakt.vč.DPH", "p31Amount_WithVat_Invoiced", BO.cfENUM.Numeric2, , , True))
            End If


            .Add(AGC("Vyfakt.sazba DPH", "p31VatRate_Invoiced", BO.cfENUM.Numeric0))

            If Not bolHideRatesColumns Then .Add(AGC("Vyfakt.bez DPH CZK", "p31Amount_WithoutVat_Invoiced_Domestic", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Faktura", "p91Code"))
            .Add(AGC("Fakt.oddíl", "p95Name"))
            .Add(AGC("Billing dávka", "p31ApprovingSet"))

            ''.Add(AGC("Je plán", "p31IsPlanRecord", BO.cfENUM.Checkbox))

            .Add(AGC("Kalk/počet", "p31Calc_Pieces", BO.cfENUM.Numeric2))
            .Add(AGC("Kalk/cena 1 ks", "p31Calc_PieceAmount", BO.cfENUM.Numeric2))

            .Add(AGC("Vlastník záznamu", "Owner", , False))
            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p31Worksheet, lis)
    End Sub
    Private Sub InhaleP91ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Číslo", "p91Code"))

            .Add(AGC("Klient", "p28Name"))
            .Add(AGC("Měna", "j27Code"))
            .Add(AGC("Typ faktury", "p92Name"))
            .Add(AGC("Projekt", "p41Name"))
            .Add(AGC("Stát", "j17Name"))

            .Add(AGC("Bez dph", "p91Amount_WithoutVat", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Dluh", "p91Amount_Debt", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Celkem", "p91Amount_TotalDue", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Celk.dph", "p91Amount_Vat", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Bez dph CZK", "WithoutVat_CZK", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Bez dph EUR", "WithoutVat_EUR", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Dluh CZK", "Debt_CZK", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Dluh EUR", "Debt_EUR", BO.cfENUM.Numeric, , , True))

            .Add(AGC("Datum", "p91Date", BO.cfENUM.DateOnly))
            .Add(AGC("Plnění", "p91DateSupply", BO.cfENUM.DateOnly))
            .Add(AGC("Splatnost", "p91DateMaturity", BO.cfENUM.DateOnly))

            .Add(AGC("Text", "p91Text1"))

            .Add(AGC("Vlastník záznamu", "Owner"))
            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p91Invoice, lis)
    End Sub

    Private Sub InhaleP56ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Kód", "p56Code"))
            .Add(AGC("Typ", "p57Name"))
            .Add(AGC("Název", "p56Name"))
            .Add(AGC("Aktuální stav", "b02Name"))            
            .Add(AGC("Klient", "Client"))
            .Add(AGC("Projekt", "p41Name"))
            .Add(AGC("Kód projektu", "p41Code"))
            .Add(AGC("Příjemci", "ReceiversInLine"))
            .Add(AGC("Vlastník", "Owner"))
            .Add(AGC("Termín", "p56PlanUntil", BO.cfENUM.DateTime))
            .Add(AGC("Plánované zahájení", "p56PlanFrom", BO.cfENUM.DateTime))
            .Add(AGC("Hotovo%", "p56CompletePercent", BO.cfENUM.Numeric0))
            .Add(AGC("Produkt", "p58Name"))
            .Add(AGC("Priorita zadavatele", "p59NameSubmitter"))

            .Add(AGC("Hodnocení", "p56RatingValue", BO.cfENUM.Numeric0))
            .Add(AGC("Připomenutí", "p56ReminderDate", BO.cfENUM.DateTime))

            .Add(AGC("Plán hodin", "p56Plan_Hours", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Plán výdajů", "p56Plan_Expenses", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vykázané hodiny", "Hours_Orig", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vykázané výdaje", "Expenses_Orig", BO.cfENUM.Numeric2, , , True))

            
            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p56Task, lis)
    End Sub
    Private Sub InhaleO23ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Typ dokumentu", "o24Name"))
            .Add(AGC("Název", "o23Name"))
            .Add(AGC("Kód dokumentu", "o23Code"))
            .Add(AGC("Aktuální stav", "b02Name"))
            .Add(AGC("Projekt", "Project"))
            .Add(AGC("Klient projektu", "ProjectClient"))
            .Add(AGC("Firma", "p28Name"))
            .Add(AGC("Faktura", "p91Code"))
            .Add(AGC("Úkol", "p56Code"))
            .Add(AGC("Příjemci", "ReceiversInLine"))
            .Add(AGC("Vlastník", "Owner"))
            .Add(AGC("Datum", "o23Date", BO.cfENUM.DateTime))
            .Add(AGC("Připomenutí", "o23ReminderDate", BO.cfENUM.DateTime))

            .Add(AGC("Založeno", "DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "UserInsert"))
            .Add(AGC("Aktualizace", "DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.o23Notepad, lis)
    End Sub



    Private Function AGC(strHeader As String, strName As String, Optional colType As BO.cfENUM = BO.cfENUM.AnyString, Optional bolSortable As Boolean = True, Optional strDBName As String = "", Optional bolShowTotals As Boolean = False)
        Dim col As BO.GridColumn
        col = New BO.GridColumn(_x29id, strHeader, strName, colType)
        col.IsSortable = bolSortable
        col.ColumnDBName = strDBName
        col.IsShowTotals = bolShowTotals
        Return col
    End Function

    Public Function GroupByPallet(x29id As BO.x29IdEnum) As List(Of BO.GridGroupByColumn) Implements Ij74SavedGridColTemplateBL.GroupByPallet
        Dim lis As New List(Of BO.GridGroupByColumn)
        lis.Add(New BO.GridGroupByColumn("Bez souhrnů", ""))
        Select Case x29id
            Case BO.x29IdEnum.p41Project
                lis.Add(New BO.GridGroupByColumn("Klient", "Client"))
                lis.Add(New BO.GridGroupByColumn("Typ projektu", "p42Name"))
                lis.Add(New BO.GridGroupByColumn("Středisko", "j18Name"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "p41IsDraft"))
            Case BO.x29IdEnum.p28Contact
                lis.Add(New BO.GridGroupByColumn("Typ klienta", "p29Name"))
                lis.Add(New BO.GridGroupByColumn("Typ faktury", "p92Name"))
                lis.Add(New BO.GridGroupByColumn("Fakturační jazyk", "p87Name"))
                lis.Add(New BO.GridGroupByColumn("Vlastník klienta", "Owner"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "p28IsDraft"))
            Case BO.x29IdEnum.o23Notepad
                lis.Add(New BO.GridGroupByColumn("Typ dokumentu", "o24Name"))
                lis.Add(New BO.GridGroupByColumn("Klient", "p28Name"))
                lis.Add(New BO.GridGroupByColumn("Projekt", "Project"))
                lis.Add(New BO.GridGroupByColumn("Aktuální stav", "b02Name"))
                lis.Add(New BO.GridGroupByColumn("Vlastník dokumentu", "Owner"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "o23IsDraft"))
            Case BO.x29IdEnum.p31Worksheet
                lis.Add(New BO.GridGroupByColumn("Sešit", "p34Name"))
                lis.Add(New BO.GridGroupByColumn("Aktivita", "p32Name"))
                lis.Add(New BO.GridGroupByColumn("Osoba", "Person"))
                lis.Add(New BO.GridGroupByColumn("Klient projektu", "p28Name"))
                lis.Add(New BO.GridGroupByColumn("Projekt", "p41Name"))
                lis.Add(New BO.GridGroupByColumn("Faktura", "p91Code"))
                lis.Add(New BO.GridGroupByColumn("Schváleno", "p71Name"))
                lis.Add(New BO.GridGroupByColumn("Fakt.status", "p70Name"))
                lis.Add(New BO.GridGroupByColumn("Billing dávka", "p31ApprovingSet"))
            Case BO.x29IdEnum.p56Task
                lis.Add(New BO.GridGroupByColumn("Typ úkolu", "p57Name"))
                lis.Add(New BO.GridGroupByColumn("Klient", "Client"))
                lis.Add(New BO.GridGroupByColumn("Projekt", "ProjectCodeAndName"))
                lis.Add(New BO.GridGroupByColumn("Aktuální stav", "b02Name"))
                lis.Add(New BO.GridGroupByColumn("Produkt", "p58Name"))
                lis.Add(New BO.GridGroupByColumn("Priorita zadavatele", "p59NameSubmitter"))
                lis.Add(New BO.GridGroupByColumn("Příjemce", "ReceiversInLine"))
                lis.Add(New BO.GridGroupByColumn("Vlastník úkolu", "Owner"))
            Case BO.x29IdEnum.j02Person
                lis.Add(New BO.GridGroupByColumn("Pozice", "j07Name"))
                lis.Add(New BO.GridGroupByColumn("Pracovní fond", "c21Name"))
                lis.Add(New BO.GridGroupByColumn("Středisko", "j18Name"))
                lis.Add(New BO.GridGroupByColumn("Interní osoba", "j02IsIntraPerson"))
            Case BO.x29IdEnum.p91Invoice
                lis.Add(New BO.GridGroupByColumn("Klient", "p28Name"))
                lis.Add(New BO.GridGroupByColumn("Měna", "j27Code"))
                lis.Add(New BO.GridGroupByColumn("Typ faktury", "p92Name"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "p91IsDraft"))
        End Select
        Return lis
    End Function
End Class
