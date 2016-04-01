Imports Telerik.Web.UI

Public Class x40_framework
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private Property _needFilterIsChanged As Boolean = False
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
    Private Sub x40_framework_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "x40_framework"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            With Master
                .PageTitle = "Odeslané poštovní zprávy"
                .SiteMenuValue = "x40"

                Dim lisPars As New List(Of String)
                With lisPars
                    .Add("x40_framework-pagesize")

                    .Add("x40_framework-filter_setting")
                    .Add("x40_framework-filter_sql")
                End With
                .Factory.j03UserBL.InhaleUserParams(lisPars)



            End With
            With Master.Factory.j03UserBL
                cbxPaging.SelectedValue = .GetUserParam("x40_framework-pagesize", "20")

            End With


            With Master.Factory.j03UserBL
                SetupGrid(.GetUserParam("x40_framework-filter_setting"), .GetUserParam("x40_framework-filter_sql"))
            End With
            If Me.CurrentMasterPrefix <> "" Then
                With Me.lblFormHeader
                    .CssClass = ""
                    .Text = Master.Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix), Me.CurrentMasterPID)
                    If .Text.Length > 30 Then .Text = Left(.Text, 28) & "..."
                    .Text = "<a href='" & Me.CurrentMasterPrefix & "_framework.aspx?pid=" & Me.CurrentMasterPID.ToString & "'>" & .Text & "</a>"
                End With
            End If
        End If
    End Sub

    Private Sub SetupGrid(strFilterSetting As String, strFilterExpression As String)
        With grid1
            .ClearColumns()
            .PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
            .radGridOrig.ShowFooter = False

            .AddSystemColumn(20, "UserInsert")
            .AddColumn("DateUpdate", "Čas", BO.cfENUM.DateTime, , , , , , False)
            .AddColumn("x40State", "Stav", , , , , , , False)
            .AddColumn("x40SenderName", "Odesílatel")
            '.AddColumn("x40SenderAddress", "Adresa")
            .AddColumn("x40Recipient", "Příjemce")
            .AddColumn("x40Subject", "Předmět zprávy")
            .AddColumn("x40WhenProceeded", "Zpracováno", BO.cfENUM.DateTime, , , , , , False)
            .AddColumn("x40ErrorMessage", "Chyba")

            .SetFilterSetting(strFilterSetting, strFilterExpression)
        End With
    End Sub

    Private Sub grid1_FilterCommand(strFilterFunction As String, strFilterColumn As String, strFilterPattern As String) Handles grid1.FilterCommand
        _needFilterIsChanged = True
    End Sub

    Private Sub grid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grid1.ItemDataBound
        basUIMT.x40_grid_Handle_ItemDataBound(sender, e)
    End Sub

   

    Private Sub grid1_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grid1.NeedDataSource
        If _needFilterIsChanged Then
            With Master.Factory.j03UserBL
                .SetUserParam("x40_framework-filter_setting", grid1.GetFilterSetting())
                .SetUserParam("x40_framework-filter_sql", grid1.GetFilterExpression())
            End With
        End If
        Dim mq As New BO.myQueryX40
        mq.ColumnFilteringExpression = grid1.GetFilterExpression
        If Me.CurrentMasterPrefix <> "" Then
            mq.x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
            mq.RecordPID = Me.CurrentMasterPID
        Else
            If Not Master.Factory.SysUser.IsAdmin Then
                mq.j03ID_MyRecords = Master.Factory.SysUser.PID
            End If
        End If
        

        Dim lis As IEnumerable(Of BO.x40MailQueue) = Master.Factory.x40MailQueueBL.GetList(mq)
      
        grid1.DataSource = lis
    End Sub

    Private Sub cbxPaging_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxPaging.SelectedIndexChanged
        Master.Factory.j03UserBL.SetUserParam("x40_framework-pagesize", cbxPaging.SelectedValue)

        grid1.PageSize = BO.BAS.IsNullInt(cbxPaging.SelectedItem.Text)
        If grid1.radGridOrig.CurrentPageIndex > 0 Then grid1.radGridOrig.CurrentPageIndex = 0
        grid1.Rebind(True)
    End Sub
End Class