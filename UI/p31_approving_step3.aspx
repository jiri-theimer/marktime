<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_approving_step3.aspx.vb" Inherits="UI.p31_approving_step3" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="billingmemo" Src="~/billingmemo.ascx" %>
<%@ Register TagPrefix="uc" TagName="mygrid" Src="~/mygrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {


            $(".slidingDiv2").hide();





            $('.show_hide2').click(function () {



                $(".slidingDiv2").slideToggle();
            });




        });



        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;

            self.document.getElementById("<%=fraSubform.ClientID%>").src = "p31_approving_step3_subform.aspx?guid=<%=ViewState("guid")%>&pid=" + pid;

        }

        function RowDoubleClick(sender, args) {
            //nic
        }

        function GetAllSelectedPIDs() {

            var masterTable = $find("<%=grid1.radGridOrig.ClientID%>").get_masterTableView();
            var sel = masterTable.get_selectedItems();
            var pids = "";

            for (i = 0; i < sel.length; i++) {
                if (pids == "")
                    pids = sel[i].getDataKeyValue("pid");
                else
                    pids = pids + "," + sel[i].getDataKeyValue("pid");
            }

            return (pids);
        }

        function record_clone() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_orig("p31_record.aspx?clone=1&pid=" + pid);

        }

        function hardrefresh(pid, flag) {

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

     

        function batch_p31text() {

            //dialog_master("p31_approving_batch_p31text.aspx?guid=<%=viewstate("guid")%>", true);
            location.replace("p31_approving_batch_p31text.aspx?guid=<%=viewstate("guid")%>");

        }

        function p31_create(field, pid) {
            dialog_master("p31_record.aspx?" + field + "=" + pid + "&pid=0&guid_approve=<%=viewstate("guid")%>", false, 800, 600);
        }


        function sw_orig(url, is_maximize, window_width, window_height) {
            var wnd = $find("<%=okno1.ClientID%>");
            wnd.setUrl(url);
            if (window_width != null)
                wnd.setSize(window_width, window_height);

            wnd.show();
            if (is_maximize == true) {
                wnd.maximize();
            }
            else {
                wnd.center();
            }

        }

        function o23_record(pid) {

            dialog_master("o23_record.aspx?billing=1&masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&pid=" + pid, true);

        }

        function report() {

            dialog_master("report_modal.aspx?prefix=app&guid=<%=viewstate("guid")%>", true);
        }
        function tags() {
            var pids = GetAllSelectedPIDs();
            if (pids == "" || pids == null) {
                $.alert("Není vybrán záznam.");
                return
            }
            dialog_master("tag_binding.aspx?prefix=p31&pids=" + pids, "Images/tag.png");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="slidingDiv2" style="padding: 10px;">
        <div class="innerform_light">
            <div class="div6">
                <uc:mygrid id="designer1" runat="server" prefix="p31" x36key="p31_approving_step3-j70id" masterprefix="approving_step3" MasterPrefixFlag="2" reloadurl="javascript:hardrefresh(0, 'j70')" Width="250px" ModeFlag="3"></uc:mygrid>
            </div>
            <div class="div6">
                <asp:CheckBox ID="chkAutoFilter" runat="server" Text="Nabízet nad sloupci filtrování dat" AutoPostBack="true" CssClass="chk" />
                <asp:CheckBox ID="chkUseInternalApproving" runat="server" Text="Využívat i interní (vnitropodnikové) schvalování" AutoPostBack="true" CssClass="chk" />
                <asp:CheckBox ID="chkDefaultApproveSetup" runat="server" Text="U rozpracovaných úkonů nahazovat výchozí fakturační status" AutoPostBack="true" CssClass="chk" />
            </div>
            <fieldset>
                <legend>Souhrny</legend>
                <div class="div6">
                    <asp:RadioButtonList ID="opgGroupBy" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                        <asp:ListItem Text="Bez souhrnů" Value="" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="Fakturační oddíl" Value="p95Name"></asp:ListItem>
                        <asp:ListItem Text="Fakturační status" Value="approve_p72Name"></asp:ListItem>
                        <asp:ListItem Text="Datum úkonu" Value="p31Date"></asp:ListItem>
                        <asp:ListItem Text="Sešit" Value="p34Name"></asp:ListItem>
                        <asp:ListItem Text="Aktivita" Value="p32Name"></asp:ListItem>
                        <asp:ListItem Text="Osoba" Value="Person"></asp:ListItem>
                        <asp:ListItem Text="Projekt" Value="p41Name"></asp:ListItem>
                        <asp:ListItem Text="Klient" Value="ClientName"></asp:ListItem>
                        <asp:ListItem Text="Úkol" Value="p56Name"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </fieldset>
        </div>
    </div>


    <div style="height: 60px; width: 100%;">
        <table cellpadding="5" cellspacing="2">
            <tr>
                <td>Počet úkonů:
                </td>
                <td align="right">
                    <asp:Label ID="RowCount" runat="server" CssClass="valbold"></asp:Label>
                </td>
                <td>Vykázaný čas (fakturovatelný):
                </td>
                <td align="right">
                    <asp:Label ID="hours_billable_orig" runat="server" CssClass="valbold"></asp:Label>h.
                <asp:Label ID="fee_billable_orig" runat="server" CssClass="valboldblue"></asp:Label>
                </td>
                <td>
                    <img src='Images/a14.gif' />Fakturovat čas:
                </td>
                <td align="right">
                    <asp:Label ID="hours_4" runat="server" CssClass="valbold"></asp:Label>h.
                <asp:Label ID="fee_4" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                </td>
                <td>
                    <asp:Image ID="imgProfitLost_Time" runat="server" runat="server" />
                    <asp:Label ID="profit_lost_time" runat="server" CssClass="valbold" BackColor="Yellow"></asp:Label>
                </td>
                <td>
                    <img src='Images/a12.gif' />Viditelný odpis:
                </td>
                <td align="right">
                    <asp:Label ID="hours_2" runat="server" CssClass="valbold"></asp:Label>h.
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Schválené úkony:
                </td>
                <td>
                    <asp:Label ID="RowsCount_Approved" runat="server" CssClass="valbold"></asp:Label>
                </td>
                <td>Ostatní vykázané příjmy:
                </td>
                <td align="right">
                    <asp:Label ID="other_income_orig" runat="server" CssClass="valboldblue"></asp:Label>
                </td>
                <td>
                    <img src='Images/a14.gif' />Fakturovat ostatní:
                </td>
                <td align="right">
                    <asp:Label ID="other_income_approved" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                </td>
                <td>
                    <asp:Image ID="imgProfitLost_Other" runat="server" runat="server" />
                    <asp:Label ID="profit_lost_other" runat="server" CssClass="valbold" BackColor="Yellow"></asp:Label>
                </td>
                <td>
                    <img src='Images/a13.gif' />Skrytý odpis:
                </td>
                <td align="right">
                    <asp:Label ID="hours_3" runat="server" CssClass="valbold"></asp:Label>h.
                </td>
                <td>
                    <img src='Images/a16.gif' />Zahrnout do paušálu:
                </td>
                <td align="right">
                    <asp:Label ID="hours_6" runat="server" CssClass="valbold"></asp:Label>h.
                </td>
                <td></td>
            </tr>
        </table>
    </div>

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
    <div style="clear: both;"></div>


    <table width="100%" cellpadding="0" cellspacing="0">
        <tr valign="top">
            <td style="min-width: 400px; min-height: 450px;">
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
                    <Tabs>
                        <telerik:RadTab Text="Aktuální záznam" Selected="true" Value="one"></telerik:RadTab>
                        <telerik:RadTab Text="Hromadné operace" Value="selected"></telerik:RadTab>
                        <telerik:RadTab Text="Fakt.poznámka" Value="memo"></telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
                    <telerik:RadPageView ID="one" runat="server" Selected="true">


                        <iframe id="fraSubform" runat="server" width="100%" height="460px" frameborder="0" src=""></iframe>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="selected" runat="server">
                        <p></p>
                        <div class="content-box1">
                            <div class="title" style="text-align:center;">Operace nad vybranými (zaškrtlými) záznamy</div>
                            <div class="content">
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_4" Text="[Fakturovat]" runat="server" CssClass="cmd" Width="280px" />
                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_Clear" Text="Vyčistit schvalování - vrátit na [Nerozhodnuto]" runat="server" CssClass="cmd" Width="280px" />
                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_6" Text="[Zahrnout do paušálu]" runat="server" CssClass="cmd" Width="280px" />

                                </div>
                                <div style="display: none;">
                                    <asp:Button ID="cmdBatch_ApprovingSet" Text="Vybrané záznamy zařadit do billing dávky:" runat="server" CssClass="cmd" Width="280px" Visible="false" />
                                    <telerik:RadComboBox ID="p31ApprovingSet" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="200px" AllowCustomText="true" ToolTip="Název billing dávky" Visible="false"></telerik:RadComboBox>
                                    <asp:Button ID="cmdBatch_ApprovingSet_Clear" Text="Vybraným vyčistit přiřazení billing dávky" runat="server" CssClass="cmd" Width="280px" Visible="false" />
                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_3" Text="[Skrytý odpis]" runat="server" CssClass="cmd" Width="280px" />


                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_2" Text="[Viditelný odpis]" runat="server" CssClass="cmd" Width="280px" />


                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_7" Text="[Fakturovat později]" runat="server" CssClass="cmd" Width="280px" />

                                </div>
                                <div class="div6">
                                    <button id="cmdTags" type="button" onclick="tags()" style="width:280px">Oštítkovat</button>
                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_8" Text="Nahodit úroveň schvalování #0" runat="server" CssClass="cmd" Width="280px" />
                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_9" Text="Nahodit úroveň schvalování #1" runat="server" CssClass="cmd" Width="280px" />
                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_10" Text="Nahodit úroveň schvalování #2" runat="server" CssClass="cmd" Width="280px" />
                                </div>
                            </div>
                        </div>

                    </telerik:RadPageView>
                    <telerik:RadPageView ID="memo" runat="server">
                        <uc:billingmemo ID="bm1" runat="server" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>

            </td>
            <td>
                <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>


            </td>

        </tr>
    </table>



    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />

    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
    <asp:HiddenField ID="hidApprovingLevel" runat="server" />

    <telerik:RadWindow ID="okno1" runat="server" Modal="true" KeepInScreenBounds="true" VisibleTitlebar="true" VisibleStatusbar="false" Skin="WebBlue" ShowContentDuringLoad="false" Width="800px" Height="600px" Behaviors="Close,Move,Maximize" IconUrl="Images/window.png" Style="z-index: 9900;">
        <Shortcuts>
            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
        </Shortcuts>
    </telerik:RadWindow>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
