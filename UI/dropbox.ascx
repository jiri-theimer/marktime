<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="dropbox.ascx.vb" Inherits="UI.dropbox" %>
<div class="div6">
    <asp:HyperLink ID="rootURL" runat="server" NavigateUrl="https://www.dropbox.com/home" Target="_blank" Text="Dropbox ROOT"></asp:HyperLink>
    <asp:linkbutton ID="cmdInhaleAccessToken" runat="server" Text="Přidat projektovou složku" style="padding-left:20px;"></asp:linkbutton>
    <asp:HyperLink ID="cmdFolder" runat="server" NavigateUrl="javascript:dropbox_folder()" style="padding-left:20px;"></asp:HyperLink>
</div>
<table>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <tr>
            <td>
                <asp:HyperLink ID="clue_content" runat="server" CssClass="reczoom" Text="i" title="Dropbox detail"></asp:HyperLink>
            </td>
            <td>
                <asp:Image ID="img1" runat="server" Width="16px" Height="16px" />
            </td>
            <td>
                <asp:HyperLink ID="o25Path" runat="server" Target="_blank"></asp:HyperLink>
            </td>
            <td style="padding-left:15px;">
                <asp:HyperLink ID="cmdEdit" runat="server" Text="Změnit složku"></asp:HyperLink>
                <asp:linkbutton ID="cmdInhaleAccessToken" runat="server" text="Zavolat Dropbox" />
            </td>
        </tr>
       
    </ItemTemplate>
</asp:Repeater>
</table>
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />