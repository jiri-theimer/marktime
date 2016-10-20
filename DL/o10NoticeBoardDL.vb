Public Class o10NoticeBoardDL
    Inherits DLMother

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.o10NoticeBoard
        Dim s As String = "select a.*," & bas.RecTail("o10", "a") & " FROM o10NoticeBoard a WHERE a.o10ID=@pid"
        
        Return _cDB.GetRecord(Of BO.o10NoticeBoard)(s, New With {.pid = intPID})
    End Function

    Public Function Save(cRec As BO.o10NoticeBoard, lisX69 As List(Of BO.x69EntityRole_Assign)) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "o10ID=@pid"
            pars.Add("pid", cRec.PID)
        Else
            cRec.ValidFrom = Now
            cRec.ValidUntil = DateSerial(3000, 1, 1)
        End If
        With cRec
            pars.Add("j02ID_Owner", BO.BAS.IsNullDBKey(.j02ID_Owner), DbType.Int32)
            pars.Add("o10Name", .o10Name, DbType.String)
            pars.Add("o10BodyHtml", .o10BodyHtml, DbType.String)
            pars.Add("o10BodyPlainText", .o10BodyPlainText, DbType.String)
            pars.Add("o10Ordinary", .o10Ordinary, DbType.Int32)
            pars.Add("o10BackColor", .o10BackColor, DbType.String)
            pars.Add("o10Locality", BO.BAS.IsNullDBKey(.o10Locality))
            pars.Add("o10validfrom", .ValidFrom, DbType.DateTime)
            pars.Add("o10validuntil", .ValidUntil, DbType.DateTime)

        End With

        If _cDB.SaveRecord("o10NoticeBoard", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            If Not lisX69 Is Nothing Then   'přiřazení rolí k článku
                bas.SaveX69(_cDB, BO.x29IdEnum.o10NoticeBoard, _cDB.LastSavedRecordPID, lisX69, bolINSERT)
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
        Return _cDB.RunSP("o10_delete", pars)
    End Function

    Public Function GetList(mq As BO.myQuery) As IEnumerable(Of BO.o10NoticeBoard)
        Dim s As String = "select a.*," & bas.RecTail("o10", "a") & " FROM o10NoticeBoard a", pars As New DbParameters
        Dim strW As String = "1=1"
        If Not BO.BAS.TestPermission(_curUser, BO.x53PermValEnum.GR_Admin) Then
            strW = "(a.j02ID_Owner=@j02id_query OR a.o10ID IN (SELECT x69.x69RecordPID FROM x69EntityRole_Assign x69 INNER JOIN x67EntityRole x67 ON x69.x67ID=x67.x67ID WHERE x67.x29ID=210 AND (x69.j02ID=@j02id_query OR x69.j11ID IN (SELECT j11ID FROM j12Team_Person WHERE j02ID=@j02id_query))))"
            pars.Add("j02id_query", _curUser.j02ID, DbType.Int32)
        End If

        strW += bas.ParseWhereMultiPIDs("a.o10ID", mq)
        strW += bas.ParseWhereValidity("o10", "a", mq)
      
        s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.o21Ordinary"
        Return _cDB.GetList(Of BO.o10NoticeBoard)(s, pars)

    End Function

   
End Class
