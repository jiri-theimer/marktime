Public Class capacity_plan_entry
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Public Class PlanHours
        Public Property Year As Integer
        Public Property Month As Integer
        Public Property HoursTotal As Double
        Public Property HoursBillable As Double
        Public Property HoursNonBillable As Double
    End Class
    Public Class PlanRow
        Public Property Person As String

        Public Property j02ID As Integer
        Public Property Hours As List(Of PlanHours)
        Public Sub New(intJ02ID As Integer)

            Me.j02ID = intJ02ID
            Me.Hours = New List(Of PlanHours)
        End Sub
    End Class
    
    Public ReadOnly Property CurrentD1 As Date
        Get
            Dim d As Date = Me.LimitD1
            If Me.CurrentPageIndex > 0 Then
                d = d.AddMonths(12 * (Me.CurrentPageIndex))
            End If
            Return d


        End Get
    End Property
    Public ReadOnly Property CurrentD2 As Date
        Get
            Dim d As Date = Me.LimitD1
            If d.AddMonths(12).AddDays(-1) > Me.LimitD2 Then
                Return Me.LimitD2   'plánované období je menší než 12 měsíců
            Else
                Dim d1 As Date = Me.CurrentD1.AddMonths(12 * (Me.CurrentPageIndex))

                If d1.AddMonths(12).AddDays(-1) >= Me.LimitD2 Then
                    Return Me.LimitD2
                Else
                    Return d1.AddMonths(12).AddDays(-1)
                End If
            End If

        End Get

    End Property
    Public Property LimitD1 As Date
        Get
            Return BO.BAS.ConvertString2Date(Me.hidLimD1.Value)
        End Get
        Set(value As Date)
            Me.hidLimD1.Value = Format(value, "dd.MM.yyyy")
        End Set
    End Property
    Public Property LimitD2 As Date
        Get
            Return BO.BAS.ConvertString2Date(Me.hidLimD2.Value)
        End Get
        Set(value As Date)
            Me.hidLimD2.Value = Format(value, "dd.MM.yyyy")
        End Set
    End Property
    Public Property CurrentPageIndex As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidCurrentPageIndex.Value)
        End Get
        Set(value As Integer)
            Me.hidCurrentPageIndex.Value = value.ToString
        End Set
    End Property

    Private Sub capacity_plan_entry_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "capacity_plan_entry"
    End Sub
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
            ViewState("readonly") = "1"
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Master.DataPID = 0 Then .StopPage("pid missing.")
                .HeaderText = "Kapacitní plán | " & Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)


            End With


            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim mq As New BO.myQueryP47, d1 As Date = DateSerial(Year(Now), 1, 1), d2 As Date = DateSerial(Year(Now), 12, 31)

        mq.p41ID = Master.DataPID
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        lblHeader.Text = BO.BAS.FD(cRec.p41PlanFrom) & " - " & BO.BAS.FD(cRec.p41PlanUntil) & " (" & DateDiff(DateInterval.Day, cRec.p41PlanFrom.Value, cRec.p41PlanUntil.Value).ToString & "d.)"
        With cRec
            d1 = DateSerial(Year(.p41PlanFrom), Month(.p41PlanFrom), 1)
            d2 = DateSerial(Year(.p41PlanUntil), Month(.p41PlanUntil), 1).AddMonths(1).AddDays(-1)

        End With
        If Not Page.IsPostBack Then
            Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
            If cDisp.p47_Create Then
                Master.AddToolbarButton("Uložit změny", "save", 0, "Images/save.png")
                ViewState("readonly") = "0"
            Else
                cmdAddPerson.Visible = False
                Master.Notify("Kapacitní plán projektu můžete pouze číst, nikoliv upravovat.")
            End If
        End If

        Me.LimitD1 = d1
        Me.LimitD2 = d2

        SetupHeader(0)
        RenderTabHeader()

        mq.DateFrom = d1
        mq.DateUntil = d2
        Dim lis As IEnumerable(Of BO.p47CapacityPlan) = Master.Factory.p47CapacityPlanBL.GetList(mq).OrderBy(Function(p) p.j02ID).ThenBy(Function(p) p.p41ID)
        For Each c In lis
            Dim cTemp As New BO.p85TempBox()
            With cTemp
                .p85GUID = ViewState("guid")
                .p85DataPID = c.PID
                .p85OtherKey1 = c.j02ID
                .p85OtherKey2 = c.p41ID
                .p85FreeDate01 = c.p47DateFrom
                .p85FreeDate02 = c.p47DateUntil
                .p85FreeText01 = c.Person

                .p85FreeFloat01 = c.p47HoursBillable
                .p85FreeFloat02 = c.p47HoursNonBillable
                .p85FreeFloat03 = c.p47HoursBillable + c.p47HoursNonBillable
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next

        
        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, Master.DataPID)
        Dim j02ids As List(Of Integer) = lisX69.Select(Function(p) p.j02ID).Distinct.ToList
        Dim j11ids As List(Of Integer) = lisX69.Select(Function(p) p.j11ID).Distinct.ToList
        Dim persons As IEnumerable(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList_j02_join_j11(j02ids, j11ids)
        rpJ02.DataSource = persons
        rpJ02.DataBind()
        If rpJ02.Items.Count = 0 Then
            Master.Notify("V projektových rolích projektu nejsou přiřazeny osoby!", NotifyLevel.WarningMessage)
        End If

        RefreshFromTemp()

    End Sub

    Private Sub SetupHeader(intPageIndex As Integer)
        opgPageIndex.Items.Clear()
        Dim d As Date = Me.LimitD1, d2 As Date = Me.LimitD2, x As Integer = 1
        opgPageIndex.Items.Add(New ListItem("#1", Format(d, "dd.MM.yyyy")))

        While d <= d2
            If x > 12 Then
                opgPageIndex.Items.Add(New ListItem("#" & (opgPageIndex.Items.Count + 1).ToString, Format(d, "dd.MM.yyyy")))
                x = 1
            End If
            d = d.AddMonths(1)
            x += 1
        End While
        Me.CurrentPageIndex = intPageIndex
        opgPageIndex.SelectedIndex = intPageIndex
        If opgPageIndex.Items.Count <= 1 Then
            opgPageIndex.Visible = False
        Else
            opgPageIndex.Visible = True
        End If
    End Sub
    Private Sub RenderTabHeader()
        For i As Integer = 1 To 12
            CType(trHeader.FindControl("m" & i.ToString), Label).Text = ""


        Next
        Dim d As Date = Me.CurrentD1, x As Integer = 1, d2 As Date = Me.CurrentD2
        While d <= d2
            CType(trHeader.FindControl("m" & x.ToString), Label).Text = Format(d, "MM") & "/" & Year(d).ToString

            d = d.AddMonths(1)
            x += 1
        End While
    End Sub

    Private Sub rp1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rp1.ItemCommand

        Dim intJ02ID As Integer = BO.BAS.IsNullInt(CType(e.Item.FindControl("hidJ02ID"), HiddenField).Value)
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), True).Where(Function(p) p.p85OtherKey1 = intJ02ID)
        For Each c In lis
            Master.Factory.p85TempBoxBL.Delete(c)
        Next
        
        RefreshFromTemp()
    End Sub

    'Private Function GetDataSource(mq As BO.myQueryP47) As List(Of PlanRow)
    '    Dim lis As IEnumerable(Of BO.p47CapacityPlan) = Master.Factory.p47CapacityPlanBL.GetList(mq).OrderBy(Function(p) p.j02ID).ThenBy(Function(p) p.p41ID)
    '    Dim intLastJ02ID As Integer = 0, cRow As PlanRow = Nothing, intLastP41ID As Integer = 0
    '    Dim lisPlan As New List(Of PlanRow)
    '    For Each c In lis
    '        If intLastJ02ID <> c.j02ID Or intLastP41ID <> c.p41ID Then
    '            cRow = New PlanRow(c.j02ID, c.p41ID)
    '            cRow.Person = c.Person
    '            cRow.Project = c.Project
    '            lisPlan.Add(cRow)
    '        End If

    '        Dim cHours As New PlanHours
    '        With cHours
    '            .HoursBillable = c.p47HoursBillable
    '            .HoursNonBillable = c.p47HoursNonBillable
    '            .HoursTotal = c.p47HoursTotal
    '            .Year = Year(c.p47DateFrom)
    '            .Month = Month(c.p47DateFrom)
    '        End With
    '        cRow.Hours.Add(cHours)


    '        intLastJ02ID = c.j02ID
    '        intLastP41ID = c.p41ID
    '    Next
    '    Return lisPlan
    'End Function

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As PlanRow = CType(e.Item.DataItem, PlanRow)
        With CType(e.Item.FindControl("Person"), HyperLink)
            .Text = cRec.Person
            If ViewState("readonly") = "0" Then .NavigateUrl = "javascript:plan_item(" & cRec.j02ID.ToString & ")"
        End With

        CType(e.Item.FindControl("clue_person"), HyperLink).Attributes("rel") = "clue_j02_capacity.aspx?pid=" & cRec.j02ID.ToString & "&p41id=" & Master.DataPID.ToString

        CType(e.Item.FindControl("hidJ02ID"), HiddenField).Value = cRec.j02ID.ToString
        If ViewState("readonly") = "1" Then
            e.Item.FindControl("cmdDelete").Visible = False
        End If

        For Each c In cRec.Hours
            Dim x As Integer = GTI(e.Item, c.Year, c.Month)
            If x > 0 Then
                
                If c.HoursBillable <> 0 And c.HoursNonBillable <> 0 Then
                    CType(e.Item.FindControl("m" & x.ToString & "a"), Label).Text = FN(c.HoursBillable) & " + " & FN(c.HoursNonBillable) & " = "
                End If

                If c.HoursBillable <> 0 And c.HoursNonBillable <> 0 Then
                    CType(e.Item.FindControl("m" & x.ToString & "c"), Label).Text = FN(c.HoursTotal)
                    e.Item.FindControl("m" & x.ToString & "c").Visible = True
                Else
                    e.Item.FindControl("m" & x.ToString & "c").Visible = False
                End If

                If c.HoursTotal <> 0 Then
                    With CType(e.Item.FindControl("m" & x.ToString & "c"), Label)
                        .CssClass = "valbold"
                        .Text = FN(c.HoursTotal)
                        If c.HoursNonBillable = 0 And c.HoursBillable <> 0 Then
                            .BackColor = Me.totala.BackColor
                        End If
                        If c.HoursNonBillable <> 0 And c.HoursBillable = 0 Then
                            .BackColor = Me.totalb.BackColor
                        End If
                    End With
                End If


            End If
        Next

    End Sub

    Private Sub RefreshOutBoxes()
        For Each ri As RepeaterItem In rp1.Items
            Dim d As Date = Me.CurrentD1
            For x As Integer = 1 To 12
                Dim bolVisible As Boolean = True
                If d > Me.CurrentD2 Then bolVisible = False

                ri.FindControl("m" & x.ToString & "a").Visible = bolVisible

                ri.FindControl("m" & x.ToString & "c").Visible = bolVisible
                d = d.AddMonths(1)
            Next
        Next
    End Sub
    Private Sub RefreshTotals(lisPlan As List(Of PlanRow))
        Dim a(11) As Double, b(11) As Double, c(11) As Double

        For Each ri As RepeaterItem In rp1.Items
            Dim intJ02ID As Integer = CInt(CType(ri.FindControl("hidJ02ID"), HiddenField).Value)
            If lisPlan.Where(Function(p) p.j02ID = intJ02ID).Count > 0 Then
                Dim cRec As PlanRow = lisPlan.Where(Function(p) p.j02ID = intJ02ID)(0)
                Dim dblBillable As Double = 0, dblNonBillable As Double = 0
                For Each ph In cRec.Hours
                    Dim x As Integer = GTI(ri, ph.Year, ph.Month)
                    If x > 0 Then
                        dblBillable += ph.HoursBillable
                        dblNonBillable += ph.HoursNonBillable
                        a(x - 1) += ph.HoursBillable
                        b(x - 1) += ph.HoursNonBillable
                        c(x - 1) += ph.HoursTotal
                    End If
                Next
                If dblBillable <> 0 And dblNonBillable <> 0 Then
                    CType(ri.FindControl("totala"), Label).Text = FN(dblBillable)
                    CType(ri.FindControl("totalb"), Label).Text = FN(dblNonBillable)
                Else
                    If dblBillable <> 0 Then
                        CType(ri.FindControl("totalc"), Label).BackColor = Me.totala.BackColor
                    End If
                    If dblNonBillable <> 0 Then
                        CType(ri.FindControl("totalc"), Label).BackColor = Me.totalb.BackColor
                    End If
                End If
                CType(ri.FindControl("totalc"), Label).Text = FN(dblBillable + dblNonBillable)
            End If

        Next
        Dim dblA As Double, dblB As Double, dblC As Double
        For i As Integer = 1 To 12
            dblA += a(i - 1)
            dblB += b(i - 1)
            dblC += c(i - 1)
            CType(trFooter.FindControl("t" & i.ToString & "c"), Label).Text = FN(c(i - 1))

            If a(i - 1) <> 0 And b(i - 1) <> 0 Then
                ''CType(trFooter.FindControl("t" & i.ToString & "a"), Label).Text = FN(a(i - 1)) & " + " & FN(b(i - 1)) & " = "

            End If

        Next
        If dblA <> 0 And dblB <> 0 Then
            Me.totala.Text = FN(dblA)
            Me.totalb.Text = FN(dblB)
        End If
        Me.totalc.Text = FN(dblC)
    End Sub
    Private Function FN(dbl As Double) As String
        If dbl = 0 Then
            Return ""
        Else
            Return dbl.ToString
        End If
    End Function

    Private Function GTI(ri As RepeaterItem, intYear As Integer, intMonth As Integer) As Integer
        Dim d As Date = Me.CurrentD1
        For i As Integer = 1 To 12
            If Year(d) = intYear And Month(d) = intMonth Then
                Return i
            End If
            d = d.AddMonths(1)
        Next
        Return 0
    End Function

   

    Private Sub RefreshFromTemp()
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).OrderBy(Function(p) p.p85OtherKey2).ThenBy(Function(p) p.p85OtherKey1)

        Dim intLastJ02ID As Integer = 0, cRow As PlanRow = Nothing
        Dim lisPlan As New List(Of PlanRow)
        For Each c In lis
            If intLastJ02ID <> c.p85OtherKey1 Then
                cRow = New PlanRow(c.p85OtherKey1)
                cRow.Person = c.p85FreeText01

                lisPlan.Add(cRow)
            End If

            Dim cHours As New PlanHours
            With cHours
                .HoursBillable = c.p85FreeFloat01
                .HoursNonBillable = c.p85FreeFloat02
                .HoursTotal = .HoursBillable + .HoursNonBillable
                .Year = Year(c.p85FreeDate01)
                .Month = Month(c.p85FreeDate01)
            End With
            cRow.Hours.Add(cHours)


            intLastJ02ID = c.p85OtherKey1

        Next


        rp1.DataSource = lisPlan
        rp1.DataBind()

        RefreshTotals(lisPlan)

        Dim j02ids As IEnumerable(Of Integer) = lis.Select(Function(p) p.p85OtherKey1).Distinct
        For Each ri As RepeaterItem In rpJ02.Items
            Dim strJ02ID As String = CType(ri.FindControl("hidJ02ID"), HiddenField).Value
            If j02ids.Where(Function(p) p = CInt(strJ02ID)).Count > 0 Then
                ri.FindControl("cmdInsert").Visible = False
            Else
                ri.FindControl("cmdInsert").Visible = True
            End If
        Next
        
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim lisP47 As New List(Of BO.p47CapacityPlan)
            Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), True)
            For Each cTemp In lisTemp
                Dim c As New BO.p47CapacityPlan
                c.SetPID(cTemp.p85DataPID)
                If cTemp.p85IsDeleted And cTemp.p85DataPID <> 0 Then
                    c.SetAsDeleted()
                End If
                c.j02ID = cTemp.p85OtherKey1
                c.p41ID = Master.DataPID
                c.p47DateFrom = cTemp.p85FreeDate01
                c.p47DateUntil = cTemp.p85FreeDate02
                c.p47HoursBillable = cTemp.p85FreeFloat01
                c.p47HoursNonBillable = cTemp.p85FreeFloat02
                c.p47HoursTotal = c.p47HoursBillable + c.p47HoursNonBillable


                lisP47.Add(c)

            Next
            If Master.Factory.p47CapacityPlanBL.SaveProjectPlan(Master.DataPID, lisP47) Then
                Master.CloseAndRefreshParent("p47-save")
            End If
        End If
    End Sub



    Private Sub opgPageIndex_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgPageIndex.SelectedIndexChanged
        Me.CurrentPageIndex = Me.opgPageIndex.SelectedIndex
        Dim d As Date = Me.CurrentD1
        RenderTabHeader()
        RefreshFromTemp()
    End Sub

    'Private Sub cbxAddJ02ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAddJ02ID.SelectedIndexChanged
    '    Save2Temp()
    '    If Me.cbxAddJ02ID.SelectedIndex > 0 Then
    '        Dim j02ids As New List(Of Integer)
    '        j02ids.Add(CInt(Me.cbxAddJ02ID.SelectedValue))
    '        InsertPersons2Temp(j02ids)
    '        RefreshFromTemp()
    '        Me.cbxAddJ02ID.SelectedItem.Enabled = False
    '        Me.cbxAddJ02ID.SelectedIndex = 0
    '    End If

    'End Sub

    Private Sub InsertPersons2Temp(j02ids As List(Of Integer))
        Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        Dim lisJ02 As IEnumerable(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList_j02_join_j11(j02ids, New List(Of Integer))
        For Each c In lisJ02
            If lisTemp.Where(Function(p) p.p85OtherKey1 = c.PID).Count = 0 Then
                Dim cTemp As New BO.p85TempBox
                With cTemp
                    .p85GUID = ViewState("guid")
                    .p85OtherKey1 = c.PID
                    .p85OtherKey2 = Master.DataPID
                    .p85FreeDate01 = Me.CurrentD1
                    .p85FreeDate02 = Me.CurrentD1.AddMonths(1).AddDays(-1)
                    .p85FreeText01 = c.FullNameDesc
                End With
                Master.Factory.p85TempBoxBL.Save(cTemp)
            End If

        Next

    End Sub

    Private Sub capacity_plan_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        RefreshOutBoxes()

        If rp1.Items.Count = 0 Then
           
            panMatrix.Visible = False
        Else
            
            panMatrix.Visible = True
        End If
       

    End Sub

    
    Private Sub rpJ02_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpJ02.ItemDataBound
        Dim cRec As BO.j02Person = CType(e.Item.DataItem, BO.j02Person)
        CType(e.Item.FindControl("hidJ02ID"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("Person"), Label)
            .Text = cRec.FullNameDesc
        End With
        CType(e.Item.FindControl("clue_person"), HyperLink).Attributes("rel") = "clue_j02_capacity.aspx?pid=" & cRec.PID.ToString & "&p41id=" & Master.DataPID.ToString
        With CType(e.Item.FindControl("cmdInsert"), HyperLink)
            .NavigateUrl = "javascript:plan_item(" & cRec.PID.ToString & ")"
        End With
    End Sub

    Private Sub cmdHardRefreshOnBehind_Click(sender As Object, e As EventArgs) Handles cmdHardRefreshOnBehind.Click
        RefreshFromTemp()
    End Sub
End Class