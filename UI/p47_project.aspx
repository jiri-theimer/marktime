<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p47_project.aspx.vb" Inherits="UI.p47_project" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".slidingDiv1").hide();
            $(".show_hide1").show();

            $('.show_hide1').click(function () {
                $(".slidingDiv1").slideToggle();
            });


            
        });

        $(window).load(function (e) {
            show_totals();
        });


        function BatchEditCellValueChanged(sender, args) {
            var row = args.get_row();
            var cell = args.get_cell();
            var tableView = args.get_tableView();
            var column = args.get_column();
            var columnUniqueName = args.get_columnUniqueName();
            var editorValue = args.get_editorValue();
            var cellValue = args.get_cellValue();
            //alert(row.attributes["j02id"].value);

            //alert(row.rowIndex + " - " + columnUniqueName + ": " + editorValue);
            save_cellvalue(row.attributes["j02id"].value, columnUniqueName, editorValue)

        }

        function save_cellvalue(j02id, colName, cellValue) {
            var guid = "<%=viewstate("guid")%>";
            var d1 = document.getElementById("<%=hidLimD1.clientid%>").value;
            var d2 = document.getElementById("<%=hidLimD2.ClientID%>").value;

            $.post("Handler/handler_capacity_plan.ashx", { guid: guid, d1: d1, d2: d2, j02id: j02id, value: cellValue, colName: colName, oper: "capacity_plan_value" }, function (data) {
                if (data == ' ') {
                    return;
                }

                
                show_totals();

            });
        }

        function show_totals() {
            grid = $find("<%=grid1.ClientID%>");
            
            var total = 0;
            var cols = $('tr.rgFooter').find('td').length;
            
            var MasterTable = grid.get_masterTableView();
            
            var Rows = MasterTable.get_dataItems();
            
            for (var col = 2; col < cols; col++) {
                total = 0;

                for (var i = 0; i < Rows.length; i++) {
                    var row = Rows[i];                    
                    var cell = row.get_element().cells[col].textContent;
                    
                    if (IsNumeric(cell))
                        total += parseFloat(cell);

                }
                $('tr.rgFooter').each(function () {
                    $(this).find('td').eq(col).text(total);
                });

            }
            
            var total_total = 0;            
            for (var i = 0; i < Rows.length; i++) {
                var row = Rows[i];                
                total = 0;
                
                for (var col = 2; col < cols; col++) {
                    var cell = row.get_element().cells[col].textContent;
                    if (IsNumeric(cell)) {
                        total += parseFloat(cell);
                        total_total += parseFloat(cell);
                        
                    }
                    
                }
                
                var cell = row.get_element().cells[1].textContent = total;
            }
            
            $('tr.rgFooter').find('td').eq(1).text(total_total);
               

        }

        function IsNumeric(val) {
            return Number(parseFloat(val)) == val;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
    <asp:Panel ID="panHeader" runat="server" Style="padding: 10px;">
        <asp:Label ID="lblP45" runat="server" CssClass="lbl" Text="Pracovat ve verzi rozpočtu:"></asp:Label>
        <asp:DropDownList ID="p45ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="VersionWithName"></asp:DropDownList>
        
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
   <div style="float:left;padding:6px;">
       <button type="button" class="show_hide1" id="cmdAddPerson" runat="server">Přidat osobu do plánu<img src="Images/arrow_down.gif" /></button>
   </div>
   <div style="float:left;padding:6px;">
       <asp:Label ID="lblHeader" runat="server" CssClass="valboldblue"></asp:Label>
   </div>     
   <div style="float:left;padding:6px;">
       <asp:DropDownList ID="m1" runat="server" AutoPostBack="true"></asp:DropDownList>
       ->
       <asp:DropDownList ID="m2" runat="server" AutoPostBack="true"></asp:DropDownList>
   </div>      
   <div style="clear:both;"></div>
          
    <div class="slidingDiv1">
        <div class="content-box2">
            <div class="title">
                Osoby s rolí v projektu
            </div>
            <div class="content">
                <asp:Repeater ID="rpJ02" runat="server">
                    <ItemTemplate>
                        <div style="padding: 10px;">
                            <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i" title="Kapacita osoby"></asp:HyperLink>

                            <asp:Label ID="Person" runat="server"></asp:Label>
                            <asp:LinkButton ID="cmdInsert" runat="server" CommandName="add" Text="Vložit do plánu"></asp:LinkButton>
                            <asp:HiddenField ID="hidJ02ID" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

    </div>


    <telerik:RadGrid ID="grid1" AutoGenerateColumns="false" AutoGenerateHierarchy="false" AutoGenerateEditColumn="false" EnableViewState="true" RenderMode="Lightweight" runat="server" PageSize="500" ShowFooter="true" AllowPaging="false" AllowSorting="true" Skin="Metro">
        <MasterTableView EditMode="Batch" ClientDataKeyNames="PID">
            <FooterStyle HorizontalAlign="left" BackColor="silver" Font-Bold="true" />
            <BatchEditingSettings EditType="Cell" OpenEditingEvent="Click" />
            <ItemStyle Height="35px" />
            <AlternatingItemStyle Height="35px" />
        </MasterTableView>
        <ExportSettings ExportOnlyData="true" OpenInNewWindow="true" FileName="marktime_export" UseItemStyles="false">
            <Excel Format="Biff" />
        </ExportSettings>

        <ClientSettings AllowKeyboardNavigation="true" EnableAlternatingItems="true">
            <Scrolling AllowScroll="false" FrozenColumnsCount="1" UseStaticHeaders="false" />
            <Selecting CellSelectionMode="SingleCell" AllowRowSelect="false" />
            <ClientEvents OnBatchEditCellValueChanged="BatchEditCellValueChanged" />
            <KeyboardNavigationSettings EnableKeyboardShortcuts="false" />
        </ClientSettings>
        <PagerStyle Position="TopAndBottom" AlwaysVisible="false" />
        <FooterStyle BackColor="#bcc7d8" HorizontalAlign="Right" />

    </telerik:RadGrid>

    <telerik:GridNumericColumnEditor ID="gridnumber1" runat="server" NumericTextBox-Width="30px" NumericTextBox-BackColor="LightGoldenrodYellow">
        <NumericTextBox NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="false"></NumericTextBox>
    </telerik:GridNumericColumnEditor>

    <asp:HiddenField ID="hidLimD1" runat="server" />
    <asp:HiddenField ID="hidLimD2" runat="server" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">

   
</asp:Content>
