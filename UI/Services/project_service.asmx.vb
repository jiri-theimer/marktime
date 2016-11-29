﻿Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Telerik.Web.UI

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class project_service
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function LoadComboData(ByVal context As Object) As RadComboBoxItemData()
        Dim contextDictionary As IDictionary(Of String, Object) = DirectCast(context, IDictionary(Of String, Object))
        Dim filterString As String = (DirectCast(contextDictionary("filterstring"), String)).ToLower()
        Dim strFlag As String = (DirectCast(contextDictionary("flag"), String))
        Dim strJ02ID_Explicit As String = (DirectCast(contextDictionary("j02id_explicit"), String))
        
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

        Dim intJ02ID_Explicit As Integer = factory.SysUser.j02ID
        factory.j03UserBL.InhaleUserParams("handler_search_project-toprecs", "handler_search_project-bin")


        Dim mq As New BO.myQueryP41
        mq.SearchExpression = filterString
        With factory.j03UserBL
            mq.TopRecordsOnly = BO.BAS.IsNullInt(.GetUserParam("handler_search_project-toprecs", "20"))
        End With




        If strJ02ID_Explicit <> "" Then
            mq.j02ID_ExplicitQueryFor = BO.BAS.IsNullInt(strJ02ID_Explicit)
        Else
            mq.j02ID_ExplicitQueryFor = factory.SysUser.j02ID
        End If

        Select Case strFlag
            Case "p31_entry"
                'omezit pouze na projekty, ke kterým má osoba oprávnění zapisovat worksheet
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
            Case "p48"
                'zapisovat operativní plán
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForOperPlanEntry
            Case "createtask"
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForCreateTask
            Case "createinvoice"
            Case "searchbox"
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
                If factory.j03UserBL.GetUserParam("handler_search_project-bin", "") = "1" Then
                    mq.Closed = BO.BooleanQueryMode.NoQuery
                Else
                    mq.Closed = BO.BooleanQueryMode.FalseQuery
                End If
            Case Else
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead

        End Select

        Dim lis As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mq)

        Dim result As List(Of RadComboBoxItemData) = Nothing

        result = New List(Of RadComboBoxItemData)(lis.Count)

        For Each rec As BO.p41Project In lis
            Dim itemData As New RadComboBoxItemData()
            With rec
                ''itemData.Text = .FullName & " [" & .p41Code & "]"
                itemData.Text = .ProjectWithMask(factory.SysUser.j03ProjectMaskIndex)
                ''If .IsClosed Then itemData.Attributes.Item("class") = "radcomboitem_archive"
                If .IsClosed Then itemData.Text = "<span class='radcomboitem_archive'>" & itemData.Text & "</span>"
                If .p41IsDraft Then
                    If .p41Code.LastIndexOf("DRAFT") < 0 Then itemData.Text += " DRAFT"
                End If
                itemData.Value = .PID.ToString
            End With


            result.Add(itemData)
        Next

        Return result.ToArray

        
    End Function



    Public Function LoadSearchBoxData(context As SearchBoxContext) As SearchBoxItemData()
        Dim cF As New BO.clsFile
        'cF.SaveText2File("c:\temp\hovado.txt", "Jsem tu")
        Dim filterString As String = context.Text.ToLower

        Dim strFlag As String = context.UserContext("flag").ToString
        Dim strJ02ID_Explicit As String = context.UserContext("j02id_explicit").ToString


        Dim factory As BL.Factory = Nothing

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If

        If factory Is Nothing Then
            Dim nic As List(Of SearchBoxItemData) = New List(Of SearchBoxItemData)(1)
            Dim xx As New SearchBoxItemData()
            xx.Text = "Nelze ověřit přihlášeného uživatele"
            nic.Add(xx)
            Return nic.ToArray
        End If

        Dim intJ02ID_Explicit As Integer = factory.SysUser.j02ID

        Dim mq As New BO.myQueryP41
        mq.SearchExpression = filterString
        mq.TopRecordsOnly = 80

        If strJ02ID_Explicit <> "" Then
            mq.j02ID_ExplicitQueryFor = BO.BAS.IsNullInt(strJ02ID_Explicit)
        Else
            mq.j02ID_ExplicitQueryFor = factory.SysUser.j02ID
        End If

        Select Case strFlag
            Case "p31_entry"
                'omezit pouze na projekty, ke kterým má osoba oprávnění zapisovat worksheet
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForWorksheetEntry
            Case "createtask"
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForCreateTask
            Case Else
                mq.SpecificQuery = BO.myQueryP41_SpecificQuery.AllowedForRead
        End Select

        Dim lis As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mq)

        Dim result As List(Of SearchBoxItemData) = Nothing

        result = New List(Of SearchBoxItemData)(lis.Count)

        For Each rec As BO.p41Project In lis
            Dim itemData As New SearchBoxItemData()
            With rec
                itemData.Text = .FullName & " [" & .p41Code & "]" & mq.j02ID_ExplicitQueryFor

            End With

            itemData.Value = rec.PID.ToString
            result.Add(itemData)
        Next

        Return result.ToArray


    End Function

    Public Function GetItems(context As SearchBoxContext) As SearchBoxItemData()
        Dim filterString As String = context.Text

        Dim factory As BL.Factory = Nothing

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If

        If factory Is Nothing Then
            Dim nic As List(Of SearchBoxItemData) = New List(Of SearchBoxItemData)(1)
            Dim xx As New SearchBoxItemData()
            xx.Text = "Nelze ověřit přihlášeného uživatele"
            nic.Add(xx)
            Return nic.ToArray
        End If

        Dim intJ02ID_Explicit As Integer = factory.SysUser.j02ID

        Dim mq As New BO.myQueryP41
        mq.SearchExpression = filterString
        mq.TopRecordsOnly = 10


        Dim result As New List(Of SearchBoxItemData)()
        Dim lis As IEnumerable(Of BO.p41Project) = factory.p41ProjectBL.GetList(mq)

        For Each rec As BO.p41Project In lis
            Dim itemData As New SearchBoxItemData()
            With rec
                itemData.Text = .FullName & " [" & .p41Code & "]"
                itemData.Value = .PID.ToString
            End With


            result.Add(itemData)
        Next

        Return result.ToArray()
    End Function
End Class