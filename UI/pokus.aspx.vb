Imports Telerik.Web.UI
Imports Aspose.Words



Public Class pokus
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site
    Private fileFormatProvider As IFormatProvider


    
    Private Sub pokus_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub
    

    
    Private Sub pokus_Load(sender As Object, e As EventArgs) Handles Me.Load

        ''Dim u As String = Request.ServerVariables("HTTP_USER_AGENT")
        ''Dim b As New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase)
        ''Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
        ''If b.IsMatch(u) Or v.IsMatch(Left(u, 4)) Then
        ''    'detekováno mobilní zařízení
        ''    Response.Redirect("http://detectmobilebrowser.com/mobile")
        ''End If
        If Not Page.IsPostBack Then
            

        End If
    End Sub

   
    Private Sub cmdPokus_Click(sender As Object, e As EventArgs) Handles cmdPokus.Click


        Dim client As New mtService()

       
        Dim fields As New Dictionary(Of String, Object)
        fields.Add("p28CompanyName", "WCF Klient, založený v čase: " & Now.ToString)
        fields.Add("j02ID_Owner", 3)
        fields.Add("p28IsCompany", True)
        Dim addresses As New List(Of BO.o37Contact_Address)
        Dim a As New BO.o37Contact_Address
        a.o36ID = BO.o36IdEnum.InvoiceAddress
        a.o38City = "Zliv"
        a.o38Street = "Jáchymovská 338"
        a.o38ZIP = "37344"
        a.o38Country = "Česká Republika"
        addresses.Add(a)



        Dim ret As BO.ServiceResult = client.SaveClient(0, fields, addresses, Nothing, "lama", "123456")
        If ret.IsSuccess Then
            MsgBox(ret.PID.ToString & ": OK")
        Else
            MsgBox(ret.ErrorMessage)
        End If

        ''Dim u As String = Request.ServerVariables("HTTP_USER_AGENT")
        ''Dim b As New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase)
        ''Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
        ''If b.IsMatch(u) Or v.IsMatch(Left(u, 4)) Then
        ''    Master.Notify("Mobilní zařízení")
        ''Else
        ''    Master.Notify("Není detekováno mobilní zařízení")
        ''End If

        
    End Sub

    
    Private Sub cmdPostback_Click(sender As Object, e As EventArgs) Handles cmdPostback.Click
        Dim ds As New DataSet, s As String = ""
        ds.ReadXml("c:\temp\pokus.xml", XmlReadMode.ReadSchema)

        Dim dt As DataTable = ds.Tables("b02WorkflowStatus")
        Master.Factory.copyManagerBL.CopyDataTableContent(dt)

        's += dt.TableName & ":" & dt.Rows.Count & "<hr>"
        'For i = 0 To dt.Columns.Count - 1
        '    s += " # " & dt.Columns(i).ColumnName & " | " & dt.Columns(i).DataType.ToString
        'Next

        'For Each dbrow As DataRow In dt.Rows

        'Next

        'Master.Notify(s)
    End Sub

   

    'Private Sub cmdARES_Click(sender As Object, e As EventArgs) Handles cmdARES.Click

    '    Dim strURL As String = "http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_bas.cgi?ico=" & Me.txtIC.Text

    '    Dim client As System.Net.WebClient = New System.Net.WebClient()
    '    Dim data As System.IO.Stream
    '    Try
    '        data = client.OpenRead(strURL)
    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '        Return
    '    End Try
    '    Dim c As New BO.AresRecord
    '    Dim reader As System.IO.StreamReader = New System.IO.StreamReader(data, True)

    '    Dim ds As New DataSet
    '    ds.ReadXml(reader)

    '    reader.Close()
    '    data.Close()


    '    Try
    '        c.Company = ds.Tables("OF").Rows(0).Item(1)
    '    Catch ex As Exception
    '    End Try
    '    Try
    '        c.DIC = ds.Tables("DIC").Rows(0).Item(1)
    '    Catch ex As Exception
    '    End Try


    '    Dim dbRow As DataRow = ds.Tables("AA").Rows(0)
    '    c.Street = Trim(RF(dbRow, "NU") & " " & RF(dbRow, "CD"))
    '    c.PostCode = RF(dbRow, "PSC")
    '    c.City = RF(dbRow, "NMC")
    '    c.Country = RF(dbRow, "NS")
    '    c.ID_adresy = RF(dbRow, "IDA")

    '    ds.Clear()



    'End Sub

    'Private Function RF(dbRow As DataRow, strProperty As String) As String
    '    Try
    '        Return dbRow.Item(strProperty)
    '    Catch ex As Exception
    '        Return ""
    '    End Try
    'End Function

    'Private Sub cmdPDF_Click(sender As Object, e As EventArgs) Handles cmdPDF.Click
    '    Dim doc1 As New ceTe.DynamicPDF.Merger.MergeDocument("c:\temp\faktura_koncept_20150040.pdf")
    '    doc1.Author = "Jiri Theimer"
    '    doc1.Append("c:\temp\priloha.pdf")
    '    doc1.DrawToWeb("vysledek.pdf")

    'End Sub

    Private Sub cmd1_Click(sender As Object, e As EventArgs) Handles cmd1.Click
        Dim s As String = ""
        For Each c In cbx1.CheckedItems
            s += "," & c.Value
        Next
        Master.Notify(s)

    End Sub

    

    Private Sub cmdPDF_Click(sender As Object, e As EventArgs) Handles cmdPDF.Click

        Dim strLicFile As String = BO.ASS.GetApplicationRootFolder() & "\bin\Aspose_Words.lic"
        Dim license As New Aspose.Words.License()

        license.SetLicense(strLicFile)

        Dim doc As New Document("c:\temp\bez-krokovy komentar.docx")
        doc.Save("c:\temp\aspose\vysledek.pdf", SaveFormat.Pdf)


    End Sub

  
End Class