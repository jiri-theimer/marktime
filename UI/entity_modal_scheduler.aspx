<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="entity_modal_scheduler.aspx.vb" Inherits="UI.entity_modal_scheduler" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
        })

        function sw_local(url, img, is_maximize) {
            dialog_master(url, is_maximize);
        }


        function od(pid) {
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }


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

            document.getElementById("<%=hidCurTime.clientid %>").value = timeSlot.get_startTime().toISOString();



        }

        function record_new(sender, eventArgs) {
        }

        function o23_record(pid) {
            sw_local("o23_record.aspx?pid=" + pid, "Images/notepad_32.png", true);

        }
        function o22_record(pid) {

            sw_local("o22_record.aspx?pid=" + pid, "Images/calendar_32.png", true);

        }

        function hardrefresh(pid, flag) {


            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="slidingDiv1" style="padding: 10px;">
        <table cellpadding="10">

            <tr>
                <td>
                    <img src="Images/calendar.png" />
                </td>
             
                <td>
                    <asp:CheckBox ID="chkSetting_O22" runat="server" Checked="true" Text="Zobrazovat kalendářové události" AutoPostBack="true" CssClass="chk" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="Images/task.png" />
                </td>
               
                <td>
                    <asp:CheckBox ID="chkSetting_P56" runat="server" Checked="true" Text="Zobrazovat úkoly" AutoPostBack="true" CssClass="chk" />
                </td>
            </tr>

        </table>
    </div>



    <div id="offsetY"></div>
    <div id="divScheduler">
        <telerik:RadScheduler ID="scheduler1" SelectedView="AgendaView" EnableViewState="true" Skin="Default" AppointmentStyleMode="Simple" RowHeight="35px" Height="100%" ShowFooter="false" runat="server" ShowViewTabs="true" OnClientTimeSlotContextMenuItemClicked="record_new" EnableAdvancedForm="false"
            Culture="cs-CZ" AllowEdit="false" AllowDelete="false" AllowInsert="false" Localization-HeaderToday="Dnes" Localization-ShowMore="více..." OnClientAppointmentEditing="OnClientAppointmentEditing" OnClientTimeSlotContextMenu="clientTimeSlotClick"
            Localization-AllDay="celý den" Localization-HeaderMonth="Měsíc" Localization-HeaderDay="Den" Localization-HeaderWeek="Týden"
            HoursPanelTimeFormat="HH:mm"
            DataSubjectField="o22Name" DataStartField="o22DateFrom" DataEndField="o22DateUntil" DataKeyField="pid">
            <AgendaView UserSelectable="true" NumberOfDays="500" ShowColumnHeaders="false" />
            <TimelineView SlotDuration="10" ColumnHeaderDateFormat="dd.MM" NumberOfSlots="10" UserSelectable="true" TimeLabelSpan="2" />
            <MonthView UserSelectable="true" />
            <DayView UserSelectable="false" DayStartTime="08:00" DayEndTime="22:00" ShowInsertArea="true" />
            <AppointmentTemplate>
                <img src="Images/<%# Eval("Description")%>" style="padding-right: 5px;" /><span><%# Eval("Subject")%></span><a class="reczoom" rel="<%# Eval("ID")%>">i</a>



            </AppointmentTemplate>



        </telerik:RadScheduler>
    </div>
    <asp:HiddenField ID="hidX29ID" runat="server" Value="141" />
    <asp:HiddenField ID="hidPrefix" runat="server" Value="p41" />
    <asp:HiddenField ID="hidCurResource" runat="server" />
    <asp:HiddenField ID="hidCurTime" runat="server" />

    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
