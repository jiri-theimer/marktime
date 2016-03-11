﻿Public Class x40MailQueueDL
    Inherits DLMother
    ''Private _cB65 As BO.b65WorkflowMessage = Nothing

    Public Sub New(ServiceUser As BO.j03User)
        _curUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x40MailQueue
        Dim s As String = GetSQLPart1() & " WHERE a.x40ID=@pid"

        Return _cDB.GetRecord(Of BO.x40MailQueue)(s, New With {.pid = intPID})
    End Function
    Public Function Delete(intX40ID As Integer) As Boolean
        Dim pars As New DbParameters()
        With pars
            .Add("j03id_sys", _curUser.PID, DbType.Int32)
            .Add("pid", intX40ID, DbType.Int32)
            .Add("err_ret", , DbType.String, ParameterDirection.Output, 500)
        End With
        If _cDB.RunSP("x40_delete", pars) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function UpdateMessageState(intX40ID As Integer, NewState As BO.x40StateENUM) As Boolean
        Dim cRec As BO.x40MailQueue = Load(intX40ID)
        Select Case NewState
            Case BO.x40StateENUM.InQueque
                cRec.x40ErrorMessage = ""
                cRec.x40WhenProceeded = Nothing
                cRec.x40State = BO.x40StateENUM.InQueque
            Case BO.x40StateENUM.IsStopped
                cRec.x40State = BO.x40StateENUM.IsStopped

        End Select
        Return Save(cRec, Nothing)
    End Function
    Public Function Save(cRec As BO.x40MailQueue, lisX43 As List(Of BO.x43MailQueue_Recipient)) As Boolean
        Dim pars As New DbParameters(), bolINSERT As Boolean = True, strW As String = ""
        If cRec.PID <> 0 Then
            bolINSERT = False
            strW = "x40id=@pid"
            pars.Add("pid", cRec.PID)
        End If
        With cRec
            If .x40State > BO.x40StateENUM._NotSpecified Then
                pars.Add("x40State", .x40State, DbType.Int32)
            End If
            pars.Add("j03ID_Sys", BO.BAS.IsNullDBKey(.j03ID_Sys), DbType.Int32)
            pars.Add("x29ID", BO.BAS.IsNullDBKey(cRec.x29ID), DbType.Int32)
            pars.Add("x40State", .x40State, DbType.Int32)
            pars.Add("x40RecordPID", BO.BAS.IsNullDBKey(.x40RecordPID), DbType.Int32)
            pars.Add("x40IsHtmlBody", cRec.x40IsHtmlBody, DbType.Boolean)

            pars.Add("x40IsAutoNotification", .x40IsAutoNotification, DbType.Boolean)

            pars.Add("x40Subject", .x40Subject, DbType.String)
            pars.Add("x40Body", .x40Body, DbType.String)
            pars.Add("x40Recipient", .x40Recipient, DbType.String)
            pars.Add("x40CC", .x40CC, DbType.String)
            pars.Add("x40BCC", .x40BCC, DbType.String)
            pars.Add("x40Attachments", .x40Attachments, DbType.String)

            pars.Add("x40SenderName", .x40SenderName, DbType.String)
            pars.Add("x40SenderAddress", .x40SenderAddress, DbType.String)


            pars.Add("x40WhenProceeded", BO.BAS.IsNullDBDate(.x40WhenProceeded), DbType.DateTime)
            pars.Add("x40ErrorMessage", .x40ErrorMessage, DbType.String)

        End With


        If _cDB.SaveRecord("x40MailQueue", pars, bolINSERT, strW, True, _curUser.j03Login) Then
            Dim intX40ID As Integer = Me.LastSavedRecordPID
            If Not lisX43 Is Nothing Then
                For Each c In lisX43
                    pars = New DbParameters
                    pars.Add("x40ID", intX40ID, DbType.Int32)
                    pars.Add("x43DisplayName", c.x43DisplayName, DbType.String)
                    pars.Add("x43Email", c.x43Email, DbType.String)
                    pars.Add("x43RecipientFlag", BO.BAS.IsNullDBKey(c.x43RecipientFlag), DbType.Int32)
                    _cDB.SaveRecord("x43MailQueue_Recipient", pars, True, , , , False)
                Next

            End If
            
            Return True
        Else
            _Error = _cDB.ErrorMessage
            Return False
        End If
    End Function

    Public Function GetList_AllHisMessages(intJ03ID_Sender As Integer, intJ02ID_Person As Integer, Optional intTopRecs As Integer = 500) As IEnumerable(Of BO.x40MailQueue)
        Dim s As String = GetSQLPart1(intTopRecs)
        Dim pars As New DbParameters, strW As String = ""
        strW += " AND (a.x40RecordPID=@j02id AND a.x29ID=102) OR a.j03ID_Sys=@j03id"
        pars.Add("j02id", intJ02ID_Person, DbType.Int32)
        pars.Add("j03id", intJ03ID_Sender, DbType.Int32)

        s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.x40ID DESC"

        Return _cDB.GetList(Of BO.x40MailQueue)(s, pars)
    End Function
    Public Function GetList(x29id As BO.x29IdEnum, intRecordPID As Integer, x40State As BO.x40StateENUM, Optional intTopRecs As Integer = 500) As IEnumerable(Of BO.x40MailQueue)
        Dim s As String = GetSQLPart1(intTopRecs)

        Dim pars As New DbParameters, strW As String = ""
        If intRecordPID <> 0 Then
            strW += " AND a.x40RecordPID=@pid"
            pars.Add("pid", intRecordPID, DbType.Int32)
        End If
        If x29id <> 0 Then
            strW += " AND a.x29ID=@x29id"
            pars.Add("x29id", x29id, DbType.Int32)
        End If
        If x40State > BO.x40StateENUM._NotSpecified Then
            strW += " AND a.x40State=@x40state"
            pars.Add("x40state", x40State, DbType.Int32)
        End If

        If strW <> "" Then s += " WHERE " & bas.TrimWHERE(strW)
        s += " ORDER BY a.x40ID DESC"

        Return _cDB.GetList(Of BO.x40MailQueue)(s, pars)

    End Function

    Public Function GetList_Recipients(intPID As Integer) As IEnumerable(Of BO.x43MailQueue_Recipient)
        Dim pars As New DbParameters
        pars.Add("pid", intPID, DbType.Int32)
        Dim s As String = "select * FROM x43MailQueue_Recipient WHERE x40ID=@pid"
        Return _cDB.GetList(Of BO.x43MailQueue_Recipient)(s, pars)
    End Function

    Private Function GetSQLPart1(Optional intTOP As Integer = 0) As String
        Dim s As String = "SELECT"
        If intTOP > 0 Then s += " TOP " & intTOP.ToString
        s += " a.*," & bas.RecTail("x40", , False, True)
        s += ",x29Name"
        s += " FROM x40MailQueue a LEFT OUTER JOIN x29Entity x29 ON a.x29ID=x29.x29ID"
        Return s
    End Function

    
    


End Class
