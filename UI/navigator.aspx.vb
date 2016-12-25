Imports Telerik.Web.UI
Public Class navigator
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Public Class DTS
        Public Property PID As Integer
        Public Property ParentPID As Integer
        Public Property ItemText As String
        Public Property Prefix As String
        Public Property Level As Integer
        Public Property IsFinalLevel As Integer

        Public Sub New(pid As Integer, strText As String, strPrefix As String)
            Me.PID = pid
            Me.ItemText = strText
            Me.Prefix = strPrefix
        End Sub
    End Class

    Private Sub navigator_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
       

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'hidAutoScrollHashID.Value = ""
            
            With Master
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("navigator_framework-pagesize")
                    .Add("navigator-navigationPane_width")
                End With

                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)


                    ''basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam(Me.CurrentPrefix + "_framework-pagesize", "20"))
                    Dim strDefWidth As String = "420"
                    
                    Dim strW As String = .GetUserParam("navigator-navigationPane_width", strDefWidth)
                    If strW = "-1" Then
                        Me.navigationPane.Collapsed = True
                    Else
                        Me.navigationPane.Width = Unit.Parse(strW & "px")
                    End If

                End With
            End With

            RestartData()

        End If
    End Sub
    Private Function GetData(nParent As RadTreeNode) As List(Of DTS)
        Dim dtss As New List(Of DTS), intParentLevel As Integer = 0, prefixes As New List(Of String), intParentPID As Integer = 0, strParentPrefix As String = ""

        If nParent Is Nothing Then
            prefixes.Add(cbxLevel0.SelectedValue)
        Else
            intParentPID = CInt(nParent.Value)
            intParentLevel = CInt(nParent.Attributes.Item("level"))
            strParentPrefix = nParent.Attributes.Item("prefix")
            Select Case strParentPrefix
                Case "p41"
                    prefixes.Add("p41") 'podřízené projekty
                Case "p28"
                    prefixes.Add("p28") 'podřízení klienti
            End Select
            Select Case intParentLevel
                Case 0
                    If Me.cbxLevel1.SelectedValue <> "" Then prefixes.Add(Me.cbxLevel1.SelectedValue)
                Case 1
                    If Me.cbxLevel2.SelectedValue <> "" Then prefixes.Add(Me.cbxLevel2.SelectedValue)
            End Select
        End If
        For Each strPrefix In prefixes
            Select Case strPrefix
                Case "p28"
                    Dim mq As New BO.myQueryP28
                    mq.MG_GridSqlColumns = "p28Name"
                    If intParentPID <> 0 And strParentPrefix = "p28" Then mq.p28ParentID = intParentPID
                    Dim dt As DataTable = Master.Factory.p28ContactBL.GetGridDataSource(mq)
                    For Each dbRow In dt.Rows
                        Dim c As New DTS(dbRow.item("pid"), dbRow.item("p28Name"), "p28")
                        If Not nParent Is Nothing Then
                            c.ParentPID = intParentPID
                            c.Level = intParentLevel + 1
                        End If
                        dtss.Add(c)
                    Next
                Case "p41"
                    Dim mq As New BO.myQueryP41
                    mq.MG_GridSqlColumns = "p41Name"
                    If intParentPID <> 0 Then
                        If strParentPrefix = "p41" Then mq.p41ParentID = intParentPID
                        If strParentPrefix = "p28" Then mq.p28ID = intParentPID
                    End If
                    Dim dt As DataTable = Master.Factory.p41ProjectBL.GetGridDataSource(mq)
                    For Each dbRow In dt.Rows
                        Dim c As New DTS(dbRow.item("pid"), dbRow.item("p41Name"), "p41")
                        If Not nParent Is Nothing Then
                            c.ParentPID = intParentPID
                            c.Level = intParentLevel + 1
                        End If
                        dtss.Add(c)
                    Next
                Case "j02"
                    Dim mq As New BO.myQueryJ02
                    mq.MG_GridSqlColumns = "j02LastName"
                    If intParentPID <> 0 Then
                        If strParentPrefix = "p41" Then mq.p41ID = intParentPID
                        If strParentPrefix = "p28" Then mq.p28ID = intParentPID
                    End If
                    Dim dt As DataTable = Master.Factory.j02PersonBL.GetGridDataSource(mq)
                    For Each dbRow In dt.Rows
                        Dim c As New DTS(dbRow.item("pid"), dbRow.item("j02LastName"), "j02")
                        If Not nParent Is Nothing Then
                            c.ParentPID = intParentPID
                            c.Level = intParentLevel + 1
                        End If
                        dtss.Add(c)
                    Next
                Case "j18"
                    Dim lis As IEnumerable(Of BO.j18Region) = Master.Factory.j18RegionBL.GetList(New BO.myQuery)
                    For Each dbRow In lis
                        Dim c As New DTS(dbRow.PID, dbRow.j18Name, "j18")
                        If Not nParent Is Nothing Then
                            c.ParentPID = intParentPID
                            c.Level = intParentLevel + 1
                        End If
                        dtss.Add(c)
                    Next
            End Select
        Next


        Return dtss
    End Function
    Private Sub RefreshTreeData(nParent As RadTreeNode)
        Dim dtss As List(Of DTS) = GetData(nParent)
        If nParent Is Nothing Then
            tr1.Nodes.Clear()
        End If

        For Each c In dtss
            RenderTreeItem(c, nParent)
        Next

    End Sub
    Public Function RenderTreeItem(cRec As DTS, nParent As Telerik.Web.UI.RadTreeNode) As RadTreeNode
        Dim n As New Telerik.Web.UI.RadTreeNode(cRec.ItemText, cRec.PID.ToString)
        If Len(cRec.ItemText) > 30 Then
            n.Text = Left(n.Text, 30) & "..."
            n.ToolTip = cRec.ItemText
        End If

        n.Attributes.Add("prefix", cRec.Prefix)
        n.Attributes.Add("level", cRec.Level.ToString)
        Select Case cRec.Prefix
            Case "p28" : n.ImageUrl = "Images/contact.png"
            Case "p41" : n.ImageUrl = "Images/project.png"
            Case "j02" : n.ImageUrl = "Images/person.png"
        End Select
        If Not cRec.IsFinalLevel Then
            n.ExpandMode = TreeNodeExpandMode.ServerSideCallBack
        End If

        If nParent Is Nothing Then
            tr1.Nodes.Add(n)
        Else
            nParent.Nodes.Add(n)
        End If

        Return n

    End Function

  

    Private Sub tr1_NodeExpand(sender As Object, e As RadTreeNodeEventArgs) Handles tr1.NodeExpand
        RefreshTreeData(e.Node)
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        RefreshTreeData(Nothing)
    End Sub

    Private Sub cbxLevel0_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxLevel0.SelectedIndexChanged
        RestartData()
        hidSettingIsActive.Value = "1"
    End Sub

    Private Sub RestartData()
        RefreshTreeData(Nothing)
    End Sub
End Class