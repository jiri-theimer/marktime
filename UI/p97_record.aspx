<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p97_record.aspx.vb" Inherits="UI.p97_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="6" cellspacing="2">

        <tr>
            <td>
                <asp:Label ID="lblJ27ID" Text="Měna faktury:" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="j27ID" runat="server" DataTextField="j27Code" DataValueField="pid" IsFirstEmptyRow="true" Width="100px"></uc:datacombo>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblp97AmountFlag" Text="Předmět zaokrouhlení:" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="p97AmountFlag" runat="server">
                    <asp:ListItem Text="Částka bez DPH" Value="2"></asp:ListItem>
                     <asp:ListItem Text="Částka vč. DPH" Value="3"></asp:ListItem>
                     <asp:ListItem Text="Částka DPH" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblp97Scale" Text="Na kolik des.míst zaokrouhlovat:" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="p97Scale" runat="server">
                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                     <asp:ListItem Text="1" Value="1"></asp:ListItem>
                     <asp:ListItem Text="2" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
