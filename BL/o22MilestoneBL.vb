Public Interface Io22MilestoneBL
    Inherits IFMother
    Function Save(cRec As BO.o22Milestone, lisO20 As List(Of BO.o20Milestone_Receiver), lisO19 As List(Of BO.o19Milestone_NonPerson)) As Boolean
    Function Load(intPID As Integer) As BO.o22Milestone
    Function LoadMyLastCreated() As BO.o22Milestone
    Function Delete(intPID As Integer) As Boolean
    Function GetList(mq As BO.myQueryO22) As IEnumerable(Of BO.o22Milestone)
    Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.o22Milestone)
    Function GetList_o20(intPID As Integer) As IEnumerable(Of BO.o20Milestone_Receiver)
    Function GetList_o19(intPID As Integer) As IEnumerable(Of BO.o19Milestone_NonPerson)
    Sub Handle_Reminder()
End Interface
Class o22MilestoneBL
    Inherits BLMother
    Implements Io22MilestoneBL
    Private WithEvents _cDL As DL.o22MilestoneDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.o22MilestoneDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.o22Milestone, lisO20 As List(Of BO.o20Milestone_Receiver), lisO19 As List(Of BO.o19Milestone_NonPerson)) As Boolean Implements Io22MilestoneBL.Save
        With cRec
            If .o21ID = 0 Then _Error = "Chybí typ milníku/události." : Return False
            Dim cO21 As BO.o21MilestoneType = Me.Factory.o21MilestoneTypeBL.Load(.o21ID)
            Select Case .x29ID
                Case BO.x29IdEnum.j02Person
                    If .j02ID = 0 Then _Error = "Chybí vazba na osobu." : Return False
                Case BO.x29IdEnum.p41Project
                    If .p41ID = 0 Then _Error = "Chybí vazba na projekt." : Return False
                Case BO.x29IdEnum.p28Contact
                    If .p28ID = 0 Then _Error = "Chybí vazba na záznam kontaktu." : Return False
                Case BO.x29IdEnum.p91Invoice
                    If .p28ID = 0 Then _Error = "Chybí vazba na záznam faktury." : Return False
                Case BO.x29IdEnum.p90Proforma
                    If .p28ID = 0 Then _Error = "Chybí vazba na záznam zálohové faktury." : Return False
            End Select
            If .o22Name = "" Then
                _Error = "Chybí název (předmět)." : Return False
            End If
            Select Case cO21.o21Flag
                Case BO.o21FlagEnum.DeadlineOrMilestone
                    If BO.BAS.IsNullDBDate(.o22DateUntil) Is Nothing Then
                        _Error = "Chybí datum (termín) milníku." : Return False
                    End If
                    .o22DateFrom = Nothing
                    .o22IsAllDay = False
                Case BO.o21FlagEnum.EventFromUntil
                    If .o22IsAllDay Then
                        .o22DateFrom = DateSerial(Year(.o22DateFrom), Month(.o22DateFrom), Day(.o22DateFrom))
                        .o22DateUntil = DateSerial(Year(.o22DateUntil), Month(.o22DateUntil), Day(.o22DateUntil)).AddDays(1).AddSeconds(-1)
                    End If
                    If BO.BAS.IsNullDBDate(.o22DateUntil) Is Nothing Then
                        _Error = "Chybí [Konec] události." : Return False
                    End If
                    If BO.BAS.IsNullDBDate(.o22DateFrom) Is Nothing Then
                        _Error = "Chybí [Začátek] události." : Return False
                    End If
                    If .o22DateFrom.Value > .o22DateUntil.Value Then
                        _Error = "[Začátek] musí být menší [Konec] události." : Return False
                    End If
                    
                Case BO.o21FlagEnum.MemoOnly
                    .o22DateFrom = Nothing
                    .o22DateUntil = Nothing
            End Select
            If Not BO.BAS.IsNullDBDate(.o22ReminderDate) Is Nothing Then
                If .o22ReminderDate > .o22DateFrom Then
                    _Error = "[Čas připomenutí] musí být menší než [Začátek]." : Return False
                End If
            End If

            If .j02ID_Owner = 0 Then .j02ID_Owner = _cUser.j02ID
        End With

        If _cDL.Save(cRec, lisO20, lisO19) Then
            If cRec.PID = 0 Then
                Me.RaiseAppEvent(BO.x45IDEnum.o22_new, _LastSavedPID)
            Else
                Me.RaiseAppEvent(BO.x45IDEnum.o22_update, _LastSavedPID)
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Load(intPID As Integer) As BO.o22Milestone Implements Io22MilestoneBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadMyLastCreated() As BO.o22Milestone Implements Io22MilestoneBL.LoadMyLastCreated
        Return _cDL.LoadMyLastCreated()
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Io22MilestoneBL.Delete
        Dim s As String = Me.Factory.GetRecordCaption(BO.x29IdEnum.o22Milestone, intPID)
        If _cDL.Delete(intPID) Then
            Me.RaiseAppEvent(BO.x45IDEnum.o22_delete, intPID, s)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(mq As BO.myQueryO22) As IEnumerable(Of BO.o22Milestone) Implements Io22MilestoneBL.GetList
        Return _cDL.GetList(mq)
    End Function
    Public Function GetList_o20(intPID As Integer) As IEnumerable(Of BO.o20Milestone_Receiver) Implements Io22MilestoneBL.GetList_o20
        Return _cDL.GetList_o20(intPID)
    End Function
    Public Function GetList_o19(intPID As Integer) As IEnumerable(Of BO.o19Milestone_NonPerson) Implements Io22MilestoneBL.GetList_o19
        Return _cDL.GetList_o19(intPID)
    End Function
    
    Public Sub Handle_Reminder() Implements Io22MilestoneBL.Handle_Reminder
        Dim d1 As Date = DateAdd(DateInterval.Day, -2, Now)
        Dim d2 As Date = Now
        Dim lisO22 As IEnumerable(Of BO.o22Milestone) = _cDL.GetList_WaitingOnReminder(d1, d2)
        For Each cRec In lisO22
            Me.RaiseAppEvent(BO.x45IDEnum.o22_remind, cRec.PID, cRec.NameWithDate)

        Next

    End Sub
    Public Function GetList_forMessagesDashboard(intJ02ID As Integer) As IEnumerable(Of BO.o22Milestone) Implements Io22MilestoneBL.GetList_forMessagesDashboard
        Return _cDL.GetList_forMessagesDashboard(intJ02ID)
    End Function
End Class
