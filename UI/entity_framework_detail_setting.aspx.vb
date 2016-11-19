Public Class entity_framework_detail_setting
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm
    Public Property CurrentPrefix As String
        Get
            Return hidPrefix.Value
        End Get
        Set(value As String)
            hidPrefix.Value = value
        End Set
    End Property

    Private Sub entity_framework_detail_setting_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentPrefix = Request.Item("prefix")
            With Master
                .HeaderText = "Nastavení vzhledu stránky"
                .AddToolbarButton("Uložit změny", "ok")
            End With

            Dim lisPars As New List(Of String)
            With lisPars
                .Add(Me.CurrentPrefix + "_framework_detail-switchHeight")
                .Add(Me.CurrentPrefix + "_framework_detail-tabskin")
                .Add(Me.CurrentPrefix + "_framework_detail-searchbox")
            End With

            With Master.Factory.j03UserBL
                .InhaleUserParams(lisPars)
                basUI.SelectRadiolistValue(Me.switchHeight, .GetUserParam(Me.CurrentPrefix + "_framework_detail-switchHeight", "auto"))
                basUI.SelectDropdownlistValue(Me.skin1, .GetUserParam(Me.CurrentPrefix + "_framework_detail-tabskin", "Default"))
                Me.chkSearchBox.Checked = BO.BAS.BG(.GetUserParam(Me.CurrentPrefix + "_framework_detail-searchbox", "0"))
            End With
            With Master.Factory
                colsSource.DataSource = .ftBL.GetList_X61(BO.BAS.GetX29FromPrefix(Me.CurrentPrefix))
                colsSource.DataBind()

                Dim lis As IEnumerable(Of BO.x61PageTab) = .j03UserBL.GetList_PageTabs(.SysUser.PID, BO.BAS.GetX29FromPrefix(Me.CurrentPrefix))
                For Each c In lis
                    Dim it As Telerik.Web.UI.RadListBoxItem = colsSource.FindItem(Function(p) p.Value = c.x61ID.ToString)
                    colsSource.Transfer(it, colsSource, colsDest)
                    colsSource.ClearSelection()
                    colsDest.ClearSelection()
                Next
            End With
            colsSource.ClearSelection()

            Select Case Me.CurrentPrefix
                Case "p28" : chkSearchBox.Text = "Na stránce zapnutý vyhledávač klienta"
                Case "j02" : chkSearchBox.Text = "Na stránce zapnutý vyhledávač osoby"
                Case "p91"
                    Me.chkSearchBox.Text = "Na stránce zapnutý vyhledávač faktury"
                    panTabs.Visible = False : panSwitchHeight.Visible = False
                Case "p41"
                    panTabs.Visible = False : chkSearchBox.Visible = False
                Case Else
                    chkSearchBox.Visible = False
            End Select
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            If panTabs.Visible Then
                Dim x61ids As New List(Of Integer)
                For Each it As Telerik.Web.UI.RadListBoxItem In colsDest.Items
                    x61ids.Add(CInt(it.Value))
                Next
                With Master.Factory.j03UserBL
                    If Not .SavePageTabs(Master.Factory.SysUser.PID, BO.BAS.GetX29FromPrefix(Me.CurrentPrefix), x61ids) Then
                        Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
                        Return
                    End If
                End With
            End If
            With Master.Factory.j03UserBL
                If panSwitchHeight.Visible Then .SetUserParam(Me.CurrentPrefix + "_framework_detail-switchHeight", Me.switchHeight.SelectedValue)
                .SetUserParam(Me.CurrentPrefix + "_framework_detail-tabskin", Me.skin1.SelectedValue)
                .SetUserParam(Me.CurrentPrefix + "_framework_detail-searchbox", BO.BAS.GB(Me.chkSearchBox.Checked))
            End With
            Master.CloseAndRefreshParent("setting")
        End If

    End Sub
End Class