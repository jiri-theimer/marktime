<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="entity_framework_detail_setting.aspx.vb" Inherits="UI.entity_framework_detail_setting" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-box2">
        <div class="title">
            Výška horní poloviny stránky (boxy nad pod-přehledem)
        </div>
        <div class="content">
            <asp:RadioButtonList ID="switchHeight" runat="server" CellPadding="6" RepeatColumns="3" RepeatDirection="Horizontal">
                <asp:ListItem Text="auto" Value="auto"></asp:ListItem>
                <asp:ListItem Text="200" Value="200"></asp:ListItem>
                <asp:ListItem Text="300" Value="300"></asp:ListItem>
                <asp:ListItem Text="400" Value="400"></asp:ListItem>
                <asp:ListItem Text="500" Value="500"></asp:ListItem>
                <asp:ListItem Text="600" Value="600"></asp:ListItem>
                <asp:ListItem Text="700" Value="700"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>

    <asp:HiddenField ID="hidPrefix" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
