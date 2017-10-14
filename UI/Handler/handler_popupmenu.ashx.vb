Imports System.Web
Imports System.Web.Services

Public Class handler_popupmenu
    Implements System.Web.IHttpHandler
    Private _lis As New List(Of String)
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "text/plain"

        Dim factory As BL.Factory = Nothing
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            FinalRW(context, "factory is nothing") : Return
        End If

        Dim strPREFIX As String = Trim(context.Request.Item("prefix"))
        Dim intPID As Integer = BO.BAS.IsNullInt(Trim(context.Request.Item("pid")))
        Dim strPAGE As String = Trim(context.Request.Item("page"))

        If strPREFIX = "" Or intPID = 0 Then
            FinalRW(context, "prefix or pid is missing") : Return
        End If

        Select Case strPREFIX
            Case "p56"
                Dim cRec As BO.p56Task = factory.p56TaskBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return

                Dim cDisp As BO.p56RecordDisposition = factory.p56TaskBL.InhaleRecordDisposition(cRec)
                If Not cDisp.ReadAccess Then FinalRW(context, "Nemáte přístup k tomuto úkolu.") : Return
                CI("Přejít na stránku úkolu", "p56_framework.aspx?pid=" & intPID.ToString)
                CI("Posunout/Doplnit", "workflow_dialog.aspx?prefix=p56&pid=" & intPID.ToString)
                If cDisp.OwnerAccess Then
                    SEP()
                    CI("Upravit záznam (karta úkolu)", "p56_record.aspx?pid=" & intPID.ToString)
                    CI("Kopírovat", "p56_record.aspx?clone=1&pid=" & intPID.ToString)
                End If
                If cDisp.P31_Create Then
                    SEP()
                    CI("Vykázat v úkolu worksheet úkon", "p31_record.aspx?p56id=" & intPID.ToString)
                End If
                CI("Tisková sestava", "report_modal.aspx?prefix=p56&pid=" & intPID.ToString)
            Case Else
                CI("Nezpracovatelný PREFIX", "")
        End Select

        
        context.Response.Write(String.Join("", _lis))

    End Sub

    Private Sub FinalRW(context As HttpContext, strMessage As String)
        If strMessage <> "" Then
            CI(strMessage, "")
        End If
        context.Response.Write(String.Join("", _lis))
    End Sub
    Private Sub SEP()
        _lis.Add("<hr>")
    End Sub
    Private Sub CI(strText As String, strURL As String, Optional bolDisabled As Boolean = False)
        _lis.Add("<menuitem label=" & Chr(34) & strText & Chr(34))
        If strURL = "" Then
            If bolDisabled Then _lis.Add(" disabled")
        Else
            _lis.Add(" onclick=contMenu(" & Chr(34) & strURL & Chr(34) & ")>")
        End If
        _lis.Add("</menuitem>")
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class