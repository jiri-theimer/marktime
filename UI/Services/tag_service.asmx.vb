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
Public Class tag_service
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function LoadComboData(ByVal context As Object) As AutoCompleteBoxData
        'Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        'Dim filterString As String = DirectCast(context, Dictionary(Of String, Object))("filterstring").ToString()
        Dim filterString As String = DirectCast(context, Dictionary(Of String, Object))("Text").ToString()

        ''Dim filterString As String = DirectCast(context, Dictionary(Of String, Object))("filterstring").ToString()
        ''Dim strFlag As String = (DirectCast(contextDictionary("flag"), String)
        Dim strFlag As String = ""

        Dim factory As BL.Factory = Nothing

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            Dim nic As List(Of AutoCompleteBoxItemData) = New List(Of AutoCompleteBoxItemData)(1)
            Dim xx As New AutoCompleteBoxItemData()
            xx.Text = "Nelze ověřit přihlášeného uživatele"
            nic.Add(xx)
            Dim resNic As New AutoCompleteBoxData()
            resNic.Items = nic.ToArray()
            Return resNic
        End If

        Dim result As List(Of AutoCompleteBoxItemData) = Nothing

        Dim mq As New BO.myQuery
        mq.SearchExpression = filterString
        mq.TopRecordsOnly = 50


        Dim lis As IEnumerable(Of BO.o51Tag) = factory.o51TagBL.GetList(mq)
        result = New List(Of AutoCompleteBoxItemData)(lis.Count)
        Dim itemData As New AutoCompleteBoxItemData()
        itemData.Enabled = False
        Select Case lis.Count
            Case 0
                If (Len(filterString) > 0 And Len(filterString) < 15) Then itemData.Text = "Ani jeden štítek pro zadanou podmínku."
            Case Is >= mq.TopRecordsOnly
                itemData.Text = String.Format("Nalezeno více než {0} štítků. Je třeba zpřesnit podmínku hledání.", mq.TopRecordsOnly.ToString)
            Case Else
                If filterString <> "" Then itemData.Text = String.Format("Počet nalezených štítků: {0}.", lis.Count.ToString)
        End Select
        If itemData.Text <> "" Then result.Add(itemData)


        For Each c As BO.o51Tag In lis
            itemData = New AutoCompleteBoxItemData()
            itemData.Value = c.PID.ToString
            Select Case strFlag
                Case "1"
                    'zobrazovat u štítku timestamp
                    itemData.Text = c.o51Name & " (" & c.Owner & "/" & BO.BAS.FD(c.DateInsert) & ")"
                Case Else
                    itemData.Text = c.o51Name
            End Select



            itemData.Value = c.PID.ToString
            result.Add(itemData)
        Next
        Dim res As New AutoCompleteBoxData()
        res.Items = result.ToArray()

        Return res
    End Function

End Class