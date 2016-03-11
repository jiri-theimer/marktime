<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p31_approve_onerec.ascx.vb" Inherits="UI.p31_approve_onerec" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="content-box2">
    <div class="title">

        <asp:Label ID="lblHeader" runat="server"></asp:Label>
        <asp:Button ID="cmdUlozitSchvalovani" runat="server" Text="Uložit změny" CssClass="cmd" />
        <asp:Button ID="cmdZrusitSchvalovani" runat="server" Text="Zrušit" CssClass="cmd" />
        <img src="Images/approve.png" style="float: right;" />
    </div>
    <div class="content">
        <div class="div6">
            <asp:RadioButtonList ID="p71id" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">

                <asp:ListItem Text="Schváleno" Value="1"></asp:ListItem>
                <asp:ListItem Text="Nerozhodnuto (zůstane rozpracované)" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <table cellpadding="0">
            <tr style="vertical-align: baseline;">

                <td style="min-width: 180px;">
                    <asp:RadioButtonList ID="p72id" runat="server" AutoPostBack="true" RepeatDirection="Vertical" RepeatColumns="1" CellPadding="3">
                        <asp:ListItem Text="<img src='Images/a14.gif'/>Fakturovat" Value="4"></asp:ListItem>
                        <asp:ListItem Text="<img src='Images/a16.gif'/>Zahrnout do paušálu" Value="6"></asp:ListItem>
                        <asp:ListItem Text="<img src='Images/a12.gif'/>Viditelný odpis" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<img src='Images/a13.gif'/>Skrytý odpis" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <asp:PlaceHolder ID="place1" runat="server"></asp:PlaceHolder>
                <td>


                    <table cellpadding="5">
                        <tr>
                            <td style="width: 190px;">
                                <asp:Label ID="lblFakturovat" runat="server" Text="Hodnota pro fakturaci:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="value_approved" runat="server" Style="width: 80px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRate_Billing_Approved" runat="server" Text="Fakturační sazba:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>

                                <telerik:RadNumericTextBox ID="Rate_Billing_Approved" runat="server" NumberFormat-DecimalDigits="2" Width="80px" Value="0">
                                </telerik:RadNumericTextBox>
                                <asp:Label ID="j27Code" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblVatRate_Approved" runat="server" Text="Sazba DPH:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>

                                <telerik:RadNumericTextBox ID="VatRate_Approved" runat="server" NumberFormat-DecimalDigits="2" Width="50px" Value="0">
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                    </table>

                    <asp:Panel ID="panInternalContainer" runat="server" Visible="false">
                        <asp:Panel ID="panInternal" runat="server">
                            <fieldset style="border: dotted 1px black;">
                                <legend style="font-weight: normal; font-size: 90%;">Vnitropodnikové schvalování</legend>
                                <table cellpadding="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblInternal" runat="server" Text="Hodnota pro interní schvalování:" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="value_approved_internal" runat="server" Style="width: 80px;"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 190px;">
                                            <asp:Label ID="lblRate_Internal_Approved" runat="server" Text="Interní (nákladová) sazba:" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="Rate_Internal_Approved" runat="server" NumberFormat-DecimalDigits="2" Width="80px" Value="0">
                                            </telerik:RadNumericTextBox>
                                        </td>
                                    </tr>


                                </table>
                            </fieldset>
                        </asp:Panel>
                    </asp:Panel>
                </td>

            </tr>
        </table>
        <div>
            <asp:Label ID="lblP31Text" runat="server" Text="Podrobný popis úkonu:" CssClass="lbl"></asp:Label>
        </div>
        <asp:TextBox ID="p31Text" runat="server" TextMode="MultiLine" Style="width: 97%; height: 50px;"></asp:TextBox>
        <div>
            <span>Zařadit do billing dávky:</span>
        <telerik:RadComboBox ID="p31ApprovingSet" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="150px" AllowCustomText="true"></telerik:RadComboBox>
        </div>
    </div>
</div>
<asp:HiddenField ID="p33id" runat="server" />
<asp:HiddenField ID="j03id" runat="server" />
<asp:HiddenField ID="p31id" runat="server" />
<asp:HiddenField ID="isbillable" runat="server" />
<asp:HiddenField ID="hidIsVertical" runat="server" Value="0" />
<asp:HiddenField ID="hidGUID_TempData" runat="server" />
