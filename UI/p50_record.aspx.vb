Public Class p50_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p50_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .HeaderText = "Nákladové ceníky"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                period1.SetupData(.Factory, .Factory.j03UserBL.GetUserParam("periodcombo-custom_query"))

            End With
            Dim mq As New BO.myQuery
            mq.Closed = BO.BooleanQueryMode.FalseQuery
            Me.p51ID.DataSource = Master.Factory.p51PriceListBL.GetList(mq).Where(Function(p) p.p51IsMasterPriceList = False And p.p51IsInternalPriceList = True)
            Me.p51ID.DataBind()


            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            Return
        End If
        Dim cRec As BO.p50OfficePriceList = Master.Factory.p50OfficePriceListBL.Load(Master.DataPID)
        With cRec
            Me.p51ID.SelectedValue = .p51ID.ToString
            Me.p50RatesFlag.SelectedValue = CInt(.p50RatesFlag).ToString
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
            Master.Timestamp = .Timestamp
        End With

       
    End Sub

    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p50OfficePriceListBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p50-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p50OfficePriceListBL
            Dim cRec As BO.p50OfficePriceList = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p50OfficePriceList)
            With cRec
                .p51ID = BO.BAS.IsNullInt(Me.p51ID.SelectedValue)
                .p50RatesFlag = BO.BAS.IsNullInt(Me.p50RatesFlag.SelectedValue)

                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
            End With
            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("p50-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub p50_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.p51ID.SelectedValue <> "" Then
            Me.clue1.Attributes("rel") = "clue_p51_record.aspx?pid=" & Me.p51ID.SelectedValue
            Me.clue1.Visible = True
        Else
            Me.clue1.Visible = False
        End If
        Me.panRecalcFPR.Visible = False
        Select Case Me.p50RatesFlag.SelectedValue
            Case "1"
            Case "2"
                Me.panRecalcFPR.Visible = True
        End Select
    End Sub

    Private Sub cmdRecalcFPR_Click(sender As Object, e As EventArgs) Handles cmdRecalcFPR.Click
        Dim intP51ID As Integer = BO.BAS.IsNullInt(Me.p51ID.SelectedValue)
        If intP51ID = 0 Then
            Master.Notify("Musíte vybrat ceník.", NotifyLevel.WarningMessage)
            Return
        End If
        With Master.Factory.p91InvoiceBL
            If .RecalcFPR(period1.DateFrom, period1.DateUntil, intP51ID) Then
                Master.Notify("Operace dokončena.")
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub

    Private Sub p51ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p51ID.NeedMissingItem
        Dim cRec As BO.p51PriceList = Master.Factory.p51PriceListBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then strAddMissingItemText = cRec.NameWithCurr
    End Sub
End Class