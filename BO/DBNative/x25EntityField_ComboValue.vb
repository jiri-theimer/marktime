﻿Public Class x25EntityField_ComboValue
    Inherits BOMother
    Public Property x23ID As Integer
    Public Property x25Name As String
    Public Property x25Ordinary As Integer
    Public Property x25UserKey As String
    Public Property x25BackColor As String
    Public Property x25ForeColor As String

    Private Property _x23Name As String
    Public ReadOnly Property x23Name As String
        Get
            Return _x23Name
        End Get
    End Property

    Public ReadOnly Property NameWithComboName As String
        Get
            Return _x23Name & ": " & Me.x25Name
        End Get
    End Property
    Public ReadOnly Property StyleDecoration As String
        Get
            If Me.IsClosed Then Return "line-through" Else Return ""
        End Get
    End Property
End Class
