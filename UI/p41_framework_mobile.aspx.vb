Public Class p41_framework_mobile
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile

    Private Sub p41_framework_mobile_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        

        If Not Page.IsPostBack Then
            With Master
                .MenuPrefix = "p41"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
            End With
            Me.dat1.SelectedDate = Now.AddDays(-10)
            Me.dat2.SelectedDate = Now


            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            Return
        End If
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        Dim cClient As BO.p28Contact = Nothing
        With cRec
            Me.ProjectHeader.Text = .p41Name & " (" & .p41Code & ")"
            Me.ProjectHeader.NavigateUrl = "p41_framework_mobile.aspx?pid=" & .PID.ToString
            If .p28ID_Client <> 0 Then
                cClient = Master.Factory.p28ContactBL.Load(.p28ID_Client)
                Me.Client.Text = .Client
                Me.Client.NavigateUrl = "p28_framework_mobile.aspx?pid=" & .p28ID_Client.ToString
                Me.clue_client.Attributes("rel") = "clue_p28_record.aspx?pid=" & .p28ID_Client.ToString
            Else
                Me.clue_client.Visible = False : Me.Client.Visible = False
            End If

            Me.clue_p42id.Attributes("rel") = "clue_p42_record.aspx?pid=" & .p42ID.ToString
            Me.p42Name.Text = .p42Name
        End With

        RefreshPricelist(cRec, cClient)

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)

    End Sub

    Private Sub RefreshPricelist(cRec As BO.p41Project, cClient As BO.p28Contact)
        Me.PriceList_Billing_Message.Text = ""
        If Not Master.Factory.TestPermission(BO.x53PermValEnum.GR_P31_AllowRates) Then
            Me.panPricelist.Visible = False  'uživatel nemá oprávnění vidět sazby
            Return
        End If
        Dim bolVisible As Boolean = False
        With cRec
            If .p51ID_Billing > 0 Then
                bolVisible = True
                If .p51Name_Billing.IndexOf(cRec.p41Name) >= 0 Then
                    'sazby na míru
                    Me.PriceList_Billing.Text = "Tento projekt má sazby na míru"
                End If
                Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
            Else
                If Not cClient Is Nothing Then
                    With cClient
                        If .p51ID_Billing > 0 Then
                            bolVisible = True
                            Me.PriceList_Billing_Message.Text = "(dědí se z klienta)"
                            Me.PriceList_Billing.Text = .p51Name_Billing
                            Me.clue_p51id_billing.Attributes("rel") = "clue_p51_record.aspx?pid=" & .p51ID_Billing.ToString
                        End If
                    End With
                End If
            End If
        End With
        Me.clue_p51id_billing.Visible = bolVisible

    End Sub

    Private Sub p41_framework_mobile_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'row1.Controls.Remove(inv1)
        'panKos.Controls.Add(inv1)
        'inv1.CssClass = ""

        'inv1.CssClass = "col-sm-6 col-md-4"
        'row1.Controls.Add(inv1)
        'inv2.CssClass = "col-sm-6 col-md-4"
        'row1.Controls.Add(inv2)
    End Sub

    Private Sub p41_framework_mobile_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

       
    End Sub

    Private Sub cmdDate_Click(sender As Object, e As EventArgs) Handles cmdDate.Click
        Master.Notify(Me.txtDate.Text)

    End Sub

    Private Sub cmdDatum_Click(sender As Object, e As EventArgs) Handles cmdDatum.Click
        If Me.dat1.SelectedDate Is Nothing Then
            Master.Notify("Nothing")
        Else
            Master.Notify(Me.dat1.SelectedDate)
        End If
       
    End Sub

    Private Sub cmdDatum2_Click(sender As Object, e As EventArgs) Handles cmdDatum2.Click
        If Me.dat2.SelectedDate Is Nothing Then
            Master.Notify("Nothing")
        Else
            Master.Notify(Me.dat2.SelectedDate)
        End If
    End Sub
End Class