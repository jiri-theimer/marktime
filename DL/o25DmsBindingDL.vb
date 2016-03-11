Public Class o25DmsBindingDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.o25DmsBinding
        Dim s As String = GetSQLPart1()
        s += " WHERE a.o25ID=@o25id"

        Return _cDB.GetRecord(Of BO.o25DmsBinding)(s, New With {.o25id = intPID})
    End Function

    Public Function Save(cRec As BO.o25DmsBinding) As Boolean

        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "o25ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("o25Path", .o25Path, DbType.String, , , True, "Cesta")
            pars.Add("o25Icon", .o25Icon, DbType.String)
            pars.Add("o25DmsApp", .o25DmsApp, DbType.Int32)
            pars.Add("o25DmsObject", .o25DmsObject, DbType.Int32)
            pars.Add("x29ID", .x29ID, DbType.Int32)
            pars.Add("o25RecordPID", BO.BAS.IsNullDBKey(.o25RecordPID), DbType.Int32)
            
            pars.Add("o25validfrom", .ValidFrom, DbType.DateTime)
            pars.Add("o25validuntil", .ValidUntil, DbType.DateTime)

        End With

        If _cDB.SaveRecord("o25DmsBinding", pars, bolINSERT, strW, True, _curUser.j03Login) Then

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
        Return _cDB.RunSP("o25_delete", pars)
    End Function


    Public Function GetList(appDMS As BO.o25DmsAppENUM, Optional x29id As BO.x29IdEnum = BO.x29IdEnum._NotSpecified, Optional intRecordPID As Integer = 0) As IEnumerable(Of BO.o25DmsBinding)
        Dim s As String = GetSQLPart1(), pars As New DbParameters
        Dim strW As String = "a.o25DmsApp=@app"
        pars.Add("app", CInt(appDMS), DbType.Int32)
        If x29id > BO.x29IdEnum._NotSpecified Then
            pars.Add("x29id", CInt(x29id), DbType.Int32)
            strW += " AND a.x29ID=@x29id"
        End If
        If intRecordPID <> 0 Then
            pars.Add("recordPID", intRecordPID, DbType.Int32)
            strW += " AND a.o25RecordPID=@recordPID"
        End If
      

        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)

        s += " ORDER BY a.o25ID DESC"

        Return _cDB.GetList(Of BO.o25DmsBinding)(s, pars)

    End Function


    Private Function GetSQLPart1() As String
        Dim s As String = "SELECT a.*," & bas.RecTail("o25", "a")
        s += " FROM o25DmsBinding a"

        Return s
    End Function
End Class
