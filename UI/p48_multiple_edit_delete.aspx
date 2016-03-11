<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p48_multiple_edit_delete.aspx.vb" Inherits="UI.p48_multiple_edit_delete" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <table cellpadding="6" cellspacing="2">
        <tr>
            <th></th>
            <th>Osoba</th>
            <th>Datum</th>
            
            <th>Projekt/Sešit/Aktivita</th>
            <th>Hodiny</th>
            <th>Text</th>
        </tr>
        <asp:Repeater ID="rp1" runat="server">
            <ItemTemplate>
                <tr class="trHover">
                    <td style="width:20px;">
                        <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" CommandName="delete" ToolTip="Odstranit položku" />
                        
                    </td>
                    <td>
                            
                        <asp:Label ID="Person" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="p48Date" runat="server" ForeColor="blue"></asp:Label>
                        <asp:HiddenField ID="j02id" runat="server" />
                        <asp:HiddenField ID="p41id" runat="server" />
                        <asp:HiddenField ID="date" runat="server" />
                        <asp:HiddenField ID="p85id" runat="server" />
                    </td>
                    
                    <td>
                        <asp:Label ID="Project" runat="server"></asp:Label>
                        <div>
                            <asp:Label ID="p34Name" runat="server"></asp:Label>
                            <asp:DropDownList ID="p32ID" runat="server" DataTextField="p32Name" DataValueField="pid" style="width:200px;"></asp:DropDownList>
                        </div>
                    </td>

                    <td>
                        <telerik:RadNumericTextBox ID="p48Hours" runat="server" Width="60px"></telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="p48Text" runat="server" Style="width: 250px;"></asp:TextBox>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
