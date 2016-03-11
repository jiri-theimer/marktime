<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="contactpersons.ascx.vb" Inherits="UI.contactpersons" %>

<asp:Repeater ID="rpP30" runat="server">
    <ItemTemplate>
        <div style="padding: 5px; float: left;">
            <asp:HyperLink ID="clue_j02" runat="server" CssClass="reczoom" Text="i" title="Detail"></asp:HyperLink>

            <asp:Label ID="Person" runat="server" CssClass="valbold"></asp:Label>

            <asp:HyperLink ID="j02Email" runat="server"></asp:HyperLink>
        </div>
    </ItemTemplate>
</asp:Repeater>


<asp:HiddenField ID="hidIsShowClueTip" Value="1" runat="server" />
