Public Class DateTimeByQuerystring
    Public Property DateOnly As Date
    Public Property TimeOnly As String

    Public Sub New(QueryStringExpression As String)
        Dim a() As String = Split(QueryStringExpression, "_")
        Me.DateOnly = BO.BAS.ConvertString2Date(a(0))
        a = Split(a(1), ".")
        Me.TimeOnly = Right("0" & a(0), 2) & ":" & Right("0" & a(1), 2)

    End Sub
End Class
