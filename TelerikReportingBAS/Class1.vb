Public NotInheritable Class Class1
    Public Shared Function ShowAsHHMM(dblHours As Double?) As String
        If dblHours Is Nothing Then Return ""

        Dim cT As New BO.clsTime
        Return cT.GetTimeFromSeconds(CInt(dblHours * 60 * 60))

    End Function

    Public Shared Function GetSubstring(strDelimiter As String, strString As String, intZeroIndex As Integer) As String
        If strString = "" Then Return ""
        Dim a() As String = Split(strString, strDelimiter)
        If UBound(a) < intZeroIndex Then
            Return strString
        Else
            Return a(intZeroIndex)
        End If
    End Function
    Public Shared Function GetDateFormat(d As Date, strFormat As String) As String
        If strFormat = "" Then strFormat = "dd.MM.yyyy"
        Return Format(d, strFormat)
    End Function
End Class
