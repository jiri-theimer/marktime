Public Class j62MenuHomeDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.j62MenuHome
        Dim s As String = GetSqlPart1() & " WHERE a.j62ID=@pid"
        Return _cDB.GetRecord(Of BO.j62MenuHome)(s, New With {.pid = intPID})
    End Function

    Private Function GetSqlPart1() As String
        Return "select a.*,x29.x29Name as _x29Name," & bas.RecTail("j62", "a") & ",a.j62TreeIndex as _j62TreeIndex,a.j62TreePrev as _j62TreePrev,a.j62TreeNext as _j62TreeNext,a.j62TreeLevel as _j62TreeLevel FROM j62MenuHome a LEFT OUTER JOIN x29Entity x29 ON a.x29ID=x29.x29ID"
    End Function
    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("j62_delete", pars)

    End Function
    Public Function Save(cRec As BO.j62MenuHome, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "j62ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("x29ID", .x29ID, DbType.Int32)
            pars.Add("j70ID", BO.BAS.IsNullDBKey(.j70ID), DbType.Int32)
            pars.Add("j74ID", BO.BAS.IsNullDBKey(.j74ID), DbType.Int32)
            pars.Add("x31ID", BO.BAS.IsNullDBKey(.x31ID), DbType.Int32)
            pars.Add("j62ParentID", BO.BAS.IsNullDBKey(.j62ParentID), DbType.Int32)
            pars.Add("j62Name", .j62Name, DbType.String)
            pars.Add("j62Url", .j62Url, DbType.String)
            pars.Add("j62Target", .j62Target, DbType.String)
            pars.Add("j62GridGroupBy", .j62GridGroupBy, DbType.String)
            pars.Add("j62IsSeparator", .j62IsSeparator, DbType.Boolean)
            pars.Add("j62ImageUrl", .j62ImageUrl, DbType.String)
            pars.Add("j62Ordinary", .j62Ordinary, DbType.Int32)
            pars.Add("j62ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("j62ValidUntil", .ValidUntil, DbType.DateTime)
        End With

        If _cDB.SaveRecord("j62MenuHome", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intLastSavedID As Integer = _cDB.LastSavedRecordPID
            If Not lisX69 Is Nothing Then   'přiřazení rolí k šabloně
                bas.SaveX69(_cDB, BO.x29IdEnum.j62MenuHome, intLastSavedID, lisX69, bolINSERT)
            End If
            Dim cDbTree As New clsDBTree(_cDB), strDbTreeErr As String = ""
            With cDbTree
                .BasicWHERE = ""
                .SaveTree("j62MenuHome", "j62TreeLevel", "j62UserInsert", "j62TreeIndex", "j62ParentID", "j62ID", "j62Ordinary,j62Name", True, "j62TreePrev", "j62TreeNext", strDbTreeErr)
            End With

            bas.RecoveryUserCache(_cDB, _curUser)
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.j62MenuHome)
        Dim s As String = GetSqlPart1()
        Dim strW As String = ""
        If Not myQuery Is Nothing Then
            strW += bas.ParseWhereMultiPIDs("a.j62ID", myQuery)
            strW += bas.ParseWhereValidity("j62", "a", myQuery)
            If strW = " AND " Then strW = ""
        End If
        If Not _curUser.IsAdmin Then
            strW += " AND a.j62ID IN (SELECT x69.x69RecordPID FROM x69EntityRole_Assign x69 INNER JOIN x67EntityRole x67 ON x69.x67ID=x67.x67ID WHERE x67.x29ID=162 AND (x69.j02ID=@j02id_query OR x69.j11ID IN (SELECT j11ID FROM j12Team_Person WHERE j02ID=@j02id_query)))"   'obdržel nějakou (jakoukoliv) roli v šabloně
        End If
        
        Dim pars As New DbParameters
        pars.Add("j02id_query", _curUser.j02ID, DbType.Int32)
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.j62TreeIndex,a.j62Ordinary,a.j62Name"

        Return _cDB.GetList(Of BO.j62MenuHome)(s, pars)

    End Function
End Class
