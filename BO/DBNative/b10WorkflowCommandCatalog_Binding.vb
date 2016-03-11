Public Class b10WorkflowCommandCatalog_Binding
    Public Property b10ID As Integer
    Public Property b09ID As Integer
    Public Property b02ID As Integer
    Public Property b06ID As Integer
    Private Property _b09Name As String
    Private Property _b09SQL As String
    Private Property _b09Ordinary As Integer
    
    Public ReadOnly Property b09Name As String
        Get
            Return _b09Name
        End Get
    End Property
    Public ReadOnly Property b09SQL As String
        Get
            Return _b09SQL
        End Get
    End Property
    Public ReadOnly Property b09Ordinary As Integer
        Get
            Return _b09Ordinary
        End Get
    End Property
    

End Class
