Imports Telerik.Web.UI

Public Class admin_workflow
    Inherits System.Web.UI.Page

    Public Property CurrentB01ID As Integer
        Get
            Return BO.BAS.IsNullInt(cbxB01ID.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.cbxB01ID, value.ToString)
        End Set
    End Property
    Public Property CurrentX29ID As BO.x29IdEnum
        Get
            If Me.hidcurx29id.Value = "" Then Return BO.x29IdEnum.p41Project
            Return CType(Me.hidcurx29id.Value, BO.x29IdEnum)
        End Get
        Set(value As BO.x29IdEnum)
            Me.hidcurx29id.Value = CInt(value).ToString
        End Set
    End Property
    Public Property CurrentB02ID As Integer
        Get
            Return BO.BAS.IsNullInt(hidcurb02id.Value)
        End Get
        Set(value As Integer)
            hidcurb02id.Value = value.ToString
        End Set
    End Property
    Public Property CurrentB06ID As Integer
        Get
            Return BO.BAS.IsNullInt(hidcurb06id.Value)
        End Get
        Set(value As Integer)
            hidcurb06id.Value = value.ToString
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .PageTitle = "Workflow návrhář"
                .SiteMenuValue = "admin_framework"
                .TestNeededPermission(BO.x53PermValEnum.GR_Admin)

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("admin_workflow-b01id")
                    .Add("admin_workflow-showb65")
                    .Add("admin_workflow-include_nonactual")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)
            End With

            Me.cbxB01ID.DataSource = Master.Factory.b01WorkflowTemplateBL.GetList()
            Me.cbxB01ID.DataBind()

            Me.CurrentB01ID = BO.BAS.IsNullInt(Request.Item("b01id"))
            With Master.Factory.j03UserBL
                If Me.CurrentB01ID = 0 Then
                    Me.CurrentB01ID = BO.BAS.IsNullInt(.GetUserParam("admin_workflow-b01id"))
                End If
                chkShowB65.Checked = BO.BAS.BG(.GetUserParam("admin_workflow-showb65", "1"))
                Me.chkIncludeNonActual.Checked = BO.BAS.BG(.GetUserParam("admin_workflow-include_nonactual", "0"))
            End With
            ''With Master
            ''    .ActivateContextMenu(True)
            ''    .AddContextMenu("Nový stav", "javascript:b02_new()", True)
            ''    .AddContextMenu("Detail vybraného stavu", "javascript:b02_edit()", True, , , "b02_edit")

            ''    .AddContextMenu("Nový krok", "javascript:b06_new(true)", True, , , "b06_new")


            ''    .AddContextMenu("Hlavička šablony", "javascript:b01_edit()", True)
            ''    '.AddContextMenu("Kopírovat", "javascript:b01_clone()", True)
            ''    .AddContextMenu("Export do XML", "javascript:b01_export()", True)
            ''    .AddContextMenu("Import z XML", "javascript:b01_import()", True)
            ''End With

            

            RefreshRecord()


        End If
    End Sub
    Private Sub RefreshState()
        panB65.Visible = chkShowB65.Checked
        If Me.CurrentB02ID = 0 Then
            cmdNewB06.Visible = False
        Else
            cmdNewB06.Visible = True
        End If
    End Sub


    

    Private Sub RefreshRecord()
        Me.CurrentB02ID = 0
        If Me.CurrentB01ID = 0 Then
            Return
        End If
        Dim cRec As BO.b01WorkflowTemplate = Master.Factory.b01WorkflowTemplateBL.Load(Me.CurrentB01ID)
        Me.CurrentX29ID = cRec.x29ID
        If chkShowB65.Checked Then
            RefreshB65List(cRec)
        End If

        RefreshTree()
    End Sub

    Private Sub RefreshB65List(cRec As BO.b01WorkflowTemplate)

        Dim lis As IEnumerable(Of BO.b65WorkflowMessage) = Master.Factory.b65WorkflowMessageBL.GetList(New BO.myQuery).Where(Function(p) p.x29ID = Me.CurrentX29ID)

        rpB65.DataSource = lis
        rpB65.DataBind()
        ph1.Text = BO.BAS.OM2(ph1.Text, lis.Count.ToString)
    End Sub
    

    Private Sub rpB65_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpB65.ItemDataBound
        Dim cRec As BO.b65WorkflowMessage = CType(e.Item.DataItem, BO.b65WorkflowMessage)
        With CType(e.Item.FindControl("b65name"), HyperLink)
            .Text = cRec.b65Name
            .NavigateUrl = "javascript:b65_edit(" & cRec.PID.ToString & ")"
        End With
        CType(e.Item.FindControl("b65MessageSubject"), Label).Text = cRec.b65MessageSubject
    End Sub

    Private Sub chkShowB65_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkShowB65.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("admin_workflow-showb65", BO.BAS.GB(chkShowB65.Checked))
        RefreshRecord()
    End Sub

    Private Sub RefreshTree()
        Dim myQuery As New BO.myQuery
        If Me.chkIncludeNonActual.Checked Then
            myQuery.Closed = BO.BooleanQueryMode.NoQuery
        End If
        Dim lisB02 As IEnumerable(Of BO.b02WorkflowStatus) = Master.Factory.b02WorkflowStatusBL.GetList(Me.CurrentB01ID)
        Dim lisB06 As IEnumerable(Of BO.b06WorkflowStep) = Master.Factory.b06WorkflowStepBL.GetList(Me.CurrentB01ID)
        With tree1
            .Clear()
            For Each cRec As BO.b02WorkflowStatus In lisB02
                Dim intB02ID As Integer = cRec.PID
                If Me.CurrentB02ID = 0 Then
                    Me.CurrentB02ID = intB02ID
                End If
                Dim strImgB02 As String = "Images/a14.gif"
               
                Dim n As New RadTreeNode(cRec.b02Name, "b02-" & intB02ID.ToString)
                If cRec.b02Code <> "" Then n.Text += " (" & cRec.b02Code & ")"
                n.NavigateUrl = "javascript:b02_click(" & intB02ID.ToString & ")"
                n.ImageUrl = strImgB02
                n.ToolTip = "Workflow stav [" & cRec.b02Code & "]"
                If cRec.IsClosed Then n.Font.Strikeout = True
                .AddItem(n)

                For Each cB06 As BO.b06WorkflowStep In lisB06.Where(Function(p As BO.b06WorkflowStep) p.b02ID = intB02ID)
                    Dim strName As String = cB06.b06Name
                    Dim strIMG As String = "Images/a13.gif"
                    If cB06.b02ID_Target <> 0 Then
                        strName += "->" & cB06.TargetStatus
                        strIMG = "Images/a12.gif"
                    End If
                    If cB06.b06IsKickOffStep Then
                        strIMG = "Images/star.png"
                    End If
                    n = New RadTreeNode(strName, "b06-" & cB06.PID.ToString)
                    If Not cB06.b06IsManualStep Then
                        n.ForeColor = Drawing.Color.Brown    'neuživatelský krok
                        n.Font.Italic = True
                    End If
                    
                    n.ImageUrl = strIMG
                    n.ToolTip = "Workflow krok"
                    n.NavigateUrl = "javascript:b06_click(" & cB06.PID.ToString & ")"
                    If cB06.IsClosed Then n.Font.Strikeout = True

                    .AddItem(n, "b02-" & intB02ID.ToString)
                Next
            Next
            .SelectedValue = "b02-" & Me.CurrentB02ID.ToString
            .ExpandAll()
        End With

        RefreshB02Record()
        ''If lisB02.Where(Function(p) p.b02IsDefaultStatus = True).Count = 0 Then
        ''    Master.Notify("Pozor, tato workflow šablona zatím nemá nastavený výchozí (startovací) stav!", 1)
        ''End If
    End Sub

    Private Sub RefreshB02Record()
        panB02Rec.Visible = False
        If Me.CurrentB02ID = 0 Then
            Return
        End If

        panB02Rec.Visible = True
        Dim cRec As BO.b02WorkflowStatus = Master.Factory.b02WorkflowStatusBL.Load(Me.CurrentB02ID)
        With cRec
            ab02name.Text = .b02Name
            ab02name.NavigateUrl = "javascript:b02_edit(" & .PID.ToString & ")"
            ''Master.ChangeContextMenuItem("b02_edit", "Detail stavu [" & .b02Name & "]", "javascript:b02_edit(" & .PID.ToString & ")")
            ''Master.ChangeContextMenuItem("b06_new", "Nový krok v rámci stavu [" & .b02Name & "]", "javascript:b06_new()")
            b02ident.Text = .b02Code
            If .b02Color <> "" Then
                b02ident.Style.Item("background-color") = .b02Color
            Else
                b02ident.Style.Item("background-color") = ""
            End If
        End With
        
        Dim lisB06 As IEnumerable(Of BO.b06WorkflowStep) = Master.Factory.b06WorkflowStepBL.GetList(Me.CurrentB01ID).Where(Function(p) p.b02ID = cRec.PID)
        rpB02_B06.DataSource = lisB06
        rpB02_B06.DataBind()


        ''Dim lisB10 As IEnumerable(Of BO.b10WorkflowCommandCatalog_Binding) = Master.Factory.b02WorkflowStatusBL.GetList_B10(cRec.PID)
        ''rpB10.DataSource = lisB10
        ''rpB10.DataBind()

    End Sub

    Private Sub cmdRefreshOnBehind_Click(sender As Object, e As System.EventArgs) Handles cmdRefreshOnBehind.Click
        Select Case hidcurflag.Value
            Case "b01-save"
                Response.Redirect("admin_workflow.aspx?b01id=" & Me.CurrentB01ID.ToString)
            Case "b01-delete"
                Master.Factory.j03UserBL.SetUserParam("admin_workflow-b01id", "")
                Response.Redirect("admin_workflow.aspx")
            Case "b02-save", "b06-save", "b06-delete"
                RefreshTree()
            Case "b02-change"  'výběr jiného stavu
                RefreshB02Record()
            Case "b06-change"   'výběr jiného kroku
                Dim cRec As BO.b06WorkflowStep = Master.Factory.b06WorkflowStepBL.Load(Me.CurrentB06ID)
                Me.CurrentB02ID = cRec.b02ID
                RefreshB02Record()
            Case "b65-save", "b65-delete"
                If chkShowB65.Checked Then
                    Dim cRec As BO.b01WorkflowTemplate = Master.Factory.b01WorkflowTemplateBL.Load(Me.CurrentB01ID)
                    RefreshB65List(cRec)
                End If
                RefreshB02Record()
            Case Else
                RefreshRecord()
        End Select
        hidcurflag.Value = ""
    End Sub

    Private Sub rpB02_B06_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpB02_B06.ItemDataBound
        Dim cRec As BO.b06WorkflowStep = CType(e.Item.DataItem, BO.b06WorkflowStep)
        With CType(e.Item.FindControl("b06name"), HyperLink)
            .Text = cRec.b06Name
            If cRec.TargetStatus <> "" Then .Text += "->" & cRec.TargetStatus
            .NavigateUrl = "javascript:b06_edit(" & cRec.PID.ToString & ")"
            
            If cRec.PID = Me.CurrentB06ID Then
                .Font.Bold = True
            End If
            If Not cRec.b06IsManualStep Then
                .ForeColor = Drawing.Color.Brown    'neuživatelský krok
                .Font.Italic = True
            End If
            If cRec.IsClosed Then
                .Font.Strikeout = True
            End If
        End With
        With CType(e.Item.FindControl("imgB06"), Image)
            If cRec.b02ID_Target <> 0 Then
                .ImageUrl = "Images/a12.gif"
            Else
                .ImageUrl = "Images/a13.gif"
            End If
            If cRec.b06IsKickOffStep Then
                .ImageUrl = "Images/star.png"
            End If
        End With

    End Sub

    
    Private Sub admin_workflow_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        RefreshState()
    End Sub

    Private Sub chkIncludeNonActual_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIncludeNonActual.CheckedChanged
        Master.Factory.j03UserBL.SetUserParam("admin_workflow-include_nonactual", BO.BAS.GB(chkIncludeNonActual.Checked))
        RefreshRecord()
    End Sub

    Private Sub rpB10_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpB10.ItemDataBound
        Dim cRec As BO.b10WorkflowCommandCatalog_Binding = CType(e.Item.DataItem, BO.b10WorkflowCommandCatalog_Binding)
        CType(e.Item.FindControl("b09name"), Label).Text = cRec.b09Name
    End Sub

    Private Sub cbxB01ID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxB01ID.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("admin_workflow-b01id", Me.CurrentB01ID.ToString)
        RefreshRecord()
    End Sub

    Private Sub cmdGenerateDump_Click(sender As Object, e As EventArgs) Handles cmdGenerateDump.Click
        Dim pars As New List(Of BO.PluginDbParameter)
        Dim par As New BO.PluginDbParameter("b01id", Me.CurrentB01ID)
        pars.Add(par)
        Dim s As String = "SELECT * from b01WorkflowTemplate where b01ID=@b01id"
        s += ";SELECT * FROM b02WorkflowStatus WHERE b01ID=@b01id"
        s += ";SELECT * FROM b65WorkflowMessage"
        s += ";SELECT * FROM b06WorkflowStep WHERE b02ID IN (SELECT b02ID FROM b02WorkflowStatus where b01ID=@b01id)"
        s += ";SELECT * from b08WorkflowReceiverToStep WHERE b06ID IN (SELECT a.b06ID FROM b06WorkflowStep a INNER JOIN b02WorkflowStatus b ON a.b02ID=b.b02ID)"
        s += ";SELECT * from b11WorkflowMessageToStep WHERE b06ID IN (SELECT a.b06ID FROM b06WorkflowStep a INNER JOIN b02WorkflowStatus b ON a.b02ID=b.b02ID)"
        s += ";SELECT * from b10WorkflowCommandCatalog_Binding WHERE b06ID IN (SELECT a.b06ID FROM b06WorkflowStep a INNER JOIN b02WorkflowStatus b ON a.b02ID=b.b02ID)"

        Dim ds As System.Data.DataSet = Master.Factory.pluginBL.GetDataSet(s, pars, "b01WorkflowTemplate,b02WorkflowStatus,b65WorkflowMessage,b06WorkflowStep,b08WorkflowReceiverToStep,b11WorkflowMessageToStep,b10WorkflowCommandCatalog_Binding")


        ds.WriteXml("c:\temp\pokus.xml", XmlWriteMode.WriteSchema)

        Dim lis As New List(Of BO.b01WorkflowTemplate)

    End Sub
End Class