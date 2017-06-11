<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x18_record.aspx.vb" Inherits="UI.x18_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název štítku:"></asp:Label></td>
            <td>
                <asp:TextBox ID="x18Name" runat="server" Style="width: 400px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblX23ID" Text="Datový zdroj (combo seznam):" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="x23ID" runat="server" DataTextField="x23Name" DataValueField="pid" IsFirstEmptyRow="true" AutoPostBack="true"></uc:datacombo>
                <asp:HyperLink ID="cmdX23" runat="server" Visible="false" Text="Nastavení combo seznamu"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset>
                    <legend>Štítek se nabízí pro entity</legend>
                    <asp:CheckBoxList ID="x29IDs" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                        <asp:ListItem Text="Projekt" Value="141"></asp:ListItem>
                        <asp:ListItem Text="Klient" Value="328"></asp:ListItem>
                        <asp:ListItem Text="Úkol" Value="356"></asp:ListItem>
                        <asp:ListItem Text="Dokument" Value="223"></asp:ListItem>
                        <asp:ListItem Text="Vystavená faktura" Value="391"></asp:ListItem>
                        <asp:ListItem Text="Zálohová faktura" Value="390"></asp:ListItem>
                        <asp:ListItem Text="Osoba" Value="102"></asp:ListItem>
                        <asp:ListItem Text="Worksheet úkon" Value="331"></asp:ListItem>
                    </asp:CheckBoxList>
                </fieldset>

            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="x18IsMultiSelect" runat="server" Text="Povolen MULTI-SELECT (možnost oštítkovat záznam entity více položkami najednou)" Checked="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl"></asp:Label></td>
            <td>

                <telerik:RadNumericTextBox ID="x18Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
     
    </table>
    <div class="div6">
        <asp:CheckBox ID="x18IsAllEntityTypes" runat="server" CssClass="chk" AutoPostBack="true" Text="Štítek je aplikovatelný pro všechny entitní typy" Checked="true" />
    </div>
    <asp:panel ID="panEntityTypes" runat="server" cssclass="content-box2">
        <div class="title">
            Štítek se bude nabízet k oštítkování pouze u níže zaškrtlých typů entity:
        </div>
        <div class="content">
            <table cellpadding="10">

                <asp:Repeater ID="rp1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkEntityType" runat="server" CssClass="chk" Font-Bold="true" />
                                <asp:HiddenField ID="x22EntityTypePID" runat="server" />
                                <asp:HiddenField ID="x29ID_EntityType" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="x22IsEntryRequired" runat="server" Text="Povinné oštítkování u záznamu entity" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </asp:panel>

    <div class="div6">
        <asp:CheckBox ID="x18IsRequired" runat="server" Text="U záznamu entity je oštítkování povinné" CssClass="chk" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
