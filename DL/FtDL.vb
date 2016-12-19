'výpisy z fixních číselníků
Public Class FtDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function GetList_Emails(strFilterExpression As String, intTOP As Integer) As IEnumerable(Of BO.GetString)
        strFilterExpression = Trim(strFilterExpression)
        Dim s As String = "select TOP " & intTOP.ToString & " Adresa as Value FROM dbo.view_Emails"
        If strFilterExpression <> "" Then s += " WHERE Adresa LIKE '%'+@expr+'%'"
        s += " ORDER BY Adresa"
        Dim pars As New DbParameters
        pars.Add("expr", strFilterExpression, DbType.String)
        Return _cDB.GetList(Of BO.GetString)(s, pars)
    End Function
    Public Function GetList_X53(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x53Permission)
        Dim s As String = "select *," & bas.RecTail("x53", , True, False) & " FROM x53Permission WHERE " & bas.RecValiditySqlWhere("x53", "", mq)
        s += bas.ParseWhereMultiPIDs("x53id", mq)
        s += " ORDER BY x29ID,x53Ordinary"

        Return _cDB.GetList(Of BO.x53Permission)(s)
    End Function

    Public Function GetList_X29(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x29Entity)
        Dim s As String = "select *," & bas.RecTail("x29", , True, False) & " FROM x29Entity WHERE " & bas.RecValiditySqlWhere("x29", "", mq)
        s += bas.ParseWhereMultiPIDs("x29id", mq)

        Return _cDB.GetList(Of BO.x29Entity)(s)
    End Function

    Public Function GetList_X15(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x15VatRateType)
        Dim s As String = "select *," & bas.RecTail("x15", , True, False) & " FROM x15VatRateType WHERE " & bas.RecValiditySqlWhere("x15", "", mq)
        s += bas.ParseWhereMultiPIDs("x15id", mq)

        Return _cDB.GetList(Of BO.x15VatRateType)(s)
    End Function

    Public Function GetList_X21(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x21DatePeriod)
        Dim s As String = "select *," & bas.RecTail("x21", , True, False) & ",x21Name as _x21Name,x21id as _x21ID FROM x21DatePeriod WHERE " & bas.RecValiditySqlWhere("x21", "", mq)
        s += bas.ParseWhereMultiPIDs("x21", mq)

        Return _cDB.GetList(Of BO.x21DatePeriod)(s)
    End Function

    Public Function GetList_P71(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p71ApproveStatus)
        Dim s As String = "select *," & bas.RecTail("p71", , True, False) & ",p71ID as _p71ID FROM p71ApproveStatus WHERE " & bas.RecValiditySqlWhere("p71", "", mq)
        s += bas.ParseWhereMultiPIDs("p71", mq)

        Return _cDB.GetList(Of BO.p71ApproveStatus)(s)
    End Function
    Public Function GetList_P70(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p70BillingStatus)
        Dim s As String = "select *," & bas.RecTail("p70", , True, False) & ",p70ID as _p70ID FROM p70BillingStatus WHERE " & bas.RecValiditySqlWhere("p70", "", mq)
        s += bas.ParseWhereMultiPIDs("p70", mq)

        Return _cDB.GetList(Of BO.p70BillingStatus)(s)
    End Function
    Public Function GetList_P72(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p72PreBillingStatus)
        Dim s As String = "select *," & bas.RecTail("p72", , True, False) & ",p72ID as _p72ID FROM p72PreBillingStatus WHERE " & bas.RecValiditySqlWhere("p72", "", mq)
        s += bas.ParseWhereMultiPIDs("p72", mq)

        Return _cDB.GetList(Of BO.p72PreBillingStatus)(s)
    End Function
    Public Function GetList_P33(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p33ActivityInputType)
        Dim s As String = "select *," & bas.RecTail("p33", , True, False) & " FROM p33ActivityInputType WHERE " & bas.RecValiditySqlWhere("p33", "", mq)
        s += bas.ParseWhereMultiPIDs("p33", mq)

        Return _cDB.GetList(Of BO.p33ActivityInputType)(s)
    End Function
    Public Function GetList_P35(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p35Unit)
        Dim s As String = "select *," & bas.RecTail("p35", , True, False) & " FROM p35Unit WHERE " & bas.RecValiditySqlWhere("p35", "", mq)
        s += bas.ParseWhereMultiPIDs("p35", mq)

        Return _cDB.GetList(Of BO.p35Unit)(s)
    End Function
    Public Function GetList_X24(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x24DataType)
        Dim s As String = "select *," & bas.RecTail("x24", , True, False) & " FROM x24DataType WHERE " & bas.RecValiditySqlWhere("x24", "", mq)
        s += bas.ParseWhereMultiPIDs("x24", mq)

        Return _cDB.GetList(Of BO.x24DataType)(s)
    End Function
    Public Function GetList_P87(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.p87BillingLanguage)
        Dim s As String = "select *," & bas.RecTail("p87", , True, True) & " FROM p87BillingLanguage WHERE " & bas.RecValiditySqlWhere("p87", "", mq)
        s += bas.ParseWhereMultiPIDs("p87", mq)

        Return _cDB.GetList(Of BO.p87BillingLanguage)(s)
    End Function

    Public Sub SaveP87(lisP87 As List(Of BO.p87BillingLanguage))
        For Each c In lisP87
            Dim pars As New DbParameters
            pars.Add("p87Name", c.p87Name, DbType.String)
            pars.Add("p87Icon", c.p87Icon, DbType.String)
            pars.Add("pid", c.PID, DbType.Int32)
            _cDB.SaveRecord("p87BillingLanguage", pars, False, "p87ID=@pid", True, _curUser.j03Login)
        Next
    End Sub
    Public Function LoadP87(intP87ID As Integer) As BO.p87BillingLanguage
        Dim s As String = "select *," & bas.RecTail("p87") & " FROM p87BillingLanguage WHERE p87ID=@pid"
        Return _cDB.GetRecord(Of BO.p87BillingLanguage)(s, New With {.pid = intP87ID})
    End Function
    Public Function LoadX90(intX90ID As Integer) As BO.x90EntityLog
        Dim s As String = "select a.*,j02.j02LastName+' '+j02.j02FirstName as Person FROM x90EntityLog a INNER JOIN j02Person j02 ON a.j02ID_Author=j02.j02ID WHERE a.x90ID=@pid"
        Return _cDB.GetRecord(Of BO.x90EntityLog)(s, New With {.pid = intX90ID})
    End Function
    Public Function LoadJ27(intJ27ID As Integer) As BO.j27Currency
        Dim s As String = "select *," & bas.RecTail("j27") & " FROM j27Currency WHERE j27ID=@pid"
        Return _cDB.GetRecord(Of BO.j27Currency)(s, New With {.pid = intJ27ID})
    End Function

    Public Function GetList_J27(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.j27Currency)
        Dim s As String = "select *," & bas.RecTail("j27", , True, True) & " FROM j27Currency WHERE " & bas.RecValiditySqlWhere("j27", "", mq)
        s += bas.ParseWhereMultiPIDs("j27", mq)
        s += " ORDER BY j27Ordinary,j27ID"
        Return _cDB.GetList(Of BO.j27Currency)(s)
    End Function

    Public Function GetList_C11(datFrom As Date, datUntil As Date, levelFrom As BO.PeriodLevel, levelUntil As BO.PeriodLevel) As IEnumerable(Of BO.c11StatPeriod)
        Dim s As String = "select * FROM c11StatPeriod WHERE c11DateFrom>=@d1 AND c11DateUntil<=@d2"
        s += " AND c11Level>=@l1 AND c11Level<=@l2"
        s += " ORDER BY c11Ordinary"
        Dim pars As New DbParameters
        pars.Add("d1", datFrom, DbType.DateTime)
        pars.Add("d2", datUntil, DbType.DateTime)
        pars.Add("l1", levelFrom, DbType.Int32)
        pars.Add("l2", levelUntil, DbType.Int32)
        Return _cDB.GetList(Of BO.c11StatPeriod)(s, pars)
    End Function

    Public Function LoadX45(intX45ID As Integer) As BO.x45Event
        Dim s As String = "select *," & bas.RecTail("x45") & " FROM x45Event WHERE x45ID=@pid"
        Return _cDB.GetRecord(Of BO.x45Event)(s, New With {.pid = intX45ID})
    End Function
    Public Function GetList_X45(Optional mq As BO.myQuery = Nothing) As IEnumerable(Of BO.x45Event)
        Dim s As String = "select *," & bas.RecTail("x45", , True, True) & " FROM x45Event WHERE " & bas.RecValiditySqlWhere("x45", "", mq)
        s += bas.ParseWhereMultiPIDs("x45", mq)
        s += " ORDER BY x45Ordinary,x45ID"
        Return _cDB.GetList(Of BO.x45Event)(s)
    End Function
    Public Function GetList_X61(x29id As BO.x29IdEnum) As IEnumerable(Of BO.x61PageTab)
        Dim s As String = "SELECT x61ID,x61Name FROM x61PageTab WHERE x29ID=@x29id ORDER BY x61Ordinary"
        Return _cDB.GetList(Of BO.x61PageTab)(s, New With {.x29id = CInt(x29id)})
    End Function
End Class
