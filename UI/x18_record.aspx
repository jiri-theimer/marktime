<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x18_record.aspx.vb" Inherits="UI.x18_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function x25_record(x25id) {

            dialog_master("x25_record.aspx?source=x18_record&x23id=<%=Me.CurrentX23ID%>&pid=" + x25id, true)

        }

        function hardrefresh(pid, flag) {
            document.getElementById("<%=HardRefreshPID.ClientID%>").value = pid;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefresh, "", False)%>;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
        <Tabs>
            <telerik:RadTab Text="Vlastnosti" Selected="true" Value="core"></telerik:RadTab>
            <telerik:RadTab Text="Ostatní" Value="other"></telerik:RadTab>

        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="core" runat="server" Selected="true">
            <table cellpadding="5" cellspacing="2">
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název štítku:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="x18Name" runat="server" Style="width: 400px;"></asp:TextBox>
                        <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl"></asp:Label>
                        <telerik:RadNumericTextBox ID="x18Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
                    </td>

                </tr>

            </table>



            

            <div class="div6">
                <asp:CheckBox ID="x18IsMultiSelect" runat="server" Text="Povolen MULTI-SELECT (možnost oštítkovat záznam entity více položkami najednou)" Checked="true" CssClass="chk" />
            </div>

            <div class="content-box2" style="margin-top: 20px;">
                <div class="title">Štítek se nabízí pro entity:</div>
                <div class="content">
                    <asp:CheckBoxList ID="x29IDs" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                        <asp:ListItem Text="Projekt" Value="141"></asp:ListItem>
                        <asp:ListItem Text="Klient" Value="328"></asp:ListItem>
                        <asp:ListItem Text="Úkol" Value="356"></asp:ListItem>
                        <asp:ListItem Text="Dokument" Value="223"></asp:ListItem>
                        <asp:ListItem Text="Vystavená faktura" Value="391"></asp:ListItem>
                        <asp:ListItem Text="Zálohová faktura" Value="390"></asp:ListItem>
                        <asp:ListItem Text="Osoba" Value="102"></asp:ListItem>
                        <asp:ListItem Text="Worksheet úkon" Value="331"></asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </div>

            <div class="content-box2" style="margin-top:20px;">
                <div class="title">
                    Položky štítku
            <% If Me.CurrentX23ID <> 0 Then%>
                    <a href="javascript:x25_record(0)" style="margin-left: 40px;">Přidat položku</a>
                    <%End If%>
                </div>
                <div class="content">

                    <asp:Repeater ID="rpX25" runat="server">
                        <ItemTemplate>
                            <div class="badge_label" style="background-color: <%#Eval("x25BackColor")%>">
                                <a href="javascript:x25_record(<%# Eval("pid") %>)" style="color: <%#Eval("x25ForeColor")%>; text-decoration: <%#Eval("StyleDecoration")%>" title="Upravit/odstranit položku"><%# Eval("x25Name") %></a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <div class="div6" style="margin-top: 20px;">
                        <asp:RadioButtonList ID="opg1" runat="server" AutoPostBack="true" Visible="false" RepeatDirection="Vertical">
                            <asp:ListItem Text="Štítek bude mít vlastní zdroj položek" Value="1" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Štítek bude využívat již existující zdroj položek" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div>
                            <asp:Label ID="lblX23ID" Text="Datový zdroj položek štítku:" runat="server" CssClass="lblReq"></asp:Label>
                            <uc:datacombo ID="x23ID" runat="server" DataTextField="x23Name" DataValueField="pid" IsFirstEmptyRow="true" AutoPostBack="true"></uc:datacombo>
                            <asp:Button ID="cmdConfirmOpg1" runat="server" CssClass="cmd" Text="Potvrdit" />
                        </div>
                    </div>
                </div>
            </div>



        </telerik:RadPageView>

        <telerik:RadPageView ID="other" runat="server">
            <div class="div6">
                <asp:CheckBox ID="x18IsAllEntityTypes" runat="server" CssClass="chk" AutoPostBack="true" Text="Štítek je aplikovatelný pro všechny záznamy vybraných entit" Checked="true" />
            </div>
            <asp:Panel ID="panEntityTypes" runat="server" CssClass="content-box2">
                <div class="title">
                    Štítek se bude nabízet k oštítkování pouze u níže zaškrtlých typů entity:
                </div>
                <div class="content">
                    <table cellpadding="10">

                        <asp:Repeater ID="rp1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkEntityType" runat="server" CssClass="chk" Font-Bold="true" />
                                        <asp:HiddenField ID="x22EntityTypePID" runat="server" />
                                        <asp:HiddenField ID="x29ID_EntityType" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="x22IsEntryRequired" runat="server" Text="Povinné oštítkování u záznamu entity" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </asp:Panel>

            <div class="div6">
                <asp:CheckBox ID="x18IsRequired" runat="server" Text="U záznamu entity je oštítkování povinné" CssClass="chk" />
            </div>

            <div class="content-box2" style="margin-top: 100px;">
                <div class="title">
                    <img src="Images/projectrole.png" width="16px" height="16px" />
                    <asp:Label ID="ph1" runat="server" Text="Oprávnění ke správě položek štítku"></asp:Label>
                    <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
                </div>
                <div class="content">
                    <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="x18EntityCategory"></uc:entityrole_assign>
                </div>
            </div>
            <div class="div6">
                <asp:Label ID="lblOwner" runat="server" Text="Vlastník záznamu:" CssClass="lblReq"></asp:Label>
                <uc:person ID="j02ID_Owner" runat="server" Width="300px" Flag="all" />
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>

    <asp:HiddenField ID="hidTempX23ID" runat="server" />
    <asp:HiddenField ID="hidGUID" runat="server" />
    <asp:HiddenField ID="HardRefreshPID" runat="server" />
    <asp:Button ID="cmdHardRefresh" runat="server" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
