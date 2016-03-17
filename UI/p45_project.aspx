<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p45_project.aspx.vb" Inherits="UI.p45_project" %>

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

            show_totals();

            save_cellvalue(row.attributes["p85id"].value, columnUniqueName, editorValue)

        }

        function show_totals() {
            grid = $find("<%=grid1.ClientID%>");

            var total_fa = 0;
            var total_nefa = 0;
            var total_total = 0;

            var MasterTable = grid.get_masterTableView();

            var Rows = MasterTable.get_dataItems();

            for (var i = 0; i < Rows.length; i++) {
                var total_row = 0;
                var row = Rows[i];
                var cell = row.get_element().cells[1].textContent;
                if (IsNumeric(cell)) {
                    total_fa += parseFloat(cell);
                    total_row += parseFloat(cell);
                }
                cell = row.get_element().cells[2].textContent;
                if (IsNumeric(cell)) {
                    total_nefa += parseFloat(cell);
                    total_row += parseFloat(cell);
                }
                row.get_element().cells[3].textContent = total_row;
                total_total = total_total + total_row;



            }
            $('tr.rgFooter').each(function () {
                $(this).find('td').eq(1).text(total_fa);
                $(this).find('td').eq(2).text(total_nefa);
                $(this).find('td').eq(3).text(total_total);
            });




        }

        function IsNumeric(val) {
            return Number(parseFloat(val)) == val;
        }

        function save_cellvalue(p85id, field, cellValue) {
            var guid = "<%=viewstate("guid")%>";

            $.post("Handler/handler_p45_project.ashx", { guid: guid, p85id: p85id, value: cellValue, field: field, oper: "p46_value" }, function (data) {
                if (data == ' ') {
                    return;
                }


            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
    <asp:panel ID="panHeader" runat="server" style="padding: 10px;">
        <asp:Label ID="lblP45" runat="server" CssClass="lbl" Text="Pracovat ve verzi rozpočtu:"></asp:Label>
        <asp:DropDownList ID="p45ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="VersionWithName"></asp:DropDownList>
        <asp:Button ID="cmdNewVersion" runat="server" Text="Založit novou verzi rozpočtu" CssClass="cmd" />
        <asp:Button ID="cmdDeleteVersion" runat="server" Text="Odstranit tuto verzi rozpočtu" CssClass="cmd" />

    </asp:panel>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="panRecordHeader" runat="server" CssClass="content-box2">
        <div class="title">
            Hlavička rozpočtu projektu
        </div>
        <div class="content">

            <div style="padding: 6px; float: left;">
                <asp:Label ID="lblFrom" Text="Plánované zahájení:" runat="server" CssClass="lbl"></asp:Label>
                <telerik:RadDatePicker ID="p45PlanFrom" runat="server" RenderMode="Lightweight" Width="120px" SharedCalendarID="SharedCalendar">
                    <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                </telerik:RadDatePicker>
            </div>
            <div style="padding: 6px; float: left;">
                <asp:Label ID="lblUntil" Text="Plánované dokončení:" runat="server" CssClass="lbl"></asp:Label>
                <telerik:RadDatePicker ID="p45PlanUntil" runat="server" RenderMode="Lightweight" Width="120px" SharedCalendarID="SharedCalendar" MaxDate="1.1.3000">
                    <DateInput ID="DateInput2" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                </telerik:RadDatePicker>
            </div>
            <div style="padding: 6px; float: left;">
                <span>Popis:</span>
                <asp:TextBox ID="p45Name" runat="server" Width="200px"></asp:TextBox>
            </div>


        </div>
    </asp:Panel>
    <div style="clear: both;"></div>
    <asp:Panel ID="panRecordBody" runat="server">
        <div>
            <button type="button" class="show_hide1" id="cmdAddPerson" runat="server">Okruh osob v rozpočtu<img src="Images/arrow_down.gif" /></button>
        </div>

        <div class="slidingDiv1">
            <div class="content-box2">
                <div class="title">
                    <asp:Label ID="lblInsertPersonsHeader" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="cmdInsertPersons" runat="server" CommandName="add" Text="Přidat zaškrtlé osoby do rozpočtu"></asp:LinkButton>
                </div>
                <div class="content">
                    <asp:Repeater ID="rpJ02" runat="server">
                        <ItemTemplate>
                            <div style="padding: 10px;">
                                <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i"></asp:HyperLink>

                                <asp:CheckBox ID="Person" runat="server"></asp:CheckBox>

                                <asp:HiddenField ID="hidJ02ID" runat="server" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>
    </asp:Panel>

    

    <telerik:RadGrid ID="grid1" AutoGenerateColumns="false" AutoGenerateHierarchy="false" AutoGenerateEditColumn="false" EnableViewState="true" RenderMode="Lightweight" runat="server" PageSize="500" ShowFooter="true" AllowPaging="false" AllowSorting="true" Skin="Metro">
        <MasterTableView EditMode="Batch" ClientDataKeyNames="PID" NoMasterRecordsText="Zatím žádné osoby v rozpočtu" Caption="Rozpočet celkových hodin">
            <Columns>
                <telerik:GridBoundColumn HeaderText="Osoba" DataField="p85FreeText01" ReadOnly="true" AllowSorting="true"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn HeaderText="Hodiny Fa" DataField="p85FreeFloat01" ColumnEditorID="gridnumber1">
                    <ItemStyle ForeColor="Green" />
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn HeaderText="Hodiny NeFa" DataField="p85FreeFloat02" ColumnEditorID="gridnumber1">
                    <ItemStyle ForeColor="Red" />
                </telerik:GridNumericColumn>
                <telerik:GridBoundColumn HeaderText="Celkem" DataField="p85FreeFloat03" ReadOnly="true" AllowSorting="true">
                    <ItemStyle Font-Bold="true" />
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="p85OtherKey2" UniqueName="p85OtherKey2">
                    <ItemTemplate>
                        <asp:DropDownList ID="combo1" runat="server">
                            <asp:ListItem Value="1" Text="Zákaz vykázat přes Fa i přes Nefa"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Zákaz vykázat přes součet"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Zákaz překročit Fa, lze překročit Nefa"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Zákaz překročit Nefa, lze překročit Fa"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Bez omezení vykazovat hodiny"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridButtonColumn UniqueName="delete" ButtonType="ImageButton" ImageUrl="Images/delete.png" ButtonCssClass="button-link" ItemStyle-Width="20px" CommandName="delete"></telerik:GridButtonColumn>
            </Columns>
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


    <asp:Panel ID="panCreateClone" runat="server" CssClass="content-box2" Visible="false">
        <div class="title">
            Založit novou verzi rozpočtu
            <asp:Button ID="cmdSaveClone" runat="server" Text="Uložit novou verzi" CssClass="cmd" />
            <asp:Button ID="cmdCreateCloneCancel" runat="server" Text="Zrušit operaci" CssClass="cmd" />
        </div>
        <div class="content">
            <asp:Label ID="Label1" runat="server" CssClass="lbl" Text="Vzorový rozpočet pro novou verzi:"></asp:Label>
            <asp:DropDownList ID="p45ID_Template" runat="server" DataValueField="pid" DataTextField="VersionWithName"></asp:DropDownList>
            <asp:CheckBox ID="chkNewIsLast" runat="server" Text="Nová verze bude aktuální verzí" Checked="true" />
            <div>
                <asp:CheckBox ID="chkCloneP45" runat="server" Text="Zkopírovat nastavení rozpočtu" Checked="true" Enabled="false" />
            </div>
            <div>
                <asp:CheckBox ID="chkCloneP47" runat="server" Text="Zkopírovat kapacitní plán rozpočtu" Checked="true" />
            </div>
            <div>
                <asp:CheckBox ID="chkCloneP49" runat="server" Text="Zkopírovat finanční plán rozpočtu" Checked="true" />
            </div>
        </div>
    </asp:Panel>


    <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">

        <SpecialDays>
            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
        </SpecialDays>
    </telerik:RadCalendar>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
