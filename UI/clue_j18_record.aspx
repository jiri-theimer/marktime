﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_j18_record.aspx.vb" Inherits="UI.clue_j18_record" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>

<%@ Register TagPrefix="uc" TagName="entityrole_assign_preview" Src="~/entityrole_assign_preview.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <div>
           <img src="Images/setting_32.png" />
           <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
       </div>

        <div class="div6">
            <asp:Label ID="lblRolesHeader" runat="server" CssClass="framework_header_span" Text="Nastavení projektových rolí střediska, které se dědí do projektu"></asp:Label>
        </div>
        <uc:entityrole_assign_preview ID="roles_region" runat="server" EntityX29ID="j18Region" NoDataText="V tomto středisku nejsou nastaveny projektové role."></uc:entityrole_assign_preview>

    </div>
</asp:Content>
