Public Class periodmonth
    Inherits System.Web.UI.UserControl
    Public Event OnSelectedChanged()

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        With query_year
            If .Items.Count = 0 Then
                For i As Integer = -2 To 1
                    Dim intY As Integer = Year(Now) + i
                    .Items.Add(New ListItem(intY.ToString, intY.ToString))
                Next
            End If
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Public Property SelectedYear As Integer
        Get
            Return CInt(Me.query_year.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.query_year, value.ToString)
        End Set
    End Property
    Public Property SelectedMonth As Integer
        Get
            Return CInt(Me.query_month.SelectedValue)
        End Get
        Set(value As Integer)
            basUI.SelectDropdownlistValue(Me.query_month, value.ToString)
        End Set
    End Property
    Public ReadOnly Property SelectedDate As Date
        Get
            Return DateSerial(Me.SelectedYear, Me.SelectedMonth, 1)
        End Get
    End Property

    Private Sub query_month_SelectedIndexChanged(sender As Object, e As EventArgs) Handles query_month.SelectedIndexChanged
        RaiseEvent OnSelectedChanged()
    End Sub

    Private Sub query_year_SelectedIndexChanged(sender As Object, e As EventArgs) Handles query_year.SelectedIndexChanged
        RaiseEvent OnSelectedChanged()
    End Sub
End Class