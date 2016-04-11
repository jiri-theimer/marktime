<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="x18_readonly.ascx.vb" Inherits="UI.x18_readonly" %>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <div class="div6">
            <asp:Label ID="x18Name" runat="server"></asp:Label>
            <asp:Label ID="items" runat="server" CssClass="valbold"></asp:Label>
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />