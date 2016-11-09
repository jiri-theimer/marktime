<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="clue_search.aspx.vb" Inherits="UI.clue_search" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="invoice" Src="~/invoice.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function p41id_search(sender, eventArgs) {
            //var item = eventArgs.get_item();
            var pid = <%=Me.p41id_search.ClientID%>_get_value();
            window.open("p41_framework.aspx?pid=" + pid, "_top");
        }
        function p28id_search(sender, eventArgs) {                        
            var pid = <%=Me.p28id_search.ClientID%>_get_value();
            window.open("p28_framework.aspx?pid=" + pid, "_top");
        }
        function j02id_search(sender, eventArgs) {
            var pid = <%=Me.j02id_search.ClientID%>_get_value();
            window.open("j02_framework.aspx?pid=" + pid, "_top");
        }
        function p91id_search(sender, eventArgs) {
            //var item = eventArgs.get_item();
            var pid = <%=Me.p91id_search.ClientID%>_get_value();
            window.open("p91_framework.aspx?pid=" + pid, "_top");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr id="trP41" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>
                Projekt:
            </td>
            <td>
                <uc:project ID="p41id_search" runat="server" Width="420px" Flag="searchbox" AutoPostBack="false" OnClientSelectedIndexChanged="p41id_search" />
            </td>
            <td>
                <asp:CheckBox ID="chkP41Bin" runat="server" Text="Hledat i v archivu" AutoPostBack="true" CssClass="chk" />
            </td>
            <td>
                <asp:DropDownList ID="cbxP41Top" runat="server" ToolTip="Kolik maximálně zobrazit nalezených projektů" AutoPostBack="true">
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trP28" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>
                Klient:
            </td>
            <td>
                <uc:contact ID="p28id_search" runat="server" Width="420px" Flag="searchbox" AutoPostBack="false" />
            </td>
            <td>
                <asp:CheckBox ID="chkP28Bin" runat="server" Text="Hledat i v archivu" AutoPostBack="true" CssClass="chk" />
            </td>
            <td>
                <asp:DropDownList ID="cbxP28Top" runat="server" ToolTip="Kolik maximálně zobrazit nalezených klientů" AutoPostBack="true">
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trP91" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>
                Faktura:
            </td>
            <td>
                <uc:invoice ID="p91id_search" runat="server" Width="420px" Flag="searchbox" />
               
            </td>
            <td>
                
            </td>
            <td>
               
            </td>
        </tr>
        <tr id="trJ02" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>
                Osoba:
            </td>
            <td>
                <uc:person ID="j02id_search" runat="server" Width="420px" Flag="searchbox" AutoPostBack="false" />
            </td>
            <td>
                <asp:CheckBox ID="chkJ02Bin" runat="server" Text="Hledat i v archivu" AutoPostBack="true" CssClass="chk" />
            </td>
        </tr>
    </table>
   

</asp:Content>
