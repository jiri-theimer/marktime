<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p41_framework_rec_p31.aspx.vb" Inherits="UI.p41_framework_rec_p31" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="p41_menu" Src="~/p41_menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">
        function p31_RowSelected(sender, args) {
            ///volá se z p31_subgrid
            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;

            sw_decide("p31_record.aspx?pid=" + pid, "Images/worksheet.png", false);

        }
        function p31_clone() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            if (pid == "") {
                alert("Musíte vybrat záznam")
                return;
            }
            sw_decide("p31_record.aspx?clone=1&pid=" + pid, "Images/worksheet.png", false);

        }
        function p31_split() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            if (pid == "") {
                alert("Musíte vybrat záznam")
                return;
            }
            sw_decide("p31_record_split.aspx?pid=" + pid, "Images/split.png", false);

        }
        function p31_entry() {
            ///volá se z p31_subgrid
            var url = "p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>";
          
            sw_decide(url, "Images/worksheet.png", false);

        }
        function p31_subgrid_setting(j74id) {
            ///volá se z p31_subgrid
            sw_decide("grid_designer.aspx?prefix=p31&masterprefix=<%=gridP31.MasterPrefixWithQueryFlag%>&pid=" + j74id, "Images/griddesigner_32.png", true);

        }
        function p31_subgrid_approving(pids) {
            try {
                window.parent.sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);
            }
            catch (err) {
                window.sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);
            }
        }
        function p31_subgrid_querybuilder(j70id) {
            sw_decide("query_builder.aspx?prefix=p31&x36key=p31_subgrid-j70id&pid=" + j70id, "Images/query_32.png", true);

        }
        function p31_subgrid_periodcombo() {
            sw_decide("periodcombo_setting.aspx", "Images/settings_32.png");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:p41_menu id="menu1" runat="server"></uc:p41_menu>

    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="p41Project" AllowMultiSelect="true"></uc:p31_subgrid>


    <asp:HiddenField ID="hiddatapid_p31" runat="server" />
    <asp:HiddenField ID="hidIsCanApprove" runat="server" />


    
</asp:Content>
