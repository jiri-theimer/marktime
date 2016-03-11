Public Enum x90EventFlagEnum
    Created = 1
    Updated = 2
    MovedToBin = 3
    RestoreFromBin = 4
End Enum
Public Class x90EntityLog
    Public Property x90ID As Integer
    Public Property x29ID As BO.x29IdEnum
    Public Property x90Date As Date
    Public Property x90EventFlag As x90EventFlagEnum
    Public Property x90RecordPID As Integer
    Public Property x90RecordValidFrom As Date
    Public Property x90RecordValidUntil As Date
    Public Property j03ID_Author As Integer
    Public Property j02ID_Author As Integer

    Public Property Person As String
End Class
