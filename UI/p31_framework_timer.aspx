<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="p31_framework_timer.aspx.vb" Inherits="UI.p31_framework_timer" %>

<%@ MasterType VirtualPath="~/Clue.Master" %>
<%@ Register TagPrefix="uc" TagName="timer" Src="~/timer.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        @media screen and (max-width: 600px) {

        .RadComboBox {
        width: 300px !important;
    }

            
        }
    </style>

    <script src="Scripts/jquery.timer.js" type="text/javascript"></script>

    <script type="text/javascript">
        function timer_change(ctl) {
            if (ctl.checked == true)
                window.open("p31_framework.aspx?showtimer=1", "_top");
            else
                window.open("p31_framework.aspx?showtimer=0", "_top");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-box2">
        <div class="title">
            <img src="Images/stopwatch.png" alt="STOPKY" />
            
            <asp:CheckBox ID="chkTimer" runat="server" Text="Zobrazovat STOPKY" AutoPostBack="false" Checked="true" onClick="timer_change(this)" meta:resourcekey="chkTimer" />
        </div>
        <div class="content">
            <uc:timer ID="timer1" runat="server" IsPanelView="true" IsIFrame="true"></uc:timer>
        </div>
    </div>

</asp:Content>
