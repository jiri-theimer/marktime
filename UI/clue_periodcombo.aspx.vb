Public Class clue_periodcombo
    Inherits System.Web.UI.Page
    Public Property NeedRebindPeriodCombo As Boolean = False


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master.Factory.j03UserBL
                .InhaleUserParams("periodcombo-custom_query")

                Dim s As String = .GetUserParam("periodcombo-custom_query")
                Me.hidCustomQueries.Value = s
                If s <> "" Then
                    Dim lis As New List(Of BO.x21DatePeriod)
                    Dim a() As String = s.Split("|"), x As Integer = 0
                    For Each strPair As String In a
                        x += 1
                        Dim b() As String = strPair.Split(";")
                        Dim cX21 As New BO.x21DatePeriod(x, BO.BAS.ConvertString2Date(b(0)), BO.BAS.ConvertString2Date(b(1)), b(2))
                        lis.Add(cX21)
                    Next
                    Me.period1.DataSource = lis
                    Me.period1.DataBind()
                End If
            End With

            'If Request.Item("value") <> "" Then
            '    basUI.SelectRadiolistValue(Me.period1, Request.Item("value"))
            '    If Not Me.period1.SelectedItem Is Nothing Then
            '        Me.d1.SelectedDate = Me.DateFrom
            '        Me.d2.SelectedDate = Me.DateUntil
            '        Me.period1.ClearSelection()
            '    Else

            '    End If
            'Else
            '    ''End If
            '    'If Request.Item("d1") <> "" Then
            '    '    Me.d1.SelectedDate = BO.BAS.ConvertString2Date(Request.Item("d1"))
            '    'End If
            '    'If Request.Item("d2") <> "" Then
            '    '    Me.d2.SelectedDate = BO.BAS.ConvertString2Date(Request.Item("d2"))
            '    'End If

            'End If
            
        End If
    End Sub

    Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
        If Me.d1.IsEmpty Or Me.d2.IsEmpty Then
            Master.Notify("Datumy musí být vyplněny.", NotifyLevel.WarningMessage) : Return
        End If
        If Me.d1.SelectedDate > Me.d2.SelectedDate Then
            Master.Notify("[Datum od] musí být menší nebo rovno než [Datum do].", NotifyLevel.WarningMessage) : Return
        End If
        If Me.d1.SelectedDate = Me.DateFrom Or Me.d2.SelectedDate = Me.DateUntil Then
            Master.Notify("Období s tímto rozsahem datumů již máte uložené.", NotifyLevel.InfoMessage)
            Return
        End If

        
        'přidat katalogu vlastních období

        Master.Factory.j03UserBL.InhaleUserParams("periodcombo-custom_query")
        Dim s As String = Master.Factory.j03UserBL.GetUserParam("periodcombo-custom_query")

        Dim t As String = Format(Me.d1.SelectedDate, "dd.MM.yyyy") & ";" & Format(Me.d2.SelectedDate, "dd.MM.yyyy") & ";" & IIf(Trim(Me.txtPeriodName.Text) = "", "Vlastní období", Me.txtPeriodName.Text)
        If Len(t) + Len(s) < 500 Then
            If s <> "" Then
                s += "|" & t
            Else
                s = t
            End If
        Else
            Dim a() As String = Split(s, "|"), ss As New StringBuilder, xx As Integer = 0
            For i As Integer = 1 To UBound(a)
                If i > 1 Then
                    ss.Append("|")
                End If
                ss.Append(a(i))
            Next
            s = ss.ToString
        End If

        Master.Factory.j03UserBL.SetUserParam("periodcombo-custom_query", s)
        Me.NeedRebindPeriodCombo = True

    End Sub

    Public Property x21ID As BO.x21IdEnum
        Get
            If Me.period1.SelectedValue = "" Then Return BO.x21IdEnum._NoQuery

            Dim a() As String = Me.period1.SelectedValue.Split("-")

            Return CType(CInt(a(0)), BO.x21IdEnum)
        End Get
        Set(value As BO.x21IdEnum)
            basUI.SelectRadiolistValue(Me.period1, CInt(value).ToString)
        End Set
    End Property

    Public ReadOnly Property DateFrom As Date
        Get
            Select Case Me.x21ID
                Case BO.x21IdEnum._CutomQuery
                    Return Me.CurrentX21.DateFrom
                Case BO.x21IdEnum._NoQuery
                    Return DateSerial(1900, 1, 1)
                Case Else
                    Dim c As New BO.x21DatePeriod(Me.x21ID)
                    Return c.DateFrom
            End Select

        End Get
    End Property
    Public ReadOnly Property DateUntil As Date
        Get
            Select Case Me.x21ID
                Case BO.x21IdEnum._CutomQuery
                    Return Me.CurrentX21.DateUntil
                Case BO.x21IdEnum._NoQuery
                    Return DateSerial(3000, 1, 1)
                Case Else
                    Dim c As New BO.x21DatePeriod(Me.x21ID)
                    Return c.DateUntil
            End Select
        End Get
    End Property
    Public ReadOnly Property CurrentX21 As BO.x21DatePeriod
        Get
            If Me.x21ID < BO.x21IdEnum._CutomQuery Then
                Return New BO.x21DatePeriod(Me.x21ID)
            End If

            Dim a() As String = Me.hidCustomQueries.Value.Split("|"), x As Integer = 0
            Dim strPair As String = a(Me.CustomQueryIndex - 1)
            a = strPair.Split(";")
            Return New BO.x21DatePeriod(x, BO.BAS.ConvertString2Date(a(0)), BO.BAS.ConvertString2Date(a(1)), a(2))

        End Get
    End Property
    Public ReadOnly Property CustomQueryIndex As Integer
        Get
            Dim a() As String = Me.period1.SelectedValue.Split("-")

            Return CInt(a(1))
        End Get
    End Property

    Private Sub period1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles period1.SelectedIndexChanged
        Me.d1.SelectedDate = Me.DateFrom
        Me.d2.SelectedDate = Me.DateUntil
    End Sub
End Class