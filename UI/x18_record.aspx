<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalDataRecord.Master" CodeBehind="x18_record.aspx.vb" Inherits="UI.x18_record" %>

<%@ MasterType VirtualPath="~/ModalDataRecord.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datacombo" Src="~/datacombo.ascx" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="entityrole_assign" Src="~/entityrole_assign.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        

        function hardrefresh(pid, flag) {
            document.getElementById("<%=HardRefreshPID.ClientID%>").value = pid;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefresh, "", False)%>;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true">
        <Tabs>
            <telerik:RadTab Text="Vlastnosti" Selected="true" Value="core"></telerik:RadTab>
            <telerik:RadTab Text="Uživatelská pole" Value="ff"></telerik:RadTab>
            <telerik:RadTab Text="Ostatní" Value="other"></telerik:RadTab>

        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="core" runat="server" Selected="true">
            <table cellpadding="5" cellspacing="2">
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server" CssClass="lblReq" Text="Název štítku:"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="x18Name" runat="server" Style="width: 400px;"></asp:TextBox>
                        <asp:Label ID="lblOrdinary" Text="Index pořadí:" runat="server" CssClass="lbl"></asp:Label>
                        <telerik:RadNumericTextBox ID="x18Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <span class="lbl">Zkrácený název:</span>
                    </td>
                    <td>
                        <asp:TextBox ID="x18NameShort" runat="server"></asp:TextBox>
                    </td>
                </tr>

            </table>
            


            <div class="content-box2" style="margin-top: 20px;">
                <div class="title">
                    Vazba štítku na entity
                    <asp:DropDownList ID="x29ID_addX20" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="--Vyberte entitu--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Projekt" Value="141"></asp:ListItem>
                        <asp:ListItem Text="Klient" Value="328"></asp:ListItem>
                        <asp:ListItem Text="Úkol" Value="356"></asp:ListItem>
                        <asp:ListItem Text="Událost v kalendáři" Value="222"></asp:ListItem>
                        <asp:ListItem Text="Dokument" Value="223"></asp:ListItem>
                        <asp:ListItem Text="Vystavená faktura" Value="391"></asp:ListItem>
                        <asp:ListItem Text="Zálohová faktura" Value="390"></asp:ListItem>
                        <asp:ListItem Text="Osoba" Value="102"></asp:ListItem>
                        <asp:ListItem Text="Worksheet úkon" Value="331"></asp:ListItem>
                        <asp:ListItem Text="Jiný štítek" Value="925"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="x20EntityTypePID_addX20" runat="server"></asp:DropDownList>
                    <asp:Button ID="cmdAddX20" runat="server" CssClass="cmd" Text="Vložit vybranou vazbu" />
                </div>
                <div class="content">
                    <table cellpadding="10">

                        <asp:Repeater ID="rpX20" runat="server">
                            <ItemTemplate>
                                <tr class="trHover">
                                    <td>
                                        <div>
                                            <span>Vazba na entitu:</span>
                                        </div>
                                        <div>
                                            <span>Název vazby (nepovinné):</span>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="x29ID" runat="server" Enabled="false">
                                                <asp:ListItem Text="Projekt" Value="141"></asp:ListItem>
                                                <asp:ListItem Text="Klient" Value="328"></asp:ListItem>
                                                <asp:ListItem Text="Úkol" Value="356"></asp:ListItem>
                                                <asp:ListItem Text="Událost v kalendáři" Value="222"></asp:ListItem>
                                                <asp:ListItem Text="Dokument" Value="223"></asp:ListItem>
                                                <asp:ListItem Text="Vystavená faktura" Value="391"></asp:ListItem>
                                                <asp:ListItem Text="Zálohová faktura" Value="390"></asp:ListItem>
                                                <asp:ListItem Text="Osoba" Value="102"></asp:ListItem>
                                                <asp:ListItem Text="Worksheet úkon" Value="331"></asp:ListItem>
                                                <asp:ListItem Text="Jiný štítek" Value="925"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="x20EntityTypePID_Alias" runat="server" ForeColor="Green"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:TextBox ID="x20Name" runat="server" Width="200px"></asp:TextBox>
                                            <asp:HiddenField ID="x20EntityTypePID" runat="server" />
                                            <asp:HiddenField ID="x29ID_EntityType" runat="server" />
                                            
                                        </div>
                                        
                                    </td>

                                    <td>
                                        <div>
                                            <asp:CheckBox ID="x20IsMultiselect" runat="server" CssClass="chk" Text="Multi-Select (možnost zaškrtnout více položek najednou)" />
                                        </div>
                                        <div>
                                            <asp:CheckBox ID="x20IsEntryRequired" runat="server" CssClass="chk" Text="Povinná vazba k přiřazení" />
                                        </div>
                                        
                                        <div>
                                            <asp:CheckBox ID="x20IsClosed" runat="server" Text="Vazba uzavřena pro přiřazování" />
                                        </div>

                                    </td>
                                    <td>
                                        <div>
                                            <asp:DropDownList ID="x20EntryModeFlag" runat="server">
                                                <asp:ListItem Text="Vazbu vyplňovat odkazem na položku štítku (combo-list v záznamu entity)" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Vazbu vyplňovat odkazem na záznam entity (přímo v záznamu položky štítku)" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:DropDownList ID="x20GridColumnFlag" runat="server">
                                                <asp:ListItem Text="Sloupec v přehledu záznamů entity" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Sloupec v přehledu položek štítku" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Sloupec v entitním přehledu i v přehledu položek" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Nezobrazovat jako sloupec" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:DropDownList ID="x20EntityPageFlag" runat="server">
                                                <asp:ListItem Text="Na stránce záznamu entity zobrazovat jako info" Value="1"></asp:ListItem>   
                                                <asp:ListItem Text="Na stránce záznamu entity zobrazovat jako odkaz" Value="2"></asp:ListItem> 
                                                <asp:ListItem Text="Na stránce záznamu entity zobrazovat jako odkaz + tlačítko [Přidat]" Value="3"></asp:ListItem>                                                                                            
                                                <asp:ListItem Text="Na stránce záznamu entity nic nezobrazovat" Value="9"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <span>Pořadí:</span>
                                            <telerik:RadNumericTextBox ID="x20Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
                                        </div>
                                    </td>

                                    <td>
                                        <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete_row.png" ToolTip="Odstranit položku" CssClass="button-link" />
                                        <asp:HiddenField ID="p85id" runat="server" />
                                        <asp:HiddenField ID="x20ID" runat="server" />
                                        
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>

            <asp:panel ID="panX23" runat="server" CssClass="div6">
                <asp:RadioButtonList ID="opg1" runat="server" AutoPostBack="true" Visible="false" RepeatDirection="Vertical">
                    <asp:ListItem Text="Štítek bude mít vlastní zdroj položek" Value="1" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="Štítek bude využívat již existující zdroj položek" Value="2"></asp:ListItem>
                </asp:RadioButtonList>

                <div style="margin-top:20px;">
                    <asp:Label ID="lblX23ID" Text="Datový zdroj položek štítku:" runat="server" CssClass="lblReq"></asp:Label>
                                <uc:datacombo ID="x23ID" runat="server" DataTextField="x23Name" DataValueField="pid" IsFirstEmptyRow="true" AutoPostBack="true" Width="400px"></uc:datacombo>
                </div>
            </asp:panel>



        </telerik:RadPageView>

        <telerik:RadPageView ID="ff" runat="server">
            <div class="content-box2" style="margin-top: 10px;">
                <div class="title">
                    <img src="Images/form.png" width="16px" height="16px" />
                    Rozšíření položky štítku o další pole
                    <asp:Button ID="cmdNewX16" runat="server" CssClass="cmd" Text="Přidat" />
                </div>
                <div class="content">
                    <table cellpadding="4">
                        <tr>
                            <th>Pole</th>
                            <th>Název</th>
                            <th></th>
                            <th>#</th>
                            <th>Možné hodnoty textového pole</th>
                            <th></th>
                        </tr>
                        <asp:Repeater ID="rpX16" runat="server">
                            <ItemTemplate>
                                <tr class="trHover" valign="top">
                                    <td>

                                        <asp:DropDownList ID="x16Field" runat="server">
                                            <asp:ListItem Text="--Obsazené pole--" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Text 1" Value="x25FreeText01"></asp:ListItem>
                                            <asp:ListItem Text="Text 2" Value="x25FreeText02"></asp:ListItem>
                                            <asp:ListItem Text="Text 3" Value="x25FreeText03"></asp:ListItem>
                                            <asp:ListItem Text="Text 4" Value="x25FreeText04"></asp:ListItem>
                                            <asp:ListItem Text="Text 5" Value="x25FreeText05"></asp:ListItem>
                                            <asp:ListItem Text="Text 6" Value="x25FreeText06"></asp:ListItem>
                                            <asp:ListItem Text="Text 7" Value="x25FreeText07"></asp:ListItem>
                                            <asp:ListItem Text="Text 8" Value="x25FreeText08"></asp:ListItem>
                                            <asp:ListItem Text="Text 9" Value="x25FreeText09"></asp:ListItem>
                                            <asp:ListItem Text="Text 10" Value="x25FreeText10"></asp:ListItem>
                                            <asp:ListItem Text="Velký text" Value="x25BigText"></asp:ListItem>
                                            <asp:ListItem Text="Číslo 1" Value="x25FreeNumber01"></asp:ListItem>
                                            <asp:ListItem Text="Číslo 2" Value="x25FreeNumber02"></asp:ListItem>
                                            <asp:ListItem Text="Číslo 3" Value="x25FreeNumber03"></asp:ListItem>
                                            <asp:ListItem Text="Číslo 4" Value="x25FreeNumber04"></asp:ListItem>
                                            <asp:ListItem Text="Číslo 5" Value="x25FreeNumber05"></asp:ListItem>
                                            <asp:ListItem Text="Datum 1" Value="x25FreeDate01"></asp:ListItem>
                                            <asp:ListItem Text="Datum 2" Value="x25FreeDate02"></asp:ListItem>
                                            <asp:ListItem Text="Datum 3" Value="x25FreeDate03"></asp:ListItem>
                                            <asp:ListItem Text="Datum 4" Value="x25FreeDate04"></asp:ListItem>
                                            <asp:ListItem Text="Datum 5" Value="x25FreeDate05"></asp:ListItem>
                                            <asp:ListItem Text="ANO/NE 1" Value="x25FreeBoolean01"></asp:ListItem>
                                            <asp:ListItem Text="ANO/NE 2" Value="x25FreeBoolean02"></asp:ListItem>
                                            <asp:ListItem Text="ANO/NE 3" Value="x25FreeBoolean03"></asp:ListItem>
                                            <asp:ListItem Text="ANO/NE 4" Value="x25FreeBoolean04"></asp:ListItem>
                                            <asp:ListItem Text="ANO/NE 5" Value="x25FreeBoolean05"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="x16Name" runat="server" Width="250px"></asp:TextBox>
                                        <div>
                                            <span>Název sloupce:</span>
                                            <asp:TextBox ID="x16NameGrid" runat="server" Width="150px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="x16IsEntryRequired" runat="server" Text="Povinné k vyplnění" />
                                        <div>
                                            <asp:CheckBox ID="x16IsGridField" runat="server" Text="Sloupec v přehledu" Checked="true" />
                                        </div>

                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="x16Ordinary" runat="server" NumberFormat-DecimalDigits="0" Width="50px" ShowSpinButtons="true"></telerik:RadNumericTextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="x16DataSource" runat="server" Width="400px"></asp:TextBox>
                                        <div>
                                            <asp:CheckBox ID="x16IsFixedDataSource" runat="server" Text="Okruh hodnot je zafixován" />
                                        </div>
                                        <div>
                                            <span>Šířka pole:</span>
                                            <asp:TextBox ID="x16TextboxWidth" runat="server" Width="40px"></asp:TextBox>(px)
                                            <span>Výška pole:</span>
                                            <asp:TextBox ID="x16TextboxHeight" runat="server" Width="40px"></asp:TextBox>(px)
                                        </div>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="del" runat="server" ImageUrl="Images/delete_row.png" ToolTip="Odstranit položku" CssClass="button-link" />
                                        <asp:HiddenField ID="p85id" runat="server" />
                                    </td>
                                </tr>

                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            
        </telerik:RadPageView>

        <telerik:RadPageView ID="other" runat="server" Style="margin-top: 10px;">
            <div class="content-box2">
                <div class="title">
                    <img src="Images/projectrole.png" width="16px" height="16px" />
                    <asp:Label ID="ph1" runat="server" Text="Oprávnění k položkám štítku"></asp:Label>
                    <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
                </div>
                <div class="content">
                    <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="x18EntityCategory"></uc:entityrole_assign>
                    <div class="div6" style="clear: both; margin-top: 20px; border-top: dashed silver 1px; display: none;">
                        <asp:Label ID="lblOwner" runat="server" Text="Vlastník záznamu štítku:" CssClass="lblReq"></asp:Label>
                        <uc:person ID="j02ID_Owner" runat="server" Width="300px" Flag="all" />
                    </div>
                </div>
            </div>


            

            <div class="content-box2" style="margin-top: 60px;">
                <div class="title">Různé</div>
                <div class="content">
                    <div class="div6">
                <asp:DropDownList ID="x18GridColsFlag" runat="server">
                    <asp:ListItem Text="Jako sloupce v přehledu zobrazovat i [Název] a [Kód]" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Jako sloupce v přehledu zobrazovat i [Kód]" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Jako sloupce v přehledu zobrazovat i [Název]" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Nezobrazovat ani [Název] ani [Kód]" Value="4"></asp:ListItem>
                </asp:DropDownList>
            </div>
                    <table cellpadding="5" cellspacing="2">
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="x18IsColors" runat="server" CssClass="chk" Text="Možnost rozlišovat položky štítku barvou" Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="x18IsManyItems" runat="server" CssClass="chk" AutoPostBack="false" Text="Jedná se o štítek s mnoha položkami (100 a více)" />
                            </td>
                        </tr>
                      
                        <tr>
                            <td>
                                <span>Vyplňování názvu položky:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18EntryNameFlag" runat="server">
                                    <asp:ListItem Text="Ručně" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Nevyplňovat" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Vyplňování kódu položky:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18EntryCodeFlag" runat="server">
                                    <asp:ListItem Text="Ručně" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Nepoužívat" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Generovat automaticky v rámci všech položek štítku" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Generovat automaticky v rámci projektu" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Vyplňování pořadí:</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18EntryOrdinaryFlag" runat="server">
                                    <asp:ListItem Text="Ručně" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Nepoužívat" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblB01ID" Text="Workflow šablona:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <uc:datacombo ID="b01ID" runat="server" AutoPostBack="false" DataTextField="b01Name" DataValueField="pid" IsFirstEmptyRow="true" Width="300px"></uc:datacombo>


                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="x18IsCalendar" runat="server" Text="Štítek podporuje i kalendářové rozhraní (samostatný kalendář)" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblx18CalendarFieldStart" runat="server" Text="DB pole pro začátek kalendářové události"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18CalendarFieldStart" runat="server">
                                    <asp:ListItem Text=""></asp:ListItem>
                                    <asp:ListItem Text="Datum 1" Value="x25FreeDate01"></asp:ListItem>
                                    <asp:ListItem Text="Datum 2" Value="x25FreeDate02"></asp:ListItem>
                                    <asp:ListItem Text="Datum 3" Value="x25FreeDate03"></asp:ListItem>
                                    <asp:ListItem Text="Datum 4" Value="x25FreeDate04"></asp:ListItem>
                                    <asp:ListItem Text="Datum 5" Value="x25FreeDate05"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblx18CalendarFieldEnd" runat="server" Text="DB pole pro konec kalendářové události"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18CalendarFieldEnd" runat="server">
                                    <asp:ListItem Text=""></asp:ListItem>
                                    <asp:ListItem Text="Datum 1" Value="x25FreeDate01"></asp:ListItem>
                                    <asp:ListItem Text="Datum 2" Value="x25FreeDate02"></asp:ListItem>
                                    <asp:ListItem Text="Datum 3" Value="x25FreeDate03"></asp:ListItem>
                                    <asp:ListItem Text="Datum 4" Value="x25FreeDate04"></asp:ListItem>
                                    <asp:ListItem Text="Datum 5" Value="x25FreeDate05"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblx18CalendarFieldSubject" runat="server" Text="Pole pro název události v kalendáři:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18CalendarFieldSubject" runat="server">
                                    <asp:ListItem Text="--Prázdné--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Název záznamu" Value="x25Name"></asp:ListItem>
                                    <asp:ListItem Text="Kód záznamu" Value="x25Code"></asp:ListItem>
                                    <asp:ListItem Text="Osoba" Value="j02_alias"></asp:ListItem>
                                    <asp:ListItem Text="Projekt" Value="p41_alias"></asp:ListItem>
                                    <asp:ListItem Text="Klient" Value="p28_alias"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblx18CalendarResourceField" runat="server" Text="Zdroj v timeline kalendáři:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="x18CalendarResourceField" runat="server">
                                    <asp:ListItem Text="--Zdroje nevyužívat--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Osoba" Value="j02ID"></asp:ListItem>
                                    
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Grafická ikona (16px):</span>
                            </td>
                            <td>
                                <asp:TextBox ID="x18Icon" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Grafická ikona (32px):</span>
                            </td>
                            <td>
                                <asp:TextBox ID="x18Icon32" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Vztah k úvodní (dashboard) stránce:
                            </td>
                            <td>
                                <asp:DropDownList ID="x18DashboardFlag" runat="server">
                                    <asp:ListItem Text="--Žádný vztah--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Tlačítko pro založení nového záznamu + odkaz na přehled/kalendář" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Pouze tlačítko pro založení nového záznamu" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pouze odkaz na přehled/kalendář" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Zobrazovat položky jako nástěnku" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="x18IsClueTip" runat="server" Text="U přiřazené položky zobrazovat info-bublinu" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Svázané šablony tiskových sestav:</span>
                            </td>
                            <td>
                                <asp:TextBox ID="x18ReportCodes" runat="server" Width="600px"></asp:TextBox>
                                (čárkou oddělené kódy sestav)
                            </td>
                        </tr>
                        
                    </table>
                </div>
            </div>

        </telerik:RadPageView>
    </telerik:RadMultiPage>

    <asp:HiddenField ID="hidGUID_x16" runat="server" />
    <asp:HiddenField ID="hidGUID_x20" runat="server" />
    <asp:HiddenField ID="HardRefreshPID" runat="server" />
    <asp:Button ID="cmdHardRefresh" runat="server" Style="display: none;" />
    <asp:HiddenField ID="hidx29ID_EntityType" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
