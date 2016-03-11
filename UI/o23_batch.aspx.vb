Public Class o23_batch
    Inherits System.Web.UI.Page

    Protected WithEvents _MasterPage As ModalForm

    Private Sub o23_batch_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory
        roles1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            System.Threading.Thread.Sleep(3000) 'počkat 3 sekundy než se na klientovi uloží seznam PIDů
            Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.LoadByGUID("o23_batch-pids-" & Master.Factory.SysUser.PID.ToString)
            If cRec Is Nothing Then
                Master.StopPage("pids is missing.")
            End If
            If cRec.p85Message = "" Then
                Master.StopPage("Na vstupu chybí vybrané záznamy dokumentů.")
            End If
            ViewState("pids") = cRec.p85Message
            With Master
                .HeaderText = "Hromadné operace nad vybranými dokumenty"
                .HeaderIcon = "Images/batch_32.png"
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With
            SetupFF()

            SetupGrid()

        End If
    End Sub

    Private Sub SetupFF()
        Dim lisX28 As IEnumerable(Of BO.x28EntityField) = Master.Factory.x28EntityFieldBL.GetList(BO.x29IdEnum.o23Notepad, -1)
        For Each c In lisX28
            opgTarget.Items.Add(New ListItem("Uživatelské pole [" & c.x28Name & "]", "ff-" & c.PID.ToString))
        Next
    End Sub

    Private Function ValidateBefore() As Boolean
        If Me.opgTarget.SelectedItem Is Nothing Then
            SW("Musíte zvolit předmět hromadné operace.")
            Return False
        End If
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
            Dim lis As IEnumerable(Of BO.o23Notepad) = GetList()
            For Each c In lis
                Dim bolCanEdit As Boolean = False
                Dim cDisp As BO.o23RecordDisposition = Master.Factory.o23NotepadBL.InhaleRecordDisposition(c)
                bolCanEdit = cDisp.OwnerAccess
                If Not bolCanEdit Then
                    SW("K dokumentu [" & c.o23Code & "] nedisponujete vlastnickým (editačním) oprávněním.")
                    Return False
                End If
            Next
        End If
        If panRoles.Visible Then
            roles1.SaveCurrentTempData()
            If opgComboMode.SelectedValue = "1" Then
                Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()
                If lisX69.Count = 0 Then
                    SW("Na vstupu chybí výběr dokumentové role.")
                    Return False
                End If
                With lisX69(0)
                    If (.j11ID = 0 And .j02ID = 0) Or .x67ID = 0 Then
                        SW("Na vstupu chybí výběr dokumentové role, osoby nebo týmu.")
                        Return False
                    End If
                End With
            End If

        End If

        If Me.panFF.Visible And opgComboMode.SelectedValue = "1" Then
            Dim lisFF As List(Of BO.FreeField) = Me.ff1.GetValues()
            With lisFF(0)
                Select Case .x24ID
                    Case BO.x24IdENUM.tInteger, BO.x24IdENUM.tDecimal
                        If BO.BAS.IsNullNum(.DBValue) = 0 Then
                            SW("Musíte vyplnit hodnotu pole [" & .x28Name & "].")
                            Return False
                        End If
                    Case BO.x24IdENUM.tString
                        If .DBValue Is Nothing Then
                            SW("Musíte vyplnit hodnotu pole [" & .x28Name & "].")
                            Return False
                        End If
                        If Trim(.DBValue.ToString) = "" Then
                            SW("Musíte vyplnit hodnotu pole [" & .x28Name & "].")
                            Return False
                        End If
                    Case BO.x24IdENUM.tDate, BO.x24IdENUM.tDateTime
                        If BO.BAS.IsNullDBDate(.DBValue) Is Nothing Then
                            SW("Musíte vyplnit hodnotu pole [" & .x28Name & "].")
                            Return False
                        End If
                End Select
            End With
        End If

        If panCombo.Visible Then
            If LCase(Me.opgTarget.SelectedValue) = "o24id" And opgComboMode.SelectedValue = "2" Then
                SW("Typ dokumentu je povinné pole a nelze v záznamu dokumentu vyčistit.")
                Return False
            End If
            If BO.BAS.IsNullInt(Me.cbx1.SelectedValue) = 0 And opgComboMode.SelectedValue = "1" Then
                SW("Musíte vybrat cílovou hodnotu.")
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub SW(s As String)
        Master.Notify(s, NotifyLevel.WarningMessage)
    End Sub
    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            If Not ValidateBefore() Then Return
            Dim errs As New List(Of String)

            Dim lisFF As List(Of BO.FreeField) = Nothing, lisX69 As List(Of BO.x69EntityRole_Assign) = Nothing
            If panFF.Visible Then
                lisFF = Me.ff1.GetValues()
            End If
            If panRoles.Visible Then
                roles1.SaveCurrentTempData()
                lisX69 = roles1.GetData4Save()
            End If

            Dim lis As IEnumerable(Of BO.o23Notepad) = GetList()
            For Each c In lis
                Dim bolClear As Boolean = IIf(opgComboMode.SelectedValue = "1", False, True)

                If Me.panCombo.Visible Then
                    Dim intVal As Integer = BO.BAS.IsNullInt(Me.cbx1.SelectedValue)

                    Select Case LCase(Me.opgTarget.SelectedValue)
                        Case "b02id"
                            c.b02ID = intVal
                        Case "o24id"
                            c.o24ID = intVal
                        Case Else
                    End Select
                End If
                If Me.opgTarget.SelectedValue = "bin" Then
                    c.ValidUntil = Now
                End If
                If Me.opgTarget.SelectedValue = "restore" Then
                    c.ValidUntil = DateSerial(3000, 1, 1)
                End If
                If Me.panFF.Visible Then
                    If bolClear Then
                        With lisFF(0)
                            Select Case .x24ID
                                Case BO.x24IdENUM.tBoolean
                                    .DBValue = False
                                Case Else
                                    .DBValue = Nothing
                            End Select
                        End With

                    End If
                End If
                If panRoles.Visible Then
                    If Not bolClear Then
                        Master.Factory.o23NotepadBL.UpdateSelectedNotepadRole(lisX69(0).x67ID, lisX69, c.PID)
                    Else
                        Master.Factory.o23NotepadBL.ClearSelectedNotepadRole(CInt(Me.cbxClearedX67ID.SelectedValue), c.PID)
                    End If

                Else
                    If Not Master.Factory.o23NotepadBL.Save(c, "", Nothing, lisFF) Then
                        errs.Add(c.o23Code & ": " & Master.Factory.o23NotepadBL.ErrorMessage)
                    End If
                End If


            Next

            If errs.Count = 0 Then
                Master.CloseAndRefreshParent("o23_batch")
            Else
                Dim s As String = "Počet chyb: " & errs.Count.ToString & ", počet provedených aktualizací: " & (grid1.RowsCount - errs.Count).ToString
                For Each xx As String In errs
                    s += "<hr>" & xx
                Next
                Master.Notify(s)
                grid1.Rebind(False)
            End If


        End If
    End Sub

    Private Sub RefreshCombo()
        Me.lblCombo.Text = Me.opgTarget.SelectedItem.Text & ":"
        With Me.cbx1
            Select Case LCase(Me.opgTarget.SelectedValue)
                Case "b02id"
                    .DataTextField = "NameWithb01Name"
                    .DataSource = Master.Factory.b02WorkflowStatusBL.GetList(0).Where(Function(p) p.x29ID = BO.x29IdEnum.o23Notepad)
                Case "o24id"
                    .DataTextField = "o24Name"
                    .DataSource = Master.Factory.o24NotepadTypeBL.GetList(New BO.myQuery)
            End Select

            .DataBind()
        End With



    End Sub

    Private Sub opgTarget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgTarget.SelectedIndexChanged
        Me.panCombo.Visible = False : Me.panFF.Visible = False : panRoles.Visible = False

        Select Case Me.opgTarget.SelectedValue
            Case "bin", "restore"
            Case "role"
                opgComboMode.SelectedValue = "1"
                panRoles.Visible = True
                If roles1.RowsCount = 0 Then
                    roles1.InhaleInitialData(0)
                    roles1.AddNewRow()
                    Dim mq As New BO.myQuery
                    mq.Closed = BO.BooleanQueryMode.FalseQuery
                    cbxClearedX67ID.DataSource = Master.Factory.x67EntityRoleBL.GetList(mq).Where(Function(p) p.x29ID = BO.x29IdEnum.o23Notepad)
                    cbxClearedX67ID.DataBind()
                End If
            Case Else
                opgComboMode.SelectedValue = "1"
                If Left(Me.opgTarget.SelectedValue, 3) = "ff-" Then
                    Me.panFF.Visible = True
                    Dim a() As String = Split(Me.opgTarget.SelectedValue, "-")
                    Dim intX28ID As Integer = CInt(a(1))
                    Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.o23Notepad, 0, -1)

                    ff1.FillData(fields.Where(Function(p) p.PID = intX28ID))
                Else
                    panCombo.Visible = True
                    RefreshCombo()
                End If

        End Select

    End Sub

    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = .LoadSystemTemplate(BO.x29IdEnum.o23Notepad, Master.Factory.SysUser.PID)

            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 5000, False, False)
        End With

        grid1.radGridOrig.ShowFooter = False

    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource


        grid1.DataSource = GetList()

    End Sub

    Private Function GetList() As IEnumerable(Of BO.o23Notepad)
        Dim mq As New BO.myQueryO23
        With mq
            .PIDs = BO.BAS.ConvertPIDs2List(ViewState("pids"))
            .Closed = BO.BooleanQueryMode.NoQuery
        End With


        Return Master.Factory.o23NotepadBL.GetList(mq)
    End Function

    Private Sub o23_batch_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        lblCount.Text = grid1.RowsCount.ToString
        If panCombo.Visible Or panFF.Visible Or panRoles.Visible Then opgComboMode.Visible = True Else opgComboMode.Visible = False

        If Me.opgComboMode.SelectedValue = "1" Then
            cbx1.Visible = True
            lblCombo.Visible = True
            ff1.Visible = True
            roles1.Visible = True
            lblClearedX67ID.Visible = False
            cbxClearedX67ID.Visible = False
        Else
            cbx1.Visible = False
            lblCombo.Visible = False
            ff1.Visible = False
            roles1.Visible = False
            lblClearedX67ID.Visible = True
            cbxClearedX67ID.Visible = True
        End If
    End Sub

End Class