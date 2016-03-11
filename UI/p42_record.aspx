<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p42_record.aspx.vb" Inherits="UI.p42_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="3" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název:"></asp:Label></td>
            <td>
                <asp:TextBox ID="p42Name" runat="server" Style="width: 400px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblX38ID" Text="Číselná řada projektů:" runat="server" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="x38ID" runat="server" DataTextField="x38Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
                 <asp:Label ID="Label1" Text="Číselná řada DRAFT projektů:" runat="server" CssClass="lbl"></asp:Label>
                <uc:datacombo ID="x38ID_Draft" runat="server" DataTextField="x38Name" DataValueField="pid" IsFirstEmptyRow="true" Width="200px"></uc:datacombo>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblB01ID" Text="Workflow šablona:" runat="server" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="b01ID" runat="server" AutoPostBack="false" DataTextField="b01Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>


            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCode" Text="Kód:" runat="server" CssClass="lbl" AssociatedControlID="p42Code"></asp:Label></td>
            <td>
                <asp:TextBox ID="p42Code" runat="server" Style="width: 100px;"></asp:TextBox>
                <asp:CheckBox ID="p42IsDefault" runat="server" Text="Výchozí pro nové záznamy projektů" Visible="false" />
                <!-- //
                Výchozí projekt se bere z naposledy uloženého projektu, volba p42IsDefault se již nevyužívá
                // -->
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl" AssociatedControlID="p42Ordinary"></asp:Label>

            </td>
            <td>

                <telerik:RadNumericTextBox ID="p42Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
    </table>

    <div class="content-box2">
        <div class="title">
            <asp:label ID="ph1" runat="server" Text="Povolené sešity pro vykazování" />
        </div>
        <div class="content">
            <asp:CheckBoxList ID="p34ids" runat="server" AutoPostBack="false" DataValueField="pid" DataTextField="p34Name" RepeatColumns="3" CellPadding="8" CellSpacing="2"></asp:CheckBoxList>
        </div>
    </div>
    
    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>

