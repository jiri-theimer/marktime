<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p49_record.aspx.vb" Inherits="UI.p49_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="6">
        <tr>
            <td>
                <asp:Label ID="lblP41ID" runat="server" CssClass="lblReq" Text="Projekt:"></asp:Label>
            </td>
            <td>
                <uc:project ID="p41ID" runat="server" Width="400px" AutoPostBack="false" />
            </td>
        </tr>
        <tr>
            <td><span class="lblReq">Sešit:</span></td>
            <td>
                <uc:datacombo ID="p34ID" runat="server" DataTextField="p34Name" DataValueField="pid" IsFirstEmptyRow="true" Width="400px" AutoPostBack="true"></uc:datacombo>
            </td>

        </tr>
        <tr>
            <td><span class="lbl">Aktivita:</span></td>
            <td>
                <uc:datacombo ID="p32ID" runat="server" DataTextField="p32Name" DataValueField="pid" IsFirstEmptyRow="true" Width="400px"></uc:datacombo>
            </td>
        </tr>
        <tr>
            <td><span class="lbl">Osoba:</span></td>
          <td>
                <uc:person ID="j02ID" runat="server" Width="400px" />
            </td>
        </tr>
        <tr>
            <td><span class="lblReq">Částka (bez DPH):</span></td>
            <td>
                <telerik:RadNumericTextBox ID="p49Amount" runat="server" Width="110px" NumberFormat-DecimalDigits="2" ShowSpinButtons="false"></telerik:RadNumericTextBox>
                <uc:datacombo ID="j27ID" Width="60px" runat="server" DataTextField="j27Code" DataValueField="pid"></uc:datacombo>
            </td>
        </tr>
        <tr>
            <td><span class="lblReq">Datum od:</span></td>
            <td>
                <telerik:RadDatePicker ID="p49DateFrom" runat="server" RenderMode="Lightweight" Width="120px" DateInput-EmptyMessage="Povinný údaj." DateInput-EmptyMessageStyle-ForeColor="red" SharedCalendarID="SharedCalendar">
                    <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                </telerik:RadDatePicker>
                <span class="lblReq">Do:</span>
                <telerik:RadDatePicker ID="p49DateUntil" runat="server" RenderMode="Lightweight" Width="120px" DateInput-EmptyMessage="Povinný údaj." DateInput-EmptyMessageStyle-ForeColor="red" SharedCalendarID="SharedCalendar">
                    <DateInput ID="DateInput2" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr valign="top">
            <td>Text:</td>
            <td>
                <asp:TextBox ID="p49Text" runat="server" TextMode="MultiLine" Style="width: 400px; height: 50px;"></asp:TextBox>
            </td>
        </tr>
    </table>
    
     <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
        
        <SpecialDays>
                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
                </SpecialDays>
    </telerik:RadCalendar>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
