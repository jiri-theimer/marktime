﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="x25_record_readonly.ascx.vb" Inherits="UI.x25_record_readonly" %>

    
    <table cellpadding="5" cellspacing="2" style="width: 100%;" id="responsive">    
        
        <tr class="trHover" id="trCode" runat="server">
            <td style="width:200px;">
                <span class="lbl"><img src="Images/type_text.png" /> Kód:</span>
            </td>
            <td>
                <asp:Label ID="x25Code" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
        <tr class="trHover">
            <td style="width:200px;">
                <span class="lbl"><img src="Images/type_text.png" /> Název:</span>
            </td>
            <td>
                <asp:Label ID="x25Name" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
        
        <asp:Repeater ID="rpFF" runat="server">
            <ItemTemplate>
           
                <tr class="trHover" style="vertical-align: top;">
                    <td>

                        <asp:Label ID="x16Name" runat="server" CssClass="lbl"></asp:Label>

                    </td>
                    <td>

                        <asp:Label ID="valFF" runat="server" CssClass="valbold"></asp:Label>

                        <asp:HiddenField runat="server" ID="hidField" />
                        <asp:HiddenField runat="server" ID="hidType" />
                        <asp:HiddenField ID="hidX16ID" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        
        <asp:Repeater ID="rpX19" runat="server">
            <ItemTemplate>
           
                <tr class="trHover" style="vertical-align: top;">
                    <td style="width:200px;">

                        <asp:Label ID="BindName" runat="server" CssClass="lbl"></asp:Label>

                    </td>
                    <td>

                        <asp:Label ID="BindValue" runat="server" CssClass="valbold"></asp:Label>

                        
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
<div class="div6">
    <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
</div>

<asp:HiddenField ID="hidX18ID" runat="server" />

