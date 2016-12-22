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
        End If
    End Sub

End Class