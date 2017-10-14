Imports System.Web
Imports System.Web.Services

Public Class handler_popupmenu
    Implements System.Web.IHttpHandler
    Private _lis As New List(Of BO.ContextMenuItem)
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "application/json"


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
            Case "p31"
                Dim cRec As BO.p31Worksheet = factory.p31WorksheetBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return

                Dim cDisp As BO.p31WorksheetDisposition = factory.p31WorksheetBL.InhaleRecordDisposition(intPID)
                If cDisp.RecordDisposition = BO.p31RecordDisposition._NoAccess Then FinalRW(context, "K úkonu nemáte přístup.") : Return

                Select Case cDisp.RecordState
                    Case BO.p31RecordState.Editing
                        If cDisp.RecordDisposition = BO.p31RecordDisposition.CanEdit Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Then
                            CI("Upravit záznam (dvojklik)", "p31_record.aspx?pid=" & intPID.ToString)
                            CI("Kopírovat", "p31_record.aspx?clone=1&pid=" & intPID.ToString)
                            If cDisp.RecordDisposition = BO.p31RecordDisposition.CanApprove Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Then
                                CI("Schválit", "p31_record.aspx?pid=" & intPID.ToString)
                            End If
                        End If
                    Case BO.p31RecordState.Approved
                        CI("Detail (dvojklik)", "p31_record.aspx?pid=" & intPID.ToString)
                        If cDisp.RecordDisposition = BO.p31RecordDisposition.CanApprove Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Then
                            CI("Pře-schválit", "p31_record_AI.aspx?pid=" & intPID.ToString)                        
                        End If
                        If cDisp.RecordDisposition = BO.p31RecordDisposition.CanEdit Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Or cRec.j02ID = factory.SysUser.j02ID Then
                            SEP()
                            CI("Kopírovat", "p31_record.aspx?clone=1&pid=" & intPID.ToString)
                        End If
                    Case BO.p31RecordState.Invoiced
                        CI("Detail (dvojklik)", "p31_record_AI.aspx?pid=" & intPID.ToString)
                        If factory.SysUser.j04IsMenu_Invoice Then
                            SEP()
                            REL("Stránka faktury", "p91_framework.aspx?pid=" & cRec.p91ID.ToString, "_top")
                        End If
                        If cDisp.RecordDisposition = BO.p31RecordDisposition.CanEdit Or cRec.j02ID = factory.SysUser.j02ID Then
                            SEP()
                            CI("Kopírovat", "p31_record.aspx?clone=1&pid=" & intPID.ToString)
                        End If

                End Select
                SEP()
                If cRec.p56ID > 0 Then
                    If factory.SysUser.j04IsMenu_Task Then REL("Stránka úkolu", "p56_framework.aspx?pid=" & cRec.p56ID.ToString, "_top")
                End If
                If factory.SysUser.j04IsMenu_Project Then
                    REL("Stránka projektu", "p41_framework.aspx?pid=" & cRec.p41ID.ToString, "_top")
                End If

                
            Case "p56"
                Dim cRec As BO.p56Task = factory.p56TaskBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return

                Dim cDisp As BO.p56RecordDisposition = factory.p56TaskBL.InhaleRecordDisposition(cRec)
                If Not cDisp.ReadAccess Then FinalRW(context, "Nemáte přístup k tomuto úkolu.") : Return
                CI("Stránka úkolu (dvojklik)", "p56_framework.aspx?pid=" & intPID.ToString)
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

        
        Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim s As String = jss.Serialize(_lis)
        context.Response.Write(s)


    End Sub

    Private Sub FinalRW(context As HttpContext, strMessage As String)
        If strMessage <> "" Then
            CI(strMessage, "")
        End If
        Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim s As String = jss.Serialize(_lis)
        context.Response.Write(s)
    End Sub
    Private Sub SEP()
        Dim c As New BO.ContextMenuItem
        c.IsSeparator = True
        _lis.Add(c)
    End Sub
    Private Sub CI(strText As String, strURL As String, Optional bolDisabled As Boolean = False)
        Dim c As New BO.ContextMenuItem
        c.Text = strText
        c.NavigateUrl = "javascript:contMenu(" & Chr(34) & strURL & Chr(34) & ")"
        c.IsDisabled = bolDisabled
        _lis.Add(c)

    End Sub
    Private Sub REL(strText As String, strURL As String, strTarget As String)
        Dim c As New BO.ContextMenuItem
        c.Text = strText
        c.NavigateUrl = strURL
        c.Target = strTarget
        _lis.Add(c)
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class