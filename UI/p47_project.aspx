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

      
  

        function BatchEditCellValueChanged(sender, args) {
            var row = args.get_row();
            var cell = args.get_cell();
            var tableView = args.get_tableView();
            var column = args.get_column();
            var columnUniqueName = args.get_columnUniqueName();
            var editorValue = args.get_editorValue();
            var cellValue = args.get_cellValue();
            
            
            alert(row.rowIndex + " - " + columnUniqueName + ": " + editorValue);
    
        }

        
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <table>
        <tr>

            
            <td>
                <button type="button" class="show_hide1" id="cmdAddPerson" runat="server">Přidat osobu do plánu<img src="Images/arrow_down.gif" /></button>
            </td>
            <td>
                <asp:RadioButtonList ID="opgPageIndex" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"></asp:RadioButtonList>
            </td>
           <td>
                <asp:Label ID="lblHeader" runat="server" CssClass="valboldblue"></asp:Label>
            </td>
            
        </tr>
    </table>
    <div class="slidingDiv1">
        <div class="content-box2">
            <div class="title">
                Osoby s rolí v projektu
            </div>
            <div class="content">
                <asp:Repeater ID="rpJ02" runat="server">
                    <ItemTemplate>
                        <div class="div6">
                            <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i" title="Kapacita osoby"></asp:HyperLink>
                            
                            <asp:label ID="Person" runat="server"></asp:label>
                            <asp:LinkButton ID="cmdInsert" runat="server" CommandName="add" Text="Vložit do plánu"></asp:LinkButton>
                            <asp:HiddenField ID="hidJ02ID" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

    </div>


    <telerik:RadGrid ID="grid1" AutoGenerateColumns="false" EnableViewState="false" RenderMode="Lightweight" runat="server" PageSize="500" ShowFooter="true" AllowPaging="false" AllowSorting="true" Skin="Metro">
        <MasterTableView EditMode="Batch" ClientDataKeyNames="PID" >
       
            <BatchEditingSettings EditType="Cell" OpenEditingEvent="Click" />
            <ItemStyle height="35px" />
            <AlternatingItemStyle Height="35px" />
        </MasterTableView>
        <ExportSettings ExportOnlyData="true" OpenInNewWindow="true" FileName="marktime_export" UseItemStyles="false">
            <Excel Format="Biff" />
        </ExportSettings>
        
        <ClientSettings AllowKeyboardNavigation="true" EnableAlternatingItems="true">
            <Scrolling AllowScroll="true" FrozenColumnsCount="0" UseStaticHeaders="false" />
            <Selecting CellSelectionMode="SingleCell" AllowRowSelect="false" />
            <ClientEvents OnBatchEditCellValueChanged="BatchEditCellValueChanged" />
            <KeyboardNavigationSettings/>
        </ClientSettings>
        <PagerStyle Position="TopAndBottom" AlwaysVisible="false" />
        <SortingSettings SortToolTip="Klikněte zde pro třídění" SortedDescToolTip="Setříděno sestupně" SortedAscToolTip="Setříděno vzestupně" />
        <FooterStyle BackColor="#bcc7d8" HorizontalAlign="Right" />

    </telerik:RadGrid>
    
    <telerik:GridNumericColumnEditor ID="gridnumber1" runat="server" NumericTextBox-Width="30px" NumericTextBox-BackColor="LightGoldenrodYellow">
        <NumericTextBox NumberFormat-DecimalDigits="0"></NumericTextBox>
    </telerik:GridNumericColumnEditor>
   
    <asp:HiddenField ID="hidLimD1" runat="server" />
    <asp:HiddenField ID="hidLimD2" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
