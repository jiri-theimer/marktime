Public Class p97Invoice_Round_SettingDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.p97Invoice_Round_Setting
        Dim s As String = GetSQLPart1() & " WHERE a.p97ID=@pid"
        Return _cDB.GetRecord(Of BO.p97Invoice_Round_Setting)(s, New With {.pid = intPID})
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("p97_delete", pars)

    End Function
    Public Function Save(cRec As BO.p97Invoice_Round_Setting) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "p97ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("j27ID", BO.BAS.IsNullDBKey(.j27ID), DbType.Int32)
            pars.Add("p97AmountFlag", CInt(.p97AmountFlag), DbType.Int32)
            pars.Add("p97Scale", .p97Scale, DbType.Int32)
            pars.Add("p97ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("p97ValidUntil", .ValidUntil, DbType.DateTime)
        End With

        If _cDB.SaveRecord("p97Invoice_Round_Setting", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.p97Invoice_Round_Setting)
        Dim s As String = GetSQLPart1()
        If Not myQuery Is Nothing Then
            Dim strW As String = bas.ParseWhereMultiPIDs("a.p97ID", myQuery)
            strW += bas.ParseWhereValidity("p97", "a", myQuery)
            If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        End If
        
        Return _cDB.GetList(Of BO.p97Invoice_Round_Setting)(s)

    End Function

    Private Function GetSQLPart1() As String
        Dim s As String = "SELECT a.*," & bas.RecTail("p97", "a")
        s += ",j27.j27Code as _j27Code"
        s += " FROM p97Invoice_Round_Setting a INNER JOIN j27Currency j27 ON a.j27ID=j27.j27ID"

        Return s
    End Function
End Class
