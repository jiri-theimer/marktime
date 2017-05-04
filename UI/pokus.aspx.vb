


'Imports Aspose.Words



Public Class pokus
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    'Private fileFormatProvider As IFormatProvider


    
    Private Sub pokus_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master

    End Sub
    

    
    Private Sub pokus_Load(sender As Object, e As EventArgs) Handles Me.Load



        If Not Page.IsPostBack Then
            

        End If




    End Sub

    
    
    ''Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click
    ''    Dim dblHours As Double = BO.BAS.IsNullNum(Me.txtHours.Text)
    ''    Dim cT As New BO.clsTime
    ''    Me.txtResult.Text = cT.GetTimeFromSeconds(CInt(dblHours * 60 * 60))



    ''End Sub

    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click

        'Dim c As New Telerik.Web.Spreadsheet.Workbook

        'spread1.Provider = New Telerik.Web.UI.SpreadsheetDocumentProvider("c:\temp\inspis_cp_template.xlsx")
        'spread1.Provider = New Telerik.Web.UI.SpreadsheetDocumentProvider(Server.MapPath("~/App_Data/inspis_cp_template.xlsx"))

        ''spread1.Provider = New Telerik.Web.UI.SpreadsheetDocumentProvider("c:\temp\ahoj.xlsx")



    End Sub
End Class