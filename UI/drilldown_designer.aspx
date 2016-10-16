<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="drilldown_designer.aspx.vb" Inherits="UI.drilldown_designer" %>

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
                    <asp:Label ID="lblJ75Header" runat="server" Text="Vybrat šablonu:" CssClass="lbl"></asp:Label>
                </td>
                <td>
                    <uc:datacombo ID="j75ID" runat="server" AutoPostBack="true" DataTextField="j75Name" DataValueField="pid" Width="300px"></uc:datacombo>


                </td>
                <td>
                    <asp:Button ID="cmdNew" runat="server" CssClass="cmd" Text="Založit novou šablonu" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="<%$Resources:grid_designer,NazevSablony %>" CssClass="lblReq" AssociatedControlID="j75Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="j75Name" runat="server" CssClass="important_text" Style="width: 300px;" />
                </td>
                <td>
                    <asp:ImageButton ID="cmdSave" runat="server" ToolTip="Uložit změny v šabloně vč. jejího názvu" ImageUrl="Images/save.png" CssClass="button-link" />
                    <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" ToolTip="Odstranit šablonu" OnClientClick="return trydel();" CssClass="button-link" Style="margin-left: 40px;" />
                </td>

            </tr>
        </table>

    </div>


    <telerik:RadTabStrip ID="tabs1" runat="server" ShowBaseLine="true" AutoPostBack="true">
        <Tabs>
            <telerik:RadTab Text="Úroveň #1" Selected="true" Value="1"></telerik:RadTab>
            <telerik:RadTab Text="Úroveň #2" Value="2"></telerik:RadTab>
            <telerik:RadTab Text="Úroveň #3" Value="3"></telerik:RadTab>
            <telerik:RadTab Text="Úroveň #4" Value="4"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <div class="div6">
        <asp:DropDownList ID="j75Level" runat="server" AutoPostBack="true" DataTextField="Caption" DataValueField="FieldTypeID">           
        </asp:DropDownList>
    </div>
    <table cellpadding="8">
        <tr valign="top">
            <td>
                <div><%=Resources.grid_designer.DostupneSloupce %></div>
                <telerik:RadListBox ID="colsSource" Height="200px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="colsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="true" AutoPostBackOnDelete="true" AutoPostBackOnTransfer="true">
                    <ButtonSettings TransferButtons="All" ShowTransferAll="false" />

                    <Localization ToRight="Přesunout" ToLeft="Odebrat" AllToRight="Přesunout vše" AllToLeft="Odbrat vše" MoveDown="Posunout dolu" MoveUp="Posunout nahoru" />
                </telerik:RadListBox>
            </td>
            <td>
                <div><%=Resources.grid_designer.VybraneSloupce %></div>
                <telerik:RadListBox ID="colsDest" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="true" Culture="cs-CZ" Width="350px" SelectionMode="Single">

                    <EmptyMessageTemplate>
                        <div style="padding-top: 50px;">
                            <%=Resources.grid_designer.ZadneVybraneSloupce %>
                        </div>
                    </EmptyMessageTemplate>
                </telerik:RadListBox>

            </td>

        </tr>
    </table>

    <asp:HiddenField ID="j75IsSystem" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidLevel1" runat="server" />
    <asp:HiddenField ID="hidLevel2" runat="server" />
    <asp:HiddenField ID="hidLevel3" runat="server" />
    <asp:HiddenField ID="hidLevel4" runat="server" />
    <asp:HiddenField ID="hidCols1" runat="server" />
    <asp:HiddenField ID="hidCols2" runat="server" />
    <asp:HiddenField ID="hidCols3" runat="server" />
    <asp:HiddenField ID="hidCols4" runat="server" />



    <asp:Panel ID="panRoles" runat="server" CssClass="content-box2">
        <div class="title">
            <img src="Images/projectrole.png" width="16px" height="16px" />
            <asp:Label ID="Label1" runat="server" Text="Přístupová práva k drill-down šabloně pro další osoby"></asp:Label>
            <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
        </div>
        <div class="content">
            <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="j75DrillDownTemplate" EmptyDataMessage="K šabloně nejsou definována přístupová práva, proto bude přístupná pouze Vám."></uc:entityrole_assign>

        </div>
    </asp:Panel>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>

