<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_subgrid_setting.aspx.vb" Inherits="UI.p31_subgrid_setting" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <table cellpadding="5">
        <tr>
            <td>
                <asp:Label ID="lblJ74ID" runat="server" Text="Šablona sloupců:" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="j74ID" runat="server" DataTextField="j74Name" DataValueField="pid" Width="300px" IsFirstEmptyRow="true"></uc:datacombo>

            </td>
        </tr>
    </table>
    <p></p><p></p>
    <div class="div6">
        <asp:Button ID="cmdGridDesigner" runat="server" Text="Přejít do návrháře sloupců datového přehledu" CssClass="cmd" Width="400px" />
    </div>
    
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
