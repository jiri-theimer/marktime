<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x31_record.aspx.vb" Inherits="UI.x31_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/pageheader.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload" Src="~/fileupload.ascx" %>
<%@ Register TagPrefix="uc" TagName="fileupload_list" Src="~/fileupload_list.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table cellpadding="5" cellspacing="2">
        <tr>
            <td>
                <uc:fileupload ID="upload1" runat="server" MaxFileUploadedCount="1" MaxFileInputsCount="1" InitialFileInputsCount="1" ButtonText_Add="Přidat" AllowedFileExtensions="trdx,rep,aspx,docx,xlsx" EntityX29ID="x31Report" />
            </td>
            <td>
                <uc:fileupload_list ID="uploadlist1" runat="server" />
            </td>
        </tr>
    </table>

    <div style="width: 100%; border-bottom: dashed 1px gray; margin-top: 6px; margin-bottom: 6px;"></div>

    <table cellpadding="5" cellspacing="2">
        <tr>
            <td colspan="2">
                <asp:RadioButtonList ID="x31FormatFlag" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CellPadding="10">
                    <asp:ListItem Text="<img src='Images/report.png' style='margin-right:5px;'/>Tisková sestava [TRDX]" Value="1" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="<img src='Images/plugin.png' style='margin-right:5px;'/>Plugin [ASPX]" Value="3"></asp:ListItem>
                    <asp:ListItem Text="<img src='Images/doc.png' style='margin-right:5px;'/>Slučovací dokument [DOCX]" Value="2"></asp:ListItem>
                    <asp:ListItem Text="<img src='Images/xls.png' style='margin-right:5px;'/>XLS export [XLSX]" Value="4"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblX29ID" runat="server" CssClass="lbl" Text="Kontext sestavy (entita):"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="x29ID" runat="server">
                    <asp:ListItem Text="Nemá vztah k vybranému záznamu, nabízí se přes menu [Sestavy]" Value="" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="Vybraný záznam projektu" Value="141"></asp:ListItem>
                    <asp:ListItem Text="Vybraný záznam klienta" Value="328"></asp:ListItem>
                    <asp:ListItem Text="Vybraný záznam osoby" Value="102"></asp:ListItem>
                    <asp:ListItem Text="Vybraný záznam úkolu" Value="356"></asp:ListItem>
                    <asp:ListItem Text="Vybraný záznam faktury" Value="391"></asp:ListItem>
                    <asp:ListItem Text="Vybraný záznam  zálohové faktury" Value="390"></asp:ListItem>
                    <asp:ListItem Text="Schvalování" Value="999"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název šablony:"></asp:Label></td>
            <td>
                <asp:TextBox ID="x31Name" runat="server" Style="width: 400px;"></asp:TextBox>
                <asp:Label ID="lblOrdinary" Text="Index pořadí v rámci kategorie:" runat="server" CssClass="lbl" AssociatedControlID="x31Ordinary"></asp:Label>
                <telerik:RadNumericTextBox ID="x31Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCode" runat="server" CssClass="lblReq" Text="Kód šablony:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="x31Code" runat="server" Style="width: 200px;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblJ25ID" Text="Kategorie:" runat="server" CssClass="lbl"></asp:Label>
            </td>
            <td>
                <uc:datacombo ID="j25ID" runat="server" DataTextField="j25Name" DataValueField="pid" IsFirstEmptyRow="true" Width="400px"></uc:datacombo>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="x31IsPeriodRequired" runat="server" Text="Sestava podporuje filtrování podle období" />
                <asp:CheckBox ID="x31IsUsableAsPersonalPage" runat="server" Text="Šablona je použitelná jako osobní (výchozí) stránka uživatele" />
            </td>
        </tr>
    </table>

    <div class="content-box2">
        <div class="title">
            <img src="Images/projectrole.png" width="16px" height="16px" />
            <asp:Label ID="ph1" runat="server" Text="Přístupová práva k sestavě"></asp:Label>
            <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
        </div>
        <div class="content">
            <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="x31Report" EmptyDataMessage="K sestavě nejsou deffinována přístupová práva, proto bude přístupná pouze administrátorům."></uc:entityrole_assign>

        </div>
    </div>
    <asp:Panel ID="panDocFormat" runat="server" CssClass="content-box2">
        <div class="title">
            <img src="Images/doc_32.png" />
        </div>
        <div class="content">
            <span>Zdrojový SQL dotaz:</span>
            <asp:TextBox ID="x31DocSqlSource" runat="server" style="width:99%;height:200px;" TextMode="MultiLine"></asp:TextBox>
            <br />
            <span>SQL vnořených tabulek (Název oblasti|SQL dotaz + ENTER):</span>
            <asp:TextBox ID="x31DocSqlSourceTabs" runat="server" style="width:99%;height:100px;" TextMode="MultiLine"></asp:TextBox>
        </div>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
