﻿Imports Telerik.Web.UI

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
            Me.CurrentP45ID = BO.BAS.IsNullInt(p45ID.SelectedValue)

            
            RefreshRecord()

            SetupPersonsOffer()
        End If
    End Sub
    Private Sub SetupP49Grid()
        With gridP49
            .ClearColumns()
            .radGridOrig.ShowFooter = False
            .AddColumn("p85FreeText06", "Měsíc")
            '.AddColumn("p34Name", "Sešit")
            .AddColumn("p85FreeText03", "Aktivita")
            '.AddColumn("p85FreeText01", "Osoba")
            .AddColumn("p85FreeText05", "Dodavatel")
            .AddColumn("p85Message", "Text")
            .AddColumn("p85FreeFloat01", "Částka", BO.cfENUM.Numeric2)
            .AddColumn("p85FreeText04", "Měna")
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
        Dim lisP49 As IEnumerable(Of BO.p49FinancialPlan) = Master.Factory.p49FinancialPlanBL.GetList(mq, False)
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

   
   

    Private Sub ReloadPage()
        Server.Transfer("p45_project.aspx?pid=" & Master.DataPID.ToString)
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

        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).Where(Function(p) p.p85Prefix = "p46")
        grid1.DataSource = lis
    End Sub

    Private Sub cmdInsertPersons_Click(sender As Object, e As EventArgs) Handles cmdInsertPersons.Click
        Dim b As Boolean = False
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

    End Sub

    Private Sub ShowNeedSaveMessage()
        Master.master_show_message("Provedené změny je třeba uložit tlačítkem [Uložit změny].")
    End Sub
    
    Private Sub gridP49_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridP49.NeedDataSource
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), False).Where(Function(p) p.p85Prefix = "p49")
        gridP49.DataSource = lis
        Me.total_expense.Text = BO.BAS.FN(lis.Where(Function(p) p.p85OtherKey6 = 1).Sum(Function(p) p.p85FreeFloat01))
        Me.total_income.Text = BO.BAS.FN(lis.Where(Function(p) p.p85OtherKey6 = 2).Sum(Function(p) p.p85FreeFloat01))

    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        gridP49.Rebind(True)
        ShowNeedSaveMessage()
    End Sub
End Class