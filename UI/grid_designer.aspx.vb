Public Class grid_designer
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Public Property CurrentIsSystem As Boolean
        Get
            Return BO.BAS.BG(j74IsSystem.Value)
        End Get
        Set(value As Boolean)
            j74IsSystem.Value = BO.BAS.GB(value)
        End Set
    End Property
    Private ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return CType(ViewState("x29id"), BO.x29IdEnum)
        End Get
    End Property
    Private Property CurrentJ74ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j74ID.SelectedValue)
        End Get
        Set(value As Integer)
            Me.j74ID.SelectedValue = value.ToString
        End Set
    End Property

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

  

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Item("x29id") = "" Then
                If Request.Item("prefix") <> "" Then
                    ViewState("x29id") = BO.BAS.GetX29FromPrefix(Request.Item("prefix"))
                Else
                    Master.StopPage("x29id or prefix is missing.")
                End If
            Else
                ViewState("x29id") = Request.Item("x29id")
            End If
            ViewState("masterprefix") = Request.Item("masterprefix")
            With Master
                .HeaderIcon = "Images/griddesigner_32.png"
                .HeaderText = "Šablony datového přehledu"
                .AddToolbarButton("Vybrat", "ok", , "Images/ok.png")
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

            End With
            

            SetupJ74Combo()
            RefreshRecord()

            If Request.Item("nodrilldown") = "1" Then
                'drilldown nepodporovat
                Me.j74DrillDownField1.Enabled = False : Me.j74DrillDownField1.SelectedIndex = 0 : Me.j74DrillDownField1.Items(0).Text = "Přehled bez podpory drill-down"
            End If
        End If
    End Sub

    Private Sub SetupJ74Combo()
        Dim lisJ74 As IEnumerable(Of BO.j74SavedGridColTemplate) = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j74RecordState = BO.p31RecordState._NotExists)
        If ViewState("masterprefix") <> "" Then
            lisJ74 = lisJ74.Where(Function(p) p.j74MasterPrefix = ViewState("masterprefix"))
        End If
        If lisJ74.Count = 0 Then
            'uživatel zatím nemá žádnou šablonu - založit první j74IsSystem=1
            Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID, ViewState("masterprefix"))
            lisJ74 = Master.Factory.j74SavedGridColTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j74RecordState = BO.p31RecordState._NotExists)
            If ViewState("masterprefix") <> "" Then
                lisJ74 = lisJ74.Where(Function(p) p.j74MasterPrefix = ViewState("masterprefix"))
            End If
        End If
        j74ID.DataSource = lisJ74
        j74ID.DataBind()
        Me.CurrentJ74ID = Master.DataPID

    End Sub

    Private Sub SetupCols()
        Dim lisAllCols As List(Of BO.GridColumn) = Master.Factory.j74SavedGridColTemplateBL.ColumnsPallete(Me.CurrentX29ID)

        colsSource.Items.Clear()
        colsDest.Items.Clear()
        For Each c In lisAllCols
            Dim it As New Telerik.Web.UI.RadListBoxItem(c.ColumnHeader, c.ColumnName)
            Select Case c.ColumnType
                Case BO.cfENUM.DateTime, BO.cfENUM.DateTime
                    it.ImageUrl = "Images/type_datetime.png"
                Case BO.cfENUM.DateOnly
                    it.ImageUrl = "Images/type_date.png"
                Case BO.cfENUM.Numeric, BO.cfENUM.Numeric2, BO.cfENUM.Numeric0
                    it.ImageUrl = "Images/type_number.png"
                Case BO.cfENUM.AnyString
                    it.ImageUrl = "Images/type_text.png"
                Case BO.cfENUM.Checkbox
                    it.ImageUrl = "Images/type_checkbox.png"
            End Select
            colsSource.Items.Add(it)
        Next

        Me.cbxOrderBy1.DataSource = lisAllCols.Where(Function(p) p.IsSortable = True)
        Me.cbxOrderBy1.DataBind()
        Me.cbxOrderBy1.Items.Insert(0, "")
        Me.cbxOrderBy2.DataSource = lisAllCols.Where(Function(p) p.IsSortable = True)
        Me.cbxOrderBy2.DataBind()
        Me.cbxOrderBy2.Items.Insert(0, "")

        Dim lisDrillDownFields As List(Of BO.GridGroupByColumn) = Master.Factory.j74SavedGridColTemplateBL.GroupByPallet(Me.CurrentX29ID)
        Me.j74DrillDownField1.DataSource = lisDrillDownFields.Where(Function(p) p.ColumnField <> "")
        Me.j74DrillDownField1.DataBind()
        Me.j74DrillDownField1.Items.Insert(0, "")
    End Sub
    Private Sub RefreshRecord()
        cmdDelete.Visible = False : cmdNew.Visible = False
        If Me.CurrentJ74ID = 0 Then
            'If Me.CurrentJ74ID = 0 Then
            '    j74Name.Text = "Výchozí datový přehled"
            'End If
            j74Name.Enabled = True
            Return
        End If
        Dim cRec As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        With cRec
            j74Name.Text = .j74Name
            Master.HeaderText = "Šablona datového přehledu" & " | " & .j74Name
            cmdDelete.Visible = True
            cmdNew.Visible = True

            Me.CurrentIsSystem = .j74IsSystem
            SetupCols()
            Master.DataPID = .PID

            For Each s In Split(.j74ColumnNames, ",")
                If s <> "" Then
                    Dim strField As String = Trim(s)
                    Dim it As Telerik.Web.UI.RadListBoxItem = colsSource.FindItem(Function(p) LCase(p.Value) = LCase(strField))
                    If Not it Is Nothing Then
                        colsSource.Transfer(it, colsSource, colsDest)
                        colsSource.ClearSelection()
                        colsDest.ClearSelection()
                    End If
                End If
            Next
            If .j74OrderBy <> "" Then
                Dim a() As String = Split(.j74OrderBy, ",")
                If a(0).IndexOf("DESC") > 0 Then
                    Me.cbxOrderBy1Dir.SelectedValue = "DESC"
                    basUI.SelectDropdownlistValue(Me.cbxOrderBy1, Replace(a(0), " DESC", ""))
                Else
                    basUI.SelectDropdownlistValue(Me.cbxOrderBy1, a(0))
                End If
                If UBound(a) > 0 Then
                    If a(1).IndexOf("DESC") > 0 Then
                        Me.cbxOrderBy2Dir.SelectedValue = "DESC"
                        basUI.SelectDropdownlistValue(Me.cbxOrderBy2, Replace(a(1), " DESC", ""))
                    Else
                        basUI.SelectDropdownlistValue(Me.cbxOrderBy2, a(1))
                    End If
                End If
            End If
            Me.j74IsFilteringByColumn.Checked = cRec.j74IsFilteringByColumn
            Me.j74IsVirtualScrolling.Checked = cRec.j74IsVirtualScrolling
            basUI.SelectDropdownlistValue(Me.j74DrillDownField1, .j74DrillDownField1)
        End With
        colsSource.ClearSelection()


    End Sub


    

 


    Private Function SaveRecord(cRec As BO.j74SavedGridColTemplate) As Integer
        With cRec
            .x29ID = Me.CurrentX29ID
            .j74Name = j74Name.Text
            Dim s As String = ""
            For Each it As Telerik.Web.UI.RadListBoxItem In colsDest.Items
                s += "," & it.Value
            Next
            .j74ColumnNames = BO.BAS.OM1(s)
            .j74OrderBy = GetOrderBy()
            .j74IsFilteringByColumn = Me.j74IsFilteringByColumn.Checked
            .j74IsVirtualScrolling = Me.j74IsVirtualScrolling.Checked
            .j74DrillDownField1 = Me.j74DrillDownField1.SelectedValue
        End With

        If Master.Factory.j74SavedGridColTemplateBL.Save(cRec) Then
            Return Master.Factory.j74SavedGridColTemplateBL.LastSavedPID
        Else
            Master.Notify(Master.Factory.j74SavedGridColTemplateBL.ErrorMessage, 2)
            Return 0
        End If
    End Function

    Private Sub TestAndSaveChanges(bolShowConfirmMessage As Boolean)
        If Me.colsDest.ClientChanges.Count > 0 Then

            Dim cRec As BO.j74SavedGridColTemplate = Master.Factory.j74SavedGridColTemplateBL.Load(Master.DataPID)
            If SaveRecord(cRec) Then
                If bolShowConfirmMessage Then Master.Notify("Změny v nastavení sloupců uloženy [" & cRec.j74Name & "].", NotifyLevel.InfoMessage)
            End If
        End If
    End Sub
    Private Sub Handle_ChangeJ74()
        TestAndSaveChanges(True)

        If Me.CurrentJ74ID = 0 Then
            j74Name.Text = ""
            Me.CurrentIsSystem = False
        End If
        RefreshRecord()

    End Sub

   

    Private Function SaveCompleteChanges() As Boolean
        Dim cRec As BO.j74SavedGridColTemplate = Nothing

        If Me.CurrentJ74ID = 0 Then
            cRec = New BO.j74SavedGridColTemplate
            cRec.j74MasterPrefix = ViewState("masterprefix")
        Else
            cRec = Master.Factory.j74SavedGridColTemplateBL.Load(Me.CurrentJ74ID)
        End If

        Dim intJ74ID As Integer = SaveRecord(cRec)
        If intJ74ID > 0 Then
            Master.DataPID = intJ74ID.ToString
            SetupJ74Combo()
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetOrderBy() As String
        Dim s As String = ""
        If Me.cbxOrderBy1.SelectedValue <> "" Then
            s = Me.cbxOrderBy1.SelectedValue
            If Me.cbxOrderBy1Dir.SelectedValue <> "" Then
                s += " DESC"
            End If
        End If
        If Me.cbxOrderBy2.SelectedValue <> "" Then
            If s = "" Then
                s = Me.cbxOrderBy2.SelectedValue
            Else
                s += "," & Me.cbxOrderBy2.SelectedValue
            End If

            If Me.cbxOrderBy2Dir.SelectedValue <> "" Then
                s += " DESC"
            End If
        End If
        Return s
    End Function

    Private Sub cmdDelete_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdDelete.Click
        If Me.CurrentJ74ID = 0 Then Return

        If Master.Factory.j74SavedGridColTemplateBL.Delete(Me.CurrentJ74ID) Then
            Server.Transfer("grid_designer.aspx?x29id=" & CInt(Me.CurrentX29ID).ToString)
        Else
            Master.Notify(Master.Factory.j74SavedGridColTemplateBL.ErrorMessage, 2)
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            'If Me.colsDest.ClientChanges.Count > 0 Then
            '    SaveCompleteChanges()
            'End If
            If Me.cbxOrderBy1.SelectedValue = Me.cbxOrderBy2.SelectedValue And Me.cbxOrderBy1.SelectedValue <> "" Then
                Master.Notify("Třídění není korektně nastaveno.", NotifyLevel.ErrorMessage)
                Return
            End If
            SaveCompleteChanges()
            If ViewState("masterprefix") <> "" Then
                Dim strKey As String = BO.BAS.GetDataPrefix(ViewState("x29id")) & "_subgrid-j74id_" & ViewState("masterprefix")
                Master.Factory.j03UserBL.SetUserParam(strKey, Me.CurrentJ74ID.ToString)
            End If
            ClearUserParams()

            Master.DataPID = Me.CurrentJ74ID
            Master.CloseAndRefreshParent("j74")
        End If
    End Sub

    Private Sub ClearUserParams()
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Master.Factory.j03UserBL.SetUserParam("p41_framework-sort", "")
            Case BO.x29IdEnum.p28Contact
                Master.Factory.j03UserBL.SetUserParam("p28_framework-sort", "")
            Case BO.x29IdEnum.p91Invoice
                Master.Factory.j03UserBL.SetUserParam("p91_framework-sort", "")
            Case BO.x29IdEnum.p31Worksheet
                Master.Factory.j03UserBL.SetUserParam("p31_grid-sort", "")
            Case BO.x29IdEnum.j02Person
                Master.Factory.j03UserBL.SetUserParam("j02_framework-sort", "")
        End Select
    End Sub

    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        lblName.Visible = Not Me.CurrentIsSystem
        j74Name.Visible = Not Me.CurrentIsSystem
        If cmdDelete.Visible Then
            cmdDelete.Visible = Not Me.CurrentIsSystem
        End If
        cmdSave.Visible = Not Me.CurrentIsSystem
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        TestAndSaveChanges(True)

        Me.CurrentIsSystem = False
        If Me.j74ID.RadCombo.FindItemByValue("0") Is Nothing Then
            Me.j74ID.RadCombo.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("---Založit novou šablonu sloupců---", "0"))

        End If


        j74ID.SelectedIndex = 0
        j74Name.Text = "Přehled " & Master.Factory.SysUser.Person & " " & Now.ToString : j74Name.Focus()

        RefreshRecord()
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSave.Click
        If SaveCompleteChanges() Then
            ClearUserParams()
            RefreshRecord()
        End If
    End Sub

    Private Sub j74ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles j74ID.SelectedIndexChanged
        Handle_ChangeJ74()
    End Sub
End Class