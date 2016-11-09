Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Telerik.Web.UI

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class user_service
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

        Dim mq As New BO.myQueryJ02
        mq.SearchExpression = filterString
        mq.TopRecordsOnly = 50
        Select Case strFlag
            Case "all2"
                mq.IntraPersons = BO.myQueryJ02_IntraPersons._NotSpecified
            Case "all"
                'bez omezení - všechny osoby

            Case "p31_entry"    'pouze pro zapisování worksheet
                mq.SpecificQuery = BO.myQueryJ02_SpecificQuery.AllowedForWorksheetEntry
            Case "p48_entry"
                mq.SpecificQuery = BO.myQueryJ02_SpecificQuery.AllowedForP48Entry
            Case "searchbox"
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
                If factory.j03UserBL.GetUserParam("handler_search_person-bin", "") = "1" Then
                    mq.Closed = BO.BooleanQueryMode.NoQuery
                Else
                    mq.Closed = BO.BooleanQueryMode.FalseQuery
                End If
            Case Else
                mq.SpecificQuery = BO.myQueryJ02_SpecificQuery.AllowedForRead
        End Select


        Dim lis As IEnumerable(Of BO.j02Person) = factory.j02PersonBL.GetList(mq)
        result = New List(Of RadComboBoxItemData)(lis.Count)

        For Each usr As BO.j02Person In lis
            Dim itemData As New RadComboBoxItemData()
            itemData.Text = usr.FullNameDesc
            If usr.j07ID > 0 Then
                itemData.Text += " [" & usr.j07Name & "]"
            Else
                If usr.j02JobTitle <> "" Then itemData.Text += " [" & usr.j02JobTitle & "]"
            End If
            If usr.IsClosed Then itemData.Text = "<span class='radcomboitem_archive'>" & itemData.Text & "</span>"
            
            itemData.Value = usr.PID.ToString
            result.Add(itemData)
        Next

        Return result.ToArray
    End Function

End Class