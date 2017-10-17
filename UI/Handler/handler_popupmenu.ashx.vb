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
        Dim strFlag As String = Trim(context.Request.Item("flag"))

        If strPREFIX = "" Then
            FinalRW(context, "prefix is missing") : Return
        End If
        If Left(strPREFIX, 5) = "admin" Then
            HandleAdminMenu(strPREFIX, intPID, factory) 'číselníky za administrace
            FinalRW(context, "") : Return
        End If
        CI("flag: " & strFlag, "", True)

        Select Case strPREFIX
            Case "newrec"
                RenderNewRecMenu(factory)  'hlavní menu - odkazy k založení nového záznamu
            Case "p31"
                HandleP31(intPID, factory, strFlag)

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
                SEP()
                CI("Oštítkovat", "tag_binding.aspx?prefix=p56&pids=" & intPID.ToString, , "Images/tag.png")
            Case "p28"
                HandleP28(intPID, factory, strFlag)
            Case "p41"
                HandleP41(intPID, factory, strFlag)
            Case "p91"
                HandleP91(intPID, factory)
            Case "o23"
                HandleO23(intPID, factory, strFlag)
            Case "j02"
                HandleJ02(intPID, factory)
            Case "p90"
                HandleP90(intPID, factory)
            Case "p51"
                HandleP51(intPID, factory)
            Case "x40"
                HandleX40(intPID, factory)
            Case Else
                CI("Nezpracovatelný PREFIX", "")
        End Select

        
        Dim jss As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim s As String = jss.Serialize(_lis)
        context.Response.Write(s)


    End Sub
    Private Sub HandleP91(intPID As Integer, factory As BL.Factory)
        Dim cRec As BO.p91Invoice = factory.p91InvoiceBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen.", "", True) : Return

        Dim cDisp As BO.p91RecordDisposition = factory.p91InvoiceBL.InhaleRecordDisposition(cRec)
        If cDisp.ReadAccess Then
            REL(cRec.p92Name & ": " & cRec.p91Code, "p91_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
            If cDisp.OwnerAccess Then
                SEP()
                CI("Upravit kartu faktury", "p91_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
            End If
            If factory.SysUser.j04IsMenu_Contact And cRec.p28ID > 0 Then
                SEP()
                REL(cRec.p28Name, "p28_framework.aspx?pid=" & cRec.p28ID.ToString, "_top", "Images/contact.png")
            End If
            If factory.SysUser.j04IsMenu_Project And cRec.p41ID_First > 0 Then

                REL(cRec.p41Name, "p41_framework.aspx?pid=" & cRec.p41ID_First.ToString, "_top", "Images/project.png")
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
        SEP()
        CI("Oštítkovat", "tag_binding.aspx?prefix=p91&pids=" & intPID.ToString, , "Images/tag.png")
    End Sub
    Private Sub HandleP31(intPID As Integer, factory As BL.Factory, strFlag As String)
        Dim cRec As BO.p31Worksheet = factory.p31WorksheetBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen.", "", True) : Return
        Dim strMasterPrefix As String = Left(strFlag, 3)
        Select Case strFlag
            Case "p31_approving_step3"  'schvalování
                CI("Fakturovat", "javascript:ContextMenu_Batch4(" & cRec.PID.ToString & ")", , "Images/a14.gif")
                SEP()
                CI("Zahrnout do paušálu", "javascript:ContextMenu_Batch6(" & cRec.PID.ToString & ")", , "Images/a16.gif")
                SEP()                
                CI("Viditelný odpis", "javascript:ContextMenu_Batch2(" & cRec.PID.ToString & ")", , "Images/a12.gif")
                CI("Skrytý odpis", "javascript:ContextMenu_Batch3(" & cRec.PID.ToString & ")", , "Images/a13.gif")
                SEP()
                CI("Fakturovat později", "javascript:ContextMenu_Batch7(" & cRec.PID.ToString & ")", , "Images/a17.gif")
                SEP()
                CI("Vyčistit schválování (bude rozpracováno)", "javascript:ContextMenu_BatchClear(" & cRec.PID.ToString & ")", , "Images/clear.png")
                If cRec.p33ID = BO.p33IdENUM.Cas Then
                    SEP()
                    CI("Rozdělit úkon na 2 kusy", "javascript:ContextMenu_Split(" & cRec.PID.ToString & ")", , "Images/split.png")
                End If

                Return
            Case "p91_framework_detail"    'položky faktury
        End Select


        Dim cDisp As BO.p31WorksheetDisposition = factory.p31WorksheetBL.InhaleRecordDisposition(intPID)
        If cDisp.RecordDisposition = BO.p31RecordDisposition._NoAccess Then CI("K úkonu nemáte přístup.", "", True) : Return

        Select Case cDisp.RecordState
            Case BO.p31RecordState.Editing
                If cDisp.RecordDisposition = BO.p31RecordDisposition.CanEdit Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Then
                    CI("Upravit úkon", "p31_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
                    CI("Kopírovat", "p31_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
                    If cRec.p33ID = BO.p33IdENUM.Cas Then
                        CI("Rozdělit úkon na 2 kusy", "p31_record_split.aspx?pid=" & intPID.ToString, , "Images/split.png")
                    End If
                    If cDisp.RecordDisposition = BO.p31RecordDisposition.CanApprove Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Then
                        SEP()
                        CI("Schválit", "", , "Images/approve.png")
                        CI("Schvalovací dialog", "p31_approving_step2.aspx?pids=" & intPID.ToString, , "Images/approve.png", True, True)

                        CI("Fakturovat", "javascript:ContextMenu_Approve(4," & cRec.PID.ToString & ")", , "Images/a14.gif", True)
                        CI("Zahrnout do paušálu", "javascript:ContextMenu_Approve(6," & cRec.PID.ToString & ")", , "Images/a16.gif", True)
                        CI("Viditelný odpis", "javascript:ContextMenu_Approve(2," & cRec.PID.ToString & ")", , "Images/a12.gif", True)
                        CI("Skrytý odpis", "javascript:ContextMenu_Approve(3," & cRec.PID.ToString & ")", , "Images/a13.gif", True)
                        CI("Fakturovat později", "javascript:ContextMenu_Approve(7," & cRec.PID.ToString & ")", , "Images/a17.gif", True)

                    End If
                End If
            Case BO.p31RecordState.Approved
                CI("Detail úkonu", "p31_record.aspx?pid=" & intPID.ToString, , "Images/zoom.png")

                If cDisp.RecordDisposition = BO.p31RecordDisposition.CanEdit Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Or cRec.j02ID = factory.SysUser.j02ID Then
                    CI("Kopírovat", "p31_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
                End If
                If cDisp.RecordDisposition = BO.p31RecordDisposition.CanApprove Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit Then
                    SEP()
                    CI("Pře-schválit", "", , "Images/approve.png")

                    CI("Schvalovací dialog", "p31_approving_step2.aspx?pids=" & intPID.ToString, , "Images/approve.png", True, True)
                    CI("Vyčistit schvalování", "javascript:ContextMenu_Approve(0," & cRec.PID.ToString & ")", , "Images/clear.png", True)
                    CI("Fakturovat", "javascript:ContextMenu_Approve(4," & cRec.PID.ToString & ")", , "Images/a14.gif", True)
                    CI("Zahrnout do paušálu", "javascript:ContextMenu_Approve(6," & cRec.PID.ToString & ")", , "Images/a16.gif", True)
                    CI("Viditelný odpis", "javascript:ContextMenu_Approve(2," & cRec.PID.ToString & ")", , "Images/a12.gif", True)
                    CI("Skrytý odpis", "javascript:ContextMenu_Approve(3," & cRec.PID.ToString & ")", , "Images/a13.gif", True)
                    CI("Fakturovat později", "javascript:ContextMenu_Approve(7," & cRec.PID.ToString & ")", , "Images/a17.gif", True)
                End If
            Case BO.p31RecordState.Invoiced
                If strMasterPrefix = "p91" Then
                    CI("Upravit úkon", "p31_record_AI.aspx?pid=" & intPID.ToString, , "Images/zoom.png")
                Else
                    CI("Detail úkonu", "p31_record.aspx?pid=" & intPID.ToString, , "Images/zoom.png")
                End If

                If cDisp.RecordDisposition = BO.p31RecordDisposition.CanEdit Or cRec.j02ID = factory.SysUser.j02ID Then
                    SEP()
                    CI("Kopírovat", "p31_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
                End If

        End Select

        SEP()
        If cRec.p56ID > 0 And strMasterPrefix <> "p56" Then
            If factory.SysUser.j04IsMenu_Task Then REL("Stránka úkolu", "p56_framework.aspx?pid=" & cRec.p56ID.ToString, "_top", "Images/task.png")
        End If
        If factory.SysUser.j04IsMenu_Project And strMasterPrefix <> "p41" Then
            Dim ss As String = cRec.p41NameShort
            If ss = "" Then ss = cRec.p41Name
            If cRec.p28ID_Client > 0 Then ss = cRec.ClientName & " - " & ss
            REL(ss, "p41_framework.aspx?pid=" & cRec.p41ID.ToString, "_top", "Images/project.png")
        End If
        If factory.SysUser.j04IsMenu_Invoice And cRec.p91ID > 0 And strMasterPrefix <> "p91" Then
            SEP()
            Dim cP91 As BO.p91Invoice = factory.p91InvoiceBL.Load(cRec.p91ID)
            REL(cP91.p92Name & ": " & cP91.p91Code, "p91_framework.aspx?pid=" & cRec.p91ID.ToString, "_top", "Images/invoice.png")
        End If
        SEP()
        CI("Oštítkovat", "tag_binding.aspx?prefix=p31&pids=" & intPID.ToString, , "Images/tag.png")
    End Sub
    Private Sub HandleP28(intPID As Integer, factory As BL.Factory, strFlag As String)
        Dim cRec As BO.p28Contact = factory.p28ContactBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen", "", True) : Return
        Dim cDisp As BO.p28RecordDisposition = factory.p28ContactBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then CI("Ke klientovi nemáte oprávnění.", "", True) : Return

        If factory.SysUser.j04IsMenu_Contact Then
            REL(cRec.p28Name, "p28_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
            If cRec.p28ParentID > 0 Then
                Dim cParent As BO.p28Contact = factory.p28ContactBL.Load(cRec.p28ParentID)
                REL(cParent.p28Name, "p28_framework.aspx?pid=" & cRec.p28ParentID.ToString, "_top", "Images/tree.png")
            End If
        Else
            CI(cRec.p28Name, "", True, "Images/information.png")
        End If
        Dim bolCanInvoice As Boolean = factory.TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator), bolInvoicing As Boolean = False

        If (factory.SysUser.IsApprovingPerson Or bolCanInvoice) And cRec.p28SupplierFlag <> BO.p28SupplierFlagENUM.NotClientNotSupplier Then
            Dim mq As New BO.myQueryP31
            mq.p28ID_Client = cRec.PID
            mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            Dim cSum As BO.p31WorksheetSum = factory.p31WorksheetBL.LoadSumRow(mq, True, True)

            Dim intWIPx As Integer = 0, intAPPx As Integer = 0
            With cSum
                intWIPx = .WaitingOnApproval_Hours_Count + .WaitingOnApproval_Other_Count
                intAPPx = .WaitingOnInvoice_Hours_Count + .WaitingOnInvoice_Other_Count
            End With
            If intWIPx > 0 Or intAPPx > 0 Then
                SEP()
                Dim ss As String = String.Format("Schválit rozpracované úkony ({0}x)", intWIPx)
                If intWIPx = 0 And intAPPx > 0 Then ss = String.Format("Přes-schválit ({0}x) schválené", intAPPx)
                If intWIPx > 0 And intAPPx > 0 Then ss = String.Format("Schválit ({0}x)/přes-schválit ({1}x)", intWIPx, intAPPx)


                CI(ss, "entity_modal_approving.aspx?prefix=p28&pid=" & intPID.ToString, , "Images/approve.png", False, True)

                If bolCanInvoice Then CI("Fakturovat", "", , "Images/invoice.png") : bolInvoicing = True
                If bolCanInvoice And intAPPx > 0 Then
                    CI(String.Format("Fakturovat schválené úkony ({0}x)", intAPPx.ToString), "p91_create_step1.aspx?nogateway=1&prefix=p28&pid=" & intPID.ToString, , "Images/invoice.png", True, True)
                End If
                If bolCanInvoice And intAPPx = 0 And intWIPx > 0 Then
                    CI(String.Format("Fakturovat bez schvalování ({0}x+{1}x)", intWIPx, intAPPx), "entity_modal_invoicing.aspx?prefix=p28&pids=" & intPID.ToString, , "Images/invoice.png", True, True)
                End If
                If factory.SysUser.j04IsMenu_Invoice Then
                    If cSum.Last_p91ID > 0 Then
                        REL(String.Format("Poslední faktura: {0}", factory.p91InvoiceBL.Load(cSum.Last_p91ID).p91Code), "p91_framework.aspx?pid=" & cSum.Last_p91ID.ToString, "_top", "Images/invoice.png", True)
                    Else
                        CI("Klient zatím nefakturován", "", True, , True)
                    End If

                End If
            End If
        End If
        If bolCanInvoice Then
            If Not bolInvoicing Then CI("Fakturovat", "", , "Images/invoice.png")
            CI("Fakturovat jednou částkou bez úkonů", "p91_create_step1.aspx?quick=1&prefix=p28&pid=" & intPID.ToString, , "Images/invoice.png", True, True)
        End If

        If cDisp.OwnerAccess Then
            SEP()
            CI("Upravit kartu klienta", "p28_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
            CI("Kopírovat", "p28_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
        End If
        If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            SEP()
            REL("Statistiky klienta", "p31_sumgrid.aspx?masterprefix=p28&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
        End If
        SEP()
        CI("Tisková sestava", "report_modal.aspx?prefix=p28&pid=" & intPID.ToString, , "Images/report.png")
        SEP()
        CI("Oštítkovat", "tag_binding.aspx?prefix=p28&pids=" & intPID.ToString, , "Images/tag.png")
    End Sub
    Private Sub HandleP41(intPID As Integer, factory As BL.Factory, strFlag As String)
        Dim cRec As BO.p41Project = factory.p41ProjectBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen.", "", True) : Return
        If factory.SysUser.j04IsMenu_Project Then
            REL(cRec.PrefferedName, "p41_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
            If cRec.p41ParentID > 0 Then
                Dim cParent As BO.p41Project = factory.p41ProjectBL.Load(cRec.p41ParentID)
                REL(cParent.PrefferedName, "p41_framework.aspx?pid=" & cRec.p41ParentID.ToString, "_top", "Images/tree.png")
            End If
        Else
            CI(cRec.PrefferedName, "", True, "Images/information.png")
        End If


        Dim cP42 As BO.p42ProjectType = factory.p42ProjectTypeBL.Load(cRec.p42ID)
        Dim cDisp As BO.p41RecordDisposition = factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        If Not cDisp.ReadAccess Then CI("Nemáte přístup k tomuto projektu.", "", True) : Return
        If cP42.p42IsModule_p31 Then
            If Not (cRec.IsClosed Or cRec.p41IsDraft) Then
                CI("Zapsat WORKSHEET", "", , "Images/worksheet.png")

                Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.PID, cRec.p42ID, cRec.j18ID, factory.SysUser.j02ID)
                For Each c In lisP34
                    CI("[" & c.p34Name & "]", "p31_record.aspx?pid=0&p41id=" & cRec.PID.ToString & "&p34id=" + c.PID.ToString, , "Images/worksheet.png", True)
                Next
            End If

            Dim mq As New BO.myQueryP31
            mq.p41ID = cRec.PID
            mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead
            Dim cSum As BO.p31WorksheetSum = factory.p31WorksheetBL.LoadSumRow(mq, True, True)
            Dim intWIPx As Integer = 0, intAPPx As Integer = 0
            With cSum
                intWIPx = .WaitingOnApproval_Hours_Count + .WaitingOnApproval_Other_Count
                intAPPx = .WaitingOnInvoice_Hours_Count + .WaitingOnInvoice_Other_Count
            End With

            Dim bolCanInvoice As Boolean = factory.TestPermission(BO.x53PermValEnum.GR_P91_Creator, BO.x53PermValEnum.GR_P91_Draft_Creator), bolInvoicing As Boolean = False
            If intWIPx > 0 Or intAPPx > 0 Then
                Dim bolCanApprove As Boolean = factory.TestPermission(BO.x53PermValEnum.GR_P31_Approver)
                If Not bolCanApprove And cDisp.x67IDs.Count > 0 Then
                    Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                    If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                        bolCanApprove = True
                    End If
                End If
                If bolCanApprove Or bolCanInvoice Then SEP()
                If bolCanApprove And (intWIPx > 0 Or intAPPx > 0) Then
                    Dim ss As String = String.Format("Schválit rozpracované úkony ({0}x)", intWIPx)
                    If intWIPx = 0 And intAPPx > 0 Then ss = String.Format("Přes-schválit ({0}x) schválené", intAPPx)
                    If intWIPx > 0 And intAPPx > 0 Then ss = String.Format("Schválit ({0}x)/přes-schválit ({1}x)", intWIPx, intAPPx)
                    CI(ss, "entity_modal_approving.aspx?prefix=p41&pid=" & intPID.ToString, , "Images/approve.png", False, True)
                End If
                If bolCanInvoice Then CI("Fakturovat", "", , "Images/invoice.png") : bolInvoicing = True
                If bolCanInvoice And intAPPx > 0 Then
                    CI(String.Format("Fakturovat schválené úkony ({0}x)", intAPPx.ToString), "p91_create_step1.aspx?nogateway=1&prefix=p41&pid=" & intPID.ToString, , "Images/invoice.png", True) : bolInvoicing = True
                End If
                If bolCanInvoice And intAPPx = 0 And intWIPx > 0 Then
                    CI(String.Format("Fakturovat bez schvalování ({0}x+{1}x)", intWIPx, intAPPx), "entity_modal_invoicing.aspx?prefix=p41&pids=" & intPID.ToString, , "Images/invoice.png", True) : bolInvoicing = True
                End If

                If factory.SysUser.j04IsMenu_Invoice Then
                    If cSum.Last_p91ID > 0 Then
                        REL(String.Format("Poslední faktura: {0}", factory.p91InvoiceBL.Load(cSum.Last_p91ID).p91Code), "p91_framework.aspx?pid=" & cSum.Last_p91ID.ToString, "_top", "Images/invoice.png", True)
                    Else
                        CI("Projekt zatím nefakturován", "", True, , True)
                    End If

                End If
            End If
            If bolCanInvoice Then
                If Not bolInvoicing Then CI("Fakturovat", "", , "Images/invoice.png")
                CI("Fakturovat jednou částkou bez úkonů", "p91_create_step1.aspx?quick=1&prefix=p41&pid=" & intPID.ToString, , "Images/invoice.png", True)
            End If
            

        End If
        

        If cDisp.OwnerAccess Then
            SEP()
            CI("Upravit kartu projektu", "p41_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
            CI("Kopírovat", "p41_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
        End If

        If factory.SysUser.j04IsMenu_Contact And cRec.p28ID_Client > 0 Then
            SEP()
            REL(cRec.Client, "p28_framework.aspx?pid=" & cRec.p28ID_Client.ToString, "_top", "Images/contact.png")
        End If
        If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
            SEP()
            REL("Statistiky projektu", "p31_sumgrid.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
        End If
        SEP()
        CI("Tisková sestava", "report_modal.aspx?prefix=p41&pid=" & intPID.ToString, , "Images/report.png")
        SEP()
        CI("Oštítkovat", "tag_binding.aspx?prefix=p41&pids=" & intPID.ToString, , "Images/tag.png")
    End Sub
    Private Sub HandleO23(intPID As Integer, factory As BL.Factory, strFlag As String)
        Dim cRec As BO.o23Doc = factory.o23DocBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen.", "", True) : Return
        Dim cDisp As BO.o23RecordDisposition = factory.o23DocBL.InhaleDisposition(cRec)
        Dim cX18 As BO.x18EntityCategory = factory.x18EntityCategoryBL.Load(cRec.x18ID)
        If factory.SysUser.j04IsMenu_Notepad Then
            Select Case strFlag
                Case "o23_fixwork"
                    REL("Přejít do obecných přehledů", "entity_framework.aspx?prefix=o23&pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
                Case ""
                    REL("Přejít do pevných přehledů", "o23_fixwork.aspx?pid=" & intPID.ToString & "&x18id=" & cRec.x18ID.ToString, "_top", "Images/fullscreen.png")
                Case Else
                    REL("Přejít do pevných přehledů", "o23_fixwork.aspx?pid=" & intPID.ToString & "&x18id=" & cRec.x18ID.ToString, "_top", "Images/fullscreen.png")
                    REL("Přejít do obecných přehledů", "entity_framework.aspx?prefix=o23&pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
            End Select
            

        Else
            CI(cRec.o23Name, "", True, "Images/information.png")
        End If

        If cDisp.OwnerAccess Then
            SEP()
            If cX18.x18IsManyItems Then
                CI("Upravit kartu dokumentu", "o23_record.aspx?pid=" & intPID.ToString & "&x18id=" & cRec.x18ID.ToString, , "Images/edit.png")
            Else
                CI("Upravit kartu kategorie", "o23_record.aspx?pid=" & intPID.ToString & "&x18id=" & cRec.x18ID.ToString, , "Images/edit.png")
            End If

            CI("Kopírovat", "o23_record.aspx?clone=1&pid=" & intPID.ToString & "&x18id=" & cRec.x18ID.ToString, , "Images/copy.png")
        End If
        SEP()
        CI("Oštítkovat", "tag_binding.aspx?prefix=o23&pids=" & intPID.ToString, , "Images/tag.png")
    End Sub
    Private Sub HandleX40(intPID As Integer, factory As BL.Factory)
        Dim cRec As BO.x40MailQueue = factory.x40MailQueueBL.Load(intPID)
        CI(cRec.StatusAlias, "", True, "Images/information.png")
        SEP()
        CI("Detail", "x40_record.aspx?pid=" & intPID.ToString, , "Images/zoom.png")

        If cRec.x40MessageID <> "" And cRec.x40ArchiveFolder <> "" Then
            SEP()
            REL("EML formát zprávy", "binaryfile.aspx?prefix=x40-eml&pid=" & cRec.PID.ToString, "", "Images/email.png")
            REL("Otevřít v MS-OUTLOOK", "binaryfile.aspx?prefix=x40-msg&pid=" & cRec.PID.ToString, "", "Images/outlook.png")
        End If

    End Sub
    Private Sub HandleP51(intPID As Integer, factory As BL.Factory)
        Dim cRec As BO.p51PriceList = factory.p51PriceListBL.Load(intPID)
        If Not factory.TestPermission(BO.x53PermValEnum.GR_P51_Admin) Then
            CI("Chybí oprávnění pro správu ceníků.", "", True) : Return
        Else
            CI(cRec.p51Name, "", True, "Images/information.png")
        End If
        SEP()
        CI("Upravit ceník", "p51_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
        CI("Kopírovat", "p51_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
        If cRec.p51IsCustomTailor Then
            SEP()
            Dim mqP28 As New BO.myQueryP28, b As Boolean = False
            mqP28.p51ID = intPID
            Dim lisP28 As IEnumerable(Of BO.p28Contact) = factory.p28ContactBL.GetList(mqP28)
            For Each c In lisP28
                REL(c.p28Name, "p28_framework.aspx?pid=" & c.PID.ToString, "_top", "Images/contact.png")
            Next
            Dim mqP41 As New BO.myQueryP41
            mqP41.p51ID = intPID
            mqP41.TopRecordsOnly = 10
            Dim lisP41 As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mqP41)
            For Each c In lisP41
                REL(c.FullName, "p41_framework.aspx?pid=" & c.PID.ToString, "_top", "Images/project.png")
            Next

        End If
        If cRec.p51ID_Master > 0 Then
            SEP()
            CI(String.Join("MASTER ceník: {0}", cRec.p51Name_Master), "p51_record.aspx?pid=" & cRec.p51ID_Master.ToString, , "Images/edit.png")
        End If
        If cRec.p51IsInternalPriceList And factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
            SEP()
            REL("Nastavení interních ceníků", "admin_framework.aspx?prefix=p50", "_top", "Images/setting.png")
        End If
    End Sub

    Private Sub HandleJ02(intPID As Integer, factory As BL.Factory)
        Dim cRec As BO.j02Person = factory.j02PersonBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen.", "", True) : Return
        If factory.SysUser.j04IsMenu_People Then
            REL(cRec.FullNameDesc, "j02_framework.aspx?pid=" & intPID.ToString, "_top", "Images/fullscreen.png")
        Else
            CI(cRec.FullNameDesc, "", True, "Images/information.png")
        End If
        If factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
            SEP()
            CI("Upravit kartu osoby", "j02_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
            CI("Kopírovat", "j02_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
        End If
        If cRec.j02IsIntraPerson Then
            If factory.TestPermission(BO.x53PermValEnum.GR_P31_Pivot) Then
                SEP()
                REL("Statistiky osoby", "p31_sumgrid.aspx?masterprefix=j02&masterpid=" & cRec.PID.ToString, "_top", "Images/pivot.png")
            End If
            SEP()
            CI("Tisková sestava", "report_modal.aspx?prefix=p28&pid=" & intPID.ToString, , "Images/report.png")
        Else
            Dim mqP28 As New BO.myQueryP28, b As Boolean = False
            mqP28.j02ID = intPID
            Dim lisP28 As IEnumerable(Of BO.p28Contact) = factory.p28ContactBL.GetList(mqP28)
            For Each c In lisP28
                If Not b Then SEP() : b = True
                REL(c.p28Name, "p28_framework.aspx?pid=" & c.PID.ToString, "_top", "Images/contact.png")
            Next
            Dim mqP41 As New BO.myQueryP41
            mqP41.j02ID_ContactPerson = intPID
            Dim lisP41 As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mqP41)
            For Each c In lisP41
                If lisP28.Where(Function(p) p.PID = c.p28ID_Client).Count = 0 Then
                    If Not b Then SEP() : b = True
                    REL(c.FullName, "p41_framework.aspx?pid=" & c.PID.ToString, "_top", "Images/project.png")
                End If
            Next
        End If

        If factory.SysUser.j04IsMenu_People Then
            SEP()
            CI("Oštítkovat", "tag_binding.aspx?prefix=j02&pids=" & intPID.ToString, , "Images/tag.png")
        End If
    End Sub
    Private Sub HandleP90(intPID As Integer, factory As BL.Factory)
        Dim cRec As BO.p90Proforma = factory.p90ProformaBL.Load(intPID)
        If cRec Is Nothing Then CI("Záznam nebyl nalezen.", "", True) : Return
        If Not factory.TestPermission(BO.x53PermValEnum.GR_P90_Reader) Then
            CI("Chybí oprávnění ke čtení záloh.", "", True) : Return
        End If
        CI(cRec.p89Name & ": " & cRec.p90Code, "", True, "Images/information.png")
        If factory.TestPermission(BO.x53PermValEnum.GR_P90_Owner) Or cRec.j02ID_Owner = factory.SysUser.j02ID Then
            SEP()
            CI("Upravit kartu zálohy", "p90_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
            CI("Kopírovat", "p90_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
        End If
        SEP()
        CI("Tisková sestava", "report_modal.aspx?prefix=p90&pid=" & intPID.ToString, , "Images/report.png")
        If factory.SysUser.j04IsMenu_Contact And cRec.p28ID > 0 Then
            SEP()
            REL(cRec.p28Name, "p28_framework.aspx?pid=" & cRec.p28ID.ToString, "_top", "Images/contact.png")
        End If
        Dim lis As IEnumerable(Of BO.p99Invoice_Proforma) = factory.p90ProformaBL.GetList_p99(0, cRec.PID, 0)
        If lis.Count > 0 Then SEP()
        For Each c In lis
            REL(String.Join("Spárováno: {0}", c.p91Code), "p91_framework.aspx?pid=" & c.p91ID.ToString, "_top", "Images/invoice.png")
        Next


        SEP()
        CI("Oštítkovat", "tag_binding.aspx?prefix=p90&pids=" & intPID.ToString, , "Images/tag.png")
    End Sub
    Private Sub HandleAdminMenu(strPrefix As String, intPID As Integer, factory As BL.Factory)
        Dim a() As String = Split(strPrefix, "-")
        If Not factory.TestPermission(BO.x53PermValEnum.GR_Admin) Then
            CI("Nedisponujete oprávněním administrátora.", "", True) : Return
        End If

        CI("Upravit záznam", Right(a(1), 3) & "_record.aspx?pid=" & intPID.ToString, , "Images/edit.png")
        CI("Kopírovat", Right(a(1), 3) & "_record.aspx?clone=1&pid=" & intPID.ToString, , "Images/copy.png")
    End Sub
    Private Sub RenderNewRecMenu(factory As BL.Factory)
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
    Private Sub CI(strText As String, strURL As String, Optional bolDisabled As Boolean = False, Optional strImageUrl As String = "", Optional bolChild As Boolean = False, Optional bolTopWindow As Boolean = False)
        Dim c As New BO.ContextMenuItem
        If Len(strText) > 35 Then strText = Left(strText, 35) & "..."
        c.Text = strText
        If strURL <> "" Then
            If strURL.IndexOf("javascript") >= 0 Then
                c.NavigateUrl = strURL
            Else
                If bolTopWindow Then
                    c.NavigateUrl = "javascript:contMenu(" & Chr(34) & strURL & Chr(34) & ",true)"
                Else
                    c.NavigateUrl = "javascript:contMenu(" & Chr(34) & strURL & Chr(34) & ",false)"
                End If
            End If
            
        End If
        c.IsDisabled = bolDisabled
        c.ImageUrl = strImageUrl
        c.IsChildOfPrevious = bolChild
        _lis.Add(c)

    End Sub
    Private Sub REL(strText As String, strURL As String, strTarget As String, Optional strImageUrl As String = "", Optional bolChild As Boolean = False)
        Dim c As New BO.ContextMenuItem
        If Len(strText) > 35 Then strText = Left(strText, 35) & "..."
        c.Text = strText
        c.NavigateUrl = "javascript:contReload(" & Chr(34) & strURL & Chr(34) & "," & Chr(34) & strTarget & Chr(34) & ")"
        c.ImageUrl = strImageUrl
        c.Target = strTarget
        c.IsChildOfPrevious = bolChild
        _lis.Add(c)
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class