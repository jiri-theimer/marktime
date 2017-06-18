<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x25_record.aspx.vb" Inherits="UI.x25_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function cbx1_OnClientSelectedIndexChanged(sender, eventArgs) {
            var combo = sender;
            var pid = combo.get_value();
            <%If opgLayout.SelectedValue = "1" Then%>
            var url = "<%=Me.CurrentPrefix%>_framework_detail.aspx?pid=" + pid + "&source=<%=opgLayout.SelectedValue%>";
            location.replace("<%=Me.CurrentPrefix%>_framework.aspx?pid=" + pid);
            <%End If%>
            <%If opgLayout.SelectedValue = "2" Then%>
            location.replace("<%=Me.CurrentPrefix%>_framework.aspx?pid=" + pid);
            <%End If%>
            <%If opgLayout.SelectedValue = "3" Then%>
            location.replace("<%=Me.CurrentPrefix%>_framework_detail.aspx?source=3&pid=" + pid);
            <%End If%>
        }
        function cbx1_OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            var combo = sender;

            if (combo.get_value() == "")
                context["filterstring"] = eventArgs.get_text();
            else
                context["filterstring"] = "";

            context["j03id"] = "<%=Master.Factory.SysUser.PID%>";
            context["flag"] = "searchbox";
            <%If Me.CurrentPrefix = "p41" Then%>
            context["j02id_explicit"] = "<%=Master.Factory.SysUser.j02ID%>";
            <%End If%>
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="div6">
        <div>
            <asp:RadioButtonList ID="opgX20ID" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" DataValueField="x20ID" DataTextField="BindName"></asp:RadioButtonList>
        </div>
        <div>
            <telerik:RadComboBox ID="cbx1" runat="server" RenderMode="Auto" DropDownWidth="400" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" Text="Hledat..." Width="120px" OnClientSelectedIndexChanged="cbx1_OnClientSelectedIndexChanged" OnClientItemsRequesting="cbx1_OnClientItemsRequesting" AutoPostBack="false">
                <WebServiceSettings Method="LoadComboData" UseHttpGet="false" />
            </telerik:RadComboBox>
        </div>
        <asp:HiddenField ID="hidSearchPrefix" runat="server" />
    </div>
    <table cellpadding="5" cellspacing="2">
        <asp:Repeater ID="rpX20" runat="server">
            <ItemTemplate>
                <tr>
                    <td style="width: 140px;">
                        <asp:Label ID="x20Name" runat="server"></asp:Label>
                    </td>
                    <td></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td style="width: 140px;">
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
        <table cellpadding="5" cellspacing="2">
            <asp:Repeater ID="rpX16" runat="server">
                <ItemTemplate>
                    <tr style="vertical-align: top;">

                        <td style="min-width: 140px;">
                            <asp:HiddenField ID="x16IsEntryRequired" runat="server" />

                            <asp:Label ID="x16Name" runat="server" CssClass="lbl"></asp:Label>

                        </td>
                        <td>

                            <asp:TextBox ID="txtFF_Text" runat="server"></asp:TextBox>
                            <telerik:RadNumericTextBox ID="txtFF_Number" runat="server"></telerik:RadNumericTextBox>
                            <asp:CheckBox ID="chkFF" runat="server" ForeColor="Black" />

                            <telerik:RadDatePicker ID="txtFF_Date" runat="server" Width="130px">
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

    <div class="div6" style="clear: both; margin-top: 20px;">
        <asp:Label ID="lblOwner" runat="server" Text="Vlastník záznamu:" CssClass="lblReq" Style="padding-right: 30px;"></asp:Label>
        <uc:person ID="j02ID_Owner" runat="server" Width="300px" Flag="all" />
    </div>
    <asp:HiddenField ID="hidX18ID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>


