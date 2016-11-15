﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="j03_myprofile.aspx.vb" Inherits="UI.j03_myprofile" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function personalpage() {
            sw_master("j03_myprofile_defaultpage.aspx", "Images/plugin_32.png")


        }
        function sendmail() {
            sw_master("sendmail.aspx", "Images/email_32.png")


        }
        function hardrefresh(pid, flag) {
            document.getElementById("<%=hidHardRefreshFlag.clientid%>").value = flag;
            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefreshOnBehind, "", False)%>;

        }

        function RowSelected(sender, args) {

            document.getElementById("<%=hiddatapid.clientid%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            x40_edit();
        }
        function x40_edit() {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            sw_master("x40_record.aspx?pid=" + pid, "Images/email_32.png");

        }
        function report() {
            sw_master("report_modal.aspx?prefix=j02&pid=<%=Master.Factory.SysUser.j02ID%>", "Images/reporting.png", true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 10px; background: white;">
        <table cellpadding="5">
            <tr>
                <td>
                    <img src="Images/user_32.png" />
                </td>
                <td>
                    <asp:Label ID="lblHeader" runat="server" CssClass="framework_header_span" Style="font-size: 200%;" Text="Můj profil"></asp:Label>

                </td>
                <td style="padding-left: 20px;" id="tdX31" runat="server">
                    <img src="Images/report.png" />
                    <a type="button" href="javascript:report()">Osobní tiskové sestavy</a>
                </td>
                <td style="padding-left: 20px;">
                    <img src="Images/plugin.png" />
                    <a type="button" href="javascript:personalpage()">Zvolit si startovací (výchozí) stránku</a>
                </td>
                <td style="padding-left: 20px;">
                    <img src="Images/email.png" />
                    <a type="button" href="javascript:sendmail()">Odeslat poštovní zprávu</a>
                </td>

            </tr>
        </table>


        <table cellpadding="10" cellspacing="2">
            <tr>
                <td>
                    <asp:Label ID="lblLogin" runat="server" CssClass="lbl" Text="Přihlašovací jméno:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="j03login" runat="server" CssClass="valbold"></asp:Label>
                    <asp:Label ID="Label3" runat="server" CssClass="lbl" Text="Expirace přihlašovacího hesla:" Style="padding-left: 20px;"></asp:Label>
                    <asp:Label ID="j03PasswordExpiration" runat="server" CssClass="valboldred"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Aplikační role:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="j04Name" runat="server" CssClass="valbold"></asp:Label>

                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblPerson" runat="server" CssClass="lbl" Text="Osobní profil:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="FullName" runat="server" CssClass="valbold"></asp:Label>
                    <asp:HyperLink ID="j02clue" runat="server" CssClass="reczoom" Text="i"></asp:HyperLink>
                    <asp:Label ID="PersonalMessage" runat="server" CssClass="valboldblue"></asp:Label>
                </td>
            </tr>



        </table>



        <asp:Panel ID="panJ02Update" runat="server" CssClass="content-box2">
            <div class="title">

                <asp:Label ID="ph1" runat="server" Text="Aktualizovat můj MARKTIME profil" Style="display: inline-block; min-width: 150px;"></asp:Label>
                <asp:Button ID="cmdSave" runat="server" CssClass="cmd" Text="Uložit změny v osobním profilu" />
                
            </div>
            <div class="content">
                <div class="div6" style="display: none;">
                    <asp:CheckBox ID="j03IsLiveChatSupport" runat="server" Text="Zapnutá Live chat MARKTIME podpora" CssClass="chk" />

                </div>
                <div class="div6">
                    <asp:CheckBox ID="j03IsSiteMenuOnClick" runat="server" Text="Hlavní aplikační menu se otevírá až na click myši" CssClass="chk" />
                </div>

                <div class="div6">
                    <span class="lbl">Vzhled (Skin) hlavního aplikačního menu:</span>
                    <asp:DropDownList ID="j03SiteMenuSkin" runat="server">
                        <asp:ListItem Text="--Výchozí--" Value="Windows7" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="Metro" Value="Metro"></asp:ListItem>
                        <asp:ListItem Text="WebBlue" Value="WebBlue"></asp:ListItem>
                        <asp:ListItem Text="Silk" Value="Silk"></asp:ListItem>
                        <asp:ListItem Text="Outlook" Value="Outlook"></asp:ListItem>
                        <asp:ListItem Text="Office2007" Value="Office2007"></asp:ListItem>
                        <asp:ListItem Text="Office2010Blue" Value="Office2010Blue"></asp:ListItem>
                        <asp:ListItem Text="Telerik" Value="Telerik"></asp:ListItem>
                        <asp:ListItem Text="Sunset" Value="Sunset"></asp:ListItem>
                        <asp:ListItem Text="Simple" Value="Simple"></asp:ListItem>
                        <asp:ListItem Text="Black" Value="Black"></asp:ListItem>
                        <asp:ListItem Text="Glow" Value="Glow"></asp:ListItem>
                        <asp:ListItem Text="Web20" Value="Web20"></asp:ListItem>

                    </asp:DropDownList>

                </div>
                <div class="div6">
                    <asp:DropDownList ID="j03ModalWindowsFlag" runat="server">
                        <asp:ListItem Text="Velikost modálních dialogových oken 900 x 700" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Modální dialogová okna automaticky maximalizovat na plnou velikost" Value="1"></asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="div6">
                    <span>Maska ve vyhledávači projektu:</span>
                    <asp:DropDownList ID="j03ProjectMaskIndex" runat="server">
                        <asp:ListItem Text="Klient+název projektu+kód projektu" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Název projektu" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Název projektu+kód projektu" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Název projektu+klient" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Kód projektu" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <table cellpadding="10">
                   
                    <tr>
                        <td>
                            <asp:Label ID="lblj02Email" Text="E-mail:" runat="server" CssClass="lbl" AssociatedControlID="j02Email"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02Email" runat="server" Style="width: 300px;"></asp:TextBox>

                            <asp:Label ID="lblEmailInfo" runat="server" CssClass="infoNotification"></asp:Label>


                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="Label2" Text="Podpis pro e-mail:" runat="server" CssClass="lbl" AssociatedControlID="j02EmailSignature"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02EmailSignature" runat="server" Style="width: 300px; height: 70px;" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>

                        <td>
                            <asp:Label ID="lblj02Mobile" Text="Mobil:" runat="server" CssClass="lbl" AssociatedControlID="j02Mobile"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02Mobile" runat="server" Style="width: 300px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Label ID="lblj02Phone" Text="Pevný telefon:" runat="server" CssClass="lbl" AssociatedControlID="j02Phone"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02Phone" runat="server" Style="width: 300px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblOffice" Text="Kancelář:" runat="server" CssClass="lbl" AssociatedControlID="j02Office"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="j02Office" runat="server" Style="width: 300px;"></asp:TextBox>
                        </td>
                    </tr>





                </table>
            </div>
        </asp:Panel>
        <asp:Button ID="cmdDeleteUserParams" runat="server" Text="Vyčistit paměť (cache) v mém uživatelském profilu" CssClass="cmd" />

    </div>
    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:LinkButton ID="cmdRefreshOnBehind" runat="server" Text="refreshonbehind" Style="display: none;"></asp:LinkButton>
</asp:Content>
