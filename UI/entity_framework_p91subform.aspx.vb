Public Class entity_framework_p91subform
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
        gridP91.Factory = Master.Factory
        If Not Page.IsPostBack Then
            Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            If Me.CurrentMasterPID = 0 Or Me.CurrentMasterPrefix = "" Then Master.StopPage("masterpid or masterprefix missing.")

        End If
        
        gridP91.MasterDataPID = Me.CurrentMasterPID
        gridP91.x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)


    End Sub

End Class