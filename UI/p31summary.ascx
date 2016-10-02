<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p31summary.ascx.vb" Inherits="UI.p31summary" %>
<table class="table table-hover">
  
    <tr style="background-color:whitesmoke;">
        <td>            
        </td>
        <td title="Rozpracovanost" style="text-align:right;"><asp:Label ID="lblRozpracovano" runat="server" Text="ROZPR"></asp:Label></td>
        <td title="Fakturovat" style="text-align:right;"><asp:Label ID="lblFakturovat" runat="server" Text="FAK"></asp:Label></td>
        <td title="Paušál" style="text-align:right;">PAU</td>
        <td title="Odpis" style="text-align:right;">ODP</td>        
    </tr>
<asp:Repeater ID="rp1" runat="server">
<ItemTemplate>    
    <tr>
        <td>
            Hodiny:           
        </td>
        <td style="text-align:right;">
            <asp:Label ID="hodiny_rozpracovano" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="hodiny_fakturovat" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="hodiny_pausal" runat="server"></asp:Label>
        </td>
         <td style="text-align:right;">
            <asp:Label ID="hodiny_odpis" runat="server"></asp:Label>
        </td>
       
    </tr>
    <tr>
        <td>
            Honorář:
        </td>
        <td style="text-align:right;">
            <asp:Label ID="honorar_rozpracovano" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="honorar_fakturovat" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
           
        </td>
         <td style="text-align:right;">
            
        </td>
     
    </tr>
    <tr>
        <td>
            Výdaje:            
        </td>
        <td style="text-align:right;">
            <asp:Label ID="vydaje_rozpracovano" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="vydaje_fakturovat" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="vydaje_pausal" runat="server"></asp:Label>
        </td>
         <td style="text-align:right;">
            <asp:Label ID="vydaje_odpis" runat="server"></asp:Label>
        </td>
        
    </tr>
    <tr>
        <td>
            Ostatní odměny:            
        </td>
        <td style="text-align:right;">
            <asp:Label ID="odmeny_rozpracovano" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="odmeny_fakturovat" runat="server"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="odmeny_pausal" runat="server"></asp:Label>
        </td>
         <td style="text-align:right;">
            <asp:Label ID="odmeny_odpis" runat="server"></asp:Label>
        </td>
       
    </tr>
</ItemTemplate>
</asp:Repeater>
</table>

<asp:HiddenField ID="hidState" runat="server" Value="1" />