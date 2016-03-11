Public Class p47CapacityPlanDL
    Inherits DLMother
    Public Sub New(ServiceUser As BO.j03UserSYS)
        _curUser = ServiceUser
    End Sub

    Public Function GetList(mq As BO.myQueryP47) As IEnumerable(Of BO.p47CapacityPlan)
        Dim pars As New DbParameters, strW As String = ""
        With mq
            If .DateFrom > DateSerial(1900, 1, 1) Then
                strW += " AND a.p47DateFrom>=@datefrom" : pars.Add("datefrom", .DateFrom, DbType.DateTime)
            End If
            If .DateUntil < DateSerial(3000, 1, 1) Then
                strW += " AND a.p47DateUntil<=@dateuntil" : pars.Add("dateuntil", .DateUntil, DbType.DateTime)
            End If
            If .p41ID > 0 Then
                pars.Add("p41id", .p41ID, DbType.Int32)
                strW += " AND a.p41ID=@p41id"
            End If
            If .p28ID > 0 Then
                pars.Add("p28id", .p28ID, DbType.Int32)
                strW += " AND a.p41ID IN (SELECT p41ID FROM p41Project WHERE p28ID_Client=@p28id)"
            End If
            If .j02ID > 0 Then
                pars.Add("j02id", .j02ID, DbType.Int32)
                strW += " AND a.j02ID=@j02id"
            End If
            If Not .j02IDs Is Nothing Then
                If .j02IDs.Count > 0 Then
                    strW += " AND a.j02ID IN (" & String.Join(",", .j02IDs) & ")"
                End If
            End If
            If .p56ID > 0 Then
                pars.Add("p56id", .p56ID, DbType.Int32)
                strW += " AND a.p56ID=@p56id"
            End If
        End With
        strW = bas.TrimWHERE(strW)

        Dim s As String = "select a.*,j02.j02LastName+' '+j02.j02FirstName as _Person,isnull(p28.p28Name+' - ','') + p41.p41Name as _Project," & bas.RecTail("p47", "a")
        s += " FROM p47CapacityPlan a INNER JOIN j02Person j02 ON a.j02ID=j02.j02ID"
        s += " LEFT OUTER JOIN p41Project p41 ON a.p41ID=p41.p41ID"
        s += " LEFT OUTER JOIN p28Contact p28 ON p41.p28ID_Client=p28.p28ID"
        s += " WHERE " & strW
        s += " ORDER BY a.p47DateFrom"
        Return _cDB.GetList(Of BO.p47CapacityPlan)(s, pars)
    End Function

    Public Function SaveProjectPlan(intP41ID As Integer, lisP47 As List(Of BO.p47CapacityPlan)) As Boolean
        Dim mq As New BO.myQueryP47
        mq.p41ID = intP41ID
        Dim lisSaved As IEnumerable(Of BO.p47CapacityPlan) = GetList(mq)
        For Each c In lisP47
            Dim intPID As Integer = 0, bolNew As Boolean = True
            Dim lisFound As IEnumerable(Of BO.p47CapacityPlan) = lisSaved.Where(Function(p) p.p41ID = c.p41ID And p.j02ID = c.j02ID And p.p47DateFrom = c.p47DateFrom And p.p47DateUntil = c.p47DateUntil)
            If lisFound.Count > 0 Then
                intPID = lisFound(0).PID : bolNew = False
            End If
            Dim pars As New DbParameters, strW As String = ""
            If intPID > 0 Then
                strW = "p47ID=@pid"
                pars.Add("pid", intPID)
            End If
            pars.Add("j02ID", c.j02ID, DbType.Int32)
            pars.Add("p41ID", c.p41ID, DbType.Int32)
            pars.Add("p47DateFrom", c.p47DateFrom, DbType.DateTime)
            pars.Add("p47DateUntil", c.p47DateUntil, DbType.DateTime)
            pars.Add("p47HoursBillable", c.p47HoursBillable, DbType.Double)
            pars.Add("p47HoursNonBillable", c.p47HoursNonBillable, DbType.Double)
            pars.Add("p47HoursTotal", c.p47HoursTotal, DbType.Double)
            If c.IsSetAsDeleted Then
                _cDB.RunSQL("DELETE FROM p47CapacityPlan WHERE p47ID=" & c.PID.ToString)
            Else
                _cDB.SaveRecord("p47CapacityPlan", pars, bolNew, strW, True, _curUser.j03Login, False)
            End If


        Next
        Return True
    End Function
End Class
