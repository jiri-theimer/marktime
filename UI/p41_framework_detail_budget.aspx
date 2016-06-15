<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p41_framework_detail_budget.aspx.vb" Inherits="UI.p41_framework_detail_budget" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            window.parent.stoploading();

        });
        function p45_detail() {
            var p45id = "";
            <%If Me.p45ID.Items.Count > 0 Then%>
            p45id = document.getElementById("<%=Me.p45ID.ClientID%>").value;
            <%End If%>
            window.parent.sw_local("p45_project.aspx?pid=<%=master.datapid%>&p45id=" + p45id, "Images/budget_32.png", true);

        }
        function RowSelected_budget(sender, args) {
            document.getElementById("<%=hidBudgetPID.clientid%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_budget(sender, args) {
            budget_edit();

        }
        function budget_edit() {
            var pid = document.getElementById("<%=hidBudgetPID.clientid%>").value;
            <%If Me.cmdBudgetP46.Checked Then%>
            p45_detail();
            <%End If%>
            <%If Me.cmdBudgetP49.Checked Then%>
            p49_record(pid);
            <%End If%>
        }
        function p49_record(pid) {
            var p45id = document.getElementById("<%=Me.p45ID.ClientID%>").value;
            window.parent.sw_local("p49_record.aspx?pid=" + pid + "&p45id=" + p45id, "Images/budget_32.png");
        }
        function p49_to_p31() {
            var p49id = document.getElementById("<%=hidBudgetPID.clientid%>").value;
            if (p49id == "" || p49id == null) {
                alert("Musíte vybrat položku rozpočtu.");
                return
            }
            window.parent.sw_local("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>&p49id=" + p49id, "Images/worksheet_32.png", true);
            return (false);
        }
        function p47_plan() {
            window.parent.sw_local("p47_project.aspx?pid=<%=Master.DataPID%>&p45id=<%=Me.p45ID.SelectedValue%>", "Images/plan_32.png", true);
        }
        function budgetprefix_change(prefix) {
            location.replace("p41_framework_detail_budget.aspx?masterpid=<%=Master.DataPID%>&budgetprefix=" + prefix);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="div6">
        <div style="float: left;">
            <asp:DropDownList ID="p45ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="VersionWithName" BackColor="yellow"></asp:DropDownList>
            <button type="button" id="cmdP45" runat="server" onclick="p45_detail()" class="cmd">Nastavení rozpočtu</button>

        </div>
        <div style="float: left;">

            <asp:RadioButton ID="cmdBudgetP46" Text="Rozpočet hodin" AutoPostBack="false" runat="server" onclick="budgetprefix_change('p46')" />
            <asp:RadioButton ID="cmdBudgetP49" Text="Rozpočet výdajů a fixních odměn" AutoPostBack="false" runat="server" onclick="budgetprefix_change('p49')" />
            <button type="button" id="cmdP47" runat="server" onclick="p47_plan()" class="cmd" visible="false">Kapacitní plán projektu</button>
            <button type="button" id="cmdNewP49" runat="server" onclick="p49_record(0)" class="cmd" visible="false" title="Nová položka peněžního rozpočtu">
                <img src="Images/new.png" alt="Nový" /></button>
            <button type="button" id="cmdConvert2P31" runat="server" onclick="p49_to_p31()" class="cmd" visible="false" title="Překlopit položku rozpočtu do worksheet úkonu">
                <img src="Images/worksheet.png" alt="Worksheet" /></button>
        </div>
    </div>
     <div style="clear:both;"></div>
    <uc:datagrid ID="gridBudget" runat="server" OnRowSelected="RowSelected_budget" OnRowDblClick="RowDoubleClick_budget"></uc:datagrid>

    <asp:Panel ID="panP49Setting" runat="server">
        <asp:CheckBox ID="chkP49GroupByP34" runat="server" Text="Mezisoučty za sešity" AutoPostBack="true" Checked="true" />
        <asp:CheckBox ID="chkP49GroupByP32" runat="server" Text="Mezisoučty za aktivity" AutoPostBack="true" />
    </asp:Panel>

    <asp:HiddenField ID="hidBudgetPID" runat="server" />

</asp:Content>
