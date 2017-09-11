Public Class tag_binding
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub tag_binding_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hidPrefix.Value = Request.Item("prefix")
            hidPIDs.Value = Request.Item("pids")

            With Master
                .HeaderText = "Oštítkovat vybraný záznam"
                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With
        End If
    End Sub

    Private Sub tag_binding_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.cbxScope.Visible = Not chkCreate4All.Checked
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click
        Me.txtCreate.Text = Trim(Me.txtCreate.Text)
        If Not Master.Factory.o51TagBL.LoadByName(Me.txtCreate.Text) Is Nothing Then
            Master.Notify(String.Format("Štítek s názvem [%1%] již existuje!", Me.txtCreate.Text), NotifyLevel.ErrorMessage)
            Return
        End If
        Dim c As New BO.o51Tag
        c.o51Name = Me.txtCreate.Text
        If Me.chkCreate4All.Checked Then
            c.o51ScopeFlag = 1
        Else
            c.o51IsP41 = Me.cbxScope.Items.FindItemByValue("p41").Checked
            c.o51IsP28 = Me.cbxScope.Items.FindItemByValue("p28").Checked
            c.o51IsP56 = Me.cbxScope.Items.FindItemByValue("p56").Checked
            c.o51IsJ02 = Me.cbxScope.Items.FindItemByValue("j02").Checked
            c.o51IsP91 = Me.cbxScope.Items.FindItemByValue("p91").Checked
            c.o51IsP90 = Me.cbxScope.Items.FindItemByValue("p90").Checked
            c.o51IsO23 = Me.cbxScope.Items.FindItemByValue("o23").Checked
            c.o51IsP31 = Me.cbxScope.Items.FindItemByValue("p31").Checked
        End If
        With Master.Factory.o51TagBL
            If .Save(c) Then
                c = .LoadByName(c.o51Name)
                tags1.Entries.Insert(0, New Telerik.Web.UI.AutoCompleteBoxEntry(c.o51Name, c.PID.ToString))
                Me.txtCreate.Text = ""
            Else

            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Dim pids As List(Of Integer) = BO.BAS.ConvertPIDs2List(hidPIDs.Value).Distinct.ToList
        End If
    End Sub
End Class