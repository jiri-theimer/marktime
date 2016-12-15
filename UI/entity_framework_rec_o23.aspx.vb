Public Class entity_framework_rec_o23
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    

    Private Sub entity_framework_rec_o23_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        menu1.Factory = Master.Factory
        gridO23.Factory = Master.Factory
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                Me.CurrentMasterPrefix = Request.Item("masterprefix")
                If .DataPID = 0 Or Me.CurrentMasterPrefix = "" Then .StopPage("masterpid or masterprefix is missing")

                .SiteMenuValue = Me.CurrentMasterPrefix

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add(Me.CurrentMasterPrefix & "_menu-tabskin")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    menu1.TabSkin = .GetUserParam(Me.CurrentMasterPrefix & "_menu-tabskin")
                End With
            End With

            RefreshRecord()
        End If

        gridO23.x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
        gridO23.MasterDataPID = Master.DataPID

    End Sub
    Private Sub RefreshRecord()
        menu1.DataPrefix = Me.CurrentMasterPrefix
        Select Case Me.CurrentMasterPrefix
            Case "p41"
                Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
                Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
                Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

                menu1.p41_RefreshRecord(cRec, cRecSum, "o23")
        End Select

    End Sub
End Class