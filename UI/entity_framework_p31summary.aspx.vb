Public Class entity_framework_p31summary
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Private _IsHiddenRates As Boolean = False
    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            hidMasterPID.Value = value.ToString
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

    Private Sub entity_framework_p31summary_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            If Me.CurrentMasterPID = 0 Or Me.CurrentMasterPrefix = "" Then Master.StopPage("masterpid or masterprefix missing.")
            Dim lisPars As New List(Of String)
            With lisPars
                .Add("periodcombo-custom_query")
                .Add("p31_grid-period")
            End With
            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                period1.SelectedValue = .GetUserParam("p31_grid-period")
            End With
        End If
        If Request.Item("IsApprovingPerson") = "" Then
            Me.IsApprovingPerson = Master.Factory.SysUser.IsApprovingPerson
        Else
            Me.IsApprovingPerson = BO.BAS.BG(Request.Item("IsApprovingPerson"))
        End If

        If Not Page.IsPostBack Then
            RefreshData()
        End If
    End Sub

    Private Sub entity_framework_p31summary_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If period1.SelectedValue <> "" Then
            period1.BackColor = Drawing.Color.Red
        Else
            period1.BackColor = Nothing
        End If
    End Sub

    Public Sub RefreshData()
        _IsHiddenRates = Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates)

        Dim mq As New BO.myQueryP31
        Select Case Me.CurrentMasterPrefix
            Case "p41"
                mq.p41ID = Me.CurrentMasterPID
            Case "p28"
                mq.p28ID_Client = Me.CurrentMasterPID
            Case "j02"
                mq.j02ID = Me.CurrentMasterPID
        End Select

        mq.DateFrom = period1.DateFrom
        mq.DateUntil = period1.DateUntil
        mq.SpecificQuery = BO.myQueryP31_SpecificQuery.AllowedForRead

        Dim lis As IEnumerable(Of BO.p31WorksheetBigSummary) = Master.Factory.p31WorksheetBL.GetList_BigSummary(mq)
        rpWaiting.DataSource = lis.Where(Function(p) p.rozpracovano_pocet > 0)
        rpWaiting.DataBind()

        rpApproved.DataSource = lis.Where(Function(p) p.schvaleno_pocet > 0)
        rpApproved.DataBind()
        If rpApproved.Items.Count = 0 Then panApproved.Visible = False
    End Sub



   
    Private Sub period1_OnChanged(DateFrom As Date, DateUntil As Date) Handles period1.OnChanged
        Master.Factory.j03UserBL.SetUserParam("p31_grid-period", period1.SelectedValue)
        RefreshData()
    End Sub

    Private Sub rpWaiting_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpWaiting.ItemDataBound
        Dim cRec As BO.p31WorksheetBigSummary = CType(e.Item.DataItem, BO.p31WorksheetBigSummary)
        CType(e.Item.FindControl("rozpracovano_j27Code"), Label).Text = cRec.j27Code

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

        If cRec.rozpracovano_pocet > 0 Then
            CType(e.Item.FindControl("rozpracovano_pocet"), Label).Text = BO.BAS.FNI(cRec.rozpracovano_pocet) & "x"
        Else
            e.Item.FindControl("rozpracovano_pocet").Visible = False
            CType(e.Item.FindControl("cmdApprove"), HyperLink).Visible = False
        End If

        CType(e.Item.FindControl("cmdApprove"), HyperLink).Visible = Me.IsApprovingPerson

    End Sub

    Private Sub rpApproved_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpApproved.ItemDataBound
        Dim cRec As BO.p31WorksheetBigSummary = CType(e.Item.DataItem, BO.p31WorksheetBigSummary)
        CType(e.Item.FindControl("schvaleno_j27Code"), Label).Text = cRec.j27Code
       
        
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

        CType(e.Item.FindControl("cmdReApprove"), HyperLink).Visible = Me.IsApprovingPerson
        CType(e.Item.FindControl("cmdClearApprove"), HyperLink).Visible = Me.IsApprovingPerson
        CType(e.Item.FindControl("cmdInvoice"), HyperLink).Visible = Me.IsInvoicingPerson

     
        If cRec.schvaleno_pocet > 0 Then
            CType(e.Item.FindControl("schvaleno_pocet"), Label).Text = BO.BAS.FNI(cRec.schvaleno_pocet) & "x"
        Else
            CType(e.Item.FindControl("cmdReApprove"), HyperLink).Visible = False
            CType(e.Item.FindControl("cmdClearApprove"), HyperLink).Visible = False
            CType(e.Item.FindControl("cmdInvoice"), HyperLink).Visible = False
        End If
    End Sub
End Class