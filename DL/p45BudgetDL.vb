Public Class p45BudgetDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.p45Budget
        Dim s As String = GetSQLPart1() & " WHERE a.p45ID=@pid"
        Return _cDB.GetRecord(Of BO.p45Budget)(s, New With {.pid = intPID})
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
    Public Function Save(cRec As BO.p45Budget) As Boolean
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
            Return True
        Else
            Return False
        End If

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
End Class
