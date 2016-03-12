﻿Imports Telerik.Web.UI

Public Class p47_project
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Private Property _bolNeedSetupGrid As Boolean = False

    Public Class PlanMatrix
        Public Property Person As String
        Public Property Total As String
        Public Property PID As Integer
        Public Property RowIndex As Integer
        Public Property Col1Fa As Double?
        Public Property Col1NeFa As Double?
        Public Property Col1P85ID As Integer?
        Public Property Col2Fa As Double?
        Public Property Col2NeFa As Double?
        Public Property Col2P85ID As Integer?
        Public Property Col3Fa As Double?
        Public Property Col3NeFa As Double?
        Public Property Col3P85ID As Integer?
        Public Property Col4Fa As Double?
        Public Property Col4NeFa As Double?
        Public Property Col4P85ID As Integer?
        Public Property Col5Fa As Double?
        Public Property Col5NeFa As Double?
        Public Property Col5P85ID As Integer?
        Public Property Col6Fa As Double?
        Public Property Col6NeFa As Double?
        Public Property Col6P85ID As Integer?
        Public Property Col7Fa As Double?
        Public Property Col7NeFa As Double?
        Public Property Col7P85ID As Integer?
        Public Property Col8Fa As Double?
        Public Property Col8NeFa As Double?
        Public Property Col8P85ID As Integer?
        Public Property Col9Fa As Double?
        Public Property Col9NeFa As Double?
        Public Property ColP9P85ID As Integer?
        Public Property Col10Fa As Double?
        Public Property Col10NeFa As Double?
        Public Property Col10P85ID As Integer?
        Public Property Col11Fa As Double?
        Public Property Col11NeFa As Double?
        Public Property Col11P85ID As Integer?
        Public Property Col12Fa As Double?
        Public Property Col12NeFa As Double?
        Public Property Col12P85ID As Integer?
    End Class

    Public Property LimitD1 As Date
        Get
            Return BO.BAS.ConvertString2Date(Me.m1.SelectedValue)
        End Get
        Set(value As Date)
            Me.hidLimD1.Value = Format(value, "dd.MM.yyyy")
        End Set
    End Property
    Public Property LimitD2 As Date
        Get
            Return BO.BAS.ConvertString2Date(Me.m2.SelectedValue)
        End Get
        Set(value As Date)
            Me.hidLimD2.Value = Format(value, "dd.MM.yyyy")
        End Set
    End Property

    Private Sub p47_project_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p47_project"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Kapacitní plán | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
            End With
           

            RefreshRecord()


        End If
    End Sub

    Private Sub AddGroupHeader(strName As String, strHeaderText As String)
        Dim group As New GridColumnGroup
        grid1.MasterTableView.ColumnGroups.Add(group)
        With group
            With .HeaderStyle
                .ForeColor = Drawing.Color.Black
                .BackColor = Drawing.Color.Silver
                .HorizontalAlign = HorizontalAlign.Center
                .Font.Bold = True
            End With
            
            .Name = strName
            .HeaderText = strHeaderText
            
        End With

    End Sub


    Private Sub SetupGrid()
        If Me.m1.SelectedIndex > Me.m2.SelectedIndex Then
            Me.m2.SelectedIndex = Me.m1.SelectedIndex
        End If
        If Math.Abs(Me.m1.SelectedIndex - Me.m2.SelectedIndex) >= 12 Then
            Me.m2.SelectedIndex = Me.m1.SelectedIndex
        End If
        grid1.Columns.Clear()
        grid1.MasterTableView.GroupByExpressions.Clear()
        grid1.MasterTableView.ColumnGroups.Clear()
        With grid1.PagerStyle
            .PageSizeLabelText = ""
            .LastPageToolTip = "Poslední strana"
            .FirstPageToolTip = "První strana"
            .PrevPageToolTip = "Předchozí strana"
            .NextPageToolTip = "Další strana"
            .PagerTextFormat = "{4} Strana {0} z {1}, záznam {2} až {3} z {5}"
        End With

        With grid1.MasterTableView
            .NoMasterRecordsText = "Žádné záznamy"
        End With
        Dim d1 As Date = Me.LimitD1, d2 As Date = Me.LimitD2
        Dim d As Date = d1, x As Integer = 1
        AddColumn("Person", "Osoba")
        AddColumn("Total", "<img src='Images/sum.png'/>")
        While d <= d2

            AddGroupHeader("M" & x.ToString, Format(d, "MM.yyyy"))
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

    Private Sub grid1_DataBound(sender As Object, e As EventArgs) Handles grid1.DataBound
        Dim footerItem As GridFooterItem = grid1.MasterTableView.GetItems(GridItemType.Footer)(0)

        
        footerItem.Item("Person").Text = "<img src='Images/sum.png'/>"
       
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim cRec As PlanMatrix = CType(e.Item.DataItem, PlanMatrix)
            e.Item.Attributes.Item("j02id") = cRec.PID.ToString

        End If
        
    End Sub

   
    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource

        grid1.DataSource = GetListMatrix()

    End Sub


    Private Sub RefreshRecord()
        Dim mq As New BO.myQueryP47, d1 As Date = DateSerial(Year(Now), 1, 1), d2 As Date = DateSerial(Year(Now), 12, 31)

        mq.p41ID = Master.DataPID
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        If cRec.p41PlanFrom Is Nothing Or cRec.p41PlanUntil Is Nothing Then
            Master.StopPage("Pro kapacitní plánování je nutné mít v projektu nastavené datumy [Plánované zahájení] a [Plánované dokončení].")
        End If
        lblHeader.Text = BO.BAS.FD(cRec.p41PlanFrom) & " - " & BO.BAS.FD(cRec.p41PlanUntil) & " (" & DateDiff(DateInterval.Day, cRec.p41PlanFrom.Value, cRec.p41PlanUntil.Value).ToString & "d.)"
        If Not Page.IsPostBack Then
            Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
            If cDisp.p47_Create Then
                Master.AddToolbarButton("Uložit změny", "save", 0, "Images/save.png")

            Else
                cmdAddPerson.Visible = False
                Master.Notify("Kapacitní plán projektu můžete pouze číst, nikoliv upravovat.")
            End If
        End If

        With cRec
            d1 = DateSerial(Year(.p41PlanFrom), Month(.p41PlanFrom), 1)
            d2 = DateSerial(Year(.p41PlanUntil), Month(.p41PlanUntil), 1).AddMonths(1).AddDays(-1)
        End With
        Dim d As Date = d1
        Me.m1.Items.Clear() : Me.m2.Items.Clear()
        While d <= d2
            Me.m1.Items.Add(New ListItem(Format(d, "MM.yyyy"), Format(d, "dd.MM.yyyy")))
            Me.m2.Items.Add(New ListItem(Format(d, "MM.yyyy"), Format(d.AddMonths(1).AddDays(-1), "MM.yyyy")))
            d = d.AddMonths(1)
        End While
        Me.m2.SelectedIndex = Me.m2.Items.Count - 1

        If DateDiff(DateInterval.Month, d1, d2) > 12 Then
            d2 = d1.AddMonths(12).AddDays(-1)
            Me.m2.SelectedIndex = 11
        End If
        Me.LimitD1 = d1
        Me.LimitD2 = d2
        SetupGrid()

        
        mq.DateFrom = d1
        mq.DateUntil = d2

        Dim lis As IEnumerable(Of BO.p47CapacityPlan) = Master.Factory.p47CapacityPlanBL.GetList(mq).OrderBy(Function(p) p.Person).ThenBy(Function(p) p.j02ID)
        Dim intLastJ02ID As Integer = 0
        For Each c In lis
            If c.j02ID <> intLastJ02ID Then

            End If
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
            intLastJ02ID = c.j02ID
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


    End Sub

    Private Function GetListMatrix() As List(Of PlanMatrix)
        Dim d1 As Date = Me.LimitD1, d2 As Date = Me.LimitD2
        Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).OrderBy(Function(p) p.p85FreeText01).ThenBy(Function(p) p.p85OtherKey1).ThenBy(Function(p) p.p85FreeDate01)
        Dim dats As New List(Of Date), lis As New List(Of PlanMatrix)
        For i As Integer = Me.m1.SelectedIndex To Me.m2.SelectedIndex
            dats.Add(d1.AddMonths(i - Me.m1.SelectedIndex))
        Next

        Dim intRowIndex As Integer = 0, intLastJ02ID As Integer = 0, row As PlanMatrix = Nothing
        For Each c In lisTemp
            If c.p85OtherKey1 <> intLastJ02ID Then
                intRowIndex += 1
                row = New PlanMatrix()
                row.Person = c.p85FreeText01
                row.RowIndex = intRowIndex
                row.PID = c.p85OtherKey1
                lis.Add(row)
                DisablePersonInOffeList(c.p85OtherKey1)
            End If
            For x As Integer = Me.m1.SelectedIndex To Me.m2.SelectedIndex
                Dim i As Integer = x - Me.m1.SelectedIndex
                If c.p85FreeDate01.Month = dats(i).Month And c.p85FreeDate01.Year = dats(i).Year Then
                    BO.BAS.SetPropertyValue(row, "Col" & (i + 1).ToString & "Fa", c.p85FreeFloat01)
                    BO.BAS.SetPropertyValue(row, "Col" & (i + 1).ToString & "NeFa", c.p85FreeFloat02)
                    BO.BAS.SetPropertyValue(row, "Col" & (i + 1).ToString & "P85ID", c.PID)
                End If
            Next
            intLastJ02ID = c.p85OtherKey1
        Next

        Return lis
    End Function

    Private Sub DisablePersonInOffeList(intJ02ID As Integer)
        For Each ri As RepeaterItem In rpJ02.Items
            If CType(ri.FindControl("hidJ02ID"), HiddenField).Value = intJ02ID.ToString Then
                ri.FindControl("cmdInsert").Visible = False : Return
            End If
        Next
    End Sub

    Private Sub rpJ02_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpJ02.ItemCommand

        If e.CommandName = "add" Then
            Dim cJ02 As BO.j02Person = Master.Factory.j02PersonBL.Load(e.CommandArgument)
            Dim cTemp As New BO.p85TempBox()
            With cTemp
                .p85GUID = ViewState("guid")
                .p85OtherKey1 = cJ02.PID
                .p85OtherKey2 = Master.DataPID
                .p85FreeDate01 = Me.LimitD1
                .p85FreeDate02 = Me.LimitD1.AddMonths(1).AddDays(-1)
                .p85FreeText01 = cJ02.FullNameDesc


            End With
            Master.Factory.p85TempBoxBL.Save(cTemp)
            grid1.Rebind()
        End If
    End Sub

    Private Sub rpJ02_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpJ02.ItemDataBound
        Dim cRec As BO.j02Person = CType(e.Item.DataItem, BO.j02Person)
        CType(e.Item.FindControl("hidJ02ID"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("Person"), Label)
            .Text = cRec.FullNameDesc
        End With
        CType(e.Item.FindControl("clue_person"), HyperLink).Attributes("rel") = "clue_j02_capacity.aspx?pid=" & cRec.PID.ToString & "&p41id=" & Master.DataPID.ToString
        With CType(e.Item.FindControl("cmdInsert"), LinkButton)
            .CommandArgument = cRec.PID
        End With
    End Sub

    

    Private Sub p47_project_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If _bolNeedSetupGrid Then
            SetupGrid()
        End If

        If Page.IsPostBack Then
            grid1.Rebind()
        End If
    End Sub

   

    Private Function SaveChanges() As Boolean
        Dim lisP47 As New List(Of BO.p47CapacityPlan)
        Dim lisTemp As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), False)
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
        Return Master.Factory.p47CapacityPlanBL.SaveProjectPlan(Master.DataPID, lisP47)

    End Function

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            If SaveChanges() Then
                Master.CloseAndRefreshParent("p47-save")
            End If
        End If
    End Sub

   
    Private Sub m2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles m2.SelectedIndexChanged
        _bolNeedSetupGrid = True
        If Math.Abs(Me.m1.SelectedIndex - Me.m2.SelectedIndex) >= 12 Then
            Me.m1.SelectedIndex = Me.m2.SelectedIndex
        End If


    End Sub

    Private Sub m1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles m1.SelectedIndexChanged
        _bolNeedSetupGrid = True
        If Math.Abs(Me.m1.SelectedIndex - Me.m2.SelectedIndex) >= 12 Then
            Me.m2.SelectedIndex = Me.m1.SelectedIndex
        End If

    End Sub

    
End Class