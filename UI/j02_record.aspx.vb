Public Class j02_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub j02_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "j02_record"
    End Sub
    'Protected Overrides Sub InitializeCulture()
    '    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US")
    '    System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")
    '    MyBase.InitializeCulture()
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            If Request.Item("iscontact") = "1" Then
                'režim zakládání kontaktní osoby
                Me.j02IsIntraPerson.SelectedValue = "0"
            End If
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/person_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Osobní profil"

                Me.c21ID.DataSource = .Factory.c21FondCalendarBL.GetList(New BO.myQuery)
                Me.c21ID.DataBind()
                Me.j17ID.DataSource = .Factory.j17CountryBL.GetList(New BO.myQuery)
                Me.j17ID.DataBind()
                Me.j18ID.DataSource = Master.Factory.j18RegionBL.GetList(New BO.myQuery)
                Me.j18ID.DataBind()
            End With


            RefreshRecord()

            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub

    Private Sub RefreshRecord()
        j07ID.DataSource = Master.Factory.j07PersonPositionBL.GetList(New BO.myQuery)
        j07ID.DataBind()


        If Master.DataPID = 0 Then
            Handle_FF()
            Return
        End If

        Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
        With cRec
            Me.j02Email.Text = .j02Email
            Me.j02Code.Text = .j02Code
            Me.j02FirstName.Text = .j02FirstName
            Me.j02LastName.Text = .j02LastName
            Me.j02TitleAfterName.SetText(.j02TitleAfterName)
            Me.j02TitleBeforeName.SetText(.j02TitleBeforeName)
            Me.j02Mobile.Text = .j02Mobile
            Me.j02Phone.Text = .j02Phone
            Me.j07ID.SelectedValue = .j07ID.ToString
            Handle_FF()
            Me.j17ID.SelectedValue = .j17ID.ToString
            Me.c21ID.SelectedValue = .c21ID.ToString
            Me.j18ID.SelectedValue = .j18ID.ToString
            Me.j02Office.Text = .j02Office
            Me.j02EmailSignature.Text = .j02EmailSignature
            Me.j02IsIntraPerson.SelectedValue = BO.BAS.GB(.j02IsIntraPerson)
            Me.j02JobTitle.SetText(.j02JobTitle)
            Me.j02RobotAddress.Text = .j02RobotAddress
            Me.j02ExternalPID.Text = .j02ExternalPID
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
            Master.Timestamp = .Timestamp


            'val1.InitVals(.ValidFrom, .ValidUntil, .DateInsert)
        End With

    End Sub
    Private Sub Handle_FF()
        With RadTabStrip1.FindTabByValue("ff")
            If .Visible Then
                Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.j02Person, Master.DataPID, BO.BAS.IsNullInt(Me.j07ID.SelectedValue))
                ff1.FillData(fields)
                .Text = BO.BAS.OM2(.Text, ff1.FieldsCount.ToString)
            End If
        End With
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.j02PersonBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("j02-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.j02PersonBL
            Dim cRec As BO.j02Person = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.j02Person)
            With cRec
                .j07ID = BO.BAS.IsNullInt(j07ID.SelectedValue)
                .j17ID = BO.BAS.IsNullInt(j17ID.SelectedValue)
                .c21ID = BO.BAS.IsNullInt(c21ID.SelectedValue)
                .j18ID = BO.BAS.IsNullInt(Me.j18ID.SelectedValue)
                .j02FirstName = j02FirstName.Text
                .j02LastName = j02LastName.Text
                .j02TitleBeforeName = j02TitleBeforeName.Text
                .j02TitleAfterName = j02TitleAfterName.Text
                .j02Code = Me.j02Code.Text
                .j02Mobile = j02Mobile.Text
                .j02Phone = j02Phone.Text
                .j02Office = Me.j02Office.Text
                .j02Email = j02Email.Text
                .j02EmailSignature = j02EmailSignature.Text
                .j02IsIntraPerson = BO.BAS.BG(Me.j02IsIntraPerson.SelectedValue)
                .j02JobTitle = Me.j02JobTitle.Text
                .j02RobotAddress = Me.j02RobotAddress.Text
                .j02ExternalPID = Me.j02ExternalPID.Text
                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
            End With

            Dim lisFF As List(Of BO.FreeField) = Me.ff1.GetValues()

            If .Save(cRec, lisFF) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("j02-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

   
    Private Sub j07ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles j07ID.NeedMissingItem
        Dim cRec As BO.j07PersonPosition = Master.Factory.j07PersonPositionBL.Load(BO.BAS.IsNullInt(strFoundedMissingItemValue))
        strAddMissingItemText = cRec.j07Name
    End Sub

    Private Sub c21ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles c21ID.NeedMissingItem
        Dim cRec As BO.c21FondCalendar = Master.Factory.c21FondCalendarBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then strAddMissingItemText = cRec.c21Name
    End Sub

    Private Sub j17ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles j17ID.NeedMissingItem
        Dim cRec As BO.j17Country = Master.Factory.j17CountryBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then strAddMissingItemText = cRec.j17Name
    End Sub

    Private Sub j18ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles j18ID.NeedMissingItem
        Dim cRec As BO.j18Region = Master.Factory.j18RegionBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then strAddMissingItemText = cRec.j18Name
    End Sub

    Private Sub j02_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim b As Boolean = BO.BAS.BG(Me.j02IsIntraPerson.SelectedValue)
        lblJ07ID.Visible = b : Me.j07ID.Visible = b
        lblJ18ID.Visible = b : Me.j18ID.Visible = b
        lblC21ID.Visible = b : Me.c21ID.Visible = b
        trJ17ID.Visible = b
        If b Then lblj02Email.CssClass = "lblReq" Else lblj02Email.CssClass = "lbl"
        lblj02EmailSignature.Visible = b : Me.j02EmailSignature.Visible = b

        trJobTitle.Visible = Not b
    End Sub
End Class