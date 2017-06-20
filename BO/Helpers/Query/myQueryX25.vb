
Public Class myQueryX25
    Inherits myQuery
    Public Property x23ID As Integer
    Public Property p41IDs As List(Of Integer)
    Public Property j02IDs As List(Of Integer)
    Public Property Owners As List(Of Integer)
    Public Property p56IDs As List(Of Integer)
    Public Property p28IDs As List(Of Integer)

    Public Property Record_x29ID As BO.x29IdEnum = x29IdEnum._NotSpecified
    Public Property RecordPID As Integer

    Public Property DateQueryFieldBy As String

    Public Property CalendarDateFieldStart As String
    Public Property CalendarDateFieldEnd As String



    Public Sub New(intX23ID As Integer)
        Me.x23ID = intX23ID
    End Sub

End Class
