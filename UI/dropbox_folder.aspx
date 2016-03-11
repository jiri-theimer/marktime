<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="dropbox_folder.aspx.vb" Inherits="UI.dropbox_folder" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:RadioButtonList ID="opgMode" runat="server" AutoPostBack="true" RepeatDirection="Vertical" CellPadding="6">
        <asp:ListItem Text="Založím novou složku do Dropbox ROOT" Value="1" Selected="true"></asp:ListItem>
        <asp:ListItem Text="Založím novou složku pod vybranou nadřízenou složku" Value="2"></asp:ListItem>
        <asp:ListItem Text="Nebudu zakládat novou složku , vyberu již existující" Value="3"></asp:ListItem>
    </asp:RadioButtonList>


    <table cellpadding="10">
        <tr>
            <td>
                <asp:Label ID="lblNewFolder" Text="Název nové složky:" runat="server"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNewFolder" runat="server" Width="400px"></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSelectedFolder" runat="server" Text="Vybraná složka:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="SelectedFolder" runat="server" CssClass="valboldblue"></asp:Label>
            </td>
        </tr>
    </table>

    <asp:Panel ID="panFolders" runat="server" CssClass="content-box2">
        <div class="title">
            <asp:Label ID="FolderHeader" Text="Nadřazená složka" runat="server"></asp:Label>
        </div>

        <div class="content">
            <table>
                <tr>
                    <asp:Repeater ID="rpBred" runat="server">
                        <ItemTemplate>
                            <td>->
                            </td>
                            <td>
                                <asp:LinkButton ID="cmdFolder" runat="server"></asp:LinkButton>
                            </td>

                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </table>
            <table cellpadding="5">
                <asp:Repeater ID="rp1" runat="server">
                    <ItemTemplate>
                        <tr class="trHover">

                            <td>
                                <asp:Image ID="img1" runat="server" Width="16px" Height="16px" />
                            </td>
                            <td>
                                <asp:HyperLink ID="clue_content" runat="server" CssClass="reczoom" Text="i" title="Dropbox detail"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:LinkButton ID="cmdFolder" runat="server"></asp:LinkButton>


                            </td>
                            <td>
                                <asp:LinkButton ID="cmdSelect" runat="server" Text="Vybrat složku" CommandName="select" Style="margin-left: 20px;"></asp:LinkButton>
                                
                            </td>
                        </tr>


                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="hidX29ID" runat="server" />
    <asp:HiddenField ID="hidRecordPID" runat="server" />
        <asp:HiddenField ID="hidO25ID" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
