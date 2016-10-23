<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="main_menu.ascx.vb" Inherits="UI.main_menu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


        <asp:panel runat="server" ID="panContainer" style="background-color: #25a0da;height:32px;" Visible="false">

            <telerik:RadMenu ID="menu1" runat="server" Width="100%" RenderMode="Auto" Skin="Windows7" ClickToOpen="true" ExpandDelay="0" ExpandAnimation-Duration="0" ExpandAnimation-Type="None" CollapseDelay="0" CollapseAnimation-Duration="0" CollapseAnimation-Type="None" Style="z-index: 2901;background-color:blue !important;">                            
            </telerik:RadMenu>

        </asp:panel>
        <div id="search_result" style="position: relative;z-index:9999;"></div>
       <asp:HiddenField ID="hidSearchBox1" runat="server" />

<%If panContainer.Visible Then%>
<script type="text/javascript">
    function help(page) {
        window.open("help.aspx?page=" + page, "_blank");
    }

    function messages() {        
        try {
            sw_master("j03_messages.aspx", "Images/messages_32.png", false)
        }
        catch (err) {
            sw_local("j03_messages.aspx", "Images/messages_32.png", false)
        }
    }

    function setsaw(value) {

        createCookie('MT50-SAW', value, 30);
        location.replace(window.location.href);
    }

    function setlang(value) {

        createCookie('MT50-CultureInfo', value, 30);
        location.replace(window.location.href);
    }

    function createCookie(name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    function __highlight(s, t) {
        var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
        return s.replace(matcher, "<strong>$1</strong>");
    }

    function search1Focus() {
        document.getElementById("<%=hidSearchBox1.Value%>").value = "";
        document.getElementById("<%=hidSearchBox1.Value%>").style.background = "yellow";
    }
    function search1Blur() {

        document.getElementById("<%=hidSearchBox1.Value%>").style.background = "";
        document.getElementById("<%=hidSearchBox1.Value%>").value = "<%=Resources.Site.NajitProjekt%>";
    }
</script>
<%end if %>