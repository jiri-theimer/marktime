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
                lblHeader.Text = cRec.j02FirstName & ", vítejte v systému"
            End If

            ShowImage()

            With Master.Factory
                menu1.FindItemByValue("p31_create").Visible = .SysUser.j04IsMenu_Worksheet
                menu1.FindItemByValue("p31_create").Visible = .SysUser.j04IsMenu_Worksheet
                menu1.FindItemByValue("p41_create").Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)
                menu1.FindItemByValue("p91_create").Visible = .TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator)
                menu1.FindItemByValue("p28_create").Visible = .TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator)
                menu1.FindItemByValue("o23_create").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator, BO.x53PermValEnum.GR_O23_Draft_Creator)
                menu1.FindItemByValue("p56_create").Visible = .TestPermission(BO.x53PermValEnum.GR_P56_Creator)
                menu1.FindItemByValue("admin").Visible = .SysUser.IsAdmin
                menu1.FindItemByValue("approve").Visible = .SysUser.IsApprovingPerson
                menu1.FindItemByValue("report").Visible = .SysUser.j04IsMenu_Report
            End With

            RefreshBoxes()
        End If
    End Sub

    Private Sub ShowImage()
        If Request.Item("image") <> "" Then
            imgWelcome.ImageUrl = "Images/Welcome/" & Request.Item("image")
            Return
        End If
        Dim cF As New BO.clsFile
        Dim lisFiles As List(Of String) = cF.GetFileListFromDir(BO.ASS.GetApplicationRootFolder & "\Images\Welcome", "*.*")
        Dim intCount As Integer = lisFiles.Count - 1

        Randomize()
        Dim x As Integer = CInt(Rnd() * 100), strPreffered As String = ""
        'If x > intCount Then x = CInt(Rnd() * 100)

        If x > intCount And Now.Hour > 18 Then strPreffered = "19422837_s.jpg" 'rodina
        If x > intCount And Now.Hour > 19 Then strPreffered = "10694994_s.jpg" 'pivo
        If x > intCount And (Now.Hour > 22 Or Now.Hour <= 6) Then strPreffered = "16805586_s.jpg" 'postel
        If x > intCount And Now.Hour >= 12 And Now.Hour <= 13 Then strPreffered = "7001764_s.jpg" 'oběd
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
End Class