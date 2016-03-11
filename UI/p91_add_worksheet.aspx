<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p91_add_worksheet.aspx.vb" Inherits="UI.p91_add_worksheet" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function periodcombo_setting() {

            dialog_master("periodcombo_setting.aspx");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Image ID="imgEntity" runat="server" ImageUrl="Images/project_32.png" />
            </td>
            <td>
                <asp:Label ID="lblEntityHeader" CssClass="framework_header_span" runat="server"></asp:Label>
            </td>
            
            <td>
                <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
            </td>
        
            
        </tr>
    </table>

    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid"></uc:datagrid>

    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
