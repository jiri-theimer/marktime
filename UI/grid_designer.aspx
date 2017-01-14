<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="grid_designer.aspx.vb" Inherits="UI.grid_designer" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">


        function trydel() {

            if (confirm("Opravdu odstranit šablonu?")) {
                return (true);
            }
            else {
                return (false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="innerform_light" style="min-height: 55px;">
        <table cellpadding="5">
            <tr>
                <td>
                    <asp:Label ID="lblJ74Header" runat="server" Text="<%$Resources:grid_designer,SablonaSloupcu %>" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <uc:datacombo ID="j74ID" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Width="300px"></uc:datacombo>


                </td>
                <td>
                    <asp:Button ID="cmdNew" runat="server" CssClass="cmd" Text="<%$Resources:grid_designer,ZalozitNovouSablonu %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="<%$Resources:grid_designer,NazevSablony %>" CssClass="lblReq" AssociatedControlID="j74Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="j74Name" runat="server" CssClass="important_text" Style="width: 300px;" />
                </td>
                <td>
                    <asp:ImageButton ID="cmdSave" runat="server" ToolTip="Uložit změny v šabloně vč. jejího názvu" ImageUrl="Images/save.png" CssClass="button-link" />
                    <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" ToolTip="Odstranit šablonu sloupců" OnClientClick="return trydel();" CssClass="button-link" Style="margin-left: 40px;" />
                </td>

            </tr>
        </table>

    </div>


    <asp:HiddenField ID="j74IsSystem" runat="server" />

    <div style="float: left; width: 300px;">
        <div><%=Resources.grid_designer.DostupneSloupce %></div>
        <telerik:RadTreeView ID="tr1" runat="server" Skin="Default" ShowLineImages="false" SingleExpandPath="true" Height="500px">
        </telerik:RadTreeView>
    </div>
    <div style="float: left; padding: 10px;">
        <asp:Button ID="cmdAdd" runat="server" CssClass="cmd" Text=">" ToolTip="Vybrat sloupec do přehledu" />
        <p></p>
        <asp:Button ID="cmdRemove" runat="server" CssClass="cmd" Text="<" ToolTip="Odstranit sloupec z přehledu" />
    </div>
    <div style="float: left;">
        <div><%=Resources.grid_designer.VybraneSloupce %></div>
        <telerik:RadListBox ID="lt1" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="false" Culture="cs-CZ" Width="350px" SelectionMode="Single">

            <EmptyMessageTemplate>
                <div style="padding-top: 50px;">
                    <%=Resources.grid_designer.ZadneVybraneSloupce %>
                </div>
            </EmptyMessageTemplate>
        </telerik:RadListBox>

        <div style="margin-top: 20px;">
                    <asp:RadioButtonList ID="j74ScrollingFlag" runat="server" RepeatDirection="Vertical">
                        <asp:ListItem Text="Pevné ukotvení záhlaví tabulky (názvy sloupců)" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Ukotvení všeho nad tabulkou (filtrování a menu)" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Bez podpory ukotvení" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>

                </div>
                <div style="margin-top: 20px; display: none;">
                    <asp:Label ID="lblDrillDown" runat="server" Text="Drill-down v přehledu:"></asp:Label>
                    <asp:DropDownList ID="j74DrillDownField1" runat="server" DataTextField="ColumnHeader" DataValueField="ColumnField"></asp:DropDownList>
                </div>
                <div style="margin-top: 20px;">
                    <asp:CheckBox ID="j74IsFilteringByColumn" runat="server" CssClass="chk" Text="<%$Resources:grid_designer, NabizetSloupcovyFiltr%>" />
                </div>

                <div style="margin-top: 10px;">
                    <span><%=Resources.grid_designer.AutomatickyTriditPodle %> 1):</span>
                    <asp:DropDownList ID="cbxOrderBy1" runat="server" DataTextField="ColumnHeader" DataValueField="ColumnSqlSyntax_OrderBy"></asp:DropDownList>
                    <asp:DropDownList ID="cbxOrderBy1Dir" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:grid_designer,Sestupne %>" Value="DESC"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <span><%=Resources.grid_designer.AutomatickyTriditPodle %> 2):</span>
                    <asp:DropDownList ID="cbxOrderBy2" runat="server" DataTextField="ColumnHeader" DataValueField="ColumnSqlSyntax_OrderBy"></asp:DropDownList>
                    <asp:DropDownList ID="cbxOrderBy2Dir" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:grid_designer,Sestupne %>" Value="DESC"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div style="margin-top: 20px; display: none;">
                    <asp:CheckBox ID="j74IsVirtualScrolling" runat="server" CssClass="chk" Text="Zapnutá funkce [Virtual Scrolling]" />
                </div>
    </div>
    <div style="clear: both;"></div>
    

    <asp:Panel ID="panRoles" runat="server" CssClass="content-box2">
        <div class="title">
            <img src="Images/projectrole.png" width="16px" height="16px" />
            <asp:Label ID="Label1" runat="server" Text="Přístupová práva k přehledu pro další osoby"></asp:Label>
            <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
        </div>
        <div class="content">
            <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="j74SavedGridColTemplate" EmptyDataMessage="K šabloně nejsou definována přístupová práva, proto bude přístupná pouze Vám."></uc:entityrole_assign>

        </div>
    </asp:Panel>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
