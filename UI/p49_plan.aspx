<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p49_plan.aspx.vb" Inherits="UI.p49_plan" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function RowSelected(sender, args) {

            document.getElementById("<%=hiddatapid.clientid%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            p49_record();
        }
        function p49_record() {
            var p49id = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (p49id == "" || p49id == null) {
                alert("Není vybrán záznam.");
                return
            }
            dialog_master("p49_record.aspx?p41id=<%=Master.DataPID%>&pid=" + p49id, false, "800", "600");

        }
        function p49_edit() {
            var p49id = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (p49id == "" || p49id == null) {
                alert("Není vybrán záznam.");
                return
            }
            dialog_master("p49_record.aspx?pid="+p49id+"&p41id=<%=Master.DataPID%>", false, "800", "600");
            return (false);
        }
        function p49_new() {            
            dialog_master("p49_record.aspx?pid=0&p41id=<%=Master.DataPID%>", false, "800", "600");
            return (false);
        }
        function p49_clone() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return (false);
            }
            dialog_master("p49_record.aspx?p41id=<%=Master.DataPID%>&clone=1&pid=" + pid, false, "800", "600");
            return (false);
        }
        function hardrefresh(pid, flag) {
            
            location.replace("p49_plan.aspx?p41id=<%=Master.datapid%>&p49id="+pid);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-box2" style="margin-top: 20px;">
        <div class="title">
            Záznamy finančního plánu projektu (<asp:Label ID="lblCount" runat="server"></asp:Label>)
            
                <asp:ImageButton ID="cmdNew" runat="server" ImageUrl="Images/new.png" ToolTip="Nový záznam plánu" OnClientClick="return p49_new()" CssClass="button-link" />
                <asp:ImageButton ID="cmdEdit" runat="server" ImageUrl="Images/edit.png" ToolTip="Upravit" OnClientClick="return p49_edit()" CssClass="button-link" />
                <asp:ImageButton ID="cmdClone" runat="server" ImageUrl="Images/copy.png" ToolTip="Kopírovat záznam do nového" OnClientClick="return p49_clone()" CssClass="button-link" />

        </div>
        <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowDblClick="RowDoubleClick" OnRowSelected="RowSelected"></uc:datagrid>
    </div>

    <asp:HiddenField ID="hiddatapid" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
