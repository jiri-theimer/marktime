<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p31_subgrid.ascx.vb" Inherits="UI.p31_subgrid" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Panel runat="server" ID="panCommand" CssClass="div6">
    <div style="float: left;">
        <img src="Images/worksheet.png" alt="Worksheet" />
        <asp:Label ID="lblHeaderP31" CssClass="framework_header_span" runat="server" Text=""></asp:Label>
    </div>

    <div class="commandcell">
        <uc:periodcombo ID="period1" runat="server" Width="150px"></uc:periodcombo>

        <asp:LinkButton ID="cmdExplicitPeriod" ToolTip="Zrušit filtr podle kalendáře" runat="server" Style="font-size: 120%; font-weight: bold; color: red; padding-left: 10px; padding-right: 10px;"></asp:LinkButton>

    </div>
    <div class="commandcell">
        <asp:TextBox ID="txtSearch" runat="server" Text="" Style="width: 80px;" ToolTip="Hledat podle názvu klienta, projektu, kódu projektu, příjmení osoby, názvu aktivity nebo podrobného popisu úkonu"></asp:TextBox>
        <asp:ImageButton ID="cmdSearch" runat="server" ImageUrl="Images/search.png" CssClass="button-link" ToolTip="Hledat" />
    </div>
    <div class="commandcell" style="padding-left: 10px;">
        <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
    </div>
    <div class="commandcell">
        <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 150px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>


        <asp:ImageButton ID="cmdQuery" runat="server" OnClientClick="return querybuilder()" ImageUrl="Images/query.png" ToolTip="Návrhář filtrů" CssClass="button-link" />

    </div>


    <div class="commandcell" style="padding-left: 20px;">
        <asp:ImageButton ID="cmdNew" runat="server" ImageUrl="Images/new.png" ToolTip="Nový úkon" OnClientClick="return p31_entry()" CssClass="button-link" />
        <asp:ImageButton ID="cmdCopy" runat="server" ImageUrl="Images/copy.png" ToolTip="Kopírovat do nového úkonu" OnClientClick="return p31_clone()" CssClass="button-link" />
        <asp:ImageButton ID="cmdSplit" runat="server" ImageUrl="Images/split.png" ToolTip="Rozdělit časový úkon na 2 kusy" OnClientClick="return p31_split()" CssClass="button-link" />

    </div>
    <div class="commandcell" style="padding-left: 10px;">
        <button type="button" id="cmdSetting" class="show_hide1xx" style="padding: 3px; border-radius: 4px; border-top: solid 1px silver; border-left: solid 1px silver; border-bottom: solid 1px gray; border-right: solid 1px gray; background: buttonface; height: 23px;" title="Více nastavení k přehledu">

            <img src="Images/arrow_down.gif" alt="Nastavení" />
        </button>
    </div>



</asp:Panel>
<div style="clear: both; width: 100%;"></div>
<div class="slidingDiv1xx" style="padding: 20px;">
    <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="width: 200px;" ToolTip="Pojmenované šablony sloupců"></asp:DropDownList>
    <asp:ImageButton ID="cmdGridDesigner" runat="server" OnClientClick="return p31_subgrid_columns()" ImageUrl="Images/griddesigner.png" ToolTip="Návrhář sloupců" CssClass="button-link" />



    <span style="padding-left: 40px;">Stránkování:</span>
    <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
        <asp:ListItem Text="10"></asp:ListItem>
        <asp:ListItem Text="20"></asp:ListItem>
        <asp:ListItem Text="50" Selected="True"></asp:ListItem>
        <asp:ListItem Text="100"></asp:ListItem>
        <asp:ListItem Text="200"></asp:ListItem>
        <asp:ListItem Text="500"></asp:ListItem>
    </asp:DropDownList>

    <asp:Panel ID="panGroupBy" runat="server" Style="margin-top: 20px;">
        <span>Datové souhrny:</span>
        <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Bez souhrnů" Value=""></asp:ListItem>
            <asp:ListItem Text="Sešit" Value="p34Name"></asp:ListItem>
            <asp:ListItem Text="Aktivita" Value="p32Name"></asp:ListItem>
            <asp:ListItem Text="Osoba" Value="Person"></asp:ListItem>
            <asp:ListItem Text="Klient" Value="p28Name"></asp:ListItem>
            <asp:ListItem Text="Projekt" Value="p41Name"></asp:ListItem>
            <asp:ListItem Text="Faktura" Value="p91Code"></asp:ListItem>
            <asp:ListItem Text="Úkol" Value="p56Name"></asp:ListItem>
            <asp:ListItem Text="Schvalování" Value="p71Name"></asp:ListItem>
            <asp:ListItem Text="Fakt.status" Value="p70Name"></asp:ListItem>
            <asp:ListItem Text="Billing dávka" Value="p31ApprovingSet"></asp:ListItem>
        </asp:DropDownList>
        <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="Auto-rozbalené souhrny" AutoPostBack="true" Checked="true" />
    </asp:Panel>
    <div style="margin-top:20px;">
        <img src="Images/export.png" />
        <asp:LinkButton ID="cmdExport" runat="server" Text="MS Excel" />

        <asp:Image ID="imgApprove" ImageUrl="Images/approve.png" runat="server" Style="margin-left: 20px;" />
        <asp:HyperLink ID="cmdApprove" runat="server" Text="Schvalovat/Pře-schvalovat označené úkony" NavigateUrl="javascript:approving();"></asp:HyperLink>

    </div>
</div>
<uc:datagrid ID="grid2" runat="server" ClientDataKeyNames="pid" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick_first" HeaderText="Projektový worksheet"></uc:datagrid>
<asp:HiddenField ID="hidMasterDataPID" runat="server" />
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidNeedRefreshP31_subgrid" runat="server" />
<asp:HiddenField ID="hidExplicitDateFrom" runat="server" Value="01.01.1900" />
<asp:HiddenField ID="hidExplicitDateUntil" runat="server" Value="01.01.3000" />
<asp:HiddenField ID="hidJ74RecordState" runat="server" />
<asp:HiddenField ID="hidDefaultSorting" runat="server" />
<asp:HiddenField ID="hidJ74ID" runat="server" />
<asp:HiddenField ID="hidDrillDownField" runat="server" />

<script type="text/javascript">
    $(document).ready(function () {

        $(".slidingDiv1xx").hide();
        $(".show_hide1xx").show();

        $('.show_hide1xx').click(function () {
            $(".slidingDiv1xx").slideToggle();
        });

        $('#<%=Me.txtSearch.ClientID%>').keydown(function (event) {
            var keypressed = event.keyCode || event.which;
            if (keypressed == 13) {
                search();
            }
        });

        if ($("#<%=Me.txtSearch.ClientID%>").val() != '') {
            $("#<%=Me.txtSearch.ClientID%>").focus();
            $("#<%=Me.txtSearch.ClientID%>").select();
        }

        $("#<%=Me.txtSearch.ClientID%>").focus(function () { $(this).select(); });


    });

    function search() {
        var s = document.getElementById("<%=Me.txtSearch.ClientID%>").value;

        $.post("Handler/handler_userparam.ashx", { x36value: s, x36key: "p31_subgrid-search", oper: "set" }, function (data) {
            if (data == ' ') {
                return;
            }

            var clickButton = document.getElementById("<%=cmdSearch.ClientID %>");
            clickButton.click();
        });
    }

    function periodcombo_setting() {
        p31_subgrid_periodcombo();
        
    }

    function querybuilder() {
        var j70id = "<%=Me.CurrentJ70ID%>";
        p31_subgrid_querybuilder(j70id);
        return (false);
    }

    function approving() {
        var pids = GetAllSelectedPIDs();
        if (pids == "") {
            alert("Není vybrán ani jeden záznam.");
            return;

        }
        p31_subgrid_approving(pids);
        

    }

    function GetAllSelectedPIDs() {

        var masterTable = $find("<%=grid2.radGridOrig.ClientID%>").get_masterTableView();
        var sel = masterTable.get_selectedItems();
        var pids = "";

        for (i = 0; i < sel.length; i++) {
            if (pids == "")
                pids = sel[i].getDataKeyValue("pid");
            else
                pids = pids + "," + sel[i].getDataKeyValue("pid");
        }

        return (pids);
    }

    function p31_subgrid_columns() {
        p31_subgrid_setting(document.getElementById("<%=Me.j74id.ClientID%>").value);
        return (false);
    }

    function p31_RowDoubleClick_first(sender, args) {        
        if (args.get_tableView().get_name() == "grid") {
            p31_RowDoubleClick();
        }
        if (args.get_tableView().get_name() == "drilldown") {
            var item = sender.get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()];

            var rowid = item.get_id();
            var firstInput = $('#' + rowid).find('input[type=submit]').filter(':visible:first');
            if (firstInput != null) {
                firstInput.click();
            }

        }

    }
</script>
