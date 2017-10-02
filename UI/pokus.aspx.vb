

Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

'Imports Aspose.Words




Public Class pokus
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    'Private fileFormatProvider As IFormatProvider



    Private Sub pokus_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master


    End Sub


   
    Private Sub pokus_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    
    

    Private Sub cmdFolders_Click(sender As Object, e As EventArgs) Handles cmdFolders.Click

        'System.IO.Directory.Move("c:\temp\hovado", "c:\temp\beast")

        Dim cF As New BO.clsFile

        If System.IO.File.Exists("c:\temp\beast\MTInfo.txt") Then
            System.IO.File.SetAttributes("c:\temp\beast\MTInfo.txt", IO.FileAttributes.Normal)
        End If
        If Not cF.SaveText2File("c:\temp\beast\MTInfo.txt", "Byl jsem tu " & Now.ToString, False, , False) Then
            Master.Notify(cF.ErrorMessage)
        End If


        System.IO.File.SetAttributes("c:\temp\beast\MTInfo.txt", IO.FileAttributes.Hidden)

        Dim lis As New List(Of String)
        lis.Add("MtHovadoNesmysl")
        cF.CreateDirectoryWithSecurity("c:\temp\hovado2", lis, True, False)
      
    End Sub

    Private Sub cmdRemoveCookie_Click(sender As Object, e As EventArgs) Handles cmdRemoveCookie.Click
        Request.Cookies.Remove("MT50-CultureInfo")
    End Sub
End Class