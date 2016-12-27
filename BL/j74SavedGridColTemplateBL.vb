Public Interface Ij74SavedGridColTemplateBL
    Inherits IFMother
    Function Save(cRec As BO.j74SavedGridColTemplate, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
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
    Public Function Save(cRec As BO.j74SavedGridColTemplate, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean Implements Ij74SavedGridColTemplateBL.Save
        With cRec
            If .j03ID = 0 Then .j03ID = _cUser.PID
            If Trim(.j74Name) = "" Then _Error = "Chybí název šablony sloupců."
            If .x29ID = BO.x29IdEnum._NotSpecified Then _Error = "Chybí x29ID."
            If .j74ColumnNames = "" Then
                _Error = "Ve výběru musí být minimálně jeden sloupec."
            End If
        End With

        If _Error <> "" Then Return False

        Return _cDL.Save(cRec, lisX69)

    End Function

    Public Function CheckDefaultTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "", Optional recState As BO.p31RecordState = BO.p31RecordState._NotExists) As Boolean Implements Ij74SavedGridColTemplateBL.CheckDefaultTemplate
        If Not _cDL.LoadSystemTemplate(x29id, intJ03ID, strMasterPrefix, recState) Is Nothing Then Return True 'systém šablona již existuje

        Dim c As New BO.j74SavedGridColTemplate
        c.x29ID = x29id
        c.j74IsSystem = True
        c.j74Name = BL.My.Resources.common.VychoziDatovyPrehled
        c.j74MasterPrefix = strMasterPrefix
        c.j74RecordState = recState
        If c.j74MasterPrefix = "" Or (x29id = BO.x29IdEnum.p31Worksheet And c.j74MasterPrefix = "p31_grid") Or c.j74MasterPrefix = "p31_framework" Then
            c.j74IsFilteringByColumn = True 'pro hlavní přehledy nahodit sloupcový auto-filter
            c.j74ScrollingFlag = BO.j74ScrollingFlagENUM.StaticHeaders    'pro hlavní přehledy nastavit ukotvení záhlaví
        End If

        Select Case x29id
            Case BO.x29IdEnum.p31Worksheet
                Select Case strMasterPrefix
                    Case "j02"
                        c.j74Name = String.Format(BL.My.Resources.common.VychoziPrehledOsoby, "osoby")
                        Select Case c.j74RecordState
                            Case BO.p31RecordState.Approved
                                c.j74Name = "Přehled schválených úkonů v detailu osoby"
                                c.j74ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Amount_WithoutVat_Approved,p31Text"
                            Case Else
                                c.j74ColumnNames = "p31Date,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                        End Select

                    Case "p28"
                        c.j74Name = My.Resources.common.VychoziPrehledKlienta
                        Select Case c.j74RecordState
                            Case BO.p31RecordState.Approved
                                c.j74Name = "Přehled schválených úkonů v detailu klienta"
                                c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"
                            Case Else
                                c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                        End Select

                    Case "p41"
                        c.j74Name = My.Resources.common.VychoziPrehledProjektu
                        Select Case c.j74RecordState
                            Case BO.p31RecordState.Approved
                                c.j74Name = "Přehled schválených úkonů v detailu projektu"
                                c.j74ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Amount_WithoutVat_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Approved,p31Amount_WithoutVat_Approved,p31Text"
                            Case Else
                                c.j74ColumnNames = "p31Date,Person,p34Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                        End Select
                    Case "p41-time"
                        c.j74Name = "Hodiny v projektu"
                        c.j74ColumnNames = "p31Date,Person,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p41-expense", "p41-fee"
                        c.j74Name = "Peněžní výdaje nebo paušály v projektu"
                        c.j74ColumnNames = "p31Date,Person,p34Name,p32Name,p31Amount_WithoutVat_Orig,p31VatRate_Orig,p31Amount_WithVat_Orig,p31Text"
                    Case "p41-kusovnik"
                        c.j74Name = "Kusovník v projektu"
                        c.j74ColumnNames = "p31Date,Person,p32Name,p31Value_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "p91"
                        c.j74Name = My.Resources.common.VychoziPrehledFaktury
                        c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Invoiced,p31Rate_Billing_Invoiced,p31Amount_WithoutVat_Invoiced,p31VatRate_Invoiced,p31Amount_WithVat_Invoiced,p31Text"
                    Case "approving_step3"  'schvalovací rozhraní
                        c.j74Name = "Rozhraní pro schvalování úkonů | Příprava k fakturaci"
                        c.j74ColumnNames = "p31Date,Person,p41Name,p32Name,p31Hours_Orig,p31Hours_Approved_Billing,p31Rate_Billing_Orig,p31Amount_WithoutVat_Approved,p31Text"
                    Case "p31_grid"
                        c.j74Name = My.Resources.common.VychoziPrehled
                        c.j74ColumnNames = "p31Date,Person,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                    Case "mobile_grid"
                        c.j74ColumnNames = "p31Date,Person,p41Name,p31Value_Orig,p31Text"
                    Case Else
                        c.j74ColumnNames = "p31Date,Person,ClientName,p41Name,p32Name,p31Hours_Orig,p31Rate_Billing_Orig,p31Amount_WithoutVat_Orig,p31Text"
                End Select

            Case BO.x29IdEnum.p41Project
                Select Case strMasterPrefix
                    Case "p31_framework"
                        c.j74Name = My.Resources.common.VychoziPrehledZapisovaniUkonu
                    Case "mobile_grid"
                        c.j74ColumnNames = "FullName"
                    Case "p28"
                        c.j74Name = "Projekty klienta"
                        c.j74ColumnNames = "p41Name,p41Code,p42Name"    'projekty v záložce pod klientem
                    Case "p41"
                        c.j74Name = "Pod-projekty"
                        c.j74ColumnNames = "p41Code,p41Name,p42Name"    'podřízené projekty
                End Select
                If c.j74ColumnNames = "" Then c.j74ColumnNames = "Client,p41Name"
            Case BO.x29IdEnum.p28Contact
                c.j74ColumnNames = "p28Name"
            Case BO.x29IdEnum.p91Invoice

                Select Case strMasterPrefix
                    Case "p41"
                        c.j74Name = "Výchozí přehled v detailu projektu"
                        c.j74ColumnNames = "p91Code,p91DateSupply,p91Amount_WithoutVat,p91Amount_Debt"
                    Case "p28"
                        c.j74Name = "Výchozí přehled v detailu klienta"
                        c.j74ColumnNames = "p91Code,p91DateSupply,p91Amount_WithoutVat,p91Amount_Debt"
                    Case "j02"
                        c.j74Name = "Výchozí přehled v detailu osoby"
                        c.j74ColumnNames = "p91Code,p28Name,p91DateSupply,p91Amount_WithoutVat,p91Amount_Debt"
                    Case "mobile_grid"
                        c.j74ColumnNames = "p91Code,p91Client,p91Amount_WithoutVat"
                    Case Else
                        c.j74ColumnNames = "p91Code,p28Name,p91Amount_WithoutVat,p91Amount_Debt"
                End Select
            Case BO.x29IdEnum.j02Person
                c.j74ColumnNames = "FullNameDesc"
            Case BO.x29IdEnum.p56Task
                Select Case strMasterPrefix
                    Case "j02"
                        c.j74Name = My.Resources.common.VychoziPrehledOsoby
                        c.j74ColumnNames = "p57Name,Client,p41Name,p56Name,p56PlanUntil,ReceiversInLine,Hours_Orig,Owner"
                    Case "p28"
                        c.j74Name = My.Resources.common.VychoziPrehledKlienta
                        c.j74ColumnNames = "p57Name,p41Name,p56Name,p56PlanUntil,ReceiversInLine,Hours_Orig,Owner"
                    Case "p31_framework"
                        c.j74Name = My.Resources.common.VychoziPrehledZapisovaniUkonu
                        c.j74ColumnNames = "p57Name,p56Name"
                    Case "mobile_grid"
                        c.j74Name = My.Resources.common.VychoziPrehled
                        c.j74ColumnNames = "p56Code,p56Name"
                    Case Else
                        c.j74ColumnNames = "p56Code,p56Name,b02Name"
                End Select
            Case BO.x29IdEnum.o23Notepad
                Select Case strMasterPrefix
                    Case "p41"
                        c.j74ColumnNames = "o24Name,o23Name"
                    Case "mobile_grid"
                        c.j74ColumnNames = "o24Name,o23Name"
                    Case Else
                        c.j74ColumnNames = "o24Name,o23Name,Project"
                End Select

        End Select
        c.j03ID = intJ03ID
        Return Save(c, Nothing)
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
        Dim lisX28 As IEnumerable(Of BO.x28EntityField) = Factory.x28EntityFieldBL.GetList(x29id, -1, True)
        For Each c In lisX28
            If c.x28Flag = BO.x28FlagENUM.UserField Then
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
            End If
            If c.x28Flag = BO.x28FlagENUM.GridField Then
                Dim col As New BO.GridColumn(c.x29ID, c.x28Name, c.x28Grid_Field)
                col.ColumnDBName = c.x28Grid_SqlSyntax
                col.SqlSyntax_FROM = c.x28Grid_SqlFrom
                Select Case c.x24ID
                    Case BO.x24IdENUM.tDate : col.ColumnType = BO.cfENUM.DateOnly
                    Case BO.x24IdENUM.tDateTime : col.ColumnType = BO.cfENUM.DateTime
                    Case BO.x24IdENUM.tDecimal : col.ColumnType = BO.cfENUM.Numeric
                    Case BO.x24IdENUM.tInteger : col.ColumnType = BO.cfENUM.Numeric0
                    Case BO.x24IdENUM.tBoolean : col.ColumnType = BO.cfENUM.Checkbox
                End Select
                lis.Add(col)
            End If
        Next
    End Sub

    Private Sub InhaleP41ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC(My.Resources.common.NazevProjektu, "p41Name", , , "a.p41Name"))
            .Add(AGC(My.Resources.common.Kod, "p41Code", , , "a.p41Code"))
            .Add(AGC("Stromový název", "p41TreePath", , True, "a.p41TreePath"))
            ''.Add(AGC("Strom název", "TreeName", , False, "Replicate('.',a.p41TreeLevel*4)+a.p41Name"))
            .Add(AGC("DRAFT", "p41IsDraft", BO.cfENUM.Checkbox, , "a.p41IsDraft"))
            .Add(AGC(My.Resources.common.KlientProjektu, "Client", , , "p28client.p28Name"))
            .Add(AGC(My.Resources.common.KlientPlusProjekt, "FullName", , True, "isnull(p28client.p28Name+char(32),'')+a.p41Name"))
            .Add(AGC(My.Resources.common.Zkratka, "p41NameShort", , , "a.p41NameShort"))
            .Add(AGC(My.Resources.common.TypProjektu, "p42Name"))
            .Add(AGC(My.Resources.common.Stredisko, "j18Name"))
            .Add(AGC(My.Resources.common.FakturacniCenik, "p51Name_Billing", , , "p51billing.p51Name"))
            .Add(AGC(My.Resources.common.NakladovyCenik, "p51Name_Internal", , , "p51internal.p51Name", , "LEFT OUTER JOIN p51PriceList p51internal ON a.p51ID_Internal=p51internal.p51ID"))
            .Add(AGC(My.Resources.common.TypFaktury, "p92Name"))
            .Add(AGC("Fakturační poznámka", "p41BillingMemo", , , "a.p41BillingMemo"))


            
            .Add(AGC(My.Resources.common.PlanStart, "p41PlanFrom", BO.cfENUM.DateOnly, , "a.p41PlanFrom"))
            .Add(AGC(My.Resources.common.PlanEnd, "p41PlanUntil", BO.cfENUM.DateOnly, , "a.p41PlanUntil"))
            .Add(AGC(My.Resources.common.LimitHodin, "p41LimitHours_Notification", BO.cfENUM.Numeric, , "a.p41LimitHours_Notification", True))
            .Add(AGC(My.Resources.common.LimitniHonorar, "p41LimitFee_Notification", BO.cfENUM.Numeric, , "a.p41LimitFee_Notification", True))

            .Add(AGC("Odběratel faktury", "InvoiceClient", , , "p28billing.p28Name", , "LEFT OUTER JOIN p28Contact p28billing ON a.p28ID_Billing=p28billing.p28ID"))

            .Add(AGC(My.Resources.common.VlastnikZaznamu, "Owner", , , "j02owner.j02LastName+char(32)+j02owner.j02FirstName"))
            .Add(AGC("Strom index", "p41TreeIndex", , True, "a.p41TreeIndex"))
            .Add(AGC("Strom level", "p41TreeLevel", , True, "a.p41TreeLevel"))
            .Add(AGC(My.Resources.common.Zalozeno, "p41DateInsert", BO.cfENUM.DateTime, , "a.p41DateInsert"))
            .Add(AGC(My.Resources.common.Zalozil, "p41UserInsert", , , "a.p41UserInsert"))
            .Add(AGC(My.Resources.common.Aktualizace, "p41DateUpdate", BO.cfENUM.DateTime, , "a.p41DateUpdate"))
            .Add(AGC(My.Resources.common.Aktualizoval, "p41UserUpdate", , , "a.p41UserUpdate"))
            .Add(AGC(My.Resources.common.ExterniKod, "p41ExternalPID", , , "a.p41ExternalPID"))
            If Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                .Add(AGC("Vykázané hodiny", "Vykazano_Hodiny", BO.cfENUM.Numeric2, , "view_p41_worksheet.Vykazano_Hodiny", True, "LEFT OUTER JOIN view_p41_worksheet ON a.p41ID=view_p41_worksheet.p41ID"))
                .Add(AGC("FAKTURA/hodiny", "Vyfakturovano_Hodiny", BO.cfENUM.Numeric2, , "view_p41_worksheet.Vyfakturovano_Hodiny", True, "LEFT OUTER JOIN view_p41_worksheet ON a.p41ID=view_p41_worksheet.p41ID"))
                .Add(AGC("FAKTURA/částka", "Vyfakturovano_Castka", BO.cfENUM.Numeric2, , "view_p41_worksheet.Vyfakturovano_Celkem_Domestic", True, "LEFT OUTER JOIN view_p41_worksheet ON a.p41ID=view_p41_worksheet.p41ID"))
                .Add(AGC("WIP/Hodiny", "WIP_Hodiny", BO.cfENUM.Numeric2, , "view_p41_wip.Hodiny", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Částka", "WIP_Castka", BO.cfENUM.Numeric2, , "view_p41_wip.Castka_Celkem", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Honorář", "WIP_Honorar", BO.cfENUM.Numeric2, , "view_p41_wip.Honorar", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Honorář CZK", "WIP_Honorar_CZK", BO.cfENUM.Numeric2, , "view_p41_wip.Honorar_CZK", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Honorář EUR", "WIP_Honorar_EUR", BO.cfENUM.Numeric2, , "view_p41_wip.Honorar_EUR", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Výdaje", "WIP_Vydaje", BO.cfENUM.Numeric2, , "view_p41_wip.Vydaje", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Výdaje CZK", "WIP_Vydaje_CZK", BO.cfENUM.Numeric2, , "view_p41_wip.Vydaje_CZK", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Výdaje EUR", "WIP_Vydaje_EUR", BO.cfENUM.Numeric2, , "view_p41_wip.Vydaje_EUR", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Odměny", "WIP_Odmeny", BO.cfENUM.Numeric2, , "view_p41_wip.Odmeny", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Odměny CZK", "WIP_Odmeny_CZK", BO.cfENUM.Numeric2, , "view_p41_wip.Odmeny_CZK", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
                .Add(AGC("WIP/Odměny EUR", "WIP_Odmeny_EUR", BO.cfENUM.Numeric2, , "view_p41_wip.Odmeny_EUR", True, "LEFT OUTER JOIN view_p41_wip ON a.p41ID=view_p41_wip.p41ID"))
            End If
        End With
        AppendFreeFields(BO.x29IdEnum.p41Project, lis)

    End Sub
    Private Sub InhaleP28ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC(My.Resources.common.Nazev, "p28Name", , , "a.p28Name"))
            .Add(AGC(My.Resources.common.Spolecnost, "p28CompanyName", , , "a.p28CompanyName"))
            .Add(AGC(My.Resources.common.Kod, "p28Code", , , "a.p28Code"))
            .Add(AGC(My.Resources.common.KodDodavatele, "p28SupplierID", , , "a.p28SupplierID"))
            .Add(AGC(My.Resources.common.IC, "p28RegID", , , "a.p28RegID"))
            .Add(AGC(My.Resources.common.DIC, "p28VatID", , , "a.p28VatID"))
            .Add(AGC(My.Resources.common.Typ, "p29Name"))
            .Add(AGC("Fakturační poznámka", "p28BillingMemo", , , "a.p28BillingMemo"))
            .Add(AGC("Stromový název", "p28TreePath", , True, "a.p28TreePath"))
            ''.Add(AGC("Nadřízený klient", "ParentContact", , , "p28parent.p28Name", , "LEFT OUTER JOIN p28Contact p28parent ON a.p28ParentID=p28parent.p28ID"))
            .Add(AGC("Město", "Adress1_City", , , "pa.o38City", , "LEFT OUTER JOIN view_PrimaryAddress pa ON a.p28ID=pa.p28ID"))
            .Add(AGC("Ulice", "Adress1_Street", , , "pa.o38Street", , "LEFT OUTER JOIN view_PrimaryAddress pa ON a.p28ID=pa.p28ID"))
            .Add(AGC("PSČ", "Adress1_ZIP", , , "pa.o38ZIP", , "LEFT OUTER JOIN view_PrimaryAddress pa ON a.p28ID=pa.p28ID"))
            .Add(AGC("Stát", "Adress1_Country", , , "pa.o38Country", , "LEFT OUTER JOIN view_PrimaryAddress pa ON a.p28ID=pa.p28ID"))
            .Add(AGC(My.Resources.common.Zkratka, "p28CompanyShortName", , , "a.p28CompanyShortName"))
            .Add(AGC(My.Resources.common.FakturacniCenik, "p51Name_Billing", , , "p51billing.p51Name"))
            .Add(AGC(My.Resources.common.NakladovyCenik, "p51Name_Internal", , , "p51internal.p51Name"))
            .Add(AGC(My.Resources.common.TypFaktury, "p92Name"))
            .Add(AGC(My.Resources.common.FakturacniJazyk, "p87Name"))

            .Add(AGC(My.Resources.common.LimitHodin, "p28LimitHours_Notification", BO.cfENUM.Numeric, , "a.p28LimitHours_Notification", True))
            .Add(AGC(My.Resources.common.LimitniHonorar, "p28LimitFee_Notification", BO.cfENUM.Numeric, , "a.p28LimitFee_Notification", True))

            .Add(AGC(My.Resources.common.VlastnikZaznamu, "Owner", , , "j02owner.j02LastName+char(32)+j02owner.j02FirstName"))
            .Add(AGC("Strom index", "p28TreeIndex", , True, "a.p28TreeIndex"))
            .Add(AGC("Strom level", "p28TreeLevel", , True, "a.p28TreeLevel"))
            .Add(AGC(My.Resources.common.Zalozeno, "p28DateInsert", BO.cfENUM.DateTime, , "a.p28DateInsert"))
            .Add(AGC(My.Resources.common.Zalozil, "p28UserInsert", , , "a.p28UserInsert"))
            .Add(AGC(My.Resources.common.Aktualizace, "p28DateUpdate", BO.cfENUM.DateTime, , "a.p28DateUpdate"))
            .Add(AGC(My.Resources.common.Aktualizoval, "p28UserUpdate", , , "a.p28UserUpdate"))
            .Add(AGC("Externí kód", "p28ExternalPID", , , "a.p28ExternalPID"))
        End With
        AppendFreeFields(BO.x29IdEnum.p28Contact, lis)
    End Sub
    Private Sub InhaleJ02ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Příjmení+jméno", "FullNameDesc", , , "a.j02LastName+char(32)+a.j02FirstName"))
            .Add(AGC("Jméno+příjmení", "FullNameAsc", , , "a.j02FirstName+char(32)+a.j02LastName"))
            .Add(AGC("Jméno", "j02FirstName"))
            .Add(AGC("Příjmení", "j02LastName"))
            .Add(AGC("Titul", "j02TitleBeforeName"))
            .Add(AGC("E-mail", "j02Email"))
            .Add(AGC("Pozice", "j07Name"))
            .Add(AGC("Osobní číslo (kód)", "j02Code"))
            .Add(AGC("Fond", "c21Name"))
            .Add(AGC("Středisko", "j18Name"))
            .Add(AGC("Oslovení", "j02Salutation"))

            .Add(AGC("Interní osoba", "j02IsIntraPerson", BO.cfENUM.Checkbox))
            .Add(AGC("Založeno", "j02DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "j02UserInsert"))
            .Add(AGC("Aktualizace", "j02DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "j02UserUpdate"))
            .Add(AGC("Externí kód", "j02ExternalPID"))
        End With
        AppendFreeFields(BO.x29IdEnum.j02Person, lis)
    End Sub
    Private Sub InhaleP31ColList(ByRef lis As List(Of BO.GridColumn))
        Dim bolHideRatesColumns As Boolean = Not Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates)   'zda uživatel nemá právo vidět sazby a fakturační údaje


        With lis
            .Add(AGC(My.Resources.common.Datum, "p31Date", BO.cfENUM.DateOnly))
            .Add(AGC(My.Resources.common.TimeFrom, "TimeFrom", , False, "p31DateTimeFrom_Orig"))
            .Add(AGC(My.Resources.common.TimeUntil, "TimeUntil", , False, "p31DateTimeUntil_Orig"))

            .Add(AGC(My.Resources.common.Osoba, "Person", , , "j02.j02LastName+char(32)+j02.j02FirstName"))
            .Add(AGC(My.Resources.common.p32Name, "p32Name"))
            .Add(AGC(My.Resources.common.FA, "p32IsBillable", BO.cfENUM.Checkbox))
            .Add(AGC(My.Resources.common.Sesit, "p34Name"))

            .Add(AGC(My.Resources.common.Projekt, "p41Name", , , "isnull(p41NameShort,p41Name)"))
            .Add(AGC(My.Resources.common.KodProjektu, "p41Code"))
            .Add(AGC(My.Resources.common.KlientProjektu, "ClientName", , , "p28Client.p28Name"))
            .Add(AGC(My.Resources.common.NazevUkolu, "p56Name"))
            .Add(AGC(My.Resources.common.KodUkolu, "p56Code"))

            .Add(AGC("Text", "p31Text"))
            .Add(AGC(My.Resources.common.Dodavatel, "SupplierName", , , "supplier.p28Name"))
            .Add(AGC(My.Resources.common.KodDokladu, "p31Code"))
            .Add(AGC(My.Resources.common.KontaktniOsoba, "ContactPerson", , , "cp.j02LastName+char(32)+cp.j02FirstName"))

            .Add(AGC(My.Resources.common.Schvaleno, "p71Name"))

            .Add(AGC(My.Resources.common.VykazanaHodnota, "p31Value_Orig", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.VykazaneHodiny, "p31Hours_Orig", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.VykazaneHodinyHHMM, "p31HHMM_Orig", , , "p31Hours_Orig"))
            If Not bolHideRatesColumns Then
                .Add(AGC(My.Resources.common.VychoziSazba, "p31Rate_Billing_Orig", BO.cfENUM.Numeric2))
                .Add(AGC(My.Resources.common.CastkaBezDPH, "p31Amount_WithoutVat_Orig", BO.cfENUM.Numeric2, , , True))
                .Add(AGC("Částka vč. DPH", "p31Amount_WithVat_Orig", BO.cfENUM.Numeric2, , , True))
                .Add(AGC(My.Resources.common.NakladovaSazba, "p31Rate_Internal_Orig", BO.cfENUM.Numeric2))
                .Add(AGC(My.Resources.common.NakladovaCastka, "p31Amount_Internal", BO.cfENUM.Numeric2, , , True))
            End If

            .Add(AGC(My.Resources.common.SazbaDPH, "p31VatRate_Orig", BO.cfENUM.Numeric0))
            .Add(AGC(My.Resources.common.Mena, "j27Code_Billing_Orig", , , "j27billing_orig.j27Code"))

            .Add(AGC(My.Resources.common.NavrhFakturacnihoStatusu, "approve_p72Name", , , "p72approve.p72Name"))
            .Add(AGC(My.Resources.common.SchvalenaHodnota, "p31Value_Approved_Billing", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.SchvaleneHodiny, "p31Hours_Approved_Billing", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.SchvalenoHHMM, "p31HHMM_Approved_Billing"))
            .Add(AGC(My.Resources.common.SchvalenaSazba, "p31Rate_Billing_Approved", BO.cfENUM.Numeric2))
            .Add(AGC("Schváleno interně", "p31Value_Approved_Internal", BO.cfENUM.Numeric2, , , True))
            If Not bolHideRatesColumns Then .Add(AGC(My.Resources.common.SchvalenoBezDPH, "p31Amount_WithoutVat_Approved", BO.cfENUM.Numeric2, , , True))
            If Not bolHideRatesColumns Then .Add(AGC(My.Resources.common.SchvalenoVcDPH, "p31Amount_WithVat_Approved", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.SchvalenoKdy, "p31Approved_When", BO.cfENUM.DateTime))


            .Add(AGC(My.Resources.common.VyfakturovanaHodnota, "p31Value_Invoiced", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.VyfakturovaneHodiny, "p31Hours_Invoiced", BO.cfENUM.Numeric2, , , True))
            .Add(AGC("Vyfakt.HH:mm", "p31HHMM_Invoiced"))
            If Not bolHideRatesColumns Then
                .Add(AGC(My.Resources.common.VyfakturovanaSazba, "p31Rate_Billing_Invoiced", BO.cfENUM.Numeric2))
                .Add(AGC(My.Resources.common.VyfakturovanoBezDPH, "p31Amount_WithoutVat_Invoiced", BO.cfENUM.Numeric2, , , True))
                .Add(AGC(My.Resources.common.VyfakturovanoVcDPH, "p31Amount_WithVat_Invoiced", BO.cfENUM.Numeric2, , , True))
            End If


            .Add(AGC("Vyfakt.sazba DPH", "p31VatRate_Invoiced", BO.cfENUM.Numeric0))

            If Not bolHideRatesColumns Then .Add(AGC("Vyfakt.bez DPH x Kurz", "p31Amount_WithoutVat_Invoiced_Domestic", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.Faktura, "p91Code"))
            .Add(AGC("Vyfakt.status", "p70Name"))
            .Add(AGC(My.Resources.common.FakturacniOddil, "p95Name"))
            .Add(AGC(My.Resources.common.BillingDavka, "p31ApprovingSet"))

            ''.Add(AGC("Je plán", "p31IsPlanRecord", BO.cfENUM.Checkbox))

            .Add(AGC("Kalk/počet", "p31Calc_Pieces", BO.cfENUM.Numeric2))
            .Add(AGC("Kalk/cena 1 ks", "p31Calc_PieceAmount", BO.cfENUM.Numeric2))

            .Add(AGC("Typ úhrady", "TypUhrady", , True, "j19.j19Name", , "LEFT OUTER JOIN j19PaymentType j19 ON a.j19ID=j19.j19ID"))

            .Add(AGC(My.Resources.common.VlastnikZaznamu, "Owner", , False, "j02owner.j02LastName+char(32)+j02owner.j02FirstName"))
            .Add(AGC(My.Resources.common.Zalozeno, "p31DateInsert", BO.cfENUM.DateTime))
            .Add(AGC(My.Resources.common.Zalozil, "p31UserInsert"))
            .Add(AGC(My.Resources.common.Aktualizace, "p31DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC(My.Resources.common.Aktualizoval, "p31UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p31Worksheet, lis)
    End Sub
    Private Sub InhaleP91ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Číslo", "p91Code"))

            .Add(AGC("Název klienta (vazba)", "p28Name"))
            .Add(AGC("Firma klienta (vazba)", "p28CompanyName"))
            .Add(AGC("Klient ve faktuře", "p91Client"))

            .Add(AGC("Měna", "j27Code"))
            .Add(AGC("Typ faktury", "p92Name"))
            .Add(AGC("Projekt", "p41Name", , , "isnull(p41NameShort,p41Name)"))
            .Add(AGC("DPH region", "j17Name"))

            .Add(AGC("Bez dph", "p91Amount_WithoutVat", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Bez dph x Kurz", "WithoutVat_Krat_Kurz", BO.cfENUM.Numeric, , "p91Amount_WithoutVat*p91ExchangeRate", True))
            .Add(AGC("Dluh", "p91Amount_Debt", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Dluh x Kurz", "Debt_Krat_Kurz", BO.cfENUM.Numeric, , "p91Amount_Debt*p91ExchangeRate", True))
            .Add(AGC("Celkem", "p91Amount_TotalDue", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Celkem x Kurz", "p91Amount_TotalDue_Krat_Kurz", BO.cfENUM.Numeric, , "p91Amount_TotalDue*p91ExchangeRate", True))
            .Add(AGC("Celk.dph", "p91Amount_Vat", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Vč.dph", "p91Amount_WithVat", BO.cfENUM.Numeric, , , True))
            .Add(AGC("Haléřové zaokrouhlení", "p91RoundFitAmount", BO.cfENUM.Numeric))
            .Add(AGC("Uhrazené zálohy", "p91ProformaBilledAmount", BO.cfENUM.Numeric))
            ''.Add(AGC("Bez dph CZK", "WithoutVat_CZK", BO.cfENUM.Numeric, , , True))
            ''.Add(AGC("Bez dph EUR", "WithoutVat_EUR", BO.cfENUM.Numeric, , , True))
            ''.Add(AGC("Dluh CZK", "Debt_CZK", BO.cfENUM.Numeric, , , True))
            ''.Add(AGC("Dluh EUR", "Debt_EUR", BO.cfENUM.Numeric, , , True))


            .Add(AGC("Datum", "p91Date", BO.cfENUM.DateOnly))
            .Add(AGC("Plnění", "p91DateSupply", BO.cfENUM.DateOnly))
            .Add(AGC("Splatnost", "p91DateMaturity", BO.cfENUM.DateOnly))
            .Add(AGC("Datum úhrady", "p91DateBilled", BO.cfENUM.DateOnly))
            .Add(AGC("Aktuální stav", "b02Name"))



            .Add(AGC("Text", "p91Text1"))

            .Add(AGC("Ulice klienta", "p91ClientAddress1_Street"))
            .Add(AGC("Město klienta", "p91ClientAddress1_City"))
            .Add(AGC("PSČ klienta", "p91ClientAddress1_ZIP"))
            .Add(AGC("Stát klienta", "p91ClientAddress1_Country"))
            .Add(AGC("Kontaktní osoba", "p91ClientPerson"))
            .Add(AGC("IČ klienta", "p91Client_RegID"))

            .Add(AGC("Vlastník záznamu", "Owner", , , "j02owner.j02LastName+char(32)+j02owner.j02FirstName"))
            .Add(AGC("Založeno", "p91DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "p91UserInsert"))
            .Add(AGC("Aktualizace", "p91DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "p91UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.p91Invoice, lis)
    End Sub

    Private Sub InhaleP56ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC(My.Resources.common.Kod, "p56Code"))
            .Add(AGC(My.Resources.common.Typ, "p57Name"))
            .Add(AGC(My.Resources.common.Nazev, "p56Name"))
            .Add(AGC("Aktuální stav", "b02Name"))            
            .Add(AGC(My.Resources.common.Klient, "Client", , , "p28client.p28Name"))
            .Add(AGC(My.Resources.common.Projekt, "p41Name", , , "isnull(p41NameShort,p41Name)"))
            .Add(AGC(My.Resources.common.KodProjektu, "p41Code"))
            .Add(AGC(My.Resources.common.Prijemce, "ReceiversInLine", , , "dbo.p56_getroles_inline(a.p56ID)"))
            .Add(AGC(My.Resources.common.VlastnikZaznamu, "Owner", , , "j02owner.j02LastName+char(32)+j02owner.j02FirstName"))
            .Add(AGC(My.Resources.common.Termin, "p56PlanUntil", BO.cfENUM.DateTime))
            .Add(AGC(My.Resources.common.PlanStart, "p56PlanFrom", BO.cfENUM.DateTime))
            .Add(AGC("Hotovo%", "p56CompletePercent", BO.cfENUM.Numeric0))
            .Add(AGC("Produkt", "p58Name"))
            .Add(AGC(My.Resources.common.PrioritaZadavatele, "p59NameSubmitter", , , "p59submitter.p59Name"))

            .Add(AGC("Hodnocení", "p56RatingValue", BO.cfENUM.Numeric0))
            .Add(AGC("Připomenutí", "p56ReminderDate", BO.cfENUM.DateTime))

            .Add(AGC(My.Resources.common.PlanHodin, "p56Plan_Hours", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.PlanVydaju, "p56Plan_Expenses", BO.cfENUM.Numeric2, , , True))
            .Add(AGC(My.Resources.common.VykazaneHodiny, "Hours_Orig", BO.cfENUM.Numeric2, , "p31.Hours_Orig", True))
            .Add(AGC(My.Resources.common.VykazaneVydaje, "Expenses_Orig", BO.cfENUM.Numeric2, , "p31.Expenses_Orig", True))

            
            .Add(AGC(My.Resources.common.Zalozeno, "p56DateInsert", BO.cfENUM.DateTime))
            .Add(AGC(My.Resources.common.Zalozil, "p56UserInsert"))
            .Add(AGC(My.Resources.common.Aktualizace, "p56DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC(My.Resources.common.Aktualizoval, "p56UserUpdate"))
            .Add(AGC(My.Resources.common.ExterniKod, "p56ExternalPID"))
        End With
        AppendFreeFields(BO.x29IdEnum.p56Task, lis)
    End Sub
    Private Sub InhaleO23ColList(ByRef lis As List(Of BO.GridColumn))
        With lis
            .Add(AGC("Typ dokumentu", "o24Name"))
            .Add(AGC("Název", "o23Name"))
            .Add(AGC("Kód dokumentu", "o23Code"))
            .Add(AGC("Aktuální stav", "b02Name"))
            .Add(AGC("Projekt", "Project", , , "isnull(p41.p41NameShort,p41.p41Name)"))
            .Add(AGC("Klient projektu", "ProjectClient", , , "p28_client.p28Name"))
            .Add(AGC("Firma", "DocCompany", , , "p28.p28Name"))
            .Add(AGC("Faktura", "p91Code"))
            .Add(AGC("Úkol", "p56Code"))
            .Add(AGC("Příjemci", "ReceiversInLine", , , "dbo.o23_getroles_inline(a.o23ID)"))
            .Add(AGC("Vlastník", "Owner", , , "j02owner.j02LastName+char(32)+j02owner.j02FirstName"))
            .Add(AGC("Datum", "o23Date", BO.cfENUM.DateTime))
            .Add(AGC("Připomenutí", "o23ReminderDate", BO.cfENUM.DateTime))

            .Add(AGC("Založeno", "o23DateInsert", BO.cfENUM.DateTime))
            .Add(AGC("Založil", "o23UserInsert"))
            .Add(AGC("Aktualizace", "o23DateUpdate", BO.cfENUM.DateTime))
            .Add(AGC("Aktualizoval", "o23UserUpdate"))
        End With
        AppendFreeFields(BO.x29IdEnum.o23Notepad, lis)
    End Sub



    Private Function AGC(strHeader As String, strName As String, Optional colType As BO.cfENUM = BO.cfENUM.AnyString, Optional bolSortable As Boolean = True, Optional strDBName As String = "", Optional bolShowTotals As Boolean = False, Optional strSqlSyntax_FROM As String = "")
        Dim col As BO.GridColumn
        col = New BO.GridColumn(_x29id, strHeader, strName, colType)
        col.IsSortable = bolSortable
        col.ColumnDBName = strDBName
        col.IsShowTotals = bolShowTotals
        col.SqlSyntax_FROM = strSqlSyntax_FROM
        Return col
    End Function

    Public Function GroupByPallet(x29id As BO.x29IdEnum) As List(Of BO.GridGroupByColumn) Implements Ij74SavedGridColTemplateBL.GroupByPallet
        Dim lis As New List(Of BO.GridGroupByColumn)
        lis.Add(New BO.GridGroupByColumn(My.Resources.common.BezSouhrnu, "", "", ""))
        Select Case x29id
            Case BO.x29IdEnum.p41Project
                lis.Add(New BO.GridGroupByColumn("Klient", "Client", "a.p28ID_Client", "min(p28client.p28Name)"))
                lis.Add(New BO.GridGroupByColumn("Typ projektu", "p42Name", "a.p42ID", "min(p42.p42Name)"))
                lis.Add(New BO.GridGroupByColumn("Středisko", "j18Name", "a.j18ID", "min(j18.j18Name)"))
                ''lis.Add(New BO.GridGroupByColumn("DRAFT", "p41IsDraft", "a.p41IsDraft", "a.p41IsDraft"))


            Case BO.x29IdEnum.p28Contact
                lis.Add(New BO.GridGroupByColumn("Typ klienta", "p29Name", "a.p29ID", "min(p29.p29Name)"))
                lis.Add(New BO.GridGroupByColumn("Typ faktury", "p92Name", "a.p92ID", "min(p92.p92Name)"))
                lis.Add(New BO.GridGroupByColumn("Fakturační jazyk", "p87Name", "a.p87ID", "min(p87Name)"))
                lis.Add(New BO.GridGroupByColumn("Vlastník záznamu", "Owner", "a.j02ID_Owner", "min(j02owner.j02LastName+' '+j02owner.j02FirstName)"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "p28IsDraft", "a.p28IsDraft", "a.p28IsDraft"))
            Case BO.x29IdEnum.o23Notepad
                lis.Add(New BO.GridGroupByColumn("Typ dokumentu", "o24Name", "a.o24ID", "min(o24Name)"))
                lis.Add(New BO.GridGroupByColumn("Klient", "ProjectClient", "a.p28ID", "min(p28Name)"))
                lis.Add(New BO.GridGroupByColumn("Projekt", "Project", "a.p41ID", "min(isnull(p41NameShort,p41Name))"))
                lis.Add(New BO.GridGroupByColumn("Aktuální stav", "b02Name", "a.b02ID", "min(b02.b02Name)"))
                lis.Add(New BO.GridGroupByColumn("Vlastník záznamu", "Owner", "a.j02ID_Owner", "min(j02owner.j02LastName+' '+j02owner.j02FirstName)"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "o23IsDraft", "a.o23IsDraft", "a.o23IsDraft"))
            Case BO.x29IdEnum.p31Worksheet
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.Sesit, "p34Name", "p32.p34ID", "min(p34.p34Name)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.p32Name, "p32Name", "a.p32ID", "min(p34.p34Name+' - '+p32.p32Name)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.Osoba, "Person", "a.j02ID", "min(j02.j02LastName+' '+j02.j02Firstname)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.KlientProjektu, "ClientName", "p41.p28ID_Client", "min(p28Client.p28Name)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.Projekt, "p41Name", "a.p41ID", "min(isnull(p28Client.p28Name+' - ','')+isnull(p41.p41NameShort,p41.p41Name))"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.Faktura, "p91Code", "a.p91ID", "min(p91.p91Code)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.Schvaleno, "p71Name", "a.p71ID", "min(p71.p71Name)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.FakturacniStatus, "p70Name", "a.p70ID", "min(p70.p70Name)"))
                lis.Add(New BO.GridGroupByColumn(My.Resources.common.Dodavatel, "SupplierName", "a.p28ID_Supplier", "min(supplier.p28Name)"))
                ''lis.Add(New BO.GridGroupByColumn("Billing dávka", "p31ApprovingSet", "a.p31ApprovingSet", "min(a.p31ApprovingSet)"))
            Case BO.x29IdEnum.p56Task
                lis.Add(New BO.GridGroupByColumn("Typ úkolu", "p57Name", "a.p57ID", "min(p57.p57Name)"))
                lis.Add(New BO.GridGroupByColumn("Klient", "Client", "p41.p28ID_Client", "min(p28client.p28Name)"))
                lis.Add(New BO.GridGroupByColumn("Projekt", "ProjectCodeAndName", "a.p41ID", "min(isnull(p28client.p28Name+' - ','')+isnull(p41NameShort,p41Name))"))
                lis.Add(New BO.GridGroupByColumn("Aktuální stav", "b02Name", "a.b02ID", "min(b02.b02Name)"))
                lis.Add(New BO.GridGroupByColumn("Produkt", "p58Name", "a.p58ID", "min(p58Name)"))
                lis.Add(New BO.GridGroupByColumn("Priorita zadavatele", "p59NameSubmitter", "a.p59ID_Submitter", "min(p59submitter.p59name)"))
                lis.Add(New BO.GridGroupByColumn("Příjemce", "ReceiversInLine", "", ""))
                lis.Add(New BO.GridGroupByColumn("Vlastník záznamu", "Owner", "a.j02ID_Owner", "min(j02owner.j02LastName+' '+j02owner.j02FirstName)"))
            Case BO.x29IdEnum.j02Person
                lis.Add(New BO.GridGroupByColumn("Pozice", "j07Name", "a.j07ID", "min(j07Name)"))
                lis.Add(New BO.GridGroupByColumn("Pracovní fond", "c21Name", "a.c21ID", "min(c21Name)"))
                lis.Add(New BO.GridGroupByColumn("Středisko", "j18Name", "a.j18ID", "min(j18Name)"))
                lis.Add(New BO.GridGroupByColumn("Interní osoba", "j02IsIntraPerson", "j02IsIntraPerson", "j02IsIntraPerson"))
            Case BO.x29IdEnum.p91Invoice
                lis.Add(New BO.GridGroupByColumn("Klient", "p28Name", "a.p28ID", "min(p28Name)"))
                lis.Add(New BO.GridGroupByColumn("Měna", "j27Code", "a.j27ID", "min(j27Code)"))
                lis.Add(New BO.GridGroupByColumn("Typ faktury", "p92Name", "a.p92ID", "min(p92Name)"))
                lis.Add(New BO.GridGroupByColumn("DRAFT", "p91IsDraft", "p91IsDraft", "p91IsDraft"))
        End Select
        Return lis
    End Function
End Class
