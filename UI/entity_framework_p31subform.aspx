﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_p31subform.aspx.vb" Inherits="UI.entity_framework_p31subform" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>

<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

        });

        function p31_RowSelected(sender, args) {
            ///volá se z p31_subgrid
            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            
            window.parent.sw_local("p31_record.aspx?pid=" + pid, "Images/worksheet_32.png",false);

        }
        function p31_clone() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            window.parent.sw_local("p31_record.aspx?clone=1&pid=" + pid, "Images/worksheet_32.png", false);
            return (false);
        }
        function p31_entry() {
            ///volá se z p31_subgrid
            var p41id = "";
            <%if Me.CurrentMasterPrefix="p41" then%>
            p41id = "<%=me.CurrentMasterPID%>";
            <%End If%>
            window.parent.sw_local("p31_record.aspx?pid=0&p41id="+p41id, "Images/worksheet_32.png", false);
            return (false);
        }
        function p31_subgrid_setting(j74id) {
            ///volá se z p31_subgrid
            window.parent.sw_local("grid_designer.aspx?prefix=p31&masterprefix=p41&pid=" + j74id, "Images/griddesigner_32.png", true);

        }
        function p31_subgrid_approving(pids) {
            window.parent.parent.sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);
        }
        function p31_subgrid_querybuilder(j70id) {           
            window.parent.sw_local("query_builder.aspx?prefix=p31&x36key=p31_subgrid-j70id&pid=" + j70id, "Images/query_32.png", true);
           
        }
        function p31_subgrid_periodcombo() {
            window.parent.sw_local("periodcombo_setting.aspx", "Images/settings_32.png");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="load1"></div>
    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="p41Project" AllowMultiSelect="true"></uc:p31_subgrid>


    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />


</asp:Content>