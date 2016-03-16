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
            Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
            If cDisp.p47_Create Then
                Master.AddToolbarButton("Uložit změny", "save", 0, "Images/save.png")
            Else
                cmdAddPerson.Visible = False
                Master.Notify("Rozpočet projektu můžete pouze číst, nikoliv upravovat.")
            End If

            SetupP45Combo()
            If Me.p45ID.Items.Count > 0 Then
                Me.CurrentP45ID = CInt(p45ID.SelectedValue)
            End If

            RefreshRecord()


        End If
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
            Master.Notify("V projektových rolích projektu nejsou přiřazeny osoby!", NotifyLevel.WarningMessage)
        End If
    End Sub

    Private Sub SetupTempData()
        Dim lis As IEnumerable(Of BO.p46BudgetPerson) = Master.Factory.p45BudgetBL.GetList_p46(Master.DataPID)
    End Sub

    Private Sub SetupGrid()
        grid1.Columns.Clear()
        grid1.MasterTableView.GroupByExpressions.Clear()
        grid1.MasterTableView.ColumnGroups.Clear()
        
        AddColumn("Person", "Osoba")
        AddColumn("Total", "<img src='Images/sum.png'/>")
        While d <= d2

            AddNumbericTextboxColumn("Col" & x.ToString & "Fa", "Fa", "gridnumber1", True, , "M" & x.ToString)
            AddNumbericTextboxColumn("Col" & x.ToString & "NeFa", "NeFa", "gridnumber1", True, , "M" & x.ToString)


            d = d.AddMonths(1)
            x += 1
        End While


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
        End If
        cmdNewVersion.Visible = Me.p45ID.Visible
        cmdDeleteVersion.Visible = Me.p45ID.Visible
        panRecordBody.Visible = Me.p45ID.Visible
        grid1.Visible = panRecordBody.Visible

    End Sub
End Class