Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Telerik.Web.UI

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class fulltext_service
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function LoadComboData(ByVal context As Object) As RadComboBoxItemData()
        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim filterString As String = (DirectCast(contextDictionary("filterstring"), String)).ToLower()
        Dim strFlag As String = (DirectCast(contextDictionary("flag"), String))


        Dim factory As BL.Factory = Nothing

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            Dim nic As List(Of RadComboBoxItemData) = New List(Of RadComboBoxItemData)(1)
            Dim xx As New RadComboBoxItemData()
            xx.Text = "Nelze ověřit přihlášeného uživatele"
            nic.Add(xx)
            Return nic.ToArray
        End If

        Dim lisPars As New List(Of String)
        lisPars.Add("handler_search_fulltext-main")
        lisPars.Add("handler_search_fulltext-invoice")
        lisPars.Add("handler_search_fulltext-task")
        lisPars.Add("handler_search_fulltext-worksheet")
        lisPars.Add("handler_search_fulltext-doc")

        Dim result As List(Of RadComboBoxItemData) = Nothing

        Dim input As New BO.FullTextQueryInput
        With factory.j03UserBL
            input.IncludeMain = BO.BAS.BG(.GetUserParam("handler_search_fulltext-main", "1"))
            input.IncludeInvoice = BO.BAS.BG(.GetUserParam("handler_search_fulltext-invoice", "1"))
            input.IncludeDocument = BO.BAS.BG(.GetUserParam("handler_search_fulltext-doc", "0"))
            input.IncludeWorksheet = BO.BAS.BG(.GetUserParam("handler_search_fulltext-worksheet", "1"))
            input.IncludeTask = BO.BAS.BG(.GetUserParam("handler_search_fulltext-task", "0"))
        End With

        Dim lis As List(Of BO.FullTextRecord) = factory.ftBL.FulltextSearch(input)
        result = New List(Of RadComboBoxItemData)(lis.Count)
        Dim itemData As New RadComboBoxItemData()
        itemData.Enabled = False
        Select Case lis.Count
            Case 0
                If Len(filterString) > 0 And Len(filterString) < 15 Then itemData.Text = "Ani jeden úkol pro zadanou podmínku."
            Case Is >= input.TopRecs
                itemData.Text = String.Format("Podmínce vyhovuje více než {0} výskytů. Je třeba zpřesnit podmínku hledání.", input.TopRecs)
            Case Else
                If filterString <> "" Then itemData.Text = String.Format("Počet vyhovujících výskytů: {0}.", lis.Count.ToString)
        End Select
        If itemData.Text <> "" Then result.Add(itemData)

        For Each c As BO.FullTextRecord In lis
            itemData = New RadComboBoxItemData()

            itemData.Text = c.Field & ": " & c.RecValue
            item.data.text += " ..." & c.RecName


            itemData.Value = c.RecPid.ToString
            result.Add(itemData)
        Next

    End Function


End Class