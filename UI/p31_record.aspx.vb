Public Class p31_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord
    Private Property _Project As BO.p41Project = Nothing
    Private Property _Sheet As BO.p34ActivityGroup = Nothing

    Private Sub p31_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_record"
    End Sub
    Private ReadOnly Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p41ID.Value)
        End Get
    End Property
    Private Property CurrentJ02ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j02ID.Value)
        End Get
        Set(value As Integer)
            Me.j02ID.Value = value.ToString
            Me.p41ID.J02ID_Explicit = Me.j02ID.Value
        End Set
    End Property
    Private Property CurrentP34ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p34ID.SelectedValue)
        End Get
        Set(value As Integer)
            Me.p34ID.SelectedValue = value.ToString
        End Set
    End Property
    Private ReadOnly Property CurrentP33ID As BO.p33IdENUM
        Get
            If Me.hidP33ID.Value = "" Or Me.hidP33ID.Value = "0" Then Return BO.p33IdENUM.Cas
            Return CType(Me.hidP33ID.Value, BO.p33IdENUM)
        End Get
    End Property
    Private Property CurrentHoursEntryFlag As BO.p31HoursEntryFlagENUM
        Get
            Return CType(BO.BAS.IsNullInt(Me.hidHoursEntryFlag.Value), BO.p31HoursEntryFlagENUM)
        End Get
        Set(value As BO.p31HoursEntryFlagENUM)
            Me.hidHoursEntryFlag.Value = CInt(value).ToString
        End Set
    End Property
    Private Property CurrentIsScheduler As Boolean
        Get
            Return BO.BAS.BG(Me.hidCurIsScheduler.Value)
        End Get
        Set(value As Boolean)
            Me.hidCurIsScheduler.Value = BO.BAS.GB(value)
        End Set
    End Property
    Private Property CurrentP32ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p32ID.SelectedValue)
        End Get
        Set(value As Integer)
            Me.p32ID.SelectedValue = value.ToString
        End Set
    End Property
    Private Property CurrentP91ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP91ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP91ID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentP85ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP85ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP85ID.Value = value.ToString
        End Set
    End Property
    Public ReadOnly Property DocGUID As String
        Get
            If Me.hidDocGUID.Value = "" Then Me.hidDocGUID.Value = BO.BAS.GetGUID()
            Return Me.hidDocGUID.Value
        End Get
    End Property
    Private ReadOnly Property MyDefault_p34ID As Integer
        Get
            Return ViewState("last_p34id")
        End Get
    End Property
    Private ReadOnly Property MyDefault_p31Date As Date
        Get
            Return ViewState("last_p31date")
        End Get
    End Property
    Private ReadOnly Property MyDefault_p32ID As Integer
        Get
            Return ViewState("last_p32id")
        End Get
    End Property
    Private ReadOnly Property MyDefault_j02ID As Integer
        Get
            Return ViewState("last_j02id")
        End Get
    End Property
    Private ReadOnly Property MyDefault_Person As String
        Get
            Return ViewState("last_person")
        End Get
    End Property
    Public ReadOnly Property MyDefault_j27ID As Integer
        Get
            Return ViewState("last_j27id")
        End Get
    End Property
    Public ReadOnly Property MyDefault_VatRate As Double
        Get
            Return ViewState("last_vatrate")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            With Master
                .HeaderIcon = "Images/worksheet_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
            End With
            InhaleWorksheetSetting()

            InhaleMyDefault()


            RefreshRecord()

            If Master.IsRecordClone Then
                Master.DataPID = 0
                If Not Me.CurrentIsScheduler Then p31Date.SelectedDate = MyDefault_p31Date
            End If

            If Master.DataPID = 0 Then
                Master.HeaderText = "Zapsat worksheet úkon"
                If Me.CurrentP91ID > 0 Then
                    Master.HeaderText = "Zapsat úkon do faktury | " & Master.Factory.GetRecordCaption(BO.x29IdEnum.p91Invoice, Me.CurrentP91ID)
                End If
            End If
            cmdDoc.NavigateUrl = "javascript:o23_create('" & Me.DocGUID & "'," & Master.DataPID.ToString & ")"
            RefreshListO23()

            If Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.PresnyCasOdDo And (Me.TimeFrom.Text <> "" Or Me.TimeUntil.Text <> "") Then
                panT.Visible = True
            End If
        End If
    End Sub

    Private Sub InhaleMyDefault()
        With Master.Factory.SysUser
            ViewState("last_j02id") = .j02ID
            ViewState("last_person") = .PersonDesc
        End With
        ViewState("last_p34id") = 0
        ViewState("last_p32id") = 0
        ViewState("last_p31date") = Today
        If Request.Item("scheduler") = "1" Then
            Me.CurrentIsScheduler = True
        End If
        If Master.DataPID <> 0 Or Master.IsRecordClone Then Return

        Dim intDefP41ID As Integer = BO.BAS.IsNullInt(Request.Item("p41id"))
        Dim intDefP56ID As Integer = BO.BAS.IsNullInt(Request.Item("p56id"))

        'dál se pokračuje pouze pro nové záznamy
        If Master.DataPID = 0 Then
            If Request.Item("j02id") <> "" Then
                Dim intJ02ID As Integer = BO.BAS.IsNullInt(Request.Item("j02id"))
                'zápis za jinou osobu
                If intJ02ID > 0 Then
                    ViewState("last_j02id") = intJ02ID
                    ViewState("last_person") = Master.Factory.j02PersonBL.Load(intJ02ID).FullNameDesc
                End If
            End If
            If Request.Item("guid_approve") <> "" Then
                'zápis vyvolaný z rozhraní schvalování
                ViewState("guid_approve") = Request.Item("guid_approve")
            End If
            'načíst mnou naposledy zapsaný worksheet záznam
            Dim cRecLast As BO.p31Worksheet = Master.Factory.p31WorksheetBL.LoadMyLastCreated(True, intDefP41ID)
            If Not cRecLast Is Nothing Then
                With cRecLast
                    ViewState("last_p34id") = .p34ID
                    ViewState("last_p32id") = .p32ID
                    If DateDiff(DateInterval.Hour, .DateInsert.Value, Now) < 1 Then
                        'do hodiny starý záznam bere jako výchozí datum posledního úkonu + uživatele posledního úkonu
                        ViewState("last_p31date") = .p31Date
                    End If
                    If .p33ID = BO.p33IdENUM.Cas Then
                        If .p31HoursEntryFlag = BO.p31HoursEntryFlagENUM.NeniCas Then
                            Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.Hodiny
                        Else
                            Me.CurrentHoursEntryFlag = .p31HoursEntryFlag
                        End If
                    Else
                        Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.NeniCas
                    End If
                    ViewState("last_j27id") = .j27ID_Billing_Orig
                    ViewState("last_vatrate") = .p31VatRate_Orig
                End With
            Else
                'uživatel zatím nezapsal žádný worksheet úkon
                Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.Hodiny

            End If
        End If
        If Request.Item("p91id") <> "" Then
            Me.CurrentP91ID = BO.BAS.IsNullInt(Request.Item("p91id"))

        End If
        If Request.Item("p34id") <> "" Then
            'uživatel  požaduje zapisovat napřímo do konkrétního sešitu
            ViewState("last_p34id") = BO.BAS.IsNullInt(Request.Item("p34id"))
        End If
        If Request.Item("p31date") <> "" Then
            ViewState("last_p31date") = BO.BAS.ConvertString2Date(Request.Item("p31date"))
        End If
        
        Me.CurrentJ02ID = Me.MyDefault_j02ID
        Me.j02ID.Text = Me.MyDefault_Person
        Me.p31Date.SelectedDate = Me.MyDefault_p31Date

        
        If Master.DataPID = 0 And Request.Item("timelineinput") <> "" Then
            InhaleTimelineInput(intDefP41ID)    'zápis času pro hromadně označené dny v timeline
        End If
        
        If Request.Item("p85id") <> "" Then
            'zakládání záznamu z časovače - uloženo v tempu
            Me.CurrentP85ID = BO.BAS.IsNullInt(Request.Item("p85id"))
            Dim cTempRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(Me.CurrentP85ID)
            With cTempRec
                intDefP41ID = .p85OtherKey1
                Me.p31Text.Text = .p85Message
                Dim cT As New BO.clsTime
                Me.p31Value_Orig.Text = cT.GetTimeFromSeconds(.p85FreeNumber01 / 100)
                ViewState("last_p31date") = .p85FreeDate05
                Me.p31Date.SelectedDate = .p85FreeDate05
            End With
        End If

        If intDefP56ID > 0 Then
            Dim cP56 As BO.p56Task = Master.Factory.p56TaskBL.Load(BO.BAS.IsNullInt(Request.Item("p56id")))
            If Not cP56 Is Nothing Then intDefP41ID = cP56.p41ID
        End If
        If intDefP41ID = 0 And Request.Item("p28id") <> "" Then
            intDefP41ID = FindDefaultP41ID_FromClient(BO.BAS.IsNullInt(Request.Item("p28id")))
        End If

        If intDefP41ID > 0 Then
            Me.p41ID.Value = intDefP41ID.ToString
            Handle_ChangeP41(True)
            If Not _Project Is Nothing Then
                p41ID.Text = _Project.FullName
            End If
        End If
        If intDefP56ID > 0 Then
            Me.chkBindToP56.Checked = True
            SetupP56Combo()
            If Not Me.p56ID.RadCombo.Items.FindItemByValue(intDefP56ID.ToString) Is Nothing Then
                Me.p56ID.SelectedValue = intDefP56ID.ToString
            Else
                Master.Notify("V kontextu projektu a osoby není tento úkol dostupný k zapisování úkonu.", NotifyLevel.WarningMessage)
            End If

        End If
        If Request.Item("t1") <> "" And Request.Item("t2") <> "" Then Me.CurrentIsScheduler = True
        If Master.DataPID = 0 And Me.CurrentIsScheduler Then
            Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.PresnyCasOdDo   'požadavek na zápis hodin z denního kalendáře, přepnout na čas od/do
            Dim a() As String = Split(Request.Item("t1"), "_")
            Me.p31Date.SelectedDate = BO.BAS.ConvertString2Date(a(0))
            a = Split(a(1), ".")
            Me.TimeFrom.Text = Right("0" & a(0), 2) & ":" & Right("0" & a(1), 2)

            a = Split(Request.Item("t2"), "_") : a = Split(a(1), ".")
            Me.TimeUntil.Text = Right("0" & a(0), 2) & ":" & Right("0" & a(1), 2)
            Dim cT As New BO.clsTime
            Me.p31Value_Orig.Text = cT.ShowAsDec(Me.TimeUntil.Text) - cT.ShowAsDec(Me.TimeFrom.Text)
            Handle_ChangeHoursEntryFlag()

        End If
        
    End Sub

    Private Function FindDefaultP41ID_FromClient(intP28ID As Integer) As Integer
        If intP28ID = 0 Then Return 0
        Dim mq As New BO.myQueryP41 'najít první možný projekt v rámci klienta
        mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
        mq.p28ID = intP28ID
        Dim lisP41 As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq)
        If lisP41.Count > 0 Then
            Return lisP41(0).PID
        Else
            Return 0
        End If
    End Function

    Private Sub InhaleTimelineInput(ByRef intDefP41ID As Integer)
        Dim intYear As Integer = BO.BAS.IsNullInt(Request.Item("year"))
        Dim intMonth As Integer = BO.BAS.IsNullInt(Request.Item("month"))
        Dim a() As String = Split(Request.Item("timelineinput"), ","), dats As New List(Of String), intLastP41ID As Integer, intLastJ02ID As Integer
        For i As Integer = 0 To UBound(a)
            Dim b() As String = Split(a(i), "-")
            Dim intJ02ID As Integer = BO.BAS.IsNullInt(b(1))
            If intJ02ID = 0 Then intJ02ID = Master.Factory.SysUser.j02ID
            Dim intP41ID As Integer = BO.BAS.IsNullInt(b(2))
            Dim d As Date = DateSerial(intYear, intMonth, CInt(b(0)))
            dats.Add(Format(d, "dd.MM.yyyy"))
            If i = 0 Then
                Me.p31Date.SelectedDate = d
                ViewState("last_p31date") = d
            End If

            If intLastP41ID <> intP41ID And intLastP41ID > 0 Then
                Master.StopPage("Zapsat časový úkon do více dnů můžete pouze v rámci jednoho projektu.", False)
            End If
            If intLastJ02ID <> intJ02ID And intLastJ02ID > 0 Then
                Master.StopPage("Zapsat časový úkon do více dnů můžete pouze v rámci jedné osoby.", False)
            End If
            intLastP41ID = intP41ID
            intLastJ02ID = intJ02ID
        Next
        If intLastP41ID > 0 Then
            If Request.Item("rozklad") = "3" Then
                'v timeline je rozklad osoba->klient a nikoliv osoba->projekt, v intLastP41ID je p28id
                intDefP41ID = FindDefaultP41ID_FromClient(intLastP41ID)
            Else
                intDefP41ID = intLastP41ID
            End If
        End If
        If intLastJ02ID > 0 Then
            ViewState("last_j02id") = intLastJ02ID
            ViewState("last_person") = Master.Factory.j02PersonBL.Load(intLastJ02ID).FullNameDesc
            Me.CurrentJ02ID = Me.MyDefault_j02ID
            Me.j02ID.Text = Me.MyDefault_Person
        End If
        If dats.Count = 1 Then
            'pouze jeden datum na vstupu - normální režim bez multi-day vstupu
            Return
        End If
        Me.MultiDateInput.Text = String.Join(", ", dats.Distinct) : Me.MultiDateInput.Visible = True
        lblDate.Text = BO.BAS.OM2(Me.lblDate.Text, dats.Distinct.Count.ToString & "x")
        Me.p31Date.Visible = False
        Me.SharedCalendar.Visible = False
    End Sub
   
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then
            Handle_FF()
            Return
        End If

        Dim cD As BO.p31WorksheetDisposition = Master.Factory.p31WorksheetBL.InhaleRecordDisposition(Master.DataPID)
        If cD.RecordDisposition <> BO.p31RecordDisposition.CanEdit Then
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Owner) Then
                cD.RecordDisposition = BO.p31RecordDisposition.CanEdit  'uživatel má právo vlastníka všech worksheet záznamů v db
            End If
        End If
        If cD.RecordDisposition = BO.p31RecordDisposition._NoAccess Then
            If Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_Reader) Then
                cD.RecordDisposition = BO.p31RecordDisposition.CanRead  'právo číst všechny worksheet záznamy v db
            End If
        End If
        Select Case cD.RecordDisposition
            Case BO.p31RecordDisposition._NoAccess
                Master.StopPage("K záznamu nedisponujete oprávněním.")
            Case BO.p31RecordDisposition.CanRead, BO.p31RecordDisposition.CanApprove
                If Not Master.IsRecordClone Then Server.Transfer("p31_record_AA.aspx?pid=" & Master.DataPID.ToString)
            Case BO.p31RecordDisposition.CanEdit, BO.p31RecordDisposition.CanApproveAndEdit
                'zbývá právo k editaci - lze pokračovat dál
            Case Else
                Master.StopPage("Nelze zjistit oprávnění k záznamu.")
        End Select


        Select Case cD.RecordState
            Case BO.p31RecordState._NotExists
                Master.StopPage("Záznam nebyl nalezen v databázi.", True)
            Case BO.p31RecordState.Editing
                'záznam lze editovat
                'lze pokračovat v editaci
            Case Else
                'záznam pouze pro čtení
                If Not Master.IsRecordClone Then Server.Transfer("p31_record_AA.aspx?pid=" & Master.DataPID.ToString)
        End Select

        Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
        If cRec Is Nothing Then
            Master.StopPage("Záznam nebyl nalezen.", True)
        End If
        With cRec

            If Me.CurrentJ02ID <> .j02ID Then
                Me.CurrentJ02ID = .j02ID
                Me.j02ID.Text = .Person
                Handle_ChangeJ02()
            End If

            If .p41ID <> Me.CurrentP41ID Then
                Me.p41ID.Value = .p41ID.ToString
                Handle_ChangeP41(False)
                If Not _Project Is Nothing Then
                    Me.p41ID.Text = _Project.FullName
                End If

            End If
            If .p34ID <> Me.CurrentP34ID Then
                Me.CurrentP34ID = .p34ID
                Handle_ChangeP34()
            End If
            Me.CurrentP32ID = .p32ID


            Me.p31Text.Text = .p31Text
            Me.p31Date.SelectedDate = .p31Date
            Me.CurrentHoursEntryFlag = .p31HoursEntryFlag
            Handle_ChangeHoursEntryFlag()

            Select Case CurrentP33ID
                Case BO.p33IdENUM.Cas
                    Select Case .p31HoursEntryFlag
                        Case BO.p31HoursEntryFlagENUM.Minuty
                            Me.p31Value_Orig.Text = .p31Minutes_Orig.ToString
                        Case BO.p31HoursEntryFlagENUM.PresnyCasOdDo
                            Me.p31Value_Orig.Text = .p31Value_Orig.ToString
                            Me.TimeFrom.Text = .TimeFrom
                            Me.TimeUntil.Text = .TimeUntil()
                        Case Else
                            If .IsRecommendedHHMM() Then
                                Me.p31Value_Orig.Text = .p31HHMM_Orig
                            Else
                                Me.p31Value_Orig.Text = .p31Value_Orig.ToString
                            End If
                            ''If .p31Value_Orig_Entried > "" Then
                            ''    If .p31Value_Orig_Entried.IndexOf(":") > 0 Then Me.p31Value_Orig.Text = .p31HHMM_Orig Else Me.p31Value_Orig.Text = .p31Value_Orig.ToString
                            ''Else
                            ''    Me.p31Value_Orig.Text = .p31Value_Orig.ToString
                            ''End If

                    End Select
                Case BO.p33IdENUM.Kusovnik
                    Me.Units_Orig.Value = .p31Value_Orig
                Case BO.p33IdENUM.PenizeBezDPH
                    Me.p31Amount_WithoutVat_Orig.Value = .p31Amount_WithoutVat_Orig
                    Me.j27ID_Orig.SelectedValue = .j27ID_Billing_Orig.ToString
                Case BO.p33IdENUM.PenizeVcDPHRozpisu
                    Me.p31Amount_WithoutVat_Orig.Value = .p31Amount_WithoutVat_Orig
                    Me.p31Amount_WithVat_Orig.Value = .p31Amount_WithVat_Orig

                    Me.p31VatRate_Orig.SetText(.p31VatRate_Orig.ToString)
                    Me.p31VatRate_Orig.SelectedValue = .p31VatRate_Orig.ToString

                    Me.p31Amount_Vat_Orig.Value = .p31Amount_Vat_Orig
                    Me.j27ID_Orig.SelectedValue = .j27ID_Billing_Orig.ToString
            End Select
            If CurrentP33ID = BO.p33IdENUM.PenizeBezDPH Or CurrentP33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                Me.p31Calc_PieceAmount.Value = cRec.p31Calc_PieceAmount
                Me.p31Calc_Pieces.Value = cRec.p31Calc_Pieces
                Me.p35ID.SelectedValue = cRec.p35ID.ToString
            End If
            If .p56ID > 0 Then
                Me.chkBindToP56.Checked = True
                SetupP56Combo()
                Me.p56ID.SelectedValue = .p56ID.ToString
            End If

            Master.Timestamp = .Timestamp & " | Vlastník záznamu: <span class='val'>" & .Owner & "</span>"
            Master.HeaderText = .p34Name & " | " & BO.BAS.FD(.p31Date) & " | " & .Person & " | " & .p41Name
            If .IsClosed Then
                Master.ChangeToolbarSkin("BlackMetroTouch")
            End If
        End With


    End Sub

    Private Sub Handle_FF()
        Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p31Worksheet, Master.DataPID, BO.BAS.IsNullInt(Me.p34ID.SelectedValue))
        ff1.FillData(fields)
    End Sub

    Private Sub Handle_ChangeP41(bolTryRun_Handle_P34 As Boolean)
        imgFlag.Visible = False
        If Me.CurrentP41ID = 0 Then
            _Project = Nothing
        Else
            _Project = Master.Factory.p41ProjectBL.Load(Me.CurrentP41ID)
        End If

        Dim intDefP34ID As Integer = Me.CurrentP34ID
        Dim intDefP32ID As Integer = Me.CurrentP32ID
        Dim intCurP34ID As Integer = intDefP32ID, intCurP32ID As Integer = intDefP32ID

        If intDefP34ID = 0 And Master.DataPID = 0 Then
            intDefP34ID = Me.MyDefault_p34ID
        End If
        If intDefP32ID = 0 And Master.DataPID = 0 Then
            intDefP32ID = Me.MyDefault_p32ID
        End If

        If _Project Is Nothing Then
            Me.p34ID.Clear()
            Me.p32ID.Clear()
            Me.clue_project.Visible = False
        Else
            Me.clue_project.Visible = True
            Me.clue_project.Attributes.Item("rel") = "clue_p41_record.aspx?pid=" & _Project.PID.ToString
            Dim intLangIndex As Integer = 0
            If _Project.p87ID > 0 Or _Project.p87ID_Client > 0 Then
                Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(IIf(_Project.p87ID > 0, _Project.p87ID, _Project.p87ID_Client))
                intLangIndex = cP87.p87LangIndex
                If cP87.p87Icon <> "" Then
                    imgFlag.Visible = True
                    imgFlag.ImageUrl = "Images/flags/" & cP87.p87Icon
                    imgFlag.ToolTip = cP87.p87Name
                End If
            Else
                If BO.ASS.GetConfigVal("Implementation") = "hh" Then
                    imgFlag.Visible = True : imgFlag.ImageUrl = "Images/flags/czechrepublic.gif"
                End If
            End If

            Me.p34ID.DataSource = Master.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(_Project.PID, _Project.p42ID, _Project.j18ID, Master.Factory.SysUser.j02ID)
            Me.p34ID.DataBind()

            If Not bolTryRun_Handle_P34 Then Return 'dál už se nemá pokračovat


            If Me.p34ID.Rows > 1 And intDefP34ID > 0 Then
                Me.CurrentP34ID = intDefP34ID
            End If
            If Me.CurrentP34ID <> intCurP34ID Then
                Handle_ChangeP34()  'došlo ke změně sešitu -> vyvolat změnu sešitu, aby se naplnila nabídka aktivit
            End If
            If Me.p32ID.Rows > 1 And intDefP32ID > 0 Then
                Me.CurrentP32ID = intDefP32ID
                If Me.CurrentP32ID > 0 And Master.DataPID = 0 Then
                    'výchozí hodnoty aktivity pro nový záznam
                    Dim strDefText As String = ""
                    Dim cP32 As BO.p32Activity = Master.Factory.p32ActivityBL.Load(Me.CurrentP32ID)
                    Select Case intLangIndex
                        Case 0 : strDefText = cP32.p32DefaultWorksheetText
                        Case 1 : strDefText = cP32.p32DefaultWorksheetText_Lang1
                        Case 2 : strDefText = cP32.p32DefaultWorksheetText_Lang2
                        Case 3 : strDefText = cP32.p32DefaultWorksheetText_Lang3
                        Case 4 : strDefText = cP32.p32DefaultWorksheetText_Lang4
                    End Select
                    If strDefText <> "" Then
                        If Me.p31Text.Text = "" Then
                            Me.p31Text.Text = strDefText
                        Else
                            Me.p31Text.Text += vbCrLf & vbCrLf & strDefText
                        End If
                    End If
                    If cP32.p32IsTextRequired Then Me.lblP31Text.CssClass = "lblReq" Else lblP31Text.CssClass = "lbl"
                End If
            End If

            If Me.chkBindToP56.Checked Then
                SetupP56Combo()
            End If
        End If
    End Sub
    Private Sub Handle_ChangeP34()
        If Me.CurrentP34ID = 0 Then
            _Sheet = Nothing
            Me.p32ID.Clear()
          
            Return
        End If

        _Sheet = Master.Factory.p34ActivityGroupBL.Load(Me.CurrentP34ID)
        Dim mq As New BO.myQueryP32
        mq.p34ID = Me.CurrentP34ID
        Me.p32ID.DataSource = Master.Factory.p32ActivityBL.GetList(mq)
        Me.p32ID.DataBind()

        panT.Visible = False : panU.Visible = False : panM.Visible = False
        Me.hidP33ID.Value = CInt(_Sheet.p33ID).ToString
        Select Case _Sheet.p33ID
            Case BO.p33IdENUM.Cas
                panT.Visible = True
                If Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.NeniCas Then
                    Me.CurrentHoursEntryFlag = FindLastHoursEntryFlag()
                End If
                Handle_ChangeHoursEntryFlag()
                'If Me.p31Value_Orig.Visible Then Me.p31Value_Orig.Focus()
            Case BO.p33IdENUM.Kusovnik
                panU.Visible = True
                'Me.Units_Orig.Focus()
            Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                panM.Visible = True

                Dim b As Boolean = True
                If _Sheet.p33ID = BO.p33IdENUM.PenizeBezDPH Then
                    b = False
                End If
                Me.p31Amount_WithVat_Orig.Visible = b : Me.lblp31Amount_WithVat_Orig.Visible = b
                Me.p31Amount_Vat_Orig.Visible = b : Me.lblp31Amount_Vat_Orig.Visible = b
                Me.p31VatRate_Orig.Visible = b : Me.lblp31VatRate_Orig.Visible = b


                If Me.j27ID_Orig.Rows = 0 Then
                    Me.j27ID_Orig.DataSource = Master.Factory.ftBL.GetList_J27(New BO.myQuery)
                    Me.j27ID_Orig.DataBind()
                    If Me.MyDefault_j27ID > 0 Then
                        Me.j27ID_Orig.SelectedValue = Me.MyDefault_j27ID.ToString
                    End If
                    Me.p31VatRate_Orig.SetText(Me.MyDefault_VatRate.ToString)
                End If
                SetupVatRateCombo()
                If Me.p35ID.Rows = 0 Then
                    Me.p35ID.DataSource = Master.Factory.ftBL.GetList_P35()
                    Me.p35ID.DataBind()
                End If
        End Select
        If _Sheet.p34ActivityEntryFlag = BO.p34ActivityEntryFlagENUM.AktivitaSeNezadava Then
            'aktivita se ručně nezadává
            Me.lblP32ID.Visible = False
            Me.p32ID.Visible = False
        Else
            Me.lblP32ID.Visible = True
            Me.p32ID.Visible = True
        End If
        Handle_FF()
    End Sub

    Private Function FindLastHoursEntryFlag() As BO.p31HoursEntryFlagENUM
        Dim cRecLast As BO.p31Worksheet = Master.Factory.p31WorksheetBL.LoadMyLastCreated_TimeRecord()
        If Not cRecLast Is Nothing Then
            If cRecLast.p31HoursEntryFlag = BO.p31HoursEntryFlagENUM.NeniCas Then Return BO.p31HoursEntryFlagENUM.Hodiny
            Return cRecLast.p31HoursEntryFlag
        Else
            Return BO.p31HoursEntryFlagENUM.Hodiny
        End If
    End Function

    Private Sub Handle_ChangeHoursEntryFlag()
        Dim bolET As Boolean = False
        Select Case Me.CurrentHoursEntryFlag
            Case BO.p31HoursEntryFlagENUM.Hodiny, BO.p31HoursEntryFlagENUM.PresnyCasOdDo
                lblHours.Text = "Hodiny:"
                lblHours.ToolTip = "Hodiny zapisujte jako dekadické číslo (např. 0,25 nebo 1,5) nebo ve formátu HH:mm (např. 00:15 nebo 01:30)"
                Me.p31Value_Orig.Attributes.Item("onchange") = "handle_hours()"
            Case BO.p31HoursEntryFlagENUM.Minuty
                lblHours.Text = "Minuty:"
                lblHours.ToolTip = "Čas zapisujte jako celé číslo v minutách."
                Me.p31Value_Orig.Attributes.Item("onchange") = "handle_minutes()"
        End Select
        If Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.PresnyCasOdDo Or Me.p31_default_HoursEntryFlag.Value = "3" Or Me.CurrentIsScheduler Then
            bolET = True
        End If
        lblTimeFrom.Visible = bolET : lblTimeUntil.Visible = bolET
        Me.TimeFrom.Visible = bolET : Me.TimeUntil.Visible = bolET
     

        
    End Sub

   

    Private Sub p41ID_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles p41ID.AutoPostBack_SelectedIndexChanged
        Handle_ChangeP41(True)
    End Sub

    Private Sub p34ID_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles p34ID.ItemDataBound
        Dim cRec As BO.p34ActivityGroup = CType(e.Item.DataItem, BO.p34ActivityGroup)
        Select Case cRec.p33ID
            Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                If cRec.p34IncomeStatementFlag = BO.p34IncomeStatementFlagENUM.Prijem Then
                    e.Item.ForeColor = Drawing.Color.Blue
                Else
                    e.Item.ForeColor = Drawing.Color.Brown
                End If

            Case BO.p33IdENUM.Kusovnik
                e.Item.ForeColor = Drawing.Color.Green
        End Select
    End Sub

    Private Sub p34ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles p34ID.SelectedIndexChanged
        Handle_ChangeP34()
        Me.p32ID.Focus()
    End Sub

    Private Sub p32ID_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles p32ID.ItemDataBound
        Dim cRec As BO.p32Activity = CType(e.Item.DataItem, BO.p32Activity)
        If Not cRec.p32IsBillable Then
            e.Item.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p31WorksheetBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p31-delete")
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.WarningMessage)
            End If
        End With
    End Sub
    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p31WorksheetBL
            Dim cRec As New BO.p31WorksheetEntryInput()
            With cRec
                .SetPID(Master.DataPID)
                .j02ID = Me.CurrentJ02ID
                .p41ID = Me.CurrentP41ID
                .p34ID = BO.BAS.IsNullInt(Me.p34ID.SelectedValue)
                .p32ID = BO.BAS.IsNullInt(Me.p32ID.SelectedValue)
                .p56ID = BO.BAS.IsNullInt(Me.p56ID.SelectedValue)
                .DocGUID = Me.DocGUID
                If Me.p31Date.IsEmpty Then
                    .p31Date = Today
                Else
                    .p31Date = Me.p31Date.SelectedDate
                End If
                .p31Text = Me.p31Text.Text
              
                Select Case Me.CurrentP33ID
                    Case BO.p33IdENUM.Cas
                        .p31HoursEntryflag = Me.CurrentHoursEntryFlag
                        .Value_Orig = Me.p31Value_Orig.Text
                        .Value_Orig_Entried = .Value_Orig

                        If Me.CurrentHoursEntryFlag = BO.p31HoursEntryFlagENUM.PresnyCasOdDo Then
                            .TimeFrom = Me.TimeFrom.Text
                            .TimeUntil = Me.TimeUntil.Text
                        End If
                        If Not .ValidateEntryTime(5) Then
                            Master.Notify(.ErrorMessage, 2)
                            Return
                        End If
                    Case BO.p33IdENUM.Kusovnik
                        .Value_Orig = BO.BAS.IsNullNum(Me.Units_Orig.Value)
                        .Value_Orig_Entried = .Value_Orig
                        If Not .ValidateEntryKusovnik() Then
                            Master.Notify(.ErrorMessage, 2)
                            Return
                        End If
                    Case BO.p33IdENUM.PenizeBezDPH
                        .Amount_WithoutVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithoutVat_Orig.Value)
                        .Value_Orig_Entried = .Amount_WithoutVat_Orig.ToString
                        .j27ID_Billing_Orig = BO.BAS.IsNullInt(Me.j27ID_Orig.SelectedValue)
                    Case BO.p33IdENUM.PenizeVcDPHRozpisu
                        .Amount_WithoutVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithoutVat_Orig.Value)
                        .Value_Orig_Entried = .Amount_WithoutVat_Orig.ToString
                        .VatRate_Orig = BO.BAS.IsNullNum(Me.p31VatRate_Orig.Text)
                        .j27ID_Billing_Orig = BO.BAS.IsNullInt(Me.j27ID_Orig.SelectedValue)
                        .Amount_WithVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithVat_Orig.Value)
                        .Amount_Vat_Orig = BO.BAS.IsNullNum(Me.p31Amount_Vat_Orig.Value)
                End Select
                Select Case Me.CurrentP33ID
                    Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                        .p31Calc_PieceAmount = BO.BAS.IsNullNum(Me.p31Calc_PieceAmount.Value)
                        .p31Calc_Pieces = BO.BAS.IsNullNum(Me.p31Calc_Pieces.Value)
                        .p35ID = BO.BAS.IsNullInt(Me.p35ID.SelectedValue)
                End Select
               
            End With

            If Me.MultiDateInput.Visible And Me.MultiDateInput.Text <> "" Then
                Dim dats As New List(Of Date)   'hromadné uložení úkonu do více dnů najednou
                Dim a() As String = Split(Me.MultiDateInput.Text, ",")
                For i = 0 To UBound(a)
                    dats.Add(BO.BAS.ConvertString2Date(Trim(a(i))))
                Next
                If .SaveOrigRecordBatch(dats, cRec, Nothing) Then
                    Master.CloseAndRefreshParent("p31-save")
                Else
                    Master.Notify(.ErrorMessage, NotifyLevel.WarningMessage)
                End If
                Return
            End If

            Dim lisFF As List(Of BO.FreeField) = Me.ff1.GetValues()

            If .SaveOrigRecord(cRec, lisFF) Then
                Master.DataPID = .LastSavedPID
                If Me.CurrentP91ID > 0 Then
                    'přesměrovat úkon rovnou na schvalování
                    Server.Transfer("p31_record_approve.aspx?pid=" & Master.DataPID.ToString & "&p91id=" & Me.CurrentP91ID.ToString, False)
                    Return
                End If
                If ViewState("guid_approve") <> "" Then
                    'zařadit úkon do rozpracovaného schvalování
                    Dim c As New BO.p85TempBox()
                    c.p85GUID = ViewState("guid_approve")
                    c.p85DataPID = Master.DataPID
                    Master.Factory.p85TempBoxBL.Save(c)
                    AddRecord2Approving()
                End If
                If Me.CurrentP85ID > 0 Then
                    'odstranit temp záznam předlohy
                    Master.Factory.p85TempBoxBL.Delete(Master.Factory.p85TempBoxBL.Load(Me.CurrentP85ID))
                End If
                Master.CloseAndRefreshParent("p31-save")
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.WarningMessage)
            End If
        End With
    End Sub

    Private Sub AddRecord2Approving()
        Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
        Dim cApprove As New BO.p31WorksheetApproveInput(Master.DataPID, cRec.p33ID)
        With cApprove
            .GUID_TempData = ViewState("guid_approve")
            .p71id = BO.p71IdENUM.Schvaleno
            If cRec.p32IsBillable Then
                .p72id = BO.p72IdENUM.Fakturovat
                .Value_Approved_Billing = cRec.p31Value_Orig
                .Value_Approved_Internal = cRec.p31Value_Orig
                Select Case .p33ID
                    Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                        .Rate_Billing_Approved = cRec.p31Rate_Billing_Orig
                        If cRec.p31Rate_Billing_Orig = 0 Then
                            .p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                        End If
                    Case BO.p33IdENUM.PenizeBezDPH
                        If cRec.p31Value_Orig = 0 Then
                            .p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                        End If
                    Case BO.p33IdENUM.PenizeVcDPHRozpisu
                        .VatRate_Approved = cRec.p31VatRate_Orig
                        If cRec.p31Value_Orig = 0 Then
                            .p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                        End If
                End Select
            Else
                .p72id = BO.p72IdENUM.SkrytyOdpis
            End If
        End With
        Master.Factory.p31WorksheetBL.Save_Approving(cApprove, True)
    End Sub

    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        Select Case HardRefreshFlag.Value
            Case "p31-setting"
                If Me.CurrentP33ID = BO.p33IdENUM.Cas Then
                    InhaleWorksheetSetting()
                    Handle_ChangeHoursEntryFlag()

                End If
            Case "o23-save"
                Dim cRec As BO.o23Notepad = Master.Factory.o23NotepadBL.Load(BO.BAS.IsNullInt(Me.HardRefreshPID.Value))
                If cRec.p31ID = 0 Then
                    Master.Notify("Vazbu úkonu na dokument potvrdíte až tlačítkem [Uložit změny].")
                End If
                RefreshListO23()
            Case "o23-queue"
                RefreshListO23()
        End Select


        Me.HardRefreshFlag.Value = ""
        Me.HardRefreshPID.Value = ""
    End Sub

    Private Sub RefreshListO23()
        Dim mq As New BO.myQueryO23
        mq.o23GUID = Me.DocGUID
        mq.p31ID = Master.DataPID
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mq)
        notepad1.RefreshData(lisO23, Master.DataPID)
    End Sub

    Private Sub p32ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p32ID.NeedMissingItem
        If Master.DataPID = 0 Then Return
        Dim cRec As BO.p32Activity = Master.Factory.p32ActivityBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then
            strAddMissingItemText = cRec.p32Name
        End If
    End Sub

    Private Sub j27ID_Orig_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles j27ID_Orig.NeedMissingItem
        Dim cRec As BO.j27Currency = Master.Factory.ftBL.LoadJ27(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then
            strAddMissingItemText = cRec.j27Code
        End If
    End Sub

    Private Sub j02ID_AutoPostBack_SelectedIndexChanged(NewValue As String, OldValue As String) Handles j02ID.AutoPostBack_SelectedIndexChanged
        Me.CurrentJ02ID = BO.BAS.IsNullInt(Me.j02ID.Value)
        Handle_ChangeJ02()
    End Sub

    Private Sub Handle_ChangeJ02()
       

    End Sub

    Private Sub j27ID_Orig_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles j27ID_Orig.SelectedIndexChanged
        SetupVatRateCombo()
    End Sub

    Private Sub SetupVatRateCombo()

        Dim intJ27ID As Integer = BO.BAS.IsNullInt(Me.j27ID_Orig.SelectedValue)
        Dim lis As IEnumerable(Of BO.p53VatRate) = Master.Factory.p53VatRateBL.GetList(New BO.myQuery).Where(Function(p) p.j27ID = intJ27ID)
        Me.p31VatRate_Orig.DataSource = lis
        Me.p31VatRate_Orig.DataBind()
    End Sub

    Private Sub p31_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.p56ID.Visible = Me.chkBindToP56.Checked
        ''imgTask.Visible = Me.p56ID.Visible
    End Sub

    Private Sub chkBindToP56_CheckedChanged(sender As Object, e As EventArgs) Handles chkBindToP56.CheckedChanged
        If Me.chkBindToP56.Checked Then
            SetupP56Combo()
            If Me.p56ID.Rows <= 1 Then
                Master.Notify("Pro vybranou osobu a projekt není dostupný ani jeden otevřený úkol k vykazování.", NotifyLevel.WarningMessage)
            End If
        End If
    End Sub

    Private Sub SetupP56Combo()
        If Me.CurrentP41ID = 0 Then
            Master.Notify("Seznam úkolů je možné zobrazit až po výběru projektu.", NotifyLevel.WarningMessage)
            Return
        End If
        Dim mq As New BO.myQueryP56
        mq.p41ID = Me.CurrentP41ID
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        mq.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForWorksheetEntry
        mq.j02ID_ExplicitQueryFor = Me.CurrentJ02ID

        Me.p56ID.DataSource = Master.Factory.p56TaskBL.GetList(mq)
        Me.p56ID.DataBind()

    End Sub

    Private Sub p56ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p56ID.NeedMissingItem
        Dim cRec As BO.p56Task = Master.Factory.p56TaskBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then
            strAddMissingItemText = cRec.p56Name
        End If
    End Sub

    Private Sub InhaleWorksheetSetting()
        Dim lisPars As New List(Of String)
        lisPars.Add("p31_default_HoursEntryFlag")
        lisPars.Add("p31_HoursInputInterval")
        lisPars.Add("p31_HoursInputFormat")
        lisPars.Add("p31_TimeInputInterval")
        lisPars.Add("p31_TimeInput_Start")
        lisPars.Add("p31_TimeInput_End")

        With Master.Factory.j03UserBL
            .InhaleUserParams(lisPars)
            Me.p31_default_HoursEntryFlag.Value = .GetUserParam("p31_default_HoursEntryFlag", "1")
            Dim intStart As Integer = CInt(.GetUserParam("p31_HoursInputInterval", "30"))
            Dim s As String = "", cT As New BO.clsTime, strFormat As String = .GetUserParam("p31_HoursInputFormat", "dec")
            Dim intKratHodin As Integer = 4
            If intStart = 60 Then intKratHodin = 8
            If intStart = 30 Then intKratHodin = 6
            If intStart = 5 Or intStart = 10 Then intKratHodin = 3
            For i As Integer = intStart To intKratHodin * 60 Step intStart
                If i > intStart Then
                    s += ","
                End If
                If strFormat = "hhmm" Then
                    s += "'" & cT.ShowAsHHMM((CDbl(i) / 60).ToString) & "'"
                Else
                    s += "'" & (CDbl(i) / 60).ToString & "'"
                End If
            Next
            ViewState("hours_offer") = s
            'nabídka času od-do
            intStart = CInt(.GetUserParam("p31_TimeInput_Start", "8")) : s = ""
            Dim intEnd As Integer = CInt(.GetUserParam("p31_TimeInput_End", "19"))
            Dim intInterval As Integer = CInt(.GetUserParam("p31_TimeInputInterval", "30"))
            For i As Integer = intStart To intEnd
                For j As Integer = 0 To 59 Step intInterval
                    If s <> "" Then s += ","
                    s += "'" & Right("0" & i.ToString, 2) & ":" & Right("0" & j.ToString, 2) & "'"
                    If i = intEnd Then Exit For
                Next
            Next
            ViewState("times_offer") = s
        End With
    End Sub
End Class