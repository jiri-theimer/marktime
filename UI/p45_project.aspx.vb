Imports Telerik.Web.UI

Public Class p45_project
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentP45ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.p45ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.p45ID, value.ToString)
        End Set
    End Property

    Private Sub p45_project_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))    'p41ID
                .HeaderText = "Rozpočet projektu | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
            End With
            Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
            If Not cRec.p41PlanFrom Is Nothing Then Me.p45PlanFrom.SelectedDate = cRec.p41PlanFrom Else Me.p45PlanFrom.SelectedDate = DateSerial(Year(Now), Month(Now), 1).AddMonths(1)
            If Not cRec.p41PlanUntil Is Nothing Then Me.p45PlanUntil.SelectedDate = cRec.p41PlanUntil

            Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
            If cDisp.p47_Create Then
                Master.AddToolbarButton("Uložit změny", "save", 0, "Images/save.png")
            Else
                cmdAddPerson.Visible = False
                Master.Notify("Rozpočet projektu můžete pouze číst, nikoliv upravovat.")
            End If

            SetupP45Combo()
            If Request.Item("p45id") <> "" Then
                Me.CurrentP45ID = BO.BAS.IsNullInt(Request.Item("p45id"))
            Else
                Me.CurrentP45ID = BO.BAS.IsNullInt(p45ID.SelectedValue)
            End If


            
            RefreshRecord()

            SetupPersonsOffer()
        End If
    End Sub
    Private Sub SetupP49Grid()
        With gridP49
            .ClearColumns()
            .radGridOrig.MasterTableView.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            Dim group As New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            group.Name = "plan" : group.HeaderText = "Rozpočet"
            group = New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            group.Name = "real" : group.HeaderText = "Vykázaná realita"

            .radGridOrig.ShowFooter = False
            .AddColumn("p85FreeText06", "Měsíc", , , , , , , , "plan")
            '.AddColumn("p34Name", "Sešit")
            .AddColumn("p85FreeText03", "Aktivita", , , , , , , , "plan")
            '.AddColumn("p85FreeText01", "Osoba")
            .AddColumn("p85FreeText05", "Dodavatel", , , , , , , , "plan")
            .AddColumn("p85Message", "Text", , , , , , , , "plan")
            .AddColumn("p85FreeFloat01", "Částka", BO.cfENUM.Numeric2, , , , , , , "plan")
            .AddColumn("p85FreeText04", "Měna", , , , , , , , "plan")

            .AddColumn("p85FreeDate03", "Datum", BO.cfENUM.DateOnly, , , , , , , "real")
            .AddColumn("p85FreeFloat02", "Částka", BO.cfENUM.Numeric, , , , , , , "real")
            .AddColumn("p85FreeText07", "Kód dokladu", , , , , , , , "real")
            .AddColumn("p85FreeNumber01", "Počet", BO.cfENUM.Numeric0, , , , , , , "real")
        End With
        With gridP49.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            .ShowGroupFooter = False
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = "p85FreeText02"
            fld.HeaderText = "Sešit"

            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)
        End With
    End Sub
    Private Sub p45_project_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

        If Me.CurrentP45ID <> 0 Then
            Me.p45ID.Visible = True
            If Me.p45ID.Items.Count = 1 Then
                cmdDeleteVersion.Text = "Odstranit rozpočet"
            Else
                cmdDeleteVersion.Text = "Odstranit tuto verzi rozpočtu"
            End If
            cmdNewVersion.Text = "Založit novou verzi rozpočtu"
            Master.RenameToolbarButton("save", "Uložit změny")
            lblP45.Text = "Pracovat ve verzi rozpočtu:"
            cmdAddPerson.InnerText = "Přidat do rozpočtu další osoby"
        Else
            Me.p45ID.Visible = False
            Master.RenameToolbarButton("save", "Založit v projektu rozpočet")
            lblP45.Text = ""
            cmdAddPerson.InnerText = "Přidat do rozpočtu osoby"

        End If
        cmdAddPerson.InnerHtml += "<img src='Images/arrow_down.gif' />"
        cmdNewVersion.Visible = Me.p45ID.Visible
        cmdDeleteVersion.Visible = Me.p45ID.Visible
        If Me.p45ID.Items.Count > 1 Then
            cmdMakeActualVersion.Visible = True
        Else
            cmdMakeActualVersion.Visible = False
        End If
        grid1.Rebind()

    End Sub

    Private Sub SetupP45Combo()
        Dim lis As IEnumerable(Of BO.p45Budget) = Master.Factory.p45BudgetBL.GetList(Master.DataPID)
        Me.p45ID.DataSource = lis
        Me.p45ID.DataBind()
        
    End Sub


    Private Sub RefreshRecord()
        If Me.CurrentP45ID = 0 Then
            Return
        End If
        Dim cRec As BO.p45Budget = Master.Factory.p45BudgetBL.Load(Me.CurrentP45ID)
        With cRec
            Me.p45PlanFrom.SelectedDate = .p45PlanFrom
            Me.p45PlanUntil.SelectedDate = .p45PlanUntil
            Me.p45Name.Text = .p45Name
        End With

        SetupTempData()
        
        SetupP49Grid()


    End Sub

    Private Sub SetupPersonsOffer()
        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, Master.DataPID)
        Dim j02ids As List(Of Integer) = lisX69.Select(Function(p) p.j02ID).Distinct.ToList
        Dim j11ids As List(Of Integer) = lisX69.Select(Function(p) p.j11ID).Distinct.ToList
        Dim persons As List(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList_j02_join_j11(j02ids, j11ids).Where(Function(p) p.IsClosed = False).ToList
        If persons.Count = 0 Then
            Master.Notify("Projekt nemá obsazené projektové role osobami!", NotifyLevel.WarningMessage)
        End If
        Dim j02ids_used As List(Of Integer) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), False).Where(Function(p) p.p85Prefix = "p46").Select(Function(p) p.p85OtherKey1).ToList
        For Each intJ02ID As Integer In j02ids_used
            If persons.Where(Function(p) p.PID = intJ02ID).Count > 0 Then
                persons.Remove(persons.First(Function(p) p.PID = intJ02ID))
            End If
        Next
        rpJ02.DataSource = persons
        rpJ02.DataBind()
        If rpJ02.Items.Count = 0 Then
            cmdInsertPersons.Visible = False
            lblInsertPersonsHeader.Text = "Pro tento rozpočet nejsou další osoby k dispozici."
        End If
    End Sub

    Private Sub SetupTempData()
        Dim lisP46 As IEnumerable(Of BO.p46BudgetPerson) = Master.Factory.p45BudgetBL.GetList_p46(Me.CurrentP45ID)
        For Each c In lisP46
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = ViewState("guid")
                .p85Prefix = "p46"
                .p85DataPID = c.PID
                .p85OtherKey1 = c.j02ID
                .p85OtherKey2 = c.p46ExceedFlag
                .p85FreeText01 = c.Person
                .p85FreeFloat01 = c.p46HoursBillable
                .p85FreeFloat02 = c.p46HoursNonBillable
                .p85FreeFloat03 = c.p46HoursTotal
                .p85FreeText02 = c.p46Description
                .p85FreeNumber01 = c.p46BillingRate
                .p85FreeNumber02 = c.p46CostRate
                .p85FreeNumber03 = c.BillingAmount
                .p85FreeNumber04 = c.CostAmount
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
        Dim mq As New BO.myQueryP49
        mq.p45ID = Me.CurrentP45ID
        Dim lisP49 As IEnumerable(Of BO.p49FinancialPlanExtended) = Master.Factory.p49FinancialPlanBL.GetList_Extended(mq, Master.DataPID)
        For Each c In lisP49
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = ViewState("guid")
                .p85Prefix = "p49"
                .p85DataPID = c.PID
                .p85OtherKey1 = c.j02ID
                .p85OtherKey2 = c.p34ID
                .p85OtherKey3 = c.p32ID
                .p85OtherKey4 = c.j27ID
                .p85OtherKey5 = c.p28ID_Supplier
                .p85OtherKey6 = c.p34IncomeStatementFlag
                .p85FreeFloat01 = c.p49Amount
                .p85Message = c.p49Text
                .p85FreeDate01 = c.p49DateFrom
                .p85FreeDate02 = c.p49DateUntil
                .p85FreeText01 = c.Person
                .p85FreeText02 = c.p34Name
                .p85FreeText03 = c.p32Name
                .p85FreeText04 = c.j27Code
                .p85FreeText05 = c.SupplierName
                .p85FreeText06 = c.Period
                If c.p31ID > 0 Then
                    .p85FreeDate03 = c.p31Date
                    .p85FreeText07 = c.p31Code
                    .p85FreeFloat02 = c.p31Amount_WithoutVat_Orig
                    .p85FreeNumber01 = c.p31Count
                End If

            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
    End Sub

    


  

    
    Private Sub cmdNewVersion_Click(sender As Object, e As EventArgs) Handles cmdNewVersion.Click
        panCreateClone.Visible = True
        grid1.Visible = False
        tabs1.Visible = False
        Master.RadToolbar.Visible = False
        panHeader.Visible = False
        Me.p45ID_Template.DataSource = Master.Factory.p45BudgetBL.GetList(Master.DataPID)
        Me.p45ID_Template.DataBind()
    End Sub

    Private Sub rpJ02_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpJ02.ItemDataBound
        Dim cRec As BO.j02Person = CType(e.Item.DataItem, BO.j02Person)
        CType(e.Item.FindControl("hidJ02ID"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("Person"), CheckBox)
            .Text = cRec.FullNameDesc
            If cRec.IsClosed Then .Font.Strikeout = True
            If Me.CurrentP45ID = 0 Then .Checked = True
        End With
        CType(e.Item.FindControl("clue_person"), HyperLink).Attributes("rel") = "clue_j02_capacity.aspx?pid=" & cRec.PID.ToString & "&p41id=" & Master.DataPID.ToString
    End Sub

   
   

    Private Sub ReloadPage(Optional strGoToP45ID As String = "")
        If strGoToP45ID = "" Then strGoToP45ID = Me.CurrentP45ID.ToString
        Server.Transfer("p45_project.aspx?pid=" & Master.DataPID.ToString & "&p45id=" & strGoToP45ID)
    End Sub

   
    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        Select Case strButtonValue
            Case "save"
                Dim c As New BO.p45Budget
                If Me.CurrentP45ID <> 0 Then c = Master.Factory.p45BudgetBL.Load(Me.CurrentP45ID)

                If Not Me.p45PlanFrom.IsEmpty Then c.p45PlanFrom = Me.p45PlanFrom.SelectedDate
                If Not Me.p45PlanUntil.IsEmpty Then c.p45PlanUntil = Me.p45PlanUntil.SelectedDate
                c.p45Name = Me.p45Name.Text
                c.p41ID = Master.DataPID

                Dim lisP46 As New List(Of BO.p46BudgetPerson), lisP49 As New List(Of BO.p49FinancialPlan)
                Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), True)
                For Each cTemp In lisTemp.Where(Function(p) p.p85Prefix = "p46")
                    Dim item As New BO.p46BudgetPerson
                    With cTemp
                        item.SetPID(.p85DataPID)
                        item.j02ID = .p85OtherKey1
                        item.p46HoursBillable = .p85FreeFloat01
                        item.p46HoursNonBillable = .p85FreeFloat02
                        item.p46HoursTotal = item.p46HoursBillable + item.p46HoursNonBillable
                        item.p46ExceedFlag = .p85OtherKey2
                        item.p46Description = .p85FreeText02
                        item.IsSetAsDeleted = .p85IsDeleted
                    End With
                    lisP46.Add(item)
                Next
                For Each cTemp In lisTemp.Where(Function(p) p.p85Prefix = "p49")
                    Dim item As New BO.p49FinancialPlan
                    With cTemp
                        item.SetPID(.p85DataPID)
                        item.j02ID = .p85OtherKey1
                        item.p34ID = .p85OtherKey2
                        item.p32ID = .p85OtherKey3
                        item.j27ID = .p85OtherKey4
                        item.p49DateFrom = .p85FreeDate01
                        item.p49DateUntil = .p85FreeDate02
                        item.p28ID_Supplier = .p85OtherKey5
                        item.p49Amount = .p85FreeFloat01
                        item.p49Text = .p85Message
                        If .p85IsDeleted Then item.SetAsDeleted()
                    End With
                    lisP49.Add(item)
                Next

                With Master.Factory.p45BudgetBL
                    If .Save(c, lisP46, lisP49) Then
                        Master.CloseAndRefreshParent("p45-save")
                    Else
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                    End If
                End With
        End Select
    End Sub

    Private Sub grid1_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles grid1.ItemCommand
        If e.CommandName = "delete" Then
            Dim intP85ID As Integer = CInt(e.Item.Attributes("p85id"))
            Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(intP85ID)
            If Master.Factory.p85TempBoxBL.Delete(cRec) Then
                SetupPersonsOffer()
                grid1.Rebind()
                ShowNeedSaveMessage()
            End If
        End If
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)
            With CType(CType(e.Item, GridDataItem)("p85OtherKey2").FindControl("combo1"), DropDownList)
                If cRec.p85OtherKey2 > 0 Then
                    .SelectedValue = cRec.p85OtherKey2.ToString
                End If
                .Attributes.Item("onchange") = "save_cellvalue(" & cRec.PID.ToString & ",'p85OtherKey2',this.value)"
            End With
          
            e.Item.Attributes.Item("p85id") = cRec.PID.ToString
        End If
    End Sub

    

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource

        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        grid1.DataSource = lis.Where(Function(p) p.p85Prefix = "p46")

        RecalcStatement(lis)
    End Sub

    Private Sub cmdInsertPersons_Click(sender As Object, e As EventArgs) Handles cmdInsertPersons.Click
        Dim b As Boolean = False, mq As New BO.myQueryP32
        mq.p33ID = BO.p33IdENUM.Cas
        mq.Billable = BO.BooleanQueryMode.TrueQuery
        Dim lisP32 As IEnumerable(Of BO.p32Activity) = Master.Factory.p32ActivityBL.GetList(mq)
        Dim datRate As Date = Now, intP32ID As Integer = lisP32(0).PID
        If Not Me.p45PlanFrom.IsEmpty Then datRate = Me.p45PlanFrom.SelectedDate
        For Each ri As RepeaterItem In rpJ02.Items
            With CType(ri.FindControl("Person"), CheckBox)
                If .Checked Then
                    b = True
                    Dim intJ02ID As Integer = CInt(CType(ri.FindControl("hidJ02ID"), HiddenField).Value)
                    Dim cJ02 As BO.j02Person = Master.Factory.j02PersonBL.Load(intJ02ID)
                    Dim cTemp As New BO.p85TempBox()
                    With cTemp
                        .p85GUID = ViewState("guid")
                        .p85Prefix = "p46"
                        .p85OtherKey1 = intJ02ID
                        .p85FreeText01 = cJ02.FullNameDesc
                        .p85FreeNumber01 = Master.Factory.p31WorksheetBL.LoadRate(False, datRate, intJ02ID, Master.DataPID, intP32ID, 0)    'fakturační sazba
                        .p85FreeNumber02 = Master.Factory.p31WorksheetBL.LoadRate(True, datRate, intJ02ID, Master.DataPID, intP32ID, 0)    'nákladová sazba
                    End With
                    Master.Factory.p85TempBoxBL.Save(cTemp)
                End If
            End With
        Next
        If b Then
            grid1.Rebind()
        Else
            Master.Notify("Musíte zaškrtnout minimálně jednu osobu.", NotifyLevel.WarningMessage)
        End If

    End Sub

    Private Sub cmdCreateCloneCancel_Click(sender As Object, e As EventArgs) Handles cmdCreateCloneCancel.Click
        ReloadPage()
    End Sub

    Private Sub cmdSaveClone_Click(sender As Object, e As EventArgs) Handles cmdSaveClone.Click
        Dim cRec As BO.p45Budget = Master.Factory.p45BudgetBL.Load(CInt(Me.p45ID_Template.SelectedValue))
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
        Dim intNewP45ID As Integer = 0
        With Master.Factory.p45BudgetBL
            If Not .Save(cRec, lisP46, lisP49) Then
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                Return
            Else
                intNewP45ID = .LastSavedPID
                If Me.chkNewIsLast.Checked Then
                    .MakeActualVersion(intNewP45ID)
                End If
                If Me.chkCloneP47.Checked Then
                    lisP46 = .GetList_p46(intNewP45ID).ToList
                    For Each c In lisP47
                        c.p46ID = lisP46.First(Function(p) p.j02ID = c.j02ID).PID
                    Next
                    Master.Factory.p47CapacityPlanBL.SaveProjectPlan(intNewP45ID, lisP47)
                End If
                ReloadPage(intNewP45ID.ToString)
            End If
        End With
        
    End Sub

    Private Sub ShowNeedSaveMessage()
        Master.master_show_message("Provedené změny je třeba uložit tlačítkem [Uložit změny].")
    End Sub
    
    Private Sub gridP49_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridP49.NeedDataSource
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), False)
        gridP49.DataSource = lis.Where(Function(p) p.p85Prefix = "p49")
        RecalcStatement(lis)

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        gridP49.Rebind(True)
        ShowNeedSaveMessage()

        
    End Sub

    Private Sub cmdRefreshStatement_Click(sender As Object, e As EventArgs) Handles cmdRefreshStatement.Click
        RecalcStatement(Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), False))
        
    End Sub

    Private Sub RecalcStatement(lis As IEnumerable(Of BO.p85TempBox))
        Me.result_profit.Text = "" : Me.result_lost.Text = ""
        Dim dblExpenses As Double = lis.Where(Function(p) p.p85Prefix = "p49" And p.p85OtherKey6 = 1).Sum(Function(p) p.p85FreeFloat01)
        Dim dblIncome As Double = lis.Where(Function(p) p.p85Prefix = "p49" And p.p85OtherKey6 = 2).Sum(Function(p) p.p85FreeFloat01)

        Me.total_expense.Text = BO.BAS.FN(dblExpenses)
        Me.total_income.Text = BO.BAS.FN(dblIncome)

        lis = lis.Where(Function(p) p.p85Prefix = "p46")
        Dim dblCostFee As Double = lis.Sum(Function(p) (p.p85FreeFloat01 + p.p85FreeFloat02) * p.p85FreeNumber02)
        Dim dblBillingFee As Double = lis.Sum(Function(p) p.p85FreeFloat01 * p.p85FreeNumber01)
        Me.total_costfee.Text = BO.BAS.FN(dblCostFee)
        Me.total_billingfee.Text = BO.BAS.FN(dblBillingFee)

        Me.total_cost.Text = BO.BAS.FN(dblExpenses + dblCostFee)
        Me.total_billing.Text = BO.BAS.FN(dblIncome + dblBillingFee)

        Dim dblResult As Double = (dblIncome + dblBillingFee) - (dblExpenses + dblCostFee)
        Select Case dblResult
            Case Is > 0
                Me.result_profit.Text = "+" + BO.BAS.FN(dblResult)
                imgEmotion.ImageUrl = "Images/emotion_happy.png"
            Case Is < 0
                Me.result_lost.Text = BO.BAS.FN(dblResult)
                imgEmotion.ImageUrl = "Images/emotion_unhappy.png"
            Case 0
                Me.result_profit.Text = "?"
                imgEmotion.ImageUrl = "Images/emotion_amazing.png"
        End Select
        

    End Sub

    Private Sub cmdDeleteVersion_Click(sender As Object, e As EventArgs) Handles cmdDeleteVersion.Click
        If Master.Factory.p45BudgetBL.Delete(Me.CurrentP45ID) Then
            Master.CloseAndRefreshParent("p45-delete")
        Else
            Master.Notify(Master.Factory.p45BudgetBL.ErrorMessage, NotifyLevel.ErrorMessage)
        End If
    End Sub

    Private Sub p45ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles p45ID.SelectedIndexChanged
        ReloadPage()
    End Sub

    Private Sub cmdMakeActualVersion_Click(sender As Object, e As EventArgs) Handles cmdMakeActualVersion.Click
        With Master.Factory.p45BudgetBL
            If Not .MakeActualVersion(Me.CurrentP45ID) Then
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            Else
                Dim x As Integer = Me.CurrentP45ID
                SetupP45Combo()
                Me.CurrentP45ID = x
            End If
        End With
    End Sub
End Class