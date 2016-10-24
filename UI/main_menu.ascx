﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="main_menu.ascx.vb" Inherits="UI.main_menu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


        <asp:panel runat="server" ID="panContainer" style="background-color: #25a0da;height:32px;" Visible="false">

            <telerik:RadMenu ID="menu1" runat="server" Width="100%" RenderMode="Auto" Skin="Windows7" ClickToOpen="true" ExpandDelay="0" ExpandAnimation-Duration="0" ExpandAnimation-Type="None" CollapseDelay="0" CollapseAnimation-Duration="0" CollapseAnimation-Type="None" Style="z-index: 2901;background-color:blue !important;">                            
            </telerik:RadMenu>

        </asp:panel>
        <div id="search1_result" style="position: relative;z-index:9999;"></div>
       <asp:HiddenField ID="hidSearch1" runat="server" />
        <asp:HiddenField ID="hidMasterPageName" runat="server" Value="Site" />

<script type="text/javascript">
    <%If panContainer.Visible Then%>

    $(function () {

        $("#<%=hidSearch1.Value%>").autocomplete({
            source: "Handler/handler_search_project.ashx",
            minLength: 1,
            select: function (event, ui) {
                if (ui.item) {
                    location.replace("p41_framework.aspx?pid=" + ui.item.PID);
                    return false;
                }
            }
            <%If Me.hidMasterPageName.Value="Site" then%>
            ,
            open: function (event, ui) {
                $('ul.ui-autocomplete')
                   .removeAttr('style').hide()
                   .appendTo('#search1_result').show();
            },
            close: function (event, ui) {
                $('ul.ui-autocomplete')
                .hide();                   
            }   
            <%end If%>


        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            var s = "<div>";

            if (item.Closed == "1")
                s = s + "<a style='text-decoration:line-through;'>";
            else
                s = s + "<a>";


            s = s + __highlight1(item.Project, item.FilterString);


            s = s + "</a>";

            if (item.Draft == "1")
                s = s + "<img src='Images/draft.png' alt='DRAFT'/>"


            s = s + "</div>";


            return $(s).appendTo(ul);


        };
    });


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
        var s = window.location.href;
        
        if (s.indexOf("p31_framework") > -1) {
            location.replace("p31_framework.aspx");
            return;
        }
            

        if (s.indexOf("p41_framework") > -1 || s.indexOf("prefix=p41")>-1) {
            location.replace("p41_framework.aspx");
            return;
        }
        if (s.indexOf("j02_framework") > -1 || s.indexOf("prefix=j02") > -1) {
            location.replace("j02_framework.aspx");
            return;
        }
        if (s.indexOf("p28_framework") > -1 || s.indexOf("prefix=p28") > -1) {
            location.replace("p28_framework.aspx");
            return;
        }
        if (s.indexOf("p91_framework") > -1 || s.indexOf("prefix=p91") > -1) {
            location.replace("p91_framework.aspx");
            return;
        }
        if (s.indexOf("p56_framework") > -1 || s.indexOf("prefix=p56") > -1) {
            location.replace("p56_framework.aspx");
            return;
        }
        if (s.indexOf("o23_framework") > -1 || s.indexOf("prefix=o23") > -1) {
            location.replace("o23_framework.aspx");
            return;
        }
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

    function __highlight1(s, t) {
        var matcher = new RegExp("(" + $.ui.autocomplete.escapeRegex(t) + ")", "ig");
        return s.replace(matcher, "<strong>$1</strong>");
    }

    function search1Focus() {
        document.getElementById("<%=hidSearch1.Value%>").value = "";
        document.getElementById("<%=hidSearch1.Value%>").style.background = "yellow";
    }
    function search1Blur() {

        document.getElementById("<%=hidSearch1.Value%>").style.background = "";
        document.getElementById("<%=hidSearch1.Value%>").value = "<%=Resources.Site.NajitProjekt%>";
    }

<%end if %>
</script>
