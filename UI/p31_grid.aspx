﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p31_grid.aspx.vb" Inherits="UI.p31_grid" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {



            $('#<%=Me.txtSearch.ClientID%>').keydown(function (event) {
                var keypressed = event.keyCode || event.which;
                if (keypressed == 13) {
                    search();
                }
            });

            if ($("#<%=Me.txtSearch.ClientID%>").val() != '') {
                $("#<%=Me.txtSearch.ClientID%>").focus();
                $("#<%=Me.txtSearch.ClientID%>").select();
            }

            $("#<%=Me.txtSearch.ClientID%>").focus(function () { $(this).select(); });




        });





        function record_edit() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_master("p31_record.aspx?pid=" + pid, "Images/worksheet.png");

        }

        function record_new() {
            sw_master("p31_record.aspx?pid=0", "Images/worksheet.png");


        }

        function RowSelected(sender, args) {
            document.getElementById("<%=hiddatapid.clientid%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            if (args.get_tableView().get_name() == "grid") {

                record_edit();
            }
            if (args.get_tableView().get_name() == "drilldown") {
                var item = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];

                var rowid = item.get_id();
                var firstInput = $('#' + rowid).find('input[type=submit]').filter(':visible:first');
                if (firstInput != null) {
                    firstInput.click();
                }

            }
        }

        function GetAllSelectedPIDs() {

            var masterTable = $find("<%=grid1.radGridOrig.ClientID%>").get_masterTableView();
            <%If Me.hidDrillDownField.Value = "" Then%>
            var sel = masterTable.get_selectedItems();
            <%Else%>

            var dataItems = masterTable.get_dataItems();
            for (var i = 0; i < dataItems.length; i++) {
                if (dataItems[i].get_nestedViews().length > 0) {
                    var sel = dataItems[i].get_nestedViews()[0].get_selectedItems();
                }
            }

            <%End If%>
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
            var pids = GetAllSelectedPIDs();            
            if (pids == "" || pids == null) {
                alert("Není vybrán záznam.");
                return
            }            
            sw_master("p31_record.aspx?clone=1&pid=" + pids, "Images/worksheet.png");

        }

        function record_split() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_master("p31_record_split.aspx?pid=" + pid, "Images/worksheet.png");

        }

        function search() {
            var s = document.getElementById("<%=Me.txtSearch.ClientID%>").value;

            $.post("Handler/handler_userparam.ashx", { x36value: s, x36key: "p31_grid-search", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }
                location.replace("p31_grid.aspx");

            });
        }

        function hardrefresh(pid, flag) {
            if (flag == "quickquery") {
                location.replace("p31_grid.aspx");
                return;
            }

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            sw_master("grid_designer.aspx?prefix=p31&masterprefix=p31_grid&pid=" + j74id, "Images/griddesigner.png");
            
        }

        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx", "Images/settings.png");
        }


        function approving() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
                //return (false);
            }

            sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve.png", true);


        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query.png");
            return (false);
        }
        function drilldown() {
            
            var masterprefix = document.getElementById("<%=Me.hidMasterPrefix.ClientID%>").value;
            var masterpid = document.getElementById("<%=Me.hidMasterPID.ClientID%>").value;
            var tabqueryflag = document.getElementById("<%=Me.cbxTabQueryFlag.ClientID%>").value;

            location.replace("p31_sumgrid.aspx?masterprefix=" + masterprefix + "&masterpid=" + masterpid + "&tabqueryflag=" + tabqueryflag);
           
            return (false);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="min-height: 44px; background-color: white; border-bottom: solid 1px silver;">
        <div style="float: left; padding-top: 3px;">
            <img src="Images/worksheet_32.png" alt="Worksheet přehled" />
        </div>
        <div class="commandcell" style="padding-left: 10px; padding-right: 6px;">
            <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Style="vertical-align: top;"></asp:Label>
        </div>
        <div class="commandcell">
            <asp:ImageButton ID="cmdSummary" runat="server" OnClientClick="return drilldown()" ImageUrl="Images/pivot.png" ToolTip="Statistika WORKSHEET přehledu" CssClass="button-link" />
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 150px;" ToolTip="Pojmenované šablony sloupců"></asp:DropDownList>
            <button type="button" onclick="griddesigner()" id="cmdGridDesigner2" runat="server">Sloupce</button>
            
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <uc:periodcombo ID="period1" runat="server" Width="170px"></uc:periodcombo>
        </div>

        <div class="commandcell" style="padding-left: 10px;">
            <asp:TextBox ID="txtSearch" runat="server" Text="" Style="width: 90px;" ToolTip="Hledat podle názvu klienta, projektu, kódu projektu, příjmení osoby, názvu aktivity nebo podrobného popisu úkonu"></asp:TextBox>
            <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="Images/search.png" CssClass="button-link" ToolTip="Hledat" />

        </div>
        <div class="commandcell">
            <asp:DropDownList ID="cbxTabQueryFlag" runat="server" AutoPostBack="true">
                <asp:ListItem Text="--Druh úkonů--" Value="p31" Selected="true"></asp:ListItem>
                <asp:ListItem Text="Pouze hodiny" Value="time"></asp:ListItem>
                <asp:ListItem Text="Pouze výdaje" Value="expense"></asp:ListItem>
                <asp:ListItem Text="Paušální odměny" Value="fee"></asp:ListItem>
                <asp:ListItem Text="Pouze kusovník" Value="kusovnik"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="commandcell">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 150px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
        </div>

        <div class="commandcell" style="padding-left: 20px;">
            <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
                <Items>

                    <telerik:RadMenuItem Text="<%$Resources:common,Zaznam %>" Value="record" PostBack="false" ImageUrl="Images/arrow_down_menu.png">
                        <Items>
                            <telerik:RadMenuItem Value="cmdNew" Text="<%$Resources:common,Novy %>" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="cmdEdit" Text="<%$Resources:common,Upravit %>" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="cmdClone" Text="Kopírovat (i hromadně)" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="cmdSplit" Text="<%$Resources:p31_grid,Rozdelit %>" NavigateUrl="javascript:record_split();" ImageUrl="Images/split.png"></telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Akce" Value="action" PostBack="false" ImageUrl="Images/arrow_down_menu.png">
                        <Items>
                            <telerik:RadMenuItem Value="cmdApprove" Text="<%$Resources:p31_grid,Schvalovat_Preschvalovat %>" NavigateUrl="javascript:approving();" ImageUrl="Images/approve.png"></telerik:RadMenuItem>

                        </Items>
                    </telerik:RadMenuItem>


                    <telerik:RadMenuItem Text="<%$Resources:common,Dalsi %>" ImageUrl="Images/menuarrow.png" Value="columns" PostBack="false">
                        <ContentTemplate>
                            <div style="padding: 20px;">
                                <div class="div6">
                                </div>
                                <asp:Panel ID="panGroupBy" runat="server" CssClass="div6">
                                    <span><%=Resources.p31_grid.DatoveSouhrny%></span>
                                    <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny" DataTextField="ColumnHeader" DataValueField="ColumnField">
                                    </asp:DropDownList>
                                    <div>
                                        <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="<%$Resources:p31_framework,AutoRozbaleneSouhrny %>" AutoPostBack="true" Checked="true" />
                                    </div>
                                </asp:Panel>


                                <asp:Panel ID="panExport" runat="server" CssClass="div6">
                                    <img src="Images/export.png" alt="export" />
                                    <asp:LinkButton ID="cmdExport" runat="server" Text="Export" />

                                    <img src="Images/xls.png" alt="xls" />
                                    <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                    <img src="Images/pdf.png" alt="pdf" />
                                    <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                    <img src="Images/doc.png" alt="doc" />
                                    <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                </asp:Panel>
                                <div class="div6">
                                    <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="<%$Resources:common,PocetZaznamuNaStranku %>"></asp:Label>
                                    <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování" TabIndex="3">
                                        <asp:ListItem Text="20"></asp:ListItem>
                                        <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="100"></asp:ListItem>
                                        <asp:ListItem Text="200"></asp:ListItem>
                                        <asp:ListItem Text="500"></asp:ListItem>
                                    </asp:DropDownList>

                                </div>


                            </div>
                        </ContentTemplate>
                    </telerik:RadMenuItem>

                    <telerik:RadMenuItem Text="Obnovit" Visible="false" ImageUrl="Images/refresh.png" Value="refresh" NavigateUrl="p31_grid.aspx"></telerik:RadMenuItem>
                </Items>
            </telerik:RadMenu>

        </div>
        <div class="commandcell">
            <asp:LinkButton ID="cmdCĺearFilter" runat="server" Text="Vyčistit sloupcový filtr" Style="margin-left: 10px; font-weight: bold; color: red;"></asp:LinkButton>
        </div>
        <div style="clear: both;"></div>
        <asp:Panel ID="panAdditionalQuery" runat="server" CssClass="div6" Visible="false">
            <table cellpadding="0">
                <tr>
                    <td>
                        <asp:Image ID="imgEntity" runat="server" />
                    </td>
                    <td style="padding-left: 10px;">

                        <asp:HyperLink ID="MasterRecord" runat="server"></asp:HyperLink>

                    </td>
                </tr>
            </table>
            <asp:Label ID="lblDrillDown" runat="server" CssClass="valboldred"></asp:Label>
            <asp:HyperLink ID="linkDrillDown" runat="server"></asp:HyperLink>
        </asp:Panel>
    </div>


    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>


    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidDrillDownField" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
    <asp:HiddenField ID="hidSumCols" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
    <asp:HiddenField ID="hidMasterAW" runat="server" />
    
    <asp:HiddenField ID="hidFooterString" runat="server" />
    <asp:HiddenField ID="hidJ62ID" runat="server" />
    <asp:HiddenField ID="hidSGF" runat="server" />
    <asp:HiddenField ID="hidSGA" runat="server" />
    <asp:HiddenField ID="hidSGV" runat="server" />

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
