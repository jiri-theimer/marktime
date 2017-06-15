Public Class p64BinderDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.p64Binder
        Dim s As String = GetSQLPart1()
        s += " WHERE a.p64ID=@p64ID"

        Return _cDB.GetRecord(Of BO.p64Binder)(s, New With {.p64ID = intPID})
    End Function

    Public Function Save(cRec As BO.p64Binder) As Boolean
        ''Using sc As New Transactions.TransactionScope()     'ukládání podléhá transakci
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = "", c As New BO.clsArabicNumber
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "p64ID=@pid"
            pars.Add("pid", cRec.PID, DbType.Int32)
        End If
        With cRec
            pars.Add("p41ID", BO.BAS.IsNullDBKey(.p41ID), DbType.Int32)
            pars.Add("j02ID_Owner", BO.BAS.IsNullDBKey(.j02ID_Owner), DbType.Int32)
            pars.Add("p64Name", .p64Name, DbType.String, , , True, "Název")
            pars.Add("p64Code", .p64Code, DbType.String)
            pars.Add("p64Ordinary", .p64Ordinary, DbType.Int32)
            pars.Add("p64ArabicCode", c.NumberToRoman(.p64Ordinary), DbType.String)
            pars.Add("p64Location", .p64Location, DbType.String)
            pars.Add("p64Description", .p64Description, DbType.String)

            pars.Add("p64validfrom", .ValidFrom, DbType.DateTime)
            pars.Add("p64validuntil", .ValidUntil, DbType.DateTime)

        End With

        If _cDB.SaveRecord("p64Binder", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intP64ID As Integer = _cDB.LastSavedRecordPID
            If bolINSERT Then
                Dim x As Integer = cRec.p64Ordinary
                If x = 0 Then x = _cDB.GetIntegerValueFROMSQL("select count(*) from p64Binder WHERE p41ID=" & cRec.p41ID.ToString)
                pars = New DbParameters
                pars.Add("p64ArabicCode", c.NumberToRoman(x), DbType.String)
                pars.Add("p64Ordinary", x, DbType.Int32)
                _cDB.SaveRecord("p64Binder", pars, False, "p64ID=" & intP64ID.ToString, False, , False)
            End If
            ''sc.Complete()
            Return True
        Else
            Return False
        End If
        ''End Using
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("p64_delete", pars)
    End Function


    Public Function GetList(intP41ID As Integer, Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.p64Binder)
        Dim s As String = GetSQLPart1()
        Dim strW As String = bas.ParseWhereMultiPIDs("a.p64ID", myQuery)
        strW += bas.ParseWhereValidity("p64", "a", myQuery)
        If intP41ID <> -1 Then
            s += " WHERE a.p41ID=" & intP41ID.ToString
        End If
        If strW <> "" Then s += " AND " & bas.TrimWHERE(strW)
        If intP41ID = -1 Then
            s += " ORDER BY a.p64ID DESC"
        Else
            s += " ORDER BY a.p64Ordinary"
        End If


        Return _cDB.GetList(Of BO.p64Binder)(s)

    End Function


    Private Function GetSQLPart1() As String

        Dim s As String = "SELECT TOP 1000 a.*,j02.j02LastName+' '+j02.j02FirstName as _Owner,isnull(p41.p41NameShort,p41Name) as _Project,p28.p28Name as _Client," & bas.RecTail("p64", "a")
        s += " FROM p64Binder a INNER JOIN p41Project p41 ON a.p41ID=p41.p41ID LEFT OUTER JOIN p28Contact p28 ON p41.p28ID_Client=p28.p28ID LEFT OUTER JOIN j02Person j02 ON a.j02ID_Owner=j02.j02ID"

        Return s
    End Function
End Class
