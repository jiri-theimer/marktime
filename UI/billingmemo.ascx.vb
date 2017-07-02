﻿Public Class billingmemo
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

   
    
    Public ReadOnly Property IsEmpty As Boolean
        Get
            If Me.BillingMemo.Text = "" Then Return True Else Return False
        End Get
    End Property
    Public Sub RefreshData(factory As BL.Factory, strPrefix As String, intRecordPID As Integer)
        If intRecordPID = 0 Or strPrefix = "" Then Return

        Select strPrefix
            Case "p28"
                Me.BillingMemo.Text = LoadTheRightBillingMemo(factory, strPrefix, intRecordPID)
            Case "p41"
                Me.BillingMemo.Text = LoadTheRightBillingMemo(factory, strPrefix, intRecordPID)
            Case "j02"
        End Select

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