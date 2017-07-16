
Public Class DatovkaRoot
    Public Property name As String
    Public Property id As String
    Public Property metadata() As Object
    Public Property [sub] As List(Of DatovkaItem)
End Class

Public Class DatovkaItem
    Public Property name As String
    Public Property id As String
    Public Property metadata() As Object
    Public Property [sub] As List(Of DatovkaItem)
End Class
