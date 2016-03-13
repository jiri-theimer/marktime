Public Class j74SavedGridColTemplateDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.j74SavedGridColTemplate
        Dim s As String = GetSQLPart1() & " WHERE a.j74ID=@j74id"
        Return _cDB.GetRecord(Of BO.j74SavedGridColTemplate)(s, New With {.j74id = intPID})
    End Function
    Public Function LoadSystemTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "", Optional recState As BO.p31RecordState = BO.p31RecordState._NotExists) As BO.j74SavedGridColTemplate
        Dim pars As New DbParameters
        pars.Add("j03id", intJ03ID, DbType.Int32)
        pars.Add("x29id", x29id, DbType.Int32)

        Dim s As String = GetSQLPart1() & " WHERE a.j74IsSystem=1 AND a.j03ID=@j03id AND x29ID=@x29id"
        If recState > BO.p31RecordState._NotExists Then
            pars.Add("recstate", recState, DbType.Int32)
            s += " AND a.j74RecordState=@recstate"
        End If
        If strMasterPrefix <> "" Then
            pars.Add("masterprefix", strMasterPrefix, DbType.String)
            s += " AND j74MasterPrefix=@masterprefix"
        Else
            s += " AND j74MasterPrefix is null"
        End If
        Return _cDB.GetRecord(Of BO.j74SavedGridColTemplate)(s, pars)
    End Function

    Public Function Save(cRec As BO.j74SavedGridColTemplate) As Boolean
        _Error = ""
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "j74ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("j03ID", .j03ID, DbType.Int32)
            pars.Add("j02ID_Owner", _curUser.j02ID, DbType.Int32)
            pars.Add("x29ID", BO.BAS.IsNullDBKey(cRec.x29ID), DbType.Int32)

            pars.Add("j74Name", .j74Name, DbType.String, , , True, "Název")
            pars.Add("j74ColumnNames", .j74ColumnNames, DbType.String, , , True, "Vybrané sloupce")
            pars.Add("j74IsSystem", .j74IsSystem, DbType.Boolean)
            pars.Add("j74MasterPrefix", .j74MasterPrefix, DbType.String)
            pars.Add("j74RecordState", .j74RecordState, DbType.Int32)
            pars.Add("j74OrderBy", .j74OrderBy, DbType.String)
            pars.Add("j74IsFilteringByColumn", .j74IsFilteringByColumn, DbType.Boolean)
            pars.Add("j74IsVirtualScrolling", .j74IsVirtualScrolling, DbType.Boolean)

            pars.Add("j74validfrom", cRec.ValidFrom, DbType.DateTime2)
            pars.Add("j74validuntil", cRec.ValidUntil, DbType.DateTime2)

        End With


        If _cDB.SaveRecord("j74SavedGridColTemplate", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As IEnumerable(Of BO.j74SavedGridColTemplate)
        Dim s As String = GetSQLPart1()
        Dim pars As New DbParameters

        pars.Add("j03id", _curUser.PID, DbType.Int32)
        Dim strW As String = "a.j03ID=@j03id"

        If _x29id > BO.x29IdEnum._NotSpecified Then
            pars.Add("x29id", _x29id, DbType.Int32)
            strW += " AND a.x29ID=@x29id"
        End If

        If Not myQuery Is Nothing Then
            strW += bas.ParseWhereMultiPIDs("a.j74ID", myQuery)
            strW += bas.ParseWhereValidity("j74", "a", myQuery)

            If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        End If
        s += " ORDER BY a.j74IsSystem DESC,a.j74ID DESC"

        Return _cDB.GetList(Of BO.j74SavedGridColTemplate)(s, pars)

    End Function
    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("j74_delete", pars)

    End Function

    Private Function GetSQLPart1() As String
        Dim s As String = "select a.*," & bas.RecTail("j74", "a")
        s += " FROM j74SavedGridColTemplate a INNER JOIN j03User j03 ON a.j03ID=j03.j03ID"
        Return s
    End Function
End Class
