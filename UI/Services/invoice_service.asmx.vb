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
Public Class invoice_service
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


        Dim result As List(Of RadComboBoxItemData) = Nothing

        Dim mq As New BO.myQueryP91
        mq.SearchExpression = filterString
        mq.TopRecordsOnly = 50
        Select Case strFlag
            Case "searchbox"
                mq.SpecificQuery = BO.myQueryP91_SpecificQuery.AllowedForRead
                mq.Closed = BO.BooleanQueryMode.NoQuery
            Case Else
        End Select


        Dim lis As IEnumerable(Of BO.p91Invoice) = factory.p91InvoiceBL.GetList(mq)
        result = New List(Of RadComboBoxItemData)(lis.Count)

        For Each cRec As BO.p91Invoice In lis
            Dim itemData As New RadComboBoxItemData()
            With cRec
                itemData.Text = .p91Code
                If .p91Client = "" Then
                    itemData.Text += " " & .p28Name
                Else
                    itemData.Text += " " & .p91Client
                End If
                itemData.Text += " (" & BO.BAS.FN(.p91Amount_TotalDue) & " " & .j27Code & ")"
                If .p91IsDraft Then itemData.Text += " DRAFT"

                itemData.Value = .PID.ToString
            End With

            result.Add(itemData)
        Next

        Return result.ToArray
    End Function

End Class