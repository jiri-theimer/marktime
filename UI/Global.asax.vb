﻿Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication
    Private Const CacheItemKey As String = "MARKTIME50_TIMER"
    Private _onRemove As System.Web.Caching.CacheItemRemovedCallback
    
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        log4net.Config.XmlConfigurator.Configure()
        Dim factory As New BL.Factory(Nothing)
        Dim strRobotHost As String = factory.x35GlobalParam.GetValueString("robot_host")        
        'Dim strWakeHost As String = BO.ASS.GetConfigVal("wakeup_host")
        If strRobotHost <> "" Then
            Dim intCacheTimeOut As Integer = factory.x35GlobalParam.GetValueInteger("robot_cache_timeout", 300)
            RegisterCacheEntry(strRobotHost, intCacheTimeOut)
        End If

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        'Dim strWakeHost As String = BO.ASS.GetConfigVal("LocalAppUrl")  'přednost má URL definované ve web.config
        'If strWakeHost = "" Then
        '    'pokud ve web.config není definován klíč LocalAppUrl, potom se bere aktuální URL
        '    strWakeHost = Context.Request.Url.GetLeftPart(UriPartial.Authority)
        'End If
        'Dim strWakeupURL As String = strWakeHost & "/Public/wakeup_engine.aspx"
        
        If Context.Request.Url.PathAndQuery.IndexOf("robot.aspx") > 0 Then
            'je spuštěna robot stránka nebo ještě nebyla incializovaná cache
            'Dim strRobotHost As String = Context.Request.Url.GetLeftPart(UriPartial.Authority)

            Dim factory As New BL.Factory(Nothing)
            Dim strRobotHost As String = factory.x35GlobalParam.GetValueString("robot_host")
            If strRobotHost <> "" Then
                Dim intCacheTimeOut As Integer = factory.x35GlobalParam.GetValueInteger("robot_cache_timeout", 300)
                RegisterCacheEntry(strRobotHost, intCacheTimeOut)
                factory.x35GlobalParam.UpdateValue("robot_cache_lastrequest", Format(Now, "dd.MM.yyyy HH:mm:ss"))
            End If

        End If

        'If LCase(HttpContext.Current.Request.Url.ToString) = LCase(strWakeupURL) Then RegisterCacheEntry()
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    Private Function RegisterCacheEntry(strHostURL As String, intCacheTimeout As Integer) As Boolean

        If Not HttpContext.Current.Cache(CacheItemKey) Is Nothing Then Return False

        _onRemove = New CacheItemRemovedCallback(AddressOf Me.CacheItemRemovedCallback)

        HttpContext.Current.Cache.Add(CacheItemKey, strHostURL, Nothing, Now.AddSeconds(intCacheTimeout), Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, _onRemove)

        Return True
    End Function

    Public Sub CacheItemRemovedCallback(ByVal key As String, ByVal value As Object, ByVal reason As CacheItemRemovedReason)
        HitPage(value)

    End Sub

    Private Sub HitPage(strHostURL As String)
        If strHostURL = "" Then Return

        Dim client As New System.Net.WebClient

        client.DownloadString(strHostURL & "/Public/robot.aspx")

        client.Dispose()
    End Sub
End Class