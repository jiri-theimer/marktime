Imports Telerik.Web.UI

Public Class x18_querybuilder
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub x18_querybuilder_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Private ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(hidPrefix.Value)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Item("prefix") = "" Then
                Master.StopPage("prefix is missing.")
            End If
            hidPrefix.Value = Request.Item("prefix")

            With Master
                .HeaderIcon = "Images/label_32.png"
                .HeaderText = "Filtrování podle štítků"
                .AddToolbarButton(Resources.grid_designer.Vybrat, "ok", , "Images/ok.png")
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))

            End With
            Dim pids As List(Of Integer) = BO.BAS.ConvertPIDs2List(Request.Item("pids"))

            SetupTree


        End If
    End Sub

    Private Sub SetupTree()
        Dim nParent As RadTreeNode = WN0("Štítky", Me.hidPrefix.Value, Nothing)
        nParent.ImageUrl = "Images/folder.png"
        nParent.Checkable = False
        tr1.Nodes.Add(nParent)
        Dim lisX18 As IEnumerable(Of BO.x18EntityCategory) = Master.Factory.x18EntityCategoryBL.GetList(, Me.CurrentX29ID)
        For Each c In lisX18
            WN(c, hidPrefix.Value & "-" & c.PID.ToString, nParent)
        Next

        Select Case Me.CurrentX29ID
            Case BO.x29IdEnum.p41Project
                lisX18 = Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p28Contact, -1)
                If lisX18.Count > 0 Then
                    nParent = WN0("Štítky klienta projektu", "p28", nParent)
                    For Each c In lisX18
                        WN(c, "a.p28ID_Client-" & c.PID.ToString, nParent)
                    Next
                End If
                
            Case BO.x29IdEnum.p31Worksheet
                lisX18 = Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p41Project, -1)
                If lisX18.Count > 0 Then
                    nParent = WN0("Štítky projektu", "p41", nParent)
                    
                    For Each c In lisX18
                        WN(c, "a.p28ID_Client-" & c.PID.ToString, nParent)
                    Next
                End If
                lisX18 = Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p28Contact, -1)
                If lisX18.Count > 0 Then
                    nParent = WN0("Štítky klienta projektu", "p28", nParent)
                    
                    For Each c In lisX18
                        WN(c, "p41.p28ID_Client-" & c.PID.ToString, nParent)
                    Next
                End If
            Case BO.x29IdEnum.p91Invoice
                lisX18 = Master.Factory.x18EntityCategoryBL.GetList(, BO.x29IdEnum.p28Contact, -1)
                If lisX18.Count > 0 Then
                    nParent = WN0("Štítky klienta faktury", "p28", nParent)
                    For Each c In lisX18
                        WN(c, "p41.p28ID_Client-" & c.PID.ToString, nParent)
                    Next
                End If
        End Select
        tr1.ExpandAllNodes()
    End Sub
    Private Function WN0(strName As String, strValue As String, nParent As RadTreeNode) As RadTreeNode
        Dim n As RadTreeNode = New RadTreeNode(strName, strValue)
        n.Checkable = False
        n.ImageUrl = "Images/folder.png"
        If nParent Is Nothing Then
            tr1.Nodes.Add(n)
        Else
            nParent.Nodes.Add(n)
        End If

        Return n
    End Function
    Private Sub WN(cX18 As BO.x18EntityCategory, strValue As String, nParent As RadTreeNode)
        Dim n0 As New RadTreeNode(cX18.x18Name, strValue)
        n0.ImageUrl = "Images/label.png"
        n0.Checkable = False
        nParent.Nodes.Add(n0)

        Dim lis As IEnumerable(Of BO.x25EntityField_ComboValue) = Master.Factory.x25EntityField_ComboValueBL.GetList(cX18.x23ID)
        For Each c In lis
            Dim n As New RadTreeNode(c.x25Name, strValue & "-" & c.PID.ToString)
            n.Checkable = True
            If c.IsClosed Then n.Font.Strikeout = True
            n0.Nodes.Add(n)
        Next

    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            Dim s As String = String.Join(",", GetCheckedNodes)
            With Master.Factory.j03UserBL
                .SetUserParam("x18_querybuilder-" & hidPrefix.Value, s)
            End With
            Master.CloseAndRefreshParent("x18_querybuilder")
        End If
    End Sub

    Private Function GetCheckedNodes() As List(Of String)
        Dim lis As New List(Of String)
        For Each c In Me.tr1.CheckedNodes
            lis.Add(c.Value)
        Next
        Return lis
    End Function
End Class