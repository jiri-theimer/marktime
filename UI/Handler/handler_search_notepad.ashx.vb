Imports System.Web
Imports System.Web.Services

Public Class NotepadValue
    Public Property Name As String
    Public Property Project As String
    Public Property Client As String
    Public Property Code As String
    Public Property Owner As String
    Public Property PID As String
    Public Property Closed As String = "0"
    Public Property Draft As String = "0"
    Public Property FilterString As String
End Class
Public Class handler_search_notepad
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
        
        Dim strFilterString As String = context.Request.Item("term")

        Dim mq As New BO.myQueryO23
        mq.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead
        mq.SearchExpression = strFilterString
        mq.Closed = BO.BooleanQueryMode.FalseQuery

        mq.TopRecordsOnly = 40
        Dim lisO23 As IEnumerable(Of BO.o23NotepadGrid) = factory.o23NotepadBL.GetList4Grid(mq)

        Dim lis As New List(Of NotepadValue)
        For Each cO23 In lisO23
            Dim c As New NotepadValue
            With cO23
                c.Name = .o24Name & ": " & .o23Name
                c.Owner = .Owner
                c.Project = .Project & ""
                c.Client = .p28Name & ""
                c.Code = .o23Code
                c.PID = .PID.ToString
                If .IsClosed Then c.Closed = "1"
                If .o23IsDraft Then c.Draft = "1"
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