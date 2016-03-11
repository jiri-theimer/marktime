Public Class o24NotepadTypeDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.o24NotepadType
        Dim s As String = "select a.*,b01.b01Name as _b01Name,x29.x29Name," & bas.RecTail("o24", "a") & " FROM o24NotepadType a LEFT OUTER JOIN b01WorkflowTemplate b01 ON a.b01ID=b01.b01ID LEFT OUTER JOIN x29Entity x29 ON a.x29ID=x29.x29ID WHERE a.o24ID=@pid"
        Return _cDB.GetRecord(Of BO.o24NotepadType)(s, New With {.pid = intPID})
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("o24_delete", pars)

    End Function
    Public Function Save(cRec As BO.o24NotepadType) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "o24ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("x29ID", .x29ID, DbType.Int32)
            pars.Add("b01ID", BO.BAS.IsNullDBKey(.b01ID), DbType.Int32)
            pars.Add("x38ID", BO.BAS.IsNullDBKey(.x38ID), DbType.Int32)
            pars.Add("x38ID_Draft", BO.BAS.IsNullDBKey(.x38ID_Draft), DbType.Int32)
            pars.Add("o24Name", .o24Name, DbType.String, , , True, "Název")
            pars.Add("o24HelpText", .o24HelpText, DbType.String, , , True, "Nápověda k typu dokumentu")
            pars.Add("o24Ordinary", .o24Ordinary, DbType.Int32)
            pars.Add("o24IsEntityRelationRequired", .o24IsEntityRelationRequired, DbType.Boolean)
            pars.Add("o24IsBillingMemo", .o24IsBillingMemo, DbType.Boolean)
            pars.Add("o24IsAllowDropbox", .o24IsAllowDropbox, DbType.Boolean)
            pars.Add("o24MaxOneFileSize", .o24MaxOneFileSize, DbType.Int32)
            pars.Add("o24AllowedFileExtensions", .o24AllowedFileExtensions, DbType.String)
            pars.Add("o24ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("o24ValidUntil", .ValidUntil, DbType.DateTime)
        End With

        If _cDB.SaveRecord("o24NotepadType", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.o24NotepadType)
        Dim s As String = "select a.*,b01.b01Name as _b01Name,x29.x29name as _x29Name," & bas.RecTail("o24")
        s += " FROM o24NotepadType a LEFT OUTER JOIN b01WorkflowTemplate b01 ON a.b01ID=b01.b01ID LEFT OUTER JOIN x29Entity x29 ON a.x29ID=x29.x29ID"
        If Not myQuery Is Nothing Then
            Dim strW As String = bas.ParseWhereMultiPIDs("a.o24ID", myQuery)
            strW += bas.ParseWhereValidity("o24", "", myQuery)
            If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        End If
        s += " ORDER BY a.o24Ordinary,a.o24Name"

        Return _cDB.GetList(Of BO.o24NotepadType)(s)

    End Function
End Class
