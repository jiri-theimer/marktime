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
            <%If Me.CurrentX29ID = BO.x29IdEnum.j02Person Then%>
            context["flag"] = "all2";
            <%End If%>
            <%If Me.CurrentX29ID = BO.x29IdEnum.p41Project Then%>
            context["j02id_explicit"] = "<%=Master.Factory.SysUser.j02ID%>";
            <%End If%>
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panX20" runat="server" CssClass="content-box2">
        <div class="title">
            Vazby [<asp:Label ID="x18Name" runat="server"></asp:Label>]
        </div>
        <div class="content">
            <div>
                <asp:RadioButtonList ID="opgX20ID" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" DataValueField="x20ID" DataTextField="BindName"></asp:RadioButtonList>
            </div>
            <div>
                <telerik:RadComboBox ID="cbx1" runat="server" RenderMode="Auto" DropDownWidth="600px" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" Text="Hledat..." Width="600px" OnClientItemsRequesting="cbx1_OnClientItemsRequesting" AutoPostBack="true">
                    <WebServiceSettings Method="LoadComboData" UseHttpGet="false" />
                </telerik:RadComboBox>
            </div>
        </div>


    </asp:Panel>

    <div style="overflow:auto;max-height:200px;width:700px;">
    <table cellpadding="5" cellspacing="2">
    <asp:Repeater ID="rpX19" runat="server">
        <ItemTemplate>
            <tr class="trHover">
                <td style="width: 140px;">
                    <asp:Label ID="Entity" runat="server" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="RecordAlias" runat="server" CssClass="valbold"></asp:Label>
                    <asp:ImageButton ID="del" runat="server" CommandName="delete" ImageUrl="Images/delete.png" ToolTip="Odstranit vazbu" CssClass="button-link" />
                    <asp:HiddenField ID="p85id" runat="server" />
                </td>
            </tr>

        </ItemTemplate>
    </asp:Repeater>
    </table>
    </div>

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
                <asp:Button ID="cmdChangeCode" runat="server" CssClass="cmd" Text="Změnit kód ručně" Visible="false" />
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
    <asp:HiddenField ID="hidX29ID" runat="server" />
    <asp:HiddenField ID="hidGUID_x19" runat="server" />

    <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
            <SpecialDays>
                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
            </SpecialDays>
        </telerik:RadCalendar>

    <asp:HiddenField ID="hidx18CalendarFieldStart" runat="server" />
    
    <asp:HiddenField ID="hidx18CalendarFieldEnd" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>


