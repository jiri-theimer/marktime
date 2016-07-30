﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="o23_list.ascx.vb" Inherits="UI.o23_list" %>

<asp:Repeater ID="rpO23" runat="server">
    <ItemTemplate>
        <div style="float: left; padding: 5px;">
            <asp:HyperLink ID="clue_o23" runat="server" CssClass="reczoom" Text="i" title="Detail dokumentu"></asp:HyperLink>

            <asp:Label ID="o24Name" runat="server" CssClass="valbold"></asp:Label>

            <asp:Image ID="img1" runat="server" ImageUrl="Images/notepad.png" />

            <asp:HyperLink ID="o23Name" runat="server"></asp:HyperLink>
            <asp:PlaceHolder ID="place1" runat="server"></asp:PlaceHolder>
            
        </div>
    </ItemTemplate>
</asp:Repeater>

<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidInhaledDataPID" runat="server" />
