<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_rec_summary.aspx.vb" Inherits="UI.entity_framework_rec_summary" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="entity_menu" Src="~/entity_menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31summary_drilldown" Src="~/p31summary_drilldown.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function hardrefresh(pid, flag) {
            <%If menu1.PageSource<>"navigator" then%>
            if (flag == "<%=Me.CurrentMasterPrefix%>-create") {
                parent.window.location.replace("<%=Me.CurrentMasterPrefix%>_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "<%=Me.CurrentMasterPrefix%>-delete") {
                parent.window.location.replace("<%=Me.CurrentMasterPrefix%>_framework.aspx");
                return;
            }
            <%End If%>

            location.replace("entity_framework_rec_summary.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=master.datapid%>");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:entity_menu id="menu1" runat="server"></uc:entity_menu>

    <uc:p31summary_drilldown id="summary1" runat="server"></uc:p31summary_drilldown>


    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
</asp:Content>
