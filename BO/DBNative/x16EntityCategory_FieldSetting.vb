Public Class x16EntityCategory_FieldSetting
    Public Property x16ID As Integer
    Public Property x18ID As Integer
    Public Property x16IsEntryRequired As Boolean
    Public Property x16Name As String
    Public Property x16Field As String
    Public Property x16Ordinary As Integer
    Public Property x16DataSource As String
    Public Property x16IsFixedDataSource As Boolean
    Public Property x16IsGridField As Boolean
    Public Property x16TextboxHeight As Integer
    Public Property x16TextboxWidth As Integer
    Public Property FormatString As String


    Public ReadOnly Property FieldType As BO.x24IdENUM
        Get
            If LCase(Me.x16Field).IndexOf("number") > 0 Then
                Return x24IdENUM.tDecimal
            End If
            If LCase(Me.x16Field).IndexOf("date") > 0 Then
                Return x24IdENUM.tDateTime
            End If
            If LCase(Me.x16Field).IndexOf("boolean") > 0 Then
                Return x24IdENUM.tBoolean
            End If

            Return x24IdENUM.tString
        End Get
    End Property
End Class
