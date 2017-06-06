<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="contactpersons.ascx.vb" Inherits="UI.contactpersons" %>

<asp:Repeater ID="rpP30" runat="server">
    <ItemTemplate>
        <div style="padding: 5px;">
            <img src="Images/person.png" />
            <asp:HyperLink ID="clue_j02" runat="server" CssClass="reczoom" Text="i" title="Detail"></asp:HyperLink>

            <asp:Label ID="Person" runat="server" CssClass="valbold"></asp:Label>

            <asp:HyperLink ID="j02Email" runat="server"></asp:HyperLink>

            <asp:Label ID="j02JobTitle" runat="server" Font-Italic="true"></asp:Label>
            <asp:Label ID="j02Mobile" runat="server"></asp:Label>
        </div>
    </ItemTemplate>
</asp:Repeater>


<asp:HiddenField ID="hidIsShowClueTip" Value="1" runat="server" />
