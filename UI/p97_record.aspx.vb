Public Class p97_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p97_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Zaokrouhlování vystavených faktur"


                Me.j27ID.DataSource = .Factory.ftBL.GetList_J27()
                Me.j27ID.DataBind()
              
            End With

            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0
            End If


        End If
    End Sub
    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            Return
        End If

        Dim cRec As BO.p97Invoice_Round_Setting = Master.Factory.p97Invoice_Round_SettingBL.Load(Master.DataPID)
        With cRec
            
            Me.j27ID.SelectedValue = .j27ID.ToString
            basUI.SelectDropdownlistValue(Me.p97Scale, .p97Scale.ToString)
            basUI.SelectDropdownlistValue(Me.p97AmountFlag, CInt(.p97AmountFlag).ToString)


            'Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
            Master.Timestamp = .Timestamp


        End With
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p97Invoice_Round_SettingBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p97-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p97Invoice_Round_SettingBL
            Dim cRec As BO.p97Invoice_Round_Setting = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p97Invoice_Round_Setting)
            With cRec
                .j27ID = BO.BAS.IsNullInt(Me.j27ID.SelectedValue)
                .p97Scale = CInt(Me.p97Scale.SelectedValue)
                .p97AmountFlag = CType(Me.p97AmountFlag.SelectedValue, BO.p97AmountFlagEnum)
                
                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil
            End With



            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("p97-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub
End Class