Public Class x25_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Public Property _curIsExport As Boolean
    Private Property _needFilterIsChanged As Boolean = False


    Public ReadOnly Property CurrentX18ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.x18ID.SelectedValue)
        End Get
    End Property
    Public ReadOnly Property CurrentX23ID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidX23ID.Value)
        End Get
    End Property
    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            hidMasterPID.Value = value.ToString
        End Set
    End Property

    Private Sub x25_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Item("masterpid") <> "" Then
                Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid")) : Me.CurrentMasterPrefix = Request.Item("masterprefix")
            End If
            Handle_Permissions()
            SetupPeriodQuery()
            With Master
                .SiteMenuValue = "x25_framework"
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("x25_framework-pagesize")
                    .Add("x25_framework-navigationPane_width")
                    .Add("x25_framework-x18id")
                    .Add("x25_framework-x23id")
                    .Add("x25_framework_detail-pid")

                    .Add("x25_framework-sort")
                    .Add("periodcombo-custom_query")
                    .Add("x25_framework-periodtype")
                    .Add("x25_framework-period")
                    .Add("x25_framework-filter_setting")
                    .Add("x25_framework-filter_sql")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)


                    basUI.SelectDropdownlistValue(cbxPaging, .GetUserParam("x25_framework-pagesize", "20"))
                    Dim strDefWidth As String = "800"
                    Dim strW As String = .GetUserParam("x25_framework-navigationPane_width", strDefWidth)
                    If strW = "-1" Then
                        Me.navigationPane.Collapsed = True
                    Else
                        Me.navigationPane.Width = Unit.Parse(strW & "px")
                    End If

                    If .GetUserParam("x25_framework-sort") <> "" Then
                        grid1.radGridOrig.MasterTableView.SortExpressions.AddSortExpression(.GetUserParam("x25_framework-sort"))
                    End If
                    basUI.SelectDropdownlistValue(Me.cbxPeriodType, .GetUserParam("x25_framework-periodtype", ""))
                    If Me.cbxPeriodType.SelectedIndex > 0 Then
                        period1.SetupData(Master.Factory, .GetUserParam("periodcombo-custom_query"))
                        period1.SelectedValue = .GetUserParam("x25_framework-period")
                    End If
                End With
            End With



            With Master.Factory.j03UserBL
                SetupX18Combo(BO.BAS.IsNullInt(.GetUserParam("x25_framework-x18id")))
                Dim intJ74ID As Integer = BO.BAS.IsNullInt(.GetUserParam(Me.CurrentPrefix + "_framework-j74id"))
                If intJ74ID = 0 Then
                    If Master.Factory.j74SavedGridColTemplateBL.CheckDefaultTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID) Then
                        _curJ74 = Master.Factory.j74SavedGridColTemplateBL.LoadSystemTemplate(Me.CurrentX29ID, Master.Factory.SysUser.PID)
                        .SetUserParam(Me.CurrentPrefix + "_framework-j74id", _curJ74.PID)
                    End If
                End If
                SetupJ74Combo(intJ74ID)
                SetupGrid(.GetUserParam(Me.CurrentPrefix + "_framework-filter_setting"), .GetUserParam(Me.CurrentPrefix + "_framework-filter_sql"))
            End With
            RecalcVirtualRowCount()

            If Me.CurrentMasterPID = 0 Then
                Handle_DefaultSelectedRecord()
            Else
                Me.hidContentPaneDefUrl.Value = "entity_framework_detail_missing.aspx?prefix=" & Me.CurrentPrefix & "&masterpid=" & Me.CurrentMasterPID.ToString & "&masterprefix=" & Me.CurrentMasterPrefix & "&source=" & opgLayout.SelectedValue
            End If


        End If
    End Sub

    Private Sub SetupX18Combo(strDef As String)

    End Sub

    Private Sub SetupPeriodQuery()
        With Me.cbxPeriodType.Items
            If .Count > 0 Then .Clear()
            .Add(New ListItem("--Filtrovat období--", ""))
            .Add(New ListItem("Založení záznamu", "DateInsert"))
            
        End With

    End Sub
    Private Sub Handle_Permissions()
        
    End Sub
End Class