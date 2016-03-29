<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p31_framework_detail.aspx.vb" Inherits="UI.p31_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="timesheet_calendar" Src="~/timesheet_calendar.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {





        });

        function p31_entry() {
            var p41id = <%=me.p41ID.ClientID%>_get_value();
            var b = false;
            if (screen.availWidth < 1000)
                b = true;

            sw_local("p31_record.aspx?pid=0&p31date=<%=Format(Me.cal1.SelectedDate, "dd.MM.yyyy")%>&j02id=<%=Me.CurrentJ02ID%>&p41id="+p41id, "Images/worksheet_32.png",b);
            return (false);
        }
        function p31_clone() {
            ///volá se z p31_subgrid
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_local("p31_record.aspx?clone=1&pid=" + pid, "Images/worksheet_32.png", true);
            return (false);
        }




        function hardrefresh(pid, flag) {
            if (flag == "j74") {
                location.replace("p31_framework_detail.aspx");
                return;
            }
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }





        function p31_RowSelected(sender, args) {

            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            record_p31_edit();
        }

        function record_p31_edit() {
            var pid = document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_local("p31_record.aspx?pid=" + pid, "Images/worksheet_32.png");

        }

        function p31_subgrid_setting(j74id) {
            sw_local("grid_designer.aspx?prefix=p31&masterprefix=j02&pid=" + j74id, "Images/griddesigner_32.png", true);

        }

        function j02id_onchange() {            
            var j02id = document.getElementById("<%=me.j02id.clientid%>").value;            

            $.post("Handler/handler_userparam.ashx", { x36value: j02id, x36key: "p31_framework_detail-j02id", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }

                window.open("p31_framework.aspx","_top")
            });
            
        }

        function p41id_onchange(sender, eventArgs) {
            //var item = eventArgs.get_item();
            p31_entry();
        }

        function report() {
            
            sw_local("report_modal.aspx?prefix=j02&pid=<%=me.j02id.selectedvalue%>", "Images/reporting_32.png", true);

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Image ID="img1" runat="server" ImageUrl="Images/worksheet_32.png" />
            </td>
            <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Worksheet kalendář pro zapisování úkonů"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="j02ID" runat="server" onChange="j02id_onchange()"></asp:DropDownList>
            </td>
        </tr>
    </table>

    <table cellpadding="10">
        <tr>
            <td style="vertical-align: top;">
                <uc:timesheet_calendar ID="cal1" runat="server" />
            </td>
            <td style="vertical-align: top;">
                <div class="content-box2">
                    <div class="title">
                        <asp:HyperLink ID="clue_timesheet" runat="server" CssClass="reczoom" Text="i" ToolTip="Statistika vykázaných hodin v měsíci"></asp:HyperLink>
                        <asp:Label ID="StatHeader" runat="server"></asp:Label>
                        <asp:HyperLink ID="cmdReport" runat="server" Text="Tisková sestava" NavigateUrl="javascript:report()" style="float:right;"></asp:HyperLink>
                    </div>
                    <div class="content">
                        <div class="div6">
                            <span>Vykázané hodiny:</span>
                            <asp:Label ID="Hours_All" runat="server" CssClass="valboldblue"></asp:Label>
                            
                            <asp:label ID="lblHours_Billable" runat="server" Text="z toho fakturovatelné:"></asp:label>
                            <asp:Label ID="Hours_Billable" runat="server" CssClass="valboldblue" ForeColor="green"></asp:Label>
                            
                        </div>
                        <div class="div6">
                            <span>Fond pracovní doby:</span>
                            <asp:Label ID="Fond_Hours" runat="server" CssClass="valboldred"></asp:Label>

                        </div>
                        <div class="div6">
                            <span>Utilizace za všechny hodiny:</span>
                            <asp:Label ID="Util_Total" runat="server" CssClass="valboldred"></asp:Label>
                            <asp:Label ID="lblUtil_Billable" runat="server" Text="Utilizace za fakturovatelné hodiny:"></asp:Label>                            
                            <asp:Label ID="Util_Billable" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="content-box2" style="margin-top:10px;">
                    <div class="title">Vyhledat projekt pro nový úkon...</div>
                    <div class="content">
                         <uc:project ID="p41ID" runat="server" Width="99%" AutoPostBack="false" Flag="p31_entry" OnClientSelectedIndexChanged="p41id_onchange" />
                    </div>
                </div>

                <div style="margin-top: 40px;">
                    <asp:Button ID="cmdNewP31" runat="server" Text="Nový úkon" CssClass="cmd" OnClientClick="return p31_entry();" />
                </div>

            </td>
        </tr>

    </table>


    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="j02Person" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick"></uc:p31_subgrid>


    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
