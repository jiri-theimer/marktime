<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p41_framework_detail.aspx.vb" Inherits="UI.p41_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>
<%@ Register TagPrefix="uc" TagName="entity_worksheet_summary" Src="~/entity_worksheet_summary.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
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
            hh=hh-4;
            document.getElementById("<%=me.fraSubform.ClientID%>").style.height=hh+"px";
        }

        function report() {
            
            sw_decide("report_modal.aspx?prefix=p41&pid=<%=Master.DataPID%>","Images/reporting.png",true);

        }

        function sw_decide(url, iconUrl, is_maximize) {
            var isInIFrame = (window.location != window.parent.location);
            if (isInIFrame==true){

                var w = parseInt(document.getElementById("<%=hidParentWidth.ClientID%>").value);
                var h = screen.availHeight;

                if ((w < 901 || h < 800) && w>0) {
                    window.parent.sw_master(url, iconUrl);
                    return;
                }                

                if (w < 910)
                    is_maximize = true;
            }
            sw_local(url, iconUrl, is_maximize);
        }
        
        
        
        function p31_entry_menu(p34id) {
            ///z menu1           
            sw_decide("p31_record.aspx?pid=0&p41id=<%=Master.DataPID%>&p34id="+p34id,"Images/worksheet.png",true);
            

        }

        function record_new() {
            
            sw_decide("p41_create.aspx?client_family=1&pid=<%=Master.DataPID%>","Images/project.png",true);

        }

        function record_edit() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_decide("p41_record.aspx?pid=" + pid,"Images/project.png",true);

        }
        
        
        function record_clone() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_decide("p41_create.aspx?clone=1&pid=" + pid,"Images/project.png",true);

        }

        function favourite(){
            hardrefresh(<%=Master.DataPID%>,'favourite');
           
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
            
            sw_decide("o23_record.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/notepad.png",true);

        }
        function o22_record(pid) {
            
            sw_decide("o22_record.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/calendar.png",true);

        }
        function p56_record(pid,bolReturnFalse) {
            sw_decide("p56_record.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/task.png",true);
            if (bolReturnFalse==true)
                return(false)
        }
        
        
        function p31_move2bin(){            
            sw_decide("p31_move2bin.aspx?prefix=p41&pid=<%=master.datapid%>","Images/bin.png",true);
        }
        function p31_move2project(){            
            sw_decide("p31_move2project.aspx?prefix=p41&pid=<%=master.datapid%>","Images/cut.png",true);
        }
        function p31_recalc(){            
            sw_decide("p31_recalc.aspx?prefix=p41&pid=<%=master.datapid%>","Images/recalc.png",true);
        }
        function timeline(){            
            sw_decide("entity_timeline.aspx?prefix=p41&pid=<%=master.datapid%>","Images/timeline.png",true);
        }
        function approve(){     
            var isInIFrame = (window.location != window.parent.location);
            if (isInIFrame==true){
                window.parent.sw_master("entity_modal_approving.aspx?prefix=p41&pid=<%=master.datapid%>","Images/approve_32.png",true);
            }
            else{
                sw_decide("entity_modal_approving.aspx?prefix=p41&pid=<%=master.datapid%>","Images/approve_32.png",true);
            }
            
        }
        function tasks(){            
            window.open("p56_framework.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")
        }
        function childs(){            
            window.open("p41_framework.aspx?masterprefix=p41&masterpid=<%=Master.DataPID%>","_top")
            
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
            sw_decide("p40_record.aspx?p41id=<%=master.datapid%>&pid="+p40id,"Images/worksheet_recurrence.png",true);
        }
        
        function p48_plan(){            
            window.open("p48_framework.aspx?masterprefix=p41&masterpid=<%=master.datapid%>","_top");
        }

        function workflow(){            
            sw_decide("workflow_dialog.aspx?prefix=p41&pid=<%=master.datapid%>","Images/workflow.png",false);
        }

        
        
        function p31_recurrence_record(pid) {            
            sw_decide("p31_record.aspx?pid="+pid,"Images/worksheet.png");

        }
        function p30_binding() {            
            sw_decide("p30_binding.aspx?masterprefix=p41&masterpid=<%=master.datapid%>","Images/person.png",false);
        }
        function p30_record(pid) {            
            sw_decide("p30_binding.aspx?masterprefix=p41&masterpid=<%=master.datapid%>&pid="+pid,"Images/person.png",true);
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

            sw_decide("b07_create.aspx?masterprefix=p41&masterpid=<%=master.datapid%>", "Images/comment.png", true);

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
            sw_decide("entity_framework_detail_setting.aspx?prefix=p41", "Images/setting.png",false);
        }
        function stoploading(){            
            document.getElementById("<%=me.imgLoading.clientid%>").style.display="none";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height:44px;border-bottom:solid 1px gray;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Default" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true" EnableAutoScroll="true" Width="100%">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/project_32.png" alt="Projekt" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="280px">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="saw" text="<img src='Images/open_in_new_window.png'/>" Target="_blank" NavigateUrl="p41_framework_detail.aspx?saw=1" ToolTip="Otevřít projekt v nové záložce prohlížeče"></telerik:RadMenuItem>          
                <telerik:RadMenuItem Value="switch" NavigateUrl="javascript:OnSwitch()" Text="&darr;&uarr;" ToolTip="Skrýt/zobrazit horní polovinu detailu projektu (boxy)">
                </telerik:RadMenuItem>                
                <telerik:RadMenuItem Text="ZÁZNAM PROJEKTU" ImageUrl="Images/arrow_down_menu.png" Value="record" Style="margin-top: 6px;" meta:resourcekey="menu_zaznam">
                    <Items>
                        <telerik:RadMenuItem Value="cmdEdit" Text="Upravit kartu projektu" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png" ToolTip="Zahrnuje i možnost přesunutí do archviu nebo nenávratného odstranění." meta:resourcekey="menu_upravit"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdNew" Text="Založit nový projekt" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png" ToolTip="Z aktuálního projektu se předvyplní klient, typ, středisko,projektové role, fakturační ceník, jazyk a typ faktury." meta:resourcekey="menu_novy"></telerik:RadMenuItem>

                        <telerik:RadMenuItem Value="cmdCopy" Text="Založit nový projekt kopírováním" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" ToolTip="Nový projekt se kompletně předvyplní podle vzoru tohoto záznamu." meta:resourcekey="menu_kopirovat"></telerik:RadMenuItem>

                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdApprove" Text="Schvalovat nebo vystavit fakturu" NavigateUrl="javascript:approve()" ImageUrl="Images/approve.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:RadMenuItem>

                    </Items>
                </telerik:RadMenuItem>


                <telerik:RadMenuItem Text="ZAPSAT WORKSHEET" ImageUrl="Images/worksheet.png" Value="p31" meta:resourcekey="menu_zapsat_worksheet"></telerik:RadMenuItem>

                <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/menuarrow.png" Value="more" meta:resourcekey="menu_dalsi">
                    <Items>
                        <telerik:RadMenuItem Value="switchHeight" Text="Nastavení vzhledu stránky" ImageUrl="Images/setting.png" NavigateUrl="javascript:page_setting()" meta:resourcekey="switchHeight">                           
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdPivot" Text="Worksheet PIVOT za projekt" Target="_top" ImageUrl="Images/pivot.png" meta:resourcekey="cmdPivot"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP30" Text="Přiřadit k projektu kontaktní osobu" NavigateUrl="javascript:p30_record(0);" ImageUrl="Images/person.png" meta:resourcekey="cmdP30"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP56" Text="Vytvořit úkol" NavigateUrl="javascript:p56_record(0);" ImageUrl="Images/task.png" meta:resourcekey="cmdP56"></telerik:RadMenuItem>                        
                        <telerik:RadMenuItem Value="cmdO23" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" ImageUrl="Images/notepad.png" meta:resourcekey="cmdO23"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO22" Text="Zapsat událost do kalendáře" NavigateUrl="javascript:o22_record(0);" ImageUrl="Images/calendar.png" meta:resourcekey="cmdO22"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO22" Text="Kalendář projektu" NavigateUrl="javascript:scheduler()" ImageUrl="Images/calendar.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        
                        
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP40Create" Text="Definovat opakovanou odměnu/paušál/úkon" NavigateUrl="javascript:p40_record(0);" ImageUrl="Images/worksheet_recurrence.png" meta:resourcekey="cmdP40Create"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP31Recalc" Text="Přepočítat sazby rozpracovaných čas.úkonů" NavigateUrl="javascript:p31_recalc();" ImageUrl="Images/recalc.png" meta:resourcekey="cmdP31Recalc"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP31Move2Bin" Text="Přesunout nevyfakturované do/z archivu" NavigateUrl="javascript:p31_move2bin();" ImageUrl="Images/bin.png" meta:resourcekey="cmdP31Move2Bin"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP31MoveToOtherProject" Text="Přesunout rozpracovanost na jiný projekt" NavigateUrl="javascript:p31_move2project();" ImageUrl="Images/cut.png" meta:resourcekey="cmdP31MoveToOtherProject"></telerik:RadMenuItem>

                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdB07" Text="Zapsat komentář" NavigateUrl="javascript:b07_record();" ImageUrl="Images/comment.png" meta:resourcekey="cmdB07"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdX40" Text="Historie odeslané pošty" Target="_top" ImageUrl="Images/email.png" meta:resourcekey="cmdX40"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP48" Text="Operativní plán" NavigateUrl="javascript:p48_plan();" ImageUrl="Images/oplan.png" meta:resourcekey="cmdP48"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdLog" Text="Historie záznamu" NavigateUrl="javascript: timeline()" ImageUrl="Images/event.png" meta:resourcekey="cmdLog"></telerik:RadMenuItem>
                    </Items>

                </telerik:RadMenuItem>

            </Items>
        </telerik:RadMenu>

    </asp:Panel>

    <div style="clear:both;"></div>
  <p></p>


    <asp:Panel ID="panSwitch" runat="server" Style="height: 300px; overflow: auto;">
        <div class="content-box1">
            <div class="title">                
                <img src="Images/properties.png" style="margin-right: 10px;" />
                
                <asp:Label ID="boxCoreTitle" Text="Záznam projektu" runat="server" meta:resourcekey="boxCoreTitle"></asp:Label>                

                <asp:HyperLink ID="cmdChilds" runat="server" Text="Podřízené projekty" NavigateUrl="javascript:childs()" Visible="false" style="float:right;"></asp:HyperLink>
                <asp:HyperLink ID="cmdFavourite" runat="server" ImageUrl="Images/not_favourite.png" ToolTip="Zařadit do mých oblíbených projektů" NavigateUrl="javascript:favourite()" style="float:right;border-radius:5px;border:solid 1px gray;"></asp:HyperLink>
                
                
            </div>
            <div class="content">

                <table cellpadding="10" cellspacing="2" id="responsive">
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
                        </td>

                    </tr>
                    <tr valign="baseline">
                        <td>

                            <asp:Label ID="lblClient" runat="server" Text="Klient:" CssClass="lbl" meta:resourcekey="lblClient"></asp:Label>

                        </td>
                        <td>

                            <asp:HyperLink ID="Client" runat="server" NavigateUrl="#" Target="_parent"></asp:HyperLink>
                            <asp:HyperLink ID="clue_client" runat="server" CssClass="reczoom" Text="i" title="Detail klienta"></asp:HyperLink>
                            <asp:Image ID="imgFlag_Client" runat="server" />

                        </td>

                    </tr>
                     <tr id="trParent" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Nadřízený projekt:" CssClass="lbl"></asp:Label>

                        </td>
                        <td>                            
                            <asp:HyperLink ID="ParentProject" runat="server" Target="_top"></asp:HyperLink>

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
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Label ID="lblX51" runat="server" Text="Fakturační ceník:" CssClass="lbl" meta:resourcekey="lblX51"></asp:Label>
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
        <asp:panel ID="boxX18" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/label.png" style="margin-right: 10px;" />
                <asp:Label ID="boxX18Title" runat="server" Text="Štítky" meta:resourcekey="boxX18Title"></asp:Label>
                <asp:HyperLink ID="x18_binding" runat="server" Text="Přiřadit" meta:resourcekey="x18_binding"></asp:HyperLink>
            </div>
            <div class="content">
                <uc:x18_readonly id="labels1" runat="server"></uc:x18_readonly>
            </div>
        </asp:panel>
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

        <asp:Panel ID="boxO23" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/notepad.png" style="margin-right: 10px;" />
                <asp:Label ID="boxO23Title" runat="server" Text="Dokumenty" meta:resourcekey="boxO23Title"></asp:Label>
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
            <telerik:RadTab Text="Worksheet přehled" Value="1" Target="fraSubform" meta:resourcekey="tab_worksheetprehled"></telerik:RadTab>
            <telerik:RadTab Text="Úkoly" Value="4" Target="fraSubform" meta:resourcekey="topLink2"></telerik:RadTab>
            <telerik:RadTab Text="Rozpočet" Value="5" Target="fraSubform" meta:resourcekey="tab_rozpocet"></telerik:RadTab>
            <telerik:RadTab Text="Vystavené faktury" Value="2" Target="fraSubform" meta:resourcekey="tab_faktury"></telerik:RadTab>
            <telerik:RadTab Text="Komentáře a workflow" Value="3" Target="fraSubform" meta:resourcekey="tab_workflow"></telerik:RadTab>
            <telerik:RadTab Text="x" Value="0" ToolTip="Nezobrazovat pod-přehled" Target="fraSubform" meta:resourcekey="tab_none"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <div id="offsetY"></div>
    <iframe frameborder="0" id="fraSubform" name="fraSubform" runat="server" width="100%" height="300px"></iframe>
    <asp:Image ID="imgLoading" runat="server" ImageUrl="Images/loading.gif" style="position:absolute;top:500px;left:200px;" />

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />

    <asp:HiddenField ID="hidIsBin" runat="server" />
    <asp:HiddenField ID="hidIsCanApprove" runat="server" />
    <asp:HiddenField ID="hidParentWidth" runat="server" />

    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


</asp:Content>
