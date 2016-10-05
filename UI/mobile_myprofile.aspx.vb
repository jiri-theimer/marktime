Public Class mobile_myprofile
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As Mobile

    Private Sub mobile_myprofile_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .MenuPrefix = "myprofile"
                With .Factory.SysUser
                    lblUser.Text = .Person
                    Me.j03Login.Text = .j03Login
                    Me.j04Name.Text = .j04Name
                    Me.Person.Text = .Person
                    basUI.SelectRadiolistValue(Me.j03MobileForwardFlag, CInt(.j03MobileForwardFlag).ToString)
                End With
                If .Factory.SysUser.j02ID <> 0 Then
                    Dim cJ02 As BO.j02Person = .Factory.j02PersonBL.Load(.Factory.SysUser.j02ID)
                    Me.j07Name.Text = cJ02.j07Name
                    Me.j18Name.Text = cJ02.j18Name
                    Me.j02Email.Text = cJ02.j02Email
                    Me.Teams.Text = .Factory.j02PersonBL.GetTeamsInLine(cJ02.PID)
                End If
                

            End With
        End If
    End Sub

    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        With Master.Factory
            Dim cRec As BO.j03User = .j03UserBL.Load(.SysUser.PID)
            cRec.j03MobileForwardFlag = CInt(Me.j03MobileForwardFlag.SelectedValue)
            If .j03UserBL.Save(cRec) Then
                Master.Notify("Změny uloženy.")
            End If
        End With

    End Sub
End Class