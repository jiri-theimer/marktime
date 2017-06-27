﻿Public Class x25_record_readonly1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rec1.Factory = Master.Factory
        If Not Page.IsPostBack Then
            Master.DataPID = BO.BAS.IsNullInt(Request.Item("pid"))
            If Master.DataPID = 0 Then Master.StopPage("pid is missing")
            RefreshRecord()
        End If
    End Sub


    Private Sub RefreshRecord()
        Dim cRec As BO.x25EntityField_ComboValue = Master.Factory.x25EntityField_ComboValueBL.Load(Master.DataPID)
        Dim cX18 As BO.x18EntityCategory = Master.Factory.x18EntityCategoryBL.LoadByX23ID(cRec.x23ID)


        rec1.FillData(cRec, cX18)

        Dim lisX69 As IEnumerable(Of BO.x69EntityRole_Assign) = Master.Factory.x67EntityRoleBL.GetList_x69(BO.x29IdEnum.x25EntityField_ComboValue, cRec.PID)
        roles1.RefreshData(lisX69, cRec.PID)
        comments1.RefreshData(Master.Factory, BO.x29IdEnum.x25EntityField_ComboValue, cRec.PID)
    End Sub
End Class