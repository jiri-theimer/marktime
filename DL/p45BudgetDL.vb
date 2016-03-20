Public Class p45BudgetDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.p45Budget
        Dim s As String = GetSQLPart1() & " WHERE a.p45ID=@pid"
        Return _cDB.GetRecord(Of BO.p45Budget)(s, New With {.pid = intPID})
    End Function
    Public Function LoadByProject(intP41ID As Integer) As BO.p45Budget
        Dim s As String = GetSQLPart1() & " WHERE a.p41ID=@p41id AND getdate() BETWEEN a.p45ValidFrom and a.p45ValidUntil"
        Return _cDB.GetRecord(Of BO.p45Budget)(s, New With {.p41id = intP41ID})
    End Function
    Private Function GetSQLPart1() As String
        Dim s As String = "SELECT a.*," & bas.RecTail("p45", "a")
        s += ",p41.p41Name as _p41Name FROM p45Budget a INNER JOIN p41Project p41 ON a.p41ID=p41.p41ID"
        Return s
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("p45_delete", pars)

    End Function
    Public Function Save(cRec As BO.p45Budget, lisP46 As List(Of BO.p46BudgetPerson), lisP49 As List(Of BO.p49FinancialPlan)) As Boolean
        Using sc As New Transactions.TransactionScope()     'ukládání podléhá transakci
            Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
            If cRec.PID <> 0 Then
                bolINSERT = False
                strW = "p45ID=@pid"
                pars.Add("pid", cRec.PID, DbType.Int32)
            End If
            With cRec
                pars.Add("p45Name", .p45Name, DbType.String, , , True, "Název")
                pars.Add("p41ID", .p41ID, DbType.Int32)
                pars.Add("p45PlanFrom", .p45PlanFrom, DbType.DateTime)
                pars.Add("p45PlanUntil", .p45PlanUntil, DbType.DateTime)
                pars.Add("p45VersionIndex", .p45VersionIndex, DbType.Int32)
                pars.Add("p45ValidFrom", .ValidFrom, DbType.DateTime)
                pars.Add("p45ValidUntil", .ValidUntil, DbType.DateTime)
            End With

            If _cDB.SaveRecord("p45Budget", pars, bolINSERT, strW, True, _curUser.j03Login) Then
                Dim intLastSavedP45ID As Integer = _cDB.LastSavedRecordPID
                If Not lisP46 Is Nothing Then
                    For Each c In lisP46
                        pars = New DbParameters
                        If c.PID = 0 Then
                            bolINSERT = True : strW = ""
                        Else
                            bolINSERT = False : strW = "p46ID=@pid" : pars.Add("pid", c.PID, DbType.Int32)
                        End If
                        If c.PID <> 0 And c.IsSetAsDeleted Then
                            _cDB.RunSQL("DELETE FROM p46BudgetPerson WHERE p46ID=@pid", pars)
                        Else
                            pars.Add("p45ID", intLastSavedP45ID, DbType.Int32)
                            pars.Add("j02ID", c.j02ID, DbType.Int32)
                            pars.Add("p46ExceedFlag", c.p46ExceedFlag, DbType.Int32)
                            pars.Add("p46HoursBillable", c.p46HoursBillable, DbType.Double)
                            pars.Add("p46HoursNonBillable", c.p46HoursNonBillable, DbType.Double)
                            pars.Add("p46HoursTotal", c.p46HoursTotal, DbType.Double)
                            pars.Add("p46Description", c.p46Description, DbType.String)
                            _cDB.SaveRecord("p46BudgetPerson", pars, bolINSERT, strW, True, _curUser.j03Login, False)
                        End If
                    Next
                End If
                If Not lisP49 Is Nothing Then
                    For Each c In lisP49
                        pars = New DbParameters
                        If c.PID = 0 Then
                            bolINSERT = True : strW = ""
                        Else
                            bolINSERT = False : strW = "p49ID=@pid" : pars.Add("pid", c.PID, DbType.Int32)
                        End If
                        If c.PID <> 0 And c.IsSetAsDeleted Then
                            _cDB.RunSQL("DELETE FROM p49FinancialPlan WHERE p49ID=@pid", pars)
                        Else
                            pars.Add("p45ID", intLastSavedP45ID, DbType.Int32)
                            pars.Add("p28ID_Supplier", BO.BAS.IsNullDBKey(c.p28ID_Supplier), DbType.Int32)
                            pars.Add("p34ID", BO.BAS.IsNullDBKey(c.p34ID), DbType.Int32)
                            pars.Add("p32ID", BO.BAS.IsNullDBKey(c.p32ID), DbType.Int32)
                            pars.Add("j27ID", BO.BAS.IsNullDBKey(c.j27ID), DbType.Int32)
                            pars.Add("j02ID", BO.BAS.IsNullDBKey(c.j02ID), DbType.Int32)
                            pars.Add("p49DateFrom", c.p49DateFrom, DbType.DateTime)
                            pars.Add("p49DateUntil", c.p49DateUntil, DbType.DateTime)
                            pars.Add("p49Amount", c.p49Amount, DbType.Double)
                            pars.Add("p49Text", c.p49Text, DbType.String)

                            _cDB.SaveRecord("p49FinancialPlan", pars, bolINSERT, strW, True, _curUser.j03Login, False)
                        End If
                    Next
                End If
                pars = New DbParameters
                With pars
                    .Add("p45id", intLastSavedP45ID, DbType.Int32)
                    .Add("j03id_sys", _curUser.PID, DbType.Int32)
                End With
                If _cDB.RunSP("p45_aftersave", pars) Then
                    sc.Complete()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Using
    End Function

    Public Function GetList(intP41ID As Integer, Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.p45Budget)
        Dim s As String = GetSQLPart1() & " WHERE a.p41ID=@p41id"
        Dim pars As New DbParameters
        pars.Add("p41id", intP41ID, DbType.Int32)
        If Not myQuery Is Nothing Then
            Dim strW As String = bas.ParseWhereMultiPIDs("p45ID", myQuery)
            strW += bas.ParseWhereValidity("p45", "", myQuery)
            If strW <> "" Then s += " AND " & bas.TrimWHERE(strW)
        End If
        s += " ORDER BY p45VersionIndex DESC"

        Return _cDB.GetList(Of BO.p45Budget)(s, pars)

    End Function

    Public Function LoadP46(intP46ID As Integer) As BO.p46BudgetPerson
        Dim s As String = "select a.*,j02.j02LastName+' '+j02.j02FirstName as _Person," & bas.RecTail("p46", "a") & " FROM p46BudgetPerson a INNER JOIN j02Person j02 ON a.j02ID=j02.j02ID"
        s += " WHERE a.p46ID=@pid"
        Return _cDB.GetRecord(Of BO.p46BudgetPerson)(s, New With {.pid = intP46ID})
    End Function
    Public Function GetList_p46(intPID As Integer) As IEnumerable(Of BO.p46BudgetPerson)
        Dim s As String = "select a.*,j02.j02LastName+' '+j02.j02FirstName as _Person," & bas.RecTail("p46", "a") & " FROM p46BudgetPerson a INNER JOIN j02Person j02 ON a.j02ID=j02.j02ID"
        s += " WHERE a.p45ID=@p45id ORDER BY j02.j02LastName,j02.j02FirstName"
        Return _cDB.GetList(Of BO.p46BudgetPerson)(s, New With {.p45id = intPID})
    End Function
    Public Function GetList_p46_extended(intPID As Integer, intP41ID As Integer) As IEnumerable(Of BO.p46BudgetPersonExtented)
        Dim pars As New DbParameters
        pars.Add("p45id", intPID, DbType.Int32)
        pars.Add("p41id", intP41ID, DbType.Int32)
        Dim s As String = "select a.*,j02.j02LastName+' '+j02.j02FirstName as _Person,timesheet.TimesheetFa,timesheet.TimesheetNeFa," & bas.RecTail("p46", "a")
        s += ",timesheet.TimeshetAmountBilling,timesheet.TimesheetAmountCost"
        s += " FROM p46BudgetPerson a INNER JOIN j02Person j02 ON a.j02ID=j02.j02ID"
        s += " LEFT OUTER JOIN (select xa.j02ID,sum(case when p32.p32IsBillable=1 THEN xa.p31Hours_Orig end) as TimesheetFa,sum(case when p32.p32IsBillable=0 THEN xa.p31Hours_Orig end) as TimesheetNeFa,sum(xa.p31Amount_WithoutVat_Orig) as TimeshetAmountBilling,sum(xa.p31Amount_Internal) as TimesheetAmountCost"
        s += " FROM p31Worksheet xa INNER JOIN p32Activity p32 ON xa.p32ID=p32.p32ID WHERE xa.p41ID=@p41id GROUP BY xa.j02ID) timesheet ON a.j02ID=timesheet.j02ID"
        s += " WHERE a.p45ID=@p45id ORDER BY j02.j02LastName,j02.j02FirstName"
        Return _cDB.GetList(Of BO.p46BudgetPersonExtented)(s, pars)
    End Function
End Class
