Imports Telerik.Web.UI
Public Class p91_delete
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p91_delete_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p91_delete"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hidGUID.Value = BO.BAS.GetGUID
            With Master

                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then .StopPage("pid missing.")
                .AddToolbarButton("Potvrdit odstranění faktury", "ok", , "Images/delete.png")

                Dim cRec As BO.p91Invoice = .Factory.p91InvoiceBL.Load(.DataPID)
                .HeaderText = String.Format("Odstranit fakturu {0}", cRec.p91Code)
                Dim cDisp As BO.p91RecordDisposition = Master.Factory.p91InvoiceBL.InhaleRecordDisposition(cRec)
                If Not cDisp.OwnerAccess Then .StopPage("Nemáte vlastnické oprávnění k této faktuře.")

            End With

            SetupGrid()

            RefreshTemp()
        End If
    End Sub

    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p91")

            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 500, False, True, True, , , , , 80)
        End With

    End Sub
    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)

        If Not TypeOf e.Item Is GridDataItem Then Return
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        'Dim cRec As BO.p31Worksheet = CType(e.Item.DataItem, BO.p31Worksheet)
        With dataItem("systemcolumn")

            Dim c As BO.p85TempBox = Master.Factory.p85TempBoxBL.GetList(hidGUID.Value).First(Function(p) p.p85DataPID = dataItem.GetDataKeyValue("pid"))
            If Not c Is Nothing Then
                Select Case c.p85OtherKey1
                    Case 1
                        .Text = "<span class='valbold'>Přesunout do schválených</span>"
                    Case 2
                        .Text = "<span class='valbold'>Přesunout do rozpracovaných</span>"
                    Case 3
                        .Text = "<span class='valbold'>Přesunout do archivu</span>"
                End Select
            End If


        End With
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryP31
        mq.p91ID = Master.DataPID
        With mq
            .MG_PageSize = 500
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        grid1.DataSource = lis

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            If Master.Factory.p85TempBoxBL.GetList(hidGUID.Value).Where(Function(p) p.p85OtherKey1 = 0).Count > 0 Then
                Master.Notify("Minimálně u jednoho úkonu faktury musíte ještě rozhodnout!", NotifyLevel.ErrorMessage)
                Return
            End If
            With Master.Factory.p91InvoiceBL
                If .Delete(Master.DataPID) Then
                    Master.DataPID = 0
                    Master.CloseAndRefreshParent("p91-delete")
                Else
                    Master.Notify(.ErrorMessage, 2)
                End If
            End With
        End If
        
    End Sub

    Private Sub cmdBatch1_Click(sender As Object, e As EventArgs) Handles cmdBatch1.Click
        RunBatch(1)
    End Sub

    Private Sub cmdBatch2_Click(sender As Object, e As EventArgs) Handles cmdBatch2.Click
        RunBatch(2)
    End Sub

    Private Sub RunBatch(intOper As Integer)
        Dim lis As List(Of Integer) = grid1.GetSelectedPIDs()
        If lis.Count = 0 Then
            Master.Notify("Musíte vybrat (zaškrtnout) alespoň jeden záznam.")
            Return
        End If
        RefreshTemp(lis)
    End Sub

    Private Sub p91_delete_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
       
    End Sub


    Private Sub opg1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opg1.SelectedIndexChanged
        RefreshTemp()
        grid1.Rebind(False)
    End Sub

    Private Sub RefreshTemp(Optional lis As List(Of Integer) = Nothing, Optional intOper As Integer = 0)
        Master.Factory.p85TempBoxBL.Truncate(hidGUID.Value)
        Dim mq As New BO.myQueryP31
        mq.p91ID = Master.DataPID
        If lis Is Nothing Then lis = Master.Factory.p31WorksheetBL.GetList(mq).Select(Function(p) p.PID).ToList
        For Each c In lis
            Dim cTemp As New BO.p85TempBox
            cTemp.p85GUID = hidGUID.Value
            cTemp.p85DataPID = c
            If intOper = 0 Then
                intOper = CInt(opg1.SelectedValue)
            End If
            If intOper <> 4 Then
                cTemp.p85OtherKey1 = intOper
            End If

            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next
    End Sub

    Private Sub cmdBatch3_Click(sender As Object, e As EventArgs) Handles cmdBatch3.Click
        RunBatch(3)
    End Sub
End Class