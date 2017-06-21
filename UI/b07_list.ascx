<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="b07_list.ascx.vb" Inherits="UI.b07_list" %>
<asp:panel ID="panHeader" runat="server">
    <img src="Images/comment.png" />
    <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Text="Poznámky/komentáře/přílohy"></asp:Label>
    <button type="button" style="margin-left: 20px;" onclick="javascript:comment()" runat="server" id="cmdAdd">Přidat</button>
</asp:panel>

<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <asp:Panel ID="panRecord" runat="server" cssclass="box_comment">
            <table cellpadding="10" cellspacing="2">
                <tr valign="top">
                    <td style="border-right:solid 1px silver;">
                        <asp:Image ID="imgPhoto" src="Images/nophoto.png" runat="server" />
                    </td>
                    <td>

                        <asp:Label ID="Author" runat="server" CssClass="valbold"></asp:Label>
                        <div>
                        <asp:Label ID="b07WorkflowInfo" runat="server" CssClass="valboldred"></asp:Label>
                        </div>
                        <div style="padding-top: 4px;">
                            <asp:Label ID="b07Value" runat="server" Style="font-style: italic; font-size: 110%;"></asp:Label>
                            <asp:HyperLink ID="att1" runat="server"></asp:HyperLink>
                        </div>
                      
                        <div style="padding-top: 4px;">
                            <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
                            <asp:HyperLink ID="aAnswer" runat="server" Text="Reagovat"></asp:HyperLink>
                            <asp:HyperLink ID="aDelete" runat="server" Text="Odstranit" Visible="false"></asp:HyperLink>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </ItemTemplate>
</asp:Repeater>

<asp:HiddenField ID="hidPrefix" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />
<asp:HiddenField ID="hidJS_Create" runat="server" Value="comment()" />
<asp:HiddenField ID="hidJS_Reaction" runat="server" Value="reaction" />
<script type="text/javascript">
    function comment() {
        window.parent.sw_everywhere("b07_create.aspx?masterprefix=" + document.getElementById("<%=Me.hidPrefix.ClientID%>").value + "&masterpid=" + document.getElementById("<%=Me.hidRecordPID.ClientID%>").value, "Images/comment.png", true);

    }
    function reaction(pid) {

        window.parent.sw_everywhere("b07_create.aspx?parentpid=" + pid + "&masterprefix=" + document.getElementById("<%=Me.hidPrefix.ClientID%>").value + "&masterpid=" + document.getElementById("<%=Me.hidRecordPID.ClientID%>").value, "Images/comment.png", true);

    }
    function trydeleteb07(pid) {

        window.parent.sw_everywhere("b07_delete.aspx?pid=" + pid, "Images/comment.png", true);

    }
    
</script>
