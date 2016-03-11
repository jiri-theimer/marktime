<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p91_subgrid.ascx.vb" Inherits="UI.p91_subgrid" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<div class="div6">
    <img src="Images/invoice.png" alt="Faktury" />
    <asp:Label ID="lblHeaderP91" CssClass="framework_header_span" runat="server" Text="Vystavené faktury"></asp:Label>
    <asp:DropDownList ID="cbxPeriodType" AutoPostBack="true" runat="server">
        <asp:ListItem Text="Datum plnění" Value="p91DateSupply" Selected="true"></asp:ListItem>
        <asp:ListItem Text="Datum splatnosti" Value="p91DateMaturity"></asp:ListItem>
        <asp:ListItem Text="Datum vystavení" Value="p91Date"></asp:ListItem>
    </asp:DropDownList>
    <uc:periodcombo ID="period1" runat="server" Width="200px"></uc:periodcombo>
</div>
<uc:datagrid ID="gridP91" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected_p91" OnRowDblClick="RowDoubleClick_p91"></uc:datagrid>

