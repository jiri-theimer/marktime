<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="import_object.aspx.vb" Inherits="UI.import_object" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(window).load(function () {
            <%If Me.hidPopupUrl.Value <> "" Then%>
            sw_everywhere("<%=me.hidPopupUrl.value%>", "", true);
            <%End If%>


        })


        function repeat_popup() {            
            sw_everywhere("<%=me.hidPopupUrl.value%>", "", true);
        }

        function sw_local(url, iconUrl, is_maximize) {
            sw_master(url, iconUrl, is_maximize)

        }

        function hardrefresh(pid, flag) {
            //nic

        }

        function close_curtab() {
            
            window.open('', '_parent', '');
            window.close();
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div style="background-color: white; padding: 10px;">
        <table cellpadding="5">
            <tr>
                <td>
                    <img src="Images/outlook_32.png" />
                </td>
                <td>
                    <asp:Label ID="lblTopHeader" runat="server" CssClass="framework_header_span" Text="Založit z MS-OUTLOOK..."></asp:Label>
                    <button type="button" id="cmdPopup" runat="server" onclick="repeat_popup()" visible="false">Pokračovat</button>
                    <button type="button" id="Button1" runat="server" onclick="close_curtab()" style="margin-left:100px;">Zavřít</button>
                </td>

            </tr>
        </table>

        <asp:Panel ID="panO23" runat="server" CssClass="content-box2" Visible="false">
            <div class="title">Vyberte typ dokumentu</div>
            <div class="content">
                <asp:RadioButtonList ID="x18ID" runat="server" AutoPostBack="true" RepeatDirection="Vertical" DataValueField="pid" DataTextField="x18Name" CellPadding="6"></asp:RadioButtonList>
                
            </div>
        </asp:Panel>

        <div class="content-box2" style="margin-top:20px;">
            <div class="title">
                <img src="Images/outlook.png" />
                <asp:HyperLink ID="linkMSG" runat="server" Text="Otevřít v MS-OUTLOOK"></asp:HyperLink>
            </div>
            <div class="content">
                <table cellpadding="8">
                    <tr>
                        <td>Název:
                        </td>
                        <td>
                            <asp:Label ID="Subject" runat="server" CssClass="valbold"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hidPopupUrl" runat="server" />
    <asp:HiddenField ID="hidPrefix" runat="server" />
    <asp:HiddenField ID="hidGUID" runat="server" />
    <asp:HiddenField ID="hidP41ID" runat="server" />

   
</asp:Content>
