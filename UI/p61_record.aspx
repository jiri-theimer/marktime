<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p61_record.aspx.vb" Inherits="UI.p61_record" %>
<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/PageHeader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-box2">
        <div class="title">
            Výběr aktivit
        </div>
        <div class="content">
            <div class="div6">
                <asp:Label ID="lblName" Text="Název výběru:" runat="server" CssClass="lblReq"></asp:Label>
                <asp:TextBox ID="p61Name" runat="server" Style="width: 300px;"></asp:TextBox>
            </div>
           
        </div>
    </div>

    <asp:Panel ID="panMembers" runat="server">
        <div style="padding: 6px;">
            <asp:Label ID="lblAdd" runat="server" Text="Vybrat aktivitu:" CssClass="lbl"></asp:Label>
            <uc:datacombo ID="p32ID" runat="server" AutoPostBack="false" DataTextField="NameWithSheet" DataValueField="pid" IsFirstEmptyRow="true" Width="400px" Filter="Contains"></uc:datacombo>

            <asp:Button ID="cmdAdd" runat="server" Text="Přidat do výběru" CssClass="cmd" />
            <span style="padding-left: 40px;"></span>
            <asp:Button ID="cmdRemoveSelected" runat="server" Text="Odebrat označené" CssClass="cmd" />
        </div>
        <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" AllowMultiSelect="true"></uc:datagrid>
    </asp:Panel>

    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
