<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="x25_framework.aspx.vb" Inherits="UI.x25_framework" %>

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
                $(".slidingDiv1").slideToggle();
            });



            if (document.getElementById("<%=Me.hidUIFlag.ClientID%>").value != "")
                $(".slidingDiv1").show();


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


            var splitter = $find("<%= RadSplitter1.ClientID %>");
            var pane = splitter.getPaneById("<%=contentPane.ClientID%>");
            
            var url = "x25_framework_detail.aspx?pid=" + pid;
            pane.set_contentUrl(url);


        }
        function RowDoubleClick(sender, args) {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == null)
                return;

            x25_record(pid);
        }

        function x25_record(pid) {
            sw_everywhere("x25_record.aspx?x18id=<%=Me.CurrentX18ID%>&pid=" + pid, "Images/label.png", true);
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

            var keyname = "x25_framework-navigationPane_width";

            $.post("Handler/handler_userparam.ashx", { x36value: w, x36key: keyname, oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }


            });
        }

        function AfterPaneResized(sender, args) {
            var w = sender.get_width();
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


            location.replace("x25_framework.aspx?pid=" + pid + "&x18id=<%=Me.CurrentX18ID%>");


        }



        function report() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }

            sw_master("report_modal.aspx?prefix=x25&pids=" + pids, "Images/report.png", true);

        }

        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx");
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
            <%If 1 = 1 Then%>
            context["j02id_explicit"] = "<%=Master.Factory.SysUser.j02ID%>";
            <%End If%>
        }

        function x18_setting() {
            var pid =
            sw_master("x18_record.aspx?pid=<%=Me.currentx18id%>", "Images/label.png", true);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="offsetY"></div>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" ResizeMode="Proportional" OnClientLoad="loadSplitter" PanesBorderSize="0" Skin="Default" RenderMode="Auto" Orientation="Vertical">
        <telerik:RadPane ID="navigationPane" runat="server" Width="353px" OnClientResized="AfterPaneResized" OnClientCollapsed="AfterPaneCollapsed" OnClientExpanded="AfterPaneExpanded" BackColor="white">

            <asp:Panel ID="panSearch" runat="server" Style="min-height: 42px; background-color: #f7f7f7;">
                <div style="float: left;">
                    <asp:Image ID="img1" runat="server" ImageUrl="Images/label_32.png" />
                </div>

                <div class="commandcell">
                    <asp:DropDownList ID="x18ID" runat="server" AutoPostBack="true" DataTextField="x18Name" DataValueField="pid" Style="width: 200px;" ToolTip="Štítek"></asp:DropDownList>
                    
                </div>
                <asp:Panel ID="panSearchbox" runat="server" CssClass="commandcell" Style="padding-left: 10px;">
                    <telerik:RadComboBox ID="cbx1" runat="server" RenderMode="Auto" DropDownWidth="400" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" Text="Hledat..." Width="120px" OnClientItemsRequesting="cbx1_OnClientItemsRequesting" AutoPostBack="false">
                        <WebServiceSettings Method="LoadComboData" UseHttpGet="false" />
                    </telerik:RadComboBox>
                </asp:Panel>


                <div class="commandcell" style="padding-left: 4px;">

                    <button type="button" class="show_hide1" style="padding: 5px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; color: white; background-color: #25a0da;">
                        <asp:Label ID="lblGridHeader" runat="server" Text="Akce nad přehledem"></asp:Label>
                        <img src="Images/arrow_down_menu.png" />

                    </button>
                </div>

                <div class="commandcell" id="divQueryContainer"></div>
            </asp:Panel>

            <div style="clear: both; width: 100%;"></div>
            <div style="float: left;">
                <asp:Label ID="MasterEntity" runat="server" Visible="false"></asp:Label>
            </div>

            <div style="float: left; padding-left: 6px;">
                <asp:Label ID="CurrentPeriodQuery" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <div style="float: left; padding-left: 6px;">

                <asp:Label ID="CurrentQuery" runat="server" ForeColor="Red"></asp:Label>
            </div>
            <div style="float: left; padding-left: 6px;">
                <asp:LinkButton ID="cmdCĺearFilter" runat="server" Text="Vyčistit sloupcový filtr" Style="font-weight: bold; color: red;" Visible="false"></asp:LinkButton>
            </div>
            <div style="clear: both; width: 100%;"></div>

            <div class="slidingDiv1" style="display: none; background: #f0f8ff;">
                <div class="content-box3">
                    <div class="title">
                        <img src="Images/query.png" />
                        <span>Filtrování záznamů</span>
                    </div>
                    <div class="content">
                        <div class="div6">
                            <asp:DropDownList ID="cbxX25Validity" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="Otevřené i archivované záznamy" Value="1" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="Pouze otevřené záznamy" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Pouze archivované záznamy" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div style="margin-top: 20px;">
                            <asp:DropDownList ID="cbxPeriodType" AutoPostBack="true" runat="server" ToolTip="Druh filtrovaného období">
                            </asp:DropDownList>
                            <uc:periodcombo ID="period1" runat="server" Width="160px" Visible="false"></uc:periodcombo>
                        </div>
                    </div>
                </div>


                <asp:Panel ID="panExport" runat="server" CssClass="content-box3" Style="margin-top: 20px;">
                    <div class="title">
                        <img src="Images/export.png" />
                        <span>Export záznamů v aktuálním přehledu</span>

                    </div>
                    <div class="content">

                        <img src="Images/export.png" alt="export" />
                        <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

                        <img src="Images/xls.png" alt="xls" />
                        <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                        <img src="Images/pdf.png" alt="pdf" />
                        <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                        <img src="Images/doc.png" alt="doc" />
                        <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />


                    </div>
                </asp:Panel>
                <div class="content-box3" style="margin-top: 20px;">
                    <div class="title">
                        <img src="Images/griddesigner.png" />
                        <span>Nastavení přehledu</span>

                    </div>
                    <div class="content">




                        <div class="div6">
                            <span class="val">Stránkování přehledu:</span>
                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="100"></asp:ListItem>
                                <asp:ListItem Text="200"></asp:ListItem>
                                <asp:ListItem Text="500"></asp:ListItem>
                            </asp:DropDownList>



                        </div>


                    </div>
                </div>
                <div class="div6">
                    <button type="button" onclick="x18_setting()" id="cmdSetting" runat="server">
                        <img src="Images/label.png" />Nastavení štítku</button>
                </div>
            </div>

            <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick" Skin="Default"></uc:datagrid>


            <asp:HiddenField ID="hidX23ID" runat="server" />
            <asp:HiddenField ID="hiddatapid" runat="server" />
            <asp:HiddenField ID="hidDefaultSorting" runat="server" />

            <asp:HiddenField ID="hidMasterPrefix" runat="server" />
            <asp:HiddenField ID="hidMasterPID" runat="server" />
            <asp:HiddenField ID="hidUIFlag" runat="server" />
            <asp:HiddenField ID="hidContentPaneDefUrl" runat="server" />
            <asp:HiddenField ID="hidContentPaneWidth" runat="server" />
            <asp:HiddenField ID="hidx18GridColsFlag" runat="server" Value="1" />
            <asp:HiddenField ID="hidCols" runat="server" />
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server" ShowContentDuringLoad="true">
            Detail projektu
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
