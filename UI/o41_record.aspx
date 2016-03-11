<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="o41_record.aspx.vb" Inherits="UI.o41_record" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="3" cellspacing="2">
        <tr>
            <td >
                <asp:Label ID="Label1" Text="Název účtu:" runat="server" CssClass="lblReq"></asp:Label></td>
            <td>
                <asp:TextBox ID="o41Name" runat="server" Style="width: 300px;" ></asp:TextBox>
               (jakýkoliv výraz)
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="Label2" Text="Adresa serveru:" runat="server" CssClass="lblReq"></asp:Label></td>
            <td>
                <asp:TextBox ID="o41Server" runat="server" Style="width: 300px;" ></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblo41Login" Text="Login účtu:" runat="server" CssClass="lblReq"></asp:Label></td>
            <td>
                <asp:TextBox ID="o41login" runat="server" Style="width: 300px;" ></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblo41Password" Text="Heslo:" runat="server" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:TextBox ID="o41Password" runat="server" Style="width: 300px;" TextMode="Password" ></asp:TextBox>
               <asp:Button ID="cmdChangePWD" runat="server" Text="Změnit heslo" CssClass="cmd" Visible="false" />
            </td>
        </tr>
        
        <tr>
            <td >
                <asp:Label ID="Label3" Text="Port:" runat="server" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:TextBox ID="o41Port" runat="server" Style="width: 50px;" ></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblo41Folder" Text="Složka:" runat="server" CssClass="lbl"></asp:Label></td>
            <td>
                <asp:TextBox ID="o41Folder" runat="server" Text="inbox" Style="width: 300px;" ></asp:TextBox>
               
            </td>
        </tr>
         <tr>
            <td colspan="2">
                <asp:CheckBox ID="o41IsDeleteMesageAfterImport" runat="server" Text="Po zpracování zprávu v mailboxu nenávratně odstranit" Checked="true" />
            </td>
        </tr>
       <tr>
           <td colspan="2">
                <asp:CheckBox ID="o41IsUseSSL" runat="server" Text="SSL komunikace" />
           </td>
       </tr>
       
       
       
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
