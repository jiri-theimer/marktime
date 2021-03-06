﻿Public Class p28_batch
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p28_batch_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory
        
        If Not Page.IsPostBack Then
            System.Threading.Thread.Sleep(3000) 'počkat 3 sekundy než se na klientovi uloží seznam PIDů
            Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.LoadByGUID("p28_batch-pids-" & Master.Factory.SysUser.PID.ToString)
            If cRec Is Nothing Then
                Master.StopPage("pids is missing.")
            End If
            If cRec.p85Message = "" Then
                Master.StopPage("Na vstupu chybí vybrané záznamy klientů.")
            End If
            ViewState("pids") = cRec.p85Message
            With Master
                .HeaderText = "Hromadné operace nad vybranými klienty"
                .HeaderIcon = "Images/batch_32.png"
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With
            SetupFF()

            SetupGrid()

        End If
    End Sub

    Private Sub SetupFF()
        Dim lisX28 As IEnumerable(Of BO.x28EntityField) = Master.Factory.x28EntityFieldBL.GetList(BO.x29IdEnum.p28Contact, -1, True)
        For Each c In lisX28
            opgTarget.Items.Add(New ListItem("Uživatelské pole [" & c.x28Name & "]", "ff-" & c.PID.ToString))
        Next
    End Sub

    Private Sub SW(s As String)
        Master.Notify(s, NotifyLevel.WarningMessage)
    End Sub

    Private Sub RefreshCombo()
        Me.lblCombo.Text = Me.opgTarget.SelectedItem.Text & ":"
        With Me.cbx1
            Select Case LCase(Me.opgTarget.SelectedValue)
                Case "p87id"
                    .DataTextField = "p87Name"
                    basUI.SetupP87Combo(Master.Factory, Me.cbx1)
                Case "p51id_billing"
                    .DataTextField = "p51Name"
                    .DataSource = Master.Factory.p51PriceListBL.GetList(New BO.myQuery).Where(Function(p) p.p51IsInternalPriceList = False And p.p51IsMasterPriceList = False And p.p51IsCustomTailor = False)

                Case "p51id_internal"
                    .DataTextField = "p51Name"
                    .DataSource = Master.Factory.p51PriceListBL.GetList(New BO.myQuery).Where(Function(p) p.p51IsInternalPriceList = True And p.p51IsMasterPriceList = False And p.p51IsCustomTailor = False)
                Case "p29id"
                    .DataTextField = "p29name"
                    .DataSource = Master.Factory.p29ContactTypeBL.GetList(New BO.myQuery)
                Case "p92id"
                    .DataTextField = "p92name"
                    .DataSource = Master.Factory.p92InvoiceTypeBL.GetList(New BO.myQuery)
            End Select

            .DataBind()
        End With
    End Sub

    Private Sub opgTarget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgTarget.SelectedIndexChanged
        Me.panCombo.Visible = False : Me.panFF.Visible = False

        Select Case Me.opgTarget.SelectedValue
            Case "bin", "restore"
            Case Else
                opgComboMode.SelectedValue = "1"
                If Left(Me.opgTarget.SelectedValue, 3) = "ff-" Then
                    Me.panFF.Visible = True
                    Dim a() As String = Split(Me.opgTarget.SelectedValue, "-")
                    Dim intX28ID As Integer = CInt(a(1))
                    Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p28Contact, 0, -1)

                    ff1.FillData(fields.Where(Function(p) p.PID = intX28ID), Nothing, "p28Contact_FreeField", 0)
                Else
                    panCombo.Visible = True
                    RefreshCombo()
                End If

        End Select
    End Sub
    Private Sub SetupGrid()
       With Master.Factory.j70QueryTemplateBL
            Dim cJ70 As BO.j70QueryTemplate = .LoadSystemTemplate(BO.x29IdEnum.p28Contact, Master.Factory.SysUser.PID)
            cJ70.j70IsFilteringByColumn = False
            basUIMT.SetupDataGrid(Master.Factory, Me.grid1, cJ70, 5000, False, False)
        End With

        grid1.radGridOrig.ShowFooter = False
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        grid1.DataSource = GetList()
    End Sub

    Private Function GetList() As IEnumerable(Of BO.p28Contact)
        Dim mq As New BO.myQueryP28
        With mq
            .PIDs = BO.BAS.ConvertPIDs2List(ViewState("pids"))
            .Closed = BO.BooleanQueryMode.NoQuery
        End With
        Return Master.Factory.p28ContactBL.GetList(mq)
    End Function

    Private Sub p28_batch_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        lblCount.Text = grid1.RowsCount.ToString
        If panCombo.Visible Or panFF.Visible Then opgComboMode.Visible = True Else opgComboMode.Visible = False

        If Me.opgComboMode.SelectedValue = "1" Then
            cbx1.Visible = True
            lblCombo.Visible = True
            ff1.Visible = True
        Else
            cbx1.Visible = False
            lblCombo.Visible = False
            ff1.Visible = False
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            If Not ValidateBefore() Then Return
            Dim errs As New List(Of String)

            Dim lisFF As List(Of BO.FreeField) = Nothing
            If panFF.Visible Then
                lisFF = Me.ff1.GetValues()
            End If

            Dim lis As IEnumerable(Of BO.p28Contact) = GetList()
            For Each c In lis
                Dim bolClear As Boolean = IIf(opgComboMode.SelectedValue = "1", False, True)

                If Me.panCombo.Visible Then
                    Dim intVal As Integer = BO.BAS.IsNullInt(Me.cbx1.SelectedValue)

                    Select Case LCase(Me.opgTarget.SelectedValue)
                        Case "p87id"
                            If Not bolClear Then c.p87ID = intVal Else c.p87ID = 0
                        Case "p51id_billing"
                            If Not bolClear Then c.p51ID_Billing = intVal Else c.p51ID_Billing = 0
                        Case "p51id_internal"
                            If Not bolClear Then c.p51ID_Internal = intVal Else c.p51ID_Internal = 0
                        Case "p29id"
                            c.p29ID = intVal
                        Case "p92id"
                            c.p92ID = intVal
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
                If Not Master.Factory.p28ContactBL.Save(c, Nothing, Nothing, Nothing, Nothing, lisFF) Then
                    errs.Add(c.p28Name & ": " & Master.Factory.p28ContactBL.ErrorMessage)
                End If


            Next

            If errs.Count = 0 Then
                Master.CloseAndRefreshParent("p28_batch")
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


    Private Function ValidateBefore() As Boolean
        If Me.opgTarget.SelectedItem Is Nothing Then
            SW("Musíte zvolit předmět hromadné operace.")
            Return False
        End If
        If Not (Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin) Or Master.Factory.TestPermission(BO.x53PermValEnum.GR_P28_Owner)) Then
            Dim lis As IEnumerable(Of BO.p28Contact) = GetList()
            For Each c In lis
                'oprávnění pro editaci kontaktu
                If c.j02ID_Owner <> Master.Factory.SysUser.j02ID And c.j02ID_Owner <> 0 Then
                    SW("Ke klientovi [" & c.p28Name & "] nedisponujete editačním oprávněním.")
                    Return False
                End If
            Next
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
            If BO.BAS.IsNullInt(Me.cbx1.SelectedValue) = 0 And opgComboMode.SelectedValue = "1" Then
                SW("Musíte vybrat cílovou hodnotu.")
                Return False
            End If
        End If

        Return True
    End Function

End Class