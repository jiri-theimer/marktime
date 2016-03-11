Public Class x18EntityCategoryDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x18EntityCategory
        Dim s As String = GetSQLPart1() & " WHERE a.x18ID=@x18id"

        Return _cDB.GetRecord(Of BO.x18EntityCategory)(s, New With {.x18id = intPID})
    End Function


    Public Function Save(cRec As BO.x18EntityCategory) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "x18ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("x18Name", .x18Name, DbType.String, , , True, "Název")
            pars.Add("x18Ordinary", .x18Ordinary, DbType.Int32)
            pars.Add("x18validfrom", .ValidFrom, DbType.DateTime2)
            pars.Add("x18validuntil", .ValidUntil, DbType.DateTime2)
            pars.Add("x29ID", BO.BAS.IsNullDBKey(.x29ID), DbType.Int32)
            pars.Add("x23ID", BO.BAS.IsNullDBKey(.x23ID), DbType.Int32)
        End With

        If _cDB.SaveRecord("x18EntityCategory", pars, bolINSERT, strW, True, _curUser.j03Login) Then
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
        Return _cDB.RunSP("x18_delete", pars)
    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.x18EntityCategory)
        Dim s As String = GetSQLPart1()
        Dim strW As String = bas.ParseWhereMultiPIDs("a.x18ID", myQuery)
        strW += bas.ParseWhereValidity("x18", "a", myQuery)
      
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)

        s += " ORDER BY a.x29ID,a.x18Ordinary"
        Return _cDB.GetList(Of BO.x18EntityCategory)(s)

    End Function

    Private Function GetSQLPart1() As String
        Dim s As String = "select a.*,x23.x23Name as _x23Name," & bas.RecTail("x18", "a")
        s += " FROM x18EntityCategory a INNER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID"
        Return s
    End Function

End Class
