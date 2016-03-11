Public Class p57TaskType
    Inherits BOMother
    Public Property b01ID As Integer
    Public Property x38ID As Integer
    Public Property p57Name As String
    Public Property p57Code As String
    Public Property p57IsDefault As Boolean
    Public Property p57IsHelpdesk As Boolean
    Public Property p57Ordinary As Integer

    Private Property _b01Name As String
    Public ReadOnly Property b01Name As String
        Get
            Return _b01Name
        End Get
    End Property
End Class
