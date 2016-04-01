﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p31_record.aspx.vb" Inherits="UI.p31_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields" Src="~/freefields.ascx" %>
<%@ Register TagPrefix="uc" TagName="o23_list" Src="~/o23_list.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Scripts/jqueryui/jquery-ui.min.css" />
    <style type="text/css">
        .ui-autocomplete {
     max-height: 300px;
     overflow-y: auto;
     width:80px;
     /* prevent horizontal scrollbar */
     overflow-x: hidden;
 }
    </style>
    <script src="Scripts/jqueryui/jquery-ui.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {            
            var hours_interval = [
            <%=ViewState("hours_offer")%>
            ];

           

            $("#<%=p31Value_Orig.ClientID%>").autocomplete({
                source: hours_interval,
                minLength: 0,
                scroll: true,
                change: function (event, ui) {
                    handle_hours();                    
                    
                }
            }).focus(function () {                
                $(this).autocomplete("search", "")
                $(this).select();
            });



            var time_interval = [
      <%=ViewState("times_offer")%>
            ];



            $("#<%=Me.TimeFrom.ClientID%>").autocomplete({
                source: time_interval,
                minLength: 0,
                scroll: true,
                change: function (event, ui) {
                    
                    recalcduration();
                    
                }
            }).focus(function () {
                $(this).autocomplete("search", "")
                $(this).select();
            });

            $("#<%=Me.TimeUntil.ClientID%>").autocomplete({
                source: time_interval,
                minLength: 0,
                scroll: true,
                change: function (event, ui) {
                    recalcduration();
                }
            }).focus(function () {
                $(this).autocomplete("search", "")
                $(this).select();
            });
            
        });

        


        function RecalcWithVat() {
            <%If Not Me.p31Amount_WithVat_Orig.Visible Then%>
            return;
            <%End If%>

            var vatrate = new Number;
            var withoutvat = new Number;
            var withvat = new Number;
            var vat = new Number;

            withoutvat = $find("<%= p31Amount_WithoutVat_Orig.ClientID%>").get_value();

            var ss = $find("<%= p31VatRate_Orig.RadComboClientID%>").get_text();
            vatrate = ss.replace(",", ".");

            vat = withoutvat * vatrate / 100;
            withvat = vat + withoutvat;

            $find("<%= p31Amount_WithVat_Orig.ClientID%>").set_value(withvat);
            $find("<%= p31Amount_Vat_Orig.ClientID%>").set_value(vat);
        }

        function RecalcAmount_ByPieces() {
            var vatrate = new Number;
            var withoutvat = new Number;
            var withvat = new Number;
            var vat = new Number;

            pieces_count = $find("<%= p31Calc_Pieces.ClientID%>").get_value();
            pieces_amount = $find("<%= p31Calc_PieceAmount.ClientID%>").get_value();
            withoutvat = pieces_count * pieces_amount;

            $find("<%= p31Amount_WithoutVat_Orig.ClientID%>").set_value(withoutvat);

            <%If Not Me.p31Amount_WithVat_Orig.Visible Then%>
            return;
            <%End If%>

            var ss = $find("<%= p31VatRate_Orig.RadComboClientID%>").get_text();
            vatrate = ss.replace(",", ".");

            vat = withoutvat * vatrate / 100;
            withvat = vat + withoutvat;

            $find("<%= p31Amount_WithVat_Orig.ClientID%>").set_value(withvat);
            $find("<%= p31Amount_Vat_Orig.ClientID%>").set_value(vat);
        }

        function p31Amount_WithoutVat_Orig_OnChanged(sender, eventArgs) {
            RecalcWithVat();
        }

        function p31VatRate_Orig_OnChange() {

            RecalcWithVat();
        }


        function p31VatRate_Orig_OnChange(sender, eventArgs) {
            RecalcWithVat();
        }

        function Calc_OnChanged(sender, eventArgs) {
            RecalcAmount_ByPieces();
        }



        function p32id_OnClientSelectedIndexChanged(sender, eventArgs) {
            var item = eventArgs.get_item();
            var p32id = item.get_value();
            var p41id_pid = "<%=p41id.value%>";

            $.post("Handler/handler_activity.ashx", { pid: p32id, p41id: p41id_pid, oper: "profile" }, function (data) {

                if (data == ' ') {
                    return;
                }

                var s = data.split("|");

                <%If panT.Visible Then%>
                if (s[0] != "" && self.document.getElementById("<%=p31Value_Orig.ClientID%>").value == "") {
                    self.document.getElementById("<%=p31Value_Orig.ClientID%>").value = s[0];
                }
                <%End If%>
                <%If panU.Visible Then%>
                var ctl = $find("<%= Units_Orig.ClientID%>");
                if (s[0] != "" && ctl.get_value() == "") {
                    ctl.set_value(s[0]);
                }

                <%End If%>
                if (s[1] != "") {
                    if (self.document.getElementById("<%=p31text.ClientID%>").value == "")
                        self.document.getElementById("<%=p31text.ClientID%>").value = s[1];
                    else {
                        <%If Master.DataPID = 0 Then%>
                        self.document.getElementById("<%=p31text.ClientID%>").value = s[1] + "\n\r" + self.document.getElementById("<%=p31text.ClientID%>").value;
                        <%End If%>
                        //nebo nic 
                    }
                }

                if (s[2] == "1")
                    document.getElementById("<%=Me.lblP31Text.ClientID%>").className = "lblReq";
                else
                    document.getElementById("<%=Me.lblP31Text.ClientID%>").className = "lbl";








            });
        }

        function recalcduration() {
            var t1 = self.document.getElementById("<%=Me.TimeFrom.ClientID%>").value;
            var t2 = self.document.getElementById("<%=Me.TimeUntil.ClientID%>").value;

            $.post("Handler/handler_time.ashx", { t1: t1, t2: t2, oper: "duration" }, function (data) {
                if (data == ' ') {
                    return;
                }

                var s = data.split("|");


                if (s.length <= 1) {
                    document.getElementById("<%=Me.HandlerMessage.ClientID%>").innerHTML = data;
                    return
                }


                self.document.getElementById("<%=Me.TimeFrom.ClientID%>").value = s[0];
                self.document.getElementById("<%=Me.TimeUntil.ClientID%>").value = s[1];

                self.document.getElementById("<%=p31Value_Orig.ClientID%>").value = s[2];
                document.getElementById("<%=Me.HandlerMessage.ClientID%>").innerHTML = s[3];

            });
        }

        function handle_hours() {

            var h = document.getElementById("<%=Me.p31Value_Orig.ClientID%>").value;

            $.post("Handler/handler_time.ashx", { hours: h, oper: "hours" }, function (data) {
                if (data == ' ') {
                    return;
                }

                document.getElementById("<%=Me.HandlerMessage.ClientID%>").innerHTML = data;

            });
        }

        function handle_minutes() {

            var h = document.getElementById("<%=Me.p31Value_Orig.ClientID%>").value;

            $.post("Handler/handler_time.ashx", { hours: h, oper: "minutes" }, function (data) {
                if (data == ' ') {
                    return;
                }

                document.getElementById("<%=Me.HandlerMessage.ClientID%>").innerHTML = data;

            });
        }

        function setting() {
            var p33id = document.getElementById("<%=hidP33ID.ClientID%>").value;
            var hoursentryflag = document.getElementById("<%=me.hidHoursEntryFlag.ClientID%>").value;

            dialog_master("p31_setting.aspx?p33id=" + p33id + "&hoursentryflag=" + hoursentryflag);


        }

        function hardrefresh(pid, flag, par1) {
            document.getElementById("<%=Me.HardRefreshFlag.ClientID%>").value = flag;
            document.getElementById("<%=HardRefreshPID.ClientID%>").value = pid;

            if (par1 != null) {

                document.getElementById("<%=me.hidHoursEntryFlag.ClientID%>").value = par1;
            }

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefresh, "", False)%>

        }

        function sw_local(url, img, is_maximize)  //je to zde kvůli zapisování komentářů přes info-bublinu
        {
            dialog_master(url, is_maximize);
        }

        function o23_create(guid) {
            dialog_master("o23_record.aspx?masterprefix=p31&masterpid=<%=master.datapid%>&masterguid=" + guid, true);
        }
        function o23_record(pid) {
            dialog_master("o23_record.aspx?masterprefix=p31&pid=" + pid, true);
        }
        function p49_bind() {
            var p41id = "<%=me.p41ID.value%>";
            var p34id = "<%=Me.p34ID.SelectedValue%>";
            dialog_master("p49_bind.aspx?p34id="+p34id+"&p41id="+p41id, true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="5" cellspacing="2" id="responsive">
        <tr>
            <td style="width: 120px;">
                <asp:Label ID="lblJ02ID" runat="server" Text="Osoba:" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:person ID="j02ID" runat="server" Width="400px" AutoPostBack="true" Flag="p31_entry" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblP41ID" runat="server" Text="Projekt:" CssClass="lblReq"></asp:Label>
                <asp:HyperLink ID="clue_project" runat="server" CssClass="reczoom" Text="i" title="Detail projektu" Visible="false"></asp:HyperLink>
            </td>
            <td>
                <uc:project ID="p41ID" runat="server" Width="400px" AutoPostBack="true" Flag="p31_entry" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblP34ID" runat="server" Text="Sešit:" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="p34ID" runat="server" Width="400px" DataTextField="p34Name" DataValueField="pid" IsFirstEmptyRow="true" AutoPostBack="true" Filter="Contains" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblP32ID" runat="server" Text="Aktivita:" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="p32ID" runat="server" Width="400px" DataTextField="p32Name" DataValueField="pid" IsFirstEmptyRow="true" Filter="Contains" OnClientSelectedIndexChanged="p32id_OnClientSelectedIndexChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDate" runat="server" Text="Datum:" CssClass="lblReq"></asp:Label>
            </td>
            <td>
                <telerik:RadDatePicker ID="p31Date" runat="server" Width="120px" SharedCalendarID="SharedCalendar" DateInput-EmptyMessage="Povinný údaj." DateInput-EmptyMessageStyle-ForeColor="red">
                    <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                </telerik:RadDatePicker>
                <asp:Label ID="MultiDateInput" runat="server" Visible="false" ForeColor="blue"></asp:Label>
            </td>
        </tr>

    </table>
    
    <asp:Panel ID="panT" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="2">
            <tr>
                <td style="width: 120px;">
                    <asp:Label ID="lblHours" runat="server" Text="Hodiny:" CssClass="lblReq"></asp:Label>
                </td>
                <td>
                    
                    <asp:TextBox ID="p31Value_Orig" runat="server" Style="width: 50px;" onchange="handle_hours()"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblTimeFrom" runat="server" Text="Začátek:" CssClass="lbl" Visible="false"></asp:Label>
                </td>
                <td>

                    
                    <asp:TextBox ID="TimeFrom" runat="server" Style="width: 50px;"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblTimeUntil" runat="server" Text="Konec:" CssClass="lbl" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TimeUntil" runat="server" Style="width: 50px;"></asp:TextBox>
                    
                </td>
                <td>
                    <asp:Label ID="HandlerMessage" runat="server" Style="color: navy; font-size: 90%;"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panU" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="2">
            <tr>
                <td style="width: 120px;">
                    <asp:Label ID="lblUnits" runat="server" Text="Počet:" CssClass="lblReq"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="Units_Orig" runat="server" Width="70px" NumberFormat-ZeroPattern="n"></telerik:RadNumericTextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panM" runat="server" Visible="false">
        <table cellpadding="5" cellspacing="2">
            <tr>
                <td style="width: 120px;">
                    <asp:Label ID="lblp31Amount_WithoutVat_Orig" runat="server" Text="Částka bez DPH:" CssClass="lblReq"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="p31Amount_WithoutVat_Orig" runat="server" Width="100px" NumberFormat-ZeroPattern="n">
                        <ClientEvents OnValueChanged="p31Amount_WithoutVat_Orig_OnChanged" />
                    </telerik:RadNumericTextBox>
                </td>
                <td>
                    <uc:datacombo ID="j27ID_Orig" Width="60px" runat="server" DataTextField="j27Code" DataValueField="pid" AutoPostBack="true"></uc:datacombo>
                </td>
                <td>
                    <asp:Label ID="lblp31VatRate_Orig" runat="server" Text="Sazba DPH (%):" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <uc:datacombo ID="p31VatRate_Orig" Width="60px" runat="server" AllowCustomText="true" Filter="StartsWith" OnClientTextChange="p31VatRate_Orig_OnChange" OnClientSelectedIndexChanged="p31VatRate_Orig_OnChange" DataValueField="pid" DataTextField="p53Value"></uc:datacombo>
                </td>

            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblp31Amount_WithVat_Orig" runat="server" Text="Částka vč. DPH:" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="p31Amount_WithVat_Orig" runat="server" Width="100px" NumberFormat-ZeroPattern="n"></telerik:RadNumericTextBox>
                </td>
                <td>
                    <asp:Label ID="lblp31Amount_Vat_Orig" runat="server" Text="Částka DPH:" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="p31Amount_Vat_Orig" runat="server" Width="100px" NumberFormat-ZeroPattern="n"></telerik:RadNumericTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblp31Calc_Pieces" runat="server" Text="Počet:" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="p31Calc_Pieces" runat="server" Width="50px" NumberFormat-ZeroPattern="n">
                        <ClientEvents OnValueChanged="Calc_OnChanged" />
                    </telerik:RadNumericTextBox>
                    <uc:datacombo ID="p35ID" Width="60px" runat="server" AllowCustomText="false" Filter="StartsWith" DataValueField="pid" DataTextField="p35Code" IsFirstEmptyRow="true"></uc:datacombo>
                </td>
                <td>
                    <asp:Label ID="lblp31Calc_PieceAmount" runat="server" Text="Cena 1 ks:" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="p31Calc_PieceAmount" runat="server" Width="100px" NumberFormat-ZeroPattern="n">
                        <ClientEvents OnValueChanged="Calc_OnChanged" />
                    </telerik:RadNumericTextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSupplier" runat="server" CssClass="lbl" Text="Dodavatel:"></asp:Label>
                </td>
                <td colspan="4">
                    <uc:contact ID="p28ID_Supplier" runat="server" Width="250px" Flag="supplier" />
                    <asp:Label ID="lblCode" runat="server" CssClass="lbl" Text="Kód dokladu:"></asp:Label>
                    <asp:TextBox ID="p31Code" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="div6">
        <div>
            <asp:Label ID="lblP31Text" runat="server" Text="Podrobný popis úkonu:" CssClass="lbl"></asp:Label>
            <asp:Image ID="imgFlag" runat="server" />
        </div>
        <asp:TextBox ID="p31Text" runat="server" Style="height: 90px; width: 99%;" TextMode="MultiLine"></asp:TextBox>
        <uc:freefields ID="ff1" runat="server" />
    </div>
    <div class="content-box1" style="min-width:50px;">
        <div class="title">
            <img src="Images/task.png" alt="Úkol" />
            <asp:CheckBox ID="chkBindToP56" runat="server" Text="Projektový úkol" AutoPostBack="true" />
        </div>
        <div class="content">
            <asp:DropDownList ID="p56ID" runat="server" DataTextField="NameWithTypeAndCode" DataValueField="pid" Visible="false" ></asp:DropDownList>
            
        </div>

    </div>
    <div class="content-box1" style="min-width:50px;">
        <div class="title">
            <img src="Images/contactperson.png" alt="Kontaktní osoba" />
            <asp:CheckBox ID="chkBindToContactPerson" runat="server" Text="Kontaktní osoba" AutoPostBack="true" />
        </div>
        <div class="content">
            <asp:DropDownList ID="j02ID_ContactPerson" runat="server" Visible="false" DataValueField="pid" DataTextField="FullNameDescWithEmail"></asp:DropDownList>
        </div>
    </div>
    <div class="content-box1" style="min-width:270px;">
        <div class="title">
            <img src="Images/notepad.png" alt="Dokument" />Dokumenty
            <asp:HyperLink ID="cmdDoc" runat="server" Text="Nahrát/Spárovat dokument" NavigateUrl="javascript:o23_record()"></asp:HyperLink>
        </div>
        <div class="content">
            <uc:o23_list ID="notepad1" runat="server" EntityX29ID="p31Worksheet"></uc:o23_list>
        </div>
    </div>
    <asp:panel ID="panP49" runat="server" cssclass="content-box1" style="min-width:270px;" Visible="false">
        <div class="title">
            <img src="Images/finplan.png" alt="Rozpočet" />rozpočet
            <asp:HyperLink ID="cmdP49" runat="server" Text="Spárovat" NavigateUrl="javascript:p49_bind()"></asp:HyperLink>
        </div>
        <div class="content">
            <asp:Label ID="p49_record" runat="server" CssClass="valboldblue"></asp:Label>           
            <asp:Button ID="cmdClearP49ID" runat="server" Text="Vyčistit vazbu na rozpočet" CssClass="cmd" />
            <asp:HiddenField ID="p49ID" runat="server" />
        </div>
    </asp:panel>
    <div style="clear: both;"></div>
    <div class="div6">

        <a href="javascript:setting()" style="text-align: right; float: right;">Nastavení</a>
    </div>



    <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ShowRowHeaders="false">

        <SpecialDays>
            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
        </SpecialDays>
    </telerik:RadCalendar>


    <asp:HiddenField ID="HardRefreshPID" runat="server" />
    <asp:HiddenField ID="HardRefreshFlag" runat="server" />
    <asp:Button ID="cmdHardRefresh" runat="server" Style="display: none;" />
    <asp:HiddenField ID="hidP33ID" runat="server" />
    <asp:HiddenField ID="hidHoursEntryFlag" runat="server" Value="1" />
    <asp:HiddenField ID="hidP91ID" runat="server" />
    <asp:HiddenField ID="hidP48ID" runat="server" />
    <asp:HiddenField ID="hidP85ID" runat="server" />
    <asp:HiddenField ID="hidP61ID" runat="server" />
    <asp:HiddenField ID="hidDocGUID" runat="server" />
    <asp:HiddenField ID="p31_default_HoursEntryFlag" runat="server" />
    <asp:HiddenField ID="hidCurIsScheduler" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
