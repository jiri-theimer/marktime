Public Class dropbox
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory

    Public ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return CType(BO.BAS.IsNullInt(Me.hidX29ID.Value), BO.x29IdEnum)
        End Get
      
    End Property
    Public ReadOnly Property CurrentRecordPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidRecordPID.Value)
        End Get
       
    End Property

    Public Sub RefreshData(x29ID As BO.x29IdEnum, intRecordPID As Integer)
        Me.hidX29ID.Value = CInt(x29ID).ToString
        Me.hidRecordPID.Value = intRecordPID.ToString
        Dim lis As IEnumerable(Of BO.o25DmsBinding) = Me.Factory.o25DmsBindingBL.GetList(BO.o25DmsAppENUM.Dropbox, x29ID, intRecordPID)
        rp1.DataSource = lis
        rp1.DataBind()

        Select Case x29ID
            Case BO.x29IdEnum.p41Project
                cmdFolder.Text = "Přidat projektovou složku"
            Case BO.x29IdEnum.p28Contact
                cmdFolder.Text = "Přidat složku kontaktu"
            Case BO.x29IdEnum.j02Person
                cmdFolder.Text = "Přidat osobní složku"
            Case BO.x29IdEnum.o23Doc
                cmdFolder.Text = "Přidat složku"
        End Select
        cmdInhaleAccessToken.Text = cmdFolder.Text
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("DropBoxLogin") Is Nothing Then
            cmdFolder.Visible = False
        Else
            cmdFolder.Visible = True
        End If
        cmdInhaleAccessToken.Visible = Not cmdFolder.Visible
    End Sub

    Private Sub rp1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rp1.ItemCommand
        InhaleAccessToken()
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.o25DmsBinding = CType(e.Item.DataItem, BO.o25DmsBinding)
        With CType(e.Item.FindControl("img1"), Image)
            If cRec.o25Icon = "" Then
                .Visible = False
            Else
                .ImageUrl = "Images/Dropbox/" & cRec.o25Icon & ".gif"
            End If
        End With
        With CType(e.Item.FindControl("o25Path"), HyperLink)
            .Text = cRec.o25Path
            Select Case cRec.o25DmsObject
                Case BO.o25DmsObjectENUM.Folder
                    .NavigateUrl = Me.rootURL.NavigateUrl & cRec.o25Path
                Case BO.o25DmsObjectENUM.Link
                    .NavigateUrl = cRec.o25Path
            End Select

        End With
        With CType(e.Item.FindControl("clue_content"), HyperLink)
            .Attributes("rel") = "dropbox_clue_metadata.aspx?prefix=" & BO.BAS.GetDataPrefix(Me.CurrentX29ID) & "&path=" & cRec.o25Path
        End With
        With CType(e.Item.FindControl("cmdEdit"), HyperLink)
            If Session.Item("DropBoxLogin") Is Nothing Then
                .Visible = False
            Else
                .NavigateUrl = "javascript:dropbox_folder(" & cRec.PID.ToString & ")"
            End If
        End With
        With CType(e.Item.FindControl("cmdInhaleAccessToken"), LinkButton)
            If Not Session.Item("DropBoxLogin") Is Nothing Then
                .Visible = False
            Else
                .CommandArgument = cRec.PID.ToString
            End If
        End With
    End Sub

    
    
    
    Private Sub cmdInhaleAccessToken_Click(sender As Object, e As EventArgs) Handles cmdInhaleAccessToken.Click
        InhaleAccessToken()
    End Sub

    Private Sub InhaleAccessToken()
        If Session.Item("DropBoxLogin") Is Nothing Then

            Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")

            Dim login As DropNet.Models.UserLogin = c.GetToken()
            Me.Factory.j03UserBL.SetMyDropboxAccessToken(login.Token, login.Secret)

            Dim strURL As String = c.BuildAuthorizeUrl(Context.Request.Url.GetLeftPart(UriPartial.Authority) & "/" & BO.BAS.GetDataPrefix(Me.CurrentX29ID) & "_framework.aspx")
            Response.Write("<script>window.open('" & strURL & "','_top');</script>")

        End If
    End Sub
End Class