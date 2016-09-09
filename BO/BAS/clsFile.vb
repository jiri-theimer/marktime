Imports System.IO

Public Class clsFile
    Private _Error As String
    Public ReadOnly Property ErrorMessage As String
        Get
            Return _Error
        End Get
    End Property
    Public Function GetFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "", Optional ByVal bolWin1250 As Boolean = True, Optional bolReadFirstLineOnly As Boolean = False) As String

        Dim strContents As String
        Dim objReader As StreamReader
        Try

            If bolWin1250 Then
                objReader = New StreamReader(FullPath, System.Text.Encoding.GetEncoding(1250))
            Else
                objReader = New StreamReader(FullPath, System.Text.Encoding.UTF8)
            End If
            If bolReadFirstLineOnly Then
                strContents = objReader.ReadLine()
            Else
                strContents = objReader.ReadToEnd()
            End If

            objReader.Close()
            Return strContents

        Catch Ex As Exception
            _Error = Ex.Message
            ErrInfo = Ex.Message
        End Try

        Return ""

    End Function



    Public Function SaveText2File(ByVal FullPath As String, ByVal strText As String, Optional ByVal bolAppend As Boolean = False, Optional ByRef ErrInfo As String = "", Optional ByVal bolWin1250 As Boolean = True) As Boolean
        Dim objWriter As StreamWriter
        Try
            If bolWin1250 Then
                objWriter = New StreamWriter(FullPath, bolAppend, System.Text.Encoding.GetEncoding(1250))
            Else
                objWriter = New StreamWriter(FullPath, bolAppend, System.Text.Encoding.UTF8)
            End If
            objWriter.Write(strText)
            objWriter.Close()

            Return True

        Catch ex As Exception
            ErrInfo = ex.Message
            _Error = ex.Message
        End Try
        Return False
    End Function

    Public Function DeleteFile(ByVal FullPath As String) As Boolean
        If File.Exists(FullPath) Then
            Try
                File.Delete(FullPath)
                Return True
            Catch ex As Exception
                _Error = ex.Message
            End Try
        Else
            _Error = "File not found"
        End If
        Return False
    End Function

    Public Function GetNameFromFullpath(ByVal FullPath As String, Optional ByVal bolExcludeSuffix As Boolean = False, Optional ByRef strRetSuffix As String = "") As String
        Dim a() As String = Split(FullPath, "\")
        Dim s As String = a(UBound(a))
        strRetSuffix = Right(FullPath, 3)
        If bolExcludeSuffix Then
            a = Split(s, ".")
            s = a(0)
            strRetSuffix = a(UBound(a))
        End If
        Return s
    End Function

    Public Function GetFileDirectory(ByVal FullPath As String) As String
        Dim a() As String = Split(FullPath, "\")
        Dim i As Integer, s As String = ""
        For i = 0 To UBound(a) - 1
            If s <> "" Then
                s += "\" & a(i)
            Else
                s = a(i)
            End If

        Next
        Return s
    End Function


    Public Function GetFileSize(ByVal FullPath As String) As Long
        Try
            Dim info As New FileInfo(FullPath)
            Return info.Length
        Catch ex As Exception
            _Error = ex.Message
            Return 0
        End Try
    End Function

    Public Function GetFileExtension(ByVal FullPath As String) As String
        Try
            Dim info As New FileInfo(FullPath)
            Return info.Extension
        Catch ex As Exception
            _Error = ex.Message
            Return ""
        End Try
    End Function

    Public Function RenameFile(ByVal FullPath As String, ByVal NewPath As String) As Boolean
        Try
            File.Move(FullPath, NewPath)
            Return True
        Catch ex As System.IO.IOException
            _Error = ex.Message
        End Try
        Return False
    End Function
    Public Function CopyFile(ByVal FullPath As String, ByVal NewPath As String) As Boolean
        Try
            File.Copy(FullPath, NewPath, True)
            Return True
        Catch ex As System.IO.IOException
            _Error = ex.Message
        End Try
        Return False
    End Function


    Public Function FileExist(ByVal FullPath As String) As Boolean
        Return File.Exists(FullPath)
    End Function

    Public Function GetFileListFromDir(ByVal strDir As String, ByVal strMask As String) As List(Of String)
        Dim lis As New List(Of String)
        If Not IO.Directory.Exists(strDir) Then Return lis

        Dim di As New IO.DirectoryInfo(strDir)
        Dim diar1 As IO.FileInfo() = di.GetFiles(strMask)
        Dim dra As IO.FileInfo, s As String = ""
        For Each dra In diar1
            lis.Add(dra.Name)
        Next
        Return lis

    End Function

    Public Function GetContentType(ByVal strFullPath As String) As String
        Dim strExt As String = LCase(GetFileExtension(strFullPath))
        If Left(strExt, 1) = "." Then strExt = Right(strExt, Len(strExt) - 1)
        Select Case strExt
            Case "" : Return ""
            Case "pdf" : Return "application/pdf"
            Case "doc" : Return "application/msword"
            Case "docx" : Return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            Case "xlsx" : Return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Case "jpg", "jpeg" : Return "image/jpeg"
            Case "gif", "png", "bmp" : Return "image/" & strExt
            Case "msg" : Return "message/rfc822"
            Case Else
                Return ("application/." & strExt).Replace("..", ".")
        End Select
    End Function

    Public Function GetBinaryContent(strFullPath As String) As Byte()
        If Not File.Exists(strFullPath) Then Return Nothing

        Dim fi As FileInfo = New FileInfo(strFullPath)
        Dim sr As New StreamReader(strFullPath)
        Dim reader As New BinaryReader(sr.BaseStream)

        Dim ret As Byte() = reader.ReadBytes(reader.BaseStream.Length())
        reader.Close()

        Return ret

        
    End Function
End Class




