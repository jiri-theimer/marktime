<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  

      
    </style>

    <script type="text/javascript">
        function gogo(par) {
            window.history.pushState({}, "Ahoj-titulek", "/pokus.aspx?gogo="+par);
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    

    <asp:Button ID="cmdPokus" runat="server" Text="Generovat" />


    <hr />
    <span>j02ids:</span><asp:TextBox ID="txt1" runat="server" text="3,1,4"></asp:TextBox>
    <span>j11ids:</span><asp:TextBox ID="txt2" runat="server" Text=""></asp:TextBox>
    <hr />
    <asp:TextBox ID="txt3" runat="server" Width="800px" Height="100px"></asp:TextBox>
</asp:Content>



