﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p56_framework_detail.aspx.vb" Inherits="UI.p56_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entity_menu" Src="~/entity_menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="imap_record" Src="~/imap_record.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="searchbox" Src="~/searchbox.ascx" %>
<%@ Register TagPrefix="uc" TagName="alertbox" Src="~/alertbox.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function b07_reaction(b07id) {
            sw_decide("b07_create.aspx?parentpid=" + b07id + "&masterprefix=p56&masterpid=<%=Master.datapid%>", "Images/comment_32.png", true)

        }
        function hardrefresh(pid, flag) {

            if (flag == "p56-save" || flag == "workflow-dialog") {
                parent.window.location.replace("p56_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p56-delete") {
                parent.window.location.replace("p56_framework.aspx");
                return;
            }


            location.replace("p56_framework_detail.aspx?pid=<%=master.datapid%>");

        }

        function p56_record_new(p41id) {

            sw_decide("p56_record.aspx?pid=0&p41id="+p41id, "Images/task_32.png", true);

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:entity_menu ID="menu1" runat="server"></uc:entity_menu>
    <div style="height: 10px; clear: both;"></div>

    <div class="content-box1">
        <div class="title">
            <img src="Images/properties.png" style="margin-right: 10px;" />
            <asp:Label ID="boxCoreTitle" Text="Záznam úkolu" runat="server"></asp:Label>




        </div>
        <div class="content">
            <table cellpadding="10" cellspacing="2" id="responsive">
                <tr valign="top">
                    <td style="min-width: 120px;">
                        <asp:Label ID="lblTask" runat="server" Text="Úkol:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="p56Code" runat="server" CssClass="valbold"></asp:Label>
                        <asp:Label ID="p56Name" runat="server" CssClass="valbold"></asp:Label>


                    </td>


                </tr>

                <tr valign="top">
                    <td>
                        <asp:Label ID="lblType" runat="server" Text="Typ:" CssClass="lbl"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="p57Name" runat="server" CssClass="valbold"></asp:Label>

                        <asp:Label ID="lblp59NameSubmitter" runat="server" CssClass="lbl" Text="Priorita zadavatele:"></asp:Label>
                        <asp:Label ID="p59NameSubmitter" runat="server" CssClass="valbold"></asp:Label>
                    </td>

                </tr>
                <tr valign="top" id="trProduct" runat="server">
                    <td>
                        <asp:Label ID="lblProduct" runat="server" Text="Produkt:" CssClass="lbl"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="p58Name" runat="server" CssClass="valbold"></asp:Label>

                    </td>

                </tr>
                <tr valign="top">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Projekt:" CssClass="lbl"></asp:Label>

                    </td>
                    <td>
                        <asp:HyperLink ID="Project" runat="server" NavigateUrl="#" Target="_parent"></asp:HyperLink>
                        <asp:HyperLink ID="clue_project" runat="server" CssClass="reczoom" Text="i" title="Detail projektu"></asp:HyperLink>
                    </td>

                </tr>
                <tr id="trWorkflow" runat="server" valign="top">
                    <td>
                        <asp:Label ID="lblB02ID" runat="server" Text="Workflow stav:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="b02Name" runat="server" CssClass="valboldred"></asp:Label>
                        <a href="javascript: workflow()" title="Změnit stav úkolu, zapsat komentář, případně další kroky, které podporuje aktuální workflow šablona..."><img src="Images/workflow.png" />Posunout/doplnit</a>
                      
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:Label ID="lblDeadline" runat="server" Text="Termín:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="p56PlanUntil" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Vlastník záznamu:"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="Owner" runat="server" CssClass="valbold"></asp:Label>
                        <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>

                    </td>
                </tr>
            </table>
            <div class="div6">
                <uc:entityrole_assign_inline ID="roles_task" runat="server" EntityX29ID="p56Task" NoDataText=""></uc:entityrole_assign_inline>
            </div>
        </div>
    </div>

    <asp:Panel ID="boxX18" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/label.png" style="margin-right: 10px;" />
            <asp:Label ID="boxX18Title" runat="server" Text="Štítky"></asp:Label>
            <asp:HyperLink ID="x18_binding" runat="server" Text="Přiřadit"></asp:HyperLink>
        </div>
        <div class="content">
            <uc:x18_readonly ID="labels1" runat="server"></uc:x18_readonly>
        </div>
    </asp:Panel>


    <asp:Panel ID="boxVysledovka" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/worksheet.png" />
            WORKSHEET
        </div>
        <div class="content">
            <table cellpadding="6" id="responsive">
                <tr>
                    <td>Vykázané hodiny:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="Hours_Orig" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                    <td></td>
                </tr>

                <tr id="trPlanHours" runat="server" visible="false">
                    <td>
                        <img src="Images/plan.png" />
                        Plán (limit) hodin:
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="p56Plan_Hours" runat="server" CssClass="valbold"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="PlanHoursSummary" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                </tr>
                <tr id="trExpenses" runat="server" visible="false">
                    <td>Vykázané výdaje:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="Expenses_Orig" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr id="trPlanExpenses" runat="server" visible="false">
                    <td>
                        <img src="Images/finplan.png" />
                        Plán (limit) výdajů:
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="p56Plan_Expenses" runat="server" CssClass="valbold"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="PlanExpensesSummary" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Poslední nevyfakturovaný úkon:
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Last_WIP_Worksheet" runat="server" ForeColor="Brown" style="float:right;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Poslední vystavená faktura:
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Last_Invoice" runat="server" ForeColor="Brown" style="float:right;"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="boxFF" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/form.png" style="margin-right: 10px;" />
            <asp:Label ID="boxFFTitle" runat="server" Text="Uživatelská pole"></asp:Label>
            <asp:CheckBox ID="chkFFShowFilledOnly" runat="server" AutoPostBack="true" Text="Zobrazovat pouze vyplněná pole" />
        </div>
        <div class="content">
            <uc:freefields_readonly ID="ff1" runat="server" />
        </div>

    </asp:Panel>

    <asp:Panel ID="boxO23" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/notepad.png" style="margin-right: 10px;" />
            <asp:Label ID="boxO23Title" runat="server" Text="Dokumenty"></asp:Label>
        </div>
        <div class="content" style="overflow: auto; max-height: 200px;">

            <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p56Task"></uc:o23_list>


        </div>
    </asp:Panel>
    <asp:Panel ID="boxIMAP" runat="server" CssClass="content-box1" Visible="false">
        <div class="title">
            <img src="Images/imap.png" style="margin-right: 10px;" />
            <span>Úkol byl vygenerován poštovní zprávou</span>
        </div>
        <div class="content">
            <uc:imap_record ID="imap1" runat="server"></uc:imap_record>
        </div>
    </asp:Panel>
    <uc:alertbox ID="alert1" runat="server"></uc:alertbox>

    <div style="clear: both; width: 100%;"></div>

    <asp:Panel ID="panDescription" runat="server" CssClass="content-box1" Style="width: 99%; max-width: none;">
        <div class="title">Podrobný popis</div>
        <div class="content" style="background-color: #ffffcc; max-height: 120px; overflow: auto;">
            <asp:Label ID="p56Description" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
        </div>
    </asp:Panel>

    <div style="clear:both;"></div>
    <uc:b07_list ID="comments1" runat="server" JS_Create="menu_b07_record()" JS_Reaction="b07_reaction" />

    <asp:HiddenField ID="hidCurP41ID" runat="server" />
</asp:Content>
