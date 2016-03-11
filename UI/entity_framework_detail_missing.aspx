<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_detail_missing.aspx.vb" Inherits="UI.entity_framework_detail_missing" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function p28_create() {

            sw_local("p28_record.aspx?pid=0", "Images/contact_32.png", true);

        }
        function p41_create() {

            sw_local("p41_create.aspx", "Images/project_32.png", true);

        }
        function p56_create() {
            <%if viewstate("masterprefix")="p41" then%>
            sw_local("p56_record.aspx?pid=0&masterprefix=p41&masterpid=<%=ViewState("masterpid")%>", "Images/task_32.png", true);
            <%Else%>
            sw_local("p56_record.aspx?pid=0&masterprefix=p41&masterpid=0", "Images/task_32.png", true);
            <%end if%>
            

        }
        function j02_create() {

            sw_local("j02_record.aspx?pid=0", "Images/person_32.png", true);

        }
        function p91_create() {
            sw_local("p91_create_step1.aspx?prefix=p28", "Images/invoice_32.png", true)

        }
        function x31_create() {

            sw_local("x31_record.aspx?pid=0", "Images/report_32.png", true);

        }
        function o23_create() {

            sw_local("o23_record.aspx?pid=0", "Images/notepad_32.png", true);

        }

        function hardrefresh(pid, flag) {
            if (flag == "p28-create") {
                parent.window.location.replace("p28_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p41-create") {
                parent.window.location.replace("p41_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "j02-save") {
                parent.window.location.replace("j02_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "o23-save") {
                parent.window.location.replace("o23_framework.aspx?pid=" + pid);
                return;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:pageheader ID="ph1" runat="server" IsInForm="true" Visible="false" Text="Nebyl vybrán záznam." />
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Image ID="img1" runat="server" />
            </td>
            <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Style="font-size: 200%;"></asp:Label>
            </td>
            <td>
                <asp:HyperLink ID="cmdNew" runat="server" Visible="false"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <div class="div6">
        <asp:hyperlink ID="MasterRecord" runat="server" Font-Bold="true" Target="_top"></asp:hyperlink>
    </div>
    

    <asp:Panel ID="panStat" runat="server">
        <table cellpadding="10" id="responsive">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblCount" Text="Počet dostupných záznamů:"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="Count4Read" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblBin" Text="Z toho přesunuté do koše:"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="CountBin" CssClass="valbold"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
