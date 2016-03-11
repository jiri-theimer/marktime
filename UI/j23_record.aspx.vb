Public Class j23_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub j23_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Nastavení nepersonálního zdroje"
            End With
            Me.j24ID.DataSource = Master.Factory.j24NonePersonTypeBL.GetList(New BO.myQuery)
            Me.j24ID.DataBind()
            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then Return

        Dim cRec As BO.j23NonPerson = Master.Factory.j23NonPersonBL.Load(Master.DataPID)
        With cRec
            Me.j23Name.Text = .j23Name
            Me.j23Ordinary.Value = .j23Ordinary
            Me.j23Code.Text = .j23Code
            Me.j24ID.SelectedValue = .j24ID.ToString

            Master.Timestamp = .Timestamp

            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.j23NonPersonBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("j23-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.j23NonPersonBL
            Dim cRec As BO.j23NonPerson = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.j23NonPerson)
            cRec.j23Name = Me.j23Name.Text
            cRec.j23Ordinary = BO.BAS.IsNullInt(Me.j23Ordinary.Value)
            cRec.j24ID = BO.BAS.IsNullInt(Me.j24ID.SelectedValue)
            cRec.j23Code = Me.j23Code.Text
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil

            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("j23-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub j24ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles j24ID.NeedMissingItem
        Dim cRec As BO.j24NonPersonType = Master.Factory.j24NonePersonTypeBL.Load(BO.BAS.IsNullInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then
            strAddMissingItemText = cRec.j24Name
        End If
    End Sub
End Class