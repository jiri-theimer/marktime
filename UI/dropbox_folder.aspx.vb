Public Class dropbox_folder
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Class BredItem
        Public Property Index As Integer
        Public Property Path As String
        Public Property Name As String

    End Class
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
    Public ReadOnly Property CurrentO25ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidO25ID.Value)
        End Get

    End Property

    Private Sub dropbox_folder_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.hidX29ID.Value = Request.Item("x29id")
            Me.hidRecordPID.Value = Request.Item("pid")
            Me.hidO25ID.Value = Request.Item("o25id")
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If Session.Item("DropBoxLogin") Is Nothing Then
                    .StopPage("Dropbox uživatel není načten.")
                End If
                .HeaderText = "Dropbox složka | " & .Factory.GetRecordCaption(Me.CurrentX29ID, Me.CurrentRecordPID)
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
                If Me.CurrentO25ID <> 0 Then
                    .AddToolbarButton("Kompletně odstranit složku", "delete", 1, "Images/delete.png")
                    .AddToolbarButton("Odstranit vazbu na složku", "cut", 1, "Images/cut.png")

                End If
            End With


            RefreshRecord()
        End If
    End Sub

    'Private Sub InhaleAccessToken()
    '    If Session.Item("DropBoxLogin") Is Nothing Then

    '        Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")

    '        Dim login As DropNet.Models.UserLogin = c.GetToken()
    '        Master.Factory.j03UserBL.SetMyDropboxAccessToken(login.Token, login.Secret)

    '        Dim strURL As String = c.BuildAuthorizeUrl(Context.Request.Url.GetLeftPart(UriPartial.Authority) & "/dropbox_folder.aspx")
    '        Response.Redirect(strURL, True)
    '        'Response.Write("<script>window.open('" & strURL & "','_self');</script>")
    '    Else

    '        Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")
    '        Dim login As DropNet.Models.UserLogin = Session.Item("DropBoxLogin")
    '        c.UserLogin = login
    '        'Dim md As DropNet.Models.MetaData = c.GetMetaData(strPath)


    '        'rp1.DataSource = md.Contents.OrderByDescending(Function(p) p.Is_Dir)
    '        'rp1.DataBind()
    '    End If
    'End Sub

    Private Sub RefreshRecord()
        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Me.CurrentRecordPID)
                Me.txtNewFolder.Text = cRec.p41Name
            Case BO.x29IdEnum.p28Contact
                Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Me.CurrentRecordPID)
                Me.txtNewFolder.Text = cRec.p28Name
            Case BO.x29IdEnum.j02Person
                Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Me.CurrentRecordPID)
                Me.txtNewFolder.Text = cRec.FullNameDesc
            Case BO.x29IdEnum.o23Doc
                Me.txtNewFolder.Text = "Dropbox složka"
        End Select
        Me.txtNewFolder.Text = Replace(Me.txtNewFolder.Text, ".", "_")
        Me.txtNewFolder.Text = Replace(Me.txtNewFolder.Text, ",", "_")
        If Me.CurrentO25ID <> 0 Then
            Dim cRec As BO.o25DmsBinding = Master.Factory.o25DmsBindingBL.Load(Me.CurrentO25ID)
            Me.SelectedFolder.Text = cRec.o25Path
            ShowFolders(cRec.o25Path)
        End If
    End Sub

    Private Sub ShowFolders(strPath As String)
        If strPath = "/" Then strPath = ""
        Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")
        Dim login As DropNet.Models.UserLogin = Session.Item("DropBoxLogin")
        c.UserLogin = login
        Dim md As DropNet.Models.MetaData = Nothing
        Try
            md = c.GetMetaData(strPath)
        Catch ex As Exception
            Master.Notify(ex.Message, NotifyLevel.ErrorMessage)
            If strPath <> "" Then ShowFolders("")
            Return
        End Try
        rp1.DataSource = md.Contents.Where(Function(p) p.Is_Dir = True).OrderBy(Function(p) p.Name)
        rp1.DataBind()

        Dim a() As String = Split(strPath, "/"), lis As New List(Of BredItem), strPart As String = ""
        For i As Integer = 0 To UBound(a)
            Dim item As New BredItem
            item.Index = i
            strPart += "/" & a(i)
            item.Path = Replace(strPart, "//", "/")
            item.Name = a(i)
            If item.Name = "" Then item.Name = "ROOT"
            lis.Add(item)
        Next
        rpBred.DataSource = lis
        rpBred.DataBind()

    End Sub

    Private Sub dropbox_folder_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.CurrentO25ID <> 0 Then
            Me.opgMode.SelectedValue = "3"
            Me.opgMode.Visible = False
        End If
        Select Case Me.opgMode.SelectedValue
            Case "1"
                panFolders.Visible = False
                Me.lblNewFolder.Visible = True
                Me.txtNewFolder.Visible = True
                lblSelectedFolder.Text = "Nadřízená složka:"
                Me.SelectedFolder.Text = "ROOT"
            Case "2"
                panFolders.Visible = True
                FolderHeader.Text = "Vyberte nadřízenou složku"
                lblSelectedFolder.Text = "Nadřízená složka:"
                Me.lblNewFolder.Visible = True
                Me.txtNewFolder.Visible = True
            Case "3"
                panFolders.Visible = True
                FolderHeader.Text = "Vyberte složku"
                lblSelectedFolder.Text = "Vybraná složka:"
                Me.lblNewFolder.Visible = False
                Me.txtNewFolder.Visible = False
        End Select

    End Sub

    Private Sub rp1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rp1.ItemCommand
        If e.CommandName = "folder" Then
            ShowFolders(e.CommandArgument)
        End If
        If e.CommandName = "select" Then
            Me.SelectedFolder.Text = CType(e.Item.FindControl("cmdFolder"), LinkButton).CommandArgument
        End If
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As DropNet.Models.MetaData = CType(e.Item.DataItem, DropNet.Models.MetaData)

        With CType(e.Item.FindControl("cmdFolder"), LinkButton)
            .Text = cRec.Name
            .CommandArgument = cRec.Path
            .CommandName = "folder"
        End With

        With CType(e.Item.FindControl("img1"), Image)
            .ImageUrl = "Images/Dropbox/" & cRec.Icon & ".gif"

        End With
        With CType(e.Item.FindControl("clue_content"), HyperLink)
            .Attributes("rel") = "dropbox_clue_metadata.aspx?path=" & cRec.Path
        End With
    End Sub

    Private Sub opgMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles opgMode.SelectedIndexChanged
        If Me.opgMode.SelectedValue <> "1" Then
            ShowFolders("")
        Else
            rp1.DataSource = Nothing
            rp1.DataBind()
        End If

    End Sub

    Private Sub rpBred_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpBred.ItemCommand
        ShowFolders(e.CommandArgument)
    End Sub

   

    Private Sub rpBred_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpBred.ItemDataBound
        Dim cRec As BredItem = CType(e.Item.DataItem, BredItem)

        With CType(e.Item.FindControl("cmdFolder"), LinkButton)
            .Text = cRec.Name
            .CommandArgument = cRec.Path
            .CommandName = "folder"
        End With

        
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Dim strSelectedFolder As String = ""
            Select Case Me.opgMode.SelectedValue
                Case "1"
                    strSelectedFolder = CreateFolder()
                    
                Case "2"
                    If Me.SelectedFolder.Text = "" Or Me.SelectedFolder.Text = "ROOT" Then
                        Master.Notify("Musíte vybrat nadřízenou složku.", NotifyLevel.WarningMessage)
                        Return
                    End If
                    strSelectedFolder = CreateFolder()
                Case "3"
                    strSelectedFolder = Me.SelectedFolder.Text
                    If strSelectedFolder = "" Then
                        Master.Notify("Musíte vybrat složku.", NotifyLevel.WarningMessage)
                        Return
                    End If
            End Select
            If strSelectedFolder <> "" Then
                Dim cRec As New BO.o25DmsBinding
                If Me.CurrentO25ID <> 0 Then cRec = Master.Factory.o25DmsBindingBL.Load(Me.CurrentO25ID)
                cRec.o25DmsApp = BO.o25DmsAppENUM.Dropbox
                cRec.o25DmsObject = BO.o25DmsObjectENUM.Folder
                cRec.o25Path = strSelectedFolder
                cRec.o25Icon = "folder"
                cRec.x29ID = Me.CurrentX29ID
                cRec.o25RecordPID = Me.CurrentRecordPID
                If Master.Factory.o25DmsBindingBL.Save(cRec) Then
                    Master.CloseAndRefreshParent()
                End If


            End If
        End If
        If strButtonValue = "cut" Then
            If Master.Factory.o25DmsBindingBL.Delete(Me.CurrentO25ID) Then
                Master.CloseAndRefreshParent()
            End If
        End If
        If strButtonValue = "delete" Then
            Dim cRec As BO.o25DmsBinding = Master.Factory.o25DmsBindingBL.Load(Me.CurrentO25ID)
            If DeleteFolder(cRec.o25Path) <> "" Then
                If Master.Factory.o25DmsBindingBL.Delete(Me.CurrentO25ID) Then
                    Master.CloseAndRefreshParent()
                End If
            End If
        End If
        
    End Sub

    Private Function CreateFolder() As String
        Dim strParentFolder As String = Me.SelectedFolder.Text
        If strParentFolder = "ROOT" Then strParentFolder = "/"
        If Trim(Me.txtNewFolder.Text) = "" Then
            Master.Notify("Chybí název složky", NotifyLevel.WarningMessage)
            Return ""
        End If
        Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")
        Dim login As DropNet.Models.UserLogin = Session.Item("DropBoxLogin")
        c.UserLogin = login
        Try
            Return c.CreateFolder(strParentFolder & "/" & Me.txtNewFolder.Text).Path
        Catch ex As Exception
            Master.Notify(ex.Message, NotifyLevel.ErrorMessage)
            Return ""
        End Try


    End Function
    Private Function DeleteFolder(strPath As String) As String
        Dim c As New DropNet.DropNetClient("exjpken3uxh45kw", "yq8gd0alxsul0qh")
        Dim login As DropNet.Models.UserLogin = Session.Item("DropBoxLogin")
        c.UserLogin = login
        Try
            Return c.Delete(strPath).Path
        Catch ex As Exception
            Master.Notify(ex.Message, NotifyLevel.ErrorMessage)
            Return ""
        End Try


    End Function
End Class