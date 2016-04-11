Public Class x18_binding
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Public Property CurrentPrefix As String
        Get
            Return Me.hidPrefix.Value
        End Get
        Set(value As String)
            Me.hidPrefix.Value = value
        End Set
    End Property
    Public ReadOnly Property CurrentX29ID As BO.x29IdEnum
        Get
            Return BO.BAS.GetX29FromPrefix(Me.CurrentPrefix)
        End Get
    End Property

    Private Sub x18_binding_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                Me.CurrentPrefix = Request.Item("prefix")
                If .DataPID = 0 Or Me.CurrentPrefix = "" Then .StopPage("pid or prefix missing")
                .HeaderIcon = "Images/label_32.png"

                .AddToolbarButton("Uložit změny", "save", , "Images/save.png")

               
                .HeaderText = "Štíky | " & .Factory.GetRecordCaption(Me.CurrentX29ID, .DataPID)

                Dim lisX18 As IEnumerable(Of BO.x18EntityCategory) = .Factory.x18EntityCategoryBL.GetList(, Me.CurrentX29ID)
                If lisX18.Count = 0 Then
                    .StopPage("Pro tuto entitu nejsou v systému zavedeny štítky.")
                End If

                rp1.DataSource = lisX18
                rp1.DataBind()
            End With

            RefreshRecord()
        End If
    End Sub

    Private Sub RefreshRecord()
        Dim lisX19 As IEnumerable(Of BO.x19EntityCategory_Binding) = Master.Factory.x18EntityCategoryBL.GetList_X19(Me.CurrentX29ID, Master.DataPID)
        For Each ri As RepeaterItem In rp1.Items
            Dim intX18ID As Integer = CInt(CType(ri.FindControl("x18ID"), HiddenField).Value)
            Dim lis As List(Of String) = lisX19.Where(Function(p) p.x18ID = intX18ID).Select(Function(p) p.x25ID.ToString).ToList
            If lis.Count > 0 Then
                With CType(ri.FindControl("x25IDs"), UI.datacombo)
                    If .AllowCheckboxes Then
                        .SelectCheckboxItems(lis)
                    Else
                        If lis.Count > 0 Then .SelectedValue = lis(0)
                    End If

                End With
            End If
        Next
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.x18EntityCategory = CType(e.Item.DataItem, BO.x18EntityCategory)
        With CType(e.Item.FindControl("x18Name"), Label)
            .Text = cRec.x18Name & ":"
        End With
        Dim lisX25 As IEnumerable(Of BO.x25EntityField_ComboValue) = Master.Factory.x25EntityField_ComboValueBL.GetList(cRec.x23ID).Where(Function(p) p.IsClosed = False)
        With CType(e.Item.FindControl("x25IDs"), UI.datacombo)
            If Not cRec.x18IsMultiSelect Then
                .AllowCheckboxes = False
                .IsFirstEmptyRow = True
            End If
            .DataSource = lisX25
            .DataBind()
        End With
        CType(e.Item.FindControl("x18ID"), HiddenField).Value = cRec.PID.ToString
        CType(e.Item.FindControl("x18IsMultiSelect"), HiddenField).Value = BO.BAS.GB(cRec.x18IsMultiSelect)
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "save" Then
            Dim lis As New List(Of BO.x19EntityCategory_Binding)
            For Each ri As RepeaterItem In rp1.Items
                Dim intX18ID As Integer = CInt(CType(ri.FindControl("x18ID"), HiddenField).Value)
                Dim x25IDs As New List(Of Integer)
                With CType(ri.FindControl("x25IDs"), UI.datacombo)
                    If .AllowCheckboxes Then
                        x25IDs = .GetAllCheckedIntegerValues
                    Else
                        If .SelectedValue <> "" Then x25IDs.Add(CInt(.SelectedValue))
                    End If
                End With
                For Each intX25ID As Integer In x25IDs
                    Dim c As New BO.x19EntityCategory_Binding
                    c.x18ID = intX18ID
                    c.x25ID = intX25ID
                    c.x19RecordPID = Master.DataPID
                    lis.Add(c)
                Next
            Next
            With Master.Factory.x18EntityCategoryBL
                If .SaveX19Binding(Me.CurrentX29ID, Master.DataPID, lis) Then
                    Master.CloseAndRefreshParent()
                End If
            End With
        End If
    End Sub
End Class