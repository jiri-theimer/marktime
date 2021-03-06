﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p48_framework.aspx.vb" Inherits="UI.p48_framework" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="persons" Src="~/persons.ascx" %>
<%@ Register TagPrefix="uc" TagName="projects" Src="~/projects.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Scripts/jqueryui/jquery-ui.min.css" />
    <script src="Scripts/jqueryui/jquery-ui.min.js" type="text/javascript"></script>

    <style type="text/css">
        #selectable .ui-selecting {
            background: #FECA40;
        }

        #selectable .ui-selected {
            background: #F39814;
            color: red;
        }

        td.weekend {
            background-color: #F1F1F1;
            text-align: center;
        }

        td.workday {
            background-color: white;
            text-align: center;
        }

        .nondate {
            background-color: snow;
        }

        .outfond {
            background-color: #F1F1F1;
        }

        div.plan {
            background-color: #E0FFFF;
            width: 20px;
            height: 15px;
            padding: 1px;
            cursor: default;
            border: solid 1px gray;
            text-align: center;
            font-family: Calibri;
            font-size: 100%;
        }

        div.reality {
            background-color: #E0FFFF;
            width: 20px;
            height: 15px;
            padding: 1px;
            cursor: default;
            border: solid 1px gray;
            text-align: center;
            font-family: Calibri;
            font-size: 100%;
            text-decoration: line-through;
        }

        div.sum {
            background-color: white;
            width: 20px;
            height: 15px;
            padding: 1px;
            cursor: default;
            border: solid 1px whitesmoke;
            text-align: center;
            font-family: Calibri;
            font-size: 100%;
            color: gray;
        }

        div.sumoverfond {
            background-color: white;
            width: 20px;
            height: 15px;
            padding: 1px;
            cursor: default;
            border: solid 1px whitesmoke;
            text-align: center;
            font-family: Calibri;
            font-size: 100%;
            font-weight: bold;
            color: red;
        }

        .total {
            cursor: default;
            border: solid 1px whitesmoke;
            text-align: center;
            font-family: Calibri;
            font-size: 90%;
        }

        a.reczoom {
            border: none !important;
            font-family: Calibri !important;
            font-size: 100% !important;
            padding: 0px !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv2").hide();
            $(".show_hide2").show();
            $(".show_hide3").show();
            $(".slidingDiv3").hide();

            <%If chkIncludeWeekend.Checked Then%>
            $("#selectable").selectable({ filter: "td:not(.nondate)" });
            <%Else%>
            $("#selectable").selectable({ filter: "td:not(.weekend,.holiday,.nondate)" });
            <%End If%>
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv2").hide();
                $(".slidingDiv3").hide();
                $(".slidingDiv1").slideToggle();
            });

            $('.show_hide2').click(function () {
                $(".slidingDiv1").hide();
                $(".slidingDiv3").hide();
                $(".slidingDiv2").slideToggle();
            });

            $('.show_hide3').click(function () {
                $(".slidingDiv1").hide();
                $(".slidingDiv2").hide();
                $(".slidingDiv3").slideToggle();
            });

            <%If hidIsPersonsChange.Value = "1" Then%>
            $('.show_hide2').click();
            document.getElementById("<%=hidIsPersonsChange.ClientID%>").value = "";
            <%End if%>
            <%If hidIsProjectsChange.Value = "1" Then%>
            $('.show_hide3').click();
            document.getElementById("<%=hidIsProjectsChange.ClientID%>").value = "";
            <%End if%>


            var h1 = new Number;
            var h2 = new Number;
            var hh = new Number;

            h1 = $(window).height();

            ss = self.document.getElementById("offsetY");
            var offset = $(ss).offset();

            h2 = offset.top;

            hh = h1 - h2 - 50;
            self.document.getElementById("divTimeline").style.height = hh + "px";

        });





        function getSelected() {

            var ss = $(".ui-selected[isl='1']");

            var i = 0;
            var s = '';
            ss.each(function () {
                if (s == '') {
                    s = $(ss[i]).attr("val");

                }
                else {
                    s = s + ',' + $(ss[i]).attr("val");

                }
                i = i + 1;
            });
            return (s);
        }

        function getSelectedP48() {

            var ss = $(".ui-selected[isl='1']");

            var i = 0;
            var s = '';
            ss.each(function () {
                if (s == '') {
                    if ($(ss[i]).attr("p48ids") != null)
                        s = $(ss[i]).attr("p48ids");

                }
                else {
                    if ($(ss[i]).attr("p48ids") != null)
                        s = s + ',' + $(ss[i]).attr("p48ids");

                }
                i = i + 1;
            });

            return (s);
        }


        function entry() {

            var s = getSelected();
            if (s == "") {
                alert("Musíte označit alespoň jednu buňku (den).")
                return
            }

            var url = "p48_multiple_create.aspx?year=<%=Me.CurrentYear%>&month=<%=Me.CurrentMonth%>&input=" + s
            <%If Me.CurrentMasterPrefix = "p41" Then%>
            url = url + "&p41id=<%=Me.CurrentMasterPID%>";
            <%End If%>
            sw_master(url, "Images/oplan.png")

        }

        function seledit() {

            var s = getSelectedP48();

            if (s == "") {
                alert("Ve výběru není ani jeden uložený záznam plánu.")
                return
            }

            sw_master("p48_multiple_edit_delete.aspx?p48ids=" + s, "Images/oplan.png")

        }



        function hardrefresh(pid, flag) {
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefreshOnBehind, "", False)%>

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: white; padding: 10px;">
        <div style="float: left;">
            <img src="Images/oplan_32.png" />
            <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Text="Operativní plán"></asp:Label>
        </div>



        <div class="commandcell" style="padding-left: 20px;">
            <asp:DropDownList ID="query_year" runat="server" AutoPostBack="true"></asp:DropDownList>

            <asp:DropDownList ID="query_month" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Leden" Value="1"></asp:ListItem>
                <asp:ListItem Text="Únor" Value="2"></asp:ListItem>
                <asp:ListItem Text="Březen" Value="3"></asp:ListItem>
                <asp:ListItem Text="Duben" Value="4"></asp:ListItem>
                <asp:ListItem Text="Květen" Value="5"></asp:ListItem>
                <asp:ListItem Text="Červen" Value="6"></asp:ListItem>
                <asp:ListItem Text="Červenec" Value="7"></asp:ListItem>
                <asp:ListItem Text="Srpen" Value="8"></asp:ListItem>
                <asp:ListItem Text="Září" Value="9"></asp:ListItem>
                <asp:ListItem Text="Říjen" Value="10"></asp:ListItem>
                <asp:ListItem Text="Listopad" Value="11"></asp:ListItem>
                <asp:ListItem Text="Prosinec" Value="12"></asp:ListItem>
            </asp:DropDownList>


            <asp:ImageButton ID="cmdPrevMonth" runat="server" ImageUrl="Images/prevpage.png" ToolTip="Předchozí měsíc" />

            <asp:ImageButton ID="cmdNextMonth" runat="server" ImageUrl="Images/nextpage.png" ToolTip="Další měsíc" />
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <asp:DropDownList ID="cbxRozklad" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Podle osob a projektů" Value="1" Selected="true"></asp:ListItem>
                <asp:ListItem Text="Podle osob" Value="2"></asp:ListItem>
                <asp:ListItem Text="Podle projektů a osob" Value="3"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="commandcell" style="padding-left: 10px;">
            <button type="button" onclick="entry()">
                <img src="Images/new.png" />Vytvořit plány do označených dnů</button>

            <button type="button" onclick="seledit()">
                <img src="Images/edit.png" />Označené upravit</button>

            <button type="button" onclick="seledit()">
                <img src="Images/delete.png" />Označené odstranit</button>
        </div>
        <div class="show_hide1" style="float: left; margin-top: 10px;">
            <button type="button">
                <img src="Images/arrow_down_menu.png" />Nastavení</button>
        </div>

        <div style="clear: both; width: 100%;"></div>

        <div class="slidingDiv1">
            <div class="div6">
                <asp:CheckBox ID="chkIncludeWeekend" runat="server" Text="Možnost plánovat i do dnů víkendů a svátků" AutoPostBack="true" CssClass="chk" />
            </div>
            <div class="div6">
                <asp:CheckBox ID="chkShowWorksheet" runat="server" Text="Zobrazovat i sumy skutečné vykázaných hodin" AutoPostBack="true" CssClass="chk" />
            </div>

        </div>

        <div style="clear: both; width: 100%;"></div>

        <div class="show_hide2" style="float: left; margin-top: 8px;">
            <button type="button">
                <img src="Images/arrow_down.gif" alt="Výběr osob" />
                <asp:Label ID="PersonsHeader" runat="server"></asp:Label>

            </button>

        </div>
        <div class="show_hide3" style="float: left; margin-top: 8px;">
            <button type="button">
                <img src="Images/arrow_down.gif" alt="Výběr projektů" />               
                <asp:Label ID="ProjectsHeader" runat="server"></asp:Label>

            </button>

        </div>

        <div style="clear: both;"></div>
        <div class="slidingDiv2" style="background-color: khaki; padding-bottom: 20px;">
            <uc:persons ID="persons1" runat="server"></uc:persons>
        </div>

        
        <div style="clear: both;"></div>
        <div class="slidingDiv3" style="background-color: khaki; padding-bottom: 20px;">
            <uc:projects ID="projects1" runat="server"></uc:projects>
        </div>

        <asp:Panel ID="panLayout" runat="server">
            <table cellpadding="3">
                <tr>
                    <td class="nondate" style="width: 270px;">
                        <asp:HyperLink ID="MasterRecord" runat="server" Font-Bold="true" Target="_top"></asp:HyperLink>
                    </td>
                    <td class="nondate" style="width: 30px; text-align: right;" title="Kapacitní plán celkem">
                        <img src="Images/plan.png" />
                    </td>
                    <td class="nondate" style="width: 50px; text-align: right;" title="Operativní plán celkem">
                        <img src="Images/oplan.png" />
                    </td>
                    <td class="nondate" style="width: 30px;"></td>


                    <td id="tdd1" runat="server">
                        <asp:Label ID="d1" runat="server" Text="1"></asp:Label></td>
                    <td id="tdd2" runat="server">
                        <asp:Label ID="d2" runat="server" Text="2"></asp:Label></td>
                    <td id="tdd3" runat="server">
                        <asp:Label ID="d3" runat="server" Text="3"></asp:Label></td>
                    <td id="tdd4" runat="server">
                        <asp:Label ID="d4" runat="server" Text="4"></asp:Label></td>
                    <td id="tdd5" runat="server">
                        <asp:Label ID="d5" runat="server" Text="5"></asp:Label></td>
                    <td id="tdd6" runat="server">
                        <asp:Label ID="d6" runat="server" Text="6"></asp:Label></td>
                    <td id="tdd7" runat="server">
                        <asp:Label ID="d7" runat="server" Text="7"></asp:Label></td>
                    <td id="tdd8" runat="server">
                        <asp:Label ID="d8" runat="server" Text="8"></asp:Label></td>
                    <td id="tdd9" runat="server">
                        <asp:Label ID="d9" runat="server" Text="9"></asp:Label></td>
                    <td id="tdd10" runat="server">
                        <asp:Label ID="d10" runat="server" Text="10"></asp:Label></td>
                    <td id="tdd11" runat="server">
                        <asp:Label ID="d11" runat="server" Text="11"></asp:Label></td>
                    <td id="tdd12" runat="server">
                        <asp:Label ID="d12" runat="server" Text="12"></asp:Label></td>
                    <td id="tdd13" runat="server">
                        <asp:Label ID="d13" runat="server" Text="13"></asp:Label></td>
                    <td id="tdd14" runat="server">
                        <asp:Label ID="d14" runat="server" Text="14"></asp:Label></td>
                    <td id="tdd15" runat="server">
                        <asp:Label ID="d15" runat="server" Text="15"></asp:Label></td>
                    <td id="tdd16" runat="server">
                        <asp:Label ID="d16" runat="server" Text="16"></asp:Label></td>
                    <td id="tdd17" runat="server">
                        <asp:Label ID="d17" runat="server" Text="17"></asp:Label></td>
                    <td id="tdd18" runat="server">
                        <asp:Label ID="d18" runat="server" Text="18"></asp:Label></td>
                    <td id="tdd19" runat="server">
                        <asp:Label ID="d19" runat="server" Text="19"></asp:Label></td>
                    <td id="tdd20" runat="server">
                        <asp:Label ID="d20" runat="server" Text="20"></asp:Label></td>
                    <td id="tdd21" runat="server">
                        <asp:Label ID="d21" runat="server" Text="21"></asp:Label></td>
                    <td id="tdd22" runat="server">
                        <asp:Label ID="d22" runat="server" Text="22"></asp:Label></td>
                    <td id="tdd23" runat="server">
                        <asp:Label ID="d23" runat="server" Text="23"></asp:Label></td>
                    <td id="tdd24" runat="server">
                        <asp:Label ID="d24" runat="server" Text="24"></asp:Label></td>
                    <td id="tdd25" runat="server">
                        <asp:Label ID="d25" runat="server" Text="25"></asp:Label></td>
                    <td id="tdd26" runat="server">
                        <asp:Label ID="d26" runat="server" Text="26"></asp:Label></td>
                    <td id="tdd27" runat="server">
                        <asp:Label ID="d27" runat="server" Text="27"></asp:Label></td>
                    <td id="tdd28" runat="server">
                        <asp:Label ID="d28" runat="server" Text="28"></asp:Label></td>
                    <td id="tdd29" runat="server">
                        <asp:Label ID="d29" runat="server" Text="29"></asp:Label></td>
                    <td id="tdd30" runat="server">
                        <asp:Label ID="d30" runat="server" Text="30"></asp:Label></td>
                    <td id="tdd31" runat="server">
                        <asp:Label ID="d31" runat="server" Text="31"></asp:Label></td>

                    <td class="nondate" style="width: 30px; text-align: center;">
                        <img src="Images/worksheet.png" />
                    </td>
                </tr>
            </table>

            <div id="offsetY"></div>
            <div style="overflow: auto;" id="divTimeline">
                <table cellpadding="3" id="selectable">
                    <asp:Repeater ID="rp1" runat="server">
                        <ItemTemplate>
                            <tr style="border-top: dotted silver 1px; vertical-align: top;">
                                <td style="width: 270px;" class="nondate">
                                    <div>
                                        
                                        <asp:Label ID="person" runat="server" CssClass="valbold"></asp:Label>
                                        <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i"></asp:HyperLink>
                                    </div>
                                    <div>
                                        <asp:Label ID="project" runat="server" CssClass="val" Style="padding-left: 15px;"></asp:Label>
                                    </div>

                                    <asp:HiddenField ID="j02id" runat="server" />
                                    <asp:HiddenField ID="p41id" runat="server" />

                                </td>
                                <td class="nondate" style="width: 30px; text-align: right;">
                                    <asp:Label ID="capaplan" runat="server" CssClass="total" ToolTip="Kapacitní plán"></asp:Label>
                                </td>
                                <td class="nondate" style="width: 50px; text-align: right;">
                                    <asp:Label ID="fond" runat="server" CssClass="total"></asp:Label>
                                </td>
                                <td class="nondate" style="width: 30px; text-align: right;">
                                    <asp:Label ID="util" runat="server" CssClass="total"></asp:Label>
                                </td>

                                <td id="tdd1" runat="server"></td>
                                <td id="tdd2" runat="server"></td>
                                <td id="tdd3" runat="server"></td>
                                <td id="tdd4" runat="server"></td>
                                <td id="tdd5" runat="server"></td>
                                <td id="tdd6" runat="server"></td>
                                <td id="tdd7" runat="server"></td>
                                <td id="tdd8" runat="server"></td>
                                <td id="tdd9" runat="server"></td>
                                <td id="tdd10" runat="server"></td>
                                <td id="tdd11" runat="server"></td>
                                <td id="tdd12" runat="server"></td>
                                <td id="tdd13" runat="server"></td>
                                <td id="tdd14" runat="server"></td>
                                <td id="tdd15" runat="server"></td>
                                <td id="tdd16" runat="server"></td>
                                <td id="tdd17" runat="server"></td>
                                <td id="tdd18" runat="server"></td>
                                <td id="tdd19" runat="server"></td>
                                <td id="tdd20" runat="server"></td>
                                <td id="tdd21" runat="server"></td>
                                <td id="tdd22" runat="server"></td>
                                <td id="tdd23" runat="server"></td>
                                <td id="tdd24" runat="server"></td>
                                <td id="tdd25" runat="server"></td>
                                <td id="tdd26" runat="server"></td>
                                <td id="tdd27" runat="server"></td>
                                <td id="tdd28" runat="server"></td>
                                <td id="tdd29" runat="server"></td>
                                <td id="tdd30" runat="server"></td>
                                <td id="tdd31" runat="server"></td>

                                <td class="nondate" style="width: 30px; text-align: right;">
                                    <asp:Label ID="worksheet" runat="server" CssClass="total"></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>




        </asp:Panel>



    </div>

    <asp:Button ID="cmdHardRefreshOnBehind" runat="server" Style="display: none;" />
    <asp:HiddenField ID="hidIsPersonsChange" runat="server" Value="" />
    <asp:HiddenField ID="hidIsProjectsChange" runat="server" Value="" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
</asp:Content>
