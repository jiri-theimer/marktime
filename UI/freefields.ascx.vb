﻿Imports Telerik.Web.UI


Public Class freefields
    Inherits System.Web.UI.UserControl
    Public Event RenderField(strName As String, ByVal strField As String, ByVal strDataType As String, ByVal dbValue As Object)
    Private _Error As String
    Private _dataprefix As String
    Private _lastX27ID As Integer
    Private _lisX25 As IEnumerable(Of BO.x25EntityField_ComboValue)
    Public Factory As BL.Factory

    Public Function IsEmpty() As Boolean
        If rpFF.Items.Count > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Public ReadOnly Property FieldsCount As Integer
        Get
            Return rpFF.Items.Count
        End Get
    End Property
    Public ReadOnly Property ErrorMessage As String
        Get
            Return _Error
        End Get
    End Property

    Public Property DataTable() As String
        Get
            Return hidDataTable.Value
        End Get
        Set(ByVal value As String)
            hidDataTable.Value = value
            _dataprefix = Left(value, 3)
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub FillData(ByVal listFF As IEnumerable(Of BO.FreeField))
        rpFF.DataSource = listFF
        rpFF.DataBind()
        If Me.rpFF.Items.Count = 0 Then
            Me.panContainer.Visible = False
        Else
            Me.panContainer.Visible = True
        End If
    End Sub

    Private Sub rpFF_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpFF.ItemDataBound
        Dim cRec As BO.FreeField = CType(e.Item.DataItem, BO.FreeField)

        e.Item.FindControl("txtFF_Number").Visible = False
        e.Item.FindControl("txtFF_Text").Visible = False
        e.Item.FindControl("chkFF").Visible = False
        e.Item.FindControl("txtFF_Date").Visible = False
        e.Item.FindControl("cbxFF").Visible = False
        CType(e.Item.FindControl("x28IsRequired"), HiddenField).Value = BO.BAS.GB(cRec.x28IsRequired)

        If BO.BAS.IsNullInt(cRec.x27ID) <> _lastX27ID Then
            e.Item.FindControl("trHeader").Visible = True
            CType(e.Item.FindControl("headerFF"), Label).Text = cRec.X27Name
        End If
        _lastX27ID = BO.BAS.IsNullInt(cRec.X27ID)
        With CType(e.Item.FindControl("lblFF"), Label)
            .Text = cRec.x28Name & ":"
            If cRec.x28IsRequired Then
                .ForeColor = Drawing.Color.Red
                .Text = .Text & "*"
            End If
        End With

        CType(e.Item.FindControl("hidField"), HiddenField).Value = cRec.x28Field
        CType(e.Item.FindControl("hidType"), HiddenField).Value = cRec.TypeName
        CType(e.Item.FindControl("hidX28ID"), HiddenField).Value = cRec.PID.ToString
        CType(e.Item.FindControl("hidX23ID"), HiddenField).Value = cRec.x23ID.ToString
       
        Select Case cRec.TypeName
            Case "string"
                If cRec.x28DataSource <> "" Then
                    e.Item.FindControl("cbxFF").Visible = True
                    Dim a() As String = Split(cRec.x28DataSource, ";")
                    With CType(e.Item.FindControl("cbxFF"), RadComboBox)
                        .AllowCustomText = Not cRec.x28IsFixedDataSource
                        If .AllowCustomText Then .Items.Add(New RadComboBoxItem(" "))
                        For i As Integer = 0 To UBound(a)
                            .Items.Add(New RadComboBoxItem(a(i)))
                        Next
                        If Not cRec.DBValue Is System.DBNull.Value Then
                            If .AllowCustomText Then
                                .Text = cRec.DBValue
                            Else
                                Try
                                    .Items.FindItemByText(cRec.DBValue).Selected = True
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        If cRec.x28TextboxWidth > 0 Then
                            .Style.Item("width") = cRec.x28TextboxWidth.ToString & "px"
                        End If
                    End With
                Else
                    With CType(e.Item.FindControl("txtFF_Text"), TextBox)
                        .Visible = True
                        If cRec.x28TextboxWidth > 40 Then
                            .Style.Item("width") = cRec.x28TextboxWidth.ToString & "px"
                        End If
                        If cRec.x28TextboxHeight > 20 Then
                            .Style.Item("height") = cRec.x28TextboxHeight.ToString & "px"
                            .TextMode = TextBoxMode.MultiLine
                        End If
                        If Not cRec.DBValue Is System.DBNull.Value Then
                            .Text = cRec.DBValue
                        End If
                    End With
                End If

            Case "decimal"
                With CType(e.Item.FindControl("txtFF_Number"), RadNumericTextBox)
                    .Visible = True
                    .NumberFormat.DecimalDigits = 2
                    If Not cRec.DBValue Is System.DBNull.Value Then .Value = CDbl(cRec.DBValue)
                End With
            Case "integer"
                If cRec.x23ID <> 0 Then
                    If cRec.x23DataSource = "" Then
                        'combo seznam bez externího datového zdroje
                        If _lisX25 Is Nothing Then _lisX25 = Me.Factory.x25EntityField_ComboValueBL.GetList(0)
                        With CType(e.Item.FindControl("cbxFF"), RadComboBox)
                            .Visible = True
                            .AllowCustomText = False
                            .ShowToggleImage = True
                            .Items.Add(New RadComboBoxItem(""))
                            For Each c In _lisX25.Where(Function(p) p.IsClosed = False And p.x23ID = cRec.x23ID)
                                .Items.Add(New RadComboBoxItem(c.x25Name, c.PID.ToString))
                            Next
                            If Not (cRec.DBValue Is Nothing Or cRec.DBValue Is System.DBNull.Value) Then
                                .SelectedValue = cRec.DBValue.ToString
                                If .SelectedValue = "" Then
                                    'najít uzavřenou položku
                                    Dim lis As IEnumerable(Of BO.x25EntityField_ComboValue) = _lisX25.Where(Function(p) p.PID = CInt(cRec.DBValue))
                                    If lis.Count > 0 Then
                                        Dim cItem As BO.x25EntityField_ComboValue = lis(0)
                                        If Not cItem Is Nothing Then
                                            Dim cbxItem As RadComboBoxItem = New RadComboBoxItem(cItem.x25Name, cItem.PID.ToString)
                                            cbxItem.Font.Strikeout = cItem.IsClosed
                                            .Items.Add(cbxItem)
                                        End If
                                        .SelectedValue = cRec.DBValue.ToString
                                    End If
                                End If
                            End If
                        End With
                    End If
                    If cRec.x23DataSource <> "" Then
                        'combobox s externím datovým zdrojem
                        With CType(e.Item.FindControl("cbxFF"), RadComboBox)
                            .Visible = True : .AllowCustomText = False : .ShowToggleImage = True
                            .EnableLoadOnDemand = True
                            .OnClientItemsRequesting = Me.ClientID & "_OnClientItemsRequesting"
                            .MarkFirstMatch = True
                            .EnableTextSelection = True

                            .WebServiceSettings.Method = "LoadComboData"
                            .WebServiceSettings.UseHttpGet = False
                            .WebServiceSettings.Path = "~/Services/combo_external_datasource.asmx"
                            .Attributes.Item("x23id") = cRec.x23ID.ToString

                            If Not (cRec.DBValue Is Nothing Or cRec.DBValue Is System.DBNull.Value) Then
                                .SelectedValue = cRec.DBValue.ToString
                                .Text = cRec.ComboText
                            End If
                        End With

                    End If
                Else
                    With CType(e.Item.FindControl("txtFF_Number"), RadNumericTextBox)
                        .Visible = True
                        .NumberFormat.DecimalDigits = 0
                        If Not cRec.DBValue Is System.DBNull.Value Then .Value = CDbl(cRec.DBValue)
                    End With
                End If

            Case "boolean"
                e.Item.FindControl("lblFF").Visible = False
                With CType(e.Item.FindControl("chkFF"), CheckBox)
                    .Visible = True
                    .Text = cRec.x28Name
                    If Not cRec.DBValue Is System.DBNull.Value Then
                        .Checked = cRec.DBValue
                    End If
                End With
            Case "date", "datetime", "time"
                With CType(e.Item.FindControl("txtFF_Date"), RadDateInput)
                    .Visible = True
                    .MinDate = DateSerial(1900, 1, 1)
                    .MaxDate = DateSerial(2100, 1, 1)
                    Select Case cRec.TypeName
                        Case "datetime"
                            .DateFormat = "dd.MM.yyyy HH:mm"
                        Case "time"
                            .DateFormat = "HH:mm"
                        Case Else
                            .DateFormat = "dd.MM.yyyy"
                    End Select

                    If Not cRec.DBValue Is System.DBNull.Value Then
                        If Year(cRec.DBValue) > 1900 Then .SelectedDate = cRec.DBValue
                    End If
                End With
        End Select
        With cRec
            RaiseEvent RenderField(.x28Name, .x28Field, .TypeName, .DBValue)
        End With

    End Sub

    Public Function GetValues() As List(Of BO.FreeField)
        Dim lis As New List(Of BO.FreeField)

        For Each ri As RepeaterItem In rpFF.Items
            Dim cRec As New BO.FreeField()
            With cRec
                .SetPID(BO.BAS.IsNullInt(CType(ri.FindControl("hidX28ID"), HiddenField).Value))
                .x28Field = CType(ri.FindControl("hidField"), HiddenField).Value
                .SetTypeFromName(CType(ri.FindControl("hidType"), HiddenField).Value)
                .x28Name = CType(ri.FindControl("lblFF"), Label).Text.Replace(":", "").Replace("*", "")
                .x28IsRequired = BO.BAS.BG(CType(ri.FindControl("x28IsRequired"), HiddenField).Value)
                .x23ID = BO.BAS.IsNullInt(CType(ri.FindControl("hidX23ID"), HiddenField).Value)


                Select Case .x24ID
                    Case BO.x24IdENUM.tString
                        If ri.FindControl("cbxFF").Visible Then
                            .DBValue = CType(ri.FindControl("cbxFF"), RadComboBox).Text
                        Else
                            .DBValue = CType(ri.FindControl("txtFF_Text"), TextBox).Text
                        End If
                    Case BO.x24IdENUM.tDate, BO.x24IdENUM.tDateTime, BO.x24IdENUM.tTime
                        With CType(ri.FindControl("txtFF_Date"), RadDateInput)
                            If .IsEmpty Then
                                cRec.DBValue = Nothing
                            Else
                                cRec.DBValue = .DbSelectedDate
                            End If
                        End With
                    Case BO.x24IdENUM.tDecimal
                        .DBValue = CType(ri.FindControl("txtFF_Number"), RadNumericTextBox).DbValue
                    Case BO.x24IdENUM.tInteger
                        If ri.FindControl("cbxFF").Visible Then
                            .DBValue = BO.BAS.IsNullInt(CType(ri.FindControl("cbxFF"), RadComboBox).SelectedValue)
                            .ComboText = CType(ri.FindControl("cbxFF"), RadComboBox).Text
                            If CType(ri.FindControl("cbxFF"), RadComboBox).EnableLoadOnDemand Then
                                .IsExternalDataSource = True    'externí datový zdroj
                            End If
                        Else
                            .DBValue = CType(ri.FindControl("txtFF_Number"), RadNumericTextBox).DbValue
                        End If
                    Case BO.x24IdENUM.tBoolean
                        .DBValue = CType(ri.FindControl("chkFF"), CheckBox).Checked
                End Select
            End With

            lis.Add(cRec)
        Next

        Return lis
    End Function



    

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Me.Factory Is Nothing Then
            Me.hidJ03ID.Value = Factory.SysUser.PID.ToString
        End If

    End Sub
End Class