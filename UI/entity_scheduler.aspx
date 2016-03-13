<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="entity_scheduler.aspx.vb" Inherits="UI.entity_scheduler" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        @media screen and (max-width: 900px) {

            #left_panel {
                width: 0px !important;
            }

            #right_panel {
                margin-left: 0px !important;
            }

            #left_panel {
                display: none !important;
            }
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {


            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });


            var h1 = new Number;
            var h2 = new Number;
            var hh = new Number;

            h1 = $(window).height();

            ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            <%If LCase(Request.Browser.Browser) = "ie" Then%>
            hh = h1 - h2 - 4;
            <%Else%>
            hh = h1 - h2 - 2;
            <%End If%>

            self.document.getElementById("divScheduler").style.height = hh + "px";

        });





        function hardrefresh(pid, flag) {

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefreshOnBehind, "", False)%>

        }

        function o22_record(pid) {

            sw_master("o22_record.aspx?pid=" + pid, "Images/milestone_32.png")
        }

        function o22_clone(pid) {

            sw_master("o22_record.aspx?clone=1&pid=" + pid, "Images/milestone_32.png")
        }



        function record_create(sender, eventArgs) {
            var firstSlot = sender.get_selectedSlots()[0];

            var lastSlot = sender.get_selectedSlots()[sender.get_selectedSlots().length - 1];
            var d1 = firstSlot.get_startTime()
            var d2 = lastSlot.get_endTime();
            var j02id = "<%=Master.Factory.SysUser.j02ID%>";
            
            <%If cbxNewRecType.SelectedValue="p48" then%>
            var url = "p48_multiple_create.aspx?t1=" + formattedDate(d1) + "&t2=" + formattedDate(d2) + "&j02id=" + j02id;            
            <%end if%>
            <%If cbxNewRecType.SelectedValue = "o22" Then%>
            var url = "o22_record.aspx?t1=" + formattedDate(d1) + "&t2=" + formattedDate(d2) + "&j02id=" + j02id;
            <%end if%>

            sw_master(url, "Images/milestone_32.png")

        }

        function OnClientAppointmentEditing(sender, eventArgs) {
            eventArgs.set_cancel(true);
        }

        function clientTimeSlotClick(sender, eventArgs) {

            document.getElementById("<%=hidCurResource.clientid %>").value = "";
            var resource = null;
            var timeSlot = eventArgs.get_targetSlot();



            if (timeSlot.get_resource) {
                resource = timeSlot.get_resource();
                document.getElementById("<%=hidCurResource.clientid %>").value = resource.get_key();
            }

            //var start = timeSlot.get_startTime().toISOString();
            var start = timeSlot.get_startTime();
            var end = timeSlot.get_endTime();
            //var end = timeSlot.get_endTime().toISOString();
            document.getElementById("<%=hidCurTime.clientid %>").value = start;
            //alert(start+" **** "+end);
        }

        function OnClientAppointmentMoveEnd(sender, eventArgs) {
            alert("OnClientAppointmentMoveEnd");

        }

        function formattedDate(date) {
            var d = date;

            var month = '' + (d.getMonth() + 1);
            var day = '' + d.getDate();
            var year = d.getFullYear();
            var hour = d.getHours();
            var minute = d.getMinutes();




            return (day + "." + month + "." + year + "_" + "0" + hour + "." + "0" + minute);
        }





        function isPartOfSchedulerAppointmentArea(htmlElement) {
            // Determines if an HTML element is part of the scheduler appointment area.
            // This can be either the rsContent or the rsAllDay div (in day and week view).
            return $telerik.$(htmlElement).parents().is("div.rsAllDay") ||
                        $telerik.$(htmlElement).parents().is("div.rsContent")
        }

        window.nodeDropping = function (sender, eventArgs) {
            var htmlElement = eventArgs.get_htmlElement();

            var scheduler = $find('<%=scheduler1.ClientID%>');

            if (isPartOfSchedulerAppointmentArea(htmlElement)) {
                //Gets the TimeSlot where an Appointment is dropped. 
                var timeSlot = scheduler.get_activeModel().getTimeSlotFromDomElement(htmlElement);
                var d1 = timeSlot.get_startTime();
                var d2 = new Date(d1);
                <%If Me.CurrentView <> SchedulerViewType.MonthView Then%>
                d2.setHours(d1.getHours() + 1);
                <%End If%>

                //Gets all the data needed for the an Appointment, from the TreeView node.
                var node = eventArgs.get_sourceNode();
                <%If cbxNewRecType.SelectedValue="p48" then%>
                var url = "p48_multiple_create.aspx?d1=" + formattedDate(d1) + "&d2=" + formattedDate(d2);
                alert(url);
                <%end if%>
                <%If cbxNewRecType.SelectedValue = "o22" Then%>
                var url = "o22_record.aspx?d1=" + formattedDate(d1) + "&d2=" + formattedDate(d2);
                <%end if%>

                sw_master(url, "Images/oplan_32.png");

            }
            else {
                //The node was dropped elsewhere on the document.
                eventArgs.set_cancel(true);
            }
        }

        function OnSchedulerCommand(sender, args) {
            var loadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID %>");
            loadingPanel.show(sender.get_id());

        }

        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="offsetY"></div>
    <div id="divScheduler">

        <div id="left_panel" style="float: left; width: 250px;">
            <div style="float: left;">
                <img src="Images/calendar_32.png" />
            </div>
            <div class="div6" style="float: left;">

                <span class="page_header_span">Kalendář</span>
            </div>
            <div class="show_hide1" style="float: left; margin-top: 8px;">
                <button type="button">
                    <img src="Images/arrow_down.gif" alt="Nastavení" />
                    Nastavení

                </button>
            </div>
            <div style="clear: both;"></div>

            <asp:Panel ID="panMasterRecord" runat="server" CssClass="div6">
                <asp:Image ID="imgMaster" runat="server" />
                <asp:HyperLink ID="MasterRecord" runat="server" Target="_top"></asp:HyperLink>
            </asp:Panel>
            <div class="div6">
                <asp:Label ID="Persons" runat="server" CssClass="valbold"></asp:Label>
            </div>



            <div class="slidingDiv1">
                <asp:Panel ID="panPersonScope" runat="server">
                    <div style="text-align: center;">
                        <b>Osoby v kalendáři</b>

                        <asp:LinkButton ID="cmdAppendJ02IDs" runat="server" CssClass="cmd" Text="Přidat" />
                        <asp:LinkButton ID="cmdReplaceJ02IDs" runat="server" CssClass="cmd" Text="Nahradit" />
                    </div>
                    <div>
                        Vybrat osobu (jednotlivce):
                    </div>
                    <div>
                        <uc:person ID="j02ID_Add" runat="server" Width="100%" />
                    </div>
                    <div>
                        Vybrat tým:
                    
                    </div>
                    <div>
                        <uc:datacombo ID="j11ID_Add" runat="server" DataTextField="j11Name" DataValueField="pid" IsFirstEmptyRow="true" Width="100%"></uc:datacombo>
                    </div>
                    <div>
                        Vybrat pozici:
                    </div>
                    <div>
                        <uc:datacombo ID="j07ID_Add" runat="server" DataTextField="j07Name" DataValueField="pid" IsFirstEmptyRow="true" Width="100%"></uc:datacombo>
                    </div>

                </asp:Panel>
                <div>
                        Na click v kalendáři založit:
                    
                    </div>
                <div class="div6">
                    <asp:DropDownList ID="cbxNewRecType" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Operativní plán" Value="p48"></asp:ListItem>
                        <asp:ListItem Text="Kalendářová událost" Value="o22"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="div6">
                    <img src="Images/oplan.png" />
                    <asp:CheckBox ID="chkSetting_P48" runat="server" Checked="true" Text="Zobrazovat operatavní plán" AutoPostBack="true" CssClass="chk" />
                </div>
                <div class="div6">
                    <img src="Images/calendar.png" />
                    <asp:CheckBox ID="chkSetting_O22" runat="server" Checked="true" Text="Zobrazovat kalendářové události" AutoPostBack="true" CssClass="chk" />
                </div>
                <div class="div6">
                    <img src="Images/task.png" />
                    <asp:CheckBox ID="chkSetting_P56" runat="server" Checked="false" Text="Zobrazovat úkoly" AutoPostBack="true" CssClass="chk" />
                </div>

                <div class="div6">
                    <span>Čas v kalendáři od:</span>
                    <asp:DropDownList ID="entity_scheduler_daystarttime" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="05:00" Value="5"></asp:ListItem>
                        <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                        <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                        <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                        <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                        <asp:ListItem Text="12:00" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="div6">
                    <span>Čas v kalendáři do:</span>
                    <asp:DropDownList ID="entity_scheduler_dayendtime" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="15:00" Value="15"></asp:ListItem>
                        <asp:ListItem Text="16:00" Value="16"></asp:ListItem>
                        <asp:ListItem Text="17:00" Value="17"></asp:ListItem>
                        <asp:ListItem Text="18:00" Value="18"></asp:ListItem>
                        <asp:ListItem Text="19:00" Value="19"></asp:ListItem>
                        <asp:ListItem Text="20:00" Value="20"></asp:ListItem>
                        <asp:ListItem Text="21:00" Value="21"></asp:ListItem>
                        <asp:ListItem Text="22:00" Value="22"></asp:ListItem>
                        <asp:ListItem Text="23:00" Value="23"></asp:ListItem>

                    </asp:DropDownList>
                </div>
                <div class="div6">
                    <span>Počet dní v [Multi-den]:</span>
                    <asp:DropDownList ID="entity_scheduler_multidays" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>

                    </asp:DropDownList>
                </div>


                <div class="div6">
                    <asp:Button ID="cmdExportICalendar" runat="server" CssClass="cmd" Text="Export do ICalendar" />
                </div>
                <div class="div6">
                    <img src="Images/help.png" /><i>Zápis do kalendáře provedete přes pravé tlačítko myši nad označenými buňkami nebo přes click do kalendáře.</i>
                </div>
            </div>



        </div>
        <div id="right_panel" style="margin-left: 250px;">
            <telerik:RadScheduler ID="scheduler1" SelectedView="WeekView" RenderMode="Lightweight" FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" Width="100%" Height="90%" EnableViewState="false" Skin="Default" AppointmentStyleMode="Simple" ShowFooter="false" runat="server" ShowViewTabs="true" EnableAdvancedForm="false"
                Culture="cs-CZ" AllowEdit="false" AllowDelete="false" AllowInsert="false" Localization-HeaderToday="Dnes" Localization-ShowMore="více..."
                OnClientAppointmentEditing="OnClientAppointmentEditing" OnClientTimeSlotClick="record_create" OnClientTimeSlotContextMenuItemClicked="record_create"
                Localization-AllDay="Celý den" Localization-HeaderMonth="Měsíc" Localization-HeaderDay="Den" Localization-HeaderWeek="Týden" Localization-HeaderMultiDay="Multi-den"
                HoursPanelTimeFormat="HH:mm" ShowNavigationPane="true" OnClientAppointmentMoveEnd="OnClientAppointmentMoveEnd" OnClientNavigationCommand="OnSchedulerCommand"
                DataSubjectField="o22Name" DataStartField="o22DateFrom" DataEndField="o22DateUntil" DataKeyField="pid">

                <DayView UserSelectable="true" DayStartTime="08:00" DayEndTime="22:00" ShowInsertArea="true" />
                <WeekView UserSelectable="true" DayStartTime="08:00" DayEndTime="22:00" ShowInsertArea="true" />
                <MultiDayView UserSelectable="true" DayStartTime="08:00" DayEndTime="22:00" NumberOfDays="10" />
                <TimelineView UserSelectable="true" />
                <AgendaView UserSelectable="true" />
                <MonthView UserSelectable="true" VisibleAppointmentsPerDay="4" />
                <TimelineView UserSelectable="false" />
                <AppointmentTemplate>
                    <a class="reczoom" rel="<%# Eval("Description")%>">i</a>
                    <a href="javascript:re(<%# Eval("ID")%>)"><%# Eval("Subject")%></a>


                </AppointmentTemplate>

                <TimeSlotContextMenus>
                    <telerik:RadSchedulerContextMenu>
                        <Items>
                            <telerik:RadMenuItem Text="Operativní plán" ImageUrl="Images/oplan.png"></telerik:RadMenuItem>                            
                            <telerik:RadMenuItem Text="Kalendářová událost" ImageUrl="Images/milestone.png"></telerik:RadMenuItem>                            
                            <telerik:RadMenuItem Text="Úkol" ImageUrl="Images/task.png"></telerik:RadMenuItem>
                            <telerik:RadMenuItem IsSeparator="true" Text="."></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="Jdi na DNES" Value="CommandGoToToday" />
                        </Items>
                    </telerik:RadSchedulerContextMenu>
                </TimeSlotContextMenus>
                <ExportSettings OpenInNewWindow="true" FileName="SchedulerExport">
                    <Pdf PageTitle="Schedule" Author="Telerik" Creator="Telerik" Title="Schedule" />
                </ExportSettings>
            </telerik:RadScheduler>
            <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" RenderMode="Lightweight" Transparency="30" BackColor="#E0E0E0">
                <div style="float: none; padding-top: 80px;">
                    <img src="Images/loading.gif" />
                    <h2>LOADING...</h2>
                </div>
            </telerik:RadAjaxLoadingPanel>
        </div>

    </div>


    <asp:Button ID="cmdHardRefreshOnBehind" runat="server" Style="display: none;" />
    <asp:HiddenField ID="hidJ02IDs_All" runat="server" />
    <asp:HiddenField ID="hidJ02IDs" runat="server" />
    <asp:HiddenField ID="hidJ07IDs" runat="server" />
    <asp:HiddenField ID="hidJ11IDs" runat="server" />

    <asp:HiddenField ID="hidCurResource" runat="server" />
    <asp:HiddenField ID="hidCurTime" runat="server" />

    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />


    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />

</asp:Content>

