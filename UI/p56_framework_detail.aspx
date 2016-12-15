﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p56_framework_detail.aspx.vb" Inherits="UI.p56_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign_inline" Src="~/entityrole_assign_inline.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="imap_record" Src="~/imap_record.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="searchbox" Src="~/searchbox.ascx" %>
<%@ Register TagPrefix="uc" TagName="alertbox" Src="~/alertbox.ascx" %>

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
      

        function record_new() {
            
            sw_decide("p56_record.aspx?pid=0&p41id=<%=Me.CurrentP41ID%>","Images/task_32.png",true);

        }

      
        function report() {
            
            sw_decide("report_modal.aspx?prefix=p56&pid=<%=Master.DataPID%>","Images/reporting_32.png",true);

        }

        function record_edit() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_decide("p56_record.aspx?pid=" + pid,"Images/task_32.png",true);

        }
        
        
        function record_clone() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_decide("p56_record.aspx?clone=1&pid=" + pid,"Images/task_32.png",true);

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

      

        function o23_record(pid) {
            
            sw_decide("o23_record.aspx?masterprefix=p56&masterpid=<%=master.datapid%>&pid="+pid,"Images/notepad_32.png",true);

        }
        
        function b07_record() {
            
            sw_decide("b07_create.aspx?masterprefix=p56&masterpid=<%=master.datapid%>","Images/comment_32.png",true);

        }
        function b07_reaction(b07id) {
            sw_decide("b07_create.aspx?parentpid="+b07id+"&masterprefix=p56&masterpid=<%=master.datapid%>","Images/comment_32.png", true)
           
        }

        
        function approve(){   
            var isInIFrame = (window.location != window.parent.location);
            if (isInIFrame==true){
                window.parent.sw_master("entity_modal_approving.aspx?prefix=p56&pid=<%=master.datapid%>","Images/approve_32.png",true);
            }
            else{
                sw_decide("entity_modal_approving.aspx?prefix=p56&pid=<%=master.datapid%>","Images/approve_32.png",true);
            }
        }

        function workflow(){            
            sw_decide("workflow_dialog.aspx?prefix=p56&pid=<%=master.datapid%>","Images/workflow_32.png",false);
        }
        function p31_grid(){            
            window.open("p31_grid.aspx?masterprefix=p56&masterpid=<%=Master.DataPID%>","_top")
        }
        function p31_subgrid_approving(pids) {
            window.parent.sw_master("p31_approving_step2.aspx?pids=" + pids, "Images/approve_32.png", true);
        }

       
        function p31_entry_menu(p34id) {
            ///z menu1           
            sw_decide("p31_record.aspx?pid=0&p56id=<%=Master.DataPID%>&p34id="+p34id,"Images/worksheet.png",true);
            

        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 44px;border-bottom:solid 1px gray;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Default" Width="100%" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/task_32.png" alt="Úkol" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="250px"></telerik:RadMenuItem>
                <telerik:RadMenuItem Value="saw" text="<img src='Images/open_in_new_window.png'/>" Target="_blank" NavigateUrl="p56_framework_detail.aspx?saw=1" ToolTip="Otevřít úkol v nové záložce prohlížeče"></telerik:RadMenuItem>          
                <telerik:RadMenuItem Text="ZÁZNAM ÚKOLU" ImageUrl="Images/arrow_down_menu.png" Value="record">
                    <Items>
                        <telerik:RadMenuItem Value="cmdEdit" Text="Upravit kartu úkolu" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png" ToolTip="Zahrnuje i možnost uzavření (přesunutí do archivu) nebo nenávratného odstranění."></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdNew" Text="Založit úkol" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png"></telerik:RadMenuItem>

                        <telerik:RadMenuItem Value="cmdCopy" Text="Založit úkol kopírováním" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" ToolTip="Nový úkol se kompletně předvyplní podle vzoru tohoto záznamu."></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdApprove" Text="Schvalovat nebo fakturovat úkol" NavigateUrl="javascript:approve()" ImageUrl="Images/approve.png"></telerik:RadMenuItem>
                        
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="ZAPSAT WORKSHEET" ImageUrl="Images/worksheet.png" Value="p31"></telerik:RadMenuItem>
                <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/menuarrow.png" Value="more">
                    <Items>
                        <telerik:RadMenuItem Value="cmdPivot" Text="Worksheet Pivot za úkol" Target="_top" ImageUrl="Images/pivot.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO23" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" ImageUrl="Images/notepad.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdB07" Text="Zapsat komentář" NavigateUrl="javascript:b07_record();" ImageUrl="Images/comment.png"></telerik:RadMenuItem>

                    </Items>


                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="searchbox">
                    <ItemTemplate>

                        <input id="search2" style="width: 100px; margin-top: 7px;" value="Najít úkol..." onfocus="search2Focus()" onblur="search2Blur()" />
                        <div id="search2_result" style="position: relative;left:-150px;"></div>
                    </ItemTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>

    </asp:Panel>
    <div style="clear:both;"></div>
    <p></p>

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
                        <br />
                        <button type="button" onclick="workflow()" class="cmd" title="Změnit stav úkolu, zapsat komentář, případně další kroky, které podporuje aktuální workflow šablona..."><img src="Images/workflow.png" /> Posunout/doplnit</button>
                        
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
    <uc:alertbox id="alert1" runat="server"></uc:alertbox>

    <div style="clear: both; width: 100%;"></div>

    <asp:Panel ID="panDescription" runat="server" CssClass="content-box1" Style="width: 99%; max-width: none;">
        <div class="title">Podrobný popis</div>
        <div class="content" style="background-color: #ffffcc;max-height:120px;overflow:auto;">
            <asp:Label ID="p56Description" runat="server" CssClass="val" Style="font-family: 'Courier New'; word-wrap: break-word; display: block; font-size: 120%;"></asp:Label>
        </div>
    </asp:Panel>

    <div style="clear: both; width: 100%;"></div>
    <telerik:RadTabStrip ID="tabs1" runat="server" Skin="Default" Width="100%" ShowBaseLine="true">
        <Tabs>

            <telerik:RadTab Text="Worksheet přehled" Value="p31" Selected="true" Target="fraSubform"></telerik:RadTab>            
            <telerik:RadTab Text="Historie" Value="b07" Target="fraSubform"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

   <div id="offsetY"></div>
    <iframe frameborder="0" id="fraSubform" name="fraSubform" runat="server" width="100%" height="300px"></iframe>
    

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hiddatapid_p31" runat="server" />
    <asp:HiddenField ID="hidCurP41ID" runat="server" />
    
    <asp:HiddenField ID="hidParentWidth" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
    <uc:searchbox id="sb1" runat="server"></uc:searchbox>

    
    
</asp:Content>
