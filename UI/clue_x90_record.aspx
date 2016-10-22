<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Clue.Master" CodeBehind="clue_x90_record.aspx.vb" Inherits="UI.clue_x90_record" %>
<%@ MasterType VirtualPath="~/Clue.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <div>
           <asp:image ID="img1" runat="server" imageurl="Images/record_32.png" />
           <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
       </div>

        <table cellpadding="10" cellspacing="2">
            <tr>
                <td>Akce:</td>
                <td>
                    <asp:Label ID="Event" runat="server" CssClass="valbold" style="color:red;"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>Čas:</td>
                <td>
                    <asp:Label ID="x90Date" runat="server" CssClass="valbold"></asp:Label>

                </td>
            </tr>
            
            <tr>
                <td>Autor:</td>
                <td>
                    <asp:Label ID="Author" runat="server" CssClass="valbold"></asp:Label>

                </td>
            </tr>

        </table>


        
    </div>
</asp:Content>
