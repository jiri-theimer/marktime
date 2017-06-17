Public Enum x20EntryFlagENUM
    Combo = 1
    InsertUpdateWithoutCombo = 2
End Enum

Public Class x20EntiyToCategory
    Public Property x20ID As Integer
    Public Property x18ID As Integer
    Public Property x29ID As Integer
    Public Property x20Name As String
    Public Property x20IsMultiSelect As Boolean
    Public Property x20IsEntryRequired As Boolean
    Public Property x20EntityTypePID As Integer
    Public Property x29ID_EntityType As Integer
    Public Property x20EntryFlag As x20EntryFlagENUM = x20EntryFlagENUM.Combo

    Public Property EntityTypeAlias As String   'pomocný atribut - není v SQL
    Public Property x20IsClosed As Boolean

End Class
