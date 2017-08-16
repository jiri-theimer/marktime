Public Class import_object
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub import_object_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.SiteMenuValue = "dashboard"
        If Not Page.IsPostBack Then
            If Request.Item("prefix") = "" Or Request.Item("guid") = "" Then
                Master.StopPage("Na vstupu chybí identifikace vstupního objektu (prefix a guid).")
                Return
            End If
           

            hidGUID.Value = Request.Item("guid")
            hidPrefix.Value = Request.Item("prefix")
            Dim lis As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(hidGUID.Value)
            If lis.Count = 0 Then
                Master.StopPage("Objekt pro tento klíč neexistuje.")
            End If
            If lis.Where(Function(p) p.p85FreeText04 = "MS-Outlook").Count > 0 Then
                linkMSG.NavigateUrl = "binaryfile.aspx?tempfile=" & lis.Where(Function(p) p.p85FreeText04 = "MS-Outlook")(0).p85FreeText02
            Else
                linkMSG.Visible = False
            End If
            If lis.Where(Function(p) p.p85FreeText01 = "p41id").Count > 0 Then
                hidP41ID.Value = lis.Where(Function(p) p.p85FreeText01 = "p41id")(0).p85OtherKey1.ToString
            End If

            Dim strURL As String = ""
            Select Case hidPrefix.Value
                Case "p31"
                    lblTopHeader.Text = "Zapsat worksheet z MS-OUTLOOK..."
                    strURL = "p31_record.aspx?pid=0&guid_import=" & hidGUID.Value
                    If hidP41ID.Value <> "" Then
                        strURL += "&p41id=" & hidP41ID.Value
                    End If
                Case "o23"
                    lblTopHeader.Text = "Vytvořit dokument z MS-OUTLOOK..."
                    panO23.Visible = True
                    Dim lisX18 As IEnumerable(Of BO.x18EntityCategory) = Master.Factory.x18EntityCategoryBL.GetList(New BO.myQuery, BO.x29IdEnum._NotSpecified).Where(Function(p) p.x18IsManyItems = True)
                    Me.x18ID.DataSource = lisX18
                    Me.x18ID.DataBind()
                    If lisX18.Count = 0 Then
                        Master.Notify("V databázi ani jeden mně dostupný typ dokumentu.")
                    End If
                Case "p28"
                    lblTopHeader.Text = "Založit klienta z MS-OUTLOOK..."
                    strURL = "p28_record.aspx?pid=0&guid_import=" & hidGUID.Value
                Case "p56"
                    lblTopHeader.Text = "Vytvořit úkol z MS-OUTLOOK..."
                    strURL = "p56_record.aspx?pid=0&guid_import=" & hidGUID.Value
                    If hidP41ID.Value <> "" Then
                        strURL += "&p41id=" & hidP41ID.Value
                    End If
            End Select
            hidPopupUrl.Value = strURL
        End If

    End Sub

    Private Sub x18ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles x18ID.SelectedIndexChanged
        hidPopupUrl.Value = "o23_record.aspx?x18id=" & Me.x18ID.SelectedValue & "&guid_import=" & Me.hidGUID.Value
        If hidP41ID.Value <> "" Then
            hidPopupUrl.Value += "&p41id=" & hidP41ID.Value
        End If
    End Sub

    Private Sub import_object_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If hidPopupUrl.Value <> "" Then
            cmdPopup.Visible = True
        Else
            cmdPopup.Visible = False
        End If
    End Sub
End Class