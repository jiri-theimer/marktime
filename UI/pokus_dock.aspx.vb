Public Class pokus_dock
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs)

        'set properties according to configuration panel
        FileExplorer1.Configuration.SearchPatterns = New String() {"*.jpg", "*.jpeg", "*.gif", "*.png", "*.pdf"}
        
       
        If Not IsPostBack Then
            'Set initial folder to open. Note that the path is case sensitive!
            FileExplorer1.InitialPath = Page.ResolveUrl("~/public/images")
        End If
    End Sub
   

End Class