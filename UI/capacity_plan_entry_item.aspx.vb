Public Class capacity_plan_entry_item
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    
    Private Sub capacity_plan_entry_item_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = Request.Item("guid")
            ViewState("p41id") = Request.Item("p41id")
            With Master
                If ViewState("guid") = "" Then .StopPage("guid missing.")
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .AddToolbarButton("OK", "ok", , "Images/ok.png")
            End With

            RefreshData()
        End If

    End Sub

    Private Sub RefreshData()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(ViewState("p41id"))
        Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"), True).Where(Function(p) p.p85OtherKey1 = Master.DataPID)
        For Each c In lis
            If c.p85IsDeleted Then
                c.p85IsDeleted = False
                Master.Factory.p85TempBoxBL.Save(c)
            End If
        Next
        Dim cJ02 As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
        Me.lblHeader.Text = cJ02.FullNameAsc

        Dim d1 As Date = DateSerial(Year(cRec.p41PlanFrom), Month(cRec.p41PlanFrom), 1)
        Dim d2 As Date = DateSerial(Year(cRec.p41PlanUntil), Month(cRec.p41PlanUntil), 1).AddMonths(1).AddDays(-1)
        Dim d As Date = d1
        While d <= d2
            If lis.Where(Function(p) p.p85FreeDate01 = d).Count = 0 Then
                Dim c As New BO.p85TempBox
                With c
                    .p85GUID = ViewState("guid")
                    .p85OtherKey1 = cJ02.PID
                    .p85FreeDate01 = d
                    .p85FreeDate02 = d.AddMonths(1).AddDays(-1)
                    .p85FreeText01 = cJ02.FullNameDesc
                    .p85OtherKey2 = CInt(ViewState("p41id"))
                End With
                Master.Factory.p85TempBoxBL.Save(c)
            End If
            d = d.AddMonths(1)
        End While


        rp1.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid")).Where(Function(p) p.p85OtherKey1 = Master.DataPID).OrderBy(Function(p) p.p85FreeDate01)
        rp1.DataBind()
    End Sub

    Private Sub rp1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rp1.ItemCommand

    End Sub

    
    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)
        CType(e.Item.FindControl("p85id"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("Mesic"), Label)
            .Text = Month(cRec.p85FreeDate01).ToString & "/" & Year(cRec.p85FreeDate01).ToString
        End With
        
        If cRec.p85FreeFloat01 <> 0 Then
            With CType(e.Item.FindControl("p85FreeFloat01"), Telerik.Web.UI.RadNumericTextBox)
                .Value = cRec.p85FreeFloat01
                .Style.Item("background-color") = "#98FB98"
            End With
        End If
        If cRec.p85FreeFloat02 <> 0 Then
            With CType(e.Item.FindControl("p85FreeFloat02"), Telerik.Web.UI.RadNumericTextBox)
                .Value = cRec.p85FreeFloat02
                .Style.Item("background-color") = "#FFA07A"
            End With
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Save2Temp()
            Master.CloseAndRefreshParent()
        End If
    End Sub

    Private Sub Save2Temp()
        
        For Each ri As RepeaterItem In rp1.Items
            Dim intP85ID As Integer = CInt(CType(ri.FindControl("p85id"), HiddenField).Value)
            Dim c As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(intP85ID)
            c.p85FreeFloat01 = BO.BAS.IsNullNum(CType(ri.FindControl("p85FreeFloat01"), Telerik.Web.UI.RadNumericTextBox).Value)
            c.p85FreeFloat02 = BO.BAS.IsNullNum(CType(ri.FindControl("p85FreeFloat02"), Telerik.Web.UI.RadNumericTextBox).Text)

            Master.Factory.p85TempBoxBL.Save(c)
        Next
    End Sub
End Class