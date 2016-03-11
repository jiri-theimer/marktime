Public Class dbupdate
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _MasterPage = Me.Master
        If Not Page.IsPostBack Then
            Dim cF As New BO.clsFile
            lblLastSpLog.Text = "Naposledy provedená SP aktualizace: " & cF.GetFileContents(Master.Factory.x35GlobalParam.UploadFolder & "\sql_step2_sp.log")
            lblDbVersion.Text = "Tato distribuce obsahuje db verzi: " & cF.GetFileContents(BO.ASS.GetApplicationRootFolder() & "\sys\dbupdate\version.txt")
            lblLastRunDifferenceLog.Text = "Naposledy provedená změna db struktury: " & cF.GetFileContents(Master.Factory.x35GlobalParam.UploadFolder & "\sql_db_difference.log")
        End If
    End Sub

    Private Sub cmdGo_Click(sender As Object, e As EventArgs) Handles cmdGo.Click
        Me.lblError.Text = "" : cmdRunResult.Visible = False
        Dim cBL As New BL.SysDbUpdateBL()
        Dim s As String = cBL.FindDifference()
        If cBL.ErrorMessage <> "" Then
            Me.lblError.Text = cBL.ErrorMessage
            Master.Notify(Me.lblError.Text, NotifyLevel.ErrorMessage)
            Return
        End If
        If s = "" Then
            Master.Notify("Není potřeba aktualizovat strukturu databáze.", NotifyLevel.InfoMessage)
        Else
            cmdRunResult.Visible = True
        End If
        Me.txtScript.Text = s
    End Sub

  

    Private Sub cmdSP_Click(sender As Object, e As EventArgs) Handles cmdSP.Click
        Dim cBL As New BL.SysDbUpdateBL()
        If Not cBL.RunSql_step2_sp() Then
            cBL.RunFixingSQLs_BeforeDbUpdate()  'spustit fixing sql dotazy
            cBL.RunFixingSQLs_AfterDbUpdate()  'spustit fixing sql dotazy
            Master.Notify(cBL.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            cBL.RunFixingSQLs_BeforeDbUpdate()  'spustit fixing sql dotazy
            cBL.RunFixingSQLs_AfterDbUpdate()  'spustit fixing sql dotazy
            Master.Notify("Operace dokončena.", NotifyLevel.InfoMessage)

        End If
    End Sub

    Private Sub cmdRunResult_Click(sender As Object, e As EventArgs) Handles cmdRunResult.Click
        Dim s As String = Trim(txtScript.Text)
        If s = "" Then
            Master.Notify("Není co spouštět!", NotifyLevel.WarningMessage)
            Return
        End If
        Dim cBL As New BL.SysDbUpdateBL()
        If Not cBL.RunDbDifferenceResult(s) Then
            Master.Notify(cBL.ErrorMessage, NotifyLevel.ErrorMessage)
        Else
            txtScript.Text = ""
            cmdRunResult.Visible = False

            Master.Notify("Operace dokončena.", NotifyLevel.InfoMessage)

        End If
    End Sub
End Class