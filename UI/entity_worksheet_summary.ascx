<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="entity_worksheet_summary.ascx.vb" Inherits="UI.entity_worksheet_summary" %>
<table style="table-layout: fixed;padding:3px;">
    <tr>
        <td></td>
        <td style="text-align:right;font-size:80%;"><asp:Label ID="lblHodiny" runat="server" Text="Hodiny" meta:resourcekey="lblHodiny"></asp:Label></td>

        <td style="text-align:right;font-size:80%;"><asp:Label ID="lblOstatni" runat="server" Text="Ostatní" meta:resourcekey="lblOstatni"></asp:Label></td>
    </tr>
    <tr>
     
        <td><asp:Label ID="lblCelkemVykazano" runat="server" Text="Celkem vykázáno:" meta:resourcekey="lblCelkemVykazano"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="p31Hours_Orig" runat="server" CssClass="val"></asp:Label>
        </td>
        <td></td>
    </tr>
    <tr id="trWait4Approval" runat="server">
      
        <td><asp:hyperlink id="cmdApproving" runat="server" Text="Schválit rozpracovanost:" meta:resourcekey="cmdApproving"></asp:hyperlink></td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnApproval_Hours_Sum" runat="server" CssClass="val" ForeColor="red"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnApproval_Other_Sum" runat="server" CssClass="val" ForeColor="red"></asp:Label>
        </td>
    </tr>
    <tr id="trWait4Invoice" runat="server">
       
        <td><asp:Label ID="lblCekaNaFakturaci" runat="server" Text="Schváleno, čeká na fakturaci:" meta:resourcekey="lblCekaNaFakturaci"></asp:Label></td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnInvoice_Hours_Sum" runat="server" CssClass="val" ForeColor="green"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnInvoice_Other_Sum" runat="server" CssClass="val" ForeColor="green"></asp:Label>
        </td>
    </tr>
</table>
