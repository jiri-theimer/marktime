﻿Imports System.Reflection
Imports System.IO

Public Class AppAssembly
    Public Shared Function GetRootFolder() As String
        Dim s As String = AppDomain.CurrentDomain.BaseDirectory
        If Right(s, 1) = "\" Then
            Return Left(s, Len(s) - 1)
        Else
            Return s
        End If
    End Function
    Public Shared Function GetFrameworkVersion() As String
        Return Environment.Version.ToString

    End Function
    
    Public Shared Function GetUIVersion() As String
        Dim ass As [Assembly] = [Assembly].GetExecutingAssembly()
        Dim a() As String = Split(ass.ToString, ",")
        Dim strAppVer As String = a(1)

        Dim strFile As String = GetRootFolder() & "\bin\UI.dll"
        If System.IO.File.Exists(strFile) Then
            Dim info As New FileInfo(strFile)
            Return strAppVer & ", build: " & Format(info.LastWriteTime, "dd.MM.yyyy HH:mm")
        Else
            Return strAppVer
        End If
    End Function

    Public Shared Function GetConfigVal(ByVal strKey As String, Optional ByVal strDefault As String = "") As String
        Dim s As String = System.Configuration.ConfigurationManager.AppSettings.Item(strKey)
        If s Is Nothing Then Return strDefault
        If s = "" Then
            Return strDefault
        Else
            Return s
        End If

    End Function

    'Public Shared Function GetUploadFolder() As String
    '    Dim strDir As String = BL.AppAssembly.GetConfigVal("UploadDir")
    '    If strDir = "" Then Return ""

    '    If Not System.IO.Directory.Exists(strDir) Then
    '        System.IO.Directory.CreateDirectory(strDir)
    '    End If

    '    Return strDir
    'End Function
End Class
