Public Class mytags
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RefreshData(factory As BL.Factory, strPrefix As String, intRecordPID As Integer)
        hidPrefix.Value = strPrefix
        hidRecordPID.Value = intRecordPID.ToString
        rp1.DataSource = factory.o51TagBL.GetList_o52(strPrefix, intRecordPID)
        rp1.DataBind()
        If rp1.Items.Count > 0 Then
            cmdTags.Text = "Štítky"
        Else
            cmdTags.Text = "Zatím žádné štítky"
        End If

    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim c As BO.o52TagBinding = CType(e.Item.DataItem, BO.o52TagBinding)
        With CType(e.Item.FindControl("o51Name"), Label)
            .Text = c.o51Name
            .ToolTip = c.o52UserUpdate & "/" & BO.BAS.FD(c.o52DateUpdate, True)
        End With
    End Sub
End Class