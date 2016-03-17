Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Telerik.Web.UI

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class contact_service
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

        Dim mq As New BO.myQueryP28
        mq.SearchExpression = filterString
        mq.TopRecordsOnly = 50
        mq.SpecificQuery = BO.myQueryP28_SpecificQuery.AllowedForRead

        Select Case strFlag
            Case "nondraft"
                mq.QuickQuery = BO.myQueryP28_QuickQuery.NonDraftCLients
            Case "draft"
                mq.QuickQuery = BO.myQueryP28_QuickQuery.DraftClients
            Case "client"
                mq.CanBeClient = BO.BooleanQueryMode.TrueQuery
            Case "supplier"
                mq.CanBeSupplier = BO.BooleanQueryMode.TrueQuery
        End Select

        Dim lis As IEnumerable(Of BO.p28Contact) = factory.p28ContactBL.GetList(mq)
        result = New List(Of RadComboBoxItemData)(lis.Count)

        For Each rec As BO.p28Contact In lis
            Dim itemData As New RadComboBoxItemData()
            With rec
                If .p28IsCompany Then
                    itemData.Text = rec.p28CompanyName
                Else
                    itemData.Text = rec.p28Name
                End If
            End With

            If rec.p29ID > 0 Then
                itemData.Text += " [" & rec.p29Name & "]"

            End If
            If rec.p28IsDraft Then itemData.Text += " DRAFT"


            itemData.Value = rec.PID.ToString
            result.Add(itemData)
        Next

        Return result.ToArray
    End Function

End Class