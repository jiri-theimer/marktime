Imports System.Web
Imports System.Web.Services

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
        Dim lis As New List(Of BO.SearchBoxItem)
        Dim c As New BO.SearchBoxItem
        Select Case lisO23.Count
            Case 0
                c.ItemText = "Ani jeden dokument pro zadanou podmínku."
            Case Is >= mq.TopRecordsOnly
                c.ItemText = String.Format("Nalezeno více než {0} dokumentů.<br>Je třeba zpřesnit podmínku hledání.", mq.TopRecordsOnly.ToString)
            Case Else
                c.ItemText = String.Format("Počet nalezených dokumentů: {0}.", lisO23.Count.ToString)
        End Select
        c.FilterString = strFilterString : lis.Add(c)

        For Each cO23 In lisO23
            c = New BO.SearchBoxItem
            With cO23
                c.ItemText = .o24Name & ": " & .o23Name & " (" & .Owner & ")"

                c.ItemComment = .p28Name & " | " & .Project
               
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