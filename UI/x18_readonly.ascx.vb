Public Class x18_readonly
    Inherits System.Web.UI.UserControl
    Private _lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding)
    Private Class _qry
        Public Property x18ID As Integer
        Public Property x18Name As String
        Public Property x18Icon As String
    End Class

    Public Property IsShowLinks As Boolean
        Get
            Return BO.BAS.BG(hidIsLinks.Value)
        End Get
        Set(value As Boolean)
            hidIsLinks.Value = BO.BAS.GB(value)
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RefreshData(x29id As BO.x29IdEnum, intRecordPID As Integer, lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding))
        lisX19 = lisX19.Where(Function(p) p.x20EntityPageFlag < BO.x20EntityPageENUM.NotUsed)
        Me.hidRecordPID.Value = intRecordPID.ToString
        Me.hidX29ID.Value = CInt(x29id).ToString

        _lisX19 = lisX19

        Dim qry = From p In _lisX19 Select p.x18ID, p.x18Name, p.x18Icon Distinct
        Dim lis As New List(Of _qry)
        For Each rec In qry
            Dim c As New _qry
            c.x18ID = rec.x18ID
            c.x18Name = rec.x18Name
            c.x18Icon = rec.x18Icon
            lis.Add(c)
        Next




        ''Dim x18IDs As List(Of Integer) = lisX19.Select(Function(p) p.x18ID).Distinct.ToList
        'rp1.DataSource = x18IDs
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
        'Dim intX18ID As Integer = CType(e.Item.DataItem, Integer)
        'CType(e.Item.FindControl("x18Name"), Label).Text = _lisX19.Where(Function(p) p.x18ID = intX18ID).First.x18Name & ":"
        'For Each c In _lisX19.Where(Function(p) p.x18ID = intX18ID)

        'Next
        'CType(e.Item.FindControl("items"), Label).Text = String.Join(", ", _lisX19.Where(Function(p) p.x18ID = intX18ID).Select(Function(p) p.x25Name))
        CType(e.Item.FindControl("rpItems"), Repeater).DataSource = _lisX19.Where(Function(p) p.x18ID = c.x18ID And p.x20EntityPageFlag = BO.x20EntityPageENUM.Hyperlink)
        CType(e.Item.FindControl("rpItems"), Repeater).DataBind()
        CType(e.Item.FindControl("rpLabels"), Repeater).DataSource = _lisX19.Where(Function(p) p.x18ID = c.x18ID And p.x20EntityPageFlag = BO.x20EntityPageENUM.Label)
        CType(e.Item.FindControl("rpLabels"), Repeater).DataBind()
    End Sub
End Class