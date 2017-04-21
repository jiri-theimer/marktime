Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml

Public Class flexibee
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub flexibee_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .Factory.j03UserBL.InhaleUserParams("periodcombo-custom_query")
                period1.SetupData(Master.Factory, .Factory.j03UserBL.GetUserParam("periodcombo-custom_query"))

            End With
        End If
    End Sub

    Private Sub cmdExport_Click(sender As Object, e As EventArgs) Handles cmdExport.Click
        Dim doc As New XmlDocument
        Dim strInputDIR As String = BO.ASS.GetApplicationRootFolder & "\Plugins"

        Dim strSQL As String = "select a.*,j27.j27Code,p28sup.*"
        strSQL += " FROM p31Worksheet a INNER JOIN p32Activity p32 ON a.p32ID=p32.p32ID INNER JOIN p34ActivityGroup p34 ON p32.p34ID=p34.p34ID"
        strSQL += " LEFT OUTER JOIN p28ID_Supplier p28sup ON a.p28ID_Supplier=p28sup.p28ID LEFT OUTER JOIN j27Currency j27 ON a.j27ID_Billing_Orig=j27.j27ID"
        strSQL += " WHERE a.p31Date BETWEEN @d1 AND @d2 AND p34.p33ID IN (2,5) AND p34.p34IncomeStatementFlag=1"
        strSQL += " ORDER BY a.p31Date"

        Dim pars As New List(Of BO.PluginDbParameter)
        pars.Add(New BO.PluginDbParameter("d1", period1.DateFrom))
        pars.Add(New BO.PluginDbParameter("d2", period1.DateUntil))

        Dim dt As DataTable = Master.Factory.pluginBL.GetDataTable(strSQL, pars)

        Dim nd As XmlNode, s As String, ndHeader As XmlNode = Nothing, strFileName As String = ""

        Try
            doc.Load(strInputDIR & "\flexibee_vzor_faktura_prijata_mini.xml")

        Catch ex As Exception
            Master.Notify(ex.Message)
            Return
        End Try

        For Each dbRow In dt.Rows
            nd = doc.ChildNodes(1)

            nd.Attributes("application").Value = "MARKTIME"

        Next

        ''doc.Save(strExportDir & "\" & strFileName)
    End Sub

    Private Function GetChild(ByVal ndMaster As XmlNode, ByVal strName As String) As XmlNode
        Dim i As Integer
        For i = 0 To ndMaster.ChildNodes.Count - 1
            If LCase(ndMaster.ChildNodes(i).Name) = LCase(strName) Then
                Return ndMaster.ChildNodes(i)
            End If
        Next
        Return Nothing
    End Function
End Class