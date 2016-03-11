Imports System.Web
Imports System.Web.Services

Public Class TaskValue
    Public Property Name As String
    Public Property Project As String
    Public Property Client As String
    Public Property PID As String
    Public Property Closed As String = "0"
    Public Property FilterString As String
End Class
Public Class handler_search_task
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
        factory.j03UserBL.InhaleUserParams("handler_search_task-toprecs", "handler_search_task-bin")

        Dim strFilterString As String = context.Request.Item("term")

        Dim mq As New BO.myQueryP56
        mq.SpecificQuery = BO.myQueryP56_SpecificQuery.AllowedForRead
        mq.SearchExpression = strFilterString
        If factory.j03UserBL.GetUserParam("handler_search_task-bin", "1") = "1" Then
            mq.Closed = BO.BooleanQueryMode.NoQuery
        End If

        mq.TopRecordsOnly = BO.BAS.IsNullInt(factory.j03UserBL.GetUserParam("handler_search_task-toprecs", "40"))
        Dim lisP56 As IEnumerable(Of BO.p56Task) = factory.p56TaskBL.GetList(mq)

        Dim lis As New List(Of TaskValue)
        For Each cP56 In lisP56
            Dim c As New TaskValue
            With cP56
                If .p57IsHelpdesk Then
                    c.Name = .p56Name & " (" & .p56Code & ")"
                Else
                    c.Name = .p57Name & ": " & .p56Name
                End If
                c.Project = .ProjectCodeAndName
                c.Client = .Client
                c.PID = .PID.ToString
                If .IsClosed Then c.Closed = "1"
            End With
            

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