<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="dropbox_clue_metadata.aspx.vb" Inherits="UI.dropbox_clue_metadata" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:Panel ID="panContainer" runat="server" Style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
    <div>
        <asp:hyperlink ID="HeaderPath" runat="server" Target="_blank" Font-Bold="true" Font-Size="120%"></asp:hyperlink>
    </div>
    <asp:Label ID="lblMessage" runat="server" CssClass="failureNotification"></asp:Label>
    <table cellpadding="5">
        <asp:Repeater ID="rp1" runat="server">
            <ItemTemplate>
                <tr class="trHover">
                   
                    <td>
                        <asp:Image ID="img1" runat="server" Width="16px" Height="16px" />
                    </td>
                    <td>
                        <asp:HyperLink ID="Folder" runat="server" Target="_blank"></asp:HyperLink>
                        <asp:Label ID="File" runat="server" CssClass="val" ForeColor="navy"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="Size" runat="server" CssClass="val" ForeColor="green" ToolTip="Velikost"></asp:Label>
                    </td>
                  
                    <td>
                        <asp:Label ID="Modified" runat="server" Font-Italic="true"></asp:Label>
                    </td>
                </tr>

            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>
</asp:Content>
