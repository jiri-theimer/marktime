<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_p56_record.aspx.vb" Inherits="UI.clue_p56_record" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function detail() {

            window.parent.sw_local("p56_record.aspx?masterprefix=<%=ViewState("masterprefix")%>&masterpid=<%=ViewState("masterpid")%>&pid=<%=Master.DataPID%>", "Images/task_32.png", true);

        }
        function go2module() {

            window.open("p56_framework.aspx?pid=<%=Master.DataPID%>", "_top");

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panContainer" runat="server" Style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">

        <div>
            <asp:Image ID="img1" runat="server" ImageUrl="Images/task_32.png" />
            <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
        </div>

        <div class="content-box2">
            <div class="title">
                Záznam úkolu
                <a style="margin-left: 20px;" href="javascript:go2module()">Skočit na detail úkolu</a>
            </div>
            <div class="content">
                <table cellpadding="5" cellspacing="2" id="responsive">
                    
                    <tr>
                        <td>Název:</td>
                        <td>
                            <asp:Label ID="p56Name" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                    </tr>

                    <tr>
                        <td>Aktuální stav:</td>
                        <td>
                            <asp:Label ID="b02Name" runat="server" CssClass="valboldred"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>Termín dokončení:</td>
                        <td>
                            <asp:Label ID="p56PlanUntil" runat="server" CssClass="valboldred"></asp:Label>
                            <asp:Label ID="lblp56PlanFrom" runat="server" Text="Plánované zahájení:"></asp:Label>
                            <asp:Label ID="p56PlanFrom" runat="server" CssClass="valbold" ForeColor="green"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Projekt:</td>
                        <td>
                            <asp:Label ID="Project" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                    </tr>
                    <tr id="trP58" runat="server">
                        <td>Produkt:</td>
                        <td>
                            <asp:Label ID="p58Name" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                    </tr>
                    <tr id="trP59" runat="server">
                        <td>Priorita zadavatele:</td>
                        <td>
                            <asp:Label ID="p59name_submitter" runat="server" CssClass="valbold"></asp:Label>

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
                            <asp:Label ID="PlanHoursSummary" runat="server" CssClass="valbold" Style="padding-left: 30px;"></asp:Label>
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
                            <asp:Label ID="PlanExpensesSummary" runat="server" CssClass="valbold" Style="padding-left: 30px;"></asp:Label>
                        </td>
                    </tr>

                </table>
            </div>
        </div>



         <asp:Panel ID="panBody" runat="server" CssClass="content-box2">
            <div class="title">Podrobný popis</div>
            <div class="content" style="background-color: #ffffcc;">
                <asp:Label ID="p56Description" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
            </div>
        </asp:Panel>



        <div class="div6">
            <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
        </div>

        
    </asp:Panel>

</asp:Content>

