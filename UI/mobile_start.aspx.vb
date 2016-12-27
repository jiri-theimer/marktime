Public Class mobile_start
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile

    Private Sub mobile_start_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.RecordHeader.Text = Master.Factory.SysUser.Person
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("p41_framework_detail-pid")
                .Add("p28_framework_detail-pid")
                .Add("p91_framework_detail-pid")
            End With
            Master.MenuPrefix = "home"
            With Master.Factory
                .j03UserBL.InhaleUserParams(lisPars)

                linkEntryWorksheet.Visible = .SysUser.j04IsMenu_Worksheet
                If .TestPermission(BO.x53PermValEnum.GR_X31_Personal) Then
                    linkPersonalReports.NavigateUrl = "javascript:rp('mobile_report.aspx?prefix=j02&pid=" & .SysUser.j02ID.ToString & "')"

                End If

                Dim intPID As Integer = BO.BAS.IsNullInt(.j03UserBL.GetUserParam("p41_framework_detail-pid", "O"))
                If intPID <> 0 Then
                    linkLastProject.Text = "<img src='Images/project.png' /> " & .GetRecordCaption(BO.x29IdEnum.p41Project, intPID)
                Else
                    linkLastProject.Visible = False
                End If
                intPID = BO.BAS.IsNullInt(.j03UserBL.GetUserParam("p28_framework_detail-pid", "O"))
                If intPID <> 0 Then                   
                    linkLastClient.Text = "<img src='Images/contact.png' /> " & .GetRecordCaption(BO.x29IdEnum.p28Contact, intPID)
                Else
                    linkLastClient.Visible = False
                End If
             
            End With
            If Request.Item("w") <> "" And Request.Item("h") <> "" Then
                basUI.Write2AccessLog(Master.Factory, True, Request, Request.Item("w"), Request.Item("h"))
            End If
            RefreshRecord()
            RefreshTasks()
            

        End If
    End Sub
    Private Sub RefreshTasks()
        Dim lis As IEnumerable(Of BO.p56Task) = Master.Factory.p56TaskBL.GetList_forMessagesDashboard(Master.Factory.SysUser.j02ID)
        rp1.DataSource = lis
        rp1.DataBind()
        If lis.Count > 0 Then
            panP56.Visible = True
            CountP56.Text = lis.Count.ToString
        Else
            panP56.Visible = False
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim c As BO.p31Worksheet = Master.Factory.p31WorksheetBL.LoadMyLastCreated(False, 0)
        If c Is Nothing Then
            Me.LastWorksheet.Text = "Zatím jsem nezapsal WORKSHEET úkon."
        Else
            Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(c.p41ID)
            With c
                Me.LastWorksheet.Text = BO.BAS.FD(.p31Date) & "/" & .ClientName & "/" & cP41.PrefferedName & "/" & c.p32Name
                Me.LastWorksheet.NavigateUrl = "mobile_p31_framework.aspx?source=calendar&pid=" & c.PID.ToString
                Me.LastWorksheet.ToolTip = c.p31Text
            End With
        End If
    End Sub

End Class