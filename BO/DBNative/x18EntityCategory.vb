Public Class x18EntityCategory
    Inherits BOMother
    Public Property x23ID As Integer
    Public Property j02ID_Owner As Integer
    Public Property x18Name As String
    Public Property x18IsMultiSelect As Boolean
    Public Property x18Ordinary As Integer

    Private Property _x23Name As String
    Public Property x18IsAllEntityTypes As Boolean
    Public Property x18IsRequired As Boolean


    Public ReadOnly Property x23Name As String
        Get
            Return _x23Name
        End Get
    End Property
    Private Property _Owner As String
    Public ReadOnly Property Owner As String
        Get
            Return _Owner
        End Get
    End Property

    Private Property _Is_p41 As Boolean
    Public ReadOnly Property Is_p41 As Boolean
        Get
            Return _Is_p41
        End Get
    End Property
    Private Property _Is_p28 As Boolean
    Public ReadOnly Property Is_p28 As Boolean
        Get
            Return _Is_p28
        End Get
    End Property
    Private Property _Is_p31 As Boolean
    Public ReadOnly Property Is_p31 As Boolean
        Get
            Return _Is_p31
        End Get
    End Property
    Private Property _Is_j02 As Boolean
    Public ReadOnly Property Is_j02 As Boolean
        Get
            Return _Is_j02
        End Get
    End Property
    Private Property _Is_o23 As Boolean
    Public ReadOnly Property Is_o23 As Boolean
        Get
            Return _Is_o23
        End Get
    End Property
    Private Property _Is_p91 As Boolean
    Public ReadOnly Property Is_p91 As Boolean
        Get
            Return _Is_p91
        End Get
    End Property
    Private Property _Is_p56 As Boolean
    Public ReadOnly Property Is_p56 As Boolean
        Get
            Return _Is_p56
        End Get
    End Property
End Class
