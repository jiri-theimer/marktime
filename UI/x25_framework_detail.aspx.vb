Public Class x25_framework_detail
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

    Private Sub x25_framework_detail_Init(sender As Object, e As EventArgs) Handles Me.Init
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


                'Dim lisPars As New List(Of String)
                'With lisPars

                'End With
                'With .Factory.j03UserBL
                '    .InhaleUserParams(lisPars)
                'End With
                'With .Factory.j03UserBL


                'End With

            End With

            RefreshRecord()
        End If


    End Sub

    Private Sub RefreshRecord()
        menu1.FindItemByValue("begin").Controls.Add(New LiteralControl("<img src='Images/label_32.png'/>"))

        If Master.DataPID = 0 Then Return
        Dim cRec As BO.x25EntityField_ComboValue = Master.Factory.x25EntityField_ComboValueBL.Load(Master.DataPID)
        If cRec Is Nothing Then Return

        Dim cX18 As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.Load(Me.CurrentX18ID)

        If cX18.b01ID <> 0 Then
            hidB01ID.Value = cX18.b01ID.ToString
            menu1.FindItemByValue("cmdWorkflow").Text = "Posunout stav/doplnit"
            menu1.FindItemByValue("cmdWorkflow").ImageUrl = "Images/workflow.png"
            menu1.FindItemByValue("cmdWorkflow").NavigateUrl = "javascript:workflow()"
        Else
            hidB01ID.Value = ""
            menu1.FindItemByValue("cmdWorkflow").Text = "Doplnit poznámku, komentář, přílohu"
            menu1.FindItemByValue("cmdWorkflow").ImageUrl = "Images/comment.png"
            menu1.FindItemByValue("cmdWorkflow").NavigateUrl = "javascript:b07_create()"
        End If


        Dim cDisp As BO.x18RecordDisposition = Master.Factory.x18EntityCategoryBL.InhaleDisposition(cX18)
        menu1.FindItemByValue("cmdNew").Visible = cDisp.CreateItem
        menu1.FindItemByValue("cmdClone").Visible = cDisp.CreateItem


        rec1.FillData(cRec, cX18)

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.x25EntityField_ComboValue, cRec.PID)
        roles1.RefreshData(lisX69, cRec.PID)
        comments1.RefreshData(Master.Factory, BO.x29IdEnum.x25EntityField_ComboValue, Master.DataPID)

        menu1.FindItemByValue("reload").NavigateUrl = "x25_framework_detail.aspx?pid=" & Master.DataPID.ToString & "&x18id=" & Me.CurrentX18ID.ToString
    End Sub

    Private Sub Handle_ChangeX18ID()

        
        'If c.b01ID <> 0 Then
        '    hidB01ID.Value = c.b01ID.ToString
        '    menu1.FindItemByValue("cmdWorkflow").Text = "Posunout stav/doplnit"
        '    menu1.FindItemByValue("cmdWorkflow").ImageUrl = "Images/workflow.png"
        '    menu1.FindItemByValue("cmdWorkflow").NavigateUrl = "javascript:workflow()"
        'Else
        '    hidB01ID.Value = ""
        '    menu1.FindItemByValue("cmdWorkflow").Text = "Doplnit komentář nebo přílohu"
        '    menu1.FindItemByValue("cmdWorkflow").ImageUrl = "Images/comment.png"
        '    menu1.FindItemByValue("cmdWorkflow").NavigateUrl = "javascript:b07_create()"
        'End If
        'With menu1.FindItemByValue("scheduler")
        '    .Visible = c.x18IsCalendar
        '    If .Visible Then .NavigateUrl = "x25_scheduler.aspx?x18id=" & Me.CurrentX18ID.ToString
        'End With

        'Dim cDisp As BO.x18RecordDisposition = Master.Factory.x18EntityCategoryBL.InhaleDisposition(c)
        'menu1.FindItemByValue("cmdNew").Visible = cDisp.CreateItem

    End Sub
End Class