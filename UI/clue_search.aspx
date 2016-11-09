<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="clue_search.aspx.vb" Inherits="UI.clue_search" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

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
            var pid = <%=Me.p41id_search.ClientID%>_get_value();
            window.open("p91_framework.aspx?pid=" + pid, "_top");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr id="trP41" runat="server">
            <td>
                <img src="Images/project.png" />
            </td>
            <td>
                Projekt:
            </td>
            <td>
                <uc:project ID="p41id_search" runat="server" Width="500px" Flag="searchbox" AutoPostBack="false" OnClientSelectedIndexChanged="p41id_search" />
            </td>
            <td>
                <asp:CheckBox ID="chkP41Bin" runat="server" Text="Zahrnout i archiv" AutoPostBack="true" />
            </td>
        </tr>
        <tr id="trP28" runat="server">
            <td>
                <img src="Images/contact.png" />
            </td>
            <td>
                Klient:
            </td>
            <td>
                <uc:contact ID="p28id_search" runat="server" Width="500px" Flag="searchbox" AutoPostBack="false" />
            </td>
            <td>
                <asp:CheckBox ID="chkP28Bin" runat="server" Text="Zahrnout i archiv" AutoPostBack="true" />
            </td>
        </tr>
        <tr id="trJ02" runat="server">
            <td>
                <img src="Images/person.png" />
            </td>
            <td>
                Osoba:
            </td>
            <td>
                <uc:person ID="j02id_search" runat="server" Width="500px" Flag="searchbox" AutoPostBack="false" />
            </td>
            <td>
                <asp:CheckBox ID="chkJ02Bin" runat="server" Text="Zahrnout i archiv" AutoPostBack="true" />
            </td>
        </tr>
    </table>
   

</asp:Content>
