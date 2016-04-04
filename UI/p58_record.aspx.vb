Public Class p58_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord

    Private Sub p58_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderIcon = "Images/settings_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Nastavení produktu"

                Me.p58ParentID.DataSource = .Factory.p58ProductBL.GetList(New BO.myQuery).Where(Function(p) p.PID <> .DataPID)
                Me.p58ParentID.DataBind()
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

        Dim cRec As BO.p58Product = Master.Factory.p58ProductBL.Load(Master.DataPID)
        With cRec
           
            Me.p58ParentID.SelectedValue = .p58ParentID.ToString
            Me.p58Name.Text = .p58Name
            Me.p58Ordinary.Value = .p58Ordinary
            Master.Timestamp = .Timestamp
            Me.p58Code.Text = .p58Code
            Me.p58ExternalPID.Text = .p58ExternalPID
            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)
        End With

    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.p58ProductBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("p58-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave

        With Master.Factory.p58ProductBL
            Dim cRec As BO.p58Product = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.p58Product)
            With cRec
                .p58ParentID = BO.BAS.IsNullInt(Me.p58ParentID.SelectedValue)
                .p58Name = Me.p58Name.Text
                .p58Code = Trim(Me.p58Code.Text)
                .p58ExternalPID = Me.p58ExternalPID.Text

                .p58Ordinary = BO.BAS.IsNullInt(Me.p58Ordinary.Value)
                .ValidFrom = Master.RecordValidFrom
                .ValidUntil = Master.RecordValidUntil

            End With



            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID


                Master.CloseAndRefreshParent("p58-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub
End Class