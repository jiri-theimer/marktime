Public Class p56Task
    Inherits BOMother
    Public Property p41ID As Integer
    Public Property p57ID As Integer
    Public Property o22ID As Integer
    Public Property b02ID As Integer
    Public Property p58ID As Integer
    Public Property j02ID_Owner As Integer
    Public Property p59ID_Submitter As Integer
    Public Property p59ID_Receiver As Integer


    Public Property p56Name As String
    Public Property p56NameShort As String
    Public Property p56Code As String
    Public Property p56Description As String

    Public Property p56PlanFrom As Date?
    Public Property p56PlanUntil As Date?
    Public Property p56ReminderDate As Date?

    Public Property p56Ordinary As Integer
    Public Property p56Plan_Hours As Double
    Public Property p56Plan_Expenses As Double
    Public Property p56CompletePercent As Integer
    Public Property p56RatingValue As Integer?

    Friend Property _ReceiversInLine As String
    Public ReadOnly Property ReceiversInLine As String
        Get
            Return _ReceiversInLine
        End Get
    End Property
    Friend Property _o22Name As String
    Public ReadOnly Property o22Name As String
        Get
            Return _o22Name
        End Get
    End Property
    Friend Property _Owner As String
    Public ReadOnly Property Owner As String
        Get
            Return _Owner
        End Get
    End Property
    Friend Property _o43ID As Integer
    Public ReadOnly Property o43ID As Integer
        Get
            Return _o43ID
        End Get
    End Property

    Friend Property _p57Name As String
    Public ReadOnly Property p57Name As String
        Get
            Return _p57Name
        End Get
    End Property
    Friend Property _p58Name As String
    Public ReadOnly Property p58Name As String
        Get
            Return _p58Name
        End Get
    End Property
    Friend Property _p59NameSubmitter As String
    Public ReadOnly Property p59NameSubmitter As String
        Get
            Return _p59NameSubmitter
        End Get
    End Property
    Friend Property _p57IsHelpdesk As Boolean
    Public ReadOnly Property p57IsHelpdesk As Boolean
        Get
            Return _p57IsHelpdesk
        End Get
    End Property
    Friend Property _b01ID As Integer
    Public ReadOnly Property b01ID As Integer
        Get
            Return _b01ID
        End Get
    End Property

    Friend Property _b02Name As String
    Public ReadOnly Property b02Name As String
        Get
            Return _b02Name
        End Get
    End Property
    Friend Property _b02Color As String
    Public ReadOnly Property b02Color As String
        Get
            Return _b02Color
        End Get
    End Property

    Friend Property _p41Name As String
    Public ReadOnly Property p41Name As String
        Get
            Return _p41Name
        End Get
    End Property

    Friend Property _p41Code As String

    Public ReadOnly Property p41Code As String
        Get
            Return _p41Code
        End Get
    End Property
    Public ReadOnly Property ProjectCodeAndName As String
        Get
            Return _p41Code & " - " & _p41Name
        End Get
    End Property

    Friend Property _Client As String
    Public ReadOnly Property Client As String
        Get
            Return _Client
        End Get
    End Property

    Public ReadOnly Property FullName As String
        Get
            If _Client <> "" Then
                Return Me.p56Name & " [" & _Client & " - " & _p41Name & "]"
            Else
                Return Me.p56Name & " [" & _p41Name & "]"
            End If
        End Get
    End Property
    Public ReadOnly Property NameWithTypeAndCode As String
        Get
            Return _p57Name & " - " & Me.p56Name & " (" & Me.p56Code & ")"
        End Get
    End Property

    '----uživatelská pole--------------------
    Public Property p56FreeText01 As String = Nothing
    Public Property p56FreeText02 As String = Nothing
    Public Property p56FreeText03 As String = Nothing
    Public Property p56FreeText04 As String = Nothing
    Public Property p56FreeText05 As String = Nothing
    Public Property p56FreeText06 As String = Nothing
    Public Property p56FreeText07 As String = Nothing
    Public Property p56FreeText08 As String = Nothing
    Public Property p56FreeText09 As String = Nothing
    Public Property p56FreeText10 As String = Nothing

    Public Property p56FreeBoolean01 As Boolean
    Public Property p56FreeBoolean02 As Boolean
    Public Property p56FreeBoolean03 As Boolean
    Public Property p56FreeBoolean04 As Boolean
    Public Property p56FreeBoolean05 As Boolean
    Public Property p56FreeBoolean06 As Boolean
    Public Property p56FreeBoolean07 As Boolean
    Public Property p56FreeBoolean08 As Boolean
    Public Property p56FreeBoolean09 As Boolean
    Public Property p56FreeBoolean10 As Boolean

    Public Property p56FreeDate01 As DateTime?
    Public Property p56FreeDate02 As DateTime?
    Public Property p56FreeDate03 As DateTime?
    Public Property p56FreeDate04 As DateTime?
    Public Property p56FreeDate05 As DateTime?
    Public Property p56FreeDate06 As DateTime?
    Public Property p56FreeDate07 As DateTime?
    Public Property p56FreeDate08 As DateTime?
    Public Property p56FreeDate09 As DateTime?
    Public Property p56FreeDate10 As DateTime?

    Public Property p56FreeNumber01 As Double
    Public Property p56FreeNumber02 As Double
    Public Property p56FreeNumber03 As Double
    Public Property p56FreeNumber04 As Double
    Public Property p56FreeNumber05 As Double
    Public Property p56FreeNumber06 As Double
    Public Property p56FreeNumber07 As Double
    Public Property p56FreeNumber08 As Double
    Public Property p56FreeNumber09 As Double
    Public Property p56FreeNumber10 As Double

    Public Property p56FreeCombo01 As Integer?
    Public Property p56FreeCombo02 As Integer?
    Public Property p56FreeCombo03 As Integer?
    Public Property p56FreeCombo04 As Integer?
    Public Property p56FreeCombo05 As Integer?
    Public Property p56FreeCombo06 As Integer?
    Public Property p56FreeCombo07 As Integer?
    Public Property p56FreeCombo08 As Integer?
    Public Property p56FreeCombo09 As Integer?
    Public Property p56FreeCombo10 As Integer?

    Public Property p56FreeCombo01Text As String = Nothing
    Public Property p56FreeCombo02Text As String = Nothing
    Public Property p56FreeCombo03Text As String = Nothing
    Public Property p56FreeCombo04Text As String = Nothing
    Public Property p56FreeCombo05Text As String = Nothing
    Public Property p56FreeCombo06Text As String = Nothing
    Public Property p56FreeCombo07Text As String = Nothing
    Public Property p56FreeCombo08Text As String = Nothing
    Public Property p56FreeCombo09Text As String = Nothing
    Public Property p56FreeCombo10Text As String = Nothing
End Class
