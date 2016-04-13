Public Enum myQueryP56_SpecificQuery
    _NotSpecified = 0
    AllowedForWorksheetEntry = 1    'pouze úkoly, kam lze zapisovat worksheet
    AllowedForRead = 2              'pouze úkoly, ke kterým má právo na čtení

End Enum

Public Enum myQueryP56_QuickQuery
    _NotSpecified = 0
    OpenTasks = 1
    Removed2Bin = 2
    WaitingOnApproval = 3
    WaitingOnInvoice = 5
End Enum
Public Class myQueryP56
    Inherits myQuery

    Public Property p57ID As Integer
    Public Property p58ID As Integer
    Public Property p41ID As Integer
    Public Property o22ID As Integer
    Public Property b02ID As Integer
    Public Property p28ID As Integer
    Public Property j70ID As Integer
    Public Property j02ID As Integer    'bráno z pohledu, kde je daná osoba příjemcem úkolu - nic víc
    Public Property p56PlanFrom_D1 As Date?
    Public Property p56PlanFrom_D2 As Date?
    Public Property p56PlanUntil_D1 As Date?
    Public Property p56PlanUntil_D2 As Date?

    Public SpecificQuery As myQueryP56_SpecificQuery = myQueryP56_SpecificQuery._NotSpecified
    Public Property j02ID_ExplicitQueryFor As Integer = 0   'ID osoby, z jejíhož pohledu se mají odfiltrovat úkoly, pokud je 0, pak přihlášený uživatel
    Public Property x25ID As Integer    'štítek
    Public Property MG_SelectPidFieldOnly As Boolean
End Class
