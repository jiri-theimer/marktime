<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p28_framework_detail.aspx.vb" Inherits="UI.p28_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="p28_address" Src="~/p28_address.ascx" %>
<%@ Register TagPrefix="uc" TagName="p28_medium" Src="~/p28_medium.ascx" %>
<%@ Register TagPrefix="uc" TagName="contactpersons" Src="~/contactpersons.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="entity_worksheet_summary" Src="~/entity_worksheet_summary.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="p56_subgrid" Src="~/p56_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="p91_subgrid" Src="~/p91_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_bigsummary" Src="~/p31_bigsummary.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="Scripts/jqueryui/jquery-ui.min.css" />
    <script src="Scripts/jqueryui/jquery-ui.min.js" type="text/javascript"></script>

    <style type="text/css">
       

        .ui-autocomplete {
            width: 600px;
            height: 300px;
            overflow-y: auto;
            /* prevent horizontal scrollbar */
            overflow-x: hidden;
            font-family: 'Microsoft Sans Serif';
            z-index: 9900;
        }

        * html .ui-autocomplete {
            height: 300px;
        }


        .ui-state-hover, .ui-widget-content .ui-state-hover, .ui-widget-header .ui-state-hover, .ui-state-focus, .ui-widget-content .ui-state-focus, .ui-widget-header .ui-state-focus {
            background: #DCDCDC;
            border: none;
            border-radius: 0;
            font-weight: normal;
        }

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

      

        function record_new() {
            
            sw_local("p28_record.aspx?pid=0","Images/contact_32.png",true);

        }

        function p41_new() {
            
            sw_local("p41_create.aspx?p28id=<%=Master.DataPID%>","Images/project_32.png",true);

        }
        function report() {
            
            sw_local("report_modal.aspx?prefix=p28&pid=<%=Master.DataPID%>","Images/reporting_32.png",true);

        }

        function record_edit() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_local("p28_record.aspx?pid=" + pid,"Images/contact_32.png",true);

        }
        
        
        function record_clone() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_local("p28_record.aspx?clone=1&pid=" + pid,"Images/contact_32.png",true);

        }

        function hardrefresh(pid, flag) {
            if (flag=="p28-create"){
                parent.window.location.replace("p28_framework.aspx?pid="+pid);
                return;
            }
            if (flag=="p28-delete"){
                parent.window.location.replace("p28_framework.aspx");
                return;
            }
            
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        function p31_entry() {
            ///volá se z p31_subgrid
            sw_local("p31_record.aspx?pid=0&p28id=<%=master.DataPID%>","Images/worksheet_32.png",true);
            return(false);

        }
        
        function p31_clone() {
            ///volá se z p31_subgrid
            var pid=document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_local("p31_record.aspx?clone=1&pid="+pid,"Images/worksheet_32.png",true);
            return(false);
        }


        function p31_RowSelected(sender, args) {

            document.getElementById("<%=hiddatapid_p31.clientid%>").value = args.getDataKeyValue("pid");

        }

        function p31_RowDoubleClick(sender, args) {
            record_p31_edit();
        }

        function record_p31_edit() {
            var pid=document.getElementById("<%=hiddatapid_p31.clientid%>").value;
            sw_local("p31_record.aspx?pid="+pid,"Images/worksheet_32.png");

        }

        function p31_subgrid_setting(j74id) {
          
            sw_local("grid_designer.aspx?prefix=p31&masterprefix=p28&pid="+j74id, "Images/griddesigner_32.png",true);
        }
        function p56_subgrid_setting(j74id) {
            ///volá se z p56_subgrid
            sw_local("grid_designer.aspx?prefix=p56&masterprefix=p28&pid="+j74id, "Images/griddesigner_32.png",true);
        }
        function p30_record(pid) {            
            sw_local("p30_binding.aspx?masterprefix=p28&masterpid=<%=master.datapid%>&pid="+pid,"Images/person_32.png",true);
        }

        function o23_record(pid) {
            
            sw_local("o23_record.aspx?masterprefix=p28&masterpid=<%=master.datapid%>&pid="+pid,"Images/notepad_32.png",true);

        }
        function o22_record(pid) {
            
            sw_local("o22_record.aspx?masterprefix=p28&masterpid=<%=master.datapid%>&pid="+pid,"Images/calendar_32.png",true);

        }
        function b07_record() {
            
            sw_local("b07_create.aspx?masterprefix=p28&masterpid=<%=master.datapid%>","Images/comment_32.png",true);

        }
        function b07_reaction(b07id) {
            sw_local("b07_create.aspx?parentpid="+b07id+"&masterprefix=p28&masterpid=<%=master.datapid%>","Images/comment_32.png", true)
           
        }

        
        function timeline(){            
            sw_local("entity_timeline.aspx?prefix=p28&pid=<%=master.datapid%>","Images/timeline_32.png",true);
        }
        
        function RowSelected_p56(sender, args) {
            document.getElementById("<%=hiddatapid_subform.clientid%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p56(sender, args) {
            p56_record(document.getElementById("<%=hiddatapid_subform.clientid%>").value);
        }
        function p56_record(pid,bolReturnFalse) {
            sw_local("p56_record.aspx?masterprefix=p28&masterpid=<%=master.datapid%>&pid="+pid,"Images/task_32.png",true);
            if (bolReturnFalse==true)
                return(false)
        
        }
        function p56_clone() {
            ///volá se z gridu úkolů
            var pid=document.getElementById("<%=hiddatapid_subform.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return(false);
            }
            sw_local("p56_record.aspx?clone=1&pid="+pid,"Images/task_32.png",true);
            return(false);
        }
        function p31_entry_p56() {
            ///volá se z gridu úkolů
            var p56id=document.getElementById("<%=hiddatapid_subform.clientid%>").value;
            if (p56id == "" || p56id == null) {
                alert("Není vybrán úkol.");
                return(false);
            }
            sw_local("p31_record.aspx?pid=0&p28id=<%=Master.DataPID%>&p56id="+p56id,"Images/worksheet_32.png",true);
            return(false);
        }
        function RowSelected_p91(sender, args) {
            document.getElementById("<%=hiddatapid_subform.clientid%>").value = args.getDataKeyValue("pid");
        }

        function RowDoubleClick_p91(sender, args) {
            <%If Master.Factory.SysUser.j04IsMenu_Invoice Then%>
            window.open("p91_framework.aspx?pid="+document.getElementById("<%=hiddatapid_subform.clientid%>").value,"_top")
            <%End If%>            
        }
        function p30_binding() {
            
            sw_local("p30_binding.aspx?masterprefix=p28&masterpid=<%=master.datapid%>","Images/person_32.png",false);

        }
        function approve(){            
            window.parent.sw_master("entity_modal_approving.aspx?prefix=p28&pid=<%=master.datapid%>","Images/approve_32.png",true);
        }
        function tasks(){            
            window.open("p56_framework.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>","_top")
            
        }
        function projects(){            
            window.open("p41_framework.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>","_top")
            
        }
        function invoices(){            
            window.open("p91_framework.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>","_top")
            
        }
        function notepads(){            
            window.open("o23_framework.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>","_top")
        }
        function p31_grid(){            
            window.open("p31_grid.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>","_top")
        }
        function scheduler(){            
            window.open("entity_scheduler.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>","_top")
        }
        function draft2normal() {

            if (confirm("Převést záznam z režimu DRAFT?")) {
                hardrefresh(<%=Master.DataPID%>,'draft2normal');
            }
            else {
                return (false);
            }
        }
        function p48_plan(){            
            window.open("p48_framework.aspx?masterprefix=p28&masterpid=<%=master.datapid%>","_top");
        }
        function workflow(){            
            sw_local("workflow_dialog.aspx?prefix=p28&pid=<%=master.datapid%>","Images/workflow_32.png",false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 40px;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Silk" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/contact_32.png" alt="Klient" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="300px">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Záznam klienta" ImageUrl="Images/arrow_down_menu.png" Value="record">
                    <ContentTemplate>
                        <div style="padding: 10px; width: 450px;">

                            <asp:Panel ID="panEdit" runat="server" CssClass="div6">
                                <img src="Images/edit.png" />
                                <asp:HyperLink ID="cmdEdit" runat="server" Text="Upravit nastavení klienta" NavigateUrl="javascript:record_edit()"></asp:HyperLink>
                                <div>
                                    <span class="infoInForm">Zahrnuje i možnost přesunutí do archivu nebo nenávratného odstranění.</span>
                                </div>

                            </asp:Panel>
                            <asp:Panel ID="panCreateCommands" runat="server">
                                <div class="div6">
                                    <img src="Images/new.png" />
                                    <asp:HyperLink ID="cmdNew" runat="server" Text="Založit klienta" NavigateUrl="javascript:record_new()"></asp:HyperLink>

                                </div>
                                <div class="div6">
                                    <img src="Images/copy.png" />
                                    <asp:HyperLink ID="cmdCopy" runat="server" Text="Založit klienta kopírováním" NavigateUrl="javascript:record_clone()"></asp:HyperLink>
                                    <div>
                                        <span class="infoInForm">Nový klient se kompletně předvyplní podle vzoru tohoto záznamu.</span>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panNewP41" runat="server" CssClass="div6">
                                <img src="Images/project.png" />
                                <asp:HyperLink ID="cmdNewP41" runat="server" Text="Založit pro klienta nový projekt" NavigateUrl="javascript:p41_new()"></asp:HyperLink>

                            </asp:Panel>

                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>



                <telerik:RadMenuItem Text="Další" ImageUrl="Images/more.png" Value="more">
                    <ContentTemplate>
                        <div style="float: left; min-width: 200px;">
                           
                            <asp:Panel ID="panCommandPivot" runat="server" CssClass="menu-group-item">
                                <img src="Images/pivot.png" />
                                <a href="p31_pivot.aspx?masterprefix=p28&masterpid=<%=Master.DataPID%>" target="_top">Worksheet Pivot za klienta projektů</a>
                            </asp:Panel>

                            
                            <asp:Panel ID="panP30" runat="server" CssClass="menu-group-item">
                                <img src="Images/person.png" />
                                <asp:HyperLink ID="cmdP30" runat="server" Text="Přiřadit kontaktní osobu" NavigateUrl="javascript:p30_record(0);" />
                            </asp:Panel>
                            <asp:Panel ID="panO23" runat="server" CssClass="menu-group-item">
                                <img src="Images/notepad.png" />
                                <asp:HyperLink ID="cmdO23" runat="server" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" />
                            </asp:Panel>
                            <asp:Panel ID="panO22" runat="server" CssClass="menu-group-item">
                                <img src="Images/calendar.png" />
                                <asp:HyperLink ID="cmdO22" runat="server" Text="Zapsat událost do kalendáře" NavigateUrl="javascript:o22_record(0);" />
                            </asp:Panel>
                            <div class="menu-group-item">
                                <img src="Images/comment.png" />
                                <asp:HyperLink ID="cmdB07" runat="server" Text="Zapsat komentář" NavigateUrl="javascript:b07_record();" />
                            </div>
                            <div class="menu-group-item">
                                <img src="Images/oplan.png" />
                                <asp:HyperLink ID="cmdP48" runat="server" Text="Operativní plán" NavigateUrl="javascript:p48_plan()"></asp:HyperLink>
                            </div>

                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="searchbox">
                    <ItemTemplate>

                        <input id="search2" style="width: 100px; margin-top: 7px;" value="Najít klienta..." onfocus="search2Focus()" onblur="search2Blur()" />

                    </ItemTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>

    </asp:Panel>


    <div style="height: 3px; page-break-after: always; width: 100%;"></div>
    <div class="div_radiolist_metro">
        <asp:HyperLink ID="topLink0" runat="server" Text="Úkony" CssClass="toplink" NavigateUrl="javascript:p31_grid()" Style="margin-left: 6px;"></asp:HyperLink>
        <asp:HyperLink ID="topLink5" runat="server" Text="Projekty" CssClass="toplink" NavigateUrl="javascript:projects()"></asp:HyperLink>
        <asp:HyperLink ID="topLink1" runat="server" Text="Schvalování/fakturační podklady/fakturace" CssClass="toplink" NavigateUrl="javascript:approve()"></asp:HyperLink>
        <asp:HyperLink ID="topLink4" runat="server" Text="Sestava" CssClass="toplink" NavigateUrl="javascript:report()"></asp:HyperLink>
        <asp:HyperLink ID="topLink2" runat="server" Text="Úkoly" CssClass="toplink" NavigateUrl="javascript:tasks()"></asp:HyperLink>
        <asp:HyperLink ID="topLink6" runat="server" Text="Faktury" CssClass="toplink" NavigateUrl="javascript:invoices()"></asp:HyperLink>
        <asp:HyperLink ID="topLink3" runat="server" Text="Kalendář klienta" CssClass="toplink" NavigateUrl="javascript:scheduler()"></asp:HyperLink>
        
    </div>


        <div class="content-box1">
            <div class="title">
                <img src="Images/properties.png" style="margin-right: 10px;" />
                <asp:Label ID="boxCoreTitle" Text="Záznam klienta" runat="server"></asp:Label>
                <asp:HyperLink ID="cmdNewWindow" runat="server" ImageUrl="Images/open_in_new_window.png" Target="_blank" ToolTip="Otevřít v nové záložce" CssClass="button-link" Style="float: right; vertical-align: top; padding: 0px;"></asp:HyperLink>
            </div>
            <div class="content">
                <table cellpadding="10" cellspacing="2" id="responsive">
                    <tr valign="top">
                        <td style="min-width: 120px;">
                            <asp:Label ID="lblContact" runat="server" Text="Název:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>

                            <asp:Label ID="Contact" runat="server" CssClass="valbold"></asp:Label>
                            <asp:Image ID="imgFlag_Contact" runat="server" />
                            <asp:Label ID="p29Name" runat="server" CssClass="val"></asp:Label>
                            <asp:Image ID="imgDraft" runat="server" ImageUrl="Images/draft_icon.gif" Visible="false" AlternateText="DRAFT záznam" Style="float: right;" />
                            <asp:Panel ID="panDraftCommands" runat="server" Visible="false">
                                <button type="button" onclick="draft2normal()">
                                    Převést z režimu DRAFT na oficiální záznam
                                </button>
                            </asp:Panel>

                        </td>


                    </tr>
                    <tr id="trWorkflow" runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblB02ID" runat="server" Text="Workflow stav:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="b02Name" runat="server" CssClass="valboldred"></asp:Label>
                            <img src="Images/workflow.png" />
                            <asp:HyperLink ID="cmdWorkflow" runat="server" Text="Posunout/doplnit" NavigateUrl="javascript: workflow()"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblX51" runat="server" Text="Fakturační ceník:" CssClass="lbl"></asp:Label>

                        </td>
                        <td>
                            <asp:Label ID="p51Name_Billing" runat="server" CssClass="valbold"></asp:Label>
                            <asp:HyperLink ID="clue_p51id_billing" runat="server" CssClass="reczoom" Text="i" title="Detail ceníku"></asp:HyperLink>

                        </td>

                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Vlastník klienta:"></asp:Label>

                        </td>
                        <td>
                            <asp:Label ID="Owner" runat="server" CssClass="valbold"></asp:Label>
                            <asp:Label ID="lblRegID" runat="server" CssClass="lbl" Text="IČ:" Style="padding-left: 10px;"></asp:Label>

                            <asp:Label ID="p28RegID" runat="server" CssClass="valbold"></asp:Label>
                            <asp:Label ID="Label2" runat="server" CssClass="lbl" Text="DIČ:"></asp:Label>

                            <asp:Label ID="p28VatID" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:HyperLink ID="cmdLog" runat="server" Text="Historie klienta" NavigateUrl="javascript: timeline()"></asp:HyperLink>
                </div>
            </div>

        </div>

        <asp:Panel ID="boxP31Summary" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/worksheet.png" style="margin-right: 10px;" />
                <asp:Label ID="boxP31SummaryTitle" runat="server" Text="Projektový worksheet"></asp:Label>
            </div>
            <div class="content">
                <uc:entity_worksheet_summary ID="p31summary1" runat="server"></uc:entity_worksheet_summary>
            </div>
        </asp:Panel>


        <asp:Panel ID="panRoles" runat="server" CssClass="content-box1">
            <div class="title">Obsazení klientských rolí</div>
            <div class="content">
                <uc:entityrole_assign_inline ID="roles1" runat="server" EntityX29ID="p28Contact" NoDataText=""></uc:entityrole_assign_inline>
            </div>
        </asp:Panel>










        <asp:Panel ID="boxO32" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/email.png" style="margin-right: 10px;" />
                <asp:Label ID="boxO32Title" runat="server" Text="Kontaktní média"></asp:Label>
            </div>
            <div class="content">
                <uc:p28_medium ID="medium1" runat="server"></uc:p28_medium>
            </div>
        </asp:Panel>


        <asp:Panel ID="boxO37" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/address.png" style="margin-right: 10px;" />
                <asp:Label ID="boxO37Title" runat="server" Text="Adresy"></asp:Label>
            </div>
            <div class="content">
                <uc:p28_address ID="address1" runat="server"></uc:p28_address>
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


        <asp:Panel ID="boxP30" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/person.png" style="margin-right: 10px;" />
                <asp:Label ID="boxP30Title" runat="server" Text="Kontaktní osoby klienta"></asp:Label>
                <asp:HyperLink ID="cmdEditP30" runat="server" NavigateUrl="javascript:p30_binding()" Text="Upravit" style="margin-left:20px;"></asp:HyperLink>
            </div>
            <div class="content">
                <uc:contactpersons ID="persons1" runat="server"></uc:contactpersons>
            </div>
        </asp:Panel>

        <asp:Panel ID="boxO23" runat="server" CssClass="content-box1" >
            <div class="title">
                <img src="Images/notepad.png" style="margin-right: 10px;" />
                <asp:Label ID="boxO23Title" runat="server" Text="Dokumenty"></asp:Label>
            </div>
            <div class="content" style="overflow: auto; max-height: 200px;">

                <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p28Contact"></uc:o23_list>


            </div>
        </asp:Panel>


        <asp:Panel ID="boxP41" runat="server" CssClass="content-box1" >
            <div class="title">
                <img src="Images/project.png" style="margin-right: 10px;" />
                <asp:Label ID="boxP41Title" runat="server" Text="Projekty"></asp:Label>
            </div>
            <asp:Panel ID="panProjects" runat="server" CssClass="content" Style="overflow: auto; max-height: 200px;">

                <asp:Repeater ID="rpP41" runat="server">
                    <ItemTemplate>
                        <div style="padding: 5px; float: left;">
                            <asp:HyperLink ID="clue_project" runat="server" CssClass="reczoom" Text="i" title="Detail projektu"></asp:HyperLink>

                            <asp:HyperLink ID="aProject" runat="server" Target="_parent"></asp:HyperLink>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>


            </asp:Panel>

        </asp:Panel>

   
    <div style="clear:both; width: 100%;"></div>
    <telerik:RadTabStrip ID="opgSubgrid" runat="server" Skin="Metro" Width="100%" AutoPostBack="true">
        <Tabs>
            <telerik:RadTab Text="Worksheet summary" Value="-1"></telerik:RadTab>
            <telerik:RadTab Text="Worksheet přehled" Value="1" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Úkoly" Value="4"></telerik:RadTab>
            <telerik:RadTab Text="Vystavené faktury" Value="2"></telerik:RadTab>
            <telerik:RadTab Text="Komentáře" Value="3"></telerik:RadTab>
            <telerik:RadTab Text="Žádný pod-přehled" Value="0"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    


    <uc:p31_bigsummary ID="bigsummary1" runat="server" MasterDataPrefix="p28" />

    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="p28Contact" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick" AllowMultiSelect="true"></uc:p31_subgrid>
    <uc:b07_list ID="comments1" runat="server" JS_Create="b07_record()" JS_Reaction="b07_reaction" />
    <uc:p56_subgrid ID="gridP56" runat="server" x29ID="p28Contact" />
    <uc:p91_subgrid ID="gridP91" runat="server" x29ID="p28Contact" />

    <asp:HiddenField ID="hiddatapid_subform" runat="server" />

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />
    <asp:HiddenField ID="hidDetailMode" runat="server" Value="detail" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />




    <script type="text/javascript">
        $(function () {

            $("#search2").autocomplete({
                source: "Handler/handler_search_contact.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {                        
                        window.open("p28_framework.aspx?pid=" + ui.item.PID,"_top");
                        return false;
                    }
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Project, item.FilterString);


                s = s + "</a>";

                if (item.Draft == "1")
                    s = s + "<img src='Images/draft.png' alt='DRAFT'/>"

                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });

        function __highlight(s, t) {
            var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
            return s.replace(matcher, "<strong>$1</strong>");
        }

        function search2Focus() {            
            document.getElementById("search2").value=""; 
            document.getElementById("search2").style.background = "yellow";
        }
        function search2Blur() {
           
            document.getElementById("search2").style.background = "";
            document.getElementById("search2").value = "Najít klienta...";
        }
    </script>
</asp:Content>

