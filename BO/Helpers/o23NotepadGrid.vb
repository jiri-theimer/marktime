Public Class o23NotepadGrid
    Inherits o23Notepad
    Public Property p41Name As String
    Public Property p41Code As String
    Public Property ProjectClient As String
    Public Property p28Name As String
    Public Property p28CompanyName As String
    Public Property Person As String
    Public Property p57Name As String
    Public Property p56Code As String
    Public Property p91Code As String
    Public Property p92Name As String

    Public ReadOnly Property Project As String
        Get
            If Me.ProjectClient = "" Then
                Return p41Name
            Else
                Return Me.ProjectClient & " - " & Me.p41Name
            End If
        End Get
    End Property

    
End Class
