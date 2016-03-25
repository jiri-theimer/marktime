Imports Telerik.Reporting
Public Class report_modal
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Public Property CurrentX31ID As Integer
        Get
            If Me.x31ID.Items.Count = 0 Then Return 0
            Return BO.BAS.IsNullInt(Me.x31ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.x31ID, value.ToString)
        End Set
    End Property
    Public Property MultiPIDs As String
        Get
            Return Me.hidPIDS.Value
        End Get
        Set(value As String)
            Me.hidPIDS.Value = value
        End Set
    End Property

    Private Sub InhaleLic()
        ceTe.DynamicPDF.Document.AddLicense("DPS50NPDFHMIDOmx0oPeZ+nF3z6VFeFQHDwPiglUKQ/xyRdT8Uvdb5Moivhseqj3bxlt//+w6FtkfFfsGjYwOAJOXNbss5x7huJQ")
    End Sub

    Private Sub report_modal_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master

    End Sub
    Public Property CurrentX29ID As BO.x29IdEnum
        Get
            Return DirectCast(CInt(Me.hidX29ID.Value), BO.x29IdEnum)
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidX29ID.Value = CInt(value).ToString
            Me.hidPrefix.Value = BO.BAS.GetDataPrefix(value)
        End Set
    End Property
    Public ReadOnly Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentX29ID = BO.BAS.GetX29FromPrefix(Request.Item("prefix"))
            If Me.CurrentX29ID = BO.x29IdEnum._NotSpecified Then
                Master.StopPage("prefix missing")
            End If
            ViewState("guid") = BO.BAS.GetGUID
            Me.MultiPIDs = Request.Item("pids")
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                ''If .DataPID = 0 And Request.Item("guid") = "" Then .StopPage("pid missing")
                If .DataPID = 0 And Me.MultiPIDs = "" And Request.Item("guid") = "" Then .StopPage("pid missing")
                .AddToolbarButton("PDF merge", "merge", "0", "Images/merge.png", False)
                .AddToolbarButton("PDF export", "pdf", "0", "Images/pdf.png")
                .AddToolbarButton("Odeslat poštou jako PDF", "mail", "0", "Images/email.png")
                .RadToolbar.FindItemByValue("merge").CssClass = "show_hide1"

            End With
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("periodcombo-custom_query")
                .Add("report_modal-x31id-" & Me.CurrentPrefix)
                .Add("report_modal-period")
                .Add("report_modal-x31id-merge1-" & Me.CurrentPrefix)
                .Add("report_modal-x31id-merge2-" & Me.CurrentPrefix)
                .Add("report_modal-x31id-merge3-" & Me.CurrentPrefix)
            End With
            With Master
                If .DataPID <> 0 Then .HeaderText = "Report | " & .Factory.GetRecordCaption(Me.CurrentX29ID, .DataPID)
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("report_modal-period")

                    Dim strDefX31ID As String = Request.Item("x31id")
                    If strDefX31ID = "" Then strDefX31ID = .GetUserParam("report_modal-x31id-" & Me.CurrentPrefix)
                    SetupX31Combo(strDefX31ID)
                    strDefX31ID = .GetUserParam("report_modal-x31id-merge1-" & Me.CurrentPrefix)
                    If strDefX31ID <> "" Then basUI.SelectDropdownlistValue(Me.x31ID_Merge1, strDefX31ID)
                    strDefX31ID = .GetUserParam("report_modal-x31id-merge2-" & Me.CurrentPrefix)
                    If strDefX31ID <> "" Then basUI.SelectDropdownlistValue(Me.x31ID_Merge2, strDefX31ID)
                    strDefX31ID = .GetUserParam("report_modal-x31id-merge3-" & Me.CurrentPrefix)
                    If strDefX31ID <> "" Then basUI.SelectDropdownlistValue(Me.x31ID_Merge3, strDefX31ID)
                End With
            End With



            InhaleOtherInputParameters()
            If Me.MultiPIDs <> "" Then
                period1.Visible = True
                Master.HideShowToolbarButton("mail", False)

                Select Case Me.CurrentX29ID
                    Case BO.x29IdEnum.p28Contact
                        Dim mq As New BO.myQueryP28
                        mq.PIDs = BO.BAS.ConvertPIDs2List(Me.MultiPIDs)
                        Me.multiple_records.Text = String.Join("<hr>", Master.Factory.p28ContactBL.GetList(mq).Select(Function(p) p.p28Name))
                    Case BO.x29IdEnum.p41Project
                        Dim mq As New BO.myQueryP41
                        mq.PIDs = BO.BAS.ConvertPIDs2List(Me.MultiPIDs)
                        Me.multiple_records.Text = String.Join("<hr>", Master.Factory.p41ProjectBL.GetList(mq).Select(Function(p) p.FullName))
                    Case BO.x29IdEnum.p91Invoice
                        Dim mq As New BO.myQueryP91
                        mq.PIDs = BO.BAS.ConvertPIDs2List(Me.MultiPIDs)
                        Me.multiple_records.Text = String.Join("<hr>", Master.Factory.p91InvoiceBL.GetList(mq).Select(Function(p) p.p91Code))
                    Case BO.x29IdEnum.j02Person
                        Dim mq As New BO.myQueryJ02
                        mq.PIDs = BO.BAS.ConvertPIDs2List(Me.MultiPIDs)
                        Me.multiple_records.Text = String.Join("<hr>", Master.Factory.j02PersonBL.GetList(mq).Select(Function(p) p.FullNameDesc))
                End Select
            Else
                RenderReport()
                multiple_records.Visible = False
            End If

        End If
    End Sub

    Private Sub MultiPidsGeneratePDF()
        InhaleLic()
        Dim doc1 As New ceTe.DynamicPDF.Merger.MergeDocument()
        doc1.Author = "MARKTIME"

        Dim a() As String = Split(Me.MultiPIDs, ",")
        For Each strPID As String In a
            Master.DataPID = CInt(strPID)
            Dim strPdfFileName As String = GenerateOnePDF2Temp(Me.CurrentX31ID, ViewState("guid") & "_" & strPID & ".pdf")
            doc1.Append(Master.Factory.x35GlobalParam.TempFolder & "\" & strPdfFileName)
        Next
        Master.DataPID = 0
        doc1.DrawToWeb("MARKTIME_REPORT_MULTIPLE.pdf", True)
    End Sub

    Private Sub SetupX31Combo(strDefX31ID As String)
        Dim mq As New BO.myQuery
        mq.Closed = BO.BooleanQueryMode.FalseQuery
        Dim lisX31 As IEnumerable(Of BO.x31Report) = Master.Factory.x31ReportBL.GetList(mq).Where(Function(p) p.x29ID = Me.CurrentX29ID And p.x31FormatFlag = BO.x31FormatFlagENUM.Telerik)
        Me.x31ID.DataSource = lisX31
        Me.x31ID.DataBind()
        If strDefX31ID <> "" Then Me.x31ID.SelectedValue = strDefX31ID
        If lisX31.Count = 0 Then
            Master.Notify("V katalogu šablon sestav není žádná položka pro tento typ entity.", NotifyLevel.InfoMessage)

        End If
        With Me.x31ID_Merge1
            .DataSource = lisX31
            .DataBind()
            .Items.Insert(0, "")
        End With
        With Me.x31ID_Merge2
            .DataSource = lisX31
            .DataBind()
            .Items.Insert(0, "")
        End With
        With Me.x31ID_Merge3
            .DataSource = lisX31
            .DataBind()
            .Items.Insert(0, "")
        End With
    End Sub


    Private Sub RenderReport()
        If Me.CurrentX31ID = 0 Or Me.MultiPIDs <> "" Then Return

        Dim cRec As BO.x31Report = Master.Factory.x31ReportBL.Load(Me.CurrentX31ID)
        If cRec Is Nothing Then
            Master.StopPage("Nelze načíst sestavu.<hr>" & Master.Factory.x31ReportBL.ErrorMessage)
            Return
        End If
        Master.HeaderText = cRec.x31Name
        If cRec.x31FormatFlag <> BO.x31FormatFlagENUM.Telerik Then
            Master.StopPage("NOT TRDX format.")
        End If
        Dim strRepFullPath As String = Master.Factory.x35GlobalParam.UploadFolder
        If cRec.ReportFolder <> "" Then
            strRepFullPath += "\" & cRec.ReportFolder
        End If
        strRepFullPath += "\" & cRec.ReportFileName

        Dim cF As New BO.clsFile
        If Not cF.FileExist(strRepFullPath) Then
            Master.Notify("XML soubor šablony tiskové sestavy nelze načíst.", 2)
            Return
        End If

        Dim strXmlContent As String = cF.GetFileContents(strRepFullPath, , False), bolPeriod As Boolean = False


        Dim xmlRepSource As New Telerik.Reporting.XmlReportSource()
        xmlRepSource.Xml = strXmlContent
        If strXmlContent.IndexOf("@datfrom") > 0 Or strXmlContent.IndexOf("@datuntil") > 0 Or cRec.x31IsPeriodRequired Then
            period1.Visible = True
            xmlRepSource.Parameters.Add(New Parameter("datfrom", period1.DateFrom))
            xmlRepSource.Parameters.Add(New Parameter("datuntil", period1.DateUntil))
        Else
            period1.Visible = False
        End If
        If Master.DataPID <> 0 Then
            xmlRepSource.Parameters.Add(New Parameter("pid", Master.DataPID))
        End If


        If Not ViewState("params") Is Nothing Then
            Dim params As Dictionary(Of String, String) = CType(ViewState("params"), Dictionary(Of String, String))
            For Each par In params
                xmlRepSource.Parameters.Add(New Parameter(par.Key, par.Value))
            Next
        End If

        rv1.ReportSource = xmlRepSource
    End Sub

    Private Sub InhaleOtherInputParameters()
        ViewState("params") = Nothing

        Dim a As New Dictionary(Of String, String)

        With Request.QueryString
            For i As Integer = 0 To .Count - 1
                Select Case LCase(.GetKey(i))
                    Case "x29id", "pid", "x31pid", "x31id", "prefix"
                    Case Else
                        a.Add(.GetKey(i), .Item(i))

                End Select
            Next
        End With
        ViewState("params") = a
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("report_modal-period", Me.period1.SelectedValue)
        RenderReport()
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        If Me.MultiPIDs <> "" Then
            MultiPidsGeneratePDF()
        Else
            RenderReport()
        End If

    End Sub

    Private Sub cmdRefreshOnBehind_Click(sender As Object, e As EventArgs) Handles cmdRefreshOnBehind.Click
        RenderReport()
    End Sub

    Private Sub x31ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles x31ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-" & Me.CurrentPrefix, Me.x31ID.SelectedValue)
        RenderReport()

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        Select Case strButtonValue
            Case "mail"
                Server.Transfer("sendmail.aspx?x31id=" & Me.CurrentX31ID.ToString & "&prefix=" & Me.CurrentPrefix & "&pid=" & Master.DataPID.ToString)
            Case "pdf"
                If Me.MultiPIDs <> "" Then
                    MultiPidsGeneratePDF()
                    Return
                End If
                InhaleLic()
                Dim doc1 As New ceTe.DynamicPDF.Merger.MergeDocument()
                With doc1
                    .Author = "MARKTIME 5.0"
                    Dim strOutputFileName As String = Master.Factory.GetRecordFileName(Me.CurrentX29ID, Master.DataPID, "pdf", False, Me.x31ID.SelectedItem.Text)

                    .Append(Master.Factory.x35GlobalParam.TempFolder & "\" & GenerateOnePDF2Temp(Me.CurrentX31ID, strOutputFileName))
                    .DrawToWeb(IIf(strOutputFileName = "", "MARKTIME_REPORT.pdf", strOutputFileName), True)

                End With

        End Select
    End Sub

    Private Sub GenerateMerge(bolForceDownload As Boolean, bolSendByMail As Boolean)
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-merge1-" & Me.CurrentPrefix, Me.x31ID_Merge1.SelectedValue)
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-merge2-" & Me.CurrentPrefix, Me.x31ID_Merge2.SelectedValue)
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-merge3-" & Me.CurrentPrefix, Me.x31ID_Merge3.SelectedValue)
        Dim reps As New List(Of String)

        Dim s As String = ""
        If Me.CurrentX31ID > 0 Then
            s = GenerateOnePDF2Temp(Me.CurrentX31ID)
            If s <> "" Then reps.Add(s)
        End If
        If Me.x31ID_Merge1.SelectedValue <> "" Then
            s = GenerateOnePDF2Temp(CInt(Me.x31ID_Merge1.SelectedValue))
            If s <> "" Then reps.Add(s)
        End If
        If Me.x31ID_Merge2.SelectedValue <> "" Then
            s = GenerateOnePDF2Temp(CInt(Me.x31ID_Merge2.SelectedValue))
            If s <> "" Then reps.Add(s)
        End If
        If Me.x31ID_Merge3.SelectedValue <> "" Then
            s = GenerateOnePDF2Temp(CInt(Me.x31ID_Merge3.SelectedValue))
            If s <> "" Then reps.Add(s)
        End If
        If reps.Count = 0 Then Return

        InhaleLic()

        Dim doc1 As New ceTe.DynamicPDF.Merger.MergeDocument()
        doc1.Author = "MARKTIME"
        For Each strFile As String In reps
            doc1.Append(Master.Factory.x35GlobalParam.TempFolder & "\" & strFile)
        Next

        If bolSendByMail Then
            Dim strFileName As String = BO.BAS.GetGUID & ".pdf"
            doc1.Draw(Master.Factory.x35GlobalParam.TempFolder & "\" & strFileName)
            Server.Transfer("sendmail.aspx?prefix=" & Me.CurrentPrefix & "&pid=" & Master.DataPID.ToString & "&tempfile=" & strFileName, False)
        End If
        doc1.DrawToWeb("MARKTIME_REPORT.pdf", bolForceDownload)

    End Sub
    Private Sub MultiPidsGenerateMerge(bolForceDownload As Boolean, bolSendByMail As Boolean)
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-merge1-" & Me.CurrentPrefix, Me.x31ID_Merge1.SelectedValue)
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-merge2-" & Me.CurrentPrefix, Me.x31ID_Merge2.SelectedValue)
        Master.Factory.j03UserBL.SetUserParam("report_modal-x31id-merge3-" & Me.CurrentPrefix, Me.x31ID_Merge3.SelectedValue)

        InhaleLic()
        Dim doc1 As New ceTe.DynamicPDF.Merger.MergeDocument()
        doc1.Author = "MARKTIME"

        Dim a() As String = Split(Me.MultiPIDs, ",")
        For Each strPID As String In a
            Master.DataPID = CInt(strPID)
            
            Dim reps As New List(Of String)
            Dim s As String = ""
            If Me.CurrentX31ID > 0 Then
                s = GenerateOnePDF2Temp(Me.CurrentX31ID)
                If s <> "" Then reps.Add(s)
            End If
            If Me.x31ID_Merge1.SelectedValue <> "" Then
                s = GenerateOnePDF2Temp(CInt(Me.x31ID_Merge1.SelectedValue))
                If s <> "" Then reps.Add(s)
            End If
            If Me.x31ID_Merge2.SelectedValue <> "" Then
                s = GenerateOnePDF2Temp(CInt(Me.x31ID_Merge2.SelectedValue))
                If s <> "" Then reps.Add(s)
            End If
            If Me.x31ID_Merge3.SelectedValue <> "" Then
                s = GenerateOnePDF2Temp(CInt(Me.x31ID_Merge3.SelectedValue))
                If s <> "" Then reps.Add(s)
            End If
            

            For Each strFile As String In reps
                doc1.Append(Master.Factory.x35GlobalParam.TempFolder & "\" & strFile)
            Next
        Next

        Master.DataPID = 0

        ''If bolSendByMail Then
        ''    Dim strFileName As String = BO.BAS.GetGUID & ".pdf"
        ''    doc1.Draw(Master.Factory.x35GlobalParam.TempFolder & "\" & strFileName)
        ''    Server.Transfer("sendmail.aspx?prefix=" & Me.CurrentPrefix & "&pid=" & Master.DataPID.ToString & "&tempfile=" & strFileName, False)
        ''End If
        doc1.DrawToWeb("MARKTIME_REPORT_MULTIPLE.pdf", bolForceDownload)

    End Sub

    Private Function GenerateOnePDF2Temp(intX31ID As Integer, Optional strOutputFileName As String = "") As String
        Dim cRec As BO.x31Report = Master.Factory.x31ReportBL.Load(intX31ID)
        Dim strRepFullPath As String = Master.Factory.x35GlobalParam.UploadFolder
        If cRec.ReportFolder <> "" Then
            strRepFullPath += "\" & cRec.ReportFolder
        End If
        strRepFullPath += "\" & cRec.ReportFileName
        Dim cRep As New clsReportOnBehind()
        cRep.Query_RecordPID = Master.DataPID
        If Not ViewState("params") Is Nothing Then
            cRep.OtherParams = ViewState("params")
        End If
        If period1.Visible Then
            cRep.Query_DateFrom = period1.DateFrom
            cRep.Query_DateUntil = period1.DateUntil
        End If


        strOutputFileName = cRep.GenerateReport2Temp(Master.Factory, strRepFullPath, , strOutputFileName)
        If strOutputFileName = "" Then
            Master.Notify("Chyba při generování PDF.", NotifyLevel.ErrorMessage) : Return ""
        End If

        Return strOutputFileName
    End Function

    Private Sub cmdMergePDF_Download_Click(sender As Object, e As EventArgs) Handles cmdMergePDF_Download.Click
        If Me.MultiPIDs <> "" Then
            MultiPidsGenerateMerge(True, False)
        Else
            GenerateMerge(True, False)
        End If

    End Sub

    Private Sub cmdMergePDF_Preview_Click(sender As Object, e As EventArgs) Handles cmdMergePDF_Preview.Click
        If Me.MultiPIDs <> "" Then
            MultiPidsGenerateMerge(False, False)
        Else
            GenerateMerge(False, False)
        End If

    End Sub

    Private Sub report_modal_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If period1.Visible And period1.SelectedValue <> "" Then
            period1.BackColor = System.Drawing.Color.Red
        Else
            period1.BackColor = Nothing
        End If
    End Sub

    Private Sub cmdMergePDF_SendMail_Click(sender As Object, e As EventArgs) Handles cmdMergePDF_SendMail.Click
        If Me.MultiPIDs <> "" Then
            MultiPidsGenerateMerge(True, True)
        Else
            GenerateMerge(True, True)
        End If

    End Sub
End Class