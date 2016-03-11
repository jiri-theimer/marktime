Public Class p51_fpr
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub p51_fpr_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .HeaderText = "Nastavit výpočet efektivních sazeb"
                .AddToolbarButton("Uložit změny", "save", , "Images/save.png")

                period1.SetupData(.Factory, .Factory.j03UserBL.GetUserParam("periodcombo-custom_query"))

            End With
            Dim mq As New BO.myQuery
            mq.Closed = BO.BooleanQueryMode.FalseQuery
            Me.p51ID.DataSource = Master.Factory.p51PriceListBL.GetList(mq).Where(Function(p) p.p51IsMasterPriceList = False)
            Me.p51ID.DataBind()

            
            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.x35GlobalParam = Master.Factory.x35GlobalParam.Load("p51ID_FPR")
        If crec Is Nothing Then
            Return
        End If

        Me.p51ID.SelectedValue = cRec.x35Value

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim cRec As BO.x35GlobalParam = Master.Factory.x35GlobalParam.Load("p51ID_FPR")
            If cRec Is Nothing Then
                cRec = New BO.x35GlobalParam
                cRec.x35Key = "p51ID_FPR"
            End If
            cRec.x35Value = Me.p51ID.SelectedValue

            With Master.Factory.x35GlobalParam
                If .Save(cRec) Then
                    Master.CloseAndRefreshParent()
                Else
                    Master.Notify(.ErrorMessage, 2)
                End If
            End With
            
        End If
    End Sub

    Private Sub p51_fpr_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Me.p51ID.SelectedValue <> "" Then
            Me.clue1.Attributes("rel") = "clue_p51_record.aspx?pid=" & Me.p51ID.SelectedValue
            Me.clue1.Visible = True
        Else
            Me.clue1.Visible = False
        End If
    End Sub

    Private Sub p51ID_NeedMissingItem(strFoundedMissingItemValue As String, ByRef strAddMissingItemText As String) Handles p51ID.NeedMissingItem
        Dim cRec As BO.p51PriceList = Master.Factory.p51PriceListBL.Load(CInt(strFoundedMissingItemValue))
        If Not cRec Is Nothing Then strAddMissingItemText = cRec.NameWithCurr
    End Sub

    Private Sub cmdRecalcAll_Click(sender As Object, e As EventArgs) Handles cmdRecalcAll.Click
        Dim intP51ID As Integer = BO.BAS.IsNullInt(Me.p51ID.SelectedValue)
        If intP51ID = 0 Then
            Master.Notify("Musíte specifikovat vzorový ceník.", NotifyLevel.WarningMessage)
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
End Class