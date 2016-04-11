Public Class x19EntityCategory_Binding
    Inherits BOMother
    Public Property x18ID As Integer
    Public Property x25ID As Integer
    Public Property x19RecordPID As Integer

    Private Property _x18Name As String
    Public ReadOnly Property x18Name As String
        Get
            Return _x18Name
        End Get
    End Property
    Private Property _x25Name As String
    Public ReadOnly Property x25Name As String
        Get
            Return _x25Name
        End Get
    End Property
End Class
