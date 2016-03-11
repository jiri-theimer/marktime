﻿Public Interface Ip30Contact_PersonBL
    Inherits IFMother
    Function Save(cRec As BO.p30Contact_Person) As Boolean
    Function Load(intPID As Integer) As BO.p30Contact_Person
    Function Delete(intPID As Integer) As Boolean
    Function GetList(intP28ID As Integer, intP41ID As Integer, intJ02ID As Integer) As IEnumerable(Of BO.p30Contact_Person)
    Function GetList_J02(intP28ID As Integer, intP41ID As Integer, bolIncludeClientProjects As Boolean) As IEnumerable(Of BO.j02Person)
End Interface

Class p30Contact_PersonBL
    Inherits BLMother
    Implements Ip30Contact_PersonBL
    Private WithEvents _cDL As DL.p30Contact_PersonDL


    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p30Contact_PersonDL(ServiceUser)
        _cUser = ServiceUser
    End Sub
    Public Function Save(cRec As BO.p30Contact_Person) As Boolean Implements Ip30Contact_PersonBL.Save
        With cRec
            If .j02ID = 0 Then _Error = "Chybí vazba na osobu." : Return False
            If GetList(.p28ID, .p41ID, 0).Where(Function(p) p.j02ID = .j02ID).Count > 0 Then
                If .p28ID <> 0 Then
                    _Error = "Klient již má s touto osobou vazbu." : Return False
                Else
                    _Error = "Projekt již má s touto osobou vazbu." : Return False
                End If
            End If
        End With

        If _cDL.Save(cRec) Then
            If cRec.PID = 0 Then
                Me.RaiseAppEvent(BO.x45IDEnum.p30_new, _LastSavedPID)
            Else
                Me.RaiseAppEvent(BO.x45IDEnum.p30_new, _LastSavedPID)
            End If
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Load(intPID As Integer) As BO.p30Contact_Person Implements Ip30Contact_PersonBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function Delete(intPID As Integer) As Boolean Implements Ip30Contact_PersonBL.Delete
        Dim cRec As BO.p30Contact_Person = Load(intPID)
        Dim s As String = cRec.FullNameDesc
        If cRec.p28ID <> 0 Then
            s += " | " & Me.Factory.GetRecordCaption(BO.x29IdEnum.p28Contact, cRec.p28ID, True)
        End If
        If cRec.p41ID <> 0 Then
            s += " | " & Me.Factory.GetRecordCaption(BO.x29IdEnum.p41Project, cRec.p41ID, True)
        End If
        If _cDL.Delete(intPID) Then
            Me.RaiseAppEvent(BO.x45IDEnum.p30_delete, intPID, s)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetList(intP28ID As Integer, intP41ID As Integer, intJ02ID As Integer) As IEnumerable(Of BO.p30Contact_Person) Implements Ip30Contact_PersonBL.GetList
        Return _cDL.GetList(intP28ID, intP41ID, intJ02ID)
    End Function
    Public Function GetList_J02(intP28ID As Integer, intP41ID As Integer, bolIncludeClientProjects As Boolean) As IEnumerable(Of BO.j02Person) Implements Ip30Contact_PersonBL.GetList_J02
        Return _cDL.GetList_J02(intP28ID, intP41ID, bolIncludeClientProjects)
    End Function
End Class
