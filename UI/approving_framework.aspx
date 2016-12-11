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

            sw_master("entity_modal_approving.aspx?prefix=<%=me.hidCurPrefix.Value%>&pid=" + pid, "Images/approve_32.png", true);

        }

        function approve_selected() {
            var pids = GetAllSelectedPIDs();
            if (pids == "" || pids == null) {
                alert("Není vybrán řádek.");
                return
            }

            sw_master("entity_modal_approving.aspx?prefix=<%=me.hidCurPrefix.Value%>&pids=" + pids, "", true);

        }

        function invoice_selected() {
            var pids = GetAllSelectedPIDs();
            if (pids == "" || pids == null) {
                alert("Není vybrán řádek.");
                return
            }

            sw_master("entity_modal_invoicing.aspx?prefix=<%=me.hidCurPrefix.Value%>&pids=" + pids, "", true);

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

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=<%=Me.CurrentPrefix%>&pid=" + j70id, "Images/query_32.png");
            return (false);
        }

        function report() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }

            sw_master("report_modal.aspx?prefix=<%=Me.CurrentPrefix%>&pid=" + pids, "Images/report_32.png", true);

        }
        function p31_move2bin() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
            }
            var direction = "1";
            <%If Me.cbxScope.SelectedValue = "2" Then%>
            direction = "3";
            <%End If%>

            sw_master("p31_move2bin.aspx?prefix=<%=Me.CurrentPrefix%>&pid=" + pids + "&direction=" + direction, "Images/bin.png", true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadTabStrip ID="tabs1" runat="server" ShowBaseLine="true" Width="100%" Skin="Default" OnClientTabSelected="OnClientTabSelected">
        <Tabs>
            <telerik:RadTab Text="Projekty" Value="p41" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Klienti" Value="p28"></telerik:RadTab>
            <telerik:RadTab Text="Osoby" Value="j02"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <div style="background-color: white;">
        <div style="float: left;">
            <img src="Images/approve_32.png" title="Příprava fakturačních podkladů" />
            <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Schvalovat úkony"></asp:Label>
        </div>

        <div class="commandcell" style="padding-left: 20px;">
            <asp:DropDownList ID="cbxScope" runat="server" AutoPostBack="true" BackColor="Yellow">
                <asp:ListItem Text="Rozpracované (čeká na schvalování)" Value="1"></asp:ListItem>
                <asp:ListItem Text="Schválené (čeká na fakturaci)" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="commandcell">
            <uc:periodcombo ID="period1" runat="server" Width="180px"></uc:periodcombo>
        </div>
        <div class="commandcell">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 170px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
        </div>
        <div class="commandcell" style="margin-left: 12px;">
            <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
                <Items>
                    <telerik:RadMenuItem Text="Akce" ImageUrl="Images/menuarrow.png">
                        <Items>
                            <telerik:RadMenuItem Text="Zahájit schvalovací/fakturační průvodce pro označené" Value="approve" NavigateUrl="javascript:approve_selected()" ImageUrl="Images/approve.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="Hromadně vygenerovat DRAFT faktury" Value="draft" NavigateUrl="javascript:invoice_selected()" ImageUrl="Images/invoice.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="Přesunout nevyfakturované úkony do archivu" Value="bin" NavigateUrl="javascript:p31_move2bin()" ImageUrl="Images/bin.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="Tisková sestava (funguje i hromadný tisk)" Value="report" NavigateUrl="javascript:report()" ImageUrl="Images/report.png"></telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Export" Value="export" ImageUrl="Images/menuarrow.png">
                        <Items>
                            <telerik:RadMenuItem Text="XLS" NavigateUrl="javascript:hardrefresh(0,'xls')"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="DOC" NavigateUrl="javascript:hardrefresh(0,'doc')"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="PDF" NavigateUrl="javascript:hardrefresh(0,'pdf')"></telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Nastavení" ImageUrl="Images/menuarrow.png" Value="more" PostBack="false">

                        <ContentTemplate>
                            <div class="div6">
                                <asp:RadioButtonList ID="cbxScrollingFlag" runat="server" RepeatDirection="Vertical" AutoPostBack="true">
                                    <asp:ListItem Text="Pevné ukotvení záhlaví tabulky (názvy sloupců)" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Ukotvení všeho nad tabulkou (filtrování a menu)" Value="1" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="Bez podpory ukotvení" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
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
                            <div class="div6">
                                <asp:CheckBox ID="chkFirstLastCount" runat="server" AutoPostBack="true" Text="Zobrazovat sloupce [Datum prvního úkonu], [Datum posledního úkonu]" Checked="true" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadMenuItem>
                </Items>
            </telerik:RadMenu>

        </div>


        <div style="clear: both;"></div>
        <asp:Button ID="cmdHardRefreshOnBehind" runat="server" Style="display: none;" />


        <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowDblClick="RowDoubleClick" OnRowSelected="RowSelected" AllowFilteringByColumn="true"></uc:datagrid>

    
    </div>

    <asp:HiddenField ID="hidCurPID" runat="server" />
    <asp:HiddenField ID="hidCurPrefix" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
</asp:Content>
