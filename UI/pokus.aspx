<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  

    <script type="text/javascript">
     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="cmdRemoveCookie" runat="server" Text="Remove cookie" />
    
    
    <asp:TextBox ID="txtMaskaFolders" runat="server" Width="300px" ></asp:TextBox>

    <hr />
    <asp:Button ID="cmdFolders" runat="server" Text="Přejmenovat složky" />

    <table>
    <asp:Repeater ID="rp1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="Project" runat="server"></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
</asp:Content>



