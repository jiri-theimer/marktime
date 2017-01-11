Public Class entity_framework_rec_summary
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

    Private Sub entity_framework_rec_summary_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        menu1.Factory = Master.Factory
        summary1.Factory = Master.Factory
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                Me.CurrentMasterPrefix = Request.Item("masterprefix")
                If .DataPID = 0 Or Me.CurrentMasterPrefix = "" Then .StopPage("masterpid or masterprefix is missing")

                .SiteMenuValue = Me.CurrentMasterPrefix
                menu1.DataPrefix = Me.CurrentMasterPrefix

                summary1.CurrentMasterPrefix = Me.CurrentMasterPrefix
                summary1.CurrentMasterPID = Master.DataPID

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add(Me.CurrentMasterPrefix & "_menu-tabskin")
                    .Add("p31_drilldown-pagesize")
                    .Add(Me.CurrentMasterPrefix & "-j75id")
                    .Add("p31_drilldown-j70id")
                    .Add("p31_grid-period")
                    .Add("periodcombo-custom_query")
                    .Add(Me.CurrentMasterPrefix & "_menu-x31id-plugin")
                    .Add("p31_drilldown-includechilds")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    menu1.TabSkin = .GetUserParam(Me.CurrentMasterPrefix & "_menu-tabskin")
                    menu1.x31ID_Plugin = .GetUserParam(Me.CurrentMasterPrefix & "_menu-x31id-plugin")
                    summary1.FirstSetup(.GetUserParam("p31_drilldown-pagesize", "20"), .GetUserParam("p31_grid-period"), .GetUserParam("periodcombo-custom_query"), .GetUserParam(Me.CurrentMasterPrefix & "-j75id"), .GetUserParam("p31_drilldown-j70id"))
                    If summary1.CurrentMasterPrefix = "p41" Then
                        summary1.IncludeEntityChilds = BO.BAS.BG(.GetUserParam("p31_drilldown-includechilds"))
                    End If

                End With
            End With

            RefreshRecord()
        End If

        menu1.DataPID = Master.DataPID
        
    End Sub

    Private Sub RefreshRecord()

        Select Case Me.CurrentMasterPrefix
            Case "p41"
                Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
                Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
                Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

                menu1.p41_RefreshRecord(cRec, cRecSum, "summary")
                summary1.IsApprovingPerson = menu1.IsExactApprovingPerson
                If cRec.p41TreeNext > cRec.p41TreePrev Then summary1.EnableEntityChilds = True

            Case "p28"
                Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
                Dim cRecSum As BO.p28ContactSum = Master.Factory.p28ContactBL.LoadSumRow(cRec.PID)
                menu1.p28_RefreshRecord(cRec, cRecSum, "summary")
                summary1.IsApprovingPerson = Master.Factory.SysUser.IsApprovingPerson
            Case "j02"
                Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
                Dim cRecSum As BO.j02PersonSum = Master.Factory.j02PersonBL.LoadSumRow(cRec.PID)
                menu1.j02_RefreshRecord(cRec, cRecSum, "summary")
                summary1.IsApprovingPerson = Master.Factory.SysUser.IsApprovingPerson
        End Select

    End Sub
End Class