﻿Public Class query_builder
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Private _lastField As String

    Private Class myItem
        Public Property x29ID As BO.x29IdEnum
        Public Property Field As String
        Public Property Text As String
        Public Sub New(x29ID As BO.x29IdEnum, strField As String, strText As String)
            Me.Text = strText
            Me.x29ID = x29ID
            Me.Field = strField
        End Sub
    End Class
    
    
    Public Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
        Set(value As String)
            Me.hidPrefix.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.CurrentPrefix)
        End Get
    End Property
    Public ReadOnly Property CurrentIsOwner As Boolean
        Get
            Return BO.BAS.BG(hidIsOwner.Value)
        End Get
    End Property
    Private Sub query_builder_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        roles1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID()
            ViewState("x36key") = Request.Item("x36key")
            Me.CurrentPrefix = Request.Item("prefix")
            If ViewState("x36key") = "" Then ViewState("x36key") = Me.CurrentPrefix & "-j70id"
            With Master
                .Factory.j03UserBL.InhaleUserParams(CStr(ViewState("x36key")))

                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Request.Item("j70id") <> "" Then
                    .DataPID = BO.BAS.IsNullInt(Request.Item("j70id"))
                End If
                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(Master.Factory.j03UserBL.GetUserParam(CStr(ViewState("x36key"))))
                End If

                .AddToolbarButton("Uložit a spustit filtr", "run", , "Images/ok.png")

            End With

            SetupQuery()
            SetupJ70Combo()
            RefreshRecord()

        End If
    End Sub

    Private Sub SetupQuery()
        Dim lis As New List(Of myItem)
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                ph1.Text = "Návrhář filtrů nad přehledem projektů"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.p42ProjectType, "p42id", "Typ projektu"))
                lis.Add(New myItem(BO.x29IdEnum.j18Region, "j18id", "Středisko projektu"))
                lis.Add(New myItem(BO.x29IdEnum.p92InvoiceType, "p92id", "Výchozí typ faktury projektu"))
                lis.Add(New myItem(BO.x29IdEnum.p51PriceList, "p51id_billing", "Fakturační ceník projektu"))
                lis.Add(New myItem(BO.x29IdEnum.p87BillingLanguage, "p87id", "Fakturační jazyk projektu"))
                lis.Add(New myItem(BO.x29IdEnum.j02Person, "j02id_owner", "Vlastník záznamu"))
                lis.Add(New myItem(BO.x29IdEnum.x67EntityRole, "x67id", "Obsazení projektové role"))
                lis.Add(New myItem(BO.x29IdEnum.x67EntityRole, "x67id-j11id", "Obsazení role přes tým"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "p28id_client", "Klient projektu"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "p28id_billing", "Odběratel faktury"))
                lis.Add(New myItem(BO.x29IdEnum.p29ContactType, "p28client.p29id", "Typ klienta"))

            Case BO.x29IdEnum.p28Contact
                ph1.Text = "Návrhář filtrů nad přehledem klientů"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.p29ContactType, "p29id", "Typ klienta"))
                lis.Add(New myItem(BO.x29IdEnum.p92InvoiceType, "p92id", "Výchozí typ faktury klienta"))
                lis.Add(New myItem(BO.x29IdEnum.p51PriceList, "p51id_billing", "Fakturační ceník klienta"))
                lis.Add(New myItem(BO.x29IdEnum.p87BillingLanguage, "p87id", "Fakturační jazyk klienta"))
                lis.Add(New myItem(BO.x29IdEnum.j02Person, "j02id_owner", "Vlastník záznamu"))
            Case BO.x29IdEnum.p56Task
                ph1.Text = "Návrhář filtrů nad přehledem úkolů/požadavků"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.p57TaskType, "p57id", "Typ úkolu"))
                lis.Add(New myItem(BO.x29IdEnum.p58Product, "p58id", "Produkt"))
                lis.Add(New myItem(BO.x29IdEnum.b02WorkflowStatus, "b02id", "Aktuální workflow stav"))
                lis.Add(New myItem(BO.x29IdEnum.x67EntityRole, "x67id", "Obsazení role v úkolu"))
                lis.Add(New myItem(BO.x29IdEnum.x67EntityRole, "x67id-j11id", "Obsazení role přes tým"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "p41.p28ID_Client", "Klient projektu"))

                lis.Add(New myItem(BO.x29IdEnum.j02Person, "j02id_owner", "Vlastník/autor záznamu"))
            Case BO.x29IdEnum.o23Notepad
                ph1.Text = "Návrhář filtrů nad přehledem dokumentů"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.o24NotepadType, "o24ID", "Typ dokumentu"))
                lis.Add(New myItem(BO.x29IdEnum.b02WorkflowStatus, "b02id", "Aktuální workflow stav"))
                lis.Add(New myItem(BO.x29IdEnum.x67EntityRole, "x67id", "Role příjemce (čtenáře) dokumentu"))
                lis.Add(New myItem(BO.x29IdEnum.x67EntityRole, "x67id-j11id", "Obsazení role přes tým"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "a.p28ID", "Klient přiřazený k dokumentu"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "p41.p28ID_Client", "Klient projektu svázaného s dokumentem"))

                lis.Add(New myItem(BO.x29IdEnum.j02Person, "j02id_owner", "Vlastník/autor záznamu"))
            Case BO.x29IdEnum.p91Invoice
                ph1.Text = "Návrhář filtrů nad přehledem faktur"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.p92InvoiceType, "p92id", "Typ faktury"))
                lis.Add(New myItem(BO.x29IdEnum.j27Currency, "j27id", "Měna faktury"))
                lis.Add(New myItem(BO.x29IdEnum.j02Person, "j02id_owner", "Vlastník záznamu"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "a.p28ID", "Klient faktury"))
                lis.Add(New myItem(BO.x29IdEnum.p29ContactType, "p28client.p29id", "Typ klienta faktury"))
            Case BO.x29IdEnum.j02Person
                ph1.Text = "Návrhář filtrů nad přehledem osob"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.j07PersonPosition, "j07id", "Pozice"))
                lis.Add(New myItem(BO.x29IdEnum.c21FondCalendar, "c21id", "Pracovní fond"))
                lis.Add(New myItem(BO.x29IdEnum.j11Team, "j11id", "Tým osob"))
            Case BO.x29IdEnum.p31Worksheet
                ph1.Text = "Návrhář filtrů nad worksheet přehledem"

                lis.Add(New myItem(BO.x29IdEnum._NotSpecified, "_other", "Různé"))
                lis.Add(New myItem(BO.x29IdEnum.p34ActivityGroup, "p32.p34id", "Sešit"))
                lis.Add(New myItem(BO.x29IdEnum.p32Activity, "p32id", "Aktivita"))
                lis.Add(New myItem(BO.x29IdEnum.j02Person, "j02id", "Osoba"))
                lis.Add(New myItem(BO.x29IdEnum.j02Person, "a.j02ID_ContactPerson", "Kontaktní osoba"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "p41.p28ID_Client", "Klient projektu"))
                lis.Add(New myItem(BO.x29IdEnum.p28Contact, "a.p28ID_Supplier", "Dodavatel"))
                lis.Add(New myItem(BO.x29IdEnum.p71ApproveStatus, "p71id", "Schvalování"))
                lis.Add(New myItem(BO.x29IdEnum.p72PreBillingStatus, "p72ID_AfterApprove", "Návrh fakturace po schvalování"))
                lis.Add(New myItem(BO.x29IdEnum.p70BillingStatus, "p70id", "Fakturace"))
                lis.Add(New myItem(BO.x29IdEnum.j27Currency, "j27ID_Billing_Orig", "Měna úkonu"))
                lis.Add(New myItem(BO.x29IdEnum.p95InvoiceRow, "p32.p95id", "Fakturační oddíl"))
        End Select

        lis.Add(New myItem(BO.x29IdEnum.x25EntityField_ComboValue, "x25id", "Štítky"))

        Dim lisFF As IEnumerable(Of BO.x28EntityField) = Master.Factory.x28EntityFieldBL.GetList(Me.CurrentX29ID, -1, True).Where(Function(p) p.x28Flag = BO.x28FlagENUM.UserField)
        For Each cField In lisFF
            lis.Add(New myItem(BO.x29IdEnum.System, cField.x28Field, cField.x28Name))
        Next
        For Each c In lis
            Me.opgField.Items.Add(New ListItem(c.Text, CInt(c.x29ID).ToString & "-" & c.Field))

        Next
    End Sub

    Private Sub Handle_ChangeField()
        cbxItemsExtension.Visible = False

        If Me.opgField.SelectedItem Is Nothing Then Return
        Dim a() As String = Split(Me.opgField.SelectedValue, "-")
        Dim x29id As BO.x29IdEnum = CType(CInt(a(0)), BO.x29IdEnum)
        Dim strField As String = a(1), mq As New BO.myQuery, strFieldAfter As String = ""
        If UBound(a) > 1 Then strFieldAfter = a(2)

        mq.Closed = BO.BooleanQueryMode.NoQuery
        Me.panQueryItems.Visible = True : Me.panQueryNonItems.Visible = False : Me.panQueryPeriod.Visible = False : Me.panQueryString.Visible = False

        Select Case x29id
            Case BO.x29IdEnum.System
                'uživatelské - volné pole
                Dim cField As BO.x28EntityField = Master.Factory.x28EntityFieldBL.LoadByField(strField)
                If cField.x23ID <> 0 Or cField.x24ID = BO.x24IdENUM.tBoolean Then
                    'číselníkové (combo) pole
                    If cField.x23ID <> 0 Then
                        Me.cbxItems.DataTextField = "x25Name"
                        Me.cbxItems.DataSource = Master.Factory.x25EntityField_ComboValueBL.GetList(cField.x23ID)
                    Else
                        Me.cbxItems.Clear()
                        Me.cbxItems.AddOneComboItem("1", "ANO")
                        Me.cbxItems.AddOneComboItem("0", "NE")
                    End If

                Else
                    'nečíselníkové pole
                    Me.panQueryItems.Visible = False
                    Select Case cField.x24ID
                        Case BO.x24IdENUM.tDate, BO.x24IdENUM.tDateTime, BO.x24IdENUM.tTime
                            panQueryPeriod.Visible = True
                            Dim bolEnglish As Boolean = False
                            If Page.Culture.IndexOf("Czech") < 0 Then bolEnglish = True

                            period1.FillData(Master.Factory.ftBL.GetList_X21_NonDB(False, bolEnglish), "", "--Vlastní období--")
                        Case BO.x24IdENUM.tString
                            panQueryString.Visible = True
                        Case Else
                            Me.panQueryNonItems.Visible = True
                    End Select


                End If
            Case BO.x29IdEnum._NotSpecified
                Dim lis As List(Of BO.OtherQueryItem) = Master.Factory.j70QueryTemplateBL.GetList_OtherQueryItem(Me.CurrentX29ID)

                Me.cbxItems.DataTextField = "Text"
                Me.cbxItems.DataSource = lis


            Case BO.x29IdEnum.p92InvoiceType
                Me.cbxItems.DataTextField = "p92Name"
                Me.cbxItems.DataSource = Master.Factory.p92InvoiceTypeBL.GetList(mq)
            Case BO.x29IdEnum.p42ProjectType
                Me.cbxItems.DataTextField = "p42Name"
                Me.cbxItems.DataSource = Master.Factory.p42ProjectTypeBL.GetList(mq)
            Case BO.x29IdEnum.p29ContactType
                Me.cbxItems.DataTextField = "p29Name"
                Me.cbxItems.DataSource = Master.Factory.p29ContactTypeBL.GetList(mq)
            Case BO.x29IdEnum.p57TaskType
                Me.cbxItems.DataTextField = "p57Name"
                Me.cbxItems.DataSource = Master.Factory.p57TaskTypeBL.GetList(mq)
            Case BO.x29IdEnum.p58Product
                Me.cbxItems.DataTextField = "TreeMenuItem"
                Me.cbxItems.DataSource = Master.Factory.p58ProductBL.GetList(mq)
            Case BO.x29IdEnum.o24NotepadType
                Me.cbxItems.DataTextField = "o24Name"
                Me.cbxItems.DataSource = Master.Factory.o24NotepadTypeBL.GetList(mq)
            Case BO.x29IdEnum.b02WorkflowStatus
                Me.cbxItems.DataTextField = "NameWithb01Name"
                Me.cbxItems.DataSource = Master.Factory.b02WorkflowStatusBL.GetList(0).Where(Function(p) p.x29ID = Me.CurrentX29ID)
            Case BO.x29IdEnum.j18Region
                Me.cbxItems.DataTextField = "j18Name"
                Me.cbxItems.DataSource = Master.Factory.j18RegionBL.GetList(mq)
            Case BO.x29IdEnum.j02Person
                Me.cbxItems.DataTextField = "FullNameDesc"
                Dim mqJ02 As New BO.myQueryJ02
                mqJ02.IntraPersons = BO.myQueryJ02_IntraPersons.IntraOnly
                mqJ02.Closed = BO.BooleanQueryMode.NoQuery
                Me.cbxItems.DataSource = Master.Factory.j02PersonBL.GetList(mqJ02)
            Case BO.x29IdEnum.p87BillingLanguage
                Me.cbxItems.DataTextField = "p87Name"
                Me.cbxItems.DataSource = Master.Factory.ftBL.GetList_P87()
            Case BO.x29IdEnum.j27Currency
                Me.cbxItems.DataTextField = "j27Code"
                Me.cbxItems.DataSource = Master.Factory.ftBL.GetList_J27()
            Case BO.x29IdEnum.j07PersonPosition
                Me.cbxItems.DataTextField = "j07Name"
                Me.cbxItems.DataSource = Master.Factory.j07PersonPositionBL.GetList(mq)
            Case BO.x29IdEnum.j11Team
                Me.cbxItems.DataTextField = "j11Name"
                Me.cbxItems.DataSource = Master.Factory.j11TeamBL.GetList(mq).Where(Function(p) p.j11IsAllPersons = False)
            Case BO.x29IdEnum.c21FondCalendar
                Me.cbxItems.DataTextField = "c21Name"
                Me.cbxItems.DataSource = Master.Factory.c21FondCalendarBL.GetList(mq)
            Case BO.x29IdEnum.p34ActivityGroup
                Me.cbxItems.DataTextField = "p34Name"
                Me.cbxItems.DataSource = Master.Factory.p34ActivityGroupBL.GetList(mq)
            Case BO.x29IdEnum.p32Activity
                Dim mqP32 As New BO.myQueryP32
                mqP32.Closed = BO.BooleanQueryMode.NoQuery
                Me.cbxItems.DataTextField = "NameWithSheet"
                Me.cbxItems.DataSource = Master.Factory.p32ActivityBL.GetList(mqP32).OrderBy(Function(p) p.p34ID).ThenBy(Function(p) p.p32Ordinary).ThenBy(Function(p) p.p32Name)
            Case BO.x29IdEnum.p95InvoiceRow
                Me.cbxItems.DataTextField = "p95Name"
                Me.cbxItems.DataSource = Master.Factory.p95InvoiceRowBL.GetList(mq)
            Case BO.x29IdEnum.p71ApproveStatus
                Me.cbxItems.DataTextField = "p71Name"
                Me.cbxItems.DataSource = Master.Factory.ftBL.GetList_P71()
            Case BO.x29IdEnum.p72PreBillingStatus
                Me.cbxItems.DataTextField = "p72Name"
                Me.cbxItems.DataSource = Master.Factory.ftBL.GetList_P72()
            Case BO.x29IdEnum.p70BillingStatus
                Me.cbxItems.DataTextField = "p70Name"
                Me.cbxItems.DataSource = Master.Factory.ftBL.GetList_P70()
            Case BO.x29IdEnum.p51PriceList
                Me.cbxItems.DataTextField = "p51Name"
                Me.cbxItems.DataSource = Master.Factory.p51PriceListBL.GetList(mq).Where(Function(p) p.p51IsMasterPriceList = False And p.p51IsInternalPriceList = False And p.p51IsCustomTailor = False)
            Case BO.x29IdEnum.x67EntityRole
                Me.cbxItems.DataTextField = "x67Name"
                Me.cbxItems.DataSource = Master.Factory.x67EntityRoleBL.GetList(mq).Where(Function(p) p.x29ID = Me.CurrentX29ID)
                Me.cbxItemsExtension.Visible = True
                If strFieldAfter = "j11id" Then
                    Me.cbxItemsExtension.DataTextField = "j11Name"
                    Me.cbxItemsExtension.DataSource = Master.Factory.j11TeamBL.GetList()
                    Me.cbxItemsExtension.DataBind()
                Else
                    Me.cbxItemsExtension.DataTextField = "FullNameDesc"
                    Dim mqJ02 As New BO.myQueryJ02
                    mqJ02.IntraPersons = BO.myQueryJ02_IntraPersons.IntraOnly
                    mqJ02.Closed = BO.BooleanQueryMode.NoQuery
                    Me.cbxItemsExtension.DataSource = Master.Factory.j02PersonBL.GetList(mqJ02)
                    Me.cbxItemsExtension.DataBind()
                    Me.cbxItemsExtension.AddOneComboItem("-1", "Aktuálně přihlášený uživatel", 1)
                End If

            Case BO.x29IdEnum.p28Contact
                Me.cbxItems.DataTextField = "p28Name"
                Dim mqP28 As New BO.myQueryP28
                mqP28.Closed = BO.BooleanQueryMode.NoQuery
                Select Case strField
                    Case "p28id_client"
                        mqP28.QuickQuery = BO.myQueryP28_QuickQuery.ProjectClient
                    Case "p28id_billing"
                        mqP28.QuickQuery = BO.myQueryP28_QuickQuery.ProjectInvoiceReceiver
                End Select
                Me.cbxItems.DataSource = Master.Factory.p28ContactBL.GetList(mqP28)
            Case BO.x29IdEnum.x25EntityField_ComboValue 'štítky
                Me.cbxItems.DataTextField = "NameWithComboName"
                Me.cbxItems.DataSource = Master.Factory.x18EntityCategoryBL.GetList_X25(Me.CurrentX29ID)
            Case Else
                Me.cbxItems.DataSource = Nothing
        End Select

        If panQueryItems.Visible Then Me.cbxItems.DataBind()

    End Sub

  
    Private Sub opgField_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgField.SelectedIndexChanged
        Handle_ChangeField()
    End Sub

    Private Sub cbxItems_ItemDataBound(sender As Object, e As Telerik.Web.UI.RadComboBoxItemEventArgs) Handles cbxItems.ItemDataBound
        Try
            If e.Item.DataItem.IsClosed Then
                e.Item.Font.Strikeout = True
            End If
        Catch ex As Exception

        End Try
        
    End Sub

    Private Sub SaveTempQueryItem()
        Dim a() As String = Split(Me.opgField.SelectedValue, "-")
        Dim x29id As BO.x29IdEnum = CType(CInt(a(0)), BO.x29IdEnum)
        Dim strField As String = a(1), mq As New BO.myQuery, bolCiselnik As Boolean = True
        Dim intj71RecordPID As Integer = 0, strj71ValueFrom As String = "", strj71ValueUntil As String = "", intExtensionValue As Integer = 0
        Dim strJ71ValueType As String = "combo", strj71RecordName As String = ""
        Dim strJ71StringOperator As String = "", strJ71ValueString As String = ""

        If x29id = BO.x29IdEnum.System Then
            'uživatelské - volné pole
            Dim cField As BO.x28EntityField = Master.Factory.x28EntityFieldBL.LoadByField(strField)
            Select Case cField.x24ID
                Case BO.x24IdENUM.tBoolean
                    strJ71ValueType = "boolean"
                Case BO.x24IdENUM.tDecimal, BO.x24IdENUM.tInteger
                    strJ71ValueType = "number"
                    If Me.panQueryNonItems.Visible Then
                        If Not IsNumeric(Trim(Me.j71ValueFrom.Text)) Then
                            Master.Notify("[Hodnota od] musí být číslo.", NotifyLevel.WarningMessage) : Return
                        End If
                        If Not IsNumeric(Trim(Me.j71ValueUntil.Text)) Then
                            Master.Notify("[Hodnota do] musí být číslo.", NotifyLevel.WarningMessage) : Return
                        End If
                    End If
                    
                Case BO.x24IdENUM.tString
                    strJ71ValueType = "string"
                    strJ71StringOperator = Me.cbxStringOperator.SelectedValue
                    strJ71ValueString = Trim(Me.txtStringValue.Text)
                    If strJ71ValueString = "" And Me.cbxStringOperator.SelectedValue <> "EMPTY" And Me.cbxStringOperator.SelectedValue <> "NOTEMPTY" Then
                        Master.Notify("Musíte zadat hodnotu.", NotifyLevel.WarningMessage)
                        Return
                    End If
            End Select
        End If

        If Me.panQueryNonItems.Visible Then
            strj71ValueFrom = Trim(Me.j71ValueFrom.Text)
            strj71ValueUntil = Trim(Me.j71ValueUntil.Text)
            strj71RecordName = strj71ValueFrom & " - " & strj71ValueUntil
        End If
        If Me.panQueryString.Visible Then
            strJ71ValueType = "string"
            strj71RecordName = Me.cbxStringOperator.SelectedItem.Text & ": " & strJ71ValueString
        End If
        If Me.panQueryPeriod.Visible Then
            'filtrování podle datumu
            strj71ValueFrom = Format(period1.DateFrom, "dd.MM.yyyy")
            strj71ValueUntil = Format(period1.DateUntil, "dd.MM.yyyy")
            If Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).Where(Function(p) p.p85FreeText04 = strField).Count > 0 Then
                Master.Notify("Toto datumové pole již bylo do filtru zařazeno.", NotifyLevel.WarningMessage)
                Return
            End If
            strJ71ValueType = "date"
            strj71RecordName = strj71ValueFrom & " - " & strj71ValueUntil
        End If
        If Me.panQueryItems.Visible Then
            'číselník
            strj71RecordName = Me.cbxItems.Text
            intj71RecordPID = BO.BAS.IsNullInt(Me.cbxItems.SelectedValue)
            If Me.cbxItemsExtension.Visible Then
                intExtensionValue = BO.BAS.IsNullInt(Me.cbxItemsExtension.SelectedValue)
            End If
            If intj71RecordPID = 0 And strJ71ValueType <> "boolean" Then
                Master.Notify("Musíte vybrat hodnotu.", NotifyLevel.WarningMessage)
                Return
            End If

            If Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).Where(Function(p) p.p85OtherKey1 = intj71RecordPID And p.p85FreeText04 = strField And p.p85OtherKey3 = intExtensionValue).Count > 0 Then
                Master.Notify("Tato hodnota již byla do filtru zařazena.", NotifyLevel.WarningMessage)
                Return
            End If
        End If


        Dim cRec As New BO.p85TempBox
        cRec.p85GUID = ViewState("guid")
        cRec.p85OtherKey1 = intj71RecordPID
        cRec.p85FreeText01 = strj71RecordName

        cRec.p85OtherKey2 = CInt(x29id)
        cRec.p85FreeText03 = Me.opgField.SelectedItem.Text
        cRec.p85FreeText04 = strField

        If intExtensionValue <> 0 Then
            cRec.p85OtherKey3 = intExtensionValue
            cRec.p85FreeText02 = Me.cbxItemsExtension.Text
        End If

        cRec.p85FreeText05 = strj71ValueFrom
        cRec.p85FreeText06 = strj71ValueUntil
        cRec.p85FreeText07 = strJ71ValueType
        cRec.p85FreeText08 = strJ71StringOperator
        cRec.p85FreeText09 = strJ71ValueString
        cRec.p85Message = opgField.SelectedItem.Text

        Master.Factory.p85TempBoxBL.Save(cRec)
        RefreshJ71TempList()
    End Sub
    Private Sub RefreshJ71TempList()
        Dim lisTMP As List(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).OrderBy(Function(p) p.p85FreeText04).ThenBy(Function(p) p.p85OtherKey2).ToList

        rpJ71.DataSource = lisTMP
        rpJ71.DataBind()

    End Sub
    Private Sub rpJ71_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpJ71.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)
        CType(e.Item.FindControl("p85id"), HiddenField).Value = cRec.PID.ToString

        If _lastField <> cRec.p85FreeText04 Then

            CType(e.Item.FindControl("x29Name"), Label).Text = cRec.p85FreeText03 & ":"
            e.Item.FindControl("nebo").Visible = False
        Else
            e.Item.FindControl("nebo").Visible = True
            
        End If
        CType(e.Item.FindControl("x29id"), HiddenField).Value = cRec.p85OtherKey2.ToString
        CType(e.Item.FindControl("j71Field"), HiddenField).Value = cRec.p85FreeText04
        If Not opgField.Items.FindByValue(cRec.p85OtherKey2.ToString & "-" & cRec.p85FreeText04) Is Nothing Then
            CType(e.Item.FindControl("x29Name"), Label).Text = opgField.Items.FindByValue(cRec.p85OtherKey2.ToString & "-" & cRec.p85FreeText04).Text & ":"
        End If

        CType(e.Item.FindControl("j71RecordName"), Label).Text = cRec.p85FreeText01
        If cRec.p85FreeText05 <> "" Or cRec.p85FreeText06 <> "" Then
            CType(e.Item.FindControl("j71RecordName"), Label).Text = cRec.p85FreeText05 & " - " & cRec.p85FreeText06
        End If
        CType(e.Item.FindControl("j71RecordPID"), HiddenField).Value = cRec.p85OtherKey1.ToString

        CType(e.Item.FindControl("j71RecordName_Extension"), Label).Text = cRec.p85FreeText02
        CType(e.Item.FindControl("j71RecordPID_Extension"), HiddenField).Value = cRec.p85OtherKey3.ToString
        If Not Me.CurrentIsOwner Then
            e.Item.FindControl("del").Visible = False
        End If
        _lastField = cRec.p85FreeText04
    End Sub
    Private Sub rpJ71_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rpJ71.ItemCommand
        Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(CInt(CType(e.Item.FindControl("p85id"), HiddenField).Value))
        If Master.Factory.p85TempBoxBL.Delete(cRec) Then
            RefreshJ71TempList()

        End If
    End Sub

    Private Sub query_builder_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        RefreshState()
    End Sub
    Private Sub RefreshState()
        'panQueryItems.Visible = False : panQueryNonItems.Visible = False
        'If Me.opgField.SelectedItem Is Nothing Then
        '    panQueryItems.Visible = False
        'Else
        '    panQueryItems.Visible = True
        'End If
        If rpJ71.Items.Count > 0 Then
            panJ71.Visible = True
        Else
            panJ71.Visible = False
        End If
        cmdClear.Visible = panJ71.Visible

        Dim bolIsSystem As Boolean = BO.BAS.BG(Me.j70IsSystem.Value)
        lblName.Visible = Not bolIsSystem
        Me.j70Name.Visible = Not bolIsSystem

        If cmdDelete.Visible Then
            cmdDelete.Visible = Not bolIsSystem
        End If

        If Not Me.CurrentIsOwner Then
            Master.RenameToolbarButton("run", "Spustit filtr")
            panRoles.Visible = False
            cmdClear.Visible = False
            cmdDelete.Visible = False
            lblName.Visible = False
            Me.j70Name.Visible = False
            Me.panQueryCondition.Visible = False
            Me.opgBin.Enabled = False
        Else
            Master.RenameToolbarButton("run", "Uložit a spustit filtr")
            panRoles.Visible = Not bolIsSystem
            Me.panQueryCondition.Visible = True
            Me.opgBin.Enabled = True
        End If

        Select Case Me.cbxStringOperator.SelectedValue
            Case "EMPTY", "NOTEMPTY"
                Me.txtStringValue.Visible = False
            Case Else
                Me.txtStringValue.Visible = True
        End Select

    End Sub

    Private Sub SetupJ70Combo()
        Dim mq As BO.myQuery = Nothing
        Dim lisJ70 As IEnumerable(Of BO.j70QueryTemplate) = Master.Factory.j70QueryTemplateBL.GetList(mq, Me.CurrentX29ID)
        If lisJ70.Count = 0 Then
            'uživatel zatím nemá žádný filtr - založit první j70IsSystem=1
            Dim c As New BO.j70QueryTemplate
            c.j70IsSystem = True
            c.j70Name = "Můj výchozí filtr"
            c.j03ID = Master.Factory.SysUser.PID
            c.x29ID = Me.CurrentX29ID
            If Not Master.Factory.j70QueryTemplateBL.Save(c, New List(Of BO.j71QueryTemplate_Item), Nothing) Then
                Master.Notify(Master.Factory.j70QueryTemplateBL.ErrorMessage, NotifyLevel.ErrorMessage)

            End If
            
            lisJ70 = Master.Factory.j70QueryTemplateBL.GetList(mq, Me.CurrentX29ID)

        End If
        j70ID.DataSource = lisJ70
        j70ID.DataBind()
        basUI.SelectDropdownlistValue(Me.j70ID, Master.DataPID.ToString)
    End Sub


    Private Sub ClearQuery()
        
        With Master.Factory.p85TempBoxBL
            .Truncate(ViewState("guid"))
        End With
        RefreshJ71TempList()

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        ClearQuery()
    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Master.DataPID = BO.BAS.IsNullInt(j70ID.SelectedValue)
        If Master.DataPID = 0 Then
            j70Name.Text = ""
            Me.j70IsSystem.Value = "0"
        End If
        roles1.RefreshGUID()
        RefreshRecord()

    End Sub

    Private Sub RefreshRecord()
        Master.DataPID = BO.BAS.IsNullInt(j70ID.SelectedValue)
        cmdDelete.Visible = False : cmdNew.Visible = False
        If Master.DataPID = 0 Then
            If j70ID.Items.Count = 0 Then
                j70Name.Text = "Můj výchozí filtr"
            End If
            j70Name.Enabled = True
            ClearQuery()
            Return
        End If
        Dim cRec As BO.j70QueryTemplate = Master.Factory.j70QueryTemplateBL.Load(Master.DataPID)
        With cRec
            j70Name.Text = .j70Name
            cmdDelete.Visible = True
            cmdNew.Visible = True
            lblTimeStamp.Text = .Timestamp
            
            Me.j70IsSystem.Value = BO.BAS.GB(.j70IsSystem)
        End With
        roles1.InhaleInitialData(cRec.PID)

        If cRec.j03ID = Master.Factory.SysUser.PID Or Master.Factory.SysUser.IsAdmin Then
            hidIsOwner.Value = "1"
        Else
            hidIsOwner.Value = "0"
        End If

        basUI.SelectRadiolistValue(Me.opgBin, cRec.j70BinFlag.ToString)
        Master.Factory.j70QueryTemplateBL.Setupj71TempList(Master.DataPID, ViewState("guid"))
        RefreshJ71TempList()

    End Sub

    Private Sub cmdNew_Click(sender As Object, e As ImageClickEventArgs) Handles cmdNew.Click
        Me.j70IsSystem.Value = "0"
        Me.hidIsOwner.Value = "1"
        If j70ID.Items.FindByValue("0") Is Nothing Then
            j70ID.Items.Insert(0, New ListItem("---Založit nový filtr---", "0"))
        End If
        j70ID.SelectedIndex = 0
        j70Name.Text = "" : j70Name.Focus()
        Me.opgBin.SelectedValue = "0"

        RefreshRecord()
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As ImageClickEventArgs) Handles cmdDelete.Click
        If Master.DataPID = 0 Then Return

        If Master.Factory.j70QueryTemplateBL.Delete(Master.DataPID) Then
            SetupJ70Combo()
            RefreshRecord()
        Else
            Master.Notify(Master.Factory.j70QueryTemplateBL.ErrorMessage, 2)
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "run" Then
            If Not Me.CurrentIsOwner Then
                SubmitQuery()
                Return
            End If
            roles1.SaveCurrentTempData()
            Dim cRec As BO.j70QueryTemplate = IIf(Master.DataPID = 0, New BO.j70QueryTemplate, Master.Factory.j70QueryTemplateBL.Load(Master.DataPID))
            With cRec
                .x29ID = Me.CurrentX29ID
                .j70Name = j70Name.Text
                .j70BinFlag = BO.BAS.IsNullInt(Me.opgBin.SelectedValue)
            End With

            Dim lisTMP As List(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).ToList
            Dim lisJ71 As New List(Of BO.j71QueryTemplate_Item)
            For Each cTMP In lisTMP
                Dim c As New BO.j71QueryTemplate_Item
                c.j71RecordPID = cTMP.p85OtherKey1
                c.j71RecordName = cTMP.p85FreeText01

                c.x29ID = cTMP.p85OtherKey2
                c.j71Field = cTMP.p85FreeText04
                c.j71RecordPID_Extension = cTMP.p85OtherKey3
                c.j71RecordName_Extension = cTMP.p85FreeText02
                c.j71ValueFrom = cTMP.p85FreeText05
                c.j71ValueUntil = cTMP.p85FreeText06
                c.j71ValueType = cTMP.p85FreeText07
                c.j71StringOperator = cTMP.p85FreeText08
                c.j71ValueString = cTMP.p85FreeText09
                c.j71FieldLabel = cTMP.p85Message
                lisJ71.Add(c)
            Next
            Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()
            If roles1.ErrorMessage <> "" Then
                Master.Notify(roles1.ErrorMessage, 2)
                Return
            End If


            If Master.Factory.j70QueryTemplateBL.Save(cRec, lisJ71, lisX69) Then
                Master.DataPID = Master.Factory.j70QueryTemplateBL.LastSavedPID
                SubmitQuery()
            Else
                Master.Notify(Master.Factory.j70QueryTemplateBL.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End If
    End Sub

    Private Sub SubmitQuery()
        Master.Factory.j03UserBL.SetUserParam(CStr(ViewState("x36key")), Master.DataPID.ToString)
        Master.CloseAndRefreshParent("j70-run")
    End Sub
    Private Sub cmdAddX69_Click(sender As Object, e As EventArgs) Handles cmdAddX69.Click
        roles1.AddNewRow()
    End Sub

    
    Private Sub cmdAdd2Query_Click(sender As Object, e As EventArgs) Handles cmdAdd2Query.Click
        SaveTempQueryItem()
    End Sub

    Private Sub cmdAdd2QueryNonItems_Click(sender As Object, e As EventArgs) Handles cmdAdd2QueryNonItems.Click
        SaveTempQueryItem()
    End Sub

    Private Sub cmdAdd2QueryPeriod_Click(sender As Object, e As EventArgs) Handles cmdAdd2QueryPeriod.Click
        SaveTempQueryItem()
    End Sub

    Private Sub cmdAdd2QueryString_Click(sender As Object, e As EventArgs) Handles cmdAdd2QueryString.Click
        SaveTempQueryItem()
    End Sub
End Class