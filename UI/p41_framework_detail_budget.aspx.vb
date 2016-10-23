Imports Telerik.Web.UI

Public Class p41_framework_detail_budget
    Inherits System.Web.UI.Page
 

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                If .DataPID = 0 Then .StopPage("masterpid is missing")
                If Request.Item("budgetprefix") <> "" Then
                    .Factory.j03UserBL.SetUserParam("p41_framework_detail_budget-prefix", Request.Item("budgetprefix"))
                End If
            End With
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p41_framework_detail_budget-prefix")
                .Add("p41_framework_detail_budget-chkP49GroupByP34")
                .Add("p41_framework_detail_budget-chkP49GroupByP32")
            End With
            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)

                If .GetUserParam("p41_framework_detail_budget-prefix", "p46") = "p46" Then
                    cmdBudgetP46.Checked = True : cmdBudgetP46.Font.Bold = True
                    panP49Setting.Visible = False
                Else
                    cmdBudgetP49.Checked = True : cmdBudgetP49.Font.Bold = True
                    panP49Setting.Visible = True
                    Me.chkP49GroupByP34.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail_budget-chkP49GroupByP34", "1"))
                    Me.chkP49GroupByP32.Checked = BO.BAS.BG(.GetUserParam("p41_framework_detail_budget-chkP49GroupByP32"))

                End If
            End With
            RefreshRecord()

            SetupGrid()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cP41)
        If Not cDisp.p45_Read Then
            Master.StopPage("V tomto projektu nemáte přístup k rozpočtu.")
        End If
        Me.cmdP45.Visible = cDisp.p45_Owner

        Dim lis As IEnumerable(Of BO.p45Budget) = Master.Factory.p45BudgetBL.GetList(Master.DataPID)
        If lis.Count > 0 Then
            Me.p45ID.DataSource = lis
            Me.p45ID.DataBind()
            cmdP45.InnerText = "Nastavení rozpočtu"
            cmdP47.Visible = cmdBudgetP46.Checked
            cmdNewP49.Visible = cmdBudgetP49.Checked : cmdConvert2P31.Visible = cmdBudgetP49.Checked
        Else
            Me.p45ID.Visible = False : cmdBudgetP46.Visible = False : cmdBudgetP49.Visible = False
            cmdP45.InnerText = "Založit rozpočet"
        End If
    End Sub


    Private Sub SetupGrid()
        With gridBudget
            .ClearColumns()
            .radGridOrig.ShowFooter = True
            .radGridOrig.AllowSorting = False
            .radGridOrig.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
            .PageSize = 20
            Dim group As New Telerik.Web.UI.GridColumnGroup
            .radGridOrig.MasterTableView.ColumnGroups.Add(group)
            .ClientDataKeyNames = "pid"
            If Me.cmdBudgetP46.Checked Then
                group.Name = "rozpocet_hodiny" : group.HeaderText = "Limity hodin"
                group = New Telerik.Web.UI.GridColumnGroup
                .radGridOrig.MasterTableView.ColumnGroups.Add(group)
                group.Name = "rozpocet_cena" : group.HeaderText = "Cena hodin v rozpočtu"
                group = New Telerik.Web.UI.GridColumnGroup
                .radGridOrig.MasterTableView.ColumnGroups.Add(group)
                group.Name = "timesheet_hodiny" : group.HeaderText = "Vykázané hodiny"
                group = New Telerik.Web.UI.GridColumnGroup
                .radGridOrig.MasterTableView.ColumnGroups.Add(group)
                group.Name = "timesheet_cena" : group.HeaderText = "Cena vykázaných hodin"

                .AddColumn("Person", "Osoba", , True)
                .AddColumn("p46HoursBillable", "Fa", BO.cfENUM.Numeric0, , , , , True, , "rozpocet_hodiny")
                .AddColumn("p46HoursNonBillable", "NeFa", BO.cfENUM.Numeric0, , , , , True, , "rozpocet_hodiny")
                .AddColumn("p46HoursTotal", "Celkem", BO.cfENUM.Numeric0, , , , , True, , "rozpocet_hodiny")

                .AddColumn("BillingAmount", "Fakturační", BO.cfENUM.Numeric, , , , , True, , "rozpocet_cena")
                .AddColumn("CostAmount", "Nákladová", BO.cfENUM.Numeric, , , , , True, , "rozpocet_cena")

                .AddColumn("TimesheetFa", "Fa", BO.cfENUM.Numeric2, , , , , True, , "timesheet_hodiny")
                .AddColumn("TimesheetNeFa", "NeFa", BO.cfENUM.Numeric2, , , , , True, , "timesheet_hodiny")
                .AddColumn("TimesheetAll", "Celkem", BO.cfENUM.Numeric2, , , , , True, , "timesheet_hodiny")
                .AddColumn("TimesheetAllVersusBudget", "+-", BO.cfENUM.Numeric, False, , , , True, , "timesheet_hodiny")

                .AddColumn("TimeshetAmountBilling", "Fakturační", BO.cfENUM.Numeric2, , , , , True, , "timesheet_cena")
                .AddColumn("TimesheetAmountCost", "Nákladová", BO.cfENUM.Numeric2, , , , , True, , "timesheet_cena")
            Else
                group.Name = "plan" : group.HeaderText = "Rozpočet"
                group = New Telerik.Web.UI.GridColumnGroup
                .radGridOrig.MasterTableView.ColumnGroups.Add(group)
                group.Name = "real" : group.HeaderText = "Vykázaná realita"
                .AddColumn("Period", "Měsíc", , , , , , , , "plan")
                '.AddColumn("p34Name", "Sešit")
                .AddColumn("p32Name", "Aktivita", , , , , , , , "plan")
                .AddColumn("SupplierName", "Dodavatel", , , , , , , , "plan")
                .AddColumn("p49Text", "Text", , , , , , , , "plan")
                .AddColumn("p49Amount", "Částka", BO.cfENUM.Numeric2, , , , , True, , "plan")
                .AddColumn("j27Code", "Měna", , , , , , , , "plan")

                .AddColumn("p31Date", "Datum", BO.cfENUM.DateOnly, , , , , , , "real")
                .AddColumn("p31Amount_WithoutVat_Orig", "Částka", BO.cfENUM.Numeric, , , , , True, , "real")
                .AddColumn("p31Code", "Kód dokladu", , , , , , , , "real")
                .AddColumn("p31Count", "Počet", BO.cfENUM.Numeric0, , , , , , , "real")

                
            End If


        End With


    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        gridBudget.radGridOrig.GroupingSettings.RetainGroupFootersVisibility = True
        With gridBudget.radGridOrig.MasterTableView
            ''.GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
            .GroupsDefaultExpanded = True
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strFieldHeader
            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)

            ''.ShowFooter = True
        End With

    End Sub

    Private Sub gridBudget_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gridBudget.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Or gridBudget.Visible = False Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        If Me.cmdBudgetP46.Checked Then
            Dim cRec As BO.p46BudgetPersonExtented = CType(e.Item.DataItem, BO.p46BudgetPersonExtented)
            If cRec.TimesheetAllVersusBudget > 0 Then
                'vykázáno přes rozpočet
                dataItem.ForeColor = Drawing.Color.Red
            End If
        End If

    End Sub

    Private Sub gridBudget_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridBudget.NeedDataSource
        If Me.p45ID.Items.Count = 0 Then
            gridBudget.Visible = False
            Return
        End If
        Dim intP45ID As Integer = CInt(Me.p45ID.SelectedValue)
        If Me.cmdBudgetP46.Checked Then
            Dim lis As IEnumerable(Of BO.p46BudgetPersonExtented) = Master.Factory.p45BudgetBL.GetList_p46_extended(intP45ID, Master.DataPID)
            gridBudget.DataSource = lis
        Else
            Dim mq As New BO.myQueryP49
            mq.p45ID = intP45ID
            Dim lis As IEnumerable(Of BO.p49FinancialPlanExtended) = Master.Factory.p49FinancialPlanBL.GetList_Extended(mq, Master.DataPID)
            gridBudget.DataSource = lis
            If lis.Count > 0 Then
                gridBudget.radGridOrig.MasterTableView.GroupByExpressions.Clear()
                If Me.chkP49GroupByP34.Checked Then SetupGrouping("p34Name", "Sešit")
                If Me.chkP49GroupByP32.Checked Then SetupGrouping("p32Name", "Aktivita")
                gridBudget.radGridOrig.ShowFooter = False
            End If
        End If

    End Sub

   
    Private Sub chkP49GroupByP34_CheckedChanged(sender As Object, e As EventArgs) Handles chkP49GroupByP34.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail_budget-chkP49GroupByP34", BO.BAS.GB(Me.chkP49GroupByP34.Checked))
        gridBudget.Rebind(False)
    End Sub

    Private Sub chkP49GroupByP32_CheckedChanged(sender As Object, e As EventArgs) Handles chkP49GroupByP32.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("p41_framework_detail_budget-chkP49GroupByP32", BO.BAS.GB(Me.chkP49GroupByP32.Checked))
        gridBudget.Rebind(False)
    End Sub
End Class