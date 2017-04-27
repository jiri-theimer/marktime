<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="p31_sumgrid.aspx.vb" Inherits="UI.p31_sumgrid" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: white;">
        <div style="float: left;">
            <img src="Images/drilldown_32.png" title="Summary worksheet přehledy" />

        </div>
        <div class="commandcell">

            <span>Seskupit podle:</span>
            <telerik:RadComboBox ID="dd1" runat="server" AutoPostBack="true"></telerik:RadComboBox>
        </div>
        
        <div class="commandcell" style="padding-left: 3px;">
            <button type="button" id="cmdGUI" class="show_hide1" style="padding: 2px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface;" title="Rozvržení panelů">
                Sledované veličiny<img src="Images/arrow_down.gif" />
            </button>
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <uc:periodcombo ID="period1" runat="server" Width="180px"></uc:periodcombo>
            
        </div>
        <div class="commandcell">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 170px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />
        </div>
       
        <div class="commandcell" style="margin-left: 12px;">
            <telerik:RadMenu ID="menu1" RenderMode="Auto" Skin="Metro" Style="z-index: 2900;" runat="server" ExpandDelay="0" ExpandAnimation-Type="None" ClickToOpen="true">
                <Items>
                   <telerik:RadMenuItem text="Pivot" Value="pivot" NavigateUrl="javascript:pivot()"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Export" Value="export" ImageUrl="Images/menuarrow.png">
                        <Items>
                            <telerik:RadMenuItem Text="XLS" NavigateUrl="javascript:hardrefresh(0,'xls')"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="DOC" NavigateUrl="javascript:hardrefresh(0,'doc')"></telerik:RadMenuItem>
                            <telerik:RadMenuItem Text="PDF" NavigateUrl="javascript:hardrefresh(0,'pdf')"></telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Nastavení" ImageUrl="Images/menuarrow.png" Value="more" PostBack="false">

                        <ContentTemplate>
                           
                            <div class="div6">
                                <asp:Label ID="lblPaging" runat="server" CssClass="lbl" Text="Stránkování:"></asp:Label>
                                <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true">
                                    <asp:ListItem Text="20"></asp:ListItem>
                                    <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="100"></asp:ListItem>
                                    <asp:ListItem Text="200"></asp:ListItem>
                                    <asp:ListItem Text="500"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                           
                           
                            <div class="div6">
                                <asp:CheckBox ID="chkFirstLastCount" runat="server" AutoPostBack="true" Text="Zobrazovat sloupce [Datum prvního úkonu], [Datum posledního úkonu]" Checked="true" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadMenuItem>
                </Items>
            </telerik:RadMenu>

        </div>

        <div style="clear: both;"></div>



        <div class="slidingDiv1">
          
            <asp:Panel ID="panSumCols" runat="server" CssClass="content-box2">
                <div class="title">                    
                    Nastavení sledovaných veličin                
                </div>
                <div class="content">
                    <div class="div6">
                        <span>Druhá úroveň seskupení:</span> <telerik:RadComboBox ID="dd2" runat="server" AutoPostBack="true"></telerik:RadComboBox>
                    </div>
                    <div class="div6">
                        <asp:Button ID="cmdRefresh" runat="server" CssClass="cmd" Text="Obnovit výstup statistiky" />
                    </div>
                    <table cellpadding="8">
                        <tr valign="top">
                            <td style="border-right:solid 2px skyblue;">
                                <div><%=Resources.grid_designer.DostupneSloupce %></div>
                                <telerik:RadListBox ID="colsSource" Height="200px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="colsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="false" AutoPostBackOnDelete="false" AutoPostBackOnTransfer="false">                                  
                                    <ButtonSettings TransferButtons="All" ShowTransferAll="false" Position="Bottom" />

                                    <Localization ToRight="Přesunout" ToLeft="Odebrat" AllToRight="Přesunout vše" AllToLeft="Odbrat vše" MoveDown="Posunout dolu" MoveUp="Posunout nahoru" />
                                </telerik:RadListBox>
                           
                                <div><%=Resources.grid_designer.VybraneSloupce %></div>
                                <telerik:RadListBox ID="colsDest" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="true" Culture="cs-CZ" Width="350px" SelectionMode="Single">

                                    <EmptyMessageTemplate>
                                        <div style="padding-top: 50px;">
                                            <%=Resources.grid_designer.ZadneVybraneSloupce %>
                                        </div>
                                    </EmptyMessageTemplate>
                                </telerik:RadListBox>
                                <div>
                                    
                                    <asp:DropDownList ID="cbxMaxMinAll" runat="server" ToolTip="Okruh zobrazovaných hodnot pole ve statistice" AutoPostBack="true">
                                    <asp:ListItem Text="Změnit u pole typ agregace hodnot na:" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Všechny hodnoty pole (ALL)" Value="all"></asp:ListItem>
                                    <asp:ListItem Text="Maximální hodnota pole (MAX)" Value="max"></asp:ListItem>
                                    <asp:ListItem Text="Minimální hodnota pole (MIN)" Value="min"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </td>

                       
                            <td>
                                <div>Dostupné SUM veličiny</div>
                                <telerik:RadListBox ID="sumsSource" Height="200px" runat="server" AllowTransfer="true" TransferMode="Move" TransferToID="sumsDest" SelectionMode="Single" Culture="cs-CZ" AllowTransferOnDoubleClick="true" Width="350px" AutoPostBackOnReorder="false" AutoPostBackOnDelete="false" AutoPostBackOnTransfer="false">
                                    <ButtonSettings TransferButtons="All" ShowTransferAll="false" Position="Bottom" />

                                    <Localization ToRight="Přesunout" ToLeft="Odebrat" AllToRight="Přesunout vše" AllToLeft="Odbrat vše" MoveDown="Posunout dolu" MoveUp="Posunout nahoru" />
                                </telerik:RadListBox>
                           
                                <div>Vybrané SUM veličiny</div>
                                <telerik:RadListBox ID="sumsDest" runat="server" AllowReorder="true" AllowTransferOnDoubleClick="true" Culture="cs-CZ" Width="350px" SelectionMode="Single">

                                    <EmptyMessageTemplate>
                                        <div style="padding-top: 50px;">
                                            <%=Resources.grid_designer.ZadneVybraneSloupce %>
                                        </div>
                                    </EmptyMessageTemplate>
                                </telerik:RadListBox>

                            </td>

                        </tr>
                    </table>

                </div>
            </asp:Panel>
        </div>

        <div style="clear: both;"></div>

        <asp:Panel ID="panQueryByEntity" runat="server" CssClass="div6" Visible="false">
            <table cellpadding="0">
                <tr>
                    <td>
                        <asp:Image ID="imgEntity" runat="server" />
                    </td>
                    <td style="padding-left: 10px;">

                        <asp:HyperLink ID="MasterRecord" runat="server"></asp:HyperLink>

                    </td>
                </tr>
            </table>
            <asp:Label ID="lblQuery" runat="server" CssClass="valboldred"></asp:Label>
        </asp:Panel>


        <uc:datagrid ID="grid1" runat="server" ClientDataKeyNames="pid" OnRowSelected="RowSelected" OnRowDblClick="RowDoubleClick"></uc:datagrid>

    </div>


    <asp:HiddenField ID="hiddatapid" runat="server" />
    <asp:HiddenField ID="hidHardRefreshFlag" runat="server" />
    <asp:HiddenField ID="hidHardRefreshPID" runat="server" />
    <asp:HiddenField ID="hidJ74ID" runat="server" />
    <asp:HiddenField ID="hidJ70ID" runat="server" />
    <asp:HiddenField ID="hidMasterPrefix" runat="server" />
    <asp:HiddenField ID="hidMasterPID" runat="server" />
    <asp:HiddenField ID="hidFrom" runat="server" />
    <asp:HiddenField ID="hidTabQueryFlag" runat="server" />
    <asp:HiddenField ID="hidSGF" runat="server" />
    
    
    <asp:HiddenField ID="hidToggle" runat="server" />

    <asp:HiddenField ID="hidMasterAW" runat="server" />
    <asp:HiddenField ID="hidGridColumnSql" runat="server" />
    <asp:Button ID="cmdHardRefresh" runat="server" Style="display: none;" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });

            <%if me.hidToggle.value="1" then%>
            $(".slidingDiv1").slideToggle();
            document.getElementById("<%=Me.hidToggle.ClientID%>").value="";
            <%end If%>

        });


        function RowSelected(sender, args) {
            document.getElementById("<%=hiddatapid.clientid%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            var pid = document.getElementById("<%=hiddatapid.clientid%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return
            }
            var grid = sender;
            var MasterTable = grid.get_masterTableView()
            var row = MasterTable.get_dataItems()[args.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "<%=Me.dd1.SelectedValue%>");
            var sga = cell.innerHTML;
            <%If Me.dd2.SelectedIndex>0 then%>
            cell = MasterTable.getCellByColumnUniqueName(row, "<%=Me.dd2.SelectedValue%>");
            sga = sga + "->" + cell.innerHTML;
            <%End If%>
            
            go2grid(pid,sga);
        }

        function go2grid(pid,sga) {
            var sgf = document.getElementById("<%=hidSGF.ClientID%>").value;            
            var j70id = "<%=hidJ70ID.Value%>";
            var masterprefix = document.getElementById("<%=hidMasterPrefix.ClientID%>").value;
            var masterpid = document.getElementById("<%=hidMasterPID.ClientID%>").value;
            
            location.replace("p31_grid.aspx?sgf=" + sgf + "&sgv=" + pid+"&masterprefix="+masterprefix+"&masterpid="+masterpid+"&sga="+encodeURI(sga));
        }

        function querybuilder() {            
            var j70id = "<%=hidJ70ID.Value%>";
            sw_master("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query.png");
            return (false);
        }

        function hardrefresh(pid, flag) {
                        
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = pid;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdHardRefresh, "", False)%>;

        }

        function pivot() {
            sw_master("p31_sumgrid_pivot.aspx", "Images/pivot.png", true);
        }
    </script>
</asp:Content>
