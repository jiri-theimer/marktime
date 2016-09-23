Public Class j03_myprofile
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Sub j03_myprofile_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "j03_myprofile"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Můj profil v MARKTIME"
                .SiteMenuValue = "cmdMyProfile"
                Dim lis As New List(Of String)
                lis.Add("handler_search_project-toprecs")
                lis.Add("handler_search_project-bin")
                .Factory.j03UserBL.InhaleUserParams(lis)

                page2.Visible = .Factory.SysUser.j04IsMenu_Project
                tabs1.FindTabByValue("search").Visible = page2.Visible
            End With


            RefreshRecord()
            With Master.Factory.j03UserBL
                basUI.SelectDropdownlistValue(Me.ShowTopFoundRecords, .GetUserParam("handler_search_project-toprecs", "10"))
                chkSearchInBin.Checked = BO.BAS.BG(.GetUserParam("handler_search_project-bin"))
            End With

            If Master.Factory.j03UserBL.GetMyTag(True) = "1" Then
                Master.Notify("Osobní profil byl aktualizován.", NotifyLevel.InfoMessage)
            End If

        End If
    End Sub

    Private Sub RefreshRecord()

        Dim cRec As BO.j03User = Master.Factory.j03UserBL.Load(Master.Factory.SysUser.PID)
        With cRec
            Me.j04Name.Text = .j04Name
            Me.j03login.Text = .j03Login
            Me.FullName.Text = .Person
            Me.j03IsLiveChatSupport.Checked = .j03IsLiveChatSupport
            Me.j03IsSiteMenuOnClick.Checked = .j03IsSiteMenuOnClick
            If .j03PasswordExpiration Is Nothing Then
                Me.j03PasswordExpiration.Text = "Heslo bez časové expirace"
            Else
                Me.j03PasswordExpiration.Text = BO.BAS.FD(.j03PasswordExpiration, , True)
            End If

            basUI.SelectDropdownlistValue(Me.j03SiteMenuSkin, .j03SiteMenuSkin)
        End With
        If cRec.j02ID <> 0 Then
            Me.panJ02Update.Visible = True
            Dim cJ02 As BO.j02Person = Master.Factory.j02PersonBL.Load(cRec.j02ID)
            With cJ02
                If .IsClosed Then Me.FullName.Font.Strikeout = True : Me.FullName.ToolTip = "Osobní profil byl přesunut do archivu."
                Me.j02clue.Attributes("rel") = "clue_j02_record.aspx?pid=" & .PID.ToString
                Me.j02Email.Text = .j02Email
                Me.j02Mobile.Text = .j02Mobile
                Me.j02Phone.Text = .j02Phone
                Me.j02Office.Text = .j02Office
                Me.j02EmailSignature.Text = .j02EmailSignature
            End With
        Else
            Me.panJ02Update.Visible = False
        End If
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.Factory.SysUser.j02ID)
        If cRec.IsClosed Then
            Master.Notify("Osobní profil byl přesunut do archivu, nemůžete ho aktualizovat.", NotifyLevel.ErrorMessage)
            Return
        End If
        With cRec
            .j02Email = Me.j02Email.Text
            .j02Mobile = Me.j02Mobile.Text
            .j02Phone = Me.j02Phone.Text
            .j02Office = Me.j02Office.Text
            .j02EmailSignature = Me.j02EmailSignature.Text
        End With
        If Master.Factory.j02PersonBL.Save(cRec, Nothing) Then
            Dim cJ03 As BO.j03User = Master.Factory.j03UserBL.Load(Master.Factory.SysUser.PID)
            With cJ03
                .j03IsLiveChatSupport = Me.j03IsLiveChatSupport.Checked
                .j03IsSiteMenuOnClick = Me.j03IsSiteMenuOnClick.Checked
                .j03SiteMenuSkin = Me.j03SiteMenuSkin.SelectedValue
            End With

            If Not Master.Factory.j03UserBL.Save(cJ03) Then
                Master.Notify(Master.Factory.j03UserBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Return
            End If
            Master.Factory.j03UserBL.SetMyTag("1")
            Response.Redirect("j03_myprofile.aspx")
        Else
            Master.Notify(Master.Factory.j02PersonBL.ErrorMessage, NotifyLevel.ErrorMessage)
        End If
    End Sub

    Private Sub cmdRefreshOnBehind_Click(sender As Object, e As EventArgs) Handles cmdRefreshOnBehind.Click
        If Me.hidHardRefreshFlag.Value = "send-mail" Then
            Master.Notify("Poštovní zpráva byla odeslána.", NotifyLevel.InfoMessage)
        End If
        hidHardRefreshFlag.Value = ""
        hidHardRefreshPID.Value = ""
    End Sub

    ''Private Sub SetupGrid()
    ''    With grid1
    ''        .PageSize = 20
    ''        .radGridOrig.ShowFooter = False

    ''        .AddSystemColumn(20, "UserInsert")
    ''        .AddColumn("DateUpdate", "Čas", BO.cfENUM.DateTime)
    ''        .AddColumn("x40State", "Stav")
    ''        .AddColumn("x40SenderName", "Odesílatel")
    ''        .AddColumn("x40Recipient", "Příjemce")
    ''        .AddColumn("x40Subject", "Předmět zprávy")
    ''        .AddColumn("x40WhenProceeded", "Zpracováno", BO.cfENUM.DateTime)
    ''        .AddColumn("x40ErrorMessage", "Chyba")
    ''    End With
    ''End Sub

    ''Private Sub grid1_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grid1.ItemDataBound
    ''    basUIMT.x40_grid_Handle_ItemDataBound(sender, e)
    ''End Sub

    ''Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
    ''    Dim lis As IEnumerable(Of BO.x40MailQueue) = Master.Factory.x40MailQueueBL.GetList_AllHisMessages(Master.Factory.SysUser.PID, Master.Factory.SysUser.j02ID)
    ''    grid1.DataSource = lis
    ''End Sub

  
    Private Sub cmdSaveSearch_Click(sender As Object, e As EventArgs) Handles cmdSaveSearch.Click
        With Master.Factory.j03UserBL
            .SetUserParam("handler_search_project-toprecs", Me.ShowTopFoundRecords.SelectedValue)
            .SetUserParam("handler_search_project-bin", BO.BAS.GB(Me.chkSearchInBin.Checked))
            .SetMyTag("1")
            Response.Redirect("j03_myprofile.aspx")
        End With
        

    End Sub

    Private Sub cmdDeleteUserParams_Click(sender As Object, e As EventArgs) Handles cmdDeleteUserParams.Click
        If Master.Factory.j03UserBL.DeleteAllUserParams(Master.Factory.SysUser.PID) Then
            Master.Notify("Paměť uživatelského profilu byla vyčištěna.")
        End If
    End Sub
End Class