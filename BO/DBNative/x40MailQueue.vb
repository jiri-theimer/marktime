Public Enum x40StateENUM
    _NotSpecified = 0
    InQueque = 1
    IsError = 2
    IsProceeded = 3
    IsStopped = 4
End Enum

Public Class x40MailQueue
    Inherits BOMother
    Public Property x29ID As BO.x29IdEnum
    Public Property x40State As x40StateENUM = x40StateENUM.InQueque
    Public Property x40RecordPID As Integer
    Public Property j03ID_Sys As Integer

    Public Property x40Subject As String
    Public Property x40Body As String
    Public Property x40IsHtmlBody As Boolean

    Public Property x40SenderName As String
    Public Property x40SenderAddress As String
    Public Property x40Recipient As String
    
    Public Property x40CC As String
    
    Public Property x40BCC As String
    
    Public Property x40Attachments As String


    Public Property x40WhenProceeded As Date?

    Public Property x40ErrorMessage As String

    Public Property x40IsAutoNotification As Boolean
End Class
