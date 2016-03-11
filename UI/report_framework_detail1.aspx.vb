Imports Telerik.Reporting

Public Class report_framework_detail1
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Property CurrentX31ID As Integer
        Get
            Return BO.BAS.IsNullInt(hidCurX31ID.Value)
        End Get
        Set(value As Integer)
            hidCurX31ID.Value = value.ToString
        End Set
    End Property
    Private Sub report_framework_detail1_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                Me.CurrentX31ID = BO.BAS.IsNullInt(Request.Item("x31id"))
                If Me.CurrentX31ID = 0 Then
                    .StopPage("x31id missing.")
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("periodcombo-custom_query")
                    .Add("report_framework_detail1-period")

                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("report_framework_detail1-period")


                End With

                cmdSetting.Visible = .Factory.TestPermission(BO.x53PermValEnum.GR_Admin)
            End With

            RenderReport()
        End If
    End Sub

    
    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("report_framework_detail1-period", Me.period1.SelectedValue)
        RenderReport()
    End Sub

    Private Sub RenderReport()
        Dim cRec As BO.x31Report = Master.Factory.x31ReportBL.Load(Me.CurrentX31ID)
        If cRec Is Nothing Then
            Master.StopPage("Nelze načíst sestavu.<hr>" & Master.Factory.x31ReportBL.ErrorMessage)
            Return
        End If
        lblHeader.Text = cRec.x31Name
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

        Dim intJ70ID As Integer = 0, bolQueryJ70 As Boolean = False
        If strXmlContent.IndexOf("1=1") > 0 Then
            'sestava má vztah k návrháři filtrů
            ''intJ70ID = BO.BAS.IsNullInt(query_j70id.SelectedValue)
            ''bolQueryJ70 = True
        End If

        ''If strXmlContent.IndexOf("#query_alias#") > 0 Then
        ''    If intJ70ID <> 0 Then
        ''        strXmlContent = Replace(strXmlContent, "#query_alias#", "Filtr: " & Master.Factory.j70SavedBasicQueryBL.GetAliasCondition(intJ70ID))
        ''    Else
        ''        strXmlContent = Replace(strXmlContent, "#query_alias#", "")
        ''    End If
        ''End If
        Dim xmlRepSource As New Telerik.Reporting.XmlReportSource()
        xmlRepSource.Xml = strXmlContent

        If strXmlContent.IndexOf("@datfrom") > 0 Or strXmlContent.IndexOf("@datuntil") > 0 Or cRec.x31IsPeriodRequired Then
            period1.Visible = True
            'uriReportSource.Parameters.Add(New Parameter("datFrom", period1.DateFrom))
            'uriReportSource.Parameters.Add(New Parameter("datUntil", period1.DateUntil))

            xmlRepSource.Parameters.Add(New Parameter("datfrom", period1.DateFrom))
            xmlRepSource.Parameters.Add(New Parameter("datuntil", period1.DateUntil))

        Else
            period1.Visible = False
        End If

        ''If intJ70ID <> 0 And strXmlContent.IndexOf("1=1") > 0 Then
        ''    'externí filtr z návrháře
        ''    Dim strW As String = Master.Factory.j70SavedBasicQueryBL.CompleteReportSqlQuerySource(intJ70ID, "xa")
        ''    If strW <> "" Then
        ''        Dim strInSQL As String = "a.a01ID IN (SELECT xa.a01ID FROM a01Event xa WHERE " & strW & ")"
        ''        xmlRepSource.Xml = Replace(xmlRepSource.Xml, "1=1", strInSQL)
        ''    End If

        ''End If
        ''If strXmlContent.IndexOf("2=2") > 0 Then
        ''    Dim strW As String = GetLimitWHERE_ByUser() 'externí podmínka s omezením na aplikační roli
        ''    If strW <> "" Then
        ''        Dim strInSQL As String = "a.a01ID IN (SELECT xz.a01ID FROM a01Event xz WHERE " & strW & ")"
        ''        xmlRepSource.Xml = Replace(xmlRepSource.Xml, "2=2", strInSQL)
        ''    End If

        ''End If

        ''lblQuery.Visible = bolQueryJ70
        ''query_j70id.Visible = bolQueryJ70

     

        rv1.ReportSource = xmlRepSource

    End Sub

    Private Sub report_framework_detail1_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = System.Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    

    

 
End Class