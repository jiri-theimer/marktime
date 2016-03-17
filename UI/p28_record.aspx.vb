Public Class p28_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p28_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p28_record"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory
        roles1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            ViewState("guid_o37") = BO.BAS.GetGUID()
            ViewState("guid_o32") = BO.BAS.GetGUID()

            With Master
                .HeaderIcon = "Images/contact_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Or .IsRecordClone Then
                    'oprávnění pro zakládání nových kontaktů
                    .neededPermission = BO.x53PermValEnum.GR_P28_Creator
                    .neededPermissionIfSecond = BO.x53PermValEnum.GR_P28_Draft_Creator
                End If
            End With
            Me.p29ID.DataSource = Master.Factory.p29ContactTypeBL.GetList(New BO.myQuery)
            Me.p29ID.DataBind()

            basUI.SetupP87Combo(Master.Factory, Me.p87ID)

            SetupPriceList()
            Me.p58IDs.DataSource = Master.Factory.p58ProductBL.GetList(New BO.myQuery)
            Me.p58IDs.DataBind()
            
            If Me.p92id.Visible Then
                Me.p92id.DataSource = Master.Factory.p92InvoiceTypeBL.GetList(New BO.myQuery).Where(Function(p) p.p92InvoiceType = BO.p92InvoiceTypeENUM.ClientInvoice)
                Me.p92id.DataBind()
            End If
           
            RefreshRecord()

            If Master.IsRecordClone Then
                Master.DataPID = 0
            End If

            If Master.DataPID = 0 Then
                Master.HeaderText = "Založit klienta"
            End If
        End If



    End Sub
    Private Sub SetupPriceList()
        Dim lis As IEnumerable(Of BO.p51PriceList) = Master.Factory.p51PriceListBL.GetList(New BO.myQuery)
        Me.p51ID_Billing.DataSource = lis.Where(Function(p) p.p51IsInternalPriceList = False And p.p51IsMasterPriceList = False And p.p51IsCustomTailor = False)
        Me.p51ID_Billing.DataBind()
        Me.p51ID_Internal.DataSource = lis.Where(Function(p) p.p51IsInternalPriceList = True And p.p51IsMasterPriceList = False And p.p51IsCustomTailor = False)
        Me.p51ID_Internal.DataBind()
    End Sub
    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            TryInhaleInitialData()
            Handle_FF()
            Me.j02ID_Owner.Value = Master.Factory.SysUser.j02ID.ToString
            Me.j02ID_Owner.Text = Master.Factory.SysUser.PersonDesc
            Me.p28IsDraft.Visible = True
            Return
        Else
            Me.p28IsDraft.Visible = False
        End If

        Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
        If cRec Is Nothing Then Master.StopPage("Záznam klienta nelze načíst.")
        Dim cDisp As BO.p28RecordDisposition = Master.Factory.p28ContactBL.InhaleRecordDisposition(cRec)
        If Not cDisp.OwnerAccess Then
            'oprávnění pro editaci klienta
            Master.StopPage("Pro klienta nedisponujete vlastnickým (editačním) oprávněním.")
        End If
        With cRec
            Master.HeaderText = "Klient | " & .p28Name
            Me.p29ID.SelectedValue = .p29ID.ToString
            Handle_FF()
            Me.p28IsCompany.SelectedValue = BO.BAS.GB(.p28IsCompany)
            Me.p28Code.Text = .p28Code
            Me.p28Code.NavigateUrl = "javascript:recordcode()"
            If .p28IsCompany Then
                Me.p28CompanyName.Text = .p28CompanyName
                Me.p28CompanyShortName.Text = .p28CompanyShortName
            Else
                Me.p28TitleAfterName.SetText(.p28TitleAfterName)
                Me.p28TitleBeforeName.SetText(.p28TitleBeforeName)
                Me.p28FirstName.Text = .p28FirstName
                Me.p28LastName.Text = .p28LastName
            End If
            Me.p28RegID.Text = .p28RegID
            Me.p28VatID.Text = .p28VatID

            Me.j02ID_Owner.Value = .j02ID_Owner.ToString
            Me.j02ID_Owner.Text = .Owner
            Me.p51ID_Billing.SelectedValue = .p51ID_Billing.ToString
            If .p51ID_Billing > 0 Then
                Dim cP51 As BO.p51PriceList = Master.Factory.p51PriceListBL.Load(.p51ID_Billing)
                If cP51.p51IsCustomTailor Then
                    Me.hidP51ID_Tailor.Value = cP51.PID.ToString
                    Me.opgPriceList.SelectedValue = "3"
                Else
                    Me.opgPriceList.SelectedValue = "2"
                End If
            Else
                Me.opgPriceList.SelectedValue = "1"
            End If
            Me.p51ID_Internal.SelectedValue = .p51ID_Internal.ToString

            Me.p87ID.SelectedValue = .p87ID.ToString
            Me.p92id.SelectedValue = .p92ID.ToString
            Me.p28InvoiceDefaultText1.Text = .p28InvoiceDefaultText1
            Me.p28InvoiceDefaultText2.Text = .p28InvoiceDefaultText2
            Me.p28InvoiceMaturityDays.Value = .p28InvoiceMaturityDays

            Me.p28LimitHours_Notification.Value = .p28LimitHours_Notification
            Me.p28LimitFee_Notification.Value = .p28LimitFee_Notification
            If .p28LimitHours_Notification > 0 Or .p28LimitFee_Notification > 0 Then Me.chkDefineLimits.Checked = True Else Me.chkDefineLimits.Checked = False


            Me.p28CompanyShortName.Text = .p28CompanyShortName
            Me.p28RobotAddress.Text = .p28RobotAddress
            basUI.SelectRadiolistValue(Me.p28SupplierFlag, .p28SupplierFlag)
            Me.p28SupplierID.Text = .p28SupplierID
            Master.Timestamp = .Timestamp


            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With
        Dim lisO32 As IEnumerable(Of BO.o32Contact_Medium) = Master.Factory.p28ContactBL.GetList_o32(Master.DataPID)
        Master.Factory.p85TempBoxBL.Truncate(ViewState("guid_o32"))
        For Each c In lisO32
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = ViewState("guid_o32")
                .p85DataPID = c.PID
                .p85OtherKey1 = c.o33ID
                .p85FreeText01 = c.o32Value
                .p85FreeText02 = c.o32Description
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
        RefreshTempO32()

        Dim lisO37 As IEnumerable(Of BO.o37Contact_Address) = Master.Factory.p28ContactBL.GetList_o37(Master.DataPID)
        Master.Factory.p85TempBoxBL.Truncate(ViewState("guid_o37"))
        For Each c In lisO37
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = ViewState("guid_o37")
                .p85DataPID = c.PID
                .p85OtherKey1 = c.o36ID
                .p85OtherKey2 = c.o38ID
                .p85FreeText01 = c.o38City
                .p85FreeText02 = c.o38Street
                .p85FreeText03 = c.o38ZIP
                .p85FreeText04 = c.o38Country
                .p85FreeText05 = c.o38Name
                .p85FreeText09 = c.o38AresID
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next

        roles1.InhaleInitialData(cRec.PID)

        Dim lisP58 As IEnumerable(Of BO.p58Product) = Master.Factory.p58ProductBL.GetList(New BO.myQuery, cRec.PID)
        Me.p58IDs.SelectCheckboxItems(lisP58.Select(Function(p) p.PID.ToString).ToList)

        RefreshTempO37()

        
    End Sub
    Private Sub Handle_FF()
        With RadTabStrip1.FindTabByValue("ff")
            If .Visible Then
                Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p28Contact, Master.DataPID, BO.BAS.IsNullInt(Me.p29ID.SelectedValue))
                ff1.FillData(fields)
                .Text = BO.BAS.OM2(.Text, ff1.FieldsCount.ToString)
            End If
        End With
    End Sub
    Private Sub rpO37_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpO37.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)

        With CType(e.Item.FindControl("del"), ImageButton)
            .CommandArgument = cRec.PID.ToString
            .CommandName = "delete"
        End With

        With cRec
            CType(e.Item.FindControl("p85id"), HiddenField).Value = .PID.ToString
            basUI.SelectDropdownlistValue(CType(e.Item.FindControl("o36id"), DropDownList), .p85OtherKey1.ToString)
            CType(e.Item.FindControl("o38City"), TextBox).Text = .p85FreeText01
            CType(e.Item.FindControl("o38Street"), TextBox).Text = .p85FreeText02
            CType(e.Item.FindControl("o38ZIP"), TextBox).Text = .p85FreeText03
            CType(e.Item.FindControl("o38Country"), TextBox).Text = .p85FreeText04
            CType(e.Item.FindControl("o38Name"), TextBox).Text = .p85FreeText05
        End With
    End Sub

    Private Sub RefreshTempO37()
        rpO37.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o37"))
        rpO37.DataBind()
    End Sub
    Private Sub RefreshTempO32()
        rpO32.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o32"))
        rpO32.DataBind()
    End Sub
   

    Private Sub p28_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        panCompany.Visible = False : panPerson.Visible = False
        If Me.p28IsCompany.SelectedValue = "1" Then
            panCompany.Visible = True
        Else
            panPerson.Visible = True
        End If
        p28CompanyShortName.Visible = Not panPerson.Visible
        lblp28CompanyShortName.Visible = Not panPerson.Visible
        If rpO37.Items.Count > 0 Then panO37.Visible = True Else panO37.Visible = False
        If rpO32.Items.Count > 0 Then panO32.Visible = True Else panO32.Visible = False
        Me.panLimits.Visible = Me.chkDefineLimits.Checked
        RefreshState_Pricelist()

        Select Case Me.p28SupplierFlag.SelectedValue
            Case "2", "3"
                lblSupplierID.Visible = True : p28SupplierID.Visible = True
            Case Else
                lblSupplierID.Visible = False : p28SupplierID.Visible = False
        End Select
    End Sub
    Private Sub RefreshState_Pricelist()
        lblP51ID_Billing.Visible = True : Me.p51ID_Billing.Visible = True
        Select Case Me.opgPriceList.SelectedValue
            Case "1"
                lblP51ID_Billing.Visible = False
                Me.p51ID_Billing.Visible = False
                cmdNewP51.Visible = False
                cmdEditP51.Visible = False
            Case "2"
                Me.cmdNewP51.NavigateUrl = "javascript:p51_billing_add(0)"
                Me.cmdNewP51.Visible = True : cmdEditP51.Visible = True
                If Me.p51ID_Billing.SelectedValue <> "" Then
                    cmdEditP51.NavigateUrl = "javascript:p51_edit(" & Me.p51ID_Billing.SelectedValue & ")"
                    cmdEditP51.Visible = True
                Else
                    cmdEditP51.Visible = False
                End If

            Case "3"
                If BO.BAS.IsNullInt(Me.hidP51ID_Tailor.Value) <> 0 Then
                    cmdEditP51.NavigateUrl = "javascript:p51_edit(" & Me.hidP51ID_Tailor.Value & ")"
                    Me.cmdNewP51.Visible = False
                    cmdEditP51.Visible = True
                Else
                    Me.cmdNewP51.Visible = True
                    Me.cmdNewP51.NavigateUrl = "javascript:p51_billing_add(1)"
                    cmdEditP51.Visible = False
                End If

                lblP51ID_Billing.Visible = False
                Me.p51ID_Billing.Visible = False

        End Select
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p28ContactBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p28-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub SaveTempO37()
        Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o37"))
        For Each ri As RepeaterItem In rpO37.Items
            Dim intP85ID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("p85id"), HiddenField).Value)
            Dim cRec As BO.p85TempBox = lisTEMP.Where(Function(p) p.PID = intP85ID)(0)
            With cRec
                .p85OtherKey1 = BO.BAS.IsNullInt(CType(ri.FindControl("o36id"), DropDownList).SelectedValue)
                .p85FreeText01 = CType(ri.FindControl("o38City"), TextBox).Text
                .p85FreeText02 = CType(ri.FindControl("o38Street"), TextBox).Text
                .p85FreeText03 = CType(ri.FindControl("o38ZIP"), TextBox).Text
                .p85FreeText04 = CType(ri.FindControl("o38Country"), TextBox).Text
                .p85FreeText05 = CType(ri.FindControl("o38Name"), TextBox).Text
            End With
            Master.Factory.p85TempBoxBL.Save(cRec)
        Next
    End Sub
    Private Sub SaveTempO32()
        Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o32"))
        For Each ri As RepeaterItem In rpO32.Items
            Dim intP85ID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("p85id"), HiddenField).Value)
            Dim cRec As BO.p85TempBox = lisTEMP.Where(Function(p) p.PID = intP85ID)(0)
            With cRec
                .p85OtherKey1 = BO.BAS.IsNullInt(CType(ri.FindControl("o33id"), DropDownList).SelectedValue)
                .p85FreeText01 = CType(ri.FindControl("o32Value"), TextBox).Text
                .p85FreeText02 = CType(ri.FindControl("o32Description"), TextBox).Text
                
            End With
            Master.Factory.p85TempBoxBL.Save(cRec)
        Next
    End Sub
    
    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        SaveTempO37()
        SaveTempO32()
        roles1.SaveCurrentTempData()

        With Master.Factory.p28ContactBL
            Dim cRec As BO.p28Contact = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p28Contact)
            With cRec
                If .PID = 0 Then .p28IsDraft = Me.p28IsDraft.Checked
                .p28IsCompany = BO.BAS.BG(Me.p28IsCompany.SelectedValue)
                .j02ID_Owner = BO.BAS.IsNullInt(Me.j02ID_Owner.Value)
                .p29ID = BO.BAS.IsNullInt(Me.p29ID.SelectedValue)
                If .p28IsCompany Then
                    .p28CompanyName = Me.p28CompanyName.Text
                    .p28CompanyShortName = Me.p28CompanyShortName.Text
                Else
                    .p28TitleBeforeName = Me.p28TitleBeforeName.Text
                    .p28FirstName = Me.p28FirstName.Text
                    .p28LastName = Me.p28LastName.Text
                    .p28TitleAfterName = Me.p28TitleAfterName.Text
                End If
                .p28RegID = Me.p28RegID.Text
                .p28VatID = Me.p28VatID.Text
                .p28InvoiceMaturityDays = BO.BAS.IsNullInt(Me.p28InvoiceMaturityDays.Value)
                Select Case Me.opgPriceList.SelectedValue
                    Case "1"
                        .p51ID_Billing = 0
                    Case "2"
                        .p51ID_Billing = BO.BAS.IsNullInt(Me.p51ID_Billing.SelectedValue)
                    Case "3"
                        .p51ID_Billing = BO.BAS.IsNullInt(Me.hidP51ID_Tailor.Value)
                End Select
                If Me.opgPriceList.SelectedValue <> "1" And .p51ID_Billing = 0 Then
                    Master.Notify("Chybí ceník sazeb.", NotifyLevel.WarningMessage) : Return
                End If
                .p87ID = BO.BAS.IsNullInt(Me.p87ID.SelectedValue)
                .p92ID = BO.BAS.IsNullInt(Me.p92id.SelectedValue)
                .p28InvoiceDefaultText1 = Me.p28InvoiceDefaultText1.Text
                .p28InvoiceDefaultText2 = Me.p28InvoiceDefaultText2.Text
                .p51ID_Internal = BO.BAS.IsNullInt(Me.p51ID_Internal.SelectedValue)
                .p28RobotAddress = Me.p28RobotAddress.Text
                .p28SupplierFlag = CInt(Me.p28SupplierFlag.SelectedValue)
                If .p28SupplierFlag = BO.p28SupplierFlagENUM.ClientAndSupplier Or .p28SupplierFlag = BO.p28SupplierFlagENUM.SupplierOnly Then
                    .p28SupplierID = Me.p28SupplierID.Text
                End If

                If Me.chkDefineLimits.Checked Then
                    .p28LimitHours_Notification = BO.BAS.IsNullNum(Me.p28LimitHours_Notification.Value)
                    .p28LimitFee_Notification = BO.BAS.IsNullNum(Me.p28LimitFee_Notification.Value)
                Else
                    .p28LimitHours_Notification = 0 : .p28LimitFee_Notification = 0
                End If

                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
            End With

            Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o37"), True)
            Dim lisO37 As New List(Of BO.o37Contact_Address)
            For Each cTMP In lisTEMP
                Dim c As New BO.o37Contact_Address
                With cTMP
                    c.IsSetAsDeleted = .p85IsDeleted
                    c.o36ID = .p85OtherKey1
                    c.o38ID = .p85OtherKey2
                    c.o38City = .p85FreeText01
                    c.o38Street = .p85FreeText02
                    c.o38ZIP = .p85FreeText03
                    c.o38Country = .p85FreeText04
                    c.o38Name = .p85FreeText05
                    c.o38AresID = .p85FreeText09
                End With
                lisO37.Add(c)
            Next
            lisTEMP = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o32"), True)
            Dim lisO32 As New List(Of BO.o32Contact_Medium)
            For Each cTMP In lisTEMP
                Dim c As New BO.o32Contact_Medium
                With cTMP
                    c.SetPID(.p85DataPID)
                    c.IsSetAsDeleted = .p85IsDeleted
                    c.o33ID = .p85OtherKey1
                    c.o32Value = .p85FreeText01
                    c.o32Description = .p85FreeText02
                End With
                lisO32.Add(c)
            Next

            Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()
            If roles1.ErrorMessage <> "" Then
                Master.Notify(roles1.ErrorMessage, 2)
                Return
            End If

            Dim lisFF As List(Of BO.FreeField) = Me.ff1.GetValues()
            Dim p58vals As List(Of Integer) = Me.p58IDs.GetAllCheckedIntegerValues()

            If .Save(cRec, lisO37, lisO32, Nothing, lisX69, lisFF, p58vals) Then
                Dim bolNew As Boolean = Master.IsRecordNew
                Master.DataPID = .LastSavedPID
                If bolNew Then
                    Master.CloseAndRefreshParent("p28-create")
                Else
                    Master.CloseAndRefreshParent("p28-save")
                End If

            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub rpO37_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpO37.ItemCommand
        SaveTempO37()
        Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(BO.BAS.IsNullInt(e.CommandArgument))
        If e.CommandName = "delete" Then
            If cRec.p85DataPID > 0 Then
                Dim mq As New BO.myQueryP91
                mq.o38ID = cRec.p85DataPID
                Dim lisTest As IEnumerable(Of BO.p91Invoice) = Master.Factory.p91InvoiceBL.GetList(mq)
                If lisTest.Count > 0 Then
                    Master.Notify("Tuto adresu není možné odstranit, protože má vazbu na " & lisTest.Count.ToString & " klientských faktur (první z nich: " & lisTest(0).p91Code & ").", NotifyLevel.WarningMessage)
                    Return
                End If
            End If

            If Master.Factory.p85TempBoxBL.Delete(cRec) Then
                RefreshTempO37()
            End If
        End If
    End Sub

    
    Private Sub cmdAddO37_Click(sender As Object, e As EventArgs) Handles cmdAddO37.Click
        SaveTempO37()
        Dim cRec As New BO.p85TempBox()
        cRec.p85GUID = ViewState("guid_o37")
        cRec.p85OtherKey1 = BO.o36IdEnum.InvoiceAddress
        Master.Factory.p85TempBoxBL.Save(cRec)
        RefreshTempO37()
    End Sub

    Private Sub p29ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p29ID.NeedMissingItem
        Dim cRec As BO.p29ContactType = Master.Factory.p29ContactTypeBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then
            strAddMissingItemText = cRec.p29Name
        End If
    End Sub

    Private Sub p51ID_Billing_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p51ID_Billing.NeedMissingItem
        Dim cRec As BO.p51PriceList = Master.Factory.p51PriceListBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then
            strAddMissingItemText = cRec.NameWithCurr
        End If
    End Sub

    Private Sub rpO32_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpO32.ItemCommand
        SaveTempO32()
        Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(BO.BAS.IsNullInt(e.CommandArgument))
        If e.CommandName = "delete" Then
            If Master.Factory.p85TempBoxBL.Delete(cRec) Then
                RefreshTempO32()
            End If
        End If
    End Sub

    Private Sub rpO32_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpO32.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)

        With CType(e.Item.FindControl("del"), ImageButton)
            .CommandArgument = cRec.PID.ToString
            .CommandName = "delete"
        End With

        With cRec
            CType(e.Item.FindControl("p85id"), HiddenField).Value = .PID.ToString
            basUI.SelectDropdownlistValue(CType(e.Item.FindControl("o33id"), DropDownList), .p85OtherKey1.ToString)
            CType(e.Item.FindControl("o32Value"), TextBox).Text = .p85FreeText01
            CType(e.Item.FindControl("o32Description"), TextBox).Text = .p85FreeText02
        End With
    End Sub

    Private Sub cmdAddO32_Click(sender As Object, e As EventArgs) Handles cmdAddO32.Click
        SaveTempO32()
        Dim cRec As New BO.p85TempBox()
        cRec.p85GUID = ViewState("guid_o32")
        cRec.p85OtherKey1 = BO.o33FlagEnum.Tel
        Master.Factory.p85TempBoxBL.Save(cRec)
        RefreshTempO32()
    End Sub



    Private Sub cmdHardRefresh_Click(sender As Object, e As EventArgs) Handles cmdHardRefresh.Click
        Dim strPID As String = Me.HardRefreshPID.Value
        
        Select Case Me.HardRefreshFlag.Value
            Case "p51-save"
                SetupPriceList()
                Me.p51ID_Billing.SelectedValue = strPID
                Dim cRec As BO.p51PriceList = Master.Factory.p51PriceListBL.Load(CInt(strPID))
                If cRec.p51IsCustomTailor Then
                    hidP51ID_Tailor.Value = cRec.PID.ToString
                    Me.opgPriceList.SelectedValue = "3"
                Else
                    hidP51ID_Tailor.Value = ""
                    Me.opgPriceList.SelectedValue = "2"
                End If
                RefreshState_Pricelist()
            Case "p51-delete"
                SetupPriceList()

        End Select

        Me.HardRefreshPID.Value = ""
        Me.HardRefreshFlag.Value = ""
    End Sub
    Private Sub p51ID_Billing_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles p51ID_Billing.SelectedIndexChanged
        RefreshState_Pricelist()
    End Sub

    Private Sub cmdARES_Click(sender As Object, e As EventArgs) Handles cmdARES.Click
        If Trim(Me.p28RegID.Text) = "" Then
            Master.Notify("Musíte vyplnit IČ.", NotifyLevel.WarningMessage)
            Return
        End If
        Dim cAres As New clsAresImport()
        Dim cRec As BO.AresRecord = cAres.LoadAresRecord(Trim(Me.p28RegID.Text))
        If cRec Is Nothing Then
            Master.Notify("ARES záznam nebylo možné načíst, chyba: " & cAres.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            p28IsCompany.SelectedIndex = 0
            Me.p28CompanyName.Text = cRec.Company
            Me.p28VatID.Text = cRec.DIC
            SaveTempO37()
            Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid_o37"))
            Dim cTemp As New BO.p85TempBox()
            If lisTemp.Where(Function(p) p.p85FreeText09 = cRec.ID_adresy).Count > 0 Then
                cTemp = lisTemp(0)
            End If
            With cTemp
                .p85GUID = ViewState("guid_o37")
                .p85OtherKey1 = BO.o36IdEnum.InvoiceAddress
                .p85FreeText01 = cRec.City
                .p85FreeText02 = cRec.Street
                .p85FreeText03 = cRec.PostCode
                .p85FreeText04 = cRec.Country
                .p85FreeText09 = cRec.ID_adresy
            End With

            Master.Factory.p85TempBoxBL.Save(cTemp)
            RefreshTempO37()

        End If
    End Sub

  

    
    Private Sub p51ID_Internal_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p51ID_Internal.NeedMissingItem
        Dim cRec As BO.p51PriceList = Master.Factory.p51PriceListBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then strAddMissingItemText = cRec.p51Name
    End Sub

    Private Sub p29ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles p29ID.SelectedIndexChanged
        Handle_FF()
    End Sub

    Private Sub cmdAddX69_Click(sender As Object, e As EventArgs) Handles cmdAddX69.Click
        roles1.AddNewRow()
    End Sub

    Private Sub TryInhaleInitialData()
        Dim cRecLast As BO.p28Contact = Master.Factory.p28ContactBL.LoadMyLastCreated()
        If cRecLast Is Nothing Then Return
        With cRecLast
            Me.p29ID.SelectedValue = .p29ID.ToString
            Me.p28IsCompany.SelectedValue = BO.BAS.GB(.p28IsCompany)
            Me.p28InvoiceMaturityDays.Value = .p28InvoiceMaturityDays
            roles1.InhaleInitialData(.PID)
        End With

    End Sub
End Class