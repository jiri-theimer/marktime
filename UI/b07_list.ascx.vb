﻿Public Class b07_list
    Inherits System.Web.UI.UserControl
    Private Property _intSelectedB07ID As Integer
    Private Property _lastB07ID As Integer
    Private Property _sysUser As BO.j03UserSYS

    Public Property ShowInsertButton As Boolean
        Get
            Return cmdAdd.Visible
        End Get
        Set(value As Boolean)
            cmdAdd.Visible = value
        End Set
    End Property
    Public Property AttachmentIsReadonly As Boolean
        Get
            If hidAttachmentsReadonly.Value = "1" Then Return True Else Return False
        End Get
        Set(value As Boolean)
            hidAttachmentsReadonly.Value = BO.BAS.GB(value)
        End Set
    End Property
    Public Property ShowHeader As Boolean
        Get
            Return panHeader.Visible
        End Get
        Set(value As Boolean)
            panHeader.Visible = value
        End Set
    End Property
    Public ReadOnly Property RowsCount As Integer
        Get
            Return rp1.Items.Count
        End Get
    End Property
    Public Property JS_Create As String
        Get
            Return hidJS_Create.Value
        End Get
        Set(value As String)
            hidJS_Create.Value = value
        End Set
    End Property
    Public Property JS_Reaction As String
        Get
            Return hidJS_Reaction.Value
        End Get
        Set(value As String)
            hidJS_Reaction.Value = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdAdd.Attributes.Item("onclick") = Me.JS_Create
    End Sub

    Public Sub RefreshData(factory As BL.Factory, x29id As BO.x29IdEnum, intRecordPID As Integer, Optional intSelectedB07ID As Integer = 0)
        _sysUser = factory.SysUser
        Dim mq As New BO.myQueryB07
        mq.RecordDataPID = intRecordPID
        mq.x29id = x29id
        Me.hidPrefix.Value = BO.BAS.GetDataPrefix(x29id)
        Dim lisB07 As IEnumerable(Of BO.b07Comment) = factory.b07CommentBL.GetList(mq)


        Me.hidRecordPID.Value = intRecordPID.ToString
        _intSelectedB07ID = intSelectedB07ID

        rp1.DataSource = lisB07
        rp1.DataBind()

        If cmdAdd.Visible = False And rp1.Items.Count = 0 Then
            panHeader.Visible = False
        End If

        lblHeader.Text = BO.BAS.OM2(Me.lblHeader.Text, lisB07.Count.ToString)
    End Sub


    Private Sub rp1_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rp1.ItemDataBound
        Dim cRec As BO.b07Comment = CType(e.Item.DataItem, BO.b07Comment)

        With CType(e.Item.FindControl("panRecord"), Panel)
            If cRec.b07TreeLevel > 1 Then
                .Style.Item("padding-left") = ((cRec.b07TreeLevel - 1) * 20).ToString & "px"
            End If
            If _intSelectedB07ID > 0 And cRec.PID = _intSelectedB07ID Then
                .BackColor = Drawing.Color.Orange
            End If
        End With
        With CType(e.Item.FindControl("b07Value"), Label)
            .Text = BO.BAS.CrLfText2Html(cRec.b07Value)
        End With
        With CType(e.Item.FindControl("Author"), Label)
            .Text = cRec.Author
        End With
        If cRec.Avatar <> "" Then
            With CType(e.Item.FindControl("imgPhoto"), Image)
                .ImageUrl = "Plugins/Avatar/" & cRec.Avatar
            End With
        End If
        CType(e.Item.FindControl("b07WorkflowInfo"), Label).Text = cRec.b07WorkflowInfo

        With CType(e.Item.FindControl("aAnswer"), HyperLink)
            If cRec.o43ID = 0 Then
                .NavigateUrl = "javascript:" & Me.hidJS_Reaction.Value & "(" & cRec.PID.ToString & ")"
            Else
                .Visible = False
            End If

        End With
        With CType(e.Item.FindControl("aDelete"), HyperLink)
            If (cRec.j02ID_Owner = _sysUser.j02ID Or _sysUser.IsAdmin) And (cRec.b07Value <> "" Or cRec.o27ID > 0) Then
                .Visible = True
                .NavigateUrl = "javascript:trydeleteb07(" & cRec.PID.ToString & ")"
            Else
                .Visible = False
            End If
        End With
        With CType(e.Item.FindControl("aMSG"), HyperLink)
            If cRec.o43ID = 0 Then
                .Visible = False
            Else
                .Visible = True
                .NavigateUrl = "binaryfile.aspx?format=msg&prefix=o43&pid=" & cRec.o43ID.ToString
            End If
        End With
        With CType(e.Item.FindControl("aAtts"), Label)
            If cRec.o43Attachments = "" Then
                .Visible = False
            Else
                .Visible = True
                Dim lis As List(Of String) = BO.BAS.ConvertDelimitedString2List(cRec.o43Attachments, ",")
                For Each strAtt As String In lis
                    .Text += "<a href='binaryfile.aspx?prefix=o43&pid=" & cRec.o43ID.ToString & "&disposition=inline&att=" & strAtt & "' style='margin-left:6px;'><img src='Images/attachment.png'/>" & strAtt & "</a>"
                Next
            End If
        End With
        CType(e.Item.FindControl("Timestamp"), Label).Text = BO.BAS.FD(cRec.DateInsert, True, True)
        With CType(e.Item.FindControl("att1"), HyperLink)
            If cRec.o27ID > 0 Then
                .Text = cRec.o27OriginalFileName
                '.NavigateUrl = "binaryfile.aspx?prefix=o27&disposition=inline&pid=" & cRec.o27ID.ToString
                If hidAttachmentsReadonly.Value = "1" Then
                    .NavigateUrl = ""
                Else
                    .NavigateUrl = "javascript:file_preview('o27'," & cRec.o27ID.ToString & ")"
                End If

                CType(e.Item.FindControl("img1"), Image).ImageUrl = "Images/Files/" & BO.BAS.GetFileExtensionIcon(Right(cRec.o27OriginalFileName, 4))
            Else
                .Visible = False
                e.Item.FindControl("img1").Visible = False
            End If

        End With
        If cRec.PID = _lastB07ID Then
            CType(e.Item.FindControl("b07Value"), Label).Text = ""
            e.Item.FindControl("aAnswer").Visible = False
            e.Item.FindControl("Timestamp").Visible = False
            CType(e.Item.FindControl("Author"), Label).Text = "Komentář obsahuje více příloh:"
        End If
        _lastB07ID = cRec.PID
    End Sub
End Class