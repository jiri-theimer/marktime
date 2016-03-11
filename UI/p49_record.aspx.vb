Public Class p49_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p49_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Public Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p41ID.Value)
        End Get
        Set(value As Integer)
            Me.p41ID.Value = value.ToString
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .HeaderIcon = "Images/finplan_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Záznam finančního plánu"
                Me.CurrentP41ID = BO.BAS.IsNullInt(Request.Item("p41id"))
                If .DataPID = 0 And Me.CurrentP41ID = 0 Then
                    .StopPage("p41id is missing")
                End If
            End With

            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub

    Private Sub RefreshRecord()
        Me.j27ID.DataSource = Master.Factory.ftBL.GetList_J27(New BO.myQuery)
        Me.j27ID.DataBind()
        Me.j27ID.SelectedValue = Master.Factory.x35GlobalParam.j27ID_Invoice.ToString

        Me.p34ID.DataSource = Master.Factory.p34ActivityGroupBL.GetList(New BO.myQuery).Where(Function(p) p.p33ID = BO.p33IdENUM.PenizeBezDPH Or p.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu)
        Me.p34ID.DataBind()

        Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(Me.CurrentP41ID)

        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cP41)
        If Not cDisp.p49_Create Then
            Master.StopPage("Nedisponujete oprávněním k zapisování finančního plánu do tohoto projektu.")
        End If

        If Master.DataPID = 0 Then
            Me.p41ID.Value = cP41.PID.ToString
            Me.p41ID.Text = cP41.FullName
            Dim cRecLast As BO.p49FinancialPlan = Master.Factory.p49FinancialPlanBL.LoadMyLastCreated()
            If Not cRecLast Is Nothing Then
                With cRecLast
                    Me.j27ID.SelectedValue = .j27ID.ToString
                    Me.p49DateFrom.SelectedDate = .p49DateFrom
                    Me.p49DateUntil.SelectedDate = .p49DateUntil
                    Me.p34ID.SelectedValue = .p34ID.ToString
                    Handle_ChangeP34ID(.p34ID)
                    If .p32ID <> 0 Then
                        Me.p32ID.SelectedValue = .p32ID.ToString
                    End If
                End With
            End If
            Return
        End If
        Dim cRec As BO.p49FinancialPlan = Master.Factory.p49FinancialPlanBL.Load(Master.DataPID)
        With cRec
            Me.p41ID.Value = .p41ID.ToString
            Me.p41ID.Text = .Project
            Me.j02ID.Value = .j02ID.ToString
            Me.j02ID.Text = .Person
            Me.j27ID.SelectedValue = .j27ID.ToString
            Me.p34ID.SelectedValue = .p34ID.ToString
            Handle_ChangeP34ID(.p34ID)
            Me.p32ID.SelectedValue = .p32ID.ToString
            Me.p49Amount.Value = .p49Amount
            Me.p49Text.Text = .p49Text
            Me.p49DateFrom.SelectedDate = .p49DateFrom
            Me.p49DateUntil.SelectedDate = .p49DateUntil
            Master.Timestamp = .Timestamp
        End With
    End Sub
    Private Sub Handle_ChangeP34ID(intP34ID As Integer)
        Dim mq As New BO.myQueryP32
        mq.p34ID = intP34ID
        Me.p32ID.DataSource = Master.Factory.p32ActivityBL.GetList(mq)
        Me.p32ID.DataBind()
    End Sub

    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p49FinancialPlanBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p49-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub
    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p49FinancialPlanBL
            Dim cRec As BO.p49FinancialPlan = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p49FinancialPlan)
            cRec.p41ID = BO.BAS.IsNullInt(Me.p41ID.Value)
            cRec.j02ID = BO.BAS.IsNullInt(Me.j02ID.Value)
            cRec.p34ID = BO.BAS.IsNullInt(Me.p34ID.SelectedValue)
            cRec.p32ID = BO.BAS.IsNullInt(Me.p32ID.SelectedValue)
            cRec.j27ID = BO.BAS.IsNullInt(Me.j27ID.SelectedValue)
            If Not Me.p49DateFrom.IsEmpty Then cRec.p49DateFrom = Me.p49DateFrom.SelectedDate
            If Not Me.p49DateUntil.IsEmpty Then cRec.p49DateUntil = Me.p49DateUntil.SelectedDate
            cRec.p49Amount = BO.BAS.IsNullNum(Me.p49Amount.Value)
            cRec.p49Text = Me.p49Text.Text
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil

            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("p49-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub p34ID_SelectedIndexChanged(OldValue As String, OldText As String, CurValue As String, CurText As String) Handles p34ID.SelectedIndexChanged
        Handle_ChangeP34ID(BO.BAS.IsNullInt(Me.p34ID.SelectedValue))
    End Sub
End Class