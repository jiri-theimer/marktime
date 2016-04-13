<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="x18_framework.aspx.vb" Inherits="UI.x18_framework" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var _initResizing = "1";

        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

           


            
        });

        function hardrefresh(pid, flag) {

            location.replace("x18_framework.aspx");


        }
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

            pane.set_contentUrl("x18_framework_detail.aspx?pid=" + pid);


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

            $.post("Handler/handler_userparam.ashx", { x36value: w, x36key: "x18_framework-navigationPane_width", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }


            });


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="offsetY"></div>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" ResizeMode="Proportional" OnClientLoaded="loadSplitter" PanesBorderSize="0" Skin="Metro" RenderMode="Lightweight">
        <telerik:RadPane ID="navigationPane" runat="server" Width="350px" OnClientResized="AfterPaneResized" MaxWidth="1500" MinWidth="50" BackColor="white">
            <asp:Panel ID="panSearch" runat="server" Style="min-height: 42px;">
                <div style="float: left;">
                    <asp:Image ID="img1" runat="server" ImageUrl="Images/label_32.png" />
                </div>
                <div class="commandcell">                    
                    <asp:DropDownList ID="x18ID" runat="server" AutoPostBack="true" DataTextField="x18Name" DataValueField="pid" Style="width: 170px;"></asp:DropDownList>
                    
                </div>
               <div style="float: right; margin-top: 10px;">                    
                    <button type="button" id="cmdSetting" class="show_hide1" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Další nastavení přehledu">

                        <img src="Images/arrow_down.gif" />
                    </button>
                </div>

            </asp:Panel>
            <div style="clear: both; width: 100%;"></div>
           
            <div class="slidingDiv1">
                <div class="content-box2">
                    <div class="title">
                        Datový přehled
                        
                        
                    </div>
                    <div class="content">
                        

                        <div class="div6">
                            <span class="lbl">Stránkování přehledu:</span>
                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="100"></asp:ListItem>
                                <asp:ListItem Text="200"></asp:ListItem>
                                <asp:ListItem Text="500"></asp:ListItem>
                            </asp:DropDownList>
                            
                            <asp:LinkButton ID="cmdExport" runat="server" Text="XLS export" />
                        </div>
                        <div class="div6">
                            <asp:CheckBox ID="chkCheckboxSelector" runat="server" Text="Checkbox selektor" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>

            <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" Skin="Default"></uc:datagrid>
            
            <span id="goto1"></span>
            <input type="hidden" id="goto2" />
            <asp:HiddenField ID="hiddatapid" runat="server" />
           <asp:HiddenField ID="hidX23ID" runat="server" />
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server" ShowContentDuringLoad="true">
            Detail štítku
        </telerik:RadPane>
    </telerik:RadSplitter>

</asp:Content>
