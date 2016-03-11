Public Enum myQueryP41_SpecificQuery
    _NotSpecified = 0
    AllowedForWorksheetEntry = 1    'pouze projekty, kam lze zapisovat worksheet
    AllowedForRead = 2              'pouze projekty, ke kterým má právo na čtení
    AllowedForCreateTask = 3          'projekty, v kterých lze vytvořit úkol
    AllowedForCreateInvoice = 4       'pouze projekty, kde má právo vystavit fakturu
End Enum
Public Enum myQueryP41_QuickQuery
    _NotSpecified = 0
    OpenProjects = 1
    Removed2Bin = 2
    WaitingOnApproval = 3
    OverWorksheetLimit = 4
    WaitingOnInvoice = 5
    WithOpenTasks = 6
    WithAnyTasks = 7
    WithFutureMilestones = 8
    WithNotepad = 9
    WithRecurrenceWorksheet = 10
    DraftProjects = 11
    NonDraftProjects = 12
    WithoutPricelist = 13
    WithPricelist = 14
    Invoiced = 15
    WithContactPersons = 16
    WithoutContactPersons = 17
End Enum
Public Class myQueryP41
    Inherits myQuery

    Public Property MG_SelectPidFieldOnly As Boolean = False  'true - bude vrace pouze sloupec s primárním klíčem
    Public Property p28ID As Integer
    Public Property p42ID As Integer

    Public Property b02ID As Integer
    Public Property p51ID As Integer
    Public Property p41PlanFrom_D1 As Date?
    Public Property p41PlanFrom_D2 As Date?
    Public Property p41PlanUntil_D1 As Date?
    Public Property p41PlanUntil_D2 As Date?

    Public Property j70ID As Integer
    Public Property p41WorksheetOperFlag As BO.p41WorksheetOperFlagEnum = p41WorksheetOperFlagEnum._NotSpecified

    Public SpecificQuery As myQueryP41_SpecificQuery = myQueryP41_SpecificQuery._NotSpecified
    Public QuickQuery As myQueryP41_QuickQuery = myQueryP41_QuickQuery._NotSpecified
    
    Public Property j02ID_ExplicitQueryFor As Integer = 0   'ID osoby, z jejíhož pohledu se mají odfiltrovat projekty, pokud je 0, pak přihlášený uživatel
    Public Property j02ID_ContactPerson As Integer          'Kontaktní osoba projektu nebo klienta projektu
End Class
