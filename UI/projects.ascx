<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="projects.ascx.vb" Inherits="UI.projects" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>

<asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label>
<div style="clear: both;">
    <asp:RadioButtonList ID="opgScope" runat="server" AutoPostBack="true" RepeatDirection="Vertical">
        <asp:ListItem Text="Všechny projekty" Value="1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Jeden nebo více projektů" Value="2"></asp:ListItem>
        <asp:ListItem Text="Pojmenovaný filtr projektů" Value="3"></asp:ListItem>
    </asp:RadioButtonList>
</div>
<asp:Panel ID="panQuery" runat="server">
    <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
    <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 170px;"></asp:DropDownList>    
</asp:Panel>
<asp:Panel ID="panManual" runat="server">
    <asp:Repeater ID="rpP28" runat="server">
        <ItemTemplate>
            <div>
                <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" CommandName="remove" ToolTip="Odstranit položku" />
                <asp:HyperLink ID="linkClient" runat="server"></asp:HyperLink>
            </div>
        </ItemTemplate>
    </asp:Repeater>
   
    <asp:Repeater ID="rpP41" runat="server">
        <ItemTemplate>
            <div>
                <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" CommandName="remove" ToolTip="Odstranit položku" />
                <asp:HyperLink ID="linkProject" runat="server"></asp:HyperLink>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div style="text-align: center;">        
        <asp:LinkButton ID="linkClearAll" runat="server" Text="Vyčistit výběr projektů"></asp:LinkButton>
    </div>


    <div>
        <uc:project ID="p41ID_Add" runat="server" Width="100%" AutoPostBack="true" Flag="searchbox" />
    </div>
    <div>
        <uc:contact ID="p28ID_Add" runat="server" Width="100%" AutoPostBack="true" Flag="searchbox" />
    </div>
   



    
</asp:Panel>

<asp:HiddenField ID="hidHeader" runat="server" />
<asp:HiddenField ID="hidP41IDs_All" runat="server" />
<asp:HiddenField ID="hidP41IDs" runat="server" />
<asp:HiddenField ID="hidP28IDs" runat="server" />


