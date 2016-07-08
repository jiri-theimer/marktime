Imports System.Web
Imports System.Web.Services

Public Class handler_activity
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "text/plain"

        Dim strOperation As String = Trim(context.Request.Item("oper"))
        Dim intP32ID As Integer = BO.BAS.IsNullInt(context.Request.Item("pid"))
        Dim intP41ID As Integer = BO.BAS.IsNullInt(context.Request.Item("p41id"))
        Dim intJ27ID As Integer = BO.BAS.IsNullInt(context.Request.Item("j27id"))
        
        If strOperation = "" Or intP32ID = 0 Then
            context.Response.Write(" ")
            Return
        End If
        Dim factory As BL.Factory = Nothing

        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim strLogin As String = HttpContext.Current.User.Identity.Name
            factory = New BL.Factory(, strLogin)
        End If
        If factory Is Nothing Then
            context.Response.Write(" ")
            Return
        End If
        Dim cRec As BO.p32Activity = factory.p32ActivityBL.Load(intP32ID)
        If cRec Is Nothing Then
            context.Response.Write(" ")
            Return
        End If
        Dim cP34 As BO.p34ActivityGroup = factory.p34ActivityGroupBL.Load(cRec.p34ID)

        Select Case strOperation
            'Case "defaultvatrate"
            '    Dim intJ27ID As Integer = BO.BAS.IsNullInt(context.Request.Item("j27id"))
            '    If intJ27ID = 0 Then
            '        intJ27ID = BO.BAS.IsNullInt(factory.x35GlobalParam.GetValueString("j27ID_Domestic"))
            '    End If
            '    Dim lisP53 As IEnumerable(Of BO.p53VatRate) = factory.p53VatRateBL.GetList(New BO.myQuery).Where(Function(p) p.j27ID = intJ27ID And p.x15ID = cRec.x15ID)
            '    If lisP53.Count > 0 Then
            '        context.Response.Write(lisP53(0).p53Value.ToString)
            '    End If

            '    'Case "defaultall"
            '    '    Dim strP41ID As String = context.Request.Item("p41id")
            '    '    If strP41ID = "" Then strP41ID = "0"
            '    '    Dim strDefVatRate As String = c.GetValueFromSQL("select dbo.p32_getvatrate(" & strP32ID & "," & strP41ID & ")")

            '    '    Dim dbl1 As Double = c.GetDoubleValueFROMSQL("select p32Value_Default FROM p32activity where p32id=" & strP32ID)
            '    '    Dim intBillingLangIndex As Integer = c.GetIntegerValueFROMSQL("select dbo.p41_GetLangIndex(" & strP41ID & ")")
            '    '    Dim strDefText As String = c.GetValueFromSQL("select p32WorksheetText_Lang" & intBillingLangIndex.ToString & " from p32activity where p32id=" & strP32ID)
            '    '    Dim dblP31Margin As Double = c.GetDoubleValueFROMSQL("select dbo.p41_GetActivityMargin(" & strP41ID & "," & strP32ID & ")")

            '    '    context.Response.Write(strDefVatRate & "|" & dbl1.ToString & "|" & strDefText & "|" & IIf(dblP31Margin = -1, "", dblP31Margin.ToString))
            Case "profile"
                Dim cP41 As BO.p41Project = Nothing
                Dim strDefText As String = cRec.p32DefaultWorksheetText
                If intP41ID > 0 Then
                    cP41 = factory.p41ProjectBL.Load(intP41ID)
                    Dim intP87ID As Integer = cP41.p87ID_Client 'fakturační jazyk klienta projektu
                    If cP41.p87ID > 0 Then intP87ID = cP41.p87ID 'fakturační jazyk projektu má přednost
                    Select Case intP87ID
                        Case 1 : strDefText = cRec.p32DefaultWorksheetText_Lang1
                        Case 2 : strDefText = cRec.p32DefaultWorksheetText_Lang2
                        Case 3 : strDefText = cRec.p32DefaultWorksheetText_Lang3
                        Case 4 : strDefText = cRec.p32DefaultWorksheetText_Lang4
                    End Select
                    If strDefText <> "" Then
                        If strDefText.IndexOf("[%") >= 0 Then
                            've výchozím popisu aktivity jsou slučovací pole z projektu
                            Dim matches As MatchCollection = Regex.Matches(strDefText, "\[%.*?\%]")
                            For Each m As Match In matches
                                Dim strField As String = Replace(m.Value, "[%", "").Replace("%]", "")
                                strDefText = Replace(strDefText, m.Value, BO.BAS.GetPropertyValue(cP41, strField))
                            Next
                        End If
                    End If
                End If
                
                Dim strDefaultVatRate As String = ""
                If cP34.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu Then
                    If cRec.x15ID > BO.x15IdEnum.Nic Then
                        If intJ27ID = 0 Then
                            intJ27ID = BO.BAS.IsNullInt(factory.x35GlobalParam.GetValueString("j27ID_Domestic"))
                        End If
                        Dim lisP53 As IEnumerable(Of BO.p53VatRate) = factory.p53VatRateBL.GetList(New BO.myQuery).Where(Function(p) p.j27ID = intJ27ID And p.x15ID = cRec.x15ID)
                        If lisP53.Count > 0 Then
                            strDefaultVatRate = lisP53(0).p53Value.ToString
                        End If
                    End If
                End If

                'Výchozí hodnota|Výchozí popis úkonu|Povinnost zadávat popis|výchozí sazba DPH
                context.Response.Write(cRec.p32Value_Default.ToString & "|" & strDefText & "|" & BO.BAS.GB(cRec.p32IsTextRequired) & "|" & strDefaultVatRate)

                'If cRec.p32Value_Default = 0 And strDefText = "" Then
                '    context.Response.Write(" ")
                'Else
                '    context.Response.Write(cRec.p32Value_Default.ToString & "|" & strDefText)
                'End If

            Case Else
                    context.Response.Write(" ")
        End Select

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class