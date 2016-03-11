Public Class p47_framework_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p47_framework_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
            ViewState("month") = Request.Item("month")
            ViewState("year") = Request.Item("year")
            ViewState("j02ids") = Request.Item("j02ids")
            Me.Period.Text = ViewState("month") & "/" & ViewState("year")
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("p41id"))
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
                .HeaderText = "Kapacitní plán | " & ViewState("month") & "/" & ViewState("year") & " | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
            End With

            SetupData()
        End If
    End Sub
    Private Function GetAppPlansInProject() As IEnumerable(Of BO.p47CapacityPlan)
        Dim mq As New BO.myQueryP47
        mq.p41ID = Master.DataPID
        mq.DateFrom = DateSerial(ViewState("year"), ViewState("month"), 1)
        mq.DateUntil = mq.DateFrom.AddMonths(1).AddDays(-1)
        Return Master.Factory.p47CapacityPlanBL.GetList(mq)
    End Function
    Private Function GetAllPersonsPlans() As IEnumerable(Of BO.p47CapacityPlan)
        Dim mq As New BO.myQueryP47
        mq.j02IDs = BO.BAS.ConvertPIDs2List(ViewState("j02ids"))
        mq.DateFrom = DateSerial(ViewState("year"), ViewState("month"), 1)
        mq.DateUntil = mq.DateFrom.AddMonths(1).AddDays(-1)
        Return Master.Factory.p47CapacityPlanBL.GetList(mq).Where(Function(p) p.p47HoursTotal <> 0)
    End Function
    Private Sub SetupData()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        With cRec
            Me.Project.Text = .FullName
            Try
                Me.PlanScope.Text = BO.BAS.FD(.p41PlanFrom.Value) & " - " & BO.BAS.FD(.p41PlanUntil.Value)
                Me.PlanScope.Text += " (" & DateDiff(DateInterval.Day, .p41PlanFrom.Value, .p41PlanUntil.Value).ToString & "d.)"
            Catch ex As Exception

            End Try
            Me.clue_project.Attributes("rel") = "clue_p41_record.aspx?pid=" & .PID.ToString
        End With

        
        Dim lisP47 As IEnumerable(Of BO.p47CapacityPlan) = GetAppPlansInProject(), lisP47_All As IEnumerable(Of BO.p47CapacityPlan) = GetAllPersonsPlans()
        For Each cP47 In lisP47
            Dim c As New BO.p85TempBox()
            c.p85GUID = ViewState("guid")
            c.p85DataPID = cP47.PID
            c.p85OtherKey1 = cP47.j02ID
            c.p85FreeText01 = cP47.Person
            c.p85FreeFloat01 = cP47.p47HoursBillable
            c.p85FreeFloat02 = cP47.p47HoursNonBillable
            c.p85FreeFloat03 = cP47.p47HoursTotal
            c.p85OtherKey2 = CInt(ViewState("year"))
            c.p85OtherKey3 = CInt(ViewState("month"))
            If lisP47_All.Where(Function(p) p.j02ID = cP47.j02ID And p.p41ID <> Master.DataPID).Count > 0 Then
                c.p85FreeNumber01 = lisP47_All.Where(Function(p) p.j02ID = cP47.j02ID And p.p41ID <> Master.DataPID).Sum(Function(p) p.p47HoursTotal)
            End If
            Master.Factory.p85TempBoxBL.Save(c)
        Next
        Dim mqJ02 As New BO.myQueryJ02
        mqJ02.PIDs = BO.BAS.ConvertPIDs2List(ViewState("j02ids"))
        Dim lisJ02 As IEnumerable(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList(mqJ02)
        For Each cJ02 In lisJ02
            If lisP47.Where(Function(p) p.j02ID = cJ02.PID).Count = 0 Then
                Dim c As New BO.p85TempBox()
                c.p85GUID = ViewState("guid")
                c.p85OtherKey1 = cJ02.PID
                c.p85FreeText01 = cJ02.FullNameDesc
                c.p85OtherKey2 = CInt(ViewState("year"))
                c.p85OtherKey3 = CInt(ViewState("month"))
                If lisP47_All.Where(Function(p) p.j02ID = cJ02.PID And p.p41ID <> Master.DataPID).Count > 0 Then
                    c.p85FreeNumber01 = lisP47_All.Where(Function(p) p.j02ID = cJ02.PID And p.p41ID <> Master.DataPID).Sum(Function(p) p.p47HoursTotal)
                End If
                Master.Factory.p85TempBoxBL.Save(c)
            End If
        Next

        Me.Total_Fa.Text = BO.BAS.FN3(lisP47.Sum(Function(p) p.p47HoursBillable))
        Me.Total_Nefa.Text = BO.BAS.FN3(lisP47.Sum(Function(p) p.p47HoursNonBillable))
        Me.Total_Project.Text = BO.BAS.FN3(lisP47.Sum(Function(p) p.p47HoursTotal))
        RefreshTempList()
    End Sub

    Private Sub RefreshTempList()
        rp1.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).OrderBy(Function(p) p.p85FreeText01)
        rp1.DataBind()
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)
        CType(e.Item.FindControl("p85id"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("Person"), Label)
            .Text = cRec.p85FreeText01
            If BO.BAS.ConvertPIDs2List(ViewState("j02ids")).Where(Function(p) p = cRec.p85OtherKey1).Count > 0 Then
                .BackColor = Drawing.Color.Yellow
            End If
        End With
        CType(e.Item.FindControl("clue_person"), HyperLink).Attributes("rel") = "clue_j02_capacity.aspx?pid=" & cRec.p85OtherKey1.ToString & "&p41id=" & Master.DataPID.ToString
        
        If cRec.p85FreeFloat01 <> 0 Then
            With CType(e.Item.FindControl("p85FreeFloat01"), Telerik.Web.UI.RadNumericTextBox)
                .Value = cRec.p85FreeFloat01
                .Style.Item("background-color") = "#98FB98"
            End With
        End If
        If cRec.p85FreeFloat02 <> 0 Then
            With CType(e.Item.FindControl("p85FreeFloat02"), Telerik.Web.UI.RadNumericTextBox)
                .Value = cRec.p85FreeFloat02
                .Style.Item("background-color") = "#FFA07A"
            End With
        End If
        If cRec.p85FreeFloat01 + cRec.p85FreeFloat02 <> 0 Then
            CType(e.Item.FindControl("p47HoursTotal"), TextBox).Text = BO.BAS.FN3(cRec.p85FreeFloat01 + cRec.p85FreeFloat02)

        End If
        If cRec.p85FreeNumber01 <> 0 Then
            CType(e.Item.FindControl("Plan_MimoProjekt"), Label).Text = BO.BAS.FN3(cRec.p85FreeNumber01)
            CType(e.Item.FindControl("clue_oplan"), HyperLink).Attributes("rel") = "clue_j02_oplan.aspx?pid=" & cRec.p85OtherKey1.ToString & "&month=" & ViewState("month") & "&year=" & ViewState("year")
        Else
            e.Item.FindControl("clue_oplan").Visible = False
        End If
        If cRec.p85FreeFloat01 + cRec.p85FreeFloat02 + cRec.p85FreeNumber01 <> 0 Then
            CType(e.Item.FindControl("Plan_Celkem"), Label).Text = BO.BAS.FN3(cRec.p85FreeFloat01 + cRec.p85FreeFloat02 + cRec.p85FreeNumber01)
        End If

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Save2Temp()
            Dim lisOrigP47 As IEnumerable(Of BO.p47CapacityPlan) = GetAppPlansInProject()
            Dim lisP47 As New List(Of BO.p47CapacityPlan)
            Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), True).Where(Function(p) p.p85DataPID <> 0 Or p.p85FreeFloat01 <> 0 Or p.p85FreeFloat02 <> 0)
            For Each cTMP In lisTemp
                Dim c As New BO.p47CapacityPlan
                If cTMP.p85DataPID <> 0 Then
                    c = lisOrigP47.First(Function(p) p.PID = cTMP.p85DataPID)
                    If cTMP.p85IsDeleted Then
                        c.SetAsDeleted()
                    End If
                Else
                    c.p41ID = Master.DataPID
                    c.j02ID = cTMP.p85OtherKey1
                    Dim intYear As Integer = cTMP.p85OtherKey2, intMonth As Integer = cTMP.p85OtherKey3
                    c.p47DateFrom = DateSerial(intYear, intMonth, 1)
                    c.p47DateUntil = c.p47DateFrom.AddMonths(1).AddDays(-1)
                End If
                c.p47HoursBillable = cTMP.p85FreeFloat01
                c.p47HoursNonBillable = cTMP.p85FreeFloat02
                c.p47HoursTotal = c.p47HoursBillable + c.p47HoursNonBillable
                lisP47.Add(c)
            Next

            If Master.Factory.p47CapacityPlanBL.SaveProjectPlan(Master.DataPID, lisP47) Then
                Master.CloseAndRefreshParent()
            Else
                Master.Notify(Master.Factory.p47CapacityPlanBL.ErrorMessage, NotifyLevel.ErrorMessage)
            End If

        End If
    End Sub

    Private Sub Save2Temp()

        For Each ri As RepeaterItem In rp1.Items
            Dim intP85ID As Integer = CInt(CType(ri.FindControl("p85id"), HiddenField).Value)
            Dim c As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(intP85ID)
            c.p85FreeFloat01 = BO.BAS.IsNullNum(CType(ri.FindControl("p85FreeFloat01"), Telerik.Web.UI.RadNumericTextBox).Value)
            c.p85FreeFloat02 = BO.BAS.IsNullNum(CType(ri.FindControl("p85FreeFloat02"), Telerik.Web.UI.RadNumericTextBox).Text)

            Master.Factory.p85TempBoxBL.Save(c)
        Next
    End Sub

    Private Sub cmdAddPerson_Click(sender As Object, e As EventArgs) Handles cmdAddPerson.Click
        Save2Temp()
        Dim intJ02ID As Integer = BO.BAS.IsNullInt(Me.j02ID_Add.Value)
        If intJ02ID = 0 Then
            Master.Notify("Musíte vybrat osobu.", NotifyLevel.WarningMessage) : Return
        End If
        Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), True)
        If lisTemp.Where(Function(p) p.p85OtherKey1 = intJ02ID).Count > 0 Then
            Master.Notify("Osoba [" & Me.j02ID_Add.Text & "] již v plánu existuje.", NotifyLevel.WarningMessage) : Return
        End If
        Dim cTMP As New BO.p85TempBox()
        With cTMP
            .p85GUID = ViewState("guid")
            .p85OtherKey1 = intJ02ID
            .p85FreeText01 = Master.Factory.j02PersonBL.Load(intJ02ID).FullNameDesc
            .p85OtherKey2 = ViewState("year")
            .p85OtherKey3 = ViewState("month")
        End With
        Master.Factory.p85TempBoxBL.Save(cTMP)
        RefreshTempList()
    End Sub
End Class