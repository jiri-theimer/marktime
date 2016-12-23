Public Class OtherQueryItem
    Public Property pid As Integer
    Public Property Text As String
    Public Property IsClosed As Boolean
    Public Sub New(intPID As Integer, strText As String)
        Me.pid = intPID
        Me.Text = strText
    End Sub
End Class
Public Class j70QueryTemplate
    Inherits BOMother
    Public Property j70Name As String
    Public Property x29ID As x29IdEnum
    Public Property j03ID As Integer
    Public Property j02ID_Owner As Integer
    Public Property j70IsSystem As Boolean
    Public Property j70BinFlag As Integer
    Public Property j70IsNegation As Boolean

    Private Property _Mark As String
    Public ReadOnly Property NameWithMark As String
        Get
            Return Trim(j70Name & " " & _Mark)
        End Get
    End Property
    Public ReadOnly Property NameWithCreator As String
        Get
            Return j70Name & " (" & Me.UserInsert & ")"
        End Get
    End Property
End Class
