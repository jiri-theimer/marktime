Public Class x18EntityCategory
    Inherits BOMother
    Public Property x23ID As Integer
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
End Class
