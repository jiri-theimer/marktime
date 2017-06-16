Public Class x25EntityField_ComboValue
    Inherits BOMother
    Public Property x23ID As Integer
    Public Property x25Name As String
    Public Property x25Ordinary As Integer
    Public Property x25Code As String
    Public Property x25BackColor As String
    Public Property x25ForeColor As String

    Public Property x25FreeText01 As String
    Public Property x25FreeText02 As String
    Public Property x25FreeText03 As String
    Public Property x25FreeText04 As String
    Public Property x25FreeText05 As String
    Public Property x25FreeNumber01 As Double
    Public Property x25FreeNumber02 As Double
    Public Property x25FreeNumber03 As Double
    Public Property x25FreeNumber04 As Double
    Public Property x25FreeNumber05 As Double
    Public Property x25FreeDate01 As Date?
    Public Property x25FreeDate02 As Date?
    Public Property x25FreeDate03 As Date?
    Public Property x25FreeDate04 As Date?
    Public Property x25FreeDate05 As Date?


    Private Property _x23Name As String
    Public ReadOnly Property x23Name As String
        Get
            Return _x23Name
        End Get
    End Property

    Public ReadOnly Property NameWithComboName As String
        Get
            Return _x23Name & ": " & Me.x25Name
        End Get
    End Property
    Public ReadOnly Property StyleDecoration As String
        Get
            If Me.IsClosed Then Return "line-through" Else Return ""
        End Get
    End Property
End Class
