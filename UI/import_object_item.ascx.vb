Public Class import_object_item
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory
    Private Property _lis As IEnumerable(Of BO.p85TempBox) = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InhaleObjectRecord(strGUID As String, strPrefix As String)
        hidGUID.Value = strGUID
        hidPrefix.Value = strPrefix

        LoadList()


    End Sub
    Private Sub LoadList()
        _lis = Me.Factory.p85TempBoxBL.GetList(hidGUID.Value)
    End Sub
    Private Function GetRec(strKey As String) As BO.p85TempBox
        If _lis Is Nothing Then
            LoadList()
        End If
        If _lis.Where(Function(p) LCase(p.p85FreeText02) = LCase(strKey)).Count > 0 Then
            Return _lis.Where(Function(p) LCase(p.p85FreeText02) = LCase(strKey))(0)
        Else
            Return New BO.p85TempBox
        End If
    End Function
    Public Function FindString(strKey As String) As String
        Return GetRec(strKey).p85Message
    End Function
    Public Function FindDate(strKey As String) As Date
        Return GetRec(strKey).p85FreeDate01

    End Function
    Public Function FindDouble(strKey As String) As Double
        Return GetRec(strKey).p85FreeFloat01
    End Function
    Public Function FindInteget(strKey As String) As Integer
        Return GetRec(strKey).p85OtherKey1
    End Function
    Public Function FindBoolean(strKey As String) As Boolean
        Return GetRec(strKey).p85FreeBoolean01
    End Function
End Class