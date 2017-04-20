<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


    <script type="text/javascript">
        function gogo(par) {
            window.history.pushState({}, "Ahoj-titulek", "/pokus.aspx?gogo="+par);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    Hodiny: <asp:TextBox ID="txtHours" runat="server" Text="2,58333"></asp:TextBox>
    
    <asp:Button ID="cmdPokus" runat="server" Text="test" />


   
    <asp:TextBox ID="txtResult" runat="server"></asp:TextBox>

    <asp:HiddenField ID="hiddatapid" runat="server" />


    <button type="button" onclick="gogo('1')">Změnit URL 1</button>
    <button type="button" onclick="gogo('2')">Změnit URL 2</button>

    
</asp:Content>



