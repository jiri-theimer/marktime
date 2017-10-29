

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
        Master.Notify(Master.Factory.ftBL.get_ParsedText_With_Period("Měsíční DPH [YYYY]/[MM]", DateSerial(2017, 10, 1), 0))
    End Sub
End Class