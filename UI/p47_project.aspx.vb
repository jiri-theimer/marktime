Imports Telerik.Web.UI

Public Class p47_project
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Public Class PlanMatrix
        Public Property Person As String
        Public Property PID As Integer
        Public Property RowIndex As Integer
        Public Property Col1Fa As Double?
        Public Property Col1NeFa As Double?
        Public Property Col1D As Date?
        Public Property Col2Fa As Double?
        Public Property Col2NeFa As Double?
        Public Property Col2D As Date?
        Public Property Col3Fa As Double?
        Public Property Col3NeFa As Double?
        Public Property Col3D As Date?
        Public Property Col4Fa As Double?
        Public Property Col4NeFa As Double?
        Public Property Col4D As Date?
        Public Property Col5Fa As Double?
        Public Property Col5NeFa As Double?
        Public Property Col5D As Date?
        Public Property Col6Fa As Double?
        Public Property Col6NeFa As Double?
        Public Property Col6D As Date?
        Public Property Col7Fa As Double?
        Public Property Col7NeFa As Double?
        Public Property Col7D As Date?
        Public Property Col8Fa As Double?
        Public Property Col8NeFa As Double?
        Public Property Col8D As Date?
        Public Property Col9Fa As Double?
        Public Property Col9NeFa As Double?
        Public Property Col9D As Date?
        Public Property Col10Fa As Double?
        Public Property Col10NeFa As Double?
        Public Property Col10D As Date?
        Public Property Col11Fa As Double?
        Public Property Col11NeFa As Double?
        Public Property Col11D As Date?
        Public Property Col12Fa As Double?
        Public Property Col12NeFa As Double?
        Public Property Col12D As Date?
    End Class

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
                .HeaderText = "Kapacitní plán | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
            End With


            RefreshRecord()


        End If
    End Sub

    Private Sub AddGroupHeader(strName As String, strHeaderText As String)
        Dim group As New GridColumnGroup
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
        grid1.MasterTableView.ColumnGroups.Add(group)
    End Sub


    Private Sub SetupGrid()
        grid1.Columns.Clear()
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

   
    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource


        grid1.DataSource = GetListMatrix()

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
        Me.LimitD1 = d1
        Me.LimitD2 = d2
        SetupGrid()

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

        mq.DateFrom = d1
        mq.DateUntil = d2

        Dim lis As IEnumerable(Of BO.p47CapacityPlan) = Master.Factory.p47CapacityPlanBL.GetList(mq).Where(Function(p) p.p47HoursBillable <> 0 Or p.p47HoursNonBillable <> 0).OrderBy(Function(p) p.Person).ThenBy(Function(p) p.j02ID)
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
        For i As Integer = 0 To 11
            dats.Add(d1.AddMonths(i))
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
            End If
            For i As Integer = 0 To 11
                If c.p85FreeDate01.Month = dats(i).Month And c.p85FreeDate01.Year = dats(i).Year Then
                    BO.BAS.SetPropertyValue(row, "Col" & (i + 1).ToString & "Fa", c.p85FreeFloat01)
                    BO.BAS.SetPropertyValue(row, "Col" & (i + 1).ToString & "NeFa", c.p85FreeFloat02)
                    BO.BAS.SetPropertyValue(row, "Col" & (i + 1).ToString & "D", c.p85FreeDate01)
                End If
            Next
            intLastJ02ID = c.p85OtherKey1
        Next

        Return lis
    End Function

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

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        SetupGrid()
        grid1.Rebind()

    End Sub
End Class