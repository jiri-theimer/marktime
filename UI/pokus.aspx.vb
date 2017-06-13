


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
            Dim c As New Telerik.Web.UI.Appointment
            c.Start = Now
            c.End = Now.AddHours(1)
            c.ID = 1
            c.Subject = "AK lhůta"
            c.Description = "Poznámka"

            scheduler1.InsertAppointment(c)

            c = New Telerik.Web.UI.Appointment
            c.Start = Now.AddHours(1)
            c.End = Now.AddHours(1)
            c.ID = 2
            c.Subject = "AK úkol"
            c.Description = "Poznámka k úkolu"
            c.BackColor = Drawing.Color.SkyBlue
            scheduler1.InsertAppointment(c)

            c = New Telerik.Web.UI.Appointment
            c.Start = Now.AddDays(50)
            c.End = Now.AddDays(50)
            c.ID = 3
            c.Subject = "Zajít na pivo"
            c.Description = "Poznámka k úkolu"
            c.BackColor = Drawing.Color.SkyBlue
            scheduler1.InsertAppointment(c)

            c = New Telerik.Web.UI.Appointment
            c.Start = Now.AddHours(71)
            c.End = Now.AddHours(71)
            c.ID = 4
            c.Subject = "Zajít na poštu"
            c.Description = "Poznámka k úkolu"
            c.BackColor = Drawing.Color.SkyBlue
            scheduler1.InsertAppointment(c)

            c = New Telerik.Web.UI.Appointment
            c.ID = 43
            c.Subject = "Úkol bez termínu"
            c.Description = "Poznámka k úkolu"
            c.BackColor = Drawing.Color.SkyBlue

            scheduler1.InsertAppointment(c)

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