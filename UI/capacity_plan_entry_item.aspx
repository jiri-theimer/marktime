<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="capacity_plan_entry_item.aspx.vb" Inherits="UI.capacity_plan_entry_item" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblHeader" runat="server" CssClass="clue_header_span"></asp:Label>
    <table cellpadding="6">
        <tr>
            <th>Měsíc</th>
            <th title="Plán fakturovatelných hodin">Fa</th>
            <th title="Plán nefakturovatelných hodin">NeFa</th>
        </tr>
        <asp:Repeater ID="rp1" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:label ID="Mesic" runat="server"></asp:label>
                        <asp:HiddenField ID="p85id" runat="server" />
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="p85FreeFloat01" runat="server" Width="60px" ToolTip="Fakturovatelné hodiny" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                        
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="p85FreeFloat02" runat="server" Width="60px" ToolTip="Nefakturovatelné hodiny" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                        
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
