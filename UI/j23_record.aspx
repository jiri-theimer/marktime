<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="j23_record.aspx.vb" Inherits="UI.j23_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/PageHeader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název:"></asp:Label></td>
            <td>
                <asp:TextBox ID="j23Name" runat="server" Style="width: 400px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCode" Text="Kód:" runat="server" CssClass="lbl" AssociatedControlID="j23Code"></asp:Label>

            </td>
            <td>
                <asp:TextBox ID="j23Code" runat="server" Style="width: 100px;"></asp:TextBox>




            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblJ24ID" Text="Typ zdroje:" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="j24ID" runat="server" DataTextField="j24Name" DataValueField="pid" IsFirstEmptyRow="true"></uc:datacombo>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOrdinary" Text="Index pořadí v rámci typu:" runat="server" CssClass="lbl" AssociatedControlID="j23Ordinary"></asp:Label></td>
            <td>

                <telerik:RadNumericTextBox ID="j23Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
