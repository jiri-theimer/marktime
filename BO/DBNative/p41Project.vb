Public Enum p41WorksheetOperFlagEnum
    _NotSpecified = 0                   '0 - do projektu je povoleno zapisovat i fakturovat worksheet
    NoEntryData = 1                     '1 - V projektu není povoleno zapisování úkonů
    OnlyEntryData = 2                   '2 - V projektu je povoleno pouze zapisování úkonů
    WithTaskOnly = 3                      '3 - V projektu je povoleno zapisovat úkony pouze přes úkol
    NoLimit = 9
End Enum

Public Class p41Project
    Inherits BOMother
    Public Property p42ID As Integer
    Public Property p28ID_Client As Integer
    Public Property p28ID_Billing As Integer
    Public Property b02ID As Integer
    Public Property p87ID As Integer
    Public Property p51ID_Billing As Integer
    Public Property p51ID_Internal As Integer
    Public Property p92ID As Integer
    Public Property j18ID As Integer
    Public Property p61ID As Integer

    Public Property j02ID_Owner As Integer

    Public Property p41Name As String
    Public Property p41IsDraft As Boolean
    Public Property p41NameShort As String
    Public Property p41RobotAddress As String
    Public Property p41ExternalPID As String
    Public Property p41ParentID As Integer
    Protected Property _p41Code As String
    Public Property p41Code As String
        Get
            Return _p41Code
        End Get
        Set(value As String)
            _p41Code = value
        End Set
    End Property
       
    Public Property p41InvoiceDefaultText1 As String
    Public Property p41InvoiceDefaultText2 As String
    Public Property p41InvoiceMaturityDays As Integer

    Public Property p41WorksheetOperFlag As p41WorksheetOperFlagEnum = p41WorksheetOperFlagEnum.NoLimit

    Public Property p41PlanFrom As Date?
    Public Property p41PlanUntil As Date?

    Public Property p41LimitHours_Notification As Double
    Public Property p41LimitFee_Notification As Double

    Private Property _Owner As String
    Private Property _p42name As String
    Private Property _j18Name As String

    Public ReadOnly Property p42Name As String
        Get
            Return _p42name
        End Get
    End Property
    Public ReadOnly Property j18Name As String
        Get
            Return _j18Name
        End Get
    End Property

    Private Property _b02Name As String
    Private Property _b01ID As Integer
    Public ReadOnly Property b01ID As Integer
        Get
            Return _b01ID
        End Get
    End Property
    Public ReadOnly Property b02Name As String
        Get
            Return _b02Name
        End Get
    End Property

    Private Property _p92Name As String
    Public ReadOnly Property p92Name As String
        Get
            Return _p92Name
        End Get
    End Property

    Private Property _Client As String
    Public ReadOnly Property Client As String
        Get
            Return _Client
        End Get
    End Property

    Private Property _ClientBilling As String
    Public ReadOnly Property ClientBilling As String
        Get
            Return _ClientBilling
        End Get
    End Property

    Private Property _p51Name_Billing As String
    Public ReadOnly Property p51Name_Billing As String
        Get
            Return _p51Name_Billing
        End Get
    End Property

    Private Property _p51Name_Internal As String
    Public ReadOnly Property p51Name_Internal As String
        Get
            Return _p51Name_Internal
        End Get
    End Property

    Private Property _p87ID_Client As Integer
    Public ReadOnly Property p87ID_Client As Integer
        Get
            Return _p87ID_Client
        End Get
    End Property
    
    Public ReadOnly Property FullName As String
        Get
            If p28ID_Client > 0 Then
                If Me.p41NameShort = "" Then Return _Client & " - " & Me.p41Name Else Return _Client & " - " & Me.p41NameShort
            Else
                If Me.p41NameShort = "" Then Return Me.p41Name Else Return Me.p41NameShort
            End If
        End Get
    End Property
    Public ReadOnly Property Owner As String
        Get
            Return _Owner
        End Get
    End Property




    '----uživatelská pole--------------------
    Public Property p41FreeText01 As String = Nothing
    Public Property p41FreeText02 As String = Nothing
    Public Property p41FreeText03 As String = Nothing
    Public Property p41FreeText04 As String = Nothing
    Public Property p41FreeText05 As String = Nothing
    Public Property p41FreeText06 As String = Nothing
    Public Property p41FreeText07 As String = Nothing
    Public Property p41FreeText08 As String = Nothing
    Public Property p41FreeText09 As String = Nothing
    Public Property p41FreeText10 As String = Nothing

    Public Property p41FreeBoolean01 As Boolean
    Public Property p41FreeBoolean02 As Boolean
    Public Property p41FreeBoolean03 As Boolean
    Public Property p41FreeBoolean04 As Boolean
    Public Property p41FreeBoolean05 As Boolean
    Public Property p41FreeBoolean06 As Boolean
    Public Property p41FreeBoolean07 As Boolean
    Public Property p41FreeBoolean08 As Boolean
    Public Property p41FreeBoolean09 As Boolean
    Public Property p41FreeBoolean10 As Boolean

    Public Property p41FreeDate01 As DateTime?
    Public Property p41FreeDate02 As DateTime?
    Public Property p41FreeDate03 As DateTime?
    Public Property p41FreeDate04 As DateTime?
    Public Property p41FreeDate05 As DateTime?
    Public Property p41FreeDate06 As DateTime?
    Public Property p41FreeDate07 As DateTime?
    Public Property p41FreeDate08 As DateTime?
    Public Property p41FreeDate09 As DateTime?
    Public Property p41FreeDate10 As DateTime?

    Public Property p41FreeNumber01 As Double
    Public Property p41FreeNumber02 As Double
    Public Property p41FreeNumber03 As Double
    Public Property p41FreeNumber04 As Double
    Public Property p41FreeNumber05 As Double
    Public Property p41FreeNumber06 As Double
    Public Property p41FreeNumber07 As Double
    Public Property p41FreeNumber08 As Double
    Public Property p41FreeNumber09 As Double
    Public Property p41FreeNumber10 As Double

    Public Property p41FreeCombo01 As Integer?
    Public Property p41FreeCombo02 As Integer?
    Public Property p41FreeCombo03 As Integer?
    Public Property p41FreeCombo04 As Integer?
    Public Property p41FreeCombo05 As Integer?
    Public Property p41FreeCombo06 As Integer?
    Public Property p41FreeCombo07 As Integer?
    Public Property p41FreeCombo08 As Integer?
    Public Property p41FreeCombo09 As Integer?
    Public Property p41FreeCombo10 As Integer?

    Public Property p41FreeCombo01Text As String = Nothing
    Public Property p41FreeCombo02Text As String = Nothing
    Public Property p41FreeCombo03Text As String = Nothing
    Public Property p41FreeCombo04Text As String = Nothing
    Public Property p41FreeCombo05Text As String = Nothing
    Public Property p41FreeCombo06Text As String = Nothing
    Public Property p41FreeCombo07Text As String = Nothing
    Public Property p41FreeCombo08Text As String = Nothing
    Public Property p41FreeCombo09Text As String = Nothing
    Public Property p41FreeCombo10Text As String = Nothing
End Class
