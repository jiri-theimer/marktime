<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="pokus.aspx.vb" Inherits="UI.pokus" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  

    <script type="text/javascript">
        function gogo(par) {
            window.history.pushState({}, "Ahoj-titulek", "/pokus.aspx?gogo="+par);
            
        }

        function requesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            //Data passed to the service.
            document.getElementById("<%=txtPokus.clientid%>").value = sender.get_text();
            
            context["prefix"] = "p41";
        }

        function entryAdding(sender, eventArgs) {            
            if (eventArgs.get_entry().get_value() == "") {
                eventArgs.set_cancel(true);
            }
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAutoCompleteBox ID="tags1" runat="server" RenderMode="Lightweight" EmptyMessage="Uveďte název štítku" Width="400px" OnClientEntryAdding="entryAdding" OnClientRequesting="requesting">     
        <WebServiceSettings Method="LoadComboData" Path="~/Services/tag_service.asmx"/>   
        <Localization ShowAllResults="Zobrazit všechny výsledky" RemoveTokenTitle="Zrušit výběr štítku" />
        
    </telerik:RadAutoCompleteBox>
    
    <asp:Literal Text="Popisek přes literal:" runat="server"></asp:Literal>
    <asp:TextBox ID="txtPokus" runat="server"></asp:TextBox>

    <hr />
    <asp:Literal Text="Popisek přes literal:" runat="server"></asp:Literal>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

    <asp:Button ID="cmdPokus" runat="server" Text="Generovat" />
    <asp:Button ID="cmdFormat" runat="server" Text="Format date" />

    <hr />
    <span>j02ids:</span><asp:TextBox ID="txt1" runat="server" text="MM/DD/YYYY"></asp:TextBox>
    <span>j11ids:</span><asp:TextBox ID="txt2" runat="server" Text=""></asp:TextBox>
    <hr />
    <asp:TextBox ID="txt3" runat="server" Width="800px" Height="100px"></asp:TextBox>
</asp:Content>



