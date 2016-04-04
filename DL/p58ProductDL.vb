Public Class p58ProductDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.p58Product
        Dim s As String = GetSqlPart1() & " WHERE a.p58ID=@pid"
        Return _cDB.GetRecord(Of BO.p58Product)(s, New With {.pid = intPID})
    End Function

    Private Function GetSqlPart1() As String
        Return "select a.*," & bas.RecTail("p58", "a") & ",a.p58TreeIndex as _p58TreeIndex,a.p58TreePrev as _p58TreePrev,a.p58TreeNext as _p58TreeNext,a.p58TreeLevel as _p58TreeLevel FROM p58Product a"
    End Function
    Public Function Delete(intPID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intPID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        Return _cDB.RunSP("p58_delete", pars)

    End Function
    Public Function Save(cRec As BO.p58Product) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "p58ID=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            pars.Add("p58ParentID", BO.BAS.IsNullDBKey(.p58ParentID), DbType.Int32)
            pars.Add("p58Name", .p58Name, DbType.String)
            pars.Add("p58Code", .p58Code, DbType.String)
            pars.Add("p58ExternalPID", .p58ExternalPID, DbType.String)
           
            pars.Add("p58Ordinary", .p58Ordinary, DbType.Int32)
            pars.Add("p58ValidFrom", .ValidFrom, DbType.DateTime)
            pars.Add("p58ValidUntil", .ValidUntil, DbType.DateTime)
        End With

        If _cDB.SaveRecord("p58Product", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intLastSavedID As Integer = _cDB.LastSavedRecordPID
            
            Dim cDbTree As New clsDBTree(_cDB), strDbTreeErr As String = ""
            With cDbTree
                .BasicWHERE = ""
                .SaveTree("p58Product", "p58TreeLevel", "p58UserInsert", "p58TreeIndex", "p58ParentID", "p58ID", "p58Ordinary,p58Name", True, "p58TreePrev", "p58TreeNext", strDbTreeErr)
            End With

            bas.RecoveryUserCache(_cDB, _curUser)
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetList(Optional myQuery As BO.myQuery = Nothing, Optional intP28ID As Integer = 0) As IEnumerable(Of BO.p58Product)
        Dim s As String = GetSqlPart1()
        Dim strW As String = ""
        If Not myQuery Is Nothing Then
            strW += bas.ParseWhereMultiPIDs("a.p58ID", myQuery)
            strW += bas.ParseWhereValidity("p58", "a", myQuery)
        End If
        Dim pars As New DbParameters

        If intP28ID <> 0 Then
            strW += " AND a.p58ID IN (SELECT p58ID FROM p26Contact_Product WHERE p28ID=@p28id)"
            pars.Add("p28id", intP28ID, DbType.Int32)
        End If

        If strW = " AND " Then strW = ""

        pars.Add("j02id_query", _curUser.j02ID, DbType.Int32)
        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.p58TreeIndex,a.p58Ordinary,a.p58Name"

        Return _cDB.GetList(Of BO.p58Product)(s, pars)

    End Function
End Class
