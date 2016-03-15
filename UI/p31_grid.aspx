<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p31_grid.aspx.vb" Inherits="UI.p31_grid" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {

            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

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
            sw_master("p31_record.aspx?pid=" + pid, "Images/worksheet_32.png");

        }

        function record_new() {
            sw_master("p31_record.aspx?pid=0", "Images/worksheet_32.png");


        }

        function RowSelected(sender, args) {

            document.getElementById("<%=hiddatapid.clientid%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            record_edit();
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
            sw_master("p31_record.aspx?clone=1&pid=" + pid, "Images/worksheet_32.png");

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
            sw_master("grid_designer.aspx?prefix=p31&masterprefix=p31_grid&pid=" + j74id, "Images/griddesigner_32.png");
        }

        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx", "Images/settings_32.png");
        }


        function approving() {
            var pids = GetAllSelectedPIDs();
            if (pids == "") {
                alert("Není vybrán ani jeden záznam.");
                return;
                //return (false);
            }

            sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);


        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query_32.png");
            return (false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 44px; background-color: white; border-bottom: solid 1px silver;">
        <div style="float:left;padding-top:3px;">
            <img src="Images/worksheet_32.png" alt="Worksheet přehled" />
        </div>
        <div class="commandcell" style="min-width:200px;padding-left:10px;">
            <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Text="Worksheet" Style="vertical-align: top;"></asp:Label>
        </div>
        <div class="commandcell">
            <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true" ToolTip="Datové souhrny" DataTextField="ColumnHeader" DataValueField="ColumnField">
            </asp:DropDownList>
        </div>
        <div class="commandcell">
            <uc:periodcombo ID="period1" runat="server" Width="170px"></uc:periodcombo>
        </div>

        <div class="commandcell" style="padding-left: 10px;">
            <asp:TextBox ID="txtSearch" runat="server" Text="" Style="width: 90px;" ToolTip="Hledat podle názvu klienta, projektu, kódu projektu, příjmení osoby, názvu aktivity nebo podrobného popisu úkonu"></asp:TextBox>
            <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="Images/search.png" CssClass="button-link" ToolTip="Hledat" />

        </div>

        <div class="commandcell">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 150px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
        </div>

        <div style="float:left;padding-left: 20px;">
            <telerik:RadMenu ID="menu1" RenderMode="Lightweight" Skin="Silk" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
                <Items>

                    <telerik:RadMenuItem Text="Záznam" Value="record" PostBack="false" ImageUrl="Images/arrow_down_menu.png">
                        <ContentTemplate>
                            <div style="padding: 10px; min-width: 200px;">
                                <div class="div6">
                                    <img src="Images/new.png" />
                                    <asp:HyperLink ID="cmdNew" Text="Nový" runat="server" NavigateUrl="javascript:record_new();"></asp:HyperLink>
                                </div>
                                <div class="div6">
                                    <img src="Images/edit.png" />
                                    <asp:HyperLink ID="cmdEdit" Text="Upravit" runat="server" NavigateUrl="javascript:record_edit();"></asp:HyperLink>

                                </div>
                                <div class="div6">
                                    <img src="Images/copy.png" />
                                    <asp:HyperLink ID="cmdClone" Text="Kopírovat" runat="server" NavigateUrl="javascript:record_clone();"></asp:HyperLink>
                                </div>
                            </div>
                        </ContentTemplate>

                    </telerik:RadMenuItem>



                    <telerik:RadMenuItem Text="Další" ImageUrl="Images/more.png" Value="columns" PostBack="false">
                        <ContentTemplate>
                            <div style="padding: 20px;">
                                <div class="div6">
                                    <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 200px;" ToolTip="Šablony datového přehledu"></asp:DropDownList>
                                    <button type="button" onclick="griddesigner()">Sloupce</button>


                                </div>
                                <div class="div6">
                                    <img src="Images/approve.png" />
                                    <asp:HyperLink ID="cmdApprove" runat="server" Text="Schvalovat/Pře-schvalovat označené úkony" NavigateUrl="javascript:approving();"></asp:HyperLink>
                                </div>
                                <div class="div6">
                                    <img src="Images/export.png" />
                                    <asp:LinkButton ID="cmdExport" runat="server" Text="Export přehledu do MS EXCEL." />
                                </div>
                                <div class="div6">
                                    <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="Počet záznamů na stránku:"></asp:Label>
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
            <asp:LinkButton ID="cmdCĺearFilter" runat="server" Text="Vyčistit sloupcový filtr" style="margin-left:10px;font-weight:bold;color:red;"></asp:LinkButton>
        </div>
    </div>
    <div id="offsetY"></div>    
    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>



    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
