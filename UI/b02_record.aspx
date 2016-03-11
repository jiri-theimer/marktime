<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="b02_record.aspx.vb" Inherits="UI.b02_record" %>
<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="10">
        <tr>

            <td style="width: 120px;">
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název stavu:"></asp:Label></td>
            <td>
                <asp:TextBox ID="b02Name" runat="server" Style="width: 400px;"></asp:TextBox>
            </td>
            
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCode" runat="server" CssClass="lbl" Text="Kód:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="b02Code" runat="server" Style="width: 100px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblB01ID" runat="server" CssClass="lbl" Text="Workflow šablona:"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="b01ID" runat="server" DataTextField="b01Name" DataValueField="pid" AutoPostBack="true" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
            </td>
        </tr>
        <tr>
            <td>
                <asp:label runat="server" ID="Label2" Text="Pořadí mezi stavy:"></asp:label>
            </td>
            <td>

                <telerik:RadNumericTextBox ID="b02Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
        <td><asp:Label ID="Label1" Text="Barva:" runat="server" cssclass="lbl"></asp:Label></td>
        <td>
        <telerik:RadColorPicker ID="b02Color" runat="server" CurrentColorText="Vybraná barva" NoColorText="Bez barvy" ShowIcon="true" Preset="Default" >
        </telerik:RadColorPicker>
        </td>
    </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
