<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p41_framework_detail.aspx.vb" Inherits="UI.p41_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>
<%@ Register TagPrefix="uc" TagName="entity_worksheet_summary" Src="~/entity_worksheet_summary.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        html .RadMenu_Metro .rmRootGroup {
            background-image: none;
        }

        html .RadMenu_Metro ul.rmRootGroup {
            <%if me.hidisbin.value="1" then%> background-color: black;
            <%else%> background-color: white;
            <%End If%>;
        }

        .rmLink {
            margin-top: 6px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {           

            AdjustHeight();
        });

        function AdjustHeight(){
            var h1 = new Number;
            var h2 = new Number;
            var hh = new Number;

            h1 = $(window).height();

            ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;
            hh = h1 - h2;

            if (navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0) {
                hh=hh-10;
            }
            
            document.getElementById("<%=me.fraSubform.ClientID%>").style.height=hh+"px";
        }

        function report() {
            
            sw_local("report_modal.aspx?prefix=p41&pid=<%=Master.DataPID%>","Images/reporting_32.png",true);

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
        
        function p48_plan(){            
            window.open("p48_framework.aspx?masterprefix=p41&masterpid=<%=master.datapid%>","_top");
        }

        function workflow(){            
            sw_local("workflow_dialog.aspx?prefix=p41&pid=<%=master.datapid%>","Images/workflow_32.png",false);
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
        
        function draft2normal() {

            if (confirm("Převést záznam z režimu DRAFT?")) {
                hardrefresh(<%=Master.DataPID%>,'draft2normal');
            }
            else {
                return (false);
            }
        }
        
        function OnSwitch()
        {       
            var s="none";
            if (document.getElementById("<%=Me.panSwitch.ClientID%>").style.display=="none")
                s="block";
           
            document.getElementById("<%=Me.panSwitch.ClientID%>").style.display=s;

            $.post("Handler/handler_userparam.ashx", { x36value: s, x36key: "p41_framework_detail-switch", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }                
            });  
            AdjustHeight();
        }
        function b07_record() {

            sw_local("b07_create.aspx?masterprefix=p41&masterpid=<%=master.datapid%>", "Images/comment_32.png", true);

        }
        function OnClientTabSelected(sender, eventArgs)
        {
            var tab = eventArgs.get_tab();
            var s=tab.get_value();
            $.post("Handler/handler_userparam.ashx", { x36value: s, x36key: "p41_framework_detail-subgrid", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }                
            });
            <%If Me.fraSubform.Visible = False Then%>
            location.replace("p41_framework_detail.aspx?tab="+s);           
            <%End If%>
            if (s=="0")
                location.replace("p41_framework_detail.aspx?tab="+s)
            
                
        }
        function page_setting(){
            sw_local("entity_framework_detail_setting.aspx?prefix=p41", "Images/setting_32.png",false);
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 40px;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true" EnableAutoScroll="true" Width="100%">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/project_32.png" alt="Projekt" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="280px">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="switch" NavigateUrl="javascript:OnSwitch()" Text="&darr;&uarr;" ToolTip="Skrýt/zobrazit horní polovinu detailu projektu (boxy)">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="ZÁZNAM PROJEKTU" ImageUrl="Images/arrow_down_menu.png" Value="record" Style="margin-top: 6px;">
                    <Items>
                        <telerik:RadMenuItem Value="cmdEdit" Text="Upravit nastavení projektu" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png" ToolTip="Zahrnuje i možnost přesunutí do archviu nebo nenávratného odstranění."></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdNew" Text="Založit nový projekt" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png" ToolTip="Z aktuálního projektu se předvyplní klient, typ, středisko,projektové role, fakturační ceník, jazyk a typ faktury."></telerik:RadMenuItem>

                        <telerik:RadMenuItem Value="cmdCopy" Text="Založit nový projekt kopírováním" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" ToolTip="Nový projekt se kompletně předvyplní podle vzoru tohoto záznamu."></telerik:RadMenuItem>

                    </Items>
                </telerik:RadMenuItem>


                <telerik:RadMenuItem Text="ZAPSAT WORKSHEET" ImageUrl="Images/worksheet.png" Value="p31"></telerik:RadMenuItem>

                <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/more.png" Value="more">
                    <Items>
                        <telerik:RadMenuItem Value="switchHeight" Text="Nastavení vzhledu stránky" ImageUrl="Images/setting.png" NavigateUrl="javascript:page_setting()">                           
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdPivot" Text="Worksheet Pivot za projekt" Target="_top" ImageUrl="Images/pivot.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP30" Text="Přiřadit k projektu kontaktní osobu" NavigateUrl="javascript:p30_record(0);" ImageUrl="Images/person.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP56" Text="Vytvořit úkol" NavigateUrl="javascript:p56_record(0);" ImageUrl="Images/task.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO23" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" ImageUrl="Images/notepad.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO22" Text="Zapsat událost do kalendáře" NavigateUrl="javascript:o22_record(0);" ImageUrl="Images/calendar.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdB07" Text="Zapsat komentář" NavigateUrl="javascript:b07_record();" ImageUrl="Images/comment.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP40Create" Text="Zapsat opakovanou odměnu/paušál/úkon" NavigateUrl="javascript:p40_record(0);" ImageUrl="Images/worksheet_recurrence.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP31Recalc" Text="Přepočítat sazby rozpracovaných úkonů" NavigateUrl="javascript:p31_recalc();" ImageUrl="Images/recalc.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP31Move2Bin" Text="Přesunout rozpracovanost do/z archivu" NavigateUrl="javascript:p31_move2bin();" ImageUrl="Images/bin.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP31MoveToOtherProject" Text="Přesunout rozpracovanost na jiný projekt" NavigateUrl="javascript:p31_move2project();" ImageUrl="Images/cut.png"></telerik:RadMenuItem>

                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdX40" Text="Historie odeslané pošty" Target="_top" ImageUrl="Images/email.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP48" Text="Operativní plán" NavigateUrl="javascript:p48_plan();" ImageUrl="Images/oplan.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdLog" Text="Historie záznamu" NavigateUrl="javascript: timeline()" ImageUrl="Images/event.png"></telerik:RadMenuItem>
                    </Items>

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


    <asp:Panel ID="panSwitch" runat="server" Style="height: 300px; overflow: auto;">
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
                            <asp:Label ID="lblPlan" runat="server" Text="Zahájení/dokončení:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="PlanPeriod" runat="server" CssClass="val"></asp:Label>
                            <div>

                                <a href="javascript: p48_plan()">Operativní plán projektu</a>
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
    </asp:Panel>

    <div style="clear: both; width: 100%;"></div>
    <telerik:RadTabStrip ID="opgSubgrid" runat="server" Skin="Metro" Width="100%" AutoPostBack="false" OnClientTabSelected="OnClientTabSelected">
        <Tabs>
            <telerik:RadTab Text="Worksheet summary" Value="-1" Selected="true" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Worksheet přehled" Value="1" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Úkoly" Value="4" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Rozpočet" Value="5" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Vystavené faktury" Value="2" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Komentáře a workflow" Value="3" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="x" Value="0" ToolTip="Nezobrazovat pod-přehled" Target="fraSubform"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <div id="offsetY"></div>
    <iframe frameborder="0" id="fraSubform" name="fraSubform" runat="server" width="100%" height="300px"></iframe>
   

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />

    <asp:HiddenField ID="hidIsBin" runat="server" />
    <asp:HiddenField ID="hidIsCanApprove" runat="server" />

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


</asp:Content>
