<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_search.aspx.vb" Inherits="UI.mobile_search" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="invoice" Src="~/invoice.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function p41id_search(sender, eventArgs) {
            //var item = eventArgs.get_item();
            var pid = <%=Me.p41id_search.ClientID%>_get_value();
            location.replace("mobile_p41_framework.aspx?pid=" + pid);
        }
        function p28id_search(sender, eventArgs) {
            var pid = <%=Me.p28id_search.ClientID%>_get_value();
            location.replace("mobile_p28_framework.aspx?pid=" + pid);
        }
        function j02id_search(sender, eventArgs) {
            var pid = <%=Me.j02id_search.ClientID%>_get_value();
            location.replace("mobile_j02_framework.aspx?pid=" + pid);
        }
        function p91id_search(sender, eventArgs) {
            //var item = eventArgs.get_item();
            var pid = <%=Me.p91id_search.ClientID%>_get_value();
            location.replace("mobile_p91_framework.aspx?pid=" + pid);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="well">
        Naposledy hledané záznamy:
        <div></div>
    </div>
    <div>
    <img src="Images/search_20.png" />
    Projekt:
    </div>
    <div>
        <uc:project ID="p41id_search" runat="server" Width="99%" Flag="searchbox" AutoPostBack="false" OnClientSelectedIndexChanged="p41id_search" />
    </div>
    <table class="table table-hover">
        <tr id="trP41" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>Projekt:
            </td>
            <td>
                
            </td>


        </tr>
        <tr id="trP28" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>Klient:
            </td>
            <td>
                <uc:contact ID="p28id_search" runat="server" Width="600px" Flag="searchbox" AutoPostBack="false" />
            </td>


        </tr>
        <tr id="trP91" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>Faktura:
            </td>
            <td>
                <uc:invoice ID="p91id_search" runat="server" Width="600px" Flag="searchbox" />

            </td>

        </tr>
        <tr id="trJ02" runat="server">
            <td>
                <img src="Images/search_20.png" />
            </td>
            <td>Osoba:
            </td>

            <td>
                <uc:person ID="j02id_search" runat="server" Width="600px" Flag="searchbox" AutoPostBack="false" />

            </td>


        </tr>
    </table>
</asp:Content>
