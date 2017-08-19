﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="b07_list.ascx.vb" Inherits="UI.b07_list" %>



<asp:Panel ID="panHeader" runat="server" style="background: #F1F1F1;border-top: 1px solid silver;border-bottom: 1px solid silver;height: 20px;font-family: Arial;line-height: 20px;padding:6px;color: black;">
    <img src="Images/comment.png" />
    <asp:Label ID="lblHeader" runat="server" Text="Poznámky/komentáře/přílohy"></asp:Label>
    <button type="button" style="margin-left: 20px;" onclick="javascript:comment()" runat="server" id="cmdAdd" title="Přidat" class="button-link">
        <img src="Images/new.png" /></button>
</asp:Panel>

<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
       
        <asp:Panel ID="panRecord" runat="server" CssClass="box_comment">
            <table cellpadding="10" cellspacing="2">
                <tr valign="top">
                    <td style="border-right: solid 1px silver;">
                        <asp:Image ID="imgPhoto" ImageUrl="Images/nophoto.png" runat="server" Style="max-height: 80px; max-width: 80px;" />
                    </td>
                    <td>
                        <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail"></asp:HyperLink>
                        <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
                        <asp:Label ID="Author" runat="server" CssClass="timestamp" Style="padding-left: 6px;"></asp:Label>
                        
                        <div style="padding-top: 4px;">
                            <asp:HyperLink ID="aMSG" runat="server" Text="<img src='Images/files/msg_24.png'/>Otevřít v MS-OUTLOOK" Visible="false"></asp:HyperLink>
                            <asp:HyperLink ID="aEML" runat="server" Text="<img src='Images/files/eml_24.png'/>EML" Visible="false"></asp:HyperLink>
                            <asp:Label ID="aAtts" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="b07WorkflowInfo" runat="server" CssClass="val"></asp:Label>
                        </div>
                        <asp:Panel ID="pan100" runat="server" style="padding-top: 4px;max-height:100px;overflow:auto;direction:rtl;">
                            <div style="direction:ltr;">
                            <asp:Literal ID="b07Value" runat="server"></asp:Literal>
                          
                            <asp:Image ID="img1" runat="server" AlternateText="File format" ImageUrl="Images/Files/other.png" Style="vertical-align: bottom;" />
                            <asp:HyperLink ID="att1" runat="server" ToolTip="Náhled nebo stáhnout přílohu"></asp:HyperLink>
                            </div>
                        </asp:Panel>
                       

                        <div style="padding-top: 8px;">

                            <asp:HyperLink ID="aAnswer" runat="server" Text="<img src='Images/comment.png'/>Reagovat"></asp:HyperLink>
                            <asp:HyperLink ID="aDelete" runat="server" Text="<img src='Images/delete.png'/>Odstranit" Visible="false"></asp:HyperLink>


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
<asp:HiddenField ID="hidAttachmentsReadonly" runat="server" Value="0" />
<asp:HiddenField ID="hidIsClueTip" runat="server" Value="0" />
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

    function file_preview(prefix, pid) {
        ///náhled na soubor

        window.parent.sw_everywhere("fileupload_preview.aspx?prefix=" + prefix + "&pid=" + pid, "Images/attachment.png", true);

    }

</script>
