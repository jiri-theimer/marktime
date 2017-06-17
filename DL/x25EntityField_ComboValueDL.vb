Public Class x25EntityField_ComboValueDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x25EntityField_ComboValue
        Dim s As String = GetSQLPart1(0) & " WHERE a.x25ID=@pid"
        Return _cDB.GetRecord(Of BO.x25EntityField_ComboValue)(s, New With {.pid = intPID})
    End Function
    Public Function LoadByCode(strCode As String, intX23ID As Integer) As BO.x25EntityField_ComboValue
        Dim pars As New DbParameters
        pars.Add("x23id", intX23ID, DbType.Int32)
        pars.Add("code", strCode, DbType.String)
        Dim s As String = GetSQLPart1(0) & " WHERE a.x23ID=@x23id AND a.x25Code=@code"
        Return _cDB.GetRecord(Of BO.x25EntityField_ComboValue)(s, pars)
    End Function
    Private Function GetSQLPart1(intTopRecs As Integer) As String
        Dim s As String = "select "
        If intTopRecs > 0 Then s += " TOP " & intTopRecs.ToString
        s += " a.*," & bas.RecTail("x25", "a") & ",x23.x23Name as _x23Name"
        s += " FROM x25EntityField_ComboValue a INNER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID"
        Return s
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

            pars.Add("x25FreeText01", .x25FreeText01, DbType.String)
            pars.Add("x25FreeText02", .x25FreeText02, DbType.String)
            pars.Add("x25FreeText03", .x25FreeText03, DbType.String)
            pars.Add("x25FreeText04", .x25FreeText04, DbType.String)
            pars.Add("x25FreeText05", .x25FreeText05, DbType.String)
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
    Public Function GetList(intX23ID As Integer, Optional myQuery As BO.myQuery = Nothing) As IEnumerable(Of BO.x25EntityField_ComboValue)
        If myQuery Is Nothing Then
            myQuery = New BO.myQuery
            myQuery.Closed = BO.BooleanQueryMode.NoQuery
        End If
        Dim s As String = GetSQLPart1(myQuery.TopRecordsOnly)
        Dim strW As String = bas.ParseWhereMultiPIDs("a.x25ID", myQuery)
        strW += bas.ParseWhereValidity("x25", "a", myQuery)
        Dim pars As New DbParameters
        If intX23ID <> 0 Then
            strW += " AND  a.x23ID=@x23id"
            pars.Add("x23id", intX23ID, DbType.Int32)
        End If
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.x23ID,a.x25Ordinary,a.x25Name"

        Return _cDB.GetList(Of BO.x25EntityField_ComboValue)(s, pars)

    End Function
End Class
