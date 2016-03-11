Public Class o41InboxAccount
    Inherits BOMother
    Public Property o41Name As String
    Public Property o41Server As String
    Public Property o41Port As String
    Public Property o41Login As String
    Public Property o41Password As String
    Public Property o41Folder As String = "inbox"
    Public Property o41IsUseSSL As Boolean
    Public Property o41IsDeleteMesageAfterImport As Boolean
    Public Property o41DateImportAfter As Date?

    Public ReadOnly Property DecryptedPassword As String
        Get
            Return BO.Crypto.Decrypt(Me.o41Password, "o41InboxAccount")
        End Get
    End Property
End Class
