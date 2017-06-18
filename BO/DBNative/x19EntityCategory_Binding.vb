Public Class x19EntityCategory_Binding
    Inherits BOMother
    Public Property x20ID As Integer
    Public Property x25ID As Integer

    Public Property x19RecordPID As Integer

    Private Property _x18ID As Integer
    Public ReadOnly Property x18ID As Integer
        Get
            Return _x18ID
        End Get
    End Property
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
    Private Property _NameWithCode As String
    Public ReadOnly Property NameWithCode As String
        Get
            Return _NameWithCode
        End Get
    End Property
    Private Property _BackColor As String
    Public ReadOnly Property BackColor As String
        Get
            Return _BackColor
        End Get
    End Property
    Private Property _ForeColor As String
    Public ReadOnly Property ForeColor As String
        Get
            Return _ForeColor
        End Get
    End Property
End Class
