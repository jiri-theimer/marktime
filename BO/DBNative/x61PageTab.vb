Public Class x61PageTab
    Public Property x61ID As Integer
    Public Property x61Code As String
    Public Property x29ID As BO.x29IdEnum
    Public Property x61Name As String
    Public Property x61Ordinary As Integer
    Private Property _URL As String

    Public Function GetPageUrl(strMasterPrefix As String, intMasterPID As Integer, strIsApprovingPerson As String) As String
        Dim s As String = "masterprefix=" & strMasterPrefix & "&masterpid=" & intMasterPID.ToString
        If strIsApprovingPerson <> "" Then s += "&IsApprovingPerson=" & strIsApprovingPerson
        Select Case x61Code
            Case "summary"
                Return "entity_framework_p31summary.aspx?" & s
            Case "p31"
                Return "entity_framework_p31subform.aspx?" & s
            Case "time", "expense", "fee", "kusovnik"
                Return "entity_framework_p31subform.aspx?p31tabautoquery=" & x61Code & "&" & s
            Case "p56"
                Return "entity_framework_p56subform.aspx?" & s
            Case "p91"
                Return "entity_framework_p91subform.aspx?" & s
            Case "p45"
                Return "p41_framework_detail_budget.aspx?" & s
            Case "workflow"
                Return "entity_framework_b07subform.aspx?" & s
            Case Else
                Return _URL
        End Select
    End Function

End Class
