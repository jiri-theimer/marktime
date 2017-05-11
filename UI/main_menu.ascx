<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="main_menu.ascx.vb" Inherits="UI.main_menu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


        <asp:panel runat="server" ID="panContainer" style="background-color: #25a0da;height:32px;" Visible="false">

            <telerik:RadMenu ID="menu1" runat="server" Width="100%" EnableViewState="false" RenderMode="Auto" ShowToggleHandle="false" Skin="Windows7" ClickToOpen="true" EnableRoundedCorners="true" EnableShadows="true" ExpandDelay="0" ExpandAnimation-Duration="0" ExpandAnimation-Type="None" CollapseDelay="0" CollapseAnimation-Duration="0" CollapseAnimation-Type="None" Style="z-index: 2901;background-color:blue !important;">                            
            </telerik:RadMenu>

        </asp:panel>
        <div id="search1_result" style="position:relative;z-index:9000;"></div>
       <asp:HiddenField ID="hidSearch1" runat="server" />
        <asp:HiddenField ID="hidAllowSearch1" runat="server" Value="0" />
        <asp:HiddenField ID="hidMasterPageName" runat="server" Value="Site" />

<script type="text/javascript">
    <%If panContainer.Visible Then%>    
   
    

    function help(page) {
        window.open("help.aspx?page=" + page, "_blank");
    }

    function sw_menu_decide(url,img,is_max){        
        try {
            sw_master(url, img, is_max)
        }
        catch (err) {
            sw_local(url, img, is_max)
        }
    }

    function messages() {
        sw_menu_decide("j03_messages.aspx","Images/globe.png",false);       
    }

    function p28_create() {
        
        sw_menu_decide("p28_record.aspx?pid=0&hrjs=hardrefresh_menu", "Images/contact.png", false);
    }
    function p41_create() {
        sw_menu_decide("p41_create.aspx?hrjs=hardrefresh_menu", "Images/project.png", false);
    }

    function p56_create() {
        sw_menu_decide("p56_record.aspx?masterprefix=p41&masterpid=0&hrjs=hardrefresh_menu", "Images/task.png");

    }
    function p91_create() {
        sw_menu_decide("p91_create_step1.aspx?prefix=p28&hrjs=hardrefresh_menu", "Images/invoice.png", true);

    }
    function p31_create() {
        sw_menu_decide("p31_record.aspx?pid=0&hrjs=hardrefresh_menu", "Images/worksheet.png")

    }
    function o23_create() {
        sw_menu_decide("o23_record.aspx?pid=0&hrjs=hardrefresh_menu", "Images/notepad.png")

    }
    function o10_create() {
        sw_menu_decide("o10_record.aspx?pid=0&hrjs=hardrefresh_menu", "Images/article.png", true)

    }
    function p90_create() {
        sw_menu_decide("p90_record.aspx?pid=0&hrjs=hardrefresh_menu", "Images/proforma.png")

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

    


    function hardrefresh_menu(pid, flag) {        
        if (flag == "p41-create" || flag == "p41-save") {
            location.replace("p41_framework.aspx?pid=" + pid);
            return;
        }
        if (flag == "p56-save" || flag == "p56-create") {
            location.replace("p56_framework.aspx?pid="+pid);
            return;
        }
        if (flag == "p91-create" || flag == "p91-save") {
            location.replace("p91_framework.aspx?pid=" + pid);
            return;
        }

        if (flag == "p28-save" || flag == "p28-create") {
            location.replace("p28_framework.aspx?pid=" + pid);
            return;
        }
        if (flag == "o23-save" || flag == "o23-create") {
            location.replace("o23_framework.aspx?pid=" + pid);
            return;
        }
        if (flag == "j03_myprofile_defaultpage") {
            location.replace("default.aspx");
            return;
        }

        

    }

    

    <%end if %>

    function MainMenuClose() {
        <%If panContainer.Visible Then%>    
        var menu = $find("<%= menu1.ClientID%>");
        menu.close();
        <%end if%>
    }
</script>
