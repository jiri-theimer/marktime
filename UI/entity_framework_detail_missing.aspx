<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="entity_framework_detail_missing.aspx.vb" Inherits="UI.entity_framework_detail_missing" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function p28_create() {

            sw_local("p28_record.aspx?pid=0", "Images/contact_32.png", true);

        }
        function p41_create() {

            sw_local("p41_create.aspx", "Images/project_32.png", true);

        }
        function p56_create() {
            <%If ViewState("masterprefix") = "p41" Then%>
            sw_local("p56_record.aspx?pid=0&masterprefix=p41&masterpid=<%=ViewState("masterpid")%>", "Images/task_32.png", true);
            <%Else%>
            sw_local("p56_record.aspx?pid=0&masterprefix=p41&masterpid=0", "Images/task_32.png", true);
            <%End If%>


        }
        function j02_create() {

            sw_local("j02_record.aspx?pid=0", "Images/person_32.png", true);

        }
        function p91_create() {
            sw_local("p91_create_step1.aspx?prefix=p28", "Images/invoice_32.png", true)

        }
        function x31_create() {

            sw_local("x31_record.aspx?pid=0", "Images/report_32.png", true);

        }
        function o23_create() {

            sw_local("o23_record.aspx?pid=0", "Images/notepad_32.png", true);

        }

        function hardrefresh(pid, flag) {
            if (flag == "p28-create") {
                parent.window.location.replace("p28_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "p41-create") {
                parent.window.location.replace("p41_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "j02-save") {
                parent.window.location.replace("j02_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "o23-save") {
                parent.window.location.replace("o23_framework.aspx?pid=" + pid);
                return;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:pageheader ID="ph1" runat="server" IsInForm="true" Visible="false" Text="Nebyl vybrán záznam." />
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Image ID="img1" runat="server" />
            </td>
            <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Style="font-size: 200%;"></asp:Label>
            </td>
            <td>
                <asp:HyperLink ID="cmdNew" runat="server" Visible="false"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <div class="div6">
        <asp:HyperLink ID="MasterRecord" runat="server" Font-Bold="true" Target="_top"></asp:HyperLink>
    </div>

    <asp:Panel ID="panStat" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/information.png" />
        </div>
        <div class="content">
        <table cellpadding="10" id="responsive">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblCount" Text="Počet dostupných záznamů:"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="Count4Read" CssClass="valbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblBin" Text="Z toho přesunuté do archivu:"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="CountBin" CssClass="valbold"></asp:Label>
                </td>
            </tr>
        </table>
        </div>
    </asp:Panel>


    <asp:Panel ID="panSearch" runat="server" CssClass="content-box1">
        <div class="title">
            <img src="Images/search.png" />
        </div>
        <div class="content">
            <asp:Panel ID="panSearch_p28" runat="server" Visible="false">

                <input id="search_p28" style="width: 200px; margin-top: 7px;" value="Najít klienta..." onfocus="search2Focus(this)" onblur="search2Blur(this,'Najít klienta...')" />
            </asp:Panel>
            <asp:Panel ID="panSearch_p91" runat="server" Style="margin-top: 6px;" Visible="false">

                <input id="search_p91" style="width: 200px; margin-top: 7px;" value="Najít fakturu..." onfocus="search2Focus(this)" onblur="search2Blur(this,'Najít fakturu...')" />
            </asp:Panel>
            <asp:Panel ID="panSearch_p56" runat="server" Style="margin-top: 6px;" Visible="false">

                <input id="search_p56" style="width: 200px; margin-top: 7px;" value="Najít úkol..." onfocus="search2Focus(this)" onblur="search2Blur(this,'Najít úkol...')" />
            </asp:Panel>
            <asp:Panel ID="panSearch_j02" runat="server" Style="margin-top: 6px;" Visible="false">

                <input id="search_j02" style="width: 200px; margin-top: 7px;" value="Najít osobu..." onfocus="search2Focus(this)" onblur="search2Blur(this,'Najít osobu...')" />
            </asp:Panel>
            <asp:Panel ID="panSearch_o23" runat="server" Style="margin-top: 6px;" Visible="false">

                <input id="search_o23" style="width: 200px; margin-top: 7px;" value="Najít dokument..." onfocus="search2Focus(this)" onblur="search2Blur(this,'Najít dokument...')" />
            </asp:Panel>
        </div>
    </asp:Panel>

    


    <script type="text/javascript">
        <%If panSearch_p28.Visible Then%>
        $(function () {

            $("#search_p28").autocomplete({
                source: "Handler/handler_search_contact.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {
                        window.open("p28_framework.aspx?pid=" + ui.item.PID, "_top");
                        return false;
                    }
                }

            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Project, item.FilterString);


                s = s + "</a>";

                if (item.Draft == "1")
                    s = s + "<img src='Images/draft.png' alt='DRAFT'/>"

                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });
        <%End If%>
        <%If panSearch_p91.Visible Then%>
        $(function () {

            $("#search_p91").autocomplete({
                source: "Handler/handler_search_invoice.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {
                        window.open("p91_framework.aspx?pid=" + ui.item.PID, "_top");
                        return false;
                    }
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Invoice, item.FilterString);


                s = s + "</a>";

                if (item.Draft == "1")
                    s = s + "<img src='Images/draft.png' alt='DRAFT'/>"

                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });
        <%End If%>
        <%If panSearch_p56.Visible Then%>
        $(function () {

            $("#search_p56").autocomplete({
                source: "Handler/handler_search_task.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {
                        window.open("p56_framework.aspx?pid=" + ui.item.PID, "_top");
                        return false;
                    }
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Name + " | " + item.Project, item.FilterString);


                s = s + "</a>";



                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });
        <%End If%>
        <%If panSearch_j02.Visible Then%>
        $(function () {

            $("#search_j02").autocomplete({
                source: "Handler/handler_search_person.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {
                        window.open("j02_framework.aspx?pid=" + ui.item.PID, "_top");
                        return false;
                    }
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Project, item.FilterString);


                s = s + "</a>";



                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });
        <%End If%>
        <%If panSearch_o23.Visible Then%>
        $(function () {

            $("#search_j02").autocomplete({
                source: "Handler/handler_search_notepad.ashx",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {
                        window.open("o23_framework.aspx?pid=" + ui.item.PID, "_top");
                        return false;
                    }
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.Name, item.FilterString);


                s = s + "</a>";



                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });
        <%End If%>



        function __highlight(s, t) {
            var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
            return s.replace(matcher, "<strong>$1</strong>");
        }

        function search2Focus(ctl) {
            ctl.value = "";
            ctl.style.background = "yellow";
        }
        function search2Blur(ctl, defaultMessage) {

            ctl.style.background = "";
            ctl.value = defaultMessage;
        }

    </script>
</asp:Content>
