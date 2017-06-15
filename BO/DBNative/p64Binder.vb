Public Class p64Binder
    Inherits BOMother
    Public Property p41ID As Integer
    Public Property j02ID_Owner As Integer
    Public Property p64Name As String
    Public Property p64Code As String
    Public Property p64ArabicCode As String
    Public Property p64Location As String
    Public Property p64Ordinary As Integer
    Public Property p64Description As String
    Private Property _Owner As String
    Public ReadOnly Property Owner As String
        Get
            Return _Owner
        End Get
    End Property
    Private Property _Project As String
    Public ReadOnly Property Project As String
        Get
            Return _Project
        End Get
    End Property
    Private Property _Client As String
    Public ReadOnly Property Client As String
        Get
            Return _Client
        End Get
    End Property

End Class
