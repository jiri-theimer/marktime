<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="license.aspx.vb" Inherits="UI.license" %>
<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadUpload ID="upload1" runat="server" InputSize="35" InitialFileInputsCount="1" AllowedFileExtensions="licx" MaxFileInputsCount="1" ControlObjectsVisibility="None">
    </telerik:RadUpload>
   
    <asp:Panel ID="panGuru" runat="server" Visible="false">
        <p></p>
        <fieldset>
            <legend>Vygenerovat licenční klíč</legend>
            <table cellpadding="6">
                <tr>
                    <td>MaxUsers:<asp:TextBox ID="txtMaxUsers" runat="server" Text="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>License subject:<asp:TextBox ID="txtLicenseSubject" runat="server" Style="width: 400px;"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="TaxInvoices" runat="server" Text="Fakturace" />
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:CheckBox ID="Workflow" runat="server" Text="Workflow" />
                    </td>
                </tr>
                
              
            
            </table>
            <asp:Button ID="cmdGenerate" Text="Vygenerovat licenční klíč" runat="server" CssClass="cmd" />
            <table cellpadding="6">
                <tr valign="top">
                    <td>Zašifrovaný klíč:</td>
                    <td>
                        <asp:TextBox ID="NewKey_Crypted" runat="server" Style="width: 600px; height: 100px;" TextMode="MultiLine"></asp:TextBox>

                    </td>
                </tr>
                <tr valign="top">
                    <td>Rozšifrovaný klíč:</td>
                    <td>
                        <asp:Label ID="NewKey_Decrypted" runat="server"></asp:Label></td>
                </tr>
            </table>


        </fieldset>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
