<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p41_framework_rec_board.aspx.vb" Inherits="UI.p41_framework_rec_board" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entity_menu" Src="~/entity_menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>
<%@ Register TagPrefix="uc" TagName="entity_worksheet_summary" Src="~/entity_worksheet_summary.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="alertbox" Src="~/alertbox.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function b07_reaction(b07id) {
            sw_decide("b07_create.aspx?parentpid=" + b07id + "&masterprefix=p41&masterpid=<%=Master.datapid%>", "Images/comment_32.png", true)

        }
        function hardrefresh(pid, flag) {

            if (flag == "p41-create") {
                parent.window.location.replace("p41_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p41-delete") {
                parent.window.location.replace("p41_framework.aspx");
                return;
            }


            location.replace("p41_framework_rec_board.aspx?pid=<%=master.datapid%>");

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:entity_menu id="menu1" runat="server"></uc:entity_menu>

    <div style="height:10px;clear:both;"></div>
    <div class="content-box1">
        <div class="title">
            <img src="Images/properties.png" style="margin-right: 10px;" />

            <asp:Label ID="boxCoreTitle" Text="Záznam projektu" runat="server" meta:resourcekey="boxCoreTitle"></asp:Label>

            <asp:ImageButton ID="cmdFavourite" runat="server" ImageUrl="Images/not_favourite.png" ToolTip="Zařadit do mých oblíbených projektů" CssClass="button-link" Style="float: right;" />


        </div>
        <div class="content">



            <table cellpadding="10" cellspacing="2" id="responsive">
                <tr id="trParent" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Nadřízený projekt:" CssClass="lbl"></asp:Label>

                    </td>
                    <td>
                        <asp:HyperLink ID="ParentProject" runat="server" Target="_top"></asp:HyperLink>

                    </td>

                </tr>
                <tr valign="baseline">
                    <td style="min-width: 120px;">
                        <asp:Label ID="lblProject" runat="server" Text="Projekt:" CssClass="lbl" meta:resourcekey="lblProject"></asp:Label>
                    </td>
                    <td>

                        <asp:Label ID="Project" runat="server" CssClass="valbold"></asp:Label>
                        <asp:Image ID="imgFlag_Project" runat="server" />
                        <asp:Image ID="imgDraft" runat="server" ImageUrl="Images/draft_icon.gif" Visible="false" AlternateText="DRAFT záznam" Style="float: right;" />
                        <asp:Panel ID="panDraftCommands" runat="server" Visible="false">
                            <button type="button" onclick="draft2normal()">
                                Převést z režimu DRAFT na oficiální záznam
                            </button>
                        </asp:Panel>
                        <div>
                            <asp:HyperLink ID="cmdChilds" runat="server" Text="Pod-projekty" NavigateUrl="javascript:childs()" Visible="false"></asp:HyperLink>
                        </div>
                    </td>

                </tr>

                <tr valign="baseline">
                    <td>

                        <asp:Label ID="lblClient" runat="server" Text="Klient:" CssClass="lbl" meta:resourcekey="lblClient"></asp:Label>

                    </td>
                    <td>

                        <asp:HyperLink ID="Client" runat="server" NavigateUrl="#" Target="_top"></asp:HyperLink>
                        <asp:HyperLink ID="clue_client" runat="server" CssClass="reczoom" Text="i" title="Detail klienta"></asp:HyperLink>
                        <asp:Image ID="imgFlag_Client" runat="server" />

                    </td>

                </tr>

                <tr id="trWorkflow" runat="server">
                    <td>
                        <asp:Label ID="lblB02ID" runat="server" Text="Workflow stav:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="b02Name" runat="server" CssClass="valboldred"></asp:Label>
                        <img src="Images/workflow.png" />
                        <asp:HyperLink ID="cmdWorkflow" runat="server" Text="Posunout/doplnit" NavigateUrl="javascript: workflow()"></asp:HyperLink>
                    </td>
                </tr>
                <tr id="trPlan" runat="server" style="vertical-align: top;">
                    <td>
                        <asp:Label ID="lblPlan" runat="server" Text="Zahájení/dokončení:" CssClass="lbl" meta:resourcekey="lblPlan"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="PlanPeriod" runat="server" CssClass="val"></asp:Label>
                        <div>


                            <asp:HyperLink ID="aP48" runat="server" NavigateUrl="javascript:p48_plan()" Text="Operativní plán projektu" meta:resourcekey="aP48"></asp:HyperLink>
                        </div>

                    </td>
                </tr>
                <tr id="trP51" runat="server">
                    <td style="vertical-align: top;">
                        <asp:Label ID="lblX51" runat="server" Text="Fakturační ceník:" CssClass="lbl" meta:resourcekey="lblX51"></asp:Label>
                    </td>
                    <td>

                        <asp:Label ID="p51Name_Billing" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_p51id_billing" runat="server" CssClass="reczoom" Text="i" title="Detail ceníku projektu"></asp:HyperLink>
                        <asp:Label ID="lblX51_Message" runat="server" CssClass="lbl"></asp:Label>



                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblP42Name" runat="server" Text="Typ projektu:" CssClass="lbl" meta:resourcekey="lblP42Name"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="p42Name" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_p42name" runat="server" CssClass="reczoom" Text="i" title="Detail typu projektu"></asp:HyperLink>

                        <asp:Label ID="lblJ18Name" runat="server" Text="Středisko:" CssClass="lbl" meta:resourcekey="lblJ18Name"></asp:Label>
                        <asp:Label ID="j18Name" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_j18name" runat="server" CssClass="reczoom" Text="i" title="Detail střediska"></asp:HyperLink>
                    </td>

                </tr>


            </table>


        </div>
    </div>
    <asp:Panel ID="boxX18" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/label.png" style="margin-right: 10px;" />
            <asp:Label ID="boxX18Title" runat="server" Text="Štítky" meta:resourcekey="boxX18Title"></asp:Label>
            <asp:HyperLink ID="x18_binding" runat="server" Text="Přiřadit" meta:resourcekey="x18_binding"></asp:HyperLink>
        </div>
        <div class="content">
            <uc:x18_readonly ID="labels1" runat="server"></uc:x18_readonly>
        </div>
    </asp:Panel>
    <asp:Panel ID="boxP40" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/worksheet_recurrence.png" style="margin-right: 10px;" />
            <asp:Label ID="boxP40Title" runat="server" Text="Opakované odměny/paušály/úkony"></asp:Label>
        </div>
        <div class="content">
            <asp:Repeater ID="rpP40" runat="server">
                <ItemTemplate>
                    <div class="div6">
                        <asp:HyperLink ID="p40Name" runat="server"></asp:HyperLink>
                        <asp:HyperLink ID="clue_p40" runat="server" CssClass="reczoom" Text="i"></asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>


    <asp:Panel ID="boxP31Summary" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/worksheet.png" style="margin-right: 10px;" />
            <asp:Label ID="boxP31SummaryTitle" runat="server" Text="WORKSHEET Summary"></asp:Label>
        </div>
        <div class="content">
            <uc:entity_worksheet_summary ID="p31summary1" runat="server"></uc:entity_worksheet_summary>

            <div style="width:100%;">
                <span class="val">Poslední vystavená faktura:</span>
                <asp:Label ID="Last_Invoice" runat="server" ForeColor="Brown" style="float:right;"></asp:Label>

            </div>
            <div style="width:100%;">
                <span class="val">Poslední nevyfakturovaný úkon:</span>
                <asp:Label ID="Last_WIP_Worksheet" runat="server" ForeColor="Brown" style="float:right;"></asp:Label>

            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="boxP30" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/person.png" style="margin-right: 10px;" />
            <asp:Label ID="boxP30Title" runat="server" Text="Kontaktní osoby projektu" meta:resourcekey="boxP30Title"></asp:Label>
            <asp:HyperLink ID="cmdEditP30" runat="server" NavigateUrl="javascript:p30_binding()" Text="Upravit" Style="margin-left: 20px;" meta:resourcekey="cmdEditP30"></asp:HyperLink>
        </div>
        <div class="content">
            <uc:contactpersons ID="persons1" runat="server"></uc:contactpersons>
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

    <asp:Panel ID="boxRoles" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/projectrole.png" style="margin-right: 10px;" />
            <asp:Label ID="boxRolesTitle" runat="server" Text="Projektové role" meta:resourcekey="boxRolesTitle"></asp:Label>
        </div>
        <div class="content">
            <uc:entityrole_assign_inline ID="roles_project" runat="server" EntityX29ID="p41Project" NoDataText="V projektu nejsou přiřazeny projektové role."></uc:entityrole_assign_inline>
        </div>
    </asp:Panel>
    <asp:Panel ID="boxBillingMemo" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/billing.png" style="margin-right: 10px;" />
            <span>Fakturační poznámka projektu</span>
        </div>
        <div class="content">
            <asp:Label ID="p41BillingMemo" runat="server"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel ID="boxO23" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/notepad.png" style="margin-right: 10px;" />
            <asp:Label ID="boxO23Title" runat="server" Text="Dokumenty" meta:resourcekey="boxO23Title"></asp:Label>
        </div>
        <div class="content" style="overflow: auto; max-height: 200px;">

            <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p41Project"></uc:o23_list>


        </div>
    </asp:Panel>
    <uc:alertbox ID="alert1" runat="server"></uc:alertbox>

    <div style="clear:both;"></div>
    <uc:b07_list ID="comments1" runat="server" JS_Create="b07_record()" JS_Reaction="b07_reaction" />
</asp:Content>
