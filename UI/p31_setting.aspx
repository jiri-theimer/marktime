<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p31_setting.aspx.vb" Inherits="UI.p31_setting" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <p></p>
    <asp:Panel ID="panT" runat="server" CssClass="content-box2">
        <div class="title">
            Nastavení zapisování hodin
        </div>
        <div class="content">
            
            <asp:RadioButtonList ID="opgHoursEntryFlag" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Hodiny vyplňovat dekadickým číslem (příklad: 1,5) nebo HH:mm formátem (příklad: 01:30)" Value="1" Selected="true"></asp:ListItem>
                               
                <asp:ListItem Text="Čas zadávat v minutách (celé číslo)" Value="2"></asp:ListItem>
            </asp:RadioButtonList>

            <div class="div6">
                
            </div>

            <table cellpadding="6" id="responsive">
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkShowTimeInterval" runat="server" Text="Kromě hodin nabízet možnost vyplnit přesný čas od - do" CssClass="chk" />
                    </td>
                </tr>
                <tr>                    
                    <td>
                        Nabídka předvyplněných intervalů pro pole [Hodiny]:
                    </td>
                    <td>
                        <asp:DropDownList ID="p31_HoursInputInterval" runat="server">
                            <asp:ListItem Text="Po 5ti minutách" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Po 15ti minutách" Value="15"></asp:ListItem>
                            <asp:ListItem Text="Po 30ti minutách" Value="30"></asp:ListItem>
                            <asp:ListItem Text="Po 60ti minutách" Value="60"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>                    
                    <td>
                        Formát předvyplněných intervalů pro pole [Hodiny]:
                    </td>
                    <td>
                        <asp:DropDownList ID="p31_HoursInputFormat" runat="server">
                            <asp:ListItem Text="Dekadické číslo (např. 1,5)" Value="dec" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="HH:mm (např. 01:30)" Value="hhmm"></asp:ListItem>
                            
                        </asp:DropDownList>
                    </td>
                </tr>                       
                <tr>                    
                    <td>
                        Velikost intervalů pro předvyplněné čas v polích [Začátek]/[Konec]:
                    </td>
                    <td>
                        <asp:DropDownList ID="p31_TimeInputInterval" runat="server">                            
                            <asp:ListItem Text="Po 30ti minutách" Value="30"></asp:ListItem>
                            <asp:ListItem Text="Po 60ti minutách" Value="60"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>                    
                    <td>
                        Nabídka intervalů v poli [Začátek]/[Konec] začíná od:
                    </td>
                    <td>
                        <asp:DropDownList ID="p31_TimeInput_Start" runat="server">   
                            <asp:ListItem Text="05:00" Value="5"></asp:ListItem>                         
                            <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                            <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                            <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                            <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12:00" Value="12"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>                    
                    <td>
                        Nabídka intervalů v poli [Začátek]/[Konec] končí v:
                    </td>
                    <td>
                        <asp:DropDownList ID="p31_TimeInput_End" runat="server">   
                            <asp:ListItem Text="15:00" Value="15"></asp:ListItem>
                            <asp:ListItem Text="16:00" Value="16"></asp:ListItem>
                            <asp:ListItem Text="17:00" Value="17"></asp:ListItem>                         
                            <asp:ListItem Text="18:00" Value="18"></asp:ListItem>
                            <asp:ListItem Text="19:00" Value="19"></asp:ListItem>
                            <asp:ListItem Text="20:00" Value="20"></asp:ListItem>
                            <asp:ListItem Text="21:00" Value="21"></asp:ListItem>
                            <asp:ListItem Text="22:00" Value="22"></asp:ListItem>
                            <asp:ListItem Text="23:00" Value="23"></asp:ListItem>
                            
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
