Imports System.DateTime

Public Enum x21IdEnum
    Dnes = 10
    Vcera = 11
    Zitra = 12
    TydenTento = 20
    TydenMinuly = 21
    TydenPristi = 22
    MesicTento = 30
    MesicMinuly = 31
    MesicMinus2 = 33
    MesicMinus3 = 34
    MesicMinus4 = 35
    MesicMinus5 = 36
    MesicPristi = 32
    KvartalTento = 40
    KvartalMinuly = 41
    KvartalPristi = 42
    RokTento = 50
    RokMinuly = 51
    RokPristi = 52
    DoDnes = 61
    DoMinulyMesic = 62
    DoMinulyMesic2 = 63
    DoMinulyMesic3 = 64
    DoMinulyRok = 70
    _NoQuery = 0
    _CutomQuery = 101
End Enum

Public Class x21DatePeriod
    Inherits BOMotherFT
    Private Property _x21Name As String
    Private Property _x21NameDef As String
    public Property _x21ID As x21IdEnum
    Private _DateFrom As Date = DateSerial(1900, 1, 1)
    Private _DateUntil As Date = DateSerial(3000, 1, 1)
    Private _Today As Date = Today

    Public Property CustomQueryIndex As Integer = 0

    Public Sub New(setX21ID As x21IdEnum)
        _x21ID = setX21ID
        If _x21ID > x21IdEnum._NoQuery And _x21ID < x21IdEnum._CutomQuery Then Me.SetPeriod(Today)
    End Sub
    Public Sub New(intCustomQueryIndex As Integer, datFrom As Date, datUntil As Date, strName As String)
        _x21ID = x21IdEnum._CutomQuery
        Me.CustomQueryIndex = intCustomQueryIndex
        _DateFrom = datFrom
        _DateUntil = datUntil
        _x21NameDef = strName
        _x21Name = strName
    End Sub

    Public ReadOnly Property x21Name As String
        Get
            If _x21Name = "" Then Return _x21NameDef
            Return _x21Name
        End Get
    End Property
    Public ReadOnly Property x21ID As Integer
        Get
            Return _x21ID
        End Get
    End Property
    Public ReadOnly Property NameWithDates As String
        Get
            Dim s As String = _x21Name
            If s = "" Then s = _x21NameDef
            If _x21ID = x21IdEnum._NoQuery Then Return s
            Select Case _x21ID
                Case x21IdEnum.Dnes, x21IdEnum.Vcera
                    Return s & " [" & BO.BAS.FD(_DateUntil) & "]"
                Case x21IdEnum.DoDnes, x21IdEnum.DoMinulyMesic, x21IdEnum.DoMinulyMesic2, x21IdEnum.DoMinulyMesic3, x21IdEnum.DoMinulyRok
                    Return s & " [ -> " & BO.BAS.FD(_DateUntil) & "]"

                Case Else
                    Return s & " [" & BO.BAS.FD(_DateFrom) & " - " & BO.BAS.FD(_DateUntil) & "]"
            End Select

        End Get
    End Property
    Public ReadOnly Property ComboValue As String
        Get
            If _x21ID = x21IdEnum._NoQuery Then
                Return ""
            Else
                Return Me.x21ID.ToString & "-" & Me.CustomQueryIndex.ToString
            End If
        End Get
    End Property

    Public ReadOnly Property DateFrom As Date
        Get
            InhaleDates()
            Return _DateFrom
        End Get
    End Property
    Public ReadOnly Property DateUntil As Date
        Get
            InhaleDates()
            Return _DateUntil
        End Get
    End Property



    Public Sub SetPeriod(datToday As Date)
        _Today = datToday
        InhaleDates()
    End Sub
    
    Private Sub InhaleDates()
        Select Case _x21ID
            Case x21IdEnum._NoQuery
                _x21NameDef = "--Nefiltrovat období--"
            Case x21IdEnum.Dnes
                _DateFrom = _Today
                _DateUntil = _Today
                _x21NameDef = "Dnes"
            Case x21IdEnum.Vcera
                _DateFrom = _Today.AddDays(-1)
                _DateUntil = _Today.AddDays(-1)
                _x21NameDef = "Včera"
            Case x21IdEnum.Zitra
                _DateFrom = _Today.AddDays(1)
                _DateUntil = _Today.AddDays(1)
                _x21NameDef = "Zítra"
            Case x21IdEnum.TydenTento
                _DateFrom = GetFirstMondayOfCurrentWeek()
                _DateUntil = _DateFrom.AddDays(6)
                _x21NameDef = "Tento týden"
            Case x21IdEnum.TydenMinuly
                _DateFrom = GetFirstMondayOfCurrentWeek().AddDays(-7)
                _DateUntil = _DateFrom.AddDays(6)
                _x21NameDef = "Minulý týden"
            Case x21IdEnum.TydenPristi
                _DateFrom = GetFirstMondayOfCurrentWeek().AddDays(7)
                _DateUntil = _DateFrom.AddDays(6)
                _x21NameDef = "Příští týden"
            Case x21IdEnum.MesicTento
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.MesicMinuly
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1).AddMonths(-1)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.MesicMinus2
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1).AddMonths(-2)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.MesicMinus3
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1).AddMonths(-3)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.MesicMinus4
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1).AddMonths(-4)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.MesicMinus5
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1).AddMonths(-5)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.MesicPristi
                _DateFrom = DateSerial(_Today.Year, _Today.Month, 1).AddMonths(1)
                _DateUntil = _DateFrom.AddMonths(1).AddDays(-1)
                _x21NameDef = Format(_DateFrom, "MM") & "/" & Format(_DateFrom, "yyyy")
            Case x21IdEnum.KvartalTento
                Dim x As Integer = Math.Ceiling(_Today.Month / 3)
                Dim d1 As New DateTime(_Today.Year, (3 * x) - 2, 1)
                _DateFrom = d1
                _DateUntil = _DateFrom.AddMonths(3).AddDays(-1)
                _x21NameDef = "Tento kvartál"
            Case x21IdEnum.KvartalMinuly
                Dim x As Integer = Math.Ceiling(_Today.Month / 3)
                Dim d1 As New DateTime(_Today.Year, (3 * x) - 2, 1)
                _DateFrom = d1.AddMonths(-3)
                _DateUntil = _DateFrom.AddMonths(3).AddDays(-1)
                _x21NameDef = "Minulý kvartál"
            Case x21IdEnum.KvartalPristi
                Dim x As Integer = Math.Ceiling(_Today.Month / 3)
                Dim d1 As New DateTime(_Today.Year, (3 * x) - 2, 1)
                _DateFrom = d1.AddMonths(3)
                _DateUntil = _DateFrom.AddMonths(3).AddDays(-1)
                _x21NameDef = "Příští kvartál"
            Case x21IdEnum.RokTento
                _DateFrom = DateSerial(Today.Year, 1, 1)
                _DateUntil = DateSerial(Today.Year, 12, 31)
                _x21NameDef = "Tento rok"
            Case x21IdEnum.RokMinuly
                _DateFrom = DateSerial(Today.Year - 1, 1, 1)
                _DateUntil = DateSerial(Today.Year - 1, 12, 31)
                _x21NameDef = "Minulý rok"
            Case x21IdEnum.RokPristi
                _DateFrom = DateSerial(Today.Year + 1, 1, 1)
                _DateUntil = DateSerial(Today.Year + 1, 12, 31)
                _x21NameDef = "Příští rok"
            Case x21IdEnum.DoDnes
                _DateFrom = DateSerial(2000, 1, 1)
                _DateUntil = Today
                _x21NameDef = "Do dnes vč."
            Case x21IdEnum.DoMinulyMesic
                _DateFrom = DateSerial(2000, 1, 1)
                _DateUntil = DateSerial(Today.Year, Today.Month, 1).AddDays(-1)
                _x21NameDef = "Do konce " & Format(_DateUntil, "MM") & "/" & Format(_DateUntil, "yyyy")
            Case x21IdEnum.DoMinulyMesic2
                _DateFrom = DateSerial(2000, 1, 1)
                _DateUntil = DateSerial(Today.Year, Today.Month, 1).AddDays(-1).AddMonths(-1)
                _x21NameDef = "Do konce " & Format(_DateUntil, "MM") & "/" & Format(_DateUntil, "yyyy")
            Case x21IdEnum.DoMinulyMesic3
                _DateFrom = DateSerial(2000, 1, 1)
                _DateUntil = DateSerial(Today.Year, Today.Month, 1).AddDays(-1).AddMonths(-2)
                _x21NameDef = "Do konce " & Format(_DateUntil, "MM") & "/" & Format(_DateUntil, "yyyy")
            Case x21IdEnum.DoMinulyRok
                _DateFrom = DateSerial(2000, 1, 1)
                _DateUntil = DateSerial(Today.Year - 1, 12, 31)
                _x21NameDef = "Do konce " & Format(_DateUntil, "yyyy")
        End Select
    End Sub


    

    Private Function GetFirstMondayOfCurrentWeek() As DateTime
        Select Case _Today.DayOfWeek
            Case System.DayOfWeek.Sunday
                Return _Today.AddDays(-6)
            Case System.DayOfWeek.Monday
                Return _Today
            Case Else
                Return _Today.AddDays(-_Today.DayOfWeek + 1)
        End Select

    End Function
End Class
