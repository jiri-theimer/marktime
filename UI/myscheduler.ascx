<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="myscheduler.ascx.vb" Inherits="UI.myscheduler" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadScheduler ID="scheduler1" SelectedView="AgendaView" RenderMode="Lightweight" FirstDayOfWeek="Monday" LastDayOfWeek="Sunday" Width="600px" Height="300px" EnableViewState="false" Skin="Default" AppointmentStyleMode="Simple" ShowFooter="false" runat="server" ShowViewTabs="false" EnableAdvancedForm="false" ShowHeader="true" ShowAllDayRow="true"
    Culture="cs-CZ" AllowEdit="false" AllowDelete="false" AllowInsert="false"
    HoursPanelTimeFormat="HH:mm" ShowNavigationPane="true"
    DataSubjectField="o22Name" DataStartField="o22DateFrom" DataEndField="o22DateUntil" DataKeyField="pid">
    <Localization HeaderAgendaDate="Datum" AllDay="Bez času od/do" HeaderMonth="Měsíc" HeaderDay="Den" HeaderMultiDay="Multi-den" HeaderWeek="Týden" ShowMore="více..." HeaderToday="Dnes" HeaderAgendaAppointment="Úkol nebo Událost" HeaderAgendaTime="Čas" />

    <AgendaView UserSelectable="false" NumberOfDays="100" ShowDateHeaders="true" ReadOnly="true" ShowColumnHeaders="false" TimeColumnWidth="110px" DateColumnWidth="130px" />
    <DayView UserSelectable="true" DayStartTime="08:00" DayEndTime="20:00" ShowInsertArea="false" />

    <AppointmentTemplate>
        <a class="reczoom" rel="<%# Eval("Description")%>">i</a>
        <a href="javascript:re(<%# Eval("ID")%>)"><%# Eval("Subject")%></a>


    </AppointmentTemplate>


</telerik:RadScheduler>
<div>
    <asp:Label ID="lblNoAppointments" runat="server" Text="Za zvolené období žádné úkoly/události s termínem." CssClass="timestamp"></asp:Label>
</div>
<asp:Panel ID="panP56" runat="server" CssClass="content-box2" Visible="false" style="width:600px;">
    <div class="title">
        <img src="Images/task.png" alt="Úkol" />
        Otevřené úkoly bez termínu
                    <asp:Label ID="p56Count" runat="server" CssClass="badge1"></asp:Label>

    </div>
    <div class="content" style="overflow: auto; max-height: 200px;">

        <table>

            <asp:Repeater ID="rpP56" runat="server">
                <ItemTemplate>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="Project" runat="server" CssClass="timestamp"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">

                        <td style="max-width: 500px;">
                            <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail úkolu"></asp:HyperLink>
                            <asp:HyperLink ID="link1" runat="server" Target="_top"></asp:HyperLink>
                            <div>
                                <asp:Label ID="p56PlanUntil" runat="server" ToolTip="Termín úkolu"></asp:Label>
                                <asp:Image ID="img1" runat="server" ImageUrl="Images/reminder.png" ToolTip="Připomenutí" />
                            </div>
                        </td>
                        <td style="width: 30px;">
                            <asp:HyperLink ID="linkWorksheet" ImageUrl="Images/worksheet.png" runat="server" ToolTip="Vykázat úkon do úkolu"></asp:HyperLink>
                        </td>
                        <td style="text-align: center;">
                            <asp:HyperLink ID="linkWorkflow" runat="server" Text="Posunout/doplnit"></asp:HyperLink>

                        </td>

                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

    </div>
</asp:Panel>
<asp:HiddenField ID="hidPrefix" runat="server" />
<asp:HiddenField ID="hidRecordPID" runat="server" />
<asp:HiddenField ID="hidDefHeight" runat="server" Value="300px" />