<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="timesheet_calendar.ascx.vb" Inherits="UI.timesheet_calendar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div>
    <div>
        <asp:DropDownList ID="CalRows" runat="server" AutoPostBack="true" ToolTip="Počet měsíců vertikálně" Visible="false">
            <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
        </asp:DropDownList>
        <span>Najednou zobrazené měsíce:</span>
        <asp:DropDownList ID="CalCols" runat="server" AutoPostBack="true">
            <asp:ListItem Text="1" Value="1" Selected="true"></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
            <asp:ListItem Text="3" Value="3"></asp:ListItem>
            <asp:ListItem Text="4" Value="4"></asp:ListItem>
        </asp:DropDownList>
        <asp:LinkButton ID="cmdToday" Text="Dnes" runat="server" Style="padding-left: 20px;"></asp:LinkButton>
    </div>

    <telerik:RadCalendar ID="cal1" ShowRowHeaders="false" ShowColumnHeaders="true" ShowDayCellToolTips="false" runat="server" AutoPostBack="true"
        RenderMode="Lightweight" Skin="Metro" EnableMultiSelect="false" EnableNavigationAnimation="false" EnableRepeatableDaysOnClient="false"
        EnableMonthYearFastNavigation="false" Width="300px">

        <FastNavigationSettings TodayButtonCaption="Dnes" EnableTodayButtonSelection="true"></FastNavigationSettings>
        
    </telerik:RadCalendar>
</div>
