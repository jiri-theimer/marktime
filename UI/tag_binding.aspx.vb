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
                If hidPrefix.Value = "" Or hidPIDs.Value = "" Then
                    .StopPage("prefix or pid is missing.")
                End If
                If hidPIDs.Value.IndexOf(".") > 0 Then
                    .HeaderText = "Oštítkovat vybrané záznamy"
                Else
                    .HeaderText = String.Format("Oštítkovat {0}", .Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(hidPrefix.Value), CInt(hidPIDs.Value)))
                End If
                .HeaderIcon = "Images/tag_32.png"

                .AddToolbarButton("Uložit změny", "ok", , "Images/save.png")
            End With

            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim strPID As String = Split(hidPIDs.Value, ",")(0)
        Dim lis As IEnumerable(Of BO.o52TagBinding) = Master.Factory.o51TagBL.GetList_o52(hidPrefix.Value, CInt(strPID))
        With tags1.Entries
            For Each c In lis
                .Add(New Telerik.Web.UI.AutoCompleteBoxEntry(c.o51Name, c.o51ID.ToString))
            Next
        End With

    End Sub

    Private Sub tag_binding_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Me.cbxScope.Visible = Not chkCreate4All.Checked
        If Not chkCreate4All.Checked Then
            If Me.cbxScope.CheckedItems.Count = 0 Then
                If Not Me.cbxScope.Items.FindItemByValue(Me.hidPrefix.Value) Is Nothing Then
                    Me.cbxScope.Items.FindItemByValue(Me.hidPrefix.Value).Checked = True
                End If
            End If
        End If
        If Me.cbxFind.SelectedValue <> "" Then
            Me.cmdSave.Visible = True
            panRecord.Visible = True
            panEntities.Visible = Not Me.o51ScopeFlag.Checked
        Else
            Me.cmdSave.Visible = False
            panRecord.Visible = False
        End If
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click
        Me.txtCreate.Text = Trim(Me.txtCreate.Text)
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
                c = .LoadByName(c.o51Name, 0)
                tags1.Entries.Insert(0, New Telerik.Web.UI.AutoCompleteBoxEntry(c.o51Name, c.PID.ToString))
                Me.txtCreate.Text = ""
                Me.txtCreate.Focus()
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Dim pids As List(Of Integer) = BO.BAS.ConvertPIDs2List(hidPIDs.Value).Distinct.ToList
            Dim o51IDs As New List(Of Integer)
            For i As Integer = 0 To tags1.Entries.Count - 1
                o51IDs.Add(CInt(tags1.Entries.Item(i).Value))
            Next
            
            For Each intRecordPID As Integer In pids
                If Not Master.Factory.o51TagBL.SaveBinding(hidPrefix.Value, intRecordPID, o51IDs) Then
                    Master.Notify(Master.Factory.o51TagBL.ErrorMessage, NotifyLevel.ErrorMessage)
                Else
                    Master.CloseAndRefreshParent()
                End If
            Next
        End If
    End Sub

    Private Sub cbxFind_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cbxFind.SelectedIndexChanged
        Dim intO51ID As Integer = BO.BAS.IsNullInt(Me.cbxFind.SelectedValue)
        If intO51ID = 0 Then Return
        Dim cRec As BO.o51Tag = Master.Factory.o51TagBL.Load(intO51ID)
        With cRec
            Me.o51Name.Text = .o51Name
            If .o51ScopeFlag = 1 Then
                o51ScopeFlag.Checked = True
            Else
                o51ScopeFlag.Checked = False
                Me.o51IsJ02.Checked = .o51IsJ02
                Me.o51IsO23.Checked = .o51IsO23
                Me.o51IsP28.Checked = .o51IsP28
                Me.o51IsP31.Checked = .o51IsP31
                Me.o51IsP41.Checked = .o51IsP41
                Me.o51IsP56.Checked = .o51IsP56
                Me.o51IsP90.Checked = .o51IsP90
                Me.o51IsP91.Checked = .o51IsP91
            End If
            Me.Timestamp.Text = .Timestamp
        End With


    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        Dim intO51ID As Integer = BO.BAS.IsNullInt(Me.cbxFind.SelectedValue)
        If intO51ID = 0 Then Return
        Dim cRec As BO.o51Tag = Master.Factory.o51TagBL.Load(intO51ID)
        With cRec
            .o51Name = Me.o51Name.Text
            If o51ScopeFlag.Checked Then
                .o51ScopeFlag = 1
            Else
                .o51ScopeFlag = 0
                .o51IsJ02 = Me.o51IsJ02.Checked
                .o51IsO23 = Me.o51IsO23.Checked
                .o51IsP28 = Me.o51IsP28.Checked
                .o51IsP31 = Me.o51IsP31.Checked
                .o51IsP41 = Me.o51IsP41.Checked
                .o51IsP56 = Me.o51IsP56.Checked
                .o51IsP90 = Me.o51IsP90.Checked
                .o51IsP91 = Me.o51IsP91.Checked
            End If
            
        End With
        With Master.Factory.o51TagBL
            If .Save(cRec) Then
                Me.cbxFind.Text = "" : Me.cbxFind.SelectedValue = ""
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub
End Class