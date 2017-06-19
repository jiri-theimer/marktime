
Public Class myQueryX25
    Inherits myQuery
    Public Property x23ID As Integer
    Public Property Record_x29ID As BO.x29IdEnum = x29IdEnum._NotSpecified
    Public Property RecordPID As Integer

    Public Property DateQueryFieldBy As String



    Public Sub New(intX23ID As Integer)
        Me.x23ID = intX23ID
    End Sub

End Class
