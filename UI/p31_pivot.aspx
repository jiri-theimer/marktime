<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p31_pivot.aspx.vb" Inherits="UI.p31_pivot" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

        });


        function periodcombo_setting() {

            sw_master("periodcombo_setting.aspx", "Images/settings_32.png");
        }



        function hardrefresh(pid, flag) {
            if (flag == "j70-run") {
                location.replace("p31_pivot.aspx");
                return;
            }

            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;
        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query_32.png");
            return (false);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height: 42px;background-color:white;border-bottom:solid 1px silver">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <img src="Images/pivot_32.png" alt="PIVOT" />
                </td>
                <td style="padding-left:6px;">
                    <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Worksheet PIVOT"></asp:Label>
                </td>
                <td style="padding-left:6px;">
                    <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
                </td>
                <td style="width: 10px;padding-left:20px;">
                    <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
                </td>
                <td style="width: 201px;">
                    <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 200px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>

                </td>
                <td style="text-align: left;">
                    <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />

                </td>

                <td style="padding-left: 40px;">
                    <telerik:RadMenu ID="menu1" RenderMode="Lightweight" Skin="Silk" runat="server" Style="z-index: 2900;" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
                        <Items>
                            
                            <telerik:RadMenuItem Text="Další" ImageUrl="Images/more.png" PostBack="false">
                                <ContentTemplate>
                                    <div style="padding: 20px;">
                                        <div class="div6">
                                            <img src="Images/refresh.png" />
                                            <asp:LinkButton ID="cmdRebind" runat="server" Text="Obnovit výstup sestavy." />
                                        </div>
                                        <div class="div6">
                                            <img src="Images/export.png" />
                                            <asp:LinkButton ID="cmdExport" runat="server" Text="Export do MS EXCEL." />
                                        </div>
                                        <div class="div6">
                                            <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="Stránkování:"></asp:Label>
                                            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování PIVOT výstupu">
                                                <asp:ListItem Text="5"></asp:ListItem>
                                                <asp:ListItem Text="10"></asp:ListItem>
                                                <asp:ListItem Text="20" Selected="true"></asp:ListItem>
                                                <asp:ListItem Text="50"></asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <p></p>

                                    </div>
                                </ContentTemplate>

                            </telerik:RadMenuItem>

                        </Items>
                    </telerik:RadMenu>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="panQueryByEntity" runat="server" CssClass="div6">
        <table cellpadding="0">
            <tr>
                <td>
                    <asp:Image ID="imgEntity" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEntity" runat="server" CssClass="framework_header_span" style="padding-left:10px;"></asp:Label>
                </td>
            </tr>
        </table>
        
    </asp:Panel>
    <div class="div6" >
        <table>
            <tr valign="bottom">
                <td>
                    <fieldset style="padding: 10px;">
                        <legend>Řádky souhrnů</legend>
                        <asp:DropDownList ID="row1" runat="server" AutoPostBack="true">
                           
                            <asp:ListItem Text="Osoba" Value="201" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Klient projektu" Value="2801"></asp:ListItem>
                            <asp:ListItem Text="Projekt" Value="4101"></asp:ListItem>
                            <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>
                            <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>   
                            <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>                         
                            <asp:ListItem Text="Úkol" Value="5601"></asp:ListItem>
                            <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                            <asp:ListItem Text="Fakturovatelné" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Fakt.status" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Status ve faktuře" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>      
                                                  
                        </asp:DropDownList>
                        <span>-></span>
                        <asp:DropDownList ID="row2" runat="server" AutoPostBack="true">
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
                            <asp:ListItem Text="Fakturovatelné" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Fakt.status" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Status ve faktuře" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>
                                                        
                        </asp:DropDownList>
                        <span>-></span>
                        <asp:DropDownList ID="row3" runat="server" AutoPostBack="true">
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
                            <asp:ListItem Text="Fakturovatelné" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Fakt.status" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Status ve faktuře" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="ID faktury" Value="9101"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem> 
                                                       
                        </asp:DropDownList>
                    </fieldset>
                </td>
                <td>
                    <fieldset style="padding: 10px;">
                        <legend>Pivot sloupec</legend>
                        <asp:DropDownList ID="col1" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Osoba" Value="201"></asp:ListItem>
                            <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>                            
                            <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>
                            <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>                                                        
                            <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                            <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                            <asp:ListItem Text="Fakt.aktivita" Value="9801"></asp:ListItem>
                            <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                            <asp:ListItem Text="Fakt.status" Value="7201"></asp:ListItem>
                            <asp:ListItem Text="Status ve faktuře" Value="7001"></asp:ListItem>
                            <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                            <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>      
                            <asp:ListItem Text="Měna úkonu" Value="2701"></asp:ListItem>   
                            <asp:ListItem Text="Měna faktury" Value="2702"></asp:ListItem>                         
                        </asp:DropDownList>
                    </fieldset>
                </td>
                <td>
                    <fieldset style="padding: 10px;">
                        <legend>Veličiny</legend>
                        <asp:DropDownList ID="sum1" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Vykázané hodiny" Value="1" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Částka bez DPH" Value="21"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakturovat]" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakturovat]" Value="12"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Paušál]" Value="126"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Odpis]" Value="123"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakt.později]" Value="127"></asp:ListItem>
                            <asp:ListItem Text="Schváleno bez DPH" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny" Value="3"></asp:ListItem> 
                            <asp:ListItem Text="Vyfakturované hodiny [Paušál]" Value="36"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny [Odpis]" Value="33"></asp:ListItem>                                                      
                            <asp:ListItem Text="Vyfakturováno bez DPH" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturováno bez DPH x Kurz" Value="25"></asp:ListItem>
                            <asp:ListItem Text="Vykázaná hodnota" Value="11"></asp:ListItem>                                                       
                            <asp:ListItem Text="Vyfakturovaná hodnota" Value="13"></asp:ListItem>
                            
                            <asp:ListItem Text="Přepočteno fixním kurzem" Value="24"></asp:ListItem>                                                                 
                        </asp:DropDownList>
                        
                        <asp:DropDownList ID="sum2" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Vykázané hodiny" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Částka bez DPH" Value="21"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakturovat]" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Schválená hodiny [Paušál]" Value="126"></asp:ListItem>
                            <asp:ListItem Text="Schválená hodiny [Odpis]" Value="123"></asp:ListItem>
                            <asp:ListItem Text="Schválená hodiny [Fakt.později]" Value="127"></asp:ListItem>
                            <asp:ListItem Text="Schváleno bez DPH" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny" Value="3"></asp:ListItem> 
                            <asp:ListItem Text="Vyfakturované hodiny [Paušál]" Value="36"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny [Odpis]" Value="33"></asp:ListItem>                                                      
                            <asp:ListItem Text="Vyfakturováno bez DPH" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturováno bez DPH x Kurz" Value="25"></asp:ListItem>
                            <asp:ListItem Text="Vykázaná hodnota" Value="11"></asp:ListItem>                            
                            <asp:ListItem Text="Schválená hodnota [Fakturovat]" Value="12"></asp:ListItem>                            
                            <asp:ListItem Text="Vyfakturovaná hodnota" Value="13"></asp:ListItem>
                            
                            
                            <asp:ListItem Text="Přepočteno fixním kurzem" Value="24"></asp:ListItem>           
                        </asp:DropDownList>
                        
                        <asp:DropDownList ID="sum3" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Vykázané hodiny" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Částka bez DPH" Value="21"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakturovat]" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Paušál]" Value="126"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Odpis]" Value="123"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakt.později]" Value="127"></asp:ListItem>
                            <asp:ListItem Text="Schváleno bez DPH" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny" Value="3"></asp:ListItem>  
                            <asp:ListItem Text="Vyfakturované hodiny [Paušál]" Value="36"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny [Odpis]" Value="33"></asp:ListItem>                                                     
                            <asp:ListItem Text="Vyfakturováno bez DPH" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturováno bez DPH x Kurz" Value="25"></asp:ListItem>
                            <asp:ListItem Text="Vykázaná hodnota" Value="11"></asp:ListItem>                            
                            <asp:ListItem Text="Schválená hodnota [Fakturovat]" Value="12"></asp:ListItem>                            
                            <asp:ListItem Text="Vyfakturovaná hodnota" Value="13"></asp:ListItem>
                            
                            
                            <asp:ListItem Text="Přepočteno fixním kurzem" Value="24"></asp:ListItem>           
                        </asp:DropDownList>
                        <asp:DropDownList ID="sum4" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Vykázané hodiny" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Částka bez DPH" Value="21"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakturovat]" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Paušál]" Value="126"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Odpis]" Value="123"></asp:ListItem>
                            <asp:ListItem Text="Schválené hodiny [Fakt.později]" Value="127"></asp:ListItem>
                            <asp:ListItem Text="Schváleno bez DPH" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny" Value="3"></asp:ListItem>   
                            <asp:ListItem Text="Vyfakturované hodiny [Paušál]" Value="36"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturované hodiny [Odpis]" Value="33"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturováno bez DPH" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Vyfakturováno bez DPH x Kurz" Value="25"></asp:ListItem>
                            <asp:ListItem Text="Vykázaná hodnota" Value="11"></asp:ListItem>                            
                            <asp:ListItem Text="Schválená hodnota [Fakturovat]" Value="12"></asp:ListItem>                            
                            <asp:ListItem Text="Vyfakturovaná hodnota" Value="13"></asp:ListItem>
                            
                            
                            <asp:ListItem Text="Přepočteno fixním kurzem" Value="24"></asp:ListItem>           
                        </asp:DropDownList>
                            
                    </fieldset>
                </td>
            </tr>
        </table>

    </div>


    
    <telerik:RadPivotGrid runat="server" ID="pivot1" AllowPaging="true" ShowColumnHeaderZone="false" AllowSorting="false" AllowFiltering="false" ShowFilterHeaderZone="false" AllowNaturalSort="false" Skin="Silk">
        <DataCellStyle BackColor="white" />
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="false"></PagerStyle>
        <Fields>
            <telerik:PivotGridColumnField DataField="Col1" Caption="Osoba">
            </telerik:PivotGridColumnField>
           
            <telerik:PivotGridAggregateField DataField="Sum1" Caption="Vykázané hodiny" Aggregate="Sum" DataFormatString="{0:F2}">
            </telerik:PivotGridAggregateField>
          
        </Fields>
        
       <ConfigurationPanelSettings EnableDragDrop="false" />
        <ClientSettings EnableFieldsDragDrop="false" ClientMessages-DragToReorder=""></ClientSettings>
    </telerik:RadPivotGrid>
    


    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />
</asp:Content>
