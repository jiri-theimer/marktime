<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="x18_framework_detail.aspx.vb" Inherits="UI.x18_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function griddesigner() {
            var j74id = "<%=Me.CurrentJ74ID%>";
            sw_local("grid_designer.aspx?nodrilldown=1&prefix=<%=Me.CurrentPrefix%>&pid=" + j74id, "Images/griddesigner_32.png");
        }

        function RowSelected(sender, args) {
            var pid = args.getDataKeyValue("pid");
            document.getElementById("<%=hiddatapid.clientid%>").value = pid;

            

        }

        function RowDoubleClick(sender, args) {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            location.replace("<%=Me.CurrentPrefix%>_framework_detail.aspx?pid=" + pid);
        }

        function hardrefresh(pid, flag) {
            location.replace("x18_framework_detail.aspx");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-box1" style="margin-bottom:6px;">
        <div class="title">
            <asp:Image ID="imgRecord" runat="server" ImageUrl="Images/label.png" Style="margin-right: 10px;" />
            <asp:Label ID="boxCoreTitle" Text="Záznam štítku" runat="server"></asp:Label>
            <asp:HyperLink ID="cmdNewWindow" runat="server" ImageUrl="Images/open_in_new_window.png" Target="_blank" ToolTip="Otevřít v nové záložce" CssClass="button-link" Style="float: right; vertical-align: top; padding: 0px;"></asp:HyperLink>
        </div>
        <div class="content">
            <asp:Label ID="lblPermissionMessage" runat="server" CssClass="infoNotificationRed"></asp:Label>
            <table cellpadding="10" cellspacing="2" id="responsive">
                <tr>
                    
               
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Položka:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="x25Name" runat="server" CssClass="valbold"></asp:Label>
                    </td>

                    <td >
                        <asp:Label ID="lblName" runat="server" Text="Datový zdroj:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="x23Name" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                </tr>


            </table>


            <asp:Label ID="Timestamp" runat="server" CssClass="timestamp" Visible="false"></asp:Label>
        </div>
    </div>

    <div style="clear: both; width: 100%;"></div>
    <telerik:RadTabStrip ID="tabs1" runat="server" Skin="Metro" Width="100%" AutoPostBack="true">
        <Tabs>
            <telerik:RadTab Text="Projekty" Value="p41" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Klienti" Value="p28"></telerik:RadTab>
            <telerik:RadTab Text="Osoby" Value="j02"></telerik:RadTab>
            <telerik:RadTab Text="Vystavené faktury" Value="p91"></telerik:RadTab>
            <telerik:RadTab Text="Úkoly" Value="p56"></telerik:RadTab>
            <telerik:RadTab Text="Dokumenty" Value="o23"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <div>
    <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 180px;" ToolTip="Šablony datového přehledu"></asp:DropDownList>
    <button type="button" onclick="griddesigner()" id="cmdGridDesiger" runat="server">Sloupce</button>
    </div>

    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick" Skin="Default"></uc:datagrid>

    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidDefaultSorting" runat="server" />
    <asp:HiddenField ID="hidCols" runat="server" />
</asp:Content>
