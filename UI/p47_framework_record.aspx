<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p47_framework_record.aspx.vb" Inherits="UI.p47_framework_record" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  
    <table cellpadding="6">
        <tr>
            <th><asp:Label ID="Period" runat="server" CssClass="valboldblue"></asp:Label></th>
            <th title="Plán fakturovatelných hodin" style="background-color: #98FB98;">Fa</th>
            <th title="Plán nefakturovatelných hodin" style="background-color: #FFA07A;">NeFa</th>
            <th>
                <img src="Images/sum.png" />
            </th>
            <th>Plán mimo projekt</th>
            <th>Celkový osobní plán</th>
        </tr>
        <asp:Repeater ID="rp1" runat="server">
            <ItemTemplate>
                <tr class="trHover">
                    <td>
                        <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i" title="Kapacita osoby"></asp:HyperLink>
                        <asp:Label ID="Person" runat="server"></asp:Label>
                        
                        <asp:HiddenField ID="p85id" runat="server" />
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="p85FreeFloat01" EnabledStyle-HorizontalAlign="right" runat="server" Width="60px" ToolTip="Fakturovatelné hodiny" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>

                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="p85FreeFloat02" EnabledStyle-HorizontalAlign="right" runat="server" Width="60px" ToolTip="Nefakturovatelné hodiny" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>

                    </td>
                    <td >
                        
                        <asp:TextBox ID="p47HoursTotal" runat="server" ReadOnly="true" Width="50px" style="text-align: right;font-weight:bold;"></asp:TextBox>
                    </td>
                    <td style="text-align:right;">
                        <asp:Label ID="Plan_MimoProjekt" runat="server" CssClass="valbold"></asp:Label>
                        <asp:HyperLink ID="clue_oplan" runat="server" CssClass="reczoom" Text="i" title="Detail osobního plánu"></asp:HyperLink>
                    </td>
                    <td style="text-align:right;">
                        <asp:Label ID="Plan_Celkem" runat="server" CssClass="valboldblue"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr class="trHover">
            <th><img src="Images/sum.png" /></th>
            <th style="text-align:right;"><asp:Label ID="Total_Fa" runat="server" CssClass="valbold" BackColor="#98FB98"></asp:Label></th>
            <th style="text-align:right;"><asp:Label ID="Total_Nefa" runat="server" CssClass="valbold" BackColor="#FFA07A"></asp:Label></th>
            <th style="text-align:right;"><asp:Label ID="Total_Project" runat="server" CssClass="valbold"></asp:Label></th>
            <th></th>
            <th></th>
        </tr>
    </table>

    <asp:Panel ID="panAddPerson" runat="server" CssClass="content-box2">
        <div class="title">
            <img src="Images/person.png" />
            Přidat do plánu další osobu
            
        </div>
        <div class="content">
            <uc:person ID="j02ID_Add" runat="server" Width="300px" />
            <asp:Button ID="cmdAddPerson" runat="server" Text="Přidat" CssClass="cmd" />
        </div>
    </asp:Panel>
    <asp:Panel ID="panProject" runat="server" CssClass="content-box2">
        <div class="title">
            <asp:HyperLink ID="clue_project" runat="server" CssClass="reczoom" Text="i" title="Detail projektu"></asp:HyperLink>
            <asp:label ID="Project" runat="server"></asp:label>
            
        </div>
        <div class="content">
            <table cellpadding="10">
                <tr>
                    <td>Časový rozsah plánu:</td>
                    <td>
                        <asp:Label ID="PlanScope" runat="server" CssClass="valbold"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
