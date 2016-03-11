<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="x40_record.aspx.vb" Inherits="UI.x40_record" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" CssClass="lbl" Text="Stav zprávy:"></asp:Label>

            </td>
            <td>
                <asp:Label ID="x40State" runat="server" CssClass="valbold"></asp:Label>
                <asp:Button ID="cmdChangeState" Text="Změnit stav zprávy" runat="server" CssClass="cmd" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="lbl" Text="Čas:"></asp:Label>

            </td>
            <td>

                <asp:Label ID="DateInsert" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Od:"></asp:Label>

            </td>
            <td>

                <asp:Label ID="x40SenderName" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblx40Recipient" runat="server" CssClass="lbl" Text="Komu:"></asp:Label>

            </td>
            <td>

                <asp:Label ID="x40Recipient" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCC" runat="server" CssClass="lbl" Text="V kopii (Cc):"></asp:Label>

            </td>
            <td>
                <asp:Label ID="x40CC" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBCC" runat="server" CssClass="lbl" Text="Skrytá kopie (Bcc):"></asp:Label>

            </td>
            <td>
                <asp:Label ID="x40BCC" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
       
        <tr valign="top">
            <td>
                <asp:Label ID="lblSubject" runat="server" CssClass="lbl" Text="Předmět zprávy:"></asp:Label>

            </td>
            <td>
                <asp:Label ID="x40Subject" runat="server" CssClass="valbold"></asp:Label>
            </td>
        </tr>
    </table>

    <asp:TextBox ID="x40Body" runat="server" TextMode="MultiLine" Style="width: 100%; height: 200px;" ReadOnly="true">
    </asp:TextBox>
    <div>
        <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
    </div>
    <div class="div6">
        <uc:fileupload_list ID="uploadlist1" runat="server" />
    </div>
    <div class="div6">
        <asp:Label ID="x40ErrorMessage" runat="server" CssClass="infoNotificationRed"></asp:Label>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
