Public Enum SslModeENUM
    _NoSSL = 0
    Implicit = 1
    Explicit = 2
End Enum

Public Class o40SmtpAccount
    Inherits BOMother
    Public Property o40Name As String
    Public Property o40EmailAddress As String
    Public Property o40Server As String
    Public Property o40Port As String
    Public Property o40Login As String
    Public Property o40Password As String
    Public Property o40IsVerify As Boolean
    Public Property o40SslModeFlag As SslModeENUM = SslModeENUM._NoSSL
    Public Property o40IsGlobalDefault As Boolean

    Public ReadOnly Property DecryptedPassword As String
        Get
            Return BO.Crypto.Decrypt(Me.o40Password, "o40SmtpAccount")
        End Get
    End Property
End Class
