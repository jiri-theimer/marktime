<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="j02_framework_detail.aspx.vb" Inherits="UI.j02_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="Scripts/jqueryui/jquery-ui.min.css" />
    <script src="Scripts/jqueryui/jquery-ui.min.js" type="text/javascript"></script>

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

        .ui-autocomplete {
            width: 250px;
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
            <%if Me.fraSubform.Visible then%>
            document.getElementById("<%=me.fraSubform.ClientID%>").style.height=hh+"px";
            <%end If%>
        }

        function sw_decide(url, iconUrl, is_maximize) {
            var w = parseInt(document.getElementById("<%=hidParentWidth.ClientID%>").value);
            var h = screen.availHeight;

            if ((w < 901 || h < 800) && w>0) {
                window.parent.sw_master(url, iconUrl);
                return;
            }                

            if (w < 910)
                is_maximize = true;
            
            sw_local(url, iconUrl, is_maximize);
        }

        function report() {
            
            sw_decide("report_modal.aspx?prefix=j02&pid=<%=Master.DataPID%>","Images/reporting_32.png",true);

        }
      

        function record_new() {
            
            sw_decide("j02_record.aspx?pid=0","Images/person_32.png",true);

        }

        function record_edit() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_decide("j02_record.aspx?pid=" + pid,"Images/person_32.png",true);

        }
        
        
        function record_clone() {
            var pid = <%=master.DataPID%>;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_decide("j02_record.aspx?clone=1&pid=" + pid,"Images/person_32.png",true);

        }

        function hardrefresh(pid, flag) {
            if (flag=="j02-save"){
                parent.window.location.replace("j02_framework.aspx?pid="+pid);
                return;
            }
            if (flag=="j02-delete"){
                parent.window.location.replace("j02_framework.aspx");
                return;
            }
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

       

        function o23_record(pid) {
            
            sw_decide("o23_record.aspx?masterprefix=j02&masterpid=<%=master.datapid%>&pid="+pid,"Images/notepad_32.png",true);

        }

        function j03_create() {            
            sw_decide("j03_create.aspx?j02id=<%=master.datapid%>","Images/user_32.png",true);
        }

        function j03_edit() {            
            sw_decide("j03_record.aspx?pid=<%=Me.CurrentJ03ID%>","Images/user_32.png",true);
        }

        function j05_record(j05id) {
            
            sw_decide("j05_record.aspx?pid="+j05id,"Images/masterslave_32.png",false);

        }

        function o22_record(pid) {
            
            sw_decide("o22_record.aspx?masterprefix=j02&masterpid=<%=master.datapid%>&pid="+pid,"Images/calendar_32.png",true);

        }

      
        function timeline(){            
            sw_decide("entity_timeline.aspx?prefix=j02&pid=<%=master.datapid%>","Images/timeline_32.png",true);
        }
       
        
        function approve(){            
            window.parent.sw_master("entity_modal_approving.aspx?prefix=j02&pid=<%=master.datapid%>","Images/approve_32.png",true);
        }
        function tasks(){            
            window.open("p56_framework.aspx?masterprefix=j02&masterpid=<%=Master.DataPID%>","_top")
        }
        function invoices(){            
            window.open("p91_framework.aspx?masterprefix=j02&masterpid=<%=Master.DataPID%>","_top")            
        }
        function p31_grid(){            
            window.open("p31_grid.aspx?masterprefix=j02&masterpid=<%=Master.DataPID%>","_top")
        }
        function scheduler(){            
            window.open("entity_scheduler.aspx?masterprefix=j02&masterpid=<%=Master.DataPID%>","_top")
        }
        function p48_plan(){            
            window.open("p48_framework.aspx?masterprefix=j02&masterpid=<%=master.datapid%>","_top");
        }
        function OnSwitch()
        {       
            var s="none";
            if (document.getElementById("<%=Me.panSwitch.ClientID%>").style.display=="none")
                s="block";
           
            document.getElementById("<%=Me.panSwitch.ClientID%>").style.display=s;

            $.post("Handler/handler_userparam.ashx", { x36value: s, x36key: "j02_framework_detail-switch", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }                
            });  
            AdjustHeight();
        }
        function OnClientTabSelected(sender, eventArgs)
        {
            var tab = eventArgs.get_tab();
            var s=tab.get_value();            
            $.post("Handler/handler_userparam.ashx", { x36value: s, x36key: "j02_framework_detail-subgrid", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }                
            });
            <%If Me.fraSubform.Visible = False Then%>
            location.replace("j02_framework_detail.aspx?tab="+s);           
            <%End If%>
            if (s=="0")
                location.replace("j02_framework_detail.aspx?tab="+s)
            
            
                
        }
        function page_setting(){
            sw_decide("entity_framework_detail_setting.aspx?prefix=j02", "Images/setting_32.png",false);
        }
        function stoploading(){            
            document.getElementById("<%=me.imgLoading.clientid%>").style.display="none";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height: 40px;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" Width="100%" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/person_32.png" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="300px"></telerik:RadMenuItem>
                <telerik:RadMenuItem Value="switch" NavigateUrl="javascript:OnSwitch()" Text="&darr;&uarr;" ToolTip="Skrýt/zobrazit horní polovinu detailu osoby (boxy)" />
                <telerik:RadMenuItem Text="ZÁZNAM OSOBY" ImageUrl="Images/arrow_down_menu.png">
                    <Items>
                        <telerik:RadMenuItem Value="cmdNew" Text="Založit novou osobu" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdEdit" Text="Upravit osobní profil" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png" ToolTip="Zahrnuje i možnost přesunutí do archivu nebo nenávratného odstranění."></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdCopy" Text="Založit novou osobu kopírováním" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" ToolTip="Nově zakládaná osoba se kompletně předvyplní nastavením z aktuálního profilu."></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>



                <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/more.png" Value="more">
                    <Items>
                        <telerik:RadMenuItem Value="switchHeight" Text="Nastavení vzhledu stránky" ImageUrl="Images/setting.png" NavigateUrl="javascript:page_setting()">
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdPivot" Text="Worksheet Pivot za osobu" Target="_top" ImageUrl="Images/pivot.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO23" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" ImageUrl="Images/notepad.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO22" Text="Zapsat událost do kalendáře" NavigateUrl="javascript:o22_record(0);" ImageUrl="Images/calendar.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP48" Text="Operativní plán" NavigateUrl="javascript:p48_plan();" ImageUrl="Images/oplan.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdX40" Text="Historie odeslané pošty" Target="_top" ImageUrl="Images/email.png"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="searchbox">
                    <ItemTemplate>

                        <input id="search2" style="width: 100px; margin-top: 7px;" value="Najít osobu..." onfocus="search2Focus()" onblur="search2Blur()" />

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
        <asp:HyperLink ID="topLink2" runat="server" Text="Úkoly" CssClass="toplink" NavigateUrl="javascript:tasks()"></asp:HyperLink>
        <asp:HyperLink ID="topLink6" runat="server" Text="Faktury" CssClass="toplink" NavigateUrl="javascript:invoices()"></asp:HyperLink>
        <asp:HyperLink ID="topLink3" runat="server" Text="Kalendář osoby" CssClass="toplink" NavigateUrl="javascript:scheduler()"></asp:HyperLink>
    </div>



    <asp:Panel ID="panSwitch" runat="server">
        <div class="content-box1">
            <div class="title">
                <img src="Images/properties.png" style="margin-right: 10px;" />Záznam osobního profilu
                        <asp:HyperLink ID="cmdNewWindow" runat="server" ImageUrl="Images/open_in_new_window.png" Target="_blank" ToolTip="Otevřít v nové záložce" CssClass="button-link" Style="float: right; vertical-align: top; padding: 0px;"></asp:HyperLink>
            </div>
            <div class="content">
                <table cellpadding="10" cellspacing="2" id="responsive">
                    <tr valign="top">
                        <td style="min-width: 120px;">
                            <asp:Label ID="lblPerson" runat="server" Text="Osoba:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>

                            <asp:Label ID="FullNameAsc" runat="server" CssClass="valbold"></asp:Label>

                            <div>
                                <asp:Label ID="j02Code" runat="server" CssClass="valbold" ForeColor="gray"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblJ07Name" runat="server" Text="Pozice:" CssClass="lbl"></asp:Label>
                            <div>
                                <asp:Label ID="lblJ18Name" runat="server" Text="Středisko:" CssClass="lbl"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblJ17Name" runat="server" Text="Stát:" CssClass="lbl"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="j07Name" runat="server" CssClass="valbold"></asp:Label>
                            <div>
                                <asp:Label ID="j18Name" runat="server" CssClass="valbold"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="j17Name" runat="server" CssClass="valbold"></asp:Label>
                            </div>
                        </td>

                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblEmail" runat="server" Text="E-mail:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:HyperLink ID="j02Email" runat="server"></asp:HyperLink>

                        </td>
                        <td>
                            <asp:Label ID="lblFond" runat="server" Text="Fond hodin:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="c21Name" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblTeams" runat="server" Text="Člen týmů:" CssClass="lbl"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="TeamsInLine" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                        
                    </tr>

                </table>

                <asp:Label ID="Mediums" runat="server" CssClass="valbold"></asp:Label>
                <div>
                    <asp:Label ID="Correspondence" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <asp:Panel ID="panIntraPerson" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/user.png" style="margin-right: 10px;" />Uživatelský účet
            </div>
            <div class="content">
                <asp:Panel ID="panAccount" runat="server">
                    <table cellpadding="10" cellspacing="2">
                        <tr valign="top">
                            <td style="min-width: 120px;">
                                <asp:Label ID="lblLogin" runat="server" Text="Přihlašovací jméno:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>

                                <asp:Label ID="j03Login" runat="server" CssClass="valbold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblJ04Name" runat="server" Text="Aplikační role:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="j04Name" runat="server" CssClass="valbold"></asp:Label>

                            </td>

                        </tr>


                    </table>
                </asp:Panel>
                <asp:Label ID="AccountMessage" runat="server" CssClass="infoInForm"></asp:Label>
                <asp:HyperLink ID="cmdLog" runat="server" Text="Historie aktivit" NavigateUrl="javascript: timeline()"></asp:HyperLink>
                <span style="padding-left: 40px;"></span>
                <asp:HyperLink ID="cmdAccount" runat="server" Text="Založit uživatelský účet" Visible="false"></asp:HyperLink>
            </div>
        </asp:Panel>
        <asp:Panel ID="boxJ05" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/masterslave.png" style="margin-right: 10px;" />
                <asp:Label ID="boxJ05Title" runat="server" Text="Nadřízenost | Podřízenost"></asp:Label>
                <asp:HyperLink ID="cmdAddJ05" runat="server" Text="Přidat" NavigateUrl="javascript:j05_record(0)"></asp:HyperLink>
            </div>
            <div class="content">

                <asp:Panel ID="panSlaves" runat="server" CssClass="div6">
                    <img src="Images/slave.png" /><span>Podřízení:</span>
                    <asp:Repeater ID="rpSlaves" runat="server">
                        <ItemTemplate>
                            <a href="javascript:j05_record(<%#Eval("pid")%>)"><%#Eval("PersonSlave")%><%#Eval("TeamSlave")%></a>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="SlavesInLine" runat="server"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="panMasters" runat="server" CssClass="div6">
                    <img src="Images/master.png" /><span>Nadřízení:</span>
                    <asp:Repeater ID="rpMasters" runat="server">
                        <ItemTemplate>
                            <a href="javascript:j05_record(<%#Eval("pid")%>)"><%#Eval("PersonMaster")%></a>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </div>

        </asp:Panel>
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


        <asp:Panel ID="panP30" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/contact.png" style="margin-right: 10px;" />Svázané firmy (klienti)
            </div>
            <div class="content">
                <asp:Repeater ID="rpP30" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <asp:HyperLink ID="Company" runat="server" Target="_top"></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>





        <asp:Panel ID="boxO23" runat="server" CssClass="content-box1">

            <div class="title">
                <img src="Images/notepad.png" style="margin-right: 10px;" />
                <asp:Label ID="boxO23Title" runat="server" Text="Dokumenty"></asp:Label>
            </div>
            <div class="content" style="overflow: auto; max-height: 200px;">
                <uc:o23_list ID="notepad1" runat="server" EntityX29ID="j02Person"></uc:o23_list>
            </div>

        </asp:Panel>

    </asp:Panel>
    <div style="clear: both; width: 100%;"></div>
    <telerik:RadTabStrip ID="opgSubgrid" runat="server" Skin="Metro" Width="100%" AutoPostBack="false" OnClientTabSelected="OnClientTabSelected">
        <Tabs>
            <telerik:RadTab Text="Worksheet summary" Value="-1" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Worksheet přehled" Value="1" Selected="true" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Úkoly" Value="4" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Vystavené faktury s úkony osoby" Value="2" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Komentáře" Value="3" Target="fraSubform"></telerik:RadTab>
            <telerik:RadTab Text="Žádný pod-přehled" Value="0" Target="fraSubform"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <div id="offsetY"></div>
    <iframe frameborder="0" id="fraSubform" name="fraSubform" runat="server" width="100%" height="300px"></iframe>
    <asp:Image ID="imgLoading" runat="server" ImageUrl="Images/loading.gif" Style="position: absolute; top: 500px; left: 200px;" />


    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidIsBin" runat="server" />
    <asp:HiddenField ID="hidParentWidth" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


    <script type="text/javascript">
        $(function () {

            $("#search2").autocomplete({
                source: "Handler/handler_search_person.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {                        
                        window.open("j02_framework.aspx?pid=" + ui.item.PID,"_top");
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
            document.getElementById("search2").value = "Najít osobu...";
        }
    </script>
</asp:Content>

