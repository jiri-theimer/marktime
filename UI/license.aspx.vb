Public Class license
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As ModalForm

    Private Sub license_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .neededPermission = BO.x53PermValEnum.GR_Admin
                .HeaderText = "Nahrát licenční soubor"
                .HeaderIcon = "Images/license_32.png"
                .AddToolbarButton("Nahrát soubor na server", "ok", , "Images/ok.png")
            End With
            If BO.ASS.GetConfigVal("Guru") = "1" Then
                panGuru.Visible = True
            End If

        End If
    End Sub

    Private Sub _MasterPage_Master_OnToolbarClick(strButtonValue As String) Handles _MasterPage.Master_OnToolbarClick
        If strButtonValue = "ok" Then
            
            For Each invalidFile As Telerik.Web.UI.UploadedFile In upload1.InvalidFiles
                Master.Notify("Soubor [" & invalidFile.FileName & "] nelze nahrát na server jako licenční klíč.", 2)
            Next
            If upload1.UploadedFiles.Count = 0 Then
                Master.Notify("Musíte vybrat soubor soubor licenčního klíče.", 2)
                Return
            End If

            For Each validFile As Telerik.Web.UI.UploadedFile In upload1.UploadedFiles
                Dim strPath As String = Master.Factory.x35GlobalParam.TempFolder & "\" & validFile.FileName
                validFile.SaveAs(strPath, True)
                Dim cF As New BO.clsFile
                Dim strKeyCrypted As String = cF.GetFileContents(strPath), strLicenseSubject As String = ""
                If strKeyCrypted.IndexOf(Chr(10)) > 0 Then
                    Dim a() As String = Split(strKeyCrypted, Chr(10))
                    strKeyCrypted = a(0)
                    strLicenseSubject = a(1)
                End If
                Dim strKeyDecrypted As String = BO.Crypto.Decrypt(strKeyCrypted, "5lepsi4urcite")
                If Not IsNumeric(Replace(strKeyDecrypted, "-", "")) Then
                    Master.Notify("Soubor obsahuje nesprávný tvar klíče.", 2)
                    Return
                End If
                Dim cRec As BO.x35GlobalParam = Master.Factory.x35GlobalParam.Load("AppScope")
                cRec.x35Value = strKeyCrypted
                If Master.Factory.x35GlobalParam.Save(cRec) Then
                    If strLicenseSubject <> "" Then
                        cRec = Master.Factory.x35GlobalParam.Load("License_Company")
                        cRec.x35Value = strLicenseSubject
                        Master.Factory.x35GlobalParam.Save(cRec)
                    End If
                    Master.CloseAndRefreshParent("license")
                End If
            Next
        End If
    End Sub

    Private Sub cmdGenerate_Click(sender As Object, e As EventArgs) Handles cmdGenerate.Click
        Dim s As String = Right("000" & Me.txtMaxUsers.Text, 3) & "-"
        s += BO.BAS.GB(Me.TaxInvoices.Checked)
        s += "0"
        s += BO.BAS.GB(Me.Workflow.Checked)
        s += "0"
        s += "0"
        s += "0"
        s += "0"
        s = Left(s & "0000000000000000000000000000000000000000000000000000000", 20)
        Dim t As String = BO.Crypto.Encrypt(s, "5lepsi4urcite")
        If txtLicenseSubject.Text <> "" Then
            t += vbCrLf & txtLicenseSubject.Text
        End If
        t += vbCrLf & "TaxInvoices: " & BO.BAS.GB(Me.TaxInvoices.Checked)

        t += vbCrLf & "Workflow: " & BO.BAS.GB(Me.Workflow.Checked)
       

        t += vbCrLf & "Generated: " & BO.BAS.FD(Now, True)


        Me.NewKey_Crypted.Text = t

        Dim u As String = BO.Crypto.Decrypt(t, "5lepsi4urcite")
        Me.NewKey_Decrypted.Text = u
    End Sub
End Class