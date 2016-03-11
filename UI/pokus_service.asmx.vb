Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Telerik.Web.UI

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class pokus_service
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetItems(context As SearchBoxContext) As SearchBoxItemData()
        Dim filterString As String = context.Text

        Dim result As New List(Of SearchBoxItemData)()
        For i As Integer = 1 To 10
            Dim itemData As New SearchBoxItemData()
            itemData.Text = "řádek " & i.ToString
            itemData.Value = i.ToString
            result.Add(itemData)
        Next

       
        Return result.ToArray()
    End Function

End Class