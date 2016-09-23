<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="admin_smtp.aspx.vb" Inherits="UI.admin_smtp" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="div6">
        <asp:Label ID="Label2" runat="server" Text="Globální e-mail adresa odesílatele:"></asp:Label>
        <asp:TextBox ID="SMTP_SenderAddress" runat="server" Style="width: 150px;"></asp:TextBox>

    </div>
    <div class="div6">
        <asp:CheckBox ID="SMTP_SenderIsUser" runat="server" Text="Adresa odesílatele bude e-mail přihlášeného uživatele" />
    </div>
    <div class="div6">
        <asp:Label ID="Label1" runat="server" Text="Aplikační adresa (Host URL):"></asp:Label>
        <asp:TextBox ID="AppHost" runat="server" Style="width: 300px;"></asp:TextBox>
        <span class="infoInForm">Tato URL adresa se bude zobrazovat v notifikačních zprávách</span>
    </div>
    <fieldset>        
    <div class="div6">
        <asp:CheckBox ID="chkIsSMTP_UseWebConfigSetting" runat="server" Text="Používat výchozí SMTP server z globálního nastavení (web.config)" AutoPostBack="true"></asp:CheckBox>

    </div>
    <asp:Panel ID="panWebConfig" runat="server">
        <table cellpadding="3" cellspacing="2">
            <tr>
                <td>
                    <asp:Label ID="Datalabel1" runat="server" Text="SMTP server:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="default_server" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Adresa odesílatele:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="default_sender" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </fieldset>


    <asp:Panel ID="panRec" runat="server">
        <table cellpadding="6" cellspacing="2">
            <tr>
                <td>
                    <asp:Label ID="lblServer" runat="server" Text="SMTP server:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="SMTP_Server" runat="server" Style="width: 300px;"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblLogin" runat="server" Text="SMTP login:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="SMTP_Login" runat="server" Style="width: 300px;" Text="inbox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="SMTP_IsVerify" runat="server" Text="SMTP server vyžaduje ověření"></asp:CheckBox>
                </td>

            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="SMTP heslo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="SMTP_Password" runat="server" Style="width: 300px;" TextMode="Password"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblVerifyPassword" runat="server" Text="Ověření hesla:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVerify" runat="server" Style="width: 300px;" TextMode="Password"></asp:TextBox>
                </td>
            </tr>

        </table>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>

