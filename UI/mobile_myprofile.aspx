<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Mobile.Master" CodeBehind="mobile_myprofile.aspx.vb" Inherits="UI.mobile_myprofile" %>
<%@ MasterType VirtualPath="~/Mobile.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h3>
            <asp:Label ID="lblUser" runat="server"></asp:Label><small>, MARKTIME profil</small>
        </h3>

    </div>
     <div class="container-fluid">
        <div id="row1" class="row">
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/setting.png" />
                        Nastavení
                        <asp:LinkButton ID="cmdSave" CssClass="btn btn-primary btn-xs" runat="server" Text="Uložit změny"></asp:LinkButton>
                    </div>
                    

                    <asp:RadioButtonList ID="j03MobileForwardFlag" runat="server" RepeatDirection="Vertical">
                        <asp:ListItem Text="Úvodní stránka systému automaticky detekuje, zda se přepnout do mobilního rozhraní" Value="0" class="radio-inline"></asp:ListItem>
                        <asp:ListItem Text="Systém se nebude snažit o automatickou detekci mobilního zařízení" Value="1" class="radio-inline"></asp:ListItem>
                    </asp:RadioButtonList>

                </div>
            </div>

            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/user.png" />
                        Uživatelský účet
                    </div>                    
                    <table class="table table-hover">
                        <tr>
                            <td>Přihlašovací jméno:</td>
                            <td><asp:Label ID="j03Login" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Aplikační role:</td>
                            <td><asp:Label ID="j04Name" runat="server"></asp:Label></td>
                        </tr>
                        
                    </table>
                </div>
            </div>

            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <div class="caption">
                        <img src="Images/person.png" />
                        Osobní profil
                    </div>
                    <table class="table table-hover">
                        <tr>
                            <td>Osoba:</td>
                            <td><asp:Label ID="Person" runat="server"></asp:Label></td>
                        </tr>
                         <tr>
                            <td>E-mail:</td>
                            <td><asp:Label ID="j02Email" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Pozice:</td>
                            <td><asp:Label ID="j07Name" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Členství v týmech:</td>
                            <td><asp:Label ID="Teams" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Středisko:</td>
                            <td><asp:Label ID="j18Name" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
