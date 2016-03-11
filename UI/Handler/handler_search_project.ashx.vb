Imports System.Web
Imports System.Web.Services

Public Class NameValue
    Public Property Project As String
    Public Property PID As String
    Public Property Closed As String = "0"
    Public Property FilterString As String
    Public Property Draft As String = "0"
End Class
Public Class handler_search_project
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest


        context.Response.ContentType = "application/json"

        Dim factory As BL.Factory = Nothing
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            context.Response.Write(" ")
            Return
        End If
        factory.j03UserBL.InhaleUserParams("handler_search_project-toprecs", "handler_search_project-bin")

        'context.Response.Write("Hello World!")
        Dim strFilterString As String = context.Request.Item("term")
        
        Dim mq As New BO.myQueryP41
        mq.SearchExpression = strFilterString
        If factory.j03UserBL.GetUserParam("handler_search_project-bin", "0") = "1" Then
            mq.Closed = BO.BooleanQueryMode.NoQuery
        End If

        mq.TopRecordsOnly = BO.BAS.IsNullInt(factory.j03UserBL.GetUserParam("handler_search_project-toprecs", "20"))
        Dim lisP41 As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mq)

        Dim lis As New List(Of NameValue)
        For Each cP41 In lisP41
            Dim c As New NameValue

            c.Project = cP41.FullName & " [" & cP41.p41Code & "]"

            c.PID = cP41.PID.ToString
            If cP41.IsClosed Then c.Closed = "1"
            If cP41.p41IsDraft Then c.Draft = "1"

            c.FilterString = strFilterString

            lis.Add(c)
        Next



        Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim s As String = jss.Serialize(lis)
        context.Response.Write(s)

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class