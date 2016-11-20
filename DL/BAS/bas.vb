Public Class bas
    Shared Function RecTail(strEntityPrefix As String, Optional strTableAlias As String = "", Optional bolValidity As Boolean = True, Optional bolTimeStamp As Boolean = True) As String
        If strTableAlias <> "" Then strEntityPrefix = strTableAlias & "." & strEntityPrefix
        Dim s As String = strEntityPrefix & "id as _pid"
        If bolTimeStamp Then s += "," & strEntityPrefix & "userupdate as _userupdate," & strEntityPrefix & "dateupdate as _dateupdate," & strEntityPrefix & "userinsert as _userinsert," & strEntityPrefix & "dateinsert as _dateinsert"
        If bolValidity Then
            s += "," & strEntityPrefix & "ValidFrom as _validfrom," & strEntityPrefix & "ValidUntil as _validuntil"
        End If
        Return s
    End Function

    Shared Function RecValiditySqlWhere(strDataPrefix As String, strTableAlias As String, myQuery As BO.myQuery) As String
        If myQuery Is Nothing Then Return "1=1"
        Dim s As String = "", strTA As String = strTableAlias & "."
        If strTA = "." Then strTA = ""
        Select Case myQuery.Closed
            Case BO.BooleanQueryMode.FalseQuery
                Return "getdate() BETWEEN " & strTA & strDataPrefix & "validfrom AND " & strTA & strDataPrefix & "validuntil"
                'Return strTA & strDataPrefix & "validfrom<=getdate() AND " & strTA & strDataPrefix & "validuntil>=getdate()"
            Case BO.BooleanQueryMode.TrueQuery
                Return "NOT (" & strTA & strDataPrefix & "validfrom<=getdate() AND " & strTA & strDataPrefix & "validuntil>=getdate())"
            Case Else
                Return ""
        End Select

    End Function

    Shared Function ParseWhereValidity(strDataPrefix As String, strTableAlias As String, myQuery As BO.myQuery) As String
        If myQuery Is Nothing Then
            Return ""
        End If
        If myQuery.Closed <> BO.BooleanQueryMode.NoQuery Then
            Return " AND " & bas.RecValiditySqlWhere(strDataPrefix, strTableAlias, myQuery)
        Else
            Return ""
        End If
    End Function
    ''Shared Function ParseGridSqlOrderBy(strOrderBy As String) As String
    ''    If strOrderBy = "" Then Return ""
    ''    Dim a() As String = Split(strOrderBy, ","), lis As New List(Of String)
    ''    For i As Integer = 0 To UBound(a)
    ''        If a(i).ToLower.IndexOf(" as ") > 0 Then
    ''            Dim b() As String = Split(a(i).ToLower, " as ")
    ''            If Right(a(i).ToLower, 4) = "desc" Then
    ''                lis.Add(b(0) & " DESC")
    ''            Else
    ''                lis.Add(b(0))
    ''            End If
    ''        Else
    ''            lis.Add(a(i))
    ''        End If
    ''    Next
    ''    Return String.Join(",", lis)
    ''End Function

    Shared Function ParseWhereMultiPIDs(strPKField As String, myQuery As BO.myQuery) As String
        If myQuery Is Nothing Then Return ""
        If myQuery.PIDs Is Nothing Then Return ""
        If myQuery.PIDs.Count = 0 Then Return ""
        Return " AND " & strPKField & " IN (" & String.Join(",", myQuery.PIDs) & ")"
    End Function

    Shared Function TrimWHERE(strWHERESQL As String) As String
        If Trim(strWHERESQL) = "" Then Return ""
        If LCase(Left(LTrim(strWHERESQL), 4)) = "and " Then
            Return Right(LTrim(strWHERESQL), Len(LTrim(strWHERESQL)) - 4)
        End If
        If LCase(Left(LTrim(strWHERESQL), 3)) = "or " Then
            Return Right(LTrim(strWHERESQL), Len(LTrim(strWHERESQL)) - 3)
        End If
        If LCase(Left(LTrim(strWHERESQL), 6)) = "where " Then
            Return Right(LTrim(strWHERESQL), Len(LTrim(strWHERESQL)) - 6)
        End If
       
        Return Trim(strWHERESQL)
    End Function

    Shared Function ParseMergeSQL(strSQL As String, strPIDValue As String) As String
        strSQL = Replace(strSQL, "#pid#", strPIDValue, , , CompareMethod.Text)
        strSQL = Replace(strSQL, "drop ", "", , , CompareMethod.Text)

        Return strSQL
    End Function

    Shared Sub SaveX69(cDB As DbHandler, x29id As BO.x29IdEnum, intDataRecord As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), bolAfterInsert As Boolean, Optional intOnlyX67ID_Update As Integer = 0)
        Dim strSQLDel As String = "DELETE a FROM x69EntityRole_Assign a INNER JOIN x67EntityRole b ON a.x67ID=b.x67ID WHERE b.x29ID=" & CInt(x29id).ToString & " AND a.x69RecordPID=" & intDataRecord.ToString
        If intOnlyX67ID_Update <> 0 Then
            strSQLDel += " AND a.x67ID=" & intOnlyX67ID_Update.ToString   'jedná se o aktualizaci pouze jedné role (x67id)
        End If
        If Not bolAfterInsert Or intOnlyX67ID_Update <> 0 Then
            cDB.RunSQL(strSQLDel)
        End If
        For Each c In lisX69
            If Not c.IsSetAsDeleted Then
                Dim strJ02ID As String = "NULL"
                If c.j02ID <> 0 Then strJ02ID = c.j02ID.ToString
                Dim strJ11ID As String = "NULL"
                If c.j11ID <> 0 Then strJ11ID = c.j11ID.ToString
                Dim strJ07ID As String = "NULL"
                If c.j07ID <> 0 Then strJ07ID = c.j07ID.ToString

                cDB.RunSQL("INSERT INTO x69EntityRole_Assign(x69RecordPID,x67ID,j02ID,j11ID,j07ID,x69IsWelcomeNotification) VALUES(" & intDataRecord.ToString & "," & c.x67ID.ToString & "," & strJ02ID & "," & strJ11ID & "," & strJ07ID & "," & BO.BAS.GB(c.x69IsWelcomeNotification) & ")")
            End If
        Next
    End Sub

    
    Shared Function SaveFreeFields(cDB As DbHandler, lisFF As List(Of BO.FreeField), strTableFF As String, intPID As Integer, Optional cCurUser As BO.j03UserSYS = Nothing) As Boolean
        If lisFF.Count = 0 Then Return False
        Dim bolInsert As Boolean = True, strW As String = "", strFieldPID As String = Left(strTableFF, 3) & "ID"
        If cDB.GetIntegerValueFROMSQL("SELECT " & strFieldPID & " FROM " & strTableFF & " WHERE " & strFieldPID & "=" & intPID.ToString) <> 0 Then
            bolInsert = False : strW = strFieldPID & "=@pid"
        End If
        Dim pars As New DbParameters
        If bolInsert Then
            pars.Add(strFieldPID, intPID, DbType.Int32)
        Else
            pars.Add("pid", intPID, DbType.Int32)
        End If

        For Each c In lisFF
            Select Case c.x24ID
                Case BO.x24IdENUM.tString
                    pars.Add(c.x28Field, c.DBValue, DbType.String)
                Case BO.x24IdENUM.tDate, BO.x24IdENUM.tDateTime
                    pars.Add(c.x28Field, c.DBValue, DbType.DateTime)
                Case BO.x24IdENUM.tDecimal
                    pars.Add(c.x28Field, c.DBValue, DbType.Decimal)
                Case BO.x24IdENUM.tInteger
                    If c.x23ID = 0 Then
                        pars.Add(c.x28Field, c.DBValue, DbType.Int32)
                    Else
                        pars.Add(c.x28Field, BO.BAS.IsNullDBKey(c.DBValue), DbType.Int32)   'combo položka (ID)
                        pars.Add(c.x28Field & "Text", c.ComboText, DbType.String)           'combo položka (TEXT)
                        
                    End If

                Case BO.x24IdENUM.tBoolean
                    pars.Add(c.x28Field, c.DBValue, DbType.Boolean)
            End Select
        Next
        If cDB.SaveRecord(strTableFF, pars, bolInsert, strW, , , False) Then
            Return True
        Else
            Return False
        End If
    End Function

    Shared Function GetList_x90(cDB As DbHandler, x29id As BO.x29IdEnum, intPID As Integer, datFrom As Date, datUntil As Date) As IEnumerable(Of BO.x90EntityLog)
        Dim s As String = "select a.*,j02.j02LastName+' '+j02.j02FirstName as Person FROM x90EntityLog a INNER JOIN j02Person j02 ON a.j02ID_Author=j02.j02ID WHERE a.x90RecordPID=@pid AND a.x29ID=@x29id AND a.x90Date BETWEEN @datfrom AND @datuntil"
        Dim pars As New DbParameters
        pars.Add("x29id", x29id, DbType.Int32)
        pars.Add("pid", intPID, DbType.Int32)
        pars.Add("datfrom", datFrom, DbType.DateTime)
        pars.Add("datuntil", datUntil, DbType.DateTime)

        Return cDB.GetList(Of BO.x90EntityLog)(s, pars)
    End Function

    Shared Function ClearSqlForAttacks(strSQL As String) As String
        strSQL = Replace(Trim(strSQL), "DROP", "##", , , CompareMethod.Text)  'očištění SQL
        strSQL = Replace(strSQL, "DELETE", "##", , , CompareMethod.Text)  'očištění SQL
        strSQL = Replace(strSQL, "TRUNCATE", "##", , , CompareMethod.Text)  'očištění SQL
        strSQL = Replace(strSQL, "--", "##")  'očištění SQL
        Return strSQL
    End Function

    Shared Function CompleteSqlJ70(cDB As DL.DbHandler, intJ70ID As Integer, cUser As BO.j03UserSYS) As String
        Dim s As String = "select *," & bas.RecTail("j70") & " from j70QueryTemplate WHERE j70ID=@pid"
        Dim cRec As BO.j70QueryTemplate = cDB.GetRecord(Of BO.j70QueryTemplate)(s, New With {.pid = intJ70ID}), sql As New System.Text.StringBuilder
        Dim strFK As String = ""
        Select Case cRec.x29ID
            Case BO.x29IdEnum.p41Project : strFK = "a.p41ID"
            Case BO.x29IdEnum.p28Contact : strFK = "a.p28ID"
            Case BO.x29IdEnum.p91Invoice : strFK = "a.p91ID"
            Case BO.x29IdEnum.p56Task : strFK = "a.p56ID"
            Case BO.x29IdEnum.o23Notepad : strFK = "a.o23ID"
            Case BO.x29IdEnum.j02Person : strFK = "a.j02ID"
        End Select
        Select Case cRec.j70BinFlag
            Case 1  'otevřené záznamy
                Select Case cRec.x29ID
                    Case BO.x29IdEnum.p28Contact
                        sql.Append(" AND " & GetQuickQuerySQL_p28(BO.myQueryP28_QuickQuery.OpenClients))
                    Case BO.x29IdEnum.p41Project
                        sql.Append(" AND " & GetQuickQuerySQL_p41(BO.myQueryP41_QuickQuery.OpenProjects, cUser))
                    Case BO.x29IdEnum.p91Invoice
                        sql.Append(" AND " & GetQuickQuerySQL_p91(BO.myQueryP91_QuickQuery.OpenInvoices))
                    Case BO.x29IdEnum.j02Person
                        sql.Append(" AND " & GetQuickQuerySQL_j02(BO.myQueryJ02_QuickQuery.OpenPersons))
                    Case BO.x29IdEnum.p31Worksheet
                        sql.Append(" AND NOT " & GetQuickQuerySQL_p31(BO.myQueryP31_QuickQuery.MovedToBin))
                    Case BO.x29IdEnum.p56Task
                        sql.Append(" AND " & GetQuickQuerySQL_p56(BO.myQueryP56_QuickQuery.OpenTasks))
                    Case BO.x29IdEnum.o23Notepad
                        sql.Append(" AND " & GetQuickQuerySQL_o23(BO.myQueryO23_QuickQuery.OpenNotepads))
                End Select
            Case 2  'záznamy v koši
                Select Case cRec.x29ID
                    Case BO.x29IdEnum.p28Contact
                        sql.Append("AND " & GetQuickQuerySQL_p28(BO.myQueryP28_QuickQuery.Removed2Bin))
                    Case BO.x29IdEnum.p41Project
                        sql.Append(" AND " & GetQuickQuerySQL_p41(BO.myQueryP41_QuickQuery.Removed2Bin, cUser))
                    Case BO.x29IdEnum.p91Invoice
                        sql.Append(" AND " & GetQuickQuerySQL_p91(BO.myQueryP91_QuickQuery.Removed2Bin))
                    Case BO.x29IdEnum.j02Person
                        sql.Append(" AND " & GetQuickQuerySQL_j02(BO.myQueryJ02_QuickQuery.Removed2Bin))
                    Case BO.x29IdEnum.p31Worksheet
                        sql.Append(" AND " & GetQuickQuerySQL_p31(BO.myQueryP31_QuickQuery.MovedToBin))
                    Case BO.x29IdEnum.p56Task
                        sql.Append(" AND " & GetQuickQuerySQL_p56(BO.myQueryP56_QuickQuery.Removed2Bin))
                    Case BO.x29IdEnum.o23Notepad
                        sql.Append(" AND " & GetQuickQuerySQL_o23(BO.myQueryO23_QuickQuery.Removed2Bin))
                End Select
        End Select
        s = "select * FROM j71QueryTemplate_Item WHERE j70ID=@pid order by j71Field"
        Dim lis As IEnumerable(Of BO.j71QueryTemplate_Item) = cDB.GetList(Of BO.j71QueryTemplate_Item)(s, New With {.pid = intJ70ID})
        If lis.Count = 0 Then
            Return TrimWHERE(sql.ToString)
        End If
        Dim fields As List(Of String) = lis.Select(Function(p) p.j71Field).Distinct.ToList, x As Integer = 0
        For Each strField In fields
            Dim strIN As String = String.Join(",", lis.Where(Function(p) p.j71Field = strField).Select(Function(r) r.j71RecordPID))

            Select Case LCase(strField)
                Case "_other"
                Case "x67id"
                Case "j11id"
                    sql.Append(" AND a.j02ID IN (SELECT j02ID FROM j12Team_Person WHERE j11ID IN (" & strIN & "))")

                Case "x25id"    'štítky
                    sql.Append(" AND " & strFK & " IN (SELECT x19RecordPID FROM x19EntityCategory_Binding WHERE x29ID=" & CInt(cRec.x29ID).ToString & " AND x25ID IN (" & strIN & "))")
                Case "p34id"
                    sql.Append(" AND p32.p34ID IN (" & strIN & ")")
                Case "p95id"
                    sql.Append(" AND p32.p95ID IN (" & strIN & ")")
                Case Else
                    If LCase(strField).IndexOf("free") > 0 Then
                        'filtrování podle volného pole
                        Dim strW As String = strField & " IN (" & strIN & ")"
                        Dim lisRows As IEnumerable(Of BO.j71QueryTemplate_Item) = lis.Where(Function(p) p.j71Field = strField)
                        Dim strValueType As String = lisRows(0).j71ValueType
                        If strValueType <> "combo" Then
                            strW = ""
                            For Each c In lisRows
                                Select Case strValueType
                                    Case "date"
                                        If strW = "" Then
                                            strW = strField & " BETWEEN convert(datetime,'" & c.j71ValueFrom & "',104) AND convert(datetime,'" & c.j71ValueUntil & "',104)"
                                        Else
                                            strW += " OR " & strField & " BETWEEN convert(datetime,'" & c.j71ValueFrom & "',104) AND convert(datetime,'" & c.j71ValueUntil & "',104)"
                                        End If
                                    Case "number"
                                        If LCase(strField).IndexOf("freecombo") > 0 Then
                                            If strW = "" Then
                                                strW = strField & " = " & c.j71RecordPID.ToString
                                            Else
                                                strW += " OR " & strField & " = " & c.j71RecordPID.ToString
                                            End If
                                        Else
                                            If strW = "" Then
                                                strW = strField & " BETWEEN " & BO.BAS.GN(c.j71ValueFrom) & " AND " & BO.BAS.GN(c.j71ValueUntil)
                                            Else
                                                strW += " OR " & strField & " BETWEEN " & BO.BAS.GN(c.j71ValueFrom) & " AND " & BO.BAS.GN(c.j71ValueUntil)
                                            End If
                                        End If

                                    Case "string"
                                        Dim ss As String = ""
                                        Select Case c.j71StringOperator
                                            Case "CONTAIN"
                                                ss = " LIKE " & BO.BAS.GS("%" & c.j71ValueString & "%")
                                            Case "START"
                                                ss = " LIKE " & BO.BAS.GS(c.j71ValueString & "%")
                                            Case "EQUAL"
                                                ss = " = " & BO.BAS.GS(c.j71ValueString)
                                            Case "EMPTY"
                                                ss = " IS NULL"
                                            Case "NOTEMPTY"
                                                ss = " IS NOT NULL"
                                        End Select
                                        If strW = "" Then
                                            strW = strField & ss
                                        Else
                                            strW += " OR " & strField & ss
                                        End If
                                    Case "boolean"
                                        If strW = "" Then
                                            strW = strField & "=" & c.j71RecordPID.ToString
                                        Else
                                            strW += " OR " & strField & "=" & c.j71RecordPID.ToString
                                        End If
                                End Select
                            Next
                        End If
                        Select Case cRec.x29ID
                            Case BO.x29IdEnum.p41Project
                                sql.Append(" AND a.p41ID IN (SELECT p41ID FROM p41Project_FreeField WHERE " & strW & ")")
                            Case BO.x29IdEnum.p28Contact
                                sql.Append(" AND a.p28ID IN (SELECT p28ID FROM p28Contact_FreeField WHERE " & strW & ")")
                            Case BO.x29IdEnum.p91Invoice
                                sql.Append(" AND a.p91ID IN (SELECT p91ID FROM p91Invoice_FreeField WHERE " & strW & ")")
                            Case BO.x29IdEnum.j02Person
                                sql.Append(" AND a.j02ID IN (SELECT j02ID FROM j02Person_FreeField WHERE " & strW & ")")
                            Case BO.x29IdEnum.p56Task
                                sql.Append(" AND a.p56ID IN (SELECT p56ID FROM p56Task_FreeField WHERE " & strW & ")")
                            Case BO.x29IdEnum.o23Notepad
                                sql.Append(" AND a.o23ID IN (SELECT o23ID FROM o23Notepad_FreeField WHERE " & strW & ")")
                        End Select

                    Else
                        If strField.IndexOf(".") > 0 Then
                            sql.Append(" AND " & strField & " IN (" & strIN & ")")
                        Else
                            sql.Append(" AND a." & strField & " IN (" & strIN & ")")
                        End If

                    End If

            End Select

        Next
        Dim lis1 As IEnumerable(Of BO.j71QueryTemplate_Item) = lis.Where(Function(p) p.j71Field = "x67id")
        If lis1.Count > 0 Then
            sql.Append(" AND (")
            x = 0
            For Each c In lis1
                If x > 0 Then sql.Append(" OR ")
                Select Case cRec.x29ID
                    Case BO.x29IdEnum.p41Project
                        sql.Append("a.p41ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                    Case BO.x29IdEnum.p56Task
                        sql.Append("a.p56ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                    Case BO.x29IdEnum.o23Notepad
                        sql.Append("a.o23ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                    Case BO.x29IdEnum.p28Contact
                        sql.Append("a.p28ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                    Case BO.x29IdEnum.p31Worksheet
                        sql.Append("a.p41ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                End Select
                If c.j71RecordPID_Extension = -1 Then
                    c.j71RecordPID_Extension = cUser.j02ID    'aktuálně přihlášená osoba
                End If
                If c.j71RecordPID_Extension > 0 Then
                    sql.Append(" AND (j02ID=" & c.j71RecordPID_Extension.ToString & " OR j11ID IN (select j11ID FROM j12Team_Person WHERE j02ID=" & c.j71RecordPID_Extension.ToString & "))")
                End If
                sql.Append(")")
                x += 1
            Next
            sql.Append(")")
        End If
        lis1 = lis.Where(Function(p) p.j71Field = "x67id_j11")
        If lis1.Count > 0 Then
            sql.Append(" AND (")
            x = 0
            For Each c In lis1
                If x > 0 Then sql.Append(" OR ")
                Select Case cRec.x29ID
                    Case BO.x29IdEnum.p41Project
                        sql.Append("a.p41ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                    Case BO.x29IdEnum.p56Task
                        sql.Append("a.p56ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                    Case BO.x29IdEnum.o23Notepad
                        sql.Append("a.o23ID IN (SELECT x69RecordPID FROM x69EntityRole_Assign WHERE x67ID=" & c.j71RecordPID.ToString)
                End Select
                If c.j71RecordPID_Extension > 0 Then
                    sql.Append(" AND j11ID = " & c.j71RecordPID_Extension.ToString & ")")
                End If
                sql.Append(")")
                x += 1
            Next
            sql.Append(")")
        End If
        lis1 = lis.Where(Function(p) p.j71Field = "_other")
        If lis1.Count > 0 Then
            sql.Append(" AND (")
            x = 0
            For Each c In lis1
                If x > 0 Then sql.Append(" OR ")
                Select Case cRec.x29ID
                    Case BO.x29IdEnum.p41Project
                        sql.Append(GetQuickQuerySQL_p41(CType(c.j71RecordPID, BO.myQueryP41_QuickQuery), cUser))
                    Case BO.x29IdEnum.p28Contact
                        sql.Append(GetQuickQuerySQL_p28(CType(c.j71RecordPID, BO.myQueryP28_QuickQuery)))
                    Case BO.x29IdEnum.p91Invoice
                        sql.Append(GetQuickQuerySQL_p91(CType(c.j71RecordPID, BO.myQueryP91_QuickQuery)))
                    Case BO.x29IdEnum.j02Person
                        sql.Append(GetQuickQuerySQL_j02(CType(c.j71RecordPID, BO.myQueryJ02_QuickQuery)))
                    Case BO.x29IdEnum.p31Worksheet
                        sql.Append(GetQuickQuerySQL_p31(CType(c.j71RecordPID, BO.myQueryP31_QuickQuery)))
                    Case BO.x29IdEnum.p56Task
                        sql.Append(GetQuickQuerySQL_p56(CType(c.j71RecordPID, BO.myQueryP56_QuickQuery)))
                    Case BO.x29IdEnum.o23Notepad
                        sql.Append(GetQuickQuerySQL_o23(CType(c.j71RecordPID, BO.myQueryO23_QuickQuery)))
                End Select

                x += 1
            Next
            sql.Append(")")
        End If

        If cRec.j70IsNegation Then
            Return "NOT (" & TrimWHERE(sql.ToString) & ")"
        Else
            Return TrimWHERE(sql.ToString)
        End If

    End Function
    Shared Function GetQuickQuerySQL_p56(quickQueryFlag As BO.myQueryP56_QuickQuery) As String
        Select Case quickQueryFlag
            Case BO.myQueryP56_QuickQuery.OpenTasks
                Return "getdate() BETWEEN a.p56ValidFrom AND a.p56ValidUntil"
            Case BO.myQueryP56_QuickQuery.Removed2Bin
                Return "getdate() NOT BETWEEN a.p56ValidFrom AND a.p56ValidUntil"
            Case BO.myQueryP56_QuickQuery.WaitingOnApproval
                Return "a.p56ID IN (SELECT p56ID FROM p31Worksheet WHERE p56ID IS NOT NULL AND p71ID IS NULL AND p91ID IS NULL AND getdate() BETWEEN p31ValidFrom AND p31ValidUntil)"
            Case BO.myQueryP56_QuickQuery.WaitingOnInvoice
                Return "a.p56ID IN (SELECT p56ID FROM p31Worksheet WHERE p56ID IS NOT NULL AND p71ID=1 AND p91ID IS NULL)"
            Case BO.myQueryP56_QuickQuery.Is_PlanUntil
                Return "a.p56PlanUntil IS NOT NULL"
            Case BO.myQueryP56_QuickQuery.Is_OverPlanUtil
                Return "a.p56PlanUntil IS NOT NULL AND a.p56PlanUntil<GETDATE()"
            Case BO.myQueryP56_QuickQuery.Is_PlanFrom
                Return "a.p56PlanFrom IS NOT NULL"
            Case BO.myQueryP56_QuickQuery.Is_PlanHours
                Return "ISNULL(a.p56Plan_Hours,0)>0"
            Case BO.myQueryP56_QuickQuery.Is_PlanExpenses
                Return "ISNULL(a.p56Plan_Expenses,0)>0"
            Case BO.myQueryP56_QuickQuery.Is_OverPlanHours
                Return "ISNULL(a.p56Plan_Hours,0)>0 AND a.P56ID IN (SELECT p56ID FROM p31Worksheet WHERE p56ID=a.p56ID GROUP BY p56ID HAVING sum(p31Hours_Orig)>a.p56Plan_Hours)"
            Case BO.myQueryP56_QuickQuery.Is_OverPlanEpenses
                Return "ISNULL(a.p56Plan_Expenses,0)>0 AND a.P56ID IN (SELECT p56ID FROM p31Worksheet xa INNER JOIN p32Activity xb ON xa.p32ID=xb.p32ID INNER JOIN p34ActivityGroup xc ON xb.p34ID=xc.p34ID WHERE xa.p56ID=a.p56ID AND xc.p33ID IN (2,5) AND xc.p34IncomeStatementFlag=1 GROUP BY xa.p56ID HAVING sum(xa.p31Amount_WithoutVat_Orig)>a.p56Plan_Expenses)"
            Case Else
                Return ""
        End Select
    End Function
    Shared Function GetQuickQuerySQL_o23(quickQueryFlag As BO.myQueryO23_QuickQuery) As String
        Select Case quickQueryFlag
            Case BO.myQueryO23_QuickQuery.OpenNotepads
                Return "getdate() BETWEEN a.o23ValidFrom AND a.o23ValidUntil"
            Case BO.myQueryO23_QuickQuery.Removed2Bin
                Return "getdate() NOT BETWEEN a.o23ValidFrom AND a.o23ValidUntil"
            Case BO.myQueryO23_QuickQuery.Bind2ClientExist
                Return "a.p28ID IS NOT NULL"
            Case BO.myQueryO23_QuickQuery.Bind2ClientWait
                Return "a.p28ID IS NULL AND o24.x29ID=328"
            Case BO.myQueryO23_QuickQuery.Bind2InvoiceExist
                Return "a.p91ID IS NOT NULL"
            Case BO.myQueryO23_QuickQuery.Bind2InvoiceWait
                Return "a.p91ID IS NULL AND o24.x29ID=391"
            Case BO.myQueryO23_QuickQuery.Bind2PersonExist
                Return "a.j02ID IS NOT NULL"
            Case BO.myQueryO23_QuickQuery.Bind2PersonWait
                Return "a.j02ID IS NULL AND o24.x29ID=102"
            Case BO.myQueryO23_QuickQuery.Bind2ProjectExist
                Return "a.p41ID IS NOT NULL"
            Case BO.myQueryO23_QuickQuery.Bind2ProjectWait
                Return "a.p41ID IS NULL AND o24.x29ID=141"
            Case BO.myQueryO23_QuickQuery.Bind2WorksheetExist
                Return "a.p31ID IS NOT NULL"
            Case BO.myQueryO23_QuickQuery.Bind2WorksheetWait
                Return "a.p31ID IS NULL AND o24.x29ID=331"
            Case Else
                Return ""
        End Select
    End Function
    Shared Function GetQuickQuerySQL_p28(quickQueryFlag As BO.myQueryP28_QuickQuery) As String
        Select Case quickQueryFlag
            Case BO.myQueryP28_QuickQuery.OpenClients
                Return "getdate() BETWEEN a.p28ValidFrom AND a.p28ValidUntil"
            Case BO.myQueryP28_QuickQuery.Removed2Bin
                Return "getdate() NOT BETWEEN a.p28ValidFrom AND a.p28ValidUntil"
            Case BO.myQueryP28_QuickQuery.WaitingOnApproval
                Return "a.p28ID IN (SELECT xb.p28ID_Client FROM p31Worksheet xa INNER JOIN p41Project xb ON xa.p41ID=xb.p41ID WHERE xb.p28ID_Client IS NOT NULL AND xa.p71ID IS NULL AND xa.p91ID IS NULL AND getdate() BETWEEN p31ValidFrom AND p31ValidUntil)"
            Case BO.myQueryP28_QuickQuery.WaitingOnInvoice
                Return "a.p28ID IN (SELECT xb.p28ID_Client FROM p31Worksheet xa INNER JOIN p41Project xb ON xa.p41ID=xb.p41ID WHERE xb.p28ID_Client IS NOT NULL AND xa.p71ID=1 AND xa.p91ID IS NULL)"
            Case BO.myQueryP28_QuickQuery.WIthNotepad
                Return "a.p28ID IN (SELECT p28ID FROM o23Notepad WHERE p28ID IS NOT NULL AND getdate() BETWEEN o23ValidFrom AND o23ValidUntil)"
            Case BO.myQueryP28_QuickQuery.OverWorksheetLimit
                Dim s As String = "a.p28ID IN ("
                s += "SELECT xb.p28ID_Client"
                s += " FROM p31Worksheet xa INNER JOIN p41Project xb ON xa.p41ID=xb.p41ID"
                s += " WHERE (xb.p41LimitHours_Notification>0 OR xb.p41LimitFee_Notification>0) AND xa.p71ID IS NULL AND getdate() BETWEEN xa.p31ValidFrom AND xa.p31ValidUntil"
                s += " GROUP BY xb.p28ID_Client"
                s += " HAVING sum(xa.p31Hours_Orig)>min(xb.p41LimitHours_Notification) OR sum(xa.p31Amount_WithoutVat_Orig)>min(xb.p41LimitFee_Notification)"
                s += ")"
                Return s
            Case BO.myQueryP28_QuickQuery.ProjectClient
                Return "a.p28ID IN (SELECT p28ID_Client FROM p41Project)"
            Case BO.myQueryP28_QuickQuery.ProjectInvoiceReceiver
                Return "a.p28ID IN (SELECT p28ID_Billing FROM p41Project)"
            Case BO.myQueryP28_QuickQuery.DraftClients
                Return "a.p28IsDraft=1"
            Case BO.myQueryP28_QuickQuery.NonDraftCLients
                Return "a.p28IsDraft=0"
            Case BO.myQueryP28_QuickQuery.WithContactPersons
                Return "a.p28ID IN (SELECT p28ID FROM p30Contact_Person WHERE p28ID IS NOT NULL)"
            Case BO.myQueryP28_QuickQuery.WithoutContactPersons
                Return "a.p28ID NOT IN (SELECT p28ID FROM p30Contact_Person WHERE p28ID IS NOT NULL)"
            Case BO.myQueryP28_QuickQuery.WithProjects
                Return "a.p28ID IN (SELECT p28ID_Client FROM p41Project WHERE p28ID_Client IS NOT NULL)"
            Case BO.myQueryP28_QuickQuery.WithoutProjects
                Return "a.p28ID NOT IN (SELECT p28ID_Client FROM p41Project WHERE p28ID_Client IS NOT NULL)"
            Case BO.myQueryP28_QuickQuery.SupplierSide
                Return "a.p28SupplierFlag IN (2,3)"
            Case BO.myQueryP28_QuickQuery.NotClientNotSupplier
                Return "a.p28SupplierFlag=4"
            Case BO.myQueryP28_QuickQuery.DuplicityInCompanyName
                Return "lower(left(a.p28CompanyName,25)) IN (select lower(left(p28CompanyName,25)) FROM p28Contact where p28CompanyName is not null and p28ID<>a.p28ID)"
            Case BO.myQueryP28_QuickQuery.DuplicityRegID
                Return "lower(a.p28RegID) IN (select lower(p28RegID) FROM p28Contact where p28RegID is not null and p28ID<>a.p28ID)"
            Case BO.myQueryP28_QuickQuery.DuplicityVatID
                Return "lower(a.p28VatID) IN (select lower(p28VatID) FROM p28Contact where p28VatID is not null and p28ID<>a.p28ID)"
            Case BO.myQueryP28_QuickQuery.WithParentContact
                Return "a.p28ParentID IS NOT NULL"
            Case BO.myQueryP28_QuickQuery.WithChildContact
                Return "a.p28ID IN (SELECT p28ParentID FROM p28Contact WHERE p28ParentID IS NOT NULL)"
            Case BO.myQueryP28_QuickQuery.WithOverHead
                Return "a.p63ID IS NOT NULL"
            Case BO.myQueryP28_QuickQuery.WithBillingMemo
                Return "a.p28BillingMemo IS NOT NULL"
            Case Else
                Return ""
        End Select
    End Function
    Shared Function GetQuickQuerySQL_p41(quickQueryFlag As BO.myQueryP41_QuickQuery, cUser As BO.j03UserSYS) As String
        Select Case quickQueryFlag
            Case BO.myQueryP41_QuickQuery.OpenProjects
                Return "getdate() BETWEEN a.p41ValidFrom AND a.p41ValidUntil"
            Case BO.myQueryP41_QuickQuery.Removed2Bin
                Return "getdate() NOT BETWEEN a.p41ValidFrom AND a.p41ValidUntil"
            Case BO.myQueryP41_QuickQuery.WaitingOnApproval
                Return "a.p41ID IN (SELECT p41ID FROM p31Worksheet WHERE p71ID IS NULL AND p91ID IS NULL AND getdate() BETWEEN p31ValidFrom AND p31ValidUntil)"
            Case BO.myQueryP41_QuickQuery.WaitingOnInvoice
                Return "a.p41ID IN (SELECT p41ID FROM p31Worksheet WHERE p71ID=1 AND p91ID IS NULL AND getdate() BETWEEN p31ValidFrom AND p31ValidUntil)"
            Case BO.myQueryP41_QuickQuery.OverWorksheetLimit
                Dim s As String = "(a.p41ID IN ("
                s += "SELECT xa.p41ID"
                s += " FROM p31Worksheet xa INNER JOIN p41Project xb ON xa.p41ID=xb.p41ID"
                s += " WHERE xb.p41LimitHours_Notification>0 AND xa.p71ID IS NULL AND getdate() BETWEEN xa.p31ValidFrom AND xa.p31ValidUntil"
                s += " GROUP BY xa.p41ID"
                s += " HAVING sum(xa.p31Hours_Orig)>min(xb.p41LimitHours_Notification)"
                s += ")"
                s += " OR a.p41ID IN (SELECT xa.p41ID"
                s += " FROM p31Worksheet xa INNER JOIN p41Project xb ON xa.p41ID=xb.p41ID"
                s += " WHERE xb.p41LimitFee_Notification>0 AND xa.p71ID IS NULL AND getdate() BETWEEN xa.p31ValidFrom AND xa.p31ValidUntil"
                s += " GROUP BY xa.p41ID"
                s += " HAVING sum(xa.p31Amount_WithoutVat_Orig)>min(xb.p41LimitFee_Notification)"
                s += "))"
                Return s
            Case BO.myQueryP41_QuickQuery.WithOpenTasks
                Return "a.p41ID IN (SELECT p41ID FROM p56Task WHERE getdate() BETWEEN p56ValidFrom AND p56ValidUntil)"
            Case BO.myQueryP41_QuickQuery.WithAnyTasks
                Return "a.p41ID IN (SELECT p41ID FROM p56Task)"
            Case BO.myQueryP41_QuickQuery.WithFutureMilestones
                Return "a.p41ID IN (SELECT p41ID FROM o22Milestone WHERE o22DateUntil>getdate() AND getdate() BETWEEN o22ValidFrom AND o22ValidUntil)"
            Case BO.myQueryP41_QuickQuery.WithNotepad
                Return "a.p41ID IN (SELECT p41ID FROM o23Notepad WHERE p41ID IS NOT NULL AND getdate() BETWEEN o23ValidFrom AND o23ValidUntil)"
            Case BO.myQueryP41_QuickQuery.WithRecurrenceWorksheet
                Return "a.p41ID IN (SELECT p41ID FROM p40WorkSheet_Recurrence WHERE getdate() BETWEEN p40ValidFrom AND p40ValidUntil)"
            Case BO.myQueryP41_QuickQuery.DraftProjects
                Return "a.p41IsDraft=1"
            Case BO.myQueryP41_QuickQuery.NonDraftProjects
                Return "a.p41IsDraft=0"
            Case BO.myQueryP41_QuickQuery.WithoutPricelist
                Return "(a.p51ID_Billing IS NULL AND p28client.p51ID_Billing IS NULL)"
            Case BO.myQueryP41_QuickQuery.WithPricelist
                Return "(a.p51ID_Billing IS NOT NULL OR p28client.p51ID_Billing IS NOT NULL)"
            Case BO.myQueryP41_QuickQuery.Invoiced
                Return "a.p41ID IN (SELECT p41ID FROM p31Worksheet WHERE p91ID IS NOT NULL)"
            Case BO.myQueryP41_QuickQuery.WithContactPersons
                Return "a.p41ID IN (SELECT p41ID FROM p30Contact_Person WHERE p41ID IS NOT NULL)"
            Case BO.myQueryP41_QuickQuery.WithoutContactPersons
                Return "a.p41ID NOT IN (SELECT p41ID FROM p30Contact_Person WHERE p41ID IS NOT NULL)"
            Case BO.myQueryP41_QuickQuery.WithParentProject
                Return "a.p41ParentID IS NOT NULL"
            Case BO.myQueryP41_QuickQuery.WithChildProject
                Return "a.p41ID IN (SELECT p41ParentID FROM p41Project WHERE p41ParentID IS NOT NULL)"
            Case BO.myQueryP41_QuickQuery.Favourites
                Return "a.p41ID IN (SELECT p41ID FROM j13FavourteProject WHERE j03ID=" & cUser.PID.ToString & ")"
            Case BO.myQueryP41_QuickQuery.WithBillingMemo
                Return "a.p41BillingMemo IS NOT NULL"
            Case Else
                Return ""
        End Select
    End Function
    Shared Function GetQuickQuerySQL_j02(quickQueryFlag As BO.myQueryJ02_QuickQuery) As String
        Select Case quickQueryFlag
            Case BO.myQueryJ02_QuickQuery._NotSpecified
                Return ""
                ''Return "getdate() BETWEEN a.j02ValidFrom AND a.j02ValidUntil"
            Case BO.myQueryJ02_QuickQuery.Removed2Bin
                Return "getdate() NOT BETWEEN a.j02ValidFrom AND a.j02ValidUntil"
            Case BO.myQueryJ02_QuickQuery.OpenPersons
                Return "getdate() BETWEEN a.j02ValidFrom AND a.j02ValidUntil"
            Case BO.myQueryJ02_QuickQuery.WaitingOnApproval
                Return "a.j02ID IN (SELECT j02ID FROM p31Worksheet WHERE p71ID IS NULL AND p91ID IS NULL AND getdate() BETWEEN p31ValidFrom AND p31ValidUntil)"
            Case BO.myQueryP41_QuickQuery.WaitingOnInvoice
                Return "a.j02ID IN (SELECT j02ID FROM p31Worksheet WHERE p71ID=1 AND p91ID IS NULL AND getdate() BETWEEN p31ValidFrom AND p31ValidUntil)"
            Case BO.myQueryJ02_QuickQuery.NonIntraPersonsOnly
                Return "a.j02IsIntraPerson=0"
            Case BO.myQueryJ02_QuickQuery.IntraPersonsOnly
                Return "a.j02IsIntraPerson=1"
            Case Else
                Return ""
        End Select
    End Function
    Shared Function GetQuickQuerySQL_p91(quickQueryFlag As BO.myQueryP91_QuickQuery) As String
        Select Case quickQueryFlag
            Case BO.myQueryP91_QuickQuery.OpenInvoices
                Return "getdate() BETWEEN a.p91ValidFrom AND a.p91ValidUntil"
            Case BO.myQueryP91_QuickQuery.Removed2Bin
                Return "getdate() NOT BETWEEN a.p91ValidFrom AND a.p91ValidUntil"
            Case BO.myQueryP91_QuickQuery.DebtAfterMaturity
                Return "a.p91Amount_Debt>10 AND a.p91DateMaturity<dbo.convert_to_dateserial(getdate()) and a.p91IsDraft=0"
            Case BO.myQueryP91_QuickQuery.WithDebt
                Return "a.p91Amount_Debt>10 and a.p91IsDraft=0"
            Case BO.myQueryP91_QuickQuery.InMaturity
                Return "a.p91DateMaturity>=dbo.convert_to_dateserial(getdate()) and a.p91IsDraft=0"
            Case BO.myQueryP91_QuickQuery.IsDraft
                Return "a.p91IsDraft=1"
            Case BO.myQueryP91_QuickQuery.IsOficialCode
                Return "a.p91IsDraft=0"
            Case BO.myQueryP91_QuickQuery.BoundWithProforma
                Return "a.p91ID IN (SELECT p91ID FROM p99Invoice_Proforma)"
            Case BO.myQueryP91_QuickQuery.BoundWithCreditNote
                Return "a.p91ID_CreditNoteBind IS NOT NULL"
            Case BO.myQueryP91_QuickQuery.Is_p91RoundFitAmount
                Return "a.p91RoundFitAmount<>0"
            Case BO.myQueryP91_QuickQuery.Is_p91Amount_WithoutVat_Standard
                Return "a.p91Amount_WithoutVat_Standard<>0"
            Case BO.myQueryP91_QuickQuery.Is_p91Amount_WithoutVat_Low
                Return "a.p91Amount_WithoutVat_Low<>0"
            Case BO.myQueryP91_QuickQuery.Is_p91Amount_WithoutVat_None
                Return "a.p91Amount_WithoutVat_None<>0"
            Case BO.myQueryP91_QuickQuery.Is_ExchangeRate
                Return "isnull(a.p91ExchangeRate,1)<>1"
            Case BO.myQueryP91_QuickQuery.WithOverhead
                Return "a.p63ID IS NOT NULL"
            Case Else
                Return ""
        End Select
    End Function
    Shared Function GetQuickQuerySQL_p31(quickQueryFlag As BO.myQueryP31_QuickQuery) As String
        Select Case quickQueryFlag
            Case BO.myQueryP31_QuickQuery.Approved
                Return "a.p71ID=1 AND a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil"
            Case BO.myQueryP31_QuickQuery.Editing
                Return "a.p71ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil"
            Case BO.myQueryP31_QuickQuery.EditingOrApproved
                Return "a.p91ID IS NULL AND getdate() BETWEEN a.p31ValidFrom AND a.p31ValidUntil"
            Case BO.myQueryP31_QuickQuery.EditingOrMovedToBin
                Return "a.p71ID IS NULL"
            Case BO.myQueryP31_QuickQuery.Invoiced
                Return "a.p91ID IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.MovedToBin
                Return "getdate() NOT BETWEEN a.p31ValidFrom AND a.p31ValidUntil"
            Case BO.myQueryP31_QuickQuery.Is_ContactPerson
                Return "a.j02ID_ContactPerson IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.Is_Corrention
                Return "a.p72ID_AfterTrimming IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.Is_Document
                Return "a.p31ID IN (SELECT p31ID FROM o23Notepad WHERE p31ID IS NOT NULL)"
            Case BO.myQueryP31_QuickQuery.Is_Task
                Return "a.p56ID IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.Is_Supplier
                Return "a.p28ID_Supplier IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.Is_Budget
                Return "a.p49ID IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.Is_p31Code
                Return "a.p31Code IS NOT NULL"
            Case BO.myQueryP31_QuickQuery.Is_GeneratedByRobot
                Return "a.p31ID IN (SELECT p31ID_NewInstance FROM p39WorkSheet_Recurrence_Plan WHERE p31ID_NewInstance IS NOT NULL)"
            Case Else
                Return ""
        End Select
    End Function

    Shared Sub RecoveryUserCache(cDB As DbHandler, sysUser As BO.j03UserSYS)
        Dim pars As New DbParameters
        pars.Add("j03id", sysUser.PID, DbType.Int32)
        pars.Add("j02id", sysUser.j02ID, DbType.Int32)
        cDB.RunSP("j03_recovery_cache", pars)
    End Sub

    Shared Function NormalizeOrderByClause(strOrderBySQL As String) As String
        If strOrderBySQL = "" Then Return ""
        Dim a() As String = Split(strOrderBySQL.Replace(" DESC", "").Replace(" ASC", ""), ",")
        Dim lis As New List(Of String)
        For Each s In a
            lis.Add(s)
        Next
        If lis.Select(Function(p) p).Count = lis.Select(Function(p) p).Distinct.Count Then
            Return strOrderBySQL
        Else
            Return String.Join(",", lis.Select(Function(p) p).Distinct)
        End If
    End Function
End Class
