Public Class entity_worksheet_summary
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub DisableApprovingButton()
        cmdApproving.NavigateUrl = ""
        cmdApproving.Enabled = False
    End Sub
    Public Sub RefreshData(cWorksheetSum As BO.p31WorksheetSum, strPrefix As String, intRecordPID As Integer, Optional dblLimitHours_Notification As Double = 0)
        With cWorksheetSum
            Me.p31Hours_Orig.Text = BO.BAS.FN(.p31Hours_Orig)
            Dim b As Boolean = False
            If .WaitingOnApproval_Hours_Count > 0 Then
                Me.WaitingOnApproval_Hours_Sum.Text = BO.BAS.FN(.WaitingOnApproval_Hours_Sum) & " <span class='badgebox1red'>" & .WaitingOnApproval_Hours_Count.ToString & "x</span>"

                If Me.Factory.SysUser.IsApprovingPerson Then
                    'cmdApproving.NavigateUrl = "entity_framework_detail_approving.aspx?prefix=" & strPrefix & "&pid=" & intRecordPID.ToString
                    cmdApproving.NavigateUrl = "javascript:approve()"
                End If

                b = True
                If dblLimitHours_Notification > 0 And dblLimitHours_Notification < .WaitingOnApproval_Hours_Sum Then
                    'zvýraznit překročení limitu
                    Me.WaitingOnApproval_Hours_Sum.Text += "<img src='Images/warning.png' title='Překročen limit " & dblLimitHours_Notification.ToString & "hodin!'/>"
                End If

            End If

            If .WaitingOnApproval_Other_Count > 0 Then
                Me.WaitingOnApproval_Other_Sum.Text = BO.BAS.FN(.WaitingOnApproval_Other_Sum) & " (" & .WaitingOnApproval_Other_Count.ToString & ")"
                b = True
            End If
            Me.trWait4Approval.Visible = b
            b = False
            If .WaitingOnInvoice_Hours_Count > 0 Then
                Me.WaitingOnInvoice_Hours_Sum.Text = BO.BAS.FN(.WaitingOnInvoice_Hours_Sum) & " (" & .WaitingOnInvoice_Hours_Count.ToString & ")"
                b = True
            End If
            If .WaitingOnInvoice_Other_Count > 0 Then
                Me.WaitingOnInvoice_Other_Sum.Text = BO.BAS.FN(.WaitingOnInvoice_Other_Sum) & " (" & .WaitingOnInvoice_Other_Count.ToString & ")"
                b = True
            End If
            Me.trWait4Invoice.Visible = b
        End With
    End Sub
End Class