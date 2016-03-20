<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p41_framework_detail.aspx.vb" Inherits="UI.p41_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>
<%@ Register TagPrefix="uc" TagName="entity_worksheet_summary" Src="~/entity_worksheet_summary.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="p91_subgrid" Src="~/p91_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="p56_subgrid" Src="~/p56_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_bigsummary" Src="~/p31_bigsummary.ascx" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .RadMenu_Silk .rmItem :hover {
            border-top-color: transparent !important;
        }

        .RadMenu_Silk .rmSelected .rmLink {
            border-top-color: transparent !important;
        }
    </style>
    <asp:PlaceHolder ID="placeBinMenuCss" runat="server"></asp:PlaceHolder>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

        });

        function report() {
            
            sw_local("report_modal.aspx?prefix=p41&pid=<%=Master.DataPID%>","Images/reporting_32.png",true);

        }

        function p31_entry() {
            ///volá se z p31_subgrid
            sw_local("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>","Images/worksheet_32.png",true);
            return(false);
        }
        function p31_entry_p56() {
            ///volá se z gridu úkolů
            var p56id=document.getElementById("<%=hiddatapid_subform.clientid%>").value;
            if (p56id == "" || p56id == null) {
                alert("Není vybrán úkol.");
                return(false);
            }
            sw_local("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>&p56id="+p56id,"Images/worksheet_32.png",true);
            return(false);
        }
        function p56_clone() {
            ///volá se z gridu úkolů
            var pid=document.getElementById("<%=hiddatapid_subform.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return(false);
            }
            sw_local("p56_record.aspx?clone=1&p41id=<%=Master.DataPID%>&pid="+pid,"Images/task_32.png",true);
            return(false);
        }
        function p31_clone() {
            ///volá se z p31_subgrid
            var pid=document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_local("p31_record.aspx?clone=1&pid="+pid,"Images/worksheet_32.png",true);
            return(false);
        }
        function p31_entry_menu(p34id) {
            ///z menu1
            sw_local("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>&p34id="+p34id,"Images/worksheet_32.png",true);
            

        }

        function record_new() {
            
            sw_local("p41_create.aspx?client_family=1&pid=<%=Master.DataPID%>","Images/project_32.png",true);

        }

        function record_edit() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_local("p41_record.aspx?pid=" + pid,"Images/project_32.png",true);

        }
        
        
        function record_clone() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_local("p41_create.aspx?clone=1&pid=" + pid,"Images/project_32.png",true);

        }

        function hardrefresh(pid, flag) {
            
            if (flag=="p41-create"){                
                parent.window.location.replace("p41_framework.aspx?pid="+pid);
                return;
            }
            if (flag=="p41-delete"){
                parent.window.location.replace("p41_framework.aspx");
                return;
            }
            
            
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;
            
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        

        

        function p31_RowSelected(sender, args) {
            ///volá se z p31_subgrid
            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            ///volá se z p31_subgrid
            record_p31_edit();
        }

        function record_p31_edit() {
            var pid=document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_local("p31_record.aspx?pid="+pid,"Images/worksheet_32.png");

        }

        function p31_subgrid_setting(j74id) {
            ///volá se z p31_subgrid
            sw_local("grid_designer.aspx?prefix=p31&masterprefix=p41&pid="+j74id, "Images/griddesigner_32.png",true);
        }
        function p56_subgrid_setting(j74id) {
            ///volá se z p56_subgrid
            sw_local("grid_designer.aspx?prefix=p56&masterprefix=p41&pid="+j74id, "Images/griddesigner_32.png",true);
        }

        function o23_record(pid) {
            
            sw_local("o23_record.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/notepad_32.png",true);

        }
        function o22_record(pid) {
            
            sw_local("o22_record.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/calendar_32.png",true);

        }
        function p56_record(pid,bolReturnFalse) {
            sw_local("p56_record.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/task_32.png",true);
            if (bolReturnFalse==true)
                return(false)
        }
        
        function b07_record() {
            
            sw_local("b07_create.aspx?masterprefix=p41&masterpid=<%=master.datapid%>","Images/comment_32.png",true);

        }
        function b07_reaction(b07id) {
            sw_local("b07_create.aspx?parentpid="+b07id+"&masterprefix=p41&masterpid=<%=master.datapid%>","Images/comment_32.png", true)
           
        }
        function p31_move2bin(){            
            sw_local("p31_move2bin.aspx?prefix=p41&pid=<%=master.datapid%>","Images/bin_32.png",true);
        }
        function p31_move2project(){            
            sw_local("p31_move2project.aspx?prefix=p41&pid=<%=master.datapid%>","Images/cut_32.png",true);
        }
        function p31_recalc(){            
            sw_local("p31_recalc.aspx?prefix=p41&pid=<%=master.datapid%>","Images/recalc_32.png",true);
        }
        function timeline(){            
            sw_local("entity_timeline.aspx?prefix=p41&pid=<%=master.datapid%>","Images/timeline_32.png",true);
        }
        function approve(){            
            window.parent.sw_master("entity_modal_approving.aspx?prefix=p41&pid=<%=master.datapid%>","Images/approve_32.png",true);
        }
        function tasks(){            
            window.open("p56_framework.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")
        }
        function notepads(){            
            window.open("o23_framework.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")
        }
        function invoices(){            
            window.open("p91_framework.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")            
        }
        function p31_grid(){            
            window.open("p31_grid.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")
        }
        function scheduler(){            
            window.open("entity_scheduler.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")
        }
        function p40_record(p40id){            
            sw_local("p40_record.aspx?p41id=<%=master.datapid%>&pid="+p40id,"Images/worksheet_recurrence_32.png",true);
        }
        function p47_plan(){            
            sw_local("p47_project.aspx?pid=<%=master.datapid%>","Images/plan_32.png",true);
        }
        function p48_plan(){            
            window.open("p48_framework.aspx?masterprefix=p41&masterpid=<%=master.datapid%>","_top");
        }

        function workflow(){            
            sw_local("workflow_dialog.aspx?prefix=p41&pid=<%=master.datapid%>","Images/workflow_32.png",false);
        }

     

        
       
        function RowSelected_p56(sender, args) {
            document.getElementById("<%=hiddatapid_subform.clientid%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p56(sender, args) {
            p56_record(document.getElementById("<%=hiddatapid_subform.clientid%>").value);
        }

        function RowSelected_p91(sender, args) {
            document.getElementById("<%=hiddatapid_subform.clientid%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p91(sender, args) {
            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            window.open("p91_framework.aspx?pid="+document.getElementById("<%=hiddatapid_subform.clientid%>").value,"_top")
            <%End If%>            
        }
        function periodcombo_setting() {
            
            sw_local("periodcombo_setting.aspx");
        }

        function p31_recurrence_record(pid) {            
            sw_local("p31_record.aspx?pid="+pid,"Images/worksheet_32.png");

        }
        function p30_binding() {            
            sw_local("p30_binding.aspx?masterprefix=p41&masterpid=<%=master.datapid%>","Images/person_32.png",false);
        }
        function p30_record(pid) {            
            sw_local("p30_binding.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/person_32.png",true);
        }
        function p49_plan() {
            
            sw_local("p49_plan.aspx?p41id=<%=master.datapid%>","Images/finplan_32.png",true);

        }
        function draft2normal() {

            if (confirm("Převést záznam z režimu DRAFT?")) {
                hardrefresh(<%=Master.DataPID%>,'draft2normal');
            }
            else {
                return (false);
            }
        }
        function p45_detail(){
            
            sw_local("p45_project.aspx?pid=<%=master.datapid%>","Images/budget_32.png",true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 40px;">

        <telerik:RadMenu ID="menu1" RenderMode="Lightweight" Skin="Silk" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true" EnableAutoScroll="true">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/project_32.png" alt="Projekt" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="300px">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Záznam projektu" ImageUrl="Images/arrow_down_menu.png" Value="record">
                    <ContentTemplate>
                        <div style="padding: 10px; width: 450px;">

                            <asp:Panel ID="panEdit" runat="server" CssClass="div6">
                                <img src="Images/edit.png" />
                                <asp:HyperLink ID="cmdEdit" runat="server" Text="Upravit nastavení projektu" NavigateUrl="javascript:record_edit()"></asp:HyperLink>
                                <div>
                                    <span class="infoInForm">Zahrnuje i možnost přesunutí do archviu nebo nenávratného odstranění.</span>
                                </div>

                            </asp:Panel>
                            <asp:Panel ID="panCreateCommands" runat="server">
                                <div class="div6">
                                    <img src="Images/new.png" />
                                    <asp:HyperLink ID="cmdNew" runat="server" Text="Založit nový projekt" NavigateUrl="javascript:record_new()"></asp:HyperLink>
                                    <div>
                                        <span class="infoInForm">Z aktuálního projektu se předvyplní klient, typ, středisko,projektové role, fakturační ceník, jazyk a typ faktury.</span>
                                    </div>
                                </div>
                                <div class="div6">
                                    <img src="Images/copy.png" />
                                    <asp:HyperLink ID="cmdCopy" runat="server" Text="Založit nový projekt kopírováním" NavigateUrl="javascript:record_clone()"></asp:HyperLink>
                                    <div>
                                        <span class="infoInForm">Nový projekt se kompletně předvyplní podle vzoru tohoto záznamu.</span>
                                    </div>
                                </div>

                            </asp:Panel>

                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>


                <telerik:RadMenuItem Text="Nový worksheet" ImageUrl="Images/worksheet.png" Value="p31">
                    <ContentTemplate>
                        <div style="float: left; padding: 10px;">
                            <asp:Label ID="lblP31Message" CssClass="failureNotification" runat="server"></asp:Label>
                            <asp:Repeater ID="rp1" runat="server">
                                <ItemTemplate>
                                    <div class="div6">
                                        <img src="Images/worksheet.png" />
                                        <asp:HyperLink ID="aP34" runat="server" Text=""></asp:HyperLink>
                                    </div>

                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>

                <telerik:RadMenuItem Text="Další" ImageUrl="Images/more.png" Value="more">
                    <ContentTemplate>
                        <div style="float: left; min-width: 200px;">


                            <asp:Panel ID="panCommandPivot" runat="server" CssClass="menu-group-item">
                                <img src="Images/pivot.png" />
                                <a href="p31_pivot.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>" target="_top">Worksheet Pivot za projekt</a>
                            </asp:Panel>


                            <asp:Panel ID="panP56" runat="server" CssClass="menu-group-item">
                                <img src="Images/task.png" />
                                <asp:HyperLink ID="cmdP56" runat="server" Text="Vytvořit úkol" NavigateUrl="javascript:p56_record(0);" />
                            </asp:Panel>
                            <asp:Panel ID="panP30" runat="server" CssClass="menu-group-item">
                                <img src="Images/person.png" />
                                <asp:HyperLink ID="cmdP30" runat="server" Text="Přiřadit k projektu kontaktní osobu" NavigateUrl="javascript:p30_record(0);" />
                            </asp:Panel>
                            <asp:Panel ID="panO23" runat="server" CssClass="menu-group-item">
                                <img src="Images/notepad.png" />
                                <asp:HyperLink ID="cmdO23" runat="server" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" />
                            </asp:Panel>
                            <asp:Panel ID="panO22" runat="server" CssClass="menu-group-item">
                                <img src="Images/calendar.png" />
                                <asp:HyperLink ID="cmdO22" runat="server" Text="Zapsat událost do kalendáře" NavigateUrl="javascript:o22_record(0);" />
                            </asp:Panel>
                            <asp:Panel ID="panB07" runat="server" CssClass="menu-group-item">
                                <img src="Images/comment.png" />
                                <asp:HyperLink ID="cmdB07" runat="server" Text="Zapsat k projektu komentář" NavigateUrl="javascript:b07_record();" />
                            </asp:Panel>


                            <div class="menu-group-title">
                                Worksheet operace
                            </div>
                            <asp:Panel ID="panP40" runat="server" CssClass="menu-group-item">
                                <img src="Images/worksheet_recurrence.png" />
                                <asp:HyperLink ID="cmdP40Create" runat="server" Text="Zapsat opakovanou odměnu/paušál/úkon" NavigateUrl="javascript: p40_record(0)"></asp:HyperLink>
                            </asp:Panel>
                            <asp:Panel ID="panP31Recalc" runat="server" CssClass="menu-group-item">
                                <img src="Images/recalc.png" />
                                <asp:HyperLink ID="cmdP31Recalc" runat="server" Text="Přepočítat sazby rozpracovaných úkonů" NavigateUrl="javascript: p31_recalc()"></asp:HyperLink>
                            </asp:Panel>
                            <asp:Panel ID="panP31MoveToBin" runat="server" CssClass="menu-group-item">
                                <img src="Images/bin.png" />
                                <asp:HyperLink ID="cmdP31Move2Bin" runat="server" Text="Přesunout rozpracovanost do/z archivu" NavigateUrl="javascript: p31_move2bin()"></asp:HyperLink>
                            </asp:Panel>
                            <asp:Panel ID="panP31Move2OtherProject" runat="server" CssClass="menu-group-item">
                                <img src="Images/cut.png" />
                                <asp:HyperLink ID="cmdP31MoveToOtherProject" runat="server" Text="Přesunout rozpracovanost na jiný projekt" NavigateUrl="javascript:p31_move2project()"></asp:HyperLink>
                            </asp:Panel>

                            <div class="menu-group-title">
                                Plánování
                            </div>
                            <div class="menu-group-item">
                                <img src="Images/plan.png" />
                                <asp:HyperLink ID="cmdP47" runat="server" Text="Kapacitní" NavigateUrl="javascript:p47_plan()"></asp:HyperLink>


                                <img src="Images/finplan.png" />
                                <asp:HyperLink ID="cmdP49" runat="server" Text="Finanční" NavigateUrl="javascript:p49_plan()"></asp:HyperLink>

                                <img src="Images/oplan.png" />
                                <asp:HyperLink ID="cmdP48" runat="server" Text="Operativní" NavigateUrl="javascript:p48_plan()"></asp:HyperLink>

                            </div>
                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>

            </Items>
        </telerik:RadMenu>

    </asp:Panel>

    <div style="height: 3px; page-break-after: always"></div>
    <div class="div_radiolist_metro">
        <asp:HyperLink ID="topLink0" runat="server" Text="Úkony" CssClass="toplink" NavigateUrl="javascript:p31_grid()" Style="margin-left: 6px;"></asp:HyperLink>
        <asp:HyperLink ID="topLink1" runat="server" Text="Schvalování/fakturační podklady/fakturace" CssClass="toplink" NavigateUrl="javascript:approve()"></asp:HyperLink>
        <asp:HyperLink ID="topLink4" runat="server" Text="Sestava" CssClass="toplink" NavigateUrl="javascript:report()"></asp:HyperLink>
        <asp:HyperLink ID="topLink2" runat="server" Text="Úkoly" CssClass="toplink" NavigateUrl="javascript:tasks()"></asp:HyperLink>
        <asp:HyperLink ID="topLink6" runat="server" Text="Faktury" CssClass="toplink" NavigateUrl="javascript:invoices()"></asp:HyperLink>
        <asp:HyperLink ID="topLink3" runat="server" Text="Kalendář projektu" CssClass="toplink" NavigateUrl="javascript:scheduler()"></asp:HyperLink>
    </div>



    <div class="content-box1">
        <div class="title">
            <img src="Images/properties.png" style="margin-right: 10px;" />
            <asp:Label ID="boxCoreTitle" Text="Záznam projektu" runat="server"></asp:Label>
            <asp:HyperLink ID="cmdNewWindow" runat="server" ImageUrl="Images/open_in_new_window.png" Target="_blank" ToolTip="Otevřít v nové záložce" CssClass="button-link" Style="float: right; vertical-align: top; padding: 0px;"></asp:HyperLink>
        </div>
        <div class="content">

            <table cellpadding="10" cellspacing="2" id="responsive">
                <tr valign="baseline">
                    <td style="min-width: 120px;">
                        <asp:Label ID="lblProject" runat="server" Text="Projekt:" CssClass="lbl"></asp:Label>
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
                    </td>

                </tr>
                <tr valign="baseline">
                    <td>

                        <asp:Label ID="lblClient" runat="server" Text="Klient:" CssClass="lbl"></asp:Label>

                    </td>
                    <td>

                        <asp:HyperLink ID="Client" runat="server" NavigateUrl="#" Target="_parent"></asp:HyperLink>
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
                        <asp:Label ID="lblPlan" runat="server" Text="Plánování:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="PlanPeriod" runat="server" CssClass="val"></asp:Label>
                        <div>
                            <img src="Images/plan.png" />
                            <a href="javascript: p47_plan()">Kapacitní plán</a>
                            <img src="Images/finplan.png" />
                            <a href="javascript: p49_plan()">Finanční plán</a>
                            <img src="Images/oplan.png" />
                            <a href="javascript: p48_plan()">Operativní plán</a>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:Label ID="lblX51" runat="server" Text="Fakturační ceník:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>

                        <asp:Label ID="p51Name_Billing" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_p51id_billing" runat="server" CssClass="reczoom" Text="i" title="Detail ceníku projektu"></asp:HyperLink>
                        <asp:Label ID="lblX51_Message" runat="server" CssClass="lbl"></asp:Label>

                        <asp:HyperLink ID="clue_p41_billing" runat="server" CssClass="reczoom" Text="i" title="Detail fakturačního nastavení projektu"></asp:HyperLink>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblP42Name" runat="server" Text="Typ projektu:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="p42Name" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_p42name" runat="server" CssClass="reczoom" Text="i" title="Detail typu projektu"></asp:HyperLink>

                        <asp:Label ID="lblJ18Name" runat="server" Text="Středisko:" CssClass="lbl"></asp:Label>
                        <asp:Label ID="j18Name" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_j18name" runat="server" CssClass="reczoom" Text="i" title="Detail střediska"></asp:HyperLink>
                    </td>

                </tr>


            </table>
            <div>
                <asp:HyperLink ID="cmdLog" runat="server" Text="Historie projektu" NavigateUrl="javascript: timeline()"></asp:HyperLink>


            </div>

        </div>
    </div>

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
            <asp:Label ID="boxP31SummaryTitle" runat="server" Text="Worksheet summary"></asp:Label>
        </div>
        <div class="content">
            <uc:entity_worksheet_summary ID="p31summary1" runat="server"></uc:entity_worksheet_summary>
        </div>
    </asp:Panel>
    <asp:Panel ID="boxP30" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/person.png" style="margin-right: 10px;" />
            <asp:Label ID="boxP30Title" runat="server" Text="Kontaktní osoby projektu"></asp:Label>
            <asp:HyperLink ID="cmdEditP30" runat="server" NavigateUrl="javascript:p30_binding()" Text="Upravit" Style="margin-left: 20px;"></asp:HyperLink>
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
            <asp:Label ID="boxRolesTitle" runat="server" Text="Projektové role"></asp:Label>
        </div>
        <div class="content">
            <uc:entityrole_assign_inline ID="roles_project" runat="server" EntityX29ID="p41Project" NoDataText="V projektu nejsou přiřazeny projektové role."></uc:entityrole_assign_inline>
        </div>
    </asp:Panel>

    <asp:Panel ID="boxO23" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/notepad.png" style="margin-right: 10px;" />
            <asp:Label ID="boxO23Title" runat="server" Text="Dokumenty"></asp:Label>
        </div>
        <div class="content" style="overflow: auto; max-height: 200px;">

            <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p41Project"></uc:o23_list>


        </div>
    </asp:Panel>


    <div style="clear: both; width: 100%;"></div>
    <telerik:RadTabStrip ID="opgSubgrid" runat="server" Skin="Metro" Width="100%" AutoPostBack="true">
        <Tabs>
            <telerik:RadTab Text="Worksheet summary" Value="-1" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Worksheet přehled" Value="1"></telerik:RadTab>
            <telerik:RadTab Text="Úkoly" Value="4"></telerik:RadTab>
            <telerik:RadTab Text="Rozpočet" Value="5"></telerik:RadTab>
            <telerik:RadTab Text="Vystavené faktury" Value="2"></telerik:RadTab>
            <telerik:RadTab Text="Komentáře a workflow" Value="3"></telerik:RadTab>
            <telerik:RadTab Text="x" Value="0" ToolTip="Nezobrazovat pod-přehled"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>


    <uc:p31_bigsummary ID="bigsummary1" runat="server" MasterDataPrefix="p41" />

    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="p41Project" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick" AllowMultiSelect="true"></uc:p31_subgrid>
    <uc:b07_list ID="comments1" runat="server" JS_Create="b07_record()" JS_Reaction="b07_reaction" />

    <uc:p56_subgrid ID="gridP56" runat="server" x29ID="p41Project" />
    <uc:p91_subgrid ID="gridP91" runat="server" x29ID="p41Project" />

    <asp:Panel ID="panP45" runat="server" Visible="false">
        <div>
            <asp:DropDownList ID="p45ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="VersionWithName"></asp:DropDownList>
            <button type="button" id="cmdP45" runat="server" onclick="p45_detail()" class="cmd">Nastavení rozpočtu</button>
        </div>
        <uc:datagrid ID="gridP46" runat="server"></uc:datagrid>
        
    </asp:Panel>

    <asp:HiddenField ID="hiddatapid_subform" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


</asp:Content>
