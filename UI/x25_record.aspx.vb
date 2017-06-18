Imports Telerik.Web.UI

Public Class x25_record
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalDataRecord
    Private _curRec As BO.x25EntityField_ComboValue

    Public ReadOnly Property CurrentX18ID As Integer
        Get
            Return BO.BAS.IsNullInt(hidX18ID.Value)
        End Get
    End Property

    Private Sub x25_record_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .HeaderIcon = "Images/label_32.png"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .HeaderText = "Položka"
                Me.x23ID.DataSource = .Factory.x23EntityField_ComboBL.GetList(New BO.myQuery)
                Me.x23ID.DataBind()

                If Request.Item("x23id") <> "" Then
                    Me.x23ID.SelectedValue = Request.Item("x23id")
                End If
                If Request.Item("x18id") <> "" Then
                    hidX18ID.Value = Request.Item("x18id")
                    Dim c As BO.x18EntityCategory = .Factory.x18EntityCategoryBL.Load(Me.CurrentX18ID)
                    Me.x23ID.SelectedValue = c.x23ID.ToString

                    .HeaderText += " | " & c.x18Name

                    panColors.Visible = c.x18IsColors
                    If Not c.x18IsColors Then
                        x25BackColor.Preset = Telerik.Web.UI.ColorPreset.None
                        x25BackColor.Items.Clear()
                        x25ForeColor.Preset = Telerik.Web.UI.ColorPreset.None
                        x25ForeColor.Items.Clear()

                    End If
                End If
                If Me.x23ID.SelectedIndex > 0 Then
                    lblX23ID.Visible = False
                    Me.x23ID.Visible = False
                End If

                If Not (Request.Item("source") = "x18_items" Or Request.Item("source") = "x18_record") Then
                    .neededPermission = BO.x53PermValEnum.GR_Admin
                End If
            End With


            RefreshRecord()


            If Master.IsRecordClone Then
                Master.DataPID = 0

            End If
        End If
    End Sub
    Private Sub RefreshUserFields()
        Dim lisX16 As IEnumerable(Of BO.x16EntityCategory_FieldSetting) = Master.Factory.x18EntityCategoryBL.GetList_x16(Me.CurrentX18ID)
        If lisX16.Count > 0 Then
            If lisX16.Where(Function(p) LCase(p.x16Field).IndexOf("date") > 0).Count = 0 Then
                Me.SharedCalendar.Visible = False
            End If
            rpX16.DataSource = lisX16
            rpX16.DataBind()

        Else
            panX16.Visible = False
            Me.SharedCalendar.Visible = False
        End If
    End Sub
    Private Sub RefreshRecord()
        If Master.DataPID = 0 Then
            Me.j02ID_Owner.Value = Master.Factory.SysUser.j02ID.ToString
            Me.j02ID_Owner.Text = Master.Factory.SysUser.PersonDesc
            _curRec = New BO.x25EntityField_ComboValue
            RefreshUserFields()
            Return
        End If

        _curRec = Master.Factory.x25EntityField_ComboValueBL.Load(Master.DataPID)
        With _curRec
            Me.j02ID_Owner.Value = .j02ID_Owner.ToString
            Me.j02ID_Owner.Text = .Owner
            Me.x23ID.SelectedValue = .x23ID.ToString
            Me.x25Name.Text = .x25Name
            Me.x25Ordinary.Value = .x25Ordinary
            Me.x25Code.Text = .x25Code
            Master.Timestamp = .Timestamp
            If panColors.Visible Then
                basUI.SetColorToPicker(Me.x25BackColor, .x25BackColor)
                basUI.SetColorToPicker(Me.x25ForeColor, .x25ForeColor)
            End If

            Master.InhaleRecordValidity(.ValidFrom, .ValidUntil, .DateInsert)

        End With
        Dim cX23 As BO.x23EntityField_Combo = Master.Factory.x23EntityField_ComboBL.Load(_curRec.x23ID)
        If cX23.x23DataSource <> "" Then
            Master.Notify("Tato položka byla vložena automaticky, protože pochází z externího datového zdroje.", NotifyLevel.InfoMessage)

        End If
        RefreshUserFields()
    End Sub
    Private Sub _MasterPage_Master_OnDelete() Handles _MasterPage.Master_OnDelete
        With Master.Factory.x25EntityField_ComboValueBL
            If .Delete(Master.DataPID) Then
                Master.DataPID = 0
                Master.CloseAndRefreshParent("x25-delete")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub _MasterPage_Master_OnRefresh() Handles _MasterPage.Master_OnRefresh
        RefreshRecord()
    End Sub

    Private Sub _MasterPage_Master_OnSave() Handles _MasterPage.Master_OnSave
        With Master.Factory.x25EntityField_ComboValueBL
            Dim cRec As BO.x25EntityField_ComboValue = IIf(Master.DataPID <> 0, .Load(Master.DataPID), New BO.x25EntityField_ComboValue)
            cRec.x25Name = Me.x25Name.Text
            cRec.x25Ordinary = BO.BAS.IsNullInt(Me.x25Ordinary.Value)
            cRec.x23ID = BO.BAS.IsNullInt(Me.x23ID.SelectedValue)
            cRec.x25Code = Me.x25Code.Text
            cRec.ValidFrom = Master.RecordValidFrom
            cRec.ValidUntil = Master.RecordValidUntil
            cRec.j02ID_Owner = BO.BAS.IsNullInt(Me.j02ID_Owner.Value)
            If panColors.Visible Then
                cRec.x25BackColor = basUI.GetColorFromPicker(Me.x25BackColor)
                cRec.x25ForeColor = basUI.GetColorFromPicker(Me.x25ForeColor)
            End If
            If Not InhaleUserFieldValues(cRec) Then
                Return
            End If


            If .Save(cRec) Then
                Master.DataPID = .LastSavedPID
                Master.CloseAndRefreshParent("x25-save")
            Else
                Master.Notify(.ErrorMessage, 2)
            End If
        End With
    End Sub

    Private Sub x25_record_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Master.DataPID <> 0 Then
            Me.x23ID.Enabled = False
        Else
            Me.x23ID.Enabled = True
        End If
    End Sub

    Private Function InhaleUserFieldValues(ByRef cRec As BO.x25EntityField_ComboValue) As Boolean
        Dim lisX16 As IEnumerable(Of BO.x16EntityCategory_FieldSetting) = Master.Factory.x18EntityCategoryBL.GetList_x16(Me.CurrentX18ID)
        Dim errs As New List(Of String)
        For Each ri As RepeaterItem In rpX16.Items
            Dim intX16ID As Integer = CInt(CType(ri.FindControl("x16ID"), HiddenField).Value)
            Dim c As BO.x16EntityCategory_FieldSetting = lisX16.First(Function(p) p.x16ID = intX16ID)
            Dim val As Object = Nothing
            Select Case c.FieldType
                Case BO.x24IdENUM.tString
                    If ri.FindControl("cbxFF").Visible Then
                        val = CType(ri.FindControl("cbxFF"), RadComboBox).Text
                    Else
                        val = CType(ri.FindControl("txtFF_Text"), TextBox).Text
                    End If
                    If val = "" Then val = Nothing
                Case BO.x24IdENUM.tDecimal
                    val = CType(ri.FindControl("txtFF_Number"), RadNumericTextBox).DbValue
                Case BO.x24IdENUM.tBoolean
                    val = CType(ri.FindControl("chkFF"), CheckBox).Checked
                Case BO.x24IdENUM.tDate, BO.x24IdENUM.tDateTime
                    With CType(ri.FindControl("txtFF_Date"), RadDatePicker)
                        If .IsEmpty Then
                            val = Nothing
                        Else
                            val = .DbSelectedDate
                        End If
                    End With
            End Select
            BO.BAS.SetPropertyValue(cRec, c.x16Field, val)
            If c.x16IsEntryRequired And val Is Nothing Then
                If c.FieldType <> BO.x24IdENUM.tBoolean Then
                    errs.Add(String.Format("Pole [{0}] je povinné k vyplnění.", c.x16Name))
                End If
            End If
        Next
        If errs.Count = 0 Then
            Return True
        Else
            Master.Notify(String.Join("<hr>", errs), NotifyLevel.ErrorMessage)
            Return False
        End If
    End Function

    Private Sub rpX16_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpX16.ItemDataBound
        Dim cRec As BO.x16EntityCategory_FieldSetting = CType(e.Item.DataItem, BO.x16EntityCategory_FieldSetting)

        e.Item.FindControl("txtFF_Number").Visible = False
        e.Item.FindControl("txtFF_Text").Visible = False
        e.Item.FindControl("chkFF").Visible = False
        e.Item.FindControl("txtFF_Date").Visible = False
        e.Item.FindControl("cbxFF").Visible = False

        CType(e.Item.FindControl("x16IsEntryRequired"), HiddenField).Value = BO.BAS.GB(cRec.x16IsEntryRequired)


        With CType(e.Item.FindControl("x16Name"), Label)
            Select Case cRec.FieldType
                Case BO.x24IdENUM.tDecimal : .Text = "<img src='Images/type_number.png'/> "
                Case BO.x24IdENUM.tBoolean : .Text = "<img src='Images/type_checkbox.png'/> "
                Case BO.x24IdENUM.tDate, BO.x24IdENUM.tDateTime : .Text = "<img src='Images/type_date.png'/> "
                Case Else : .Text = "<img src='Images/type_text.png'/> "
            End Select
            .Text += cRec.x16Name & ":"
            If cRec.x16IsEntryRequired Then
                .ForeColor = Drawing.Color.Red
                .Text = .Text & "*"
            End If
        End With

        CType(e.Item.FindControl("x16Field"), HiddenField).Value = cRec.x16Field

        CType(e.Item.FindControl("x16ID"), HiddenField).Value = cRec.x16ID.ToString


        Dim curValue As Object = BO.BAS.GetPropertyValue(_curRec, cRec.x16Field)
        If curValue Is System.DBNull.Value Then curValue = Nothing

        Select Case cRec.FieldType
            Case BO.x24IdENUM.tString
                CType(e.Item.FindControl("hidType"), HiddenField).Value = "string"
                If cRec.x16DataSource <> "" Then
                    e.Item.FindControl("cbxFF").Visible = True
                    Dim a() As String = Split(cRec.x16DataSource, ";")
                    With CType(e.Item.FindControl("cbxFF"), RadComboBox)
                        .AllowCustomText = Not cRec.x16IsFixedDataSource
                        If .AllowCustomText Then .Items.Add(New RadComboBoxItem(" "))
                        For i As Integer = 0 To UBound(a)
                            .Items.Add(New RadComboBoxItem(a(i)))
                        Next
                        If Not curValue Is Nothing Then
                            If .AllowCustomText Then
                                .Text = curValue
                            Else
                                Try
                                    .Items.FindItemByText(curValue).Selected = True
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        If cRec.x16TextboxWidth > 0 Then
                            .Style.Item("width") = cRec.x16TextboxWidth.ToString & "px"
                        End If
                    End With
                Else
                    With CType(e.Item.FindControl("txtFF_Text"), TextBox)
                        .Visible = True
                        If cRec.x16TextboxWidth > 40 Then
                            .Style.Item("width") = cRec.x16TextboxWidth.ToString & "px"
                        End If
                        If cRec.x16TextboxHeight > 20 Then
                            .Style.Item("height") = cRec.x16TextboxHeight.ToString & "px"
                            .TextMode = TextBoxMode.MultiLine
                        End If

                        If Not curValue Is Nothing Then
                            .Text = curValue
                        End If
                    End With
                End If

            Case BO.x24IdENUM.tDecimal
                CType(e.Item.FindControl("hidType"), HiddenField).Value = "decimal"
                With CType(e.Item.FindControl("txtFF_Number"), RadNumericTextBox)
                    .Visible = True
                    .NumberFormat.DecimalDigits = 2
                    If Not curValue Is Nothing Then .Value = CDbl(curValue)
                End With
           
            Case BO.x24IdENUM.tBoolean
                CType(e.Item.FindControl("hidType"), HiddenField).Value = "boolean"
                e.Item.FindControl("x16Name").Visible = False
                With CType(e.Item.FindControl("chkFF"), CheckBox)
                    .Visible = True
                    .Text = cRec.x16Name
                    If Not curValue Is Nothing Then
                        .Checked = curValue
                    End If
                End With
            Case BO.x24IdENUM.tDateTime
                CType(e.Item.FindControl("hidType"), HiddenField).Value = "datetime"

                With CType(e.Item.FindControl("txtFF_Date"), RadDatePicker)
                    .SharedCalendar = Me.SharedCalendar
                    .Visible = True
                    .MinDate = DateSerial(1900, 1, 1)
                    .MaxDate = DateSerial(2100, 1, 1)
                    If cRec.FormatString = "" Then cRec.FormatString = "dd.MM.yyyy"
                    .DateInput.DisplayDateFormat = cRec.FormatString
                    .DateInput.DateFormat = cRec.FormatString
                    
                    'Select Case cRec.TypeName
                    '    Case "datetime"
                    '        .DateFormat = "dd.MM.yyyy HH:mm"
                    '    Case "time"
                    '        .DateFormat = "HH:mm"
                    '    Case Else
                    '        .DateFormat = "dd.MM.yyyy"
                    'End Select

                    If Not curValue Is Nothing Then
                        If Year(curValue) > 1900 Then .SelectedDate = curValue
                    End If
                End With
        End Select
    End Sub

    Private Sub Handle_Changex20ID()
        If Me.opgX20ID.SelectedItem Is Nothing Then Return
        Dim lis As IEnumerable(Of BO.x20EntiyToCategory) = Master.Factory.x18EntityCategoryBL.GetList_x20(Me.CurrentX18ID)


    End Sub
End Class