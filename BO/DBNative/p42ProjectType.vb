Public Enum p42ArchiveFlagENUM
    NoLimit = 0
    NoArchive_Waiting_Invoice = 1
    NoArchive_Waiting_Approve = 2
End Enum
Public Class p42ProjectType
    Inherits BOMother
    Public Property b01ID As Integer
    Public Property x38ID As Integer
    Public Property x38ID_Draft As Integer
    Public Property p42Name As String
    Public Property p42Code As String
    Public Property p42IsDefault As Boolean
    Public Property p42Ordinary As Integer

    Public Property p42ArchiveFlag As p42ArchiveFlagENUM = p42ArchiveFlagENUM.NoLimit

    Private Property _b01Name As String
    Public ReadOnly Property b01Name As String
        Get
            Return _b01Name
        End Get
    End Property
End Class
