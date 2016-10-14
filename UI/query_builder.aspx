<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="query_builder.aspx.vb" Inherits="UI.query_builder" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="period" Src="~/period.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <asp:Image ID="imgEntity" runat="server" />
        <asp:Label ID="ph1" runat="server" Text="Návrhář filtrů" CssClass="page_header_span"></asp:Label>
        <span style="padding-left: 20px;"></span>
        <asp:ImageButton ID="cmdNew" runat="server" ImageUrl="Images/new.png" CssClass="button-link" ToolTip="Založit nový filtr" />

    </div>

    <div class="div6">
        <asp:Label ID="lblJ70ID" runat="server" Text="Pracujete s filtrem:" CssClass="val" AssociatedControlID="j70ID"></asp:Label>
        <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 300px;" Font-Bold="true"></asp:DropDownList>


        <asp:HiddenField ID="j70IsSystem" runat="server" />
        <asp:HiddenField ID="hidIsOwner" runat="server" Value="0" />

        <asp:Label ID="lblName" runat="server" Text="Název filtru:" CssClass="lbl" AssociatedControlID="j70Name"></asp:Label>
        <asp:TextBox ID="j70Name" runat="server" Style="width: 300px; background-color: #99CCFF;" />
        <asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="Images/delete.png" ToolTip="Odstranit uložený filtr" CssClass="button-link" OnClientClick="return trydel();" />

    </div>
    <asp:Panel ID="panEditHeader" runat="server">
    </asp:Panel>

    <div class="div6">
        <asp:RadioButtonList ID="opgBin" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
            <asp:ListItem Text="Otevřené i v archivu" Value="0" Selected="true"></asp:ListItem>
            <asp:ListItem Text="Pouze otevřené" Value="1"></asp:ListItem>
            <asp:ListItem Text="Pouze v archivu" Value="2"></asp:ListItem>
        </asp:RadioButtonList>
    </div>

    <asp:panel ID="panQueryCondition" runat="server" CssClass="content-box2">
        <div class="title">
            Filtrovací pole
            <asp:Button ID="cmdClear" runat="server" CssClass="cmd" Text="Vyčistit podmínku filtru" UseSubmitBehavior="false" Style="margin-left: 40px;" />
        </div>
        <div class="content">
            <asp:RadioButtonList ID="opgField" runat="server" AutoPostBack="true" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="chk" CellPadding="4">
            </asp:RadioButtonList>

            <asp:Panel ID="panQueryItems" runat="server" CssClass="div6" Visible="false">
                <asp:Label ID="lbl1" runat="server" CssClass="lbl" Text="Vybrat hodnotu:"></asp:Label>
                <uc:datacombo ID="cbxItems" runat="server" DataValueField="pid" AutoPostBack="false" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>
                <uc:datacombo ID="cbxItemsExtension" runat="server" DataValueField="pid" AutoPostBack="false" IsFirstEmptyRow="true" Width="200px"></uc:datacombo>

                <asp:Button ID="cmdAdd2Query" runat="server" CssClass="cmd" Text="Přidat do podmínky filtru" />

            </asp:Panel>
            <asp:Panel ID="panQueryNonItems" runat="server" CssClass="div6" Visible="false">
                
                <span>Hodnota od:</span>
                <asp:TextBox ID="j71ValueFrom" runat="server" Width="100px"></asp:TextBox>
                <span> do: </span>
                <asp:TextBox ID="j71ValueUntil" runat="server" Width="100px"></asp:TextBox>

                <asp:Button ID="cmdAdd2QueryNonItems" runat="server" CssClass="cmd" Text="Přidat do podmínky filtru" />
            </asp:Panel>
            <asp:Panel ID="panQueryPeriod" runat="server" CssClass="div6" Visible="false">
                <uc:period ID="period1" runat="server" Caption="Filtrované období" />
                <asp:Button ID="cmdAdd2QueryPeriod" runat="server" CssClass="cmd" Text="Přidat do podmínky filtru" />
            </asp:Panel>
            <asp:Panel ID="panQueryString" runat="server" CssClass="div6" Visible="false">
                <asp:DropDownList ID="cbxStringOperator" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="CONTAIN" Text="Obsahuje"></asp:ListItem>
                    <asp:ListItem Value="START" Text="Začíná na"></asp:ListItem>
                    <asp:ListItem Value="EQUAL" Text="Je rovno"></asp:ListItem>
                    <asp:ListItem Value="NOTEMPTY" Text="Není prázdné"></asp:ListItem>
                    <asp:ListItem Value="EMPTY" Text="Je prázdné"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtStringValue" runat="server" Width="100px"></asp:TextBox>
                <asp:Button ID="cmdAdd2QueryString" runat="server" CssClass="cmd" Text="Přidat do podmínky filtru" />
            </asp:Panel>
        </div>
    </asp:panel>
    <asp:Panel ID="panJ71" runat="server" CssClass="content-box2">
        <div class="title">
            Podmínka filtru
            <asp:CheckBox ID="j70IsNegation" runat="server" Text="Negovat podmínku" ToolTip="Pokud zaškrtnuto, filtr vrací záznamy nevyhovující filtrovací podmínce." style="float:right;" />
        </div>
        <div class="content">
            <table cellpadding="3" cellspacing="2">
                <asp:Repeater ID="rpJ71" runat="server">
                    <ItemTemplate>
                        <tr class="trHover">
                            <td style="min-width: 150px;">
                                <asp:Label ID="x29Name" runat="server" CssClass="val" Style="color: Gray;"></asp:Label>
                                <asp:HiddenField ID="x29id" runat="server" />
                                <asp:HiddenField ID="p85id" runat="server" />
                                <asp:HiddenField ID="j71Field" runat="server" />
                            </td>
                            <td>
                                <i>
                                    <asp:Label ID="nebo" runat="server" CssClass="val" Style="color: blue;" Text="nebo"></asp:Label>
                                </i>
                                <asp:Label ID="j71RecordName" runat="server" CssClass="val"></asp:Label>
                                <asp:HiddenField ID="j71RecordPID" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="j71RecordName_Extension" runat="server" CssClass="val" Style="color: red;"></asp:Label>
                                <asp:HiddenField ID="j71RecordPID_Extension" runat="server" />
                            </td>
                            <td>
                                <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete.png" ToolTip="Odstranit položku" CssClass="button-link" />
                            </td>
                        </tr>
                    </ItemTemplate>

                </asp:Repeater>
            </table>
        </div>
    </asp:Panel>

    <p></p><p></p>
    <asp:panel ID="panRoles" runat="server" cssclass="content-box2">
        <div class="title">
            <img src="Images/projectrole.png" width="16px" height="16px" />
            <asp:Label ID="Label1" runat="server" Text="Přístupová práva k filtru pro další osoby"></asp:Label>
            <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
        </div>
        <div class="content">
            <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="j70QueryTemplate" EmptyDataMessage="K filtru nejsou definována přístupová práva, proto bude přístupný pouze Vám."></uc:entityrole_assign>

        </div>
    </asp:panel>

    <div style="padding: 6px; margin-top: 30px;">
        <i>
            <asp:Label ID="lblTimeStamp" runat="server"></asp:Label>
        </i>
    </div>

    <asp:HiddenField ID="hidPrefix" runat="server" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
