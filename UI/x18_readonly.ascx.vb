﻿Public Class x18_readonly
    Inherits System.Web.UI.UserControl
    Private _lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding)
    Private Class _qry
        Public Property x18ID As Integer
        Public Property x18Name As String
        Public Property x18Icon As String
        Public Property ShowAddNewLink As Boolean = False
    End Class

    
    Public ReadOnly Property ContainsAnyData As Boolean
        Get
            If rp1.Items.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RefreshData(factory As BL.Factory, x29id As BO.x29IdEnum, intRecordPID As Integer, Optional bolWithoutLinksAndButtons As Boolean = False)
        Dim lisX20X18 As IEnumerable(Of BO.x20_join_x18) = factory.x18EntityCategoryBL.GetList_x20_join_x18(x29id).Where(Function(p) p.x20EntityPageFlag < BO.x20EntityPageENUM.NotUsed)
        If lisX20X18.Count = 0 Then
            Return  'pro entitu nejsou vazby k zobrazení
        End If
        hidNoLinksAndButtons.Value = BO.BAS.GB(bolWithoutLinksAndButtons)
        Dim lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding) = factory.x18EntityCategoryBL.GetList_X19(x29id, intRecordPID, "", Nothing)

        lisX19 = lisX19.Where(Function(p) p.x20EntityPageFlag < BO.x20EntityPageENUM.NotUsed)
        Me.hidRecordPID.Value = intRecordPID.ToString
        Me.hidX29ID.Value = CInt(x29id).ToString
        hidMasterPrefix.Value = BO.BAS.GetDataPrefix(x29id)

        _lisX19 = lisX19

        Dim qry = From p In _lisX19 Select p.x18ID, p.x18Name, p.x18Icon Distinct
        Dim lis As New List(Of _qry)
        For Each rec In qry
            Dim c As New _qry
            c.x18ID = rec.x18ID
            c.x18Name = rec.x18Name
            c.x18Icon = rec.x18Icon
            If lisX20X18.Where(Function(p) p.x18ID = rec.x18ID And p.x20EntityPageFlag = BO.x20EntityPageENUM.HyperlinkPlusNew).Count > 0 Then
                c.ShowAddNewLink = True
            End If
            lis.Add(c)
        Next


        rp1.DataSource = lis
        rp1.DataBind()

    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim c As _qry = CType(e.Item.DataItem, _qry)
        With CType(e.Item.FindControl("lblHeader"), Label)
            If c.x18Icon <> "" Then                
                .Text = "<img src='" & c.x18Icon & "' alt='" & c.x18Name & "' title='" & c.x18Name & "'/>"
            End If
        End With
        With CType(e.Item.FindControl("cmdNew"), HtmlButton)
            .Visible = c.ShowAddNewLink
            If c.ShowAddNewLink Then

                .Attributes.Item("onclick") = "r25new(" & c.x18ID.ToString & ")"
                If c.x18Icon = "" Then
                    .InnerHtml = "<img src='Images/new.png' />" & BO.BAS.OM3(c.x18Name, 18)
                    .Attributes.Item("title") = "Nový záznam"
                Else
                    .Attributes.Item("title") = String.Format("Nový záznam [{0}]", c.x18Name)
                End If
            End If
        End With
        
        'Dim intX18ID As Integer = CType(e.Item.DataItem, Integer)
        'CType(e.Item.FindControl("x18Name"), Label).Text = _lisX19.Where(Function(p) p.x18ID = intX18ID).First.x18Name & ":"
        'For Each c In _lisX19.Where(Function(p) p.x18ID = intX18ID)

        'Next
        'CType(e.Item.FindControl("items"), Label).Text = String.Join(", ", _lisX19.Where(Function(p) p.x18ID = intX18ID).Select(Function(p) p.x25Name))
        CType(e.Item.FindControl("rpItems"), Repeater).DataSource = _lisX19.Where(Function(p) p.x18ID = c.x18ID And (p.x20EntityPageFlag = BO.x20EntityPageENUM.Hyperlink Or p.x20EntityPageFlag = BO.x20EntityPageENUM.HyperlinkPlusNew))
        CType(e.Item.FindControl("rpItems"), Repeater).DataBind()
        CType(e.Item.FindControl("rpLabels"), Repeater).DataSource = _lisX19.Where(Function(p) p.x18ID = c.x18ID And p.x20EntityPageFlag = BO.x20EntityPageENUM.Label)
        CType(e.Item.FindControl("rpLabels"), Repeater).DataBind()
    End Sub
End Class