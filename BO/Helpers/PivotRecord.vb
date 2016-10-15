﻿Public Enum PivotRowColumnFieldType
    Person = 201
    p41Name = 4101
    p34Name = 3401
    p32Name = 3201
    p95Name = 9501
    p56Name = 5601
    p28Name = 2801
    j18Name = 1801
    j18Name_j02 = 1802
    p32IsBillable = 9801
    p71Name = 7101
    p70Name = 7001
    p72Name = 7201
    Year = 9901
    Month = 9902
    YearInvoice = 9903
    MonthInvoice = 9904
    j27code_orig = 2701
    j27code_invoice = 2702
    p42Name = 4201
    p91Code = 9101
    InvoiceClient = 9102
    p31Rate_Billing_Orig = 3101
    p31Rate_Billing_Approved = 3102
    p31Rate_Billing_Invoiced = 3103
End Enum
Public Enum PivotSumFieldType
    p31Hours_Orig = 1
    p31Hours_WIP = 4
    p31Hours_BIN = 5

    p31Hours_Invoiced = 3
    p31Value_Orig = 11
    p31Value_Approved_Billing = 12
    p31Value_Invoiced = 13
    p31Amount_WithoutVat_Orig = 21
    p31Amount_WithoutVat_Approved = 22
    p31Amount_WithoutVat_Invoiced = 23
    p31Amount_WithoutVat_FixedCurrency = 24
    p31Amount_WithoutVat_Invoiced_Domestic = 25
    p31Amount_HoursFee_WIP = 26
    p31Amount_WithoutVat_WIP = 27

    p31Hours_Approved_Billing = 2
    p31Hours_Approved_FixedPrice = 126
    p31Hours_Approved_WriteOff = 123
    p31Hours_Approved_InvoiceLater = 127

    p31Hours_Invoiced_FixedPrice = 36
    p31Hours_Invoiced_WriteOff = 33


End Enum
Public Class PivotRowColumnField
    Public FieldType As PivotRowColumnFieldType
    Public Property Caption As String
    Private Property _SelectField As String
    Private Property _GroupByField As String
    
    Public Sub New(ft As PivotRowColumnFieldType, Optional strCaption As String = "")
        Me.FieldType = ft
        Me.Caption = strCaption
        Dim s As String = ""
        Select Case ft
            Case PivotRowColumnFieldType.j18Name
                _SelectField = "min(j18.j18Name)"
                _GroupByField = "p41.j18ID"
                s = "Středisko projektu"
            Case PivotRowColumnFieldType.j18Name_j02
                _SelectField = "min(j18_j02.j18Name)"
                _GroupByField = "j02.j18ID"
                s = "Středisko osoby"
            Case PivotRowColumnFieldType.Month
                _SelectField = "convert(varchar(7), a.p31Date, 126)"
                _GroupByField = _SelectField
                s = "Měsíc"
            Case PivotRowColumnFieldType.Year
                _SelectField = "Year(a.p31date)"
                _GroupByField = _SelectField
                s = "Rok"
            Case PivotRowColumnFieldType.YearInvoice
                _SelectField = "Year(p91.p91DateSupply)"
                _GroupByField = _SelectField
                s = "Rok fakturace"
            Case PivotRowColumnFieldType.MonthInvoice
                _SelectField = "convert(varchar(7), p91.p91DateSupply, 126)"
                _GroupByField = _SelectField
                s = "Měsíc fakturace"
            Case PivotRowColumnFieldType.p28Name
                _SelectField = "min(p28Client.p28Name)"
                _GroupByField = "p41.p28ID_Client"
                s = "Klient projektu"
            Case PivotRowColumnFieldType.p34Name
                _SelectField = "min(p34.p34Name)"
                _GroupByField = "p32.p34ID"
                s = "Sešit"
            Case PivotRowColumnFieldType.p32Name
                _SelectField = "min(p32.p32Name)"
                _GroupByField = "a.p32ID"
                s = "Aktivita"
            Case PivotRowColumnFieldType.p95Name
                _SelectField = "min(p95.p95Name)"
                _GroupByField = "p32.p95ID"
                s = "Fakturační oddíl"
            Case PivotRowColumnFieldType.p32IsBillable
                _SelectField = "min(convert(int,p32.p32IsBillable))"
                _GroupByField = "p32.p32IsBillable"
                s = "Fakturovatelná aktivita"
            Case PivotRowColumnFieldType.p41Name
                _SelectField = "min(p41.p41Name)"
                _GroupByField = "a.p41ID"
                s = "Projekt"
            Case PivotRowColumnFieldType.p42Name
                _SelectField = "min(p42.p42Name)"
                _GroupByField = "p42.p42ID"
                s = "Typ projektu"
            Case PivotRowColumnFieldType.Person
                _SelectField = "min(j02LastName+' '+j02Firstname)"
                _GroupByField = "a.j02ID"
                s = "Osoba"
            Case PivotRowColumnFieldType.p56Name
                _SelectField = "min(p56Name+' ('+isnull(p56Code,'')+')')"
                _GroupByField = "a.p56ID"
                s = "Úkol"
            Case PivotRowColumnFieldType.p70Name
                _SelectField = "min(p70Name)"
                _GroupByField = "a.p70ID"
                s = "Status ve faktuře"
            Case PivotRowColumnFieldType.p71Name
                _SelectField = "min(p71Name)"
                _GroupByField = "a.p71ID"
                s = "Schváleno"
            Case PivotRowColumnFieldType.p72Name
                _SelectField = "min(p72Name)"
                _GroupByField = "a.p72ID_AfterApprove"
                s = "Návrh fakturačního statusu"
            Case PivotRowColumnFieldType.j27code_orig
                _SelectField = "min(j27orig.j27Code)"
                _GroupByField = "a.j27ID_Billing_Orig"
                s = "Měna úkonu"
            Case PivotRowColumnFieldType.j27code_invoice
                _SelectField = "min(j27invoice.j27Code)"
                _GroupByField = "a.j27ID_Billing_Invoiced"
                s = "Měna faktury"
            Case PivotRowColumnFieldType.p91Code
                _SelectField = "min(p91Code)"
                _GroupByField = "a.p91ID"
                s = "ID faktury"
            Case PivotRowColumnFieldType.InvoiceClient
                _SelectField = "min(p91Client.p28Name)"
                _GroupByField = "p91.p28ID"
                s = "Klient faktury"
            Case PivotRowColumnFieldType.p31Rate_Billing_Orig
                _SelectField = "p31Rate_Billing_Orig"
                _GroupByField = "p31Rate_Billing_Orig"
                s = "Výchozí sazba"
            Case PivotRowColumnFieldType.p31Rate_Billing_Approved
                _SelectField = "p31Rate_Billing_Approved"
                _GroupByField = "p31Rate_Billing_Approved"
                s = "Schválená sazba"
            Case PivotRowColumnFieldType.p31Rate_Billing_Invoiced
                _SelectField = "p31Rate_Billing_Invoiced"
                _GroupByField = "p31Rate_Billing_Invoiced"
                s = "Vyfakturovaná sazba"
        End Select
        If Me.Caption = "" Then Me.Caption = s
    End Sub
    
    Public ReadOnly Property SelectField As String
        Get
            Return _SelectField
        End Get
    End Property
    Public ReadOnly Property GroupByField As String
        Get
            Return _GroupByField
        End Get
    End Property
    

    
End Class

Public Class PivotSumField
    Public FieldType As PivotSumFieldType
    Public Property Caption As String
    Public Property ColumnType As BO.cfENUM = cfENUM.Numeric2
    Private Property _SelectField As String

    Public Sub New(ft As PivotSumFieldType, Optional strCaption As String = "")
        Me.FieldType = ft
        Me.Caption = strCaption
        Dim s As String = ""
        Select Case ft
            Case PivotSumFieldType.p31Hours_Orig
                _SelectField = "sum(p31Hours_Orig)"
                s = "Vykázané hodiny"
            Case PivotSumFieldType.p31Hours_WIP
                _SelectField = "sum(case when a.p71ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Hours_Orig end)"
                s = "Rozpracované hodiny"
            Case PivotSumFieldType.p31Hours_BIN
                _SelectField = "sum(case when GETDATE() NOT BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Hours_Orig end)"
                s = "Hodiny v archivu"
            Case PivotSumFieldType.p31Hours_Invoiced
                _SelectField = "sum(p31Hours_Invoiced)"
                s = "Vyfakturovné hodiny"
            Case PivotSumFieldType.p31Hours_Invoiced_FixedPrice
                _SelectField = "sum(case when a.p70ID=6 THEN p31Hours_Orig end)"
                s = "Vyfakturované hodiny [Paušál]"
            Case PivotSumFieldType.p31Hours_Invoiced_WriteOff
                _SelectField = "sum(case when a.p70ID IN (2,3) THEN p31Hours_Orig end)"
                s = "Vyfakturované hodiny [Odpis]"
            Case PivotSumFieldType.p31Hours_Approved_Billing
                _SelectField = "sum(case when a.p71ID=1 AND a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Hours_Approved_Billing END)"
                s = "Čeká na fakturaci/Hodiny [Fakturovat]"
            Case PivotSumFieldType.p31Amount_WithoutVat_Orig
                _SelectField = "sum(p31Amount_WithoutVat_Orig)"
                s = "Výchozí částka bez DPH"
            Case PivotSumFieldType.p31Amount_WithoutVat_Approved
                _SelectField = "sum(case when a.p71ID=1 AND a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil then p31Amount_WithoutVat_Approved end)"
                s = "Čeká na fakturaci/Částka bez DPH"
            Case PivotSumFieldType.p31Amount_WithoutVat_Invoiced
                _SelectField = "sum(p31Amount_WithoutVat_Invoiced)"
                s = "Vyfakturováno bez DPH"
            Case PivotSumFieldType.p31Amount_WithoutVat_Invoiced_Domestic
                _SelectField = "sum(p31Amount_WithoutVat_Invoiced_Domestic)"
                s = "Vyfakturováno bez DPH x Kurz"
            Case PivotSumFieldType.p31Amount_WithoutVat_FixedCurrency
                _SelectField = "sum(p31Amount_WithoutVat_FixedCurrency)"
                s = "Přepočteno fixním kurzem"
            Case PivotSumFieldType.p31Value_Orig
                _SelectField = "sum(p31Value_Orig)"
                s = "Vykázaná hodnota"
            Case PivotSumFieldType.p31Value_Invoiced
                _SelectField = "sum(p31Value_Invoiced)"
                s = "Vyfakturovaná hodnota"
            Case PivotSumFieldType.p31Value_Approved_Billing
                _SelectField = "sum(case when a.p71ID=1 AND a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Value_Approved_Billing END)"
                s = "Čeká na fakturaci/Hodnota [Fakturovat]"
            Case PivotSumFieldType.p31Hours_Approved_FixedPrice
                _SelectField = "sum(CASE WHEN a.p71ID=1 AND a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil AND a.p72ID_AfterApprove=6 THEN p31Hours_Orig end)"
                s = "Čeká na fakturaci/Hodiny [Paušál]"
            Case PivotSumFieldType.p31Hours_Approved_WriteOff
                _SelectField = "sum(case when a.p71ID=1 AND a.p91ID IS NULL AND a.p72ID_AfterApprove IN (2,3) AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Hours_Orig end)"
                s = "Čeká na fakturaci/Hodiny [Odpis]"
            Case PivotSumFieldType.p31Hours_Approved_InvoiceLater
                _SelectField = "sum(case when a.p71ID=1 AND a.p91ID IS NULL AND a.p72ID_AfterApprove=7 AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Hours_Orig end)"
                s = "Čeká na fakturaci/Hodiny [Fakturovat později]"
            Case PivotSumFieldType.p31Amount_WithoutVat_WIP
                _SelectField = "sum(case when a.p71ID IS NULL AND a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Amount_WithoutVat_Orig END)"
                s = "Rozpracovanost, částka bez DPH"
            Case PivotSumFieldType.p31Amount_HoursFee_WIP
                _SelectField = "sum(case when a.p71ID IS NULL AND a.p91ID IS NULL AND p34.p33ID=1 AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil THEN p31Amount_WithoutVat_Orig END)"
                s = "Honorář z rozpracovaných hodin"
        End Select
        If Me.Caption = "" Then Me.Caption = s
    End Sub
    Public ReadOnly Property SelectField As String
        Get
            Return _SelectField
        End Get
    End Property
    Public ReadOnly Property FieldTypeID As Integer
        Get
            Return CInt(Me.FieldType)
        End Get
    End Property
End Class
Public Class PivotRecord
    Public Property Row1 As String
    Public Property Row2 As String
    Public Property Row3 As String
    Public Property Row4 As String
    Public Property Col1 As String
    Public Property Col2 As String
    Public Property Sum1 As Double
    Public Property Sum2 As Double
    Public Property Sum3 As Double
    Public Property Sum4 As Double


End Class
