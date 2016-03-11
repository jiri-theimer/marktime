Public Class j24NonPersonTypeDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.j24NonPersonType
        Dim s As String = "select *," & bas.RecTail("j24") & " FROM j24NonPersonType WHERE j24ID=@pid"
        Return _cDB.GetRecord(Of BO.j24NonPersonType)(s, New With {.pid = intPID})
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("j24_delete", pars)

    End Function
    Public Function Save(cRec As BO.j24NonPersonType) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "j24ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("j24Name", .j24Name, DbType.String, , , True, "Název")
            pars.Add("j24Ordinary", .j24Ordinary, DbType.Int32)
            pars.Add("j24ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("j24ValidUntil", .ValidUntil, DbType.DateTime)
        End With

        If _cDB.SaveRecord("j24NonPersonType", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.j24NonPersonType)
        Dim s As String = "select *," & bas.RecTail("j24")
        s += " FROM j24NonPersonType"
        If Not myQuery Is Nothing Then
            Dim strW As String = bas.ParseWhereMultiPIDs("j24ID", myQuery)
            strW += bas.ParseWhereValidity("j24", "", myQuery)
            If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        End If
        s += " ORDER BY j24Ordinary,j24Name"

        Return _cDB.GetList(Of BO.j24NonPersonType)(s)

    End Function
End Class
