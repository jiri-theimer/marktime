Public Class j23NonPerson
    Inherits BOMother
    Public Property j24ID As Integer
    Public Property j23Name As String
    Public Property j23Code As String
    Public Property j23Ordinary As Integer

    Private Property _j24Name As String
    Public ReadOnly Property j24Name As String
        Get
            Return _j24Name
        End Get
    End Property
    Private Property _j24Ordinary As Integer
    Public ReadOnly Property j24Ordinary As Integer
        Get
            Return _j24Ordinary
        End Get
    End Property
  

    Public ReadOnly Property NameWithCode As String
        Get
            If Me.j23Code = "" Then
                Return Me.j23Name
            Else
                Return Me.j23Name & " (" & Me.j23Code & ")"
            End If
        End Get
    End Property
End Class
