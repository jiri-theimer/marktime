
Public Enum myQueryP31_QuickQuery
    _NotSpecified = 0
    Editing = 1     'rozpracované
    Approved = 2    'prošlo schvalováním
    Invoiced = 3    'prošlo fakturací
    MovedToBin = 4  'přesunuto do koše
    EditingOrMovedToBin = 5   'rozpracované nebo v koši
End Enum
Public Enum myQueryP31_SpecificQuery
    _NotSpecified = 0
    AllowedForRead = 1          'pouze úkony, ke kterým mám právo na čtení
    AllowedForDoApprove = 2     'pouze rozpracované úkony, které mohu schvalovat
    AllowedForReApprove = 3       'schválené úkony, které dosud nejsou vyfakturovány
    AllowedForCreateInvoice = 4     'pouze úkony, které mohu vyfakturovat
End Enum
Public Class myQueryP31
    Inherits myQuery
    Public Property j70ID As Integer
    Public Property p41ID As Integer
    Public Property p41IDs As List(Of Integer)
    Public Property p28ID As Integer
    Public Property p28IDs As List(Of Integer)

    Public Property p56IDs As List(Of Integer)
    Public Property o22ID As Integer
    Public Property j02ID As Integer
    Public Property j02IDs As List(Of Integer)
    Public Property p34ID As Integer
    Public Property p34IDs As List(Of Integer)
    Public Property p32ID As Integer

    Public Property p91ID As Integer

    Public Property p70ID As p70IdENUM?
    Public Property p71ID As p71IdENUM?
    Public Property p72ID As p72IdENUM?

    Public Property IsExpenses As Boolean? = Nothing



    Public Property Billable As BooleanQueryMode = BooleanQueryMode.NoQuery

    Public Property QuickQuery As myQueryP31_QuickQuery = myQueryP31_QuickQuery._NotSpecified
    Public Property SpecificQuery As myQueryP31_SpecificQuery = myQueryP31_SpecificQuery._NotSpecified
    Public Property j02ID_ExplicitQueryFor As Integer
End Class
