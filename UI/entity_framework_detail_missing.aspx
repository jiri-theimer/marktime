<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_detail_missing.aspx.vb" Inherits="UI.entity_framework_detail_missing" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {






        });

        function cbx1_OnClientSelectedIndexChanged(sender, eventArgs) {
            var combo = sender;
            var pid = combo.get_value();
            window.open("<%=ViewState("prefix")%>_framework.aspx?pid=" + pid,"_top");
        }
        function cbx1_OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            var combo = sender;

            if (combo.get_value() == "")
                context["filterstring"] = eventArgs.get_text();
            else
                context["filterstring"] = "";


            context["j03id"] = "<%=Master.Factory.SysUser.PID%>";
            context["flag"] = "searchbox";
            <%If ViewState("prefix") = "p41" Then%>
            context["j02id_explicit"] = "<%=Master.Factory.SysUser.j02ID%>";
            <%End If%>
        }
        function cbx1_OnClientFocus(sender, args) {
            var combo = sender;
            var s = combo.get_text();
            if (s.indexOf("...")>0)
                combo.set_text("");
        }


        function p28_create() {

            sw_local("p28_record.aspx?pid=0", "Images/contact.png", true);

        }
        function p41_create() {

            sw_local("p41_create.aspx", "Images/project.png", true);

        }
        function p56_create() {
            <%If ViewState("masterprefix") = "p41" Then%>
            sw_local("p56_record.aspx?pid=0&masterprefix=p41&masterpid=<%=ViewState("masterpid")%>", "Images/task.png", true);
            <%Else%>
            sw_local("p56_record.aspx?pid=0&masterprefix=p41&masterpid=0", "Images/task.png", true);
            <%End If%>


        }
        function j02_create() {

            sw_local("j02_record.aspx?pid=0", "Images/person.png", true);

        }
        function p91_create() {
            sw_local("p91_create_step1.aspx?prefix=p28", "Images/invoice.png", true)

        }
        function x31_create() {

            sw_local("x31_record.aspx?pid=0", "Images/report.png", true);

        }
        function o23_create() {

            sw_local("o23_record.aspx?pid=0", "Images/notepad.png", true);

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
        <asp:HyperLink ID="MasterRecord" runat="server" Font-Bold="true" Target="_top"></asp:HyperLink>
    </div>

    <asp:Panel ID="panStat" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/information.png" />
        </div>
        <div class="content">
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
                        <asp:Label runat="server" ID="lblBin" Text="Z toho přesunuté do archivu:"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label runat="server" ID="CountBin" CssClass="valbold"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <div style="clear: both;"></div>
    <asp:Panel ID="panSearch" runat="server" CssClass="div6">
       


        <telerik:RadComboBox ID="cbx1" Text="Hledat..."
            runat="server" RenderMode="Auto" DropDownWidth="400" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" ToolTip="Hledat" Width="250px" OnClientSelectedIndexChanged="cbx1_OnClientSelectedIndexChanged" OnClientItemsRequesting="cbx1_OnClientItemsRequesting" OnClientFocus="cbx1_OnClientFocus">
           
            <WebServiceSettings Method="LoadComboData" Path="~/Services/project_service.asmx" UseHttpGet="false" />
        </telerik:RadComboBox>


    </asp:Panel>






</asp:Content>
