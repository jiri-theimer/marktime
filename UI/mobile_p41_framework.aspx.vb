Public Class mobile_p41_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile

    Private Sub mobile_p41_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .MenuPrefix = "p41"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("p41_framework_detail-pid")
                    .Add("mobile_p41_framework-subgrid")
                   
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    
                End With
                If .DataPID = 0 Then
                    .DataPID = BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p41_framework_detail-pid", "O"))
                    If .DataPID = 0 Then Response.Redirect("mobile_entity_framework_missing.aspx?prefix=p41")
                Else
                    If .DataPID <> BO.BAS.IsNullInt(.Factory.j03UserBL.GetUserParam("p41_framework_detail-pid", "O")) Then
                        .Factory.j03UserBL.SetUserParam("p41_framework_detail-pid", .DataPID.ToString)
                    End If
                End If

            End With

            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then Return

        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        Dim cClient As BO.p28Contact = Nothing
        With cRec
            Me.RecordHeader.Text = BO.BAS.OM3(.p41Name, 30)
            Me.RecordHeader.NavigateUrl = "mobile_p41_framework.aspx?pid=" & .PID.ToString
            Me.RecordName.Text = "[" & .p41Code & "] " & .p41Name

            If .p28ID_Client <> 0 Then
                cClient = Master.Factory.p28ContactBL.Load(.p28ID_Client)
                Me.Client.Text = .Client
                Me.Client.NavigateUrl = "mobile_p28_framework.aspx?pid=" & .p28ID_Client.ToString
            Else
                Me.Client.Visible = False
            End If
            Me.p42Name.Text = .p42Name

            If .p51ID_Billing > 0 Then
                If .p51Name_Billing.IndexOf(cRec.p41Name) >= 0 Then
                    'sazby na míru
                    Me.PriceList_Billing.Text = "Tento projekt má sazby na míru"
                End If
            Else
                If Not cClient Is Nothing Then
                    With cClient
                        If .p51ID_Billing > 0 Then
                            Me.PriceList_Billing.Text = .p51Name_Billing & " (dědí se z klienta)"
                        End If
                    End With
                End If
            End If
            If .b02ID > 0 Then
                Me.b02Name.Text = .b02Name
            Else
                trB02.Visible = False
            End If
            If Not (.p41PlanFrom Is Nothing Or .p41PlanUntil Is Nothing) Then
                Me.p41PlanFrom.Text = BO.BAS.FD(.p41PlanFrom)
                Me.p41PlanUntil.Text = BO.BAS.FD(.p41PlanUntil)
            Else
                trPlanPeriod.Visible = False
            End If
            Me.imgDraft.Visible = .p41IsDraft
        End With

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        
        Me.roles_project.RefreshData(lisX69, cRec.PID)

        Dim mqO23 As New BO.myQueryO23
        mqO23.p41ID = Master.DataPID
        mqO23.SpecificQuery = BO.myQueryO23_SpecificQuery.AllowedForRead
        Dim lisO23 As IEnumerable(Of BO.o23Notepad) = Master.Factory.o23NotepadBL.GetList(mqO23)
        If lisO23.Count > 0 Then
            Me.boxO23.Visible = True
            notepad1.RefreshData(lisO23, cRec.PID)
            CountO23.Text = lisO23.Count.ToString
        Else
            Me.boxO23.Visible = False
        End If

        Dim lisP30 As IEnumerable(Of BO.j02Person) = Master.Factory.p30Contact_PersonBL.GetList_J02(0, Master.DataPID, False)
        If lisP30.Count > 0 Then
            Me.boxP30.Visible = True
            Me.persons1.FillData(lisP30)
            Me.CountP30.Text = lisP30.Count.ToString
            Me.titleP30.NavigateUrl = "#"
            
        Else
            Me.boxP30.Visible = False
        End If

        RefreshBillingLanguage(cRec, cClient)
    End Sub

    Private Sub RefreshBillingLanguage(cRec As BO.p41Project, cClient As BO.p28Contact)
        imgFlag_Project.Visible = False : imgFlag_Client.Visible = False
        With cRec
            If .p87ID > 0 Then
                Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID)
                If cP87.p87Icon <> "" Then
                    imgFlag_Project.Visible = True
                    imgFlag_Project.ImageUrl = "Images/flags/" & cP87.p87Icon
                End If

            End If
            If .p87ID_Client > 0 Then
                If Not cClient Is Nothing Then
                    Dim cP87 As BO.p87BillingLanguage = Master.Factory.ftBL.LoadP87(.p87ID_Client)
                    If cP87.p87Icon <> "" Then
                        imgFlag_Client.Visible = True
                        imgFlag_Client.ImageUrl = "Images/flags/" & cP87.p87Icon
                    End If
                End If
            End If
        End With
    End Sub
End Class