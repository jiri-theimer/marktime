﻿Imports Microsoft.VisualBasic
Imports System.IO
    Public Class clsINIFile
        Public Function read(ByVal iniFile As String, ByVal section As String, ByVal searchKey As String) As String

            Dim lenSS As Integer = Len(searchKey)
            Dim locQ As Integer
            Dim newStr As String, strFoundSectionLine As String = ""
            Dim bolFoundSection As Boolean = False

            Using sr As StreamReader = _
              New StreamReader(iniFile)
                Dim line As String
                ' loop until end of file

                Do
                    line = sr.ReadLine()

                    ' check each line for a key match
                    If UCase(line).IndexOf("[" & UCase(section) & "]") >= 0 Then
                        bolFoundSection = True
                        line = sr.ReadLine()
                    End If
                    If bolFoundSection Then
                        If UCase(Left(line, lenSS)) = UCase(searchKey) Then
                            ' match found, now parse out value

                            ' find the first quote mark

                            locQ = InStr(line, "=")
                            ' now create the value

                            newStr = Mid(line, (locQ + 1), ((Len(line) - locQ)))
                        ''newStr = Trim(Replace(newStr, Chr(34), ""))
                            ' return the value

                            Return newStr
                            line = Nothing
                        End If
                    If Not line Is Nothing Then
                        If line.IndexOf("[") >= 0 And line.IndexOf("]") >= 0 And line <> strFoundSectionLine Then
                            line = Nothing
                        End If
                    End If
                    End If


                Loop Until line Is Nothing
                sr.Close()
            End Using
            Return ""
        End Function

        Public Shared Function write(ByVal iniFile As String, _
               ByVal writeKey As String, ByVal writeValue As String) As String

            ' temp file #1

            Dim iniTempFile As String = "/tempsettings_del.ini"
            ' temp file #2

            Dim iniTempOrigFile As String = "/tempsettingsorig_del.ini"
            ' length of search string

            Dim lenSS As Integer = Len(writeKey)
            ' eof? t/f

            Dim boolEof As Boolean
            ' string to search for at eof

            Dim strEof As String = "[eof]"
            ' server.mappath to all files used

            Dim iniMappedFile As String = _
               HttpContext.Current.Server.MapPath(iniFile)
            Dim iniMappedTempFile As String = _
               HttpContext.Current.Server.MapPath(iniTempFile)
            Dim iniMappedTempOrigFile As String = _
               HttpContext.Current.Server.MapPath(iniTempOrigFile)

            ' setup file to output to

            Dim sw As StreamWriter = New StreamWriter(iniMappedTempFile)

            ' process overview:

            ' 1. read each line of orig ini file

            ' 2. check for match, if no match, write to temp file

            ' 3. if match, write new value

            ' 4. write rest of orig ini file


            ' new value to write

            Dim strNewValue = writeKey & " = """ & writeValue & """"

            Using sr As StreamReader = New StreamReader(iniMappedFile)
                Dim line As String
                ' loop until end of file

                Do
                    line = sr.ReadLine()
                    ' first check for eof so we don't write extra blank lines

                    If Left(line, 5) = strEof Then boolEof = True

                    ' check each line for a key match

                    If Left(line, lenSS) = writeKey Then
                        ' match found, write new value to temp file

                        sw.WriteLine(strNewValue)
                    Else
                        ' are we at the enf of the file?

                        If boolEof = True Then
                            sw.WriteLine(strEof)
                            ' break out of loop

                            Exit Do
                        End If
                        ' match not found, write line to temp file

                        sw.WriteLine(line)
                    End If
                Loop Until line Is Nothing
                sr.Close()
            End Using
            sw.Close()

            ' 1 / 3. move orig file to temp file in case 2nd move fails

            File.Move(iniMappedFile, iniMappedTempOrigFile)

            ' 2 / 3. move temp file to orig file

            File.Move(iniMappedTempFile, iniMappedFile)

            ' 3 / 3. delete renamed, now temp orig file

            File.Delete(iniMappedTempOrigFile)

            ' done

            Return ""
        End Function
    End Class

