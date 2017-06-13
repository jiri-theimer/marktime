<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="myscheduler.ascx.vb" Inherits="UI.myscheduler" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadScheduler ID="scheduler1" SelectedView="AgendaView" RenderMode="Lightweight" FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" Width="600px" Height="300px" EnableViewState="false" Skin="Default" AppointmentStyleMode="Simple" ShowFooter="false" runat="server" ShowViewTabs="false" EnableAdvancedForm="false" ShowHeader="true" ShowAllDayRow="true"
    Culture="cs-CZ" AllowEdit="false" AllowDelete="false" AllowInsert="false"
    HoursPanelTimeFormat="HH:mm" ShowNavigationPane="true"
    DataSubjectField="o22Name" DataStartField="o22DateFrom" DataEndField="o22DateUntil" DataKeyField="pid">
    <Localization HeaderAgendaDate="Datum" AllDay="Bez času od/do" HeaderMonth="Měsíc" HeaderDay="Den" HeaderMultiDay="Multi-den" HeaderWeek="Týden" ShowMore="více..." HeaderToday="Dnes" HeaderAgendaAppointment="Úkol nebo Událost" HeaderAgendaTime="Čas" />

    <AgendaView UserSelectable="false" NumberOfDays="100" ShowDateHeaders="true" ReadOnly="true" ShowColumnHeaders="false" />
    <DayView UserSelectable="true" DayStartTime="06:00" DayEndTime="22:00" ShowInsertArea="false" />

    <AppointmentTemplate>
        <a class="reczoom" rel="<%# Eval("Description")%>">i</a>
        <a href="javascript:re(<%# Eval("ID")%>)"><%# Eval("Subject")%></a>


    </AppointmentTemplate>


</telerik:RadScheduler>
<asp:HiddenField ID="hidPrefix" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />
