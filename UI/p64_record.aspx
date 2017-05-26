<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="p64_record.aspx.vb" Inherits="UI.p64_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function report() {

            dialog_master("report_modal.aspx?prefix=p64&pid=<%=master.datapid%>", true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název:"></asp:Label></td>
            <td>
                <asp:TextBox ID="p64Name" runat="server" Style="width: 400px;"></asp:TextBox>
                <asp:HyperLink ID="linkX31" runat="server" Text="Tisková sestava" NavigateUrl="javascript:report()" style="margin-left:30px;"></asp:HyperLink>
            </td>
        </tr>
        
        <tr>
            <td>
                <asp:Label ID="lblLocation" Text="Lokalita:" runat="server" CssClass="lbl" AssociatedControlID="p64Location"></asp:Label>

            </td>
            <td>
                <asp:TextBox ID="p64Location" runat="server" Style="width: 400px;"></asp:TextBox>




            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOrdinary" Text="Index šanonu:" runat="server" CssClass="lbl" AssociatedControlID="p64Ordinary"></asp:Label></td>
            <td>

                <telerik:RadNumericTextBox ID="p64Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCode" Text="Jiný kód:" runat="server" CssClass="lbl" AssociatedControlID="p64Code"></asp:Label>

            </td>
            <td>
                <asp:TextBox ID="p64Code" runat="server" Style="width: 100px;"></asp:TextBox>




            </td>
        </tr>
    </table>
    <div>Podrobný popis:</div>
    <asp:TextBox ID="p64Description" runat="server" Style="width: 99%; height: 60px;" TextMode="MultiLine"></asp:TextBox>
    <asp:HiddenField ID="hidP41ID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
