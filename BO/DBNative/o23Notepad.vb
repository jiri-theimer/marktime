Public Enum o23LockedTypeENUM
    _NotSpecified = 0
    LockAllFiles = 1

End Enum
Public Class o23Notepad
    Inherits BOMother
    Public Property o24ID As Integer
    Public Property b02ID As Integer
    Public Property p41ID As Integer
    Public Property p28ID As Integer
    Public Property p91ID As Integer
    Public Property j02ID As Integer
    Public Property p56ID As Integer
    Public Property p31ID As Integer

    Public Property o23IsDraft As Boolean

    Public Property o23Date As Date
    Public Property o23ReminderDate As Date?
    Public Property o23Name As String
    Public Property o23Code As String
    Public Property j02ID_Owner As Integer
    Public Property o23IsEncrypted As Boolean
    Public Property o23GUID As String

    Friend Property _o24IsBillingMemo As Boolean

    Public ReadOnly Property o24IsBillingMemo As Boolean
        Get
            Return _o24IsBillingMemo
        End Get
    End Property
    Friend Property _x29ID As BO.x29IdEnum
    Public ReadOnly Property x29ID As BO.x29IdEnum
        Get
            Return _x29ID
        End Get
    End Property
    Friend Property _o43ID As Integer
    Public ReadOnly Property o43ID As Integer
        Get
            Return _o43ID
        End Get
    End Property
    Public Property o23Password As String
    Public Property o23BodyHtml As String
    Public Property o23SizeHtml As Integer
    Public Property o23SizePlainText As Boolean
    Public Property o23BodyPlainText As String
    Public Property o23SizePlainTexta As String

    Private Property _FilesCount As Integer
    Public ReadOnly Property FilesCount As Integer
        Get
            Return _FilesCount
        End Get
    End Property

    Friend Property _o24Name As String
    Public ReadOnly Property o24Name As String
        Get
            Return _o24Name
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

    Friend Property _Owner As String
    Public ReadOnly Property Owner As String
        Get
            Return _Owner
        End Get
    End Property

    Friend Property _o23LockedFlag As o23LockedTypeENUM
    Public ReadOnly Property o23LockedFlag As o23LockedTypeENUM
        Get
            Return _o23LockedFlag
        End Get
    End Property
    Friend Property _o23LastLockedWhen As Date?
    Public ReadOnly Property o23LastLockedWhen As Date?
        Get
            Return _o23LastLockedWhen
        End Get
    End Property
    Friend Property _o23LastLockedBy As String
    Public ReadOnly Property o23LastLockedBy As String
        Get
            Return _o23LastLockedBy
        End Get
    End Property


    '----uživatelská pole--------------------
    Public Property o23FreeText01 As String
    Public Property o23FreeText02 As String
    Public Property o23FreeText03 As String
    Public Property o23FreeText04 As String
    Public Property o23FreeText05 As String
    Public Property o23FreeText06 As String
    Public Property o23FreeText07 As String
    Public Property o23FreeText08 As String
    Public Property o23FreeText09 As String
    Public Property o23FreeText10 As String

    Public Property o23FreeBoolean01 As Boolean
    Public Property o23FreeBoolean02 As Boolean
    Public Property o23FreeBoolean03 As Boolean
    Public Property o23FreeBoolean04 As Boolean
    Public Property o23FreeBoolean05 As Boolean
    Public Property o23FreeBoolean06 As Boolean
    Public Property o23FreeBoolean07 As Boolean
    Public Property o23FreeBoolean08 As Boolean
    Public Property o23FreeBoolean09 As Boolean
    Public Property o23FreeBoolean10 As Boolean

    Public Property o23FreeDate01 As DateTime?
    Public Property o23FreeDate02 As DateTime?
    Public Property o23FreeDate03 As DateTime?
    Public Property o23FreeDate04 As DateTime?
    Public Property o23FreeDate05 As DateTime?
    Public Property o23FreeDate06 As DateTime?
    Public Property o23FreeDate07 As DateTime?
    Public Property o23FreeDate08 As DateTime?
    Public Property o23FreeDate09 As DateTime?
    Public Property o23FreeDate10 As DateTime?

    Public Property o23FreeNumber01 As Double
    Public Property o23FreeNumber02 As Double
    Public Property o23FreeNumber03 As Double
    Public Property o23FreeNumber04 As Double
    Public Property o23FreeNumber05 As Double
    Public Property o23FreeNumber06 As Double
    Public Property o23FreeNumber07 As Double
    Public Property o23FreeNumber08 As Double
    Public Property o23FreeNumber09 As Double
    Public Property o23FreeNumber10 As Double

    Public Property o23FreeCombo01 As Integer?
    Public Property o23FreeCombo02 As Integer?
    Public Property o23FreeCombo03 As Integer?
    Public Property o23FreeCombo04 As Integer?
    Public Property o23FreeCombo05 As Integer?
    Public Property o23FreeCombo06 As Integer?
    Public Property o23FreeCombo07 As Integer?
    Public Property o23FreeCombo08 As Integer?
    Public Property o23FreeCombo09 As Integer?
    Public Property o23FreeCombo10 As Integer?
    Public Property o23FreeCombo01Text As String = Nothing
    Public Property o23FreeCombo02Text As String = Nothing
    Public Property o23FreeCombo03Text As String = Nothing
    Public Property o23FreeCombo04Text As String = Nothing
    Public Property o23FreeCombo05Text As String = Nothing
    Public Property o23FreeCombo06Text As String = Nothing
    Public Property o23FreeCombo07Text As String = Nothing
    Public Property o23FreeCombo08Text As String = Nothing
    Public Property o23FreeCombo09Text As String = Nothing
    Public Property o23FreeCombo10Text As String = Nothing
End Class
