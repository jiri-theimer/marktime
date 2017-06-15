<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p64_framework.aspx.vb" Inherits="UI.p64_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function record_edit() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_master("p64_record.aspx?pid=" + pid, "Images/binder.png", true);

        }

        function record_new() {
            sw_master("p64_record.aspx?pid=0", "Images/binder.png", true);


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
            sw_master("p64_record.aspx?clone=1&pid=" + pid, "Images/binder.png", true);

        }





        function hardrefresh(pid, flag) {

            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;



        }

        function report() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }

            sw_master("report_modal.aspx?prefix=p64&pid=" + pid, "Images/reporting.png");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 44px; background-color: white; border-bottom: solid 1px silver">
        <div style="float: left;">
            <img src="Images/binder_32.png" alt="Štítky" />
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Text="Šanony" Style="vertical-align: top;"></asp:Label>

        </div>
        <div class="commandcell" style="padding-left: 50px;">
            <uc:project ID="p41id_search" runat="server" Width="250px" Flag="searchbox" AutoPostBack="true" Text="Hledat projekt..." />
        </div>

        <div class="commandcell" style="padding-left: 10px;">
            <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" runat="server" Style="z-index: 3000;" ExpandAnimation-Duration="0" ExpandAnimation-Type="none" ClickToOpen="true">
                <Items>

                    <telerik:RadMenuItem Text="ZÁZNAM" Value="record" PostBack="false" ImageUrl="Images/arrow_down_menu.png">
                        <Items>
                            <telerik:RadMenuItem Value="cmdNew" Text="Nový" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="cmdEdit" Text="Upravit" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="cmdClone" Text="Kopírovat" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" Visible="false"></telerik:RadMenuItem>
                            <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Value="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:RadMenuItem>

                        </Items>
                    </telerik:RadMenuItem>


                    <telerik:RadMenuItem Text="Obnovit" Visible="false" ImageUrl="Images/refresh.png" Value="refresh" NavigateUrl="p64_framework.aspx"></telerik:RadMenuItem>

                    <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/menuarrow.png" Value="columns" PostBack="false">
                        <ContentTemplate>
                            <asp:Panel ID="panExport" runat="server" CssClass="content-box3">
                                <div class="title">
                                    <img src="Images/export.png" />
                                    <span>Export záznamů aktuálního přehledu</span>
                                </div>
                                <div class="content">

                                    <img src="Images/xls.png" alt="xls" />
                                    <asp:HyperLink ID="cmdXLS" runat="server" Text="XLS" NavigateUrl="javascript:hardrefresh(0,'xls')" ToolTip="Export do XLSX"></asp:HyperLink>


                                    <img src="Images/pdf.png" alt="pdf" />
                                    <asp:HyperLink ID="cmdPDF" runat="server" Text="PDF" NavigateUrl="javascript:hardrefresh(0,'pdf')" ToolTip="Export do PDF"></asp:HyperLink>

                                    <img src="Images/doc.png" alt="doc" />
                                    <asp:HyperLink ID="cmdDOC" runat="server" Text="DOC" NavigateUrl="javascript:hardrefresh(0,'doc')" ToolTip="Export do DOCX"></asp:HyperLink>

                                </div>
                            </asp:Panel>

                            <div class="content-box3">
                                <div class="title">
                                </div>
                                <div class="content">
                                    <div class="div6">
                                        <asp:DropDownList ID="cbxValidity" runat="server" AutoPostBack="true" Visible="false">
                                            <asp:ListItem Text="Zobrazovat otevřené i přesunuté do archivu" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Zobrazovat pouze otevřené (mimo archiv)" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Zobrazovat pouze přesunuté do archivu" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="div6">
                                        <span>Stránkování:</span>
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

                        </ContentTemplate>
                    </telerik:RadMenuItem>
                </Items>
            </telerik:RadMenu>

        </div>
        
        <div class="commandcell">
            <asp:Label ID="lblMessage" runat="server" CssClass="valboldred"></asp:Label>
        </div>
    </div>
    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick" AllowFilteringByColumn="true"></uc:datagrid>


    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>

