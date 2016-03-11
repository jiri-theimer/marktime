<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SubForm.Master" CodeBehind="clue_p42_record.aspx.vb" Inherits="UI.clue_p42_record" %>

<%@ MasterType VirtualPath="~/SubForm.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-height: 270px; overflow-x: auto; overflow-y: auto; width: 100%;">
        <div>
           <img src="Images/setting_32.png" />
           <asp:Label ID="ph1" runat="server" CssClass="clue_header_span"></asp:Label>
       </div>

        <fieldset>
            <legend>Povolené sešity pro vykazování</legend>
            <ul>
                <asp:Repeater ID="rpP34" runat="server">
                    <ItemTemplate>
                        <li>
                            <%# Eval("p34Name")%>
                        </li>


                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </fieldset>
    </div>
</asp:Content>
