<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="x18_readonly.ascx.vb" Inherits="UI.x18_readonly" %>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <div class="div6" style="clear:both;">
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
            <asp:Repeater ID="rpItems" runat="server">
                <ItemTemplate>                    
                    <div class="badge_label" style="background-color:<%#Eval("BackColor")%>" title="<%#Eval("BindName") %>">
                    <%If hidIsLinks.Value = "1" Then%>
                    <a style="color:<%#Eval("ForeColor")%>" href="javascript:r25(<%#Eval("x25ID")%>,<%#Eval("x18ID")%>)"><%# Eval("NameWithCode") %></a>
                    <%else %>
                    <span class="val" style="color:<%#Eval("ForeColor")%>;"><%# Eval("NameWithCode") %></span>
                    <%end if %>                    
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />
<asp:HiddenField ID="hidIsLinks" runat="server" />
<script type="text/javascript">
    function r25(x25id,x18id) {
        sw_everywhere("x25_record.aspx?pid=" + x25id + "&x18id=" + x18id, "", true);
    }
</script>