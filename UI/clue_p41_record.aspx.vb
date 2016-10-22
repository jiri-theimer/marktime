﻿Public Class clue_p41_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Clue

    Private Sub clue_p41_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
            If Master.DataPID = 0 Then Master.StopPage("pid is missing", , , False)

            RefreshRecord()

            ''comments1.RefreshData(Master.Factory, BO.x29IdEnum.p41Project, Master.DataPID)
        End If
    End Sub

    Private Sub RefreshRecord()
        
        Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
        With cRec
            ph1.Text = .p41Code & " - " & .p41Name
            Me.Client.Text = .Client
            Me.j18Name.Text = .j18Name
            Me.p42Name.Text = .p42Name
            Me.p51Name_Billing.Text = .p51Name_Billing
            Me.Owner.Text = .Owner
            Me.Timestamp.Text = .Timestamp
            Me.b02Name.Text = .b02Name
            If Not (.p41PlanFrom Is Nothing Or .p41PlanUntil Is Nothing) Then
                Me.p41PlanFrom.Text = BO.BAS.FD(.p41PlanFrom.Value)
                Me.p41PlanUntil.Text = BO.BAS.FD(.p41PlanUntil.Value)
            Else
                trDates.Visible = False
            End If
            If .p41ParentID > 0 Then
                ParentProject.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, .p41ParentID)
                ParentProject.NavigateUrl = "p41_framework.aspx?pid=" & .p41ParentID.ToString
            Else
                ParentProject.Visible = False
            End If
        End With
        If Master.Factory.p41ProjectBL.IsMyFavouriteProject(cRec.PID) Then
            cmdFavourite.ImageUrl = "Images/favourite.png"
            cmdFavourite.ToolTip = "Odstranit z mých oblíbených projektů"
        Else
            cmdFavourite.ImageUrl = "Images/not_favourite.png"
            cmdFavourite.ToolTip = "Zařadit do mých oblíbených projektů"
        End If

        Dim lisP30 As IEnumerable(Of BO.p30Contact_Person) = Master.Factory.p30Contact_PersonBL.GetList(0, Master.DataPID, 0)
        If lisP30.Count > 0 Then
            Me.persons1.FillData(lisP30)
        Else
            panP30.Visible = False
        End If

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.p41Project, cRec.PID)
        Me.roles_project.RefreshData(lisX69, cRec.PID)

    End Sub

    Private Sub cmdFavourite_Click(sender As Object, e As ImageClickEventArgs) Handles cmdFavourite.Click
        Master.Factory.j03UserBL.AppendOrRemoveFavouriteProject(Master.Factory.SysUser.PID, BO.BAS.ConvertPIDs2List(Master.DataPID), Master.Factory.p41ProjectBL.IsMyFavouriteProject(Master.DataPID))
        RefreshRecord()
    End Sub
End Class