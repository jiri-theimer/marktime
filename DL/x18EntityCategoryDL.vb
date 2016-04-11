Public Class x18EntityCategoryDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x18EntityCategory
        Dim s As String = GetSQLPart1() & " WHERE a.x18ID=@x18id"

        Return _cDB.GetRecord(Of BO.x18EntityCategory)(s, New With {.x18id = intPID})
    End Function

    Public Function GetList_X19(x29id As BO.x29IdEnum, intRecordPID As Integer) As IEnumerable(Of BO.x19EntityCategory_Binding)
        Dim pars As New DbParameters
        pars.Add("x29id", CInt(x29id), DbType.Int32)
        pars.Add("recordpid", intRecordPID, DbType.Int32)
        Return _cDB.GetList(Of BO.x19EntityCategory_Binding)("select a.*," & bas.RecTail("x19", "a") & ",x25.x25Name as _x25Name,x18.x18Name as _x18Name from x19EntityCategory_Binding a INNER JOIN x18EntityCategory x18 ON a.x18ID=x18.x18ID INNER JOIN x25EntityField_ComboValue x25 ON a.x25ID=x25.x25ID WHERE a.x29ID=@x29id AND a.x19RecordPID=@recordpid ORDER BY x18.x18Ordinary,x18.x18ID", pars)

    End Function

    Public Function SaveX19Binding(x29id As BO.x29IdEnum, intRecordPID As Integer, lisX19 As List(Of BO.x19EntityCategory_Binding)) As Boolean
        Dim pars As New DbParameters
        pars.Add("x29id", CInt(x29id), DbType.Int32)
        pars.Add("recordpid", intRecordPID, DbType.Int32)
        pars.Add("login", _curUser.j03Login, DbType.String)

        Dim lisSaved As IEnumerable(Of BO.x19EntityCategory_Binding) = _cDB.GetList(Of BO.x19EntityCategory_Binding)("select *," & bas.RecTail("x19") & " from x19EntityCategory_Binding WHERE x29ID=@x29id AND x19RecordPID=@recordpid", pars)
        For Each c In lisX19
            Dim cRec As BO.x19EntityCategory_Binding = Nothing
            If lisSaved.Where(Function(p) p.x18ID = c.x18ID And p.x25ID = c.x25ID).Count > 0 Then
                cRec = lisSaved.Where(Function(p) p.x18ID = c.x18ID And p.x25ID = c.x25ID).First
            End If
            If cRec Is Nothing Then
                _cDB.RunSQL("INSERT INTO x19EntityCategory_Binding(x18ID,x25ID,x29ID,x19RecordPID,x19UserUpdate,x19UserInsert,x19DateUpdate) VALUES(" & c.x18ID.ToString & "," & c.x25ID.ToString & ",@x29id,@recordpid,@login,@login,getdate())", pars)
            End If
        Next
        For Each c In lisSaved
            Dim cRec As BO.x19EntityCategory_Binding = Nothing
            If lisX19.Where(Function(p) p.x18ID = c.x18ID And p.x25ID = c.x25ID).Count > 0 Then
                cRec = lisX19.Where(Function(p) p.x18ID = c.x18ID And p.x25ID = c.x25ID).First
            End If
            If Not cRec Is Nothing Then
                _cDB.RunSQL("UPDATE x19EntityCategory_Binding set x19UserUpdate=@login,x19DateUpdate=getdate() WHERE x18ID=" & c.x18ID.ToString & " AND x25ID=" & c.x25ID.ToString & " AND x29ID=@x29id AND x19RecordPID=@recordpid", pars)
            Else
                _cDB.RunSQL("DELETE FROM x19EntityCategory_Binding WHERE x18ID=" & c.x18ID.ToString & " AND x25ID=" & c.x25ID.ToString & " AND x29ID=@x29id AND x19RecordPID=@recordpid", pars)
            End If
        Next
        Return True
    End Function

    Public Function Save(cRec As BO.x18EntityCategory, x29IDs As List(Of Integer)) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "x18ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("x18Name", .x18Name, DbType.String, , , True, "Název")
            pars.Add("x18Ordinary", .x18Ordinary, DbType.Int32)
            pars.Add("x18validfrom", .ValidFrom, DbType.DateTime)
            pars.Add("x18validuntil", .ValidUntil, DbType.DateTime)
            pars.Add("x18IsMultiSelect", .x18IsMultiSelect, DbType.Boolean)
            pars.Add("x23ID", BO.BAS.IsNullDBKey(.x23ID), DbType.Int32)
        End With

        If _cDB.SaveRecord("x18EntityCategory", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intX18ID As Integer = _cDB.LastSavedRecordPID
            _cDB.RunSQL("DELETE FROM x20EntiyToCategory WHERE x18ID=" & intX18ID.ToString)
            _cDB.RunSQL("INSERT INTO x20EntiyToCategory(x18ID,x29ID) SELECT " & intX18ID.ToString & ",x29ID FROM x29Entity WHERE x29ID IN (" & String.Join(",", x29IDs) & ")")
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

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing, Optional x29ID As BO.x29IdEnum = BO.x29IdEnum._NotSpecified) As IEnumerable(Of BO.x18EntityCategory)
        Dim s As String = GetSQLPart1()
        Dim strW As String = bas.ParseWhereMultiPIDs("a.x18ID", myQuery)
        strW += bas.ParseWhereValidity("x18", "a", myQuery)
        If x29ID > BO.x29IdEnum._NotSpecified Then
            strW += " AND a.x18ID IN (SELECT x18ID FROM x20EntiyToCategory WHERE x29ID=" & CInt(x29ID).ToString & ")"
        End If
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)

        s += " ORDER BY a.x18Ordinary,a.x18Name"
        Return _cDB.GetList(Of BO.x18EntityCategory)(s)

    End Function
    Public Function GetX29IDs(intX18ID As Integer) As IEnumerable(Of Integer)
        Dim pars As New DbParameters
        pars.Add("pid", intX18ID, DbType.Int32)
        Return _cDB.GetList(Of BO.GetInteger)("select x29ID as Value FROM x20EntiyToCategory WHERE x18ID=@pid", pars).Select(Function(p) p.Value)
    End Function
    Private Function GetSQLPart1() As String
        Dim s As String = "select a.*,x23.x23Name as _x23Name," & bas.RecTail("x18", "a")
        s += " FROM x18EntityCategory a INNER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID"
        Return s
    End Function

End Class
