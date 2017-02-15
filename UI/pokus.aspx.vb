Imports Telerik.Web.UI
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
        Dim dblHours As Double = BO.BAS.IsNullNum(Me.txtHours.Text)
        Dim cT As New BO.clsTime
        Me.txtResult.Text = cT.GetTimeFromSeconds(CInt(dblHours * 60 * 60))



    End Sub
End Class