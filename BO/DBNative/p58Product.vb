Public Class p58Product
    Inherits BOMother
    Public Property p58ParentID As Integer
    Public Property p58Name As String
    Public Property p58Code As String
    Public Property p58Ordinary As Integer
    Private Property _p58TreeIndex As Integer
    Public ReadOnly Property p58TreeIndex As Integer
        Get
            Return _p58TreeIndex
        End Get
    End Property
    Private Property _p58TreePrev As Integer
    Public ReadOnly Property p58TreePrev As Integer
        Get
            Return _p58TreePrev
        End Get
    End Property
    Private Property _p58TreeNext As Integer
    Public ReadOnly Property p58TreeNext As Integer
        Get
            Return _p58TreeNext
        End Get
    End Property
    Private Property _p58TreeLevel As Integer
    Public ReadOnly Property p58TreeLevel As Integer
        Get
            Return _p58TreeLevel
        End Get
    End Property

    Public ReadOnly Property TreeMenuItem As String
        Get
            If _p58TreeLevel <= 1 Then
                Return Me.p58Name
            Else
                Return Space(_p58TreeLevel * 3).Replace(" ", "-") + Me.p58Name
            End If
        End Get
    End Property
End Class
