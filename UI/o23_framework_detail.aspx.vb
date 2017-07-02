Public Class o23_framework_detail
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm
    Public Property CurrentX18ID As Integer
        Get
            Return BO.BAS.IsNullInt(hidX18ID.Value)
        End Get
        Set(value As Integer)
            hidX18ID.Value = value.ToString
        End Set
    End Property

    Private Sub o23_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rec1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                hidX18ID.Value = Request.Item("x18id")
                If Me.CurrentX18ID = 0 Then
                    .StopPage("")
                End If

            End With

            RefreshRecord()
        End If


    End Sub
    Private Function FNO(strValue As String) As Telerik.Web.UI.NavigationNode
        Return menu1.GetAllNodes.First(Function(p) p.ID = strValue)
    End Function
    Private Sub RefreshRecord()

        If Master.DataPID = 0 Then Return
        Dim cRec As BO.o23Doc = Master.Factory.o23DocBL.Load(Master.DataPID)
        If cRec Is Nothing Then Return

        Dim cX18 As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Me.CurrentX18ID)
        If cX18.x18Icon32 <> "" Then

            imgIcon32.ImageUrl = cX18.x18Icon32
        End If
        If cX18.b01ID <> 0 Then
            hidB01ID.Value = cX18.b01ID.ToString
            FNO("cmdWorkflow").Text = "Posunout stav/doplnit"
            FNO("cmdWorkflow").ImageUrl = "Images/workflow.png"
            FNO("cmdWorkflow").NavigateUrl = "javascript:workflow()"
        Else
            hidB01ID.Value = ""
            FNO("cmdWorkflow").Text = "Doplnit poznámku, komentář, přílohu"
            FNO("cmdWorkflow").ImageUrl = "Images/comment.png"
            FNO("cmdWorkflow").NavigateUrl = "javascript:b07_create()"
        End If


        Dim cDisp As BO.x18RecordDisposition = Master.Factory.x18EntityCategoryBL.InhaleDisposition(cX18)
        FNO("cmdNew").Visible = cDisp.CreateItem
        FNO("cmdClone").Visible = cDisp.CreateItem
        If cRec.IsClosed Then menu1.Skin = "Black"

        rec1.FillData(cRec, cX18)

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.o23Doc, cRec.PID)
        roles1.RefreshData(lisX69, cRec.PID)
        comments1.RefreshData(Master.Factory, BO.x29IdEnum.o23Doc, Master.DataPID)

        FNO("reload").NavigateUrl = "o23_framework_detail.aspx?pid=" & Master.DataPID.ToString & "&x18id=" & Me.CurrentX18ID.ToString
    End Sub


End Class