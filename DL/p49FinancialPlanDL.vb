Public Class p49FinancialPlanDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.p49FinancialPlan
        Dim s As String = GetSQLPart1(0)
        s += " WHERE a.p49ID=@p49id"

        Return _cDB.GetRecord(Of BO.p49FinancialPlan)(s, New With {.p49id = intPID})
    End Function
    Public Function LoadMyLastCreated() As BO.p49FinancialPlan
        Dim s As String = GetSQLPart1(1)
        s += " WHERE a.p49UserInsert=@login ORDER BY a.p49ID DESC"

        Return _cDB.GetRecord(Of BO.p49FinancialPlan)(s, New With {.login = _curUser.j03Login})
    End Function

    
    Public Function GetList(mq As BO.myQueryp49) As IEnumerable(Of BO.p49FinancialPlan)
        Dim pars As New DbParameters
        Dim strW As String = GetSqlWhere(mq, pars)
        Dim s As String = GetSQLPart1(0)
        s += " WHERE " & strW
        s += " ORDER BY a.p34ID,p32.p32Name,a.p49DateFrom"
        Return _cDB.GetList(Of BO.p49FinancialPlan)(s, pars)

    End Function

    Private Function GetSqlWhere(mq As BO.myQueryp49, ByRef pars As DbParameters) As String
        Dim strW As String = bas.ParseWhereMultiPIDs("p49ID", mq)
        With mq
            If .DateFrom > DateSerial(1900, 1, 1) And .DateUntil < DateSerial(3000, 1, 1) Then
                strW += " AND (@datefrom BETWEEN a.p49DateFrom AND a.p49DateUntil OR @dateuntil BETWEEN a.p49DateFrom AND a.p49DateUntil)"
                pars.Add("datefrom", .DateFrom, DbType.DateTime)
                pars.Add("dateuntil", .DateUntil, DbType.DateTime)
            End If

            If Not .p41IDs Is Nothing Then
                If .p41IDs.Count > 0 Then
                    strW += " AND a.p41ID IN (" & String.Join(",", .p41IDs) & ")"
                End If
            End If
            If .p28ID > 0 Then
                pars.Add("p28id", .p28ID, DbType.Int32)
                strW += " AND a.p41ID IN (SELECT p41ID FROM p41Project WHERE p28ID_Client=@p28id)"
            End If
            If Not .j02IDs Is Nothing Then
                If .j02IDs.Count > 0 Then
                    strW += " AND a.j02ID IN (" & String.Join(",", .j02IDs) & ")"
                End If
            End If
            
            If .p34ID > 0 Then
                pars.Add("p34id", .p34ID, DbType.Int32)
                strW += " AND a.p34ID=@p34id"
            End If
            If .p32ID > 0 Then
                pars.Add("p32id", .p32ID, DbType.Int32)
                strW += " AND a.p32ID=@p32id"
            End If
        End With
        Return bas.TrimWHERE(strW)
    End Function

    Public Function Save(cRec As BO.p49FinancialPlan) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "p49ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("p34ID", BO.BAS.IsNullDBKey(.p34ID), DbType.Int32)
            pars.Add("p32ID", BO.BAS.IsNullDBKey(.p32ID), DbType.Int32)
            pars.Add("p41ID", BO.BAS.IsNullDBKey(.p41ID), DbType.Int32)
            pars.Add("j27ID", BO.BAS.IsNullDBKey(.j27ID), DbType.Int32)
            pars.Add("j02id", BO.BAS.IsNullDBKey(.j02ID), DbType.Int32)

            pars.Add("p49DateFrom", .p49DateFrom, DbType.DateTime)
            pars.Add("p49DateUntil", .p49DateUntil, DbType.DateTime)
            pars.Add("p49Amount", .p49Amount, DbType.Double)

            pars.Add("p49Text", .p49Text, DbType.String)

            pars.Add("p49validfrom", .ValidFrom, DbType.DateTime)
            pars.Add("p49validuntil", .ValidUntil, DbType.DateTime)

        End With

        If _cDB.SaveRecord("p49FinancialPlan", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("p49_delete", pars)
    End Function

    Private Function GetSQLPart1(intTOPRecs As Integer) As String
        Dim s As String = "SELECT"
        If intTOPRecs > 0 Then s += " TOP " & intTOPRecs.ToString
        s += " a.*,j02.j02LastName+' '+j02.j02FirstName+isnull(' '+j02.j02TitleBeforeName,'') as _Person,isnull(p28.p28Name+' - ','') + p41.p41Name as _Project,p34.p34Name as _p34Name,p32.p32Name as _p32Name,p34.p34Color as _p34Color,p32.p32Color as _p32Color,p34.p34IncomeStatementFlag as _p34IncomeStatementFlag,j27.j27Code as _j27Code," & bas.RecTail("p49", "a")
        s += " FROM p49FinancialPlan a"
        s += " INNER JOIN p41Project p41 ON a.p41ID=p41.p41ID INNER JOIN p34ActivityGroup p34 ON a.p34ID=p34.p34ID"
        s += " LEFT OUTER JOIN p32Activity p32 ON a.p32ID=p32.p32ID"
        s += " LEFT OUTER JOIN j02Person j02 ON a.j02ID=j02.j02ID LEFT OUTER JOIN j27Currency j27 ON a.j27ID=j27.j27ID"
        s += " LEFT OUTER JOIN p28Contact p28 ON p41.p28ID_Client=p28.p28ID"
        Return s
    End Function
End Class
