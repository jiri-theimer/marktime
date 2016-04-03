<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_p56subform.aspx.vb" Inherits="UI.entity_framework_p56subform" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="p56_subgrid" Src="~/p56_subgrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function p56_subgrid_setting(j74id) {
            ///volá se z p56_subgrid
            window.parent.sw_local("grid_designer.aspx?prefix=p56&masterprefix=p41&pid=" + j74id, "Images/griddesigner_32.png", true);
        }
        function RowSelected_p56(sender, args) {
            document.getElementById("<%=hiddatapid_p56.ClientID%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p56(sender, args) {
            var pid = document.getElementById("<%=hiddatapid_p56.ClientID%>").value;
            window.parent.sw_local("p56_record.aspx?pid=" + pid, "Images/task_32.png", false);
        }
        function p56_subgrid_approving(pids) {
            window.parent.parent.sw_master("p31_approving_step1.aspx?masterpid=<%=Me.CurrentMasterPID%>&masterprefix=<%=Me.CurrentMasterPrefix%>&prefix=p56&pid=" + pids, "Images/approve_32.png", true);

           
        }

        function p31_entry_p56() {
            ///volá se z gridu úkolů
            var p56id = document.getElementById("<%=hiddatapid_p56.ClientID%>").value;
            if (p56id == "" || p56id == null) {
                alert("Není vybrán úkol.");
                return (false);
            }
            window.parent.sw_local("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>&p56id=" + p56id, "Images/worksheet_32.png", true);
            return (false);
        }
        function p56_record(pid, bolReturnFalse) {
            window.parent.sw_local("p56_record.aspx?masterprefix=<%=Me.CurrentMasterPrefix%>&masterpid=<%=Me.CurrentMasterPID%>&pid=" + pid, "Images/task_32.png", true);
            if (bolReturnFalse == true)
                return (false)

        }
        function p56_clone() {
            ///volá se z gridu úkolů
            var pid = document.getElementById("<%=hiddatapid_p56.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return (false);
            }
            window.parent.sw_local("p56_record.aspx?clone=1&p41id=<%=Master.DataPID%>&pid=" + pid, "Images/task_32.png", true);
            return (false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:p56_subgrid ID="gridP56" runat="server" x29ID="p41Project" />


    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p56" runat="server" />
</asp:Content>
