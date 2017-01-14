<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="j02_record.aspx.vb" Inherits="UI.j02_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields" Src="~/freefields.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <link rel="stylesheet" href="Scripts/jqueryui/jquery-ui.min.css" />
    <script src="Scripts/jqueryui/jquery-ui.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hidGUID" runat="server" />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
        <Tabs>
            <telerik:RadTab Text="<%$ Resources:common, vlastnosti %>" Selected="true" Value="core"></telerik:RadTab>
            <telerik:RadTab Text="<%$ Resources:common, uzivatelska_pole %>" Value="ff"></telerik:RadTab>
            <telerik:RadTab Text="SMTP účet" Value="smtp" meta:resourcekey="RadTabStrip1_smpt"></telerik:RadTab>
            <telerik:RadTab Text="<%$ Resources:common, ostatni %>" Value="other"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="core" runat="server" Selected="true">

            <div class="content-box2">
                <div class="title">
                    Typ osobního profilu
                    <asp:CheckBox ID="chkWhisper" runat="server" Text="Zapnutý našeptávač podobných osob" AutoPostBack="true" Checked="true" style="float:right;" />
                </div>
                <div class="content">
                    <asp:RadioButtonList ID="j02IsIntraPerson" runat="server" AutoPostBack="true" RepeatDirection="Vertical">
                        <asp:ListItem Text="<%$ Resources:j02_record, j02IsIntraPerson_1 %>" Value="1" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:j02_record, j02IsIntraPerson_0 %>" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>

                </div>
            </div>
            <div class="div6">
            </div>

            <table cellpadding="5" cellspacing="2">
                <tr>
                    <td>
                        <asp:Label ID="Label1" Text="Titul+Jméno+Příjmení:" runat="server" CssClass="lblReq"></asp:Label>
                    </td>
                    <td>
                        <uc:datacombo ID="j02TitleBeforeName" runat="server" Width="80px" AllowCustomText="true" ShowToggleImage="false" Filter="Contains" DefaultValues="Bc.;BcA.;Ing.;Ing.arch.;MUDr.;MVDr.;MgA.;Mgr.;JUDr.;PhDr.;RNDr.;PharmDr.;ThLic.;ThDr.;Ph.D.;Th.D.;prof.;doc.;PaedDr.;Dr.;PhMr."></uc:datacombo>
                        <asp:Label ID="lblFirstName" Text="Jméno:" runat="server" CssClass="lblReq" AssociatedControlID="j02FirstName" meta:resourcekey="lblFirstName"></asp:Label>
                        <asp:TextBox ID="j02FirstName" runat="server" Style="width: 100px;"></asp:TextBox>
                        <asp:Label ID="lblLastName" Text="Příjmení:" runat="server" CssClass="lblReq" AssociatedControlID="j02LastName" meta:resourcekey="lblLastName"></asp:Label>
                        <asp:TextBox ID="j02LastName" runat="server" Style="width: 160px;"></asp:TextBox>
                        <uc:datacombo ID="j02TitleAfterName" runat="server" Width="70px" AllowCustomText="true" ShowToggleImage="false" Filter="Contains" DefaultValues="CSc.;DrSc.;dr. h. c.;DiS."></uc:datacombo>
                        
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;">
                        <asp:Label ID="lblj02Email" Text="E-mail adresa:" runat="server" CssClass="lblReq" AssociatedControlID="j02Email" meta:resourcekey="lblj02Email"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02Email" runat="server" Style="width: 300px;"></asp:TextBox>
                        


                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblj02Code" Text="Kód/osobní číslo:" runat="server" CssClass="lbl" AssociatedControlID="j02Code" meta:resourcekey="lblj02Code"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02Code" runat="server" Style="width: 300px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="search_dupl_result" style="position:relative;left:0px;top:0px;z-index:1000px;"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblJ07ID" Text="Pozice (hladina sazby):" runat="server" CssClass="lbl" meta:resourcekey="lblJ07ID"></asp:Label></td>
                    <td>
                        <uc:datacombo ID="j07ID" runat="server" DataTextField="j07Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblC21ID" Text="Pracovní fond:" runat="server" CssClass="lbl" meta:resourcekey="lblC21ID"></asp:Label></td>
                    <td>
                        <uc:datacombo ID="c21ID" runat="server" DataTextField="c21Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblJ18ID" runat="server" CssClass="lbl" Text="Středisko:" meta:resourcekey="lblJ18ID"></asp:Label></td>
                    <td>

                        <uc:datacombo ID="j18ID" runat="server" DataTextField="j18Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
                    </td>
                </tr>

                <tr valign="top" id="trJ17ID" runat="server">
                    <td>
                        <asp:Label ID="lblJ17ID" Text="Region:" runat="server" CssClass="lbl"></asp:Label></td>
                    <td>
                        <uc:datacombo ID="j17ID" runat="server" DataTextField="j17Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
                        <span class="infoInForm">Vazba na [Region] se využívá kvůli zohlednění dnů svátků pro pracovní fondy osob z různých zemí (regionů).</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMobile" Text="TEL1 (mobil):" runat="server" CssClass="lbl" AssociatedControlID="j02Mobile"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02Mobile" runat="server" Style="width: 200px;"></asp:TextBox>
                        <asp:Label ID="lblj02Phone" Text="TEL2 (pevný):" runat="server" CssClass="lbl" AssociatedControlID="j02Phone" meta:resourcekey="lblj02Phone"></asp:Label>
                        <asp:TextBox ID="j02Phone" runat="server" Style="width: 200px;"></asp:TextBox>
                    </td>
                </tr>

                <tr id="trJobTitle" runat="server">
                    <td>
                        <asp:Label ID="lblj02JobTitle" Text="Pozice:" runat="server" CssClass="lbl" AssociatedControlID="j02JobTitle" meta:resourcekey="lblj02JobTitle"></asp:Label>
                    </td>
                    <td>
                        <uc:datacombo ID="j02JobTitle" runat="server" Width="300px" AllowCustomText="true" ShowToggleImage="false" Filter="Contains" DefaultValues="Ředitel;Jednatel;Manažer;Asistentka;Obchodní zástupce;Konzultant;Auditor;Daňový poradce;Analytik;Personální manažer;Finanční manažer;Obchodní manažer;Právník;Účetní;IT správce;Programátor;Technik"></uc:datacombo>
                        <span class="infoInForm">Můžete zadat i pozici, která není uvedena v seznamu.</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblj02Office" Text="Kancelář:" runat="server" CssClass="lbl" AssociatedControlID="j02Office" meta:resourcekey="lblj02Office"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02Office" runat="server" Style="width: 500px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblj02Salutation" Text="Oslovení pro korespondenci:" runat="server" CssClass="lbl" AssociatedControlID="j02Salutation"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02Salutation" runat="server" Style="width: 500px;"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <asp:Label ID="lblj02EmailSignature" Text="Podpis pro e-mail:" runat="server" CssClass="lbl" AssociatedControlID="j02EmailSignature" meta:resourcekey="lblj02EmailSignature"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02EmailSignature" runat="server" Style="width: 500px; height: 50px;" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>

            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="ff" runat="server">

            <uc:freefields ID="ff1" runat="server" />

        </telerik:RadPageView>
        <telerik:RadPageView ID="smtp" runat="server">
            <asp:CheckBox ID="chkIsSmtp" runat="server" Text="Odeslaná pošta osoby odchází z osobního SMTP účtu" AutoPostBack="true" meta:resourcekey="chkIsSmtp" />
            <asp:Panel ID="panSMTP" runat="server">
                <table cellpadding="5" cellspacing="2">
                    <tr>
                        <td>
                            <asp:Label ID="lblj02SmtpServer" runat="server" Text="Adresa SMTP serveru:" CssClass="lbl" meta:resourcekey="lblj02SmtpServer"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02SmtpServer" runat="server" Style="width: 300px;"></asp:TextBox>
                            <asp:CheckBox ID="j02IsSmtpVerify" runat="server" AutoPostBack="true" Text="Účet vyžaduje ověření" Checked="true" meta:resourcekey="j02IsSmtpVerify" />
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblj02SmtpLogin" runat="server" Text="SMTP login:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02SmtpLogin" runat="server" Style="width: 300px;"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblj02SmtpPassword" runat="server" Text="SMTP heslo:" CssClass="lbl" meta:resourcekey="lblj02SmtpPassword"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02SmtpPassword" runat="server" Style="width: 200px;" TextMode="Password"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblVerifyPassword" runat="server" Text="Ověření hesla:" CssClass="lbl" meta:resourcekey="lblVerifyPassword"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVerifyPassword" runat="server" Style="width: 200px;" TextMode="Password"></asp:TextBox>
                        </td>

                    </tr>
                </table>
            </asp:Panel>
        </telerik:RadPageView>
        <telerik:RadPageView ID="other" runat="server">
            <fieldset>
                <legend>Omezení přístupu k úkonům, které prošli fakturací</legend>
                <asp:DropDownList ID="j02WorksheetAccessFlag" runat="server">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="Nemá přístup k úkonům, které prošli fakturací (vč. vlastních úkonů)" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </fieldset>
            <fieldset>
                <legend>Omezení zpětně zapisovat hodiny</legend>

                <asp:Label ID="lblj02TimesheetEntryDaysBackLimit" runat="server" Text="Omezení zpětně zapisovat hodiny:" CssClass="lbl"></asp:Label>
                <asp:DropDownList ID="j02TimesheetEntryDaysBackLimit" runat="server">
                    <asp:ListItem Value="" Text="Bez omezení"></asp:ListItem>
                    <asp:ListItem Value="999" Text="Povolen pouze aktuální týden"></asp:ListItem>
                    <asp:ListItem Value="1" Text="-1 den"></asp:ListItem>
                    <asp:ListItem Value="2" Text="-2 dny"></asp:ListItem>
                    <asp:ListItem Value="3" Text="-3 dny"></asp:ListItem>
                    <asp:ListItem Value="4" Text="-4 dny"></asp:ListItem>
                    <asp:ListItem Value="5" Text="-5 dní"></asp:ListItem>
                    <asp:ListItem Value="6" Text="-6 dní"></asp:ListItem>
                    <asp:ListItem Value="7" Text="-7 dní"></asp:ListItem>
                    <asp:ListItem Value="8" Text="-8 dní"></asp:ListItem>
                    <asp:ListItem Value="9" Text="-9 dní"></asp:ListItem>
                    <asp:ListItem Value="10" Text="-10 dní"></asp:ListItem>
                    <asp:ListItem Value="14" Text="-14 dní"></asp:ListItem>
                    <asp:ListItem Value="20" Text="-20 dní"></asp:ListItem>
                    <asp:ListItem Value="30" Text="-30 dní"></asp:ListItem>
                </asp:DropDownList>
                <div style="margin-top: 10px;">
                    <label class="lbl">Výběr časových sešitů</label>
                    <uc:datacombo ID="j02TimesheetEntryDaysBackLimit_p34IDs" DataValueField="pid" DataTextField="p34Name" runat="server" AllowCheckboxes="true" Width="200px" />
                </div>
                <br />
                <span class="infoInForm">Počet dní, za které osoba může zpětně zapisovat časové úkony. Omezení se vztahuje na osobu zapisovače úkonu, nikoliv na osobu záznamu úkonu.</span>
            </fieldset>
            <table cellpadding="5" cellspacing="2">

                <tr>
                    <td>
                        <asp:Label ID="lblj02RobotAddress" runat="server" Text="Adresa pro IMAP robota:" CssClass="lbl" meta:resourcekey="lblj02RobotAddress"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02RobotAddress" runat="server" Style="width: 200px;"></asp:TextBox>
                    </td>
                    <td>
                        <span class="infoInForm">Adresa, podle které IMAP robot pozná, že načtená poštovní zpráva má vztah osobě.</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblj02ExternalPID" runat="server" Text="Externí kód:" CssClass="lbl" meta:resourcekey="lblj02ExternalPID"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02ExternalPID" runat="server" Style="width: 200px;"></asp:TextBox>
                    </td>
                    <td>
                        <span class="infoInForm">Klíč záznamu z externího IS pro integraci s MT.</span>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
    <script type="text/javascript">
        <%If Me.chkWhisper.Checked Then%>
        $(function () {

            $("#<%=Me.j02LastName.ClientID%>").autocomplete({
                source: "Handler/handler_search_person.ashx?fo=j02LastName",
                minLength: 1,
                select: function (event, ui) {
                    if (ui.item) {
                        if (ui.item.PID != null)
                        dialog_master("clue_j02_record.aspx?pid=" + ui.item.PID, false)
                        return false;
                    }
                },
                open: function (event, ui) {
                    $('ul.ui-autocomplete')
                       .removeAttr('style').hide()
                       .appendTo('#search_dupl_result').show();
                },
                close: function (event, ui) {
                    $('ul.ui-autocomplete')
                    .html("")
                    .hide();
                }



            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var s = "<div style='background-color:khaki;width:500px;'>";
                if (item.Closed == "1")
                    s = s + "<a style='text-decoration:line-through;'>";
                else
                    s = s + "<a>";

                s = s + __highlight(item.ItemText, item.FilterString);


                s = s + "</a>";

                if (item.Italic == "1")
                    s = "<i>" + s + "</i>"

                s = s + "</div>";


                return $(s).appendTo(ul);


            };
        });

        
        
       

        function __highlight(s, t) {
            var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
            return s.replace(matcher, "<strong>$1</strong>");
        }


       <%end if%>
    </script>
</asp:Content>
