<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_p91subform.aspx.vb" Inherits="UI.entity_framework_p91subform" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="p91_subgrid" Src="~/p91_subgrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

        });
        function periodcombo_setting() {

            window.parent.sw_local("periodcombo_setting.aspx", "Images/settings_32.png");
        }
        function RowSelected_p91(sender, args) {
            document.getElementById("<%=hiddatapid_p91.ClientID%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p91(sender, args) {
            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            window.parent.window.open("p91_framework.aspx?pid=" + document.getElementById("<%=hiddatapid_p91.ClientID%>").value, "_top")
            <%End If%>
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:p91_subgrid ID="gridP91" runat="server" x29ID="p41Project" />


     <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p91" runat="server" />


</asp:Content>
