Imports Telerik.Web.UI
Public Class basUIMT
    Public Shared Sub SetupGrid(factory As BL.Factory, grid As UI.datagrid, cJ74 As BO.j74SavedGridColTemplate, intPageSize As Integer, bolCustomPaging As Boolean, bolAllowMultiSelect As Boolean, Optional bolMultiSelectCheckboxSelector As Boolean = True, Optional strFilterSetting As String = "", Optional strFilterExpression As String = "", Optional strSortExpression As String = "")
        With grid
            .ClearColumns()
            .AllowMultiSelect = bolAllowMultiSelect
            .DataKeyNames = "pid"

            If cJ74.j74DrillDownField1 = "" Then
                'bez drill-down
                .AllowCustomPaging = bolCustomPaging
                If bolAllowMultiSelect And bolMultiSelectCheckboxSelector Then .AddCheckboxSelector()
                '.AddSystemColumn(5)

                .PageSize = intPageSize
                .AddSystemColumn(5)
                .radGridOrig.PagerStyle.Mode = Telerik.Web.UI.GridPagerMode.NextPrevAndNumeric
                .AllowFilteringByColumn = cJ74.j74IsFilteringByColumn
                If cJ74.j74IsVirtualScrolling Then
                    .radGridOrig.MasterTableView.TableLayout = GridTableLayout.Fixed
                    .radGridOrig.ClientSettings.Scrolling.AllowScroll = True
                    .radGridOrig.ClientSettings.Scrolling.EnableVirtualScrollPaging = True
                    .radGridOrig.ClientSettings.Scrolling.UseStaticHeaders = True
                    .radGridOrig.ClientSettings.Scrolling.SaveScrollPosition = True
                End If
                .radGridOrig.MasterTableView.Name = "grid"
                If strSortExpression <> "" Then .radGridOrig.MasterTableView.SortExpressions.AddSortExpression(strSortExpression)
            Else
                'hiearchický grid - drill-down
                .AddSystemColumn(5)
                .AllowCustomPaging = False
                .DataKeyNames = "pid"
                .PageSize = intPageSize
                .AllowFilteringByColumn = False
                .radGridOrig.MasterTableView.Name = "drilldown"

            End If

            Dim lisCols As List(Of BO.GridColumn) = factory.j74SavedGridColTemplateBL.ColumnsPallete(cJ74.x29ID)

            If cJ74.j74DrillDownField1 = "" Then
                For Each s In Split(cJ74.j74ColumnNames, ",")
                    Dim strField As String = Trim(s)

                    Dim c As BO.GridColumn = lisCols.Find(Function(p) p.ColumnName = strField)

                    If Not c Is Nothing Then
                        .AddColumn(c.ColumnName, c.ColumnHeader, c.ColumnType, c.IsSortable, , c.ColumnDBName, , c.IsShowTotals, c.IsAllowFiltering)
                    End If
                Next
                grid.SetFilterSetting(strFilterSetting, strFilterExpression)
            Else
                Dim colDrill As BO.GridGroupByColumn = factory.j74SavedGridColTemplateBL.GroupByPallet(cJ74.x29ID).Where(Function(p) p.ColumnField = cJ74.j74DrillDownField1).First
                .AddColumn(colDrill.ColumnField, colDrill.ColumnHeader)
                .AddColumn("RowsCount", "Počet", BO.cfENUM.Numeric0)
                Dim strSumFields As String = ""
                For Each s In Split(cJ74.j74ColumnNames, ",")   'součtové sloupce
                    Dim strField As String = Trim(s)
                    Dim c As BO.GridColumn = lisCols.Find(Function(p) p.ColumnName = strField And p.IsShowTotals = True)
                    If Not c Is Nothing Then
                        .AddColumn(c.ColumnName, c.ColumnHeader, c.ColumnType, c.IsSortable, , c.ColumnDBName, , True)
                        strSumFields += "|" & c.ColumnName
                    End If
                Next
                If strSumFields <> "" Then grid.radGridOrig.MasterTableView.Attributes("sumfields") = BO.BAS.OM1(strSumFields)
                Dim gtv As New GridTableView(.radGridOrig)
                With gtv
                    .HierarchyLoadMode = GridChildLoadMode.ServerOnDemand
                    .RetainExpandStateOnRebind = True
                    .Name = "grid"
                    .AllowCustomPaging = True
                    .AllowFilteringByColumn = False
                    .AllowSorting = True
                    .PageSize = intPageSize
                    .DataKeyNames = Split("pid", ",")
                    .ClientDataKeyNames = Split("pid", ",")
                    .ShowHeadersWhenNoRecords = False
                    .ShowFooter = False
                    If strSortExpression <> "" Then .SortExpressions.AddSortExpression(strSortExpression)
                End With

                .radGridOrig.MasterTableView.DetailTables.Add(gtv)
                .AddSystemColumn(5, , gtv)
                For Each s In Split(cJ74.j74ColumnNames, ",")
                    Dim strField As String = Trim(s)

                    Dim c As BO.GridColumn = lisCols.Find(Function(p) p.ColumnName = strField)
                    If Not c Is Nothing Then
                        .AddColumn(c.ColumnName, c.ColumnHeader, c.ColumnType, c.IsSortable, , c.ColumnDBName, , c.IsShowTotals, c.IsAllowFiltering, , gtv)
                    End If
                Next
            End If

        End With


    End Sub

    Public Shared Sub MakeDockZonesUserFriendly(rdl As RadDockLayout, bolLockedInteractivity As Boolean)

        For Each zone In rdl.RegisteredZones
            zone.BorderStyle = BorderStyle.Solid
            zone.BorderColor = Drawing.Color.Silver
            Dim dh As DockHandle = DockHandle.TitleBar
            If bolLockedInteractivity Then
                dh = DockHandle.None
                zone.MinHeight = Nothing
                zone.MinWidth = Nothing
                zone.BorderStyle = BorderStyle.None
            End If
            For Each d In zone.Docks
                d.DockHandle = dh

            Next
        Next
    End Sub

    Public Shared Sub p91_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p91Invoice = CType(e.Item.DataItem, BO.p91Invoice)
        With cRec
            If .p91IsDraft Then
                dataItem("systemcolumn").CssClass = "draft"
            Else
                If .p91Amount_Debt > 10 Then
                    If .p91DateMaturity >= Today Then
                        dataItem("systemcolumn").CssClass = "p91_yellow"
                    Else
                        If Math.Abs(.p91Amount_TotalDue - .p91Amount_Debt) < 50 Then
                            dataItem("systemcolumn").CssClass = "p91_red"
                        Else
                            dataItem("systemcolumn").CssClass = "p91_pink"
                        End If
                    End If
                End If
            End If
            
            If cRec.IsClosed Then dataItem.Font.Strikeout = True
            If cRec.p92InvoiceType = BO.p92InvoiceTypeENUM.CreditNote Then
                'dobropis - opravný doklad
                dataItem("systemcolumn").CssClass = "p91_creditnote"
            End If
        End With
    End Sub
    Public Shared Sub x40_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.x40MailQueue = CType(e.Item.DataItem, BO.x40MailQueue)
        dataItem("UserInsert").Text = ""  'náhrada za systemcolumn
        If cRec.x40Attachments > "" Then
            dataItem("UserInsert").CssClass = "attachment"

        End If
        Select Case cRec.x40State
            Case BO.x40StateENUM.InQueque
                dataItem("x40State").Text = "Čeká na odeslání"

            Case BO.x40StateENUM.IsError
                dataItem("x40State").Text = "Chyba"
                dataItem.ForeColor = Drawing.Color.Red
            Case BO.x40StateENUM.IsProceeded
                dataItem("x40State").Text = "Odesláno"
                dataItem.ForeColor = Drawing.Color.Green
            Case BO.x40StateENUM.IsStopped
                dataItem("x40State").Text = "Zastaveno"
                dataItem.ForeColor = Drawing.Color.Magenta
        End Select
    End Sub

    Public Shared Sub p56_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs, bolShowClueTip As Boolean)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p56Task = CType(e.Item.DataItem, BO.p56Task)
        If bolShowClueTip Then
            With dataItem("systemcolumn")
                .Text = "<a class='reczoom' title='Detail úkolu' rel='clue_p56_record.aspx?pid=" & cRec.PID.ToString & "'>i</a>"
            End With
        End If
        With cRec
            If .IsClosed Then
                dataItem.Font.Strikeout = True
            Else
                If Not .p56PlanUntil Is Nothing Then
                    If Now > .p56PlanUntil Then dataItem("systemcolumn").CssClass = "overtime"
                End If
            End If
            If .b02ID > 0 Then
                If .b02Color <> "" Then dataItem("systemcolumn").Style.Item("background-color") = .b02Color
            End If
        End With

    End Sub
    Public Shared Sub o23_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs, bolShowClueTip As Boolean, bolShowFilePreview As Boolean)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.o23Notepad = CType(e.Item.DataItem, BO.o23Notepad)
        If bolShowClueTip Then
            With dataItem("systemcolumn")
                .Text = "<a class='reczoom' title='Detail dokumentu' rel='clue_o23_record.aspx?pid=" & cRec.PID.ToString & "'>i</a>"
            End With
        End If
        If bolShowFilePreview Then
            With dataItem("systemcolumn")
                .Text += "<a href='fileupload_preview.aspx?prefix=o23&pid=" & cRec.PID.ToString & "' target='_blank'>Náhled</a>"
            End With
        End If
        With cRec
            If .o23IsDraft Then dataItem("systemcolumn").CssClass = "draft"
            If .o23IsEncrypted Then dataItem("systemcolumn").CssClass = "spy"
            If .o23LockedFlag > BO.o23LockedTypeENUM._NotSpecified Then dataItem("systemcolumn").CssClass = "locked"

            If .IsClosed Then
                dataItem.Font.Strikeout = True

            End If
            If .b02ID > 0 Then
                If .b02Color <> "" Then dataItem("systemcolumn").Style.Item("background-color") = .b02Color
            End If
        End With

    End Sub
    Public Shared Sub p41_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim cRec As BO.p41Project = CType(e.Item.DataItem, BO.p41Project)
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        If cRec.IsClosed Then dataItem.Font.Strikeout = True
        If cRec.p41IsDraft Then dataItem("systemcolumn").CssClass = "draft"

    End Sub
    
    Public Shared Sub j02_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim cRec As BO.j02Person = CType(e.Item.DataItem, BO.j02Person)
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

        If cRec.IsClosed Then dataItem.Font.Strikeout = True
        If Not cRec.j02IsIntraPerson Then
            dataItem.Font.Italic = True
        End If
    End Sub
    Public Shared Sub p28_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim cRec As BO.p28Contact = CType(e.Item.DataItem, BO.p28Contact)
        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

        If cRec.IsClosed Then dataItem.Font.Strikeout = True
        If cRec.p28IsDraft Then dataItem("systemcolumn").CssClass = "draft"

        If cRec.p28CompanyShortName > "" Then dataItem.ToolTip = cRec.p28CompanyName
    End Sub
    Public Shared Sub p31_grid_Handle_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs)
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.p31Worksheet = CType(e.Item.DataItem, BO.p31Worksheet)

        With cRec
            If .p31Date > Now Then dataItem("systemcolumn").CssClass = "future" 'záznam do budoucna vizuálně zvýrazňovat jako plán

            Select Case .p72ID_AfterTrimming
                Case BO.p72IdENUM.SkrytyOdpis, BO.p72IdENUM.ViditelnyOdpis, BO.p72IdENUM.ZahrnoutDoPausalu
                    dataItem("systemcolumn").CssClass = "corr_236"
                Case BO.p72IdENUM.Fakturovat
                    dataItem("systemcolumn").CssClass = "corr_4"
            End Select
            Select Case .p72ID_AfterApprove
                Case BO.p72IdENUM.Fakturovat : dataItem("systemcolumn").CssClass = "a14"
                Case BO.p72IdENUM.ZahrnoutDoPausalu : dataItem("systemcolumn").CssClass = "a16"
                Case BO.p72IdENUM.ViditelnyOdpis : dataItem("systemcolumn").CssClass = "a12"
                Case BO.p72IdENUM.SkrytyOdpis : dataItem("systemcolumn").CssClass = "a13"
                Case Else
                    If .p71ID = BO.p71IdENUM.Neschvaleno Then dataItem("systemcolumn").CssClass = "a20"

            End Select
            Select Case .p70ID
                Case BO.p70IdENUM.Vyfakturovano : dataItem("systemcolumn").CssClass = "a4"
                Case BO.p70IdENUM.ZahrnutoDoPausalu : dataItem("systemcolumn").CssClass = "a6"
                Case BO.p70IdENUM.ViditelnyOdpis : dataItem("systemcolumn").CssClass = "a2"
                Case BO.p70IdENUM.SkrytyOdpis : dataItem("systemcolumn").CssClass = "a3"
            End Select
            If .o23ID_First > 0 And .p49ID = 0 Then dataItem("systemcolumn").Text += "<img src='Images/attachment.png' width='12px' height='12px'/>"
            If .IsClosed Then dataItem.Font.Strikeout = True

            Select Case .p33ID
                Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                    If .p34IncomeStatementFlag = BO.p34IncomeStatementFlagENUM.Prijem Then
                        dataItem.ForeColor = Drawing.Color.Blue  'příjmy
                    Else
                        dataItem.ForeColor = Drawing.Color.Brown    'výdaje
                    End If
                    If .p49ID > 0 Then
                        If .o23ID_First = 0 Then dataItem("systemcolumn").Text += "<img src='Images/finplan.png' width='12px' height='12px'/>"
                        If .o23ID_First > 0 Then dataItem("systemcolumn").Text += "<img src='Images/finplan_attachment.png' width='12px' height='12px'/>"
                    End If
                Case BO.p33IdENUM.Kusovnik
                    dataItem.ForeColor = Drawing.Color.Green  'kusovník
                Case Else
            End Select

        End With
    End Sub
    Public Shared Sub RenderHeaderMenu(bolRecordIsClosed As Boolean, panMenuContainer As Panel, menu As Telerik.Web.UI.RadMenu)
        If bolRecordIsClosed Then

            panMenuContainer.BackColor = Drawing.Color.Black
            For i As Integer = 0 To menu.Items.Count - 1
                menu.Items(i).Style.Item("background") = "black !important"
                menu.Items(i).Style.Item("color") = "white !important"
            Next

        Else

            panMenuContainer.BackColor = Nothing
        End If
    End Sub
    Public Overloads Shared Sub RenderLevelLink(cmdLevelLink As HyperLink, strText As String, strURL As String, bolIsClosed As Boolean)
        With cmdLevelLink
            If strText = "" Then
                .Visible = False
                Return
            Else
                .Visible = True
            End If
            If bolIsClosed Then
                .Font.Strikeout = True

            Else

            End If
            If strText.Length > 40 Then
                .Text = Left(strText, 40) & "..."
                .ToolTip = strText
                .Style.Item("font-size") = "120%"
            Else
                .Text = strText
            End If

            .NavigateUrl = strURL
        End With

    End Sub
    Public Overloads Shared Sub RenderLevelLink(menuLevel1 As Telerik.Web.UI.RadMenuItem, strText As String, strURL As String, bolIsClosed As Boolean)
        With menuLevel1
            .Font.Underline = True
            .Font.Bold = True
            If bolIsClosed Then
                .Font.Strikeout = True

            Else

            End If
            If strText.Length > 40 Then
                .Text = Left(strText, 40) & "..."
                .ToolTip = strText

            Else
                .Text = strText
            End If

            .NavigateUrl = strURL
        End With

    End Sub

    Public Shared Sub RenderQueryCombo(cbx As DropDownList)
        With cbx
            If .SelectedIndex > 0 Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    Public Shared Sub RenderQuickqueryLink(cmdClueQuickQuery As HyperLink, intQuickQueryValue As Integer, intBinQueryValue As Integer)
        With cmdClueQuickQuery
            If intQuickQueryValue > 0 Then
                .BackColor = Drawing.Color.Red
                .ForeColor = Drawing.Color.White
            Else
                .BackColor = Nothing
                .ForeColor = Nothing
            End If
            Select Case intBinQueryValue
                Case 1
                    .Text = "<img src='Images/query.png'/>Filtr<img src='Images/ok.png'/>"
                Case 2
                    .Text = "<img src='Images/query.png'/>Filtr<img src='Images/bin.png'/>"
                Case Else
                    .Text = "<img src='Images/query.png'/>Filtr"
            End Select
        End With
    End Sub

    Public Shared Sub Handle_SaveDropboxAccessToken(masterPage As Site)
        Dim lastAccessToken As BO.DropboxUserToken = masterPage.Factory.j03UserBL.GetMyDropboxAccessToken()

        Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh", lastAccessToken.Token, lastAccessToken.Secret, Nothing)

        Dim login As DropNet.Models.UserLogin = c.GetAccessToken()

        masterPage.Session.Item("DropBoxLogin") = login
    End Sub

    Public Shared Function QueryProjectListByTop10(factory As BL.Factory, intJ02ID As Integer, lisBasis As IEnumerable(Of BO.p41Project)) As IEnumerable(Of BO.p41Project)
        'vybere z projektů TOP 10 podle naposledy zapisovaných úkonů
        Dim mqP31 As New BO.myQueryP31
        mqP31.j02ID = intJ02ID
        mqP31.TopRecordsOnly = 100
        mqP31.MG_SortString = "p31dateinsert desc"
        Dim lisP31 As IEnumerable(Of BO.p31Worksheet) = factory.p31WorksheetBL.GetList(mqP31)
        Dim p41ids As New List(Of Integer)
        If lisP31.Count > 0 Then
            If lisP31.Select(Function(p) p.p41ID).Distinct.Count > 10 Then
                For Each c In lisP31
                    If p41ids.Where(Function(p) p = c.p41ID).Count = 0 Then
                        p41ids.Add(c.p41ID)
                    End If
                    If p41ids.Count >= 10 Then Exit For
                Next
            Else
                p41ids = lisP31.Select(Function(p) p.p41ID).Distinct.ToList
            End If
        Else
            p41ids.Add(-1)
        End If
        Dim mqP41 As New BO.myQueryP41
        mqP41.PIDs = p41ids
        mqP41.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
        Return factory.p41ProjectBL.GetList(mqP41)
    End Function
End Class
