﻿Public Class entity_framework_rec_p91
    Inherits System.Web.UI.Page
    Protected WithEvents _MasterPage As SubForm

    Public Property CurrentMasterPrefix As String
        Get
            Return hidMasterPrefix.Value
        End Get
        Set(value As String)
            hidMasterPrefix.Value = value
        End Set
    End Property

    Private Sub entity_framework_rec_p91_Init(sender As Object, e As EventArgs) Handles Me.Init
        _MasterPage = Me.Master
        menu1.Factory = Master.Factory
        gridP91.Factory = Master.Factory
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            With Master
                .DataPID = BO.BAS.IsNullInt(Request.Item("masterpid"))
                Me.CurrentMasterPrefix = Request.Item("masterprefix")
                If .DataPID = 0 Or Me.CurrentMasterPrefix = "" Then .StopPage("masterpid or masterprefix is missing")

                .SiteMenuValue = Me.CurrentMasterPrefix
                menu1.DataPrefix = Me.CurrentMasterPrefix
                If Me.menu1.PageSource = "2" Then
                    .IsHideAllRecZooms = True
                End If
                Dim lisPars As New List(Of String)
                With lisPars
                    .Add(Me.CurrentMasterPrefix & "_menu-tabskin")
                    .Add(Me.CurrentMasterPrefix & "_menu-x31id-plugin")
                End With
                With .Factory.j03UserBL
                    .InhaleUserParams(lisPars)
                    menu1.TabSkin = .GetUserParam(Me.CurrentMasterPrefix & "_menu-tabskin")
                    menu1.x31ID_Plugin = .GetUserParam(Me.CurrentMasterPrefix & "_menu-x31id-plugin")
                End With
            End With


        End If
        RefreshRecord()
        menu1.DataPID = Master.DataPID
        gridP91.x29ID = BO.BAS.GetX29FromPrefix(Me.CurrentMasterPrefix)
        gridP91.MasterDataPID = Master.DataPID
    End Sub

    Private Sub RefreshRecord()
        Select Case Me.CurrentMasterPrefix
            Case "p41"
                Dim cRec As BO.p41Project = Master.Factory.p41ProjectBL.Load(Master.DataPID)
                Dim cP42 As BO.p42ProjectType = Master.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
                Dim cRecSum As BO.p41ProjectSum = Master.Factory.p41ProjectBL.LoadSumRow(cRec.PID)

                menu1.p41_RefreshRecord(cRec, cRecSum, "p91")
            Case "p28"
                Dim cRec As BO.p28Contact = Master.Factory.p28ContactBL.Load(Master.DataPID)
                Dim cRecSum As BO.p28ContactSum = Master.Factory.p28ContactBL.LoadSumRow(cRec.PID)
                menu1.p28_RefreshRecord(cRec, cRecSum, "p91")
            Case "j02"
                Dim cRec As BO.j02Person = Master.Factory.j02PersonBL.Load(Master.DataPID)
                Dim cRecSum As BO.j02PersonSum = Master.Factory.j02PersonBL.LoadSumRow(cRec.PID)
                menu1.j02_RefreshRecord(cRec, cRecSum, "p91")
            Case "p56"
                Dim cRec As BO.p56Task = Master.Factory.p56TaskBL.Load(Master.DataPID)
                Dim cP41 As BO.p41Project = Master.Factory.p41ProjectBL.Load(cRec.p41ID)
                Dim cRecSum As BO.p56TaskSum = Master.Factory.p56TaskBL.LoadSumRow(Master.DataPID)
                menu1.p56_RefreshRecord(cRec, cRecSum, cP41, "p91")
        End Select

    End Sub

    
End Class