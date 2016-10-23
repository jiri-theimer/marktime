Public Class entity_framework_p31subform
    Inherits System.Web.UI.Page

    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property
    Public Property CurrentMasterPID As Integer
        Get
            Return BO.BAS.IsNullInt(Me.hidMasterPID.Value)
        End Get
        Set(value As Integer)
            hidMasterPID.Value = value.ToString
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridP31.Factory = Master.Factory

        If Not Page.IsPostBack Then
            If Request.Item("pid") <> "" Then
                gridP31.DefaultSelectedPID = BO.BAS.IsNullInt(Request.Item("pid"))
            End If
            Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            If Me.CurrentMasterPID = 0 Or Me.CurrentMasterPrefix = "" Then Master.StopPage("masterpid or masterprefix missing.")

        End If

        gridP31.MasterDataPID = Me.CurrentMasterPID
        gridP31.EntityX29ID = BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
        If Request.Item("IsApprovingPerson") = "" Then
            gridP31.AllowApproving = Master.Factory.SysUser.IsApprovingPerson
        Else
            gridP31.AllowApproving = BO.BAS.BG(Request.Item("IsApprovingPerson"))
        End If

    End Sub

    Private Sub entity_framework_p31subform_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        
    End Sub



   
End Class