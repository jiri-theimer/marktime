<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="o24_record.aspx.vb" Inherits="UI.o24_record" %>

<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="3" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblX29ID" Text="Entita:" runat="server" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="x29ID" runat="server" DataTextField="x29Name" DataValueField="x29ID" AutoPostBack="true">
                    <asp:ListItem Text="Projekt" Value="141"></asp:ListItem>
                    <asp:ListItem Text="Klient" Value="328"></asp:ListItem>
                    <asp:ListItem Text="Worksheet úkon" Value="331"></asp:ListItem>
                    <asp:ListItem Text="Osoba" Value="102"></asp:ListItem>
                    <asp:ListItem Text="Faktura" Value="391"></asp:ListItem>
                    <asp:ListItem Text="Úkol" Value="356"></asp:ListItem>

                </asp:DropDownList>

            </td>
        </tr>


        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="o24Name" runat="server" Style="width: 400px;"></asp:TextBox>
                <asp:Label ID="lblOrdinary" Text="Index pořadí v seznamu typů:" runat="server" CssClass="lbl" AssociatedControlID="o24Ordinary"></asp:Label>
                <telerik:RadNumericTextBox ID="o24Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>

            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblX38ID" Text="Číselná řada dokumentu:" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="x38ID" runat="server" DataTextField="x38Name" DataValueField="pid" IsFirstEmptyRow="true" Width="250px"></uc:datacombo>
                <asp:Label ID="Label1" Text="Číselná řada DRAFT dokumentů:" runat="server" CssClass="lbl"></asp:Label>
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
            <td colspan="2">
                <asp:CheckBox ID="o24IsBillingMemo" runat="server" Text="Dokument se nabízí ve schvalování a fakturaci jako 'Fakturační poznámka'" />
            </td>
        </tr>
    </table>

    <div class="content-box2">
        <div class="title">
            Povinnost vazby na záznam entity
        </div>
        <div class="content">
            <asp:RadioButtonList ID="o24IsEntityRelationRequired" runat="server" RepeatDirection="Vertical">
                <asp:ListItem Text="Povinně" Value="1"></asp:ListItem>
                <asp:ListItem Text="Nepovinně" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <div class="content-box2">
        <div class="title">
            Limity souborových příloh
        </div>
        <div class="content">
            <div class="div6">
                <span>Maximální velikost jedné souborové přílohy:</span>
            <asp:DropDownList ID="o24MaxOneFileSize" runat="server">
                <asp:ListItem Text="1 MB" Value="1048576"></asp:ListItem>
                <asp:ListItem Text="2 MB" Value="2097152"></asp:ListItem>
                <asp:ListItem Text="3 MB" Value="3145728"></asp:ListItem>
                <asp:ListItem Text="4 MB" Value="4194304"></asp:ListItem>
                <asp:ListItem Text="5 MB" Value="5242880"></asp:ListItem>
                <asp:ListItem Text="6 MB" Value="6291456"></asp:ListItem>
                <asp:ListItem Text="7 MB" Value="7340032"></asp:ListItem>
                <asp:ListItem Text="10 MB" Value="10485760"></asp:ListItem>
            </asp:DropDownList>
            </div>
            <div class="div6">
                <span>Povolené přípony nahrávaných souborů (čárkou oddělené):</span>
                <asp:textbox ID="o24AllowedFileExtensions" runat="server" Width="400px"></asp:textbox>
            </div>
        </div>
    </div>
    <div class="content-box2">
        <div class="title">
            <img src="Images/dropbox.png" />
            Integrace s externími DMS systémy
        </div>
        <div class="content">
            <asp:CheckBox ID="o24IsAllowDropbox" runat="server" Text="Povolit v dokumentu mapovat DROPBOX složky" />
        </div>
    </div>

    <div class="content-box2" style="margin-top: 20px;">
        <div class="title">
            <img src="Images/help2.png" />
            Nápověda (text nápovědy bude systém nabízet uživatelům ve formuláři pro vytvoření dokumentu tohoto typu)
        </div>
        <div class="content">
            <asp:TextBox ID="o24HelpText" runat="server" TextMode="MultiLine" Style="width: 100%; height: 160px;"></asp:TextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
