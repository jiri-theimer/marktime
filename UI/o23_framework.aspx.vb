﻿Public Class o23_framework
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Server.Transfer("entity_framework.aspx?prefix=o23" & basUI.GetCompleteQuerystring(Request), False)
    End Sub

End Class