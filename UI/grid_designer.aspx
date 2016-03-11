﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="grid_designer.aspx.vb" Inherits="UI.grid_designer" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">


        function trydel() {

            if (confirm("Opravdu odstranit šablonu?")) {
                return (true);
            }
            else {
                return (false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="innerform_light" style="min-height: 55px;">
        <table cellpadding="5">
            <tr>
                <td>
                    <asp:Label ID="lblJ74Header" runat="server" Text="Šablona sloupců:" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <uc:datacombo ID="j74ID" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Width="300px"></uc:datacombo>


                </td>
                <td>
                    <asp:Button ID="cmdNew" runat="server" CssClass="cmd" Text="Založit novou šablonu sloupců" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Název šablony:" CssClass="lblReq" AssociatedControlID="j74Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="j74Name" runat="server" CssClass="important_text" Style="width: 300px;" />
                </td>
                <td>
                    <asp:ImageButton ID="cmdSave" runat="server" ToolTip="Uložit změny v šabloně vč. jejího názvu" ImageUrl="Images/save.png" CssClass="button-link" />
                    <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" ToolTip="Odstranit šablonu sloupců" OnClientClick="return trydel();" CssClass="button-link" Style="margin-left: 40px;" />
                </td>

            </tr>
        </table>

    </div>


    <asp:HiddenField ID="j74IsSystem" runat="server" />

    <table cellpadding="8">
        <tr valign="top">
            <td>
                <div>Dostupné sloupce</div>
                <telerik:RadListBox ID="colsSource" Height="450px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="colsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="false" AutoPostBackOnDelete="false" AutoPostBackOnTransfer="false">
                    <ButtonSettings TransferButtons="All" ShowTransferAll="false" />
                   
                    <Localization ToRight="Přesunout" ToLeft="Odebrat" AllToRight="Přesunout vše" AllToLeft="Odbrat vše" MoveDown="Posunout dolu" MoveUp="Posunout nahoru" />
                </telerik:RadListBox>
            </td>
            <td>
                <div>Vybrané sloupce</div>
                <telerik:RadListBox ID="colsDest" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="true" Culture="cs-CZ" Width="350px" SelectionMode="Single">
                   
                    <EmptyMessageTemplate>
                        <div style="padding-top: 50px;">
                            Zatím žádné vybrané sloupce
                        </div>
                    </EmptyMessageTemplate>
                </telerik:RadListBox>
                
                <div style="margin-top:20px;">
                    <span>Automaticky třídit podle 1):</span>
                    <asp:DropDownList ID="cbxOrderBy1" runat="server" DataTextField="ColumnHeader" DataValueField="ColumnName"></asp:DropDownList>
                    <asp:DropDownList ID="cbxOrderBy1Dir" runat="server">
                        <asp:ListItem text="" Value=""></asp:ListItem>
                        <asp:ListItem text="Sestupně" Value="DESC"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <span>Automaticky třídit podle 2):</span>
                    <asp:DropDownList ID="cbxOrderBy2" runat="server" DataTextField="ColumnHeader" DataValueField="ColumnName"></asp:DropDownList>
                    <asp:DropDownList ID="cbxOrderBy2Dir" runat="server">
                        <asp:ListItem text="" Value=""></asp:ListItem>
                        <asp:ListItem text="Sestupně" Value="DESC"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </td>

        </tr>
    </table>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
