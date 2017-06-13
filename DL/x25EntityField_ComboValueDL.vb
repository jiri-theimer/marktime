Public Class x25EntityField_ComboValueDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x25EntityField_ComboValue
        Dim s As String = "select a.*," & bas.RecTail("x25", "a") & ",x23.x23Name as _x23Name FROM x25EntityField_ComboValue a LEFT OUTER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID WHERE a.x25ID=@pid"
        Return _cDB.GetRecord(Of BO.x25EntityField_ComboValue)(s, New With {.pid = intPID})
    End Function
    Public Function LoadByCode(strCode As String, intX23ID As Integer) As BO.x25EntityField_ComboValue
        Dim pars As New DbParameters
        pars.Add("x23id", intX23ID, DbType.Int32)
        pars.Add("code", strCode, DbType.String)
        Dim s As String = "select TOP 1 a.*," & bas.RecTail("x25", "a") & ",x23.x23Name as _x23Name FROM x25EntityField_ComboValue a LEFT OUTER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID WHERE a.x23ID=@x23id AND a.x25Code=@code"
        Return _cDB.GetRecord(Of BO.x25EntityField_ComboValue)(s, pars)
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
            pars.Add("x25ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("x25ValidUntil", .ValidUntil, DbType.DateTime)

            pars.Add("x25FreeText01", .x25FreeText01, DbType.String)
            pars.Add("x25FreeText02", .x25FreeText02, DbType.String)
            pars.Add("x25FreeText03", .x25FreeText03, DbType.String)
            pars.Add("x25FreeText04", .x25FreeText04, DbType.String)
            pars.Add("x25FreeText05", .x25FreeText05, DbType.String)
            pars.Add("x25FreeNumber01", .x25FreeNumber01, DbType.Double)
            pars.Add("x25FreeNumber02", .x25FreeNumber02, DbType.Double)
            pars.Add("x25FreeNumber03", .x25FreeNumber03, DbType.Double)
            pars.Add("x25FreeNumber04", .x25FreeNumber04, DbType.Double)
            pars.Add("x25FreeNumber05", .x25FreeNumber05, DbType.Double)
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
    Public Function GetList(intX23ID As Integer) As IEnumerable(Of BO.x25EntityField_ComboValue)
        Dim s As String = "select a.*," & bas.RecTail("x25", "a") & ",x23.x23Name as _x23Name"
        s += " FROM x25EntityField_ComboValue a INNER JOIN x23EntityField_Combo x23 ON a.x23ID=x23.x23ID"
        If intX23ID <> 0 Then
            s += " WHERE a.x23ID=@x23id"
        End If

        s += " ORDER BY a.x23ID,a.x25Ordinary,a.x25Name"

        Dim pars As New DbParameters
        pars.Add("x23id", intX23ID, DbType.Int32)
        Return _cDB.GetList(Of BO.x25EntityField_ComboValue)(s, pars)

    End Function
End Class
