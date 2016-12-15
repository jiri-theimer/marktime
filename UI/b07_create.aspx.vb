Public Class b07_create
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub b07_create_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "b07_record"
    End Sub
    Public Property CurrentPrefix As String
        Get
            Return hidPrefix.Value
        End Get
        Set(value As String)
            hidPrefix.Value = value
        End Set
    End Property
    Public Property CurrentRecordPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidRecordPID.Value)
        End Get
        Set(value As Integer)
            Me.hidRecordPID.Value = value.ToString
        End Set
    End Property
    Public Property CurrentParentID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidParentID.Value)
        End Get
        Set(value As Integer)
            Me.hidParentID.Value = value.ToString
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        receiver1.Factory = Master.Factory
        Me.upload1.Factory = Master.Factory
        Me.uploadlist1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            Me.CurrentPrefix = Request.Item("masterprefix")
            Me.CurrentRecordPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            If Me.CurrentRecordPID = 0 Or Me.CurrentPrefix = "" Then
                Master.StopPage("masterpid or masterprefix is missing")
            End If
            Me.CurrentParentID = BO.BAS.IsNullInt(Request.Item("parentpid"))
            Me.upload1.GUID = BO.BAS.GetGUID
            Me.uploadlist1.GUID = Me.upload1.GUID

            With Master
                .HeaderText = "Zapsat komentář | " & .Factory.GetRecordCaption(BO.BAS.GetX29FromPrefix(Me.CurrentPrefix), Me.CurrentRecordPID)
                .HeaderIcon = "Images/comment_32.png"
                .AddToolbarButton("Uložit komentář", "save", , "Images/save.png")
            End With
            If Me.CurrentParentID <> 0 Then
                Dim c As BO.b07Comment = Master.Factory.b07CommentBL.Load(Me.CurrentParentID)
                receiver1.AddReceiver(c.j02ID_Owner, 0)
            End If


            history1.RefreshData(Master.Factory, BO.BAS.GetX29FromPrefix(Me.CurrentPrefix), Me.CurrentRecordPID, Me.CurrentParentID)
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        upload1.TryUploadhWaitingFilesOnClientSide()
        receiver1.SaveTemp()

        If strButtonValue = "save" Then
            Dim cRec As New BO.b07Comment
            With cRec
                .x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentPrefix)
                .b07RecordPID = Me.CurrentRecordPID
                .b07Value = Me.b07Value.Text
                .b07ID_Parent = Me.CurrentParentID
                .b07WorkflowInfo = receiver1.GetInlineContent()
            End With

            With Master.Factory.b07CommentBL
                If .Save(cRec, upload1.GUID, receiver1.GetList()) Then
                    Master.CloseAndRefreshParent("b07-save")
                Else
                    Master.Notify(Master.Factory.b07CommentBL.ErrorMessage, NotifyLevel.ErrorMessage)
                End If
            End With
        End If
    End Sub

    Private Sub upload1_AfterUploadAll() Handles upload1.AfterUploadAll
        Me.uploadlist1.RefreshData_TEMP()
    End Sub

    Private Sub cmdAddReceiver_Click(sender As Object, e As EventArgs) Handles cmdAddReceiver.Click
        receiver1.AddReceiver(0, 0)
    End Sub

End Class