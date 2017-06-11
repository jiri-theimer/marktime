Public Class x18_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord
    Private _lisX22 As IEnumerable(Of BO.x22EntiyCategory_Binding) = Nothing

    Private Sub x18_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/label_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Nastavení štítku"
                Me.x23ID.DataSource = .Factory.x23EntityField_ComboBL.GetList(New BO.myQuery)
                Me.x23ID.DataBind()

            End With

            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then Return
        _lisX22 = Master.Factory.x18EntityCategoryBL.GetList_x22(Master.DataPID)

        Dim cRec As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Master.DataPID)
        With cRec
            Me.x18IsAllEntityTypes.Checked = .x18IsAllEntityTypes
            Me.x23ID.SelectedValue = .x23ID.ToString
            Me.x18Name.Text = .x18Name
            Me.x18Ordinary.Value = .x18Ordinary
            Me.x18IsMultiSelect.Checked = .x18IsMultiSelect
            Me.x18IsRequired.Checked = .x18IsRequired
            Master.Timestamp = .Timestamp



            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With
        basUI.CheckItems(Me.x29IDs, Master.Factory.x18EntityCategoryBL.GetList_x29(Master.DataPID).Select(Function(p) p.PID).ToList)

        Handle_ChangeX29ID()
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
        With Master.Factory.x18EntityCategoryBL
            Dim cRec As BO.x18EntityCategory = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.x18EntityCategory)
            cRec.x18Name = Me.x18Name.Text
            cRec.x18Ordinary = BO.BAS.IsNullInt(Me.x18Ordinary.Value)
            cRec.x23ID = BO.BAS.IsNullInt(Me.x23ID.SelectedValue)
            cRec.x18IsMultiSelect = Me.x18IsMultiSelect.Checked
            cRec.x18IsRequired = Me.x18IsRequired.Checked
            cRec.x18IsAllEntityTypes = Me.x18IsAllEntityTypes.Checked
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
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
            
            If .Save(cRec, x29IDs, lisX22) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("x18-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub x18_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.x23ID.SelectedValue <> "" Then
            cmdX23.NavigateUrl = "x23_record.aspx?pid=" & Me.x23ID.SelectedValue
            cmdX23.Visible = True
        Else
            cmdX23.Visible = False
        End If
        Me.panEntityTypes.Visible = Not Me.x18IsAllEntityTypes.Checked
        Me.x18IsRequired.Visible = Me.x18IsAllEntityTypes.Checked
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
End Class