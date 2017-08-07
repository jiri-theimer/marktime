

Imports Telerik.Web.UI
Imports System.Web.Script.Serialization

'Imports Aspose.Words




Public Class pokus
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    'Private fileFormatProvider As IFormatProvider



    Private Sub pokus_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master


    End Sub



    
    Private Sub pokus_Load(sender As Object, e As EventArgs) Handles Me.Load
        
    End Sub

    
    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click
        Dim j02ids As List(Of Integer) = BO.BAS.ConvertPIDs2List(Me.txt1.Text)
        Dim j11ids As List(Of Integer) = BO.BAS.ConvertPIDs2List(Me.txt2.Text)
        Master.Notify(Master.Factory.j02PersonBL.GetEmails_j02_join_j11(j02ids, j11ids).Count)

        txt3.Text = String.Join(",", Master.Factory.j02PersonBL.GetEmails_j02_join_j11(j02ids, j11ids).Select(Function(p) p.x43Email))


    End Sub

    Private Sub cmdFormat_Click(sender As Object, e As EventArgs) Handles cmdFormat.Click
        Dim d As Date = DateSerial(2017, 8, 31)
        Me.txt2.Text = Right("0" & Month(d).ToString, 2) & "/" & Right("0" & Day(d).ToString, 2) & "/" & Year(d).ToString
    End Sub
End Class