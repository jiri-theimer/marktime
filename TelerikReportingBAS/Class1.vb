﻿Public NotInheritable Class Class1
    Public Shared Function ShowAsHHMM(dblHours As Double) As String

        Dim cT As New BO.clsTime
        Return cT.GetTimeFromSeconds(dblHours * 60 * 60)

    End Function
End Class