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
        Dim bolValid As Boolean = False, strName As String = "", strAddress = ""

        Dim c As New VatService.checkVatPortTypeClient
        c.checkVat("SK", "2022444392", bolValid, strName, strAddress)
        Response.Write(strName & " ## " & strAddress)

        







    End Sub

    
    
    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click
        Dim dic(0) As String, res(0) As SpolehlivostDPH.InformaceOPlatciType
        dic(0) = "CZ25722034"



        Dim c As New SpolehlivostDPH.rozhraniCRPDPHClient
        c.getStatusNespolehlivyPlatce(dic, res)
        Select Case res(0).nespolehlivyPlatce
            Case SpolehlivostDPH.NespolehlivyPlatceType.ANO
                Response.Write("NESPOLEHLIVÝ")
            Case SpolehlivostDPH.NespolehlivyPlatceType.NENALEZEN
                Response.Write("NENALEZEN")
            Case SpolehlivostDPH.NespolehlivyPlatceType.NE
                Response.Write("SPOLEHLIVÝ")
        End Select

        For i As Integer = 0 To res(0).zverejneneUcty.Count - 1
            If TypeOf res(0).zverejneneUcty(i).Item Is SpolehlivostDPH.StandardniUcetType Then

                Dim ucet As SpolehlivostDPH.StandardniUcetType = CType(res(0).zverejneneUcty(i).Item, SpolehlivostDPH.StandardniUcetType)
                txtResult.Text = ucet.predcisli & " " & ucet.cislo & "/" & ucet.kodBanky

            End If


        Next
    End Sub
End Class