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
            'Me.panSearchContact.Visible = Master.Factory.SysUser.j04IsMenu_Contact
            'Me.panSearchProject.Visible = Master.Factory.SysUser.j04IsMenu_Project
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



End Class