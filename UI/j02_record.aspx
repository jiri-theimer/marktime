<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="j02_record.aspx.vb" Inherits="UI.j02_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields" Src="~/freefields.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
        <Tabs>
            <telerik:RadTab Text="<%$ Resources:common, vlastnosti %>" Selected="true" Value="core"></telerik:RadTab>
            <telerik:RadTab Text="<%$ Resources:common, uzivatelska-pole %>" Value="ff"></telerik:RadTab>
            <telerik:RadTab Text="<%$ Resources:common, ostatni %>" Value="other"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="core" runat="server" Selected="true">

            <div class="content-box2">
                <div class="title">Typ osobního profilu</div>
                <div class="content">
                    <asp:RadioButtonList ID="j02IsIntraPerson" runat="server" AutoPostBack="true" RepeatDirection="Vertical">
                        <asp:ListItem Text="<%$ Resources:j02_record, j02IsIntraPerson_1 %>" Value="1" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:j02_record, j02IsIntraPerson_0 %>" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>

                </div>
            </div>
            <table cellpadding="5" cellspacing="2">
                <tr>
                    <td style="width: 140px;">
                        <asp:Label ID="lblTitle" Text="Titul:" runat="server" CssClass="lbl" AssociatedControlID="j02TitleBeforeName" meta:resourcekey="lblTitle"></asp:Label>
                    </td>
                    <td>
                        <uc:datacombo ID="j02TitleBeforeName" runat="server" Width="70px" AllowCustomText="true" ShowToggleImage="false" Filter="Contains" DefaultValues="Bc.;BcA.;Ing.;Ing.arch.;MUDr.;MVDr.;MgA.;Mgr.;JUDr.;PhDr.;RNDr.;PharmDr.;ThLic.;ThDr.;Ph.D.;Th.D.;prof.;doc.;PaedDr.;Dr.;PhMr."></uc:datacombo>
                    </td>
                    <td>
                        <asp:Label ID="lblFirstName" Text="Jméno:" runat="server" CssClass="lblReq" AssociatedControlID="j02FirstName" meta:resourcekey="lblFirstName"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02FirstName" runat="server" Style="width: 100px;"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblLastName" Text="Příjmení:" runat="server" CssClass="lblReq" AssociatedControlID="j02LastName" meta:resourcekey="lblLastName"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02LastName" runat="server" Style="width: 160px;"></asp:TextBox>
                    </td>
                    <td>
                        <uc:datacombo ID="j02TitleAfterName" runat="server" Width="70px" AllowCustomText="true" ShowToggleImage="false" Filter="Contains" DefaultValues="CSc.;DrSc.;dr. h. c.;DiS."></uc:datacombo>
                    </td>
                </tr>
            </table>
            <table cellpadding="5" cellspacing="2">
                <tr>
                    <td style="width: 140px;">
                        <asp:Label ID="lblj02Email" Text="E-mail adresa:" runat="server" CssClass="lblReq" AssociatedControlID="j02Email" meta:resourcekey="lblj02Email"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j02Email" runat="server" Style="width: 300px;"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="Zadejte validní e-mail adresu." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,4}){1,2}$" ControlToValidate="j02Email"></asp:RegularExpressionValidator>
                        </div>
                        
                        
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
        <telerik:RadPageView ID="other" runat="server">
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
</asp:Content>
