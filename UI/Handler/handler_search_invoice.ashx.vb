Imports System.Web
Imports System.Web.Services

Public Class handler_search_invoice
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
        'factory.j03UserBL.InhaleUserParams("handler_search_p-toprecs", "handler_search_project-bin")

        'context.Response.Write("Hello World!")
        Dim strFilterString As String = context.Request.Item("term")

        Dim mq As New BO.myQueryP91
        mq.SearchExpression = strFilterString
        'If factory.j03UserBL.GetUserParam("handler_search_project-bin", "0") = "1" Then
        '    mq.Closed = BO.BooleanQueryMode.NoQuery
        'End If

        mq.TopRecordsOnly = 10
        Dim lisP91 As IEnumerable(Of BO.p91Invoice) = factory.p91InvoiceBL.GetList(mq)

        Dim lis As New List(Of NameValue)
        For Each cP91 In lisP91
            Dim c As New NameValue
            With cP91
                c.Project = .p91Code & " - " & .p28Name & " (" & BO.BAS.FN(.p91Amount_TotalDue) & " " & .j27Code & ")"

                c.PID = .PID.ToString
                If .IsClosed Then c.Closed = "1"
                If .p91IsDraft Then c.Draft = "1"
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