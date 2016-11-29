<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="j02_framework_detail.aspx.vb" Inherits="UI.j02_framework_detail" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>
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
            <%if Me.fraSubform.Visible then%>
            document.getElementById("<%=me.fraSubform.ClientID%>").style.height=hh+"px";
            <%end If%>
        }

        function sw_decide(url, iconUrl, is_maximize) {
            <%If Master.MasterPageFile="Site" then%>
            var w = parseInt(document.getElementById("<%=hidParentWidth.ClientID%>").value);
            var h = screen.availHeight;

            if ((w < 901 || h < 800) && w>0) {
                window.parent.sw_master(url, iconUrl);
                return;
            }                

            if (w < 910)
                is_maximize = true;
            <%end If%>
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
            var isInIFrame = (window.location != window.parent.location);
            if (isInIFrame == true) {
                window.parent.sw_master("entity_modal_approving.aspx?prefix=j02&pid=<%=master.datapid%>","Images/approve_32.png",true);
            }
            else
            {
                sw_decide("entity_modal_approving.aspx?prefix=j02&pid=<%=master.datapid%>","Images/approve_32.png",true);
            }
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
        
        function page_setting(){
            sw_decide("entity_framework_detail_setting.aspx?prefix=j02", "Images/setting_32.png",false);
        }
        function stoploading(){            
            document.getElementById("<%=me.imgLoading.clientid%>").style.display="none";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMenuContainer" runat="server" Style="height:44px;border-bottom:solid 1px gray;">

        <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Default" Width="100%" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
            <Items>
                <telerik:RadMenuItem Value="begin">
                    <ItemTemplate>
                        <img src="Images/person_32.png" />
                    </ItemTemplate>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="level1" NavigateUrl="#" Width="300px"></telerik:RadMenuItem>
                <telerik:RadMenuItem Value="saw" text="<img src='Images/open_in_new_window.png'/>" Target="_blank" NavigateUrl="j02_framework_detail.aspx?saw=1" ToolTip="Otevřít detail osoby v nové záložce prohlížeče"></telerik:RadMenuItem>
                <telerik:RadMenuItem Value="switch" NavigateUrl="javascript:OnSwitch()" Text="&darr;&uarr;" ToolTip="Skrýt/zobrazit horní polovinu detailu osoby (boxy)" />
                <telerik:RadMenuItem Text="ZÁZNAM OSOBY" ImageUrl="Images/arrow_down_menu.png">
                    <Items>
                        <telerik:RadMenuItem Value="cmdNew" Text="Založit novou osobu" NavigateUrl="javascript:record_new();" ImageUrl="Images/new.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdEdit" Text="Upravit osobní profil" NavigateUrl="javascript:record_edit();" ImageUrl="Images/edit.png" ToolTip="Zahrnuje i možnost přesunutí do archivu nebo nenávratného odstranění."></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdCopy" Text="Založit novou osobu kopírováním" NavigateUrl="javascript:record_clone();" ImageUrl="Images/copy.png" ToolTip="Nově zakládaná osoba se kompletně předvyplní nastavením z aktuálního profilu."></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdApprove" Text="Schvalovat nebo fakturovat práci osoby" NavigateUrl="javascript:approve()" ImageUrl="Images/approve.png"></telerik:RadMenuItem>
                        
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdReport" Text="Tisková sestava" NavigateUrl="javascript:report();" ImageUrl="Images/report.png"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>



                <telerik:RadMenuItem Text="DALŠÍ" ImageUrl="Images/menuarrow.png" Value="more">
                    <Items>
                        <telerik:RadMenuItem Value="switchHeight" Text="Nastavení vzhledu stránky" ImageUrl="Images/setting.png" NavigateUrl="javascript:page_setting()">
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdPivot" Text="Worksheet PIVOT za osobu" Target="_top" ImageUrl="Images/pivot.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO23" Text="Vytvořit dokument" NavigateUrl="javascript:o23_record(0);" ImageUrl="Images/notepad.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdO22" Text="Zapsat událost do kalendáře" NavigateUrl="javascript:o22_record(0);" ImageUrl="Images/calendar.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdCalendar" Text="Kalendář osoby" NavigateUrl="javascript:scheduler()" ImageUrl="Images/calendar.png"></telerik:RadMenuItem>
                         <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdP48" Text="Operativní plán osoby" NavigateUrl="javascript:p48_plan();" ImageUrl="Images/oplan.png"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Value="cmdX40" Text="Historie odeslané pošty" Target="_top" ImageUrl="Images/email.png"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Value="searchbox">
                    <ItemTemplate>

                        <input id="search2" style="width: 100px; margin-top: 7px;" value="Najít osobu..." onfocus="search2Focus()" onblur="search2Blur()" />
                        <div id="search2_result" style="position: relative;left:-150px;"></div>
                    </ItemTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>

    </asp:Panel>


    <div style="clear:both;"></div>
  <p></p>

    <asp:Panel ID="panSwitch" runat="server">
        <div class="content-box1">
            <div class="title">
                <img src="Images/properties.png" style="margin-right: 10px;" />Záznam osobního profilu
                        
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
                <img src="Images/contact.png" />
                <img src="Images/project.png" style="margin-right: 10px;" />
                Kontaktní osoba pro klienty/projekty
            </div>
            <div class="content">
                <asp:Repeater ID="rpP30" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <asp:HyperLink ID="Company" runat="server" Target="_top"></asp:HyperLink>
                            <asp:Label ID="p27Name" runat="server"></asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>





     
    </asp:Panel>
    <div style="clear: both;"></div>
    <telerik:RadTabStrip ID="tabs1" runat="server" Skin="Default" Width="100%" AutoPostBack="false" ShowBaseLine="true" style="margin-top:10px;">       
    </telerik:RadTabStrip>
    
    <div id="offsetY"></div>
    <iframe frameborder="0" id="fraSubform" name="fraSubform" runat="server" width="100%" height="300px"></iframe>
    <asp:Image ID="imgLoading" runat="server" ImageUrl="Images/loading.gif" Style="position: absolute; top: 500px; left: 200px;" />


    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    
    <asp:HiddenField ID="hidParentWidth" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />


    <script type="text/javascript">
        <%if menu1.FindItemByValue("searchbox").visible then%>
        $(function () {

            $("#search2").autocomplete({
                source: "Handler/handler_search_person.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {                        
                        window.open("j02_framework.aspx?pid=" + ui.item.PID,"_top");
                        return false;
                    }
                },
                open: function (event, ui) {
                    $('ul.ui-autocomplete')
                       .removeAttr('style').hide()
                       .appendTo('#search2_result').show();
                },
                close: function (event, ui) {
                    $('ul.ui-autocomplete')
                    .hide();                   
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
        <%end if%>
    </script>
</asp:Content>

