<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_periodcombo.aspx.vb" Inherits="UI.clue_periodcombo" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function fireperiod(radioButtonList) {
            var val = GetRadioButtonListSelectedValue(radioButtonList);
            alert(val);
            window.parent.hardrefresh_periodcombo(val);

        }

        <%If Me.NeedRebindPeriodCombo = True Then%>
        window.parent.hardrefresh_periodcombo("-1");
        <%end if%>

        function GetRadioButtonListSelectedValue(radioButtonList) {

            for (var i = 0; i < radioButtonList.rows.length; ++i) {

                if (radioButtonList.rows[i].cells[0].firstChild.checked) {

                    var s = radioButtonList.rows[i].cells[0].firstChild.value;
                    window.parent.hardrefresh_periodcombo(s);

                }

            }

        }

        function periodcombo_setting(){
            window.parent.periodcombo_setting();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Zadat rozsah datumů filtru</legend>
        <table>
            <tr valign="top">

                <td>
                    <telerik:RadDatePicker ID="d1" runat="server" SharedCalendarID="SharedCalendar" Width="120px" MaxDate="1.1.3000" MinDate="1.1.1900">
                        <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                    </telerik:RadDatePicker>
                </td>
                <td style="width: 10px;">-
                </td>
                <td>
                    <telerik:RadDatePicker ID="d2" runat="server" SharedCalendarID="SharedCalendar" Width="120px" MaxDate="1.1.3000" MinDate="1.1.1900">
                        <DateInput ID="DateInput2" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                    </telerik:RadDatePicker>
                </td>
                <td>
                    <asp:Button ID="cmdSubmit" runat="server" Text="Uložit období" CssClass="button" />
                </td>
                <td>
                    <span>Pojmenovat (nepovinné):</span>
                </td>
                <td>
                    <asp:TextBox ID="txtPeriodName" runat="server" Style="width: 140px;"></asp:TextBox>
                </td>
            </tr>

        </table>
    </fieldset>
    <fieldset>
        <legend>Vlastní pojmenovaná období
        </legend>
        <asp:RadioButtonList ID="period1" runat="server" AutoPostBack="false" onclick="GetRadioButtonListSelectedValue(this);" RepeatDirection="Vertical" CellPadding="3" DataValueField="ComboValue" DataTextField="NameWithDates">
        </asp:RadioButtonList>
        <button type="button" onclick="periodcombo_setting()">Více nastavení</button>
    </fieldset>



    <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
        
        <SpecialDays>
                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
                </SpecialDays>
    </telerik:RadCalendar>
    <asp:HiddenField ID="hidCustomQueries" runat="server" />

</asp:Content>
