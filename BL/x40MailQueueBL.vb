Imports System.Net.Mail
Public Interface Ix40MailQueueBL
    Inherits IFMother

    Function SaveMessageToQueque(message As BO.smtpMessage, recipients As List(Of BO.x43MailQueue_Recipient), x29id As BO.x29IdEnum, intRecordPID As Integer, status As BO.x40StateENUM) As Integer
    Function SendMessageFromQueque(cRec As BO.x40MailQueue) As Boolean
    Function SendMessageFromQueque(intX40ID As Integer) As Boolean
    Function UpdateMessageState(intX40ID As Integer, NewState As BO.x40StateENUM) As Boolean
    Function Load(intPID As Integer) As BO.x40MailQueue
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQueryX40) As IEnumerable(Of BO.x40MailQueue)
    ''Function GetList_AllHisMessages(intJ03ID_Sender As Integer, intJ02ID_Person As Integer, Optional intTopRecs As Integer = 500) As IEnumerable(Of BO.x40MailQueue)
    Function SendMessageWithoutQueque(strRecipient As String, strBody As String, strSubject As String) As Boolean
End Interface

Class x40MailQueueBL
    Inherits BLMother
    Implements Ix40MailQueueBL
    Private WithEvents _cDL As DL.x40MailQueueDL
    
    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.x40MailQueueDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Load(intPID As Integer) As BO.x40MailQueue Implements Ix40MailQueueBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ix40MailQueueBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(myQuery As BO.myQueryX40) As IEnumerable(Of BO.x40MailQueue) Implements Ix40MailQueueBL.GetList
        Return _cDL.GetList(myQuery)
    End Function

    Public Function SaveMessageToQueue(mes As BO.smtpMessage, recipients As List(Of BO.x43MailQueue_Recipient), x29id As BO.x29IdEnum, intRecordPID As Integer, status As BO.x40StateENUM) As Integer Implements Ix40MailQueueBL.SaveMessageToQueque
        _Error = ""
        If recipients Is Nothing Then recipients = New List(Of BO.x43MailQueue_Recipient)
        If recipients.Count = 0 Then
            _Error = "Poštovní zpráva musí mít minimálně jednoho příjemce." : Return 0
        End If
        With mes
            If Trim(.Subject) = "" And Trim(.Body) = "" Then
                _Error = "Předmět zprávy i text zprávy jsou prázdné." : Return 0
            End If
            If Not String.IsNullOrEmpty(.Body) Then
                If .Body.IndexOf("--") > 0 Then
                    .Body = Replace(.Body, "[!--", "<!--")
                    .Body = Replace(.Body, "--]", "-->")
                End If
            End If
        End With
        Dim cX40 As New BO.x40MailQueue()
        With cX40
            .x40State = status
            .x29ID = x29id
            .x40RecordPID = intRecordPID
            .x40Body = mes.Body
            .x40IsHtmlBody = mes.IsHtmlBody
            .x40Subject = mes.Subject
            .x40SenderName = mes.SenderName
            .x40SenderAddress = mes.SenderAddress
            .j03ID_Sys = _cUser.PID
        End With
        'nejdříve uložit x40 záznam do databáze
        If _cDL.Save(cX40, recipients) Then
            cX40 = _cDL.Load(_cDL.LastSavedRecordPID)
        Else
            _Error = _cDL.ErrorMessage
            Return Nothing
        End If

        If mes.o27UploadGUID <> "" Then
            Dim lisTempUpload As IEnumerable(Of BO.p85TempBox) = Me.Factory.p85TempBoxBL.GetList(mes.o27UploadGUID)
            If Me.Factory.o27AttachmentBL.UploadAndSaveUserControl(lisTempUpload, BO.x29IdEnum.x40MailQueue, cX40.PID) Then

            End If
        End If
        If Not mes.AttachmentFiles_FullPath Is Nothing Then
            Dim cF As New BO.clsFile, lisO13 As IEnumerable(Of BO.o13AttachmentType) = Me.Factory.o13AttachmentTypeBL.GetList()
            For Each strFullPath As String In mes.AttachmentFiles_FullPath
                Dim cO27 As New BO.o27Attachment
                cO27.o13ID = lisO13.Where(Function(p) p.x29ID = BO.x29IdEnum.x40MailQueue)(0).PID
                cO27.x40ID = cX40.PID
                Dim strOrigFileName As String = cF.GetNameFromFullpath(strFullPath)
                Dim strExplicitArchiveFileName As String = strOrigFileName
                If Len(strExplicitArchiveFileName) < 15 Then strExplicitArchiveFileName = "" 'pokud je délka názvu souboru menší než 15, jméno archív souboru vygeneruje s GUID systém
                Me.Factory.o27AttachmentBL.UploadAndSaveOneFile(cO27, strOrigFileName, strFullPath, strExplicitArchiveFileName)
            Next
        End If
        If mes.o27UploadGUID <> "" Or Not mes.AttachmentFiles_FullPath Is Nothing Then
            Dim mq As New BO.myQueryO27
            mq.x40ID = cX40.PID
            Threading.Thread.Sleep(1000 * 2) 'počkat po dobu 2 sekundy, jinak to zlobí
            mes.o27Attachments = Me.Factory.o27AttachmentBL.GetList(mq)
        End If
        If Not mes.o27Attachments Is Nothing Then
            Dim strRootFolder As String = Me.Factory.x35GlobalParam.UploadFolder
            For Each cO27 In mes.o27Attachments
                ''Dim att As New Attachment(cO27.GetFullPath(strRootFolder))
                ''att.ContentDisposition.FileName = cO27.o27OriginalFileName
                ''mail.Attachments.Add(att)
                ''cX40.x40Attachments += ", " & att.ContentDisposition.FileName
                cX40.x40Attachments += ", " & cO27.o27OriginalFileName
            Next
        End If
        For Each c In recipients
            Select Case c.x43RecipientFlag
                Case BO.x43RecipientIdEnum.recTO
                    cX40.x40Recipient += ", " & c.x43Email
                Case BO.x43RecipientIdEnum.recCC
                    cX40.x40CC += ", " & c.x43Email
                Case BO.x43RecipientIdEnum.recBCC
                    cX40.x40BCC += ", " & c.x43Email
            End Select

        Next

        With cX40
            .x40Attachments = BO.BAS.OM1(.x40Attachments)
            .x40Recipient = BO.BAS.OM1(.x40Recipient)
            .x40BCC = BO.BAS.OM1(.x40BCC)
            .x40CC = BO.BAS.OM1(.x40CC)
        End With

        If _cDL.Save(cX40, Nothing) Then
            Return _cDL.LastSavedRecordPID
        Else
            Return 0
        End If

    End Function
    Public Function SendMessageWithoutQueque(strRecipient As String, strBody As String, strSubject As String) As Boolean Implements Ix40MailQueueBL.SendMessageWithoutQueque
        Dim mail As MailMessage = New MailMessage()
        With mail
            .From = New MailAddress("info@marktime.cz", "MARKTIME")
            .Body = strBody
            .IsBodyHtml = False
            .Subject = strSubject
            .BodyEncoding = System.Text.Encoding.UTF8
            .SubjectEncoding = System.Text.Encoding.UTF8
        End With
        mail.To.Add(New MailAddress(strRecipient, "MARKTIME support"))
        Dim smtp As SmtpClient = New SmtpClient(), bolSucceeded As Boolean = False
        Dim strIsUseWebConfigSetting As String = Me.Factory.x35GlobalParam.GetValueString("IsUseWebConfigSetting", "1")

        With smtp
            If strIsUseWebConfigSetting = "0" Then
                'vlastní SMTP nastavení z globálních proměnných
                Dim basicAuthenticationInfo As New System.Net.NetworkCredential(Me.Factory.x35GlobalParam.GetValueString("SMTP_Login"), Me.Factory.x35GlobalParam.GetValueString("SMTP_Password"), "")
                .UseDefaultCredentials = False
                .Credentials = basicAuthenticationInfo
                .Host = Me.Factory.x35GlobalParam.GetValueString("SMTP_Server")
            End If
            Try
                .Send(mail)
                Return True
            Catch ex As Exception
                _Error = ex.Message
                Return False
            End Try

        End With
    End Function
    Public Overloads Function SendMessageFromQueque(intX40ID As Integer) As Boolean Implements Ix40MailQueueBL.SendMessageFromQueque
        Dim cRec As BO.x40MailQueue = Load(intX40ID)
        If cRec Is Nothing Then Return False
        Return SendMessageFromQueque(cRec)
    End Function
    Public Overloads Function SendMessageFromQueque(cRec As BO.x40MailQueue) As Boolean Implements Ix40MailQueueBL.SendMessageFromQueque
        _Error = ""
        Dim recipients As IEnumerable(Of BO.x43MailQueue_Recipient) = _cDL.GetList_Recipients(cRec.PID)
        Dim strGlobalSenderAddress As String = Me.Factory.x35GlobalParam.GetValueString("SMTP_SenderAddress")
        Dim strSenderIsUser As String = Me.Factory.x35GlobalParam.GetValueString("SMTP_SenderIsUser")

        Dim mail As MailMessage = New MailMessage()
        With mail

            If strSenderIsUser = "1" Then
                .From = New MailAddress(cRec.x40SenderAddress, cRec.x40SenderName)
            Else
                .From = New MailAddress(strGlobalSenderAddress, cRec.x40SenderName)
                cRec.x40SenderAddress = strGlobalSenderAddress
                .ReplyToList.Add(New MailAddress(cRec.x40SenderAddress, cRec.x40SenderName))

            End If
            .Body = cRec.x40Body
            .IsBodyHtml = cRec.x40IsHtmlBody
            .Subject = cRec.x40Subject
            .BodyEncoding = System.Text.Encoding.UTF8
            .SubjectEncoding = System.Text.Encoding.UTF8
        End With

        For Each c In recipients.Where(Function(p) p.x43Email <> "")
            Try
                Select Case c.x43RecipientFlag
                    Case BO.x43RecipientIdEnum.recTO
                        mail.To.Add(New MailAddress(c.x43Email, c.x43DisplayName))
                    Case BO.x43RecipientIdEnum.recBCC
                        mail.Bcc.Add(New MailAddress(c.x43Email, c.x43DisplayName))
                    Case BO.x43RecipientIdEnum.recCC
                        mail.CC.Add(New MailAddress(c.x43Email, c.x43DisplayName))
                End Select
            Catch ex As Exception
                'chyba v mail adrese příjemce
            End Try

        Next

        Dim mqO27 As New BO.myQueryO27
        mqO27.x40ID = cRec.PID
        Dim lisO27 As IEnumerable(Of BO.o27Attachment) = Factory.o27AttachmentBL.GetList(mqO27)
        If lisO27.Count > 0 Then
            Dim strRootFolder As String = Me.Factory.x35GlobalParam.UploadFolder
            For Each cO27 In lisO27
                Dim strPath As String = cO27.GetFullPath(strRootFolder)
                Dim att As New Attachment(strPath)
                att.ContentDisposition.FileName = cO27.o27OriginalFileName
                mail.Attachments.Add(att)
                If cO27.o27OriginalFileName.IndexOf(".ics") > 0 Then
                    Dim mimeType As System.Net.Mime.ContentType = New System.Net.Mime.ContentType("text/calendar; method=REQUEST")
                    Dim icalView As New AlternateView(strPath, mimeType)
                    icalView.TransferEncoding = Net.Mime.TransferEncoding.SevenBit
                    mail.AlternateViews.Add(icalView)
                End If
            Next
        End If
        

        Dim smtp As SmtpClient = New SmtpClient(), bolSucceeded As Boolean = False
        Dim strIsUseWebConfigSetting As String = Me.Factory.x35GlobalParam.GetValueString("IsUseWebConfigSetting", "1")

        With smtp
            If strIsUseWebConfigSetting = "0" Then
                'vlastní SMTP nastavení z globálních proměnných
                Dim basicAuthenticationInfo As New System.Net.NetworkCredential(Me.Factory.x35GlobalParam.GetValueString("SMTP_Login"), Me.Factory.x35GlobalParam.GetValueString("SMTP_Password"), "")
                .UseDefaultCredentials = False
                .Credentials = basicAuthenticationInfo
                .Host = Me.Factory.x35GlobalParam.GetValueString("SMTP_Server")
            End If
            If _cUser.j02ID <> 0 Then
                Dim cPerson As BO.j02Person = Me.Factory.j02PersonBL.Load(_cUser.j02ID)
                If cPerson.j02SmtpServer <> "" Then
                    'osoba má vlastní SMTP účet
                    cRec.x40SenderAddress = _cUser.PersonEmail
                    cRec.x40SenderName = _cUser.Person
                    mail.From = New MailAddress(_cUser.PersonEmail, _cUser.Person)
                    Dim basicAuthenticationInfo As New System.Net.NetworkCredential()
                    If cPerson.j02IsSmtpVerify Then
                        basicAuthenticationInfo = New System.Net.NetworkCredential(cPerson.j02SmtpLogin, BO.Crypto.Decrypt(cPerson.j02SmtpPassword, "hoVaDo7Ivan1"), "")
                    End If
                    .UseDefaultCredentials = False
                    .Credentials = basicAuthenticationInfo
                    .Host = cPerson.j02SmtpServer
                End If
                log4net.LogManager.GetLogger("smtplog").Info("SMTP sender: " & .Host & vbCrLf & "Message subject: " & cPerson.j02SmtpLogin)
            End If
            Try
                .Send(mail)
                log4net.LogManager.GetLogger("smtplog").Info("Sender: " & mail.From.Address & "/" & mail.From.DisplayName & vbCrLf & "Message recipients: " & cRec.x40Recipient & vbCrLf & "Message subject: " & cRec.x40Subject & vbCrLf & "Message body: " & cRec.x40Body)

                For i As Integer = 0 To mail.Attachments.Count - 1
                    log4net.LogManager.GetLogger("smtplog").Info("Message attachment: " & mail.Attachments(i).ContentDisposition.FileName)
                Next
                cRec.x40State = BO.x40StateENUM.IsProceeded
                cRec.x40WhenProceeded = Now
                cRec.x40ErrorMessage = ""
                bolSucceeded = True
            Catch ex As Exception
                bolSucceeded = False
                _Error = ex.Message
                cRec.x40State = BO.x40StateENUM.IsError
                cRec.x40ErrorMessage = _Error
                log4net.LogManager.GetLogger("smtplog").Error("Error: " & _Error & vbCrLf & "Sender: " & mail.From.Address & "/" & mail.From.DisplayName & vbCrLf & "Message recipients: " & cRec.x40Recipient & vbCrLf & "Message subject: " & cRec.x40Subject & vbCrLf & "Message body: " & cRec.x40Body)
            End Try

        End With

        _cDL.Save(cRec, Nothing)

        Return bolSucceeded

    End Function
    Public Function UpdateMessageState(intX40ID As Integer, NewState As BO.x40StateENUM) As Boolean Implements Ix40MailQueueBL.UpdateMessageState
        If intX40ID = 0 Then _Error = "ID zprávy je nula." : Return False
        If Not (NewState = BO.x40StateENUM.InQueque Or NewState = BO.x40StateENUM.IsStopped Or NewState = BO.x40StateENUM.WaitOnConfirm) Then
            _Error = "Změnit stav lze pouze na [Zastaveno], [Mail fronta] nebo [Čeká na potvrzení]." : Return False
        End If
        Return _cDL.UpdateMessageState(intX40ID, NewState)
    End Function
    ''Public Function GetList_AllHisMessages(intJ03ID_Sender As Integer, intJ02ID_Person As Integer, Optional intTopRecs As Integer = 500) As IEnumerable(Of BO.x40MailQueue) Implements Ix40MailQueueBL.GetList_AllHisMessages
    ''    Return _cDL.GetList_AllHisMessages(intJ03ID_Sender, intJ02ID_Person)
    ''End Function
End Class
