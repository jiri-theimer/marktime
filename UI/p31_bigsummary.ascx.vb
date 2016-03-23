Public Class p31_bigsummary
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory
    Private _IsHiddenRates As Boolean = False


    Public Property MasterDataPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterDataPID.Value)
        End Get
        Set(value As Integer)
            Me.hidMasterDataPID.Value = value.ToString
        End Set
    End Property
    Public Property MasterDataPrefix As String
        Get
            Return BO.BAS.IsNull(Me.hidMasterDataPrefix.Value)
        End Get
        Set(value As String)
            Me.hidMasterDataPrefix.Value = value
        End Set
    End Property
    Public Property IsApprovingPerson As Boolean
        Get
            Return BO.BAS.BG(Me.hidIsApprovingPerson.Value)
        End Get
        Set(value As Boolean)
            Me.hidIsApprovingPerson.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public Property IsInvoicingPerson As Boolean
        Get
            Return BO.BAS.BG(Me.hidIsInvoicingPerson.Value)
        End Get
        Set(value As Boolean)
            Me.hidIsInvoicingPerson.Value = BO.BAS.GB(value)
        End Set
    End Property

    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Handle_SetupPeriodCombo()
        If period1.SelectedValue <> "" Then
            period1.BackColor = Drawing.Color.Red
        Else
            period1.BackColor = Nothing
        End If

    End Sub

    Private Sub Handle_SetupPeriodCombo()
        If period1.RowsCount = 0 Then
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("periodcombo-custom_query")
                .Add("p31_grid-period")
            End With

            With Factory.j03UserBL
                .InhaleUserParams(lisPars)
                period1.SetupData(Me.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("p31_grid-period")
            End With
        End If
    End Sub

    Public Sub RefreshData()
        Handle_SetupPeriodCombo()
        _IsHiddenRates = Not Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates)

        Dim mq As New BO.myQueryP31
        Select Case Me.MasterDataPrefix
            Case "p41"
                mq.p41ID = Me.MasterDataPID
            Case "p28"
                mq.p28ID_Client = Me.MasterDataPID
            Case "j02"
                mq.j02ID = Me.MasterDataPID
        End Select

        mq.DateFrom = period1.DateFrom
        mq.DateUntil = period1.DateUntil
        mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead

        Dim lis As IEnumerable(Of BO.p31WorksheetBigSummary) = Me.Factory.p31WorksheetBL.GetList_BigSummary(mq)
        rpWorksheet.DataSource = lis
        rpWorksheet.DataBind()

    End Sub



    Private Sub rpWorksheet_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpWorksheet.ItemDataBound
        Dim cRec As BO.p31WorksheetBigSummary = CType(e.Item.DataItem, BO.p31WorksheetBigSummary)
        CType(e.Item.FindControl("rozpracovano_j27Code"), Label).Text = cRec.j27Code
        CType(e.Item.FindControl("schvaleno_j27Code"), Label).Text = cRec.j27Code
        If cRec.rozpracovano_hodiny <> 0 Then CType(e.Item.FindControl("rozpracovano_hodiny"), Label).Text = BO.BAS.FN(cRec.rozpracovano_hodiny)
        If cRec.rozpracovano_odmeny <> 0 Then CType(e.Item.FindControl("rozpracovano_odmeny"), Label).Text = BO.BAS.FN(cRec.rozpracovano_odmeny)
        If cRec.rozpracovano_vydaje <> 0 Then CType(e.Item.FindControl("rozpracovano_vydaje"), Label).Text = BO.BAS.FN(cRec.rozpracovano_vydaje)
        With CType(e.Item.FindControl("rozpracovano_celkem"), Label)
            If _IsHiddenRates Then
                .Text = "#####"
            Else
                .Text = BO.BAS.FN(cRec.rozpracovano_celkem)
            End If
        End With



        If Not (BO.BAS.IsNullDBDate(cRec.rozpracovano_prvni) Is Nothing Or BO.BAS.IsNullDBDate(cRec.rozpracovano_posledni) Is Nothing) Then
            CType(e.Item.FindControl("rozpracovano_obdobi"), Label).Text = BO.BAS.FD(cRec.rozpracovano_prvni) & " - " & BO.BAS.FD(cRec.rozpracovano_posledni)
        End If
        If cRec.schvaleno_hodiny <> 0 Then CType(e.Item.FindControl("schvaleno_hodiny"), Label).Text = BO.BAS.FN(cRec.schvaleno_hodiny)
        If cRec.schvaleno_odmeny <> 0 Then CType(e.Item.FindControl("schvaleno_odmeny"), Label).Text = BO.BAS.FN(cRec.schvaleno_odmeny)
        If cRec.schvaleno_vydaje <> 0 Then CType(e.Item.FindControl("schvaleno_vydaje"), Label).Text = BO.BAS.FN(cRec.schvaleno_vydaje)
        With CType(e.Item.FindControl("schvaleno_celkem"), Label)
            If _IsHiddenRates Then
                .Text = "#####"
            Else
                .Text = BO.BAS.FN(cRec.schvaleno_celkem)
            End If
        End With


        If Not (BO.BAS.IsNullDBDate(cRec.schvaleno_prvni) Is Nothing Or BO.BAS.IsNullDBDate(cRec.schvaleno_posledni) Is Nothing) Then
            CType(e.Item.FindControl("schvaleno_obdobi"), Label).Text = BO.BAS.FD(cRec.schvaleno_prvni) & " - " & BO.BAS.FD(cRec.schvaleno_posledni)
        End If
        CType(e.Item.FindControl("cmdApprove"), HyperLink).Visible = Me.IsApprovingPerson
        CType(e.Item.FindControl("cmdReApprove"), HyperLink).Visible = Me.IsApprovingPerson
        CType(e.Item.FindControl("cmdClearApprove"), HyperLink).Visible = Me.IsApprovingPerson
        CType(e.Item.FindControl("cmdInvoice"), HyperLink).Visible = Me.IsInvoicingPerson

        If cRec.rozpracovano_pocet > 0 Then
            CType(e.Item.FindControl("rozpracovano_pocet"), Label).Text = BO.BAS.FNI(cRec.rozpracovano_pocet) & "x"
        Else
            e.Item.FindControl("rozpracovano_pocet").Visible = False
            CType(e.Item.FindControl("cmdApprove"), HyperLink).Visible = False
        End If
        If cRec.schvaleno_pocet > 0 Then
            CType(e.Item.FindControl("schvaleno_pocet"), Label).Text = BO.BAS.FNI(cRec.schvaleno_pocet) & "x"
        Else
            CType(e.Item.FindControl("cmdReApprove"), HyperLink).Visible = False
            CType(e.Item.FindControl("cmdClearApprove"), HyperLink).Visible = False
            CType(e.Item.FindControl("cmdInvoice"), HyperLink).Visible = False
        End If

    End Sub

    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Me.Factory.j03UserBL.SetUserParam("p31_grid-period", period1.SelectedValue)
        RefreshData()
    End Sub
End Class