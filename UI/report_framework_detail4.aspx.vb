Imports Winnovative.ExcelLib

Public Class report_framework_detail4
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

    Private Sub report_framework_detail4_Init(sender As Object, e As EventArgs) Handles Me.Init
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
                    .Add("report_framework_detail4-period")

                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                    period1.SelectedValue = .GetUserParam("report_framework_detail4-period")


                End With

                cmdSetting.Visible = .Factory.TestPermission(BO.x53PermValEnum.GR_Admin)
            End With

            If InhaleReport() = "" Then
                cmdGenerate.Visible = False
            End If
        End If
    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("report_framework_detail4-period", Me.period1.SelectedValue)
    End Sub

    Private Function InhaleReport() As String
        Dim cRec As BO.x31Report = Master.Factory.x31ReportBL.Load(Me.CurrentX31ID)
        If cRec Is Nothing Then
            Master.StopPage("Nelze načíst sestavu.<hr>" & Master.Factory.x31ReportBL.ErrorMessage)
            Return ""
        End If
        lblHeader.Text = cRec.x31Name
        If cRec.x31FormatFlag <> BO.x31FormatFlagENUM.XLSX Then
            Master.StopPage("NOT XLSX format.") : Return ""
        End If
        Dim strRepFullPath As String = Master.Factory.x35GlobalParam.UploadFolder
        If cRec.ReportFolder <> "" Then
            strRepFullPath += "\" & cRec.ReportFolder
        End If
        strRepFullPath += "\" & cRec.ReportFileName

        Dim cF As New BO.clsFile
        If Not cF.FileExist(strRepFullPath) Then
            Master.Notify("XLSX soubor šablony tiskové sestavy nelze načíst.", 2)
            Return ""
        End If


        Return strRepFullPath



        ''Dim intJ70ID As Integer = 0, bolQueryJ70 As Boolean = False

        ''If strXmlContent.IndexOf("@datfrom") > 0 Or strXmlContent.IndexOf("@datuntil") > 0 Or cRec.x31IsPeriodRequired Then
        ''    period1.Visible = True

        ''    xmlRepSource.Parameters.Add(New Parameter("datfrom", period1.DateFrom))
        ''    xmlRepSource.Parameters.Add(New Parameter("datuntil", period1.DateUntil))

        ''Else
        ''    period1.Visible = False
        ''End If



    End Function

    Private Function GenerateXLS(strSourceXlsFullPath As String) As String
        Dim cXLS As New clsExportToXls(Master.Factory)
        Dim sheetDef As ExcelWorksheet = cXLS.LoadSheet(strSourceXlsFullPath, 0, "marktime_definition")
        If sheetDef Is Nothing Then
            Master.Notify("XLS soubor neobsahuje sešit s názvem [marktime_definition].", NotifyLevel.ErrorMessage)
            Return ""
        End If
        Dim sheetData As ExcelWorksheet = Nothing
        Dim book As ExcelWorkbook = cXLS.LoadWorkbook(strSourceXlsFullPath)
        For i As Integer = 0 To book.Worksheets.Count - 1
            If LCase(book.Worksheets(i).Name) <> "marktime_definition" Then
                sheetData = book.Worksheets(i)
                Exit For
            End If
        Next
        If sheetData Is Nothing Then
            Master.Notify("XLS soubor neobsahuje volný datový sešit.", NotifyLevel.ErrorMessage)
            Return ""
        End If
        For i = 1 To 1000
            Select Case LCase(sheetDef.Item(i, 1).Text)
                Case "x31name", "x31code"
                Case Else
                    Dim strSQL As String = sheetDef.Item(i, 2).Value
                    Dim strRange As String = sheetDef.Item(i, 1).Text
                    If strSQL <> "" And strRange <> "" Then
                        Dim pars As New List(Of BO.PluginDbParameter)
                        pars.Add(New BO.PluginDbParameter("datfrom", period1.DateFrom))
                        pars.Add(New BO.PluginDbParameter("datuntil", period1.DateUntil))
                        Dim intRow As Integer = sheetData.Item(strRange).TopRowIndex
                        Dim intCol As Integer = sheetData.Item(strRange).LeftColumnIndex
                        Dim dt As DataTable = Master.Factory.pluginBL.GetDataTable(strSQL, pars)
                        cXLS.MergeSheetWithDataTable(sheetData, dt, intRow, intCol)
                    End If

            End Select
        Next
        book.Worksheets.RemoveWorksheet("marktime_definition")
        Dim strResult As String = cXLS.SaveAsFile(sheetData)
        Dim cF As New BO.clsFile
        Return cF.GetNameFromFullpath(strResult)
    End Function

    Private Sub report_framework_detail4_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        With Me.period1
            If .SelectedValue <> "" Then
                .BackColor = System.Drawing.Color.Red
            Else
                .BackColor = Nothing
            End If
        End With
    End Sub

    Private Sub cmdGenerate_Click(sender As Object, e As EventArgs) Handles cmdGenerate.Click
        Dim strTempFile As String = GenerateXLS(InhaleReport())
        If strTempFile <> "" Then
            Response.Redirect("binaryfile.aspx?tempfile=" & strTempFile)
        End If
    End Sub
End Class