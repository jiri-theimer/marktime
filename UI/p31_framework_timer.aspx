﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="p31_framework_timer.aspx.vb" Inherits="UI.p31_framework_timer" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content-box2">
        <div class="title">
            <img src="Images/stopwatch.png" alt="Časovač" />
            <asp:Label ID="lblItemsHeader" runat="server" Text="Časovač"></asp:Label>
        </div>
        <div class="content">
            <uc:timer ID="timer1" runat="server" IsPanelView="true" IsIFrame="true"></uc:timer>
        </div>
    </div>

</asp:Content>