<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_o24_record.aspx.vb" Inherits="UI.clue_o24_record" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <div>
           <img src="Images/notepad_32.png" />
           <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
       </div>

        <table cellpadding="5" cellspacing="2">
            <tr>
                <td>Dokument se váže k entitě:</td>
                <td>
                    <asp:Label ID="x29Name" runat="server" CssClass="valbold"></asp:Label>

                </td>
             
            </tr>
            <tr>
                <td>Max.povolená velikost 1 souboru:</td>
                <td>
                    <asp:Label ID="o24MaxOneFileSize" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Povolené formáty (přípony) nahrávaných souborů:</td>
                <td>
                    <asp:Label ID="o24AllowedFileExtensions" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
        </table>
        
        <asp:panel ID="panHelp" runat="server" CssClass="bigtext">
            <img src="Images/help2.png" />
            <asp:Label ID="o24HelpText" runat="server" Style="font-family: 'Courier New'; word-wrap: break-word; display: block;"></asp:Label>
           
        </asp:panel>
       
       
        <div>
            <span class="lbl">Povinnost vazby k záznamu entity:</span>
            
        </div>
        <div>
            <asp:Label ID="lblBindInfo" runat="server" CssClass="valbold"></asp:Label>
        </div>
    </div>
</asp:Content>
