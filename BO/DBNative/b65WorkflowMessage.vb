Public Class b65WorkflowMessage
    Inherits BOMother
    Public Property x29ID As BO.x29IdEnum
    Public Property b65Name As String
    Public Property b65MessageSubject As String
    Public Property b65MessageBody As String
    
    Private Property _x29Name As String

    Public ReadOnly Property x29Name As String
        Get
            Return _x29Name
        End Get

    End Property

End Class
