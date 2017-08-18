<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="import_object.aspx.vb" Inherits="UI.import_object" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="import_object_item" Src="~/import_object_item.ascx" %>

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

        function project_OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            var combo = sender;

            if (combo.get_value() == "")
                context["filterstring"] = eventArgs.get_text();
            else
                context["filterstring"] = "";

            context["j03id"] = "<%=Master.Factory.SysUser.PID%>";
            context["j02id_explicit"] = "<%=Master.Factory.SysUser.j02ID%>";
            context["flag"] = "searchbox";
        }
        function contact_OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            var combo = sender;

            if (combo.get_value() == "")
                context["filterstring"] = eventArgs.get_text();
            else
                context["filterstring"] = "";

            context["j03id"] = "<%=Master.Factory.SysUser.PID%>";
            context["flag"] = "searchbox";
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
                    <asp:Label ID="lblTopHeader" runat="server" CssClass="framework_header_span" Text="Import z MS-OUTLOOK..."></asp:Label>
                    <button type="button" id="cmdPopup" runat="server" onclick="repeat_popup()" visible="false">Pokračovat</button>
                    <button type="button" id="Button1" runat="server" onclick="close_curtab()" style="margin-left: 100px;">Zavřít</button>
                </td>

            </tr>
        </table>

        <telerik:RadTabStrip ID="tabs1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
            <Tabs>
                <telerik:RadTab Text="Naimportovat jako dokument" Selected="true" Value="prefix" ImageUrl="Images/notepad.png"></telerik:RadTab>
                <telerik:RadTab Text="Naimportovat pouze jako komentář s přílohou" Value="b07" ImageUrl="Images/comment.png"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>

        <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
            <telerik:RadPageView ID="prefix" runat="server" Selected="true">
                <asp:Panel ID="panO23" runat="server" Visible="false">
                    <div class="div6">
                        <span>Vyberte typ dokumentu</span>
                    </div>
                    <asp:RadioButtonList ID="x18ID" runat="server" AutoPostBack="true" RepeatDirection="Vertical" DataValueField="pid" DataTextField="x18Name" CellPadding="6"></asp:RadioButtonList>
                </asp:Panel>
            </telerik:RadPageView>
            <telerik:RadPageView ID="b07" runat="server">
                <div class="div6">
                    <asp:RadioButtonList ID="opgSearch" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Projekt" Value="p41" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Klient" Value="p28"></asp:ListItem>
                    </asp:RadioButtonList>
                
                <telerik:RadComboBox ID="search_p41" runat="server" DropDownWidth="400" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" Text="Hledat projekt..." Width="400px" OnClientItemsRequesting="project_OnClientItemsRequesting">
                    <WebServiceSettings Method="LoadComboData" Path="~/Services/project_service.asmx" UseHttpGet="false" />
                </telerik:RadComboBox>
                <telerik:RadComboBox ID="search_p28" runat="server" DropDownWidth="400" EnableTextSelection="true" MarkFirstMatch="true" EnableLoadOnDemand="true" Text="Hledat klienta..." Width="400px" OnClientItemsRequesting="contact_OnClientItemsRequesting">
                    <WebServiceSettings Method="LoadComboData" Path="~/Services/contact_service.asmx" UseHttpGet="false" />
                </telerik:RadComboBox>
                </div>
                <div class="div6">
                    <span class="lbl">Komentář:</span>
                    <asp:RadioButtonList ID="opgBodyFormat" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Jako TEXT" Value="1" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="Jako HTML" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Bez komentáře" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="b07Value" runat="server" TextMode="MultiLine" Style="width: 100%; height: 300px; font-family: 'Courier New';"></asp:TextBox>
                    <asp:panel ID="panBodyHTML" runat="server" CssClass="bigtext">
                        <asp:Literal ID="b07BodyHTML" runat="server"></asp:Literal>
                    </asp:panel>
                </div>
                <div class="div6">
                    <uc:import_object_item ID="io1" runat="server"></uc:import_object_item>
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>




        <div class="content-box2" style="margin-top: 20px;">
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
