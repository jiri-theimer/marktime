<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="entity_worksheet_summary.ascx.vb" Inherits="UI.entity_worksheet_summary" %>
<table cellpadding="3" style="table-layout: fixed;">
    <tr>
        <td></td>
        <td style="text-align:right;font-size:80%;">Hodiny</td>

        <td style="text-align:right;font-size:80%;">Ostatní</td>
    </tr>
    <tr>
     
        <td>Celkem vykázáno:
        </td>
        <td style="text-align:right;">
            <asp:Label ID="p31Hours_Orig" runat="server" CssClass="val"></asp:Label>
        </td>
        <td></td>
    </tr>
    <tr id="trWait4Approval" runat="server">
      
        <td><asp:hyperlink id="cmdApproving" runat="server" Text="Schválit rozpracovanost:"></asp:hyperlink></td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnApproval_Hours_Sum" runat="server" CssClass="val" ForeColor="red"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnApproval_Other_Sum" runat="server" CssClass="val" ForeColor="red"></asp:Label>
        </td>
    </tr>
    <tr id="trWait4Invoice" runat="server">
       
        <td>Schváleno, čeká na fakturaci:</td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnInvoice_Hours_Sum" runat="server" CssClass="val" ForeColor="green"></asp:Label>
        </td>
        <td style="text-align:right;">
            <asp:Label ID="WaitingOnInvoice_Other_Sum" runat="server" CssClass="val" ForeColor="green"></asp:Label>
        </td>
    </tr>
</table>
