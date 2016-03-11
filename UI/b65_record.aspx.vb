Public Class b65_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub b65_record_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/messages_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Šablona notifikační zprávy"
            End With
            If Request.Item("b01id") <> "" Then
                Dim cB01 As BO.b01WorkflowTemplate = Master.Factory.b01WorkflowTemplateBL.Load(CInt(Request.Item("b01id")))
                Me.x29ID.SelectedValue = CInt(cB01.x29ID).ToString
            End If

            RefreshRecord()

            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
        RefreshState()
    End Sub

    Private Sub RefreshState()
       
    End Sub
    Private Sub RefreshRecord()
       
        If Master.DataPID = 0 Then Return

        Dim cRec As BO.b65WorkflowMessage = Master.Factory.b65WorkflowMessageBL.Load(Master.DataPID)
        With cRec
            basUI.SelectDropdownlistValue(Me.x29ID, CInt(.x29ID).ToString)
            b65name.Text = .b65Name
            b65MessageSubject.Text = .b65MessageSubject
            b65MessageBody.Text = .b65MessageBody

            Master.Timestamp = .Timestamp
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.b65WorkflowMessageBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("b65-delete")
            Else
                Master.Notify(.ErrorMessage, NotifyLevel.ErrorMessage)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()

    End Sub


    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.b65WorkflowMessageBL
            Dim cRec As BO.b65WorkflowMessage = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.b65WorkflowMessage)
            With cRec
                .x29ID = BO.BAS.IsNullInt(Me.x29ID.SelectedValue)
                .b65Name = b65name.Text
                .b65MessageSubject = b65MessageSubject.Text
                .b65MessageBody = b65MessageBody.Text
                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
            End With

            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("b65-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With


    End Sub

End Class