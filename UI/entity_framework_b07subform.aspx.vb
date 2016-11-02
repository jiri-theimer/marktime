﻿Public Class entity_framework_b07subform
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

        If Not Page.IsPostBack Then
            Me.CurrentMasterPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
            Me.CurrentMasterPrefix = Request.Item("masterprefix")
            If Me.CurrentMasterPID = 0 Or Me.CurrentMasterPrefix = "" Then Master.StopPage("masterpid or masterprefix missing.")
            If Request.Item("lasttabkey") <> "" Then
                Master.Factory.j03UserBL.SetUserParam(Request.Item("lasttabkey"), Request.Item("lasttabval"))
            End If
            comments1.RefreshData(Master.Factory, BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix), Me.CurrentMasterPID)
        End If
    End Sub



End Class