Public Class j23NonPersonDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.j23NonPerson
        Dim s As String = GetSQLPart1() & " WHERE a.j23ID=@pid"
        Return _cDB.GetRecord(Of BO.j23NonPerson)(s, New With {.pid = intPID})
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("j23_delete", pars)

    End Function
    Public Function Save(cRec As BO.j23NonPerson) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "j23ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("j24ID", BO.BAS.IsNullDBKey(.j24ID), DbType.Int32)
            pars.Add("j23Name", .j23Name, DbType.String, , , True, "Název")
            pars.Add("j23Code", .j23Code, DbType.String, , , True, "Kód")
            pars.Add("j23Ordinary", .j23Ordinary, DbType.Int32)
            pars.Add("j23ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("j23ValidUntil", .ValidUntil, DbType.DateTime)
        End With

        If _cDB.SaveRecord("j23NonPerson", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.j23NonPerson)
        Dim s As String = GetSQLPart1()
        If Not myQuery Is Nothing Then
            Dim strW As String = bas.ParseWhereMultiPIDs("a.j23ID", myQuery)
            strW += bas.ParseWhereValidity("j23", "a", myQuery)
            If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        End If
        s += " ORDER BY j24.j24Ordinary,a.j24ID,a.j23Ordinary,a.j23Name,a.j23Code"

        Return _cDB.GetList(Of BO.j23NonPerson)(s)

    End Function

    Private Function GetSQLPart1() As String
        Dim s As String = "SELECT a.*," & bas.RecTail("j23", "a")
        s += ",j24.j24Name as _j24Name,j24.j24Ordinary as _j24Ordinary"
        s += " FROM j23NonPerson a INNER JOIN j24NonPersonType j24 ON a.j24ID=j24.j24ID"
        
        Return s
    End Function
End Class
