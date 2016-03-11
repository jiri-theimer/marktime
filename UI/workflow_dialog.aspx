<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="workflow_dialog.aspx.vb" Inherits="UI.workflow_dialog" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="fileupload" Src="~/fileupload.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="b07_list" Src="~/b07_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="div6">
        <asp:Label ID="lblCurrentStatus" runat="server" CssClass="valboldblue"></asp:Label>
    </div>
    <div class="content-box2">
        <div class="title">
            <span>Zvolte workflow krok:</span>

        </div>
        <div class="content">
            <asp:RadioButtonList ID="opgB06ID" runat="server" AutoPostBack="true" CssClass="chk" CellPadding="4" CellSpacing="2"></asp:RadioButtonList>
            
            <asp:Panel ID="panNominee" runat="server" Visible="false" CssClass="div6">
                <asp:Button ID="cmdAddNominee" runat="server" Text="Obsadit roli" CssClass="cmd" />
                <asp:Repeater ID="rpNominee" runat="server">
                    <ItemTemplate>
                        <div>
                        <asp:Image ID="img1" runat="server" ImageUrl="Images/projectrole_team.png"></asp:Image>
                        <span>Osoba:</span>
                        <uc:person ID="j02ID" runat="server" Width="200px" Flag="all" />
                        <span>nebo tým osob:</span>
                        <asp:DropDownList ID="j11id" runat="server" DataTextField="j11Name" DataValueField="pid" Style="width: 200px;" Font-Bold="true"></asp:DropDownList>
                        
                        <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete_row.png" ToolTip="Odstranit položku" CssClass="button-link" />
                        <asp:HiddenField ID="p85id" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
            </asp:Panel>

            <asp:TextBox ID="b07Value" runat="server" TextMode="MultiLine" Style="width: 99%; height: 100px; font-family: 'Courier New';" ToolTip="Doplňující komentář"></asp:TextBox>

            <table style="margin-bottom: 10px;">
                <tr>
                    <td>
                        <uc:fileupload ID="upload1" runat="server" MaxFileInputsCount="1" EntityX29ID="b07Comment" />
                    </td>
                    <td>
                        <uc:fileupload_list ID="uploadlist1" runat="server" />
                    </td>
                </tr>
            </table>
        </div>



    </div>
    <p></p>



    <asp:Panel ID="panHistory" runat="server" CssClass="content-box2">
        <div class="title">
            <span>Historie workflow a komentářů</span>

        </div>
        <div class="content">
            <uc:b07_list ID="history1" runat="server" ShowInsertButton="false" ShowHeader="False" />

        </div>
    </asp:Panel>

    <asp:HiddenField ID="hidPrefix" runat="server" />
    <asp:HiddenField ID="hidRecordPID" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
