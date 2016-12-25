<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="navigator.aspx.vb" Inherits="UI.navigator" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var _initResizing = "1";

        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });
            
            if (document.getElementById("<%=hidSettingIsActive.ClientID%>").value == "1") {
                $(".slidingDiv1").show();
            }

            document.getElementById("<%=hidSettingIsActive.ClientID%>").value = "0";
        });

        function loadSplitter(sender) {

            var h1 = new Number;
            var h2 = new Number;
            var h3 = new Number;

            h1 = $(window).height();

            var ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            h3 = h1 - h2;

            sender.set_height(h3);

            var pane = sender.getPaneById("<%=contentPane.ClientID%>");
            document.getElementById("<%=Me.hidContentPaneWidth.ClientID%>").value = pane.get_width();
            //pane.set_contentUrl(document.getElementById("<%=Me.hidContentPaneDefUrl.ClientID%>").value + "&parentWidth=" + pane.get_width());



        }

        function SavePaneWidth(w) {
            $.post("Handler/handler_userparam.ashx", { x36value: w, x36key: "navigator-navigationPane_width", oper: "set" }, function (data) {
                if (data == ' ') {
                    return;
                }


            });
        }

        function AfterPaneResized(sender, args) {
            if (_initResizing == "1") {
                _initResizing = "0";
                return;
            }

            var w = sender.get_width();
            SavePaneWidth(w);

        }

        function AfterPaneCollapsed(pane) {
            var w = "-1";
            SavePaneWidth(w);
        }
        function AfterPaneExpanded(pane) {
            var w = pane.get_width();
            SavePaneWidth(w);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="offsetY"></div>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="100%" ResizeMode="Proportional" OnClientLoad="loadSplitter" PanesBorderSize="0" Skin="Metro" RenderMode="Lightweight" Orientation="Vertical">
        <telerik:RadPane ID="navigationPane" runat="server" Width="350px" OnClientResized="AfterPaneResized" OnClientCollapsed="AfterPaneCollapsed" OnClientExpanded="AfterPaneExpanded" BackColor="white">
            <button type="button" id="cmdSetting" class="show_hide1" style="float:right;padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Nastavit si úrovně navigátora">
                <span>Nastavení</span>
                <img src="Images/arrow_down.gif" />
            </button>
            <div style="clear:both;"></div>
            <div class="slidingDiv1">
                <table>
                    <tr>
                        <td>
                            <span class="lbl">Úroveň #1:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbxLevel0" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="Klient" Value="p28"></asp:ListItem>
                                <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                                <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                                <asp:ListItem Text="Středisko" Value="j18"></asp:ListItem>                                
                                <asp:ListItem Text="Faktura" Value="p91"></asp:ListItem>
                                <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="lbl">Úroveň #2:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbxLevel1" runat="server" AutoPostBack="true">
                                <asp:ListItem Text=""></asp:ListItem>
                                <asp:ListItem Text="Klient" Value="p28"></asp:ListItem>
                                <asp:ListItem Text="Projekt" Value="p41" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                                <asp:ListItem Text="Středisko" Value="j18"></asp:ListItem>                                
                                <asp:ListItem Text="Faktura" Value="p91"></asp:ListItem>
                                <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="lbl">Úroveň #3:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbxLevel2" runat="server" AutoPostBack="true">
                                <asp:ListItem Text=""></asp:ListItem>
                                <asp:ListItem Text="Klient" Value="p28"></asp:ListItem>
                                <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                                <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                                <asp:ListItem Text="Středisko" Value="j18"></asp:ListItem>                                
                                <asp:ListItem Text="Faktura" Value="p91"></asp:ListItem>
                                <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="cmdRefresh" runat="server" CssClass="cmd" Text="Obnovit" />
            </div>

            <telerik:RadTreeView ID="tr1" runat="server" Skin="Default" ShowLineImages="false" SingleExpandPath="true">
            </telerik:RadTreeView>


            <asp:HiddenField ID="hiddatapid" runat="server" />
            <asp:HiddenField ID="hidDefaultSorting" runat="server" />

            <asp:HiddenField ID="hidX29ID" runat="server" Value="141" />
            <asp:HiddenField ID="hidPrefix" runat="server" Value="p41" />

            <asp:HiddenField ID="hidCols" runat="server" />
            <asp:HiddenField ID="hidSumCols" runat="server" />
            <asp:HiddenField ID="hidAdditionalFrom" runat="server" />
            <asp:HiddenField ID="hidContentPaneWidth" runat="server" />
            <asp:HiddenField ID="hidContentPaneDefUrl" runat="server" />
            <asp:HiddenField ID="hidSettingIsActive" runat="server" Value="" />

        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
        </telerik:RadSplitBar>
        <telerik:RadPane ID="contentPane" runat="server" ShowContentDuringLoad="true" ContentUrl="blank.aspx">
            Detail projektu
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
