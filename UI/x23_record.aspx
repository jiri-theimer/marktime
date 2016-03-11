<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x23_record.aspx.vb" Inherits="UI.x23_record" %>
<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název combo seznamu:"></asp:Label></td>
            <td>
                <asp:TextBox ID="x23Name" runat="server" Style="width: 400px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl"></asp:Label></td>
            <td>

                <telerik:RadNumericTextBox ID="x23Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
       
    </table>
    <div class="div6">
        <asp:CheckBox ID="chkIsDatasource" runat="server" AutoPostBack="true" Text="Combo seznam má externí datový zdroj" />
    </div>
    <asp:panel ID="panDataSource" runat="server" cssclass="content-box2">
        <div class="title">
            Externí datový zdroj combo seznamu
        </div>
        <div class="content">
            <table cellpadding="10">
                <tr valign="top">
                    <td>Datový zdroj (SQL):</td>
                    <td>
                        <asp:TextBox ID="x23DataSource" runat="server" TextMode="MultiLine" Width="600px" Height="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Název db tabulky:</td>
                    <td>
                        <asp:TextBox ID="x23DataSourceTable" runat="server" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>ID field (pole z db tabulky):</td>
                    <td>
                        <asp:TextBox ID="x23DataSourceFieldPID" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Text field (pole z db tabulky):</td>
                    <td>
                        <asp:TextBox ID="x23DataSourceFieldTEXT" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </asp:panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>

