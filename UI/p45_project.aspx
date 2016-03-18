<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ModalForm.Master" CodeBehind="p45_project.aspx.vb" Inherits="UI.p45_project" %>

<%@ MasterType VirtualPath="~/ModalForm.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="person" Src="~/person.ascx" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>


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

            save_cellvalue(row.attributes["p85id"].value, columnUniqueName, editorValue);

            
        }

        function show_needsave_message() {
            master_show_message("Změny se uloží až tlačítkem [Uložit změny].")
        }

        function show_totals() {
            grid = $find("<%=grid1.ClientID%>");

            var total_fa = 0;
            var total_nefa = 0;
            var total_total = 0;
            var total_fee_fa = 0;
            var total_fee_nefa = 0;

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

                cell = row.get_element().cells[5].textContent;
                if (IsNumeric(cell)) {
                    total_fee_fa += parseFloat(cell);                    
                }
                cell = row.get_element().cells[7].textContent;
                if (IsNumeric(cell)) {
                    total_fee_nefa += parseFloat(cell);
                }

            }
            $('tr.rgFooter').each(function () {
                $(this).find('td').eq(1).text(total_fa);
                $(this).find('td').eq(2).text(total_nefa);
                $(this).find('td').eq(3).text(total_total);
                $(this).find('td').eq(5).text(total_fee_fa);
                $(this).find('td').eq(7).text(total_fee_nefa);
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

                show_needsave_message();
            });
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
    <asp:Panel ID="panHeader" runat="server" Style="padding: 10px;">
        <asp:Label ID="lblP45" runat="server" CssClass="lbl" Text="Pracovat ve verzi rozpočtu:"></asp:Label>
        <asp:DropDownList ID="p45ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="VersionWithName"></asp:DropDownList>
        <asp:Button ID="cmdNewVersion" runat="server" Text="Založit novou verzi rozpočtu" CssClass="cmd" />
        <asp:Button ID="cmdDeleteVersion" runat="server" Text="Odstranit tuto verzi rozpočtu" CssClass="cmd" />

    </asp:Panel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


    <div style="clear: both;"></div>
    <telerik:RadTabStrip ID="tabs1" runat="server" MultiPageID="RadMultiPage1" ShowBaseLine="true" Skin="Default">
        <Tabs>
            <telerik:RadTab Text="Základní vlastnosti" Value="p45"></telerik:RadTab>
            <telerik:RadTab Text="Rozpočet hodin" Selected="true" Value="p46"></telerik:RadTab>
            <telerik:RadTab Text="Rozpočet peněžních výdajů a příjmů" Value="p49"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage ID="RadMultiPage1" runat="server">
        <telerik:RadPageView ID="p45" runat="server">
            <div class="content-box2">
                <div class="title">
                    Hlavička rozpočtu projektu
                </div>
                <div class="content">

                    <div style="padding: 10px;"">
                        <asp:Label ID="lblFrom" Text="Plánované zahájení:" runat="server" CssClass="lbl" Width="140px"></asp:Label>
                        <telerik:RadDatePicker ID="p45PlanFrom" runat="server" RenderMode="Lightweight" Width="120px" SharedCalendarID="SharedCalendar">
                            <DateInput ID="DateInput1" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                            <ClientEvents OnDateSelected="show_needsave_message" />
                        </telerik:RadDatePicker>
                    </div>
                    <div style="padding: 10px;">
                        <asp:Label ID="lblUntil" Text="Plánované dokončení:" runat="server" CssClass="lbl" Width="140px"></asp:Label>
                        <telerik:RadDatePicker ID="p45PlanUntil" runat="server" RenderMode="Lightweight" Width="120px" SharedCalendarID="SharedCalendar" MaxDate="1.1.3000">
                            <DateInput ID="DateInput2" DisplayDateFormat="d.M.yyyy ddd" runat="server"></DateInput>
                            <ClientEvents OnDateSelected="show_needsave_message" />
                        </telerik:RadDatePicker>
                    </div>
                    <div style="padding: 10px;">
                        <asp:Label ID="Label2" Text="Popis:" runat="server" CssClass="lbl" Width="140px"></asp:Label>
                        <asp:TextBox ID="p45Name" runat="server" Width="400px"></asp:TextBox>
                    </div>


                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="p46" runat="server" Selected="true">


            <div style="margin-top: 10px;">
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




            <telerik:RadGrid ID="grid1" AutoGenerateColumns="false" AutoGenerateHierarchy="false" AutoGenerateEditColumn="false" EnableViewState="true" RenderMode="Lightweight" runat="server" PageSize="500" ShowFooter="true" AllowPaging="false" AllowSorting="true" Skin="Metro">
                <MasterTableView EditMode="Batch" ClientDataKeyNames="PID" NoMasterRecordsText="Zatím žádné osoby v rozpočtu">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="Hodiny" HeaderText="Hodiny">
                            <HeaderStyle HorizontalAlign="Center" ForeColor="black" Font-Bold="true" BackColor="silver" />
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="Billing" HeaderText="Fakturační cena">
                            <HeaderStyle HorizontalAlign="Center" ForeColor="black" Font-Bold="true" BackColor="silver" />
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="Cost" HeaderText="Nákladová cena">
                            <HeaderStyle HorizontalAlign="Center" ForeColor="black" Font-Bold="true" BackColor="silver" />
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Osoba" DataField="p85FreeText01" ReadOnly="true" AllowSorting="true"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn HeaderText="Fa" DataField="p85FreeFloat01" ColumnEditorID="gridnumber1" ColumnGroupName="Hodiny">
                            <ItemStyle ForeColor="Green" />
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn HeaderText="NeFa" DataField="p85FreeFloat02" ColumnEditorID="gridnumber1" ColumnGroupName="Hodiny">
                            <ItemStyle ForeColor="Red" />
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn HeaderText="Fa+Nefa" DataField="p85FreeFloat03" ReadOnly="true" AllowSorting="true" ColumnGroupName="Hodiny">
                            <ItemStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Sazba" DataField="p85FreeNumber01" ReadOnly="true" AllowSorting="true" ColumnGroupName="Billing"> 
                            <ItemStyle ForeColor="green" />                          
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderText="Honorář" DataField="p85FreeNumber03" ReadOnly="true" AllowSorting="true" ColumnGroupName="Billing">                           
                            <ItemStyle ForeColor="green" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Sazba" DataField="p85FreeNumber02" ReadOnly="true" AllowSorting="true" ColumnGroupName="Cost">    
                            <ItemStyle ForeColor="red" />                       
                        </telerik:GridBoundColumn>                       
                         <telerik:GridBoundColumn HeaderText="Honorář" DataField="p85FreeNumber04" ReadOnly="true" AllowSorting="true" ColumnGroupName="Cost">                           
                             <ItemStyle ForeColor="red" />
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
        </telerik:RadPageView>
        <telerik:RadPageView ID="p49" runat="server">
            <div style="margin-top: 10px;">
                <asp:ImageButton ID="cmdNewP49" runat="server" ImageUrl="Images/new.png" ToolTip="Nová položka" OnClientClick="return p49_new()" CssClass="button-link" />
                <asp:ImageButton ID="cmdEditP49" runat="server" ImageUrl="Images/edit.png" ToolTip="Upravit" OnClientClick="return p49_record(false)" CssClass="button-link" />
                <asp:ImageButton ID="cmdCloneP49" runat="server" ImageUrl="Images/copy.png" ToolTip="Kopírovat položku" OnClientClick="return p49_clone()" CssClass="button-link" />
                <span>Výdaje celkem:</span>
                <asp:Label ID="total_expense" runat="server" CssClass="valboldred"></asp:Label>
                <span>Příjmy celkem:</span>
                <asp:Label ID="total_income" runat="server" CssClass="valboldblue"></asp:Label>
            </div>
            <uc:datagrid ID="gridP49" runat="server" ClientDataKeyNames="pid" OnRowDblClick="RowDoubleClick" OnRowSelected="RowSelected"></uc:datagrid>
            
            <asp:HiddenField ID="hidP49_p85ID" runat="server" />
        </telerik:RadPageView>
    </telerik:RadMultiPage>

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
                <asp:CheckBox ID="chkCloneP46" runat="server" Text="Zkopírovat rozpočet hodin" Checked="true" />
            </div>
            <div>
                <asp:CheckBox ID="chkCloneP47" runat="server" Text="Zkopírovat kapacitní plán" Checked="true" />
            </div>
            <div>
                <asp:CheckBox ID="chkCloneP49" runat="server" Text="Zkopírovat rozpočet výdajů a příjmů" Checked="true" />
            </div>
        </div>
    </asp:Panel>
    <asp:Button ID="cmdRefresh" runat="server" Style="display: none;" />

    <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">

        <SpecialDays>
            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
        </SpecialDays>
    </telerik:RadCalendar>

    <script type="text/javascript">
        function RowSelected(sender, args) {

            document.getElementById("<%=hidP49_p85ID.ClientID%>").value = args.getDataKeyValue("pid");

        }

        function RowDoubleClick(sender, args) {
            p49_record(true);
        }
        function p49_record(b) {
            var pid = document.getElementById("<%=hidP49_p85ID.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return (b);
            }
            dialog_master("p45_project_p49.aspx?p45id=<%=Me.CurrentP45ID%>&guid=<%=ViewState("guid")%>&pid=" + pid, false, "800", "600");
            return (b);
        }

        function p49_new() {
            dialog_master("p45_project_p49.aspx?pid=0&p45id=<%=Me.CurrentP45ID%>&guid=<%=ViewState("guid")%>", false, "800", "600");
            return (false);
        }
        function p49_clone() {
            var pid = document.getElementById("<%=hidP49_p85ID.ClientID%>").value;
            if (pid == "" || pid == null) {
                alert("Není vybrán záznam.");
                return (false);
            }
            dialog_master("p45_project_p49.aspx?p45id=<%=Me.CurrentP45ID%>&guid=<%=ViewState("guid")%>&clone=1&pid=" + pid, false, "800", "600");
            return (false);
        }
        function hardrefresh(pid, flag) {


            <%=Me.ClientScript.GetPostBackEventReference(Me.cmdRefresh, "", False)%>;

        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
