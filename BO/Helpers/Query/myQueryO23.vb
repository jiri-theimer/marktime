Public Enum myQueryO23_SpecificQuery
    _NotSpecified = 0
    AllowedForRead = 2              'pouze dokumenty, ke kterým má právo na čtení

End Enum
Public Enum myQueryO23_QuickQuery
    _NotSpecified = 0
    OpenNotepads = 1
    Removed2Bin = 2
    Bind2ProjectExist = 3
    Bind2ProjectWait = 4
    Bind2ClientExist = 5
    Bind2ClientWait = 6
    Bind2InvoiceExist = 7
    Bind2InvoiceWait = 8
    Bind2WorksheetExist = 9
    Bind2WorksheetWait = 10
    Bind2PersonExist = 11
    Bind2PersonWait = 12
End Enum
Public Class myQueryO23
    Inherits myQuery
    Public Property j02ID_ExplicitQueryFor As Integer
    Public Property o24ID As Integer
    Public Property p41ID As Integer
    Public Property p91ID As Integer
    Public Property p28ID As Integer
    Public Property j02ID As Integer
    Public Property p56ID As Integer
    Public Property p31ID As Integer

    Public Property b02ID As Integer

    Public Property j70ID As Integer

    Public SpecificQuery As myQueryO23_SpecificQuery = myQueryO23_SpecificQuery._NotSpecified
    Public QuickQuery As myQueryO23_QuickQuery = myQueryO23_QuickQuery._NotSpecified

    Public Property o23GUID As String
    Public Property MG_SelectPidFieldOnly As Boolean
End Class
