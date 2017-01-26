Public Class p31_record_AI
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p31_record_AI_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Then
                    .StopPage("PID is missing.")
                End If
                .AddToolbarButton("Vyjmout úkon z faktury", "remove", , "Images/cut.png")
                .AddToolbarButton("Uložit změny do faktury", "save", , "Images/save.png")

                .HeaderText = "Worksheet úkon"

            End With

            RefreshRecord()
            comments1.RefreshData(Master.Factory, BO.x29IdEnum.p31Worksheet, Master.DataPID)

        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cD As BO.p31WorksheetDisposition = Master.Factory.p31WorksheetBL.InhaleRecordDisposition(Master.DataPID)
        If cD.RecordDisposition = BO.p31RecordDisposition._NoAccess Then
            Master.StopPage("Uživatel nedisponuje přístupovým právem k tomuto worksheet záznamu.")
        End If
        Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
        If cRec Is Nothing Then
            Master.StopPage("Záznam nebyl nalezen.", True)
        End If

        With cRec
            Me.Person.Text = .Person
            Me.p34name.Text = .p34Name
            Me.p32name.Text = .p32Name

            Me.billable.Text = IIf(.p32IsBillable, "[fakturovatelné]", "[ne-fakturovatelné]")
            p31date.Text = BO.BAS.FD(.p31Date)

            Me.Client.Text = .ClientName
            p41Name.Text = .p41Name

            If .p33ID = BO.p33IdENUM.Cas Then
                p31value_orig.Text = BO.BAS.FN(.p31Value_Orig) & " (" & .p31HHMM_Orig & ")"
            Else
                p31value_orig.Text = BO.BAS.FN(.p31Value_Orig)
            End If


            If .p56ID <> 0 Then
                Me.Task.Text = .p56Name
            Else
                lblP56.Visible = False
            End If


            If cRec.p33ID = BO.p33IdENUM.Cas Or cRec.p33ID = BO.p33IdENUM.Kusovnik Then
                j27ident_orig.Text = "h."
                p31Rate_Billing_Orig.Text = BO.BAS.FN(cRec.p31Rate_Billing_Orig)
                rate_j27ident.Text = .j27Code_Billing_Orig

            Else
                j27ident_orig.Text = .j27Code_Billing_Orig
                lblBillingRate_Orig.Visible = False
            End If

            If cRec.p71ID > BO.p71IdENUM.Nic Then
                panApproved.Visible = True

                Me.p71name.Text = .p71Name
                Me.p72name.Text = .approve_p72Name

                Dim status As New BO.p72PreBillingStatus()
                status.SetStatus(.p72ID_AfterApprove)
                Me.p72img.ImageUrl = status.ImageUrl
                If .p72ID_AfterApprove = BO.p72IdENUM._NotSpecified Then Me.p72img.Visible = False

                Me.value_approved_billing.Text = BO.BAS.FN(.p31Value_Approved_Billing)
                If .p33ID = BO.p33IdENUM.Cas Then
                    Me.value_approved_billing.Text += " (" & .p31HHMM_Approved_Billing & ")"
                End If
                If .p31Value_Approved_Billing <> .p31Value_Orig Then
                    lblKorekceCaption.Visible = True
                    imgKorekce.Visible = True
                    value_korekce.Text = BO.BAS.FN(.p31Value_Approved_Billing - .p31Value_Orig)
                    If .p33ID = BO.p33IdENUM.Cas Then
                        Dim cT As New BO.clsTime
                        value_korekce.Text += " (" & cT.ShowAsHHMM(CDbl(.p31Value_Approved_Billing - .p31Value_Orig).ToString) & ")"
                    End If
                    If cRec.p31Value_Orig > .p31Value_Approved_Billing Then
                        imgKorekce.ImageUrl = "Images/correction_down.gif"
                    Else
                        imgKorekce.ImageUrl = "Images/correction_up.gif"
                    End If
                    If Not .p32IsBillable And .p31Value_Approved_Billing = 0 Then
                        imgKorekce.Visible = False : lblKorekceCaption.Visible = False : value_korekce.Visible = False
                    End If
                End If
                If .p33ID = BO.p33IdENUM.Cas Or .p33ID = BO.p33IdENUM.Kusovnik Then
                    rate_approved.Text = BO.BAS.FN(.p31Rate_Billing_Approved)
                    j07Code_Approved.Text = .j27Code_Billing_Orig
                Else
                    lblFakturacniSazba_Approved.Visible = False
                End If
                value_approved_internal.Text = BO.BAS.FN(.p31Value_Approved_Internal)
                If .p31Value_Approved_Internal <> .p31Value_Orig Then
                    imgKorekceInternal.Visible = True
                    If .p31Value_Orig > .p31Value_Approved_Internal Then
                        imgKorekceInternal.ImageUrl = "Images/correction_down.gif"
                    Else
                        imgKorekceInternal.ImageUrl = "Images/correction_up.gif"
                    End If
                Else
                    imgKorekceInternal.Visible = False
                End If
                If .j02ID_ApprovedBy > 0 Then

                End If

                lblTimestamp_Approve.Text = "<img src='Images/approve.png'> " & "Schválil" & ": " & Master.Factory.j02PersonBL.Load(.j02ID_ApprovedBy).FullNameDesc & "/" & BO.BAS.FD(.p31Approved_When, True, True)


            Else
                panApproved.Visible = False
            End If
            If cRec.p70ID > BO.p70IdENUM.Nic Then

                panInvoiced.Visible = True
                p70name.Text = .p70Name
                Dim status As New BO.p70BillingStatus
                status.SetStatus(.p70ID)
                Me.p70name.Style.Item("background-color") = status.Color

                p91ident.Text = .p91Code
                p31amount_withoutvat_invoiced.Text = BO.BAS.FN(.p31Amount_WithoutVat_Invoiced)
                Dim cJ27I As BO.j27Currency = Master.Factory.ftBL.LoadJ27(.j27ID_Billing_Invoiced)
                j27ident_invoiced.Text = cJ27I.j27Code


            Else
                panInvoiced.Visible = False

            End If
            Dim mqO23 As New BO.myQueryO23
            mqO23.p31ID = Master.DataPID
            Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
            If lisO23.Count > 0 Then
                notepad1.RefreshData(lisO23, Master.DataPID)
            Else
                Me.boxO23.Visible = False
            End If

            If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
                'uživatel nemá nárok vidět věci po schvalování a fakturaci
                panInvoiced.Visible = False
                panApproved.Visible = False
                lblBillingRate_Orig.Visible = False
                p31Rate_Billing_Orig.Visible = False
                rate_j27ident.Visible = False
            End If

            Me.Timestamp.Text = .Timestamp & " | Vlastník záznamu: <span class='val'>" & .Owner & "</span>"
            Master.HeaderText = .p34Name & " | " & BO.BAS.FD(.p31Date) & " | " & .Person & " | " & .p41Name

        End With

        RefreshRecord_4Edit(cRec)
    End Sub

    Private Sub RefreshRecord_4Edit(cRec As BO.p31Worksheet)
        Dim cP91 As BO.p91Invoice = Master.Factory.p91InvoiceBL.Load(cRec.p91ID)
        basUI.SelectRadiolistValue(Me.Edit_p70ID, CInt(cRec.p70ID).ToString)
        

        Select Case cRec.p33ID
            Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                If cRec.p33ID = BO.p33IdENUM.Cas Then
                    Me.lblEdit_Value.Text = "Fakturované hodiny:"
                    lblEdit_Rate.Text = "Fakturovaná hodinová sazba:"
                Else
                    lblEdit_Value.Text = "Fakturovat počet:"
                    lblEdit_Rate.Text = "Fakturovaná sazba:"
                End If
                Edit_p31Rate_Billing_Invoiced.Value = cRec.p31Rate_Billing_Invoiced
                Me.rate_j27code.Text = cP91.j27Code
                ''Me.lblEdit_VatRate.Visible = False : Me.Edit_p31VatRate_Invoiced.Visible = False

            Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                ''Me.lblEdit_VatRate.Visible = True : Me.Edit_p31VatRate_Invoiced.Visible = True
                lblEdit_Value.Text = "Fakturovat cenu bez DPH:"
                lblEdit_Rate.Visible = False
                Edit_p31Rate_Billing_Invoiced.Visible = False
                value_j27code.Text = cP91.j27Code
        End Select
        ''If cP91.x15ID > 0 Then
        ''    Me.Edit_p31VatRate_Invoiced.Value = cP91.p91FixedVatRate
        ''    Me.Edit_p31VatRate_Invoiced.Enabled = False
        ''Else
        ''    Me.Edit_p31VatRate_Invoiced.Value = cRec.p31VatRate_Invoiced
        ''End If

        Me.Edit_p31VatRate_Invoiced.Value = cRec.p31VatRate_Invoiced
        Edit_p31Value_Invoiced.Text = cRec.p31Value_Invoiced.ToString
        If cRec.p33ID = BO.p33IdENUM.Cas And cRec.IsRecommendedHHMM_Invoiced() Then
            Dim cT As New BO.clsTime
            Me.Edit_p31Value_Invoiced.Text = cT.ShowAsHHMM(cRec.p31Value_Invoiced.ToString)
        End If
        Me.Edit_p31Text.Text = cRec.p31Text


    End Sub

    Private Sub p31_record_AI_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

        Select Case Me.Edit_p70ID.SelectedValue
            Case "4"
                panEdit.Visible = True
            Case Else
                panEdit.Visible = False
        End Select

    End Sub
   
    
    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "remove" Then
            Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
            Dim pids As New List(Of Integer)
            pids.Add(Master.DataPID)
            With Master.Factory.p31WorksheetBL
                If .RemoveFromInvoice(cRec.p91ID, pids) Then
                    Master.CloseAndRefreshParent("p31-remove")
                Else
                    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                End If
            End With

        End If
        If strButtonValue = "save" Then
            Dim c As New BO.p31WorksheetInvoiceChange, cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.Load(Master.DataPID)
            With c
                .p31ID = cRec.PID
                .p33ID = cRec.p33ID
                .p70ID = CType(CInt(Me.Edit_p70ID.SelectedValue), BO.p70IdENUM)
                .TextUpdate = Trim(Me.Edit_p31Text.Text)
                Select Case cRec.p33ID
                    Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                        .InvoiceRate = BO.BAS.IsNullNum(Me.Edit_p31Rate_Billing_Invoiced.Value)

                    Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu


                End Select
                .InvoiceVatRate = BO.BAS.IsNullNum(Me.Edit_p31VatRate_Invoiced.Value)
                If cRec.p33ID = BO.p33IdENUM.Cas Then
                    Dim cT As New BO.clsTime
                    .InvoiceValue = cT.ShowAsDec(Me.Edit_p31Value_Invoiced.Text)

                Else
                    .InvoiceValue = BO.BAS.IsNullNum(Me.Edit_p31Value_Invoiced.Text)
                End If


            End With
            Dim lis As New List(Of BO.p31WorksheetInvoiceChange)
            lis.Add(c)
            With Master.Factory.p31WorksheetBL
                If .UpdateInvoice(cRec.p91ID, lis) Then
                    Master.CloseAndRefreshParent("p31-save")
                Else
                    Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                End If
            End With
        End If
    End Sub
End Class