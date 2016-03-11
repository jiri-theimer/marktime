<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="pokus_mobile.aspx.vb" Inherits="UI.pokus_mobile" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _initResizing = "1";

        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

           


         

           

        });

        function loadSplitter(sender) {
            var h1 = new Number;
            var h2 = new Number;
            var h3 = new Number;

            h1 = $(window).height();

            var ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            h3 = h1 - h2;

            sender.set_height(h3);

        }


        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;

            var splitter = $find("<%= RadSplitter1.ClientID %>");
            var pane = splitter.getPaneById("<%=contentPane.ClientID%>");
            
            pane.set_contentUrl("p28_framework_detail.aspx?pid=" + pid);

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

        function AfterPaneResized(sender, args) {
            if (_initResizing == "1") {
                _initResizing = "0";
                return;
            }

            var w = sender.get_width();

            $.post("Handler/handler_userparam.ashx", { x36value: w, x36key: "p28_framework-navigationPane_width", oper: "set" }, function (data) {               
                if (data == ' ') {
                    return;
                }


            });
            
            
        }

        function hardrefresh(pid, flag) {
            

            location.replace("p28_framework.aspx");
        }

        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            sw_master("grid_designer.aspx?prefix=p28&pid=" + j74id, "Images/griddesigner_32.png");
        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=p28&pid=" + j70id, "Images/query_32.png");
            return (false);
        }

        function batch() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return (false);
            }

            $.post("Handler/handler_tempbox.ashx", { guid: "p28_batch-pids-<%=Master.Factory.SysUser.PID%>", value: pids, field: "p85Message", oper: "save" }, function (data) {

                if (data == " " || data == "0" || data == "") {
                    return;
                }


            });



            sw_master("p28_batch.aspx", "Images/batch_32.png");
            return (false);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="offsetY"></div>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" ResizeMode="Proportional" OnClientLoaded="loadSplitter" PanesBorderSize="0" RenderMode="Mobile">
        <telerik:RadPane ID="navigationPane" runat="server" Width="350px" OnClientResized="AfterPaneResized" MaxWidth="1000" MinWidth="50" BackColor="white">
            <asp:Panel ID="panSearch" runat="server" CssClass="div6" Style="height: 28px;">
                <table width="98%">
                    <tr>
                        <td style="width:35px;">
                            <img src="Images/contact_32.png" />
                            
                        </td>
                        <td style="width:10px;">
                            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
                        </td>
                        <td style="width: 171px;">
                            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 170px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>

                        </td>
                        <td style="text-align: left;">
                            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />                           
                            <asp:ImageButton ID="cmdBatch" runat="server" OnClientClick="return batch()" ImageUrl="Images/batch.png" ToolTip="Hromadné operace" CssClass="button-link" />
                        </td>                        
                        <td style="text-align:right;">
                            <button type="button" id="cmdSetting" class="show_hide1" style="padding:0px;border-radius:4px;border-top: solid 1px silver;border-left: solid 1px silver;border-bottom: solid 1px gray;border-right: solid 1px gray;background:buttonface;" title="Nastavení přehledu">
                                <img src="Images/more.png" />
                                <img src="Images/arrow_down.gif" alt="Nastavení" />
                            </button>
                        </td>
                    </tr>
                </table>

            </asp:Panel>
            <div class="slidingDiv1">
                <div class="content-box2">
                    <div class="title">
                        Nastavení přehledu
                    </div>
                    <div class="content">
                        <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny">
                            <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
                            <asp:ListItem Text="Typ klienta" Value="p29Name"></asp:ListItem>
                            <asp:ListItem Text="Typ faktury" Value="p92Name"></asp:ListItem>
                            <asp:ListItem Text="Fakturační jazyk" Value="p87Name"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 150px;" ToolTip="Šablony datového přehledu"></asp:DropDownList>
                        <button type="button" onclick="griddesigner()">Sloupce</button>

                        <div class="div6">
                            <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="Stránkování přehledu:"></asp:Label>
                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="100"></asp:ListItem>
                                <asp:ListItem Text="200"></asp:ListItem>
                                <asp:ListItem Text="500"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="Auto-rozbalené souhrny" AutoPostBack="true" Checked="false" />
                            <asp:LinkButton ID="cmdExport" runat="server" Text="XLS export" />
                            <div class="div6">
                            <asp:CheckBox ID="chkCheckboxSelector" runat="server" Text="Checkbox selektor" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" Skin="Metro"></uc:datagrid>


            <asp:HiddenField ID="hiddatapid" runat="server" />
            
            
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server">
            Detail projektu
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
