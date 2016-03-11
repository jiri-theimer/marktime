<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Anonym.Master" CodeBehind="kickoff_after2.aspx.vb" Inherits="UI.kickoff_after2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="div6">
        <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Dokončení prvního nastavení systému"></asp:Label>
    </div>
    <div class="content-box2">
        <div class="title">Složka pro upload souborových příloh</div>
        <div class="content">
            Upload složka (server cesta):
            <asp:TextBox ID="txtUploadFolder" runat="server" Width="400px"></asp:TextBox>
            <div>
                <span class="infoInForm">Do server složky musíte nastavit file-systém oprávnění [Modify] pro IIS uživatele.</span>
            </div>
        </div>
        
    </div>
    <div class="content-box2">
        <div class="title">URL adresa pro spouštění MARKTIME robota</div>
        <div class="content">
            URL adresa:
            <asp:TextBox ID="AppHost" runat="server" Width="400px"></asp:TextBox>
            <div>
                <span class="infoInForm">Robot běží na pozadí a stará se o automatické procesy jako např. rozesílání notifikačních zpráv, generování opakovaných úkonů atd.</span>
                <div>
                    <span class="infoInForm">URL musí být spustitelná uvnitř na IIS serveru (localhost).</span>
                </div>
            </div>
        </div>
    </div>
    <p></p>
    <div class="div6">
        <asp:CheckBox ID="chkSmtp" runat="server" Text="Nyní nastavit SMTP server pro odesílání poštovních zpráv" Checked="true" AutoPostBack="true" />
    </div>
    <div class="div6">
        <span>E-mail adresa odesílatele:</span>
        <asp:TextBox ID="SMTP_SenderAddress" runat="server" Style="width: 150px;" Text="nasefirma@marktime.cz"></asp:TextBox>
    </div>
    <div>
        SMTP server můžete nastavit i přes konfigurační soubor WEB.config.
    </div>
    <asp:panel ID="panSMTP" runat="server" CssClass="content-box2">
        <div class="title">Nastavení SMTP serveru</div>
        <div class="content">
            <table cellpadding="6" cellspacing="2">

            <tr>
                <td>
                    SMTP server:
                </td>
                <td>
                    <asp:TextBox ID="SMTP_Server" runat="server" Style="width: 300px;"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    SMTP login:
                </td>
                <td>
                    <asp:TextBox ID="SMTP_Login" runat="server" Style="width: 300px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="SMTP_IsVerify" runat="server" Text="SMTP server vyžaduje ověření"></asp:CheckBox>
                </td>

            </tr>
            <tr>
                <td>
                    SMTP heslo:
                </td>
                <td>
                    <asp:TextBox ID="SMTP_Password" runat="server" Style="width: 300px;"></asp:TextBox>
                </td>
            </tr>

            

        </table>
        </div>
    </asp:panel>

    <div class="div6">
        <asp:Button ID="cmdGo" runat="server" CssClass="cmd" Text="Uložit nastavení a pokračovat ->" />
        <asp:Label ID="lblError" runat="server" ForeColor="red" Font-Size="120%" Font-Bold="true"></asp:Label>
    </div>
</asp:Content>
