Imports System.Web
Imports System.Web.Services

Public Class handler_approve
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "text/plain"

        Dim factory As BL.Factory = Nothing

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            context.Response.Write(" ")
            Return
        End If

        Dim intPID As Integer = BO.BAS.IsNullInt(context.Request.Item("pid"))
        Dim p72ID As BO.p72IdENUM = CType(BO.BAS.IsNullInt(context.Request.Item("p72id")), BO.p72IdENUM)
        Dim cRec As BO.p31Worksheet = factory.p31WorksheetBL.Load(intPID)

        Dim cDisp As BO.p31WorksheetDisposition = factory.p31WorksheetBL.InhaleRecordDisposition(intPID)
        If cDisp.RecordDisposition = BO.p31RecordDisposition._NoAccess Then
            context.Response.Write("K úkonu nemáte přístup.") : Return
        End If
        If cRec.p91ID <> 0 Then
            context.Response.Write("Úkon již byl vyfakturován.") : Return
        End If
        If Not (cDisp.RecordDisposition = BO.p31RecordDisposition.CanApprove Or cDisp.RecordDisposition = BO.p31RecordDisposition.CanApproveAndEdit) Then
            context.Response.Write("Nemáte oprávnění schvalovat tento úkon.") : Return
        End If

        Dim cA As New BO.p31WorksheetApproveInput(intPID, cRec.p33ID)
        With cA

            .p31Date = cRec.p31Date
            .p71id = BO.p71IdENUM.Schvaleno
            .p72id = p72ID
            .p31ApprovingSet = cRec.p31ApprovingSet
            If p72ID = BO.p72IdENUM.Fakturovat Or p72ID = BO.p72IdENUM.FakturovatPozdeji Then
                Select Case cRec.p33ID
                    Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                        .Rate_Billing_Approved = cRec.p31Rate_Billing_Orig

                End Select
                .Value_Approved_Billing = cRec.p31Value_Orig

            End If
            ''If Me.chkUseInternalApproving.Checked And (cRec.p33ID = BO.p33IdENUM.Cas Or cRec.p33ID = BO.p33IdENUM.Kusovnik) Then
            ''    'interní schvalování
            ''    If .Value_Approved_Internal = 0 And .Rate_Internal_Approved = 0 Then

            ''        .Value_Approved_Internal = cRec.p31Value_Orig
            ''        .Rate_Internal_Approved = cRec.p31Rate_Internal_Orig
            ''    End If
            ''End If
        End With
        If Not factory.p31WorksheetBL.Save_Approving(cA, False) Then
            context.Response.Write(factory.p31WorksheetBL.ErrorMessage) 'chyba
        Else
            context.Response.Write("1") 'schváleno
        End If



    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class