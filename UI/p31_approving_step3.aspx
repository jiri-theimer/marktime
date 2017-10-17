﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_approving_step3.aspx.vb" Inherits="UI.p31_approving_step3" %>

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



            var h1 = new Number;
            var h2 = new Number;
            var h3 = new Number;

            h1 = $(window).height();

            var ss = self.document.getElementById("<%=RadMultiPage1.ClientID%>");
            var offset = $(ss).offset();

            h2 = offset.top;

            h3 = h1 - h2 - 10;


            document.getElementById("<%=RadMultiPage1.ClientID%>").style.height = h3 + "px";




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
        function SaveAsSet() {
            var s = window.prompt("Zadejte název billing dávky.");

            if (s != '' && s != null) {
                self.document.getElementById("<%=hidApprovingSet_Explicit.ClientID%>").value = s;

                hardrefresh(0, "save_as_set");
            }



        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="slidingDiv2" style="padding: 10px;">
        <div class="innerform_light">
            <div class="div6">
                <uc:mygrid ID="designer1" runat="server" Prefix="p31" x36Key="p31_approving_step3-j70id" MasterPrefix="approving_step3" MasterPrefixFlag="2" ReloadUrl="javascript:hardrefresh(0, 'j70')" Width="250px" ModeFlag="3"></uc:mygrid>
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
                <td>Počet:
                </td>
                <td align="right">
                    <asp:Label ID="RowCount" runat="server" CssClass="valbold"></asp:Label>
                    x
                </td>
                <td>Vykázané fakt.hodiny:
                </td>
                <td align="right">
                    <asp:Label ID="hours_billable_orig" runat="server" CssClass="valbold"></asp:Label>h.
                <asp:Label ID="fee_billable_orig" runat="server" CssClass="valboldblue"></asp:Label>
                </td>
                <td>
                    <img src='Images/a14.gif' />Fakturovat hodiny:
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
                <td>Schváleno:
                </td>
                <td>
                    <asp:Label ID="RowsCount_Approved" runat="server" CssClass="valbold"></asp:Label>
                    x
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

    <table width="99.9%" cellpadding="0" cellspacing="0">
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
                            <div class="title" style="text-align: center;">Operace nad vybranými (zaškrtlými) záznamy</div>
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

                                <div class="div6">
                                    <asp:Button ID="cmdBatch_3" Text="[Skrytý odpis]" runat="server" CssClass="cmd" Width="280px" />


                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_2" Text="[Viditelný odpis]" runat="server" CssClass="cmd" Width="280px" />


                                </div>
                                <div class="div6">
                                    <asp:Button ID="cmdBatch_7" Text="[Fakturovat později]" runat="server" CssClass="cmd" Width="280px" />

                                </div>
                                <div class="div6" style="margin-top: 20px;">
                                    <asp:Button ID="cmdBatch_ApprovingSet" Text="Vybrané zařadit do billing dávky:" runat="server" CssClass="cmd" Width="280px" Visible="true" />
                                    <telerik:RadComboBox ID="p31ApprovingSet" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="300px" AllowCustomText="true" ToolTip="Název billing dávky"></telerik:RadComboBox>
                                    <asp:Button ID="cmdBatch_ApprovingSet_Clear" Text="Vybraným vyčistit přiřazení billing dávky" runat="server" CssClass="cmd" Width="280px" Visible="true" />
                                </div>
                                <div class="div6">
                                    <button id="cmdTags" type="button" onclick="tags()" style="width: 280px">Oštítkovat</button>
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
    <asp:HiddenField ID="hidApprovingSet_Explicit" runat="server" />



    <script type="text/javascript">
        $telerik.getViewPortSize = function () {
            var width = 0;
            var height = 0;

            var canvas = document.body;

            if ((!$telerik.quirksMode && !$telerik.isSafari) ||
                (Telerik.Web.Browser.chrome && Telerik.Web.Browser.version >= 61)) {
                canvas = document.documentElement;
            }

            if (window.innerWidth) {
                // Seems there's no completely reliable way to get the viewport size in Gecko, this should be the best one
                // Check https://bugzilla.mozilla.org/show_bug.cgi?id=189112#c7
                width = Math.max(document.documentElement.clientWidth, document.body.clientWidth);
                height = Math.max(document.documentElement.clientHeight, document.body.clientHeight);

                if (width > window.innerWidth)
                    width = document.documentElement.clientWidth;
                if (height > window.innerHeight)
                    height = document.documentElement.clientHeight;
            }
            else {
                width = canvas.clientWidth;
                height = canvas.clientHeight;
            }

            width += canvas.scrollLeft;
            height += canvas.scrollTop;

            if ($telerik.isMobileSafari) {
                width += window.pageXOffset;
                height += window.pageYOffset;
            }

            return { width: width - 6, height: height - 6 };
        }
    </script>
    <telerik:RadContextMenu ID="RadContextMenu1" runat="server" EnableViewState="false" Skin="Metro" ExpandDelay="0" RenderMode="Lightweight" OnClientHidden="RadContextMenu1_Hidden">
        <CollapseAnimation Type="None" />
        <ExpandAnimation Type="None" />
    </telerik:RadContextMenu>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
    <script type="text/javascript">
        function contMenu(url, isTopWindow) {
            sw_everywhere(url, "", true);
        }
        function contReload(url, target) {
            window.open(url, target);
        }

        var _lastCmClosedTime = null;

        function RadContextMenu1_Hidden(menu) {
            var d = new Date();
            _lastCmClosedTime = d.getTime();

        }

        function RCM(curPREFIX, curPID, ctl, strFlag) {
            var d = new Date();
            var n = d.getTime();

            var contextMenu = $find("<%= RadContextMenu1.ClientID %>");
            if (n - _lastCmClosedTime < 2) {
                return;     //menu bylo zavřeno přes link volající přímo RCM
            }

            $.ajax({
                method: "POST",
                url: "Handler/handler_popupmenu.ashx",
                beforeSend: function () {

                },
                async: true,
                timeout: 3000,
                data: { prefix: curPREFIX, pid: curPID, flag: strFlag },
                success: function (data) {
                    //alert("načítání");
                    //$('#html5menu').html('');

                    contextMenu.get_items().clear();

                    var x = 180
                    var y = 30

                    if (ctl != null) {
                        x = $(ctl).offset().left;
                        y = $(ctl).offset().top;
                    }


                    var miLastRoot = null;

                    for (var i in data) {
                        var c = data[i];
                        var mi = new Telerik.Web.UI.RadMenuItem();

                        if (c.IsSeparator == true) {
                            mi.set_isSeparator(true);
                        }

                        if (c.IsSeparator == false) {
                            mi.set_text(c.Text);

                            mi.set_navigateUrl(c.NavigateUrl);

                        }

                        if (c.ImageUrl != "") {
                            mi.set_imageUrl(c.ImageUrl);
                        }

                        if (c.IsDisabled == true) {
                            mi.disable();
                        }

                        if (c.IsChildOfPrevious == true) {
                            miLastRoot.get_items().add(mi);
                        }

                        if (c.IsChildOfPrevious == false) {
                            contextMenu.get_items().add(mi);
                            miLastRoot = mi;
                        }


                    }

                    contextMenu.showAt(x + 20, y);



                },
                complete: function () {
                    // do the job here

                }
            });

            ;

        }
    </script>
</asp:Content>
