﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_report.aspx.vb" Inherits="UI.mobile_report" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=10.0.16.204, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:HyperLink ID="MasterRecord" runat="server" Visible="false" CssClass="alinked"></asp:HyperLink>
    </div>
    <asp:DropDownList ID="x31ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="NameWithFormat" Style="width: 100%;"></asp:DropDownList>
    <div style="padding:6px;">
        <asp:LinkButton ID="cmdRunDefaultReport" CssClass="btn btn-primary btn-xs" runat="server" Text="Zobrazit náhled vybrané sestavy" Visible="false"></asp:LinkButton>
    </div>
    <uc:periodcombo ID="period1" runat="server" Width="100%"></uc:periodcombo>
    <asp:HyperLink ID="cmdDocMergeResult" runat="server" Text="Zobrazit výsledek" Visible="false"></asp:HyperLink>
    <asp:HyperLink ID="cmdXlsResult" runat="server" Text="XLS výstup" Visible="false"></asp:HyperLink>

    <div id="divReportViewer">
        <telerik:ReportViewer ID="rv1" runat="server" Width="100%" ShowParametersButton="true" ShowHistoryButtons="false" ValidateRequestMode="Disabled">            
            <Resources PrintToolTip="Tisk" ExportSelectFormatText="Exportovat do zvoleného formátu" TogglePageLayoutToolTip="Přepnout na náhled k tisku" NextPageToolTip="Další strana" PreviousPageToolTip="Předchozí strana" RefreshToolTip="Obnovit" LastPageToolTip="Poslední strana" FirstPageToolTip="První strana" ></Resources>
        </telerik:ReportViewer>
    </div>

    <asp:HiddenField ID="hidX29ID" runat="server" />
    <asp:HiddenField ID="hidPrefix" runat="server" />
</asp:Content>