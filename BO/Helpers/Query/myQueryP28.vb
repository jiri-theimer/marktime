Public Enum myQueryP28_SpecificQuery
    _NotSpecified = 0

    AllowedForRead = 2              'pouze kontakty, ke kterým má právo na čtení
End Enum
Public Enum myQueryP28_QuickQuery
    _NotSpecified = 0
    OpenClients = 1
    Removed2Bin = 2
    WaitingOnApproval = 3
    OverWorksheetLimit = 4
    WaitingOnInvoice = 5
    ProjectClient = 6
    ProjectInvoiceReceiver = 7
    DraftClients = 11
    NonDraftCLients = 12
    WithContactPersons = 16
    WithoutContactPersons = 17
    WithProjects = 18
    WithoutProjects = 19
    WIthNotepad = 20
End Enum
Public Class myQueryP28
    Inherits myQuery

    Public Property p29ID As Integer
    Public Property j02ID As Integer
    Public Property b02ID As Integer
    Public Property j70ID As Integer
    Public Property MG_SelectPidFieldOnly As Boolean
    Public QuickQuery As myQueryP28_QuickQuery = myQueryP28_QuickQuery._NotSpecified
    Public SpecificQuery As myQueryP28_SpecificQuery = myQueryP28_SpecificQuery._NotSpecified
    Public Property j02ID_ExplicitQueryFor As Integer = 0   'ID osoby, z jejíhož pohledu se mají odfiltrovat kontakty, pokud je 0, pak přihlášený uživatel
End Class
