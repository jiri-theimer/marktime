Public Class p47CapacityPlan
    Inherits BOMother
    Public Property j02ID As Integer
    Public Property p41ID As Integer
    Public Property p56ID As Integer
    Public Property p47DateFrom As Date
    Public Property p47DateUntil As Date
    Public Property p47HoursBillable As Double
    Public Property p47HoursNonBillable As Double
    Public Property p47HoursTotal As Double

    Private Property _Person As String
    Public ReadOnly Property Person As String
        Get
            Return _Person
        End Get
    End Property
    Private Property _Project As String
    Public ReadOnly Property Project As String
        Get
            Return _Project
        End Get
    End Property
    Private Property _setAsDeleted As Boolean
    Public Sub SetAsDeleted()
        _setAsDeleted = True
    End Sub
    Public ReadOnly Property IsSetAsDeleted As Boolean
        Get
            Return _setAsDeleted
        End Get
    End Property
End Class
