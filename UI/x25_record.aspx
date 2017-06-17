<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x25_record.aspx.vb" Inherits="UI.x25_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblX23ID" Text="Zdroj:" runat="server" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="x23ID" runat="server" DataTextField="x23Name" DataValueField="pid" IsFirstEmptyRow="true" Width="400px"></uc:datacombo>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název:"></asp:Label></td>
            <td>
                <asp:TextBox ID="x25Name" runat="server" Style="width: 400px;"></asp:TextBox>

                <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl"></asp:Label>
                <telerik:RadNumericTextBox ID="x25Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblx25Code" runat="server" CssClass="lbl" Text="Kód:"></asp:Label></td>
            <td>
                <asp:TextBox ID="x25Code" runat="server"></asp:TextBox>
            </td>
        </tr>



    </table>

    <asp:Panel ID="panX16" runat="server">
        <table cellpadding="3" cellspacing="2" width="100%">
            <asp:Repeater ID="rpX16" runat="server">
                <ItemTemplate>
                    <tr style="vertical-align: top;">

                        <td style="width: 150px;">
                            <asp:HiddenField ID="x16IsEntryRequired" runat="server" />

                            <asp:Label ID="x16Name" runat="server" CssClass="lbl"></asp:Label>

                        </td>
                        <td>

                            <asp:TextBox ID="txtFF_Text" runat="server"></asp:TextBox>
                            <telerik:RadNumericTextBox ID="txtFF_Number" runat="server"></telerik:RadNumericTextBox>
                            <asp:CheckBox ID="chkFF" runat="server" ForeColor="Black" />

                            <telerik:RadDatePicker ID="txtFF_Date" runat="server" Width="120px" SharedCalendarID="SharedCalendar">
                                <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                            </telerik:RadDatePicker>
                            <telerik:RadComboBox ID="cbxFF" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="400px"></telerik:RadComboBox>


                            <asp:HiddenField runat="server" ID="x16Field" />
                            <asp:HiddenField runat="server" ID="x16ID" />
                            <asp:HiddenField runat="server" ID="hidType" />


                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>

        </table>
        <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
            <SpecialDays>
                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
            </SpecialDays>
        </telerik:RadCalendar>
    </asp:Panel>

    <asp:Panel ID="panColors" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label2" Text="Barva pozadí:" runat="server" CssClass="lbl"></asp:Label>

                </td>
                <td>
                    <telerik:RadColorPicker ID="x25BackColor" runat="server" CurrentColorText="Vybraná barva" NoColorText="Bez barvy" ShowIcon="true" Preset="Standard">
                        <telerik:ColorPickerItem Value="#F0F8FF"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FAEBD7"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#00FFFF"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#7FFFD4"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#F0FFFF"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#F5F5DC"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FFE4C4"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#00FFFF"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FFFAF0"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#F8F8FF"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FFD700"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#F0E68C"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#E6E6FA"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FFB6C1"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FFA500"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#AFEEEE"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FFDAB9"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#87CEEB"></telerik:ColorPickerItem>
                        <telerik:ColorPickerItem Value="#FF6347"></telerik:ColorPickerItem>
                    </telerik:RadColorPicker>

                </td>
                <td>
                    <asp:Label ID="Label1" Text="Barva písma:" runat="server" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <telerik:RadColorPicker ID="x25ForeColor" runat="server" CurrentColorText="Vybraná barva" NoColorText="Bez barvy" ShowIcon="true" Preset="Standard">
                    </telerik:RadColorPicker>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:HiddenField ID="hidX18ID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>


