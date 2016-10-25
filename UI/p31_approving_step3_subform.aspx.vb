Public Class p31_approving_step3_subform
    Inherits System.Web.UI.Page
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        approve1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            Master.DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
            If Master.DataPID = 0 Then Master.StopPage("pid is missing")
            ViewState("guid") = Request.Item("guid")
            If ViewState("guid") = "" Then Master.StopPage("guid is missing")

            With Master.Factory.j03UserBL
                .InhaleUserParams("p31_approving-use_internal_approving")
                approve1.AllowInternalApproving = BO.BAS.BG(.GetUserParam("p31_approving-use_internal_approving", "0"))
            End With


            approve1.GUID_TempData = ViewState("guid")
            RefreshRecord()

            comments1.RefreshData(Master.Factory, BO.x29IdEnum.p31Worksheet, Master.DataPID)
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p31Worksheet = Master.Factory.p31WorksheetBL.LoadTempRecord(Master.DataPID, ViewState("guid"))
        approve1.InhaleRecord(cRec, False)

        With cRec
            If .p33ID = BO.p33IdENUM.Cas Then
                cmdSplitRecord.Visible = True
            Else
                cmdSplitRecord.Visible = False
            End If
            Me.Person.Text = .Person
            Me.p34name.Text = .p34Name
            Me.p32name.Text = .p32Name
            Me.p31date.Text = BO.BAS.FD(.p31Date)

            Me.billable.Text = IIf(.p32IsBillable, "[fakturovatelné]", "[ne-fakturovatelné]")
            Me.p31date.Text = BO.BAS.FD(.p31Date)

            Me.Client.Text = .ClientName
            Me.p41Name.Text = .p41Name

            Me.p31value_orig.Text = BO.BAS.FN(.p31Value_Orig)
            Select Case .p33ID
                Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                    If .p33ID = BO.p33IdENUM.Cas And .IsRecommendedHHMM() Then
                        Dim cT As New BO.clsTime
                        Me.p31value_orig.Text = cT.ShowAsHHMM(.p31Value_Orig.ToString)
                    End If
                    Me.p31Rate_Billing_Orig.Text = BO.BAS.FN(.p31Rate_Billing_Orig)
                    Me.rate_j27ident.Text = .j27Code_Billing_Orig
                Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                    Me.lblBillingRate_Orig.Visible = False
                    Me.p31Rate_Billing_Orig.Visible = False
                    Me.j27ident_orig.Text = .j27Code_Billing_Orig

            End Select




            If .p56ID <> 0 Then
                Me.Task.Text = .p56Name
            Else
                lblP56.Visible = False
            End If
        End With
    End Sub

    Private Sub approve1_AfterSave(ByRef strErr As String) Handles approve1.AfterSave
        If strErr <> "" Then
            Master.Notify(strErr, NotifyLevel.WarningMessage)
            Return
        End If
        Me.hidRefreshParent.Value = "1"
    End Sub
End Class