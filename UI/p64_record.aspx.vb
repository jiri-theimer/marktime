Public Class p64_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p64_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Public ReadOnly Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP41ID.Value)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hidP41ID.Value = Request.Item("p41id")
            With Master
                If Me.CurrentP41ID = 0 Then .StopPage("p41id is missing.")
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
        If Master.DataPID = 0 Then
            Dim mq As New BO.myQuery
            mq.Closed = BO.BooleanQueryMode.NoQuery
            Dim lis As IEnumerable(Of BO.p64Binder) = Master.Factory.p64BinderBL.GetList(Me.CurrentP41ID, mq)
            If lis.Count > 0 Then
                Me.p64Ordinary.Value = lis.Max(Function(p) p.p64Ordinary) + 1
            End If
            Return
        End If

        Dim cRec As BO.p64Binder = Master.Factory.p64BinderBL.Load(Master.DataPID)
        With cRec
            Me.p64Name.Text = .p64Name
            Me.p64Ordinary.Value = .p64Ordinary
            Me.p64Code.Text = .p64Code
            Me.p64Description.Text = .p64Description
            Me.p64Location.Text = .p64Location
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
            cRec.p41ID = Me.CurrentP41ID
            cRec.p64Name = Me.p64Name.Text
            cRec.p64Ordinary = BO.BAS.IsNullInt(Me.p64Ordinary.Value)
            cRec.p64Code = Me.p64Code.Text
            cRec.p64Description = Me.p64Description.Text
            cRec.p64Location = Me.p64Location.Text
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