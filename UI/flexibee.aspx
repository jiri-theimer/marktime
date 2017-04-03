<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="flexibee.aspx.vb" Inherits="UI.flexibee" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx", "Images/settings_32.png");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label ID="lblFormHeader" runat="server" CssClass="page_header_span" Text="FlexiBee Export"></asp:Label>
    </div>
    <table cellpadding="10">
        <tr valign="top">
            <td>
                <uc:periodcombo ID="period1" runat="server" Width="230px"></uc:periodcombo>
            </td>
            <td>
                <asp:Button ID="cmdExport" runat="server" CssClass="cmd" Text="Generovat XML soubor" />
            </td>
        </tr>
    </table>
   
</asp:Content>
