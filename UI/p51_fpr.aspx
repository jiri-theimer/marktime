<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p51_fpr.aspx.vb" Inherits="UI.p51_fpr" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    function periodcombo_setting() {

        dialog_master("periodcombo_setting.aspx");
    }

    function sw_local(url, iconUrl, is_maximize) {
            var wnd = $find("<%=okno1.clientid%>");
            wnd.setUrl(url);
            if (iconUrl != null)
                wnd.set_iconUrl(iconUrl);
            else
                wnd.set_iconUrl("Images/window_32.png");

            wnd.show();

            if (is_maximize == true) {
                wnd.maximize();
            }
            else {
                wnd.center();
            }

    }

    function hardrefresh(pid, flag) {

    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="10">
        <tr>
            
            <td>
                <asp:Label ID="lblP51ID" runat="server" CssClass="lbl" Text="Vzorový ceník pro výpočet efektivních sazeb:"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="p51ID" runat="server" DataTextField="NameWithCurr" DataValueField="pid" Filter="Contains" AutoPostBack="true" IsFirstEmptyRow="true" Width="500px"></uc:datacombo>
                <asp:HyperLink ID="clue1" runat="server" CssClass="reczoom" Text="i" title="Detail ceníku"></asp:HyperLink>
            </td>


        </tr>
    </table>
    <div class="div6">
        <span>Časové období podle zdanitelného plnění faktur:</span>
        <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
        <div>
        <asp:Button ID="cmdRecalcAll" Text="Přepočítat efektivní sazby vyfakturovaných úkonů za vybrané období." CssClass="cmd" runat="server" />
        </div>
        
    </div>

    <telerik:RadWindow ID="okno1" runat="server" RenderMode="Lightweight" Width="900px" Height="700px" Modal="false" VisibleStatusbar="false" Skin="MetroTouch" IconUrl="Images/project_32.png" ShowContentDuringLoad="false" InitialBehaviors="None" Behaviors="Close,Move,Resize,Maximize" Style="z-index: 9000;">
            <Shortcuts>
                <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
            </Shortcuts>
            <Localization Close="Zavřít" Restore="Základní velikost" Maximize="Maximalizovat" />
        </telerik:RadWindow>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
