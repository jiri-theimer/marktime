Imports System.Web
Imports System.Web.Services

Public Class handler_popupmenu
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

        Dim strPREFIX As String = Trim(context.Request.Item("prefix"))
        Dim intPID As Integer = BO.BAS.IsNullInt(Trim(context.Request.Item("pid")))
        Dim strPAGE As String = Trim(context.Request.Item("page"))

        Dim lis As New List(Of String)
        For i As Integer = 0 To 10
            lis.Add("<menuitem label='ITEM " & i.ToString & " for prefix [" & strPREFIX & "]' onclick='alert(" & i.ToString & ")' title='Im a hint'></menuitem>")
        Next
        context.Response.Write(String.Join("", lis))

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class