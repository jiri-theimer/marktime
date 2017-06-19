Public Class x18_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord
    Private _lisX20 As IEnumerable(Of BO.x20EntiyToCategory) = Nothing

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
            hidGUID_x16.Value = BO.BAS.GetGUID()
            hidGUID_x20.Value = BO.BAS.GetGUID()
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
        _lisX20 = Master.Factory.x18EntityCategoryBL.GetList_x20(Master.DataPID)

        Dim cRec As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Master.DataPID)
        With cRec
            Me.x23ID.SelectedValue = .x23ID.ToString
            Me.x18Name.Text = .x18Name
            Me.x18NameShort.Text = .x18NameShort
            Me.x18Ordinary.Value = .x18Ordinary

            Me.j02ID_Owner.Value = .j02ID_Owner.ToString
            Me.j02ID_Owner.Text = .Owner

            Me.x18IsColors.Checked = .x18IsColors
            Me.x18IsManyItems.Checked = .x18IsManyItems
            Me.x18IsClueTip.Checked = .x18IsClueTip
            Me.x18Icon.Text = .x18Icon
            Me.x18ReportCodes.Text = .x18ReportCodes
            Master.Timestamp = .Timestamp

            If .x23ID <> 0 Then
                Dim cX23 As BO.x23EntityField_Combo = Master.Factory.x23EntityField_ComboBL.Load(.x23ID)
                If cX23.x23Ordinary = -666 Then Me.x23ID.Enabled = False
            End If

            roles1.InhaleInitialData(.PID)
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

        RefreshItems()

        Dim lisX16 As IEnumerable(Of BO.x16EntityCategory_FieldSetting) = Master.Factory.x18EntityCategoryBL.GetList_x16(Master.DataPID)
        Master.Factory.p85TempBoxBL.Truncate(hidGUID_x16.Value)
        For Each c In lisX16
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = hidGUID_x16.Value
                .p85Prefix = "x16"
                .p85DataPID = c.x16ID
                .p85OtherKey1 = c.x16Ordinary
                .p85FreeText01 = c.x16Field
                .p85FreeText02 = c.x16Name
                .p85Message = c.x16DataSource
                .p85FreeBoolean01 = c.x16IsEntryRequired
                .p85FreeBoolean02 = c.x16IsGridField
                .p85FreeBoolean03 = c.x16IsFixedDataSource
                .p85FreeNumber01 = c.x16TextboxWidth
                .p85FreeNumber02 = c.x16TextboxHeight
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
        RefreshTempX16()
        Dim lisX20 As IEnumerable(Of BO.x20EntiyToCategory) = Master.Factory.x18EntityCategoryBL.GetList_x20(Master.DataPID)
        Master.Factory.p85TempBoxBL.Truncate(hidGUID_x20.Value)
        For Each c In lisX20
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = hidGUID_x20.Value
                .p85Prefix = "x20"
                .p85DataPID = c.x20ID
                .p85OtherKey1 = c.x29ID
                .p85FreeText01 = c.x20Name
                .p85OtherKey2 = CInt(c.x20EntryModeFlag)
                .p85OtherKey3 = CInt(c.x20GridColumnFlag)
                .p85FreeBoolean01 = c.x20IsEntryRequired
                .p85FreeBoolean02 = c.x20IsMultiSelect
                .p85FreeBoolean03 = c.x20IsClosed
                .p85FreeNumber01 = c.x20Ordinary
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
        RefreshTempX20()

       
    End Sub
    Private Sub RefreshTempX16()
        rpX16.DataSource = Master.Factory.p85TempBoxBL.GetList(hidGUID_x16.Value)
        rpX16.DataBind()
    End Sub
    Private Sub RefreshTempX20()
        rpX20.DataSource = Master.Factory.p85TempBoxBL.GetList(hidGUID_x20.Value)
        rpX20.DataBind()
    End Sub
    Private Sub RefreshItems()
        If Me.CurrentX23ID <> 0 Then
            Dim lis As IEnumerable(Of BO.x25EntityField_ComboValue) = Master.Factory.x25EntityField_ComboValueBL.GetList(New BO.myQueryX25(Me.CurrentX23ID))
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
        SaveTempX20()

        If Master.DataPID = 0 And hidTempX23ID.Value = "" Then
            Master.Notify("Musíte kliknout na tlačítko [Potvrdit].", NotifyLevel.WarningMessage)
            Return
        End If
        Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()


        Dim lisX16 As New List(Of BO.x16EntityCategory_FieldSetting)
        For Each cTMP In Master.Factory.p85TempBoxBL.GetList(hidGUID_x16.Value)
            Dim c As New BO.x16EntityCategory_FieldSetting
            With cTMP
                c.x16Field = .p85FreeText01
                c.x16Name = .p85FreeText02
                c.x16Ordinary = .p85OtherKey1
                c.x16IsEntryRequired = .p85FreeBoolean01
                c.x16IsGridField = .p85FreeBoolean02
                c.x16DataSource = .p85Message
                c.x16IsFixedDataSource = .p85FreeBoolean03
                c.x16TextboxWidth = .p85FreeNumber01
                c.x16TextboxHeight = .p85FreeNumber02
            End With
            lisX16.Add(c)
        Next
        Dim lisX20 As New List(Of BO.x20EntiyToCategory)
        For Each cTMP In Master.Factory.p85TempBoxBL.GetList(hidGUID_x20.Value)
            Dim c As New BO.x20EntiyToCategory
            c.x20ID = cTMP.p85DataPID
            c.x18ID = Master.DataPID
            c.x29ID = cTMP.p85OtherKey1
            c.x20Name = cTMP.p85FreeText01
            c.x20IsEntryRequired = cTMP.p85FreeBoolean01
            c.x20IsMultiSelect = cTMP.p85FreeBoolean02
            c.x20EntryModeFlag = cTMP.p85OtherKey2
            c.x20GridColumnFlag = cTMP.p85OtherKey3
            c.x20IsClosed = cTMP.p85FreeBoolean03
            c.x20Ordinary = cTMP.p85FreeNumber01
            lisX20.Add(c)
        Next


        With Master.Factory.x18EntityCategoryBL
            Dim cRec As BO.x18EntityCategory = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.x18EntityCategory)
            cRec.x18Name = Me.x18Name.Text
            cRec.x18NameShort = Me.x18NameShort.Text
            cRec.x18Ordinary = BO.BAS.IsNullInt(Me.x18Ordinary.Value)
            cRec.x23ID = Me.CurrentX23ID
            cRec.x18Icon = Me.x18Icon.Text
            cRec.x18IsClueTip = Me.x18IsClueTip.Checked
            cRec.x18ReportCodes = Me.x18ReportCodes.Text

            cRec.x18IsColors = Me.x18IsColors.Checked
            cRec.x18IsManyItems = Me.x18IsManyItems.Checked
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
            cRec.j02ID_Owner = BO.BAS.IsNullInt(Me.j02ID_Owner.Value)



            If .Save(cRec, lisX20, lisX69, lisX16) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("x18-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub x18_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        
        
        opg1.Visible = False
        cmdConfirmOpg1.Visible = False
        Me.rpX25.Visible = True
        lblX23ID.Visible = True
        Me.x23ID.Visible = True

        If Master.DataPID = 0 Then
            If hidGUID_confirm.Value = "" Then
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

    'Private Sub Handle_ChangeX29ID()
    '    Dim mq As New BO.myQuery, lis As New List(Of BO.x22EntiyCategory_Binding)
    '    mq.Closed = BO.BooleanQueryMode.NoQuery
    '    For Each intX29ID As Integer In basUI.GetCheckedItems(Me.x29IDs)
    '        Select Case CType(intX29ID, BO.x29IdEnum)
    '            Case BO.x29IdEnum.p28Contact
    '                For Each c In Master.Factory.p29ContactTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 329, c.p29Name & " (Klient)"))
    '                Next
    '            Case BO.x29IdEnum.p41Project
    '                For Each c In Master.Factory.p42ProjectTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 342, c.p42Name & " (Projekt)"))
    '                Next
    '            Case BO.x29IdEnum.j02Person
    '                For Each c In Master.Factory.j07PersonPositionBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 107, c.j07Name & " (Osoba)"))
    '                Next
    '            Case BO.x29IdEnum.o23Notepad
    '                For Each c In Master.Factory.o24NotepadTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 224, c.o24Name & "(Dokument)"))
    '                Next
    '            Case BO.x29IdEnum.p91Invoice
    '                For Each c In Master.Factory.p92InvoiceTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 392, c.p92Name & " (Faktura)"))
    '                Next
    '            Case BO.x29IdEnum.p90Proforma
    '                For Each c In Master.Factory.p89ProformaTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 389, c.p89Name & " (Zálohová faktura)"))
    '                Next
    '            Case BO.x29IdEnum.p31Worksheet
    '                For Each c In Master.Factory.p34ActivityGroupBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 334, c.p34Name & " (Worksheet)"))
    '                Next                    
    '            Case BO.x29IdEnum.p56Task
    '                For Each c In Master.Factory.p57TaskTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 357, c.p57Name & " (Úkol)"))
    '                Next
    '            Case BO.x29IdEnum.o22Milestone
    '                For Each c In Master.Factory.o21MilestoneTypeBL.GetList(mq)
    '                    lis.Add(x22rec(c.PID, 221, c.o21Name & " (Událost v kalendáři)"))
    '                Next
    '            Case Else
    '        End Select

    '    Next
    '    rp1.DataSource = lis
    '    rp1.DataBind()
    'End Sub
    'Private Function x22rec(intEntityTypePID As Integer, intX29ID As Integer, strEntityTypeAlias As String) As BO.x22EntiyCategory_Binding
    '    Dim c As New BO.x22EntiyCategory_Binding
    '    c.x18ID = Master.DataPID
    '    c.x22EntityTypePID = intEntityTypePID
    '    c.x29ID_EntityType = intX29ID
    '    c.EntityTypeAlias = strEntityTypeAlias
    '    Return c
    'End Function

    
    

    Private Sub x23ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles x23ID.SelectedIndexChanged
        RefreshItems()
    End Sub

    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        RefreshItems()
    End Sub

    Private Sub cmdConfirmOpg1_Click(sender As Object, e As EventArgs) Handles cmdConfirmOpg1.Click
        Me.hidGUID_confirm.Value = BO.BAS.GetGUID
        If opg1.SelectedValue = "1" Then
            Dim c As New BO.x23EntityField_Combo
            c.x23Name = hidGUID_confirm.Value
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
        cRec.p85GUID = hidGUID_x16.Value
        cRec.p85Prefix = "x16"
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
            CType(e.Item.FindControl("x16TextboxWidth"), TextBox).Text = .p85FreeNumber01
            CType(e.Item.FindControl("x16TextboxHeight"), TextBox).Text = .p85FreeNumber02
        End With
    End Sub
    Private Sub SaveTempX16()
        Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(hidGUID_x16.Value)
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
                .p85FreeNumber01 = BO.BAS.IsNullInt(CType(ri.FindControl("x16TextboxWidth"), TextBox).Text)
                .p85FreeNumber02 = BO.BAS.IsNullInt(CType(ri.FindControl("x16TextboxHeight"), TextBox).Text)
            End With
            Master.Factory.p85TempBoxBL.Save(cRec)
        Next
    End Sub

    Private Sub rpX20_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpX20.ItemCommand
        SaveTempX20()
        Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(BO.BAS.IsNullInt(e.CommandArgument))
        If e.CommandName = "delete" Then
            If Master.Factory.p85TempBoxBL.Delete(cRec) Then

            End If
        End If
        RefreshTempX20()
    End Sub

    Private Sub rpX20_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpX20.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)
        With CType(e.Item.FindControl("del"), ImageButton)
            .CommandArgument = cRec.PID.ToString
            .CommandName = "delete"
        End With
        Dim cbx0 As DropDownList = CType(e.Item.FindControl("x29ID"), DropDownList)
        With cRec
            CType(e.Item.FindControl("p85id"), HiddenField).Value = .PID.ToString
            CType(e.Item.FindControl("x20ID"), HiddenField).Value = .p85DataPID.ToString
            basUI.SelectDropdownlistValue(cbx0, .p85OtherKey1.ToString)
            CType(e.Item.FindControl("x20Name"), TextBox).Text = .p85FreeText01
            CType(e.Item.FindControl("x20IsEntryRequired"), CheckBox).Checked = .p85FreeBoolean01
            CType(e.Item.FindControl("x20IsMultiselect"), CheckBox).Checked = .p85FreeBoolean02
            CType(e.Item.FindControl("x20IsClosed"), CheckBox).Checked = .p85FreeBoolean03
            CType(e.Item.FindControl("x20Ordinary"), Telerik.Web.UI.RadNumericTextBox).Value = .p85FreeNumber01

            Dim cbx1 As DropDownList = CType(e.Item.FindControl("x20EntryModeFlag"), DropDownList), cbx2 As DropDownList = CType(e.Item.FindControl("x20GridColumnFlag"), DropDownList)
            basUI.SelectDropdownlistValue(cbx1, .p85OtherKey2.ToString)
            basUI.SelectDropdownlistValue(cbx2, .p85OtherKey3.ToString)
            Dim strName As String = Me.x18Name.Text
            If Trim(Me.x18NameShort.Text) <> "" Then strName = Me.x18NameShort.Text

            cbx2.Items.FindByValue("2").Text = String.Format("Sloupec [{0}] v přehledu položek štítku", IIf(cRec.p85FreeText01 = "", cbx0.SelectedItem.Text, cRec.p85FreeText01))
            Select Case CType(cRec.p85OtherKey1, BO.x29IdEnum)
                Case BO.x29IdEnum.p28Contact
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] v kartě klienta", strName)
                    cbx1.Items.FindByValue("2").Text = String.Format("Vazbu vyplňovat vyhledavačem klienta v záznamu položky [{0}]", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu klientů", strName)

                Case BO.x29IdEnum.p41Project
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] v kartě projektu", strName)
                    cbx1.Items.FindByValue("2").Text = String.Format("Vazbu vyplňovat vyhledavačem projektu v záznamu položky [{0}]", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu projektů", strName)
                Case BO.x29IdEnum.j02Person
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] v kartě osoby", strName)
                    cbx1.Items.FindByValue("2").Text = String.Format("Vazbu vyplňovat vyhledavačem osoby v záznamu položky [{0}]", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu osob", strName)
                Case BO.x29IdEnum.p56Task
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] v kartě úkolu", strName)
                    cbx1.Items.FindByValue("2").Text = String.Format("Vazbu vyplňovat vyhledavačem úkolu v záznamu položky [{0}]", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu úkolů", strName)
                Case BO.x29IdEnum.o23Notepad
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] v kartě dokumentu", strName)
                    cbx1.Items.FindByValue("2").Text = String.Format("Vazbu vyplňovat vyhledavačem dokumentu v záznamu položky [{0}]", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu dokumentů", strName)
                Case BO.x29IdEnum.p31Worksheet
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] ve formuláři pro zápis worksheet úkonu", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu worksheet úkonů", strName)
                    cbx2.Items.FindByValue("2").Enabled = False
                    cbx2.Items.FindByValue("3").Enabled = False
                Case BO.x29IdEnum.o22Milestone
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] ve formuláři pro kalendářovou událost", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Pole [{0}] v kalendáři událostí", strName)
                    cbx2.Items.FindByValue("2").Enabled = False
                    cbx2.Items.FindByValue("3").Enabled = False
                Case BO.x29IdEnum.p91Invoice
                    cbx1.Items.FindByValue("1").Text = String.Format("Vazbu vyplňovat z combo nabídky položek [{0}] v kartě faktury", strName)
                    cbx2.Items.FindByValue("1").Text = String.Format("Sloupec [{0}] v přehledu faktur", strName)
                    cbx2.Items.FindByValue("2").Enabled = False
                    cbx2.Items.FindByValue("3").Enabled = False
            End Select

            CType(e.Item.FindControl("x20EntityTypePID"), HiddenField).Value = .p85OtherKey4.ToString
            CType(e.Item.FindControl("x29ID_EntityType"), HiddenField).Value = .p85OtherKey5.ToString
        End With
       
        ''With CType(e.Item.FindControl("chkEntityType"), CheckBox)
        ''    .Text = cRec.EntityTypeAlias
        ''    If Not _lisX20 Is Nothing Then
        ''        If _lisX20.Where(Function(p) p.x22EntityTypePID = cRec.x22EntityTypePID And p.x29ID_EntityType = cRec.x29ID_EntityType).Count > 0 Then
        ''            .Checked = True
        ''        End If
        ''        If _lisX22.Where(Function(p) p.x22EntityTypePID = cRec.x22EntityTypePID And p.x29ID_EntityType = cRec.x29ID_EntityType And p.x22IsEntryRequired = True).Count > 0 Then
        ''            CType(e.Item.FindControl("x22IsEntryRequired"), CheckBox).Checked = True
        ''        End If
        ''    End If
        ''End With
    End Sub
    Private Sub SaveTempX20()
        Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(hidGUID_x20.Value)
        For Each ri As RepeaterItem In rpX20.Items
            Dim intP85ID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("p85id"), HiddenField).Value)
            Dim cRec As BO.p85TempBox = lisTEMP.Where(Function(p) p.PID = intP85ID)(0)
            With cRec
                .p85OtherKey1 = CInt(CType(ri.FindControl("x29ID"), DropDownList).SelectedValue)
                .p85FreeText01 = CType(ri.FindControl("x20Name"), TextBox).Text
                .p85OtherKey2 = CInt(CType(ri.FindControl("x20EntryModeFlag"), DropDownList).SelectedValue)
                .p85OtherKey3 = CInt(CType(ri.FindControl("x20GridColumnFlag"), DropDownList).SelectedValue)
                .p85FreeBoolean01 = CType(ri.FindControl("x20IsEntryRequired"), CheckBox).Checked
                .p85FreeBoolean02 = CType(ri.FindControl("x20IsMultiselect"), CheckBox).Checked
                .p85FreeBoolean03 = CType(ri.FindControl("x20IsClosed"), CheckBox).Checked
                .p85FreeNumber01 = BO.BAS.IsNullInt(CType(ri.FindControl("x20Ordinary"), Telerik.Web.UI.RadNumericTextBox).Value)
            End With
            Master.Factory.p85TempBoxBL.Save(cRec)
        Next
    End Sub

    Private Sub cmdAddX20_Click(sender As Object, e As EventArgs) Handles cmdAddX20.Click
        If Me.x29ID_addX20.SelectedValue = "" Then
            Master.Notify("Musíte vybrat entitu.", NotifyLevel.WarningMessage)
            Return
        End If
        SaveTempX20()
        Dim cRec As New BO.p85TempBox()
        cRec.p85GUID = hidGUID_x20.Value
        cRec.p85Prefix = "x20"
        cRec.p85OtherKey1 = Me.x29ID_addX20.SelectedValue
        Master.Factory.p85TempBoxBL.Save(cRec)

        RefreshTempX20()
        x29ID_addX20.SelectedIndex = 0
    End Sub
End Class