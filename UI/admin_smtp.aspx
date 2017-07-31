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
    <div class="div6" style="display:none;">
        <asp:CheckBox ID="SMTP_SenderIsUser" runat="server" Text="Adresa odesílatele bude e-mail přihlášeného uživatele" />
    </div>
    <div class="div6">
        <asp:Label ID="Label1" runat="server" Text="Aplikační adresa (Host URL):"></asp:Label>
        <asp:TextBox ID="AppHost" runat="server" Style="width: 300px;"></asp:TextBox>
        <span class="infoInForm">Tato URL adresa se bude zobrazovat v notifikačních zprávách</span>
    </div>
    <fieldset>        
    <div class="div6">
        <asp:CheckBox ID="chkIsSMTP_UseWebConfigSetting" runat="server" Text="Aplikační SMTP server je nastaven v konfiguračním souboru web.config" AutoPostBack="true" CssClass="chk"></asp:CheckBox>

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
                    <asp:Label ID="lblO40ID" runat="server" Text="Vybrat zavedený SMTP účet:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbxO40ID" runat="server" DataValueField="pid" DataTextField="o40Name"></asp:DropDownList>
                </td>
            </tr>

            

        </table>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>

