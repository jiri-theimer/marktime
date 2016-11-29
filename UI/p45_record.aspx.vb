Public Class p45_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p45_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Public Property CurrentP41ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidP41ID.Value)
        End Get
        Set(value As Integer)
            hidP41ID.Value = value.ToString
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))    'p41ID
            End With
            Me.CurrentP41ID = BO.BAS.IsNullInt(Request.Item("p41id"))


            Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Me.CurrentP41ID)
            If Not cRec.p41PlanFrom Is Nothing Then Me.p45PlanFrom.SelectedDate = cRec.p41PlanFrom Else Me.p45PlanFrom.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1)
            If Not cRec.p41PlanUntil Is Nothing Then Me.p45PlanUntil.SelectedDate = cRec.p41PlanUntil

            Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
            If Not cDisp.p45_Owner Then
                Master.StopPage("V tomto projektu nedisponujete oprávněním k definici rozpočtu.")
            End If

            RefreshRecord()
            Master.HeaderText = "Hlavička rozpočtu projektu | " & Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, Me.CurrentP41ID)

            If Request.Item("clone") = "1" Then
                Me.hidClonePID.Value = Master.DataPID.ToString
                Master.DataPID = 0
                panCreateClone.Visible = True
                Master.HeaderText = "Zkopírovat rozpočet do nového"

            End If

        End If
    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            lblVersionIndex.Visible = False
            Me.chkMakeCurrentAsFirstVersion.Checked = True
            Return
        End If
        Dim cRec As BO.p45Budget = Master.Factory.p45BudgetBL.Load(Master.DataPID)
        With cRec
            Me.CurrentP41ID = .p41ID
            Me.p45PlanFrom.SelectedDate = .p45PlanFrom
            Me.p45PlanUntil.SelectedDate = .p45PlanUntil
            Me.p45Name.Text = .p45Name
            Me.p45VersionIndex.Text = .p45VersionIndex.ToString
            Me.chkMakeCurrentAsFirstVersion.Checked = Not .IsClosed
            If .IsClosed Then
                Master.ChangeToolbarSkin("BlackMetroTouch")
            End If
        End With
       D)
        
        ''Dim lis As IEnumerable(Of BO.p45Budget) = Master.Factory.p45BudgetBL.GetList(Master.DataPID)


    End Sub

    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p45BudgetBL
            If .Delete(Master.DataPID) Then
                Master.CloseAndRefreshParent("p45-delete")
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With

    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        If BO.BAS.IsNullInt(Me.hidClonePID.Value) <> 0 Then
            Dim cRec As BO.p45Budget = Master.Factory.p45BudgetBL.Load(CInt(Me.hidClonePID.Value))
            Dim lisP46 As New List(Of BO.p46BudgetPerson)
            If Me.chkCloneP46.Checked Then lisP46 = Master.Factory.p45BudgetBL.GetList_p46(cRec.PID).ToList
            Dim mqP49 As New BO.myQueryP49
            mqP49.p45ID = cRec.PID
            Dim lisP49 As New List(Of BO.p49FinancialPlan)
            If Me.chkCloneP49.Checked Then lisP49 = Master.Factory.p49FinancialPlanBL.GetList(mqP49).ToList
            For Each c In lisP46
                c.SetPID(0)
            Next
            For Each c In lisP49
                c.SetPID(0)
            Next
            Dim mqP47 As New BO.myQueryP47
            mqP47.p45ID = cRec.PID
            Dim lisP47 As List(Of BO.p47CapacityPlan) = Master.Factory.p47CapacityPlanBL.GetList(mqP47)

            cRec.SetPID(0)
            '' Dim intNewP45ID As Integer = 0
            With Master.Factory.p45BudgetBL
                If Not .Save(cRec, lisP46, lisP49) Then
                    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    Return
                Else
                    Master.DataPID = .LastSavedPID
                    ''If Me.chkMakeCurrentAsFirstVersion.Checked Then
                    ''    .MakeActualVersion(Master.DataPID)
                    ''End If
                    If Me.chkCloneP47.Checked Then
                        lisP46 = .GetList_p46(Master.DataPID).ToList
                        For Each c In lisP47
                            c.p46ID = lisP46.First(Function(p) p.j02ID = c.j02ID).PID
                        Next
                        Master.Factory.p47CapacityPlanBL.SaveProjectPlan(Master.DataPID, lisP47, Nothing)
                    End If
                End If
            End With
        End If
        With Master.Factory.p45BudgetBL
            Dim cRec As BO.p45Budget = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p45Budget)
            If Not Me.p45PlanFrom.IsEmpty Then cRec.p45PlanFrom = Me.p45PlanFrom.SelectedDate
            If Not Me.p45PlanUntil.IsEmpty Then cRec.p45PlanUntil = Me.p45PlanUntil.SelectedDate
            cRec.p45Name = Me.p45Name.Text
            cRec.p41ID = Me.CurrentP41ID
            If Not Me.chkMakeCurrentAsFirstVersion.Checked Then
                cRec.ValidUntil = Now.AddMinutes(-1)
                If cRec.ValidFrom > cRec.ValidUntil Then cRec.ValidFrom = cRec.ValidUntil.AddMinutes(-1)
            Else
                cRec.ValidUntil = DateSerial(3000, 1, 1)
            End If
            If .Save(cRec, Nothing, Nothing) Then
                Master.DataPID = .LastSavedPID
                If Me.chkMakeCurrentAsFirstVersion.Checked Then
                    If Not .MakeActualVersion(Master.DataPID) Then
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End If
                Master.CloseAndRefreshParent("p45-save")
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub

    

    
End Class