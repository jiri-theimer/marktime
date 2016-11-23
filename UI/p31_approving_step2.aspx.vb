Public Class p31_approving_step2
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p31_approving_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "p31_approving_start"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                ViewState("can_continue") = "0"
                ViewState("masterprefix") = Request.Item("masterprefix")
                ViewState("masterpid") = Request.Item("masterpid")
                ViewState("pids") = Request.Item("pids")
                ViewState("approvingset") = Server.UrlDecode(Request.Item("approvingset"))

                If ViewState("pids") = "" Then
                    ViewState("guid") = Request.Item("guid")
                    If ViewState("guid") = "" Then .StopPage("guid is missing")
                    Dim lis As IEnumerable(Of BO.p85TempBox) = .Factory.p85TempBoxBL.GetList(ViewState("guid"))
                    ViewState("pids") = String.Join(",", lis.Select(Function(p) p.p85DataPID).Distinct)

                Else
                    ViewState("guid") = BO.BAS.GetGUID

                End If
                ViewState("clearapprove") = Request.Item("clearapprove")
                Dim s As String = "Rekapitulace úkonů pro schvalování"
                If ViewState("clearapprove") = "1" Then
                    s = "Rekapitulace úkonů pro vyčištění schvalovacího statusu"
                End If

                If ViewState("masterprefix") <> "" Then
                    .HeaderText = s & " | " & .Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(ViewState("masterprefix")), BO.BAS.IsNullInt(ViewState("masterpid")))
                Else
                    .HeaderText = s
                End If

                .HeaderIcon = "Images/approve_32.png"
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p31_grid-period")
                    .Add("periodcombo-custom_query")
                    .Add("p31_approving_step2-chkSkipThisStep")
                End With
                Me.chkSkipThisStep.Checked = BO.BAS.BG(.Factory.j03UserBL.GetUserParam("p31_approving_step2-chkSkipThisStep", "0"))
                .AddToolbarButton("Pokračovat", "continue", , "Images/continue.png")
            End With

            
            SetupGrid()

            RefreshRecord()

            
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim mqO23 As New BO.myQueryO23
        notepad1.EntityX29ID = BO.BAS.GetX29FromPrefix(ViewState("masterprefix"))
        panO23.Visible = True
        Select Case ViewState("masterprefix")
            Case "p28"
                lblO23.Text = "Fakturační poznámky ke klientovi"
                mqO23.p28ID = BO.BAS.IsNullInt(ViewState("masterpid"))
                Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(mqO23.p28ID)
                If cRec.p28BillingMemo <> "" Then Me.BillingMemo.Text = BO.BAS.CrLfText2Html(cRec.p28BillingMemo)
            Case "p41"
                lblO23.Text = "Fakturační poznámky k projektu"
                mqO23.p41ID = BO.BAS.IsNullInt(ViewState("masterpid"))
                Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(mqO23.p41ID)
                If cRec.p41BillingMemo <> "" Then Me.BillingMemo.Text = BO.BAS.CrLfText2Html(cRec.p41BillingMemo)
                If cRec.p28ID_Client > 0 Then
                    Dim cClient As BO.p28Contact = Master.Factory.p28ContactBL.Load(cRec.p28ID_Client)
                    If cClient.p28BillingMemo <> "" Then Me.BillingMemo.Text += "<br>" & BO.BAS.CrLfText2Html(cClient.p28BillingMemo)
                End If
            Case "j02"
                lblO23.Text = "Fakturační poznámky k osobě"
                mqO23.j02ID = BO.BAS.IsNullInt(ViewState("masterpid"))
            Case Else
                panO23.Visible = False
        End Select
        If panO23.Visible Then
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23).Where(Function(p) p.o24IsBillingMemo = True)
            If lisO23.Count > 0 Then
                notepad1.RefreshData(lisO23, Master.DataPID)
                Me.lblO23.Text += " (" & notepad1.RowsCount.ToString & ")"
            Else
                panO23.Visible = False
            End If
        End If
        
    End Sub

    Private Sub SetupGrid()

        With Master.Factory.j74SavedGridColTemplateBL
            Dim cJ74 As BO.j74SavedGridColTemplate = .LoadSystemTemplate(BO.x29IdEnum.p31Worksheet, Master.Factory.SysUser.PID, "approving_step3")
            
            basUIMT.SetupGrid(Master.Factory, Me.grid1, cJ74, 5000, False, False)
        End With



    End Sub

    Private Sub InhaleMyQuery(ByRef mq As BO.myQueryP31)
        With mq
            If ViewState("pids") <> "" Then
                Dim a() As String = Split(ViewState("pids"), ",")
                For Each strP31ID As String In a
                    .AddItemToPIDs(CInt(strP31ID))
                Next
            End If

        End With
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then Return
        basUIMT.p31_grid_Handle_ItemDataBound(sender, e)
        Dim cRec As BO.p31Worksheet = CType(e.Item.DataItem, BO.p31Worksheet)
        If cRec.p91ID > 0 Or cRec.p31IsPlanRecord Then
            e.Item.ForeColor = Drawing.Color.Red    'zamítnuté
        End If
    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        Dim mq As New BO.myQueryP31
        With mq
            .MG_PageSize = 0
            .MG_SortString = grid1.radGridOrig.MasterTableView.SortExpressions.GetSortString()

        End With
        InhaleMyQuery(mq)

        Dim lis As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)


        Me.CountAll.Text = lis.Count.ToString
        Dim intRefused As Integer = lis.Where(Function(p) p.p91ID > 0 Or p.p31IsPlanRecord = True).Count
        Me.CountRefused.Text = intRefused.ToString
        If intRefused > 0 Then
            Me.CountRefused.Text = intRefused.ToString
            If intRefused = lis.Count Then
                Master.Notify("Systém zamítnul pro schvalování všechny vstupní záznamy. Pravděpodobně se jedná o již vyfakturované úkony.", NotifyLevel.WarningMessage)
                Master.HideShowToolbarButton("continue", False)
            Else
                Master.Notify("Z výběru worksheet úkonů pro schvalování jich systém několik zamítnul (" & intRefused.ToString & " - odlišené červeným písmem). Pravděpodobně se jedná o již vyfakturované úkony.", NotifyLevel.InfoMessage)
            End If
        Else
            ViewState("can_continue") = "1"
        End If

        grid1.DataSource = lis.Where(Function(p) p.p91ID = 0 Or p.p31IsPlanRecord = False)

        If lis.Where(Function(p) p.p91ID = 0 Or p.p31IsPlanRecord = False).Count = 0 Then
            ViewState("can_continue") = "0"
        End If

        Dim p41ids As List(Of Integer) = lis.Select(Function(p) p.p41ID).Distinct.ToList
        Dim p28ids As List(Of Integer) = lis.Select(Function(p) p.p28ID_Client).Distinct.ToList


        Dim sets As List(Of String) = Master.Factory.p31WorksheetBL.GetList_ApprovingSet("", p41ids, p28ids)
        
        With Me.p31ApprovingSet
            For Each s In sets
                .Items.Add(New Telerik.Web.UI.RadComboBoxItem(s))
            Next
            .Text = Master.Factory.SysUser.j03Login & " " & Format(Now, "dd.MM.yyyy")
        End With


    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "continue" Then
            Master.Factory.j03UserBL.SetUserParam("p31_approving_step2-chkSkipThisStep", BO.BAS.GB(Me.chkSkipThisStep.Checked))
            Handle_Continue(False)
        End If
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        RefreshRecord()
    End Sub

    Private Sub Handle_Continue(bolAutoPilot As Boolean)
        Dim lisP31IDs As List(Of Integer) = grid1.GetAllPIDs()
        If lisP31IDs.Count = 0 Then
            Master.Notify("Žádné úkony pro schvalování.", NotifyLevel.WarningMessage) : Return
        End If
        Dim mq As New BO.myQueryP31
        mq.PIDs = lisP31IDs
        Dim lisOrigP31 As IEnumerable(Of BO.p31Worksheet) = Master.Factory.p31WorksheetBL.GetList(mq)

        With Master.Factory.p85TempBoxBL
            .Truncate(ViewState("guid"))
            For Each cRec In lisOrigP31.Where(Function(p) p.p91ID = 0 And p.p31IsPlanRecord = False)
                'vyloučit již vyfakturované úkony nebo plán záznamy
                Dim c As New BO.p85TempBox()
                c.p85GUID = ViewState("guid")
                c.p85DataPID = cRec.PID
                .Save(c)
            Next

        End With

        Dim strURL As String = "p31_approving_step3.aspx?guid=" & ViewState("guid") & "&gridheight=" & Me.hidGridHeight.Value
        If ViewState("clearapprove") = "1" Then
            strURL += "&clearapprove=1"
        End If
        If ViewState("masterprefix") <> "" Then
            strURL += "&masterprefix=" & ViewState("masterprefix") & "&masterpid=" & ViewState("masterpid")
        End If
        If Me.p31ApprovingSet.Text <> "" Then
            strURL += "&approvingset=" & Server.UrlEncode(Me.p31ApprovingSet.Text)
        End If


        Response.Redirect(strURL)

    End Sub

    

    Private Sub cmdAutoContinue_Click(sender As Object, e As EventArgs) Handles cmdAutoContinue.Click
        Handle_Continue(True)
    End Sub
End Class