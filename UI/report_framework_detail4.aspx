﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="report_framework_detail4.aspx.vb" Inherits="UI.report_framework_detail4" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function x31_record() {
            sw_local("x31_record.aspx?pid=<%=me.CurrentX31ID%>", "Images/settings_32.png")
        }

        function hardrefresh(pid, flag) {
            location.replace("report_framework_detail4.aspx?x31id=<%=me.CurrentX31ID%>");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Image ID="img1" runat="server" ImageUrl="Images/xls.png" />
            </td>
            <td>
                <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
            </td>
          
            <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span"></asp:Label>
            </td>
            <td>
                <asp:HyperLink ID="cmdSetting" runat="server" Text="Nastavení šablony" NavigateUrl="javascript:x31_record()"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <div class="div6">
        <asp:Button ID="cmdGenerate" runat="server" Text="Vygenerovat sestavu" CssClass="cmd" Font-Bold="true" />
    </div>
    <asp:HiddenField ID="hidCurX31ID" runat="server" />
</asp:Content>
