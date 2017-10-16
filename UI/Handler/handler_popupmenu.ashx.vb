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

        If strPREFIX = "" Then
            FinalRW(context, "prefix is missing") : Return
        End If
       

        Select Case strPREFIX
            Case "newrec"
                RenderNewRecMenu(factory, context)  'hlavní menu - odkazy k založení nového záznamu
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
                    If factory.SysUser.j04IsMenu_Task Then REL("Stránka úkolu", "p56_framework.aspx?pid=" & cRec.p56ID.ToString, "_top", "Images/task.png")
                End If
                If factory.SysUser.j04IsMenu_Project Then
                    REL("Stránka projektu", "p41_framework.aspx?pid=" & cRec.p41ID.ToString, "_top", "Images/project.png")
                End If


            Case "p56"
                Dim cRec As BO.p56Task = factory.p56TaskBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return

                Dim cDisp As BO.p56RecordDisposition = factory.p56TaskBL.InhaleRecordDisposition(cRec)
                If Not cDisp.ReadAccess Then FinalRW(context, "Nemáte přístup k tomuto úkolu.") : Return
                If factory.SysUser.j04IsMenu_Task Then
                    REL(cRec.FullName, "p56_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
                Else
                    CI(cRec.FullName, "", True, "Images/information.png")
                End If
                If cDisp.P31_Create Then
                    SEP()
                    CI("Zapsat WORKSHEET", "p31_record.aspx?p56id=" & intPID.ToString, cRec.IsClosed, "Images/worksheet.png")
                End If
                SEP()
                CI("Posunout/Doplnit", "workflow_dialog.aspx?prefix=p56&pid=" & intPID.ToString, , "Images/workflow.png")
                If cDisp.OwnerAccess Then
                    SEP()
                    CI("Upravit kartu úkolu", "p56_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
                    CI("Kopírovat", "p56_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
                End If
                If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                    SEP()
                    REL("Statistiky úkolu", "p31_sumgrid.aspx?masterprefix=p56&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
                End If
                SEP()
                CI("Tisková sestava", "report_modal.aspx?prefix=p56&pid=" & intPID.ToString, , "Images/report.png")
            Case "p28"
                Dim cRec As BO.p28Contact = factory.p28ContactBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return
                Dim cDisp As BO.p28RecordDisposition = factory.p28ContactBL.InhaleRecordDisposition(cRec)
                If Not cDisp.ReadAccess Then FinalRW(context, "Nemáte přístup k tomuto klientovi.") : Return
                If factory.SysUser.j04IsMenu_Contact Then
                    REL(cRec.p28Name, "p28_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
                    If cRec.p28ParentID > 0 Then
                        REL("Nadřízený klient", "p28_framework.aspx?pid=" & cRec.p28ParentID.ToString, "_top", "Images/tree.png")
                    End If
                Else
                    CI(cRec.p28Name, "", True, "Images/information.png")
                End If

                SEP()
                If cDisp.OwnerAccess Then
                    CI("Upravit kartu klienta", "p28_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
                    CI("Kopírovat", "p28_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
                End If
                If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                    SEP()
                    REL("Statistiky klienta", "p31_sumgrid.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
                End If
                SEP()
                CI("Tisková sestava", "report_modal.aspx?prefix=p28&pid=" & intPID.ToString, , "Images/report.png")
            Case "p41"
                Dim cRec As BO.p41Project = factory.p41ProjectBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return
                If factory.SysUser.j04IsMenu_Project Then
                    REL(cRec.PrefferedName, "p41_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
                    If cRec.p41ParentID > 0 Then                        
                        REL("Nadřízený projekt", "p41_framework.aspx?pid=" & cRec.p41ParentID.ToString, "_top", "Images/tree.png")
                    End If
                Else
                    CI(cRec.PrefferedName, "", True, "Images/information.png")
                End If
                SEP()

                Dim cP42 As BO.p42ProjectType = factory.p42ProjectTypeBL.Load(cRec.p42ID)
                Dim cDisp As BO.p41RecordDisposition = factory.p41ProjectBL.InhaleRecordDisposition(cRec)
                If Not cDisp.ReadAccess Then FinalRW(context, "Nemáte přístup k tomuto projektu.") : Return

                If cP42.p42IsModule_p31 And Not (cRec.IsClosed Or cRec.p41IsDraft) Then
                    CI("Zapsat WORKSHEET", "", , "Images/worksheet.png")

                    Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.PID, cRec.p42ID, cRec.j18ID, factory.SysUser.j02ID)
                    For Each c In lisP34
                        CI("[" & c.p34Name & "]", "p31_record.aspx?pid=0&p41id=" & cRec.PID.ToString & "&p34id=" + c.PID.ToString, , "Images/worksheet.png", True)
                    Next
                End If

                If cDisp.OwnerAccess Then
                    SEP()
                    CI("Upravit kartu projektu", "p41_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
                    CI("Kopírovat", "p41_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
                End If
                
                If factory.SysUser.j04IsMenu_Contact And cRec.p28ID_Client > 0 Then
                    SEP()
                    REL("Klient projektu", "p28_framework.aspx?pid=" & cRec.p28ID_Client.ToString, "_top", "Images/contact.png")
                End If
                If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                    SEP()
                    REL("Statistiky projektu", "p31_sumgrid.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
                End If
                SEP()
                CI("Tisková sestava", "report_modal.aspx?prefix=p41&pid=" & intPID.ToString, , "Images/report.png")
            Case "p91"
                Dim cRec As BO.p91Invoice = factory.p91InvoiceBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return

                Dim cDisp As BO.p91RecordDisposition = factory.p91InvoiceBL.InhaleRecordDisposition(cRec)
                If cDisp.ReadAccess Then
                    REL(cRec.p92Name & ": " & cRec.p91Code, "p91_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
                    If cDisp.OwnerAccess Then
                        SEP()
                        CI("Upravit kartu faktury", "p91_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
                    End If
                    If factory.SysUser.j04IsMenu_Contact And cRec.p28ID > 0 Then
                        SEP()
                        REL("Klient faktury", "p28_framework.aspx?pid=" & cRec.p28ID.ToString, "_top", "Images/contact.png")
                    End If
                    If factory.SysUser.j04IsMenu_Project And cRec.p41ID_First > 0 Then
                        REL("Fakturovaný projekt", "p41_framework.aspx?pid=" & cRec.p41ID_First.ToString, "_top", "Images/project.png")
                    End If
                    If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                        SEP()
                        REL("Statistiky faktury", "p31_sumgrid.aspx?masterprefix=p91&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
                    End If
                    SEP()
                    Dim cP92 As BO.p92InvoiceType = factory.p92InvoiceTypeBL.Load(cRec.p92ID)
                    With cP92
                        If .x31ID_Invoice > 0 Then
                            CI("Sestava dokladu", "report_modal.aspx?x31id=" & .x31ID_Invoice.ToString & "&prefix=p91&pid=" & intPID.ToString, , "Images/report.png")
                        End If
                        If .x31ID_Attachment > 0 Then
                            CI("Sestava přílohy", "report_modal.aspx?x31id=" & .x31ID_Attachment.ToString & "&prefix=p91&pid=" & intPID.ToString, , "Images/report.png")
                        End If
                    End With
                    CI("Tisková sestava", "report_modal.aspx?prefix=p91&pid=" & intPID.ToString, , "Images/report.png")
                Else
                    CI(cRec.p92Name & ": " & cRec.p91Code, "", True, "Images/information.png")
                End If
                
            Case "o23"
                Dim cRec As BO.o23Doc = factory.o23DocBL.Load(intPID)
                If cRec Is Nothing Then FinalRW(context, "Záznam nebyl nalezen.") : Return
                Dim cDisp As BO.o23RecordDisposition = factory.o23DocBL.InhaleDisposition(cRec)
                If factory.SysUser.j04IsMenu_Notepad Then
                    REL(cRec.o23Name, "o23_fixwork.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
                Else
                    CI(cRec.o23Name, "", True, "Images/information.png")
                End If
                SEP()
                If cDisp.OwnerAccess Then
                    SEP()
                    CI("Upravit kartu dokumentu", "o23_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
                End If
            Case Else
                CI("Nezpracovatelný PREFIX", "")
        End Select

        
        Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim s As String = jss.Serialize(_lis)
        context.Response.Write(s)


    End Sub

    Private Sub RenderNewRecMenu(factory As BL.Factory, context As HttpContext)
        With factory.SysUser

            If .j04IsMenu_Worksheet Then

                CI("Zapsat worksheet", "p31_record.aspx?pid=0", , "Images/worksheet.png")
            End If
            If .j04IsMenu_Contact Then
                If factory.TestPermission(BO.x53PermValEnum.GR_P28_Creator, BO.x53PermValEnum.GR_P28_Draft_Creator) Then
                    CI(Resources.common.Klient, "p28_record.aspx?pid=0&hrjs=hardrefresh_menu", , "Images/contact.png")
                End If
            End If
            If .j04IsMenu_Project Then
                If factory.TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator) Then
                    CI(Resources.common.Projekt, "p41_create.aspx?hrjs=hardrefresh_menu", , "Images/project.png")
                End If
            End If

            If factory.SysUser.j04IsMenu_Notepad Then
                CI(Resources.common.Dokument, "select_doctype.aspx?hrjs=hardrefresh_menu", , "Images/notepad.png")
            End If
            If factory.SysUser.j04IsMenu_Task Then
                CI(Resources.common.Ukol, "p56_record.aspx?masterprefix=p41&masterpid=0&hrjs=hardrefresh_menu", , "Images/task.png")
            End If
            CI("Událost v kalendáři", "o22_record.aspx?hrjs=hardrefresh_menu", , "Images/event.png")

            If .j04IsMenu_Invoice Then
                If factory.TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator) Then
                    CI(Resources.common.Faktura, "p91_create_step1.aspx?prefix=p28&hrjs=hardrefresh_menu", , "Images/invoice.png")
                End If
            End If
            If factory.TestPermission(BO.x53PermValEnum.GR_P90_Create) Then
                CI(Resources.common.ZalohovaFaktura, "p90_record.aspx?pid=0&hrjs=hardrefresh_menu", , "Images/proforma.png")
            End If

            If factory.SysUser.IsAdmin Then
                CI(Resources.common.Osoba, "j02_record.aspx?pid=0&hrjs=hardrefresh_menu", , "Images/person.png")
            End If
        End With


    End Sub

    Private Sub FinalRW(context As HttpContext, strMessage As String)
        If strMessage <> "" Then
            CI(strMessage, "", True)
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
    Private Sub CI(strText As String, strURL As String, Optional bolDisabled As Boolean = False, Optional strImageUrl As String = "", Optional bolChild As Boolean = False)
        Dim c As New BO.ContextMenuItem
        If Len(strText) > 35 Then strText = Left(strText, 35) & "..."
        c.Text = strText
        c.NavigateUrl = "javascript:contMenu(" & Chr(34) & strURL & Chr(34) & ")"
        c.IsDisabled = bolDisabled
        c.ImageUrl = strImageUrl
        c.IsChildOfPrevious = bolChild
        _lis.Add(c)

    End Sub
    Private Sub REL(strText As String, strURL As String, strTarget As String, Optional strImageUrl As String = "")
        Dim c As New BO.ContextMenuItem
        If Len(strText) > 35 Then strText = Left(strText, 35) & "..."
        c.Text = strText
        c.NavigateUrl = "javascript:contReload(" & Chr(34) & strURL & Chr(34) & "," & Chr(34) & strTarget & Chr(34) & ")"
        c.ImageUrl = strImageUrl
        c.Target = strTarget
        _lis.Add(c)
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class