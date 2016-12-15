Imports Telerik.Web.UI
Public Class p41_menu
    Inherits System.Web.UI.UserControl
    Public Property Factory As BL.Factory
    Public Property DataPID As Integer

    Public Property CurrentTab As String
        Get
            If tabs1.Tabs.Count = 0 Then Return ""
            If tabs1.SelectedTab Is Nothing Then
                tabs1.SelectedIndex = 0
            End If
            Return tabs1.SelectedTab.Value
        End Get
        Set(value As String)
            If tabs1.FindTabByValue(value) Is Nothing Then Return
            tabs1.FindTabByValue(value).Selected = True
        End Set
    End Property
    Public Property TabSkin As String
        Get
            Return tabs1.Skin
        End Get
        Set(value As String)
            If value = "" Then value = "Default"
            tabs1.Skin = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.hidParentWidth.Value = BO.BAS.IsNullInt(Request.Item("parentWidth")).ToString
            If Me.Factory.SysUser.OneProjectPage <> "" Then
                Server.Transfer(basUI.AddQuerystring2Page(Me.Factory.SysUser.OneProjectPage, "pid=" & Me.DataPID.ToString))
            End If
            If Request.Item("tab") <> "" Then
                Me.Factory.j03UserBL.SetUserParam("p41_framework_detail-tab", Request.Item("tab"))
            End If
        End If
        
    End Sub

    Public Sub RefreshRecord(cRec As BO.p41Project, cRecSum As BO.p41ProjectSum, strTabValue As String)
        If cRec Is Nothing Then Return
        Me.DataPID = cRec.PID

        Dim cP42 As BO.p42ProjectType = Me.Factory.p42ProjectTypeBL.Load(cRec.p42ID)
        SetupTabs(cRecSum, cP42)
        Me.CurrentTab = strTabValue
        Handle_Permissions(cRec, cP42)

        Dim cClient As BO.p28Contact = Nothing

        If cP42.p42IsModule_p31 Then
            If Not (cRec.IsClosed Or cRec.p41IsDraft) Then
                Dim lisP34 As IEnumerable(Of BO.p34ActivityGroup) = Me.Factory.p34ActivityGroupBL.GetList_WorksheetEntryInProject(cRec.PID, cRec.p42ID, cRec.j18ID, Me.Factory.SysUser.j02ID)
                With menu1.FindItemByValue("p31")
                    For Each c In lisP34
                        Dim mi As New Telerik.Web.UI.RadMenuItem(String.Format(Resources.p41_framework_detail.ZapsatUkonDo, c.p34Name), "javascript:p31_entry_menu(" & c.PID.ToString & ")")
                        mi.ImageUrl = "Images/worksheet.png"
                        .Items.Add(mi)
                    Next
                    If lisP34.Count = 0 Then
                        Dim mi As New Telerik.Web.UI.RadMenuItem(Resources.p41_framework_detail.NedisponujeteOpravnenimZapisovat)
                        mi.ForeColor = Drawing.Color.Red
                        menu1.FindItemByValue("p31").Items.Add(mi)
                    End If
                End With
            Else
                Dim mi As New Telerik.Web.UI.RadMenuItem("V projektu nemůžete zapisovat worksheet úkony.")
                If cRec.IsClosed Then
                    menu1.Skin = "Black"
                    mi.Text = Resources.p41_framework_detail.VArchivuNelzeWorksheet
                End If
                If cRec.p41IsDraft Then mi.Text = Resources.p41_framework_detail.VDraftNelzeWorksheet
                mi.ForeColor = Drawing.Color.Red
                menu1.FindItemByValue("p31").Items.Add(mi)
            End If
        Else
            hmi("p31", False)
        End If



        basUIMT.RenderHeaderMenu(cRec.IsClosed, Me.panMenuContainer, menu1)
        Dim strLevel1 As String = cRec.FullName
        If Len(cRec.Client) > 30 Then
            strLevel1 = Left(cRec.Client, 30) & "..."
            If cRec.p41NameShort <> "" Then
                strLevel1 += " - " & cRec.p41NameShort
            Else
                strLevel1 += " - " & cRec.p41Name
            End If
        End If
        basUIMT.RenderLevelLink(menu1.FindItemByValue("level1"), strLevel1, "p41_framework_detail.aspx?pid=" & cRec.PID.ToString, cRec.IsClosed)



    End Sub

    Private Sub Handle_Permissions(cRec As BO.p41Project, cP42 As BO.p42ProjectType)
        menu1.FindItemByValue("cmdX40").NavigateUrl = "x40_framework.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString

        Dim cDisp As BO.p41RecordDisposition = Me.Factory.p41ProjectBL.InhaleRecordDisposition(cRec)
        menu1.FindItemByValue("cmdLog").Visible = cDisp.OwnerAccess

        With Me.Factory
            menu1.FindItemByValue("cmdNew").Visible = .TestPermission(BO.x53PermValEnum.GR_P41_Creator, BO.x53PermValEnum.GR_P41_Draft_Creator)
            menu1.FindItemByValue("cmdCopy").Visible = menu1.FindItemByValue("cmdNew").Visible
            menu1.FindItemByValue("cmdNewChild").Visible = menu1.FindItemByValue("cmdNew").Visible
            If cP42.p42IsModule_p31 Then
                menu1.FindItemByValue("cmdPivot").Visible = .TestPermission(BO.x53PermValEnum.GR_P31_Pivot)
                menu1.FindItemByValue("cmdPivot").NavigateUrl = "p31_pivot.aspx?masterprefix=p41&masterpid=" & cRec.PID.ToString
            Else
                hmi("cmdPivot", False)
            End If

            If cP42.p42IsModule_o23 Then menu1.FindItemByValue("cmdO23").Visible = .TestPermission(BO.x53PermValEnum.GR_O23_Creator) Else hmi("cmdO23", False)
            If cP42.p42IsModule_o22 Then menu1.FindItemByValue("cmdO22").Visible = .TestPermission(BO.x53PermValEnum.GR_O22_Creator) Else hmi("cmdO22", False)
            menu1.FindItemByValue("calendarO22").Visible = menu1.FindItemByValue("cmdO22").Visible

            If cRec.b01ID <> 0 Then menu1.FindItemByValue("cmdB07").Visible = False

            If cP42.p42IsModule_p31 Then
                Dim bolCanApproveOrInvoice As Boolean = .TestPermission(BO.x53PermValEnum.GR_P31_Approver, BO.x53PermValEnum.GR_P91_Creator)
                If Not bolCanApproveOrInvoice Then bolCanApproveOrInvoice = .TestPermission(BO.x53PermValEnum.GR_P91_Draft_Creator)
                If bolCanApproveOrInvoice = False And cDisp.x67IDs.Count > 0 Then
                    Dim lisO28 As IEnumerable(Of BO.o28ProjectRole_Workload) = Me.Factory.x67EntityRoleBL.GetList_o28(cDisp.x67IDs)
                    If lisO28.Where(Function(p) p.o28PermFlag = BO.o28PermFlagENUM.CistASchvalovatVProjektu Or p.o28PermFlag = BO.o28PermFlagENUM.CistAEditASchvalovatVProjektu).Count > 0 Then
                        bolCanApproveOrInvoice = True
                    End If
                End If
                menu1.FindItemByValue("cmdApprove").Visible = bolCanApproveOrInvoice
                Me.hidIsCanApprove.Value = BO.BAS.GB(bolCanApproveOrInvoice)
            Else
                hmi("cmdApprove", False)
            End If
            hmi("cmdP48", cP42.p42IsModule_p48)
            ''aP48.Visible = cP42.p42IsModule_p48
        End With
        With cDisp
            If cP42.p42IsModule_p56 Then menu1.FindItemByValue("cmdP56").Visible = .P56_Create Else hmi("cmdP56", False)

            If cP42.p42IsModule_p31 Then
                menu1.FindItemByValue("cmdP31Recalc").Visible = .P31_RecalcRates
                menu1.FindItemByValue("cmdP31Move2Bin").Visible = .P31_Move2Bin
                menu1.FindItemByValue("cmdP31MoveToOtherProject").Visible = .P31_MoveToOtherProject
                menu1.FindItemByValue("cmdP40Create").Visible = .OwnerAccess
            Else
                hmi("cmdP31Recalc", False) : hmi("cmdP31Move2Bin", False) : hmi("cmdP31MoveToOtherProject", False) : hmi("cmdP40Create", False)
            End If


            menu1.FindItemByValue("cmdEdit").Visible = .OwnerAccess
            menu1.FindItemByValue("cmdP30").Visible = .OwnerAccess

            If Not .p91_Read Then RemoveTab("p91")

            If Not .p45_Read Then RemoveTab("p45")

        End With

        If Not (menu1.FindItemByValue("cmdEdit").Visible Or menu1.FindItemByValue("cmdNew").Visible) Then
            Try
                menu1.Items.Remove(menu1.FindItemByValue("record"))
            Catch ex As Exception

            End Try

        End If
     

        If cRec.IsClosed Then menu1.FindItemByValue("cmdO22").Visible = False : menu1.FindItemByValue("cmdP40Create").Visible = False : menu1.FindItemByValue("cmdP56").Visible = False 'projekt je v archivu
        If cRec.IsClosed Then hmi("cmdO22", False) : hmi("cmdP40Create", False) : hmi("cmdP56", False) 'projekt je v archivu

    End Sub
   

    Private Sub hmi(strMenuValue As String, bolVisible As Boolean)
        Dim mi As RadMenuItem = menu1.FindItemByValue(strMenuValue)
        If mi Is Nothing Then Return
        mi.Visible = bolVisible


    End Sub


    Private Sub cti(strName As String, strX61Code As String)
        Dim cX61 As New BO.x61PageTab
        cX61.x61Code = strX61Code
        Dim tab As New RadTab(strName, strX61Code)
        tabs1.Tabs.Add(tab)
        If strX61Code = "rec" Then
            Return
        End If
        tab.NavigateUrl = cX61.GetPageUrl("p41", Me.DataPID, Me.hidIsCanApprove.Value)
        tab.NavigateUrl += "&lasttabkey=p41_framework_detail-tab&lasttabval=" & strX61Code
        If tabs1.Tabs.Count = 0 Then tab.Selected = True
    End Sub
    Private Sub SetupTabs(crs As BO.p41ProjectSum, cP42 As BO.p42ProjectType)
        tabs1.Tabs.Clear()
        Dim s As String = ""
        If Me.hidPOS.Value = "1" Then
            ''panSwitch.Style.Item("position") = "absolute"
            ''panSwitch.Style.Item("top") = "100px"
            cti("Projekt", "board")
        End If
        If cP42.p42IsModule_p31 Then
            s = "Summary" : cti(s, "summary")
            s = "Worksheet" : cti(s, "p31")
            s = "Hodiny"
            If crs.p31_Wip_Time_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Time_Count.ToString & "</span>"
            If crs.p31_Approved_Time_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Time_Count.ToString & "</span>"
            cti(s, "time")
            s = "Výdaje"
            If crs.p31_Wip_Expense_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Expense_Count.ToString & "</span>"
            If crs.p31_Approved_Expense_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Expense_Count.ToString & "</span>"
            cti(s, "expense")
            s = "Odměny"
            If crs.p31_Wip_Fee_Count > 0 Then s += "<span class='badge1wip'>" & crs.p31_Wip_Fee_Count.ToString & "</span>"
            If crs.p31_Approved_Fee_Count > 0 Then s += "<span class='badge1approved'>" & crs.p31_Approved_Fee_Count.ToString & "</span>"
            cti(s, "fee")
            s = "Faktury"
            If crs.p91_Count > 0 Then s += "<span class='badge1'>" & crs.p91_Count.ToString & "</span>"
            cti(s, "p91")
        End If
        If crs.childs_Count > 0 Then
            s = "Pod-projekty<span class='badge1'>" & crs.childs_Count.ToString & "</span>"
            cti(s, "p41")
        End If
        If cP42.p42IsModule_p56 Then
            s = "Úkoly"
            If crs.p56_Actual_Count > 0 Then s += "<span class='badge1'>" & crs.p56_Actual_Count.ToString & "</span>"
            cti(s, "p56")
        End If
        If cP42.p42IsModule_p45 Then
            s = "Rozpočet"
            If crs.p45_Count > 0 Then s += "<span class='badge1'>" & crs.p45_Count.ToString & "</span>"
            cti(s, "p45")
        End If
        If cP42.p42IsModule_o23 And cP42.p42SubgridO23Flag = 1 Then
            s = "Dokumenty"
            If crs.o23_Count > 0 Then s += "<span class='badge1'>" & crs.o23_Count.ToString & "</span>"
            cti(s, "o23")
        End If
        ''If crs.b07_Count > 0 Then
        ''    s = "Komentáře a workflow<span class='badge1'>" & crs.b07_Count.ToString & "</span>"
        ''    cti(s, "workflow")
        ''End If

    End Sub





    Private Sub RemoveTab(strTabValue As String)
        With tabs1.Tabs
            If Not .FindTabByValue(strTabValue) Is Nothing Then
                .Remove(.FindTabByValue(strTabValue))
                If .Count > 0 Then tabs1.SelectedIndex = 0

            End If
        End With
    End Sub
End Class