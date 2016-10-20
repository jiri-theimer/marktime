Public Class x25_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub x25_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Combo položka"
                Me.x23ID.DataSource = .Factory.x23EntityField_ComboBL.GetList(New BO.myQuery)
                Me.x23ID.DataBind()
                If Request.Item("x23id") <> "" Then
                    Me.x23ID.SelectedValue = Request.Item("x23id")
                End If
            End With

            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then Return

        Dim cRec As BO.x25EntityField_ComboValue = Master.Factory.x25EntityField_ComboValueBL.Load(Master.DataPID)
        With cRec
            Me.x23ID.SelectedValue = .x23ID.ToString
            Me.x25Name.Text = .x25Name
            Me.x25Ordinary.Value = .x25Ordinary
            Me.x25UserKey.Text = .x25UserKey
            Master.Timestamp = .Timestamp
            basUI.SetColorToPicker(Me.x25BackColor, .x25BackColor)
            basUI.SetColorToPicker(Me.x25ForeColor, .x25ForeColor)
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With
        Dim cX23 As BO.x23EntityField_Combo = Master.Factory.x23EntityField_ComboBL.Load(cRec.x23ID)
        If cX23.x23DataSource <> "" Then
            Master.Notify("Tato položka byla vložena automaticky, protože pochází z externího datového zdroje.", NotifyLevel.InfoMessage)
        Else
            cmdX23.Visible = True
            cmdX23.NavigateUrl = "x23_record.aspx?pid=" & cRec.x23ID.ToString
        End If
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.x25EntityField_ComboValueBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("x25-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.x25EntityField_ComboValueBL
            Dim cRec As BO.x25EntityField_ComboValue = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.x25EntityField_ComboValue)
            cRec.x25Name = Me.x25Name.Text
            cRec.x25Ordinary = BO.BAS.IsNullInt(Me.x25Ordinary.Value)
            cRec.x23ID = BO.BAS.IsNullInt(Me.x23ID.SelectedValue)
            cRec.x25UserKey = Me.x25UserKey.Text
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
            cRec.x25BackColor = basUI.GetColorFromPicker(Me.x25BackColor)
            cRec.x25ForeColor = basUI.GetColorFromPicker(Me.x25ForeColor)

            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("x25-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub x25_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Master.DataPID <> 0 Then
            Me.x23ID.Enabled = False
        Else
            Me.x23ID.Enabled = True
        End If
    End Sub
End Class