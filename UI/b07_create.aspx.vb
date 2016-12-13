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
        Me.upload1.Factory = Master.Factory
        Me.uploadlist1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID
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


            history1.RefreshData(Master.Factory, BO.BAS.GetX29FromPrefix(Me.CurrentPrefix), Me.CurrentRecordPID, Me.CurrentParentID)
        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        upload1.TryUploadhWaitingFilesOnClientSide()
        If strButtonValue = "save" Then
            Dim cRec As New BO.b07Comment
            With cRec
                .x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentPrefix)
                .b07RecordPID = Me.CurrentRecordPID
                .b07Value = Me.b07Value.Text
                .b07ID_Parent = Me.CurrentParentID
            End With

            With Master.Factory.b07CommentBL
                If .Save(cRec, upload1.GUID, Nothing) Then
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
        SaveTemp()
        AddReceiver(0)
    End Sub
    Private Sub AddReceiver(intJ02ID As Integer)
        Dim c As New BO.p85TempBox
        c.p85GUID = ViewState("guid")
        If intJ02ID <> 0 Then
            c.p85OtherKey1 = intJ02ID
            c.p85FreeText01 = Master.Factory.j02PersonBL.Load(intJ02ID).FullNameDesc
        End If
        Master.Factory.p85TempBoxBL.Save(c)
        RefreshTempList()
    End Sub
    Private Sub RefreshTempList()
        rp1.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        rp1.DataBind()
    End Sub

    Private Sub SaveTemp()
        Dim lisTEMP As IEnumerable(Of BO.p85TempBox) = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        For Each ri As RepeaterItem In rp1.Items
            Dim intP85ID As Integer = BO.BAS.IsNullInt(CType(ri.FindControl("p85id"), HiddenField).Value)

            Dim cRec As BO.p85TempBox = lisTEMP.Where(Function(p) p.PID = intP85ID)(0)
            With cRec
                .p85OtherKey1 = BO.BAS.IsNullInt(CType(ri.FindControl("j02id"), UI.person).Value)
                .p85FreeText01 = CType(ri.FindControl("j02id"), UI.person).Text
                .p85OtherKey2 = BO.BAS.IsNullInt(CType(ri.FindControl("j11id"), DropDownList).SelectedValue)

            End With
            Master.Factory.p85TempBoxBL.Save(cRec)

        Next
    End Sub

    Private Sub rp1_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rp1.ItemCommand
        SaveTemp()
        Dim cRec As BO.p85TempBox = Master.Factory.p85TempBoxBL.Load(BO.BAS.IsNullInt(CType(e.Item.FindControl("p85id"), HiddenField).Value))
        If e.CommandName = "delete" Then
            If Master.Factory.p85TempBoxBL.Delete(cRec) Then
                RefreshTempList()
            End If
        End If
    End Sub

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)
        CType(e.Item.FindControl("p85id"), HiddenField).Value = cRec.PID.ToString
        With CType(e.Item.FindControl("del"), ImageButton)
            .CommandName = "delete"
        End With
        With CType(e.Item.FindControl("j11id"), DropDownList)
            .DataSource = Master.Factory.j11TeamBL.GetList(New BO.myQuery).Where(Function(p) p.j11IsAllPersons = False)
            .DataBind()
            .Items.Insert(0, "")
        End With
        basUI.SelectDropdownlistValue(CType(e.Item.FindControl("j11id"), DropDownList), cRec.p85OtherKey2.ToString)
        With CType(e.Item.FindControl("j02id"), UI.person)
            .Value = cRec.p85OtherKey1.ToString
            .Text = cRec.p85FreeText01
        End With
        
    End Sub
End Class