<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="clue_search.aspx.vb" Inherits="UI.clue_search" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
        <Tabs>
            <telerik:RadTab Text="Hledat projekt/klienta/fakturu/osobu" Selected="true" Value="search"></telerik:RadTab>
            <telerik:RadTab Text="Fulltext" Value="fulltext"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="search" runat="server" Selected="true">
            <table>
                <tr id="trP41" runat="server">
                    <td>
                        <img src="Images/search_20.png" />
                    </td>
                    <td>Projekt:
                    </td>
                    <td>
                        <uc:project ID="p41id_search" runat="server" Width="590px" Flag="searchbox" AutoPostBack="false" OnClientSelectedIndexChanged="p41id_search" />
                    </td>


                </tr>
                <tr id="trP28" runat="server">
                    <td>
                        <img src="Images/search_20.png" />
                    </td>
                    <td>Klient:
                    </td>
                    <td>
                        <uc:contact ID="p28id_search" runat="server" Width="590px" Flag="searchbox" AutoPostBack="false" />
                    </td>


                </tr>
                <tr id="trP91" runat="server">
                    <td>
                        <img src="Images/search_20.png" />
                    </td>
                    <td>Faktura:
                    </td>
                    <td>
                        <uc:invoice ID="p91id_search" runat="server" Width="590px" Flag="searchbox" />

                    </td>

                </tr>
                <tr id="trJ02" runat="server">
                    <td>
                        <img src="Images/search_20.png" />
                    </td>
                    <td>Osoba:
                    </td>

                    <td>
                        <uc:person ID="j02id_search" runat="server" Width="590px" Flag="searchbox" AutoPostBack="false" />

                    </td>


                </tr>
            </table>
            <fieldset style="padding: 6px;" id="fsP41" runat="server">
                <legend>Vyhledávání projektu</legend>
                <p>Částečná shoda v: Název projektu | Kód projektu | Zkrácený název projektu | Název klienta | Zkrácený název klienta</p>
                <div class="div6">
                    <span>Kolik maximálně zobrazit nalezených:</span>
                    <asp:DropDownList ID="cbxP41Top" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="200" Value="200"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkP41Bin" runat="server" Text="Hledat i v archivu" AutoPostBack="true" CssClass="chk" />
                </div>
            </fieldset>
            <fieldset style="padding: 6px;" id="fsP28" runat="server">
                <legend>Vyhledávání klienta</legend>
                <p>Částečná shoda v: Název | Kód | Zkrácený název | IČ | DIČ</p>

                <div class="div6">
                    <span>Kolik maximálně zobrazit nalezených:</span>
                    <asp:DropDownList ID="cbxP28Top" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="200" Value="200"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkP28Bin" runat="server" Text="Hledat i v archivu" AutoPostBack="true" CssClass="chk" />
                </div>
            </fieldset>
            <fieldset style="padding: 6px;" id="fsP91" runat="server">
                <legend>Vyhledávání faktury</legend>
                <p>Částečná shoda v: Číslo dokladu | Název klienta (odběratele faktury) | Text faktury | IČ klienta | DIČ klienta | Název fakturovaného projektu</p>
                <div class="div6">
                    <span>Kolik maximálně zobrazit nalezených:</span>
                    <asp:DropDownList ID="cbxP91Top" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="200" Value="200"></asp:ListItem>
                    </asp:DropDownList>

                </div>
            </fieldset>
            <fieldset style="padding: 6px;" id="fsJ02" runat="server">
                <legend>Vyhledávání osoby</legend>
                <p>Částečná shoda v: Jméno | Příjmení | E-mail</p>
                <div class="div6">
                    <asp:CheckBox ID="chkJ02Bin" runat="server" Text="Hledat i v archivu" AutoPostBack="true" CssClass="chk" />
                </div>
            </fieldset>

        </telerik:RadPageView>
        <telerik:RadPageView ID="fulltext" runat="server">

        </telerik:RadPageView>
    </telerik:RadMultiPage>

</asp:Content>
