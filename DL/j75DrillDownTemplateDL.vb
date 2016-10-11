Public Class j75DrillDownTemplateDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.j75DrillDownTemplate
        Dim s As String = GetSQLPart1() & " WHERE a.j75ID=@j75id"
        Return _cDB.GetRecord(Of BO.j75DrillDownTemplate)(s, New With {.j75id = intPID})
    End Function
    Public Function LoadSystemTemplate(x29id As BO.x29IdEnum, intJ03ID As Integer, Optional strMasterPrefix As String = "") As BO.j75DrillDownTemplate
        Dim pars As New DbParameters
        pars.Add("j03id", intJ03ID, DbType.Int32)
        pars.Add("x29id", x29id, DbType.Int32)

        Dim s As String = GetSQLPart1() & " WHERE a.j75IsSystem=1 AND a.j03ID=@j03id AND x29ID=@x29id"
        If strMasterPrefix <> "" Then
            pars.Add("masterprefix", strMasterPrefix, DbType.String)
            s += " AND j75MasterPrefix=@masterprefix"
        Else
            s += " AND j75MasterPrefix is null"
        End If
        Return _cDB.GetRecord(Of BO.j75DrillDownTemplate)(s, pars)
    End Function

    Public Function Save(cRec As BO.j75DrillDownTemplate, lisJ76 As List(Of BO.j76DrillDownTemplate_Item), lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
        _Error = ""
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "j75ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("j03ID", .j03ID, DbType.Int32)
            pars.Add("x29ID", BO.BAS.IsNullDBKey(cRec.x29ID), DbType.Int32)

            pars.Add("j75Name", .j75Name, DbType.String, , , True, "Název")
            pars.Add("j75IsSystem", .j75IsSystem, DbType.Boolean)
            pars.Add("j75MasterPrefix", .j75MasterPrefix, DbType.String)

            pars.Add("j75Level1", BO.BAS.IsNullDBKey(.j75Level1), DbType.Int32)
            pars.Add("j75Level2", BO.BAS.IsNullDBKey(.j75Level2), DbType.Int32)
            pars.Add("j75Level3", BO.BAS.IsNullDBKey(.j75Level3), DbType.Int32)
            pars.Add("j75Level4", BO.BAS.IsNullDBKey(.j75Level4), DbType.Int32)

            pars.Add("j75validfrom", cRec.ValidFrom, DbType.DateTime)
            pars.Add("j75validuntil", cRec.ValidUntil, DbType.DateTime)

        End With


        If _cDB.SaveRecord("j75DrillDownTemplate", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intJ75ID As Integer = _cDB.LastSavedRecordPID
            If Not bolINSERT Then _cDB.RunSQL("DELETE FROM j76DrillDownTemplate_Item WHERE j75ID=" & intJ75ID.ToString)
            For Each c In lisJ76
                pars = New DbParameters
                pars.Add("j75ID", intJ75ID, DbType.Int32)
                pars.Add("j76PivotSumFieldType", BO.BAS.IsNullDBKey(c.j76PivotSumFieldType), DbType.Int32)
                pars.Add("j76Level", c.j76Level, DbType.Int32)
                pars.Add("j76GridColumnName", c.j76GridColumnName, DbType.String)
                pars.Add("x28ID", BO.BAS.IsNullDBKey(c.x28ID), DbType.Int32)
                pars.Add("j76ExplicitHeader", c.j76ExplicitHeader, DbType.String)
                pars.Add("j76Ordinary", c.j76Ordinary, DbType.Int32)
                If Not _cDB.SaveRecord("j76DrillDownTemplate_Item", pars, True, , , , False) Then

                End If
            Next
            If Not lisX69 Is Nothing Then   'přiřazení rolí k šabloně sloupců
                bas.SaveX69(_cDB, BO.x29IdEnum.j75DrillDownTemplate, _cDB.LastSavedRecordPID, lisX69, bolINSERT)
            End If
            Return True
        Else
            Return False
        End If

    End Function
    Public Function GetList(myQuery As BO.myQuery, _x29id As BO.x29IdEnum) As IEnumerable(Of BO.j75DrillDownTemplate)
        Dim s As String = GetSQLPart1()
        Dim pars As New DbParameters

        Dim strW As String = "(a.j03ID=@j03id OR a.j75ID IN (SELECT x69.x69RecordPID FROM x69EntityRole_Assign x69 INNER JOIN x67EntityRole x67 ON x69.x67ID=x67.x67ID WHERE x67.x29ID=175 AND (x69.j02ID=@j02id_query OR x69.j11ID IN (SELECT j11ID FROM j12Team_Person WHERE j02ID=@j02id_query))))"
        pars.Add("j03id", _curUser.PID, DbType.Int32)
        pars.Add("j02id_query", _curUser.j02ID, DbType.Int32)


        If _x29id > BO.x29IdEnum._NotSpecified Then
            pars.Add("x29id", _x29id, DbType.Int32)
            strW += " AND a.x29ID=@x29id"
        End If

        If Not myQuery Is Nothing Then
            strW += bas.ParseWhereMultiPIDs("a.j75ID", myQuery)
            strW += bas.ParseWhereValidity("j75", "a", myQuery)

            If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        End If
        s += " ORDER BY a.j75IsSystem DESC,a.j75ID DESC"

        Return _cDB.GetList(Of BO.j75DrillDownTemplate)(s, pars)

    End Function
    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("j75_delete", pars)

    End Function

    Private Function GetSQLPart1() As String
        Dim s As String = "select a.*," & bas.RecTail("j75", "a")
        s += " FROM j75DrillDownTemplate a INNER JOIN j03User j03 ON a.j03ID=j03.j03ID"
        Return s
    End Function

    Public Function GetList_j76(intPID As Integer) As IEnumerable(Of BO.j76DrillDownTemplate_Item)
        Dim s As String = "select a.*,x28.x28Name"
        s += " FROM j76DrillDownTemplate_Item a LEFT OUTER JOIN x28EntityField x28 ON a.x28ID=x28.x28ID WHERE a.j75ID=@pid"
        s += " order by a.j76Ordinary"

        Return _cDB.GetList(Of BO.j76DrillDownTemplate_Item)(s, New With {.pid = intPID})
    End Function
End Class
