Public Class o51TagDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.o51Tag
        Dim s As String = "select a.*," & bas.RecTail("o51", "a") & ",j02.j02LastName+' '+j02.j02FirstName as _Owner FROM o51Tag a INNER JOIN j02Person j02 ON a.j02ID_Owner=j02.J02ID WHERE a.o51id=@o51id"
        Return _cDB.GetRecord(Of BO.o51Tag)(s, New With {.o51id = intPID})
    End Function

    Public Function Save(cRec As BO.o51Tag) As Boolean
        Using sc As New Transactions.TransactionScope()     'ukládání podléhá transakci
            Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
            If cRec.PID <> 0 Then
                bolINSERT = False
                strW = "o51id=@pid"
                pars.Add("pid", cRec.PID)
            End If

            With pars
                .Add("j02ID_Owner", BO.BAS.IsNullDBKey(cRec.j02ID_Owner), DbType.Int32)
                .Add("o51name", cRec.o51Name, DbType.String, , , True, "Název")
                .Add("o51validfrom", cRec.ValidFrom, DbType.DateTime)
                .Add("o51validuntil", cRec.ValidUntil, DbType.DateTime)
            End With
            If _cDB.SaveRecord("o51Tag", pars, bolINSERT, strW, True, _curUser.j03Login) Then

                sc.Complete()
                Return True
            Else
                Return False
            End If
        End Using


    End Function
    Public Function Delete(intPID) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("o51_delete", pars)

    End Function

    Public Function GetList(myQuery As BO.myQuery) As IEnumerable(Of BO.o51Tag)
        Dim s As String = "select a.*," & bas.RecTail("o51", "a") & ",j02.j02LastName+' '+j02.j02FirstName as _Owner FROM o51Tag a INNER JOIN j02Person j02 ON a.j02ID_Owner=j02.J02ID"

        Dim pars As New DbParameters

        Dim strW As String = bas.ParseWhereMultiPIDs("a.o51ID", myQuery)
        ''strW += bas.ParseWhereValidity("o51", "a", myQuery)
        If myQuery.SearchExpression <> "" Then
            pars.Add("expr", myQuery.SearchExpression, DbType.String)
            strW += " AND a.p51Name LIKE '%'+@expr+'%'"
        End If

        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)


        s += " ORDER BY a.o51name"

        Return _cDB.GetList(Of BO.o51Tag)(s, pars)

    End Function



    Public Function GetList_o52(x29id As BO.x29IdEnum, intRecordPID As Integer) As IEnumerable(Of BO.o52TagBinding)
        Dim pars As New DbParameters
        pars.Add("recpid", intRecordPID, DbType.Int32)
        pars.Add("x29id", CInt(x29id), DbType.Int32)
        Return _cDB.GetList(Of BO.o52TagBinding)("select a.*,o51.o51Name as _o51Name FROM o52TagBinding a inner join o51Tag o51 on a.o51ID=o51.o51ID WHERE a.o52RecordPID=@o51id AND a.x29ID=@x29id ORDER BY o51.o51Name", pars)
    End Function
End Class
