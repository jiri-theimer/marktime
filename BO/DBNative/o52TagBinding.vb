Public Class o52TagBinding
    Public Property o52ID As Integer
    Public Property o51ID As Integer
    Public Property o52RecordPID As Integer
    Public Property x29ID As BO.x29IdEnum = x29IdEnum._NotSpecified
    Public Property o52DateInsert As Date
    Public Property o52UserInsert As String

    Private Property _o51Name As String
    Public ReadOnly Property o51Name As String
        Get
            Return _o51Name
        End Get
    End Property
End Class
