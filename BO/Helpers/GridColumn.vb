Public Enum cfENUM
    AnyString = 1
    DateOnly = 2
    DateTime = 3
    Checkbox = 4
    Numeric = 6
    Numeric0 = 7
    Numeric2 = 8
    Numeric3 = 9
    TimeOnly = 5

End Enum

Public Class GridColumn
    Public Property x29ID As BO.x29IdEnum
    Public Property ColumnType As cfENUM
    Public Property ColumnHeader As String
    Public Property ColumnName As String
    Public Property IsSortable As Boolean = True
    Public Property ColumnDBName As String
    Public Property IsShowTotals As Boolean = False

    Public Sub New(colX29ID As BO.x29IdEnum, strHeader As String, strName As String, Optional colType As cfENUM = cfENUM.AnyString, Optional bolSortable As Boolean = True)
        Me.x29ID = colX29ID
        Me.ColumnHeader = strHeader
        Me.ColumnName = strName
        Me.ColumnType = colType
        Me.IsSortable = bolSortable
    End Sub

    
End Class

Public Class GridGroupByColumn
    Public Property ColumnHeader As String
    Public Property ColumnField As String

    Public Sub New(strHeader As String, strField As String)
        Me.ColumnHeader = strHeader
        Me.ColumnField = strField
    End Sub
End Class
