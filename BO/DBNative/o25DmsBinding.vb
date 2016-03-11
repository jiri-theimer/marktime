Public Enum o25DmsAppENUM
    Dropbox = 1
    GoogleDrive = 2
End Enum
Public Enum o25DmsObjectENUM
    Folder = 1
    File = 2
    Link = 3
End Enum
Public Class DropboxUserToken
    Public Property Token As String
    Public Property Secret As String
End Class

Public Class o25DmsBinding
    Inherits BOMother
    Public o25DmsApp As o25DmsAppENUM
    Public o25DmsObject As o25DmsObjectENUM
    Public x29ID As BO.x29IdEnum
    Public Property o25RecordPID As Integer
    Public Property o25Path As String
    Public Property o25Icon As String
End Class
