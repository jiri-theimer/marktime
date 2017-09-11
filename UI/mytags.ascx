<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="mytags.ascx.vb" Inherits="UI.mytags" %>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <div class="badge_tag">
            <asp:Label ID="o51Name" runat="server"></asp:Label>
            
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:HyperLink ID="cmdTags" runat="server" Text="Zatím žádné štítky" NavigateUrl="javascript:tags_assign()"></asp:HyperLink>
<asp:HiddenField ID="hidPrefix" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />

<script type="text/javascript">
    
    function tags_assign() {
        var prefix = document.getElementById("<%=hidPrefix.ClientID%>").value;
        var recpid = document.getElementById("<%=hidRecordPID.ClientID%>").value;
        
        sw_everywhere("tag_binding.aspx?prefix=" + prefix + "&pids="+recpid, "Images/tag.png", true);

        //sw_everywhere("tag_binding.aspx?prefix"+prefix+"&pids="+recpid, "", true);
    }
</script>
