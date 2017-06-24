<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="x25_framework.aspx.vb" Inherits="UI.x25_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var _initResizing = "1";

        $(document).ready(function () {
            


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

            var url = "x25_framework_detail.aspx?x18id=<%=Me.CurrentX18ID%>&pid=" + pid;
            pane.set_contentUrl(url);


        }
        function x18id_onchange(ctl) {

            $.post("Handler/handler_userparam.ashx", { x36value: ctl.value, x36key: "x25_framework-x18id", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }

            });

            location.replace("x25_framework.aspx?x18id=" + ctl.value);
        }
        function RowDoubleClick(sender, args) {
            record_edit();
        }

        function record_edit() {
            if (getpid(false) == "")
                return;

            x25_record(getpid(false));
        }

        function x25_record(pid) {
            sw_everywhere("x25_record.aspx?x18id=<%=Me.CurrentX18ID%>&pid=" + pid, "Images/label.png", true);
        }

        function record_clone() {
            var pid = getpid(true);
            if (pid == "")
                return;

            sw_everywhere("x25_record.aspx?clone=1&x18id=<%=Me.CurrentX18ID%>&pid=" + pid, "Images/label.png", true);
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

            var keyname = "x25_framework-navigationPane_width-<%=me.currentx18id%>";

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

            if (flag == "b07-save") {
                return;
            }
            if (flag == "workflow-dialog") {
                pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            }

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





        function x18_setting() {
            sw_master("x18_record.aspx?pid=<%=Me.currentx18id%>", "Images/label.png", true);
        }
        function x18_framework() {
            location.replace("x18_framework.aspx")
        }

        function b07_create() {
            if (getpid(true) == "")
                return;

            sw_master("b07_create.aspx?masterprefix=x25&masterpid=" + getpid(true), "Images/comment.png", true);

        }
        function workflow() {
            if (getpid(true) == "")
                return;

            sw_master("workflow_dialog.aspx?prefix=x25&pid=" + getpid(true), "Images/workflow.png", true);
        }

        function getpid(shall_alert) {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" && shall_alert == true) {
                alert("Musíte vybrat záznam.");
                return ("");
            }

            return (pid);
        }
        function sendmail() {
            sw_everywhere("sendmail.aspx?prefix=x25&pid=" + getpid(true), "Images/email.png")


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
                <div class="commandcell" style="min-width: 25px; text-align: center;">
                    <asp:Label ID="lblVirtualCount" runat="server" ToolTip="Počet záznamů v aktuálním přehledu"></asp:Label>
                </div>
                <div class="commandcell">
                    <asp:DropDownList ID="x18ID" runat="server" AutoPostBack="false" BackColor="Yellow" onchange="x18id_onchange(this)" DataTextField="x18Name" DataValueField="pid" Style="width: 220px;" ToolTip="Štítek"></asp:DropDownList>

                </div>
                <div class="commandcell" style="padding-left: 10px;">
                    <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" runat="server" Style="z-index: 3000;" ExpandAnimation-Duration="0" ExpandAnimation-Type="none" ClickToOpen="true">
                        <Items>

                            <telerik:RadMenuItem Text="ZÁZNAM" Value="record" PostBack="false" ImageUrl="Images/arrow_down_menu.png">
                                <Items>
                                    <telerik:RadMenuItem Value="cmdNew" Text="Nový" NavigateUrl="javascript:x25_record(0);" ImageUrl="Images/new.png"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="cmdEdit" Text="Upravit" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="cmdClone" Text="Kopírovat" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" Visible="false"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="cmdWorkflow" Text="Zapsat komentář/souborovou přílohu" NavigateUrl="javascript:b07_create();" ImageUrl="Images/comment.png"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="cmdEmail" Text="Odeslat e-mail" NavigateUrl="javascript:sendmail();" ImageUrl="Images/email.png"></telerik:RadMenuItem>
                                </Items>
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="KALENDÁŘ" Value="scheduler" ImageUrl="Images/calendar.png" ToolTip="Přepnout do kalendáře" Visible="false"></telerik:RadMenuItem>

                            <telerik:RadMenuItem Text="DALŠÍ AKCE" Value="more" ImageUrl="Images/arrow_down_menu.png" >
                                <GroupSettings OffsetX="-150"/>
                                <ContentTemplate>
                                    <div style="padding:10px;background-color:#f0f8ff;border:solid 1px black;">
                                    <div class="content-box3">
                                       
                                        <div class="content">
                                            <button type="button" id="cmdSetting" runat="server" onclick="x18_setting()">
                                            <img src="Images/label.png" />Nastavení štítku</button>
                                        <button type="button" onclick="x18_framework()" id="cmdAdmin" runat="server" style="margin-left: 30px;">
                                            <img src="Images/setting.png" />Administrace všech štítků</button>
                                        </div>
                                    </div>
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
                                    </div>
                                </ContentTemplate>
                            </telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenu>

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
                <asp:LinkButton ID="cmdCĺearFilter" runat="server" Text="<img src='Images/sweep.png'/>Vyčistit sloupcový filtr" Style="font-weight: bold; color: red;" Visible="false"></asp:LinkButton>
            </div>
            <div style="clear: both; width: 100%;"></div>

           

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
            <asp:HiddenField ID="hidB01ID" runat="server" />
            <asp:HiddenField ID="hidx18IsColors" runat="server" Value="0" />
            <asp:HiddenField ID="hidCols" runat="server" />
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server" ShowContentDuringLoad="true">
            Detail projektu
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
