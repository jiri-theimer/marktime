﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="j02_framework_detail.aspx.vb" Inherits="UI.j02_framework_detail" %>
<%@ MasterType VirtualPath="~/SubForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="entity_menu" Src="~/entity_menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="freefields_readonly" Src="~/freefields_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="x18_readonly" Src="~/x18_readonly.ascx" %>
<%@ Register TagPrefix="uc" TagName="alertbox" Src="~/alertbox.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function b07_reaction(b07id) {
            sw_decide("b07_create.aspx?parentpid=" + b07id + "&masterprefix=j02&masterpid=<%=Master.datapid%>", "Images/comment_32.png", true)

        }
       
        function hardrefresh(pid, flag) {
            if (flag == "j02-save") {
                parent.window.location.replace("j02_framework.aspx?pid=" + pid);
                return;
            }
            if (flag == "j02-delete") {
                parent.window.location.replace("j02_framework.aspx");
                return;
            }


            location.replace("j02_framework_detail.aspx?pid=<%=master.datapid%>");
        }

        function j03_create() {
            sw_decide("j03_create.aspx?j02id=<%=master.datapid%>", "Images/user_32.png", true);
        }

        function j03_edit() {
            sw_decide("j03_record.aspx?pid=<%=Me.CurrentJ03ID%>", "Images/user_32.png", true);
        }

        function j05_record(j05id) {

            sw_decide("j05_record.aspx?pid=" + j05id, "Images/masterslave_32.png", false);

        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc:entity_menu ID="menu1" runat="server"></uc:entity_menu>

    <div class="content-box1">
            <div class="title">
                <img src="Images/properties.png" style="margin-right: 10px;" />Záznam osobního profilu
                        
            </div>
            <div class="content">
                <table cellpadding="10" cellspacing="2" id="responsive">
                    <tr valign="top">
                        <td style="min-width: 120px;">
                            <asp:Label ID="lblPerson" runat="server" Text="Jméno:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>

                            <asp:Label ID="FullNameAsc" runat="server" CssClass="valbold"></asp:Label>

                            <div>
                                <asp:Label ID="j02Code" runat="server" CssClass="valbold" ForeColor="gray"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblJ07Name" runat="server" Text="Pozice:" CssClass="lbl"></asp:Label>
                            <div>
                                <asp:Label ID="lblJ18Name" runat="server" Text="Středisko:" CssClass="lbl"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblJ17Name" runat="server" Text="Stát:" CssClass="lbl"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="j07Name" runat="server" CssClass="valbold"></asp:Label>
                            <div>
                                <asp:Label ID="j18Name" runat="server" CssClass="valbold"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="j17Name" runat="server" CssClass="valbold"></asp:Label>
                            </div>
                        </td>

                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblEmail" runat="server" Text="E-mail:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:HyperLink ID="j02Email" runat="server"></asp:HyperLink>

                        </td>
                        <td>
                            <asp:Label ID="lblFond" runat="server" Text="Fond hodin:" CssClass="lbl"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="c21Name" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:Label ID="lblTeams" runat="server" Text="Člen týmů:" CssClass="lbl"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="TeamsInLine" runat="server" CssClass="valbold"></asp:Label>

                        </td>
                        
                    </tr>

                </table>

                <asp:Label ID="Mediums" runat="server" CssClass="valbold"></asp:Label>
                <div>
                    <asp:Label ID="Correspondence" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <asp:Panel ID="panIntraPerson" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/user.png" style="margin-right: 10px;" />Uživatelský účet
            </div>
            <div class="content">
                <asp:Panel ID="panAccount" runat="server">
                    <table cellpadding="10" cellspacing="2">
                        <tr valign="top">
                            <td style="min-width: 120px;">
                                <asp:Label ID="lblLogin" runat="server" Text="Přihlašovací jméno:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>

                                <asp:Label ID="j03Login" runat="server" CssClass="valbold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblJ04Name" runat="server" Text="Aplikační role:" CssClass="lbl"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="j04Name" runat="server" CssClass="valbold"></asp:Label>

                            </td>

                        </tr>


                    </table>
                </asp:Panel>
                <asp:Label ID="AccountMessage" runat="server" CssClass="infoInForm"></asp:Label>
                <asp:HyperLink ID="cmdLog" runat="server" Text="Historie aktivit" NavigateUrl="javascript: timeline()"></asp:HyperLink>
                <span style="padding-left: 40px;"></span>
                <asp:HyperLink ID="cmdAccount" runat="server" Text="Založit uživatelský účet" Visible="false"></asp:HyperLink>
            </div>
        </asp:Panel>
        <asp:Panel ID="boxJ05" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/masterslave.png" style="margin-right: 10px;" />
                <asp:Label ID="boxJ05Title" runat="server" Text="Nadřízenost | Podřízenost"></asp:Label>
                <asp:HyperLink ID="cmdAddJ05" runat="server" Text="Přidat" NavigateUrl="javascript:j05_record(0)"></asp:HyperLink>
            </div>
            <div class="content">

                <asp:Panel ID="panSlaves" runat="server" CssClass="div6">
                    <img src="Images/slave.png" /><span>Podřízení:</span>
                    <asp:Repeater ID="rpSlaves" runat="server">
                        <ItemTemplate>
                            <a href="javascript:j05_record(<%#Eval("pid")%>)"><%#Eval("PersonSlave")%><%#Eval("TeamSlave")%></a>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="SlavesInLine" runat="server"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="panMasters" runat="server" CssClass="div6">
                    <img src="Images/master.png" /><span>Nadřízení:</span>
                    <asp:Repeater ID="rpMasters" runat="server">
                        <ItemTemplate>
                            <a href="javascript:j05_record(<%#Eval("pid")%>)"><%#Eval("PersonMaster")%></a>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </div>

        </asp:Panel>
        <asp:Panel ID="boxX18" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/label.png" style="margin-right: 10px;" />
                <asp:Label ID="boxX18Title" runat="server" Text="Štítky"></asp:Label>
                <asp:HyperLink ID="x18_binding" runat="server" Text="Přiřadit"></asp:HyperLink>
            </div>
            <div class="content">
                <uc:x18_readonly ID="labels1" runat="server"></uc:x18_readonly>
            </div>
        </asp:Panel>
        <asp:Panel ID="boxFF" runat="server" CssClass="content-box1">

            <div class="title">
                <img src="Images/form.png" style="margin-right: 10px;" />
                <asp:Label ID="boxFFTitle" runat="server" Text="Uživatelská pole"></asp:Label>
                <asp:CheckBox ID="chkFFShowFilledOnly" runat="server" AutoPostBack="true" Text="Zobrazovat pouze vyplněná pole" />
            </div>
            <div class="content">
                <uc:freefields_readonly ID="ff1" runat="server" />
            </div>

        </asp:Panel>


        <asp:Panel ID="boxP30" runat="server" CssClass="content-box1">
            <div class="title">
                <img src="Images/contact.png" />
                <img src="Images/project.png" style="margin-right: 10px;" />
                Kontaktní osoba pro klienty/projekty
            </div>
            <div class="content">
                <asp:Repeater ID="rpP30" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <asp:HyperLink ID="Company" runat="server" Target="_top"></asp:HyperLink>
                            <asp:Label ID="p27Name" runat="server"></asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:Panel>


        <uc:alertbox id="alert1" runat="server"></uc:alertbox>

    <uc:b07_list ID="comments1" runat="server" JS_Create="b07_record()" JS_Reaction="b07_reaction" />

    <asp:HiddenField ID="hidJ03ID" runat="server" />
</asp:Content>
