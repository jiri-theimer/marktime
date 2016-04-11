Public Class x18_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

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

        Dim cRec As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Master.DataPID)
        With cRec
            Me.x23ID.SelectedValue = .x23ID.ToString
            Me.x18Name.Text = .x18Name
            Me.x18Ordinary.Value = .x18Ordinary
            Me.x18IsMultiSelect.Checked = .x18IsMultiSelect
            Master.Timestamp = .Timestamp

            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With
        basUI.CheckItems(Me.x29IDs, Master.Factory.x18EntityCategoryBL.GetList_x29(Master.DataPID).Select(Function(p) p.PID).ToList)
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
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
            Dim x29IDs As List(Of Integer) = basUI.GetCheckedItems(Me.x29IDs)
            If .Save(cRec, x29IDs) Then
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

    End Sub
End Class