Public Class entity_modal_invoicing
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Private _lisP92 As IEnumerable(Of BO.p92InvoiceType)
    Private Class InvoiceEntity
        Public Property p92ID As Integer
        Public Property Name As String
        Public Sub New(strName As String, intP92ID As Integer)
            Me.Name = strName
            Me.p92ID = intP92ID
        End Sub
    End Class
    Public Property CurrentInputPIDs As String
        Get
            Return Me.hidInputPIDS.Value
        End Get
        Set(value As String)
            Me.hidInputPIDS.Value = value
        End Set
    End Property

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

    Private Sub entity_modal_invoicing_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentX29ID = BO.BAS.GetX29FromPrefix(Request.Item("prefix"))
            If Me.CurrentX29ID = BO.x29IdEnum._NotSpecified Then
                Master.StopPage("prefix is missing")
            End If
            If Request.Item("pids") = "" Then
                Master.StopPage("Na vstupu chybí záznamy.")
            End If
            Handle_Permissions()
            
            With Master
                Select Case Me.CurrentX29ID
                    Case BO.x29IdEnum.p41Project
                        .AddToolbarButton("Vystavit DRAFT faktury pro vybrané projekty", "save", , "Images/save.png")
                    Case BO.x29IdEnum.p28Contact
                        .AddToolbarButton("Vystavit DRAFT faktury pro vybrané klienty", "save", , "Images/save.png")
                    Case Else
                        .StopPage("Hromadné generování DRAFT faktur funguje pro projekty nebo klienty.")
                End Select

                Dim pids As List(Of Integer) = BO.BAS.ConvertPIDs2List(Request.Item("pids")).Distinct.ToList

                Select Case pids.Count
                    Case Is > 100
                        .StopPage("Najednou je možné vybrat maximállně 100 položek.")
                    Case Else
                        Me.CurrentInputPIDs = String.Join(",", pids)
                End Select
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("periodcombo-custom_query")
                    .Add("p31_grid-period")
                    .Add("p91_create-rememberdates")
                    .Add("p91_create-remembermaturity")
                    .Add("p91_create-rememberdates-values")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("p31_grid-period")
                    Handle_p91Date()
                    Handle_p91DateSupply()
                    Hadle_WorksheetDates()

                    Me.chkRememberDates.Checked = BO.BAS.BG(.GetUserParam("p91_create-rememberdates", "0"))
                    Me.chkRememberMaturiy.Checked = BO.BAS.BG(.GetUserParam("p91_create-remembermaturity", "0"))

                    Dim a() As String = Split(.GetUserParam("p91_create-rememberdates-values"), "|")

                    If .GetUserParam("p91_create-rememberdates", "0") = "1" Then
                        If UBound(a) > 1 Then
                            If a(0) <> "" Then Me.p91DateSupply.SelectedDate = BO.BAS.ConvertString2Date(a(0))
                            If a(1) <> "" Then Me.p91Date.SelectedDate = BO.BAS.ConvertString2Date(a(1))
                            If a(3) <> "" Then Me.p91Datep31_From.SelectedDate = BO.BAS.ConvertString2Date(a(3))
                            If a(4) <> "" Then Me.p91Datep31_Until.SelectedDate = BO.BAS.ConvertString2Date(a(4))
                        End If
                    End If
                    If .GetUserParam("p91_create-remembermaturity", "0") = "1" Then
                        If UBound(a) > 1 Then
                            If a(2) <> "" Then Me.p91DateMaturity.SelectedDate = BO.BAS.ConvertString2Date(a(2))
                        End If
                    End If
                End With

                _lisP92 = .Factory.p92InvoiceTypeBL.GetList(New BO.myQuery).Where(Function(p) p.p92InvoiceType = BO.p92InvoiceTypeENUM.ClientInvoice).OrderBy(Function(p) p.j27ID)
                Me.p92ID.DataSource = _lisP92
                Me.p92ID.DataBind()
            End With

            RefreshRecord()

            
        End If
    End Sub

    Private Sub Handle_Permissions()
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator) Then
            Master.StopPage("Nemáte oprávnění k vystavování DRAFT faktur.")
        End If
    End Sub
    Private Sub opgDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgDate.SelectedIndexChanged
        Handle_p91Date()
    End Sub

    Private Sub Handle_p91Date()
        Select Case Me.opgDate.SelectedValue
            Case "1"    'dnes
                Me.p91Date.SelectedDate = Today
            Case "2"    'konec minulého měsíce
                Me.p91Date.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddDays(-1)
            Case "3"    'konec aktuálního měsíce
                Me.p91Date.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1).AddDays(-1)
            Case "4" '1.den příštího měsíce
                Me.p91Date.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1)
        End Select
    End Sub
    Private Sub Handle_p91DateSupply()
        Select Case Me.opgDateSupply.SelectedValue
            Case "1"    'dnes
                Me.p91DateSupply.SelectedDate = Today
            Case "2"    'konec minulého měsíce
                Me.p91DateSupply.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddDays(-1)
            Case "3"    'konec aktuálního měsíce
                Me.p91DateSupply.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1).AddDays(-1)
            Case "4" '1.den příštího měsíce
                Me.p91DateSupply.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1)
        End Select
    End Sub
    Private Sub Hadle_WorksheetDates()
        Me.p91Datep31_From.SelectedDate = period1.DateFrom
        Me.p91Datep31_Until.SelectedDate = period1.DateUntil
    End Sub

    Private Sub opgDateSupply_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgDateSupply.SelectedIndexChanged
        Handle_p91DateSupply()
    End Sub

    Private Sub RefreshRecord()
        Dim lis1 As New List(Of InvoiceEntity)

        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim mq As New BO.myQueryP41
                mq.PIDs = BO.BAS.ConvertPIDs2List(Me.CurrentInputPIDs)
                Dim lis As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq)
                For Each c In lis
                    Dim intP92ID As Integer = c.p92ID
                    If intP92ID = 0 Then
                        Dim intP28ID As Integer = c.p28ID_Billing
                        If intP28ID = 0 Then intP28ID = c.p28ID_Client
                        Dim cP28 As BO.p28Contact = Master.Factory.p28ContactBL.Load(intP28ID)
                        intP92ID = cP28.p92ID
                    End If
                    lis1.Add(New InvoiceEntity(c.Client & " - " & c.PrefferedName, intP92ID))
                Next
            Case BO.x29IdEnum.p28Contact
                Dim mq As New BO.myQueryP28
                mq.PIDs = BO.BAS.ConvertPIDs2List(Me.CurrentInputPIDs)
                Dim lis As IEnumerable(Of BO.p28Contact) = Master.Factory.p28ContactBL.GetList(mq)
                For Each c In lis
                    lis1.Add(New InvoiceEntity(c.p28Name, c.p92ID))
                Next
            Case Else
                Return
        End Select
        rp1.DataSource = lis1
        rp1.DataBind()

    End Sub

    Private Sub entity_modal_invoicing_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With

    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Hadle_WorksheetDates()
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Master.Factory.j03UserBL.SetUserParam("p91_create-rememberdates", BO.BAS.GB(Me.chkRememberDates.Checked))
            Master.Factory.j03UserBL.SetUserParam("p91_create-remembermaturity", BO.BAS.GB(Me.chkRememberMaturiy.Checked))
            If Me.chkRememberDates.Checked Or Me.chkRememberMaturiy.Checked Then
                Master.Factory.j03UserBL.SetUserParam("p91_create-rememberdates-values", MFD(Me.p91DateSupply.SelectedDate) & "|" & MFD(Me.p91Date.SelectedDate) & "|" & MFD(Me.p91DateMaturity.SelectedDate) & "|" & MFD(Me.p91Datep31_From.SelectedDate) & "|" & MFD(Me.p91Datep31_Until.SelectedDate))
            End If

            For Each ri As RepeaterItem In rp1.Items
                Dim intPID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("pid"), HiddenField).Value)
                Dim intP92ID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("p92ID"), DropDownList).SelectedValue)

                Dim intP28ID As Integer
                Dim mqP31 As New BO.myQueryP31
                With mqP31
                    .DateFrom = period1.DateFrom
                    .DateUntil = period1.DateUntil
                    .QuickQuery = BO.myQueryP31_QuickQuery.EditingOrApproved
                End With
                If Me.CurrentX29ID = BO.x29IdEnum.p41Project Then
                    mqP31.p41ID = intPID
                    Dim c As BO.p41Project = Master.Factory.p41ProjectBL.Load(intPID)
                    intP28ID = c.p28ID_Billing
                    If intP28ID = 0 Then intP28ID = c.p28ID_Client

                End If
                If Me.CurrentX29ID = BO.x29IdEnum.p28Contact Then
                    mqP31.p28ID_Client = intPID
                    intP28ID = intPID
                    Dim c As BO.p28Contact = Master.Factory.p28ContactBL.Load(intPID)

                End If
                Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mqP31)
                Dim strGUID As String = BO.BAS.GetGUID
                For Each c In lisP31.Where(Function(p) p.p71ID = BO.p71IdENUM.Nic)
                    Dim cA As New BO.p31WorksheetApproveInput(c.PID, c.p33ID)
                    cA.p71id = BO.p71IdENUM.Schvaleno
                    If c.p31Rate_Billing_Orig = 0 Then
                        cA.p72id = BO.p72IdENUM.ZahrnoutDoPausalu
                        cA.Value_Approved_Billing = 0
                    Else
                        cA.p72id = BO.p72IdENUM.Fakturovat
                        cA.Value_Approved_Billing = c.p31Value_Orig
                        cA.VatRate_Approved = c.p31VatRate_Orig
                    End If

                    Master.Factory.p31WorksheetBL.Save_Approving(cA, False)
                Next
                For Each c In lisP31.Where(Function(p) p.p71ID = BO.p71IdENUM.Schvaleno)
                    Dim cTMP As New BO.p85TempBox
                    cTMP.p85GUID = strGUID
                    cTMP.p85DataPID = c.PID
                    Master.Factory.p85TempBoxBL.Save(cTMP)
                Next
                Dim cRec As New BO.p91Create
                With cRec
                    .p28ID = intP28ID
                    .IsDraft = True
                    .TempGUID = strGUID
                    .p92ID = BO.BAS.IsNullInt(Me.p92ID.SelectedValue)
                    .InvoiceText1 = Me.p91text1.Text
                    .DateIssue = Me.p91Date.SelectedDate
                    .DateMaturity = Me.p91DateMaturity.SelectedDate
                    .DateSupply = Me.p91DateSupply.SelectedDate
                    .DateP31_From = Me.p91Datep31_From.SelectedDate
                    .DateP31_Until = Me.p91Datep31_Until.SelectedDate
                End With
                Dim intP91ID As Integer = Master.Factory.p91InvoiceBL.Create(cRec)
                If intP91ID <> 0 Then

                End If
            Next
            
            ''Dim intP91ID As Integer = Master.Factory.p91InvoiceBL.Create(cRec)
            'If intP91ID <> 0 Then


            'Else
            '    Master.Notify(Master.Factory.p91InvoiceBL.ErrorMessage, NotifyLevel.ErrorMessage)
            'End If


        End If
    End Sub
    Private Function MFD(d As Date?)
        If d Is Nothing Then Return ""
        Return Format(d, "dd.MM.yyyy")
    End Function

    
    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As InvoiceEntity = CType(e.Item.DataItem, InvoiceEntity)
        CType(e.Item.FindControl("Entity"), Label).Text = cRec.Name
        With CType(e.Item.FindControl("p92ID"), DropDownList)
            .DataSource = _lisP92
            .DataBind()
            Try
                If cRec.p92ID <> 0 Then
                    .SelectedValue = cRec.p92ID.ToString
                Else
                    .SelectedValue = Me.p92ID.SelectedValue
                End If

            Catch ex As Exception

            End Try
        End With

    End Sub
End Class