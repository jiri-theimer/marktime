Imports Telerik.Web.UI

Public Class p49_plan
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p49_plan_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                ''.HeaderIcon = "Images/finplan_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("p41id"))
                If .DataPID = 0 Then
                    .StopPage("p41id is missing")
                End If
                Dim cRec As BO.p41Project = .Factory.p41ProjectBL.Load(.DataPID)
                Dim cDisp As BO.p41RecordDisposition = Master.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
                If Not cDisp.p49_Read Then
                    .StopPage("Nedisponujete oprávněním ke čtení finančního plánu projektu.")
                End If
                cmdClone.Visible = cDisp.p49_Create
                cmdNew.Visible = cDisp.p49_Create
                If Not cDisp.p49_Create Then
                    grid1.OnRowDblClick = ""
                    .Notify("Finanční plán projektu můžete pouze číst, nikoliv upravovat.")
                End If
                .HeaderText = "Finanční plán | " & .Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .DataPID)
            End With

            SetupGrid()
        End If
    End Sub

    Private Sub SetupGrid()
        With grid1
            '.AddColumn("p34Name", "Sešit")
            .AddColumn("p32Name", "Aktivita")
            .AddColumn("Person", "Osoba")
            .AddColumn("p49DateFrom", "Od", BO.cfENUM.DateOnly)
            .AddColumn("p49DateUntil", "Do", BO.cfENUM.DateOnly)


            .AddColumn("p49Text", "Text")
            .AddColumn("p49Amount", "Částka plánu", BO.cfENUM.Numeric2, , , , , True)
            .AddColumn("j27Code", "")
            .AddColumn("Amount_Orig", "Vykázáno", BO.cfENUM.Numeric2, , , , , True)
            .AddColumn("Amount_Approved", "Schváleno", BO.cfENUM.Numeric2, , , , , True)
            .AddColumn("Amount_Invoiced", "Vyfakturováno", BO.cfENUM.Numeric2, , , , , True)
        End With
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            .ShowGroupFooter = True
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = "p34Name"
            fld.HeaderText = "Sešit"

            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)
        End With
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p49FinancialPlan = CType(e.Item.DataItem, BO.p49FinancialPlan)
        If cRec.p34IncomeStatementFlag = BO.p34IncomeStatementFlagENUM.Prijem Then
            dataItem.ForeColor = Drawing.Color.Blue
        Else
            dataItem.ForeColor = Drawing.Color.Brown
        End If
        If cRec.IsClosed Then dataItem.Font.Strikeout = True
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryP49
        mq.p41IDs.Add(Master.DataPID)

        Dim lis As IEnumerable(Of BO.p49FinancialPlan) = Master.Factory.p49FinancialPlanBL.GetList(mq, True)
        grid1.DataSource = lis

        lblCount.Text = lis.Count.ToString

        
    End Sub

    Private Sub p49_plan_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then
            If Request.Item("p49id") <> "" Then
                grid1.SelectRecords(BO.BAS.IsNullInt(Request.Item("p49id")))
                Me.hiddatapid.Value = Request.Item("p49id")
            End If
        End If
    End Sub
End Class