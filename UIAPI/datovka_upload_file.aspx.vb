Imports System.Web
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json


Public Class datovka_upload_file
    Inherits System.Web.UI.Page


    Public Class FileRequest
        Public Property ids As List(Of String)
        Public Property file_name As String
        Public Property file_content As String
    End Class



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ContentType = "application/json"
        Request.ContentType = "application/json"



        Dim cF As New BO.clsFile
        cF.SaveText2File("c:\temp\request.txt", "Request.ContentLength=" & Request.ContentLength)
        Dim s As String = Encoding.Default.GetString(Request.BinaryRead(Request.ContentLength))


        cF.SaveText2File("c:\temp\request_full.txt", s)
        

        Dim serializer As New JavaScriptSerializer()
        Dim c As New FileRequest

        c = JsonConvert.DeserializeObject(Of FileRequest)(s)

        cF.SaveText2File("c:\temp\hovado.txt", c.ids(0))
        cF.SaveText2File("c:\temp\hovado_pokus.zfo", c.file_content, , , False)
    End Sub

End Class