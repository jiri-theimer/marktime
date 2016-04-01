<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p56_framework_detail.aspx.vb" Inherits="UI.p56_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="p31_subgrid" Src="~/p31_subgrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="imap_record" Src="~/imap_record.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Scripts/jqueryui/jquery-ui.min.css" />
    <script src="Scripts/jqueryui/jquery-ui.min.js" type="text/javascript"></script>

    <style type="text/css">
        html .RadMenu_Metro .rmRootGroup {
            background-image: none;
            
        }
 
        html .RadMenu_Metro ul.rmRootGroup {
            <%if me.hidisbin.value="1" then%>
            background-color: black;
            <%else%>
            background-color: white;
            <%End If%>
            
        }

        .rmLink {
            margin-top:6px;
           
        }


        .ui-autocomplete {
            width: 400px;
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
    </style>
    
    <script type="text/javascript">
        $(document).ready(function () {
           

        });

      

        function record_new() {
            
            sw_local("p56_record.aspx?pid=0&p41id=<%=Me.CurrentP41ID%>","Images/task_32.png",true);

        }

      
        function report() {
            
            sw_local("report_modal.aspx?prefix=p56&pid=<%=Master.DataPID%>","Images/reporting_32.png",true);

        }

        function record_edit() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_local("p56_record.aspx?pid=" + pid,"Images/task_32.png",true);

        }
        
        
        function record_clone() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_local("p56_record.aspx?clone=1&pid=" + pid,"Images/task_32.png",true);

        }

        function hardrefresh(pid, flag) {
            if (flag=="p56-save" || flag=="workflow-dialog"){                
                parent.window.location.replace("p56_framework.aspx?pid="+pid);
                return;
            }
            if (flag=="p56-delete"){
                parent.window.location.replace("p56_framework.aspx");
                return;
            }

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        function p31_entry() {
            ///volá se z p31_subgrid
            sw_local("p31_record.aspx?pid=0&p56id=<%=master.DataPID%>","Images/worksheet_32.png",true);
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
            sw_local("p31_record.aspx?pid=0&p56id=<%=Master.DataPID%>&p34id="+p34id,"Images/worksheet_32.png",true);
            

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
          
            sw_local("grid_designer.aspx?prefix=p31&masterprefix=p56&pid="+j74id, "Images/griddesigner_32.png",true);
        }
        function p56_subgrid_setting(j74id) {
            ///volá se z p56_subgrid
            sw_local("grid_designer.aspx?prefix=p56&masterprefix=p56&pid="+j74id, "Images/griddesigner_32.png",true);
        }
        

        function o23_record(pid) {
            
            sw_local("o23_record.aspx?masterprefix=p56&masterpid=<%=master.datapid%>&pid="+pid,"Images/notepad_32.png",true);

        }
        
        function b07_record() {
            
            sw_local("b07_create.aspx?masterprefix=p56&masterpid=<%=master.datapid%>","Images/comment_32.png",true);

        }
        function b07_reaction(b07id) {
            sw_local("b07_create.aspx?parentpid="+b07id+"&masterprefix=p56&masterpid=<%=master.datapid%>","Images/comment_32.png", true)
           
        }

        
        function approve(){            
            window.parent.sw_master("entity_modal_approving.aspx?prefix=p56&pid=<%=master.datapid%>","Images/approve_32.png",true);
        }

        function workflow(){            
            sw_local("workflow_dialog.aspx?prefix=p56&pid=<%=master.datapid%>","Images/workflow_32.png",false);
        }
        function p31_grid(){            
            window.open("p31_grid.aspx?masterprefix=p56&masterpid=<%=Master.DataPID%>","_top")
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 40px;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" Width="100%" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/task_32.png" alt="Úkol" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="250px"></telerik:RadMenuItem>
                <telerik:RadMenuItem Text="ZÁZNAM ÚKOLU" ImageUrl="Images/arrow_down_menu.png" Value="record">
                    <ContentTemplate>
                        <div style="padding: 10px; width: 450px;">

                            <asp:Panel ID="panEdit" runat="server" CssClass="div6">
                                <img src="Images/edit.png" />
                                <asp:HyperLink ID="cmdEdit" runat="server" Text="Upravit nastavení úkolu" NavigateUrl="javascript:record_edit()"></asp:HyperLink>
                                <div>
                                    <span class="infoInForm">Zahrnuje i možnost uzavření (přesunutí do archivu) nebo nenávratného odstranění.</span>
                                </div>

                            </asp:Panel>

                            <asp:Panel ID="panCreate" runat="server">
                                <img src="Images/new.png" />
                                <asp:HyperLink ID="cmdNew" runat="server" Text="Založit úkol" NavigateUrl="javascript:record_new()"></asp:HyperLink>

                            </asp:Panel>
                            <asp:Panel ID="panClone" runat="server" CssClass="div6">
                                <img src="Images/copy.png" />
                                <asp:HyperLink ID="cmdCopy" runat="server" Text="Založit úkol kopírováním" NavigateUrl="javascript:record_clone()"></asp:HyperLink>
                                <div>
                                    <span class="infoInForm">Nový úkol se kompletně předvyplní podle vzoru tohoto záznamu.</span>
                                </div>
                            </asp:Panel>



                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="ZAPSAT WORKSHEET" ImageUrl="Images/worksheet.png" Value="p31">
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


                <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/more.png" Value="more">
                    <ContentTemplate>
                        <div style="float: left; min-width: 200px;">
                            <div class="menu-group-title">Reporting</div>
                            <div class="menu-group-item">
                                <img src="Images/report.png" />
                                <asp:HyperLink ID="cmdReport" runat="server" Text="Tisková sestava" NavigateUrl="javascript:report()"></asp:HyperLink>
                            </div>
                            <asp:Panel ID="panCommandPivot" runat="server" CssClass="menu-group-item">
                                <img src="Images/pivot.png" />
                                <a href="p31_pivot.aspx?masterprefix=p56&masterpid=<%=Master.DataPID%>" target="_top">Worksheet Pivot úkolu</a>
                            </asp:Panel>

                            <div class="menu-group-title">Komunikace</div>

                            <asp:Panel ID="panO23" runat="server" CssClass="menu-group-item">
                                <img src="Images/notepad.png" />
                                <asp:HyperLink ID="cmdO23" runat="server" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" />
                            </asp:Panel>

                            <div class="menu-group-item">
                                <img src="Images/comment.png" />
                                <asp:HyperLink ID="cmdB07" runat="server" Text="Zapsat komentář" NavigateUrl="javascript:b07_record();" />
                            </div>


                        </div>
                    </ContentTemplate>

                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="searchbox">
                    <ItemTemplate>

                        <input id="search2" style="width: 100px; margin-top: 7px;" value="Najít úkol..." onfocus="search2Focus()" onblur="search2Blur()" />

                    </ItemTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>

    </asp:Panel>
    <div style="height: 3px; page-break-after: always"></div>
    <div class="div_radiolist_metro">
        <asp:HyperLink ID="topLink0" runat="server" Text="Úkony" CssClass="toplink" NavigateUrl="javascript:p31_grid()" Style="margin-left: 6px;"></asp:HyperLink>
        <asp:HyperLink ID="topLink1" runat="server" Text="Schvalování/fakturační podklady/fakturace" CssClass="toplink" NavigateUrl="javascript:approve()"></asp:HyperLink>
        <asp:HyperLink ID="topLink4" runat="server" Text="Sestava" CssClass="toplink" NavigateUrl="javascript:report()"></asp:HyperLink>

    </div>



    <div class="content-box1">
        <div class="title">
            <img src="Images/properties.png" style="margin-right: 10px;" />
            <asp:Label ID="boxCoreTitle" Text="Záznam úkolu" runat="server"></asp:Label>


            <asp:HyperLink ID="cmdNewWindow" runat="server" ImageUrl="Images/open_in_new_window.png" Target="_blank" ToolTip="Otevřít v nové záložce" CssClass="button-link" Style="float: right; vertical-align: top; padding: 0px;"></asp:HyperLink>

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
                <tr valign="top">
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Vlastník:"></asp:Label>

                    </td>
                    <td>
                        <asp:Label ID="Owner" runat="server" CssClass="valbold"></asp:Label>
                        <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>

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
            </table>
            <div class="div6">
                <uc:entityrole_assign_inline ID="roles_task" runat="server" EntityX29ID="p56Task" NoDataText=""></uc:entityrole_assign_inline>
            </div>
        </div>
    </div>




    <asp:Panel ID="boxVysledovka" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/worksheet.png" />
            Worksheet
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

    <div style="clear: both; width: 100%;"></div>

    <asp:Panel ID="panDescription" runat="server" CssClass="content-box1" style="width:99%;max-width:none;">
        <div class="title">Podrobný popis</div>
        <div class="content" style="background-color: #ffffcc;">
            <asp:Label ID="p56Description" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
        </div>
    </asp:Panel>

    <div style="clear: both; width: 100%;"></div>    
    <telerik:RadTabStrip ID="opgSubgrid" runat="server" Skin="Metro" Width="100%" AutoPostBack="false">
        <Tabs>
            
            <telerik:RadTab Text="Worksheet přehled" Value="1" Selected="true"></telerik:RadTab>
            
        </Tabs>
    </telerik:RadTabStrip>
    <uc:p31_subgrid ID="gridP31" runat="server" EntityX29ID="p56Task" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick" AllowMultiSelect="true"></uc:p31_subgrid>

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />
    <asp:HiddenField ID="hidCurP41ID" runat="server" />
    <asp:HiddenField ID="hidIsBin" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


    <script type="text/javascript">
        $(function () {

            $("#search2").autocomplete({
                source: "Handler/handler_search_task.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {                        
                        window.open("p56_framework.aspx?pid=" + ui.item.PID,"_top");
                        return false;
                    }
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Name+" | "+item.Project, item.FilterString);


                s = s + "</a>";



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
            document.getElementById("search2").value = "Najít úkol...";
        }
    </script>
</asp:Content>
