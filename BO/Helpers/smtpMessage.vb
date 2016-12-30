Public Class smtpMessage
    Public Property Subject As String
    Public Property Body As String
    Public Property IsHtmlBody As Boolean
    Public Property SenderName As String
    Public Property SenderAddress As String

    Public Property o27UploadGUID As String
    Public Property AttachmentFiles_FullPath As List(Of String)

    Public Property o27Attachments As List(Of BO.o27Attachment)


    Public Sub AddOneFile2FullPath(strFileFullPath As String)
        If AttachmentFiles_FullPath Is Nothing Then AttachmentFiles_FullPath = New List(Of String)
        AttachmentFiles_FullPath.Add(strFileFullPath)

    End Sub
End Class
