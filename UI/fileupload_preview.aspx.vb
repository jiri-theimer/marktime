Public Class fileupload_preview
    Inherits System.Web.UI.Page

    Protected WithEvents _MasterPage As ModalForm

    Private Sub fileupload_preview_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.list1.Factory = Master.Factory

        If Not Page.IsPostBack Then
            With Master
                If Request.Item("prefix") = "" Or Request.Item("pid") = "" Then
                    .StopPage("prefix or pid missing...")
                End If
                .HeaderText = "Náhled na soubor/přílohu"
                .DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
                .AddToolbarButton("Stáhnout", "download", , "Images/download.png", False, "javascript:download2('prefix=" & Request.Item("prefix") & "&pid=" & Request.Item("pid") & "&disposition=attachment')")

            End With
            ViewState("if1_height") = "90%"
            Dim mq As New BO.myQueryO27
            Select Case Request.Item("prefix")
                Case "o23"
                    Me.panList.Visible = True
                    Me.list1.RefreshData_O23(Master.DataPID)
                    If Me.list1.ItemsCount > 0 Then
                        Dim cRec As BO.o27Attachment = Me.list1.GetListO27(0)
                        ViewState("url") = "binaryfile.aspx?prefix=o27&disposition=inline&pid=" & cRec.PID.ToString
                    End If
                    If list1.ItemsCount > 3 Then
                        list1.IsRepeatDirectionVerticaly = True : list1.RepeatColumns = 2
                    End If
                    Master.HeaderText = Master.Factory.GetRecordCaption(BO.x29IdEnum.o23Notepad, Master.DataPID)
                    ViewState("if1_height") = "80%"
                Case "o27"
                    Dim cRec As BO.o27Attachment = Master.Factory.o27AttachmentBL.Load(Master.DataPID)
                    ViewState("url") = "binaryfile.aspx?prefix=o27&disposition=inline&pid=" & cRec.PID.ToString
                Case "p85"
                    ViewState("url") = "binaryfile.aspx?prefix=p85&disposition=inline&pid=" & Master.DataPID.ToString
            End Select


        End If
    End Sub

    

End Class