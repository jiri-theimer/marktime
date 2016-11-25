Public Class billingmemo
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub HideTogleButton()
        Me.panO23.Visible = False
    End Sub
    Public Function o23Rows() As Integer
        Return notepad1.RowsCount
    End Function
    Public ReadOnly Property IsEmpty As Boolean
        Get
            If notepad1.RowsCount > 0 Or Me.BillingMemo.Text <> "" Then Return False Else Return True
        End Get
    End Property
    Public Sub RefreshData(factory As BL.Factory, strPrefix As String, intRecordPID As Integer)
        If intRecordPID = 0 Or strPrefix = "" Then Return

        Dim mq As New BO.myQueryP31
        Dim mqO23 As New BO.myQueryO23
        notepad1.EntityX29ID = BO.BAS.GetX29FromPrefix(strPrefix)

        panO23.Visible = True
        Select Case strPrefix
            Case "p28"
                mqO23.p28ID = intRecordPID
                Me.BillingMemo.Text = LoadTheRightBillingMemo(factory, strPrefix, intRecordPID)
            Case "p41"
                mqO23.p41ID = intRecordPID
                Me.BillingMemo.Text = LoadTheRightBillingMemo(factory, strPrefix, intRecordPID)
            Case "j02"
                mqO23.j02ID = intRecordPID
            Case Else
                panO23.Visible = False
        End Select
        If Me.BillingMemo.Text <> "" Then
            img1.Visible = True
        Else
            img1.Visible = False
        End If
        If panO23.Visible Then
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = factory.o23NotepadBL.GetList(mqO23).Where(Function(p) p.o24IsBillingMemo = True)
            If lisO23.Count > 0 Then
                panO23.Visible = True
                notepad1.RefreshData(lisO23, intRecordPID)
                Me.lblO23.Text += " (" & notepad1.RowsCount.ToString & ")"
            Else
                panO23.Visible = False
            End If
        End If

    End Sub

    Private Function LoadTheRightBillingMemo(factory As BL.Factory, strPrefix As String, intRecordPID As Integer) As String
        Select Case strPrefix
            Case "p28"
                Dim cRec As BO.p28Contact = factory.p28ContactBL.Load(intRecordPID)
                If cRec.p28BillingMemo <> "" Then Return BO.BAS.CrLfText2Html(cRec.p28BillingMemo)
            Case "p41"
                Dim s As String = ""
                Dim cRec As BO.p41Project = factory.p41ProjectBL.Load(intRecordPID)
                If cRec.p41BillingMemo <> "" Then s = BO.BAS.CrLfText2Html(cRec.p41BillingMemo)
                If cRec.p28ID_Client > 0 Then
                    Dim cClient As BO.p28Contact = factory.p28ContactBL.Load(cRec.p28ID_Client)
                    If cClient.p28BillingMemo <> "" Then s += "<br>" & BO.BAS.CrLfText2Html(cClient.p28BillingMemo)
                End If
                Return s
        End Select
        Return ""
    End Function

End Class