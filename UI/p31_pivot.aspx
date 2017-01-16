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



        function hardrefresh(pid, flag,field) {
            if (flag == "j70-run") {
                location.replace("p31_pivot.aspx");
                return;
            }
            alert(field)
            
            document.getElementById("<%=Me.hidHardRefreshPID.ClientID%>").value = field;
            document.getElementById("<%=Me.hidHardRefreshFlag.ClientID%>").value = flag;

            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;
        }

        function querybuilder() {
            var j70id = "<%=Me.CurrentJ70ID%>";
            sw_master("query_builder.aspx?prefix=p31&pid=" + j70id, "Images/query_32.png");
            return (false);
        }

        function select_field(index) {
            if (index == 1) {
                sw_master("select_field.aspx?prefix=p31&flag=field1&pivot=1&value="+document.getElementById("<%=Me.hidRow1.ClientID%>").value);
            }
            if (index == 2) {
                sw_master("select_field.aspx?prefix=p31&flag=field2&pivot=1&value=" + document.getElementById("<%=Me.hidRow2.ClientID%>").value);
            }
            if (index == 3) {
                sw_master("select_field.aspx?prefix=p31&flag=field3&pivot=1&value=" + document.getElementById("<%=Me.hidRow3.ClientID%>").value);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: white; padding: 10px;">
        <div style="float: left;">
            <img src="Images/pivot_32.png" alt="PIVOT" />

            <asp:Label ID="lblHeader" runat="server" CssClass="page_header_span" Text="Worksheet PIVOT"></asp:Label>
        </div>
        <div class="commandcell" style="padding-left: 6px;">
            <uc:periodcombo ID="period1" runat="server" Width="250px"></uc:periodcombo>
        </div>
        <div class="commandcell" style="padding-left: 20px;">
            <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>


            <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 200px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>



            <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />

        </div>
        <div class="commandcell" style="padding-left: 20px;">

            <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování PIVOT výstupu">
                <asp:ListItem Text="5"></asp:ListItem>
                <asp:ListItem Text="10"></asp:ListItem>
                <asp:ListItem Text="20" Selected="true"></asp:ListItem>
                <asp:ListItem Text="50"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="commandcell">
            <img src="Images/refresh.png" />
            <asp:LinkButton ID="cmdRebind" runat="server" Text="Obnovit." />
        </div>
        <div class="commandcell" style="padding-left: 10px;">
            <img src="Images/export.png" />
            <asp:LinkButton ID="cmdExport" runat="server" Text="MS EXCEL." />
            <img src="Images/doc.png" alt="doc" style="display: none;" />
            <asp:LinkButton ID="cmdDOC" runat="server" Text="MS WORD" Visible="false" />
        </div>

        <div style="clear: both;"></div>

        <asp:Panel ID="panQueryByEntity" runat="server" CssClass="div6">
            <table cellpadding="0">
                <tr>
                    <td>
                        <asp:Image ID="imgEntity" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblEntity" runat="server" CssClass="framework_header_span" Style="padding-left: 10px;"></asp:Label>
                    </td>
                </tr>
            </table>

        </asp:Panel>

        <div style="clear: both;"></div>

        <div class="content-box1">
            <div class="title">
                <b>Řádky souhrnů</b>
            </div>
            <div class="content">
                <asp:HyperLink ID="linkRow1" runat="server" Text="Pole 1" NavigateUrl="javascript:select_field(1)"></asp:HyperLink>
                <asp:HiddenField ID="hidRow1" runat="server" />                
                                           
               
                <span>-></span>
                <asp:HyperLink ID="linkRow2" runat="server" Text="Pole 2" NavigateUrl="javascript:select_field(2)"></asp:HyperLink>
                <asp:HiddenField ID="hidRow2" runat="server" />
                <asp:ImageButton ID="cmdClear2" runat="server" ImageUrl="Images/delete.png" CssClass="cmd" />
                
                <span>-></span>
                <asp:HyperLink ID="linkRow3" runat="server" Text="Pole 3" NavigateUrl="javascript:select_field(3)"></asp:HyperLink>
                <asp:HiddenField ID="hidRow3" runat="server" />
                <asp:ImageButton ID="cmdClear3" runat="server" ImageUrl="Images/delete.png" CssClass="cmd" />
                
            </div>
        </div>
        <div class="content-box1">
            <div class="title">
                <b>Pivot sloupec</b>
            </div>

            <div class="content">
                <asp:DropDownList ID="col1" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="Osoba" Value="201"></asp:ListItem>
                    <asp:ListItem Text="Typ projektu" Value="4201"></asp:ListItem>
                    <asp:ListItem Text="Středisko projektu" Value="1801"></asp:ListItem>
                    <asp:ListItem Text="Středisko osoby" Value="1802"></asp:ListItem>
                    <asp:ListItem Text="Pozice osoby" Value="107"></asp:ListItem>
                    <asp:ListItem Text="Sešit" Value="3401"></asp:ListItem>
                    <asp:ListItem Text="Aktivita" Value="3201"></asp:ListItem>
                    <asp:ListItem Text="Fakturovatelné" Value="9801"></asp:ListItem>
                    <asp:ListItem Text="Fakturační oddíl" Value="9501"></asp:ListItem>
                    <asp:ListItem Text="Schváleno" Value="7101"></asp:ListItem>
                    <asp:ListItem Text="Schvalovací status" Value="7201"></asp:ListItem>
                    <asp:ListItem Text="Status ve faktuře" Value="7001"></asp:ListItem>
                    <asp:ListItem Text="Rok" Value="9901"></asp:ListItem>
                    <asp:ListItem Text="Měsíc" Value="9902"></asp:ListItem>
                    <asp:ListItem Text="Týden" Value="9906"></asp:ListItem>
                    <asp:ListItem Text="Rok fakturace" Value="9903"></asp:ListItem>
                    <asp:ListItem Text="Měsíc fakturace" Value="9904"></asp:ListItem>
                    <asp:ListItem Text="Měna úkonu" Value="2701"></asp:ListItem>
                    <asp:ListItem Text="Měna faktury" Value="2702"></asp:ListItem>
                    <asp:ListItem Text="Výchozí sazba" Value="3101"></asp:ListItem>
                    <asp:ListItem Text="Schválená sazba" Value="3102"></asp:ListItem>
                    <asp:ListItem Text="Vyfakturovaná sazba" Value="3103"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="content-box1">
            <div class="title">
                <b>Veličiny</b>
            </div>
            <div class="content">
                <asp:DropDownList ID="sum1" runat="server" AutoPostBack="true" Width="200px" DataTextField="Caption" DataValueField="FieldTypeID">
                </asp:DropDownList>

                <asp:DropDownList ID="sum2" runat="server" AutoPostBack="true" Width="200px"  DataTextField="Caption" DataValueField="FieldTypeID">                  
                </asp:DropDownList>

                <asp:DropDownList ID="sum3" runat="server" AutoPostBack="true" Width="200px"  DataTextField="Caption" DataValueField="FieldTypeID">                   
                </asp:DropDownList>
                <asp:DropDownList ID="sum4" runat="server" AutoPostBack="true" Width="200px"  DataTextField="Caption" DataValueField="FieldTypeID">                   
                </asp:DropDownList>
            </div>
        </div>



        <div style="clear: both;"></div>

    </div>

    <telerik:RadPivotGrid runat="server" ID="pivot1" AllowPaging="true" ShowColumnHeaderZone="false" AllowSorting="false" AllowFiltering="false" ShowFilterHeaderZone="false" AllowNaturalSort="false" Skin="Metro">
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
