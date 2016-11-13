Public Class mobile_p31_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile
   
    Public Enum ProjectListStructure
        FlatList = 1
        GroupByClient = 2
    End Enum
    Public Enum ProjectMask
        NameOnly = 1
        NameAndCode = 2
        NameAndType = 3
        NameAndCenter = 4
    End Enum

    Private _lastP28ID As Integer
    Private _lastTaskProject As Integer


    Private ReadOnly Property _pls As ProjectListStructure
        Get

            Return CType(Me.cbxPLS.SelectedValue, ProjectListStructure)
        End Get
    End Property
    Private ReadOnly Property _mask As ProjectMask
        Get
            If Me.hidMask.Value = "" Then Me.hidMask.Value = "1"
            Return CType(Me.hidMask.Value, ProjectMask)
        End Get
    End Property
    Public Property CurrentTab As String
        Get
            Return Me.hidTab.Value
        End Get
        Set(value As String)
            Me.hidTab.Value = value
        End Set
    End Property
    Public Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP41ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP41ID.Value = value.ToString
            If value <> 0 Then HighlightSelectedP41ID(value)
        End Set
    End Property
    Public Property CurrentP56ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP56ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP56ID.Value = value.ToString
            HighlightSelectdP56ID(value)
        End Set
    End Property
    Public Property CurrentP31ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP31ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP31ID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentP34ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p34ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.p34ID, value.ToString)
        End Set
    End Property
    Public Property CurrentP33ID As BO.p33IdENUM
        Get
            If BO.BAS.IsNullInt(Me.hidP33ID.Value) = 0 Then
                Return BO.p33IdENUM.Cas
            Else
                Return CType(Me.hidP33ID.Value, BO.p33IdENUM)
            End If
        End Get
        Set(value As BO.p33IdENUM)
            hidP33ID.Value = CInt(value).ToString
        End Set
    End Property
    Public Property IsDirectCallP41ID As Boolean
        Get
            Return BO.BAS.BG(Me.hidDirectCallP41ID.Value)
        End Get
        Set(value As Boolean)
            hidDirectCallP41ID.Value = BO.BAS.GB(value)
        End Set
    End Property

    Private Sub mobile_p31_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then            
            Master.MenuPrefix = "p31"
            hidAllowRates.Value = BO.BAS.GB(Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates))
            If Request.Item("source") <> "" Then
                If Not Request.UrlReferrer Is Nothing Then Me.hidRef.Value = Request.UrlReferrer.PathAndQuery
            End If
            Me.p31Date.Text = Format(Now, "dd.MM.yyyy")
            If Request.Item("defdate") <> "" Then
                Me.p31Date.Text = Request.Item("defdate")
            End If
            Dim pars As New List(Of String)
            With pars
                .Add("mobile_p31_framework-tab")
                .Add("mobile-top10")
                .Add("mobile-pls")
                .Add("mobile-projectmask")
                .Add("mobile_p31_framework-p41id")
                .Add("mobile_p31_framework-p56id")
                .Add("mobile-daysquerybefore")
            End With

            With Master.Factory.j03UserBL
                .InhaleUserParams(pars)
                ''If .GetUserParam("mobile-top10", "0") = "1" Then
                ''    Me.chkShowTop10.Checked = True
                ''End If
                Me.CurrentTab = .GetUserParam("mobile_p31_framework-tab", "p41")
                Me.hidMask.Value = .GetUserParam("mobile-projectmask", "1")
                basUI.SelectDropdownlistValue(Me.cbxPLS, .GetUserParam("mobile-pls", "2"))


                Me.hidDaysQueryBefore.Value = .GetUserParam("mobile-daysquerybefore", "10")
            End With
            If BO.BAS.IsNullInt(Request.Item("p41id")) <> 0 Then
                Me.CurrentTab = "p41"
                Me.IsDirectCallP41ID = True
                Me.CurrentP41ID = BO.BAS.IsNullInt(Request.Item("p41id"))
            End If
            If BO.BAS.IsNullInt(Request.Item("p56id")) <> 0 Then
                Me.CurrentTab = "p41"
                Me.CurrentP56ID = BO.BAS.IsNullInt(Request.Item("p56id"))
                Dim cTask As BO.p56Task = Master.Factory.p56TaskBL.Load(Me.CurrentP56ID)
                Me.CurrentP41ID = cTask.p41ID
                Me.IsDirectCallP41ID = True
            End If
            If Me.CurrentP41ID = 0 And Me.CurrentP56ID = 0 Then
                With Master.Factory.j03UserBL
                    If Me.CurrentTab = "p41" Then
                        Me.CurrentP41ID = .GetUserParam("mobile_p31_framework-p41id", "0")
                    End If
                    If Me.CurrentTab = "p56" Then
                        Me.CurrentP56ID = .GetUserParam("mobile_p31_framework-p56id", "0")
                    End If
                End With
            End If

            SetupProjectList()
            
            If Request.Item("pid") = "" Then
                If Me.CurrentP56ID <> 0 Then
                    Handle_ChangeP56()
                Else
                    If Me.CurrentP41ID <> 0 Then
                        Handle_ChangeP41()
                    End If
                End If
            Else
                RefreshRecord(BO.BAS.IsNullInt(Request.Item("pid")))
            End If

            If hidVirtualCountP56.Value = "0" Or Me.IsDirectCallP41ID Then
                Me.CurrentTab = "p41"
                tabs1.Style.Item("display") = "none"
            Else
                tabs1.Style.Item("display") = "block"
            End If


            RefreshP31List()
        End If
    End Sub


    Private Sub SetupProjectList()
        If IsDirectCallP41ID Then Return

        Dim mqP41 As New BO.myQueryP41
        With mqP41
            .Closed = BO.BooleanQueryMode.FalseQuery
            .SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
        End With
        Dim intVirtualCount As Integer = Master.Factory.p41ProjectBL.GetVirtualCount(mqP41)
        If intVirtualCount < 100 Then
            lblVybratProjekt.Text = "Nabídka projektů (" & intVirtualCount.ToString & ")"
            Me.linkTabP41.Text = "Vybrat projekt <span class='badge'>" & intVirtualCount.ToString & "</span>"
        Else
            'je třeba nabízet pouze TOP 100
            mqP41.PIDs = Master.Factory.p41ProjectBL.GetTopProjectsByWorksheetEntry(Master.Factory.SysUser.j02ID, 100)
            If mqP41.PIDs.Count = 0 Then    'zatím nenapsal žádné úkony
                mqP41.TopRecordsOnly = 100
            End If
            lblVybratProjekt.Text = "Nabídka projektů (TOP 100)"
            Me.linkTabP41.Text = "Vybrat projekt <span class='badge'>TOP 100</span>"
        End If
        Dim lisP41 As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mqP41)

        If Me.CurrentTab = "p41" Then
            rpP41_cbx.DataSource = lisP41
            rpP41_cbx.DataBind()
            If Me.CurrentP41ID <> 0 Then HighlightSelectedP41ID(Me.CurrentP41ID)
        End If
        Dim mqP56 As New BO.myQueryP56
        'mqP56.j02ID = Master.Factory.SysUser.j02ID
        'mqP56.Closed = BO.BooleanQueryMode.FalseQuery
        mqP56.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForWorksheetEntry
        Dim lisP56 As IEnumerable(Of BO.p56Task) = Master.Factory.p56TaskBL.GetList(mqP56).OrderBy(Function(p) p.Client)
        hidVirtualCountP56.Value = lisP56.Count.ToString

        lblVybratUkol.Text = "Nabídka úkolů (" & lisP56.Count.ToString & ")"
        Me.linkTabP56.Text = "Vybrat úkol <span class='badge'>" & lisP56.Count.ToString & "</span>"

        If Me.CurrentTab = "p56" Then
            rpP56_cbx.DataSource = lisP56
            rpP56_cbx.DataBind()
            If Me.CurrentP56ID <> 0 Then HighlightSelectdP56ID(Me.CurrentP56ID)
        End If

        
    End Sub

    
    Private Sub rpP41_cbx_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP41_cbx.ItemDataBound
        Dim cRec As BO.p41Project = CType(e.Item.DataItem, BO.p41Project)
        CType(e.Item.FindControl("pid"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("linkClient"), HyperLink)
            If _pls = ProjectListStructure.GroupByClient And cRec.p28ID_Client > 0 Then
                If _lastP28ID <> cRec.p28ID_Client Then
                    .Text = cRec.Client
                    _lastP28ID = cRec.p28ID_Client
                    .Visible = True
                End If
            End If
        End With
        With CType(e.Item.FindControl("link1"), HyperLink)
            Dim strMask As String = cRec.p41Name
            If cRec.p41NameShort <> "" Then strMask = cRec.p41NameShort
            Select Case _mask
                Case ProjectMask.NameAndCode
                    strMask = strMask & " (" & cRec.p41Code & ")"
                Case ProjectMask.NameAndType
                    strMask = strMask & " (" & cRec.p42Name & ")"
                Case ProjectMask.NameAndCenter
                    If cRec.j18ID > 0 Then strMask = strMask & " (" & cRec.j18Name & ")"
            End Select

            If _pls = ProjectListStructure.GroupByClient Then
                .Text = strMask
            Else
                If cRec.p28ID_Client > 0 Then
                    .Text = cRec.Client
                End If
                .Text += "<span class='badge1'>" & strMask & "</span>"
            End If


            .NavigateUrl = "javascript:hardrefresh('p41id','" & cRec.PID.ToString & "')"
            If cRec.PID = Me.CurrentP41ID Then
                'označit řádek
            End If
        End With
        _lastP28ID = cRec.p28ID_Client
    End Sub


    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        SW("")
        Dim strVal As String = Me.HardRefreshValue.Value
        Select Case Me.HardRefreshFlag.Value
            Case "daysquerybefore", "save", "saveandcopy", "delete"
            Case Else
                Me.IsDirectCallP41ID = False
        End Select


        Select Case Me.HardRefreshFlag.Value
            Case "tab"
                Master.Factory.j03UserBL.SetUserParam("mobile_p31_framework-tab", strVal)
                Me.CurrentTab = strVal
                SetupProjectList()

            Case "p41id"
                Master.Factory.j03UserBL.SetUserParam("mobile_p31_framework-p41id", strVal)
                Me.CurrentP41ID = BO.BAS.IsNullInt(strVal)
                Me.CurrentP56ID = 0
                Master.Factory.j03UserBL.SetUserParam("mobile-p41id", strVal)
                Dim cRecP31 As BO.p31Worksheet = Nothing
                If Me.CurrentP31ID <> 0 Then
                    cRecP31 = Master.Factory.p31WorksheetBL.Load(Me.CurrentP31ID)
                End If
                Handle_ChangeP41(cRecP31)

                ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
            Case "p56id"
                Master.Factory.j03UserBL.SetUserParam("mobile_p31_framework-p56id", strVal)
                Me.CurrentP56ID = BO.BAS.IsNullInt(strVal)
                Dim cRecP31 As BO.p31Worksheet = Nothing
                If Me.CurrentP31ID <> 0 Then
                    cRecP31 = Master.Factory.p31WorksheetBL.Load(Me.CurrentP31ID)
                End If
                Handle_ChangeP56(cRecP31)
                ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
            
                
            Case "daysquerybefore"
                Me.hidDaysQueryBefore.Value = strVal
                Master.Factory.j03UserBL.SetUserParam("mobile-daysquerybefore", strVal)
                RefreshP31List()
                ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_list';", True)
            Case "mask"
                Me.hidMask.Value = strVal
                Master.Factory.j03UserBL.SetUserParam("mobile-projectmask", strVal)
                SetupProjectList()
            Case "save"
                If SaveChanges() Then
                    TestIfRedirect()
                    Me.IsDirectCallP41ID = False
                    RefreshRecord(Me.CurrentP31ID)
                    RefreshRecord(0)
                    RefreshP31List()
                End If
            Case "saveandcopy"
                If SaveChanges() Then
                    Me.IsDirectCallP41ID = False
                    RefreshRecord(Me.CurrentP31ID)
                    Me.CurrentP31ID = 0
                    RefreshP31List()
                End If
            Case "clear"
                If Me.CurrentP31ID <> 0 Then RefreshRecord(Me.CurrentP31ID)
                RefreshRecord(0)
            Case "edit"
                RefreshRecord(BO.BAS.IsNullInt(strVal))
                ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
            Case "delete"
                If Master.Factory.p31WorksheetBL.Delete(Me.CurrentP31ID) Then
                    TestIfRedirect()
                    Me.IsDirectCallP41ID = False
                    RefreshRecord(0)
                    RefreshP31List()
                    ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
                Else
                    SW(Master.Factory.p31WorksheetBL.ErrorMessage)
                End If

        End Select

        Me.HardRefreshFlag.Value = ""
    End Sub

    Private Sub SW(strMessage As String, Optional x As Integer = 0)
        If strMessage <> "" Then
            Me.panMessage.Visible = True
            Me.WarningMessage.Text = strMessage
            ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_message';", True)
        Else
            Me.panMessage.Visible = False
            Me.WarningMessage.Text = ""
        End If

    End Sub

    Private Sub Handle_ChangeP34()
        panT.Visible = False : panU.Visible = False : panM.Visible = False
        If Me.CurrentP34ID = 0 Then
            SW("V projektu nemáte přístup k worksheet sešitu.") : Return
        End If

        Dim mq As New BO.myQueryP32
        mq.p34ID = Me.CurrentP34ID
        Me.p32ID.DataSource = Master.Factory.p32ActivityBL.GetList(mq)
        Me.p32ID.DataBind()
        Me.p32ID.Items.Insert(0, "--Aktivita úkonu--")

        Dim cRec As BO.p34ActivityGroup = Master.Factory.p34ActivityGroupBL.Load(Me.CurrentP34ID)
        Me.p34Name.Text = cRec.p34Name
        Me.CurrentP33ID = cRec.p33ID
        Select Case cRec.p33ID
            Case BO.p33IdENUM.Cas
                panT.Visible = True
            Case BO.p33IdENUM.Kusovnik
                panU.Visible = True
            Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                panM.Visible = True
                Dim b As Boolean = True
                If cRec.p33ID = BO.p33IdENUM.PenizeBezDPH Then
                    b = False
                End If
                Me.p31Amount_WithVat_Orig.Visible = b : Me.lblp31Amount_WithVat_Orig.Visible = b
                Me.p31Amount_Vat_Orig.Visible = b ': Me.lblp31Amount_Vat_Orig.Visible = b
                Me.p31VatRate_Orig.Visible = b : Me.lblp31VatRate_Orig.Visible = b

                If Me.j27ID_Orig.Items.Count = 0 Then
                    Me.j27ID_Orig.DataSource = Master.Factory.ftBL.GetList_J27(New BO.myQuery)
                    Me.j27ID_Orig.DataBind()

                End If
                SetupVatRateCombo()
        End Select
        If cRec.p34ActivityEntryFlag = BO.p34ActivityEntryFlagENUM.AktivitaSeNezadava Then
            'aktivita se ručně nezadává
            Me.p32ID.Visible = False
        Else
            Me.p32ID.Visible = True
        End If
    End Sub
    Private Sub SetupVatRateCombo()

        Dim intJ27ID As Integer = BO.BAS.IsNullInt(Me.j27ID_Orig.SelectedValue)
        Dim lis As IEnumerable(Of BO.p53VatRate) = Master.Factory.p53VatRateBL.GetList(New BO.myQuery).Where(Function(p) p.j27ID = intJ27ID)
        Me.p31VatRate_Orig.DataSource = lis
        Me.p31VatRate_Orig.DataBind()
    End Sub

    Private Sub Handle_ChangeP41(Optional cRecP31 As BO.p31Worksheet = Nothing)
        If Me.CurrentP41ID = 0 Then
            Return
        End If
        Dim cRecP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(Me.CurrentP41ID)
        Me.Project.Text = cRecP41.PrefferedName
        Me.Project.NavigateUrl = "mobile_p41_framework.aspx?pid=" & Me.CurrentP41ID.ToString

        If cRecP41.p28ID_Client <> 0 Then
            Me.Client.Text = cRecP41.Client
            Me.Client.NavigateUrl = "mobile_p28_framework.aspx?pid=" & cRecP41.p28ID_Client.ToString
        End If

        Me.p34ID.DataSource = Master.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(Me.CurrentP41ID, cRecP41.p42ID, cRecP41.j18ID, Master.Factory.SysUser.j02ID)
        Me.p34ID.DataBind()

        If Not cRecP31 Is Nothing Then
            basUI.SelectDropdownlistValue(Me.p34ID, cRecP31.p34ID.ToString)
        End If
        Handle_ChangeP34()
        If Not cRecP31 Is Nothing Then
            basUI.SelectDropdownlistValue(Me.p32ID, cRecP31.p32ID.ToString)
        End If
        Dim lisP30 As IEnumerable(Of BO.p30Contact_Person) = Master.Factory.p30Contact_PersonBL.GetList(cRecP41.p28ID_Client, cRecP41.PID, 0)
        If lisP30.Count > 0 Then
            lisP30 = lisP30.Where(Function(p) p.p30IsDefaultInWorksheet = True)
            Dim intDefj02ID As Integer
            If lisP30.Count > 0 Then
                intDefj02ID = lisP30.First.j02ID
                If lisP30.Where(Function(p) p.p41ID = cRecP41.PID).Count > 0 Then
                    intDefj02ID = lisP30.Where(Function(p) p.p41ID = cRecP41.PID)(0).j02ID
                End If
            End If
            RefreshContactPersonCombo(cRecP41, intDefj02ID)
        Else
            Me.j02ID_ContactPerson.Visible = False
        End If
        SetupP56Combo()
    End Sub

    Private Sub HighlightSelectedP41ID(intP41ID As Integer)
        For Each ri As RepeaterItem In rpP41_cbx.Items
            With CType(ri.FindControl("li1"), HtmlGenericControl)
                .Attributes.Item("class") = ""
                If intP41ID.ToString = CType(ri.FindControl("pid"), HiddenField).Value Then
                    .Attributes.Item("class") = "active"
                End If
            End With
        Next
    End Sub
    Private Sub HighlightSelectdP56ID(intP56ID As Integer)
        For Each ri As RepeaterItem In rpP56_cbx.Items
            With CType(ri.FindControl("li1"), HtmlGenericControl)
                .Attributes.Item("class") = ""
                If intP56ID.ToString = CType(ri.FindControl("pid"), HiddenField).Value Then
                    .Attributes.Item("class") = "active"
                End If
            End With
        Next
    End Sub
    Private Sub Handle_ChangeP56(Optional cRecP31 As BO.p31Worksheet = Nothing)
        If Me.CurrentP56ID = 0 Then
            Return
        End If
        Dim cRec As BO.p56Task = Master.Factory.p56TaskBL.Load(Me.CurrentP56ID)
        Me.CurrentP41ID = cRec.p41ID
        Handle_ChangeP41(cRecP31)
        RenderTaskInfo(cRec)
    End Sub
    Private Sub RenderTaskInfo(cRec As BO.p56Task)
        Me.Task.Text = cRec.p57Name & ": " & cRec.p56Name
        Me.Task.NavigateUrl = "mobile_p56_framework.aspx?pid=" & cRec.PID.ToString
    End Sub

    Private Sub mobile_p31_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.CurrentP31ID = 0 Then
            Me.lblRecordHeader.Text = "Zápis nového úkonu"
            imgHeader.ImageUrl = "Images/new.png"
            Me.cmdDelete.Visible = False
        Else
            imgHeader.ImageUrl = "Images/fe.png"
            Me.lblRecordHeader.Text = "Úprava vybraného úkonu"
            Me.cmdDelete.Visible = True
        End If
        If Me.IsDirectCallP41ID Then
            panDropdownSelectP41ID.Visible = False
            panDropdownSelectP56ID.Visible = False
        Else
            panDropdownSelectP41ID.Visible = True
            If rpP41_cbx.Items.Count = 0 Then
                SetupProjectList()
            End If
        End If
        panDropdownSelectP41ID.Visible = False : panDropdownSelectP56ID.Visible = False
        Select Case Me.CurrentTab
            Case "p41"
                panDropdownSelectP41ID.Visible = True
            Case "p56"
                panDropdownSelectP56ID.Visible = True
        End Select
        If hidAllowRates.Value = "0" Then
            'osoba nemá právo vidět sazby
            Me.Client.NavigateUrl = "" : Me.Client.Enabled = False
            Me.Project.NavigateUrl = "" : Me.Project.Enabled = False
        End If
    End Sub

    Private Sub RefreshRecord(intP31ID As Integer)
        Me.CurrentP31ID = intP31ID

        Dim cRec As BO.p31Worksheet = Nothing
        If intP31ID <> 0 Then
            cRec = Master.Factory.p31WorksheetBL.Load(intP31ID)
            If cRec Is Nothing Then
                SW("record not found!") : Return
            End If
            Me.CurrentP41ID = cRec.p41ID
            Me.CurrentP56ID = cRec.p56ID
            Me.TimeStamp.Text = cRec.Timestamp

            Select Case cRec.p33ID
                Case BO.p33IdENUM.Cas
                    If cRec.IsRecommendedHHMM Then
                        Me.p31Hours_Orig.Text = cRec.p31HHMM_Orig.ToString
                    Else
                        Me.p31Hours_Orig.Text = cRec.p31Value_Orig.ToString
                    End If
                Case BO.p33IdENUM.Kusovnik
                    Me.p31Value_Orig.Text = cRec.p31Value_Orig.ToString
                Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                    Me.p31Amount_WithoutVat_Orig.Text = cRec.p31Amount_WithoutVat_Orig.ToString
                    If cRec.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                        Me.p31Amount_WithVat_Orig.Text = cRec.p31Amount_WithVat_Orig.ToString
                        Me.p31Amount_Vat_Orig.Text = cRec.p31Amount_Vat_Orig.ToString
                    End If
                    basUI.SelectDropdownlistValue(Me.j27ID_Orig, cRec.j27ID_Billing_Orig.ToString)
                    basUI.SelectDropdownlistValue(Me.p31VatRate_Orig, CInt(cRec.p31VatRate_Orig).ToString)
            End Select

            Me.p31Text.Text = cRec.p31Text
            Me.p31Date.Text = Format(cRec.p31Date, "dd.MM.yyyy")
            If cRec.p56ID <> 0 Then
                Dim cRecP56 As BO.p56Task = Master.Factory.p56TaskBL.Load(Me.CurrentP56ID)
                RenderTaskInfo(cRecP56)

            End If

            If cRec.p71ID > BO.p71IdENUM.Nic Then
                SW("Úkon již prošel schvalováním.")

            End If
            If cRec.p91ID > 0 Then
                SW("Tento úkon již prošel fakturací.")
            End If
            
        Else
            Me.p31Text.Text = ""
            Me.p31Value_Orig.Text = ""
            Me.p31Hours_Orig.Text = ""
            ''Me.p31Date.Text = Format(Now, "dd.MM.yyyy")

            p31Amount_WithVat_Orig.Text = "" : p31Amount_Vat_Orig.Text = "" : p31Amount_WithoutVat_Orig.Text = ""
            Me.TimeStamp.Text = ""
        End If

        If intP31ID <> 0 Then
            If Me.CurrentP56ID <> 0 Then
                Handle_ChangeP56(cRec)
            Else
                Handle_ChangeP41(cRec)
            End If
            If cRec.j02ID_ContactPerson <> 0 Then
                basUI.SelectDropdownlistValue(Me.j02ID_ContactPerson, cRec.j02ID_ContactPerson.ToString)
            End If
            If cRec.p56ID <> 0 Then
                basUI.SelectDropdownlistValue(Me.p56ID, cRec.p56ID.ToString)
            End If
        End If
        


    End Sub

    Private Sub p34ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles p34ID.SelectedIndexChanged
        Handle_ChangeP34()
        ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_p32id';", True)
    End Sub
    Private Sub p32ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles p32ID.SelectedIndexChanged
        ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_p32id';", True)
    End Sub

    Private Sub rpP56_cbx_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP56_cbx.ItemDataBound
        Dim cRec As BO.p56Task = CType(e.Item.DataItem, BO.p56Task)
        CType(e.Item.FindControl("pid"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("linkProject"), HyperLink)
            If _lastTaskProject <> cRec.p41ID Then
                If cRec.Client <> "" Then
                    .Text = cRec.Client & "<span class='badge1'>" & cRec.p41Name & "</span>"
                Else
                    .Text = cRec.p41Name
                End If
                .Visible = True
            End If
        End With
        With CType(e.Item.FindControl("link1"), HyperLink)
            .Text = cRec.p57Name & ": " & BO.BAS.OM3(cRec.p56Name, 70)
            If Not cRec.p56PlanUntil Is Nothing Then
                .Text += " (" & BO.BAS.FD(cRec.p56PlanUntil, True) & ")"
            End If


            .NavigateUrl = "javascript:hardrefresh('p56id','" & cRec.PID.ToString & "')"
            If cRec.PID = Me.CurrentP41ID Then
                'označit řádek
            End If
        End With

        _lastTaskProject = cRec.p41ID
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("mobile_p31_framework.aspx?" & basUI.GetCompleteQuerystring(Request), True)
    End Sub

    Private Sub RefreshContactPersonCombo(cP41 As BO.p41Project, intDefJ02ID As Integer)
        Me.j02ID_ContactPerson.Visible = False

        If Me.CurrentP41ID = 0 Then Return

        Dim mq As New BO.myQueryJ02
        mq.IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified
        mq.p41ID = cP41.PID

        Dim lisJ02 As List(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList(mq).ToList
        If lisJ02.Count = 0 And cP41.p28ID_Client <> 0 Then
            mq.p41ID = 0 : mq.p28ID = cP41.p28ID_Client
        End If
        If intDefJ02ID > 0 And lisJ02.Where(Function(p) p.PID = intDefJ02ID).Count = 0 Then
            Dim c As BO.j02Person = Master.Factory.j02PersonBL.Load(intDefJ02ID)
            If Not c Is Nothing Then lisJ02.Add(c)
        End If
        If lisJ02.Count > 0 Then
            Me.j02ID_ContactPerson.DataSource = lisJ02
            Me.j02ID_ContactPerson.DataBind()
            Me.j02ID_ContactPerson.Items.Insert(0, "--Kontaktní osoba--")
            If intDefJ02ID > 0 Then basUI.SelectDropdownlistValue(Me.j02ID_ContactPerson, intDefJ02ID.ToString)
            Me.j02ID_ContactPerson.Visible = True
        End If
    End Sub

    Private Sub SetupP56Combo()
        Me.p56ID.Visible = False
        Dim mq As New BO.myQueryP56
        mq.p41ID = Me.CurrentP41ID
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        mq.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForWorksheetEntry
        mq.j02ID_ExplicitQueryFor = Master.Factory.SysUser.j02ID

        Me.p56ID.DataSource = Master.Factory.p56TaskBL.GetList(mq)
        Me.p56ID.DataBind()
        If Me.p56ID.Items.Count > 0 Then
            Me.p56ID.Visible = True
            Me.p56ID.Items.Insert(0, "--Úkol v rámci projektu--")
            If Me.CurrentP56ID <> 0 Then
                basUI.SelectDropdownlistValue(Me.p56ID, Me.CurrentP56ID.ToString)
            End If
        End If
       
        
    End Sub

    Private Function SaveChanges() As Boolean
        If Me.CurrentP31ID <> 0 Then
            Dim c As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Me.CurrentP31ID)
            If c.p71ID > BO.p71IdENUM.Nic Or c.p91ID <> 0 Then
                SW("Záznam nelze uložit, protože již prošel schvalováním.") : Return False
            End If
        End If
        With Master.Factory.p31WorksheetBL
            Dim cRec As New BO.p31WorksheetEntryInput()
            With cRec
                .SetPID(Me.CurrentP31ID)
                .j02ID = Master.Factory.SysUser.j02ID
                .p41ID = Me.CurrentP41ID
                If Me.CurrentTab = "p56" Then
                    .p56ID = Me.CurrentP56ID
                Else
                    .p56ID = BO.BAS.IsNullInt(Me.p56ID.SelectedValue)
                End If
                .p34ID = Me.CurrentP34ID
                .p32ID = BO.BAS.IsNullInt(Me.p32ID.SelectedValue)
                .j02ID_ContactPerson = BO.BAS.IsNullInt(Me.j02ID_ContactPerson.SelectedValue)
                .p31Date = BO.BAS.ConvertString2Date(Me.p31Date.Text)
                .p31Text = Me.p31Text.Text

                Select Case Me.CurrentP33ID
                    Case BO.p33IdENUM.Cas
                        .p31HoursEntryflag = BO.p31HoursEntryFlagENUM.Hodiny
                        .Value_Orig = Me.p31Hours_Orig.Text
                        .Value_Orig_Entried = .Value_Orig

                        If Not .ValidateEntryTime(5) Then

                            SW(.ErrorMessage, 2)
                            Return False
                        End If
                    Case BO.p33IdENUM.Kusovnik
                        .Value_Orig = BO.BAS.IsNullNum(Me.p31Value_Orig.Text)
                        .Value_Orig_Entried = .Value_Orig
                        If Not .ValidateEntryKusovnik() Then
                            SW(.ErrorMessage, 2)
                            Return False
                        End If
                    Case BO.p33IdENUM.PenizeBezDPH
                        .Amount_WithoutVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithoutVat_Orig.Text)
                        .Value_Orig_Entried = .Amount_WithoutVat_Orig.ToString
                        .j27ID_Billing_Orig = BO.BAS.IsNullInt(Me.j27ID_Orig.SelectedValue)
                    Case BO.p33IdENUM.PenizeVcDPHRozpisu
                        .Amount_WithoutVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithoutVat_Orig.Text)
                        .Value_Orig_Entried = .Amount_WithoutVat_Orig.ToString
                        .VatRate_Orig = BO.BAS.IsNullNum(Me.p31VatRate_Orig.SelectedItem.Text)
                        .j27ID_Billing_Orig = BO.BAS.IsNullInt(Me.j27ID_Orig.SelectedValue)
                        .Amount_WithVat_Orig = BO.BAS.IsNullNum(Me.p31Amount_WithVat_Orig.Text)
                        .Amount_Vat_Orig = BO.BAS.IsNullNum(Me.p31Amount_Vat_Orig.Text)
                End Select

            End With
            If .SaveOrigRecord(cRec, Nothing) Then
                Me.CurrentP31ID = .LastSavedPID
                Return True
            Else
                SW(.ErrorMessage, 2)
                Return False
            End If

        End With

    End Function


    Private Sub RefreshP31List()
        Dim mq As New BO.myQueryP31
        mq.j02ID = Master.Factory.SysUser.j02ID
        mq.DateFrom = Today.AddDays(-1 * BO.BAS.IsNullInt(Me.hidDaysQueryBefore.Value))
        mq.MG_SortString = "p31Date DESC"

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        Dim strHeader As String = "Úkony od " & BO.BAS.FD(mq.DateFrom, False, True) & " | " & lis.Count.ToString & "x"
        list1.RefreshData(lis, strHeader)
    End Sub

    

    Private Sub cbxPLS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPLS.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("mobile-pls", Me.cbxPLS.SelectedValue)
        SetupProjectList()
    End Sub

    Private Sub TestIfRedirect()
        If Me.hidRef.Value <> "" Then
            
            Response.Redirect(Me.hidRef.Value)
        End If
    End Sub
End Class