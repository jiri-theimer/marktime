<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="b07_create.aspx.vb" Inherits="UI.b07_create" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="fileupload" Src="~/fileupload.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <table>
        <tr>
            <td>
                <uc:fileupload ID="upload1" runat="server" MaxFileInputsCount="1" EntityX29ID="b07Comment" />
            </td>
            <td>
                <uc:fileupload_list ID="uploadlist1" runat="server" />
            </td>
        </tr>
    </table>
    <p></p>
    <asp:TextBox ID="b07Value" runat="server" TextMode="MultiLine" Style="width: 100%; height: 100px; font-family: 'Courier New';"></asp:TextBox>

    <div class="content-box2">
        <div class="title">
            Komu poslat notifikaci komentáře
            <asp:Button ID="cmdAddReceiver" runat="server" CssClass="cmd" Text="Přidat" />
        </div>
        <div class="content">
            <asp:Repeater ID="rp1" runat="server">
                <ItemTemplate>
                    <div>
                        <span>Osoba:</span>
                        <uc:person ID="j02id" runat="server" Width="200px" Flag="all" />
                        <span>nebo tým:</span>
                        <asp:DropDownList ID="j11id" runat="server" DataTextField="j11Name" DataValueField="pid" Style="width: 200px;"></asp:DropDownList>
                        <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete_row.png" ToolTip="Odstranit položku" CssClass="button-link" />
                        <asp:HiddenField ID="p85id" runat="server" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>



    <uc:b07_list ID="history1" runat="server" ShowInsertButton="false" />

    <asp:HiddenField ID="hidPrefix" runat="server" />
    <asp:HiddenField ID="hidRecordPID" runat="server" />
    <asp:HiddenField ID="hidParentID" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
