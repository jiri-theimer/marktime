﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="j04_record.aspx.vb" Inherits="UI.j04_record" %>
<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
        <Tabs>
            <telerik:RadTab Text="Vlastnosti" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Další nastavení"></telerik:RadTab>
            
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="page1" runat="server" Selected="true">
            <table cellpadding="5" cellspacing="2">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblName" Text="Název role:" CssClass="lblReq"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="j04Name" runat="server" Style="width: 400px;"></asp:TextBox>
                    </td>
                </tr>

            </table>
            <div class="content-box2">
                <div class="title">Rozsah odkazů v nabídce hlavního aplikačního menu</div>
                <div class="content">
                    <asp:CheckBox ID="j04IsMenu_Worksheet" runat="server" Text="Menu [WORKSHEET]" Checked="true" />
                    <asp:CheckBox ID="j04IsMenu_Project" runat="server" Text="Menu [PROJEKTY]" Checked="true" />
                    <asp:CheckBox ID="j04IsMenu_Contact" runat="server" Text="Menu [KLIENTI]" Checked="true" />
                    <asp:CheckBox ID="j04IsMenu_People" runat="server" Text="Menu [LIDÉ]" Checked="false" />
                    <asp:CheckBox ID="j04IsMenu_Report" runat="server" Text="Menu [Sestavy]" Checked="true" />
                    <asp:CheckBox ID="j04IsMenu_Invoice" runat="server" Text="Menu [FAKTURY]" Checked="false" />
                    <asp:CheckBox ID="j04IsMenu_More" runat="server" Text="Menu [DALŠÍ]" Checked="true" />
                    <asp:CheckBox ID="j04IsMenu_MyProfile" runat="server" Text="Menu [Můj profil]" Checked="true" />
                </div>
            </div>

            
            <div class="content-box2">
                <div class="title">
                    <asp:label ID="ph1" runat="server" Text="Oprávnění aplikační role" />
                    
                    
                    <asp:Button ID="cmdUnCheckAll" runat="server" Text="Odškrtnout vše" style="float:right;" />
                    <asp:Button ID="cmdCheckAll" runat="server" Text="Zaškrtnout vše" style="float:right;"></asp:Button>
                </div>
                <div class="content">
                    <asp:CheckBoxList ID="x53ids" runat="server" DataValueField="pid" DataTextField="x53Name" RepeatColumns="1" CellPadding="8" CellSpacing="2"></asp:CheckBoxList>
                </div>
            </div>
            
        </telerik:RadPageView>
        <telerik:RadPageView ID="page2" runat="server">
            <table cellpadding="5" cellspacing="2">
                <tr style="vertical-align:top;">
                    <td>
                        <asp:Label runat="server" ID="lblPersonalPage" Text="Osobní (výchozí) stránka:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>
                        <uc:datacombo ID="j04Aspx_PersonalPage" runat="server" IsFirstEmptyRow="true" Width="400px" DataTextField="x31Name" DataValueField="PersonalPageValue"></uc:datacombo>
                        
                    </td>
                </tr>
                <tr style="vertical-align:top;">
                    <td>
                        <asp:Label runat="server" ID="lblPersonalPageMobile" Text="Osobní (výchozí) stránka pro mobilní zařízení:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>                      
                        <uc:datacombo ID="j04Aspx_PersonalPage_Mobile" runat="server" IsFirstEmptyRow="true" Width="400px" DataTextField="x31Name" DataValueField="PersonalPageValue"></uc:datacombo>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblj04Aspx_OneProjectPage" Text="Stránka pro detail projektu:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>                      
                        <asp:TextBox ID="j04Aspx_OneProjectPage" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblj04Aspx_OneContactPage" Text="Stránka pro detail klienta:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>                      
                        <asp:TextBox ID="j04Aspx_OneContactPage" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblj04Aspx_OneInvoicePage" Text="Stránka pro detail faktury:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>                      
                        <asp:TextBox ID="j04Aspx_OneInvoicePage" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label1" Text="Stránka pro detail osoby:" CssClass="lbl"></asp:Label>
                    </td>
                    <td>                      
                        <asp:TextBox ID="j04Aspx_OnePersonPage" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
