<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_p56_record.aspx.vb" Inherits="UI.clue_p56_record" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function detail() {

            window.parent.sw_local("p56_record.aspx?masterprefix=<%=ViewState("masterprefix")%>&masterpid=<%=ViewState("masterpid")%>&pid=<%=Master.DataPID%>", "Images/task_32.png", true);

        }
        function workflow_dialog() {

            window.parent.sw_local("workflow_dialog.aspx?prefix=p56&pid=<%=Master.DataPID%>", "Images/workflow_32.png", true);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:panel ID="panContainer" runat="server" style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <asp:panel ID="panHeader" runat="server">
           <asp:image ID="img1" runat="server"  ImageUrl="Images/task_32.png" />
           <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>

            <asp:HyperLink ID="cmdWorkflow" runat="server" NavigateUrl="javascript:workflow_dialog()" Text="Posunout/doplnit" style="margin-right:20px;"></asp:HyperLink>
            <asp:HyperLink ID="cmDetail" runat="server" NavigateUrl="javascript:detail()" Text="Upravit"></asp:HyperLink>
       </asp:panel>
        
        
        <table cellpadding="5" cellspacing="2">
            <tr>

                <td colspan="2">
                    <asp:Label ID="Project" runat="server" CssClass="valbold"></asp:Label>

                </td>
            </tr>
            <tr id="trName" runat="server">
                <td>Název (předmět):</td>
                <td>
                    <asp:Label ID="p56Name" runat="server" CssClass="valbold"></asp:Label>

                </td>
            </tr>
           
            <tr id="trWorkflow" runat="server">
                <td>Workflow stav:</td>
                <td>
                    <asp:Label ID="b02Name" runat="server" CssClass="valboldred"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>Termín splnění úkolu:</td>
                <td>
                    <asp:Label ID="p56PlanUntil" runat="server" CssClass="valboldred"></asp:Label>
                    <asp:Label ID="lblp56PlanFrom" runat="server" Text="Plánované zahájení:"></asp:Label>
                    <asp:Label ID="p56PlanFrom" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Příjemci úkolu:</td>
                <td>
                    <asp:Label ID="RolesInLine" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Vykázané hodiny:</td>
                <td>
                    <asp:Label ID="Hours_Orig" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            
           
           <tr id="trPlanHours" runat="server" visible="false">
               <td>
                  <img src="Images/plan.png" />
                   Plán (limit) hodin:
               </td>
               <td>
                   <asp:Label ID="p56Plan_Hours" runat="server" CssClass="valbold"></asp:Label>
                   <asp:Label ID="PlanHoursSummary" runat="server" CssClass="valbold" style="padding-left:30px;"></asp:Label>
               </td>
           </tr>
            <tr id="trExpenses" runat="server" visible="false">
                <td>Vykázané výdaje:</td>
                <td>
                    <asp:Label ID="Expenses_Orig" runat="server" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            <tr id="trPlanExpenses" runat="server" visible="false">
               <td>
                  <img src="Images/plan.png" />
                   Plán (limit) výdajů:
               </td>
               <td>
                   <asp:Label ID="p56Plan_Expenses" runat="server" CssClass="valbold"></asp:Label>
                   <asp:Label ID="PlanExpensesSummary" runat="server" CssClass="valbold" style="padding-left:30px;"></asp:Label>
               </td>
           </tr>
           
        </table>
        <div>
            <asp:Button ID="cmdMove2Bin" runat="server" Text="Uzavřít (přesunout do archivu)" CssClass="cmd" />
        </div>
        <div class="div6">
            <asp:Label ID="p56Description" runat="server" CssClass="infoInForm"></asp:Label>

        </div>
       


        <div class="div6">
            <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
        </div>

        <uc:b07_list ID="comments1" runat="server" />
    </asp:panel>

</asp:Content>

