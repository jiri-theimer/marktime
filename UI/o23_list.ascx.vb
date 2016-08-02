Public Class o23_list
    Inherits System.Web.UI.UserControl
    Private _rowsCount As Integer = 0
    Public Property EntityX29ID As BO.x29IdEnum
        Get
            If Me.hidX29ID.Value <> "" Then
                Return CInt(Me.hidX29ID.Value)
            Else
                Return BO.x29IdEnum._NotSpecified
            End If
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidX29ID.Value = CInt(value).ToString
        End Set
    End Property
    Public ReadOnly Property RowsCount As Integer
        Get
            Return rpO23.Items.Count
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RefreshData(lisO23 As IEnumerable(Of BO.o23Notepad), intDataRecordPID As Integer)
        _rowsCount = lisO23.Count

        rpO23.DataSource = lisO23
        rpO23.DataBind()
        Me.hidInhaledDataPID.Value = intDataRecordPID.ToString

    End Sub

    Private Sub rpO23_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rpO23.ItemCommand
        If e.CommandName = "go2module" Then
            Response.Redirect("o23_framework.aspx?pid=" & e.CommandArgument)
        End If
    End Sub

    Private Sub rpO23_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpO23.ItemDataBound
        Dim cRec As BO.o23Notepad = CType(e.Item.DataItem, BO.o23Notepad)
        CType(e.Item.FindControl("clue_o23"), HyperLink).Attributes.Item("rel") = "clue_o23_record.aspx?pid=" & cRec.PID.ToString
        With CType(e.Item.FindControl("img1"), Image)
            If cRec.o24IsBillingMemo Then
                .ImageUrl = "Images/billing.png" : .Visible = True
            Else
                If cRec.o23IsEncrypted Then
                    .ImageUrl = "Images/lock.png"
                Else
                    .Visible = False
                    CType(e.Item.FindControl("o23Name"), HyperLink).Style.Item("margin-left") = "16px"
                End If
            End If
           
        End With
       
        With CType(e.Item.FindControl("o24Name"), Label)
            .Text = cRec.o24Name
        End With
        With CType(e.Item.FindControl("o23Name"), HyperLink)
            If cRec.o23Name = "" Then
                .Text = cRec.o23Code
            Else
                .Text = cRec.o23Name
            End If
            .NavigateUrl = "javascript:o23_record(" & cRec.PID.ToString & ")"
            .ToolTip = cRec.UserUpdate & "/" & BO.BAS.FD(cRec.DateUpdate, True)
        End With
        ''CType(e.Item.FindControl("TimeStamp"), Label).Text = cRec.UserUpdate & "/" & BO.BAS.FD(cRec.DateUpdate, True)
        If _rowsCount < 5 And Not cRec.o23BodyPlainText Is Nothing Then
            If cRec.o23BodyPlainText.Length > 0 And Not cRec.o23IsEncrypted Then
                With CType(e.Item.FindControl("place1"), PlaceHolder)
                    .Controls.Add(New LiteralControl("<div><i>" & BO.BAS.CrLfText2Html(cRec.o23BodyPlainText) & "</i></div>"))
                End With
            End If
        End If
    End Sub
End Class