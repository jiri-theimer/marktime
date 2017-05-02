﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="entity_framework.aspx.vb" Inherits="UI.entity_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var _initResizing = "1";

        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv2").hide();
                $(".slidingDiv3").hide();
                $(".slidingDiv1").slideToggle();
            });

            $(".slidingDiv2").hide();
            $(".show_hide2").show();

            $(".slidingDiv3").hide();
            $(".show_hide3").show();

            $('.show_hide2').click(function () {
                $(".slidingDiv1").hide();
                $(".slidingDiv3").hide();
                $(".slidingDiv2").slideToggle();
            });

            $('.show_hide3').click(function () {
                $(".slidingDiv1").hide();
                $(".slidingDiv2").hide();
                $(".slidingDiv3").slideToggle();
            });


            if (document.getElementById("<%=Me.hidUIFlag.ClientID%>").value == "period")
                $(".slidingDiv2").show();


            document.getElementById("<%=Me.hidUIFlag.ClientID%>").value = "";
            <%If _curIsExport = False Then%>
            _initResizing = "0";
            <%End If%>

        });


        function loadSplitter(sender) {

            var h1 = new Number;
            var h2 = new Number;
            var h3 = new Number;

            h1 = $(window).height();

            var ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            h3 = h1 - h2 - 1;

            sender.set_height(h3);

            var pane = sender.getPaneById("<%=contentPane.ClientID%>");
            document.getElementById("<%=Me.hidContentPaneWidth.ClientID%>").value = pane.get_width();
            pane.set_contentUrl(document.getElementById("<%=Me.hidContentPaneDefUrl.ClientID%>").value);

            
            <%If grid1.radGridOrig.ClientSettings.Scrolling.UseStaticHeaders Then%>
            pane = sender.getPaneById("<%=navigationPane.ClientID%>");
            <%=Me.grid1.ClientID%>_SetScrollingHeight_Explicit(pane.get_height() - 25);
            <%End If%>
            <%=me.grid1.ClientID%>_Scroll2SelectedRow(pane.get_height());

        }



        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;            
            <%If opgLayout.SelectedValue = "3" Then%>
            return;
            <%End If%>

            var splitter = $find("<%= RadSplitter1.ClientID %>");
            var pane = splitter.getPaneById("<%=contentPane.ClientID%>");

            var url = "<%=Me.CurrentPrefix%>_framework_detail.aspx?pid=" + pid + "&source=<%=opgLayout.SelectedValue%>";
            pane.set_contentUrl(url);


        }

        function RowDoubleClick(sender, args) {

            var pid = args.getDataKeyValue("pid");
            location.replace("<%=Me.CurrentPrefix%>_framework_detail.aspx?pid=" + pid + "&source=<%=opgLayout.SelectedValue%>");


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

        function SavePaneWidth(w) {
            if (_initResizing == "1") {
                return;
            }


            <%If Me.opgLayout.SelectedValue = "1" Then%>
            var keyname = "<%=Me.CurrentPrefix%>_framework-navigationPane_width";
            <%Else%>
            var keyname = "<%=Me.CurrentPrefix%>_framework-contentPane_height";
            <%End If%>

            $.post("Handler/handler_userparam.ashx", { x36value: w, x36key: keyname, oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }


            });
        }

        function AfterPaneResized(sender, args) {
            <%If Me.opgLayout.SelectedValue = "1" Then%>
            var w = sender.get_width();
            <%End If%>
            <%If Me.opgLayout.SelectedValue = "2" Then%>
            var w = sender.get_height();
            <%End If%>
            SavePaneWidth(w);

        }


        function AfterPaneCollapsed(pane) {
            var w = "-1";
            SavePaneWidth(w);
        }
        function AfterPaneExpanded(pane) {
            var w = pane.get_width();
            SavePaneWidth(w);
        }

        function hardrefresh(pid, flag) {

            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            if (flag == "p91-create" || flag == "p31-add-p91") {
                location.replace("p91_framework.aspx?pid=" + pid);
                return;
            }
            <%End If%>
            if (flag == "p31-save" || flag == "p31-delete") {
                var splitter = $find("<%= RadSplitter1.ClientID %>");
                var pane = splitter.getPaneById("<%=contentPane.ClientID%>");
                var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
                var url = "<%=Me.CurrentPrefix%>_framework_detail.aspx?pid=" + pid + "&source=<%=opgLayout.SelectedValue%>";
                pane.set_contentUrl(url);
                return;
            }

            location.replace("entity_framework.aspx?prefix=<%=Me.CurrentPrefix%>");


        }

        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            sw_master("grid_designer.aspx?nodrilldown=1&prefix=<%=Me.CurrentPrefix%>&pid=" + j74id, "Images/griddesigner.png");
        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=<%=Me.CurrentPrefix%>&pid=" + j70id, "Images/query.png");
            return (false);
        }
        function batch() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }

            $.post("Handler/handler_tempbox.ashx", { guid: "<%=Me.CurrentPrefix%>_batch-pids-<%=Master.Factory.SysUser.PID%>", value: pids, field: "p85Message", oper: "save" }, function (data) {

                if (data == " " || data == "0" || data == "") {
                    return;
                }


            });



            sw_master("<%=Me.CurrentPrefix%>_batch.aspx", "Images/batch.png");
            return;

        }
        function sendmail_batch() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }

            $.post("Handler/handler_tempbox.ashx", { guid: "<%=Me.CurrentPrefix%>_batch_sendmail-pids-<%=Master.Factory.SysUser.PID%>", value: pids, field: "p85Message", oper: "save" }, function (data) {

                if (data == " " || data == "0" || data == "") {
                    return;
                }


            });

            sw_master("<%=Me.CurrentPrefix%>_batch_sendmail.aspx", "Images/email.png");
            return;
        }

        function report() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }

            sw_master("report_modal.aspx?prefix=<%=Me.CurrentPrefix%>&pids=" + pids, "Images/report.png", true);

        }

        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx");
        }
        function approve() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }
            sw_master("p31_approving_step1.aspx?masterprefix=<%=me.CurrentPrefix%>&masterpids=" + pids, "Images/approve.png", true);
        }
        function cbx1_OnClientSelectedIndexChanged(sender, eventArgs) {
            var combo = sender;
            var pid = combo.get_value();
            <%If opgLayout.SelectedValue="2" then%>
            location.replace("<%=Me.CurrentPrefix%>_framework.aspx?pid=" + pid);
            <%end if%>
            <%If opgLayout.SelectedValue = "3" Then%>
            location.replace("<%=Me.CurrentPrefix%>_framework_detail.aspx?source=3&pid=" + pid);
            <%end if%>
        }
        function cbx1_OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            var combo = sender;
            
            if (combo.get_value() == "")
                context["filterstring"] = eventArgs.get_text();
            else
                context["filterstring"] = "";
            
            context["j03id"] = "<%=Master.Factory.SysUser.PID%>";
            context["flag"] = "searchbox";
            <%If Me.CurrentPrefix = "p41" Then%>
            context["j02id_explicit"] = "<%=Master.Factory.SysUser.j02ID%>";
            <%End If%>           
        }
        function cbx1_OnClientFocus(sender, args) {
            var combo = sender;
            var s = combo.get_text();
            if (s.indexOf("...") > 0)
                combo.set_text("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="offsetY"></div>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" ResizeMode="Proportional" OnClientLoad="loadSplitter" PanesBorderSize="0" Skin="Default" RenderMode="Auto" Orientation="Vertical">
        <telerik:RadPane ID="navigationPane" runat="server" Width="353px" OnClientResized="AfterPaneResized" OnClientCollapsed="AfterPaneCollapsed" OnClientExpanded="AfterPaneExpanded" BackColor="white">

            <asp:Panel ID="panSearch" runat="server" Style="min-height: 42px; background: #f7f7f7;">
                <div style="float: left;">
                    <asp:Image ID="img1" runat="server" ImageUrl="Images/project_32.png" />
                </div>
                <div class="commandcell" style="padding-left: 7px; width: 10px;">
                    <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
                </div>
                <div class="commandcell">
                    <asp:Label ID="MasterEntity" runat="server" Visible="false"></asp:Label>
                    <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 170px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
                    <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
                    <asp:LinkButton ID="cmdCĺearFilter" runat="server" Text="Vyčistit sloupcový filtr" Style="font-weight: bold; color: red;" Visible="false"></asp:LinkButton>
                </div>
                <div class="commandcell" style="padding-left: 3px;">
                    <button type="button" id="cmdGUI" class="show_hide3" style="padding: 1px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Rozvržení panelů">
                        <img src="Images/grid.png" />

                        <img src="Images/arrow_down.gif" />
                    </button>
                </div>
                <div id="divPeriodAndSettings" class="commandcell" style="padding-left: 4px;">
                    <button type="button" id="cmdPeriodQuery" class="show_hide2" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Filtr podle období">
                        <asp:Label ID="CurrentPeriodQuery" runat="server" Text="Období"></asp:Label>
                        <img src="Images/arrow_down.gif" />

                    </button>
                    <button type="button" id="cmdSetting" class="show_hide1" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Další možnosti datového přehledu">
                        <span>Další</span>

                        <img src="Images/arrow_down.gif" />
                    </button>
                </div>
                <asp:Panel ID="panSearchbox" runat="server" CssClass="commandcell" style="padding-left:10px">
                    <telerik:RadComboBox ID="cbx1" runat="server" RenderMode="Auto" DropDownWidth="400" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" Text="Hledat..." Width="120px" OnClientFocus="cbx1_OnClientFocus" OnClientSelectedIndexChanged="cbx1_OnClientSelectedIndexChanged" OnClientItemsRequesting="cbx1_OnClientItemsRequesting">                        
                        <WebServiceSettings Method="LoadComboData" UseHttpGet="false" />
                    </telerik:RadComboBox>
                </asp:Panel>


            </asp:Panel>
            <div style="clear: both; width: 100%;"></div>
            <div class="slidingDiv3" style="display: none;">
                <asp:RadioButtonList ID="opgLayout" runat="server" AutoPostBack="true" RepeatDirection="Vertical">
                    <asp:ListItem Text="Levý panel = přehled, pravý panel = detail" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Horní panel = přehled, spodní panel = detail" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Pouze přehled, dvojklikem přechod na detail" Value="3"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:Label ID="lblLayoutMessage" runat="server" CssClass="infoNotification" Text="Z důvodu malého rozlišení displeje (pod 1280px) se automaticky zapnul režim jediného panelu s datovým přehledem." Visible="false"></asp:Label>
            </div>
            <asp:Panel ID="panPeriod" runat="server" CssClass="slidingDiv2">
                <span>Filtr:</span>
                <asp:DropDownList ID="cbxPeriodType" AutoPostBack="true" runat="server" ToolTip="Druh filtrovaného období">
                </asp:DropDownList>
                <uc:periodcombo ID="period1" runat="server" Width="160px"></uc:periodcombo>
            </asp:Panel>
            <div class="slidingDiv1">
                <div class="content-box2">
                    <div class="title">
                        Datový přehled
                        <%If Me.CurrentPrefix <> "p91" Then%>
                        <button type="button" onclick="batch()" title="Hromadné operace nad označenými záznamy v přehledu">Hromadné operace</button>
                        <%End If%>
                        <button type="button" onclick="report()" title="Tisková sestava">Sestava (hromadně)</button>
                        <%If Me.CurrentPrefix = "p91" Then%>
                        <button type="button" onclick="sendmail_batch()">Hromadně odeslat faktury (e-mail)</button>
                        <%End If%>
                        <button id="cmdApprove" runat="server" type="button" visible="false" onclick="approve()">Schválit/připravit k fakturaci</button>
                    </div>
                    <div class="content">
                        <div>
                            <asp:DropDownList ID="cbxQueryFlag" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny" DataTextField="ColumnHeader" DataValueField="ColumnField">
                        </asp:DropDownList>
                        <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 180px;" ToolTip="Šablony datového přehledu"></asp:DropDownList>
                        <button type="button" onclick="griddesigner()" id="cmdGridDesiger" runat="server">Sloupce</button>



                        <div class="div6">
                            <span class="lbl">Stránkování přehledu:</span>
                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="100"></asp:ListItem>
                                <asp:ListItem Text="200"></asp:ListItem>
                                <asp:ListItem Text="500"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Panel ID="panExport" runat="server">
                            <img src="Images/export.png" alt="export" />
                            <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

                            <img src="Images/xls.png" alt="xls" />
                            <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                            <img src="Images/pdf.png" alt="pdf" />
                            <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                            <img src="Images/doc.png" alt="doc" />
                            <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                        </asp:Panel>
                        <div class="div6">
                            <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="Auto-rozbalené souhrny" AutoPostBack="true" Checked="false" />

                            <asp:CheckBox ID="chkCheckboxSelector" runat="server" Text="Checkbox selektor" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>

            <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" Skin="Default"></uc:datagrid>


            <asp:HiddenField ID="hiddatapid" runat="server" />
            <asp:HiddenField ID="hidDefaultSorting" runat="server" />
            <asp:HiddenField ID="hidJ62ID" runat="server" />
            <asp:HiddenField ID="hidX29ID" runat="server" Value="141" />
            <asp:HiddenField ID="hidPrefix" runat="server" Value="p41" />
            <asp:HiddenField ID="hidFooterSum" runat="server" Value="" />
            <asp:HiddenField ID="hidUIFlag" runat="server" />
            <asp:HiddenField ID="hidMasterPrefix" runat="server" />
            <asp:HiddenField ID="hidMasterPID" runat="server" />
            <asp:HiddenField ID="hidCols" runat="server" />
            <asp:HiddenField ID="hidSumCols" runat="server" />
            <asp:HiddenField ID="hidAdditionalFrom" runat="server" />
            <asp:HiddenField ID="hidContentPaneWidth" runat="server" />
            <asp:HiddenField ID="hidContentPaneDefUrl" runat="server" />


        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server" ShowContentDuringLoad="true">
            Detail projektu
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
