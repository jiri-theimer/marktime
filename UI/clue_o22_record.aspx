<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_o22_record.aspx.vb" Inherits="UI.clue_o22_record" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function detail() {

            window.parent.sw_local("o22_record.aspx?masterprefix=<%=ViewState("masterprefix")%>&masterpid=<%=ViewState("masterpid")%>&pid=<%=Master.DataPID%>", "Images/calendar_32.png", true);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panContainer" runat="server" Style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <asp:panel ID="panHeader" runat="server">
            <asp:Image ID="img1" runat="server" ImageUrl="Images/calendar_32.png" />
            <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
        </asp:panel>

        <asp:HyperLink ID="cmDetail" runat="server" Style="position: absolute; top: 50px; left: 500px;" NavigateUrl="javascript:detail()" Text="Upravit"></asp:HyperLink>
        <table cellpadding="10" cellspacing="2">
            <tr>

                <td colspan="2">
                    <asp:Label ID="o21Name" runat="server" CssClass="valboldblue"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>Název (předmět):</td>
                <td>
                    <asp:Label ID="o22Name" runat="server" CssClass="valbold" Style="color: red;"></asp:Label>

                </td>
            </tr>
            <tr valign="top">
                <td>Čas:</td>
                <td>
                    <asp:Label ID="Period" runat="server" CssClass="valbold"></asp:Label>
                    <asp:Label ID="lblPeriodMessage" runat="server" CssClass="infoInForm" Text="Událost již proběhla." Visible="false"></asp:Label>
                    <asp:Label ID="lblReminder" runat="server" CssClass="lbl" Style="padding-left: 20px;" Text="Čas připomenutí:"></asp:Label>
                    <asp:Label ID="o22ReminderDate" runat="server" CssClass="valbold" ForeColor="LimeGreen"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLocation" runat="server" CssClass="lbl" Text="Lokalita:"></asp:Label></td>
                <td>
                    <asp:Label ID="o22Location" runat="server" CssClass="valbold"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Zapojené osoby:"></asp:Label>
                </td>
                <td>
                    <asp:Repeater ID="rpO20" runat="server">
                        <ItemTemplate>

                            <span class="valboldblue" style="padding-right: 20px;"><%# Eval("Person")%><%# Eval("j11Name")%></span>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    <asp:Label ID="o22Description" runat="server" CssClass="infoInForm"></asp:Label>

                </td>
            </tr>

        </table>




        <asp:Panel ID="panO19" runat="server">
            <fieldset>
                <legend>
                    <img src="Images/car.png" style="padding-right: 6px;" />Rezervované nepersonální zdroje k události</legend>
                <asp:Repeater ID="rpO19" runat="server">
                    <ItemTemplate>

                        <span class="valboldblue" style="padding: 20px;"><%# Eval("j23Name")%> (<%# Eval("j23Code")%>)</span>
                    </ItemTemplate>
                </asp:Repeater>

            </fieldset>
        </asp:Panel>

        <div class="div6">
            <asp:Label ID="Timestamp" runat="server" CssClass="timestamp"></asp:Label>
        </div>

        <uc:b07_list ID="comments1" runat="server" />
    </asp:Panel>

</asp:Content>
