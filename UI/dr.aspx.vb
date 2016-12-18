Public Class dr
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Site

    Private Sub dr_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.SiteMenuValue = "dashboard"
        If Not Page.IsPostBack Then
            If Request.Item("prefix") = "" Or Request.Item("pid") = "" Then
                Master.StopPage("Na vstupu chybí identifikace záznamu (prefix a pid).")
                Return
            End If
            Dim strPrefix As String = Request.Item("prefix"), strPID As String = Request.Item("pid"), bolNeedMobileUI As Boolean = False
            If strPrefix = "b07" Then
                Dim c As BO.b07Comment = Master.Factory.b07CommentBL.Load(BO.BAS.IsNullInt(strPID))
                If c Is Nothing Then Master.StopPage("Nelze najít záznam komentáře.")
                strPrefix = BO.BAS.GetDataPrefix(c.x29ID)
                strPID = c.b07RecordPID.ToString
            End If
            Dim strPage As String = strPrefix & "_framework.aspx"

            With Master.Factory.SysUser
                If .j03MobileForwardFlag = BO.j03MobileForwardFlagENUM.Auto Then bolNeedMobileUI = basUI.DetectIfMobileDefice(Request)
                If bolNeedMobileUI Then strPage = "mobile_" & strPrefix & "_framework"
                strPage += "?board=1"
                Select Case strPrefix
                    Case "p56", "p41", "p28", "p91", "o23", "j02"
                        strPage += "&pid=" & strPID
                        Response.Redirect(strPage)
                    Case Else
                        'pokud se nenajde jiná stránka, pak zobrazit přes cluetip zobrazení
                        Dim strURL As String = "clue_" & strPrefix & "_record.aspx?pid=" & strPID & "&dr=1"
                        paneContent.ContentUrl = strURL

                End Select
            End With
            
            

        End If

    End Sub

End Class