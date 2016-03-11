Public Class sendmail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Private _isChangeJ61ID As Boolean = False
    Private Property _color As System.Drawing.Color = System.Drawing.Color.SkyBlue


    Public Property CurrentX29ID As BO.x29IdEnum
        Get
            Return DirectCast(CInt(Me.hidX29ID.Value), BO.x29IdEnum)
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidX29ID.Value = CInt(value).ToString
            Me.hidPrefix.Value = BO.BAS.GetDataPrefix(value)
        End Set
    End Property
    Public ReadOnly Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
    End Property
    Private Sub sendmail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.upload1.Factory = Master.Factory
        Me.uploadlist1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            If Request.Item("prefix") <> "" Then
                Me.CurrentX29ID = BO.BAS.GetX29FromPrefix(Request.Item("prefix"))
                Master.DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Master.DataPID = 0 Then Master.StopPage("pid missing")
            Else
                Me.CurrentX29ID = BO.x29IdEnum.j02Person
                Master.DataPID = Master.Factory.SysUser.j02ID
            End If
            Me.upload1.GUID = BO.BAS.GetGUID
            Me.uploadlist1.GUID = Me.upload1.GUID
            With Master
                .HeaderText = "Odeslat poštovní zprávu"
                .HeaderIcon = "Images/email_32.png"
                .AddToolbarButton("Odeslat zprávu", "ok", , "Images/ok.png", , , , True)
            End With
            SetupCombos()
            If Master.Factory.SysUser.j02ID <> 0 Then
                Me.txtBody.Text = vbCrLf & vbCrLf & Master.Factory.j02PersonBL.Load(Master.Factory.SysUser.j02ID).j02EmailSignature
            End If
            
            SetupTemplates()

            If Request.Item("x31id") <> "" Then
                GenerateReportOnBehind(BO.BAS.IsNullInt(Request.Item("x31id")))
            End If
            If Request.Item("tempfile") <> "" Then
                PrepareTempFile(Request.Item("tempfile"))
            End If
        End If
    End Sub

    Private Sub SetupTemplates()
        Me.j61ID.DataSource = Master.Factory.j61TextTemplateBL.GetList(New BO.myQuery).Where(Function(p) p.x29ID = BO.x29IdEnum.x40MailQueue)
        Me.j61ID.DataBind()
        Me.j61ID.Items.Insert(0, "--Vyberte pojmenovanou šablonu zprávy/textu--")
    End Sub

    Private Sub SetupCombos()
        Dim mq As New BO.myQueryJ02
        mq.IntraPersons = BO.myQueryJ02_IntraPersons.IntraOnly
        Dim lisJ02 As IEnumerable(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList(mq)
        With Me.cbxAddPerson
            If lisJ02.Count < 100 Then
                .DataSource = lisJ02
                .DataBind()
                .Items.Insert(0, "--Osoba (interní)--")
            Else
                .Visible = False
            End If
        End With
        mq = New BO.myQueryJ02
        mq.IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p91Invoice
                mq.p91ID = Master.DataPID
            Case BO.x29IdEnum.p28Contact
                mq.p28ID = Master.DataPID
            Case BO.x29IdEnum.p41Project
                mq.p41ID = Master.DataPID
            Case Else
                mq.IntraPersons = BO.myQueryJ02_IntraPersons.NonIntraOnly
        End Select
        lisJ02 = Master.Factory.j02PersonBL.GetList(mq)

        With Me.cbxAddContactPerson
            If lisJ02.Count < 100 Then
                .DataSource = lisJ02
                .DataBind()
                .Items.Insert(0, "--Kontaktní osoba--")
            End If
            Dim intP28ID As Integer = 0
            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p28Contact : intP28ID = Master.DataPID
                Case BO.x29IdEnum.p91Invoice
                    Dim cP91 As BO.p91Invoice = Master.Factory.p91InvoiceBL.Load(Master.DataPID)
                    intP28ID = cP91.p28ID
            End Select
            If intP28ID > 0 Then
                Dim lisO32 As IEnumerable(Of BO.o32Contact_Medium) = Master.Factory.p28ContactBL.GetList_o32(intP28ID).Where(Function(p) p.o33ID = BO.o33FlagEnum.Email)
                For Each c In lisO32
                    Me.cbxAddContactPerson.Items.Add(New ListItem(c.o32Value & " " & c.o32Description, c.o32Value))
                Next
            End If

        End With
        With Me.cbxAddPosition
            .DataSource = Master.Factory.j07PersonPositionBL.GetList(New BO.myQuery)
            .DataBind()
            .Items.Insert(0, "--Pozice osoby--")
        End With
        With Me.cbxAddTeam
            .DataSource = Master.Factory.j11TeamBL.GetList(New BO.myQuery).Where(Function(p) p.j11IsAllPersons = False)
            .DataBind()
            .Items.Insert(0, "--Tým osob--")
        End With
        
    End Sub

    Private Sub upload1_AfterUploadAll() Handles upload1.AfterUploadAll
        Me.uploadlist1.RefreshData_TEMP()
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Dim message As New BO.smtpMessage()
            With message
                .Body = Trim(Me.txtBody.Text)
                .SenderAddress = Master.Factory.SysUser.PersonEmail
                .SenderName = Master.Factory.SysUser.Person
                .Subject = Me.txtSubject.Text
                If uploadlist1.ItemsCount > 0 Then
                    .o27UploadGUID = upload1.GUID
                End If

            End With
            Me.txtTo.Text = Replace(Replace(Me.txtTo.Text, " ", ""), ";", ",")
            Dim a() As String = Split(Trim(Me.txtTo.Text), ",")
            Dim recipients As New List(Of BO.x43MailQueue_Recipient)
            If Me.txtTo.Text <> "" Then
                For Each strEmail As String In a
                    Dim cX43 As New BO.x43MailQueue_Recipient()
                    cX43.x43Email = strEmail
                    cX43.x43RecipientFlag = BO.x43RecipientIdEnum.recTO
                    recipients.Add(cX43)
                Next
            End If
            Me.txtCC.Text = Replace(Replace(Me.txtCC.Text, " ", ""), ";", ",")
            If Me.txtCC.Text <> "" Then
                a = Split(Me.txtCC.Text, ",")
                For Each strEmail As String In a
                    Dim cX43 As New BO.x43MailQueue_Recipient()
                    cX43.x43Email = strEmail
                    cX43.x43RecipientFlag = BO.x43RecipientIdEnum.recCC
                    recipients.Add(cX43)
                Next
            End If
            
            Me.txtBCC.Text = Replace(Replace(Me.txtBCC.Text, " ", ""), ";", ",")
            If Me.txtBCC.Text <> "" Then
                a = Split(Me.txtBCC.Text, ",")
                For Each strEmail As String In a
                    Dim cX43 As New BO.x43MailQueue_Recipient()
                    cX43.x43Email = strEmail
                    cX43.x43RecipientFlag = BO.x43RecipientIdEnum.recBCC
                    recipients.Add(cX43)
                Next
            End If
            

            With Master.Factory.x40MailQueueBL
                Dim intMessageID As Integer = .SaveMessageToQueque(message, recipients, Me.CurrentX29ID, Master.DataPID)
                If intMessageID > 0 Then
                    If .SendMessageFromQueque(intMessageID) Then
                        Master.CloseAndRefreshParent("send-mail")
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                Else
                    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                End If
                'If .SendMessage(message, recipients, Me.CurrentX29ID, Master.DataPID) Then

                '    Master.CloseAndRefreshParent("send-mail")
                'Else
                '    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                'End If
            End With
            
        End If
    End Sub

    Private Sub cbxAddPerson_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAddPerson.SelectedIndexChanged
        If Me.cbxAddPerson.SelectedIndex > 0 Then
            Dim mq As New BO.myQueryJ02
            mq.AddItemToPIDs(CInt(Me.cbxAddPerson.SelectedValue))
            AR(mq)
            Me.cbxAddPerson.SelectedIndex = 0
        End If
    End Sub

    Private Sub AR(mq As BO.myQueryJ02, Optional strDirectMail As String = "")
        If strDirectMail <> "" Then
            If Me.txtTo.Text = "" Then
                Me.txtTo.Text = strDirectMail
            Else
                If Me.txtTo.Text.IndexOf(strDirectMail) < 0 Then
                    Me.txtTo.Text += "," & strDirectMail
                End If
            End If
        End If
        If mq Is Nothing Then Return
        Dim lis As IEnumerable(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList(mq).Where(Function(p) p.j02Email <> "")
        For Each c In lis
            If Me.txtTo.Text = "" Then
                Me.txtTo.Text = c.j02Email
            Else
                If Me.txtTo.Text.IndexOf(c.j02Email) < 0 Then
                    Me.txtTo.Text += "," & c.j02Email
                End If

            End If
        Next

    End Sub

    Private Sub cbxAddPosition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAddPosition.SelectedIndexChanged
        If Me.cbxAddPosition.SelectedIndex > 0 Then
            Dim mq As New BO.myQueryJ02
            mq.j07ID = CInt(Me.cbxAddPosition.SelectedValue)
            AR(mq)
            Me.cbxAddPosition.SelectedIndex = 0
        End If
    End Sub

    Private Sub cbxAddTeam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAddTeam.SelectedIndexChanged
        If Me.cbxAddTeam.SelectedIndex > 0 Then
            Dim mq As New BO.myQueryJ02
            mq.j11ID = CInt(Me.cbxAddTeam.SelectedValue)
            AR(mq)
            Me.cbxAddTeam.SelectedIndex = 0
        End If
    End Sub

    Private Sub GenerateReportOnBehind(intX31ID As Integer)
        Dim cRec As BO.x31Report = Master.Factory.x31ReportBL.Load(intX31ID)
        If cRec Is Nothing Then
            Master.Notify("Nelze načíst šablonu sestavy.", NotifyLevel.ErrorMessage)
            Return
        End If
        Dim strRepFullPath As String = Master.Factory.x35GlobalParam.UploadFolder
        If cRec.ReportFolder <> "" Then
            strRepFullPath += "\" & cRec.ReportFolder
        End If
        strRepFullPath += "\" & cRec.ReportFileName
        Dim cRep As New clsReportOnBehind()
        If Request.Item("datfrom") <> "" Then
            cRep.Query_DateFrom = BO.BAS.ConvertString2Date(Request.Item("datfrom"))
        End If
        If Request.Item("datuntil") <> "" Then
            cRep.Query_DateUntil = BO.BAS.ConvertString2Date(Request.Item("datuntil"))
        End If
        If cRec.x29ID > BO.x29IdEnum._NotSpecified And BO.BAS.IsNullInt(Request.Item("pid")) <> 0 Then
            'kontextová sestava - je třeba zjistit parametr @pid
            cRep.Query_RecordPID = BO.BAS.IsNullInt(Request.Item("pid"))
        End If

        Dim strOutputFileName As String = Master.Factory.GetRecordFileName(Me.CurrentX29ID, Master.DataPID, "pdf", False, cRec.x31Name)
        strOutputFileName = cRep.GenerateReport2Temp(Master.Factory, strRepFullPath, , strOutputFileName)
        If strOutputFileName = "" Then
            Master.Notify("Chyba při generování PDF.", NotifyLevel.ErrorMessage) : Return
        End If
        Me.txtSubject.Text = cRec.x31Name

        Dim cTemp As New BO.p85TempBox(), cF As New BO.clsFile
        Dim lisO13 As IEnumerable(Of BO.o13AttachmentType) = Master.Factory.o13AttachmentTypeBL.GetList(New BO.myQuery).Where(Function(p) p.x29ID = BO.x29IdEnum.x40MailQueue)
        With cTemp
            If lisO13.Count > 0 Then
                .p85OtherKey1 = lisO13(0).PID
                .p85FreeText06 = lisO13(0).o13Name
            End If
            .p85GUID = upload1.GUID
            .p85FreeText01 = strOutputFileName
            .p85FreeText02 = strOutputFileName
            .p85FreeText03 = "application/pdf"
            .p85FreeText04 = "PDF report"
            .p85FreeNumber01 = cF.GetFileSize(Master.Factory.x35GlobalParam.TempFolder & "\" & strOutputFileName)
        End With
        Master.Factory.p85TempBoxBL.Save(cTemp)
        uploadlist1.RefreshData_TEMP()
    End Sub

    Private Sub cbxAddContactPerson_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAddContactPerson.SelectedIndexChanged
        If Me.cbxAddContactPerson.SelectedIndex > 0 Then
            If Not IsNumeric(Me.cbxAddContactPerson.SelectedValue) Then
                'pouze kontaktní médium
                AR(Nothing, Me.cbxAddContactPerson.SelectedValue)
                Return
            End If
            Dim mq As New BO.myQueryJ02
            mq.IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified
            mq.AddItemToPIDs(CInt(Me.cbxAddContactPerson.SelectedValue))
            AR(mq)
            Me.cbxAddContactPerson.SelectedIndex = 0
        End If
    End Sub

    Private Sub j61ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j61ID.SelectedIndexChanged
        Handle_ChangeJ61ID()
    End Sub

    Private Sub Handle_ChangeJ61ID()
        Dim intJ61ID As Integer = BO.BAS.IsNullInt(Me.j61ID.SelectedValue)
        If intJ61ID <> 0 Then
            _isChangeJ61ID = True
            Dim c As BO.j61TextTemplate = Master.Factory.j61TextTemplateBL.Load(intJ61ID)
            If c.j61PlainTextBody <> "" Then
                Me.txtBody.Text = c.j61PlainTextBody : Me.txtBody.BackColor = _color
            End If
            If c.j61MailSubject <> "" Then
                Me.txtSubject.Text = c.j61MailSubject : Me.txtSubject.BackColor = _color
            Else
                If Me.txtSubject.BackColor = _color Then Me.txtSubject.Text = "" : Me.txtSubject.BackColor = Nothing
            End If
            If c.j61MailTO <> "" Then
                Me.txtTo.Text = c.j61MailTO : Me.txtTo.BackColor = _color
            Else
                If Me.txtTo.BackColor = _color Then Me.txtTo.Text = "" : Me.txtTo.BackColor = Nothing
            End If
            If c.j61MailCC <> "" Then
                Me.txtCC.Text = c.j61MailCC : Me.txtCC.BackColor = _color
            Else
                If Me.txtCC.BackColor = _color Then Me.txtCC.Text = "" : Me.txtCC.BackColor = Nothing
            End If
            If c.j61MailBCC <> "" Then
                Me.txtBCC.Text = c.j61MailBCC : Me.txtBCC.BackColor = _color
            Else
                If Me.txtBCC.BackColor = _color Then Me.txtBCC.Text = "" : Me.txtBCC.BackColor = Nothing
            End If
        End If
    End Sub

    Private Sub sendmail_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not _isChangeJ61ID Then
            Me.txtBody.BackColor = Nothing
            Me.txtSubject.BackColor = Nothing
            Me.txtBCC.BackColor = Nothing
            Me.txtCC.BackColor = Nothing
            Me.txtTo.BackColor = Nothing
        End If
        If BO.BAS.IsNullInt(Me.j61ID.SelectedValue) = 0 Then
            cmdEdit.Visible = False
        Else
            cmdEdit.Visible = True
        End If
        cmdClone.Visible = cmdEdit.Visible
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        SetupTemplates()
        If Me.hidHardRefreshPID.Value <> "" Then
            basUI.SelectDropdownlistValue(Me.j61ID, Me.hidHardRefreshPID.Value)
            Handle_ChangeJ61ID()
        End If
        hidHardRefreshPID.Value = "" : Me.hidHardRefreshFlag.Value = ""
    End Sub


    Private Sub PrepareTempFile(strTempFileName As String)
        Dim strOutputFileName As String = Master.Factory.GetRecordFileName(Me.CurrentX29ID, Master.DataPID, "pdf", False)

        Dim cTemp As New BO.p85TempBox(), cF As New BO.clsFile
        Dim lisO13 As IEnumerable(Of BO.o13AttachmentType) = Master.Factory.o13AttachmentTypeBL.GetList(New BO.myQuery).Where(Function(p) p.x29ID = BO.x29IdEnum.x40MailQueue)
        With cTemp
            If lisO13.Count > 0 Then
                .p85OtherKey1 = lisO13(0).PID
                .p85FreeText06 = lisO13(0).o13Name
            End If
            .p85GUID = upload1.GUID
            .p85FreeText01 = strOutputFileName
            .p85FreeText02 = strTempFileName
            .p85FreeText03 = cF.GetContentType(Master.Factory.x35GlobalParam.TempFolder & "\" & strTempFileName)
            .p85FreeNumber01 = cF.GetFileSize(Master.Factory.x35GlobalParam.TempFolder & "\" & strTempFileName)
        End With
        Master.Factory.p85TempBoxBL.Save(cTemp)
        uploadlist1.RefreshData_TEMP()
    End Sub
End Class