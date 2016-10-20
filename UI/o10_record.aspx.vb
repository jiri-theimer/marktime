Public Class o10_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub o10_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        Master.HelpTopicID = "o10_framework"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        roles1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            With Master
                .HeaderIcon = "Images/article_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                If .DataPID = 0 Or .IsRecordClone Then
                    'oprávnění pro zapisování článků
                    .neededPermission = BO.x53PermValEnum.GR_O10_Creator
                End If
            End With

            RefreshRecord()
            If Master.IsRecordClone Then
                Master.DataPID = 0
            End If
        End If
    End Sub

    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            Me.j02ID_Owner.Value = Master.Factory.SysUser.j02ID.ToString
            Me.j02ID_Owner.Text = Master.Factory.SysUser.PersonDesc
            Return
       
        End If
        Dim cRec As BO.o10NoticeBoard = Master.Factory.o10NoticeBoardBL.Load(Master.DataPID)
        With cRec
            Master.HeaderText = "Článek | " & .o10Name
            BodyHtml.Content = .o10BodyHtml
            Me.o10Name.Text = .o10Name
            Me.o10Ordinary.Value = .o10Ordinary
            basUI.SetColorToPicker(Me.o10BackColor, .o10BackColor)
            basUI.SelectDropdownlistValue(Me.o10Locality, BO.BAS.IsNullInt(.o10Locality))
            Me.j02ID_Owner.Value = .j02ID_Owner.ToString
            Me.j02ID_Owner.Text = Master.Factory.GetRecordCaption(BO.x29IdEnum.j02Person, .j02ID_Owner, True)
            Master.Timestamp = .Timestamp
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

        roles1.InhaleInitialData(cRec.PID)
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.o10NoticeBoardBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("o10-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.o10NoticeBoardBL
            Dim cRec As BO.o10NoticeBoard = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.o10NoticeBoard)
            cRec.o10Name = Me.o10Name.Text
            cRec.o10Ordinary = BO.BAS.IsNullInt(Me.o10Ordinary.Value)
            cRec.o10BodyHtml = BodyHtml.Content
            cRec.o10BodyPlainText = BodyHtml.Text
            cRec.o10BackColor = basUI.GetColorFromPicker(Me.o10BackColor)
            cRec.o10Locality = BO.BAS.IsNullInt(Me.o10Locality.SelectedValue)
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil

            Dim lisX69 As List(Of BO.x69EntityRole_Assign) = roles1.GetData4Save()
            If roles1.ErrorMessage <> "" Then
                Master.Notify(roles1.ErrorMessage, 2)
                Return
            End If

            If .Save(cRec, lisX69) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("o10-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub cmdAddX69_Click(sender As Object, e As EventArgs) Handles cmdAddX69.Click
        roles1.AddNewRow()
    End Sub
End Class