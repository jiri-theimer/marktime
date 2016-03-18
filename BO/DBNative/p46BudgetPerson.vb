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
    Public Property p46BillingRate As Double
    Public Property j27ID_BillingRate As Integer
    Public Property p46CostRate As Double
    Public Property j27ID_CostRate As Integer

    Public Property IsSetAsDeleted As Boolean

    Private Property _Person As String
    Public ReadOnly Property Person As String
        Get
            Return _Person
        End Get
    End Property

    Public ReadOnly Property CostAmount As Double
        Get
            Return Me.p46CostRate * Me.p46HoursTotal
        End Get
    End Property
    Public ReadOnly Property BillingAmount As Double
        Get
            Return Me.p46BillingRate * Me.p46HoursBillable
        End Get
    End Property
End Class
