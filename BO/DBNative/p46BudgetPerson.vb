Public Enum p46ExceedFlagENUM
    StrictFaStrictNefa = 1
    StrictTotal = 2
    StrictFaFreeNefa = 3
    StrictNeFaFreeFa = 4
    NoLimit = 5
End Enum
Public Class p46BudgetPerson
    Inherits BOMother
    Public Property j02ID As Integer
    Public Property p45ID As Integer
    Public Property p46HoursBillable As Double
    Public Property p46HoursNonBillable As Double
    Public Property p46HoursTotal As Double
    Public Property p46ExceedFlag As p46ExceedFlagENUM = p46ExceedFlagENUM.NoLimit
    Public Property p46Description As String

    Public Property IsSetAsDeleted As Boolean

    Private Property _Person As String
    Public ReadOnly Property Person As String
        Get
            Return _Person
        End Get
    End Property
End Class
