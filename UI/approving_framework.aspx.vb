Imports Telerik.Web.UI
Public Class approving_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _needFilterIsChanged As Boolean = False

    Public Property CurrentPrefix As String
        Get
            Return Me.hidCurPrefix.Value
        End Get
        Set(value As String)
            Me.hidCurPrefix.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.hidCurPrefix.Value)
        End Get
    End Property
    Public Property CurrentJ70ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.j70ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.j70ID, value.ToString)
        End Set
    End Property
    Private Sub approving_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Schvalování | Příprava podkladů k fakturaci"
                .SiteMenuValue = "cmdP31_Approving"

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("approving_framework-prefix")
                    .Add("approving_framework-pagesize")
                    .Add("approving_framework-scope")
                    .Add("approving_framework-groupby-p41")
                    .Add("approving_framework-groupby-j02")
                    .Add("approving_framework-groupby-p28")
                    .Add("approving_framework-kusovnik")
                    .Add("periodcombo-custom_query")
                    .Add("p31_grid-period")
                    .Add("approving_framework-filter_setting-p41")
                    .Add("approving_framework-filter_sql-p41")
                    .Add("approving_framework-filter_setting-p28")
                    .Add("approving_framework-filter_sql-p28")
                    .Add("approving_framework-filter_setting-j02")
                    .Add("approving_framework-filter_sql-j02")
                    .Add("approving_framework-j70id-p41")
                    .Add("approving_framework-j70id-p28")
                    .Add("approving_framework-j70id-j02")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("p31_grid-period")
                    If Request.Item("prefix") = "" Then
                        Me.CurrentPrefix = .GetUserParam("approving_framework-prefix", "p41")
                    Else
                        Me.CurrentPrefix = Request.Item("prefix")
                    End If
                    SetupJ70Combo(BO.BAS.IsNullInt(.GetUserParam("approving_framework-j70id-" & Me.CurrentPrefix)))
                    Me.tabs1.FindTabByValue(Me.CurrentPrefix).Selected = True
                    basUI.SelectDropdownlistValue(Me.cbxPaging, .GetUserParam("approving_framework-pagesize", "50"))
                    basUI.SelectDropdownlistValue(Me.cbxScope, .GetUserParam("approving_framework-scope", "1"))
                    basUI.SelectDropdownlistValue(Me.cbxGroupBy, .GetUserParam("approving_framework-groupby-" & Me.CurrentPrefix, ""))
                    Me.chkKusovnik.Checked = BO.BAS.BG(.GetUserParam("approving_framework-kusovnik", "0"))
                End With
                Select Case Me.CurrentX29ID
                    Case BO.x29IdEnum.p28Contact
                        Me.cbxGroupBy.Items.FindByValue("Client").Enabled = False
                    Case BO.x29IdEnum.j02Person
                        Me.cbxGroupBy.Items.FindByValue("Client").Enabled = False
                        menu1.FindItemByValue("bin").Visible = False
                End Select
                menu1.FindItemByValue("export").Visible = .Factory.TestPermission(BO.x53PermValEnum.GR_ExportGrid)
            End With
            With Master.Factory.j03UserBL
                SetupGrid(.GetUserParam("approving_framework-filter_setting-" + Me.CurrentPrefix), .GetUserParam("approving_framework-filter_sql-" + Me.CurrentPrefix))
            End With

        End If
    End Sub

    Private Sub SetupJ70Combo(intDef As Integer)
        Dim mq As New BO.myQuery
        j70ID.DataSource = Master.Factory.j70QueryTemplateBL.GetList(mq, Me.CurrentX29ID)
        j70ID.DataBind()
        j70ID.Items.Insert(0, "--Pojmenovaný filtr--")
        basUI.SelectDropdownlistValue(Me.j70ID, intDef.ToString)
        With Me.j70ID
            If .SelectedIndex > 0 Then
                .ToolTip = .SelectedItem.Text
                Me.clue_query.Attributes("rel") = "clue_quickquery.aspx?j70id=" & .SelectedValue
            Else
                Me.clue_query.Visible = False
            End If
        End With
    End Sub
    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With grid1
            .ClearColumns()
            .AllowMultiSelect = True
            .AddCheckboxSelector()
            .PageSize = BO.BAS.IsNullInt(Me.cbxPaging.SelectedValue)

            Select Case Me.CurrentX29ID
                Case BO.x29IdEnum.p41Project
                    .AddSystemColumn(10)
                    If Me.cbxGroupBy.SelectedValue <> "Client" Then
                        .AddColumn("Client", "Klient")
                    End If

                    .AddColumn("Project", "Projekt")

                Case BO.x29IdEnum.p28Contact
                    .AddSystemColumn(10)
                    .AddColumn("Client", "Klient")
                Case BO.x29IdEnum.j02Person
                    .AddColumn("Person", "Osoba")
            End Select
            If Me.cbxGroupBy.SelectedValue <> "j27Code" Then
                .AddColumn("j27Code", "")
            End If

            If Me.cbxScope.SelectedValue = "1" Then
                .AddColumn("rozpracovano_hodiny", "Hodiny", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("rozpracovano_honorar", "Honorář", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("rozpracovano_vydaje", "Výdaje", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("rozpracovano_odmeny", "Fixní odměny", BO.cfENUM.Numeric2, , , , , True)
                If Me.chkKusovnik.Checked Then
                    .AddColumn("rozpracovano_kusovnik_honorar", "Honorář z kusovníku", BO.cfENUM.Numeric2, , , , , True)
                End If
                .AddColumn("rozpracovano_prvni", "První", BO.cfENUM.DateOnly)
                .AddColumn("rozpracovano_posledni", "Poslední", BO.cfENUM.DateOnly)
                .AddColumn("rozpracovano_pocet", "Počet", BO.cfENUM.Numeric0, , , , , True)
            End If
            If Me.cbxScope.SelectedValue = "2" Then
                .AddColumn("schvaleno_hodiny_fakturovat", "Hodiny k fakturaci", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("schvaleno_honorar_fakturovat", "Honorář", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("schvaleno_vydaje_fakturovat", "Výdaje", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("schvaleno_odmeny_fakturovat", "Fixní odměny", BO.cfENUM.Numeric2, , , , , True)
                If Me.chkKusovnik.Checked Then
                    .AddColumn("schvaleno_kusovnik_honorar", "Honorář z kusovníku", BO.cfENUM.Numeric2, , , , , True)
                End If
                .AddColumn("schvaleno_hodiny_pausal", "Hodiny v paušálu", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("schvaleno_hodiny_odpis", "Odepsané hodiny", BO.cfENUM.Numeric2, , , , , True)
                .AddColumn("schvaleno_prvni", "První", BO.cfENUM.DateOnly)
                .AddColumn("schvaleno_posledni", "Poslední", BO.cfENUM.DateOnly)
                .AddColumn("schvaleno_pocet", "Počet", BO.cfENUM.Numeric0, , , , , True)
            End If
            .SetFilterSetting(strFilterSetting, strFilterExpression)
        End With
        With Me.cbxGroupBy.SelectedItem
            SetupGrouping(.Value, .Text)
        End With

        With grid1.radGridOrig.ClientSettings.Scrolling
            .AllowScroll = True
            '.UseStaticHeaders = True
            Dim intHeight As Integer = Request.Browser.ScreenPixelsHeight * 2 - 100
            intHeight = intHeight - 110
            .ScrollHeight = Unit.Parse(intHeight.ToString & "px")
        End With

    End Sub

    Private Sub SetupGrouping(strGroupField As String, strFieldHeader As String)
        With grid1.radGridOrig.MasterTableView
            .GroupByExpressions.Clear()
            If strGroupField = "" Then Return
            .ShowGroupFooter = True
            Dim GGE As New GridGroupByExpression
            Dim fld As New GridGroupByField
            fld.FieldName = strGroupField
            fld.HeaderText = strFieldHeader

            GGE.SelectFields.Add(fld)
            GGE.GroupByFields.Add(fld)

            .GroupByExpressions.Add(GGE)
        End With
    End Sub

    Private Sub RefreshRecord()

    End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        If Not TypeOf e.Item Is GridDataItem Then Return

        Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        Dim cRec As BO.ApprovingFramework = CType(e.Item.DataItem, BO.ApprovingFramework)
        If cbxScope.SelectedValue = "1" Then
            If Not cRec.rozpracovano_odmeny Is Nothing Then
                dataItem("rozpracovano_odmeny").ForeColor = Drawing.Color.Blue
            End If
            If Not cRec.rozpracovano_vydaje Is Nothing Then
                dataItem("rozpracovano_vydaje").ForeColor = Drawing.Color.Brown
            End If
        End If
        If cbxScope.SelectedValue = "2" Then
            If Not cRec.schvaleno_odmeny_fakturovat Is Nothing Then
                dataItem("schvaleno_odmeny_fakturovat").ForeColor = Drawing.Color.Blue
            End If
            If Not cRec.schvaleno_vydaje_fakturovat Is Nothing Then
                dataItem("schvaleno_vydaje_fakturovat").ForeColor = Drawing.Color.Brown
            End If
        End If
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                dataItem("systemcolumn").Text = "<a href='p41_framework.aspx?pid=" & cRec.PID.ToString & "'><img src='Images/fullscreen.png'/></a>"
            Case BO.x29IdEnum.p28Contact
                dataItem("systemcolumn").Text = "<a href='p28_framework.aspx?pid=" & cRec.PID.ToString & "'><img src='Images/fullscreen.png'/></a>"
        End Select


    End Sub

    Private Sub grid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("approving_framework-filter_setting-" + Me.CurrentPrefix, grid1.GetFilterSetting())
                .SetUserParam("approving_framework-filter_sql-" + Me.CurrentPrefix, grid1.GetFilterExpression())
            End With
        End If

        Dim mq As New BO.myQueryP31
        If Me.cbxScope.SelectedValue = "1" Then
            mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForDoApprove
        End If
        If Me.cbxScope.SelectedValue = "2" Then
            mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForReApprove
        End If
        mq.DateFrom = period1.DateFrom
        mq.DateUntil = period1.DateUntil



        Dim lis As IEnumerable(Of BO.ApprovingFramework) = Master.Factory.p31WorksheetBL.GetList_ApprovingFramework(Me.CurrentX29ID, mq, Me.CurrentJ70ID)

        grid1.DataSource = lis
    End Sub

    Private Sub approving_framework_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        basUIMT.RenderQueryCombo(Me.j70ID)
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
        If Me.cbxScope.SelectedValue = "1" Then
            lblHeader.Text = "Schvalovat"
        Else
            lblHeader.Text = "Fakturovat"
        End If
    End Sub

    
    Private Sub ReloadPage()
        Response.Redirect("approving_framework.aspx")
    End Sub

    

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", Me.period1.SelectedValue)
        grid1.Rebind(False)
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("approving_framework-pagesize", Me.cbxPaging.SelectedValue)
        ReloadPage()
    End Sub

    Private Sub cmdHardRefreshOnBehind_Click(sender As Object, e As EventArgs) Handles cmdHardRefreshOnBehind.Click
        Select Case Me.hidHardRefreshFlag.Value
            Case "pdf"
                With grid1.radGridOrig.ExportSettings.Pdf
                    .PageWidth = Unit.Parse("297mm")
                    .PageHeight = Unit.Parse("210mm")
                End With
                grid1.radGridOrig.ExportToPdf()
            Case "xls"
                grid1.radGridOrig.MasterTableView.ExportToExcel()
            Case "doc"
                grid1.radGridOrig.MasterTableView.ExportToWord()
            Case "j70-run"
                ReloadPage()
            Case Else
                grid1.Rebind(False)
        End Select


        Me.hidHardRefreshFlag.Value = ""
    End Sub

    Private Sub cbxScope_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxScope.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("approving_framework-scope", Me.cbxScope.SelectedValue)
        ReloadPage()

    End Sub

    Private Sub cbxGroupBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGroupBy.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("approving_framework-groupby-" & Me.CurrentPrefix, Me.cbxGroupBy.SelectedValue)
        ReloadPage()

    End Sub

    Private Sub chkKusovnik_CheckedChanged(sender As Object, e As EventArgs) Handles chkKusovnik.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("approving_framework-kusovnik", BO.BAS.GB(Me.chkKusovnik.Checked))
        ReloadPage()

    End Sub

    Private Sub j70ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles j70ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("approving_framework-j70id-" & Me.CurrentPrefix, j70ID.SelectedValue)
        ReloadPage()
    End Sub
End Class