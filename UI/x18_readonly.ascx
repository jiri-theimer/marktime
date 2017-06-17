<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="x18_readonly.ascx.vb" Inherits="UI.x18_readonly" %>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <div class="div6" style="clear:both;">
          
            <asp:Repeater ID="rpItems" runat="server">
                <ItemTemplate>
                    <div class="badge_label" style="background-color:<%#Eval("BackColor")%>" title="<%#Eval("x18Name") %>">
                    <span style="color:<%#Eval("ForeColor")%>"><%# Eval("NameWithCode") %></span>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />