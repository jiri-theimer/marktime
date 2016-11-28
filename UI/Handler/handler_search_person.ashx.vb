Imports System.Web
Imports System.Web.Services

Public Class handler_search_person
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "text/plain"

        Dim factory As BL.Factory = Nothing
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            context.Response.Write(" ")
            Return
        End If
     
        Dim strFilterString As String = context.Request.Item("term"), strFO As String = context.Request.Item("fo")

        Dim mq As New BO.myQueryJ02
        ''mq.IntraPersons = BO.myQueryJ02_IntraPersons.IntraOnly
        mq.IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified

        mq.Closed = BO.BooleanQueryMode.NoQuery
        Select Case strFO
            Case "j02LastName"
                mq.ColumnFilteringExpression = "a.j02LastName LIKE '%" & strFilterString & "%'"    'hledat pouze podle příjmení
            Case "j02Email"
                mq.ColumnFilteringExpression = "a.j02Email LIKE '%" & strFilterString & "%'"    'hledat pouze podle mailu             
            Case Else
                mq.SearchExpression = strFilterString
        End Select

        mq.TopRecordsOnly = 10
        Dim lisJ02 As IEnumerable(Of BO.j02Person) = factory.j02PersonBL.GetList(mq)

        Dim lis As New List(Of NameValue)
        For Each cJ02 In lisJ02
            Dim c As New NameValue
            With cJ02
                c.Project = .FullNameDesc
                If cJ02.j02IsIntraPerson Then
                    If .j07ID <> 0 Then c.Project += " [" & .j07Name & "]"
                Else
                    If .j02Email <> "" Then c.Project += " [" & cJ02.j02Email & "]"
                    c.Project = "<i>" & c.Project & "</i>"
                End If

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