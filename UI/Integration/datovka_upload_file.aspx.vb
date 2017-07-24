Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Serialization

Public Class datovka_upload_file
    Inherits System.Web.UI.Page


    Public Class FileRequest
        Public Property ids As List(Of String)
        Public Property file_name As String
        Public Property file_content As String
    End Class



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ContentType = "application/json"


    End Sub

End Class