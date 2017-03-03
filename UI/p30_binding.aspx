﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p30_binding.aspx.vb" Inherits="UI.p30_binding" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="project" Src="~/project.ascx" %>
<%@ Register TagPrefix="uc" TagName="contact" Src="~/contact.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function j02_edit(j02id) {

            dialog_master("j02_record.aspx?iscontact=1&pid=" + j02id, true)

        }

        function hardrefresh(pid, flag) {
            var j02id = "";
            if (flag == "j02-save") {
                j02id = pid;
            }
            location.replace("p30_binding.aspx?masterprefix=<%=me.currentprefix%>&masterpid=<%=master.datapid%>&default_j02id=" + j02id)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panMasterRecord" runat="server" CssClass="content-box2" Style="padding-top: 10px;" Visible="false">
        <div class="title">
            Vybrat klienta nebo projekt, s kterým má být svázána kontaktní osoba
        </div>
        <div class="content">
            <span>Klient:</span>
            <uc:contact ID="p28id" runat="server" AutoPostBack="false" Width="400px" Flag="searchbox" />
            <div>
                <span>nebo</span>
            </div>
            <div>
                <span>Projekt:</span>
                <uc:project ID="p41id" runat="server" AutoPostBack="true" Width="400px" Flag="searchbox" />
            </div>
            <hr />
            <asp:Button ID="cmdContinue" runat="server" CssClass="cmd" Text="Pokračovat" />

        </div>
    </asp:Panel>
    <asp:Panel ID="panPersons" runat="server">
        <div class="content-box2" style="padding-top: 10px;">
            <div class="title">
                <img alt="Osoba" src="Images/search.png" width="16px" height="16px" />
                <span class="framework_header_span">Vyhledat osobu v adresáři</span>

            </div>
            <div class="content">
                <table cellpadding="4">

                    <tr style="vertical-align: top;">

                        <td>
                            <uc:person ID="j02ID" runat="server" Flag="all2" Width="400px" />
                        </td>
                        <td>
                            <asp:DropDownList ID="p27ID" runat="server" DataTextField="p27Name" DataValueField="pid"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="cmdSave" runat="server" CssClass="cmd" Text="Přidat vybranou osobu" />
                        </td>

                    </tr>

                </table>
            </div>
        </div>
        <div class="div6">
            nebo <button type="button" onclick="j02_edit(0)">Založit novou osobu</button>
        </div>
        <div class="content-box2" style="padding-top: 10px;">
            <div class="title">
                <img alt="Osoba" src="Images/person.png" width="16px" height="16px" />
                <asp:label ID="lblBoundHeader" runat="server" cssclass="framework_header_span" Text="Přiřazené kontaktní osoby"></asp:label>

            </div>
            <asp:Panel ID="panP30" runat="server" CssClass="content">
                <table cellpadding="6">
                    <asp:Repeater ID="rpP30" runat="server">
                        <ItemTemplate>
                            <tr class="trHover">
                                <td>
                                    <asp:Label ID="p27Name" runat="server" CssClass="valbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:HyperLink ID="clue_j02" runat="server" CssClass="reczoom" Text="i" title="Detail"></asp:HyperLink>
                                    <asp:Label ID="Person" runat="server" CssClass="valboldblue"></asp:Label>
                                </td>

                                <td>
                                    <img src="Images/edit.png" />
                                    <asp:HyperLink ID="cmdJ02" runat="server" Text="Upravit/odstranit osobu"></asp:HyperLink>
                                    <asp:HiddenField ID="p30id" runat="server" />
                                </td>
                                <td>
                                    <asp:Image ID="imgDel" runat="server" ImageUrl="Images/delete.png" Style="margin-left: 20px;" />
                                    <asp:LinkButton ID="cmdDelete" runat="server" Text="Odstranit vazbu" CommandName="delete"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="cmdDefaultInWorksheet" runat="server" Text="Výchozí v zapisování výkazů" CommandName="default_add" Visible="false"></asp:LinkButton>
                                    <asp:Label ID="lblDefaultInWorksheet" runat="server" Text="Výchozí v zapisování výkazů" CssClass="valboldred" Visible="false"></asp:Label>
                                    <asp:ImageButton ID="cmdDeleteDefault" runat="server" ImageUrl="Images/break.png" ToolTip="Zrušit přednastavení v zapisování výkazů" CommandName="default_delete" Visible="false" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="cmdDefaultInInvoice" runat="server" Text="Nastavit jako fakturační osobu" CommandName="default_invoice_add" Visible="false"></asp:LinkButton>
                                    <asp:Label ID="lblDefaultInInvoice" runat="server" Text="Přednastaveno ve fakturaci" CssClass="valboldred" Visible="false"></asp:Label>
                                    <asp:ImageButton ID="cmdDeleteDefaultInInvoice" runat="server" ImageUrl="Images/break.png" ToolTip="Zrušit jako fakturační osobu" CommandName="default_invoice_delete" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="border-bottom: solid 1px silver;">
                                    <asp:Label ID="Message" runat="server" CssClass="infoNotification"></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </asp:Panel>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hidPrefix" runat="server" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
