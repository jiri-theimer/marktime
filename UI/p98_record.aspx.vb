Public Class p98_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p97_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ViewState("guid") = BO.BAS.GetGUID

            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Zaokrouhlování vystavených faktur"


                

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

        Dim cRec As BO.p98Invoice_Round_Setting_Template = Master.Factory.p98Invoice_Round_Setting_TemplateBL.Load(Master.DataPID)
        With cRec
            Me.p98Name.Text = .p98Name
            Me.p98IsDefault.Checked = .p98IsDefault


            'Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
            Master.Timestamp = .Timestamp
        End With

        Dim lis As IEnumerable(Of BO.p97Invoice_Round_Setting) = Master.Factory.p98Invoice_Round_Setting_TemplateBL.GetList_P97(cRec.PID)
        For Each c In lis
            Dim cTemp As New BO.p85TempBox
            cTemp.p85GUID = ViewState("guid")
            cTemp.p85DataPID = cRec.PID
            cTemp.p85OtherKey1 = c.j27ID
            cTemp.p85OtherKey2 = c.p97AmountFlag
            cTemp.p85OtherKey3 = c.p97Scale
            Master.Factory.p85TempBoxBL.Save(cTemp)
        Next

        rp1.DataSource = Master.Factory.p85TempBoxBL.GetList(ViewState("guid"))
        rp1.DataBind()
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p98Invoice_Round_Setting_TemplateBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p98-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.p98Invoice_Round_Setting_TemplateBL
            Dim cRec As BO.p98Invoice_Round_Setting_Template = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p98Invoice_Round_Setting_Template)
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

    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.p85TempBox = CType(e.Item.DataItem, BO.p85TempBox)

        With CType(e.Item.FindControl("j27ID"), DropDownList)
            .DataSource = Master.Factory.ftBL.GetList_J27()
            .DataBind()
        End With
        basUI.SelectDropdownlistValue(CType(e.Item.FindControl("j27ID"), DropDownList), cRec.p85OtherKey1.ToString)
        basUI.SelectDropdownlistValue(CType(e.Item.FindControl("p97AmountFlag"), DropDownList), cRec.p85OtherKey2.ToString)
        basUI.SelectDropdownlistValue(CType(e.Item.FindControl("p97Scale"), DropDownList), cRec.p85OtherKey3.ToString)
    End Sub
End Class