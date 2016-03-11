Public Class p31_framework_mobile
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
    Private _lastP31Date As Date? = Nothing

    Private ReadOnly Property _pls As ProjectListStructure
        Get
            If Me.hidPLS.Value = "" Then Me.hidPLS.Value = "1"
            Return CType(Me.hidPLS.Value, ProjectListStructure)
        End Get
    End Property
    Private ReadOnly Property _mask As ProjectMask
        Get
            If Me.hidMask.Value = "" Then Me.hidMask.Value = "1"
            Return CType(Me.hidMask.Value, ProjectMask)
        End Get
    End Property
    ''Public Property CurrentPID As Integer
    ''    Get
    ''        Return BO.BAS.IsNullInt(Me.hidPID.Value)
    ''    End Get
    ''    Set(value As Integer)
    ''        Me.hidPID.Value = value.ToString
    ''    End Set
    ''End Property
    Public Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP41ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP41ID.Value = value.ToString
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
    Public Property CurrentP56ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP56ID.Value)
        End Get
        Set(value As Integer)
            Me.hidP56ID.Value = value.ToString
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



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.MenuPrefix = "p31"
            Me.p31Date.Text = Format(Now, "dd.MM.yyyy")

            Dim pars As New List(Of String)
            With pars
                .Add("mobile-top10")
                .Add("mobile-pls")
                .Add("mobile-projectmask")
                .Add("mobile-p41id")
                .Add("mobile-daysquerybefore")
            End With

            With Master.Factory.j03UserBL
                .InhaleUserParams(pars)
                If .GetUserParam("mobile-top10", "0") = "1" Then
                    Me.chkShowTop10.Checked = True
                End If
                Me.hidMask.Value = .GetUserParam("mobile-projectmask", "1")
                Me.hidPLS.Value = .GetUserParam("mobile-pls", "1")

                Me.hidDaysQueryBefore.Value = .GetUserParam("mobile-daysquerybefore", "10")
                If BO.BAS.IsNullInt(Request.Item("p41id")) <> 0 Then
                    Me.IsDirectCallP41ID = True
                    Me.CurrentP41ID = BO.BAS.IsNullInt(Request.Item("p41id"))
                End If
            End With
            

           
            SetupProjectList()
            If Me.CurrentP41ID = 0 Then
                Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.LoadMyLastCreated_TimeRecord()
                If Not cRec Is Nothing Then
                    RefreshRecord(cRec.PID)
                End If
            End If
            
            RefreshRecord(0)
            If Me.CurrentP41ID <> 0 Then SelectP41ID(Me.CurrentP41ID)
            RefreshP31List()
        End If
    End Sub

    Private Sub SetupProjectList()
        If Me.IsDirectCallP41ID Then Return

        Dim mqP41 As New BO.myQueryP41
        With mqP41
            .Closed = BO.BooleanQueryMode.FalseQuery
            .SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
            'If Me.CurrentJ02ID <> Master.Factory.SysUser.j02ID Then .j02ID_ExplicitQueryFor = Me.CurrentJ02ID
        End With
        Dim lisP41 As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mqP41)
        Dim lisP41_TOP10 As IEnumerable(Of BO.p41Project) = Nothing

        lblVybratProjekt.Text = "Vybrat projekt (" & lisP41.Count.ToString & ")"

        If lisP41.Count < 10 Then
            Me.chkShowTop10.Visible = False
        Else
            Me.chkShowTop10.Visible = True

            Dim mqP31 As New BO.myQueryP31
            mqP31.j02ID = Master.Factory.SysUser.j02ID
            mqP31.TopRecordsOnly = 100
            mqP31.MG_SortString = "p31dateinsert desc"
            Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mqP31)
            If lisP31.Count = 0 Then
                chkShowTop10.Visible = False
            Else
                Dim p41ids As New List(Of Integer)
                If lisP31.Select(Function(p) p.p41ID).Distinct.Count > 10 Then
                    For Each c In lisP31
                        If p41ids.Where(Function(p) p = c.p41ID).Count = 0 Then
                            p41ids.Add(c.p41ID)
                        End If
                        If p41ids.Count >= 10 Then Exit For
                    Next
                Else
                    p41ids = lisP31.Select(Function(p) p.p41ID).Distinct.ToList
                End If

                mqP41 = New BO.myQueryP41
                mqP41.PIDs = p41ids
                lisP41_TOP10 = Master.Factory.p41ProjectBL.GetList(mqP41)

            End If
        End If
        If chkShowTop10.Checked And chkShowTop10.Visible Then
            RefreshProjectList(lisP41_TOP10)
        Else
            RefreshProjectList(lisP41)
        End If
        If Me.chkShowTop10.Checked Then
            lblVybratProjekt.Text = "Vybrat projekt (TOP " & lisP41_TOP10.Count.ToString & " z " & lisP41.Count.ToString & ")"
        End If

        Dim mqP56 As New BO.myQueryP56
        mqP56.j02ID = Master.Factory.SysUser.j02ID
        mqP56.Closed = BO.BooleanQueryMode.FalseQuery
        'mq.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForWorksheetEntry
        Dim lisP56 As IEnumerable(Of BO.p56Task) = Master.Factory.p56TaskBL.GetList(mqP56).OrderBy(Function(p) p.Client)
        rpP56_cbx.DataSource = lisP56
        rpP56_cbx.DataBind()
        lblVybratUkol.Text = "nebo vybrat úkol (" & lisP56.Count.ToString & ")"

        If lisP56.Count = 0 Then
            panDropdownSelectP56ID.Visible = False
        Else
            panDropdownSelectP56ID.Visible = True
        End If
    End Sub

    Private Sub RefreshProjectList(lisP41 As IEnumerable(Of BO.p41Project))
        rpP41_cbx.DataSource = lisP41
        rpP41_cbx.DataBind()
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
            Case "p41id"
                Me.CurrentP41ID = BO.BAS.IsNullInt(strVal)
                Me.CurrentP56ID = 0
                SelectP56ID(0)
                Master.Factory.j03UserBL.SetUserParam("mobile-p41id", strVal)
                Dim cRecP31 As BO.p31Worksheet = Nothing
                If Me.CurrentP31ID <> 0 Then
                    cRecP31 = Master.Factory.p31WorksheetBL.Load(Me.CurrentP31ID)
                End If
                Handle_ChangeP41(cRecP31)

                ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
            Case "p56id"
                Me.CurrentP56ID = BO.BAS.IsNullInt(strVal)
                Dim cRecP31 As BO.p31Worksheet = Nothing
                If Me.CurrentP31ID <> 0 Then
                    cRecP31 = Master.Factory.p31WorksheetBL.Load(Me.CurrentP31ID)
                End If
                Handle_ChangeP56(cRecP31)
                ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
            Case "pls"
                Me.hidPLS.Value = strVal
                Master.Factory.j03UserBL.SetUserParam("mobile-pls", strVal)
                SetupProjectList()
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
                    Me.IsDirectCallP41ID = False
                    RefreshRecord(0)
                    RefreshP31List()
                    ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_start';", True)
                End If

        End Select

        Me.HardRefreshFlag.Value = ""
    End Sub



    Private Sub SelectP41ID(intP41ID As Integer)
        For Each ri As RepeaterItem In rpP41_cbx.Items
            With CType(ri.FindControl("li1"), HtmlGenericControl)
                .Attributes.Item("class") = ""
                If intP41ID.ToString = CType(ri.FindControl("pid"), HiddenField).Value Then
                    .Attributes.Item("class") = "active"
                End If
            End With
        Next
    End Sub
    Private Sub SelectP56ID(intP56ID As Integer)
        For Each ri As RepeaterItem In rpP56_cbx.Items
            With CType(ri.FindControl("li1"), HtmlGenericControl)
                .Attributes.Item("class") = ""
                If intP56ID.ToString = CType(ri.FindControl("pid"), HiddenField).Value Then
                    .Attributes.Item("class") = "active"
                End If
            End With
        Next
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
        Else
            Me.p31Text.Text = ""
            Me.p31Value_Orig.Text = ""
            Me.p31Hours_Orig.Text = ""
            Me.p31Date.Text = Format(Now, "dd.MM.yyyy")
            p31Amount_WithVat_Orig.Text = "" : p31Amount_Vat_Orig.Text = "" : p31Amount_WithoutVat_Orig.Text = ""
        End If

        If Me.CurrentP56ID <> 0 Then
            Handle_ChangeP56(cRec)
        Else
            Handle_ChangeP41(cRec)
        End If



    End Sub

    Private Sub RenderTaskInfo(cRec As BO.p56Task)
        Me.Task.Text = cRec.p57Name & ": " & cRec.p56Name
        Me.Task.NavigateUrl = "p56_framework_mobile.aspx?pid=" & cRec.PID.ToString
    End Sub

    Private Sub Handle_ChangeP41(Optional cRecP31 As BO.p31Worksheet = Nothing)
        SelectP41ID(Me.CurrentP41ID)
        If Me.CurrentP41ID = 0 Then
            Return
        End If
        Dim cRecP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(Me.CurrentP41ID)
        Me.Project.Text = cRecP41.p41Name
        If cRecP41.p41NameShort <> "" Then Me.Project.Text = cRecP41.p41NameShort
        Me.Project.NavigateUrl = "p41_framework_mobile.aspx?pid=" & Me.CurrentP41ID.ToString

        If cRecP41.p28ID_Client <> 0 Then
            Me.Client.Text = cRecP41.Client
            Me.Client.NavigateUrl = "p28_framework_mobile.aspx?pid=" & Me.CurrentP41ID.ToString
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
    End Sub
    Private Sub Handle_ChangeP56(Optional cRecP31 As BO.p31Worksheet = Nothing)
        SelectP56ID(Me.CurrentP56ID)
        If Me.CurrentP56ID = 0 Then
            Return
        End If
        Dim cRec As BO.p56Task = Master.Factory.p56TaskBL.Load(Me.CurrentP56ID)
        Me.CurrentP41ID = cRec.p41ID
        Handle_ChangeP41(cRecP31)
        RenderTaskInfo(cRec)
    End Sub

    Private Sub p34ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles p34ID.SelectedIndexChanged
        Handle_ChangeP34()
        ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_p32id';", True)
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

    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.CurrentP31ID = 0 Then
            Me.lblEntryHeader.Text = "Zápis nového úkonu" : imgHeader.ImageUrl = "Images/new.png"
            Me.cmdDelete.Visible = False
        Else
            Me.lblEntryHeader.Text = "Úprava vybraného úkonu" : imgHeader.ImageUrl = "Images/edit.png"
            Me.cmdDelete.Visible = True
        End If
        If Me.Project.Text = "" Or Me.CurrentP41ID = 0 Then
            Me.Project.Style.Item("display") = "none"
        Else
            Me.Project.Style.Item("display") = "block"
        End If
        If Me.Client.Text = "" Then
            Me.Client.Style.Item("display") = "none"
        Else
            Me.Client.Style.Item("display") = "block"
        End If
        If Me.Task.Text = "" Or Me.CurrentP56ID = 0 Then
            Me.Task.Style.Item("display") = "none"
        Else
            Me.Task.Style.Item("display") = "block"
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


    End Sub

    Private Sub p32ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles p32ID.SelectedIndexChanged
        ClientScript.RegisterStartupScript(Me.GetType, "hash", "location.hash = '#record_p32id';", True)
    End Sub



    Private Sub Handle_ChangeP34()
        panT.Visible = False : panU.Visible = False : panM.Visible = False

        Dim mq As New BO.myQueryP32
        mq.p34ID = Me.CurrentP34ID
        Me.p32ID.DataSource = Master.Factory.p32ActivityBL.GetList(mq)
        Me.p32ID.DataBind()
        Me.p32ID.Items.Insert(0, "--Aktivita úkonu--")

        Dim cRec As BO.p34ActivityGroup = Master.Factory.p34ActivityGroupBL.Load(Me.CurrentP34ID)
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

    Private Function SaveChanges() As Boolean
        With Master.Factory.p31WorksheetBL
            Dim cRec As New BO.p31WorksheetEntryInput()
            With cRec
                .SetPID(Me.CurrentP31ID)
                .j02ID = Master.Factory.SysUser.j02ID
                .p41ID = Me.CurrentP41ID
                .p56ID = Me.CurrentP56ID
                .p34ID = Me.CurrentP34ID
                .p32ID = BO.BAS.IsNullInt(Me.p32ID.SelectedValue)

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
                'Select Case Me.CurrentP33ID
                '    Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                '        .p31Calc_PieceAmount = BO.BAS.IsNullNum(Me.p31Calc_PieceAmount.Value)
                '        .p31Calc_Pieces = BO.BAS.IsNullNum(Me.p31Calc_Pieces.Value)

                'End Select

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

    Private Sub chkShowTop10_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowTop10.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("mobile-top10", BO.BAS.GB(Me.chkShowTop10.Checked))
        SetupProjectList()
    End Sub

    Private Sub RefreshP31List()
        Dim mq As New BO.myQueryP31
        mq.j02ID = Master.Factory.SysUser.j02ID
        mq.DateFrom = Today.AddDays(-1 * BO.BAS.IsNullInt(Me.hidDaysQueryBefore.Value))
        mq.MG_SortString = "p31Date DESC"

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        lblListP31ListHeader.Text = "Přehled úkonů od " & BO.BAS.FD(mq.DateFrom, False, True) & " | " & lis.Count.ToString & "x"
        rpP31.DataSource = lis
        rpP31.DataBind()


    End Sub

    Private Sub rpP31_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP31.ItemDataBound
        Dim cRec As BO.p31Worksheet = CType(e.Item.DataItem, BO.p31Worksheet)
        Dim bolShowTotals As Boolean = False

        With CType(e.Item.FindControl("p31Date"), Label)
            If _lastP31Date Is Nothing Then
                bolShowTotals = True
            Else
                If _lastP31Date = cRec.p31Date Then
                    .Visible = False
                    CType(e.Item.FindControl("trDate"), HtmlTableRow).Style.Item("display") = "none"
                Else
                    bolShowTotals = True
                End If
            End If
            If bolShowTotals Then
                .Text = BO.BAS.FD(cRec.p31Date)
                Dim lis As IEnumerable(Of BO.p31Worksheet) = CType(rpP31.DataSource, IEnumerable(Of BO.p31Worksheet))
                CType(e.Item.FindControl("Pocet"), Label).Text = "(" & lis.Where(Function(p) p.p31Date = cRec.p31Date).Count.ToString & "x)"
                CType(e.Item.FindControl("Hodiny"), Label).Text = BO.BAS.FN(lis.Where(Function(p) p.p31Date = cRec.p31Date).Sum(Function(p) p.p31Hours_Orig)) & " h."
            End If
        End With



        With CType(e.Item.FindControl("Project"), Label)
            If cRec.p41NameShort <> "" Then
                .Text = cRec.p41NameShort
            Else
                .Text = cRec.p41Name
            End If
            If cRec.p28ID_Client <> 0 Then
                .Text = BO.BAS.OM3(cRec.p28Name, 15) & " - " & .Text
            End If
        End With
        With CType(e.Item.FindControl("Task"), Label)
            If cRec.p56ID <> 0 Then
                .Text = cRec.p56Name
            Else
                .Visible = False
            End If
        End With
        With CType(e.Item.FindControl("p32Name"), Label)
            .Text = cRec.p32Name
            If Not cRec.p32IsBillable Then .ForeColor = Drawing.Color.Red
        End With
        With CType(e.Item.FindControl("p31Value_Orig"), Label)
            .Text = BO.BAS.FN(cRec.p31Value_Orig)
            If Not cRec.p32IsBillable Then .ForeColor = Drawing.Color.Red
        End With
        CType(e.Item.FindControl("p31Text"), Label).Text = BO.BAS.CrLfText2Html(cRec.p31Text)

        _lastP31Date = cRec.p31Date
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
End Class