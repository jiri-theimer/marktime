Public Class o24NotepadType
    Inherits BOMother
    Public Property x29ID As BO.x29IdEnum
    Public Property b01ID As Integer
    Public Property x38ID As Integer
    Public Property x38ID_Draft As Integer
    Public Property o24Name As String
    Public Property o24Ordinary As Integer
    Public Property o24IsBillingMemo As Boolean
    Public Property o24IsEntityRelationRequired As Boolean
    Public Property o24HelpText As String
    Public Property o24IsAllowDropbox As Boolean
    Public Property o24MaxOneFileSize As Integer
    Public Property o24AllowedFileExtensions As String

    Private Property _b01Name As String
    Public ReadOnly Property b01Name As String
        Get
            Return _b01Name
        End Get
    End Property
    Private Property _x29Name As String
    Public ReadOnly Property x29Name As String
        Get
            Return _x29Name
        End Get
    End Property
End Class
