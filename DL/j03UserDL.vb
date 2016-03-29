﻿Imports System.Web

Public Class j03UserDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.j03User
        Dim s As String = GetSQLPart1()
        s += " WHERE a.j03id=@j03id"
        Return _cDB.GetRecord(Of BO.j03User)(s, New With {.j03id = intPID})
    End Function
    Public Function LoadByLogin(strLogin As String) As BO.j03User
        Dim s As String = GetSQLPart1()
        s += " WHERE a.j03login=@login"
        Return _cDB.GetRecord(Of BO.j03User)(s, New With {.login = strLogin})
    End Function
    Public Function LoadByJ02ID(intJ02ID As Integer) As BO.j03User
        Dim s As String = GetSQLPart1(1)
        s += " WHERE a.j02ID=@j02id"
        Return _cDB.GetRecord(Of BO.j03User)(s, New With {.j02id = intJ02ID})
    End Function
    Public Function LoadSysProfile(strLogin As String) As BO.j03UserSYS
        Return _cDB.GetRecord(Of BO.j03UserSYS)("dbo.j03user_load_sysuser", New With {.login = strLogin}, True)

    End Function
    'Public Function SYS_LoadDockState(strPage As String) As String
    '    Dim pars As New DL.DbParameters
    '    pars.Add("j03id", _curUser.PID, DbType.Int32)
    '    pars.Add("page", strPage, DbType.String)

    '    Return _cDB.GetValueFromSQL("SELECT x37DockState as Value FROM x37SavedDockState WHERE j03ID=@j03id AND x37Page=@page", pars)
    'End Function
    'Public Function SYS_SaveDockState(strPage As String, strDockState As String) As Boolean
    '    Dim pars As New DbParameters
    '    With pars
    '        .Add("j03id", _curUser.PID, DbType.Int32)
    '        .Add("page", strPage, DbType.String)
    '        .Add("dockstate", strDockState, DbType.String)
    '    End With
    '    Return _cDB.RunSP("x37_save", pars)
    'End Function
    
    Public Function GetVirtualCount(myQuery As BO.myQueryJ03) As Integer
        Dim s As String = "SELECT count(a.j03ID) as Value FROM j03user a INNER JOIN j04userrole j04 on a.j04id=j04.j04id LEFT OUTER JOIN j02Person j02 ON a.j02id=j02.j02id"
        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        If strW <> "" Then s += " WHERE " & strW

        Return _cDB.GetRecord(Of BO.GetInteger)(s, pars).Value
    End Function
    Private Function GetSQLWHERE(myQuery As BO.myQueryJ03, ByRef pars As DL.DbParameters) As String
        Dim strW As String = bas.ParseWhereMultiPIDs("a.j03ID", myQuery)
        strW += bas.ParseWhereValidity("j03", "a", myQuery)
        With myQuery
            If .j04ID <> 0 Then
                pars.Add("j04id", .j04ID, DbType.Int32)
                strW += " AND a.j04ID=@j04id"
            End If

            If .j02ID <> 0 Then
                pars.Add("j02id", .j02ID, DbType.Int32)
                strW += " AND a.j02ID=@j02id"
            End If
            If .SearchExpression <> "" Then
                strW += " AND (j03login like '%'+@expr+'%' OR isnull(j02LastName,'') LIKE '%'+@expr+'%' OR isnull(j02FirstName,'') LIKE '%'+@expr+'%')"
                pars.Add("expr", myQuery.SearchExpression, DbType.String)
            End If

        End With
        If strW <> "" Then strW = bas.TrimWHERE(strW)
        Return strW
    End Function
    Public Overloads Function GetList(myQuery As BO.myQueryJ03) As IEnumerable(Of BO.j03User)
        Dim s As String = GetSQLPart1(myQuery.TopRecordsOnly)

        Dim pars As New DL.DbParameters
        Dim strW As String = GetSQLWHERE(myQuery, pars)
        With myQuery
            Dim strSort As String = .MG_SortString
            strSort = Replace(strSort, "FullName", "j02LastName")
            If strSort = "" Then strSort = "j02lastname,j02firstname,j03login"
            If .MG_PageSize <> 0 Then
                'použít stránkování do gridu
                s = GetSQL_OFFSET(strW, strSort, .MG_PageSize, .MG_CurrentPageIndex, pars)
            Else
                'normální select
                If strW <> "" Then s += " WHERE " & strW
                If strSort <> "" Then
                    s += " ORDER BY " & strSort
                End If
            End If
        End With

        Return _cDB.GetList(Of BO.j03User)(s, pars)
    End Function
    
    
    Private Function GetSQLPart1(Optional intTOP As Integer = 0) As String
        Dim s As String = "SELECT"
        If intTOP > 0 Then s += " TOP " & intTOP.ToString
        s += " a.*," & bas.RecTail("j03", "a") & ",j04.j04name as _j04Name,j02.j02LastName as _j02LastName,j02.j02FirstName as _j02FirstName,j02.j02TitleBeforeName as _j02TitleBeforeName,j02.j02Email as _j02Email"
        s += " FROM j03user a INNER JOIN j04userrole j04 on a.j04id=j04.j04id LEFT OUTER JOIN j02Person j02 ON a.j02id=j02.j02id"
        Return s
    End Function
    Private Function GetSQL_OFFSET(strWHERE As String, strORDERBY As String, intPageSize As Integer, intCurrentPageIndex As Integer, ByRef pars As DL.DbParameters) As String
        Dim intStart As Integer = (intCurrentPageIndex) * intPageSize
        Dim s As String = "WITH rst AS (SELECT ROW_NUMBER() OVER (ORDER BY " & strORDERBY & ")-1 as RowIndex"
        s += ",a.*," & bas.RecTail("j03", "a") & ",j04.j04name,j02.j02LastName,j02.j02FirstName,j02.j02TitleBeforeName"
        s += " FROM j03user a INNER JOIN j04userrole j04 on a.j04id=j04.j04id LEFT OUTER JOIN j02Person j02 ON a.j02id=j02.j02id"
        If strWHERE <> "" Then s += " WHERE " & strWHERE
        s += ") SELECT TOP " & intPageSize.ToString & " *,j04Name as _j04Name,j02LastName as _j02LastName,j02FirstName as _j02FirstName,j02TitleBeforeName as _j02TitleBeforeName FROM rst"
        pars.Add("start", intStart, DbType.Int32)
        pars.Add("end", (intStart + intPageSize - 1), DbType.Int32)
        s += " WHERE RowIndex BETWEEN @start AND @end"
        s += " ORDER BY " & strORDERBY
        Return s
    End Function

    Public Function UpdateProfile(cRec As BO.j03User) As Boolean
        Dim pars As New DbParameters()
        With cRec
            pars.Add("pid", .PID)

            pars.Add("j03login", .j03Login, DbType.String)
            pars.Add("j02ID", .j02ID, DbType.Int32)
            pars.Add("j03MembershipUserId", .j03MembershipUserId, DbType.String)
        End With

        If _cDB.SaveRecord("j03user", pars, False, "j03id=@pid", True, _curUser.j03Login) Then
            Return True
        Else
            Return False
        End If

    End Function
    
    Public Function Save(cRec As BO.j03User) As Boolean
        Using sc As New Transactions.TransactionScope()     'ukládání podléhá transakci
            Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
            If cRec.PID <> 0 Then
                bolINSERT = False
                strW = "j03ID=@pid"
                pars.Add("pid", cRec.PID)
            End If
            With pars
                .Add("j04id", BO.BAS.IsNullDBKey(cRec.j04ID), DbType.Int32)
                .Add("j02id", BO.BAS.IsNullDBKey(cRec.j02ID), DbType.Int32)
                .Add("j03login", cRec.j03Login, DbType.String, , , True, "Login")
                .Add("j03IsLiveChatSupport", cRec.j03IsLiveChatSupport, DbType.Boolean)
                .Add("j03SiteMenuSkin", cRec.j03SiteMenuSkin, DbType.String)
                .Add("j03IsSiteMenuOnClick", cRec.j03IsSiteMenuOnClick, DbType.Boolean)
                .Add("j03IsDomainAccount", cRec.j03IsDomainAccount, DbType.Boolean)
                .Add("j03IsSystemAccount", cRec.j03IsSystemAccount, DbType.Boolean)
                .Add("j03MembershipUserId", cRec.j03MembershipUserId, DbType.String)
                .Add("j03validfrom", cRec.ValidFrom, DbType.DateTime)
                .Add("j03validuntil", cRec.ValidUntil, DbType.DateTime)
                .Add("j03Aspx_PersonalPage", cRec.j03Aspx_PersonalPage, DbType.String)
                .Add("j03Aspx_PersonalPage_Mobile", cRec.j03Aspx_PersonalPage_Mobile, DbType.String)
            End With

            If _cDB.SaveRecord("j03User", pars, bolINSERT, strW, True, _curUser.j03Login) Then
                sc.Complete()
                Return True
            Else
                Return False
            End If

           
        End Using


    End Function

    Public Function RenameLogin(cRec As BO.j03User, strNewLogin As String) As Boolean
        If cRec.j03MembershipUserId = "" Then
            _Error = "j03MembershipUserId není naplněn."
            Return False
        End If
        _cDB.ChangeConString2Membership()
        Dim pars As New DbParameters()
        Dim MyGuid As Guid = New Guid(cRec.j03MembershipUserId)

        With pars
            .Add("UserId", MyGuid, DbType.Guid)
            .Add("UserName", strNewLogin, DbType.String)
            .Add("ApplicationName", "/", DbType.String)
        End With
        If _cDB.RunSP("UpdateUserName", pars) Then
            _cDB.ChangeConString2Primary()
            If _cDB.RunSQL("UPDATE j03User set j03Login='" & strNewLogin & "' WHERE j03ID=" & cRec.PID.ToString) Then
                Return True
            Else
                Return False
            End If

        Else
            _Error = _cDB.ErrorMessage
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
        Return _cDB.RunSP("j03_delete", pars)
    End Function

    Public Function IsExistUserByLogin(strLogin As String, intJ03ID_Exclude As Integer) As Boolean
        Dim pars As New DbParameters
        pars.Add("login", Trim(strLogin), DbType.String)
        pars.Add("j03id_exclude", intJ03ID_Exclude, DbType.Int32)
        If _cDB.GetIntegerValueFROMSQL("select j03ID as Value FROM j03User where j03Login LIKE @login AND j03ID<>@j03id_exclude", pars) <> 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    
    Public Sub SYS_AppendJ90Log(intJ03ID As Integer, cLog As BO.j90LoginAccessLog)
        Dim pars As New DbParameters
        With pars
            .Add("j03ID", intJ03ID, DbType.Int32)
            .Add("j90Date", Now, DbType.DateTime)
            .Add("j90ClientBrowser", cLog.j90ClientBrowser, DbType.String)
            .Add("j90Platform", cLog.j90Platform, DbType.String)
            .Add("j90IsMobileDevice", cLog.j90IsMobileDevice, DbType.Boolean)
            .Add("j90MobileDevice", cLog.j90MobileDevice, DbType.String)
            .Add("j90ScreenPixelsWidth", cLog.j90ScreenPixelsWidth, DbType.Int32)
            .Add("j90ScreenPixelsHeight", cLog.j90ScreenPixelsHeight, DbType.Int32)
            .Add("j90UserHostAddress", cLog.j90UserHostAddress, DbType.String)
            .Add("j90UserHostName", cLog.j90UserHostName, DbType.String)
            .Add("j90IsDomainTrusted", cLog.j90IsDomainTrusted, DbType.Boolean)
            .Add("j90DomainUserName", cLog.j90DomainUserName, DbType.String)
        End With
        If _cDB.SaveRecord("j90LoginAccessLog", pars, True) Then

        End If
    End Sub

    Public Function SYS_GetList_UserParams(intJ03ID As Integer, x36keys As List(Of String)) As IEnumerable(Of BO.x36UserParam)
        If x36keys.Count = 0 Then Return Nothing

        Dim pars As New DbParameters, x As Integer, strParsInList As String = ""
        pars.Add("j03id", intJ03ID, DbType.Int32)
        Dim s As String = "select j03ID,x36Key,x36Value," & bas.RecTail("x36", "x36UserParam", True)
        For Each strKey As String In x36keys
            x += 1
            pars.Add("key" & x.ToString, strKey, DbType.String)
            If x = 1 Then
                strParsInList = "@key1"
            Else
                strParsInList += ",@key" & x.ToString
            End If
        Next

        s += " FROM x36UserParam WHERE j03id=@j03id AND x36Key IN (" & strParsInList & ")"

        Return _cDB.GetList(Of BO.x36UserParam)(s, pars)
    End Function
    Public Function SYS_DeleteAllUserParams(intJ03ID As Integer) As Boolean
        Return _cDB.RunSQL("DELETE FROM x36UserParam WHERE j03ID=" & intJ03ID.ToString)

    End Function
    Public Function SYS_SetUserParam(intJ03ID As Integer, strKey As String, strValue As String) As Boolean
        Dim pars As New DbParameters
        With pars
            .Add("j03id", intJ03ID, DbType.Int32)
            .Add("x36key", strKey, DbType.String)
            .Add("x36value", strValue, DbType.String)
        End With
        Return _cDB.RunSP("x36userparam_save", pars)
    End Function
    Public Function SYS_GetMyTag(intJ03ID As Integer, strKey As String, bolClearAfterRead As Boolean) As String
        Dim pars As New DbParameters
        With pars
            .Add("j03id", intJ03ID, DbType.Int32)
            .Add("x36key", strKey, DbType.String)
            .Add("clear_after_read", bolClearAfterRead, DbType.Boolean)
            .Add("x36value", Nothing, DbType.String, ParameterDirection.Output, 500)
        End With
        If _cDB.RunSP("x36userparam_get_mytag", pars) Then
            Return pars.Get(Of String)("x36value")
        Else
            Return ""
        End If
    End Function

    Public Function IsLoggedToday(intJ03ID As Integer) As Boolean
        If _cDB.GetRecord(Of Integer)("select select top 1 j90id from j90log where j03id=@j03id AND j90date>@today", New With {.j03id = intJ03ID, .today = Today}) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
End Class
