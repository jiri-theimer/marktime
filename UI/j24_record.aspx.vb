Public Class j24_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub j24_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Typ nepersonálního zdroje"
            End With

            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then Return

        Dim cRec As BO.j24NonPersonType = Master.Factory.j24NonePersonTypeBL.Load(Master.DataPID)
        With cRec
            Me.j24Name.Text = .j24Name
            Me.j24Ordinary.Value = .j24Ordinary

            Master.Timestamp = .Timestamp

            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.j24NonePersonTypeBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("j24-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.j24NonePersonTypeBL
            Dim cRec As BO.j24NonPersonType = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.j24NonPersonType)
            cRec.j24Name = Me.j24Name.Text
            cRec.j24Ordinary = BO.BAS.IsNullInt(Me.j24Ordinary.Value)
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil

            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("j24-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub
End Class