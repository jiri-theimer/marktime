﻿'Public Class FreeFormValue
'    Public x28ID As Integer
'    Public x28Name As String
'    Public x28Field As String
'    Public x28DataType As String
'    Public DbValue As Object
'End Class

Public Class x28EntityField
    Inherits BOMother
    Public Property x29ID As x29IdEnum
    Public Property x24ID As x24IdENUM
    Public Property x27ID As Integer
    Public Property x23ID As Integer
    Public Property x28Name As String

    Public Property x28Ordinary As Integer
    Public Property x28IsAllEntityTypes As Boolean

    Public Property x28DataSource As String
    Public Property x28IsFixedDataSource As Boolean
    Public Property x28TextboxHeight As Integer
    Public Property x28TextboxWidth As Integer
    Public Property x28IsRequired As Boolean

    Public Property x28Field As String

    Public Property x28IsPublic As Boolean = True
    Public Property x28NotPublic_j04IDs As String
    Public Property x28NotPublic_j07IDs As String

    Protected Property _x29Name As String
    Public ReadOnly Property x29Name As String
        Get
            Return _x29Name
        End Get
    End Property
    Protected Property _x27Name As String
    Public ReadOnly Property x27Name As String
        Get
            Return _x27Name
        End Get
    End Property
    Protected _TypeName As String
    Public ReadOnly Property TypeName As String
        Get
            Return _TypeName
        End Get
    End Property
    Protected Property _x23Name As String
    Public ReadOnly Property x23Name As String
        Get
            Return _x23Name
        End Get
    End Property
    Protected Property _x23DataSource As String
    Public ReadOnly Property x23DataSource As String
        Get
            Return _x23DataSource
        End Get
    End Property

    Public ReadOnly Property SourceTableName As String
        Get
            Select Case Me.x29ID
                Case BO.x29IdEnum.p41Project : Return "p41Project_FreeField"
                Case BO.x29IdEnum.p28Contact : Return "p28Contact_FreeField"
                Case BO.x29IdEnum.p91Invoice : Return "p91Invoice_FreeField"
                Case BO.x29IdEnum.j02Person : Return "j02Person_FreeField"
                Case BO.x29IdEnum.p56Task : Return "p56Task_FreeField"
                Case x29IdEnum.p31Worksheet : Return "p31WorkSheet_FreeField"
                Case x29IdEnum.o23Notepad : Return "o23Notepad_FreeField"
                Case Else
                    Return ""
            End Select
        End Get
    End Property

End Class
