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


        function BatchEditCellValueChanged(sender, args) {
            var row = args.get_row();
            var cell = args.get_cell();
            var tableView = args.get_tableView();
            var column = args.get_column();
            var columnUniqueName = args.get_columnUniqueName();
            var editorValue = args.get_editorValue();
            var cellValue = args.get_cellValue();

            //save_cellvalue(row.attributes["j02id"].value, columnUniqueName, editorValue)

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="OverMainContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 10px;">
        <asp:Label ID="lblP45" runat="server" CssClass="lbl" Text="Pracovat ve verzi rozpočtu:"></asp:Label>
        <asp:DropDownList ID="p45ID" runat="server" AutoPostBack="true" DataValueField="pid" DataTextField="VersionWithName"></asp:DropDownList>
        <asp:Button ID="cmdNewVersion" runat="server" Text="Založit novou verzi rozpočtu" CssClass="cmd" />
        <asp:Button ID="cmdDeleteVersion" runat="server" Text="Odstranit tuto verzi rozpočtu" CssClass="cmd" />
       <asp:Button ID="cmdSaveFirstVersion" runat="server" CssClass="cmd" Font-Bold="true" Text="Založit v projektu rozpočet" />
    </div>
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
            <button type="button" class="show_hide1" id="cmdAddPerson" runat="server">Přidat osoby do rozpočtu<img src="Images/arrow_down.gif" /></button>
        </div>

        <div class="slidingDiv1">
            <div class="content-box2">
                <div class="title">
                    Osoby s rolí v projektu
                    <asp:LinkButton ID="cmdInsertPersons" runat="server" CommandName="add" Text="Přidat zaškrtlé do rozpočtu"></asp:LinkButton>
                </div>
                <div class="content">
                    <asp:Repeater ID="rpJ02" runat="server">
                        <ItemTemplate>
                            <div style="padding: 10px;">
                                <asp:HyperLink ID="clue_person" runat="server" CssClass="reczoom" Text="i"></asp:HyperLink>

                                <asp:checkbox ID="Person" runat="server"></asp:checkbox>
                                
                                <asp:HiddenField ID="hidJ02ID" runat="server" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>
    </asp:Panel>

    <telerik:RadGrid ID="grid1" AutoGenerateColumns="false" AutoGenerateHierarchy="false" AutoGenerateEditColumn="false" EnableViewState="true" RenderMode="Lightweight" runat="server" PageSize="500" ShowFooter="true" AllowPaging="false" AllowSorting="true" Skin="Metro">
        <MasterTableView EditMode="Batch" ClientDataKeyNames="PID" NoMasterRecordsText="Žádné záznamy">
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


    <telerik:RadCalendar ID="SharedCalendar" runat="server" EnableMultiSelect="False" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">

        <SpecialDays>
            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="SkyBlue"></telerik:RadCalendarDay>
        </SpecialDays>
    </telerik:RadCalendar>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FootContent" runat="server">
</asp:Content>
