Public Class p41_framework_rec_p31
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Private Sub p41_framework_rec_p31_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        gridP31.Factory = Master.Factory
        menu1.Factory = Master.Factory
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            With Master
                .SiteMenuValue = "p41"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                gridP31.MasterTabAutoQueryFlag = Request.Item("p31tabautoquery")

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p41_framework_detail-pid")
                    .Add("p41_framework_detail-tab")
                    .Add("p41_framework_detail-tabskin")
                    .Add("p41_framework_detail-chkFFShowFilledOnly")
                    .Add("p41_framework_detail-switch")
                    .Add("p41_framework_detail_pos")
                    ''.Add("p41_framework_detail-switchHeight")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)

                End With
            End With

            RefreshRecord()

            If Request.Item("p31id") <> "" Then
                gridP31.DefaultSelectedPID = BO.BAS.IsNullInt(Request.Item("p31id"))
            End If

        End If

        gridP31.MasterDataPID = Master.DataPID
        gridP31.EntityX29ID = BO.x29IdEnum.p41Project
        gridP31.AllowApproving = BO.BAS.BG(Me.hidIsCanApprove.Value)

    End Sub

    Private Sub RefreshRecord()
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        
        Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

        menu1.RefreshRecord(cRec, cRecSum, "p31")
    End Sub

End Class