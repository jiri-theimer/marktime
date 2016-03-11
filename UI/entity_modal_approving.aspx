<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="entity_modal_approving.aspx.vb" Inherits="UI.entity_modal_approving" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="plugin_datatable" Src="~/plugin_datatable.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Plugins/Plugin.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();


            $('.show_hide1').click(function () {

                $(".slidingDiv1").slideToggle();
            });

        });

        function sw_local(url, img, is_maximize) {
            dialog_master(url, is_maximize);
        }


        function periodcombo_setting() {

            sw_local("periodcombo_setting.aspx", "Images/settings_32.png");
        }
        



        function hardrefresh(pid, flag) {


            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }

        function approve_all() {

            location.replace("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentPrefix%>&masterpid=<%=master.DataPID%>&masterpids=<%=Me.CurrentInputPIDs%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>");
        }
        function approve_j02(j02id) {

            location.replace("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentPrefix%>&masterpid=<%=master.DataPID%>&masterpids=<%=Me.CurrentInputPIDs%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>&prefix=j02&pid=" + j02id);
        }
        function approve_p34(p34id) {

            location.replace("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentPrefix%>&masterpid=<%=master.DataPID%>&masterpids=<%=Me.CurrentInputPIDs%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>&prefix=p34&pid=" + p34id);
        }
        function approve_p41(p34id) {

            location.replace("p31_approving_step1.aspx?masterprefix=<%=Me.CurrentPrefix%>&masterpid=<%=master.DataPID%>&masterpids=<%=Me.CurrentInputPIDs%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>&prefix=p41&pid=" + p41id)
        }
        function reapprove_all() {

            location.replace("p31_approving_step1.aspx?reapprove=1&masterprefix=<%=Me.CurrentPrefix%>&masterpid=<%=master.DataPID%>&masterpids=<%=Me.CurrentInputPIDs%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>");
        }
        function clearapprove_all() {

            location.replace("p31_approving_step1.aspx?clearapprove=1&masterprefix=<%=Me.CurrentPrefix%>&masterpid=<%=master.DataPID%>&masterpids=<%=Me.CurrentInputPIDs%>&datefrom=<%=Format(period1.DateFrom, "dd.MM.yyyy")%>&dateuntil=<%=Format(period1.DateUntil,"dd.MM.yyyy")%>");
        }



        function invoice() {

            location.replace("p91_create_step1.aspx?nogateway=1&prefix=<%=Me.CurrentPrefix%>&pid=<%=master.DataPID%>&period=<%=Me.period1.SelectedValue%>");
        }
        function invoice_append() {

            location.replace("p91_add_worksheet_gateway.aspx?<%=Me.CurrentPrefix%>id=<%=master.DataPID%>&period=<%=Me.period1.SelectedValue%>");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="slidingDiv1" style="padding: 10px;">
        <fieldset style="padding: 10px;">
            <legend>Rozlišovat měny</legend>
            <asp:CheckBoxList ID="j27ids" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                <asp:ListItem Value="2" Text="CZK" Selected="true"></asp:ListItem>
                <asp:ListItem Value="3" Text="EUR" Selected="true"></asp:ListItem>
                <asp:ListItem Value="58" Text="USD"></asp:ListItem>
                <asp:ListItem Value="59" Text="GBP"></asp:ListItem>
            </asp:CheckBoxList>
        </fieldset>
        <fieldset style="padding: 10px;">
            <legend>Tlačítka pro dílčí schvalování</legend>
            <asp:CheckBox ID="chkCommandsP34" runat="server" Text="Za sešity" AutoPostBack="true" Checked="true" />
            <asp:CheckBox ID="chkCommandsJ02" runat="server" Text="Za osoby" AutoPostBack="true" />
            <asp:CheckBox ID="chkCommandsP41" runat="server" Text="Za projekty" AutoPostBack="true" Visible="false" />
        </fieldset>
    </div>


    <table cellpadding="10">
        <tr>
           

            <td>
                <span>Filtrovat období:</span>
            </td>
            <td>
                <uc:periodcombo ID="period1" runat="server" Width="220px"></uc:periodcombo>
            </td>
           
        </tr>
    </table>
    <div class="content-box2" style="padding-top: 10px;">
        <div class="title">
            <img src="Images/approve.png" />
            Rozpracované úkony - čeká na schvalování
        </div>
        <div class="content">
            <table>
                <tr valign="bottom">
                    <td>
                        <span>Agregace dat:</span>

                        <asp:DropDownList ID="col1" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="p28"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                            <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="p34"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="p32"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Den" Value="day"></asp:ListItem>
                        </asp:DropDownList>
                        +
                        <asp:DropDownList ID="col2" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="p28"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                            <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="p34"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="p32"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Den" Value="day"></asp:ListItem>
                        </asp:DropDownList>
                        +
                        <asp:DropDownList ID="col3" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="p28"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                            <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="p34"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="p32"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Den" Value="day"></asp:ListItem>
                        </asp:DropDownList>

                    </td>

                </tr>
            </table>
        </div>
    </div>

    <table>
        <tr valign="top">
            <td>
                <uc:plugin_datatable ID="plugin1" TableID="gridData" runat="server" ColHeaders="" ColHideRepeatedValues="1" ColTypes="" ColFlexSubtotals="" TableCaption="Worksheet rozpracovanost" NoDataMessage="Ani jeden rozpracovaný úkon." />

            </td>
            <td>
                <asp:Repeater ID="rpCommandAll" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <button type="button" style="width: 170px; text-align: left; font-weight: bold;" onclick="approve_all()">
                                <img src="Images/approve.png" />
                                Schvalovat [vše] <%#Eval("RowCount")%>x</button>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rpCommandP34" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <button type="button" style="min-width: 170px; text-align: left;" onclick="approve_p34(<%#Eval("PID")%>)">
                                <img src="Images/approve.png" />
                                [<%#Eval("Name")%>] <%#Eval("RowCount")%>x</button>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rpCommandJ02" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <button type="button" style="min-width: 170px; text-align: left;" onclick="approve_j02(<%#Eval("PID")%>)">
                                <img src="Images/approve.png" />
                                [<%#Eval("Name")%>] <%#Eval("RowCount")%>x</button>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="rpCommandP28" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <button type="button" style="min-width: 170px; max-width: 250px; text-align: left;" onclick="approve_p28(<%#Eval("PID")%>)">
                                <img src="Images/approve.png" />
                                [<%#Eval("Name")%>] <%#Eval("RowCount")%>x</button>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>



    <div class="content-box2" style="padding-top: 10px;">
        <div class="title" style="padding-bottom:10px;">
            <img src="Images/invoice.png" />
            Schválené úkony - čeká na fakturaci

            <button type="button" id="cmdReApprove" runat="server" onclick="reapprove_all()">
                <img src="Images/approve.png" />Pře-schválit již schválené úkony</button>
            <button type="button" id="cmdClearApprove" runat="server" onclick="clearapprove_all()">
                <img src="Images/break.png" />Vyčistit schvalování</button>
            

            <button type="button" runat="server" id="cmdCreateP91" onclick="invoice()">
                    <img src="Images/invoice.png" />Vystavit fakturu</button>

            <button type="button" runat="server" id="cmdAppendP91" onclick="invoice_append()">
                    <img src="Images/invoice.png" />Přidat do existující faktury</button>

            <a href="#top" style="margin-left: 300px; padding: 5px;">Nahoru</a>
        </div>
        <div class="content">
            <table>
                <tr valign="bottom">
                    <td>
                        <span>Agregace dat:</span>

                        <asp:DropDownList ID="col1x" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Billing dávka" Value="p31ApprovingSet"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="p28"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                            <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="p34"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="p32"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Den" Value="day"></asp:ListItem>
                        </asp:DropDownList>
                        +
                        <asp:DropDownList ID="col2x" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Billing dávka" Value="p31ApprovingSet"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="p28"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                            <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="p34"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="p32"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Den" Value="day"></asp:ListItem>
                        </asp:DropDownList>
                        +
                        <asp:DropDownList ID="col3x" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Billing dávka" Value="p31ApprovingSet"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="p28"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="p41"></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="j02"></asp:ListItem>
                            <asp:ListItem Text="Úkol" Value="p56"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="p34"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="p32"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Den" Value="day"></asp:ListItem>
                        </asp:DropDownList>

                    </td>

                </tr>
            </table>

            
            <uc:plugin_datatable ID="plugin2" TableID="gridApproved" runat="server" ColHeaders="" ColHideRepeatedValues="1" ColTypes="" ColFlexSubtotals="" TableCaption="Prošlo schvalováním, čeká na fakturaci" NoDataMessage="Žádný schválený worksheet čekající na fakturaci." />
        </div>
    </div>


    <asp:HiddenField ID="hidX29ID" runat="server" Value="141" />
    <asp:HiddenField ID="hidPrefix" runat="server" Value="p41" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidInputPIDS" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
