<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_approving_step2.aspx.vb" Inherits="UI.p31_approving_step2" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

            InhaleGridHeight();

        });

        function InhaleGridHeight() {
            var h1 = new Number;
            var h2 = new Number;
            var h3 = new Number;

            h1 = $(window).height();

            var ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            h3 = h1 - h2 - 150;

            document.getElementById("<%=hidGridHeight.clientid%>").value = h3;

        }

        function o23_record(pid) {

            dialog_master("o23_record.aspx?billing=1&masterprefix=<%=ViewState("masterprefix")%>&masterpid=<%=ViewState("masterpid")%>&pid=" + pid, true);

        }
        function hardrefresh(pid, flag) {
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 50px;">
        <table cellpadding="5" cellspacing="2">
            <tr valign="top">
                <td>
                    <asp:Label ID="lblCount" runat="server" CssClass="lbl" Text="Počet úkonů pro schvalovací proces:"></asp:Label>
                    <div>
                        <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="z toho zamítnuté pro schvalování:"></asp:Label>
                    </div>
                </td>
                <td style="text-align:right;">
                    <asp:Label ID="CountAll" runat="server" CssClass="valbold"></asp:Label>
                    <div>
                        <asp:Label ID="CountRefused" runat="server" CssClass="valbold" ForeColor="red"></asp:Label>
                    </div>
                </td>
               
                <td>
                    <asp:Panel ID="panO23" runat="server">
                    <button type="button" id="cmdO23" class="show_hide1">
                        <img src="Images/notepad.png" />
                        <asp:Label ID="lblO23" runat="server" Text="Fakturační poznámky"></asp:Label>
                        <img src="Images/arrow_down.gif" alt="Nastavení" />
                    </button>
                    </asp:Panel>
                    <asp:Label ID="BillingMemo" runat="server" CssClass="infoNotification" Font-Italic="true"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="slidingDiv1">
        <uc:o23_list ID="notepad1" runat="server"></uc:o23_list>
        <div class="div6">
            <asp:HyperLink ID="cmdNewO23" runat="server" Text="Napsat novou poznámku" NavigateUrl="javascript:o23_record(0)"></asp:HyperLink>
        </div>

    </div>
    <div id="offsetY"></div>
    <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid"></uc:datagrid>
    <div class="div6">
        <span>Nezařazené zařadit do billing dávky:</span>
        <telerik:RadComboBox ID="p31ApprovingSet" runat="server" ShowToggleImage="false" ShowDropDownOnTextboxClick="true" MarkFirstMatch="true" Width="250px" AllowCustomText="true"></telerik:RadComboBox>
        
    </div>

    <asp:HiddenField ID="hidGridHeight" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
