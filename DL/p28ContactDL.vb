Public Class p28ContactDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Overloads Function Load(intPID As Integer) As BO.p28Contact
        Dim s As String = GetSQLPart1(0) & " " & GetSQLPart2()
        s += " WHERE a.p28ID=@p28id"

        Return _cDB.GetRecord(Of BO.p28Contact)(s, New With {.p28id = intPID})
    End Function
    Public Function LoadMyLastCreated() As BO.p28Contact
        Dim s As String = GetSQLPart1(1) & " " & GetSQLPart2()
        s += " WHERE a.p28UserInsert=@mylogin ORDER BY a.p28ID DESC"

        Return _cDB.GetRecord(Of BO.p28Contact)(s, New With {.mylogin = _curUser.j03Login})
    End Function
    Public Function LoadByRegID(strRegID As String, Optional intP28ID_Exclude As Integer = 0) As BO.p28Contact
        Dim s As String = GetSQLPart1(0) & " " & GetSQLPart2(), pars As New DbParameters
        pars.Add("regid", strRegID, DbType.String)
        s += " WHERE a.p28RegID LIKE @regid"
        If intP28ID_Exclude <> 0 Then
            s += " AND a.p28ID<>@p28id_exclude"
            pars.Add("p28id_exclude", intP28ID_Exclude, DbType.Int32)
        End If

        Return _cDB.GetRecord(Of BO.p28Contact)(s, pars)
    End Function
    Public Function LoadByVatID(strVatID As String, Optional intP28ID_Exclude As Integer = 0) As BO.p28Contact
        Dim s As String = GetSQLPart1(0) & " " & GetSQLPart2(), pars As New DbParameters
        pars.Add("vatid", strVatID, DbType.String)
        s += " WHERE a.p28VatID LIKE @vatid"
        If intP28ID_Exclude <> 0 Then
            s += " AND a.p28ID<>@p28id_exclude"
            pars.Add("p28id_exclude", intP28ID_Exclude, DbType.Int32)
        End If
        Return _cDB.GetRecord(Of BO.p28Contact)(s, pars)
    End Function
    Public Function LoadByPersonBirthRegID(strBirthRegID As String, Optional intP28ID_Exclude As Integer = 0) As BO.p28Contact
        Dim s As String = GetSQLPart1(0) & " " & GetSQLPart2(), pars As New DbParameters
        pars.Add("p28Person_BirthRegID", strBirthRegID, DbType.String)
        s += " WHERE a.p28Person_BirthRegID LIKE @p28Person_BirthRegID"
        If intP28ID_Exclude <> 0 Then
            s += " AND a.p28ID<>@p28id_exclude"
            pars.Add("p28id_exclude", intP28ID_Exclude, DbType.Int32)
        End If
        Return _cDB.GetRecord(Of BO.p28Contact)(s, pars)
    End Function
    Public Function LoadByImapRobotAddress(strRobotAddress As String) As BO.p28Contact
        Dim s As String = GetSQLPart1(1) & " " & GetSQLPart2()
        s += " WHERE a.p28RobotAddress LIKE @robotkey"

        Return _cDB.GetRecord(Of BO.p28Contact)(s, New With {.robotkey = strRobotAddress})
    End Function

    Public Function Save(cRec As BO.p28Contact, lisO37 As List(Of BO.o37Contact_Address), lisO32 As List(Of BO.o32Contact_Medium), lisP30 As List(Of BO.p30Contact_Person), lisX69 As List(Of BO.x69EntityRole_Assign), lisFF As List(Of BO.FreeField), p58IDs As List(Of Integer), ByRef intLastSavedP28ID As Integer) As Boolean
        intLastSavedP28ID = 0
        Using sc As New Transactions.TransactionScope()     'ukládání podléhá transakci
            Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
            If cRec.PID <> 0 Then
                bolINSERT = False
                strW = "p28ID=@pid"
                pars.Add("pid", cRec.PID)
            End If
            With cRec
                If .p28Code = "" Then .p28Code = "TEMP" & BO.BAS.GetGUID() 'dočasný kód, bude později nahrazen
                If .PID = 0 Then
                    pars.Add("p28IsDraft", .p28IsDraft, DbType.Boolean) 'info o draftu raději ukládat pouze při založení a poté už jenom pomocí workflow
                End If
                pars.Add("j02ID_Owner", BO.BAS.IsNullDBKey(.j02ID_Owner), DbType.Int32)
                pars.Add("p29ID", BO.BAS.IsNullDBKey(.p29ID), DbType.Int32)
                pars.Add("p92ID", BO.BAS.IsNullDBKey(.p92ID), DbType.Int32)
                pars.Add("p87ID", BO.BAS.IsNullDBKey(.p87ID), DbType.Int32)
                pars.Add("p51ID_Billing", BO.BAS.IsNullDBKey(.p51ID_Billing), DbType.Int32)
                pars.Add("p51ID_Internal", BO.BAS.IsNullDBKey(.p51ID_Internal), DbType.Int32)

                pars.Add("p28Code", .p28Code, DbType.String)
                pars.Add("p28IsCompany", .p28IsCompany, DbType.Boolean)
                pars.Add("p28FirstName", .p28FirstName, DbType.String, , , True, "Křestní jméno")
                pars.Add("p28LastName", .p28LastName, DbType.String, , , True, "Příjmení")
                pars.Add("p28TitleBeforeName", .p28TitleBeforeName, DbType.String, , , True, "Titul před jménem")
                pars.Add("p28TitleAfterName", .p28TitleAfterName, DbType.String, , , True, "Za jménem")
                pars.Add("p28RegID", .p28RegID, DbType.String, , , True, "IČ")
                pars.Add("p28VatID", .p28VatID, DbType.String, , , True, "DIČ")
                pars.Add("p28CompanyName", .p28CompanyName, DbType.String, , , True, "Společnost")
                pars.Add("p28CompanyShortName", .p28CompanyShortName, DbType.String, , , True, "Zkrácený název společnosti")
                pars.Add("p28RobotAddress", .p28RobotAddress, DbType.String)

                pars.Add("p28InvoiceDefaultText1", .p28InvoiceDefaultText1, DbType.String, , , True, "Výchozí fakturační text")
                pars.Add("p28InvoiceDefaultText2", .p28InvoiceDefaultText2, DbType.String, , , True, "Výchozí doplňkový text faktury")
                pars.Add("p28InvoiceMaturityDays", .p28InvoiceMaturityDays, DbType.Int32)
                pars.Add("p28AvatarImage", .p28AvatarImage, DbType.String)
                pars.Add("p28LimitHours_Notification", .p28LimitHours_Notification, DbType.Decimal)
                pars.Add("p28LimitFee_Notification", .p28LimitFee_Notification, DbType.Decimal)
                pars.Add("p28validfrom", .ValidFrom, DbType.DateTime)
                pars.Add("p28validuntil", .ValidUntil, DbType.DateTime)

            End With

            If _cDB.SaveRecord("p28Contact", pars, bolINSERT, strW, True, _curUser.j03Login) Then
                intLastSavedP28ID = _cDB.LastSavedRecordPID

                If Not lisO37 Is Nothing Then   'kontaktní adresy
                    If Not bolINSERT Then _cDB.RunSQL("DELETE FROM o37Contact_Address WHERE p28ID=" & intLastSavedP28ID.ToString)
                    Dim cDLO38 As New DL.o38AddressDL(_curUser)
                    For Each c In lisO37
                        c.SetPID(c.o38ID)
                        If c.IsSetAsDeleted Then
                            If c.PID <> 0 Then cDLO38.Delete(c.PID)
                        Else
                            If cDLO38.Save(c) Then
                                If Not _cDB.RunSQL("INSERT INTO o37Contact_Address(p28ID,o38ID,o36ID) VALUES(" & intLastSavedP28ID.ToString & "," & cDLO38.LastSavedRecordPID.ToString & "," & CInt(c.o36ID).ToString & ")") Then
                                    Return False
                                End If
                            Else
                                _Error = cDLO38.ErrorMessage : Return False
                            End If
                        End If
                    Next
                End If
                If Not lisO32 Is Nothing Then   'kontaktní média
                    Dim cDLO32 As New DL.o32Contact_MediumDL(_curUser)
                    For Each c In lisO32
                        c.p28ID = intLastSavedP28ID
                        If c.IsSetAsDeleted Then
                            If c.PID <> 0 Then cDLO32.Delete(c.PID)
                        Else
                            cDLO32.Save(c)
                        End If
                    Next
                End If
                If Not lisP30 Is Nothing Then   'kontaktní osoby
                    If Not bolINSERT Then _cDB.RunSQL("DELETE FROM p30Contact_Person WHERE p28ID=" & intLastSavedP28ID.ToString)
                    For Each c In lisP30.Where(Function(p) p.IsSetAsDeleted = False)
                        _cDB.RunSQL("INSERT INTO p30Contact_Person(p28ID,j02ID,p27ID) VALUES(" & intLastSavedP28ID.ToString & "," & c.j02ID.ToString & "," & IIf(c.p27ID <> 0, c.p27ID.ToString, "NULL") & ")")

                    Next
                End If
                If Not p58IDs Is Nothing Then   'produkty klienta
                    If Not bolINSERT Then _cDB.RunSQL("DELETE FROM p26Contact_Product WHERE p28ID=" & intLastSavedP28ID.ToString)
                    For Each intP58ID As Integer In p58IDs
                        _cDB.RunSQL("INSERT INTO p26Contact_Product(p28ID,p58ID) VALUES(" & intLastSavedP28ID.ToString & "," & intP58ID.ToString & ")")
                    Next
                End If
                If Not lisX69 Is Nothing Then   'přiřazení rolí klienta
                    bas.SaveX69(_cDB, BO.x29IdEnum.p28Contact, intLastSavedP28ID, lisX69, bolINSERT)
                End If
                If Not lisFF Is Nothing Then    'volná pole
                    bas.SaveFreeFields(_cDB, lisFF, "p28Contact_FreeField", intLastSavedP28ID)
                End If

                pars = New DbParameters
                With pars
                    .Add("p28id", intLastSavedP28ID, DbType.Int32)
                    .Add("j03id_sys", _curUser.PID, DbType.Int32)
                End With
                If _cDB.RunSP("p28_aftersave", pars) Then
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
        Return _cDB.RunSP("p28_delete", pars)
    End Function

    Public Function GetList_o37(intPID As Integer) As IEnumerable(Of BO.o37Contact_Address)
        Dim s As String = "select a.p28ID,a.o36ID,o38.*,o36.o36Name as _o36Name," & bas.RecTail("o38", "o38")
        s += " FROM o37Contact_Address a INNER JOIN o38Address o38 ON a.o38ID=o38.o38ID INNER JOIN o36AddressType o36 ON a.o36ID=o36.o36ID"
        s += " WHERE a.p28ID=@pid"

        Return _cDB.GetList(Of BO.o37Contact_Address)(s, New With {.pid = intPID})
    End Function
    Public Function GetList_o32(intPID As Integer) As IEnumerable(Of BO.o32Contact_Medium)
        Dim s As String = "select a.*,o33.o33Name as _o33Name," & bas.RecTail("o32", "a", False, True)
        s += " FROM o32Contact_Medium a INNER JOIN o33MediumType o33 ON a.o33ID=o33.o33ID"
        s += " WHERE a.p28ID=@pid"

        Return _cDB.GetList(Of BO.o32Contact_Medium)(s, New With {.pid = intPID})
    End Function
    

    Public Function GetList_x90(intPID As Integer, datFrom As Date, datUntil As Date) As IEnumerable(Of BO.x90EntityLog)
        Return bas.GetList_x90(_cDB, BO.x29IdEnum.p28Contact, intPID, datFrom, datUntil)
    End Function

    Private Function GetSQLWHERE(myQuery As BO.myQueryP28, ByRef pars As DL.DbParameters) As String
        Dim s As New System.Text.StringBuilder
        s.Append(bas.ParseWhereMultiPIDs("a.p28ID", myQuery))
        s.Append(bas.ParseWhereValidity("p28", "a", myQuery))
        With myQuery
            If .p29ID <> 0 Then
                pars.Add("p29id", .p29ID, DbType.Int32)
                s.Append(" AND a.p29ID=@p29id")
            End If
            If .j02ID <> 0 Then
                pars.Add("j02id", .j02ID, DbType.Int32)
                s.Append(" AND a.p28ID IN (select p28ID FROM p30Contact_Person WHERE j02ID=@j02id)")
            End If
            If .b02ID <> 0 Then
                pars.Add("b02id", .b02ID, DbType.Int32)
                s.Append(" AND a.b02ID=@b02id")
            End If
            If Not .DateInsertFrom Is Nothing Then
                pars.Add("d1", .DateInsertFrom)
                pars.Add("d2", .DateInsertUntil)
                s.Append(" AND a.p28DateInsert BETWEEN @d1 AND @d2")
            End If
            If .QuickQuery > BO.myQueryP28_QuickQuery._NotSpecified Then
                s.Append(" AND " & bas.GetQuickQuerySQL_p28(.QuickQuery))
            End If
            
            
            Select Case .SpecificQuery
                Case BO.myQueryP28_SpecificQuery.AllowedForRead
                    If BO.BAS.TestPermission(_curUser, BO.x53PermValEnum.GR_P28_Owner) Or BO.BAS.TestPermission(_curUser, BO.x53PermValEnum.GR_P28_Reader) Then
                        'právo paušálně číst všechny kontakty - není třeba skládat podmínku
                    Else
                        Dim strJ11IDs As String = ""
                        If _curUser.j11IDs <> "" Then strJ11IDs = "OR x69.j11ID IN (" & _curUser.j11IDs & ")"

                        s.Append(" AND (a.j02ID_Owner=@j02id_query OR a.p28ID IN (")
                        s.Append("SELECT x69.x69RecordPID FROM x69EntityRole_Assign x69 INNER JOIN x67EntityRole x67 ON x69.x67ID=x67.x67ID")
                        s.Append(" WHERE x67.x29ID=328 AND (x69.j02ID=@j02id_query " & strJ11IDs & ")")
                        s.Append("))")
                    End If
            End Select
            If .j70ID > 0 Then
                Dim strQueryW As String = bas.CompleteSqlJ70(_cDB, .j70ID)
                If strQueryW <> "" Then
                    s.Append(" AND " & strQueryW)
                End If
            End If
            If .SpecificQuery > BO.myQueryP28_QuickQuery._NotSpecified Then
                If .j02ID_ExplicitQueryFor > 0 Then
                    pars.Add("j02id_query", .j02ID_ExplicitQueryFor, DbType.Int32)
                Else
                    pars.Add("j02id_query", _curUser.j02ID, DbType.Int32)
                End If
            End If
            If .ColumnFilteringExpression <> "" Then
                s.Append(" AND " & ParseFilterExpression(.ColumnFilteringExpression))
            End If
            If .SearchExpression <> "" Then
                s.Append(" AND (")
                If Len(.SearchExpression) <= 1 Then
                    'hledat pouze podle počátečních písmen
                    s.Append("a.p28Name LIKE @expr+'%' OR a.p28Code LIKE '%'+@expr+'%' OR a.p28CompanyShortName LIKE @expr+'%' OR a.p28CompanyName LIKE @expr+'%'")
                    s.Append(" OR a.p28ID IN (select p30.p28ID FROM p30Contact_Person p30 INNER JOIN j02Person j02 ON p30.j02ID=j02.j02ID WHERE j02LastName LIKE @expr+'%')")
                Else
                    'něco jako fulltext
                    s.Append("a.p28Name LIKE '%'+@expr+'%' OR a.p28CompanyShortName LIKE '%'+@expr+'%' OR a.p28CompanyName LIKE '%'+@expr+'%'")
                    If Len(.SearchExpression) >= 2 Then
                        s.Append(" OR a.p28Code LIKE '%'+@expr+'%' OR a.p28RegID LIKE @expr+'%' OR a.p28VatID LIKE @expr+'%'")
                    End If
                    s.Append(" OR a.p28ID IN (select p30.p28ID FROM p30Contact_Person p30 INNER JOIN j02Person j02 ON p30.j02ID=j02.j02ID WHERE j02LastName LIKE '%'+@expr+'%')")
                End If
                s.Append(")")
                pars.Add("expr", .SearchExpression, DbType.String)
            End If
        End With
        Return bas.TrimWHERE(s.ToString)
    End Function

    Public Function GetList(myQuery As BO.myQueryP28) As IEnumerable(Of BO.p28Contact)
        Dim s As String = GetSQLPart1(myQuery.TopRecordsOnly) & " " & GetSQLPart2()
        If myQuery.MG_SelectPidFieldOnly Then
            'SQL SELECT klauzule bude plnit pouze hodnotu primárního klíče
            s = "SELECT a.p28ID as _pid " & GetSQLPart2()
        End If
        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        With myQuery
            Dim strSort As String = .MG_SortString
            If strSort = "" Then strSort = "p28Name"

            If .MG_PageSize > 0 Then
                'použít stránkování do gridu
                s = GetSQL_OFFSET(strW, ParseSortExpression(strSort), .MG_PageSize, .MG_CurrentPageIndex, pars)
            Else
                'normální select
                If strW <> "" Then s += " WHERE " & strW
                If strSort <> "" Then
                    s += " ORDER BY " & ParseSortExpression(strSort)
                End If
            End If
        End With
        Return _cDB.GetList(Of BO.p28Contact)(s, pars)
    End Function
   

    Private Function ParseSortExpression(strSort As String) As String
        strSort = strSort.Replace("UserInsert", "p28UserInsert").Replace("UserUpdate", "p28UserUpdate").Replace("DateInsert", "p28DateInsert").Replace("DateUpdate", "p28DateUpdate")
        strSort = strSort.Replace("Owner", "j02owner.j02LastName").Replace("p51Name_Billing", "p51billing.p51Name").Replace("p51Name_Internal", "p51internal.p51Name")
        Return bas.NormalizeOrderByClause(strSort)
    End Function
    Private Function ParseFilterExpression(strFilter As String) As String
        Return ParseSortExpression(strFilter).Replace("[", "").Replace("]", "")
    End Function
    Private Function GetSQL_OFFSET(strWHERE As String, strORDERBY As String, intPageSize As Integer, intCurrentPageIndex As Integer, ByRef pars As DL.DbParameters) As String
        Dim intStart As Integer = (intCurrentPageIndex) * intPageSize
        
        Dim s As String = "WITH rst AS (SELECT ROW_NUMBER() OVER (ORDER BY " & strORDERBY & ")-1 as RowIndex," & GetSF() & " " & GetSQLPart2()

        If strWHERE <> "" Then s += " WHERE " & strWHERE
        s += ") SELECT TOP " & intPageSize.ToString & " * FROM rst"
        pars.Add("start", intStart, DbType.Int32)
        pars.Add("end", (intStart + intPageSize - 1), DbType.Int32)
        s += " WHERE RowIndex BETWEEN @start AND @end"
        's += " ORDER BY " & strORDERBY
        Return s
    End Function

    Public Function GetVirtualCount(myQuery As BO.myQueryP28) As Integer
        Dim s As String = "SELECT count(a.p28ID) as Value " & GetSQLPart2()
        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        If strW <> "" Then s += " WHERE " & strW

        Return _cDB.GetRecord(Of BO.GetInteger)(s, pars).Value
    End Function

    Private Function GetSQLPart1(intTOP As Integer) As String
        Dim s As String = "SELECT"
        If intTOP > 0 Then s += " TOP " & intTOP.ToString
        s += " " & GetSF()
        Return s
    End Function
    Private Function GetSF() As String
        Dim s As String = "a.p29ID,a.p92ID,a.j02ID_Owner,a.p87ID,a.p51ID_Billing,a.p51ID_Internal,a.b02ID,a.p28IsCompany,a.p28IsDraft,a.p28Code,a.p28FirstName,a.p28LastName,a.p28TitleBeforeName,a.p28TitleAfterName,a.p28RegID,a.p28VatID,a.p28Person_BirthRegID,a.p28CompanyName,a.p28CompanyShortName,a.p28InvoiceDefaultText1,a.p28InvoiceDefaultText2,a.p28InvoiceMaturityDays,a.p28LimitHours_Notification,a.p28LimitFee_Notification,a.p28AvatarImage"
        s += ",a.p28Name as _p28name,p29.p29Name as _p29Name,p92.p92Name as _p92Name,b02.b02Name as _b02Name,p87.p87Name as _p87Name,a.p28RobotAddress"
        s += ",p51billing.p51Name as _p51Name_Billing,p51internal.p51Name as _p51Name_Internal,j02owner.j02LastName+' '+j02owner.j02FirstName as _Owner," & bas.RecTail("p28", "a") & ",p28free.*"
        Return s
    End Function
    Private Function GetSQLPart2() As String
        Dim s As String = "FROM p28Contact a LEFT OUTER JOIN p29ContactType p29 ON a.p29ID=p29.p29ID"
        s += " LEFT OUTER JOIN p87BillingLanguage p87 ON a.p87ID=p87.p87ID"
        s += " LEFT OUTER JOIN p92InvoiceType p92 ON a.p92ID=p92.p92ID"
        s += " LEFT OUTER JOIN b02WorkflowStatus b02 ON a.b02ID=b02.b02ID"
        s += " LEFT OUTER JOIN p51PriceList p51billing ON a.p51ID_Billing=p51billing.p51ID"
        s += " LEFT OUTER JOIN p51PriceList p51internal ON a.p51ID_Internal=p51internal.p51ID"
        s += " LEFT OUTER JOIN j02Person j02owner ON a.j02ID_Owner=j02owner.j02ID"
        s += " LEFT OUTER JOIN p28Contact_FreeField p28free ON a.p28ID=p28free.p28ID"
        Return s
    End Function

    Public Sub UpdateSelectedRole(intX67ID As Integer, lisX69 As List(Of BO.x69EntityRole_Assign), intP28ID As Integer)
        bas.SaveX69(_cDB, BO.x29IdEnum.p28Contact, intP28ID, lisX69, False, intX67ID)
    End Sub
    Public Sub ClearSelectedRole(intX67ID As Integer, intP28ID As Integer)
        _cDB.RunSQL("DELETE FROM x69EntityRole_Assign WHERE x67ID=" & intX67ID.ToString & " AND x69RecordPID=" & intP28ID.ToString)
    End Sub
    Public Function ConvertFromDraft(intPID As Integer) As Boolean
        Dim pars As New DbParameters
        With pars
            .Add("p28id", intPID, DbType.Int32)
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            pars.Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("p28_convertdraft", pars)

    End Function
End Class
