<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="j03_record.aspx.vb" Inherits="UI.j03_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/PageHeader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        function passwordrecovery() {
            if (confirm("Chcete uživateli vygenerovat nové přístupové heslo?")) {
                return (true);
            }
            else
                return (false);

        }

        function changelogin() {
            var s = window.prompt("Zadejte nové přihlašovací jméno (změnou dojde automaticky k vygenerování nového přístupového hesla)");

            if (s != '' && s != null) {
                self.document.getElementById("<%=hidNewLogin.clientid%>").value = s;

                return (true);
            }

            return (false);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="3" cellspacing="2">
        <tr>
            <td style="width: 160px;">
                <asp:Label ID="lblJ03Login" Text="Přihlašovací jméno (login):" runat="server" AssociatedControlID="j03login" CssClass="lblReq"></asp:Label></td>
            <td>
                <asp:TextBox ID="j03login" runat="server" Style="width: 300px;" Enabled="false"></asp:TextBox>
                <asp:CheckBox ID="j03IsLiveChatSupport" runat="server" Text="Zapnutá Live chat MARKTIME podpora" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblLoginWarning" runat="server" Style="color: Red;" />
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="lblRole" Text="Aplikační role:" runat="server" CssClass="lblReq" AssociatedControlID="j04id"></asp:Label></td>
            <td>
                <uc:datacombo ID="j04id" runat="server" DataTextField="j04name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblJ02ID" runat="server" Text="Osobní profil:" CssClass="lblReq"></asp:Label>

            </td>
            <td>
                <uc:person ID="j02ID" runat="server" Width="300px" />

            </td>
        </tr>
    </table>

    <asp:Panel ID="panService" runat="server" CssClass="content-box2" Style="margin-top: 20px;">
        <div class="title">
            <asp:Label ID="ph3" runat="server" Text="Servisní funkce" />
        </div>
        <div class="content">
            <asp:Button ID="cmdRecoveryPassword" runat="server" Text="Vygenerovat nové přístupové heslo" CssClass="cmd" OnClientClick="return passwordrecovery()" />
            <asp:Button ID="cmdChangeLogin" runat="server" Text="Změnit přihlašovací jméno" CssClass="cmd" OnClientClick="return changelogin()" />
            <asp:Button ID="cmdDeleteUserParams" runat="server" text="Vyčistit paměť (cache) v uživatelském profilu" CssClass="cmd" />
            <asp:Button ID="cmdRecoveryMembership" runat="server" Text="Opravit membership účet" CssClass="cmd" />


            <div>
                <asp:Button ID="cmdUnlockMembership" runat="server" Text="Odblokovat membership účet" CssClass="cmd" Visible="false" />
            </div>
            <asp:Panel ID="panPasswordRecovery" runat="server" Style="background-color: orange;" Visible="false">
                <div style="padding: 6px;">
                    <asp:Label ID="lblNewPasswordLabel" runat="server" Text="Nové přístupové heslo:"></asp:Label>
                    <asp:Label ID="lblNewPassword" runat="server" Style="margin-left: 20px; font-weight: bold; font-size: 20px;"></asp:Label>
                    <asp:Button ID="cmdResetPasswordMessage" runat="server" Text="Odeslat zprávu o novém heslu" CssClass="cmd" />
                </div>
            </asp:Panel>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hidNewLogin" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
