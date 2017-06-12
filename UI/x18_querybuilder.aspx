<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="x18_querybuilder.aspx.vb" Inherits="UI.x18_querybuilder" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="div6">
        <span class="val">Zaškrtněte jeden nebo více položek šítků</span>
    </div>
    

    <telerik:RadTreeView ID="tr1" runat="server" Skin="Default" ShowLineImages="false" SingleExpandPath="true" Height="500px" CheckBoxes="true">        
    </telerik:RadTreeView>


    <asp:HiddenField ID="hidPrefix" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
