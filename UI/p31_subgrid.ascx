﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="p31_subgrid.ascx.vb" Inherits="UI.p31_subgrid" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="datagrid" Src="~/datagrid.ascx" %>
<%@ Register TagPrefix="uc" TagName="periodcombo" Src="~/periodcombo.ascx" %>

<asp:Panel runat="server" ID="panCommand" CssClass="div6">
    <div class="commandcell">
        <img src="Images/worksheet.png" alt="Worksheet" />
        <asp:Label ID="lblHeaderP31" CssClass="framework_header_span" runat="server" Text=""></asp:Label>

    </div>

    <div class="commandcell" style="margin-left: 10px; margin-right: 10px;">
        <uc:periodcombo ID="period1" runat="server" Width="150px"></uc:periodcombo>
        <asp:Label ID="ExplicitPeriod" runat="server" CssClass="valboldblue"></asp:Label>

        <asp:ImageButton ID="cmdClearExplicitPeriod" runat="server" ImageUrl="Images/close.png" ToolTip="Zrušit filtr podle kalendáře" CssClass="button-link" />
    </div>
    <div class="commandcell" id="divQueryContainer">
        <asp:HyperLink ID="clue_query" runat="server" CssClass="reczoom" ToolTip="Detail filtru" Text="i"></asp:HyperLink>
        <asp:DropDownList ID="j70ID" runat="server" AutoPostBack="true" DataTextField="NameWithMark" DataValueField="pid" Style="width: 180px;" ToolTip="Pojmenovaný filtr"></asp:DropDownList>
    </div>


    <div class="commandcell">
        <telerik:RadMenu ID="recmenu1" Skin="Metro" runat="server" ClickToOpen="true" Style="z-index: 2000;" RenderMode="Auto" ExpandDelay="0" ExpandAnimation-Type="None">
            <Items>
                <telerik:RadMenuItem Text="Záznam" ImageUrl="Images/menuarrow.png">
                    <Items>
                        <telerik:RadMenuItem Text="Nový" Value="new" NavigateUrl="javascript:p31_entry()"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Kopírovat" Value="clone" NavigateUrl="javascript:p31_clone()"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Rozdělit časový úkon na 2 kusy" Value="split" NavigateUrl="javascript:p31_split()"></telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Akce pro vybrané záznamy" Value="akce" ImageUrl="Images/menuarrow.png">
                    <Items>
                        <telerik:RadMenuItem Text="Kopírovat" Value="clone" NavigateUrl="javascript:p31_clone()"></telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="Schvalovat/pře-schvalovat označené" Value="cmdApprove" NavigateUrl="javascript:approving()"></telerik:RadMenuItem>


                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Další akce" ImageUrl="Images/menuarrow.png">
                    <ContentTemplate>

                        <div class="content-box3">
                            <div class="title">
                                <img src="Images/query.png" />
                                <span>Filtrování záznamů</span>
                            </div>
                            <div class="content">
                                <div class="div6">
                                    <button type="button" onclick="p31_subgrid_x18query()">
                                        <img src="Images/label.png" />Štítky</button>
                                    <asp:ImageButton ID="cmdClearX18" runat="server" ToolTip="Vyčistit štítkovací filtr" ImageUrl="Images/delete.png" Visible="false" CssClass="button-link" />
                                    <asp:Label ID="x18_querybuilder_info" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                <div id="divJ70" class="div6">
                                    
                                    <button type="button" id="cmdQuery" runat="server" onclick="querybuilder()">
                                        <img src="Images/query.png" />Návrhář filtrů</button>

                                </div>
                                <asp:CheckBox ID="chkQueryOnTop" runat="server" Text="Nabídku filtrů zobrazovat nad přehledem" AutoPostBack="true" CssClass="chk" />
                            </div>
                        </div>
                        <asp:Panel ID="panExport" runat="server" CssClass="content-box3">
                            <div class="title">
                                <img src="Images/export.png" />
                                <span>Export záznamů aktuálního přehledu</span>
                            </div>
                            <div class="content">
                                <img src="Images/export.png" alt="export" />
                                <asp:LinkButton ID="cmdExport" runat="server" Text="Export" ToolTip="Export do MS EXCEL tabulky, plný počet záznamů" />

                                <img src="Images/xls.png" alt="xls" />
                                <asp:LinkButton ID="cmdXLS" runat="server" Text="XLS" ToolTip="Export do XLS vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                <img src="Images/pdf.png" alt="pdf" />
                                <asp:LinkButton ID="cmdPDF" runat="server" Text="PDF" ToolTip="Export do PDF vč. souhrnů s omezovačem na maximálně 2000 záznamů" />

                                <img src="Images/doc.png" alt="doc" />
                                <asp:LinkButton ID="cmdDOC" runat="server" Text="DOC" ToolTip="Export do DOC vč. souhrnů s omezovačem na maximálně 2000 záznamů" />
                            </div>



                        </asp:Panel>

                        <div class="content-box3">
                            <div class="title">
                                <img src="Images/griddesigner.png" />Sloupce v přehledu
                            </div>
                            <div class="content">
                                <asp:DropDownList ID="j74id" runat="server" AutoPostBack="true" DataTextField="j74Name" DataValueField="pid" Style="min-width: 200px;" ToolTip="Pojmenované šablony sloupců"></asp:DropDownList>
                                <button type="button" id="linkGridDesigner" runat="server" onclick="p31_subgrid_columns()">Sloupce</button>

                                <asp:Panel ID="panGroupBy" runat="server" CssClass="div6">
                                    <span><%=Resources.common.DatoveSouhrny%>:</span>
                                    <asp:DropDownList ID="cbxGroupBy" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:common,BezSouhrnu%>" Value=""></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Sesit%>" Value="p34Name"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Aktivita%>" Value="p32Name"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Osoba%>" Value="Person"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Klient%>" Value="ClientName"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Projekt %>" Value="p41Name"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Faktura%>" Value="p91Code"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Ukol%>" Value="p56Name"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,Schvalovani%>" Value="p71Name"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:common,FaktStatus%>" Value="p70Name"></asp:ListItem>
                                        <asp:ListItem Text="Billing dávka" Value="p31ApprovingSet"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chkGroupsAutoExpanded" runat="server" Text="Auto-rozbalené souhrny" AutoPostBack="true" Checked="true" />

                                </asp:Panel>

                                <div class="div6">
                                    <span><%=Resources.common.Strankovani%>:</span>
                                    <asp:DropDownList ID="cbxPaging" runat="server" AutoPostBack="true" ToolTip="Stránkování">
                                        <asp:ListItem Text="10"></asp:ListItem>
                                        <asp:ListItem Text="20"></asp:ListItem>
                                        <asp:ListItem Text="50" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="100"></asp:ListItem>
                                        <asp:ListItem Text="200"></asp:ListItem>
                                        <asp:ListItem Text="500"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="content-box3" style="margin-top: 20px;">
                            <div class="title"></div>
                            <div class="content">

                                <div class="div6">

                                    <asp:HyperLink ID="cmdSummary" runat="server" NavigateUrl="javascript:drilldown()" Text="<img src='Images/pivot.png' /> WORKSHEET statistika aktuálního přehledu"></asp:HyperLink>
                                </div>

                                <div class="div6">

                                    <asp:HyperLink ID="cmdFullScreen" runat="server" Text="<img src='Images/fullscreen.png' /> Zobrazit přehled na celou stránku" NavigateUrl="javascript:p31_fullscreen()"></asp:HyperLink>

                                </div>
                                <div class="div6">
                                    <asp:CheckBox ID="chkIncludeChilds" runat="server" AutoPostBack="true" Text="Zahrnout i pod-projekty" CssClass="chk" Visible="false" />
                                </div>
                            </div>


                        </div>
                    </ContentTemplate>
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadMenu>

    </div>



</asp:Panel>
<div style="clear: both; width: 100%;"></div>
<div id="divCurrentQuery">
    
    
    <asp:Label ID="CurrentQuery" runat="server" ForeColor="Red"></asp:Label>
</div>

<uc:datagrid ID="grid2" runat="server" ClientDataKeyNames="pid" OnRowSelected="p31_RowSelected" OnRowDblClick="p31_RowDoubleClick_first"></uc:datagrid>

<asp:HiddenField ID="hidMasterDataPID" runat="server" />
<asp:HiddenField ID="hidX29ID" runat="server" />
<asp:HiddenField ID="hidNeedRefreshP31_subgrid" runat="server" />
<asp:HiddenField ID="hidExplicitDateFrom" runat="server" Value="01.01.1900" />
<asp:HiddenField ID="hidExplicitDateUntil" runat="server" Value="01.01.3000" />
<asp:HiddenField ID="hidJ74RecordState" runat="server" />
<asp:HiddenField ID="hidDefaultSorting" runat="server" />
<asp:HiddenField ID="hidJ74ID" runat="server" />
<asp:HiddenField ID="hidDrillDownField" runat="server" />
<asp:HiddenField ID="hidCols" runat="server" />
<asp:HiddenField ID="hidSumCols" runat="server" />
<asp:HiddenField ID="hidFooterString" runat="server" />
<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidAllowFullScreen" runat="server" Value="1" />
<asp:HiddenField ID="hidMasterTabAutoQueryFlag" runat="server" />
<asp:HiddenField ID="hidX18_value" runat="server" />

<script type="text/javascript">

    $(document).ready(function () {
        <%If Not Me.chkQueryOnTop.Checked Then%>
        $('#<%=Me.j70ID.ClientID%>').prependTo('#divJ70');
        <%If Me.clue_query.Visible Then%>
        $('#<%=Me.clue_query.ClientID%>').prependTo('#divCurrentQuery');
        <%End If%>
        <%End If%>



    });



    function periodcombo_setting() {
        p31_subgrid_periodcombo();

    }

    function querybuilder() {
        var j70id = "<%=Me.CurrentJ70ID%>";
        p31_subgrid_querybuilder(j70id);

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
        //return (false);
    }
    function p31_subgrid_query() {
        p31_subgrid_setting(document.getElementById("<%=Me.j70ID.ClientID%>").value);
        //return (false);
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

    function p31_fullscreen() {
        window.open("p31_grid.aspx?masterpid=" + document.getElementById("<%=me.hidMasterDataPID.ClientID%>").value + "&masterprefix=<%=BO.BAS.GetDataPrefix(Me.EntityX29ID)%>&p31tabautoquery=<%=me.MasterTabAutoQueryFlag%>", "_top");
        return (false);
    }

    function drilldown() {
        var j70id = "<%=Me.CurrentJ70ID%>";

        var w = screen.availWidth - 100;
        var masterprefix = "<%=BO.BAS.GetDataPrefix(Me.EntityX29ID)%>";
        var masterpid = document.getElementById("<%=me.hidMasterDataPID.ClientID%>").value;
        var queryflag = document.getElementById("<%=hidMasterTabAutoQueryFlag.ClientID%>").value;

        window.open("p31_sumgrid.aspx?j70id=" + j70id + "&masterprefix=" + masterprefix + "&masterpid=" + masterpid + "&p31tabautoquery=" + queryflag, "_top");
        //sw_local("p31_drilldown.aspx?j70id=" + j70id + "&j74id=" + j74id + "&masterprefix=" + masterprefix + "&masterpid=" + masterpid + "&tabqueryflag=" + queryflag, "Images/pivot.png", true);
        //return (false);
    }


</script>
