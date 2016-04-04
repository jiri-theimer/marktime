﻿Public Interface Ix46EventNotificationBL
    Inherits IFMother
    Function Save(cRec As BO.x46EventNotification) As Boolean
    Function Load(intPID As Integer) As BO.x46EventNotification
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery, Optional intJ02ID As Integer = 0, Optional intX45ID As Integer = 0) As IEnumerable(Of BO.x46EventNotification)
    Sub GenerateNotifyMessages(cX47 As BO.x47EventLog)

End Interface

Class x46EventNotificationBL
    Inherits BLMother
    Implements Ix46EventNotificationBL
    Private WithEvents _cDL As DL.x46EventNotificationDL

    Private Class MR
        Public Property j02ID As Integer
        Public Property j11ID As Integer
        Public Sub New(intJ02ID As Integer, intJ11ID As Integer)
            Me.j02ID = intJ02ID
            Me.j11ID = intJ11ID
        End Sub
    End Class

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.x46EventNotificationDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.x46EventNotification) As Boolean Implements Ix46EventNotificationBL.Save
        With cRec
            Dim cEvent As BO.x45Event = Me.Factory.ftBL.LoadX45(.x45ID)

            If Not .x46IsUseSystemTemplate And Len(Trim(.x46MessageTemplate)) < 10 Then
                _Error = "Rozsah notifikační zprávy je nedostatečný." : Return False
            End If
            If .x46IsUseSystemTemplate Then
                If Trim(cEvent.x45MessageTemplate) = "" Then
                    _Error = "Událost nemá definovaný výchozí text notifikační zprávy. Obsah zprávy proto musíte napsat ručně."
                    Return False
                End If
            End If
            If Trim(.x46MessageSubject) = "" Then
                _Error = "Chybí předmět zprávy." : Return False
            End If
            If .x46IsForAllRoles Then .x67ID = 0
            If .x46IsForAllReferenceRoles Then .x67ID_Reference = 0
        End With

        Return _cDL.Save(cRec)
    End Function
    Public Function Load(intPID As Integer) As BO.x46EventNotification Implements Ix46EventNotificationBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ix46EventNotificationBL.Delete
        Return _cDL.Delete(intPID)
    End Function
    Public Function GetList(myQuery As BO.myQuery, Optional intJ02ID As Integer = 0, Optional intX45ID As Integer = 0) As IEnumerable(Of BO.x46EventNotification) Implements Ix46EventNotificationBL.GetList
        Return _cDL.GetList(myQuery, intJ02ID, intX45ID)
    End Function

    Public Sub GenerateNotifyMessages(cX47 As BO.x47EventLog) Implements Ix46EventNotificationBL.GenerateNotifyMessages
        Dim strLinkUrl As String = Factory.x35GlobalParam.GetValueString("AppHost")
        Dim lisX46 As IEnumerable(Of BO.x46EventNotification) = GetList(New BO.myQuery, , CInt(cX47.x45ID))
        If cX47.x29ID_Reference > BO.x29IdEnum._NotSpecified Then
            'odfiltrovat notifikace pouze na ty, které se týkají refereční entity nebo všech referencí
            lisX46 = lisX46.Where(Function(p) p.x29ID_Reference = cX47.x29ID_Reference Or p.x29ID_Reference = BO.x29IdEnum._NotSpecified)
        End If
        If lisX46.Count = 0 Then
            Return  'k události nejsou nadefinovány žádné notifikace
        End If
        Dim cX45 As BO.x45Event = Me.Factory.ftBL.LoadX45(CInt(cX47.x45ID))
        Dim objects As New List(Of Object), intJ02ID_Owner As Integer = 0, intJ02ID_Owner_Reference As Integer = 0, objectReference As Object = Nothing
        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Factory.x67EntityRoleBL.GetList_x69(cX47.x29ID, cX47.x47RecordPID)
        Dim mrs As New List(Of MR)

        Select Case cX45.x29ID
            Case BO.x29IdEnum.p41Project
                Dim cRec As BO.p41Project = Factory.p41ProjectBL.Load(cX47.x47RecordPID)
                intJ02ID_Owner = cRec.j02ID_Owner
                objects.Add(cRec)
                If cX47.x45ID = BO.x45IDEnum.p41_limitfee_over Or cX47.x45ID = BO.x45IDEnum.p41_limithours_over Then
                    Dim mq As New BO.myQueryP31
                    mq.p41ID = cX47.x47RecordPID
                    objects.Add(Factory.p31WorksheetBL.LoadSumRow(mq, True, True))
                End If
                strLinkUrl += "/p41_framework.aspx?pid=" & cRec.PID.ToString & "&force=detail"
            Case BO.x29IdEnum.p56Task
                Dim cRec As BO.p56Task = Factory.p56TaskBL.Load(cX47.x47RecordPID)
                intJ02ID_Owner = cRec.j02ID_Owner
                objects.Add(cRec)
                strLinkUrl += "/dr.aspx?prefix=p56&pid=" & cX47.x47RecordPID.ToString
            Case BO.x29IdEnum.p91Invoice
                Dim cRec As BO.p91Invoice = Factory.p91InvoiceBL.Load(cX47.x47RecordPID)
                intJ02ID_Owner = cRec.j02ID_Owner
                objects.Add(cRec)
                strLinkUrl += "/p91_framework.aspx?pid=" & cRec.PID.ToString & "&force=detail"
            Case BO.x29IdEnum.p28Contact
                Dim cRec As BO.p28Contact = Factory.p28ContactBL.Load(cX47.x47RecordPID)
                intJ02ID_Owner = cRec.j02ID_Owner
                objects.Add(cRec)
                If cX47.x45ID = BO.x45IDEnum.p28_limitfee_over Or cX47.x45ID = BO.x45IDEnum.p28_limithours_over Then
                    Dim mq As New BO.myQueryP31
                    mq.p28ID_Client = cX47.x47RecordPID
                    objects.Add(Factory.p31WorksheetBL.LoadSumRow(mq, True, True))
                End If
                strLinkUrl += "/p28_framework.aspx?pid=" & cRec.PID.ToString & "&force=detail"
            Case BO.x29IdEnum.j02Person
                objects.Add(Factory.j02PersonBL.Load(cX47.x47RecordPID))
                strLinkUrl += "/dr.aspx?prefix=j02&pid=" & cX47.x47RecordPID.ToString
            Case BO.x29IdEnum.p51PriceList
                objects.Add(Factory.p51PriceListBL.Load(cX47.x47RecordPID))
                strLinkUrl += "/dr.aspx?prefix=p51&pid=" & cX47.x47RecordPID.ToString
            Case BO.x29IdEnum.b07Comment
                Dim cRec As BO.b07Comment = Factory.b07CommentBL.Load(cX47.x47RecordPID)
                If cRec.b07ID_Parent <> 0 Then
                    'notifikovat i autora komentáře, na který se reaguje
                    Dim cRecParent As BO.b07Comment = Factory.b07CommentBL.Load(cRec.b07ID_Parent)
                    mrs.Add(New MR(cRecParent.j02ID_Owner, 0))
                End If
                intJ02ID_Owner = cRec.j02ID_Owner
                objects.Add(cRec)
                With cRec
                    If .x29ID = BO.x29IdEnum.p41Project Then objectReference = Factory.p41ProjectBL.Load(.b07RecordPID) : strLinkUrl += "/p41_framework.aspx?pid=" & .b07RecordPID.ToString & "&force=comment"
                    If .x29ID = BO.x29IdEnum.p28Contact Then objectReference = Factory.p28ContactBL.Load(.b07RecordPID) : strLinkUrl += "/p28_framework.aspx?pid=" & .b07RecordPID.ToString & "&force=comment"
                    If .x29ID = BO.x29IdEnum.p56Task Then objectReference = Factory.p56TaskBL.Load(.b07RecordPID) : strLinkUrl += "/dr.aspx?prefix=p56&pid=" & .b07RecordPID.ToString
                    If .x29ID = BO.x29IdEnum.p91Invoice Then objectReference = Factory.p91InvoiceBL.Load(.b07RecordPID) : strLinkUrl += "/p91_framework.aspx?pid=" & .b07RecordPID.ToString
                    If .x29ID = BO.x29IdEnum.o22Milestone Then objectReference = Factory.o22MilestoneBL.Load(.b07RecordPID) : strLinkUrl += "/dr.aspx?prefix=o22&pid=" & .b07RecordPID.ToString
                    If .x29ID = BO.x29IdEnum.p31Worksheet Then objectReference = Factory.p31WorksheetBL.Load(.b07RecordPID) : strLinkUrl += "/dr.aspx?prefix=p31&pid=" & .b07RecordPID.ToString
                    If .x29ID = BO.x29IdEnum.o23Notepad Then
                        Dim c As BO.o23Notepad = Factory.o23NotepadBL.Load(.b07RecordPID)
                        If c.o23IsEncrypted Then
                            c.o23BodyPlainText = "Obsah je zašifrovaný."
                        End If
                        objectReference = c
                        strLinkUrl += "/dr.aspx?prefix=o23&pid=" & .b07RecordPID.ToString
                    End If
                End With
            Case BO.x29IdEnum.o23Notepad
                Dim cRec As BO.o23Notepad = Factory.o23NotepadBL.Load(cX47.x47RecordPID)
                intJ02ID_Owner = cRec.j02ID_Owner
                If cRec.o23IsEncrypted Then
                    cRec.o23BodyPlainText = "Obsah je zašifrovaný."
                End If
                objects.Add(cRec)
                With cRec
                    If .p41ID > 0 Then objectReference = Factory.p41ProjectBL.Load(.p41ID)
                    If .p28ID > 0 Then objectReference = Factory.p28ContactBL.Load(.p28ID)
                End With
                strLinkUrl += "/dr.aspx?prefix=o23&pid=" & cX47.x47RecordPID.ToString
            Case BO.x29IdEnum.o22Milestone
                Dim cRec As BO.o22Milestone = Factory.o22MilestoneBL.Load(cX47.x47RecordPID)
                intJ02ID_Owner = cRec.j02ID_Owner
                objects.Add(cRec)
                With cRec
                    If .p41ID > 0 Then objectReference = Factory.p41ProjectBL.Load(.p41ID)
                    If .p28ID > 0 Then objectReference = Factory.p28ContactBL.Load(.p28ID)
                    If .j02ID > 0 Then objectReference = Factory.j02PersonBL.Load(.j02ID)
                    If .p56ID > 0 Then objectReference = Factory.p56TaskBL.Load(.p56ID)
                    If .p91ID > 0 Then objectReference = Factory.p91InvoiceBL.Load(.p91ID)
                End With
                'notifikovat všechny osoby k události
                Dim lisO20 As IEnumerable(Of BO.o20Milestone_Receiver) = Factory.o22MilestoneBL.GetList_o20(cRec.PID)
                For Each c In lisO20
                    mrs.Add(New MR(c.j02ID, c.j11ID))
                Next
                strLinkUrl += "/dr.aspx?prefix=o22&pid=" & cX47.x47RecordPID.ToString
            Case BO.x29IdEnum.p36LockPeriod
                objects.Add(Factory.p36LockPeriodBL.Load(cX47.x47RecordPID))
            Case Else

        End Select
       
        If Not objectReference Is Nothing Then
            intJ02ID_Owner_Reference = objectReference.j02ID_Owner
            objects.Add(objectReference)
        End If

        Dim mes As New BO.smtpMessage
        mes.SenderAddress = Factory.x35GlobalParam.GetValueString("SMTP_SenderAddress")
        mes.SenderName = "MARKTIME robot"



        For Each c In lisX46
            Dim strMergedSubject As String = c.x46MessageSubject, strMergedBody As String = c.x46MessageTemplate

            If c.x46IsForRecordOwner Then
                mrs.Add(New MR(intJ02ID_Owner, 0))
            End If
            If c.x46IsForAllRoles Then
                For Each cRole In lisX69
                    mrs.Add(New MR(cRole.j02ID, cRole.j11ID))
                Next
            Else
                If c.x67ID <> 0 Then
                    If lisX69.Where(Function(p) p.x67ID = c.x67ID).Count > 0 Then
                        Dim cRole As BO.x69EntityRole_Assign = lisX69.Where(Function(p) p.x67ID = c.x67ID)(0)
                        mrs.Add(New MR(cRole.j02ID, cRole.j11ID))
                    End If
                End If
            End If
            If c.j02ID > 0 Then
                mrs.Add(New MR(c.j02ID, 0))
            End If
            If c.j11ID > 0 Then
                mrs.Add(New MR(0, c.j11ID))
            End If
            If cX47.x29ID_Reference > BO.x29IdEnum._NotSpecified And cX47.x47RecordPID_Reference > 0 Then
                'příjemcem zprávy může být i osoba se vztahem k referenční entitě
                If c.x46IsForRecordOwner_Reference And intJ02ID_Owner_Reference > 0 Then
                    mrs.Add(New MR(intJ02ID_Owner_Reference, 0))    'vlastník referenčního záznamu
                End If
            End If

            If mrs.Count > 0 Then
                'existují příjemci události
                Dim j02ids As IEnumerable(Of Integer) = mrs.Where(Function(p) p.j02ID <> 0).Select(Function(p) p.j02ID).Distinct
                Dim j11ids As IEnumerable(Of Integer) = mrs.Where(Function(p) p.j11ID <> 0).Select(Function(p) p.j11ID).Distinct

                Dim lisReceivers As IEnumerable(Of BO.j02Person) = Factory.j02PersonBL.GetList_j02_join_j11(j02ids.ToList, j11ids.ToList).Where(Function(p) p.IsClosed = False)

                If c.x46IsExcludeAuthor Then    'vyloučit z příjemců zprávy autora události
                    lisReceivers = lisReceivers.Where(Function(p) p.PID <> _cUser.j02ID)
                End If
                If lisReceivers.Count > 0 Then
                    'zkompletovat zprávu a odeslat do mail fronty
                    mes.Body = MergeContent(objects, strMergedBody, strLinkUrl)
                    mes.Subject = strMergedSubject


                    CompleteMessages(cX47, mes, lisReceivers)
                End If

            End If
        Next

    End Sub

    
    Private Sub CompleteMessages(cX47 As BO.x47EventLog, message As BO.smtpMessage, lisReceivers As IEnumerable(Of BO.j02Person))
        'Dim lisX43 As New List(Of BO.x43MailQueue_Recipient)
        'For Each cPerson In lisReceivers
        '    Dim c As New BO.x43MailQueue_Recipient()
        '    c.x43Email = cPerson.j02Email
        '    c.x43DisplayName = cPerson.FullNameAsc
        '    c.x43RecipientFlag = BO.x43RecipientIdEnum.recTO
        '    lisX43.Add(c)
        'Next
       
        For Each cJ02 In lisReceivers
            'pro každou osobu jedna zpráva
            Dim recipients As New List(Of BO.x43MailQueue_Recipient)
            Dim recipient As New BO.x43MailQueue_Recipient
            With recipient
                .x43Email = cJ02.j02Email
                .x43DisplayName = cJ02.FullNameAsc
                .x43RecipientFlag = BO.x43RecipientIdEnum.recTO
            End With
            recipients.Add(recipient)
            Factory.x40MailQueueBL.SaveMessageToQueque(message, recipients, BO.x29IdEnum.j02Person, cJ02.PID)
        Next
    End Sub
    

    Private Function MergeContent(objects As List(Of Object), strTemplateContent As String, strLinkUrl As String) As String
        strTemplateContent = Replace(strTemplateContent, "[%link%]", strLinkUrl, , , CompareMethod.Text)

        Dim fields As List(Of String) = GetAllMergeFieldsInContent(strTemplateContent)
        Dim reps As New List(Of BO.EasyStringColletion)

        For Each c In objects
            For Each field As String In fields
                Dim val As Object = BO.BAS.GetPropertyValue(c, field)
                Dim strReplace As String = ""
                If Not val Is Nothing Then
                    Select Case val.GetType.ToString
                        Case "System.String"
                            strReplace = val
                        Case "System.DateTime"
                            strReplace = BO.BAS.FD(val, True)
                        Case "System.Double"
                            strReplace = BO.BAS.FN(val)
                        Case "System.Boolean"
                            If val Then strReplace = "ANO" Else strReplace = "NE"
                        Case "System.Int32"
                            strReplace = BO.BAS.FNI(val)
                        Case Else
                            strReplace = val.ToString
                    End Select
                    If strReplace <> "" Then reps.Add(New BO.EasyStringColletion(field, strReplace))
                End If
            Next
        Next
        For Each c In reps  'nahradit nalezené pole
            strTemplateContent = Replace(strTemplateContent, "[%" & c.Key & "%]", c.Value, , , CompareMethod.Text)
        Next
        For Each field In fields    'nahradit zbylá prázdná pole
            If strTemplateContent.IndexOf("[%" & field & "%]") > 0 Then
                strTemplateContent = Replace(strTemplateContent, "[%" & field & "%]", "")
            End If
        Next

        Return strTemplateContent
    End Function
    Private Function GetAllMergeFieldsInContent(ByVal strContent As String) As List(Of String)
        'vrátí seznam slučovacích polí, které se vyskytují v strContent
        Return BO.BAS.GetAllMergeFieldsInContent(strContent)
        'Dim strTmp As String = strContent
        'Dim lngPosLeft As Integer, LeftMark As String = "[%", RightMark As String = "%]"
        'Dim lngPosRight As Integer, lisRet As New List(Of String)

        'lngPosLeft = InStr(1, strTmp, LeftMark, 1)
        'lngPosRight = InStr(1, strTmp, RightMark, 1)

        'Do While lngPosLeft > 0 And lngPosRight > 0
        '    Dim strField As String = ""
        '    If lngPosRight - lngPosLeft - Len(RightMark) > 0 Then
        '        strField = Mid$(strTmp, lngPosLeft + Len(LeftMark), lngPosRight - lngPosLeft - Len(RightMark))
        '        strField = Replace(strField, ",", "ß") 'ß za čárku
        '    End If
        '    If strField <> "" Then
        '        lisRet.Add(strField)
        '    End If
        '    ' Find the next occurrence
        '    lngPosLeft = InStr(lngPosLeft + Len(LeftMark), strTmp, LeftMark, 1)
        '    lngPosRight = InStr(lngPosRight + Len(RightMark), strTmp, RightMark, 1)
        'Loop

        'Return lisRet

    End Function

End Class
