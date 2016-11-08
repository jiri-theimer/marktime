Imports Telerik.Web.UI
Public Class p91_remove_worksheet
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p91_remove_worksheet_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p91_remove_worksheet"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                ViewState("p31ids") = Request.Item("p31ids")
                If ViewState("p31ids") = "" Then .StopPage("p31ids missing.")
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then .StopPage("pid missing")
                .HeaderIcon = "Images/cut_32.png"
                .AddToolbarButton("Přesunout do archivu", "remove2bin", , "Images/bin.png")
                .AddToolbarButton("Přesunout do rozpracovanosti", "remove2wip", , "Images/worksheet.png")
                .AddToolbarButton("Přesunout do schválených", "remove2approve", , "Images/approve.png")


                Dim cRec As BO.p91Invoice = .Factory.p91InvoiceBL.Load(.DataPID)
                .HeaderText = "Vyjmout z faktury vybrané úkony | " & cRec.p91Code

                Dim cDisp As BO.p91RecordDisposition = Master.Factory.p91InvoiceBL.InhaleRecordDisposition(cRec)
                If Not cDisp.OwnerAccess Then .StopPage("V kontextu této faktury nemáte oprávnění k funkci.")
                
            End With

            SetupGrid()
        End If
    End Sub

    Private Sub SetupGrid()
        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "p91")

            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 500, False, False)
        End With



    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryP31
        Dim a() As String = Split(ViewState("p31ids"), ",")
        For Each s In a
            mq.AddItemToPIDs(CInt(s))
        Next
        With mq
            .MG_PageSize = 500
            .MG_CurrentPageIndex = grid1.radGridOrig.MasterTableView.CurrentPageIndex
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)
        grid1.DataSource = lis

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        Select Case strButtonValue
            Case "remove2approve", "remove2wip", "remove2bin"
                Dim pids As New List(Of Integer)
                Dim a() As String = Split(ViewState("p31ids"), ",")
                For i As Integer = 0 To UBound(a)
                    pids.Add(CInt(a(i)))
                Next
                With Master.Factory.p31WorksheetBL
                    
                    If strButtonValue = "remove2approve" Then
                        If .RemoveFromInvoice(Master.DataPID, pids) Then
                            Master.CloseAndRefreshParent("p31-remove")
                        Else
                            Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                        End If
                    End If
                    If strButtonValue = "remove2wip" Then
                        If .RemoveFromInvoice(Master.DataPID, pids) Then
                            If .RemoveFromApproving(pids) Then
                                Master.CloseAndRefreshParent("p31-remove")
                            End If
                        Else
                            Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                        End If
                    End If
                    If strButtonValue = "remove2bin" Then
                        If .RemoveFromInvoice(Master.DataPID, pids) Then
                            If .RemoveFromApproving(pids) Then
                                If .MoveToBin(pids) Then
                                    Master.CloseAndRefreshParent("p31-remove")
                                End If
                            End If
                        End If
                    End If
                End With
        End Select
        
    End Sub
End Class