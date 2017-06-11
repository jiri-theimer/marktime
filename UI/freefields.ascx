<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="freefields.ascx.vb" Inherits="UI.freefields" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<asp:Panel ID="panContainer" runat="server">
    <table cellpadding="3" cellspacing="2" width="100%">
        <asp:Repeater ID="rpFF" runat="server">
            <ItemTemplate>
                <tr id="trHeader" runat="server" visible="false">
                    <td colspan="2">
                        <div style="width: 100%; border-bottom: solid 2px gray;">
                            <asp:Label ID="headerFF" runat="server" Style="font-weight: bold; padding-left: 10px;"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td style="width: 140px;">
                        <asp:HiddenField ID="x28IsRequired" runat="server" />
                        <asp:Label ID="lblFF" runat="server" CssClass="lbl"></asp:Label>
                        <asp:HyperLink ID="clue_help" runat="server" CssClass="reczoom" Text="?" Visible="false" title="Nápověda"></asp:HyperLink>
                    </td>
                    <td>

                        <asp:TextBox ID="txtFF_Text" runat="server"></asp:TextBox>
                        <telerik:RadNumericTextBox ID="txtFF_Number" runat="server"></telerik:RadNumericTextBox>
                        <asp:CheckBox ID="chkFF" runat="server" ForeColor="Black" />
                        <telerik:RadDateInput ID="txtFF_Date" runat="server" DateFormat="dd.MM.yyyy"></telerik:RadDateInput>

                        <telerik:RadComboBox ID="cbxFF" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="400px"></telerik:RadComboBox>

                        <asp:HiddenField runat="server" ID="hidField" />
                        <asp:HiddenField runat="server" ID="hidType" />
                        <asp:HiddenField ID="hidX28ID" runat="server" />
                        <asp:HiddenField ID="hidX23ID" runat="server" />

                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>

    </table>
</asp:Panel>
<asp:Repeater ID="rp1" runat="server">
    <ItemTemplate>
        <div class="div6">
            <asp:Label ID="x18Name" runat="server" Width="150px"></asp:Label>
            <uc:datacombo id="x25IDs" runat="server" datatextfield="x25Name" datavaluefield="pid" allowcheckboxes="true" filter="Contains" width="400px"></uc:datacombo>
            <asp:HiddenField ID="x18ID" runat="server" />
            <asp:HiddenField ID="x18IsMultiSelect" runat="server" />
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:HiddenField ID="hidDataTable" runat="server" />
<asp:HiddenField ID="hidJ03ID" runat="server" />
<asp:HiddenField ID="hidDataPID" runat="server" />

<script type="text/javascript">
    function <%=Me.ClientID%>_OnClientItemsRequesting(sender, eventArgs) {

        var context = eventArgs.get_context();
        context["filterstring"] = eventArgs.get_text();
        context["j03id"] = document.getElementById("<%=Me.hidJ03ID.ClientID%>").value;

        context["x23id"] = sender.get_attributes().getAttribute("x23id");

    }




</script>
