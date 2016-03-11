<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p58_record.aspx.vb" Inherits="UI.p58_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table cellpadding="6" cellspacing="2">
        
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název produktu:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="p58Name" runat="server" Style="width: 400px;"></asp:TextBox>

                <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl"></asp:Label>
                <telerik:RadNumericTextBox ID="p58Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
               
            </td>
        </tr>


        <tr>
            <td>
                <asp:Label ID="lblCode" runat="server" CssClass="lbl" Text="Kód:"></asp:Label>
            </td>
            <td>


                <asp:TextBox ID="p58Code" runat="server" Style="width: 100px;"></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td>
                 <asp:Label ID="lblParentID" runat="server" CssClass="lbl" Text="Nadřízený produkt:"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="p58ParentID" runat="server" DataTextField="TreeMenuItem" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
            </td>
        </tr>
       
       
    </table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
