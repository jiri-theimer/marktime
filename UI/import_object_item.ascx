<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="import_object_item.ascx.vb" Inherits="UI.import_object_item" %>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <div class="div6">
            <asp:CheckBox ID="chk1" runat="server" Checked="true" />
            
            <asp:HyperLink ID="link1" runat="server"></asp:HyperLink>
            <asp:Label ID="FileSize" runat="server"></asp:Label>
        </div>
    </ItemTemplate>    
</asp:Repeater>

<asp:HiddenField ID="hidGUID" runat="server" />
<asp:HiddenField ID="hidPrefix" runat="server" />