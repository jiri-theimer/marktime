Public Class contactpersons
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property IsShowClueTip As Boolean
        Get
            Return BO.BAS.BG(Me.hidIsShowClueTip.Value)
        End Get
        Set(value As Boolean)
            Me.hidIsShowClueTip.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public Sub FillData(lisJ02 As IEnumerable(Of BO.j02Person))
        rpP30.DataSource = lisJ02
        rpP30.DataBind()

    End Sub

    Private Sub rpP30_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpP30.ItemDataBound
        Dim cRec As BO.j02Person = CType(e.Item.DataItem, BO.j02Person)
        With CType(e.Item.FindControl("j02Email"), HyperLink)
            If cRec.j02Email <> "" Then
                .Text = cRec.j02Email
                .NavigateUrl = "mailto:" & cRec.j02Email
            Else
                .Visible = False
            End If
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        With CType(e.Item.FindControl("Person"), Label)
            .Text = cRec.FullNameDesc
            If cRec.IsClosed Then .Font.Strikeout = True
        End With
        With CType(e.Item.FindControl("j02JobTitle"), Label)
            .Text = cRec.j02JobTitle
        End With
        With CType(e.Item.FindControl("j02Mobile"), Label)
            .Text = cRec.j02Mobile
        End With
        If Me.IsShowClueTip Then
            CType(e.Item.FindControl("clue_j02"), HyperLink).Attributes("rel") = "clue_j02_record.aspx?pid=" & cRec.PID.ToString
        Else
            e.Item.FindControl("clue_j02").Visible = False
        End If

    End Sub
End Class