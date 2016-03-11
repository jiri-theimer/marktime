Public Class b07_list
    Inherits System.Web.UI.UserControl
    Private Property _intSelectedB07ID As Integer
    Private Property _lastB07ID As Integer
    Public Property ShowInsertButton As Boolean
        Get
            Return cmdAdd.Visible
        End Get
        Set(value As Boolean)
            cmdAdd.Visible = value
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

        Dim mq As New BO.myQueryB07
        mq.RecordDataPID = intRecordPID
        mq.x29id = x29id
        Me.hidPrefix.Value = BO.BAS.GetDataPrefix(x29id)
        Dim lisB07 As IEnumerable(Of BO.b07Comment) = factory.b07CommentBL.GetList(mq)


        Me.hidRecordPID.Value = intRecordPID.ToString
        _intSelectedB07ID = intSelectedB07ID

        rp1.DataSource = lisB07
        rp1.DataBind()

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
        CType(e.Item.FindControl("b07WorkflowInfo"), Label).Text = cRec.b07WorkflowInfo

        With CType(e.Item.FindControl("aAnswer"), HyperLink)
            .NavigateUrl = "javascript:" & Me.hidJS_Reaction.Value & "(" & cRec.PID.ToString & ")"
        End With
        CType(e.Item.FindControl("Timestamp"), Label).Text = BO.BAS.FD(cRec.DateInsert, True, True)
        With CType(e.Item.FindControl("att1"), HyperLink)
            If cRec.o27ID > 0 Then
                .Text = cRec.o27OriginalFileName
                .NavigateUrl = "binaryfile.aspx?prefix=o27&pid=" & cRec.o27ID.ToString
            Else
                .Visible = False
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