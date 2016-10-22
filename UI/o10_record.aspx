<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="o10_record.aspx.vb" Inherits="UI.o10_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
        <Tabs>
            <telerik:RadTab Text="Vlastnosti" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Okruh čtenářů článku"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">
            <table cellpadding="5" cellspacing="2" id="responsive">
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název (nadpis):"></asp:Label>

                    </td>
                    <td>
                        <asp:TextBox ID="o10Name" runat="server" Style="width: 600px;"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Umístění (kromě nástěnky):"></asp:Label>
                    </td>
                    <td>
                        
                        <asp:DropDownList ID="o10Locality" runat="server">
                            <asp:ListItem Text="Úvodní stránka a nástěnka" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Pouze nástěnka" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        
                        <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl" AssociatedControlID="o10Ordinary"></asp:Label>
                        <telerik:RadNumericTextBox ID="o10Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
                    </td>
                </tr>
            </table>

            <telerik:RadEditor ID="BodyHtml" ForeColor="Black" Font-Names="Verdana" runat="server" Width="98%" Height="500px" ToolbarMode="Default" Language="cs-CZ" NewLineMode ="Br" ContentAreaMode="Div">
                <Content>
     Obsah článku
                </Content>
                <Tools>
                    <telerik:EditorToolGroup>
                        <telerik:EditorTool Name="Print" />
                        <telerik:EditorTool Name="ToggleScreenMode" />
                        <telerik:EditorSeparator />

                        <telerik:EditorTool Name="Undo" />
                        <telerik:EditorTool Name="Redo" />
                        <telerik:EditorSeparator />

                        <telerik:EditorTool Name="Cut" />
                        <telerik:EditorTool Name="Copy" />
                        <telerik:EditorTool Name="Paste" />
                        <telerik:EditorSeparator />

                        <telerik:EditorTool Name="FontName" />
                        <telerik:EditorTool Name="FontSize" />

                        <telerik:EditorTool Name="Underline" />
                        <telerik:EditorTool Name="Bold" />
                        <telerik:EditorTool Name="Italic" />
                        <telerik:EditorTool Name="ForeColor" />
                        <telerik:EditorTool Name="BackColor" />
                        <telerik:EditorSeparator />

                        <telerik:EditorTool Name="FormatBlock" />
                        <telerik:EditorTool Name="JustifyLeft" />
                        <telerik:EditorTool Name="JustifyCenter" />
                        <telerik:EditorTool Name="JustifyRight" />
                        <telerik:EditorTool Name="JustifyFull" />
                        <telerik:EditorTool Name="JustifyNone" />

                        <telerik:EditorTool Name="InsertUnorderedList" />
                        <telerik:EditorTool Name="InsertOrderedList" />

                        <telerik:EditorTool Name="InsertParagraph" />
                        <telerik:EditorTool Name="Indent" />
                        <telerik:EditorTool Name="Outdent" />


                        <telerik:EditorTool Name="InsertHorizontalRule" />

                        <telerik:EditorTool Name="InsertTable" />
                        <telerik:EditorTool Name="InsertLink" />

                        <telerik:EditorSeparator />
                        <telerik:EditorTool Name="PasteFromWord" />
                        <telerik:EditorTool Name="PasteFromWordNoFontsNoSizes" />
                        <telerik:EditorTool Name="PasteHtml" />


                    </telerik:EditorToolGroup>
                </Tools>

            </telerik:RadEditor>
            <div class="div6">
                <span>Barva pozadí:</span>
                        <telerik:RadColorPicker ID="o10BackColor" runat="server" CurrentColorText="Vybraná barva" NoColorText="Bez barvy" ShowIcon="true" Preset="None">
                           
                            <telerik:ColorPickerItem Value="#F0F8FF"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FAEBD7"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#00FFFF"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#7FFFD4"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#F0FFFF"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#F5F5DC"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FFE4C4"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#00FFFF"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FFFAF0"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#F8F8FF"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FFD700"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#F0E68C"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#E6E6FA"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FFB6C1"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FFA500"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#AFEEEE"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FFDAB9"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#87CEEB"></telerik:ColorPickerItem>
                            <telerik:ColorPickerItem Value="#FF6347"></telerik:ColorPickerItem>
                          
                            
                        </telerik:RadColorPicker>
            </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="RadPageView2" runat="server">
            <div class="div6">
                <asp:Label ID="lblOwner" runat="server" Text="Vlastník záznamu:" CssClass="lblReq"></asp:Label>
                <uc:person ID="j02ID_Owner" runat="server" Width="300px" Flag="all" />
            </div>
            <div class="content-box2">
                <div class="title">
                    <img src="Images/projectrole.png" width="16px" height="16px" />
                    <asp:Label ID="ph1" runat="server" Text="Přístupová práva k sestavě"></asp:Label>
                    <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
                </div>
                <div class="content">
                    <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="o10NoticeBoard" EmptyDataMessage="K článku nejsou deffinována přístupová práva, proto bude přístupná pouze Vám."></uc:entityrole_assign>

                </div>
            </div>

        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
