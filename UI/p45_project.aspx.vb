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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))    'p41ID
                .HeaderText = "Rozpočet projektu | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
            End With
            Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
            If Not cRec.p41PlanFrom Is Nothing Then Me.p45PlanFrom.SelectedDate = cRec.p41PlanFrom
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
    Private Sub p45_project_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.CurrentP45ID <> 0 Then
            Me.p45ID.Visible = True
            If Me.p45ID.Items.Count = 1 Then
                cmdDeleteVersion.Text = "Odstranit rozpočet"
            Else
                cmdDeleteVersion.Text = "Odstranit tuto verzi rozpočtu"
            End If
            cmdNewVersion.Text = "Založit novou verzi rozpočtu"
            Master.HideShowToolbarButton("save", True)
            lblP45.Text = "Pracovat ve verzi rozpočtu:"
            cmdSaveFirstVersion.Visible = False
        Else
            Me.p45ID.Visible = False
            Master.HideShowToolbarButton("save", False)
            cmdSaveFirstVersion.Visible = True
            lblP45.Text = ""
            cmdInsertPersons.Visible = False

        End If
        cmdNewVersion.Visible = Me.p45ID.Visible
        cmdDeleteVersion.Visible = Me.p45ID.Visible

        grid1.Visible = panRecordBody.Visible

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


        SetupGrid()





    End Sub

    Private Sub SetupPersonsOffer()
        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, Master.DataPID)
        Dim j02ids As List(Of Integer) = lisX69.Select(Function(p) p.j02ID).Distinct.ToList
        Dim j11ids As List(Of Integer) = lisX69.Select(Function(p) p.j11ID).Distinct.ToList
        Dim persons As IEnumerable(Of BO.j02Person) = Master.Factory.j02PersonBL.GetList_j02_join_j11(j02ids, j11ids)
        rpJ02.DataSource = persons
        rpJ02.DataBind()
        If rpJ02.Items.Count = 0 Then
            Master.Notify("V projektových rolích projektu není nikdo přiřazen!", NotifyLevel.WarningMessage)
        End If
    End Sub

    Private Sub SetupTempData()
        Dim lis As IEnumerable(Of BO.p46BudgetPerson) = Master.Factory.p45BudgetBL.GetList_p46(Master.DataPID)
        For Each c In lis
            Dim cTemp As New BO.p85TempBox
            With cTemp
                .p85GUID = ViewState("guid")
                .p85DataPID = c.PID
                .p85OtherKey1 = c.j02ID
                .p85OtherKey2 = c.p46ExceedFlag
                .p85FreeText01 = c.Person
                .p85FreeFloat01 = c.p46HoursBillable
                .p85FreeFloat02 = c.p46HoursNonBillable
                .p85FreeFloat03 = c.p46HoursTotal
                .p85FreeText02 = c.p46Description
            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
    End Sub

    Private Sub SetupGrid()
        grid1.Columns.Clear()
        grid1.MasterTableView.GroupByExpressions.Clear()
        grid1.MasterTableView.ColumnGroups.Clear()
        
        AddColumn("p85FreeText01", "Osoba")
       
        AddNumbericTextboxColumn("p85FreeFloat01", "Hodiny Fa", "gridnumber1", True)
        AddNumbericTextboxColumn("p85FreeFloat02", "Hodiny NeFa", "gridnumber1", True)
        AddColumn("p85FreeFloat03", "Celkem")


    End Sub


    Public Sub AddNumbericTextboxColumn(strField As String, strHeader As String, strColumnEditorID As String, bolAllowSorting As Boolean, Optional ByVal strUniqueName As String = "", Optional strGroupHeaderName As String = "")
        Dim col As New GridNumericColumn
        grid1.MasterTableView.Columns.Add(col)

        col.HeaderText = strHeader
        col.DataField = strField
        If strField.IndexOf("NeFa") > 0 Then
            col.ItemStyle.ForeColor = Drawing.Color.Red
        Else
            col.ItemStyle.ForeColor = Drawing.Color.Green
        End If
        col.ColumnEditorID = strColumnEditorID
        col.ColumnGroupName = strGroupHeaderName
        If strUniqueName <> "" Then
            col.UniqueName = strUniqueName
            col.SortExpression = strUniqueName
        End If
        col.AllowSorting = bolAllowSorting


    End Sub
    Public Sub AddColumn(ByVal strField As String, ByVal strHeader As String, Optional strWidth As String = "")
        Dim col As New GridBoundColumn
        grid1.MasterTableView.Columns.Add(col)

        col.HeaderText = strHeader
        col.DataField = strField
        col.ReadOnly = True
        col.AllowSorting = True
        If strWidth <> "" Then col.ItemStyle.Width = Unit.Parse(strWidth)


    End Sub

    
    Private Sub cmdNewVersion_Click(sender As Object, e As EventArgs) Handles cmdNewVersion.Click

    End Sub

    Private Sub rpJ02_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpJ02.ItemDataBound
        Dim cRec As BO.j02Person = CType(e.Item.DataItem, BO.j02Person)
        CType(e.Item.FindControl("hidJ02ID"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("Person"), CheckBox)
            .Text = cRec.FullNameDesc
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        CType(e.Item.FindControl("clue_person"), HyperLink).Attributes("rel") = "clue_j02_capacity.aspx?pid=" & cRec.PID.ToString & "&p41id=" & Master.DataPID.ToString
    End Sub

   
    Private Sub cmdSaveFirstVersion_Click(sender As Object, e As EventArgs) Handles cmdSaveFirstVersion.Click
        Dim c As New BO.p45Budget
        If Not Me.p45PlanFrom.IsEmpty Then c.p45PlanFrom = Me.p45PlanFrom.SelectedDate
        If Not Me.p45PlanUntil.IsEmpty Then c.p45PlanUntil = Me.p45PlanUntil.SelectedDate
        c.p45Name = Me.p45Name.Text

        Dim lisP46 As New List(Of BO.p46BudgetPerson)
        For Each ri As RepeaterItem In rpJ02.Items
            With CType(ri.FindControl("Person"), CheckBox)
                If .Checked Then
                    Dim item As New BO.p46BudgetPerson
                    item.j02ID = CInt(CType(ri.FindControl("hidJ02ID"), HiddenField).Value)
                    item.p46ExceedFlag = BO.p46ExceedFlagENUM.NoLimit
                    lisP46.Add(item)
                End If
            End With
        Next
        Master.Factory.p45BudgetBL.Save(c, lisP46)
    End Sub
End Class