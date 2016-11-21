Public Class j03_mypage_greeting
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            lblBuild.Text = "Verze: " & BO.ASS.GetUIVersion() & " | .NET framework: " & BO.ASS.GetFrameworkVersion() & " | <a href='http://www.marktime.cz' target='_blank'>www.marktime.cz</a>"

            Master.SiteMenuValue = "dashboard"
            If Master.Factory.SysUser.j02ID > 0 Then
                Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.Factory.SysUser.j02ID)
                lblHeader.Text = cRec.j02FirstName & ", vítejte v"
            End If

            Dim lisPars As New List(Of String)
            With lisPars
                .Add("j03_mypage_greeting-last_step")
                .Add("j03_mypage_greeting-chkP41")
                .Add("j03_mypage_greeting-chkP28")
                .Add("j03_mypage_greeting-chkP91")
                .Add("j03_mypage_greeting-chkO23")
                .Add("j03_mypage_greeting-chkP56")
                .Add("j03_mypage_greeting-chkShowCharts")
                .Add("j03_mypage_greeting-chkSearch")
            End With

            With Master.Factory
                cmdReadUpgradeInfo.Visible = .SysUser.j03IsShallReadUpgradeInfo

                .j03UserBL.InhaleUserParams(lisPars)
                chkP41.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkP41", "1"))
                chkP28.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkP28", "1"))
                chkP91.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkP91", "0"))
                chkO23.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkO23", "0"))
                chkP56.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkP56", "0"))
                chkSearch.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkSearch", "0"))
                chkShowCharts.Checked = BO.BAS.BG(.j03UserBL.GetUserParam("j03_mypage_greeting-chkShowCharts", "1"))

                menu1.FindItemByValue("p31_create").Visible = .SysUser.j04IsMenu_Worksheet
                menu1.FindItemByValue("p31_create").Visible = .SysUser.j04IsMenu_Worksheet
                menu1.FindItemByValue("p31_scheduler").Visible = .SysUser.j04IsMenu_Worksheet
                menu1.FindItemByValue("myreport").Visible = .TestPermission(BO.x53PermValEnum.GR_X31_Personal)
                ''If .SysUser.j04IsMenu_Invoice Then
                ''    menu1.FindItemByValue("p91_create").Visible = .TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator)
                ''End If

                ''menu1.FindItemByValue("p28_create").Visible = .TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator)
                ''menu1.FindItemByValue("o23_create").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator)
                menu1.FindItemByValue("o10_create").Visible = .TestPermission(BO.x53PermValEnum.GR_O10_Creator)

                menu1.FindItemByValue("p56_create").Visible = False
                If .TestPermission(BO.x53PermValEnum.GR_P56_Creator) Then
                    menu1.FindItemByValue("p56_create").Visible = True
                Else
                    Dim lis As IEnumerable(Of BO.x67EntityRole) = .j02PersonBL.GetList_AllAssignedEntityRoles(.SysUser.j02ID, BO.x29IdEnum.p41Project)
                    For Each c In lis
                        If .x67EntityRoleBL.GetList_BoundX53(c.PID).Where(Function(p) p.x53Value = BO.x53PermValEnum.PR_P56_Creator).Count > 0 Then
                            menu1.FindItemByValue("p56_create").Visible = True : Exit For
                        End If
                    Next
                End If
                menu1.FindItemByValue("admin").Visible = .SysUser.IsAdmin
                menu1.FindItemByValue("approve").Visible = .SysUser.IsApprovingPerson
                menu1.FindItemByValue("report").Visible = .SysUser.j04IsMenu_Report

                panSearch_j02.Visible = .SysUser.j04IsMenu_People
                panSearch_p28.Visible = .SysUser.j04IsMenu_Contact
                panSearch_p91.Visible = .SysUser.j04IsMenu_Invoice

                If .SysUser.j04IsMenu_More Then
                    If menu1.FindItemByValue("p56_create").Visible Then
                        If Master.Factory.p56TaskBL.GetTotalTasksCount() > 20 Then
                            panSearch_p56.Visible = True
                        End If
                    End If
                End If
                panSearch.Visible = (panSearch_j02.Visible Or panSearch_p28.Visible Or panSearch_p91.Visible Or panSearch_p56.Visible)

                If panSearch.Visible Then
                    If Not Me.chkSearch.Checked Then
                        panSearch_j02.Visible = False : panSearch_p28.Visible = False : panSearch_p91.Visible = False : panSearch_p56.Visible = False
                    End If

                End If

                If .SysUser.j04IsMenu_Project Then
                    RefreshFavourites()
                End If
            End With

            RefreshNoticeBoard()
            If rpNoticeBoard.Items.Count <= 1 Then  'pokud je na nástěnce 1 nebo žádný článek, pak zobrazovat grafy obrázky
                With Master.Factory.j03UserBL
                    Select Case .GetUserParam("j03_mypage_greeting-last_step", "0")
                        Case "0"
                            ShowImage()
                            .SetUserParam("j03_mypage_greeting-last_step", "1")
                        Case "1"
                            ShowChart1("1")
                            .SetUserParam("j03_mypage_greeting-last_step", "2")
                        Case "2"
                            ShowChart2("2")
                            .SetUserParam("j03_mypage_greeting-last_step", "3")
                        Case "3"
                            ShowChart2("3")
                            .SetUserParam("j03_mypage_greeting-last_step", "4")
                        Case "4"
                            ShowChart2("4")
                            .SetUserParam("j03_mypage_greeting-last_step", "5")
                        Case "5"
                            ShowChart1("3")
                            .SetUserParam("j03_mypage_greeting-last_step", "6")
                        Case "6"
                            ShowChart2("5")
                            .SetUserParam("j03_mypage_greeting-last_step", "0")
                    End Select

                End With
            Else
                chkShowCharts.Visible = False
            End If

            RefreshBoxes()

            RefreshX47Log()
            'If basUI.GetCookieValue(Request, "MT50-SAW") = "1" Then
            '    menu1.Visible = False
            'End If
        End If
    End Sub

    Private Sub ShowImage()
        imgWelcome.Visible = True
        If Request.Item("image") <> "" Then
            imgWelcome.ImageUrl = "Images/Welcome/" & Request.Item("image")
            Return
        End If
        Dim cF As New BO.clsFile
        Dim lisFiles As List(Of String) = cF.GetFileListFromDir(BO.ASS.GetApplicationRootFolder & "\Images\Welcome", "*.*")
        Dim intCount As Integer = lisFiles.Count - 1

        Randomize()
        Dim x As Integer = CInt(Rnd() * 110), strPreffered As String = ""
        If x > intCount Then x = intCount


        'If x > intCount And Now.Hour > 18 Then strPreffered = "19422837_s.jpg" 'rodina
        'If x > intCount And Now.Hour > 19 Then strPreffered = "10694994_s.jpg" 'pivo
        If x > intCount And (Now.Hour > 22 Or Now.Hour <= 6) Then strPreffered = "16805586_s.jpg" 'postel
        'If x > intCount And Now.Hour >= 12 And Now.Hour <= 13 Then strPreffered = "7001764_s.jpg" 'oběd
        If x > intCount And strPreffered = "" And (Weekday(Now, Microsoft.VisualBasic.FirstDayOfWeek.Monday) = 3 Or Weekday(Now, Microsoft.VisualBasic.FirstDayOfWeek.Monday) = 1) Then
            strPreffered = "work.jpng"
        End If
        If strPreffered <> "" Then
            If System.IO.File.Exists(BO.ASS.GetApplicationRootFolder & "\Images\Welcome\" & strPreffered) Then
                imgWelcome.ImageUrl = "Images/welcome/" & strPreffered
                Return
            End If
        End If
        If x >= 0 And x <= intCount Then
            imgWelcome.ImageUrl = "Images/welcome/" & lisFiles(x)
        End If

    End Sub

    Private Sub RefreshFavourites()
        Dim mq As New BO.myQueryP41
        mq.IsFavourite = BO.BooleanQueryMode.TrueQuery
        Dim lisP41 As IEnumerable(Of BO.p41Project) = Master.Factory.p41ProjectBL.GetList(mq)
        If lisP41.Count > 0 Then
            menu1.FindItemByValue("favourites").Visible = True
            With menu1.FindItemByValue("favourites").Items
                For Each c In lisP41
                    Dim link1 As New Telerik.Web.UI.RadPanelItem(c.FullName, "p41_framework.aspx?pid=" & c.PID.ToString)
                    link1.ImageUrl = "Images/project.png"
                    .Add(link1)
                Next
            End With
            menu1.FindItemByValue("favourites").Text += " (" & lisP41.Count.ToString & ")"
        End If
    End Sub

    Private Sub RefreshBoxes()

        Dim lisP56 As IEnumerable(Of BO.p56Task) = Master.Factory.p56TaskBL.GetList_forMessagesDashboard(Master.Factory.SysUser.j02ID)
        If lisP56.Count > 0 Then
            Me.panP56.Visible = True
            Me.p56Count.Text = lisP56.Count.ToString
            rpP56.DataSource = lisP56
            rpP56.DataBind()
        Else
            Me.panP56.Visible = False
        End If
        Dim lisO22 As IEnumerable(Of BO.o22Milestone) = Master.Factory.o22MilestoneBL.GetList_forMessagesDashboard(Master.Factory.SysUser.j02ID)
        If lisO22.Count > 0 Then
            Me.panO22.Visible = True
            Me.o22Count.Text = lisO22.Count.ToString
            rpO22.DataSource = lisO22
            rpO22.DataBind()
        Else
            Me.panO22.Visible = False
        End If
        Dim lisO23 As IEnumerable(Of BO.o23NotepadGrid) = Master.Factory.o23NotepadBL.GetList_forMessagesDashboard(Master.Factory.SysUser.j02ID)
        If lisO23.Count > 0 Then
            Me.panO23.Visible = True
            Me.o23Count.Text = lisO23.Count.ToString
            rpO23.DataSource = lisO23
            rpO23.DataBind()
        Else
            Me.panO23.Visible = False
        End If
        rpP39.DataSource = Master.Factory.p40WorkSheet_RecurrenceBL.GetList_forMessagesDashboard(Master.Factory.SysUser.j02ID)
        rpP39.DataBind()
        If rpP39.Items.Count = 0 Then
            panP39.Visible = False
        Else
            p39Count.Text = rpP39.Items.Count.ToString
            panP39.Visible = True
        End If

        Dim mqP48 As New BO.myQueryP48
        mqP48.j02ID_Owner = Master.Factory.SysUser.j02ID
        mqP48.DateFrom = Now.AddDays(-4)
        mqP48.DateUntil = Now.AddDays(1)

        Dim lisP48 As IEnumerable(Of BO.p48OperativePlan) = Master.Factory.p48OperativePlanBL.GetList(mqP48).Where(Function(p) p.p31ID = 0).OrderBy(Function(p) p.p48Date)
        If lisP48.Count > 0 Then
            Me.panP48.Visible = True
            Me.p48Count.Text = lisP48.Count.ToString
            rpP48.DataSource = lisP48
            rpP48.DataBind()
        Else
            Me.panP48.Visible = False
        End If
    End Sub

    

    Private Sub rpP56_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP56.ItemDataBound
        Dim cRec As BO.p56Task = CType(e.Item.DataItem, BO.p56Task)
        With CType(e.Item.FindControl("link1"), HyperLink)
            .Text = cRec.NameWithTypeAndCode
            .NavigateUrl = "p56_framework.aspx?pid=" & cRec.PID.ToString
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        With CType(e.Item.FindControl("clue1"), HyperLink)
            .Attributes.Item("rel") = "clue_p56_record.aspx?&pid=" & cRec.PID.ToString
        End With
        If Not cRec.p56ReminderDate Is Nothing Then
            e.Item.FindControl("img1").Visible = True
        Else
            e.Item.FindControl("img1").Visible = False
        End If
        If Not BO.BAS.IsNullDBDate(cRec.p56PlanUntil) Is Nothing Then
            With CType(e.Item.FindControl("p56PlanUntil"), Label)
                .Text = BO.BAS.FD(cRec.p56PlanUntil, True, True)
                If cRec.p56PlanUntil < Now Then
                    .Text += "...je po termínu!" : .ForeColor = Drawing.Color.Red
                Else
                    .ForeColor = Drawing.Color.Green
                End If
            End With

        End If
    End Sub

    Private Sub rpO22_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpO22.ItemDataBound
        Dim cRec As BO.o22Milestone = CType(e.Item.DataItem, BO.o22Milestone)
        With CType(e.Item.FindControl("link1"), HyperLink)
            .Text = cRec.NameWithDate
            .NavigateUrl = "dr.aspx?prefix=o22&pid=" & cRec.PID.ToString
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        With CType(e.Item.FindControl("clue1"), HyperLink)
            .Attributes.Item("rel") = "clue_o22_record.aspx?&pid=" & cRec.PID.ToString
        End With
        If Not cRec.o22ReminderDate Is Nothing Then
            e.Item.FindControl("img1").Visible = True
        Else
            e.Item.FindControl("img1").Visible = False
        End If
        
    End Sub

    Private Sub rpO23_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpO23.ItemDataBound
        Dim cRec As BO.o23NotepadGrid = CType(e.Item.DataItem, BO.o23NotepadGrid)
        With CType(e.Item.FindControl("link1"), HyperLink)
            .Text = cRec.o24Name & ": "
            If cRec.o23Name <> "" Then
                .Text += cRec.o23Name
            Else
                .Text += cRec.ProjectClient
            End If
            .NavigateUrl = "o23_framework.aspx?pid=" & cRec.PID.ToString
            If cRec.IsClosed Then .Font.Strikeout = True

        End With
        With CType(e.Item.FindControl("clue1"), HyperLink)
            .Attributes.Item("rel") = "clue_o23_record.aspx?&pid=" & cRec.PID.ToString
        End With

        
    End Sub

    Private Sub rpP39_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP39.ItemDataBound
        Dim cRec As BO.p39WorkSheet_Recurrence_Plan = CType(e.Item.DataItem, BO.p39WorkSheet_Recurrence_Plan)
        With CType(e.Item.FindControl("cmdProject"), HyperLink)
            .Text = cRec.p41Name
            If cRec.p28Name <> "" Then .Text = cRec.p28Name & " - " & cRec.p41Name
            .NavigateUrl = "p41_framework.aspx?pid=" & cRec.p41ID.ToString
        End With
        CType(e.Item.FindControl("p39DateCreate"), Label).Text = BO.BAS.FD(cRec.p39DateCreate, True)
        CType(e.Item.FindControl("p39Date"), Label).Text = BO.BAS.FD(cRec.p39Date)
        CType(e.Item.FindControl("p39Text"), Label).Text = cRec.p39Text

    End Sub

    Private Sub rpP48_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP48.ItemDataBound
        Dim cRec As BO.p48OperativePlan = CType(e.Item.DataItem, BO.p48OperativePlan)
        With CType(e.Item.FindControl("link1"), HyperLink)
            .Text = cRec.ClientAndProject
            .NavigateUrl = "javascript:p48_record(" & cRec.PID.ToString & ")"
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        With CType(e.Item.FindControl("clue1"), HyperLink)
            .Attributes.Item("rel") = "clue_p48_record.aspx?&pid=" & cRec.PID.ToString
        End With
        With CType(e.Item.FindControl("p48Date"), Label)
            .Text = BO.BAS.FD(cRec.p48Date)
            If cRec.p48TimeFrom <> "" Then .Text += " " & cRec.p48TimeFrom & " - " & cRec.p48TimeUntil
        End With
        With CType(e.Item.FindControl("p48Hours"), Label)
            .Text = BO.BAS.FN(cRec.p48Hours) & "h."
        End With
        With CType(e.Item.FindControl("convert1"), HyperLink)
            .NavigateUrl = "javascript: p48_convert(" & cRec.PID.ToString & ")"
        End With
    End Sub

    Private Sub ShowChart1(strFlag As String)
        If Not chkShowCharts.Checked Then ShowImage() : Return
        Dim s As String = "select round(sum(case when b.p32IsBillable=1 THEN p31Hours_Orig end),2) as HodinyFa,round(sum(case when b.p32IsBillable=0 THEN p31Hours_Orig end),2) as HodinyNeFa,c11.c11DateFrom as Datum"
        s += " FROM (select c11DateFrom FROM c11StatPeriod WHERE c11Level=5 AND c11DateFrom between @d1 and @d2) c11 LEFT OUTER JOIN (select * from p31Worksheet where j02ID=@j02id and p31Date between @d1 and @d2) a ON c11.c11DateFrom=a.p31Date LEFT OUTER JOIN p32Activity b ON a.p32ID=b.p32ID"
        s += " WHERE c11.c11DateFrom BETWEEN @d1 AND @d2 GROUP BY c11.c11DateFrom ORDER BY c11.c11DateFrom"
        Dim pars As New List(Of BO.PluginDbParameter)
        If strFlag = "3" Then
            pars.Add(New BO.PluginDbParameter("d1", Today.AddDays(-30)))
        Else
            pars.Add(New BO.PluginDbParameter("d1", Today.AddDays(-14)))
        End If

        pars.Add(New BO.PluginDbParameter("d2", Today.AddDays(1)))
        pars.Add(New BO.PluginDbParameter("j02id", Master.Factory.SysUser.j02ID))
        Dim dt As DataTable = Master.Factory.pluginBL.GetDataTable(s, pars)
        Dim dbl As Double = 0
        For Each row As DataRow In dt.Rows
            dbl += BO.BAS.IsNullNum(row.Item("HodinyFa")) + BO.BAS.IsNullNum(row.Item("HodinyNeFa"))
        Next
        If dbl < 20 Then
            ShowImage()
            Return
        End If
        If strFlag = "3" Then
            panChart3.Visible = True
            With chart3
                .DataSource = dt
                .DataBind()
            End With
        Else
            panChart1.Visible = True
            With chart1
                .DataSource = dt
                .DataBind()
            End With
        End If



    End Sub

    Private Sub ShowChart2(strFlag As String)
        If Not chkShowCharts.Checked Then ShowImage() : Return

        Dim s As String = "select round(sum(p31Hours_Orig),2) as Hodiny,left(min(p28name),20) as Podle FROM p31Worksheet a INNER JOIN p41Project p41 ON a.p41ID=p41.p41ID INNER JOIN p32Activity p32 ON a.p32ID=p32.p32ID INNER JOIN p34ActivityGroup p34 ON p32.p34ID=p34.p34ID LEFT OUTER JOIN p28Contact p28 ON p41.p28ID_Client=p28.p28ID WHERE a.j02ID=@j02id AND p34.p33ID=1 AND a.p31Date BETWEEN @d1 AND @d2 GROUP BY p41.p28ID_Client ORDER BY min(p28Name)"
        Select Case strFlag
            Case "3"
                s = "select round(sum(p31Hours_Orig),2) as Hodiny,left(min(p32Name),20) as Podle FROM p31Worksheet a INNER JOIN p32Activity p32 ON a.p32ID=p32.p32ID INNER JOIN p34ActivityGroup p34 ON p32.p34ID=p34.p34ID WHERE a.j02ID=@j02id AND p34.p33ID=1 AND a.p31Date BETWEEN @d1 AND @d2 GROUP BY a.p32ID ORDER BY min(p32Name)"
            Case "4"
                s = "select round(sum(p31Hours_Orig),2) as Hodiny,left(min(p34Name),20) as Podle FROM p31Worksheet a INNER JOIN p32Activity p32 ON a.p32ID=p32.p32ID INNER JOIN p34ActivityGroup p34 ON p32.p34ID=p34.p34ID WHERE a.j02ID=@j02id AND p34.p33ID=1 AND a.p31Date BETWEEN @d1 AND @d2 GROUP BY p32.p34ID ORDER BY min(p34Name)"
            Case "5"
                s = "select round(sum(p31Hours_Orig),2) as Hodiny,left(min(isnull(p28name+' - ','')+p41Name),40) as Podle FROM p31Worksheet a INNER JOIN p41Project p41 ON a.p41ID=p41.p41ID INNER JOIN p32Activity p32 ON a.p32ID=p32.p32ID INNER JOIN p34ActivityGroup p34 ON p32.p34ID=p34.p34ID LEFT OUTER JOIN p28Contact p28 ON p41.p28ID_Client=p28.p28ID WHERE a.j02ID=@j02id AND p34.p33ID=1 AND a.p31Date BETWEEN @d1 AND @d2 GROUP BY a.p41ID ORDER BY min(p28Name),min(p41Name)"

        End Select
        Dim d0 As Date = Now
        If Day(Now) <= 2 Then d0 = Now.AddDays(-10)

        Dim d1 As Date = DateSerial(Year(d0), Month(d0), 1)
        Dim d2 As Date = d1.AddMonths(1).AddDays(-1)
        Dim pars As New List(Of BO.PluginDbParameter)
        pars.Add(New BO.PluginDbParameter("d1", d1))
        pars.Add(New BO.PluginDbParameter("d2", d2))
        pars.Add(New BO.PluginDbParameter("j02id", Master.Factory.SysUser.j02ID))
        Dim dt As DataTable = Master.Factory.pluginBL.GetDataTable(s, pars)
        If strFlag = "5" And dt.Rows.Count > 17 Then ShowChart2("") : Return 'nad 17 projektů->graf podle klientů
        If strFlag = "4" And dt.Rows.Count <= 1 Then ShowChart2("3") : Return 'pokud pracuje v jednom sešitě, pak graf nemá smysl

        If dt.Rows.Count <= 1 Then
            ShowImage()
            Return
        End If
        panChart2.Visible = True
        With chart2
            .ChartTitle.Text = "Měsíc " & Month(d0).ToString & "/" & Year(d0).ToString & ": " & BO.BAS.FN(dt.Compute("Sum(Hodiny)", "")) & "h."
            .DataSource = dt
            .DataBind()
        End With


    End Sub

    Private Sub RefreshNoticeBoard()
        Dim mq As New BO.myQuery
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        Dim lis As IEnumerable(Of BO.o10NoticeBoard) = Master.Factory.o10NoticeBoardBL.GetList(mq).Where(Function(p) p.o10Locality = BO.NoticeBoardLocality.WelcomePage)
        
        rpNoticeBoard.DataSource = lis
        rpNoticeBoard.DataBind()
    End Sub
    Private Sub RefreshX47Log()
        Dim mq As New BO.myQueryX47, x45ids As New List(Of String), b As Boolean = Master.Factory.TestPermission(BO.x53PermValEnum.GR_Admin)
        If b Then x45ids.Add("10201")
        With Master.Factory
            If (b Or .TestPermission(BO.x53PermValEnum.GR_P41_Reader)) And .SysUser.j04IsMenu_Project Then
                chkP41.Visible = True
                If chkP41.Checked Then x45ids.Add("14101")
            End If
            If b Or .TestPermission(BO.x53PermValEnum.GR_P28_Reader) And .SysUser.j04IsMenu_Contact Then
                chkP28.Visible = True
                If chkP28.Checked Then x45ids.Add("32801")
            End If
            If b Or .TestPermission(BO.x53PermValEnum.GR_P91_Reader) And .SysUser.j04IsMenu_Invoice Then
                chkP91.Visible = True
                If chkP91.Checked Then
                    x45ids.Add("39101")
                    x45ids.Add("39001")
                End If
            End If
            If b Then
                chkO23.Visible = True
                If chkO23.Checked Then x45ids.Add("22301")
                chkP56.Visible = True
                If chkP56.Checked Then x45ids.Add("35601")
            End If
        End With
        If x45ids.Count > 0 Then
            mq.x45IDs = String.Join(",", x45ids)
            Dim lis As IEnumerable(Of BO.x47EventLog) = Master.Factory.x47EventLogBL.GetList(mq, 20)    ''.Where(Function(p) p.x47Description = "")
            rpX47.DataSource = lis
            rpX47.DataBind()
        Else
            If chkP41.Visible Or chkP28.Visible Or chkP91.Visible Or chkO23.Visible Then
                rpX47.Visible = False
            Else
                panX47.Visible = False
            End If

        End If




    End Sub

    Private Sub rpX47_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpX47.ItemDataBound
        Dim cRec As BO.x47EventLog = CType(e.Item.DataItem, BO.x47EventLog), s As String = ""

        With CType(e.Item.FindControl("lbl1"), Label)
            Select Case cRec.x45ID
                Case BO.x45IDEnum.p41_new : s = "Images/project.png" : .Text = Resources.common.Projekt
                Case BO.x45IDEnum.p28_new : s = "Images/contact.png" : .Text = Resources.common.Klient
                Case BO.x45IDEnum.p91_new : s = "Images/invoice.png" : .Text = Resources.common.Faktura
                Case BO.x45IDEnum.j02_new : s = "Images/person.png" : .Text = Resources.common.Osoba
                Case BO.x45IDEnum.o23_new : s = "Images/notepad.png" : .Text = Resources.common.Dokument
                Case BO.x45IDEnum.p56_new : s = "Images/task.png" : .Text = Resources.common.Ukol
                Case Else
                    .Text = cRec.x45Name
            End Select
        End With
        If s = "" Then
            e.Item.FindControl("img1").Visible = False
        Else
            CType(e.Item.FindControl("img1"), Image).ImageUrl = s
            CType(e.Item.FindControl("link1"), HyperLink).NavigateUrl = BO.BAS.GetDataPrefix(cRec.x29ID) & "_framework.aspx?pid=" & cRec.x47RecordPID.ToString
            CType(e.Item.FindControl("lbl2"), Label).Text = BO.BAS.OM3(cRec.x47NameReference, 25)
        End If

        CType(e.Item.FindControl("timestamp"), Label).Text = cRec.Person & "/" & BO.BAS.FD(cRec.DateInsert, True, True)
        CType(e.Item.FindControl("link1"), HyperLink).Text = BO.BAS.OM3(cRec.x47Name, 40)

    End Sub

    Private Sub chkP41_CheckedChanged(sender As Object, e As EventArgs) Handles chkP41.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkP41", BO.BAS.GB(Me.chkP41.Checked))
        ReloadPage()
    End Sub

    Private Sub chkP91_CheckedChanged(sender As Object, e As EventArgs) Handles chkP91.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkP91", BO.BAS.GB(Me.chkP91.Checked))
        ReloadPage()
    End Sub

    Private Sub chkP28_CheckedChanged(sender As Object, e As EventArgs) Handles chkP28.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkP28", BO.BAS.GB(Me.chkP28.Checked))
        ReloadPage()
    End Sub

    Private Sub ReloadPage()
        Response.Redirect("j03_mypage_greeting.aspx")
    End Sub

    Private Sub chkO23_CheckedChanged(sender As Object, e As EventArgs) Handles chkO23.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkO23", BO.BAS.GB(Me.chkO23.Checked))
        ReloadPage()
    End Sub

    Private Sub chkShowCharts_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowCharts.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkShowCharts", BO.BAS.GB(Me.chkShowCharts.Checked))
        ReloadPage()
    End Sub

    Private Sub chkP56_CheckedChanged(sender As Object, e As EventArgs) Handles chkP56.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkP56", BO.BAS.GB(Me.chkP56.Checked))
        ReloadPage()
    End Sub

    Private Sub chkSearch_CheckedChanged(sender As Object, e As EventArgs) Handles chkSearch.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("j03_mypage_greeting-chkSearch", BO.BAS.GB(Me.chkSearch.Checked))
        ReloadPage()
    End Sub
End Class