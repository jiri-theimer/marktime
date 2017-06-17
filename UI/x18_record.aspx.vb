Public Class x18_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord
    Private _lisX22 As IEnumerable(Of BO.x22EntiyCategory_Binding) = Nothing

    Private Sub x18_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Public ReadOnly Property CurrentX23ID As Integer
        Get
            If hidTempX23ID.Value <> "" Then
                Return BO.BAS.IsNullInt(Me.hidTempX23ID.Value)
            Else
                Return BO.BAS.IsNullInt(Me.x23ID.SelectedValue)
            End If
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        roles1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID()
            With Master
                .HeaderIcon = "Images/label_32.png"
                .HeaderText = "Nastavení štítku"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Not .Factory.TestPermission(BO.x53PermValEnum.GR_X18_Admin) Then
                    If .DataPID <> 0 Then
                        Server.Transfer("x18_items.aspx?pid=" & .DataPID.ToString, False)
                    Else
                        .StopPage("Pro správu štítků nemáte oprávnění.")
                    End If
                End If
                .neededPermission = BO.x53PermValEnum.GR_X18_Admin


                Dim lis As IEnumerable(Of BO.x23EntityField_Combo) = .Factory.x23EntityField_ComboBL.GetList(New BO.myQuery)
                Me.x23ID.DataSource = lis
                Me.x23ID.DataBind()

            End With

            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then
            Me.j02ID_Owner.Value = Master.Factory.SysUser.j02ID.ToString
            Me.j02ID_Owner.Text = Master.Factory.SysUser.PersonDesc
            Return
        End If
        _lisX22 = Master.Factory.x18EntityCategoryBL.GetList_x22(Master.DataPID)

        Dim cRec As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Master.DataPID)
        With cRec
            Me.x18IsAllEntityTypes.Checked = .x18IsAllEntityTypes
            Me.x23ID.SelectedValue = .x23ID.ToString
            Me.x18Name.Text = .x18Name
            Me.x18Ordinary.Value = .x18Ordinary
            Me.x18IsMultiSelect.Checked = .x18IsMultiSelect

            Me.j02ID_Owner.Value = .j02ID_Owner.ToString
            Me.j02ID_Owner.Text = .Owner

            Me.x18IsColors.Checked = .x18IsColors
            Master.Timestamp = .Timestamp

            If .x23ID <> 0 Then
                Dim cX23 As BO.x23EntityField_Combo = Master.Factory.x23EntityField_ComboBL.Load(.x23ID)
                If cX23.x23Ordinary = -666 Then Me.x23ID.Enabled = False
            End If

            roles1.InhaleInitialData(.PID)
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With
        basUI.CheckItems(Me.x29IDs, Master.Factory.x18EntityCategoryBL.GetList_x29(Master.DataPID).Select(Function(p) p.PID).ToList)

        Handle_ChangeX29ID()

        RefreshItems()

        Dim lisX16 As IEnumerable(Of BO.x16EntityCategory_FieldSetting) = Master.Factory.x18EntityCategoryBL.GetList_x16(Master.DataPID)
        Master.Factory.p85TempBoxBL.Truncate(ViewState("guid"))
        For Each c In lisX16
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = ViewState("guid")
                .p85DataPID = c.x16ID
                If Not Page.IsPostBack Then
                    If Master.IsRecordClone Then
                        .p85DataPID = 0
                    End If
                End If
                .p85OtherKey1 = c.x16Ordinary
                .p85FreeText01 = c.x16Field
                .p85FreeText02 = c.x16Name
                .p85Message = c.x16DataSource
                .p85FreeBoolean01 = c.x16IsEntryRequired
                .p85FreeBoolean02 = c.x16IsGridField
                .p85FreeBoolean03 = c.x16IsFixedDataSource
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
        RefreshTempX16()


    End Sub
    Private Sub RefreshTempX16()
        rpX16.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        rpX16.DataBind()
    End Sub
    Private Sub RefreshItems()
        If Me.CurrentX23ID <> 0 Then
            Dim lis As IEnumerable(Of BO.x25EntityField_ComboValue) = Master.Factory.x25EntityField_ComboValueBL.GetList(Me.CurrentX23ID)
            If lis.Count <= 50 Then
                rpX25.DataSource = lis
            Else
                lblItemsMessage.Text = "Štítek má více než 50 položek. Jejich správa by na tomto místě byla nepřehledná."
                rpX25.DataSource = Nothing
            End If
        Else
            rpX25.DataSource = Nothing
        End If
        rpX25.DataBind()



    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.x18EntityCategoryBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("x18-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        roles1.SaveCurrentTempData()
        SaveTempX16()

        If Master.DataPID = 0 And hidTempX23ID.Value = "" Then
            Master.Notify("Musíte kliknout na tlačítko [Potvrdit].", NotifyLevel.WarningMessage)
            Return
        End If
        Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()


        Dim lisX16 As New List(Of BO.x16EntityCategory_FieldSetting)
        For Each cTMP In Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
            Dim c As New BO.x16EntityCategory_FieldSetting
            With cTMP
                c.x16Field = .p85FreeText01
                c.x16Name = .p85FreeText02
                c.x16Ordinary = .p85OtherKey1
                c.x16IsEntryRequired = .p85FreeBoolean01
                c.x16IsGridField = .p85FreeBoolean02
                c.x16DataSource = .p85Message
                c.x16IsFixedDataSource = .p85FreeBoolean03
            End With
            lisX16.Add(c)
        Next


        With Master.Factory.x18EntityCategoryBL
            Dim cRec As BO.x18EntityCategory = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.x18EntityCategory)
            cRec.x18Name = Me.x18Name.Text
            cRec.x18Ordinary = BO.BAS.IsNullInt(Me.x18Ordinary.Value)
            cRec.x23ID = Me.CurrentX23ID
            cRec.x18IsMultiSelect = Me.x18IsMultiSelect.Checked
            cRec.x18IsRequired = Me.x18IsRequired.Checked
            cRec.x18IsAllEntityTypes = Me.x18IsAllEntityTypes.Checked
            cRec.x18IsRequired = Me.x18IsRequired.Checked
            cRec.x18IsColors = Me.x18IsColors.Checked
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
            cRec.j02ID_Owner = BO.BAS.IsNullInt(Me.j02ID_Owner.Value)
            Dim x29IDs As List(Of Integer) = basUI.GetCheckedItems(Me.x29IDs)
            Dim lisX22 As New List(Of BO.x22EntiyCategory_Binding)
            For Each ri As RepeaterItem In rp1.Items
                If CType(ri.FindControl("chkEntityType"), CheckBox).Checked Then
                    Dim c As New BO.x22EntiyCategory_Binding
                    c.x22EntityTypePID = CInt(CType(ri.FindControl("x22EntityTypePID"), HiddenField).Value)
                    c.x29ID_EntityType = CInt(CType(ri.FindControl("x29ID_EntityType"), HiddenField).Value)
                    c.x18ID = Master.DataPID
                    c.x22IsEntryRequired = CType(ri.FindControl("x22IsEntryRequired"), CheckBox).Checked
                    lisX22.Add(c)
                End If

            Next

            If .Save(cRec, x29IDs, lisX22, lisX69, lisX16) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("x18-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub x18_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        
        Me.panEntityTypes.Visible = Not Me.x18IsAllEntityTypes.Checked
        Me.x18IsRequired.Visible = Me.x18IsAllEntityTypes.Checked

        opg1.Visible = False
        cmdConfirmOpg1.Visible = False
        Me.rpX25.Visible = True
        lblX23ID.Visible = True
        Me.x23ID.Visible = True

        If Master.DataPID = 0 Then
            If hidGUID.Value = "" Then
                opg1.Visible = True
                cmdConfirmOpg1.Visible = True
            End If
            If opg1.SelectedValue = "1" Then
                Me.lblX23ID.Visible = False
                Me.x23ID.Visible = False
                
            End If
        End If

        
        If Me.hidTempX23ID.Value <> "" Then
            lblX23ID.Visible = False
            Me.x23ID.Visible = False
        End If

    End Sub

    Private Sub Handle_ChangeX29ID()
        Dim mq As New BO.myQuery, lis As New List(Of BO.x22EntiyCategory_Binding)
        mq.Closed = BO.BooleanQueryMode.NoQuery
        For Each intX29ID As Integer In basUI.GetCheckedItems(Me.x29IDs)
            Select Case CType(intX29ID, BO.x29IdEnum)
                Case BO.x29IdEnum.p28Contact
                    For Each c In Master.Factory.p29ContactTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 329, c.p29Name & " (Klient)"))
                    Next
                Case BO.x29IdEnum.p41Project
                    For Each c In Master.Factory.p42ProjectTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 342, c.p42Name & " (Projekt)"))
                    Next
                Case BO.x29IdEnum.j02Person
                    For Each c In Master.Factory.j07PersonPositionBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 107, c.j07Name & " (Osoba)"))
                    Next
                Case BO.x29IdEnum.o23Notepad
                    For Each c In Master.Factory.o24NotepadTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 224, c.o24Name & "(Dokument)"))
                    Next
                Case BO.x29IdEnum.p91Invoice
                    For Each c In Master.Factory.p92InvoiceTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 392, c.p92Name & " (Faktura)"))
                    Next
                Case BO.x29IdEnum.p90Proforma
                    For Each c In Master.Factory.p89ProformaTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 389, c.p89Name & " (Zálohová faktura)"))
                    Next
                Case BO.x29IdEnum.p31Worksheet
                    For Each c In Master.Factory.p34ActivityGroupBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 334, c.p34Name & " (Worksheet)"))
                    Next                    
                Case BO.x29IdEnum.p56Task
                    For Each c In Master.Factory.p57TaskTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 357, c.p57Name & " (Úkol)"))
                    Next
                Case BO.x29IdEnum.o22Milestone
                    For Each c In Master.Factory.o21MilestoneTypeBL.GetList(mq)
                        lis.Add(x22rec(c.PID, 221, c.o21Name & " (Událost v kalendáři)"))
                    Next
                Case Else
            End Select

        Next
        rp1.DataSource = lis
        rp1.DataBind()
    End Sub
    Private Function x22rec(intEntityTypePID As Integer, intX29ID As Integer, strEntityTypeAlias As String) As BO.x22EntiyCategory_Binding
        Dim c As New BO.x22EntiyCategory_Binding
        c.x18ID = Master.DataPID
        c.x22EntityTypePID = intEntityTypePID
        c.x29ID_EntityType = intX29ID
        c.EntityTypeAlias = strEntityTypeAlias
        Return c
    End Function

    Private Sub x29IDs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles x29IDs.SelectedIndexChanged
        Handle_ChangeX29ID()
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.x22EntiyCategory_Binding = CType(e.Item.DataItem, BO.x22EntiyCategory_Binding)
        CType(e.Item.FindControl("x22EntityTypePID"), HiddenField).Value = cRec.x22EntityTypePID.ToString
        CType(e.Item.FindControl("x29ID_EntityType"), HiddenField).Value = cRec.x29ID_EntityType.ToString
        With CType(e.Item.FindControl("chkEntityType"), CheckBox)
            .Text = cRec.EntityTypeAlias
            If Not _lisX22 Is Nothing Then
                If _lisX22.Where(Function(p) p.x22EntityTypePID = cRec.x22EntityTypePID And p.x29ID_EntityType = cRec.x29ID_EntityType).Count > 0 Then
                    .Checked = True
                End If
                If _lisX22.Where(Function(p) p.x22EntityTypePID = cRec.x22EntityTypePID And p.x29ID_EntityType = cRec.x29ID_EntityType And p.x22IsEntryRequired = True).Count > 0 Then
                    CType(e.Item.FindControl("x22IsEntryRequired"), CheckBox).Checked = True
                End If
            End If
        End With
    End Sub

    Private Sub x18IsAllEntityTypes_CheckedChanged(sender As Object, e As EventArgs) Handles x18IsAllEntityTypes.CheckedChanged
        Handle_ChangeX29ID()
    End Sub

    Private Sub x23ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles x23ID.SelectedIndexChanged
        RefreshItems()
    End Sub

    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        RefreshItems()
    End Sub

    Private Sub cmdConfirmOpg1_Click(sender As Object, e As EventArgs) Handles cmdConfirmOpg1.Click
        Me.hidGUID.Value = BO.BAS.GetGUID
        If opg1.SelectedValue = "1" Then
            Dim c As New BO.x23EntityField_Combo
            c.x23Name = hidGUID.Value
            c.x23Ordinary = -666
            If Master.Factory.x23EntityField_ComboBL.Save(c) Then
                hidTempX23ID.Value = Master.Factory.x23EntityField_ComboBL.LastSavedPID.ToString

            End If
        End If

        RefreshItems()

    End Sub

    Private Sub cmdAddX69_Click(sender As Object, e As EventArgs) Handles cmdAddX69.Click
        roles1.AddNewRow()
    End Sub

    Private Sub rpX16_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpX16.ItemCommand
        SaveTempX16()
        Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(BO.BAS.IsNullInt(e.CommandArgument))
        If e.CommandName = "delete" Then
            If Master.Factory.p85TempBoxBL.Delete(cRec) Then

            End If
        End If
        RefreshTempX16()
    End Sub

    Private Sub cmdNewX16_Click(sender As Object, e As EventArgs) Handles cmdNewX16.Click
        SaveTempX16()
        Dim cRec As New BO.p85TempBox()
        cRec.p85GUID = ViewState("guid")
        Master.Factory.p85TempBoxBL.Save(cRec)
        RefreshTempX16()
    End Sub

    Private Sub rpX16_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpX16.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)

        With CType(e.Item.FindControl("del"), ImageButton)
            .CommandArgument = cRec.PID.ToString
            .CommandName = "delete"
        End With

        With cRec
            CType(e.Item.FindControl("p85id"), HiddenField).Value = .PID.ToString
            basUI.SelectDropdownlistValue(CType(e.Item.FindControl("x16Field"), DropDownList), .p85FreeText01)
            CType(e.Item.FindControl("x16Name"), TextBox).Text = .p85FreeText02
            CType(e.Item.FindControl("x16Ordinary"), Telerik.Web.UI.RadNumericTextBox).Value = .p85OtherKey1
            CType(e.Item.FindControl("x16IsEntryRequired"), CheckBox).Checked = .p85FreeBoolean01
            CType(e.Item.FindControl("x16IsGridField"), CheckBox).Checked = .p85FreeBoolean02
            CType(e.Item.FindControl("x16IsFixedDataSource"), CheckBox).Checked = .p85FreeBoolean03
            CType(e.Item.FindControl("x16DataSource"), TextBox).Text = .p85Message

        End With
    End Sub
    Private Sub SaveTempX16()
        Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        For Each ri As RepeaterItem In rpX16.Items
            Dim intP85ID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("p85id"), HiddenField).Value)
            Dim cRec As BO.p85TempBox = lisTEMP.Where(Function(p) p.PID = intP85ID)(0)
            With cRec
                .p85FreeText01 = CType(ri.FindControl("x16Field"), DropDownList).SelectedValue
                .p85FreeText02 = CType(ri.FindControl("x16Name"), TextBox).Text
                .p85OtherKey1 = BO.BAS.IsNullInt(CType(ri.FindControl("x16Ordinary"), Telerik.Web.UI.RadNumericTextBox).Value)
                .p85FreeBoolean01 = CType(ri.FindControl("x16IsEntryRequired"), CheckBox).Checked
                .p85FreeBoolean02 = CType(ri.FindControl("x16IsGridField"), CheckBox).Checked
                .p85FreeBoolean03 = CType(ri.FindControl("x16IsFixedDataSource"), CheckBox).Checked
                .p85Message = CType(ri.FindControl("x16DataSource"), TextBox).Text
            End With
            Master.Factory.p85TempBoxBL.Save(cRec)
        Next
    End Sub
End Class