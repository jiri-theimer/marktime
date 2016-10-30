Public Class o23NotepadDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.o23Notepad
        Dim s As String = GetSQLPart1(0)
        s += " WHERE a.o23ID=@o23id"

        Return _cDB.GetRecord(Of BO.o23Notepad)(s, New With {.o23id = intPID})
    End Function
    Public Function Load4Grid(intPID As Integer) As BO.o23NotepadGrid
        Dim s As String = GetSQLPart1_Grid(0) & " " & GetSQLPart2_Grid(Nothing)
        s += " WHERE a.o23ID=@o23id"

        Return _cDB.GetRecord(Of BO.o23NotepadGrid)(s, New With {.o23id = intPID})
    End Function
    Public Function LoadMyLastCreated() As BO.o23Notepad
        Dim s As String = GetSQLPart1(1)
        s += " WHERE a.j02ID_Owner=@j02id_owner ORDER BY a.o23ID DESC"

        Return _cDB.GetRecord(Of BO.o23Notepad)(s, New With {.j02id_owner = _curUser.j02ID})
    End Function

    Public Function Save(cRec As BO.o23Notepad, lisX69 As List(Of BO.x69EntityRole_Assign), lisFF As List(Of BO.FreeField)) As Boolean
        Using sc As New Transactions.TransactionScope()     'ukládání podléhá transakci
            Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
            If cRec.PID <> 0 Then
                bolINSERT = False
                strW = "o23ID=@pid"
                pars.Add("pid", cRec.PID)
            End If
            With cRec
                If .PID = 0 Then
                    pars.Add("o23IsDraft", .o23IsDraft, DbType.Boolean) 'info o draftu raději ukládat pouze při založení a poté už jenom pomocí workflow
                End If
                pars.Add("j02ID_Owner", BO.BAS.IsNullDBKey(.j02ID_Owner), DbType.Int32)
                pars.Add("o24ID", BO.BAS.IsNullDBKey(.o24ID), DbType.Int32)
                pars.Add("p41ID", BO.BAS.IsNullDBKey(.p41ID), DbType.Int32)
                pars.Add("p91ID", BO.BAS.IsNullDBKey(.p91ID), DbType.Int32)
                pars.Add("j02ID", BO.BAS.IsNullDBKey(.j02ID), DbType.Int32)
                pars.Add("p28ID", BO.BAS.IsNullDBKey(.p28ID), DbType.Int32)
                pars.Add("p56ID", BO.BAS.IsNullDBKey(.p56ID), DbType.Int32)
                pars.Add("p31ID", BO.BAS.IsNullDBKey(.p31ID), DbType.Int32)
                pars.Add("o43ID", BO.BAS.IsNullDBKey(.o43ID), DbType.Int32)

                pars.Add("o23Date", BO.BAS.IsNullDBDate(.o23Date), DbType.DateTime)
                pars.Add("o23ReminderDate", BO.BAS.IsNullDBDate(.o23ReminderDate))
                pars.Add("o23Name", .o23Name, DbType.String, , , True, "Název")
                pars.Add("o23IsEncrypted", .o23IsEncrypted, DbType.Boolean)
                pars.Add("o23Password", .o23Password, DbType.String)
                pars.Add("o23BodyPlainText", .o23BodyPlainText, DbType.String)
                pars.Add("o23SizePlainText", .o23SizePlainText, DbType.Int32)

                pars.Add("o23LockedFlag", .o23LockedFlag, DbType.Int32)
                pars.Add("o23LastLockedWhen", .o23LastLockedWhen, DbType.DateTime)
                pars.Add("o23LastLockedBy", .o23LastLockedBy, DbType.String)

                pars.Add("o23GUID", .o23GUID, DbType.String)

                pars.Add("o23validfrom", .ValidFrom, DbType.DateTime)
                pars.Add("o23validuntil", .ValidUntil, DbType.DateTime)
            End With

            If _cDB.SaveRecord("o23Notepad", pars, bolINSERT, strW, True, _curUser.j03Login) Then
                Dim intLastSavedO23ID As Integer = _cDB.LastSavedRecordPID
                If Not lisX69 Is Nothing Then   'přiřazení rolí k dokumentu
                    bas.SaveX69(_cDB, BO.x29IdEnum.o23Notepad, intLastSavedO23ID, lisX69, bolINSERT)
                End If
                If Not lisFF Is Nothing Then    'volná pole
                    bas.SaveFreeFields(_cDB, lisFF, "o23Notepad_FreeField", intLastSavedO23ID, _curUser)
                End If
                pars = New DbParameters
                With pars
                    .Add("o23id", intLastSavedO23ID, DbType.Int32)
                    .Add("j03id_sys", _curUser.PID, DbType.Int32)
                End With
                If _cDB.RunSP("o23_aftersave", pars) Then
                    sc.Complete()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Using
        
    End Function

    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("o23_delete", pars)
    End Function

    Public Function GetList_WaitingOnReminder(datReminderFrom As Date, datReminderUntil As Date) As IEnumerable(Of BO.o23Notepad)
        Dim s As String = GetSQLPart1(0), pars As New DbParameters
        pars.Add("datereminderfrom", datReminderFrom)
        pars.Add("datereminderuntil", datReminderUntil)
        s += " WHERE o23ReminderDate BETWEEN @datereminderfrom AND @datereminderuntil"
        s += " AND o23ID NOT IN (SELECT x47RecordPID FROM x47EventLog WHERE x29ID=223 AND x45ID=22306)"

        Return _cDB.GetList(Of BO.o23Notepad)(s, pars)
    End Function
    Public Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.o23NotepadGrid)

        Dim s As String = GetSQLPart1_Grid(0) & " " & GetSQLPart2_Grid(Nothing), pars As New DbParameters
        s += " WHERE a.o23ReminderDate BETWEEN @d1 AND @d2"
        s += " AND (a.j02ID_Owner=@j02id OR a.j02ID=@j02id OR a.o23ID IN (SELECT x69.x69RecordPID FROM x69EntityRole_Assign x69 INNER JOIN x67EntityRole x67 ON x69.x67ID=x67.x67ID WHERE x67.x29ID=223 AND (x69.j02ID=@j02id OR x69.j11ID IN (SELECT j11ID FROM j12Team_Person WHERE j02ID=@j02id))))"

        pars.Add("j02id", intJ02ID, DbType.Int32)
        pars.Add("d1", DateAdd(DateInterval.Day, -1, Now), DbType.DateTime)
        pars.Add("d2", DateAdd(DateInterval.Day, 2, Now), DbType.DateTime)

        Return _cDB.GetList(Of BO.o23NotepadGrid)(s, pars)
    End Function

    Private Function GetSQLWHERE(myQuery As BO.myQueryO23, ByRef pars As DL.DbParameters) As String
        Dim strW As String = bas.ParseWhereMultiPIDs("a.o23ID", myQuery)
        strW += bas.ParseWhereValidity("o23", "a", myQuery)
        With myQuery
            If Year(.DateFrom) > 1900 Or Year(.DateUntil) < 3000 Then
                pars.Add("d1", .DateFrom, DbType.DateTime) : pars.Add("d2", .DateUntil, DbType.DateTime)
                strW += " AND a.o23Date BETWEEN @d1 AND @d2"
            End If
            If Not .DateInsertFrom Is Nothing Then
                If Year(.DateInsertFrom) > 1900 Then
                    pars.Add("d1", .DateInsertFrom) : pars.Add("d2", .DateInsertUntil)
                    strW += " AND a.o23DateInsert BETWEEN @d1 AND @d2"
                End If
            End If
            
            If .o23GUID <> "" Then
                If .p31ID <> 0 Then
                    strW += " AND (a.o23GUID=@o23guid OR a.p31ID=@p31id)"
                    pars.Add("p31id", .p31ID, DbType.Int32)
                    .p31ID = 0
                Else
                    strW += " AND a.o23GUID=@o23guid"
                End If
                pars.Add("o23guid", .o23GUID, DbType.String)
            End If
            
            If .o24ID <> 0 Then
                pars.Add("o24id", .o24ID, DbType.Int32)
                strW += " AND a.o24ID=@o24id"
            End If
            If .p41ID <> 0 Then
                pars.Add("p41id", .p41ID, DbType.Int32)
                strW += " AND a.p41ID=@p41id"
            End If
            If .b02ID <> 0 Then
                pars.Add("b02id", .b02ID, DbType.Int32)
                strW += " AND a.b02ID=@b02id"
            End If
            If .p28ID <> 0 Then
                pars.Add("p28id", .p28ID, DbType.Int32)
                strW += " AND a.p28ID=@p28id"
            End If
            If .p91ID <> 0 Then
                pars.Add("p91id", .p91ID, DbType.Int32)
                strW += " AND a.p91ID=@p91id"
            End If
            If .j02ID <> 0 Then
                pars.Add("j02id", .j02ID, DbType.Int32)
                strW += " AND a.j02ID=@j02id"
            End If
            If .p31ID <> 0 Then
                pars.Add("p31id", .p31ID, DbType.Int32)
                strW += " AND a.p31ID=@p31id"
            End If
            If .p56ID <> 0 Then
                pars.Add("p56id", .p56ID, DbType.Int32)
                strW += " AND a.p56ID=@p56id"
            End If
            If .SpecificQuery > BO.myQueryO23_SpecificQuery._NotSpecified Then
                If .j02ID_ExplicitQueryFor > 0 Then
                    pars.Add("j02id_query", .j02ID_ExplicitQueryFor, DbType.Int32)
                Else
                    pars.Add("j02id_query", _curUser.j02ID, DbType.Int32)
                End If
            End If
            If .j70ID > 0 Then
                Dim strQueryW As String = bas.CompleteSqlJ70(_cDB, .j70ID, _curUser)
                If strQueryW <> "" Then
                    strW += " AND " & strQueryW
                End If
            End If
            If .x25ID > 0 Then strW += " AND a.o23ID IN (SELECT x19RecordPID FROM x19EntityCategory_Binding WHERE x29ID=223 AND x25ID=" & .x25ID.ToString & ")"
            Select Case .SpecificQuery
                Case BO.myQueryO23_SpecificQuery.AllowedForRead
                    If Not _curUser.IsAdmin Then    'admin má automaticky nárok na všechny dokumenty
                        strW += " AND (a.j02ID_Owner=@j02id_query"
                        strW += " OR a.o23ID IN (SELECT x69.x69RecordPID FROM x69EntityRole_Assign x69 INNER JOIN x67EntityRole x67 ON x69.x67ID=x67.x67ID WHERE x67.x29ID=223 AND (x69.j02ID=@j02id_query OR x69.j11ID IN (SELECT j11ID FROM j12Team_Person WHERE j02ID=@j02id_query)))"   'obdržel nějakou (jakoukoliv) roli v dokumentu
                        strW += ")"
                    End If
                Case Else
            End Select
            If .QuickQuery > BO.myQueryO23_QuickQuery._NotSpecified Then
                strW += " AND " & bas.GetQuickQuerySQL_o23(.QuickQuery)
            End If
            If .ColumnFilteringExpression <> "" Then
                strW += " AND " & .ColumnFilteringExpression
            End If
            If .SearchExpression <> "" Then
                strW += " AND ("
                'něco jako fulltext
                strW += "a.o23Name LIKE '%'+@expr+'%' OR a.o23Code LIKE '%'+@expr+'%' OR p41.p41Name LIKE '%'+@expr+'%' OR p41.p41NameShort LIKE '%'+@expr+'%' OR p28_client.p28Name LIKE '%'+@expr+'%' OR p28_client.p28CompanyShortName LIKE '%'+@expr+'%'"
                strW += ")"
                pars.Add("expr", .SearchExpression, DbType.String)
            End If
        End With
        strW += bas.ParseWhereValidity("o23", "a", myQuery)
        Return bas.TrimWHERE(strW)
    End Function
    Public Function GetVirtualCount(myQuery As BO.myQueryO23) As Integer
        Dim s As String = "SELECT COUNT(a.o23ID) as Value " & GetSQLPart2_Grid(Nothing)
        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        If strW <> "" Then s += " WHERE " & strW

        Return _cDB.GetRecord(Of BO.GetInteger)(s, pars).Value
    End Function

    Public Function GetList(myQuery As BO.myQueryO23) As IEnumerable(Of BO.o23Notepad)
        Dim s As String = GetSQLPart1(0), pars As New DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)

        s += " ORDER BY a.o23ID DESC"

        Return _cDB.GetList(Of BO.o23Notepad)(s, pars)

    End Function

    Public Function GetGridDataSource(myQuery As BO.myQueryO23) As DataTable
        Dim s As String = ""
        With myQuery
            If Not String.IsNullOrEmpty(.MG_GridGroupByField) Then
                If .MG_GridSqlColumns.ToLower.IndexOf(.MG_GridGroupByField.ToLower) < 0 Then
                    Select Case .MG_GridGroupByField
                        Case "ProjectClient" : .MG_GridSqlColumns += ",p28_client.p28Name as ProjectClient"
                        Case "Project" : .MG_GridSqlColumns += ",p41.p41Name as Project"
                        Case "Owner" : .MG_GridSqlColumns += ",j02owner.j02LastName+char(32)+j02owner.j02FirstName as Owner"
                        Case Else
                            .MG_GridSqlColumns += "," & .MG_GridGroupByField
                    End Select
                End If
            End If
            .MG_GridSqlColumns += ",a.o23ID as pid,CONVERT(BIT,CASE WHEN GETDATE() BETWEEN a.o23ValidFrom AND a.o23ValidUntil THEN 0 else 1 END) as IsClosed,a.o23IsDraft as IsDraft"
            .MG_GridSqlColumns += ",a.o23IsEncrypted as o23IsEncrypted_Grid,a.o23LockedFlag as o23LockedFlag_Grid,a.b02ID,b02.b02Color"
        End With

        
        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        With myQuery
            If .MG_SelectPidFieldOnly Then .MG_GridSqlColumns = "a.o23ID as pid"
            Dim strORDERBY As String = .MG_SortString
            If .MG_GridGroupByField <> "" Then
                Dim strPrimarySortField As String = .MG_GridGroupByField
                If strPrimarySortField = "ProjectClient" Then strPrimarySortField = "p28_client.p28Name"
                If strPrimarySortField = "Owner" Then strPrimarySortField = "j02owner.j02LastName+char(32)+j02owner.j02FirstName"
                If strPrimarySortField = "Project" Then strPrimarySortField = "p41.p41Name"
                If strPrimarySortField = "DocCompany" Then strPrimarySortField = "p28.p28Name"
                If strORDERBY = "" Or LCase(strPrimarySortField) = Replace(Replace(LCase(.MG_SortString), " desc", ""), " asc", "") Then
                    strORDERBY = strPrimarySortField
                Else
                    strORDERBY = strPrimarySortField & "," & .MG_SortString
                End If
            End If
            If strORDERBY = "" Then strORDERBY = "a.o23ID DESC"
            If .MG_PageSize > 0 Then
                Dim intStart As Integer = (.MG_CurrentPageIndex) * .MG_PageSize

                s = "WITH rst AS (SELECT ROW_NUMBER() OVER (ORDER BY " & strORDERBY & ")-1 as RowIndex," & .MG_GridSqlColumns & " " & GetSQLPart2_Grid(myQuery)

                If strW <> "" Then s += " WHERE " & strW
                s += ") SELECT * FROM rst"
                pars.Add("start", intStart, DbType.Int32)
                pars.Add("end", (intStart + .MG_PageSize - 1), DbType.Int32)
                s += " WHERE RowIndex BETWEEN @start AND @end"
            Else
                'bez stránkování
                s = "SELECT " & .MG_GridSqlColumns & " " & GetSQLPart2_Grid(myQuery)
                If strW <> "" Then s += " WHERE " & strW
                s += " ORDER BY " & strORDERBY
            End If

        End With

        Dim ds As DataSet = _cDB.GetDataSet(s, , pars.Convert2PluginDbParameters())
        If Not ds Is Nothing Then Return ds.Tables(0) Else Return Nothing
    End Function

    Public Function GetList4Grid(myQuery As BO.myQueryO23) As IEnumerable(Of BO.o23NotepadGrid)
        Dim s As String = GetSQLPart1_Grid(myQuery.TopRecordsOnly), pars As New DbParameters
        If myQuery.MG_SelectPidFieldOnly Then
            'SQL SELECT klauzule bude plnit pouze hodnotu primárního klíče
            s = "SELECT a.o23ID as _pid"
        End If
        Dim strW As String = GetSQLWHERE(myQuery, pars), bolInhaleReceiversInLine As Boolean = False

        With myQuery
            Dim strSort As String = .MG_SortString
            If strSort = "" Then strSort = "a.o23ID DESC"

            If .MG_PageSize > 0 Then
                'použít stránkování do gridu = zcela jiný SQL dotaz od začátku
                s = GetSQL_OFFSET(strW, ParseSortExpression(strSort), .MG_PageSize, .MG_CurrentPageIndex, pars, bolInhaleReceiversInLine)
            Else
                'normální select - navazuje se na úvodní skladbu
                s += " " & GetSQLPart2_Grid(myQuery)
                If strW <> "" Then s += " WHERE " & strW
                If strSort <> "" Then
                    s += " ORDER BY " & ParseSortExpression(strSort)
                End If
            End If

        End With



        Return _cDB.GetList(Of BO.o23NotepadGrid)(s, pars)

    End Function

    Private Function ParseSortExpression(strSort As String) As String
        strSort = strSort.Replace("UserInsert", "o23UserInsert").Replace("UserUpdate", "o23UserUpdate").Replace("DateInsert", "o23DateInsert").Replace("DateUpdate", "o23DateUpdate").Replace("p28Name", "p28.p28Name").Replace("p28CompanyName", "p28.p28CompanyName")
        strSort = strSort.Replace("Owner", "j02owner.j02LastName").Replace("ProjectClient", "p28_client.p28Name").Replace("ReceiversInLine", "dbo.o23_getroles_inline(a.o23ID)").Replace("Project", "p41.p41Name")
        Return bas.NormalizeOrderByClause(strSort)
    End Function
    Private Function ParseFilterExpression(strFilter As String) As String
        Return ParseSortExpression(strFilter).Replace("[", "").Replace("]", "")
    End Function
    Private Function GetSQL_OFFSET(strWHERE As String, strORDERBY As String, intPageSize As Integer, intCurrentPageIndex As Integer, ByRef pars As DL.DbParameters, bolInhaleReceiversInLine As Boolean) As String
        Dim intStart As Integer = (intCurrentPageIndex) * intPageSize

        Dim s As String = "WITH rst AS (SELECT ROW_NUMBER() OVER (ORDER BY " & strORDERBY & ")-1 as RowIndex," & GetSF_Grid()
        If bolInhaleReceiversInLine Then
            s += ",dbo.o23_getroles_inline(a.o23ID) as _ReceiversInLine"
        End If
        s += " " & GetSQLPart2_Grid(Nothing)

        If strWHERE <> "" Then s += " WHERE " & strWHERE
        s += ") SELECT TOP " & intPageSize.ToString & " * FROM rst"
        pars.Add("start", intStart, DbType.Int32)
        pars.Add("end", (intStart + intPageSize - 1), DbType.Int32)
        s += " WHERE RowIndex BETWEEN @start AND @end"

        Return s
    End Function

    Private Function GetSQLPart1(intTOP As Integer) As String
        Dim s As String = "SELECT"
        If intTOP > 0 Then s += " TOP " & intTOP.ToString
        s += " a.o24ID,a.p28ID,a.p41ID,a.p91ID,a.j02ID,a.p56ID,a.p31ID,a.b02ID,a.o43ID as _o43ID,a.o23Name,a.o23Code,o24.o24Name as _o24Name,a.j02ID_Owner,a.o23Date,a.o23ReminderDate,a.o23IsEncrypted,o24.x29ID as _x29ID"
        s += ",o23free.*," & bas.RecTail("o23", "a") & ",j02owner.j02LastName+' '+j02owner.j02FirstName as _Owner,b02.b02Name as _b02Name,b02.b02Color as _b02Color,o24.o24IsBillingMemo as _o24IsBillingMemo"
        s += ",a.o23BodyPlainText,a.o23SizePlainText,a.o23Password,a.o23IsDraft,a.o23LockedFlag as _o23LockedFlag,a.o23LastLockedWhen as _o23LastLockedWhen,a.o23LastLockedBy as _o23LastLockedBy,a.o23GUID"
        s += " FROM o23Notepad a LEFT OUTER JOIN o24NotepadType o24 ON a.o24ID=o24.o24ID LEFT OUTER JOIN (SELECT o23ID,COUNT(*) as FilesCount FROM o27Attachment WHERE o23ID IS NOT NULL GROUP BY o23ID) o27 ON a.o23ID=o27.o23ID"
        s += " LEFT OUTER JOIN j02Person j02owner ON a.j02ID_Owner=j02owner.j02ID"
        s += " LEFT OUTER JOIN b02WorkflowStatus b02 ON a.b02ID=b02.b02ID"
        s += " LEFT OUTER JOIN o23Notepad_FreeField o23free ON a.o23ID=o23free.o23ID"
        Return s
    End Function

    Private Function GetSQLPart2_Grid(mq As BO.myQueryO23) As String
        Dim s As String = "FROM o23Notepad a INNER JOIN o24NotepadType o24 ON a.o24ID=o24.o24ID LEFT OUTER JOIN p41Project p41 ON a.p41ID=p41.p41ID LEFT OUTER JOIN p28Contact p28_client ON p41.p28ID_Client=p28_client.p28ID"
        s += " LEFT OUTER JOIN b02WorkflowStatus b02 ON a.b02ID=b02.b02ID LEFT OUTER JOIN p28Contact p28 ON a.p28ID=p28.p28ID LEFT OUTER JOIN j02Person j02 ON a.j02ID=j02.j02ID"
        s += " LEFT OUTER JOIN j02Person j02owner ON a.j02ID_Owner=j02owner.j02ID"
        s += " LEFT OUTER JOIN p56Task p56 ON a.p56ID=p56.p56ID"
        s += " LEFT OUTER JOIN o23Notepad_FreeField o23free ON a.o23ID=o23free.o23ID"
        If Not mq Is Nothing Then
            If mq.MG_AdditionalSqlFROM <> "" Then s += " " & mq.MG_AdditionalSqlFROM
        End If
        Return s
    End Function

    Private Function GetSQLPart1_Grid(intTOP As Integer) As String
        Dim s As String = "SELECT"
        If intTOP > 0 Then s += " TOP " & intTOP.ToString
        s += " " & GetSF_Grid()
        Return s
    End Function
    Private Function GetSF_Grid() As String
        Dim s As String = "a.o24ID,a.p28ID,a.p41ID,a.p91ID,a.j02ID,a.p31ID,a.p56ID,a.b02ID,a.o43ID as _o43ID,a.o23Name,a.o23Code,a.j02ID_Owner,a.o23Date,a.o23ReminderDate,a.o23IsEncrypted,j02owner.j02LastName+' '+j02owner.j02FirstName as _Owner,o24.o24IsBillingMemo as _o24IsBillingMemo"
        s += "," & bas.RecTail("o23", "a") & ",o24.o24Name as _o24Name,b02.b02Name as _b02Name,b02.b02Color as _b02Color,p28_client.p28Name as ProjectClient,p41.p41Name,p41.p41Code,p28.p28Name,p28.p28CompanyName,o23free.*,o24.x29ID as _x29ID"
        s += ",j02.j02LastName+' '+j02.j02FirstName as Person,a.o23BodyPlainText,a.o23SizePlainText,a.o23IsDraft,a.o23LockedFlag as _o23LockedFlag,a.o23LastLockedWhen as _o23LastLockedWhen,a.o23LastLockedBy as _o23LastLockedBy,a.o23GUID"
        Return s
    End Function

    Public Sub UpdateSelectedNotepadRole(intX67ID As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), intO23ID As Integer)
        bas.SaveX69(_cDB, BO.x29IdEnum.o23Notepad, intO23ID, lisX69, False, intX67ID)
    End Sub
    Public Sub ClearSelectedNotepadRole(intX67ID As Integer, intO23ID As Integer)
        _cDB.RunSQL("DELETE FROM x69EntityRole_Assign WHERE x67ID=" & intX67ID.ToString & " AND x69RecordPID=" & intO23ID.ToString)
    End Sub
    Public Function ConvertFromDraft(intPID As Integer) As Boolean
        Dim pars As New DbParameters
        With pars
            .Add("o23id", intPID, DbType.Int32)
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            pars.Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("o23_convertdraft", pars)

    End Function

    Public Function LockFiles(intPID As Integer, lockFlag As BO.o23LockedTypeENUM) As Boolean
        Dim pars As New DbParameters()
        pars.Add("pid", intPID)
        pars.Add("o23LockedFlag", lockFlag, DbType.Int32)
        pars.Add("o23LastLockedWhen", Now, DbType.DateTime)
        pars.Add("o23LastLockedBy", _curUser.j03Login, DbType.String)

        Return _cDB.SaveRecord("o23Notepad", pars, False, "o23ID=@pid", True, _curUser.j03Login)
    End Function
    Public Function UnLockFiles(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        pars.Add("pid", intPID)
        pars.Add("o23LockedFlag", Nothing, DbType.Int32)
        pars.Add("o23LastLockedWhen", Nothing, DbType.DateTime)
        pars.Add("o23LastLockedBy", Nothing, DbType.String)

        Return _cDB.SaveRecord("o23Notepad", pars, False, "o23ID=@pid", True, _curUser.j03Login)
    End Function
    Public Function UpdateImapSource(intPID As Integer, intO43ID As Integer) As Boolean
        Dim pars As New DbParameters()
        pars.Add("pid", intPID)
        pars.Add("o43ID", BO.BAS.IsNullDBKey(intO43ID), DbType.Int32)
        Return _cDB.SaveRecord("o23Notepad", pars, False, "o23ID=@pid", False)
    End Function
End Class
