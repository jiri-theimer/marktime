Public Class drilldown_designer
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentIsSystem As Boolean
        Get
            Return BO.BAS.BG(j75IsSystem.Value)
        End Get
        Set(value As Boolean)
            j75IsSystem.Value = BO.BAS.GB(value)
        End Set
    End Property
    Private ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            If Me.CurrentMasterPrefix = "" Then
                Return BO.x29IdEnum._NotSpecified
            Else
                Return BO.BAS.GetX29FromPrefix(Left(Me.CurrentMasterPrefix, 3))
            End If
        End Get
    End Property
    Private Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Private Property CurrentJ75ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j75ID.SelectedValue)
        End Get
        Set(value As Integer)
            Me.j75ID.SelectedValue = value.ToString
        End Set
    End Property

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        roles1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            If Me.CurrentMasterPrefix = "" Then Master.StopPage("masterprefix is missing")
            With Master
                .HeaderIcon = "Images/drilldown_32.png"
                .HeaderText = "DRILL-DOWN šablona"
                .AddToolbarButton(Resources.grid_designer.Vybrat, "ok", , "Images/ok.png")
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

            End With


            SetupJ75Combo()
            RefreshRecord()

           
        End If
    End Sub

    Private Sub SetupJ75Combo()
        Dim lisJ75 As IEnumerable(Of BO.j75DrillDownTemplate) = Master.Factory.j75DrillDownTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j03ID = Master.Factory.SysUser.PID And p.j75MasterPrefix = Me.CurrentMasterPrefix)
       
        If lisJ75.Count = 0 Then
            Master.Factory.j75DrillDownTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID, Me.CurrentMasterPrefix)
            lisJ75 = Master.Factory.j75DrillDownTemplateBL.GetList(New BO.myQuery, Me.CurrentX29ID).Where(Function(p) p.j03ID = Master.Factory.SysUser.PID And p.j75MasterPrefix = Me.CurrentMasterPrefix)
           
        End If
        Me.j75ID.DataSource = lisJ75
        Me.j75ID.DataBind()
        Me.CurrentJ75ID = Master.DataPID

    End Sub

    Private Sub SetupCols()
        Dim lisAllCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()

        colsSource.Items.Clear()
        colsDest.Items.Clear()
        For Each c In lisAllCols
            Dim it As New Telerik.Web.UI.RadListBoxItem(c.Caption, c.FieldTypeID.ToString)
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
            ''If c.ColumnName.IndexOf("Free") > 0 Then it.ForeColor = Drawing.Color.Green

            colsSource.Items.Add(it)
        Next

    End Sub
    Private Sub RefreshRecord()
        cmdDelete.Visible = False : cmdNew.Visible = False
        If Me.CurrentJ75ID = 0 Then
            j75Name.Enabled = True
            Return
        End If
        Dim cRec As BO.j75DrillDownTemplate = Master.Factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)
        With cRec
            j75Name.Text = .j75Name
            Master.HeaderText = "DRILL-DOWN šablona | " & .j75Name
            cmdDelete.Visible = True
            cmdNew.Visible = True

            Me.CurrentIsSystem = .j75IsSystem
            SetupCols()
            Master.DataPID = .PID

            If Not .j75Level1 Is Nothing Then
                basUI.SelectDropdownlistValue(Me.j75Level1, CInt(.j75Level1).ToString)
            End If
            If Not .j75Level2 Is Nothing Then
                basUI.SelectDropdownlistValue(Me.j75Level2, CInt(.j75Level2).ToString)
            End If
            If Not .j75Level3 Is Nothing Then
                basUI.SelectDropdownlistValue(Me.j75Level3, CInt(.j75Level3).ToString)
            End If
            If Not .j75Level4 Is Nothing Then
                basUI.SelectDropdownlistValue(Me.j75Level4, CInt(.j75Level4).ToString)
            End If
           
            panRoles.Visible = Not .j75IsSystem
        End With

        Dim lisAllCols As List(Of BO.PivotSumField) = Master.Factory.j75DrillDownTemplateBL.ColumnsPallete()
        Dim lisJ76 As IEnumerable(Of BO.j76DrillDownTemplate_Item) = Master.Factory.j75DrillDownTemplateBL.GetList_j76(Me.CurrentJ75ID)
        For Each c In lisJ76.Where(Function(p) p.j76PivotSumFieldType > 0)
            Dim col As BO.PivotSumField = lisAllCols.Where(Function(p) CInt(p.FieldType) = c.j76PivotSumFieldType).First
            If Not col Is Nothing Then
                Dim it As Telerik.Web.UI.RadListBoxItem = colsSource.FindItem(Function(p) p.Value = col.FieldTypeID.ToString)
                If Not it Is Nothing Then
                    colsSource.Transfer(it, colsSource, colsDest)
                    colsSource.ClearSelection()
                    colsDest.ClearSelection()
                End If
            End If

        Next
        
        colsSource.ClearSelection()

        roles1.InhaleInitialData(cRec.PID)
    End Sub

    Private Sub cmdNew_Click(sender As Object, e As EventArgs) Handles cmdNew.Click
        TestAndSaveChanges(True)

        Me.CurrentIsSystem = False
        If Me.j75ID.RadCombo.FindItemByValue("0") Is Nothing Then
            Me.j75ID.RadCombo.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("---" & Resources.grid_designer.ZalozitNovouSablonu & "---", "0"))

        End If


        j75ID.SelectedIndex = 0
        j75Name.Text = "DRILL-DOWN " & Master.Factory.SysUser.Person & " " & Now.ToString : j75Name.Focus()

        RefreshRecord()
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As ImageClickEventArgs) Handles cmdSave.Click
        roles1.SaveCurrentTempData()
        If SaveCompleteChanges() Then
            ''ClearUserParams()
            RefreshRecord()
        End If
    End Sub

    

    Private Sub cmdAddX69_Click(sender As Object, e As EventArgs) Handles cmdAddX69.Click
        roles1.AddNewRow()
    End Sub

    Private Sub TestAndSaveChanges(bolShowConfirmMessage As Boolean)
        If Me.colsDest.ClientChanges.Count > 0 Then

            Dim cRec As BO.j75DrillDownTemplate = Master.Factory.j75DrillDownTemplateBL.Load(Master.DataPID)
            If SaveRecord(cRec) Then
                If bolShowConfirmMessage Then Master.Notify("Změny v nastavení šablony uloženy [" & cRec.j75Name & "].", NotifyLevel.InfoMessage)
            End If
        End If
    End Sub
    Private Sub Handle_ChangeJ75()
        TestAndSaveChanges(True)

        If Me.CurrentJ75ID = 0 Then
            j75Name.Text = ""
            Me.CurrentIsSystem = False
        End If
        RefreshRecord()
    End Sub

    Private Sub drilldown_designer_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        ''lblName.Visible = Not Me.CurrentIsSystem
        ''j75Name.Visible = Not Me.CurrentIsSystem
        If cmdDelete.Visible Then
            cmdDelete.Visible = Not Me.CurrentIsSystem
        End If
        cmdSave.Visible = Not Me.CurrentIsSystem
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            roles1.SaveCurrentTempData()
         
        
            SaveCompleteChanges()
            If Me.CurrentMasterPrefix <> "" Then
                Master.Factory.j03UserBL.SetUserParam(Me.CurrentMasterPrefix & "_j75id", Me.CurrentJ75ID.ToString)
            End If
            ''ClearUserParams()

            Master.DataPID = Me.CurrentJ75ID
            Master.CloseAndRefreshParent("j75")
        End If
    End Sub

    Private Function SaveCompleteChanges() As Boolean
        Dim cRec As BO.j75DrillDownTemplate = Nothing

        If Me.CurrentJ75ID = 0 Then
            cRec = New BO.j75DrillDownTemplate
            cRec.j75MasterPrefix = Me.CurrentMasterPrefix
        Else
            cRec = Master.Factory.j75DrillDownTemplateBL.Load(Me.CurrentJ75ID)
        End If


        Dim intJ75ID As Integer = SaveRecord(cRec)
        If intJ75ID > 0 Then
            Master.DataPID = intJ75ID.ToString
            SetupJ75Combo()
            Return True
        Else
            Return False
        End If
    End Function

    Private Function SaveRecord(cRec As BO.j75DrillDownTemplate) As Integer
        With cRec
            .x29ID = Me.CurrentX29ID
            .j75Name = j75Name.Text
            If Me.j75Level1.SelectedValue = "" Then
                .j75Level1 = Nothing
            Else
                .j75Level1 = CType(Me.j75Level1.SelectedValue, BO.PivotRowColumnFieldType)
            End If
            If Me.j75Level2.SelectedValue = "" Then
                .j75Level2 = Nothing
            Else
                .j75Level2 = CType(Me.j75Level2.SelectedValue, BO.PivotRowColumnFieldType)
            End If
            If Me.j75Level3.SelectedValue = "" Then
                .j75Level2 = Nothing
            Else
                .j75Level3 = CType(Me.j75Level3.SelectedValue, BO.PivotRowColumnFieldType)
            End If
            If Me.j75Level4.SelectedValue = "" Then
                .j75Level4 = Nothing
            Else
                .j75Level4 = CType(Me.j75Level4.SelectedValue, BO.PivotRowColumnFieldType)
            End If
            Dim s As String = ""
            For Each it As Telerik.Web.UI.RadListBoxItem In colsDest.Items
                s += "," & it.Value
            Next
            
        End With
        Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()
        If roles1.ErrorMessage <> "" Then
            Master.Notify(roles1.ErrorMessage, 2)
            Return False
        End If
        Dim lisJ76 As New List(Of BO.j76DrillDownTemplate_Item), x As Integer = 0
        For Each it As Telerik.Web.UI.RadListBoxItem In colsDest.Items
            Dim c As New BO.j76DrillDownTemplate_Item
            c.j76PivotSumFieldType = CInt(it.Value)
            x += 1
            c.j76Ordinary = x
            lisJ76.Add(c)
        Next

        If Master.Factory.j75DrillDownTemplateBL.Save(cRec, lisJ76, lisX69) Then
            Return Master.Factory.j75DrillDownTemplateBL.LastSavedPID
        Else
            Master.Notify(Master.Factory.j75DrillDownTemplateBL.ErrorMessage, 2)
            Return 0
        End If
    End Function

    Private Sub j75ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles j75ID.SelectedIndexChanged
        Handle_ChangeJ75()
    End Sub
End Class