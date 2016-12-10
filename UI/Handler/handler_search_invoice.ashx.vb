Imports System.Web
Imports System.Web.Services

Public Class SearchInvoice
    Public Property Invoice As String
    Public Property p91Text1 As String
    Public Property PID As String
    Public Property Closed As String = "0"
    Public Property FilterString As String
    Public Property Draft As String = "0"
End Class

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
        Dim strP28ID As String = context.Request.Item("p28id")
        Dim mq As New BO.myQueryP91
        mq.SearchExpression = strFilterString
        If strP28ID <> "" Then
            mq.p28ID = CInt(strP28ID)
        End If
        'If factory.j03UserBL.GetUserParam("handler_search_project-bin", "0") = "1" Then
        '    mq.Closed = BO.BooleanQueryMode.NoQuery
        'End If

        mq.TopRecordsOnly = 10
        Dim lisP91 As IEnumerable(Of BO.p91Invoice) = factory.p91InvoiceBL.GetList(mq)
        Dim lis As New List(Of SearchInvoice)
        Dim c As New SearchInvoice
        Select Case lisP91.Count
            Case 0
                c.Invoice = "Ani jedna faktura pro zadanou podmínku."
            Case Is >= mq.TopRecordsOnly
                c.Invoice = String.Format("Nalezeno více než {0} faktur.<br>Je třeba zpřesnit hledání nebo si zvýšit počet vypisovaných faktur.", mq.TopRecordsOnly.ToString)
            Case Else
                c.Invoice = String.Format("Počet nalezených faktur: {0}.", lisP91.Count.ToString)
        End Select
        c.FilterString = strFilterString : lis.Add(c)
        For Each cP91 In lisP91
            c = New SearchInvoice
            With cP91
                c.Invoice = .p91Code & " - "
                If .p91Client = "" Then
                    c.Invoice += " " & .p28Name
                Else
                    c.Invoice += " " & .p91Client
                End If
                c.Invoice += " (" & BO.BAS.FN(.p91Amount_TotalDue) & " " & .j27Code & ")"
                c.p91Text1 = .p91Text1
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