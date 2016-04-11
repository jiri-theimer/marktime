<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="x18_binding.aspx.vb" Inherits="UI.x18_binding" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    

    <asp:Repeater ID="rp1" runat="server">
        <ItemTemplate>
            <div class="div6">
                <asp:Label ID="x18Name" runat="server" Width="150px"></asp:Label>
                <uc:datacombo ID="x25IDs" runat="server" DataTextField="x25Name" DataValueField="pid" AllowCheckboxes="true" Filter="Contains" Width="400px"></uc:datacombo>
                <asp:HiddenField ID="x18ID" runat="server" />
                <asp:HiddenField ID="x18IsMultiSelect" runat="server" />
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:HiddenField ID="hidPrefix" runat="server" />
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
