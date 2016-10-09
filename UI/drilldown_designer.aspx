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
    <div class="content-box2">
        <div class="title">
            DRILL-DOWN úrovně
        </div>
        <div class="content">
            <table cellpadding="6">
                <tr>
                    <td>
                        Úroveň #1
                    </td>
                    <td>
                        Úroveň #2
                    </td>
                    <td>
                        Úroveň #3
                    </td>
                    <td>
                        Úroveň #4
                    </td>
                </tr>
                <tr>
                   
                    <td>
                        <asp:DropDownList ID="j75Level1" runat="server" AutoPostBack="false">
                           <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="201"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="2801"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="4101"></asp:ListItem>
                            <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>
                            <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>   
                            <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>                         
                            <asp:ListItem Text="Úkol" Value="5601"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační oddíl" Value="9501"></asp:ListItem>
                            <asp:ListItem Text="Fakturovatelná aktivita" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Návrh fakturačního statusu" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační status" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Klient faktury" Value="9102"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>      
                            <asp:ListItem Text="Rok fakturace" Value="9903"></asp:ListItem>
                            <asp:ListItem Text="Měsíc fakturace" Value="9904"></asp:ListItem>   
                            <asp:ListItem Text="Měna úkonu" Value="2701"></asp:ListItem>   
                            <asp:ListItem Text="Měna faktury" Value="2702"></asp:ListItem>                            
                            <asp:ListItem Text="Výchozí sazba" Value="3101"></asp:ListItem>      
                            <asp:ListItem Text="Schválená sazba" Value="3102"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturovaná sazba" Value="3103"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
               
                    <td>
                        <asp:DropDownList ID="j75Level2" runat="server" AutoPostBack="false">
                           <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="201"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="2801"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="4101"></asp:ListItem>
                            <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>
                            <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>   
                            <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>                         
                            <asp:ListItem Text="Úkol" Value="5601"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační oddíl" Value="9501"></asp:ListItem>
                            <asp:ListItem Text="Fakturovatelná aktivita" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Návrh fakturačního statusu" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační status" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Klient faktury" Value="9102"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>      
                            <asp:ListItem Text="Rok fakturace" Value="9903"></asp:ListItem>
                            <asp:ListItem Text="Měsíc fakturace" Value="9904"></asp:ListItem>   
                            <asp:ListItem Text="Měna úkonu" Value="2701"></asp:ListItem>   
                            <asp:ListItem Text="Měna faktury" Value="2702"></asp:ListItem>                            
                            <asp:ListItem Text="Výchozí sazba" Value="3101"></asp:ListItem>      
                            <asp:ListItem Text="Schválená sazba" Value="3102"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturovaná sazba" Value="3103"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="j75Level3" runat="server" AutoPostBack="false">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="201"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="2801"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="4101"></asp:ListItem>
                            <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>
                            <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>   
                            <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>                         
                            <asp:ListItem Text="Úkol" Value="5601"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační oddíl" Value="9501"></asp:ListItem>
                            <asp:ListItem Text="Fakturovatelá aktivita" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Návrh fakturačního statusu" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační status" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Klient faktury" Value="9102"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>      
                            <asp:ListItem Text="Rok fakturace" Value="9903"></asp:ListItem>
                            <asp:ListItem Text="Měsíc fakturace" Value="9904"></asp:ListItem>   
                            <asp:ListItem Text="Měna úkonu" Value="2701"></asp:ListItem>   
                            <asp:ListItem Text="Měna faktury" Value="2702"></asp:ListItem>                            
                            <asp:ListItem Text="Výchozí sazba" Value="3101"></asp:ListItem>      
                            <asp:ListItem Text="Schválená sazba" Value="3102"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturovaná sazba" Value="3103"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="j75Level4" runat="server" AutoPostBack="false">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="201"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="2801"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="4101"></asp:ListItem>
                            <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>
                            <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>   
                            <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>                         
                            <asp:ListItem Text="Úkol" Value="5601"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační oddíl" Value="9501"></asp:ListItem>
                            <asp:ListItem Text="Fakturovatelná aktivita" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Návrh fakturačního statusu" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Fakturační status" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Klient faktury" Value="9102"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>      
                            <asp:ListItem Text="Rok fakturace" Value="9903"></asp:ListItem>
                            <asp:ListItem Text="Měsíc fakturace" Value="9904"></asp:ListItem>   
                            <asp:ListItem Text="Měna úkonu" Value="2701"></asp:ListItem>   
                            <asp:ListItem Text="Měna faktury" Value="2702"></asp:ListItem>                            
                            <asp:ListItem Text="Výchozí sazba" Value="3101"></asp:ListItem>      
                            <asp:ListItem Text="Schválená sazba" Value="3102"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturovaná sazba" Value="3103"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <asp:HiddenField ID="j75IsSystem" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />

    <table cellpadding="8">
        <tr valign="top">
            <td>
                <div><%=Resources.grid_designer.DostupneSloupce %></div>
                <telerik:RadListBox ID="colsSource" Height="200px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="colsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="false" AutoPostBackOnDelete="false" AutoPostBackOnTransfer="false">
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

    <asp:panel ID="panRoles" runat="server" cssclass="content-box2">
        <div class="title">
            <img src="Images/projectrole.png" width="16px" height="16px" />
            <asp:Label ID="Label1" runat="server" Text="Přístupová práva k drill-down šabloně pro další osoby"></asp:Label>
            <asp:Button ID="cmdAddX69" runat="server" CssClass="cmd" Text="Přidat" />
        </div>
        <div class="content">
            <uc:entityrole_assign ID="roles1" runat="server" EntityX29ID="j75DrillDownTemplate" EmptyDataMessage="K šabloně nejsou definována přístupová práva, proto bude přístupná pouze Vám."></uc:entityrole_assign>

        </div>
    </asp:panel>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>

