<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="approving_framework.aspx.vb" Inherits="UI.approving_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .RadGrid .rgRow td {
            border-left: solid 1px silver !important;
        }

        .RadGrid .rgAltRow td {
            border-left: solid 1px silver !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {


            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });



        });

        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx", "Images/settings_32.png");
        }

        function RowSelected(sender, args) {

            document.getElementById("<%=hidCurPID.ClientID%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            approve_record();
        }

        function approve_record() {
            var pid = document.getElementById("<%=hidCurPID.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán řádek.");
                return
            }

            sw_master("entity_modal_approving.aspx?prefix=<%=me.hidCurPrefix.Value%>&pid=" + pid, "", true);

        }

        function approve_selected() {
            var pids = GetAllSelectedPIDs();
            if (pids == "" || pids == null) {
                alert("Není vybrán řádek.");
                return
            }

            sw_master("entity_modal_approving.aspx?prefix=<%=me.hidCurPrefix.Value%>&pids=" + pids, "", true);

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
            document.getElementById("<%=hidCurPID.clientid%>").value = pid;
            document.getElementById("<%=hidHardRefreshFlag.ClientID%>").value = flag;
            
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefreshOnBehind, "", False)%>

        }

        function OnClientTabSelected(sender, eventArgs) {
            var tab = eventArgs.get_tab();
            var prefix = tab.get_value();

            $.post("Handler/handler_userparam.ashx", { x36value: prefix, x36key: "approving_framework-prefix", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }
                location.replace("approving_framework.aspx");

            });


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadTabStrip ID="tabs1" runat="server" ShowBaseLine="true" Width="100%" Skin="Metro" OnClientTabSelected="OnClientTabSelected">
        <Tabs>
            <telerik:RadTab Text="Projekty" Value="p41" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Klienti" Value="p28"></telerik:RadTab>
            <telerik:RadTab Text="Osoby" Value="j02"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <div style="background-color: white; padding: 10px;">
        <div style="float:left;">
            <img src="Images/approve_32.png" title="Příprava fakturačních podkladů" />
            <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Schvalovat úkony"></asp:Label>
        </div>

        <div class="commandcell" style="padding-left:20px;">
            <asp:DropDownList ID="cbxScope" runat="server" AutoPostBack="true" BackColor="Yellow">
                <asp:ListItem Text="Rozpracované (čeká na schvalování)" Value="1"></asp:ListItem>
                <asp:ListItem Text="Schválené (čeká na fakturaci)" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="commandcell">
            <uc:periodcombo ID="period1" runat="server" Width="220px"></uc:periodcombo>
        </div>
        <div class="commandcell">
            <button type="button" onclick="approve_selected()" title="Schvalovat všechny označené řádky">
                <img src="Images/approve.png" alt="Schvalovat" />
                Schvalovat/fakturovat vybrané
            </button>

            <button type="button" onclick="hardrefresh(0,'export')" title="Export přehledu do MS Excel">
                <img src="Images/export.png" />
                MS Excel
            </button>
        </div>
        <div class="show_hide1" style="float:left;margin-top:10px;">
            <button type="button">


                <img src="Images/arrow_down_menu.png" />
                Nastavení

            </button>
        </div>
        <div style="clear:both; width: 100%;"></div>

        <div class="slidingDiv1">
            <div class="div6">
                <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="Stránkování:"></asp:Label>
                <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="20"></asp:ListItem>
                    <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="100"></asp:ListItem>
                    <asp:ListItem Text="200"></asp:ListItem>
                    <asp:ListItem Text="500"></asp:ListItem>
                </asp:DropDownList>

            </div>
            <div class="div6">
                <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Souhrny podle">
                    <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
                    <asp:ListItem Text="Souhrny podle měny" Value="j27Code"></asp:ListItem>
                    <asp:ListItem Text="Souhrny podle klienta projektu" Value="Client"></asp:ListItem>

                </asp:DropDownList>
            </div>
            <div class="div6">
                <asp:CheckBox ID="chkKusovnik" runat="server" AutoPostBack="true" Text="Zobrazovat i honoráře z kusovníkových úkonů" />
            </div>


        </div>



        <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowDblClick="RowDoubleClick" OnRowSelected="RowSelected" AllowFilteringByColumn="true"></uc:datagrid>


    </div>
    <asp:Button ID="cmdHardRefreshOnBehind" runat="server" Style="display: none;" />
    <asp:HiddenField ID="hidCurPID" runat="server" />
    <asp:HiddenField ID="hidCurPrefix" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
</asp:Content>
