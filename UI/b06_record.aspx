<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="b06_record.aspx.vb" Inherits="UI.b06_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register TagPrefix="uc" TagName="pageheader" Src="~/PageHeader.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
        <Tabs>
            <telerik:RadTab Text="Vlastnosti" Selected="true"></telerik:RadTab>
            <telerik:RadTab Text="Kdo může spouštět krok"></telerik:RadTab>
            <telerik:RadTab Text="E-mail notifikace"></telerik:RadTab>
            <telerik:RadTab Text="Příkazy"></telerik:RadTab>
            <telerik:RadTab Text="SQL"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">

            <table cellpadding="3" cellspacing="2">
                <tr>
                    <td class="frif">
                        <asp:Label runat="server" ID="lblName" Text="Název kroku:" AssociatedControlID="b06Name" CssClass="lbl"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="b06Name" runat="server" Style="width: 300px;"></asp:TextBox>
                        <asp:CheckBox ID="b06IsKickOffStep" runat="server" Text="Jedná se o startovací krok celé šablony" CssClass="chk" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" Text="Cílový stav:" runat="server" CssClass="lbl"></asp:Label></td>
                    <td>
                        <uc:datacombo ID="b02ID_Target" runat="server" DataTextField="b02name" DataValueField="pid" IsFirstEmptyRow="true" Width="400px"></uc:datacombo>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="b06IsManualStep" runat="server" Text="Workflow krok je uživatelsky přístupný" Checked="true" AutoPostBack="true" CssClass="chk" />
                        <asp:CheckBox ID="b06IsRunOneInstanceOnly" runat="server" Text="Krok je povoleno spustit pouze jednou" CssClass="chk" />
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Datalabel1" CssClass="lbl" Text="Pořadí v nabídce kroků pro uživatele:"></asp:Label>
                        <telerik:RadNumericTextBox ID="b06Ordinary" runat="server" MinValue="-200" MaxValue="200" NumberFormat-DecimalDigits="0" Value="0" Width="50px" ShowSpinButtons="true">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
            </table>


            <div class="div6">
                <asp:CheckBox ID="b06IsCommentRequired" runat="server" Text="Uživatel má povinnost zapsat komentář" CssClass="chk" />

            </div>
            <div class="div6">
                <asp:CheckBox ID="b06IsNominee" runat="server" Text="V kroku může uživatel provést nominaci řešitelů" CssClass="chk" AutoPostBack="true" />

            </div>

            <asp:Panel ID="panNominee" runat="server">
                <fieldset>
                    <legend>Nominace řešitele</legend>

                    <span>Nominovaný obsadí roli:</span>
                    <asp:DropDownList ID="x67ID_Nominee" runat="server" DataTextField="x67Name" DataValueField="pid"></asp:DropDownList>
                    <div class="div6">
                        <asp:CheckBox ID="b06IsNomineeRequired" runat="server" Text="Nominace řešitelů je povinná" CssClass="chk" />

                    </div>
                </fieldset>
            </asp:Panel>

            <div class="div6">
                <asp:CheckBox ID="chkDirectNominee" runat="server" Text="V kroku dochází k automatické změně řešitele" CssClass="chk" AutoPostBack="true" />
            </div>
            <asp:Panel ID="panDirectNominee" runat="server">
                <fieldset style="padding: 6px;">
                    <legend>S krokem dochází automaticky ke změně řešitele</legend>

                    <span>Role nového řešitele:</span>
                    <asp:DropDownList ID="x67ID_Direct" runat="server" DataTextField="x67Name" DataValueField="pid"></asp:DropDownList>
                    <span>Obsazení přes tým osob:</span>
                    <asp:DropDownList ID="j11ID_Direct" runat="server" DataTextField="j11Name" DataValueField="pid"></asp:DropDownList>
                    <span style="padding-left:20px;">nebo změnit na řešitele posledního statusu:</span>
                    <asp:DropDownList ID="b02ID_LastReceiver_ReturnTo" runat="server" DataTextField="b02Name" DataValueField="pid"></asp:DropDownList>
                </fieldset>
            </asp:Panel>




        </telerik:RadPageView>

        <telerik:RadPageView ID="RadPageView2" runat="server">

            <asp:Panel ID="panB08" runat="server" Style="padding: 6px;">
                <div style="width: 100%; border-top: dotted gray 1px;">
                    <span>Kontextové role:</span>
                </div>
                <asp:CheckBoxList ID="chklX67IDs_B08" runat="server" RepeatDirection="Vertical" AutoPostBack="false" DataTextField="x67Name" DataValueField="pid"></asp:CheckBoxList>

                <div style="width: 100%; border-top: dotted gray 1px;">
                    <span>Týmy osob:</span>
                </div>
                <asp:CheckBoxList ID="chklJ11IDs_B08" runat="server" RepeatDirection="Vertical" AutoPostBack="false" DataTextField="j11Name" DataValueField="pid"></asp:CheckBoxList>


                <div style="width: 100%; border-top: dotted gray 1px;">
                    <span>Aplikační role:</span>
                </div>
                <asp:CheckBoxList ID="chklJ04IDs_B08" runat="server" RepeatDirection="Vertical" AutoPostBack="false" DataTextField="j04Name" DataValueField="pid"></asp:CheckBoxList>
            </asp:Panel>

        </telerik:RadPageView>

        <telerik:RadPageView ID="RadPageView3" runat="server">
            <uc:pageheader ID="phB03" runat="server" Text="Zprávy, které systém odešle po spuštění kroku:" IsInForm="true" />

            <div style="padding: 6px;">
                <asp:Button ID="cmdNewB11" runat="server" Text="Přidat" CssClass="cmd" />
            </div>
            <table cellpadding="6" cellspacing="3">
                <asp:Repeater ID="rpB11" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>Příjemce zprávy:
                            </td>
                            <td>
                                <asp:DropDownList ID="x67ID" runat="server" DataValueField="pid" DataTextField="x67Name" Style="width: 200px;"></asp:DropDownList>
                                nebo
                                <asp:DropDownList ID="j04ID" runat="server" DataValueField="pid" DataTextField="j04Name" Style="width: 160px;"></asp:DropDownList>
                                nebo
                                <asp:DropDownList ID="j11ID" runat="server" DataValueField="pid" DataTextField="j11Name" Style="width: 160px;"></asp:DropDownList>
                                <asp:HiddenField ID="p85id" runat="server" />
                                <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete.png" />
                            </td>
                        </tr>
                        <tr style="border-bottom: dotted 2px gray;">
                            <td>Šablona zprávy:
                            </td>
                            <td>
                                <asp:DropDownList ID="b65id" runat="server" DataValueField="pid" DataTextField="b65name"></asp:DropDownList>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>

        </telerik:RadPageView>





        <telerik:RadPageView ID="RadPageView4" runat="server">
            <p></p>
            <div class="innerform">
                <asp:DropDownList ID="cbxAddB09ID" runat="server" DataTextField="b09Name" DataValueField="b09ID" AutoPostBack="true"></asp:DropDownList>
                <asp:Button ID="cmdAddB09" runat="server" Text="Vložit vybraný příkaz" CssClass="cmd" />

            </div>
            <table cellpadding="6" cellspacing="3">
                <asp:Repeater ID="rpB10" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <b>
                                    <asp:Label ID="b09Name" runat="server"></asp:Label></b>
                                <asp:HiddenField ID="p85id" runat="server" />
                                <asp:HiddenField ID="b09id" runat="server" />
                            </td>

                            <td>
                                <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete.png" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>



        </telerik:RadPageView>



        <telerik:RadPageView ID="RadPageView5" runat="server">


            <div style="padding-top: 20px;">
                <asp:Label runat="server" ID="Datalabel4" Text="Validační SQL dotaz testující možnost spuštění kroku:"></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label4" runat="server" Text="(Pokud SQL vrací hodnotu 1, podmínka je splněna)"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="b06ValidateBeforeRunSQL" runat="server" TextMode="MultiLine" Style="width: 100%; height: 60px;"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblValidateBeforeMessage" runat="server" Text="Uživatelská hláška v případě nesplnění validačního dotazu:"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="b06ValidateBeforeErrorMessage" runat="server" Style="width: 100%;"></asp:TextBox>
            </div>
            <div style="padding-top: 20px;">
                <asp:Label runat="server" ID="Datalabel3" Text="Automatický posun | Krok bude automaticky spuštěn po splnění následujícího SQL dotazu:"></asp:Label>
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="(Splnění podmínky znamená, že SQL dotaz vrací hodnotu: 1 [typ: int], systém testuje nepřetržitě každých 5 minut)"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="b06ValidateAutoMoveSQL" runat="server" TextMode="MultiLine" Style="width: 100%; height: 60px;"></asp:TextBox>
            </div>
            <div style="padding-top: 6px;">
                <asp:Label runat="server" ID="Datalabel2" Text="S workflow krokem automaticky spouštět následující SQL dotaz:"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="b06RunSQL" runat="server" TextMode="MultiLine" Style="width: 100%; height: 60px;"></asp:TextBox>
            </div>
        </telerik:RadPageView>
    </telerik:RadMultiPage>


    <asp:HiddenField ID="hidcurx29id" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
