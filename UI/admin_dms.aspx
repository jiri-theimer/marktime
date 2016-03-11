<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="admin_dms.aspx.vb" Inherits="UI.admin_dms" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-box2">
        <div class="title">
            Spolupráce této MARKTIME databáze se systémem Dropbox
        </div>
        <div class="content">
            <asp:CheckBox ID="Dropbox_IsUse" runat="server" AutoPostBack="false" Text="Spolupracovat se systémem Dropbox" />
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
