Public Enum NoticeBoardLocality
    WelcomePage = 1

End Enum
Public Class o10NoticeBoard
    Inherits BOMother
    Public Property j02ID_Owner As Integer
    Public Property o10Name As String
    Public Property o10BodyHtml As String
    Public Property o10BodyPlainText As String
    Public Property o10Ordinary As Integer
    Public Property o10BackColor As String
    Public Property o10Locality As NoticeBoardLocality?

    Public Property Owner As String
    Public ReadOnly Property CssClassBox As String
        Get
            If Me.IsClosed Then
                Return "noticeboard-box-bin"
            Else
                Return "noticeboard-box"
            End If
        End Get
    End Property
    Public Property StyleDisplayEdit As String = "none"
End Class
