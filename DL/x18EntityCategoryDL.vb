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
        Return _cDB.GetList(Of BO.x19EntityCategory_Binding)("select a.*," & bas.RecTail("x19", "a") & ",x25.x25Name as _x25Name,x18.x18Name as _x18Name,x25.x25ForeColor as _ForeColor,x25.x25BackColor as _BackColor from x19EntityCategory_Binding a INNER JOIN x18EntityCategory x18 ON a.x18ID=x18.x18ID INNER JOIN x25EntityField_ComboValue x25 ON a.x25ID=x25.x25ID WHERE a.x29ID=@x29id AND a.x19RecordPID=@recordpid ORDER BY x18.x18Ordinary,x18.x18ID", pars)
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

    Public Function Save(cRec As BO.x18EntityCategory, x29IDs As List(Of Integer), lisX22 As List(Of BO.x22EntiyCategory_Binding)) As Boolean
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
            pars.Add("x18IsAllEntityTypes", .x18IsAllEntityTypes, DbType.Boolean)
            pars.Add("x18IsRequired", .x18IsRequired, DbType.Boolean)
            pars.Add("x23ID", BO.BAS.IsNullDBKey(.x23ID), DbType.Int32)
        End With

        If _cDB.SaveRecord("x18EntityCategory", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intX18ID As Integer = _cDB.LastSavedRecordPID
            _cDB.RunSQL("DELETE FROM x20EntiyToCategory WHERE x18ID=" & intX18ID.ToString)
            _cDB.RunSQL("INSERT INTO x20EntiyToCategory(x18ID,x29ID) SELECT " & intX18ID.ToString & ",x29ID FROM x29Entity WHERE x29ID IN (" & String.Join(",", x29IDs) & ")")

            If Not lisX22 Is Nothing Then
                If Not bolINSERT Then
                    _cDB.RunSQL("DELETE FROM x22EntiyCategory_Binding WHERE x18ID=" & intX18ID.ToString)
                End If
                For Each c In lisX22
                    _cDB.RunSQL("INSERT INTO x22EntiyCategory_Binding(x18ID,x22EntityTypePID,x29ID_EntityType,x22IsEntryRequired) VALUES (" & intX18ID.ToString & "," & c.x22EntityTypePID.ToString & "," & c.x29ID_EntityType.ToString & "," & BO.BAS.GB(c.x22IsEntryRequired) & ")")
                Next
            End If

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

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing, Optional x29ID As BO.x29IdEnum = BO.x29IdEnum._NotSpecified, Optional intEntityType As Integer = 0) As IEnumerable(Of BO.x18EntityCategory)
        Dim s As String = GetSQLPart1()
        Dim strW As String = bas.ParseWhereMultiPIDs("a.x18ID", myQuery)
        strW += bas.ParseWhereValidity("x18", "a", myQuery)
        If x29ID > BO.x29IdEnum._NotSpecified Then
            strW += " AND a.x18ID IN (SELECT x18ID FROM x20EntiyToCategory WHERE x29ID=" & CInt(x29ID).ToString & ")"
        End If
        If intEntityType >= 0 Then
            If intEntityType = 0 Then
                s += " AND a.x18IsAllEntityTypes=1"
            Else
                s += " AND (a.x18IsAllEntityTypes=1 OR a.x18ID IN (select x18ID FROM x22EntiyCategory_Binding WHERE x22EntityTypePID=" & intEntityType.ToString
                s += " AND x29ID_EntityType=" & GetEntityTypeX29ID(x29ID).ToString & "))"

            End If
        End If
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)

        s += " ORDER BY a.x18Ordinary,a.x18Name"
        Return _cDB.GetList(Of BO.x18EntityCategory)(s)

    End Function
    Private Function GetEntityTypeX29ID(x29id As BO.x29IdEnum) As Integer
        Select Case x29id
            Case BO.x29IdEnum.p41Project : Return 342
            Case BO.x29IdEnum.p28Contact : Return 329
            Case BO.x29IdEnum.p91Invoice : Return 392
            Case BO.x29IdEnum.p31Worksheet : Return 334
            Case BO.x29IdEnum.j02Person : Return 107
            Case BO.x29IdEnum.o23Notepad : Return 224
            Case BO.x29IdEnum.p56Task : Return 357
            Case Else
                Return 0
        End Select
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

    Public Function GetList_X25(x29id As BO.x29IdEnum) As IEnumerable(Of BO.x25EntityField_ComboValue)
        Dim s As String = "select a.*," & bas.RecTail("x25", "a") & ",x23.x23Name as _x23Name"
        s += " FROM x25EntityField_ComboValue a INNER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID INNER JOIN x18EntityCategory x18 ON x23.x23ID=x18.x23ID"
        s += " WHERE x18.x18ID IN (SELECT x18ID FROM x20EntiyToCategory WHERE x29ID=@x29id)"
        s += " ORDER BY x18.x18Ordinary,x18.x18Name,a.x25Ordinary,a.x25Name"

        Dim pars As New DbParameters
        pars.Add("x29id", CInt(x29id), DbType.Int32)
        Return _cDB.GetList(Of BO.x25EntityField_ComboValue)(s, pars)

    End Function
    Public Function GetList_x22(intX18ID As Integer) As IEnumerable(Of BO.x22EntiyCategory_Binding)
        Return _cDB.GetList(Of BO.x22EntiyCategory_Binding)("SELECT * FROM x22EntiyCategory_Binding WHERE x18ID=@pid", New With {.pid = intX18ID})
    End Function
End Class
