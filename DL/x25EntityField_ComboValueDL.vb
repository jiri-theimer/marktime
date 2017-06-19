Public Class x25EntityField_ComboValueDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x25EntityField_ComboValue
        Dim s As String = "SELECT " & GetSQLPart1(0) & " WHERE a.x25ID=@pid"
        Return _cDB.GetRecord(Of BO.x25EntityField_ComboValue)(s, New With {.pid = intPID})
    End Function
    Public Function LoadByCode(strCode As String, intX23ID As Integer) As BO.x25EntityField_ComboValue
        Dim pars As New DbParameters
        pars.Add("x23id", intX23ID, DbType.Int32)
        pars.Add("code", strCode, DbType.String)
        Dim s As String = "SELECT " & GetSQLPart1(0) & " WHERE a.x23ID=@x23id AND a.x25Code=@code"
        Return _cDB.GetRecord(Of BO.x25EntityField_ComboValue)(s, pars)
    End Function
    Private Function GetSQLPart1(intTopRecs As Integer) As String
        Dim s As String = ""
        If intTopRecs > 0 Then s += " TOP " & intTopRecs.ToString
        s += " a.*," & bas.RecTail("x25", "a") & ",x23.x23Name as _x23Name,j02owner.j02LastName+' '+j02owner.j02FirstName as _Owner"
        s += " " & GetSQLPart2_From()
        Return s
    End Function
    Private Function GetSQLPart2_From() As String
        Return "FROM x25EntityField_ComboValue a INNER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID LEFT OUTER JOIN j02Person j02owner ON a.j02ID_Owner=j02owner.j02ID"
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("x25_delete", pars)

    End Function
    Public Function Save(cRec As BO.x25EntityField_ComboValue) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "x25ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("x23ID", BO.BAS.IsNullDBKey(.x23ID), DbType.Int32)
            pars.Add("j02ID_Owner", BO.BAS.IsNullDBKey(.j02ID_Owner), DbType.Int32)
            pars.Add("x25Name", .x25Name, DbType.String, , , True, "Název")
            pars.Add("x25Code", .x25Code, DbType.String)
            pars.Add("x25ForeColor", .x25ForeColor, DbType.String)
            pars.Add("x25BackColor", .x25BackColor, DbType.String)
            pars.Add("x25Ordinary", .x25Ordinary, DbType.Int32)
            If .x25Ordinary > 0 Then
                Dim cAra As New BO.clsArabicNumber
                pars.Add("x25ArabicCode", cAra.NumberToRoman(.x25Ordinary), DbType.String)
            Else
                pars.Add("x25ArabicCode", "", DbType.String)
            End If
            pars.Add("x25ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("x25ValidUntil", .ValidUntil, DbType.DateTime)

            pars.Add("x25FreeText01", .x25FreeText01, DbType.String, , , True, "Text 1")
            pars.Add("x25FreeText02", .x25FreeText02, DbType.String, , , True, "Text 2")
            pars.Add("x25FreeText03", .x25FreeText03, DbType.String, , , True, "Text 3")
            pars.Add("x25FreeText04", .x25FreeText04, DbType.String, , , True, "Text 4")
            pars.Add("x25FreeText05", .x25FreeText05, DbType.String, , , True, "Text 5")
            pars.Add("x25BigText", .x25BigText, DbType.String, , , True, "Podrobný popis")

            pars.Add("x25FreeNumber01", .x25FreeNumber01, DbType.Double)
            pars.Add("x25FreeNumber02", .x25FreeNumber02, DbType.Double)
            pars.Add("x25FreeNumber03", .x25FreeNumber03, DbType.Double)
            pars.Add("x25FreeNumber04", .x25FreeNumber04, DbType.Double)
            pars.Add("x25FreeNumber05", .x25FreeNumber05, DbType.Double)
            pars.Add("x25FreeDate01", .x25FreeDate01, DbType.DateTime)
            pars.Add("x25FreeDate02", .x25FreeDate02, DbType.DateTime)
            pars.Add("x25FreeDate03", .x25FreeDate03, DbType.DateTime)
            pars.Add("x25FreeDate04", .x25FreeDate04, DbType.DateTime)
            pars.Add("x25FreeDate05", .x25FreeDate05, DbType.DateTime)
            pars.Add("x25FreeBoolean01", .x25FreeBoolean01, DbType.Boolean)
            pars.Add("x25FreeBoolean02", .x25FreeBoolean02, DbType.Boolean)
            pars.Add("x25FreeBoolean03", .x25FreeBoolean03, DbType.Boolean)
            pars.Add("x25FreeBoolean04", .x25FreeBoolean04, DbType.Boolean)
            pars.Add("x25FreeBoolean05", .x25FreeBoolean05, DbType.Boolean)
        End With

        If _cDB.SaveRecord("x25EntityField_ComboValue", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Return True

        Else
            Return False
        End If

    End Function
    Public Sub UpdateComboItemTextInData(cX28_ComboField As BO.x28EntityField, cX25 As BO.x25EntityField_ComboValue)
        Dim strTab As String = ""
        With cX28_ComboField
            _cDB.RunSQL("UPDATE " & .SourceTableName & " SET " & .x28Field & "Text=" & BO.BAS.GS(cX25.x25Name) & " WHERE " & .x28Field & "=" & cX25.PID.ToString)
        End With
    End Sub
    Public Sub ClearComboItemTextInData(cX28_ComboField As BO.x28EntityField, intX25ID As Integer)
        Dim strTab As String = ""
        With cX28_ComboField
            _cDB.RunSQL("UPDATE " & .SourceTableName & " SET " & .x28Field & "Text=NULL," & .x28Field & "=NULL WHERE " & .x28Field & "=" & intX25ID.ToString)
        End With
    End Sub
    Public Function GetList(myQuery As BO.myQueryX25) As IEnumerable(Of BO.x25EntityField_ComboValue)
        If myQuery Is Nothing Then
            myQuery = New BO.myQueryX25(0)
            myQuery.Closed = BO.BooleanQueryMode.NoQuery
        End If
        Dim s As String = "SELECT " & GetSQLPart1(myQuery.TopRecordsOnly)
        Dim strW As String = bas.ParseWhereMultiPIDs("a.x25ID", myQuery)
        strW += bas.ParseWhereValidity("x25", "a", myQuery)
        Dim pars As New DbParameters
        With myQuery
            If .x23ID <> 0 Then
                strW += " AND  a.x23ID=@x23id"
                pars.Add("x23id", .x23ID, DbType.Int32)
            End If
        End With
        
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.x23ID,a.x25Ordinary,a.x25Name"

        Return _cDB.GetList(Of BO.x25EntityField_ComboValue)(s, pars)

    End Function

    Private Function GetSQLWHERE(myQuery As BO.myQueryX25, ByRef pars As DL.DbParameters) As String
        Dim strW As String = bas.ParseWhereMultiPIDs("a.x25ID", myQuery)
        strW += bas.ParseWhereValidity("x25", "a", myQuery)
        With myQuery
            If Not BO.BAS.IsNullDBDate(.DateFrom) Is Nothing Then
                pars.Add("d1", .DateFrom, DbType.DateTime) : pars.Add("d2", .DateUntil, DbType.DateTime)
                strW += " AND " & .DateQueryFieldBy & " BETWEEN @d1 AND @d2"
            End If
            
            If Not .DateInsertFrom Is Nothing Then
                If Year(.DateInsertFrom) > 1900 Then
                    pars.Add("d1", .DateInsertFrom) : pars.Add("d2", .DateInsertUntil)
                    strW += " AND a.x25DateInsert BETWEEN @d1 AND @d2"
                End If
            End If
            If .x23ID <> 0 Then
                strW += " AND  a.x23ID=@x23id"
                pars.Add("x23id", .x23ID, DbType.Int32)
            End If
            If .Record_x29ID > BO.x29IdEnum._NotSpecified And .RecordPID <> 0 Then
                pars.Add("x29id", CInt(.Record_x29ID), DbType.Int32)
                pars.Add("recordpid", .RecordPID, DbType.Int32)
                strW += " AND a.x25ID IN (select x25ID FROM x19EntityCategory_Binding WHERE x29ID=@x29id AND x19RecordPID=@recordpid)"
            End If

            If .ColumnFilteringExpression <> "" Then
                strW += " AND " & .ColumnFilteringExpression
            End If
            If .SearchExpression <> "" Then
                strW += " AND ("
                'něco jako fulltext
                strW += "a.x25Name LIKE '%'+@expr+'%' OR a.x25Code LIKE '%'+@expr+'%' OR a.x25FreeText01 LIKE '%'+@expr+'%' OR a.x25FreeText02 LIKE '%'+@expr+'%' OR a.x25FreeText03 LIKE '%'+@expr+'%' OR a.x25FreeText04 LIKE '%'+@expr+'%' OR a.x25FreeText05 LIKE '%'+@expr+'%' OR a.x25BigText LIKE '%'+@expr+'%'"
                strW += ")"
                pars.Add("expr", .SearchExpression, DbType.String)
            End If
        End With
        strW += bas.ParseWhereValidity("x25", "a", myQuery)
        Return bas.TrimWHERE(strW)
    End Function
    Public Function GetVirtualCount(myQuery As BO.myQueryX25) As Integer
        Dim s As String = "SELECT count(a.x25ID) as Value " & GetSQLPart2_From()
        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        If strW <> "" Then s += " WHERE " & strW

        Return _cDB.GetRecord(Of BO.GetInteger)(s, pars).Value
    End Function
    Public Function GetDataTable4Grid(myQuery As BO.myQueryX25) As DataTable
        Dim s As String = ""
        With myQuery
            If .MG_GridSqlColumns <> "" Then .MG_GridSqlColumns += ","
            .MG_GridSqlColumns += "a.x25ID as pid,CONVERT(BIT,CASE WHEN GETDATE() BETWEEN a.x25ValidFrom AND a.x25ValidUntil THEN 0 else 1 END) as IsClosed,x25Name,x25Code,x25Ordinary"
        End With

        Dim pars As New DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)

        With myQuery
            If .MG_SelectPidFieldOnly Then .MG_GridSqlColumns = "a.x25ID as pid"
            Dim strORDERBY As String = .MG_SortString
            If strORDERBY = "" Then strORDERBY = "a.x25ID DESC"
            If .MG_PageSize > 0 Then
                Dim intStart As Integer = (.MG_CurrentPageIndex) * .MG_PageSize

                s = "WITH rst AS (SELECT ROW_NUMBER() OVER (ORDER BY " & strORDERBY & ")-1 as RowIndex," & .MG_GridSqlColumns & " " & GetSQLPart2_From()

                If strW <> "" Then s += " WHERE " & strW
                s += ") SELECT * FROM rst"
                pars.Add("start", intStart, DbType.Int32)
                pars.Add("end", (intStart + .MG_PageSize - 1), DbType.Int32)
                s += " WHERE RowIndex BETWEEN @start AND @end"
            Else
                'bez stránkování
                s = "SELECT " & .MG_GridSqlColumns & " " & GetSQLPart2_From()
                If strW <> "" Then s += " WHERE " & strW
                s += " ORDER BY " & strORDERBY
            End If


        End With

        Dim ds As DataSet = _cDB.GetDataSet(s, , pars.Convert2PluginDbParameters())
        If Not ds Is Nothing Then Return ds.Tables(0) Else Return Nothing
    End Function
End Class
