<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="j24_record.aspx.vb" Inherits="UI.j24_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/PageHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název:"></asp:Label></td>
            <td>
                <asp:TextBox ID="j24Name" runat="server" Style="width: 400px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl" AssociatedControlID="j24Ordinary"></asp:Label></td>
            <td>

                <telerik:RadNumericTextBox ID="j24Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
