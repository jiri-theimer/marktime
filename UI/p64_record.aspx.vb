Public Class p64_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p64_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .HeaderIcon = "Images/binder_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Šanon projektu"
            End With
           
            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then Return

        Dim cRec As BO.p64Binder = Master.Factory.p64BinderBL.Load(Master.DataPID)
        With cRec
            Me.p64Name.Text = .p64Name
            Me.p64Ordinary.Value = .p64Ordinary
            Me.p64Code.Text = .p64Code
            Me.p64Description.Text = .p64Description
            Master.Timestamp = .Timestamp

            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p64BinderBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p64-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub
    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p64BinderBL
            Dim cRec As New BO.p64Binder
            If Master.DataPID <> 0 Then cRec = .Load(Master.DataPID)
            cRec.p64Name = Me.p64Name.Text
            cRec.p64Ordinary = BO.BAS.IsNullInt(Me.p64Ordinary.Value)
            cRec.p64Code = Me.p64Code.Text
            cRec.p64Description = Me.p64Description.Text
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil

            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("p64-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub
End Class