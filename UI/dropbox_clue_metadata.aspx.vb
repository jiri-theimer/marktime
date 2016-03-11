Public Class dropbox_clue_metadata
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub dropbox_clue_metadata_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim strPrefix As String = Request.Item("prefix")
            If Session.Item("DropBoxLogin") Is Nothing Then

                Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")

                Dim login As DropNet.Models.UserLogin = c.GetToken()
                Master.Factory.j03UserBL.SetMyDropboxAccessToken(login.Token, login.Secret)

                Dim strURL As String = c.BuildAuthorizeUrl(Context.Request.Url.GetLeftPart(UriPartial.Authority) & "/" & strPrefix & "_framework.aspx")

                Response.Write("<script>window.open('" & strURL & "','_top');</script>")
            Else
                Dim strPath As String = Request.Item("path")
                If strPath = "" Then Master.StopPage("path missing")
                Me.HeaderPath.Text = strPath
                Me.HeaderPath.NavigateUrl = "https://www.dropbox.com/home" & strPath
                Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")
                Dim login As DropNet.Models.UserLogin = Session.Item("DropBoxLogin")
                c.UserLogin = login
                Dim md As DropNet.Models.MetaData = Nothing
                Try
                    md = c.GetMetaData(strPath)
                Catch ex As Exception
                    Master.StopPage(ex.Message, True)
                End Try


                rp1.DataSource = md.Contents.OrderByDescending(Function(p) p.Is_Dir)
                rp1.DataBind()
                If rp1.Items.Count = 0 Then
                    Master.StopPage("Složka je prázdná", False)

                End If
            End If
        End If
        
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As DropNet.Models.MetaData = CType(e.Item.DataItem, DropNet.Models.MetaData)

        e.Item.FindControl("Folder").Visible = cRec.Is_Dir
        e.Item.FindControl("File").Visible = Not cRec.Is_Dir
        If cRec.Is_Dir Then
            With CType(e.Item.FindControl("Folder"), HyperLink)
                .Text = cRec.Name
                .NavigateUrl = "https://www.dropbox.com/home" & cRec.Path

            End With
        Else
            With CType(e.Item.FindControl("File"), Label)
                .Text = cRec.Name
            End With
            With CType(e.Item.FindControl("Size"), Label)
                .Text = cRec.Size
            End With
        End If


       
        With CType(e.Item.FindControl("img1"), Image)
            .ImageUrl = "Images/Dropbox/" & cRec.Icon & ".gif"

        End With
        CType(e.Item.FindControl("Modified"), Label).Text = BO.BAS.FD(cRec.ModifiedDate, True, True)

    End Sub
End Class