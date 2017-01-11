Public Class p90_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p90_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ff1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            With Master

                .HeaderIcon = "Images/proforma_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

                Me.j27ID.DataSource = .Factory.ftBL.GetList_J27()
                Me.j27ID.DataBind()
                Me.p89ID.DataSource = .Factory.p89ProformaTypeBL.GetList(New BO.myQuery)
                Me.p89ID.DataBind()

            End With

          
            RefreshRecord()

            If Master.IsRecordClone Then
                Master.DataPID = 0
                Me.p90Amount_Billed.Value = 0
                Me.p90DateBilled.SelectedDate = Nothing
                Me.p82Code.Visible = False
                link_x31_dpp.Visible = False
                Me.p90Code.Visible = False : Me.link_x31id.Visible = False
                Me.p90TextDPP.Text = ""
            End If

        End If
    End Sub

    Private Sub RefreshRecord()
        Handle_FF()
        If Master.DataPID = 0 Then
            Dim cRecLast As BO.p90Proforma = Master.Factory.p90ProformaBL.LoadMyLastCreated()
            If Not cRecLast Is Nothing Then
                Me.j27ID.SelectedValue = cRecLast.j27ID.ToString
                Me.p89ID.SelectedValue = cRecLast.p89ID.ToString
                Me.p90VatRate.Value = cRecLast.p90VatRate
            End If
            Me.p90Date.SelectedDate = Today
            Me.p90DateMaturity.SelectedDate = Today.AddDays(10)
            Me.j02ID_Owner.Value = Master.Factory.SysUser.j02ID.ToString
            Me.j02ID_Owner.Text = Master.Factory.SysUser.PersonDesc
            If Request.Item("p28id") <> "" Then
                Me.p28ID.Value = Request.Item("p28id")
                Me.p28ID.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, CInt(Request.Item("p28id")), True)
            End If
            Return
        End If

        Dim cRec As BO.p90Proforma = Master.Factory.p90ProformaBL.Load(Master.DataPID)
        Dim cP89 As BO.p89ProformaType = Master.Factory.p89ProformaTypeBL.Load(cRec.p89ID)
        With cRec
            Master.HeaderText = String.Format("Záznam zálohové faktury [{0}]", .p90Code)
            Me.p90Code.Text = .p90Code
            Me.p90Code.NavigateUrl = "javascript:recordcode()"
            Me.j27ID.SelectedValue = .j27ID.ToString
            Me.p89ID.SelectedValue = .p89ID.ToString
            Me.p28ID.Value = .p28ID.ToString
            Me.p28ID.Text = .p28Name
            Me.j02ID_Owner.Value = .j02ID_Owner.ToString
            Me.j02ID_Owner.Text = .Owner
            Me.p90text1.Text = .p90Text1
            Me.p90text2.Text = .p90Text2
            Me.p90TextDPP.Text = .p90TextDPP
            Me.p90Date.SelectedDate = .p90Date
            If Not BO.BAS.IsNullDBDate(.p90DateMaturity) Is Nothing Then
                Me.p90DateMaturity.SelectedDate = .p90DateMaturity
            End If
            If Not BO.BAS.IsNullDBDate(.p90DateBilled) Is Nothing Then
                Me.p90DateBilled.SelectedDate = .p90DateBilled
            End If

            Me.p90Amount.Value = BO.BAS.IsNullNum(.p90Amount)
            Me.p90Amount_Billed.Value = BO.BAS.IsNullNum(.p90Amount_Billed)
            Me.p90Amount_Vat.Value = BO.BAS.IsNullNum(.p90Amount_Vat)
            Me.p90Amount_WithoutVat.Value = BO.BAS.IsNullNum(.p90Amount_WithoutVat)
            Me.p90VatRate.Value = BO.BAS.IsNullNum(.p90VatRate)

            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
            Master.Timestamp = .Timestamp

            link_x31id.NavigateUrl = "javascript:report(" & cP89.x31ID.ToString & ")"
            If .p82Code <> "" Then
                p82Code.Visible = True : p82Code.Text = .p82Code : Me.p82Code.NavigateUrl = "javascript:dppcode(" & .p82ID.ToString & ")"
                link_x31_dpp.Visible = True : link_x31_dpp.NavigateUrl = "javascript:report(" & cP89.x31ID_Payment.ToString & ")"
            Else
                Me.p82Code.Visible = False
                link_x31_dpp.Visible = False
            End If
        End With

    End Sub
    Private Sub Handle_FF()
        With RadTabStrip1.FindTabByValue("ff")
            If .Visible Then
                Dim fields As List(Of BO.FreeField) = Master.Factory.x28EntityFieldBL.GetListWithValues(BO.x29IdEnum.p90Proforma, Master.DataPID, 0)
                ff1.FillData(fields)
                .Text = BO.BAS.OM2(.Text, ff1.FieldsCount.ToString)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p90ProformaBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p90-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p90ProformaBL
            Dim cRec As BO.p90Proforma = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p90Proforma)
            With cRec
                .p89ID = BO.BAS.IsNullInt(Me.p89ID.SelectedValue)
                .j27ID = BO.BAS.IsNullInt(Me.j27ID.SelectedValue)
                .p28ID = BO.BAS.IsNullInt(Me.p28ID.Value)
                .j02ID_Owner = BO.BAS.IsNullInt(Me.j02ID_Owner.Value)
               
                .p90Text1 = Me.p90text1.Text
                .p90Text2 = Me.p90text2.Text
                .p90TextDPP = Me.p90TextDPP.Text
                .p90Date = Me.p90Date.SelectedDate
                .p90DateMaturity = BO.BAS.IsNullDBDate(Me.p90DateMaturity.SelectedDate)

                .p90DateBilled = BO.BAS.IsNullDBDate(Me.p90DateBilled.SelectedDate)

                .p90Amount = BO.BAS.IsNullNum(Me.p90Amount.Value)
                .p90Amount_Billed = BO.BAS.IsNullNum(Me.p90Amount_Billed.Value)
                .p90Amount_Vat = BO.BAS.IsNullNum(Me.p90Amount_Vat.Value)
                .p90Amount_WithoutVat = BO.BAS.IsNullNum(Me.p90Amount_WithoutVat.Value)
                .p90VatRate = BO.BAS.IsNullNum(Me.p90VatRate.Value)

                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
            End With

            Dim lisFF As List(Of BO.FreeField) = Me.ff1.GetValues()

            If .Save(cRec, lisFF) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("p90-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub cmdCalc1_Click(sender As Object, e As EventArgs) Handles cmdCalc1.Click
        Dim dbl1 As Double = BO.BAS.IsNullNum(Me.p90Amount_WithoutVat.Value)
        Dim rate As Double = BO.BAS.IsNullNum(Me.p90VatRate.Value)
        Me.p90Amount_Vat.Value = dbl1 * rate / 100
        Me.p90Amount.Value = Me.p90Amount_Vat.Value + dbl1
    End Sub

    Private Sub cmdCalc2_Click(sender As Object, e As EventArgs) Handles cmdCalc2.Click
        Dim dbl1 As Double = BO.BAS.IsNullNum(Me.p90Amount.Value)
        Dim rate As Double = BO.BAS.IsNullNum(Me.p90VatRate.Value)
        Me.p90Amount_WithoutVat.Value = dbl1 / (1 + rate / 100)
        Me.p90Amount_Vat.Value = dbl1 - Me.p90Amount_WithoutVat.Value
    End Sub
End Class