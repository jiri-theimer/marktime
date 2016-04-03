Imports Telerik.Web.UI
Public Class p91_subgrid
    Inherits System.Web.UI.UserControl
    Public Property MasterDataPID As Integer
    Public Property Factory As BL.Factory
    Public Property x29ID As BO.x29IdEnum

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            SetupGridP91()
        End If

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p91_framework-periodtype")
                .Add("p91_framework-period")
                .Add("periodcombo-custom_query")
            End With
            With Factory.j03UserBL
                .InhaleUserParams(lisPars)
                basUI.SelectDropdownlistValue(Me.cbxPeriodType, .GetUserParam("p91_framework-periodtype", "p91DateSupply"))
                period1.SetupData(Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("p91_framework-period")
            End With
        End If

        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub


    Private Sub gridP91_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gridP91.ItemDataBound
        basUIMT.p91_grid_Handle_ItemDataBound(sender, e)
    End Sub

    Private Sub SetupGridP91()
        With gridP91
            .ClearColumns()
            .radGridOrig.ShowFooter = True
            .AddSystemColumn(20)

            .PageSize = 20
            .AddColumn("p91Code", "Číslo", , True)
            .AddColumn("p92Name", "Typ", , True)
            If Me.x29ID = BO.x29IdEnum.j02Person Then
                .AddColumn("p28Name", "Klient", , True)
            End If
            If Me.x29ID = BO.x29IdEnum.p28Contact Then
                .AddColumn("p41Name", "Projekt", , True)
            End If
            .AddColumn("p91Date", "Datum", BO.cfENUM.DateOnly)
            .AddColumn("p91DateSupply", "Plnění", BO.cfENUM.DateOnly)
            .AddColumn("p91DateMaturity", "Splatnost", BO.cfENUM.DateOnly)
            .AddColumn("p91Amount_WithoutVat", "Bez DPH", BO.cfENUM.Numeric, True, , , , True)
            .AddColumn("p91Amount_Debt", "Dluh", BO.cfENUM.Numeric, True, , , , True)
            .AddColumn("j27Code", "Měna")

        End With
        
    End Sub

    Private Sub SetupGroupByCurrency()
        With gridP91.radGridOrig.MasterTableView
            .ShowGroupFooter = True
            Dim GGE As New Telerik.Web.UI.GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = "j27Code"
            fld.HeaderText = "Měna"


            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)

        End With
    End Sub

    Private Sub gridP91_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridP91.NeedDataSource
        If MasterDataPID = 0 Then Return

        Dim mq As New BO.myQueryP91
        InhaleMyQueryP91(mq)

        Dim lis As IEnumerable(Of BO.p91Invoice) = Factory.p91InvoiceBL.GetList(mq)
        Dim bolGroupByCurrency As Boolean = False
        If gridP91.radGridOrig.MasterTableView.GroupByExpressions.Count = 0 And lis.Count > 0 Then
            If lis.Select(Function(p) p.j27ID).Distinct.Count > 1 Then
                SetupGroupByCurrency()
                bolGroupByCurrency = True
            End If
        End If
        
        gridP91.DataSource = lis
        If Not bolGroupByCurrency Then
            gridP91.radGridOrig.ShowFooter = True
            ViewState("p91_footersum") = "p91Amount_WithoutVat;" & BO.BAS.FN(lis.Sum(Function(p) p.p91Amount_WithoutVat)) & "|p91Amount_Debt;" & BO.BAS.FN(lis.Sum(Function(p) p.p91Amount_Debt))
        Else
            gridP91.radGridOrig.ShowFooter = False
        End If

    End Sub

    Private Sub InhaleMyQueryP91(ByRef mq As BO.myQueryP91)
        With mq
            Select Case Me.x29ID
                Case BO.x29IdEnum.p41Project
                    .p41ID = MasterDataPID
                Case BO.x29IdEnum.p28Contact
                    .p28ID = MasterDataPID
                Case BO.x29IdEnum.j02Person
                    .j02ID = MasterDataPID
            End Select

            .Closed = BO.BooleanQueryMode.NoQuery
            .SpecificQuery = BO.myQueryP91_SpecificQuery.AllowedForRead

            Select Case Me.cbxPeriodType.SelectedValue
                Case "p91DateSupply" : .PeriodType = BO.myQueryP91_PeriodType.p91DateSupply
                Case "p91DateMaturity" : .PeriodType = BO.myQueryP91_PeriodType.p91DateMaturity
                Case "p91Date" : .PeriodType = BO.myQueryP91_PeriodType.p91Date
            End Select
            .DateFrom = period1.DateFrom
            .DateUntil = period1.DateUntil
            '.QuickQuery = Me.CurrentQuickQuery


        End With

    End Sub

    Private Sub gridP91_NeedFooterSource(footerItem As GridFooterItem, footerDatasource As Object) Handles gridP91.NeedFooterSource
        footerItem.Item("systemcolumn").Text = "<img src='Images/sum.png'/>"

        gridP91.ParseFooterItemString(footerItem, ViewState("p91_footersum"))
    End Sub
    Private Sub cbxPeriodType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPeriodType.SelectedIndexChanged
        Factory.j03UserBL.SetUserParam("p91_framework-periodtype", Me.cbxPeriodType.SelectedValue)
        gridP91.Rebind(False)
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Factory.j03UserBL.SetUserParam("p91_framework-period", Me.period1.SelectedValue)
        gridP91.Rebind(False)
    End Sub
End Class